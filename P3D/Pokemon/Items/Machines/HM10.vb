Namespace Items.Machines

    <Item(252, "HM 10")>
    Public Class HM10

        Inherits TechMachine

        Public Sub New()
            MyBase.New(False, 100, 291)
            _textureRectangle = New Rectangle(96, 192, 24, 24)
        End Sub

    End Class

End Namespace
