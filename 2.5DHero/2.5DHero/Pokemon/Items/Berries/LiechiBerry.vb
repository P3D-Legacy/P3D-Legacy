Namespace Items.Berries

    Public Class LiechiBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2052, "Liechi", 86400, "A Berry to be consumed by Pokémon. If a Pokémon holds one, its Attack stat will increase when it's in a pinch.", "11.1cm", "Very Hard", 1, 2)

            Me.Spicy = 30
            Me.Dry = 10
            Me.Sweet = 30
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Grass
            Me.Power = 80
        End Sub

    End Class

End Namespace