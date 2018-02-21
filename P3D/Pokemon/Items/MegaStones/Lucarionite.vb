Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Lucario.
    ''' </summary>
    <Item(525, "Lucarionite")>
    Public Class Lucarionite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Lucario", 448)
            _textureRectangle = New Rectangle(192, 24, 24, 24)
        End Sub

    End Class

End Namespace
