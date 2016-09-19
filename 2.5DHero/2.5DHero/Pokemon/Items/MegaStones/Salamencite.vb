Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Salamence.
    ''' </summary>
    <Item(548, "Salamencite")>
    Public Class Salamencite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Salamencite", 548, New Rectangle(72, 96, 24, 24), "Salamence", 373)
        End Sub

    End Class

End Namespace
