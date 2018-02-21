Namespace Items.Machines

    <Item(193, "TM 03")>
    Public Class TM03

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 3000, 174)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
