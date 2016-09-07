Namespace Items.Standard

    Public Class BlackSludge

        Inherits Item

        Public Sub New()
            MyBase.New("Black Sludge", 200, ItemTypes.Standard, 182, 1, 0, New Rectangle(432, 144, 24, 24), "A held item that gradually restores the HP of Poison-type Pokémon. It inflicts damage on all other types.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace