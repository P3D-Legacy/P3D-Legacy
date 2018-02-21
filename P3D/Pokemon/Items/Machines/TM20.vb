Namespace Items.Machines

    <Item(210, "TM 20")>
    Public Class TM20

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 3000, 203)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
