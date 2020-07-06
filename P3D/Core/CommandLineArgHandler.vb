﻿Module CommandLineArgHandler

    Private _forceGraphics As Boolean = False
    Private _nosplash As Boolean = False

    Public Sub Initialize(ByVal args() As String)
        If args.Length > 0 Then
            If args.Any(Function(arg As String)
                     Return arg = "-forcegraphics"
                     End Function) Then
                _forceGraphics = True
            End If
            If args.Any(Function(arg As String)
                     Return arg = "-nosplash"
                     End Function) Then
                _nosplash = True
            End If
        End If

        For Each arg As String In args
            If arg.Contains(":") = True Then
                Dim identifier As String = arg.Remove(arg.IndexOf(":"))
                Dim value As String = arg.Remove(0, arg.IndexOf(":") + 1)

                Select Case identifier
                    Case "MAP"
                        MapPreviewScreen.DetectMapPath(value)
                End Select
            End If
        Next
    End Sub

    Public ReadOnly Property ForceGraphics() As Boolean
        Get
            Return _forceGraphics
        End Get
    End Property

        Public ReadOnly Property NoSplash() As Boolean
        Get
            Return _nosplash
        End Get
    End Property

End Module