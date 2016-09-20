Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Abomasnow.
    ''' </summary>
    <Item(507, "Abomasite")>
    Public Class Abomasite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Abomasnow", 460)
            _textureRectangle = New Rectangle(0, 0, 24, 24)
        End Sub

    End Class

End Namespace
