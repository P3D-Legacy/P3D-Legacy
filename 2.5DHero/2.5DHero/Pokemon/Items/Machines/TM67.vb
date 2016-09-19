Namespace Items.Machines

    <Item(367, "TM 67")>
    Public Class TM67

        Inherits TechMachine

        Public Sub New()
            MyBase.New(367, True, 2000, 99, 67)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
