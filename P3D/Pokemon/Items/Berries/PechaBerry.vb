Namespace Items.Berries

    <Item(2002, "Pecha")>
    Public Class PechaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(10800, "A Berry to be consumed by Pokémon. If a Pokémon holds one, it can recover from poisoning on its own in battle.", "4.0cm", "Very Soft", 2, 3)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Electric
            Me.Power = 80
            Me.JuiceColor = "pink"
            Me.JuiceGroup = 1
        End Sub

        Public Overrides Sub Use()
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return CurePoison(PokeIndex)
        End Function

    End Class

End Namespace
