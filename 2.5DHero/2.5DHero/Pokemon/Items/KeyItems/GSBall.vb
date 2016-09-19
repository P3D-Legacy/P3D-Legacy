Namespace Items.KeyItems

    <Item(115, "GS Ball")>
    Public Class GSBall

        Inherits Item

        Public Sub New()
            MyBase.New("GS Ball", 100, ItemTypes.KeyItems, 115, 1, 1, New Rectangle(384, 96, 24, 24), "A mysterious Pokéball. Its purpose is unknown.")

            Me._canBeHold = False
            Me._canBeTraded = False
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace
