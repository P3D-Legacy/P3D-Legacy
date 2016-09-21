Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Heracross.
    ''' </summary>
    <Item(522, "Heracronite")>
    Public Class Heracronite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Heracross", 214)
            _textureRectangle = New Rectangle(120, 24, 24, 24)
        End Sub

    End Class

End Namespace
