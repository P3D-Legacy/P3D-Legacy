Namespace Items.Berries

    <Item(2051, "Chilian")>
    Public Class ChilanBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(64800, "If held by a Pokémon, this Berry will lessen the damage taken from one Normal-type attack.", "3.3cm", "Very Soft", 1, 5)

            Me.Spicy = 0
            Me.Dry = 25
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Normal
            Me.Power = 60
        End Sub

    End Class

End Namespace
