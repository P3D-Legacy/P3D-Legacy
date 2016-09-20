Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Lopunnite.
    ''' </summary>
    <Item(544, "Lopunnite")>
    Public Class Lopunnite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Lopunny", 428)
            _textureRectangle = New Rectangle(216, 72, 24, 24)
        End Sub

    End Class

End Namespace
