Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Audino.
    ''' </summary>
    <Item(536, "Audinite")>
    Public Class Audinite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Audino", 531)
            _textureRectangle = New Rectangle(24, 72, 24, 24)
        End Sub

    End Class

End Namespace
