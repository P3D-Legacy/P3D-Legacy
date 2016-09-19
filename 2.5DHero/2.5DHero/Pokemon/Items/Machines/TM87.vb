Namespace Items.Machines

    <Item(387, "TM 87")>
    Public Class TM87

        Inherits TechMachine

        Public Sub New()
            MyBase.New(387, True, 2000, 164, 87)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
