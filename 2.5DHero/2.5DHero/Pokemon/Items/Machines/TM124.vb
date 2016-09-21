Namespace Items.Machines

    <Item(424, "TM 124")>
    Public Class TM124

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 3000, 416)
            CanTeachWhenFullyEvolved = True
        End Sub

    End Class

End Namespace
