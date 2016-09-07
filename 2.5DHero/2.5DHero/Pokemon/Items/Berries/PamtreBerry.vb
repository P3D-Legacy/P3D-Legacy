Namespace Items.Berries

    Public Class PamtreBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2031, "Pamtre", 64800, "Pokéblock ingredient. Plant in loamy soil to grow Pamtre.", "24.4cm", "Very Soft", 1, 2)

            Me.Spicy = 0
            Me.Dry = 30
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Steel
            Me.Power = 70
        End Sub

    End Class

End Namespace