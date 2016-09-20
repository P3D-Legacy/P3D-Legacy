Namespace Items.Machines

    <Item(367, "TM 67")>
    Public Class TM67

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 2000, 99)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
