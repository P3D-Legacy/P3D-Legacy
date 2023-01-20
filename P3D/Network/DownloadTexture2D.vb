Public NotInheritable Class DownloadTexture2D

    Public Shared Function GetRemoteTexture2D(graphics As GraphicsDevice, url As String, logError As Boolean) As Texture2D
        Try
            Using stream = Core.HttpClient.GetStreamAsync(url).GetAwaiter().GetResult()
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

End Class