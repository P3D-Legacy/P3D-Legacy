Namespace Items.Standard

    Public Class WaveIncense

        Inherits Item

        Public Sub New()
            MyBase.New("Wave Incense", 9800, ItemTypes.Standard, 145, 1, 0, New Rectangle(480, 192, 24, 24), "An item to be held by a Pokémon. This exotic-smelling incense boots the power of Water-type moves.")

            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
            Me._canBeTraded = True
            Me._canBeHold = True

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace