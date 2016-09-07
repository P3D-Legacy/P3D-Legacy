Namespace Items.Berries

    Public Class NanabBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2017, "Nanab", 3600, "Pokéblock ingredient. Plant in loamy soil to grow Nanab.", "7.7cm", "Very Hard", 2, 3)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 10
            Me.Bitter = 10
            Me.Sour = 0

            Me.Type = Element.Types.Water
            Me.Power = 70
        End Sub

    End Class

End Namespace