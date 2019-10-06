Namespace Items

    ''' <summary>
    ''' The base item for all elemental gems.
    ''' </summary>
    Public MustInherit Class GemItem

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Standard
        Public Overrides ReadOnly Property Description As String
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 200

        Public Sub New(ByVal Type As Element.Types)
            Description = "Increases the power of the holder's first " & Type.ToString() & "-type move by 30%, and is consumed after use."
            _textureSource = "Items\Gems"
            _textureRectangle = GetTextureRectangle(Type)
        End Sub

        Private Function GetTextureRectangle(ByVal Type As Element.Types) As Rectangle
            Dim typeArray As List(Of Element.Types) = {Element.Types.Bug, Element.Types.Dark, Element.Types.Dragon, Element.Types.Electric, Element.Types.Fairy, Element.Types.Fighting, Element.Types.Fire, Element.Types.Flying, Element.Types.Ghost, Element.Types.Grass, Element.Types.Ground, Element.Types.Ice, Element.Types.Poison, Element.Types.Psychic, Element.Types.Rock, Element.Types.Steel, Element.Types.Water, Element.Types.Normal}.ToList()
            Dim i As Integer = typeArray.IndexOf(Type)

            Dim x As Integer = i
            Dim y As Integer = 0
            While x > 4
                x -= 5
                y += 1
            End While

            Return New Rectangle(x * 24, y * 24, 24, 24)
        End Function

    End Class

End Namespace