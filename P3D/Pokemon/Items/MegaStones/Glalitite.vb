Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Glalie.
    ''' </summary>
    <Item(541, "Glalitite")>
    Public Class Glalitite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Glalie", 362)
            _textureRectangle = New Rectangle(144, 72, 24, 24)
        End Sub

    End Class

End Namespace
