Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Altaria.
    ''' </summary>
    <Item(535, "Altarianite")>
    Public Class Altarianite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Altarianite", 535, New Rectangle(0, 72, 24, 24), "Altaria", 334)
        End Sub

    End Class

End Namespace
