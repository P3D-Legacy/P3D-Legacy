Namespace Items.Machines

    <Item(247, "HM 01")>
    Public Class HM01

        Inherits TechMachine

        Public Sub New()
            MyBase.New(False, 100, 15)
            _textureRectangle = New Rectangle(48, 192, 24, 24)
        End Sub

    End Class

End Namespace
