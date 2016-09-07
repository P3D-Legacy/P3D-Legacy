Namespace Items.Standard

    Public Class Protector

        Inherits Item

        Public Sub New()
            MyBase.New("Protector", 2100, ItemTypes.Standard, 141, 1, 0, New Rectangle(408, 192, 24, 24), "A protective item of some sort. It is extremely stiff and heavy. It is loved by a certain Pokémon.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 80
        End Sub

    End Class

End Namespace