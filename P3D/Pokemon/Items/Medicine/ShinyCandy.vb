Namespace Items.Medicine

    <Item(501, "Shiny Candy")>
    Public Class ShinyCandy

        Inherits MedicineItem

        Public Overrides ReadOnly Property Description As String = "This mysterious candy sparkles when unwrapped. It attracts all sorts of Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 4800
        Public Overrides ReadOnly Property MaxStack As Integer = 1
        Public Overrides ReadOnly Property CanBeHold As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(96, 240, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If Not p.IsShiny Then
                p.IsShiny = True

                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show("The Pokémon sparkled." & RemoveItem())
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                Return True
            Else
                Screen.TextBox.Show("Cannot use shiny candy~on this Pokémon.", {}, False, False)

                Return False
            End If

        End Function

    End Class

End Namespace
