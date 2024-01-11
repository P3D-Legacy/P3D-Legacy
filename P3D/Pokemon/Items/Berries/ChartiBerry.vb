Namespace Items.Berries

    <Item(2046, "Charti")>
    Public Class ChartiBerry

        Inherits Berry
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Sub New()
            MyBase.New(64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Rock-type attack.", "2.8cm", "Very Soft", 1, 5)

            Me.Spicy = 10
            Me.Dry = 20
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Rock
            Me.Power = 80
            Me.JuiceColor = "yellow"
            Me.JuiceGroup = 2
        End Sub

    End Class

End Namespace
