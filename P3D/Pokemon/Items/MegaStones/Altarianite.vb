Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Altaria.
    ''' </summary>
    <Item(535, "Altarianite")>
    Public Class Altarianite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Altaria", 334)
            _textureRectangle = New Rectangle(0, 72, 24, 24)
        End Sub

    End Class

End Namespace
