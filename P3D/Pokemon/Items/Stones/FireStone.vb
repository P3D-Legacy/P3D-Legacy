Namespace Items.Stones

    <Item(22, "Fire Stone")>
    Public Class FireStone

        Inherits StoneItem

        Public Overrides ReadOnly Property Description As String = "A peculiar stone that can make certain species of Pokémon evolve. The stone has a fiery orange heart."

        Public Sub New()
            _textureRectangle = New Rectangle(480, 0, 24, 24)
        End Sub

    End Class

End Namespace
