Namespace Items.Machines

    <Item(431, "TM 131")>
    Public Class TM131

        Inherits TechMachine

        Public Sub New()
            MyBase.New(431, True, 1500, 445, 131)
            CanTeachWhenGender = True
        End Sub

    End Class

End Namespace
