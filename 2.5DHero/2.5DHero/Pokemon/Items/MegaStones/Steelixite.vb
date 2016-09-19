Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Steelix.
    ''' </summary>
    <Item(552, "Steelixite")>
    Public Class Steelixite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Steelixite", 552, New Rectangle(168, 96, 24, 24), "Steelix", 208)
        End Sub

    End Class

End Namespace
