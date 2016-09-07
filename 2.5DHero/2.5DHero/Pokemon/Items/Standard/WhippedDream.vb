Namespace Items.Standard

    Public Class WhippedDream

        Inherits Item

        Public Sub New()
            MyBase.New("Whipped Dream", 2100, ItemTypes.Standard, 504, 1, 1, New Rectangle(168, 240, 24, 24), "A soft and sweet treat made of fluffy, puffy, whipped and whirled cream. It's loved by a certain Pokémon.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace