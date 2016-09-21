Namespace Items.Machines

    <Item(434, "TM 134")>
    Public Class TM134

        Inherits TechMachine

        Public Sub New()
            MyBase.New(True, 2000, 363)
            CanTeachAlways = True
        End Sub

    End Class

End Namespace
