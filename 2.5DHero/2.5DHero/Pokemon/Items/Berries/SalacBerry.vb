Namespace Items.Berries

    Public Class SalacBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2054, "Salac", 86400, "A Berry to be held by Pokémon. If a Pokémon holds one, its Speed stat will increase when it's in a pinch. ", "9.4cm", "Very Hard", 1, 2)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 30
            Me.Bitter = 10
            Me.Sour = 30

            Me.Type = Element.Types.Fighting
            Me.Power = 80
        End Sub

    End Class

End Namespace