Namespace Items.Balls

    Public Class FriendBall

        Inherits Item

        Public Sub New()
            MyBase.New("Friend Ball", 150, ItemTypes.Pokéballs, 164, 1.0F, 1, New Rectangle(192, 144, 24, 24), "A Pokéball that makes caught Pokémon more friendly.")

            Me._isBall = True
            Me._canBeUsed = False
        End Sub

    End Class

End Namespace