Namespace Items.Machines

    <Item(251, "HM 09")>
    Public Class HM09

        Inherits TechMachine

        Public Sub New()
            MyBase.New(False, 100, 560)
            _textureRectangle = New Rectangle(120, 192, 24, 24)
        End Sub

    End Class

End Namespace
