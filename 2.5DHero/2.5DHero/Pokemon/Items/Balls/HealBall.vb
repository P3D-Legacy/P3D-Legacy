Namespace Items.Balls

    Public Class HealBall

        Inherits Item

        Public Sub New()
            MyBase.New("Heal Ball", 300, ItemTypes.Pokéballs, 186, 1, 0, New Rectangle(456, 216, 24, 24), "A remedial Pokéball that restores the HP of a Pokémon caught with it and eliminiates any status conditions. ")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace