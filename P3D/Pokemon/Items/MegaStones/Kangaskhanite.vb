Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Kangaskhan.
    ''' </summary>
    <Item(524, "Kangaskhanite")>
    Public Class Kangaskhanite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Kangaskhan", 115)
            _textureRectangle = New Rectangle(168, 24, 24, 24)
        End Sub

    End Class

End Namespace
