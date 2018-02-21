Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Steelix.
    ''' </summary>
    <Item(552, "Steelixite")>
    Public Class Steelixite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Steelix", 208)
            _textureRectangle = New Rectangle(168, 96, 24, 24)
        End Sub

    End Class

End Namespace
