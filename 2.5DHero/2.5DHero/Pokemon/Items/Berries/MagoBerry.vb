Namespace Items.Berries

    <Item(2012, "Mago")>
    Public Class MagoBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(21600, "If held by a Pokémon, it restores the user's HP in a pinch, but it will cause confusion if the user hates the taste.", "12.6cm", "Hard", 2, 3)

            Me.Spicy = 0
            Me.Dry = 0
            Me.Sweet = 15
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Ghost
            Me.Power = 60
        End Sub

    End Class

End Namespace
