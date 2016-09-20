Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Diancie.
    ''' </summary>
    <Item(539, "Diancite")>
    Public Class Diancite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Diancie", 719)
            _textureRectangle = New Rectangle(96, 72, 24, 24)
        End Sub

    End Class

End Namespace
