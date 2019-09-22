Namespace Items.Berries

    <Item(2066, "Maranga")>
    Public Class MarangaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(86400, "If held by a Pokémon, this Berry will increase the holder's Sp. Def if it's hit with a special move.", "18.6cm", "Hard", 1, 5)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 40
            Me.Bitter = 0
            Me.Sour = 0

            Me.Type = Element.Types.Dark
            Me.Power = 100
        End Sub

    End Class

End Namespace
