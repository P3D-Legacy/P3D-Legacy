Namespace Items.Berries

    <Item(2053, "Ganlon")>
    Public Class GanlonBerry

        Inherits Berry
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Sub New()
            MyBase.New(86400, "A Berry to be consumed by Pokémon. If a Pokémon holds one, its Defense stat will increase when it's in a pinch.", "3.3cm", "Very Hard", 1, 2)

            Me.Spicy = 0
            Me.Dry = 30
            Me.Sweet = 10
            Me.Bitter = 30
            Me.Sour = 0

            Me.Type = Element.Types.Ice
            Me.Power = 100
            Me.JuiceColor = "purple"
            Me.JuiceGroup = 3
        End Sub

    End Class

End Namespace
