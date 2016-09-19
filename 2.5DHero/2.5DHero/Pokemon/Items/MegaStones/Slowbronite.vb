Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Slowbro.
    ''' </summary>
    <Item(551, "Slowbronite")>
    Public Class Slowbronite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Slowbronite", 551, New Rectangle(144, 96, 24, 24), "Slowbro", 80)
        End Sub

    End Class

End Namespace
