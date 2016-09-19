Namespace Items.KeyItems

    <Item(69, "Mystery Egg")>
    Public Class MysteryEgg

        Inherits Item

        Public Sub New()
            MyBase.New("Mystery Egg", 9800, ItemTypes.KeyItems, 69, 1, 0, New Rectangle(0, 72, 24, 24), "A mysterious Egg obtained from Mr. Pokémon. What's in the Egg is unknown.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace
