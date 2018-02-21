Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Garchompite.
    ''' </summary>
    <Item(518, "Garchompite")>
    Public Class Garchompite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Garchompite", 445)
            _textureRectangle = New Rectangle(24, 24, 24, 24)
        End Sub

    End Class

End Namespace
