Namespace Items.Berries

    Public Class BabiriBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2050, "Babiri", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Steel-type attack.", "26.5cm", "Super Hard", 1, 5)

            Me.Spicy = 25
            Me.Dry = 10
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Steel
            Me.Power = 60
        End Sub

    End Class

End Namespace