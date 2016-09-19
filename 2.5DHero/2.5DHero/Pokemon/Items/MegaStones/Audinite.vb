Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Audino.
    ''' </summary>
    <Item(536, "Audinite")>
    Public Class Audinite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Audinite", 536, New Rectangle(24, 72, 24, 24), "Audino", 531)
        End Sub

    End Class

End Namespace
