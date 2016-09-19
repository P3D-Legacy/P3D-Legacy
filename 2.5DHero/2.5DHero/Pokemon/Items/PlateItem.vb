Namespace Items

    ''' <summary>
    ''' The base item for all Arceus plates.
    ''' </summary>
    Public MustInherit Class PlateItem

        Inherits Item

        Public Sub New(ByVal Name As String, ByVal ID As Integer, ByVal Type As Element.Types)
            MyBase.New(Name, 1000, ItemTypes.Standard, ID, 1.0F, 0, New Rectangle(0, 0, 24, 24), "An item to be held by a Pokémon. It's a stone tablet that boosts the power of " & Type.ToString() & "-type moves.")

            Me._texture = TextureManager.GetTexture("Items\Plates", GetTextureRectangle(Type), "")

            Me._canBeHold = True
            Me._canBeTossed = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = False

            Me._isPlate = True
        End Sub

        Private Function GetTextureRectangle(ByVal Type As Element.Types) As Rectangle
            Dim typeArray As List(Of Element.Types) = {Element.Types.Bug, Element.Types.Dark, Element.Types.Dragon, Element.Types.Electric, Element.Types.Fairy, Element.Types.Fighting, Element.Types.Fire, Element.Types.Flying, Element.Types.Ghost, Element.Types.Grass, Element.Types.Ground, Element.Types.Ice, Element.Types.Poison, Element.Types.Psychic, Element.Types.Rock, Element.Types.Steel, Element.Types.Water}.ToList()
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
