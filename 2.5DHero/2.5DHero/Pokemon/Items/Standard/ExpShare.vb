Namespace Items.Standard

    Public Class ExpShare

        Inherits Item

        Public Sub New()
            MyBase.New("Exp Share", 3000, ItemTypes.Standard, 57, 1, 0, New Rectangle(216, 48, 24, 24), "An item to be held by a Pokémon. The holder gets a share of a battle's Exp. Points without battling.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace