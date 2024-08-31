Namespace Items.KeyItems

    <Item(658, "Exp. All")>
    Public Class ExpAll

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "Turning on this special device will allow all the Pokémon on your team to receive Exp. Points from battles."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 3000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = True

        Public Sub New()
            _textureRectangle = New Rectangle(216, 48, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Core.Player.EnableExpAll = Not Core.Player.EnableExpAll
            If Core.Player.EnableExpAll = False Then
                Screen.TextBox.Show(Localization.GetString("item_use_658_disable", "The Exp. All was turned off."))
            Else
                Screen.TextBox.Show(Localization.GetString("item_use_658_enable", "The Exp. All was turned on."))
            End If

        End Sub

    End Class

End Namespace
