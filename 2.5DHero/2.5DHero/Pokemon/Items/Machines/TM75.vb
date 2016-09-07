Namespace Items.Machines

    Public Class TM75

        Inherits TechMachine

        Public Sub New()
            MyBase.New(375, True, 2000, 117, 75)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace