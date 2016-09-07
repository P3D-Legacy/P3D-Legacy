Namespace Items.Standard

    Public Class LightBall

        Inherits Item

        Public Sub New()
            MyBase.New("Light Ball", 100, ItemTypes.Standard, 163, 1, 1, New Rectangle(168, 144, 24, 24), "An item to be held by Pikachu. It's a puzzling orb that boosts its Attack and Sp. Atk stats.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace