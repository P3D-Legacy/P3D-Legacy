Namespace Items.Berries

    <Item(2031, "Pamtre")>
    Public Class PamtreBerry

        Inherits Berry

        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Sub New()
            MyBase.New(64800, "Pokéblock ingredient. Plant in loamy soil to grow Pamtre.", "24.4cm", "Very Soft", 1, 2)

            Me.Spicy = 0
            Me.Dry = 30
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Steel
            Me.Power = 90
            Me.JuiceColor = "purple"
            Me.JuiceGroup = 1
        End Sub

    End Class

End Namespace
