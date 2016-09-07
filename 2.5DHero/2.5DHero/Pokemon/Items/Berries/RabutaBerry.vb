Namespace Items.Berries

    Public Class RabutaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2028, "Rabuta", 21600, "Pokéblock ingredient. Plant in loamy soil to grow Rabuta.", "22.6cm", "Soft", 2, 4)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 20
            Me.Sour = 10

            Me.Type = Element.Types.Ghost
            Me.Power = 60
        End Sub

    End Class

End Namespace