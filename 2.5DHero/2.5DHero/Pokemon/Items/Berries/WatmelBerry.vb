Namespace Items.Berries

Public Class WatmelBerry

    Inherits Berry

    Public Sub New()
            MyBase.New(2032, "Watmel", 64800, "Pokéblock ingredient. Plant in loamy soil to grow Watmel.", "25.0cm", "Soft", 1, 2)

        Me.Spicy = 0
        Me.Dry = 0
        Me.Sweet = 30
        Me.Bitter = 10
        Me.Sour = 0

        Me.Type = Element.Types.Fire
        Me.Power = 80
    End Sub

End Class

End Namespace