Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Pinsir.
    ''' </summary>
    <Item(531, "Pinsirite")>
    Public Class Pinsirite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Pinsirite", 531, New Rectangle(96, 48, 24, 24), "Pinsir", 127)
        End Sub

    End Class

End Namespace
