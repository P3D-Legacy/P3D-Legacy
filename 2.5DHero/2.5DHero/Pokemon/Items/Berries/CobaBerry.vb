Namespace Items.Berries

    Public Class CobaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2043, "Coba", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Flying-type attack.", "27.7cm", "Very Hard", 1, 5)

            Me.Spicy = 0
            Me.Dry = 10
            Me.Sweet = 0
            Me.Bitter = 15
            Me.Sour = 0

            Me.Type = Element.Types.Flying
            Me.Power = 60
        End Sub

    End Class

End Namespace