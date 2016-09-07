Namespace Items.Machines

    Public Class TM21

        Inherits TechMachine

        Public Sub New()
            MyBase.New(211, True, 1000, 218)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace