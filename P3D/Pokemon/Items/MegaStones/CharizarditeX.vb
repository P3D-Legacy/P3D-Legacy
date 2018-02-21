Namespace Items.MegaStones

    ''' <summary>
    ''' A Mega Stone for Charizard.
    ''' </summary>
    <Item(516, "Charizardite X")>
    Public Class CharizarditeX

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Charizard", 6)
            _textureRectangle = New Rectangle(216, 0, 24, 24)
        End Sub

    End Class

End Namespace
