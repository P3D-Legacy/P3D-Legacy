Namespace Items.Machines

    <Item(234, "TM 44")>
    Public Class TM44

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 3000, 156)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
