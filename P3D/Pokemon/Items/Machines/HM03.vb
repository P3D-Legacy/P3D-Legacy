Namespace Items.Machines

    <Item(245, "HM 03")>
    Public Class HM03

        Inherits TechMachine

        Public Sub New()
            MyBase.New(False, 100, 57)
            _textureRectangle = New Rectangle(96, 192, 24, 24)
        End Sub

    End Class

End Namespace
