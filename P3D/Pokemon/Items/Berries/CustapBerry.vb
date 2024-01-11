Namespace Items.Berries

    <Item(2061, "Custap")>
    Public Class CustapBerry

        Inherits Berry
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Sub New()
            MyBase.New(86400, "If held by a Pokémon, it gets to move first just once in a pinch.", "26.7cm", "Super Hard", 1, 5)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 40
            Me.Bitter = 10
            Me.Sour = 0

            Me.Type = Element.Types.Ghost
            Me.Power = 100
            Me.JuiceColor = "red"
            Me.JuiceGroup = 3
        End Sub

    End Class

End Namespace
