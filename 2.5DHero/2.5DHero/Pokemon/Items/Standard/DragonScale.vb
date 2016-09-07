Namespace Items.Standard

    Public Class DragonScale

        Inherits Item

        Public Sub New()
            MyBase.New("Dragon Scale", 2100, ItemTypes.Standard, 151, 1, 0, New Rectangle(480, 120, 24, 24), "A very tough and inflexible scale. Dragon-type Pokémon may be holding this item when caught.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace