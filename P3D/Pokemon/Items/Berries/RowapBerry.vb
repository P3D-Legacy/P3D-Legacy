Namespace Items.Berries

    <Item(2063, "Rowap")>
    Public Class RowapBerry

        Inherits Berry
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Sub New()
            MyBase.New(86400, "If held by a Pokémon and a special attack lands, the attacker takes damage. ", "13.2cm", "Very Soft", 1, 5)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 40

            Me.Type = Element.Types.Dark
            Me.Power = 100
            Me.JuiceColor = "blue"
            Me.JuiceGroup = 3
        End Sub

    End Class

End Namespace
