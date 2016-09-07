Namespace Items.Berries

    Public Class ChopleBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2040, "Chople", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Fighting-type attack.", "7.7cm", "Soft", 1, 5)

            Me.Spicy = 15
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 10
            Me.Sour = 0

            Me.Type = Element.Types.Fighting
            Me.Power = 60
        End Sub

    End Class

End Namespace