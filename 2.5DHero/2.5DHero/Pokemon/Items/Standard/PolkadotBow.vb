Namespace Items.Standard

    Public Class PolkadotBow

        Inherits Item

        Public Sub New()
            MyBase.New("Polkadot Bow", 100, ItemTypes.Standard, 170, 1, 1, New Rectangle(192, 240, 24, 24), "A pink bow. A certain Pokémon likes this item.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace