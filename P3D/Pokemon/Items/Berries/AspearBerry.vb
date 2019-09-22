Namespace Items.Berries

    <Item(2004, "Aspear")>
    Public Class AspearBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(10800, "A Berry to be consumed by Pokémon. If a Pokémon holds one, it can recover from being frozen on its own in battle.", "5.0cm", "Super Hard", 2, 3)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 10

            Me.Type = Element.Types.Ice
            Me.Power = 80
        End Sub

        Public Overrides Sub Use()
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return HealIce(PokeIndex)
        End Function

    End Class

End Namespace
