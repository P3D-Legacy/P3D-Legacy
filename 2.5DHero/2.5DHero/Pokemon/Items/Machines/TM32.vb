Namespace Items.Machines

    <Item(222, "TM 32")>
    Public Class TM32

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 2000, 104)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
