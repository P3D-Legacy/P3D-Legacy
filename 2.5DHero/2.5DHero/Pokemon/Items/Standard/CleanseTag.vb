Namespace Items.Standard

    Public Class CleanseTag

        Inherits Item

        Public Sub New()
            MyBase.New("CleanseTag", 200, ItemTypes.Standard, 94, 1, 0, New Rectangle(432, 72, 24, 24), "An item to be held by a Pokémon. It helps keep wild Pokémon away if the holder is the head of the party.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace