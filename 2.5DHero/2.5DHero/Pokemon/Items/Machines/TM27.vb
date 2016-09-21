Namespace Items.Machines

    <Item(217, "TM 27")>
    Public Class TM27

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 1000, 216)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
