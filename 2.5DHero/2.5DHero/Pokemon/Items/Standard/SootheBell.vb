Namespace Items.Standard

    Public Class SootheBell

        Inherits Item

        Public Sub New()
            'Tea Gardner style
            MyBase.New("Soothe Bell", 100, ItemTypes.Standard, 148, 1, 0, New Rectangle(0, 216, 24, 24), "An item to be held by a Pokémon. The comforting chime of this bell calms the holder, making it friendly.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace