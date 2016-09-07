Namespace Items.Standard

    Public Class ToxicOrb

        Inherits Item

        Public Sub New()
            MyBase.New("Toxic Orb", 200, ItemTypes.Standard, 505, 1, 1, New Rectangle(216, 240, 24, 24), "An item to be held by a Pokémon. It is a bizarre orb that will badly poison the holder during battle.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._battlePointsPrice = 16
        End Sub

    End Class

End Namespace