Namespace Items.Berries

    Public Class RindoBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2038, "Rindo", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Grass-type attack.", "15.5cm", "Soft", 1, 5)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 15
            Me.Sour = 0

            Me.Type = Element.Types.Grass
            Me.Power = 60
        End Sub

    End Class

End Namespace