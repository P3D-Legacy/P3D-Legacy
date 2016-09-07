Namespace Items.Standard

    Public Class CometShard

        Inherits Item

        Public Sub New()
            MyBase.New("Comet Shard", 120000, ItemTypes.Standard, 149, 1, 0, New Rectangle(24, 216, 24, 24), "A shard which fell to the ground when a comet approached. A maniac will buy it for a high price.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False
        End Sub

    End Class

End Namespace