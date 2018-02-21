Imports System.IO

Public Class StreamWriterLock
    Inherits StreamWriter
    Private _lock As New Object()

    Public Sub New(stream As Stream)
        MyBase.New(stream)
    End Sub

    Public Overrides Sub Write(value As Char)
        SyncLock _lock
            MyBase.Write(value)
        End SyncLock
    End Sub
End Class