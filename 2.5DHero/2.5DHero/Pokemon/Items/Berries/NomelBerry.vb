Namespace Items.Berries

    Public Class NomelBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2029, "Nomel", 21600, "Pokéblock ingredient. Plant in loamy soil to grow Nomel.", "28.5cm", "Soft", 2, 4)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 20

            Me.Type = Element.Types.Dragon
            Me.Power = 70
        End Sub

    End Class

End Namespace