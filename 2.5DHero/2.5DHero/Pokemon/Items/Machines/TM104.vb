Namespace Items.Machines

    Public Class TM104

        Inherits TechMachine

        Public Sub New()
            MyBase.New(404, True, 3000, 263, 104)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace