Namespace Items.Medicine

    <Item(501, "Shiny Candy")>
    Public Class ShinyCandy

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "This mysterious candy sparkles when unwrapped. It attracts all sorts of Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 4800

        Public Sub New()
            _textureRectangle = New Rectangle(96, 240, 24, 24)
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            p.IsShiny = Not p.IsShiny

            SoundManager.PlaySound("single_heal", False)
            Screen.TextBox.Show("The Pokémon sparkled." & RemoveItem())
            PlayerStatistics.Track("[17]Medicine Items used", 1)

            Return True
        End Function

    End Class

End Namespace
