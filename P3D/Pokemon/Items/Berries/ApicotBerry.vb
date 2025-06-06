Namespace Items.Berries

    <Item(2056, "Apicot")>
    Public Class ApicotBerry

        Inherits Berry
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Sub New()
            MyBase.New(86400, "A Berry to be consumed by Pokémon. If a Pokémon holds one, its Sp. Def. stat will increase when it's in a pinch.", "7.6cm", "Very Hard", 1, 2)

            Me.Spicy = 10
            Me.Dry = 30
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 30

            Me.Type = Element.Types.Ground
            Me.Power = 100
            Me.JuiceColor = "blue"
            Me.JuiceGroup = 3
        End Sub

    End Class

End Namespace
