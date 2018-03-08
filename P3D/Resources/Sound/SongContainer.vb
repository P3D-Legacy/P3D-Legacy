' Created because Song.Name is not what it is being set to.
' also because when loading songs from uris, it doesn't calculate its duration
Public Class SongContainer

    Public Song As Song
    Public Name As String

    Public Sub New(song As Song, name As String, duration As TimeSpan)
        Me.Song = song
        Me.Name = name

        Dim durationField = song.GetType().GetField("_duration", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
        durationField.SetValue(song, duration)
    End Sub

End Class
