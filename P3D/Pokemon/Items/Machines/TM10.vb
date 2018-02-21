Namespace Items.Machines

    <Item(200, "TM 10")>
    Public Class TM10

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 3000, 237)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
