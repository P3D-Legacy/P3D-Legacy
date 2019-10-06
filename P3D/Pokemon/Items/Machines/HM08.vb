Namespace Items.Machines

    <Item(250, "HM 08")>
    Public Class HM08

        Inherits TechMachine

        Public Sub New()
            MyBase.New(False, 100, 431)
            _textureRectangle = New Rectangle(48, 192, 24, 24)
        End Sub

    End Class

End Namespace
