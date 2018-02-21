Namespace Items.Machines

    <Item(224, "TM 34")>
    Public Class TM34

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 1000, 207)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
