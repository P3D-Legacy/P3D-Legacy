Namespace Items.Berries

    <Item(2016, "Bluk")>
    Public Class BlukBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(3600, "Pokéblock ingredient. Plant in loamy soil to grow Bluk.", "10.8cm", "Soft", 3, 6)

            Me.Spicy = 0
            Me.Dry = 10
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Fire
            Me.Power = 90
        End Sub

    End Class

End Namespace
