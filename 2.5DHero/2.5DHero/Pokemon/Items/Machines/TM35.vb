Namespace Items.Machines

    <Item(225, "TM 35")>
    Public Class TM35

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 3000, 214)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
