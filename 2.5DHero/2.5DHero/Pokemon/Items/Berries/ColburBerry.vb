Namespace Items.Berries

    Public Class ColburBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2049, "Colbur", 64800, "If held by a Pokémon, this Berry will lessen the damage taken from one supereffective Dark-type attack.", "3.8cm", "Super Hard", 1, 5)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 10
            Me.Sour = 20

            Me.Type = Element.Types.Dark
            Me.Power = 60
        End Sub

    End Class

End Namespace