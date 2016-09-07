Namespace Items.Berries

    Public Class RazzBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2015, "Razz", 3600, "Pokéblock ingredient. Plant in loamy soil to grow Razz.", "12.0cm", "Very Hard", 2, 3)

            Me.Spicy = 10
            Me.Dry = 10
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Steel
            Me.Power = 60
        End Sub

    End Class

End Namespace