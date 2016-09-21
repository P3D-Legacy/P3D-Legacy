Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Aggron.
    ''' </summary>
    <Item(510, "Aggronite")>
    Public Class Aggronite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Aggron", 306)
            _textureRectangle = New Rectangle(72, 0, 24, 24)
        End Sub

    End Class

End Namespace
