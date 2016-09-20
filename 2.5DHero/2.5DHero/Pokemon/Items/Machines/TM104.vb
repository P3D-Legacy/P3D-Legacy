Namespace Items.Machines

    <Item(404, "TM 104")>
    Public Class TM104

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 3000, 263)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
