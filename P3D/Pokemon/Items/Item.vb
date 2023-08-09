Imports P3D.Items

''' <summary>
''' An item the player stores in their inventory.
''' </summary>
Public MustInherit Class Item

    Protected _textureSource As String = "Items\ItemSheet"
    Protected _textureRectangle As Rectangle
    Private _texture As Texture2D

    'GameMode Items
    Public gmID As String = ""
    Public gmName As String = ""
    Public gmPluralName As String = gmName & "s"
    Public gmDescription As String = ""
    Public gmTextureSource As String = "Items\GameModeItems"
    Public gmTextureRectangle As Rectangle
    Public gmIsBerry As Boolean = False
    Public gmIsMail As Boolean = False
    Public gmIsMegaStone As Boolean = False
    Public gmIsPlate As Boolean = False

    Public Property IsGameModeItem As Boolean = False

    Public ReadOnly Property TextureSource As String
        Get
            Return _textureSource & "," & _textureRectangle.X & "," & _textureRectangle.Y & "," & _textureRectangle.Width & "," & _textureRectangle.Height
        End Get
    End Property

    Public Function GetDescription() As String
        If IsGameModeItem = True Then
            If Localization.TokenExists("item_desc_" & gmID) = True Then
                Return Localization.GetString("item_desc_" & gmID)
            Else
                Return Me.gmDescription
            End If
        Else
            If Localization.TokenExists("item_desc_" & GetAttribute().Id) = True Then
                Return Localization.GetString("item_desc_" & GetAttribute().Id)
            Else
                Return Me.Description
            End If
        End If
    End Function

    Private _attribute As ItemAttribute

    Private Function GetAttribute() As ItemAttribute
        If _attribute Is Nothing Then
            _attribute = CType([GetType]().GetCustomAttributes(GetType(ItemAttribute), False)(0), ItemAttribute)
        End If

        Return _attribute
    End Function

    Public Function OriginalName() As String
        If IsGameModeItem = True Then
            Return gmName
        Else
            Return GetAttribute().Name
        End If
    End Function

    ''' <summary>
    ''' The singular item name.
    ''' </summary>
    Public Overridable ReadOnly Property Name As String
        Get
            If IsGameModeItem = True Then
                If Localization.TokenExists("item_name_" & gmID) = True Then
                    Return Localization.GetString("item_name_" & gmID)
                Else
                    Return gmName
                End If
            Else
                If Localization.TokenExists("item_name_" & GetAttribute().Id) = True Then
                    Return Localization.GetString("item_name_" & GetAttribute().Id)
                Else
                    Return GetAttribute().Name
                End If
            End If
        End Get
    End Property

    Public Function OneLineName() As String
        Return Name.Replace("~", " ")
    End Function
    Public Function OneLinePluralName() As String
        If IsGameModeItem = True Then
            If Localization.TokenExists("item_pluralname_" & gmID) = True Then
                Return Localization.GetString("item_pluralname_" & gmID).Replace("~", " ")
            Else
                Return gmPluralName.Replace("~", " ")
            End If
        Else
            If Localization.TokenExists("item_pluralname_" & Me.ID) = True Then
                Return Localization.GetString("item_pluralname_" & Me.ID).Replace("~", " ")
            Else
                Return PluralName.Replace("~", " ")
            End If
        End If
    End Function

    ''' <summary>
    ''' The ID of the item.
    ''' </summary>
    Public Overridable ReadOnly Property ID As Integer
        Get
            Return GetAttribute().Id
        End Get
    End Property

    ''' <summary>
    ''' The plural name of the item.
    ''' </summary>
    Public Overridable ReadOnly Property PluralName As String
        Get
            Return Name & "s" 'Default plural name with "s" at the end.
        End Get
    End Property

    ''' <summary>
    ''' The price of this item if the player purchases it in exchange for PokéDollars. This halves when selling an item to the store.
    ''' </summary>
    Public Overridable ReadOnly Property PokeDollarPrice As Integer = 0

    ''' <summary>
    ''' The price of this item if the player purchases it exchange for BattlePoints.
    ''' </summary>
    Public Overridable ReadOnly Property BattlePointsPrice As Integer = 1

    ''' <summary>
    ''' The type of this item. This also controls in which bag this item gets sorted.
    ''' </summary>
    Public Overridable ReadOnly Property ItemType As ItemTypes = ItemTypes.Standard

    ''' <summary>
    ''' The default catch multiplier if the item gets used as a Pokéball.
    ''' </summary>
    Public Overridable ReadOnly Property CatchMultiplier As Single = 1.0F

    ''' <summary>
    ''' The maximum amount of this item type (per ID) that can be stored in the bag.
    ''' </summary>
    Public Overridable ReadOnly Property MaxStack As Integer = 999

    ''' <summary>
    ''' A value that can be used to sort items in the bag after. Lower values make items appear closer to the top.
    ''' </summary>
    Public Overridable ReadOnly Property SortValue As Integer = 0

    ''' <summary>
    ''' The texture of this item.
    ''' </summary>
    Public ReadOnly Property Texture As Texture2D
        Get
            If IsGameModeItem = True Then
                If _texture Is Nothing Then
                    _texture = TextureManager.GetTexture(gmTextureSource, gmTextureRectangle, "")
                End If
            Else
                If _texture Is Nothing Then
                    _texture = TextureManager.GetTexture(_textureSource, _textureRectangle, "")
                End If
            End If

            Return _texture
        End Get
    End Property

    ''' <summary>
    ''' The bag description of this item.
    ''' </summary>
    Public Overridable ReadOnly Property Description As String = ""

    ''' <summary>
    ''' The additional data that is stored with this item.
    ''' </summary>
    Public Property AdditionalData As String = ""
    

    ''' <summary>
    ''' The damage the Fling move does when this item is attached to a Pokémon.
    ''' </summary>
    Public Overridable ReadOnly Property FlingDamage As Integer = 30

    ''' <summary>
    ''' If this item can be traded in for money.
    ''' </summary>
    Public Overridable ReadOnly Property CanBeTraded As Boolean = True

    ''' <summary>
    ''' If this item can be given to a Pokémon.
    ''' </summary>
    Public Overridable ReadOnly Property CanBeHeld As Boolean = True

    ''' <summary>
    ''' If this item can be used from the bag.
    ''' </summary>
    Public Overridable ReadOnly Property CanBeUsed As Boolean = True

    ''' <summary>
    ''' If this item can be used in battle.
    ''' </summary>
    Public Overridable ReadOnly Property CanBeUsedInBattle As Boolean = True

    ''' <summary>
    ''' If this item can be tossed in the bag.
    ''' </summary>
    Public Overridable ReadOnly Property CanBeTossed As Boolean = True

    ''' <summary>
    ''' If this item requires the player to select a Pokémon to use the item on in battle.
    ''' </summary>
    Public Overridable ReadOnly Property BattleSelectPokemon As Boolean = True

    ''' <summary>
    ''' If this item is a Healing item.
    ''' </summary>
    Public Overridable ReadOnly Property IsHealingItem As Boolean = False

    ''' <summary>
    ''' If this item is a Pokéball item.
    ''' </summary>
    Public Overridable ReadOnly Property IsBall As Boolean
        Get
            If IsGameModeItem = True Then
                Return Me.ItemType = ItemTypes.Pokéballs
            Else
                Return [GetType]().IsSubclassOf(GetType(Items.Balls.BallItem))
            End If
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Berry item.
    ''' </summary>
    Public ReadOnly Property IsBerry As Boolean
        Get
            If IsGameModeItem = True Then
                Return gmIsBerry
            Else
                Return [GetType]().IsSubclassOf(GetType(Berry))
            End If
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Mail item.
    ''' </summary>
    Public ReadOnly Property IsMail As Boolean
        Get
            If IsGameModeItem = True Then
                Return gmIsMail
            Else
                Return [GetType]().IsSubclassOf(GetType(MailItem))
            End If
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Mega Stone.
    ''' </summary>
    Public ReadOnly Property IsMegaStone As Boolean
        Get
            If IsGameModeItem = True Then
                Return gmIsMegaStone
            Else
                Return [GetType]().IsSubclassOf(GetType(MegaStone))
            End If
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Plate.
    ''' </summary>
    Public ReadOnly Property IsPlate As Boolean
        Get
            If IsGameModeItem = True Then
                Return gmIsPlate
            Else
                Return [GetType]().IsSubclassOf(GetType(PlateItem))
            End If
        End Get
    End Property

    ''' <summary>
    ''' The color for player dialogues.
    ''' </summary>
    Public Shared ReadOnly Property PlayerDialogueColor As Color
        Get
            Return New Color(0, 128, 227)
        End Get
    End Property

    ''' <summary>
    ''' The item gets used from the bag.
    ''' </summary>
    Public Overridable Sub Use()
        Logger.Debug("PLACEHOLDER FOR ITEM USE")
    End Sub

    Public Sub UseItemhandler(ByVal params As Object())
        If UseOnPokemon(CInt(params(0))) Then
            Dim s As Screen = Core.CurrentScreen
            While Not s.PreScreen Is Nothing And s.Identification <> Screen.Identifications.InventoryScreen
                s = s.PreScreen
            End While

            If s.Identification = Screen.Identifications.InventoryScreen Then
                CType(s, NewInventoryScreen).LoadItems()
            End If
        End If
    End Sub

    ''' <summary>
    ''' A method that gets used when the item is applied to a Pokémon. Returns True if the action was successful.
    ''' </summary>
    ''' <param name="PokeIndex">The Index of the Pokémon in party.</param>
    Public Overridable Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Logger.Debug("PLACEHOLDER FOR ITEM USE ON POKEMON")
        Return False
    End Function

    ''' <summary>
    ''' Tries to remove a single item of this item type from the player's bag and returns a message which changes depending on if the item that got removed was the last one of its kind.
    ''' </summary>
    Public Function RemoveItem() As String
        Dim ItemID As String
        If IsGameModeItem = True Then
            ItemID = Me.gmID
        Else
            ItemID = Me.ID.ToString
        End If
        Core.Player.Inventory.RemoveItem(ItemID, 1)
        Dim s As Screen = Core.CurrentScreen
        While Not s.PreScreen Is Nothing And s.Identification <> Screen.Identifications.InventoryScreen
            s = s.PreScreen
        End While

        If s.Identification = Screen.Identifications.InventoryScreen Then
            CType(s, NewInventoryScreen).LoadItems()
        End If

        If Core.Player.Inventory.GetItemAmount(ItemID) <= 0 Then
            Return "*There are no~" & Me.OneLinePluralName() & " left."
        End If

        Return ""
    End Function

    Private Shared _itemBuffer As Dictionary(Of ItemIdentifier, Type)

    Private Shared Sub LoadItemBuffer()
        _itemBuffer = GetType(Item).Assembly.GetTypes().Where(Function(t As Type)
                                                                  Return t.GetCustomAttributes(GetType(ItemAttribute), False).Length = 1
                                                              End Function).ToDictionary(Function(tt As Type)
                                                                                             Dim attr = CType(tt.GetCustomAttributes(GetType(ItemAttribute), False)(0), ItemAttribute)
                                                                                             Return New ItemIdentifier() With
                                                                                             {
                                                                                                 .Id = attr.Id,
                                                                                                 .Name = attr.Name
                                                                                             }
                                                                                         End Function,
                                                                                         Function(tt As Type)
                                                                                             Return tt
                                                                                         End Function)
    End Sub

    ''' <summary>
    ''' Returns an item instance based on the passed in ID.
    ''' </summary>
    ''' <param name="ID">The desired item's ID.</param>
    Public Shared Function GetItemByID(ByVal ID As String) As Item
        If ID.Contains("gm") Then
            Dim gmItem As GameModeItem = GameModeItemLoader.GetItemByID(ID)
            If gmItem IsNot Nothing Then
                Return gmItem
            End If
        Else
            If _itemBuffer Is Nothing Then
                LoadItemBuffer()
            End If

            Dim type = _itemBuffer.FirstOrDefault(Function(itemTypePair)
                                                      Return itemTypePair.Key.Id = CInt(ID)
                                                  End Function).Value
            If type IsNot Nothing Then
                Return CType(Activator.CreateInstance(type), Item)
            End If

        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Returns an item based on its name.
    ''' </summary>
    ''' <param name="name">The name of the item.</param>
    Public Shared Function GetItemByName(ByVal name As String) As Item
        Dim type = _itemBuffer.FirstOrDefault(Function(itemTypePair)
                                                  Return itemTypePair.Key.Name.ToLowerInvariant() = name.ToLowerInvariant()
                                              End Function).Value
        If type IsNot Nothing Then
            Return CType(Activator.CreateInstance(type), Item)
        End If

        Logger.Log(Logger.LogTypes.Warning, "Item.vb: Cannot find item with the name """ & name & """.")
        Return Nothing
    End Function

End Class
