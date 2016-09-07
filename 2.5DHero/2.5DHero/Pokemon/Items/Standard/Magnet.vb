Namespace Items.Standard

    Public Class Magnet

        Inherits Item

        Public Sub New()
            MyBase.New("Magnet", 100, ItemTypes.Standard, 108, 1, 0, New Rectangle(240, 96, 24, 24), "An item to be held by a Pokémon. It is a powerful magnet that boosts the power of Electric-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

    End Class

End Namespace