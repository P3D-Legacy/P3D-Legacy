Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Mawile.
    ''' </summary>
    <Item(527, "Mawilite")>
    Public Class Mawilite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Mawilite", 527, New Rectangle(0, 48, 24, 24), "Mawile", 303)
        End Sub

    End Class

End Namespace
