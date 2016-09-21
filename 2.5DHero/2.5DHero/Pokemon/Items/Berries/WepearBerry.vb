Namespace Items.Berries

    <Item(2018, "Wepear")>
    Public Class WepearBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(3600, "Pokéblock ingredient. Plant in loamy soil to grow Wepear.", "7.4cm", "Super Hard", 3, 6)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 10
            Me.Sour = 10

            Me.Type = Element.Types.Electric
            Me.Power = 70
        End Sub

    End Class

End Namespace
