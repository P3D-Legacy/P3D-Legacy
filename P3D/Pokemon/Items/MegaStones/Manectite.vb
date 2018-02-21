Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Manectric.
    ''' </summary>
    <Item(526, "Manectite")>
    Public Class Manectite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Manectric", 310)
            _textureRectangle = New Rectangle(216, 24, 24, 24)
        End Sub

    End Class

End Namespace
