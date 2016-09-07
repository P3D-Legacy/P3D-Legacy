Namespace Items.Berries

    Public Class TangaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2045, "Tanga", 64800, "If held by a Pokémon, this berry will lessen the damage taken from one supereffective Bug-type attack.", "4.3cm", "Very Soft", 1, 5)

            Me.Spicy = 20
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 10

            Me.Type = Element.Types.Bug
            Me.Power = 60
        End Sub

    End Class

End Namespace