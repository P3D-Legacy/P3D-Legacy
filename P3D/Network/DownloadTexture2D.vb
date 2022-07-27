Imports System.Net

Public NotInheritable Class DownloadTexture2D

    Public Shared Function GetRemoteTexture2D(graphics As GraphicsDevice, url As String, logError As Boolean) As Texture2D
        Try
            Using stream = GetResponseStream(url)
                Dim result = Texture2D.FromStream(graphics, stream)
                Return result
            End Using
        Catch ex As Exception
            If logError = True Then
                Logger.Log(Logger.LogTypes.ErrorMessage, "DownloadTexture2D.vb: Failed to download image from """ & url & """!")
            End If
            Return Nothing
        End Try
    End Function

    Private Shared Function GetResponseStream(url As String) As Stream
        Dim request As WebRequest = WebRequest.Create(url)
        request.Method = "GET"
        Return request.GetResponse().GetResponseStream()
    End Function

End Class