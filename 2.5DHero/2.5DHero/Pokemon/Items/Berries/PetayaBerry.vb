Namespace Items.Berries

    <Item(2055, "Petaya")>
    Public Class PetayaBerry

        Inherits Berry

        Public Sub New()
            MyBase.New(2055, "Petaya", 86400, "A Berry to be consumed by Pok�mon. If a Pok�mon holds one, its Sp. Atk stat will increase when it's in a pinch.", "23.7cm", "Very Hard", 1, 2)

            Me.Spicy = 30
            Me.Dry = 0
            Me.Sweet = 0
            Me.Bitter = 30
            Me.Sour = 10

            Me.Type = Element.Types.Poison
            Me.Power = 80
        End Sub

    End Class

End Namespace
