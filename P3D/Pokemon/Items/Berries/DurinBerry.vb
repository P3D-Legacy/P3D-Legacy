Namespace Items.Berries

    <Item(2033, "Durin")>
    Public Class DurinBerry

        Inherits Berry

        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Sub New()
            MyBase.New(64800, "Pokéblock ingredient. Plant in loamy soil to grow Durin.", "28.0m", "Hard", 1, 2)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 30
            Me.Sour = 10

            Me.Type = Element.Types.Water
            Me.Power = 100
            Me.JuiceColor = "grass"
            Me.JuiceGroup = 1
        End Sub

    End Class

End Namespace
