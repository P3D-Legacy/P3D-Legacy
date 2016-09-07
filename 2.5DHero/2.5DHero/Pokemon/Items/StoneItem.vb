Namespace Items

    Public Class StoneItem

        Inherits Item

        ''' <summary>
        ''' Creates a new instance of the StoneItem class.
        ''' </summary>
        ''' <param name="Name">The name of the Item.</param>
        ''' <param name="Price">The purchase price.</param>
        ''' <param name="ItemType">The type of Item.</param>
        ''' <param name="ID">The ID of this Item.</param>
        ''' <param name="CatchMultiplier">The CatchMultiplier of this Item.</param>
        ''' <param name="SortValue">The SortValue of this Item.</param>
        ''' <param name="TextureRectangle">The TextureRectangle from the "Items\ItemSheet" texture.</param>
        ''' <param name="Description">The description of this Item.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal Name As String, ByVal Price As Integer, ByVal ItemType As ItemTypes, ByVal ID As Integer, ByVal CatchMultiplier As Single, ByVal SortValue As Integer, ByVal TextureRectangle As Rectangle, ByVal Description As String)
            MyBase.New(Name, Price, ItemType, ID, CatchMultiplier, SortValue, TextureRectangle, Description)

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = True
            Me._canBeUsedInBattle = False
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
            Return Me.UseStone(PokeIndex)
        End Function

        Public Function UseStone(ByVal PokeIndex As Integer) As Boolean
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If

            Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

            If p.IsEgg() = False And p.CanEvolve(EvolutionCondition.EvolutionTrigger.ItemUse, Me.ID.ToString()) = True Then
                Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New EvolutionScreen(Core.CurrentScreen, {PokeIndex}.ToList(), Me.ID.ToString(), EvolutionCondition.EvolutionTrigger.ItemUse), Color.Black, False))

                PlayerStatistics.Track("[22]Evolution stones used", 1)
                RemoveItem()

                Return True
            Else
                Screen.TextBox.Show("Cannot use on~" & p.GetDisplayName(), {}, False, False)

                Return False
            End If
        End Function

    End Class

End Namespace