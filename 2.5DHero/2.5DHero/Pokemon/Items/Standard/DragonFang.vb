Namespace Items.Standard

    Public Class DragonFang

        Inherits Item

        Public Sub New()
            MyBase.New("Dragon Fang", 100, ItemTypes.Standard, 144, 1, 0, New Rectangle(432, 120, 24, 24), "An item to be held by a Pokémon. This hard and sharp fang boosts the power of Dragon-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 70
        End Sub

    End Class

End Namespace