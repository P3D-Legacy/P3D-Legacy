Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Ampharos.
    ''' </summary>
    <Item(512, "Ampharosite")>
    Public Class Ampharosite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Ampharosite", 512, New Rectangle(120, 0, 24, 24), "Ampharos", 181)
        End Sub

    End Class

End Namespace
