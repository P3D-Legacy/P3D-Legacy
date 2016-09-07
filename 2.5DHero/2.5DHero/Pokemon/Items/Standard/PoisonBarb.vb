Namespace Items.Standard

    Public Class PoisonBarb

        Inherits Item

        Public Sub New()
            MyBase.New("Poison Barb", 100, ItemTypes.Standard, 81, 1, 0, New Rectangle(192, 72, 24, 24), "An item to be held by a Pokémon. This small, poisonous barb boosts the power of Poison-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 70
        End Sub

    End Class

End Namespace