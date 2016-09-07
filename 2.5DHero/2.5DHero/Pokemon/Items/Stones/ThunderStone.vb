Namespace Items.Stones

    Public Class ThunderStone

        Inherits Items.StoneItem

        Public Sub New()
            MyBase.New("Thunder Stone", 2100, ItemTypes.Standard, 23, 1, 0, New Rectangle(0, 24, 24, 24), "A peculiar stone that can make certain species of Pokémon evolve. It has a distinct thunderbolt pattern.")
        End Sub

    End Class

End Namespace