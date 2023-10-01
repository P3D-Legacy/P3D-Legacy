Namespace Items.Berries

    <Item(2028, "Rabuta")>
    Public Class RabutaBerry

        Inherits Berry

        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Sub New()
            MyBase.New(21600, "Pokéblock ingredient. Plant in loamy soil to grow Rabuta.", "22.6cm", "Soft", 2, 4)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 20
            Me.Sour = 10

            Me.Type = Element.Types.Ghost
            Me.Power = 90
            Me.JuiceColor = "green"
            Me.JuiceGroup = 1
        End Sub

    End Class

End Namespace
