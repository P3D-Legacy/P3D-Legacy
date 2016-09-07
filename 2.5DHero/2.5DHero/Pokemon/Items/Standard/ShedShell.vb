Namespace Items.Standard

    Public Class ShedShell

        Inherits Item

        Public Sub New()
            MyBase.New("Shed Shell", 100, ItemTypes.Standard, 154, 1, 1, New Rectangle(72, 216, 24, 24), "A tough, discarded carapace to be held by a Pokémon. It enables the holder to switch with a waiting Pokémon in battle.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace