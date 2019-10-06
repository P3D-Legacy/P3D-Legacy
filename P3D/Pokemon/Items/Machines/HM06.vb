Namespace Items.Machines

    <Item(248, "HM 06")>
    Public Class HM06

        Inherits TechMachine

        Public Sub New()
            MyBase.New(False, 100, 250)
            _textureRectangle = New Rectangle(96, 192, 24, 24)
        End Sub

    End Class

End Namespace
