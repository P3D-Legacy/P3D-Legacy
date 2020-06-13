' Created because Song.Name is not what it is being set to.
' also because when loading songs from uris, it doesn't calculate its duration
Public Class SongContainer

	Public Song As String
	Public Name As String
	Public Origin As String

	Public Sub New(song As String, name As String, origin As String)
		Me.Song = song
		Me.Name = name
		Me.Origin = origin
	End Sub


	Public ReadOnly Property IsStandardSong() As Boolean
		Get
			Return (Me.Origin = "Content")
		End Get
	End Property

End Class
