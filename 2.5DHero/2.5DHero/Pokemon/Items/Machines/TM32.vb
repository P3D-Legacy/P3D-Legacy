Namespace Items.Machines

    Public Class TM32

        Inherits TechMachine

        Public Sub New()
            MyBase.New(222, True, 2000, 104)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace