Imports System
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Collections.Generic

Public Class UserProfile
    Public Property Id As String = Guid.NewGuid().ToString()
    Public Property ProfileName As String = "Principal"
    Public Property CharacterName As String = "MeuPersonagem"
    Public Property TargetWindowTitle As String = ""
    Public Property TargetProcessName As String = ""
    Public Property TargetPid As Integer = 0
    Public Property FilterByProcessAudio As Boolean = False
    Public Property WebhookUrl As String = ""
    Public Property MentionType As String = "none"
    Public Property MentionId As String = ""
    Public Property PlayLocalSound As Boolean = True
    Public Property Triggers As New List(Of TriggerConfig)

    Public Overrides Function ToString() As String
        Return ProfileName & " (" & CharacterName & ")"
    End Function
End Class

Public Class TriggerConfig
    Public Property Id As String
    Public Property Name As String
    Public Property SoundFile As String
    Public Property Threshold As Integer = 80
    Public Property Cooldown As Integer = 6
    Public Property Enabled As Boolean = True
End Class

Public Class AppConfig
    Public Property ActiveProfileId As String = "default"
    Public Property Profiles As New List(Of UserProfile)
End Class

Public Class ConfigManager
    Private Shared ConfigPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json")

    Public Shared Function Load() As AppConfig
        Try
            If File.Exists(ConfigPath) Then
                Dim json = File.ReadAllText(ConfigPath)
                Dim serializer As New JavaScriptSerializer()
                Dim cfg = serializer.Deserialize(Of AppConfig)(json)
                If cfg IsNot Nothing AndAlso cfg.Profiles IsNot Nothing AndAlso cfg.Profiles.Count > 0 Then
                    Return cfg
                End If
            End If
        Catch ex As Exception
            Console.WriteLine("Erro ao carregar config: " & ex.Message)
        End Try

        ' Perfil Padrão Inicial
        Dim defaultCfg As New AppConfig()
        Dim defaultProfile As New UserProfile With {
            .Id = "default",
            .ProfileName = "Personagem 1",
            .CharacterName = "Lushen",
            .WebhookUrl = "",
            .MentionType = "none"
        }

        defaultProfile.Triggers.Add(New TriggerConfig With {.Id = "gm", .Name = "MENSAGEM GM", .SoundFile = "gm.wav", .Threshold = 82, .Cooldown = 6})
        defaultProfile.Triggers.Add(New TriggerConfig With {.Id = "msg_player", .Name = "MENSAGEM DE JOGADOR", .SoundFile = "msg_player.wav", .Threshold = 78, .Cooldown = 6})
        defaultProfile.Triggers.Add(New TriggerConfig With {.Id = "teleport", .Name = "TELEPORT", .SoundFile = "teleport.wav", .Threshold = 80, .Cooldown = 5})
        defaultProfile.Triggers.Add(New TriggerConfig With {.Id = "pokemon", .Name = "POKEMON FORA DA HUNT", .SoundFile = "pokemon.wav", .Threshold = 80, .Cooldown = 10})
        defaultProfile.Triggers.Add(New TriggerConfig With {.Id = "seta", .Name = "SOM DE SETA", .SoundFile = "seta.wav", .Threshold = 80, .Cooldown = 5})

        defaultCfg.Profiles.Add(defaultProfile)
        defaultCfg.ActiveProfileId = defaultProfile.Id
        Return defaultCfg
    End Function

    Public Shared Sub Save(cfg As AppConfig)
        Try
            Dim serializer As New JavaScriptSerializer()
            Dim json = serializer.Serialize(cfg)
            File.WriteAllText(ConfigPath, json)
        Catch ex As Exception
            Console.WriteLine("Erro ao salvar config: " & ex.Message)
        End Try
    End Sub
End Class
