Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Gengar.
    ''' </summary>
    <Item(520, "Gengarite")>
    Public Class Gengarite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Gengarite", 520, New Rectangle(72, 24, 24, 24), "Gengar", 94)
        End Sub

    End Class

End Namespace
