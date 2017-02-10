Imports System.IO
Imports System.Security.Principal
Imports System.Threading
Imports System.Threading.Tasks

Public Class Utils

	''' <summary>
	''' Starts action asynchronouly
	''' </summary>
	''' <param name="action"></param>
	Public Shared Sub Async(action As Action)
		Dim t As New Task(action)
		t.Start()
	End Sub

	''' <summary>
	''' Gets if App is running as Admin privilages
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property IsAdmin As Boolean
		Get
			Return New WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)
		End Get
	End Property

	''' <summary>
	''' Waits for condition to be true by blocking current thread without load
	''' </summary>
	''' <param name="condition"></param>
	Public Shared Sub WaitFor(condition As Func(Of Boolean))
		Dim sw As New SpinWait
		While Not condition()
			sw.SpinOnce()
		End While
	End Sub

	Public Shared Function NormalizePath(path__1 As String) As String
		Return Path.GetFullPath(New Uri(path__1).LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant()
	End Function

	''' <summary>
	''' Logs message only if debug mode
	''' </summary>
	''' <param name="msg">message to be logged</param>
	Public Shared Sub Log(msg As String)
#If _MyType = "Console" Then
#If DEBUG Then
		Dim st As New StackTrace
		Console.WriteLine(New String(vbTab, st.FrameCount - 8) + st.GetFrame(1).GetMethod().Name + " - " + msg)
#Else
		Console.WriteLine(msg)
#End If
#End If
	End Sub

End Class
