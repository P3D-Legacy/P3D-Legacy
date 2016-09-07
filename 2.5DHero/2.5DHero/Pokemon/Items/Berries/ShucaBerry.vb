Namespace Items.Berries

    Public Class ShucaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2042, "Shuca", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Ground-type attack.", "4.2cm", "Soft", 1, 5)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 15
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Ground
            Me.Power = 60
        End Sub

    End Class

End Namespace