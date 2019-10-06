Namespace Items.Machines

    <Item(249, "HM 07")>
    Public Class HM07

        Inherits TechMachine

        Public Sub New()
            MyBase.New(False, 100, 127)
            _textureRectangle = New Rectangle(96, 192, 24, 24)
        End Sub

    End Class

End Namespace
