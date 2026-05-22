Namespace Items.KeyItems

    <Item(242, "Shiny Charm")>
    Public Class ShinyCharm

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A shiny charm said to increase the chance of encountering Shiny Pokémon."

        Public Sub New()
            _textureRectangle = New Rectangle(120, 264, 24, 24)
        End Sub

    End Class

End Namespace
