Namespace Items.Stones

    Public Class Everstone

        Inherits Item

        Public Sub New()
            MyBase.New("Everstone", 200, ItemTypes.Standard, 112, 1, 0, New Rectangle(312, 96, 24, 24), "An item to be held by a Pokémon. A Pokémon holding this peculiar stone is prevented from evolving.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace