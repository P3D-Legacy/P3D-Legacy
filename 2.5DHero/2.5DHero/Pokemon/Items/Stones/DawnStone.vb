Namespace Items.Standard

    <Item(137, "Dawn Stone")>
    Public Class DawnStone

        Inherits StoneItem

        Public Sub New()
            MyBase.New("Dawn Stone", 2100, ItemTypes.Standard, 137, 1, 0, New Rectangle(384, 192, 24, 24), "A peculiar stone that makes certain species of Pokémon evolve. It sparkles like eyes.")
        End Sub

    End Class

End Namespace
