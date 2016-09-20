Namespace Items.MegaStones

    ''' <summary>
    ''' A Mega Stone for Charizard.
    ''' </summary>
    <Item(517, "Charizardite Y")>
    Public Class CharizarditeY

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Charizard", 6)
            _textureRectangle = New Rectangle(0, 24, 24, 24)
        End Sub

    End Class

End Namespace
