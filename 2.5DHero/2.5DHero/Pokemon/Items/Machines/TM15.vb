Namespace Items.Machines

    <Item(205, "TM 15")>
    Public Class TM15

        Inherits TechMachine

        Public Sub New()
            MyBase.New(205, True, 3000, 63)
            CanTeachWhenFullyEvolved = True
        End Sub

    End Class

End Namespace
