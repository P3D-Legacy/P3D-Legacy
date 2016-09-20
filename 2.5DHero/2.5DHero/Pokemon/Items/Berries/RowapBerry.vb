Namespace Items.Berries

    <Item(2063, "Rowap")>
    Public Class RowapBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(86400, "If held by a Pokémon and a special attack lands, the attacker takes damage. ", "13.2cm", "Very Soft", 1, 5)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 40

            Me.Type = Element.Types.Dark
            Me.Power = 80
        End Sub

    End Class

End Namespace
