Namespace Items.Machines

    <Item(207, "TM 17")>
    Public Class TM17

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 3000, 182)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
