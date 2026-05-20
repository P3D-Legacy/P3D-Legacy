Namespace Items.Standard

    <Item(187, "Ability Capsule")>
    Public Class AbilityCapsule

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A capsule that allows a Pokémon with two Abilities to switch between these Abilities when it is used."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(0, 240, 24, 24)
        End Sub

        Public Overrides Sub Use()
            If Core.Player.Pokemons.Count > 0 Then
                Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                Core.SetScreen(selScreen)
            Else
                Screen.TextBox.Show(Localization.GetString("item_cannot_use_NoPokemon", "You don't have any Pokémon."), {}, False, False)
            End If
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean

            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If p.NewAbilities.Count = 1 Then
                Screen.TextBox.Show("You cannot use this on~" & p.GetDisplayName() & ".")
                Return False
            End If

            If p.AbilitySlot = "H" Or p.AbilitySlot = "C" Or StringHelper.IsNumeric(p.AbilitySlot) = True Then
                Screen.TextBox.Show("You cannot use this on~" & p.GetDisplayName() & ".")
                Return False
            Else
                Dim oldAbilityName As String = p.Ability.Name

                If p.AbilitySlot = "A" Then
                    p.AbilitySlot = "B"
                Else
                    p.AbilitySlot = "A"
                End If
                Dim newAbilityName As String = p.Ability.Name
                p._ability = p.Ability
                p.SetOriginalAbility()

                Screen.TextBox.Show(p.GetDisplayName() & " forgot how~to use " & oldAbilityName & ".*It learned~" & newAbilityName & " instead." & RemoveItem())
                Return True
            End If
        End Function

    End Class

End Namespace
