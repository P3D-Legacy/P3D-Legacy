Namespace Items.Berries

    Public Class CornnBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2026, "Cornn", 21600, "Pokéblock ingredient. Plant in loamy soil to grow Cornn.", "7.5cm", "Hard", 2, 4)

            Me.Spicy = 0
            Me.Dry = 20
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Bug
            Me.Power = 70
        End Sub

    End Class

End Namespace