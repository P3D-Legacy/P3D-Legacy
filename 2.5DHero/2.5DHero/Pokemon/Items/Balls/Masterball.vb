Namespace Items.Balls

    <Item(1, "Masterball")>
    Public Class Masterball

        Inherits Item

        Public Sub New()
            MyBase.New("Masterball", 0, ItemTypes.Pokéballs, 1, 255, 3, New Rectangle(0, 0, 24, 24), "The best Pokéball with the ultimate level of performance. With it, you will catch any wild Pokémon without fail.")

            Me._isBall = True
            Me._canBeUsed = False
            Me._canBeTraded = False
        End Sub

    End Class

End Namespace
