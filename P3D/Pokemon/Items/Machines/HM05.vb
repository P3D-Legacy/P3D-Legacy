Namespace Items.Machines

    <Item(243, "HM 05")>
    Public Class HM05

        Inherits TechMachine

        Public Sub New()
            MyBase.New(False, 100, 148)
            _textureRectangle = New Rectangle(432, 312, 24, 24)
        End Sub

    End Class

End Namespace
