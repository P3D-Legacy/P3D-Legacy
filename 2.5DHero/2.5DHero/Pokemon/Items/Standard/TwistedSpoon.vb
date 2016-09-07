Namespace Items.Standard
    'LOL Uri Gella is raging about this one
    Public Class TwistedSpoon

        Inherits Item

        Public Sub New()
            MyBase.New("Twisted Spoon", 100, ItemTypes.Standard, 96, 1, 0, New Rectangle(480, 72, 24, 24), "An item to be held by a Pokémon. It is a spoon imbued with telekinetic power that boosts Psychic-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace