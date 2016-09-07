Namespace Items.Berries

    Public Class GanlonBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2053, "Ganlon", 86400, "A Berry to be consumed by Pokémon. If a Pokémon holds one, its Defense stat will increase when it's in a pinch.", "3.3cm", "Very Hard", 1, 2)

            Me.Spicy = 0
            Me.Dry = 30
            Me.Sweet = 10
            Me.Bitter = 30
            Me.Sour = 0

            Me.Type = Element.Types.Ice
            Me.Power = 80
        End Sub

    End Class

End Namespace