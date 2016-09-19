Namespace Items.Stones

    <Item(22, "Fire Stone")>
    Public Class FireStone

        Inherits StoneItem

        Public Sub New()
            MyBase.New("Fire Stone", 2100, ItemTypes.Standard, 22, 1, 0, New Rectangle(480, 0, 24, 24), "A peculiar stone that can make certain species of Pokémon evolve. The stone has a fiery orange heart.")
        End Sub

    End Class

End Namespace
