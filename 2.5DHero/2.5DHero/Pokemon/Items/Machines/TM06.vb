Namespace Items.Machines

    <Item(196, "TM 06")>
    Public Class TM06

        Inherits TechMachine

        Public Overrides ReadOnly Property BattlePointsPrice As Integer = 32

        Public Sub New()
            MyBase.New(True, 3000, 92)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
