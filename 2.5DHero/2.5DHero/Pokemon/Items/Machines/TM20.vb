Namespace Items.Machines

    Public Class TM20

        Inherits TechMachine

        Public Sub New()
            MyBase.New(210, True, 3000, 203)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace