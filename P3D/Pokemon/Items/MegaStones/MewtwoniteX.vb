Namespace Items.MegaStones

    ''' <summary>
    ''' A Mega Stone for Mewtwo.
    ''' </summary>
    <Item(529, "Mewtwonite X")>
    Public Class MewtwoniteX

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Mewtwo", 150)
            _textureRectangle = New Rectangle(48, 48, 24, 24)
        End Sub

    End Class

End Namespace
