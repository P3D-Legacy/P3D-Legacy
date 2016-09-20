Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Sableye.
    ''' </summary>
    <Item(547, "Sablenite")>
    Public Class Sablenite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Sableye", 302)
            _textureRectangle = New Rectangle(48, 96, 24, 24)
        End Sub

    End Class

End Namespace
