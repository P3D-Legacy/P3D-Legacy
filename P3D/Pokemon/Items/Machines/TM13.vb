Namespace Items.Machines

    <Item(203, "TM 13")>
    Public Class TM13

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 1000, 173)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
