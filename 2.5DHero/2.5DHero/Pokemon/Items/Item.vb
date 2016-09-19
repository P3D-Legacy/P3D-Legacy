Imports net.Pokemon3D.Game.Items
''' <summary>
''' An item the player stores in their inventory.
''' </summary>
Public Class Item

    ''' <summary>
    ''' The type of item. This is also the bag they get sorted into.
    ''' </summary>
    Public Enum ItemTypes
        ''' <summary>
        ''' The default item category for misc. items.
        ''' </summary>
        Standard
        ''' <summary>
        ''' Medicine items that restore Pokémon.
        ''' </summary>
        Medicine
        ''' <summary>
        ''' Plants, like berries and apricorns.
        ''' </summary>
        Plants
        ''' <summary>
        ''' All Poké Balls.
        ''' </summary>
        Pokéballs
        ''' <summary>
        ''' TMs and HMs.
        ''' </summary>
        Machines
        ''' <summary>
        ''' Keyitems of the game.
        ''' </summary>
        KeyItems
        ''' <summary>
        ''' Mail items.
        ''' </summary>
        Mail
        ''' <summary>
        ''' Items to be used in battle.
        ''' </summary>
        BattleItems
    End Enum

#Region "Fields"

    Private _id As Integer = 0
    Protected _name As String = ""
    Protected _pluralName As String = ""
    Protected _description As String = ""

    Protected _pokeDollarPrice As Integer = 0
    Protected _battlePointsPrice As Integer = 1
    Protected _itemType As ItemTypes = ItemTypes.Standard
    Protected _catchMultiplier As Single = 1.0F
    Protected _maxStack As Integer = 999
    Protected _sortValue As Integer = 0

    Protected _texture As Texture2D
    Private _additionalData As String = ""

    Protected _canBeTraded As Boolean = True
    Protected _canBeHold As Boolean = True
    Protected _canBeUsed As Boolean = True
    Protected _canBeUsedInBattle As Boolean = True
    Protected _canBeTossed As Boolean = True

    Protected _isBall As Boolean = False
    Protected _isBerry As Boolean = False
    Protected _isHealingItem As Boolean = False
    Protected _isMail As Boolean = False
    Protected _isMegaStone As Boolean = False
    Protected _isPlate As Boolean = False

    Protected _flingDamage As Integer = 30
    Protected _requiresPokemonSelectInBattle As Boolean = True

#End Region

#Region "Properties"

    ''' <summary>
    ''' The singular item name.
    ''' </summary>
    Public ReadOnly Property Name() As String
        Get
            Return Me._name
        End Get
    End Property

    ''' <summary>
    ''' The ID of the item.
    ''' </summary>
    Public ReadOnly Property ID() As Integer
        Get
            Return Me._id
        End Get
    End Property

    ''' <summary>
    ''' The plural name of the item.
    ''' </summary>
    Public ReadOnly Property PluralName() As String
        Get
            Return Me._pluralName
        End Get
    End Property

    ''' <summary>
    ''' The price of this item if the player purchases it in exchange for PokéDollars. This halves when selling an item to the store.
    ''' </summary>
    Public ReadOnly Property PokéDollarPrice() As Integer
        Get
            Return Me._pokeDollarPrice
        End Get
    End Property

    ''' <summary>
    ''' The price of this item if the player purchases it exchange for BattlePoints.
    ''' </summary>
    Public ReadOnly Property BattlePointsPrice() As Integer
        Get
            Return Me._battlePointsPrice
        End Get
    End Property

    ''' <summary>
    ''' The type of this item. This also controls in which bag this item gets sorted.
    ''' </summary>
    Public ReadOnly Property ItemType() As ItemTypes
        Get
            Return Me._itemType
        End Get
    End Property

    ''' <summary>
    ''' The default catch multiplier if the item gets used as a Pokéball.
    ''' </summary>
    Public ReadOnly Property CatchMultiplier() As Single
        Get
            Return Me._catchMultiplier
        End Get
    End Property

    ''' <summary>
    ''' The maximum amount of this item type (per ID) that can be stored in the bag.
    ''' </summary>
    Public ReadOnly Property MaxStack() As Integer
        Get
            Return Me._maxStack
        End Get
    End Property

    ''' <summary>
    ''' A value that can be used to sort items in the bag after. Lower values make items appear closer to the top.
    ''' </summary>
    Public ReadOnly Property SortValue() As Integer
        Get
            Return Me._sortValue
        End Get
    End Property

    ''' <summary>
    ''' The texture of this item.
    ''' </summary>
    Public ReadOnly Property Texture() As Texture2D
        Get
            Return Me._texture
        End Get
    End Property

    ''' <summary>
    ''' The bag description of this item.
    ''' </summary>
    Public ReadOnly Property Description() As String
        Get
            Return Me._description
        End Get
    End Property

    ''' <summary>
    ''' The additional data that is stored with this item.
    ''' </summary>
    Public Property AdditionalData() As String
        Get
            Return Me._additionalData
        End Get
        Set(value As String)
            Me._additionalData = value
        End Set
    End Property

    ''' <summary>
    ''' The damage the Fling move does when this item is attached to a Pokémon.
    ''' </summary>
    Public ReadOnly Property FlingDamage() As Integer
        Get
            Return Me._flingDamage
        End Get
    End Property

    ''' <summary>
    ''' If this item can be traded in for money.
    ''' </summary>
    Public ReadOnly Property CanBeTraded() As Boolean
        Get
            Return Me._canBeTraded
        End Get
    End Property

    ''' <summary>
    ''' If this item can be given to a Pokémon.
    ''' </summary>
    Public ReadOnly Property CanBeHold() As Boolean
        Get
            Return Me._canBeHold
        End Get
    End Property

    ''' <summary>
    ''' If this item can be used from the bag.
    ''' </summary>
    Public ReadOnly Property CanBeUsed() As Boolean
        Get
            Return Me._canBeUsed
        End Get
    End Property

    ''' <summary>
    ''' If this item can be used in battle.
    ''' </summary>
    Public ReadOnly Property CanBeUsedInBattle() As Boolean
        Get
            Return Me._canBeUsedInBattle
        End Get
    End Property

    ''' <summary>
    ''' If this item can be tossed in the bag.
    ''' </summary>
    Public ReadOnly Property CanBeTossed() As Boolean
        Get
            Return Me._canBeTossed
        End Get
    End Property

    ''' <summary>
    ''' If this item requires the player to select a Pokémon to use the item on in battle.
    ''' </summary>
    Public ReadOnly Property BattleSelectPokemon() As Boolean
        Get
            Return Me._requiresPokemonSelectInBattle
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Pokéball item.
    ''' </summary>
    Public ReadOnly Property IsBall() As Boolean
        Get
            Return Me._isBall
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Berry item.
    ''' </summary>
    Public ReadOnly Property IsBerry() As Boolean
        Get
            Return Me._isBerry
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Healing item.
    ''' </summary>
    Public ReadOnly Property IsHealingItem() As Boolean
        Get
            Return Me._isHealingItem
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Mail item.
    ''' </summary>
    Public ReadOnly Property IsMail() As Boolean
        Get
            Return Me._isMail
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Mega Stone.
    ''' </summary>
    Public ReadOnly Property IsMegaStone() As Boolean
        Get
            Return Me._isMegaStone
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Plate.
    ''' </summary>
    Public ReadOnly Property IsPlate() As Boolean
        Get
            Return Me._isPlate
        End Get
    End Property

    ''' <summary>
    ''' The color for player dialogues.
    ''' </summary>
    Public Shared ReadOnly Property PlayerDialogueColor() As Color
        Get
            Return New Color(0, 128, 227)
        End Get
    End Property

