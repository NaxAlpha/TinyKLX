Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Forms

''' <summary>
''' Represents Capture engine for screen and related devices
''' </summary>
Public Class Capture
	Implements IDisposable

	Private Shared jpegCodecInfo As ImageCodecInfo
	Private encoderParams As EncoderParameters

	Private _width As Integer
	Private _height As Integer
	Private _quality As Integer

	Private buffer As MemoryStream

	Private surface As Image
	Private gsurface As Graphics

	Private resized As Image
	Private gresized As Graphics

	''' <summary>
	''' Gets the primary screen bounds
	''' </summary>
	''' <returns></returns>
	Public Shared ReadOnly Property ScreenBounds As Rectangle
		Get
			Return Screen.PrimaryScreen.Bounds
		End Get
	End Property

	''' <summary>
	''' Gets or sets quality of JPEG image output
	''' </summary>
	''' <returns></returns>
	Public Property Quality As Integer
		Get
			Return _quality
		End Get
		Set(value As Integer)
			If value <> _quality Then
				encoderParams = New EncoderParameters(1)
				encoderParams.Param(0) = New EncoderParameter(Encoder.Quality, value)
			End If
			_quality = value
		End Set
	End Property

	''' <summary>
	''' Resizes to new width
	''' </summary>
	''' <returns></returns>
	Public Property Width As Integer
		Get
			Return _width
		End Get
		Set(value As Integer)
			_width = value
			Refresh()
		End Set
	End Property

	''' <summary>
	''' Resizes to new height
	''' </summary>
	''' <returns></returns>
	Public Property Height As Integer
		Get
			Return _height
		End Get
		Set(value As Integer)
			_height = value
			Refresh()
		End Set
	End Property

	Shared Sub New()
		For Each codec In ImageCodecInfo.GetImageDecoders()
			If codec.FormatID = ImageFormat.Jpeg.Guid Then
				jpegCodecInfo = codec
				Exit For
			End If
		Next
	End Sub

	''' <summary>
	''' Keeps aspect ratio chaning width relative to given height
	''' </summary>
	''' <param name="heightLimit"></param>
	Public Sub New(heightLimit As Integer)
		Me.New(heightLimit * ScreenBounds.Width / ScreenBounds.Height, heightLimit)
	End Sub

	''' <summary>
	''' Creates new instance of Capture Engine for primary screen
	''' </summary>
	Public Sub New(width As Integer, height As Integer)
		Quality = 100
		_width = width
		_height = height
		Refresh()
	End Sub

	''' <summary>
	''' Destroyes resources used by Capture engine
	''' </summary>
	Public Sub Dispose() Implements IDisposable.Dispose
		gresized?.Dispose()
		resized?.Dispose()

		gsurface?.Dispose()
		surface?.Dispose()

		buffer?.Dispose()
	End Sub

	''' <summary>
	''' Disposes all the buffer and recreates, used when screens change
	''' </summary>
	Public Sub Refresh()

		Dispose()

		buffer = New MemoryStream()

		surface = New Bitmap(ScreenBounds.Width, ScreenBounds.Height)
		gsurface = Graphics.FromImage(surface)

		resized = New Bitmap(Width, Height)
		gresized = Graphics.FromImage(resized)

	End Sub

	''' <summary>
	''' Captures frame and return captured image
	''' </summary>
	''' <returns></returns>
	Public Function TakeShot() As Byte()
		Dim s = ScreenBounds
		gsurface.CopyFromScreen(s.X, s.Y, 0, 0, surface.Size)
		Cursor.Current.Draw(gsurface, New Rectangle(Cursor.Position, Cursor.Current.Size))
		gresized.DrawImage(surface, 0, 0, Width, Height)

		buffer.SetLength(0)
		buffer.Position = 0
		resized.Save(buffer, jpegCodecInfo, encoderParams)
		Return buffer.ToArray()
	End Function

End Class
