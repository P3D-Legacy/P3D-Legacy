Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Swampert.
    ''' </summary>
    <Item(553, "Swampertite")>
    Public Class Swampertite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Swampert", 260)
            _textureRectangle = New Rectangle(192, 96, 24, 24)
        End Sub

    End Class

End Namespace
