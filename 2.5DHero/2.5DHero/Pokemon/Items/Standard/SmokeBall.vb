Namespace Items.Standard

    Public Class SmokeBall

        Inherits Item

        Public Sub New()
            MyBase.New("Smoke Ball", 200, ItemTypes.Standard, 106, 1, 0, New Rectangle(192, 96, 24, 24), "An item to be held by a Pokémon. It enables the holder to flee from any wild Pokémon encounter without fail.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace