Namespace Items.KeyItems

    <Item(241, "Oval Charm")>
    Public Class OvalCharm

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "An oval charm said to increase the chance of Pokémon Eggs being found at the Day Care."

        Public Sub New()
            _textureRectangle = New Rectangle(96, 264, 24, 24)
        End Sub

    End Class

End Namespace
