''' <summary>
''' A structure to store temporary data for a save state session.
''' </summary>
Public Class PlayerTemp

    ' Global game states:

    ''' <summary>
    ''' The current amount of steps required to complete the next Daycare cycle.
    ''' </summary>
    Public Property DayCareCycle As Integer = 256

    ''' <summary>
    ''' The steps since the last Pokégear call.
    ''' </summary>
    Public Property LastPokegearCall As Integer = 32

    ''' <summary>
    ''' The Item ID of the last used repel item.
    ''' </summary>
    Public Property LastUsedRepel As Integer = -1

    ''' <summary>
    ''' Amount of steps on the current map.
    ''' </summary>
    Public Property MapSteps As Integer = 0

    ''' <summary>
    ''' The currently playing radio station.
    ''' </summary>
    Public Property RadioStation As Decimal = 0D

    ' Player related:

    ''' <summary>
    ''' The last position of the player.
    ''' </summary>
    Public Property LastPosition As Vector3 = New Vector3()

    ''' <summary>
    ''' If the player is currently in battle.
    ''' </summary>
    Public Property IsInBattle As Boolean = False

    ' Saved overworld stats of the player for network usage:

    Public Property BeforeBattlePosition As Vector3 = New Vector3(0)

    Public Property BeforeBattleLevelFile As String = "yourroom.dat"

    Public Property BeforeBattleFacing As Integer = 0

    ' Screen menu states:

    ''' <summary>
    ''' The pokemon last selected in the <see cref="PartyScreen"/>.
    ''' </summary>
    Public Property PokemonScreenIndex As Integer = 0

    ''' <summary>
    ''' The last page selected in the <see cref="SummaryScreen"/>.
    ''' </summary>
    Public Property PokemonStatusPageIndex As Integer = 0

    ''' <summary>
    ''' The last tab selected in the <see cref="NewInventoryScreen"/>.
    ''' </summary>
    Public Property InventoryTabIndex As Integer = 0

    ''' <summary>
    ''' The selected item in the inventory.
    ''' </summary>
    Public Property InventoryItemIndicies As Integer() = {0, 0, 0, 0, 0, 0, 0}

    ''' <summary>
    ''' The selected item in the main menu.
    ''' </summary>
    Public Property MenuIndex As Integer = 0

    ''' <summary>
    ''' The index of the last used PC box.
    ''' </summary>
    Public Property PCBoxIndex As Integer = 0

    ''' <summary>
    ''' The last position of the cursor in the storage system.
    ''' </summary>
    Public Property StorageSystemCursorPosition As Vector2 = New Vector2(1, 0)

    ''' <summary>
    ''' The switches of the map screen that indicate which objects are displayed.
    ''' </summary>
    Public Property MapSwitch As Boolean() = {True, True, True, True}

    ''' <summary>
    ''' If the storage system is in box choose mode.
    ''' </summary>
    Public Property PCBoxChooseMode As Boolean = False

    ''' <summary>
    ''' The current selection mode for the storage system.
    ''' </summary>
    Public Property PCSelectionType As StorageSystemScreen.SelectionModes = StorageSystemScreen.SelectionModes.SingleMove

    ''' <summary>
    ''' The last page selected in the Pokégear.
    ''' </summary>
    Public Property PokegearPage As GameJolt.PokegearScreen.MenuScreens = GameJolt.PokegearScreen.MenuScreens.Main

End Class