Public Class PlayerTemp

    Public Property DayCareCycle() As Integer
        Get
            Return Me._dayCareCycle
        End Get
        Set(value As Integer)
            Me._dayCareCycle = value
        End Set
    End Property

    Private _dayCareCycle As Integer = 256

    Public Sub New()
        Me.Reset()
    End Sub

    Public Sub Reset()
        Me.DayCareCycle = 256
    End Sub

End Class