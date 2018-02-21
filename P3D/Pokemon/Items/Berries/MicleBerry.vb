Namespace Items.Berries

    <Item(2060, "Micle")>
    Public Class MicleBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(86400, "If held by a Pokémon, it raises the accuracy of a move just once in a pinch.", "4.1cm", "Soft", 1, 5)

            Me.Spicy = 0
            Me.Dry = 40
            Me.Sweet = 10
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Rock
            Me.Power = 80
        End Sub

    End Class

End Namespace
