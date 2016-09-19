Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Medicham.
    ''' </summary>
    <Item(528, "Medichamite")>
    Public Class Medichamite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Medichamite", 528, New Rectangle(24, 48, 24, 24), "Medicham", 308)
        End Sub

    End Class

End Namespace
