Namespace Items.Berries

    Public Class KasibBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2047, "Kasib", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Ghost-type attack.", "14.4cm", "Hard", 1, 5)

            Me.Spicy = 0
            Me.Dry = 10
            Me.Sweet = 20
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Ghost
            Me.Power = 60
        End Sub

    End Class

End Namespace