Namespace Items.Machines

    <Item(196, "TM 06")>
    Public Class TM06

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 3000, 92)
            CanTeachAlways = True

            Me._battlePointsPrice = 32
        End Sub

    End Class

End Namespace
