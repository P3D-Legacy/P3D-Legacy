Namespace Items.Standard

    Public Class LuckyEgg

        Inherits Item

        Public Sub New()
            MyBase.New("Lucky Egg", 100, ItemTypes.Standard, 126, 1, 0, New Rectangle(120, 120, 24, 24), "An item to be held by a Pokémon. It is an egg filled with happiness that earns extra Exp. Points in battle.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace