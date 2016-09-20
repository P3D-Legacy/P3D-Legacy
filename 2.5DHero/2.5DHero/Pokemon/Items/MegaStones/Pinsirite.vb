Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Pinsir.
    ''' </summary>
    <Item(531, "Pinsirite")>
    Public Class Pinsirite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Pinsir", 127)
            _textureRectangle = New Rectangle(96, 48, 24, 24)
        End Sub

    End Class

End Namespace
