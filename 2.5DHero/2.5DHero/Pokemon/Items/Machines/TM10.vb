Namespace Items.Machines

    Public Class TM10

        Inherits TechMachine

        Public Sub New()
            MyBase.New(200, True, 3000, 237)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace