Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Latios.
    ''' </summary>
    <Item(543, "Latiosite")>
    Public Class Latiosite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Latiosite", 543, New Rectangle(192, 72, 24, 24), "Latios", 381)
        End Sub

    End Class

End Namespace
