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
    Public gmCureStatusEffects As List(Of String)
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

        If gmIsHealingItem = True Then
            If CBool(GameModeManager.GetGameRuleValue("CanUseHealItems", "1")) = False Then
                Screen.TextBox.Show("Cannot use heal items.", {}, False, False)
                Exit Sub
            End If
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        ElseIf gmCureStatusEffects IsNot Nothing AndAlso gmCureStatusEffects.Count > 0 Then
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        ElseIf gmIsEvolutionItem = True AndAlso gmEvolutionPokemon IsNot Nothing AndAlso gmEvolutionPokemon.Count > 0 Then
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
            CType(CurrentScreen, PartyScreen).EvolutionItemID = Me.gmID
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

        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Fainted AndAlso (gmCureStatusEffects Is Nothing OrElse gmCureStatusEffects.Count = 0 OrElse gmCureStatusEffects.Contains("fnt") = False) Then
            Screen.TextBox.reDelay = 0.0F
            Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})
            Return False
        Else
            Dim healsuccess As Boolean = False
            If gmCureStatusEffects Is Nothing OrElse gmCureStatusEffects.Count = 0 Then
                If gmIsHealingItem = True AndAlso gmHealHPAmount > 0 Then
                    Return HealPokemon(PokeIndex, gmHealHPAmount)
                End If
            ElseIf gmCureStatusEffects.Contains("all") = False AndAlso gmCureStatusEffects.Contains("allwithoutfnt") = False Then
                If gmIsHealingItem = True AndAlso gmHealHPAmount > 0 Then
                    If HealPokemon(PokeIndex, gmHealHPAmount, True) Then
                        healsuccess = True
                    End If
                End If
            End If

            If gmCureStatusEffects IsNot Nothing AndAlso gmCureStatusEffects.Count > 0 Then
                If gmCureStatusEffects.Contains("allwithoutfnt") Then
                    Dim success1 As Boolean = False
                    Dim success2 As Boolean = False
                    If gmIsHealingItem = True AndAlso gmHealHPAmount > 0 Then
                        If HealPokemon(PokeIndex, gmHealHPAmount, True) = True Then
                            success1 = True
                        End If
                    End If
                    If Pokemon.Status <> Pokemon.StatusProblems.Fainted AndAlso Pokemon.Status <> Pokemon.StatusProblems.None Or Pokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                        If Pokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                            Pokemon.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)
                        End If
                        Pokemon.Status = Pokemon.StatusProblems.None
                        success2 = True
                    End If

                    Dim t As String = ""
                    If success1 = True AndAlso success2 = False Then
                        t &= "Healed " & Pokemon.GetDisplayName() & "!"
                    End If
                    If success1 = False AndAlso success2 = True Then
                        t &= "Cured " & Pokemon.GetDisplayName() & "!"
                    End If
                    If success1 = True AndAlso success2 = True Then
                        t &= "Healed and cured~" & Pokemon.GetDisplayName() & "!"
                    End If

                    If success1 = True Or success2 = True Then
                        Screen.TextBox.reDelay = 0.0F

                        t &= RemoveItem()
                        PlayerStatistics.Track("[17]Medicine Items used", 1)

                        SoundManager.PlaySound("Use_Item", False)
                        Screen.TextBox.Show(t, {})

                        Return True
                    Else
                        Screen.TextBox.Show("Cannot use" & Me.gmName & "~on " & Pokemon.GetDisplayName() & ".", {}, False, False)
                        Return False
                    End If
                ElseIf gmCureStatusEffects.Contains("all") Then
                    Dim success1 As Boolean = False
                    Dim success2 As Boolean = False
                    If gmIsHealingItem = True AndAlso gmHealHPAmount > 0 Then
                        If HealPokemon(PokeIndex, gmHealHPAmount, True) = True Then
                            success1 = True
                        End If
                    End If
                    If Pokemon.Status <> Pokemon.StatusProblems.None Or Pokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                        If Pokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                            Pokemon.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)
                        End If
                        Pokemon.Status = Pokemon.StatusProblems.None
                        success2 = True
                    End If

                    Dim t As String = ""
                    If success1 = True AndAlso success2 = False Then
                        t &= "Healed " & Pokemon.GetDisplayName() & "!"
                    End If
                    If success1 = False AndAlso success2 = True Then
                        t &= "Cured " & Pokemon.GetDisplayName() & "!"
                    End If
                    If success1 = True AndAlso success2 = True Then
                        t &= "Healed and cured~" & Pokemon.GetDisplayName() & "!"
                    End If

                    If success1 = True Or success2 = True Then
                        Screen.TextBox.reDelay = 0.0F

                        t &= RemoveItem()
                        PlayerStatistics.Track("[17]Medicine Items used", 1)

                        SoundManager.PlaySound("Use_Item", False)
                        Screen.TextBox.Show(t, {})

                        Return True
                    Else
                        Screen.TextBox.Show("Cannot use" & Me.gmName & "~on " & Pokemon.GetDisplayName() & ".", {}, False, False)
                        Return False
                    End If
                Else
                    If gmCureStatusEffects.Count = 1 Then
                        If gmCureStatusEffects.Contains("brn") Then
                            Return HealBurn(PokeIndex)
                        End If
                        If gmCureStatusEffects.Contains("frz") Then
                            Return HealIce(PokeIndex)
                        End If
                        If gmCureStatusEffects.Contains("prz") Then
                            Return HealParalyze(PokeIndex)
                        End If
                        If gmCureStatusEffects.Contains("psn") OrElse gmCureStatusEffects.Contains("bpsn") Then
                            Return CurePoison(PokeIndex)
                        End If
                        If gmCureStatusEffects.Contains("slp") Then
                            Return WakeUp(PokeIndex)
                        End If
                        If gmCureStatusEffects.Contains("cfs") Then
                            Return CureConfusion(PokeIndex)
                        End If
                        If gmCureStatusEffects.Contains("fnt") Then
                            Return Revive(PokeIndex)
                        End If
                    Else
                        Dim success As Boolean = False
                        If gmCureStatusEffects.Contains("brn") Then
                            If HealBurn(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmCureStatusEffects.Contains("frz") Then
                            If HealIce(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmCureStatusEffects.Contains("prz") Then
                            If HealParalyze(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmCureStatusEffects.Contains("psn") OrElse gmCureStatusEffects.Contains("bpsn") Then
                            If CurePoison(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmCureStatusEffects.Contains("slp") Then
                            If WakeUp(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmCureStatusEffects.Contains("cfs") Then
                            If CureConfusion(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        If gmCureStatusEffects.Contains("fnt") Then
                            If Revive(PokeIndex, True) Then
                                success = True
                            End If
                        End If
                        Dim t As String = ""
                        If healsuccess = True AndAlso success = False Then
                            t &= "Healed " & Pokemon.GetDisplayName() & "!"
                        End If
                        If healsuccess = False AndAlso success = True Then
                            t &= "Cured " & Pokemon.GetDisplayName() & "!"
                        End If
                        If healsuccess = True AndAlso success = True Then
                            t &= "Healed and cured~" & Pokemon.GetDisplayName() & "!"
                        End If

                        If healsuccess = True Or success = True Then
                            Screen.TextBox.reDelay = 0.0F

                            t &= RemoveItem()
                            PlayerStatistics.Track("[17]Medicine Items used", 1)

                            SoundManager.PlaySound("Use_Item", False)
                            Screen.TextBox.Show(t, {})

                            Return True
                        Else
                            Screen.TextBox.Show("Cannot use" & Me.gmName & "~on " & Pokemon.GetDisplayName() & ".", {}, False, False)
                            Return False
                        End If
                    End If
                End If
            End If
        End If
        If gmIsEvolutionItem = True AndAlso gmEvolutionPokemon IsNot Nothing AndAlso gmEvolutionPokemon.Count > 0 Then
            Return Me.UseEvolutionItem(PokeIndex)
        End If

        Return False
    End Function

    Public Function UseEvolutionItem(ByVal PokeIndex As Integer) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Dim p As Pokemon = Core.Player.Pokemons(PokeIndex)

        If p.IsEgg() = False And p.CanEvolve(EvolutionCondition.EvolutionTrigger.ItemUse, Me.gmID) = True Then
            Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New EvolutionScreen(Core.CurrentScreen, {PokeIndex}.ToList(), Me.gmID, EvolutionCondition.EvolutionTrigger.ItemUse), Color.Black, False))

            RemoveItem()

            Return True
        Else
            Screen.TextBox.Show("Cannot use on~" & p.GetDisplayName(), {}, False, False)

            Return False
        End If
    End Function

    ''' <summary>
    ''' Tries to heal a Pokémon from the player's party. If this succeeds, the method returns True.
    ''' </summary>
    ''' <param name="PokeIndex">The index of the Pokémon in the player's party.</param>
    ''' <param name="HP">The HP that should be healed.</param>
    Public Function HealPokemon(ByVal PokeIndex As Integer, ByVal HP As Integer, Optional NoTextOrSound As Boolean = False) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If HP < 0 Then
            HP = CInt(Pokemon.MaxHP / (100 / (HP * (-1))))
        End If

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Fainted Then
            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is fainted!", {})
            End If
            Return False
        Else
            If Pokemon.HP = Pokemon.MaxHP Then
                If NoTextOrSound = False Then
                    Screen.TextBox.reDelay = 0.0F
                    Screen.TextBox.Show(Pokemon.GetDisplayName() & " has full~HP already.", {})

                End If
                Return False
            Else
                Dim diff As Integer = Pokemon.MaxHP - Pokemon.HP
                diff = CInt(MathHelper.Clamp(diff, 1, HP))

                Pokemon.Heal(HP)

                If NoTextOrSound = False Then
                    Screen.TextBox.reDelay = 0.0F

                    Dim t As String = "Restored " & Pokemon.GetDisplayName() & "'s~HP by " & diff & "."
                    t &= RemoveItem()

                    SoundManager.PlaySound("Use_Item", False)
                    Screen.TextBox.Show(t, {})
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

                Dim t As String = Pokemon.GetDisplayName() & "~is revitalized."

                t &= RemoveItem()
                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show(t, {}, False, False)
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If
            Return True
        Else
            If NoTextOrSound = False Then
                Screen.TextBox.Show("Cannot use" & Me.gmName & "~on " & Pokemon.GetDisplayName() & ".", {}, False, False)
            End If
            Return False
        End If

    End Function
    ''' <summary>
    ''' Tries to cure a Pokémon from Poison.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function CurePoison(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Poison Or Pokemon.Status = P3D.Pokemon.StatusProblems.BadPoison Then
            Pokemon.Status = P3D.Pokemon.StatusProblems.None

            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                Dim t As String = "Cures the poison~of " & Pokemon.GetDisplayName() & "."
                t &= RemoveItem()
                PlayerStatistics.Track("[17]Medicine Items used", 1)

                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show(t, {})
            End If
            Return True
        ElseIf NoTextOrSound = False Then
            Screen.TextBox.reDelay = 0.0F
            Screen.TextBox.Show(Pokemon.GetDisplayName() & " is not poisoned.", {})

        End If
        Return False

    End Function
    ''' <summary>
    ''' Tries to cure a Pokémon from Confusion.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function CureConfusion(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If Pokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
            Pokemon.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)

            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F
                Dim t As String = Pokemon.GetDisplayName() & "~is no longer confused."

                t &= RemoveItem()
                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show(t, {}, False, False)
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If
            Return True
        Else
            If NoTextOrSound = False Then
                Screen.TextBox.Show(Pokemon.GetDisplayName() & "~is not confused.", {}, False, False)
            End If
            Return False
        End If

    End Function

    ''' <summary>
    ''' Tries to wake a Pokémon up from Sleep.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function WakeUp(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Sleep Then
            Pokemon.Status = P3D.Pokemon.StatusProblems.None

            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                Dim t As String = "Cures the sleep~of " & Pokemon.GetDisplayName() & "."

                t &= RemoveItem()

                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show(t, {})
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If

            Return True

        ElseIf NoTextOrSound = False Then
            Screen.TextBox.reDelay = 0.0F
            Screen.TextBox.Show(Pokemon.GetDisplayName() & " is not asleep.", {})

        End If
        Return False
    End Function

    ''' <summary>
    ''' Tries to heal a Pokémon from Burn.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function HealBurn(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Burn Then
            Pokemon.Status = P3D.Pokemon.StatusProblems.None

            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                Dim t As String = "Cures the burn~of " & Pokemon.GetDisplayName() & "."
                t &= RemoveItem()

                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show(t, {})
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If
            Return True
        ElseIf NoTextOrSound = False Then
            Screen.TextBox.reDelay = 0.0F
            Screen.TextBox.Show(Pokemon.GetDisplayName() & " is not burned.", {})
        End If
        Return False
    End Function

    ''' <summary>
    ''' Tries to heal a Pokémon from Ice.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function HealIce(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Freeze Then
            Pokemon.Status = P3D.Pokemon.StatusProblems.None

            Core.Player.Inventory.RemoveItem(Me.ID.ToString, 1)
            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                Dim t As String = "Cures the ice~of " & Pokemon.GetDisplayName() & "."
                t &= RemoveItem()

                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show(t, {})
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If
            Return True
        ElseIf NoTextOrSound = False Then
            Screen.TextBox.reDelay = 0.0F
            Screen.TextBox.Show(Pokemon.GetDisplayName() & " is not frozen.", {})
        End If

        Return False


    End Function

    ''' <summary>
    ''' Tries to heal a Pokémon from Paralysis.
    ''' </summary>
    ''' <param name="PokeIndex">The index of a Pokémon in the player's party.</param>
    Public Function HealParalyze(ByVal PokeIndex As Integer, Optional NoTextOrSound As Boolean = False) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Dim Pokemon As Pokemon = Core.Player.Pokemons(PokeIndex)

        If Pokemon.Status = P3D.Pokemon.StatusProblems.Paralyzed Then
            Pokemon.Status = P3D.Pokemon.StatusProblems.None

            Core.Player.Inventory.RemoveItem(Me.ID.ToString, 1)
            If NoTextOrSound = False Then
                Screen.TextBox.reDelay = 0.0F

                Dim t As String = "Cures the paralysis~of " & Pokemon.GetDisplayName() & "."
                t &= RemoveItem()

                SoundManager.PlaySound("Use_Item", False)
                Screen.TextBox.Show(t, {})
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            End If
            Return True
        ElseIf NoTextOrSound = False Then
            Screen.TextBox.reDelay = 0.0F
            Screen.TextBox.Show(Pokemon.GetDisplayName() & " is not~paralyzed.", {})
        End If
        Return False
    End Function
End Class