Namespace Items.MegaStones

    ''' <summary>
    ''' The Mega Stone for Gyarados.
    ''' </summary>
    <Item(521, "Gyaradosite")>
    Public Class Gyaradosite

        Inherits MegaStone

        Public Sub New()
            MyBase.New("Gyaradosite", 521, New Rectangle(96, 24, 24, 24), "Gyarados", 130)
        End Sub

    End Class

End Namespace
