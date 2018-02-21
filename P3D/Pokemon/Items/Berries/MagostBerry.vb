Namespace Items.Berries

    <Item(2027, "Magost")>
    Public Class MagostBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(21600, "Pokéblock ingredient. Plant in loamy soil to grow Magost.", "14.0cm", "Hard", 2, 4)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 20
            Me.Bitter = 10
            Me.Sour = 0

            Me.Type = Element.Types.Rock
            Me.Power = 70
        End Sub

    End Class

End Namespace
