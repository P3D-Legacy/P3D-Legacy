Namespace Items.Standard

    <Item(136, "Dusk Stone")>
    Public Class DuskStone

        Inherits StoneItem

        Public Overrides ReadOnly Property Description As String = "A peculiar stone that makes certain species of Pokémon evolve. It is as dark as dark can be."

        Public Sub New()
            _textureRectangle = New Rectangle(360, 192, 24, 24)
        End Sub

    End Class

End Namespace
