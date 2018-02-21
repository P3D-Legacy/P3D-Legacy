Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Gengar.
    ''' </summary>
    <Item(520, "Gengarite")>
    Public Class Gengarite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Gengar", 94)
            _textureRectangle = New Rectangle(72, 24, 24, 24)
        End Sub

    End Class

End Namespace
