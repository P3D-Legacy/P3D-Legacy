Namespace Items.Berries

    Public Class OccaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2035, "Occa", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Fire-type attack.", "8.9cm", "Super Hard", 1, 5)

            Me.Spicy = 15
            Me.Dry = 0
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Fire
            Me.Power = 60
        End Sub

    End Class

End Namespace