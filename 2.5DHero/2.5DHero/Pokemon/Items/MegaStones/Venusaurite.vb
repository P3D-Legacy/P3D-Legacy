Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Venusaur.
    ''' </summary>
    <Item(534, "Venusaurite")>
    Public Class Venusaurite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Venusaur", 3)
            _textureRectangle = New Rectangle(168, 48, 24, 24)
        End Sub

    End Class

End Namespace
