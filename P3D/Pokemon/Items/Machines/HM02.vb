Namespace Items.Machines

    <Item(244, "HM 02")>
    Public Class HM02

        Inherits TechMachine

        Public Sub New()
            MyBase.New(False, 100, 19)
            _textureRectangle = New Rectangle(73, 192, 24, 24)
        End Sub

    End Class

End Namespace
