Namespace Items.MegaStones

    ''' <summary>
    ''' A Mega Stone for Charizard.
    ''' </summary>
    <Item(516, "Charizardite X")>
    Public Class CharizarditeX

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Charizardite X", 516, New Rectangle(216, 0, 24, 24), "Charizard", 6)
        End Sub

    End Class

End Namespace
