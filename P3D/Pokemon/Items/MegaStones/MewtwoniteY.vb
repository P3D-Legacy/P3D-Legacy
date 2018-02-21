Namespace Items.MegaStones

    ''' <summary>
    ''' A Mega Stone for Mewtwo.
    ''' </summary>
    <Item(530, "Mewtwonite Y")>
    Public Class MewtwoniteY

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Mewtwo", 150)
            _textureRectangle = New Rectangle(72, 48, 24, 24)
        End Sub

    End Class

End Namespace
