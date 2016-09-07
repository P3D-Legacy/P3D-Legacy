Namespace Items.Balls

    Public Class SafariBall

        Inherits Item

        Public Sub New()
            MyBase.New("Safari Ball", 200, ItemTypes.Pokéballs, 181, 1.5F, 0, New Rectangle(72, 144, 24, 24), "A special Pokéball that is used only in the Great Marsh and the Safari Zone. It is decorated in a camouflage pattern.")

            Me._isBall = True
            Me._canBeUsed = False
            Me._canBeTraded = False
        End Sub

    End Class

End Namespace