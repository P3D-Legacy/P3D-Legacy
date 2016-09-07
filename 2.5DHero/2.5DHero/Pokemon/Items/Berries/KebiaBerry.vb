Namespace Items.Berries

    Public Class KebiaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2041, "Kebia", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Poison-type attack.", "8.9cm", "Hard", 1, 5)

            Me.Spicy = 0
            Me.Dry = 15
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 10

            Me.Type = Element.Types.Poison
            Me.Power = 60
        End Sub

    End Class

End Namespace