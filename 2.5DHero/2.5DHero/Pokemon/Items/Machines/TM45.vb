Namespace Items.Machines

    Public Class TM45

        Inherits TechMachine

        Public Sub New()
            MyBase.New(235, True, 1500, 213)
            CanTeachWhenGender = True

            Me._battlePointsPrice = 32
        End Sub

    End Class

End Namespace