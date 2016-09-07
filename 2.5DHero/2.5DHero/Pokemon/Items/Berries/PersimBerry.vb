Namespace Items.Berries

    Public Class PersimBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2007, "Persim", 14400, "A Berry to be consumed by Pokémon. If a Pokémon holds one, it can recover from confusion on its own in battle.", "4.7cm", "Hard", 2, 3)

            Me.Spicy = 10
            Me.Dry = 10
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 10

            Me.Type = Element.Types.Ground
            Me.Power = 60
        End Sub

    End Class

End Namespace