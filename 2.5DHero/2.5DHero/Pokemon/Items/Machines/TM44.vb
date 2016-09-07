Namespace Items.Machines

    Public Class TM44

        Inherits TechMachine

        Public Sub New()
            MyBase.New(234, True, 3000, 156)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace