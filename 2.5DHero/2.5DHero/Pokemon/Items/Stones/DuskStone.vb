Namespace Items.Standard

    <Item(136, "Dusk Stone")>
    Public Class DuskStone

        Inherits StoneItem

        Public Sub New()
            MyBase.New("Dusk Stone", 2100, ItemTypes.Standard, 136, 1, 0, New Rectangle(360, 192, 24, 24), "A peculiar stone that makes certain species of Pokémon evolve. It is as dark as dark can be.")
        End Sub

    End Class

End Namespace
