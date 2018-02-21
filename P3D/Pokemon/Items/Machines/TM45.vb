Namespace Items.Machines

    <Item(235, "TM 45")>
    Public Class TM45

        Inherits TechMachine

        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 32

        Public Sub New()
            MyBase.New(True, 1500, 213)
            CanTeachWhenGender = True
        End Sub

    End Class

End Namespace
