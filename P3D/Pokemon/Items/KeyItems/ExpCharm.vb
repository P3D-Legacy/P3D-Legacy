Namespace Items.KeyItems

    <Item(656, "Exp. Charm")>
    Public Class ExpCharm

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "Having one of these charms increases the Exp. Points your Pokémon get. It's a strange, stretchy charm that encourages growth."

        Public Sub New()
            _textureRectangle = New Rectangle(456, 408, 24, 24)
        End Sub

    End Class

End Namespace
