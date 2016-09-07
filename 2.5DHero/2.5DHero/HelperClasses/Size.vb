Public Class Size

    Private _Width As Integer
    Private _Height As Integer

    Public Property Width As Integer
        Get
            Return _Width
        End Get
        Set(value As Integer)
            _Width = value
        End Set
    End Property

    Public Property Height As Integer
        Get
            Return _Height
        End Get
        Set(value As Integer)
            _Height = value
        End Set
    End Property

    Public Sub New(ByVal Width As Integer, ByVal Height As Integer)
        Me._Width = Width
        Me._Height = Height
    End Sub

    Public Sub Input(ByVal Width As Integer, ByVal Height As Integer)
        Me._Width = Width
        Me._Height = Height
    End Sub

End Class