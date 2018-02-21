Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Absol.
    ''' </summary>
    <Item(508, "Absolite")>
    Public Class Absolite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Absol", 359)
            _textureRectangle = New Rectangle(24, 0, 24, 24)
        End Sub

    End Class

End Namespace
