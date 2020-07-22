Namespace Items.Berries

    <Item(2065, "Kee")>
    Public Class KeeBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(86400, "If held by a Pokémon, this Berry will increase the holder's Defense if it's hit with a physical move.", "5.0cm", "Very Soft", 1, 5)

            Me.Spicy = 10
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 0
            Me.Sour = 40

            Me.Type = Element.Types.Fairy
            Me.Power = 100
        End Sub

    End Class

End Namespace
