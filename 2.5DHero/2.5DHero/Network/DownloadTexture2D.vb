Imports System.Net

Public NotInheritable Class DownloadTexture2D

    Const n_tempPath As String = "\Temp\"

    Private Shared n_tempfiles(0) As String
    Private Shared n_tempstreams(0) As IO.FileStream

    Private Shared Function n_RemoteStream(URL As String) As IO.Stream
        Dim Request As WebRequest = WebRequest.Create(URL)
        Request.Method = "GET"
        Return Request.GetResponse().GetResponseStream()
    End Function

    Public Shared Function n_Remote_Texture2D(Graphics As GraphicsDevice, URL As String, ByVal logError As Boolean) As Texture2D
        Try
            If My.Computer.FileSystem.DirectoryExists(GameController.GamePath & n_tempPath) = False Then
                My.Computer.FileSystem.CreateDirectory(GameController.GamePath & n_tempPath)
            End If

            Dim Filename As String = GameController.GamePath & n_tempPath & Date.Now.Ticks.ToString & ".tmp"

            Dim tI As Integer = 1
            If My.Computer.FileSystem.FileExists(Filename) Then
                While True
                    If Not My.Computer.FileSystem.FileExists(Filename & tI) Then
                        Filename &= tI
                    Else
                        tI += 1
                    End If
                End While
            End If

            Dim S = n_RemoteStream(URL)
            Dim F = IO.File.Open(Filename, IO.FileMode.CreateNew)

            Dim Buffer(0) As Byte

            Try
                Dim I As Int32 = 0
                While True
                    Dim II = S.ReadByte
                    If II = -1 Then Exit While
                    Array.Resize(Buffer, Buffer.Length + 1)
                    Buffer(I) = Convert.ToByte(II)
                    I += 1
                End While
            Catch
            End Try

            F.Write(Buffer, 0, Buffer.Length)

            S.Close()
            F.Close()

            Array.Resize(n_tempstreams, n_tempstreams.Length + 1)
            n_tempstreams(n_tempstreams.Length - 1) = New IO.FileStream(Filename, IO.FileMode.Open)

            Dim Result = Texture2D.FromStream(Graphics, n_tempstreams(n_tempstreams.Length - 1))

            Array.Resize(n_tempfiles, n_tempfiles.Length + 1)
            n_tempfiles(n_tempfiles.Length - 1) = Filename

            Return Result

        Catch ex As Exception
            If logError = True Then
                Logger.Log(Logger.LogTypes.ErrorMessage, "DownloadTexture2D.vb: Failed to download image from """ & URL & """!")
            End If
            Return Nothing
        End Try
    End Function

    Public Shared Sub n_CleanupTempData()
        For i = 0 To n_tempstreams.Length - 1
            Try
                n_tempstreams(i).Close()

                n_tempstreams(i).Dispose()
            Catch : End Try
        Next

        For i = 0 To n_tempfiles.Length - 1
            Try
                My.Computer.FileSystem.DeleteFile(n_tempfiles(i))
            Catch : End Try
        Next

        Array.Resize(n_tempfiles, 0)
        Array.Resize(n_tempstreams, 0)
    End Sub

End Class