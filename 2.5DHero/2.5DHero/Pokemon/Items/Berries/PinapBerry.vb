Namespace Items.Berries

    Public Class PinapBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2019, "Pinap", 3600, "Pokéblock ingredient. Plant in loamy soil to grow Pinap.", "8.0cm", "Hard", 3, 6)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 10

            Me.Type = Element.Types.Grass
            Me.Power = 70
        End Sub

    End Class

End Namespace