Namespace Items.KeyItems

    <Item(54, "Coin Case")>
    Public Class CoinCase

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A case for holding coins obtained at the Game Corner. It holds up to 50,000 coins."
        Public Overrides ReadOnly Property CanBeUsed As Boolean = True

        Public Sub New()
            _textureRectangle = New Rectangle(168, 48, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Screen.TextBox.Show("Your coins:~" & Core.Player.Coins, {}, True, True)
        End Sub

    End Class

End Namespace
