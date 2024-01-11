Namespace Items.Berries

    <Item(2039, "Yache")>
    Public Class YacheBerry

        Inherits Berry
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Sub New()
            MyBase.New(64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Ice-type attack.", "13.5cm", "Very Hard", 1, 5)

            Me.Spicy = 0
            Me.Dry = 10
            Me.Sweet = 0
            Me.Bitter = 15
            Me.Sour = 0

            Me.Type = Element.Types.Ice
            Me.Power = 80
            Me.JuiceColor = "blue"
            Me.JuiceGroup = 2
        End Sub

    End Class

End Namespace
