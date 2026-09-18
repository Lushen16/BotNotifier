Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Collections.Generic
Imports System.Web.Script.Serialization

Public Class DiscordNotifier
    Public Shared Function SendAlert(webhookUrl As String, triggerName As String, confidence As Integer, threshold As Integer, mentionType As String, mentionId As String, colorHex As String) As Boolean
        If String.IsNullOrEmpty(webhookUrl) OrElse Not webhookUrl.StartsWith("https://discord.com/api/webhooks/") Then
            Return False
        End If

        Dim mentionText As String = ""
        If mentionType = "everyone" Then
            mentionText = "@everyone"
        ElseIf mentionType = "here" Then
            mentionText = "@here"
        ElseIf mentionType = "role" AndAlso Not String.IsNullOrEmpty(mentionId) Then
            mentionText = "<@&" & mentionId.Trim() & ">"
        ElseIf mentionType = "user" AndAlso Not String.IsNullOrEmpty(mentionId) Then
            mentionText = "<@" & mentionId.Trim() & ">"
        End If

        Dim colorInt As Integer = 16713796
        Try
            colorInt = Convert.ToInt32(colorHex.Replace("#", ""), 16)
        Catch
        End Try

        Dim payload As New Dictionary(Of String, Object) From {
            {"username", "Charm Bot Notifier"},
            {"avatar_url", "https://i.imgur.com/8Qp49X0.png"}
        }

        If Not String.IsNullOrEmpty(mentionText) Then
            payload("content") = mentionText & " **Alerta Acústico Detectado no Jogo!**"
        End If

        Dim embed As New Dictionary(Of String, Object) From {
            {"title", "🚨 ALERTA: " & triggerName.ToUpper() & " DETECTADO!"},
            {"description", "O som correspondente a **" & triggerName & "** foi identificado no áudio do sistema com sucesso."},
            {"color", colorInt},
            {"timestamp", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")}
        }

        Dim fields As New List(Of Dictionary(Of String, Object)) From {
            New Dictionary(Of String, Object) From {{"name", "🎯 Categoria"}, {"value", "`" & triggerName & "`"}, {"inline", True}},
            New Dictionary(Of String, Object) From {{"name", "📊 Confiança"}, {"value", "**" & confidence.ToString() & "%** (Limiar: " & threshold.ToString() & "%)"}, {"inline", True}},
            New Dictionary(Of String, Object) From {{"name", "⏰ Horário"}, {"value", "`" & DateTime.Now.ToString("HH:mm:ss") & "`"}, {"inline", True}},
            New Dictionary(Of String, Object) From {{"name", "🖥️ Plataforma"}, {"value", "Charm Bot Notifier (Windows Desktop)"}, {"inline", True}}
        }

        embed("fields") = fields
        payload("embeds") = New List(Of Object) From {embed}

        Try
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType) Or SecurityProtocolType.Tls
            Dim client As New WebClient()
            client.Headers(HttpRequestHeader.ContentType) = "application/json; charset=utf-8"

            Dim serializer As New JavaScriptSerializer()
            Dim json As String = serializer.Serialize(payload)
            Dim bytes As Byte() = Encoding.UTF8.GetBytes(json)

            client.UploadData(webhookUrl, "POST", bytes)
            Return True
        Catch ex As Exception
            Console.WriteLine("Erro Discord: " & ex.Message)
            Return False
        End Try
    End Function

    Public Shared Function SendTest(webhookUrl As String, mentionType As String, mentionId As String) As Boolean
        Return SendAlert(webhookUrl, "TESTE DE CONEXÃO", 100, 80, mentionType, mentionId, "#00FF87")
    End Function
End Class
