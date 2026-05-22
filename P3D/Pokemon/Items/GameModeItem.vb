Imports System.Windows.Forms
Imports P3D.BattleSystem
Imports P3D.Items
Imports P3D.Screen

''' <summary>
''' An item the player stores in their inventory.
''' </summary>
Public Class GameModeItem

    Inherits Item

    Public gmPrice As Integer = 0
    Public gmBattlePointsPrice As Integer = 1
    Public gmItemType As ItemTypes = ItemTypes.Standard
    Public gmScriptPath As String = ""

    Public gmCatchMultiplier As Single = 1.0F
    Public gmMaxStack As Integer = 999
    Public gmFlingDamage As Integer = 30
    Public gmCanBeTraded As Boolean = True
    Public gmCanBeHeld As Boolean = True
    Public gmCanBeUsed As Boolean = True
    Public gmCanBeUsedInBattle As Boolean = True
    Public gmCanBeTossed As Boolean = True
    Public gmExpMultiplier As Double = -1D
    Public gmOverrideTradeExp As Boolean = False

    Public gmSortValue As Integer = 0

    'Medicine Item
    Public gmIsHealingItem As Boolean = False
    Public gmHealHPAmount As Integer = 0
    Public gmUseOnOwnEffects As List(Of String)
    Public gmUseOnOppEffects As List(Of String)
    Public gmBattleUseSound As String

    'Evolution Item
    Public gmEvolutionPokemon As List(Of String)

    'Machine Item
    Public gmIsHM As Boolean = False
    Public gmTeachMove As BattleSystem.Attack
    Public gmCanTeachAlways As Boolean = False
    Public gmCanTeachWhenFullyEvolved As Boolean = False
    Public gmCanTeachWhenGendered As Boolean = False

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
            If Localization.TokenExists("item_pluralname_" & gmID) = True Then
                Return Localization.GetString("item_pluralname_" & gmID)
            Else
                Return gmPluralName
            End If
        End Get
    End Property

    ''' <summary>
    ''' The price of this item if the player purchases it in exchange for PokéDollars. This halves when selling an item to the store.
    ''' </summary>
    Public Overrides ReadOnly Property PokeDollarPrice As Integer
        Get
            Return gmPrice
        End Get
    End Property

    ''' <summary>
    ''' The price of this item if the player purchases it exchange for BattlePoints.
    ''' </summary>
    Public Overrides ReadOnly Property BattlePointsPrice As Integer
        Get
            Return gmBattlePointsPrice
        End Get
    End Property

    ''' <summary>
    ''' The type of this item. This also controls in which bag this item gets sorted.
    ''' </summary>
    Public Overrides ReadOnly Property ItemType As ItemTypes
        Get
            Return gmItemType
        End Get
    End Property
    ''' <summary>
    ''' The default catch multiplier if the item gets used as a Pokéball.
    ''' </summary>
    Public Overrides ReadOnly Property CatchMultiplier As Single
        Get
            Return gmCatchMultiplier
        End Get
    End Property

    ''' <summary>
    ''' The maximum amount of this item type (per ID) that can be stored in the bag.
    ''' </summary>
    Public Overrides ReadOnly Property MaxStack As Integer
        Get
            Return gmMaxStack
        End Get
    End Property

    ''' <summary>
    ''' A value that can be used to sort items in the bag after. Lower values make items appear closer to the top.
    ''' </summary>
    Public Overrides ReadOnly Property SortValue As Integer
        Get
            Return gmSortValue
        End Get
    End Property


    ''' <summary>
    ''' The bag description of this item.
    ''' </summary>
    Public Overrides ReadOnly Property Description As String
        Get
            Return gmDescription
        End Get
    End Property


    ''' <summary>
    ''' The damage the Fling move does when this item is attached to a Pokémon.
    ''' </summary>
    Public Overrides ReadOnly Property FlingDamage As Integer
        Get
            Return gmFlingDamage
        End Get
    End Property

    ''' <summary>
    ''' If this item can be traded in for money.
    ''' </summary>
    Public Overrides ReadOnly Property CanBeTraded As Boolean
        Get
            Return gmCanBeTraded
        End Get
    End Property

    ''' <summary>
    ''' If this item can be given to a Pokémon.
    ''' </summary>
    Public Overrides ReadOnly Property CanBeHeld As Boolean
        Get
            Return gmCanBeHeld
        End Get
    End Property

    ''' <summary>
    ''' If this item can be used from the bag.
    ''' </summary>
    Public Overrides ReadOnly Property CanBeUsed As Boolean
        Get
            Return gmCanBeUsed
        End Get
    End Property

    ''' <summary>
    ''' If this item can be used in battle.
    ''' </summary>
    Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean
        Get
            Return gmCanBeUsedInBattle
        End Get
    End Property

    ''' <summary>
    ''' If this item can be tossed in the bag.
    ''' </summary>
    Public Overrides ReadOnly Property CanBeTossed As Boolean
        Get
            Return gmCanBeTossed
        End Get
    End Property

    ''' <summary>
    ''' If this item requires the player to select a Pokémon to use the item on in battle.
    ''' </summary>
    Public Overrides ReadOnly Property BattleSelectPokemon As Boolean
        Get
            Return gmBattleSelectPokemon
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Healing item.
    ''' </summary>
    Public Overrides ReadOnly Property IsHealingItem As Boolean
        Get
            Return gmIsHealingItem
        End Get
    End Property

    Public Sub SetTeachMoveTextureRectangle()
        Dim r As New Rectangle(144, 168, 24, 24)
        If gmTeachMove.Type.IsGameModeElement = False Then

            Select Case gmTeachMove.Type.Type
                Case Element.Types.Blank, Element.Types.Normal
                    r = New Rectangle(144, 168, 24, 24)
                Case Element.Types.Bug
                    r = New Rectangle(24, 192, 24, 24)
                Case Element.Types.Dark
                    r = New Rectangle(384, 168, 24, 24)
                Case Element.Types.Dragon
                    r = New Rectangle(408, 168, 24, 24)
                Case Element.Types.Electric
                    r = New Rectangle(288, 168, 24, 24)
                Case Element.Types.Fairy
                    r = New Rectangle(72, 264, 24, 24)
                Case Element.Types.Fighting
                    r = New Rectangle(168, 168, 24, 24)
                Case Element.Types.Fire
                    r = New Rectangle(360, 168, 24, 24)
                Case Element.Types.Flying
                    r = New Rectangle(0, 192, 24, 24)
                Case Element.Types.Ghost
                    r = New Rectangle(480, 168, 24, 24)
                Case Element.Types.Grass
                    r = New Rectangle(336, 168, 24, 24)
                Case Element.Types.Ground
                    r = New Rectangle(456, 168, 24, 24)
                Case Element.Types.Ice
                    r = New Rectangle(312, 168, 24, 24)
                Case Element.Types.Poison
                    r = New Rectangle(264, 168, 24, 24)
                Case Element.Types.Psychic
                    r = New Rectangle(216, 168, 24, 24)
                Case Element.Types.Rock
                    r = New Rectangle(240, 168, 24, 24)
                Case Element.Types.Steel
                    r = New Rectangle(432, 168, 24, 24)
                Case Element.Types.Water
                    r = New Rectangle(192, 168, 24, 24)
            End Select
        Else
            r = gmTeachMove.Type.gmMachineTextureRectangle
        End If

        gmTextureRectangle = r
    End Sub

    ''' <summary>
    ''' The item gets used from the bag.
    ''' </summary>
    Public Overrides Sub Use()
        Dim cScreen As Screen = Core.CurrentScreen
        While Not cScreen.PreScreen Is Nothing And cScreen.Identification <> Identifications.BattleScreen
            cScreen = cScreen.PreScreen
        End While

        If cScreen.Identification = Identifications.BattleScreen OrElse gmScriptPath = "" Then
            If cScreen.Identification <> Identifications.BattleScreen Then
                If IsMail = True Then
                    Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New MailSystemScreen(Core.CurrentScreen, Me.gmID), Color.Black, False))
                End If

                If gmTeachMove IsNot Nothing Then
                    If Core.Player.Pokemons.Count > 0 Then
                        SoundManager.PlaySound("PC\LogOn", False)
                        Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                        AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                        Core.SetScreen(selScreen)
                        CType(CurrentScreen, PartyScreen).SetupLearnAttack(gmTeachMove, 1, Me)
                    Else
                        Screen.TextBox.Show(Localization.GetString("item_cannot_use_NoPokemon", "You don't have any Pokémon."), {}, False, False)
                    End If
                End If
            End If
            If gmIsHealingItem = True Then
                If CBool(GameModeManager.GetGameRuleValue("CanUseHealItems", "1")) = False Then
                    Screen.TextBox.Show(Localization.GetString("item_cannot_use_HealingItems", "Cannot use healing items."), {}, False, False)
                    Exit Sub
                End If
                If cScreen.Identification <> Identifications.BattleScreen OrElse Me.gmBattleSelectPokemon = True Then
                    If Core.Player.Pokemons.Count > 0 Then
                        Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                        AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                        Core.SetScreen(selScreen)
                    Else
                        Screen.TextBox.Show(Localization.GetString("item_cannot_use_NoPokemon", "You don't have any Pokémon."), {}, False, False)
                    End If
                Else
                    If Core.Player.Pokemons.Count > 0 Then
                        Me.UseOnPokemon(CType(cScreen, BattleSystem.BattleScreen).OwnPokemonIndex)
                    End If
                End If
            ElseIf gmUseOnOwnEffects IsNot Nothing AndAlso gmUseOnOwnEffects.Count > 0 Then
                If cScreen.Identification <> Identifications.BattleScreen OrElse Me.gmBattleSelectPokemon = True Then
                    If Core.Player.Pokemons.Count > 0 Then
                        Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                        AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                        Core.SetScreen(selScreen)
                    Else
                        Screen.TextBox.Show(Localization.GetString("item_cannot_use_NoPokemon", "You don't have any Pokémon."), {}, False, False)
                    End If
                Else
                    If Core.Player.Pokemons.Count > 0 Then
                        Me.UseOnPokemon(CType(cScreen, BattleSystem.BattleScreen).OwnPokemonIndex)
                    End If
                End If
            ElseIf gmEvolutionPokemon IsNot Nothing AndAlso gmEvolutionPokemon.Count > 0 Then
                If Core.Player.Pokemons.Count > 0 Then
                    Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                    AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

                    Core.SetScreen(selScreen)
                    CType(CurrentScreen, PartyScreen).EvolutionItemID = Me.gmID

                Else
                    Screen.TextBox.Show(Localization.GetString("item_cannot_use_NoPokemon", "You don't have any Pokémon."), {}, False, False)
                End If
            End If
            If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Count > 0 Then
                If cScreen.Identification = Identifications.BattleScreen AndAlso Me.CanBeUsedInBattle = True Then
                    UseOnOppPokemon(CType(cScreen, BattleSystem.BattleScreen))
                End If
            End If
        Else
            Dim s As Screen = Core.CurrentScreen
            While Not s.PreScreen Is Nothing And s.Identification <> Screen.Identifications.OverworldScreen
                s = s.PreScreen
            End While

            If s.Identification = Screen.Identifications.OverworldScreen Then
                Core.SetScreen(s)
                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(gmScriptPath, 0, True, False)
                Exit Sub
            End If
        End If
    End Sub


    ''' <summary>
    ''' A method that gets used when the item is applied to a Pokémon. Returns True if the action was successful.
    ''' </summary>
    ''' <param name="PokeIndex">The Index of the Pokémon in party.</param>
    Public Overrides Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If
        Dim cScreen As Screen = Core.CurrentScreen
        While Not cScreen.PreScreen Is Nothing And cScreen.Identification <> Identifications.BattleScreen
            cScreen = cScreen.PreScreen
        End While

        Dim UseOnOppSuccess = False
        If cScreen.Identification = Identifications.BattleScreen Then
            If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Count > 0 Then
               UseOnOppSuccess = UseOnOppPokemon(CType(cScreen, BattleSystem.BattleScreen))
            End If
        End If
        Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

        If gmTeachMove IsNot Nothing Then

            Dim a As BattleSystem.Attack = gmTeachMove
            Dim t As String = CanTeach(p)

            If t = "" Then
                If p.Attacks.Count = 4 Then
                    SetScreen(New LearnAttackScreen(CurrentScreen, p, a, gmID))

                    Return True
                Else
                    Dim lastItemText As String = ""
                    If gmIsHM = False Then
                        If CBool(GameModeManager.GetGameRuleValue("SingleUseTM", "0")) = True Then
                            lastItemText = "*" & Me.RemoveItem()
                        End If
                    End If
                    p.Attacks.Add(BattleSystem.Attack.GetAttackByID(a.ID))

                    SoundManager.PlaySound("success_small", False)
                    Screen.TextBox.Show(Localization.GetString("learn_move_PokemonLearnedMove", "[POKEMONNAME] learned~[MOVENAME]!").Replace("[POKEMONNAME]", p.GetDisplayName()).Replace("[MOVENAME]", a.Name) & lastItemText, {}, False, False)
                    PlayerStatistics.Track("TMs/HMs used", 1)

                    Return True
                End If
            Else
                Screen.TextBox.Show(t, {}, False, False)

                Return False
            End If
        End If

        If p.Status = P3D.Pokemon.StatusProblems.Fainted AndAlso (gmUseOnOwnEffects Is Nothing OrElse gmUseOnOwnEffects.Count = 0 OrElse gmUseOnOwnEffects.Contains("fnt") = False) Then
            Screen.TextBox.reDelay = 0.0F
            Screen.TextBox.Show(Localization.GetString("item_cannot_use_IsFainted", "[POKEMONNAME]~is fainted!").Replace("[POKEMONNAME]", p.GetDisplayName()), {})
            Return False
        Else
            Dim healsuccess As Boolean = False
            If gmUseOnOwnEffects IsNot Nothing Then
                If gmUseOnOwnEffects.Count = 1 Then
                    If gmUseOnOwnEffects.Contains("heal") AndAlso gmHealHPAmount > 0 Then
                        Return HealPokemon(PokeIndex, gmHealHPAmount)
                    End If
                ElseIf gmUseOnOwnEffects.Contains("all") = False AndAlso gmUseOnOwnEffects.Contains("allwithoutfnt") = False Then
                    If gmUseOnOwnEffects.Contains("heal") AndAlso gmHealHPAmount > 0 Then
                        If HealPokemon(PokeIndex, gmHealHPAmount, True) Then
                            healsuccess = True
                        End If
                    End If
                End If

            End If
            If gmUseOnOwnEffects IsNot Nothing AndAlso gmUseOnOwnEffects.Count > 0 Then
                If gmUseOnOwnEffects.Contains("allwithoutfnt") Then
                    Dim success1 As Boolean = False
                    Dim success2 As Boolean = False
                    If gmUseOnOwnEffects.Contains("heal") AndAlso gmHealHPAmount > 0 Then
                        If HealPokemon(PokeIndex, gmHealHPAmount, True) = True Then
                            success1 = True
                        End If
                    End If
                    If p.Status <> Pokemon.StatusProblems.Fainted AndAlso p.Status <> Pokemon.StatusProblems.None Or p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                        If p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                            p.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)
                        End If
                        p.Status = Pokemon.StatusProblems.None
                        success2 = True
                    End If

                    Dim t As String = ""
                    If success1 = True AndAlso success2 = False Then
                        If UseOnOppSuccess = True And Me.gmUseOnOppEffects.Contains("heal") Then
                            t &= Localization.GetString("item_use_HealAndCureItem_Heal_Both", "Healed [OWNPOKEMONNAME] and~[OPPPOKEMONNAME]!").Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else

                            t &= Localization.GetString("item_use_HealAndCureItem_Heal_Single", "Healed [POKEMONNAME]!").Replace("[POKEMONNAME]", p.GetDisplayName())
                        End If
                    End If
                    If success1 = False AndAlso success2 = True Then
                        If UseOnOppSuccess = True And Me.gmUseOnOppEffects.Contains("heal") = False Then
                            t &= Localization.GetString("item_use_HealAndCureItem_Cure_Both", "Cured [OWNPOKEMONNAME] and~[OPPPOKEMONNAME]!").Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else
                            t &= Localization.GetString("item_use_HealAndCureItem_Cure_Single", "Cured [POKEMONNAME]!").Replace("[POKEMONNAME]", p.GetDisplayName())
                        End If
                    End If
                    If success1 = True AndAlso success2 = True Then
                        If UseOnOppSuccess = True And Me.gmUseOnOppEffects.Contains("heal") Then
                            t &= Localization.GetString("item_use_HealAndCureItem_HealAndCure_Both", "Healed and cured~[OWNPOKEMONNAME] and~[OPPPOKEMONNAME]!").Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else
                            t &= Localization.GetString("item_use_HealAndCureItem_HealAndCure_Single", "Healed and cured~[POKEMONNAME]!").Replace("[POKEMONNAME]", p.GetDisplayName())
                        End If
                    End If

                    If success1 = True Or success2 = True Then
                        Screen.TextBox.reDelay = 0.0F

                        If Me.gmItemType <> ItemTypes.KeyItems Then
                            t &= RemoveItem()
                        End If
                        PlayerStatistics.Track("[17]Medicine Items used", 1)

                        If gmBattleUseSound <> "" Then
                            SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                        Else
                            SoundManager.PlaySound("Use_Item", False)
                        End If
                        Screen.TextBox.Show(t, {})

                        Return True
                    Else
                        Screen.TextBox.Show(Localization.GetString("item_cannot_use_OnPokemon_Single", "Cannot use [ITEMNAME]~on [POKEMONNAME].").Replace("[ITEMNAME]", Me.Name).Replace("[POKEMONNAME]", p.GetDisplayName()), {}, False, False)
                        Return False
                    End If
                ElseIf gmUseOnOwnEffects.Contains("all") Then
                    Dim success1 As Boolean = False
                    Dim success2 As Boolean = False
                    If gmUseOnOwnEffects.Contains("heal") AndAlso gmHealHPAmount > 0 Then
                        If HealPokemon(PokeIndex, gmHealHPAmount, True) = True Then
                            success1 = True
                        End If
                    End If
                    If p.Status <> Pokemon.StatusProblems.None Or p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                        If p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                            p.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)
                        End If
                        p.Status = Pokemon.StatusProblems.None
                        success2 = True
                    End If

                    Dim t As String = ""
                    If success1 = True AndAlso success2 = False Then
                        If UseOnOppSuccess = True And Me.gmUseOnOppEffects.Contains("heal") Then
                            t &= Localization.GetString("item_use_HealAndCureItem_Heal_Both", "Healed [OWNPOKEMONNAME] and~[OPPPOKEMONNAME]!").Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else

                            t &= Localization.GetString("item_use_HealAndCureItem_Heal_Single", "Healed [POKEMONNAME]!").Replace("[POKEMONNAME]", p.GetDisplayName())
                        End If
                    End If
                    If success1 = False AndAlso success2 = True Then
                        If UseOnOppSuccess = True And Me.gmUseOnOppEffects.Contains("heal") = False Then
                            t &= Localization.GetString("item_use_HealAndCureItem_Cure_Both", "Cured [OWNPOKEMONNAME] and~[OPPPOKEMONNAME]!").Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else
                            t &= Localization.GetString("item_use_HealAndCureItem_Cure_Single", "Cured [POKEMONNAME]!").Replace("[POKEMONNAME]", p.GetDisplayName())
                        End If
                    End If
                    If success1 = True AndAlso success2 = True Then
                        If UseOnOppSuccess = True And Me.gmUseOnOppEffects.Contains("heal") Then
                            t &= Localization.GetString("item_use_HealAndCureItem_HealAndCure_Both", "Healed and cured~[OWNPOKEMONNAME] and~[OPPPOKEMONNAME]!").Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else
                            t &= Localization.GetString("item_use_HealAndCureItem_HealAndCure_Single", "Healed and cured~[POKEMONNAME]!").Replace("[POKEMONNAME]", p.GetDisplayName())
                        End If
                    End If

                    If success1 = True Or success2 = True Then
                        Screen.TextBox.reDelay = 0.0F

                        If Me.gmItemType <> ItemTypes.KeyItems Then
                            t &= RemoveItem()
                        End If
                        PlayerStatistics.Track("[17]Medicine Items used", 1)

                        If gmBattleUseSound <> "" Then
                            SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                        Else
                            SoundManager.PlaySound("Use_Item", False)
                        End If
                        Screen.TextBox.Show(t, {})

                        Return True
                    Else
                        If UseOnOppSuccess = False AndAlso Me.gmUseOnOppEffects IsNot Nothing AndAlso cScreen.Identification = Identifications.BattleScreen Then
                            Screen.TextBox.Show(Localization.GetString("item_cannot_use_OnPokemon_Both", "Cannot use [ITEMNAME]~on [OWNPOKEMONNAME]~or [OPPPOKEMONNAME].").Replace("[ITEMNAME]", Me.Name).Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName()), {}, False, False)
                        Else
                            Screen.TextBox.Show(Localization.GetString("item_cannot_use_OnPokemon_Single", "Cannot use [ITEMNAME]~on [POKEMONNAME].").Replace("[ITEMNAME]", Me.Name).Replace("[POKEMONNAME]", p.GetDisplayName()), {}, False, False)
                        End If

                        Return False
                    End If
                Else
                    If gmUseOnOwnEffects.Count = 1 Then
                        If gmUseOnOwnEffects.Contains("brn") Then
                            Return HealBurn(PokeIndex,, UseOnOppSuccess)
                        End If
                        If gmUseOnOwnEffects.Contains("frz") Then
                            Return HealIce(PokeIndex,, UseOnOppSuccess)
                        End If
                        If gmUseOnOwnEffects.Contains("prz") Then
                            Return HealParalyze(PokeIndex,, UseOnOppSuccess)
                        End If
                        If gmUseOnOwnEffects.Contains("psn") OrElse gmUseOnOwnEffects.Contains("bpsn") Then
                            Return CurePoison(PokeIndex,, UseOnOppSuccess)
                        End If
                        If gmUseOnOwnEffects.Contains("slp") Then
                            Return WakeUp(PokeIndex,, UseOnOppSuccess)
                        End If
                        If gmUseOnOwnEffects.Contains("confusion") Then
                            Return CureConfusion(PokeIndex,, UseOnOppSuccess)
                        End If
                        If gmUseOnOwnEffects.Contains("fnt") Then
                            Return Revive(PokeIndex)
                        End If
                    Else
                        Dim success As Boolean = False
                        If gmUseOnOwnEffects.Contains("brn") Then
                            If HealBurn(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmUseOnOwnEffects.Contains("frz") Then
                            If HealIce(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmUseOnOwnEffects.Contains("prz") Then
                            If HealParalyze(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmUseOnOwnEffects.Contains("psn") OrElse gmUseOnOwnEffects.Contains("bpsn") Then
                            If CurePoison(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmUseOnOwnEffects.Contains("slp") Then
                            If WakeUp(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmUseOnOwnEffects.Contains("confusion") Then
                            If CureConfusion(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmUseOnOwnEffects.Contains("fnt") Then
                            If Revive(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        Dim t As String = ""
                        If healsuccess = True AndAlso success = False Then
                            If UseOnOppSuccess = True And Me.gmUseOnOppEffects.Contains("heal") Then
                                t &= Localization.GetString("item_use_HealAndCureItem_Heal_Both", "Healed [OWNPOKEMONNAME] and~[OPPPOKEMONNAME]!").Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                            Else

                                t &= Localization.GetString("item_use_HealAndCureItem_Heal_Single", "Healed [POKEMONNAME]!").Replace("[POKEMONNAME]", p.GetDisplayName())
                            End If
                        End If
                        If healsuccess = False AndAlso success = True Then
                            If UseOnOppSuccess = True And Me.gmUseOnOppEffects.Contains("heal") = False Then
                                t &= Localization.GetString("item_use_HealAndCureItem_Cure_Both", "Cured [OWNPOKEMONNAME] and~[OPPPOKEMONNAME]!").Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                            Else
                                t &= Localization.GetString("item_use_HealAndCureItem_Cure_Single", "Cured [POKEMONNAME]!").Replace("[POKEMONNAME]", p.GetDisplayName())
                            End If
                        End If
                        If healsuccess = True AndAlso success = True Then
                            If UseOnOppSuccess = True And Me.gmUseOnOppEffects.Contains("heal") Then
                                t &= Localization.GetString("item_use_HealAndCureItem_HealAndCure_Both", "Healed and cured~[OWNPOKEMONNAME] and~[OPPPOKEMONNAME]!").Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                            Else
                                t &= Localization.GetString("item_use_HealAndCureItem_HealAndCure_Single", "Healed and cured~[POKEMONNAME]!").Replace("[POKEMONNAME]", p.GetDisplayName())
                            End If
                        End If

                        If healsuccess = True Or success = True Then
                            Screen.TextBox.reDelay = 0.0F

                            If Me.gmItemType <> ItemTypes.KeyItems Then
                                t &= RemoveItem()
                            End If
                            PlayerStatistics.Track("[17]Medicine Items used", 1)
                            If gmBattleUseSound <> "" Then
                                SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                            Else
                                SoundManager.PlaySound("Use_Item", False)
                            End If
                            Screen.TextBox.Show(t, {})

                            Return True
                        Else
                            If UseOnOppSuccess = False AndAlso Me.gmUseOnOppEffects IsNot Nothing AndAlso cScreen.Identification = Identifications.BattleScreen Then
                                Screen.TextBox.Show(Localization.GetString("item_cannot_use_OnPokemon_Both", "Cannot use [ITEMNAME]~on [OWNPOKEMONNAME]~or [OPPPOKEMONNAME].").Replace("[ITEMNAME]", Me.Name).Replace("[OWNPOKEMONNAME]", p.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName()), {}, False, False)
                            Else
                                Screen.TextBox.Show(Localization.GetString("item_cannot_use_OnPokemon_Single", "Cannot use [ITEMNAME]~on [POKEMONNAME].").Replace("[ITEMNAME]", Me.Name).Replace("[POKEMONNAME]", p.GetDisplayName()), {}, False, False)
                            End If
                            Return False
                        End If
                    End If
                End If
            End If
        End If
        If gmEvolutionPokemon IsNot Nothing AndAlso gmEvolutionPokemon.Count > 0 Then
            Return Me.UseEvolutionItem(PokeIndex)
        End If

        Return False
    End Function
    Public Function UseOnOppPokemon(bScreen As BattleSystem.BattleScreen) As Boolean

        Dim healsuccess As Boolean = False
        If gmUseOnOppEffects Is Nothing OrElse gmUseOnOppEffects.Count = 1 Then
            If gmUseOnOwnEffects.Contains("heal") AndAlso gmHealHPAmount > 0 Then
                Return HealPokemon(-1, gmHealHPAmount,,, bScreen.OppPokemon)
            End If
        ElseIf gmUseOnOppEffects.Contains("allstatus") = False Then
            If gmUseOnOwnEffects.Contains("heal") AndAlso gmHealHPAmount > 0 Then
                If HealPokemon(-1, gmHealHPAmount, True, , bScreen.OppPokemon) Then
                    healsuccess = True
                End If
            End If
        End If

        If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Count > 0 Then
            If gmUseOnOppEffects.Contains("allstatus") Then
                Dim success1 As Boolean = False
                Dim success2 As Boolean = False
                If gmUseOnOwnEffects.Contains("heal") AndAlso gmHealHPAmount > 0 Then
                    If HealPokemon(-1, gmHealHPAmount, True,, bScreen.OppPokemon) = True Then
                        success1 = True
                    End If
                End If
                If bScreen.OppPokemon.Status <> Pokemon.StatusProblems.Fainted AndAlso bScreen.OppPokemon.Status <> Pokemon.StatusProblems.None Or bScreen.OppPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                    If bScreen.OppPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                        bScreen.OppPokemon.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)
                    End If
                    bScreen.OppPokemon.Status = Pokemon.StatusProblems.None
                    success2 = True
                End If

                Dim t As String = ""
                If success1 = True AndAlso success2 = False Then
                    t &= Localization.GetString("item_use_HealAndCureItem_Heal_Single", "Healed [POKEMONNAME]!").Replace("[POKEMONNAME]", bScreen.OppPokemon.GetDisplayName())
                End If
                If success1 = False AndAlso success2 = True Then
                    t &= Localization.GetString("item_use_HealAndCureItem_Cure_Single", "Cured [POKEMONNAME]!").Replace("[POKEMONNAME]", bScreen.OppPokemon.GetDisplayName())
                End If
                If success1 = True AndAlso success2 = True Then
                    t &= Localization.GetString("item_use_HealAndCureItem_HealAndCure_Single", "Healed and cured~[POKEMONNAME]!").Replace("[POKEMONNAME]", bScreen.OppPokemon.GetDisplayName())
                End If

                If success1 = True Or success2 = True Then
                    Screen.TextBox.reDelay = 0.0F

                    If Me.gmItemType <> ItemTypes.KeyItems AndAlso Me.gmUseOnOwnEffects Is Nothing Then
                        t &= RemoveItem()
                    End If
                    If gmUseOnOwnEffects Is Nothing Then
                        PlayerStatistics.Track("[17]Medicine Items used", 1)
                        If gmUseOnOwnEffects Is Nothing AndAlso gmBattleUseSound <> "" Then
                            SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                        Else
                            SoundManager.PlaySound("Use_Item", False)
                        End If
                    End If

                    Screen.TextBox.Show(t, {})

                    Return True
                Else
                    If gmUseOnOwnEffects Is Nothing Then
                        Screen.TextBox.Show(Localization.GetString("item_cannot_use_OnPokemon_Single", "Cannot use [ITEMNAME]~on [POKEMONNAME].").Replace("[ITEMNAME]", Me.Name).Replace("[POKEMONNAME]", bScreen.OppPokemon.GetDisplayName()), {}, False, False)
                    End If
                    Return False
                End If
            Else
                If gmUseOnOppEffects.Count = 1 Then
                    If gmUseOnOppEffects.Contains("brn") Then
                        Return HealBurn(-1,,, bScreen.OppPokemon)
                    End If
                    If gmUseOnOppEffects.Contains("frz") Then
                        Return HealIce(-1,,, bScreen.OppPokemon)
                    End If
                    If gmUseOnOppEffects.Contains("prz") Then
                        Return HealParalyze(-1,,, bScreen.OppPokemon)
                    End If
                    If gmUseOnOppEffects.Contains("psn") OrElse gmUseOnOppEffects.Contains("bpsn") Then
                        Return CurePoison(-1,,, bScreen.OppPokemon)
                    End If
                    If gmUseOnOppEffects.Contains("slp") Then
                        Return WakeUp(-1,,, bScreen.OppPokemon)
                    End If
                    If gmUseOnOppEffects.Contains("confusion") Then
                        Return CureConfusion(-1,,, bScreen.OppPokemon)
                    End If
                Else
                    Dim success As Boolean = False
                    If gmUseOnOppEffects.Contains("brn") Then
                        If HealBurn(-1, True,, bScreen.OppPokemon) Then
                            success = True
                        End If
                    End If
                    If gmUseOnOppEffects.Contains("frz") Then
                        If HealIce(-1, True,, bScreen.OppPokemon) Then
                            success = True
                        End If
                    End If
                    If gmUseOnOppEffects.Contains("prz") Then
                        If HealParalyze(-1, True,, bScreen.OppPokemon) Then
                            success = True
                        End If
                    End If
                    If gmUseOnOppEffects.Contains("psn") OrElse gmUseOnOppEffects.Contains("bpsn") Then
                        If CurePoison(-1, True,, bScreen.OppPokemon) Then
                            success = True
                        End If
                    End If
                    If gmUseOnOppEffects.Contains("slp") Then
                        If WakeUp(-1, True,, bScreen.OppPokemon) Then
                            success = True
                        End If
                    End If
                    If gmUseOnOppEffects.Contains("confusion") Then
                        If CureConfusion(-1, True,, bScreen.OppPokemon) Then
                            success = True
                        End If
                    End If
                    Dim t As String = ""
                    If healsuccess = True AndAlso success = False Then
                        t &= Localization.GetString("item_use_HealAndCureItem_Heal_Single", "Healed [POKEMONNAME]!").Replace("[POKEMONNAME]", bScreen.OppPokemon.GetDisplayName())
                    End If
                    If healsuccess = False AndAlso success = True Then
                        t &= Localization.GetString("item_use_HealAndCureItem_Cure_Single", "Cured [POKEMONNAME]!").Replace("[POKEMONNAME]", bScreen.OppPokemon.GetDisplayName())
                    End If
                    If healsuccess = True AndAlso success = True Then
                        t &= Localization.GetString("item_use_HealAndCureItem_HealAndCure_Single", "Healed and cured~[POKEMONNAME]!").Replace("[POKEMONNAME]", bScreen.OppPokemon.GetDisplayName())
                    End If

                    If healsuccess = True Or success = True Then
                        Screen.TextBox.reDelay = 0.0F

                        If Me.gmItemType <> ItemTypes.KeyItems Then
                            t &= RemoveItem()
                        End If

                        If gmUseOnOwnEffects Is Nothing Then
                            PlayerStatistics.Track("[17]Medicine Items used", 1)
                        End If
                        If gmUseOnOwnEffects Is Nothing AndAlso gmBattleUseSound <> "" Then
                            SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                        Else
                            SoundManager.PlaySound("Use_Item", False)
                        End If
                        Screen.TextBox.Show(t, {})

                        Return True
                    Else
                        If gmUseOnOwnEffects Is Nothing Then
                            Screen.TextBox.Show(Localization.GetString("item_cannot_use_OnPokemon_Single", "Cannot use [ITEMNAME]~on [POKEMONNAME].").Replace("[ITEMNAME]", Me.Name).Replace("[POKEMONNAME]", bScreen.OppPokemon.GetDisplayName()), {}, False, False)
                        End If

                        Return False
                    End If
                End If
            End If
        End If

        Return False
    End Function

    Public Function CanTeach(ByVal p As Pokemon) As String
        If p.IsEgg() = True Then
            Return Localization.GetString("learn_move_EggCannotLearn", "Egg cannot learn~[MOVENAME]!").Replace("[MOVENAME]", gmTeachMove.Name)
        End If

        For Each knowAttack As BattleSystem.Attack In p.Attacks
            If knowAttack.ID = gmTeachMove.ID Then
                Return Localization.GetString("learn_move_AlreadyKnowsTheMove", "[POKEMONNAME] already~knows [MOVENAME].").Replace("[POKEMONNAME]", p.GetDisplayName()).Replace("[MOVENAME]", gmTeachMove.Name)
            End If
        Next

        If p.Machines.Contains(gmTeachMove.ID) = True Then
            Return ""
        End If

        For Each aList As List(Of BattleSystem.Attack) In p.AttackLearns.Values
            For Each learnAttack As BattleSystem.Attack In aList
                If learnAttack.ID = gmTeachMove.ID Then
                    Return ""
                End If
            Next
        Next

        If gmCanTeachAlways = True Then
            If p.Machines.Count > 0 Then
                Return ""
            End If
        End If

        If gmCanTeachWhenFullyEvolved = True Then
            If p.IsFullyEvolved() = True And p.Machines.Count > 0 Then
                Return ""
            End If
        End If

        If gmCanTeachWhenGendered = True Then
            If p.Gender <> Pokemon.Genders.Genderless And p.Machines.Count > 0 Then
                Return ""
            End If
        End If

        If p.CanLearnAllMachines = True Then
            Return ""
        End If

        Return Localization.GetString("learn_move_PokemonCannotLearn", "[POKEMONNAME] cannot learn~[MOVENAME]!").Replace("[POKEMONNAME]", p.GetDisplayName()).Replace("[MOVENAME]", gmTeachMove.Name)
    End Function

    Public Function UseEvolutionItem(ByVal PokeIndex As Integer) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)
        If p.IsEgg() = False And p.CanEvolve(EvolutionCondition.EvolutionTrigger.ItemUse, Me.gmID) = True Then

            If Me.gmItemType <> ItemTypes.KeyItems Then
                RemoveItem()
            End If

            Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New EvolutionScreen(Core.CurrentScreen, {PokeIndex}.ToList(), Me.gmID, EvolutionCondition.EvolutionTrigger.ItemUse), Color.Black, False))
            Return True
        Else
            Screen.TextBox.Show(Localization.GetString("item_cannot_use_OnPokemon_Single", "Cannot use [ITEMNAME]~on [POKEMONNAME].").Replace("[ITEMNAME]", Me.Name).Replace("[POKEMONNAME]", p.GetDisplayName()), {}, False, False)

            Return False
        End If
    End Function

    ''' <summary>
    ''' Tries to heal a Pokémon from the player's party. If this succeeds, the method returns True.
    ''' </summary>
    ''' <param name="PokeIndex">The index of the Pokémon in the player's party.</param>
    ''' <param name="HP">The HP that should be healed.</param>
    Public Function HealPokemon(ByVal PokeIndex As Integer, ByVal HP As Integer, Optional NoTextOrSound As Boolean = False, Optional UseOppSuccess As Boolean = False, Optional ByVal p As Pokemon = Nothing) As Boolean
        If p Is Nothing Then
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If
        End If
        Dim cScreen As Screen = Core.CurrentScreen
        While Not cScreen.PreScreen Is Nothing And cScreen.Identification <> Identifications.BattleScreen
            cScreen = cScreen.PreScreen
        End While


        Dim Pokemon As Pokemon
        If p Is Nothing Then
            Pokemon = Core.Player.Pokemons(PokeIndex)
        Else
            Pokemon = p
        End If

        If HP < 0 Then
            HP = CInt(Pokemon.MaxHP / (100 / (HP * (-1))))
        End If

        If Pokemon.Status = Pokemon.StatusProblems.Fainted Then
            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F
                If UseOppSuccess = True Then
                    If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("heal") Then
                        If p Is Nothing Then
                            If gmBattleUseSound <> "" Then
                                SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                            Else
                                SoundManager.PlaySound("Use_Item", False)
                            End If
                            Screen.TextBox.Show(Localization.GetString("item_use_HealItem_Opponent", "Restored [POKEMONNAME]'s~HP.").Replace("[POKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName()), {})
                            Return True
                        End If
                    Else
                        Screen.TextBox.Show(Localization.GetString("item_cannot_use_IsFainted", "[POKEMONNAME]~is fainted!").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()), {})
                    End If
                Else
                    Screen.TextBox.Show(Localization.GetString("item_cannot_use_IsFainted", "[POKEMONNAME]~is fainted!").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()), {})
                End If
            End If
            Return False
        Else
            If Pokemon.HP = Pokemon.MaxHP Then
                If NoTextOrSound = False Then
                    Screen.TextBox.reDelay = 0.0F
                    If cScreen.Identification = Identifications.BattleScreen Then
                        If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("heal") Then
                            If p Is Nothing Then
                                If UseOppSuccess = True Then
                                    If gmBattleUseSound <> "" Then
                                        SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                                    Else
                                        SoundManager.PlaySound("Use_Item", False)
                                    End If
                                    Screen.TextBox.Show(Localization.GetString("item_use_HealItem_Opponent", "Restored [POKEMONNAME]'s~HP.").Replace("[POKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName()), {})
                                    Return True
                                Else
                                    Screen.TextBox.Show(Localization.GetString("item_cannot_use_FullHP_Both", "[OWNPOKEMONNAME] and~[OPPPOKEMONNAME] have full~HP already.").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName()), {})
                                End If
                            Else
                                Screen.TextBox.Show(Localization.GetString("item_cannot_use_FullHP_Single", "[POKEMONNAME] has full~HP already.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()), {})
                            End If
                        Else
                            Screen.TextBox.Show(Localization.GetString("item_cannot_use_FullHP_Single", "[POKEMONNAME] has full~HP already.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()), {})
                        End If
                    Else
                        Screen.TextBox.Show(Localization.GetString("item_cannot_use_FullHP_Single", "[POKEMONNAME] has full~HP already.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()), {})
                    End If

                End If
                Return False
            Else
                Dim diff As Integer = Pokemon.MaxHP - Pokemon.HP
                diff = CInt(MathHelper.Clamp(diff, 1, HP))

                Pokemon.Heal(HP)

                If NoTextOrSound = False Then
                    Screen.TextBox.reDelay = 0.0F
                    Dim t As String = ""
                    If cScreen.Identification = Identifications.BattleScreen Then
                        If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("heal") Then
                            If UseOppSuccess = True AndAlso p Is Nothing Then
                                t = Localization.GetString("item_use_HealItem_Both", "Restored [OWNPOKEMONNAME] and~[OPPPOKEMONNAME]'s HP.").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                            Else
                                t = Localization.GetString("item_use_HealItem_Own", "Restored [POKEMONNAME]'s~HP by [HPAMOUNT].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()).Replace("[HPAMOUNT]", diff.ToString)
                            End If
                        Else
                            t = Localization.GetString("item_use_HealItem_Own", "Restored [POKEMONNAME]'s~HP by [HPAMOUNT].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()).Replace("[HPAMOUNT]", diff.ToString)
                        End If
                    Else
                        t = Localization.GetString("item_use_HealItem_Own", "Restored [POKEMONNAME]'s~HP by [HPAMOUNT].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()).Replace("[HPAMOUNT]", diff.ToString)
                    End If


                    If Me.gmItemType <> ItemTypes.KeyItems Then
                        If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                            t &= RemoveItem()
                        End If

                    End If
                    If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                        If gmBattleUseSound <> "" Then
                            SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                        Else
                            SoundManager.PlaySound("Use_Item", False)
                        End If
                    End If
                    Screen.TextBox.Show(t, {})
                    End If
                    If gmUseOnOwnEffects IsNot Nothing And gmUseOnOppEffects Is Nothing OrElse gmUseOnOwnEffects Is Nothing And gmUseOnOppEffects IsNot Nothing Then
                    PlayerStatistics.Track("[17]Medicine Items used", 1)
                End If
                Return True
            End If
        End If
        Return True
    End Function
    ''' <summary>
    ''' Revives a Pokemon.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function Revive(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False) As Boolean

        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Fainted Then

            Pokemon.Status = P3D.Pokemon.StatusProblems.None
            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                Dim t As String = Localization.GetString("item_use_RevivalItem", "[POKEMONNAME]~is revitalized.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())

                If Me.gmItemType <> ItemTypes.KeyItems Then
                    t &= RemoveItem()
                End If
                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show(t, {}, False, False)
            End If
            PlayerStatistics.Track("[17]Medicine Items used", 1)
            Return True
        Else
            If NoTextOrSound = False Then
                Screen.TextBox.Show(Localization.GetString("item_cannot_use_OnPokemon_Single", "Cannot use [ITEMNAME]~on [POKEMONNAME].").Replace("[ITEMNAME]", Me.Name).Replace("[POKEMONNAME]", Pokemon.GetDisplayName()), {}, False, False)
            End If
            Return False
        End If

    End Function
    ''' <summary>
    ''' Tries to cure a Pokémon from Poison.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function CurePoison(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False, Optional UseOppSuccess As Boolean = False, Optional ByVal p As Pokemon = Nothing) As Boolean
        If p Is Nothing Then
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If
        End If

        Dim Pokemon As Pokemon
        If p Is Nothing Then
            Pokemon = Core.Player.Pokemons(PokeIndex)
        Else
            Pokemon = p
        End If

        Dim cScreen As Screen = Core.CurrentScreen
        While Not cScreen.PreScreen Is Nothing And cScreen.Identification <> Identifications.BattleScreen
            cScreen = cScreen.PreScreen
        End While

        Dim t As String

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Poison Or Pokemon.Status = P3D.Pokemon.StatusProblems.BadPoison Then
            Pokemon.Status = P3D.Pokemon.StatusProblems.None

            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                If cScreen.Identification = Identifications.BattleScreen Then
                    If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("psn") Then
                        If UseOppSuccess = True AndAlso p Is Nothing Then
                            t = Localization.GetString("item_use_CurePoison_Both", "Cured the poison~of [OWNPOKEMONNAME] and~[OPPPOKEMONNAME].").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else
                            t = Localization.GetString("item_use_CurePoison_Single", "Cured the poison~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                        End If
                    Else
                        t = Localization.GetString("item_use_CurePoison_Single", "Cured the poison~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_use_CurePoison_Single", "Cured the poison~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If

                If Me.gmItemType <> ItemTypes.KeyItems Then
                    If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                        t &= RemoveItem()
                    End If
                End If

                If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                    If gmBattleUseSound <> "" Then
                        SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                    Else
                        SoundManager.PlaySound("Use_Item", False)
                    End If
                End If
                Screen.TextBox.Show(t, {})
            End If
            If gmUseOnOwnEffects IsNot Nothing And gmUseOnOppEffects Is Nothing OrElse gmUseOnOwnEffects Is Nothing And gmUseOnOppEffects IsNot Nothing Then
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If
            Return True
        ElseIf NoTextOrSound = False Then
            Screen.TextBox.reDelay = 0.0F
            If cScreen.Identification = Identifications.BattleScreen Then
                If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("psn") Then
                    If UseOppSuccess = True Then
                        If gmBattleUseSound <> "" Then
                            SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                        Else
                            SoundManager.PlaySound("Use_Item", False)
                        End If
                        t = Localization.GetString("item_use_CurePoison_Single", "Cured the poison~of [POKEMONNAME].").Replace("[POKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        PlayerStatistics.Track("[17]Medicine Items used", 1)
                        Return True
                    Else
                        t = Localization.GetString("item_cannot_use_NotPoisoned_Both", "[OWNPOKEMONNAME] and~[OPPPOKEMONNAME] are not~poisoned.").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_cannot_use_NotPoisoned_Single", "[POKEMONNAME] is not~poisoned.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If
            Else
                t = Localization.GetString("item_cannot_use_NotPoisoned_Single", "[POKEMONNAME] is not~poisoned.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
            End If
            Screen.TextBox.Show(t, {})

        End If
        Return False

    End Function
    ''' <summary>
    ''' Tries to cure a Pokémon from Confusion.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function CureConfusion(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False, Optional UseOppSuccess As Boolean = False, Optional ByVal p As Pokemon = Nothing) As Boolean
        If p Is Nothing Then
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If
        End If

        Dim Pokemon As Pokemon
        If p Is Nothing Then
            Pokemon = Core.Player.Pokemons(PokeIndex)
        Else
            Pokemon = p
        End If

        Dim cScreen As Screen = Core.CurrentScreen
        While Not cScreen.PreScreen Is Nothing And cScreen.Identification <> Identifications.BattleScreen
            cScreen = cScreen.PreScreen
        End While

        Dim t As String

        If Pokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
            Pokemon.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)

            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F
                If cScreen.Identification = Identifications.BattleScreen Then
                    If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("psn") Then
                        If UseOppSuccess = True AndAlso p Is Nothing Then
                            t = Localization.GetString("item_use_CureConfusion_Both", "[OWNPOKEMONNAME] and~[OPPPOKEMONNAME]~are no longer confused.").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else
                            t = Localization.GetString("item_use_CureConfusion_Single", "[POKEMONNAME]~is no longer confused.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                        End If
                    Else
                        t = Localization.GetString("item_use_CureConfusion_Single", "[POKEMONNAME]~is no longer confused.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_use_CureConfusion_Single", "[POKEMONNAME]~is no longer confused.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If


                If Me.gmItemType <> ItemTypes.KeyItems Then
                    If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                        t &= RemoveItem()
                    End If
                End If
                If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                    If gmBattleUseSound <> "" Then
                        SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                    Else
                        SoundManager.PlaySound("Use_Item", False)
                    End If
                End If
                Screen.TextBox.Show(t, {}, False, False)
            End If
            If gmUseOnOwnEffects IsNot Nothing And gmUseOnOppEffects Is Nothing OrElse gmUseOnOwnEffects Is Nothing And gmUseOnOppEffects IsNot Nothing Then
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If
            Return True
        Else
            If NoTextOrSound = False Then
                If cScreen.Identification = Identifications.BattleScreen Then
                    If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("confusion") Then
                        If UseOppSuccess = True Then
                            If gmBattleUseSound <> "" Then
                                SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                            Else
                                SoundManager.PlaySound("Use_Item", False)
                            End If
                            t = Localization.GetString("item_use_CureConfusion_Single", "[POKEMONNAME]~is no longer confused.").Replace("[POKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                            PlayerStatistics.Track("[17]Medicine Items used", 1)
                            Return True
                        Else
                            t = Localization.GetString("item_cannot_use_NotConfused_Both", "[OWNPOKEMONNAME] and~[OPPPOKEMONNAME] are not~confused.").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        End If
                    Else
                        t = Localization.GetString("item_cannot_use_NotConfused_Single", "[POKEMONNAME] is not~confused.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_cannot_use_NotConfused_Single", "[POKEMONNAME] is not~confused.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If
                Screen.TextBox.Show(t, {}, False, False)
            End If
            Return False
        End If

    End Function

    ''' <summary>
    ''' Tries to wake a Pokémon up from Sleep.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function WakeUp(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False, Optional UseOppSuccess As Boolean = False, Optional ByVal p As Pokemon = Nothing) As Boolean
        If p Is Nothing Then
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If
        End If

        Dim Pokemon As Pokemon
        If p Is Nothing Then
            Pokemon = Core.Player.Pokemons(PokeIndex)
        Else
            Pokemon = p
        End If

        Dim cScreen As Screen = Core.CurrentScreen
        While Not cScreen.PreScreen Is Nothing And cScreen.Identification <> Identifications.BattleScreen
            cScreen = cScreen.PreScreen
        End While

        Dim t As String

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Sleep Then
            Pokemon.Status = P3D.Pokemon.StatusProblems.None

            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                If cScreen.Identification = Identifications.BattleScreen Then
                    If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("slp") Then
                        If UseOppSuccess = True AndAlso p Is Nothing Then
                            t = Localization.GetString("item_use_CureSleep_Both", "Cured the sleep~of [OWNPOKEMONNAME] and~[OPPPOKEMONNAME].").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else
                            t = Localization.GetString("item_use_CureSleep_Single", "Cured the sleep~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                        End If
                    Else
                        t = Localization.GetString("item_use_CureSleep_Single", "Cured the sleep~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_use_CureSleep_Single", "Cured the sleep~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If

                If Me.gmItemType <> ItemTypes.KeyItems Then
                    If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                        t &= RemoveItem()
                    End If
                End If

                If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                    If gmBattleUseSound <> "" Then
                        SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                    Else
                        SoundManager.PlaySound("Use_Item", False)
                    End If
                End If
                Screen.TextBox.Show(t, {})
            End If
            If gmUseOnOwnEffects IsNot Nothing And gmUseOnOppEffects Is Nothing OrElse gmUseOnOwnEffects Is Nothing And gmUseOnOppEffects IsNot Nothing Then
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If

            Return True

        ElseIf NoTextOrSound = False Then
            Screen.TextBox.reDelay = 0.0F

            If cScreen.Identification = Identifications.BattleScreen Then
                If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("slp") Then
                    If UseOppSuccess = True Then
                        If gmBattleUseSound <> "" Then
                            SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                        Else
                            SoundManager.PlaySound("Use_Item", False)
                        End If
                        t = Localization.GetString("item_use_CureSleep_Single", "Cured the sleep~of [POKEMONNAME].").Replace("[POKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        PlayerStatistics.Track("[17]Medicine Items used", 1)
                        Return True
                    Else
                        t = Localization.GetString("item_cannot_use_NotAsleep_Both", "[OWNPOKEMONNAME] and~[OPPPOKEMONNAME] are not~asleep.").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_cannot_use_NotAsleep_Single", "[POKEMONNAME] is not~asleep.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If
            Else
                t = Localization.GetString("item_cannot_use_NotAsleep_Single", "[POKEMONNAME] is not~asleep.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
            End If

            Screen.TextBox.Show(t, {})

        End If
        Return False
    End Function

    ''' <summary>
    ''' Tries to heal a Pokémon from Burn.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function HealBurn(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False, Optional UseOppSuccess As Boolean = False, Optional ByVal p As Pokemon = Nothing) As Boolean
        If p Is Nothing Then
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If
        End If

        Dim Pokemon As Pokemon
        If p Is Nothing Then
            Pokemon = Core.Player.Pokemons(PokeIndex)
        Else
            Pokemon = p
        End If

        Dim cScreen As Screen = Core.CurrentScreen
        While Not cScreen.PreScreen Is Nothing And cScreen.Identification <> Identifications.BattleScreen
            cScreen = cScreen.PreScreen
        End While

        Dim t As String

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Burn Then
            Pokemon.Status = P3D.Pokemon.StatusProblems.None

            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                If cScreen.Identification = Identifications.BattleScreen Then
                    If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("brn") Then
                        If UseOppSuccess = True AndAlso p Is Nothing Then
                            t = Localization.GetString("item_use_CureBurn_Both", "Cured the burn~of [OWNPOKEMONNAME] and~[OPPPOKEMONNAME].").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else
                            t = Localization.GetString("item_use_CureBurn_Single", "Cured the burn~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                        End If
                    Else
                        t = Localization.GetString("item_use_CureBurn_Single", "Cured the burn~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_use_CureBurn_Single", "Cured the burn~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If
                If Me.gmItemType <> ItemTypes.KeyItems Then
                    If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                        t &= RemoveItem()
                    End If
                End If

                If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                    If gmBattleUseSound <> "" Then
                        SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                    Else
                        SoundManager.PlaySound("Use_Item", False)
                    End If
                End If
                Screen.TextBox.Show(t, {})
            End If
            If gmUseOnOwnEffects IsNot Nothing And gmUseOnOppEffects Is Nothing OrElse gmUseOnOwnEffects Is Nothing And gmUseOnOppEffects IsNot Nothing Then
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If
            Return True
        ElseIf NoTextOrSound = False Then
            Screen.TextBox.reDelay = 0.0F

            If cScreen.Identification = Identifications.BattleScreen Then
                If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("brn") Then
                    If UseOppSuccess = True Then
                        If gmBattleUseSound <> "" Then
                            SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                        Else
                            SoundManager.PlaySound("Use_Item", False)
                        End If
                        t = Localization.GetString("item_use_CureBurn_Single", "Cured the burn~of [POKEMONNAME].").Replace("[POKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        PlayerStatistics.Track("[17]Medicine Items used", 1)
                        Return True
                    Else
                        t = Localization.GetString("item_cannot_use_NotBurned_Both", "[OWNPOKEMONNAME] and~[OPPPOKEMONNAME] are not~burned.").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_cannot_use_NotBurned_Single", "[POKEMONNAME] is not~burned.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If
            Else
                t = Localization.GetString("item_cannot_use_NotBurned_Single", "[POKEMONNAME] is not~burned.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
            End If

            Screen.TextBox.Show(t, {})
        End If
        Return False
    End Function

    ''' <summary>
    ''' Tries to heal a Pokémon from Ice.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function HealIce(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False, Optional UseOppSuccess As Boolean = False, Optional ByVal p As Pokemon = Nothing) As Boolean
        If p Is Nothing Then
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If
        End If

        Dim Pokemon As Pokemon
        If p Is Nothing Then
            Pokemon = Core.Player.Pokemons(PokeIndex)
        Else
            Pokemon = p
        End If

        Dim cScreen As Screen = Core.CurrentScreen
        While Not cScreen.PreScreen Is Nothing And cScreen.Identification <> Identifications.BattleScreen
            cScreen = cScreen.PreScreen
        End While

        Dim t As String

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Freeze Then
            Pokemon.Status = P3D.Pokemon.StatusProblems.None

            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                If cScreen.Identification = Identifications.BattleScreen Then
                    If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("frz") Then
                        If UseOppSuccess = True AndAlso p Is Nothing Then
                            t = Localization.GetString("item_use_CureIce_Both", "Cured the ice~of [OWNPOKEMONNAME] and~[OPPPOKEMONNAME].").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else
                            t = Localization.GetString("item_use_CureIce_Single", "Cured the ice~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                        End If
                    Else
                        t = Localization.GetString("item_use_CureIce_Single", "Cured the ice~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_use_CureIce_Single", "Cured the ice~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If

                If Me.gmItemType <> ItemTypes.KeyItems Then
                    If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                        t &= RemoveItem()
                    End If
                End If

                If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                    If gmBattleUseSound <> "" Then
                        SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                    Else
                        SoundManager.PlaySound("Use_Item", False)
                    End If
                End If
                Screen.TextBox.Show(t, {})
            End If
            If gmUseOnOwnEffects IsNot Nothing And gmUseOnOppEffects Is Nothing OrElse gmUseOnOwnEffects Is Nothing And gmUseOnOppEffects IsNot Nothing Then
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If
            Return True
        ElseIf NoTextOrSound = False Then
            Screen.TextBox.reDelay = 0.0F

            If cScreen.Identification = Identifications.BattleScreen Then
                If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("frz") Then
                    If UseOppSuccess = True Then
                        If gmBattleUseSound <> "" Then
                            SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                        Else
                            SoundManager.PlaySound("Use_Item", False)
                        End If
                        t = Localization.GetString("item_use_CureIce_Single", "Cured the ice~of [POKEMONNAME].").Replace("[POKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        PlayerStatistics.Track("[17]Medicine Items used", 1)
                        Return True
                    Else
                        t = Localization.GetString("item_cannot_use_NotFrozen_Both", "[OWNPOKEMONNAME] and~[OPPPOKEMONNAME] are not~frozen.").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_cannot_use_NotFrozen_Single", "[POKEMONNAME] is not~frozen.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If
            Else
                t = Localization.GetString("item_cannot_use_NotFrozen_Single", "[POKEMONNAME] is not~frozen.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
            End If

            Screen.TextBox.Show(t, {})
        End If

        Return False


    End Function

    ''' <summary>
    ''' Tries to heal a Pokémon from Paralysis.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function HealParalyze(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False, Optional UseOppSuccess As Boolean = False, Optional ByVal p As Pokemon = Nothing) As Boolean
        If p Is Nothing Then
            If PokeIndex < 0 Or PokeIndex > 5 Then
                Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
            End If
        End If

        Dim Pokemon As Pokemon
        If p Is Nothing Then
            Pokemon = Core.Player.Pokemons(PokeIndex)
        Else
            Pokemon = p
        End If

        Dim cScreen As Screen = Core.CurrentScreen
        While Not cScreen.PreScreen Is Nothing And cScreen.Identification <> Identifications.BattleScreen
            cScreen = cScreen.PreScreen
        End While

        Dim t As String

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Paralyzed Then
            Pokemon.Status = P3D.Pokemon.StatusProblems.None

            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                If cScreen.Identification = Identifications.BattleScreen Then
                    If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("prz") Then
                        If UseOppSuccess = True AndAlso p Is Nothing Then
                            t = Localization.GetString("item_use_CureParalysis_Both", "Cured the paralysis~of [OWNPOKEMONNAME] and~[OPPPOKEMONNAME].").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        Else
                            t = Localization.GetString("item_use_CureParalysis_Single", "Cured the paralysis~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                        End If
                    Else
                        t = Localization.GetString("item_use_CureParalysis_Single", "Cured the paralysis~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_use_CureParalysis_Single", "Cured the paralysis~of [POKEMONNAME].").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If

                If Me.gmItemType <> ItemTypes.KeyItems Then
                    If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                        t &= RemoveItem()
                    End If
                End If

                If p Is Nothing OrElse Me.gmUseOnOwnEffects Is Nothing Then
                    If gmBattleUseSound <> "" Then
                        SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                    Else
                        SoundManager.PlaySound("Use_Item", False)
                    End If
                End If
                Screen.TextBox.Show(t, {})
            End If
            If gmUseOnOwnEffects IsNot Nothing And gmUseOnOppEffects Is Nothing OrElse gmUseOnOwnEffects Is Nothing And gmUseOnOppEffects IsNot Nothing Then
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If
            Return True
        ElseIf NoTextOrSound = False Then
            Screen.TextBox.reDelay = 0.0F

            If cScreen.Identification = Identifications.BattleScreen Then
                If gmUseOnOppEffects IsNot Nothing AndAlso gmUseOnOppEffects.Contains("prz") Then
                    If UseOppSuccess = True Then
                        If gmBattleUseSound <> "" Then
                            SoundManager.PlaySound(gmBattleUseSound.GetSplit(0, ","), CBool(gmBattleUseSound.GetSplit(1, ",")))
                        Else
                            SoundManager.PlaySound("Use_Item", False)
                        End If
                        t = Localization.GetString("item_use_CureParalysis_Single", "Cured the paralysis~of [POKEMONNAME].").Replace("[POKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                        PlayerStatistics.Track("[17]Medicine Items used", 1)
                        Return True
                    Else
                        t = Localization.GetString("item_cannot_use_NotParalyzed_Both", "[OWNPOKEMONNAME] and~[OPPPOKEMONNAME] are not~paralyzed.").Replace("[OWNPOKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OPPPOKEMONNAME]", CType(cScreen, BattleSystem.BattleScreen).OppPokemon.GetDisplayName())
                    End If
                Else
                    t = Localization.GetString("item_cannot_use_NotParalyzed_Single", "[POKEMONNAME] is not~paralyzed.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
                End If
            Else
                t = Localization.GetString("item_cannot_use_NotParalyzed_Single", "[POKEMONNAME] is not~paralyzed.").Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
            End If

            Screen.TextBox.Show(t, {})
        End If
        Return False
    End Function
End Class
