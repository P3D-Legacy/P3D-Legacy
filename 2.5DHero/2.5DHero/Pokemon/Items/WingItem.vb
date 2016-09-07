Namespace Items

    ''' <summary>
    ''' Represents a Wing Item.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WingItem

        Inherits Item

        ''' <summary>
        ''' Creates a new instance of the Item class.
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

            Me._flingDamage = 20
        End Sub

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        ''' <summary>
        ''' Checks if a Wing can be used on a Pokémon.
        ''' </summary>
        ''' <param name="stat">An integer representing the stat that should be upped by the Wing.</param>
        ''' <param name="p">The Pokémon that the Wing should be used on.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function CanUseWing(ByVal stat As Integer, ByVal p As Pokemon) As Boolean
            If stat < 255 Then
                Dim allStats As Integer = p.EVAttack + p.EVDefense + p.EVSpAttack + p.EVSpDefense + p.EVHP + p.EVSpeed
                If allStats < 510 Then
                    Return True
                End If
            End If

            Return False
        End Function

    End Class

End Namespace