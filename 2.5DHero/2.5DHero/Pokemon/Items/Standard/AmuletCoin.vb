Namespace Items.Standard

    Public Class AmuletCoin

        Inherits Item

        Public Sub New()
            MyBase.New("Amulet Coin", 100, ItemTypes.Standard, 91, 1, 1, New Rectangle(360, 72, 24, 24), "An item to be held by a Pokémon. It doubles any prize money received if the holding Pokémon joins in a battle.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace