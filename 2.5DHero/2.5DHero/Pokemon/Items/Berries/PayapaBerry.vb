Namespace Items.Berries

    Public Class PayapaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2044, "Payapa", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Psychic-type attack.", "25.1cm", "Soft", 1, 5)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 15

            Me.Type = Element.Types.Psychic
            Me.Power = 60
        End Sub

    End Class

End Namespace