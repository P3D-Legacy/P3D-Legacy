Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Heracross.
    ''' </summary>
    <Item(522, "Heracronite")>
    Public Class Heracronite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Heracronite", 522, New Rectangle(120, 24, 24, 24), "Heracross", 214)
        End Sub

    End Class

End Namespace
