Namespace Items.Standard

    Public Class Sachet

        Inherits Item

        Public Sub New()
            MyBase.New("Sachet", 2100, ItemTypes.Standard, 503, 1, 1, New Rectangle(144, 240, 24, 24), "A sachet filled with fragrant perfumes that are just slightly too overwhelming. Yet it's loved by a certain Pokémon.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace