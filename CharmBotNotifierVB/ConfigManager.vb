Imports System
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Collections.Generic

Public Class AppConfig
    Public Property WebhookUrl As String = ""
    Public Property MentionType As String = "none"
    Public Property MentionId As String = ""
    Public Property PlayLocalSound As Boolean = True
    Public Property Triggers As New List(Of TriggerConfig)
End Class

Public Class TriggerConfig
    Public Property Id As String
    Public Property Name As String
    Public Property SoundFile As String
    Public Property Threshold As Integer = 80
    Public Property Cooldown As Integer = 6
    Public Property Enabled As Boolean = True
End Class

Public Class ConfigManager
    Private Shared ConfigPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json")

    Public Shared Function Load() As AppConfig
        Try
            If File.Exists(ConfigPath) Then
                Dim json = File.ReadAllText(ConfigPath)
                Dim serializer As New JavaScriptSerializer()
                Dim cfg = serializer.Deserialize(Of AppConfig)(json)
                If cfg IsNot Nothing Then Return cfg
            End If
        Catch ex As Exception
            Console.WriteLine("Erro ao carregar config: " & ex.Message)
        End Try

        ' Retorna configuração padrão se não existir
        Dim defaultCfg As New AppConfig()
        defaultCfg.Triggers.Add(New TriggerConfig With {.Id = "gm", .Name = "MENSAGEM GM", .SoundFile = "gm.wav", .Threshold = 82, .Cooldown = 6})
        defaultCfg.Triggers.Add(New TriggerConfig With {.Id = "msg_player", .Name = "MENSAGEM DE JOGADOR", .SoundFile = "msg_player.wav", .Threshold = 78, .Cooldown = 6})
        defaultCfg.Triggers.Add(New TriggerConfig With {.Id = "teleport", .Name = "TELEPORT", .SoundFile = "teleport.wav", .Threshold = 80, .Cooldown = 5})
        defaultCfg.Triggers.Add(New TriggerConfig With {.Id = "pokemon", .Name = "POKEMON FORA DA HUNT", .SoundFile = "pokemon.wav", .Threshold = 80, .Cooldown = 10})
        defaultCfg.Triggers.Add(New TriggerConfig With {.Id = "seta", .Name = "SOM DE SETA", .SoundFile = "seta.wav", .Threshold = 80, .Cooldown = 5})
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
