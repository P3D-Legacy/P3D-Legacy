Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Gallade.
    ''' </summary>
    <Item(540, "Galladite")>
    Public Class Galladite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Galladite", 540, New Rectangle(120, 72, 24, 24), "Gallade", 475)
        End Sub

    End Class

End Namespace
