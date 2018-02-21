Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Blastoise.
    ''' </summary>
    <Item(514, "Blastoisinite")>
    Public Class Blastoisinite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Blastoise", 9)
            _textureRectangle = New Rectangle(168, 0, 24, 24)
        End Sub

    End Class

End Namespace
