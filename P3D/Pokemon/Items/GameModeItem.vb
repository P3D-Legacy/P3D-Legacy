Imports P3D.Items

''' <summary>
''' An item the player stores in their inventory.
''' </summary>
Public Class GameModeItem

    Inherits Item

    Public gmTextureSource As String = "Items\GameModeItems"
    Public gmPluralName As String = gmName
    Public gmPrice As Integer = 0
    Public gmBattlePointsPrice As Integer = 1
    Public gmItemType As ItemTypes = ItemTypes.Standard

    Public gmCatchMultiplier As Single = 1.0F
    Public gmMaxStack As Integer = 999
    Public gmFlingDamage As Integer = 30
    Public gmCanBeTraded As Boolean = True
    Public gmCanBeHeld As Boolean = True
    Public gmCanBeUsed As Boolean = True
    Public gmCanBeUsedInBattle As Boolean = True
    Public gmCanBeTossed As Boolean = True
    Public gmBattleSelectPokemon As Boolean = True
    'Medicine Item
    Public gmIsHealingItem As Boolean = False
    Public gmHealHPAmount As Integer = 0
    Public gmCureStatusEffects As List(Of Pokemon.StatusProblems)
    'Evolution Item
    Public gmIsEvolutionItem As Boolean = False
    Public gmEvolutionPokemon As List(Of Integer)

    'Mega Stone Item
    Public gmMegaPokemonNumber As Integer

    Public Sub New()
        IsGameModeItem = True
    End Sub

    ''' <summary>
    ''' The plural name of the item.
    ''' </summary>
    Public Overrides ReadOnly Property PluralName As String
        Get
            Return gmPluralName
        End Get
    End Property

    ''' <summary>
    ''' The price of this item if the player purchases it in exchange for PokéDollars. This halves when selling an item to the store.
    ''' </summary>
    Public Overrides ReadOnly Property PokeDollarPrice As Integer = gmPrice

    ''' <summary>
    ''' The price of this item if the player purchases it exchange for BattlePoints.
    ''' </summary>
    Public Overrides ReadOnly Property BattlePointsPrice As Integer = gmBattlePointsPrice

    ''' <summary>
    ''' The type of this item. This also controls in which bag this item gets sorted.
    ''' </summary>
    Public Overrides ReadOnly Property ItemType As ItemTypes = gmItemType

    ''' <summary>
    ''' The default catch multiplier if the item gets used as a Pokéball.
    ''' </summary>
    Public Overrides ReadOnly Property CatchMultiplier As Single = gmCatchMultiplier

    ''' <summary>
    ''' The maximum amount of this item type (per ID) that can be stored in the bag.
    ''' </summary>
    Public Overrides ReadOnly Property MaxStack As Integer = gmMaxStack

    ''' <summary>
    ''' A value that can be used to sort items in the bag after. Lower values make items appear closer to the top.
    ''' </summary>
    Public Overrides ReadOnly Property SortValue As Integer = 0


    ''' <summary>
    ''' The bag description of this item.
    ''' </summary>
    Public Overrides ReadOnly Property Description As String = gmDescription


    ''' <summary>
    ''' The damage the Fling move does when this item is attached to a Pokémon.
    ''' </summary>
    Public Overrides ReadOnly Property FlingDamage As Integer = gmFlingDamage

    ''' <summary>
    ''' If this item can be traded in for money.
    ''' </summary>
    Public Overrides ReadOnly Property CanBeTraded As Boolean = gmCanBeTraded

    ''' <summary>
    ''' If this item can be given to a Pokémon.
    ''' </summary>
    Public Overrides ReadOnly Property CanBeHeld As Boolean = gmCanBeHeld

    ''' <summary>
    ''' If this item can be used from the bag.
    ''' </summary>
    Public Overrides ReadOnly Property CanBeUsed As Boolean = gmCanBeUsed

    ''' <summary>
    ''' If this item can be used in battle.
    ''' </summary>
    Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = gmCanBeUsedInBattle

    ''' <summary>
    ''' If this item can be tossed in the bag.
    ''' </summary>
    Public Overrides ReadOnly Property CanBeTossed As Boolean = gmCanBeTossed

    ''' <summary>
    ''' If this item requires the player to select a Pokémon to use the item on in battle.
    ''' </summary>
    Public Overrides ReadOnly Property BattleSelectPokemon As Boolean = gmBattleSelectPokemon

    ''' <summary>
    ''' If this item is a Healing item.
    ''' </summary>
    Public Overrides ReadOnly Property IsHealingItem As Boolean = gmIsHealingItem


    ''' <summary>
    ''' The item gets used from the bag.
    ''' </summary>
    Public Overrides Sub Use()
        Logger.Debug("PLACEHOLDER FOR GAMEMODE ITEM USE")
    End Sub


    ''' <summary>
    ''' A method that gets used when the item is applied to a Pokémon. Returns True if the action was successful.
    ''' </summary>
    ''' <param name="PokeIndex">The Index of the Pokémon in party.</param>
    Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Logger.Debug("PLACEHOLDER FOR GAMEMODE ITEM USE ON POKEMON")
        Return False
    End Function


End Class
