Namespace Items.Berries

    Public Class DurinBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2033, "Durin", 64800, "Pokéblock ingredient. Plant in loamy soil to grow Durin.", "28.0m", "Hard", 1, 2)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 30
            Me.Sour = 10

            Me.Type = Element.Types.Water
            Me.Power = 80
        End Sub

    End Class

End Namespace