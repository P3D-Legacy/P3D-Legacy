Namespace Items.KeyItems

    <Item(67, "SecretPotion")>
    Public Class SecretPotion

        Inherits Item

        Public Sub New()
            MyBase.New("SecretPotion", 138, ItemTypes.KeyItems, 67, 1, 0, New Rectangle(456, 48, 24, 24), "A fantastic medicine dispensed by the pharmacy in Cianwood City. It fully heals a Pokémon of any ailment.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace
