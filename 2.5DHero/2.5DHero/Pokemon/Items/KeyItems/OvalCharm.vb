Namespace Items.KeyItems

    Public Class OvalCharm

        Inherits Item

        Public Sub New()
            MyBase.New("Oval Charm", 9800, ItemTypes.KeyItems, 241, 1, 0, New Rectangle(96, 264, 24, 24), "An oval charm said to increase the chance of Pokémon Eggs being found at the Day Care.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = False
            Me._canBeHold = False
        End Sub

    End Class

End Namespace