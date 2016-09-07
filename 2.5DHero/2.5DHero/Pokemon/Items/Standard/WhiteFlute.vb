Namespace Items.Standard

    Public Class WhiteFlute

        Inherits Item

        Public Sub New()
            MyBase.New("White Flute", 500, ItemTypes.Standard, 147, 1, 0, New Rectangle(456, 192, 24, 24), "A white flute made from blown glass. Its melody makes wild Pokémon more likely to appear.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace