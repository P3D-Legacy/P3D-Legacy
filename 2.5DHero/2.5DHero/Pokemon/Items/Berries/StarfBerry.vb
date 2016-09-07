Namespace Items.Berries

    Public Class StarfBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2058, "Starf", 86400, "A Berry to be consumed by Pokémon. If a Pokémon holds one, one of its stats will sharply increase when it's in a pinch.", "15.2cm", "Super Hard", 1, 2)

            Me.Spicy = 30
            Me.Dry = 10
            Me.Sweet = 30
            Me.Bitter = 10
            Me.Sour = 30

            Me.Type = Element.Types.Psychic
            Me.Power = 80
        End Sub

    End Class

End Namespace