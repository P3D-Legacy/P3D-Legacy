Namespace Items.Berries

    Public Class BelueBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2034, "Belue", 64800, "Pokéblock ingredient. Plant in loamy soil to grow Belue.", "11.8cm", "Very Soft", 1, 2)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 30

            Me.Type = Element.Types.Electric
            Me.Power = 80
        End Sub

    End Class

End Namespace