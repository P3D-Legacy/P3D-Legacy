Namespace Items.Berries

    <Item(2001, "Chesto")>
    Public Class ChestoBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(10800, "A Berry to be consumed by Pokémon. If a Pokémon holds one, it can recover from sleep on its own in battle.", "8.0cm", "Super Hard", 2, 3)

            Me.Spicy = 0
            Me.Dry = 10
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Water
            Me.Power = 80
            Me.JuiceColor = "purple"
            Me.JuiceGroup = 1
        End Sub

        Public Overrides Sub Use()
            If Core.Player.Pokemons.Count > 0 Then
                Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                Core.SetScreen(selScreen)
            Else
                Screen.TextBox.Show("You don't have any Pokémon.", {}, False, False)
            End If
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return WakeUp(PokeIndex)
        End Function

    End Class

End Namespace
