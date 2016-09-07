Namespace Items.Standard

    Public Class DubiousDisc

        Inherits Item

        Public Sub New()
            MyBase.New("Dubious Disc", 2100, ItemTypes.Standard, 185, 1, 0, New Rectangle(0, 168, 24, 24), "A transparent device overflowing with dubious data. Its producer is unknown.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 50
        End Sub

    End Class

End Namespace