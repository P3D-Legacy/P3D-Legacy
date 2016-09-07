Namespace Items.Machines

    Public Class TM06

        Inherits TechMachine

        Public Sub New()
            MyBase.New(196, True, 3000, 92)
            CanTeachAlways = True

            Me._battlePointsPrice = 32
        End Sub

    End Class

End Namespace