Namespace Items.Berries

    <Item(2037, "Wacan")>
    Public Class WacanBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(64800, "If held by a Pokémon, this berry will lessen the damage taken from one supereffective Electric-type attack.", "25.0cm", "Very Soft", 1, 5)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 15
            Me.Bitter = 0
            Me.Sour = 10

            Me.Type = Element.Types.Electric
            Me.Power = 60
        End Sub

    End Class

End Namespace
