Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Beedrill.
    ''' </summary>
    <Item(537, "Beedrillite")>
    Public Class Beedrillite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Beedrill", 15)
            _textureRectangle = New Rectangle(48, 72, 24, 24)
        End Sub

    End Class

End Namespace
