Namespace Items.MegaStones

    ''' <summary>
    ''' A Mega Stone for Mewtwo.
    ''' </summary>
    <Item(529, "Mewtwonite X")>
    Public Class MewtwoniteX

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Mewtwonite X", 529, New Rectangle(48, 48, 24, 24), "Mewtwo", 150)
        End Sub

    End Class

End Namespace
