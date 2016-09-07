Namespace Items.Standard

    Public Class ReaperCloth

        Inherits Item

        Public Sub New()
            MyBase.New("Reaper Cloth", 2100, ItemTypes.Standard, 84, 1, 1, New Rectangle(96, 168, 24, 24), "A cloth imbued with horrifyingly strong spiritual energy. It's loved by a certain Pokémon.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._flingDamage = 10
        End Sub

    End Class

End Namespace