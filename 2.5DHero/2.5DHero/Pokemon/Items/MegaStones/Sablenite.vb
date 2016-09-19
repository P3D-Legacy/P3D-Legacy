Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Sableye.
    ''' </summary>
    <Item(547, "Sablenite")>
    Public Class Sablenite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Sablenite", 547, New Rectangle(48, 96, 24, 24), "Sableye", 302)
        End Sub

    End Class

End Namespace
