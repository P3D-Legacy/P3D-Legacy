Namespace Items.KeyItems

    <Item(576, "Mega Bracelet")>
    Public Class MegaBracelet

        Inherits KeyItem

        Public Overrides ReadOnly Property Description As String = "This bracelet contains an untold power that somehow enables Pokémon carrying a Mega Stone to Mega Evolve in battle. "

        Public Sub New()
            _textureRectangle = New Rectangle(312, 288, 24, 24)
        End Sub

    End Class

End Namespace
