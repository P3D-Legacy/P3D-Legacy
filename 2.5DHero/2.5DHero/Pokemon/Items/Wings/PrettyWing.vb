Namespace Items.Wings

    Public Class PrettyWing

        Inherits Item

        Public Sub New()
            MyBase.New("Pretty Wing", 200, ItemTypes.Standard, 260, 1.0F, 0, New Rectangle(432, 240, 24, 24), "Though this feather is beautiful, it's just a regular feather and has no effect on Pokémon.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace