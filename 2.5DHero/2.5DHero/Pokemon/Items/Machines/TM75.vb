Namespace Items.Machines

    <Item(375, "TM 75")>
    Public Class TM75

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 2000, 117)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
