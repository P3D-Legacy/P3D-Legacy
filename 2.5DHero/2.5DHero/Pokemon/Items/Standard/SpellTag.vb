Namespace Items.Standard

    Public Class SpellTag

        Inherits Item

        Public Sub New()
            MyBase.New("Spell Tag", 100, ItemTypes.Standard, 113, 1, 0, New Rectangle(336, 96, 24, 24), "An item to be held by a Pokémon. It is a sinister, eerie tag that boosts the power of Ghost-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace