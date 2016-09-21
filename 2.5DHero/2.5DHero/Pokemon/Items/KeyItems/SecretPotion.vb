Namespace Items.KeyItems

    <Item(67, "SecretPotion")>
    Public Class SecretPotion

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "A fantastic medicine dispensed by the pharmacy in Cianwood City. It fully heals a Pokémon of any ailment."

        Public Sub New()
            _textureRectangle = New Rectangle(456, 48, 24, 24)
        End Sub

    End Class

End Namespace
