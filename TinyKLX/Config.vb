Public Class Config

	''' <summary>
	''' Run / Run Once key name and Exe File name
	''' </summary>
	Public Const AppName As String = "AppModule"

	''' <summary>
	''' Gets the commandline string safe for string
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property SafeCommand As String
		Get
			Return "startup"
		End Get
	End Property

	''' <summary>
	''' Error message shown to user when run by hands
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property UserErrorMessage As String
		Get
			Return "Application crashed due to unhandled error"
		End Get
	End Property

	''' <summary>
	''' Gets the height for image captured
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property CaptureHeight As Integer
		Get
			Return 480
		End Get
	End Property

	''' <summary>
	''' Gets the thumbnail quality
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property CaptureQuality As Byte
		Get
			Return 30
		End Get
	End Property

	''' <summary>
	''' Gets the number of events to be captured before sending to server
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property MaxEvents As Integer
		Get
			Return 200
		End Get
	End Property

	''' <summary>
	''' Smtp Server used for mail sink
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property SmtpServer As String
		Get
			Return "smtp.gmail.com"
		End Get
	End Property

	''' <summary>
	''' Smtp Server port used for email sink
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property SmtpPort As Integer
		Get
			Return 587
		End Get
	End Property

	''' <summary>
	''' Sender email address used for email sink
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property Sender As String
		Get
			Return ""
		End Get
	End Property

	''' <summary>
	''' Email password for email sink
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property Password As String
		Get
			Return ""
		End Get
	End Property

	''' <summary>
	''' Receiver email address for email sink
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property Receiver As String
		Get
			Return ""
		End Get
	End Property

End Class
