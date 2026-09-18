Imports System
Imports System.IO
Imports System.Collections.Generic
Imports NAudio.Wave

Public Class AudioEngine
    Private capture As WasapiLoopbackCapture = Nothing
    Private isCapturing As Boolean = False

    ' Buffer circular de amostras PCM mono (guarda últimos 5 segundos a 44100Hz = 220500 amostras)
    Private Const RingBufferSize As Integer = 220500
    Private ringBuffer(RingBufferSize - 1) As Single
    Private ringWriteIndex As Integer = 0
    Private totalSamplesReceived As Long = 0

    ' Bandas de frequência para matching
    Private Const BandCount As Integer = 16
    Private Const FrameIntervalMs As Integer = 25
    Private analysisTimer As System.Timers.Timer = Nothing

    ' Histórico deslizante de vetores espectrais ao vivo (últimos 4 segundos = 160 frames)
    Private Const MaxHistoryFrames As Integer = 160
    Private liveHistory As New List(Of Single())

    ' Sons registrados
    Private registeredTriggers As New Dictionary(Of String, TriggerSignature)

    ' Gravador de amostras
    Public IsRecordingSample As Boolean = False
    Private sampleWriter As WaveFileWriter = Nothing
    Private sampleRecordPath As String = ""

    ' Eventos para a UI
    Public Event LevelChanged(level As Single)
    Public Event MatchProgress(triggerId As String, percent As Integer)
    Public Event SoundDetected(triggerId As String, confidence As Integer)
    Public Event StatusChanged(isConnected As Boolean, message As String)

    Public Sub New()
        analysisTimer = New System.Timers.Timer(FrameIntervalMs)
        AddHandler analysisTimer.Elapsed, AddressOf OnAnalysisTick
        analysisTimer.AutoReset = True
    End Sub

    Public Sub StartCapture()
        If isCapturing Then Return
        Try
            capture = New WasapiLoopbackCapture()
            AddHandler capture.DataAvailable, AddressOf OnDataAvailable
            AddHandler capture.RecordingStopped, AddressOf OnRecordingStopped

            capture.StartRecording()
            isCapturing = True
            analysisTimer.Start()

            RaiseEvent StatusChanged(True, "Ouvindo áudio do sistema (WASAPI Loopback)...")
        Catch ex As Exception
            RaiseEvent StatusChanged(False, "Erro ao iniciar captura: " & ex.Message)
        End Try
    End Sub

    Public Sub StopCapture()
        If Not isCapturing Then Return
        Try
            analysisTimer.Stop()
            If capture IsNot Nothing Then
                capture.StopRecording()
                capture.Dispose()
                capture = Nothing
            End If
            isCapturing = False
            RaiseEvent StatusChanged(False, "Captura de áudio parada.")
        Catch ex As Exception
            Console.WriteLine("Erro ao parar: " & ex.Message)
        End Try
    End Sub

    Private Sub OnDataAvailable(sender As Object, e As WaveInEventArgs)
        If e.BytesRecorded <= 0 Then Return

        Dim waveFormat = capture.WaveFormat
        Dim bytesPerSample = waveFormat.BitsPerSample / 8
        Dim channels = waveFormat.Channels
        Dim sampleCount = e.BytesRecorded / (bytesPerSample * channels)

        Dim sumSq As Double = 0

        ' Gravação em arquivo temporário se estiver ativo
        If IsRecordingSample AndAlso sampleWriter IsNot Nothing Then
            sampleWriter.Write(e.Buffer, 0, e.BytesRecorded)
        End If

        For i As Integer = 0 To sampleCount - 1
            Dim sampleVal As Single = 0

            If waveFormat.Encoding = WaveFormatEncoding.IeeeFloat Then
                ' Amostras em Float 32-bit (padrão do WASAPI Loopback no Windows 10/11)
                For c As Integer = 0 To channels - 1
                    Dim offset = (i * channels + c) * 4
                    If offset + 4 <= e.BytesRecorded Then
                        sampleVal += BitConverter.ToSingle(e.Buffer, offset)
                    End If
                Next
                sampleVal /= channels
            ElseIf waveFormat.Encoding = WaveFormatEncoding.Pcm AndAlso bytesPerSample = 2 Then
                ' Amostras em PCM 16-bit
                For c As Integer = 0 To channels - 1
                    Dim offset = (i * channels + c) * 2
                    If offset + 2 <= e.BytesRecorded Then
                        sampleVal += BitConverter.ToInt16(e.Buffer, offset) / 32768.0F
                    End If
                Next
                sampleVal /= channels
            End If

            ' Salva no ring buffer circular
            ringBuffer(ringWriteIndex) = sampleVal
            ringWriteIndex = (ringWriteIndex + 1) Mod RingBufferSize
            totalSamplesReceived += 1

            sumSq += sampleVal * sampleVal
        Next

        Dim rms = CSng(Math.Sqrt(sumSq / sampleCount))
        RaiseEvent LevelChanged(Math.Min(1.0F, rms * 3.5F))
    End Sub

    Private Sub OnRecordingStopped(sender As Object, e As StoppedEventArgs)
        isCapturing = False
    End Sub

    Private Sub OnAnalysisTick(sender As Object, e As System.Timers.ElapsedEventArgs)
        If Not isCapturing OrElse totalSamplesReceived < 2048 Then Return

        ' Extrair vetor espectral dos últimos 1024 samples
        Dim frameSamples(1023) As Single
        Dim readIdx = (ringWriteIndex - 1024 + RingBufferSize) Mod RingBufferSize
        For i As Integer = 0 To 1023
            frameSamples(i) = ringBuffer((readIdx + i) Mod RingBufferSize)
        Next

        Dim bands = ExtractBands(frameSamples, 44100)

        SyncLock liveHistory
            liveHistory.Add(bands)
            If liveHistory.Count > MaxHistoryFrames Then
                liveHistory.RemoveAt(0)
            End If
        End SyncLock

        ' Comparar com os gatilhos registrados
        MatchTriggers()
    End Sub

    Private Sub MatchTriggers()
        Dim now = DateTime.Now

        SyncLock liveHistory
            For Each kvp As KeyValuePair(Of String, TriggerSignature) In registeredTriggers
                Dim trigger = kvp.Value
                If Not trigger.Enabled OrElse trigger.Signature Is Nothing OrElse trigger.Signature.Count = 0 Then
                    RaiseEvent MatchProgress(trigger.Id, 0)
                    Continue For
                End If

                Dim sigLen = trigger.Signature.Count
                If liveHistory.Count < sigLen Then Continue For

                Dim bestScore As Single = 0
                ' Busca em janela deslizante de 4 frames
                For offset As Integer = 0 To Math.Min(3, liveHistory.Count - sigLen)
                    Dim endIdx = liveHistory.Count - offset
                    Dim startIdx = endIdx - sigLen

                    Dim score = ComputeCosineSimilarity(liveHistory.GetRange(startIdx, sigLen), trigger.Signature)
                    If score > bestScore Then bestScore = score
                Next

                Dim percent = CInt(Math.Min(100, Math.Round(bestScore * 100)))
                RaiseEvent MatchProgress(trigger.Id, percent)

                If percent >= trigger.Threshold AndAlso (now - trigger.LastFired).TotalSeconds >= trigger.Cooldown Then
                    trigger.LastFired = now
                    RaiseEvent SoundDetected(trigger.Id, percent)
                End If
            Next
        End SyncLock
    End Sub

    Private Function ComputeCosineSimilarity(liveFrames As List(Of Single()), targetFrames As List(Of Single())) As Single
        Dim dotProduct As Double = 0
        Dim normA As Double = 0
        Dim normB As Double = 0

        For t As Integer = 0 To targetFrames.Count - 1
            Dim liveVec = liveFrames(t)
            Dim targetVec = targetFrames(t)

            For b As Integer = 0 To BandCount - 1
                dotProduct += liveVec(b) * targetVec(b)
                normA += liveVec(b) * liveVec(b)
                normB += targetVec(b) * targetVec(b)
            Next
        Next

        Dim denom = Math.Sqrt(normA) * Math.Sqrt(normB)
        If denom <= 0.0001 Then Return 0
        Return CSng(Math.Max(0, dotProduct / denom))
    End Function

    Private Function ExtractBands(samples() As Single, sampleRate As Integer) As Single()
        Dim bands(BandCount - 1) As Single
        Dim N = samples.Length
        Dim totalEnergy As Double = 0

        Dim minFreq = 150.0
        Dim maxFreq = 6000.0

        For b As Integer = 0 To BandCount - 1
            Dim centerFreq = minFreq * Math.Pow(maxFreq / minFreq, b / (BandCount - 1.0))
            Dim omega = (2 * Math.PI * centerFreq) / sampleRate
            Dim coeff = 2 * Math.Cos(omega)

            Dim q1 As Double = 0
            Dim q2 As Double = 0
            For i As Integer = 0 To N - 1
                Dim hann = 0.5 * (1 - Math.Cos((2 * Math.PI * i) / N))
                Dim s = samples(i) * hann
                Dim q0 = coeff * q1 - q2 + s
                q2 = q1
                q1 = q0
            Next

            Dim power = Math.Max(0.0, q1 * q1 + q2 * q2 - q1 * q2 * coeff)
            Dim amp = CSng(Math.Sqrt(power) / N)
            bands(b) = amp
            totalEnergy += amp * amp
        Next

        Dim norm = Math.Sqrt(totalEnergy)
        If norm > 0.0001 Then
            For b As Integer = 0 To BandCount - 1
                bands(b) /= CSng(norm)
            Next
        End If

        Return bands
    End Function

    Public Sub RegisterTrigger(id As String, name As String, wavPath As String, threshold As Integer, cooldown As Integer)
        Dim sig = LoadSignatureFromWav(wavPath)
        Dim trig As New TriggerSignature With {
            .Id = id,
            .Name = name,
            .WavPath = wavPath,
            .Threshold = threshold,
            .Cooldown = cooldown,
            .Enabled = True,
            .Signature = sig,
            .LastFired = DateTime.MinValue
        }

        SyncLock registeredTriggers
            registeredTriggers(id) = trig
        End SyncLock
    End Sub

    Public Sub UpdateTriggerSettings(id As String, threshold As Integer, cooldown As Integer, enabled As Boolean)
        SyncLock registeredTriggers
            If registeredTriggers.ContainsKey(id) Then
                registeredTriggers(id).Threshold = threshold
                registeredTriggers(id).Cooldown = cooldown
                registeredTriggers(id).Enabled = enabled
            End If
        End SyncLock
    End Sub

    Public Function LoadSignatureFromWav(filePath As String) As List(Of Single())
        Dim signature As New List(Of Single())
        If Not File.Exists(filePath) Then Return signature

        Try
            Using reader As New AudioFileReader(filePath)
                Dim sampleRate = reader.WaveFormat.SampleRate
                Dim channels = reader.WaveFormat.Channels
                Dim frameSize = CInt(sampleRate * (FrameIntervalMs / 1000.0))

                Dim buffer(frameSize * channels - 1) As Single
                Dim monoBuffer(frameSize - 1) As Single
                Dim readCount As Integer

                Do
                    readCount = reader.Read(buffer, 0, buffer.Length)
                    If readCount <= 0 Then Exit Do

                    Dim framesRead = readCount / channels
                    For i As Integer = 0 To framesRead - 1
                        Dim sum As Single = 0
                        For c As Integer = 0 To channels - 1
                            sum += buffer(i * channels + c)
                        Next
                        monoBuffer(i) = sum / channels
                    Next

                    Dim bands = ExtractBands(monoBuffer, sampleRate)
                    signature.Add(bands)
                Loop While signature.Count < 160
            End Using
        Catch ex As Exception
            Console.WriteLine("Erro ao carregar assinatura de " & filePath & ": " & ex.Message)
        End Try

        Return signature
    End Function

    Public Sub StartRecordingSample(targetWavPath As String)
        If capture Is Nothing Then Return
        Try
            sampleRecordPath = targetWavPath
            sampleWriter = New WaveFileWriter(targetWavPath, capture.WaveFormat)
            IsRecordingSample = True
        Catch ex As Exception
            Console.WriteLine("Erro ao gravar amostra: " & ex.Message)
        End Try
    End Sub

    Public Sub StopRecordingSample()
        If Not IsRecordingSample Then Return
        Try
            IsRecordingSample = False
            If sampleWriter IsNot Nothing Then
                sampleWriter.Dispose()
                sampleWriter = Nothing
            End If
        Catch ex As Exception
            Console.WriteLine("Erro ao finalizar gravação: " & ex.Message)
        End Try
    End Sub

    Public Shared Sub PlayWav(filePath As String)
        If Not File.Exists(filePath) Then Return
        Try
            Dim player As New System.Media.SoundPlayer(filePath)
            player.Play()
        Catch ex As Exception
            Console.WriteLine("Erro ao tocar áudio: " & ex.Message)
        End Try
    End Sub

    Public Shared Sub GenerateDefaultTones(soundsDir As String)
        If Not Directory.Exists(soundsDir) Then Directory.CreateDirectory(soundsDir)

        CreateToneWav(Path.Combine(soundsDir, "gm.wav"), "gm")
        CreateToneWav(Path.Combine(soundsDir, "msg_player.wav"), "msg_player")
        CreateToneWav(Path.Combine(soundsDir, "teleport.wav"), "teleport")
        CreateToneWav(Path.Combine(soundsDir, "pokemon.wav"), "pokemon")
        CreateToneWav(Path.Combine(soundsDir, "seta.wav"), "seta")
    End Sub

    Private Shared Sub CreateToneWav(pathFile As String, toneType As String)
        If File.Exists(pathFile) Then Return
        Try
            Dim sampleRate = 44100
            Dim duration = 0.6
            Dim numSamples = CInt(sampleRate * duration)
            Dim samples(numSamples - 1) As Short

            For i As Integer = 0 To numSamples - 1
                Dim t = CDbl(i) / sampleRate
                Dim env = Math.Exp(-t * 4.0)
                Dim freq As Double = 880

                Select Case toneType
                    Case "gm"
                        freq = If(t < 0.28, 1046, 1318)
                    Case "msg_player"
                        freq = If(t < 0.25, 587, 880)
                    Case "teleport"
                        freq = 300 + (t * 2200)
                    Case "pokemon"
                        freq = 880 + Math.Sin(t * 30) * 200 + (t * 400)
                    Case "seta"
                        freq = Math.Max(200, 1600 - (t * 1400))
                End Select

                Dim val = Math.Sin(2 * Math.PI * freq * t) * env * 0.8
                samples(i) = CShort(val * 32767)
            Next

            Using writer As New WaveFileWriter(pathFile, New WaveFormat(sampleRate, 16, 1))
                For Each s As Short In samples
                    writer.WriteSample(s / 32768.0F)
                Next
            End Using
        Catch ex As Exception
            Console.WriteLine("Erro ao criar tom padrão: " & ex.Message)
        End Try
    End Sub
End Class

Public Class TriggerSignature
    Public Property Id As String
    Public Property Name As String
    Public Property WavPath As String
    Public Property Threshold As Integer
    Public Property Cooldown As Integer
    Public Property Enabled As Boolean
    Public Property Signature As List(Of Single())
    Public Property LastFired As DateTime
End Class
