Namespace Items.Berries

    Public Class LansatBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2057, "Lansat", 86400, "A Berry to be consumed by Pokémon. If a Pokémon holds one, its critical-hit ratio will increase when it's in a pinch.", "9.7cm", "Soft", 1, 2)

            Me.Spicy = 30
            Me.Dry = 10
            Me.Sweet = 30
            Me.Bitter = 10
            Me.Sour = 30

            Me.Type = Element.Types.Flying
            Me.Power = 80
        End Sub

    End Class

End Namespace