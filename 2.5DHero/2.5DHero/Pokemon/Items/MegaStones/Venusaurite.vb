Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Venusaur.
    ''' </summary>
    <Item(534, "Venusaurite")>
    Public Class Venusaurite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Venusaurite", 534, New Rectangle(168, 48, 24, 24), "Venusaur", 3)
        End Sub

    End Class

End Namespace
