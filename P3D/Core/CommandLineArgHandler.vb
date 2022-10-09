Module CommandLineArgHandler
    Public ReadOnly Property ForceGraphics As Boolean

    Public ReadOnly Property NoSplash As Boolean

    Public Sub Initialize(args() As String)
        For Each arg As String In args
            If arg.Equals("-forcegraphics", StringComparison.OrdinalIgnoreCase) Then
                _ForceGraphics = True
            End If

            If arg.Equals("-nosplash", StringComparison.OrdinalIgnoreCase) Then
                _NoSplash = True
            End If

            If arg.Contains(":"c) = True Then
                Dim identifier As String = arg.Remove(arg.IndexOf(":", StringComparison.Ordinal))
                Dim value As String = arg.Remove(0, arg.IndexOf(":", StringComparison.Ordinal) + 1)

                Select Case identifier
                    Case "MAP"
                        MapPreviewScreen.DetectMapPath(value)
                End Select
            End If
        Next
    End Sub
End Module