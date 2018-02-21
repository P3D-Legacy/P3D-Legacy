Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Tyranitar.
    ''' </summary>
    <Item(533, "Tyranitarite")>
    Public Class Tyranitarite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Tyranitar", 248)
            _textureRectangle = New Rectangle(144, 48, 24, 24)
        End Sub

    End Class

End Namespace
