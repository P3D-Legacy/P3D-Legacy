Namespace Items.Standard

    Public Class ThickClub

        Inherits Item

        Public Sub New()
            MyBase.New("Thick Club", 500, ItemTypes.Standard, 118, 1, 1, New Rectangle(456, 96, 24, 24), "A rare bone that is extremely valuable for the study of Pokémon archeology. It can be sold for a high price to shops.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 90
        End Sub

    End Class

End Namespace