#End Region

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
    Public Sub New(ByVal Name As String, ByVal Price As Integer, ByVal ItemType As ItemTypes, ByVal ID As Integer, ByVal CatchMultiplier As Single, ByVal SortValue As Integer, ByVal TextureRectangle As Rectangle, ByVal Description As String)
        Me.Initialize(Name, Price, ItemType, ID, CatchMultiplier, SortValue, TextureRectangle, Description)
    End Sub

    Friend Sub New()
    End Sub

    ''' <summary>
    ''' Sets most properties of an Item class instance.
    ''' </summary>
    ''' <param name="Name">The name of the Item.</param>
    ''' <param name="Price">The purchase price.</param>
    ''' <param name="ItemType">The type of Item.</param>
    ''' <param name="ID">The ID of this Item.</param>
    ''' <param name="CatchMultiplier">The CatchMultiplier of this Item.</param>
    ''' <param name="SortValue">The SortValue of this Item.</param>
    ''' <param name="TextureRectangle">The TextureRectangle from the "Items\ItemSheet" texture.</param>
    ''' <param name="Description">The description of this Item.</param>
    Protected Sub Initialize(ByVal Name As String, ByVal Price As Integer, ByVal ItemType As ItemTypes, ByVal ID As Integer, ByVal CatchMultiplier As Single, ByVal SortValue As Integer, ByVal TextureRectangle As Rectangle, ByVal Description As String)
        Me._name = Name
        Me._pluralName = Name & "s" 'Default plural name with "s" at the end.
        Me._pokeDollarPrice = Price
        Me._itemType = ItemType
        Me._id = ID
        Me._catchMultiplier = CatchMultiplier
        Me._sortValue = SortValue
        Me._description = Description

        Me._texture = TextureManager.GetTexture("Items\ItemSheet", TextureRectangle, "")
    End Sub

    ''' <summary>
    ''' The item gets used from the bag.
    ''' </summary>
    Public Overridable Sub Use()
        Logger.Debug("PLACEHOLDER FOR ITEM USE")
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
        Core.Player.Inventory.RemoveItem(Me.ID, 1)
        If Core.Player.Inventory.GetItemAmount(Me.ID) = 0 Then
            Return "*There are no~" & Me.PluralName & " left."
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
    Public Shared Function GetItemByID(ByVal ID As Integer) As Item
        'Check if the ID is available in the FileItem list.
        For Each f As FileItem In Core.Player.FileItems
            If f.Item.ID = ID Then
                Return f.Item
            End If
        Next

        If _itemBuffer Is Nothing Then
            LoadItemBuffer()
        End If

        Dim type = _itemBuffer.FirstOrDefault(Function(itemTypePair)
                                                  Return itemTypePair.Key.Id = ID
                                              End Function).Value
        If type IsNot Nothing Then
            Return CType(Activator.CreateInstance(type), Item)
        End If

        Return Nothing
    End Function

    ''' <summary>
    ''' Returns an item based on its name.
    ''' </summary>
    ''' <param name="name">The name of the item.</param>
    Public Shared Function GetItemByName(ByVal name As String) As Item
        'Check if the name is available in the FileItem list.
        For Each FileItem As Items.FileItem In Core.Player.FileItems
            If FileItem.Item.Name.ToLower() = name.ToLower() Then
                Return FileItem.Item
            End If
        Next

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
