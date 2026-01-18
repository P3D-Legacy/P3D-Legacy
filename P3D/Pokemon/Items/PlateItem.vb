Namespace Items

    ''' <summary>
    ''' The base item for all Arceus plates.
    ''' </summary>
    Public MustInherit Class PlateItem

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Standard
        Public Overrides ReadOnly Property Description As String
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1000

        Public Sub New(ByVal Type As Integer)
            Dim element As Element = BattleSystem.GameModeElementLoader.GetElementByID(Type)
            Description = "An item to be held by a Pokémon. It's a stone tablet that boosts the power of " & element.ToString() & "-type moves."
            _textureSource = "Items\Plates"
            _textureRectangle = GetTextureRectangle(element.Type)
        End Sub

        Private Function GetTextureRectangle(ByVal Type As Integer) As Rectangle
            If Type <= 19 Then
                Dim typeArray As List(Of Element.Types) = {Element.Types.Bug, Element.Types.Dark, Element.Types.Dragon, Element.Types.Electric, Element.Types.Fairy, Element.Types.Fighting, Element.Types.Fire, Element.Types.Flying, Element.Types.Ghost, Element.Types.Grass, Element.Types.Ground, Element.Types.Ice, Element.Types.Poison, Element.Types.Psychic, Element.Types.Rock, Element.Types.Steel, Element.Types.Water}.ToList()

                Dim i As Integer = typeArray.IndexOf(CType(Type, Element.Types))

                Dim x As Integer = i
                Dim y As Integer = 0
                While x > 4
                    x -= 5
                    y += 1
                End While

                Return New Rectangle(x * 24, y * 24, 24, 24)

            ElseIf Me.IsGameModeItem = True Then
                Return gmTextureRectangle
            Else
                Return New Rectangle(0, 0, 24, 24)
            End If
        End Function

    End Class

End Namespace
