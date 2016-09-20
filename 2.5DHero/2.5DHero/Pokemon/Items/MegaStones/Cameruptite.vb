Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Camerupt.
    ''' </summary>
    <Item(538, "Cameruptite")>
    Public Class Cameruptite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Camerupt", 323)
            _textureRectangle = New Rectangle(72, 72, 24, 24)
        End Sub

    End Class

End Namespace
