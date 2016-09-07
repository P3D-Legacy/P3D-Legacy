Namespace Items.Berries

    Public Class HabanBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2048, "Haban", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Dragon-type attack.", "2.3cm", "Soft", 1, 5)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 10
            Me.Bitter = 20
            Me.Sour = 0

            Me.Type = Element.Types.Dragon
            Me.Power = 60
        End Sub

    End Class

End Namespace