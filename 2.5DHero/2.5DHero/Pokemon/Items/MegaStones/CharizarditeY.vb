Namespace Items.MegaStones

    ''' <summary>
    ''' A Mega Stone for Charizard.
    ''' </summary>
    <Item(517, "Charizardite Y")>
    Public Class CharizarditeY

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Charizardite Y", 517, New Rectangle(0, 24, 24, 24), "Charizard", 6)
        End Sub

    End Class

End Namespace
