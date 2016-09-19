Namespace Items.MegaStones

    ''' <summary>
    ''' A Mega Stone for Mewtwo.
    ''' </summary>
    <Item(530, "Mewtwonite Y")>
    Public Class MewtwoniteY

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Mewtwonite Y", 530, New Rectangle(72, 48, 24, 24), "Mewtwo", 150)
        End Sub

    End Class

End Namespace
