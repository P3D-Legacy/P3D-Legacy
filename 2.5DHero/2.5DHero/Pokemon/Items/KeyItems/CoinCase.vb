Namespace Items.KeyItems

    <Item(54, "Coin Case")>
    Public Class CoinCase

        Inherits Item

        Public Sub New()
            MyBase.New("Coin Case", 1337, ItemTypes.KeyItems, 54, 1, 0, New Rectangle(168, 48, 24, 24), "A case for holding coins obtained at the Game Corner. It holds up to 50,000 coins.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

        Public Overrides Sub Use()
            Screen.TextBox.Show("Your COINS:~" & Core.Player.Coins, {}, True, True)
        End Sub

    End Class

End Namespace
