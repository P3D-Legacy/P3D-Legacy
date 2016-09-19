Namespace Items.Standard

    <Item(109, "Rare Bone")>
    Public Class RareBone

        Inherits Item

        Public Sub New()
            MyBase.New("Rare Bone", 30000, ItemTypes.Standard, 109, 1, 0, New Rectangle(456, 96, 24, 24), "A rare bone that is extremely valuable for the study of Pokémon archeology. It can be sold for a high price to shops.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace
