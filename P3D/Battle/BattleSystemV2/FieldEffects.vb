Namespace BattleSystem

    Public Class FieldEffects

        'Client side stuff in PvP
        Public ClientCanSwitch As Boolean = True 'Calculated by the host, sent to the client

        'Own stuff
        Public OwnSleepTurns As Integer = 0 'Sleep turns
        Public OwnTruantRound As Integer = 0 'Truant move counter
        Public OwnImprison As Integer = 0 'Imprison move counter
        Public OwnTaunt As Integer = 0 'Taunt move counter
        Public OwnTelekinesis As Integer = 0 'Telekinesis move counter
        Public OwnRageCounter As Integer = 0 'Rage move counter
        Public OwnUproar As Integer = 0 'Uproar move counter
        Public OwnFocusEnergy As Integer = 0 'Focus energy move counter
        Public OwnFlashFire As Integer = 0 'Flash Fire move counter
        Public OwnEndure As Integer = 0 'Endure move counter
        Public OwnProtectCounter As Integer = 0 'Protect move counter
        Public OwnKingsShieldCounter As Integer = 0 'Kings Shield move counter
        Public OwnSpikyShieldCounter As Integer = 0 'Spiky Shield move counter
        Public OwnDetectCounter As Integer = 0 'Detect move counter
        Public OwnBanefulBunkerCounter As Integer = 0 'Baneful Bunker move counter
        Public OwnCraftyShieldCounter As Integer = 0 'Crafty Shield move counter
        Public OwnMatBlockCounter As Integer = 0 'Mat Block move counter
        Public OwnWideGuardCounter As Integer = 0 'Wide Guard move counter
        Public OwnQuickGuardCounter As Integer = 0 'Quick Guard move counter
        Public OwnIngrain As Integer = 0 'Ingrain move counter
        Public OwnSubstitute As Integer = 0 'Substitute HP left
        Public OwnLuckyChant As Integer = 0 'Lucky chant move counter
        Public OwnMagnetRise As Integer = 0 'magnetrise move counter
        Public OwnAquaRing As Integer = 0 'Aqua ring move counter
        Public OwnPoisonCounter As Integer = 0 'Poison counter for bad poison
        Public OwnNightmare As Integer = 0 'Nightmare move counter
        Public OwnCurse As Integer = 0 'Curse move counter
        Public OwnOutrage As Integer = 0 'Outrage move counter
        Public OwnThrash As Integer = 0 'Thrash move counter
        Public OwnPetalDance As Integer = 0 'Petaldance move counter
        Public OwnEncore As Integer = 0 'Encore move counter
        Public OwnEncoreMove As Attack = Nothing 'Encore move used
        Public OwnEmbargo As Integer = 0 'Embargo move counter
        Public OwnYawn As Integer = 0 'Yawn move counter
        Public OwnPerishSongCount As Integer = 0 'Perishsong move counter
        Public OwnConfusionTurns As Integer = 0 'Turns until confusion runs out
        Public TempOwnConfusionTurns As Integer = 0 'For the functionality of Berserk Gene
        Public OwnTorment As Integer = 0 'Torment move counter
        Public OwnTormentMove As Attack = Nothing 'Torment move
        Public OwnMetronomeItemCount As Integer = 0 'The counter for the item Metronome
        Public OwnChoiceMove As Attack = Nothing 'The move chosen to use for choice items
        Public OwnRolloutCounter As Integer = 0 'Rollout move counter
        Public OwnIceBallCounter As Integer = 0 'IceBall move counter
        Public OwnRecharge As Integer = 0 'Recharge counter for moves like Hyperbeam
        Public OwnDefenseCurl As Integer = 0 'Checks if defense curl was used
        Public OwnCharge As Integer = 0 'Charge move counter
        Public OwnPayDayCounter As Integer = 0 'Counter for the payday move
        Public OwnRazorWindCounter As Integer = 0 'Razor Wind move counter
        Public OwnSkullBashCounter As Integer = 0 'Skull Bash move counter
        Public OwnSkyAttackCounter As Integer = 0 'Sky Attack move counter
        Public OwnMinimize As Integer = 0 'Set if user used the move Minimize
        Public OwnLastDamage As Integer = 0 'Last Damage the own Pokémon has done by moves.
        Public OwnLeechSeed As Integer = 0 'The opponent used leech seed
        Public OwnSolarBeam As Integer = 0 'Charge counter for solar beam
        Public OwnSolarBlade As Integer = 0 'Charge counter for solar blade
        Public OwnLockOn As Integer = 0 'Counter for the moves lock-on and mind reader
        Public OwnBideCounter As Integer = 0 'Counter for the Bide move
        Public OwnBideDamage As Integer = 0 'Half of the damage dealt by bide
        Public OwnRageFistPower As Integer = 0 'how much the Power of the attack Rage Fist increases
        Public OwnLansatBerry As Integer = 0 'Raise critical hit ration when Lansat got eaten
        Public OwnCustapBerry As Integer = 0 'Raises the attack speed once when Custap got eaten
        Public OwnTrappedCounter As Integer = 0 'If the pokemon is trapped (for example by Spider Web), this is =1
        Public OwnForesight As Integer = 0 'Own Ghost Pokémon can be hit by Normal/Fighting attacks
        Public OwnOdorSleuth As Integer = 0 'Same as Foresight
        Public OwnMiracleEye As Integer = 0 'Own Dark type Pokémon can be hit by Psychic type attacks
        Public OwnProtectMovesCount As Integer = 0 'Counts uses of protect moves
        Public OwnFuryCutter As Integer = 0 'Counter for the move fury cutter
        Public OwnEchoedVoice As Integer = 0 'Counter for the move echoed voice
        Public OwnPokemonTurns As Integer = 0 'Turns for how long the own pokemon has been in battle
        Public OwnStockpileCount As Integer = 0 'A counter for the stockpile moves used for Swallow and Spit Up
        Public OwnIceBurnCounter As Integer = 0 'Counter for the Ice Burn move.
        Public OwnFreezeShockCounter As Integer = 0
        Public OwnWaterPledge As Integer = 0 'Doubles effect propability for four turns
        Public OwnGrassPledge As Integer = 0 'Halves the opponent's speed for four turns
        Public OwnFirePledge As Integer = 0 'Deals damage of 1/8 HP at the end of turn for four turns
        Public OwnPokemonDamagedThisTurn As Boolean = False
        Public OwnPokemonDamagedLastTurn As Boolean = False
        Public OwnFlyCounter As Integer = 0
        Public OwnDigCounter As Integer = 0
        Public OwnBounceCounter As Integer = 0
        Public OwnDiveCounter As Integer = 0
        Public OwnShadowForceCounter As Integer = 0
        Public OwnPhantomForceCounter As Integer = 0
        Public OwnSkyDropCounter As Integer = 0
        Public OwnGeomancyCounter As Integer = 0
        Public OwnWrap As Integer = 0
        Public OwnWhirlpool As Integer = 0
        Public OwnBind As Integer = 0
        Public OwnClamp As Integer = 0
        Public OwnFireSpin As Integer = 0
        Public OwnMagmaStorm As Integer = 0
        Public OwnSandTomb As Integer = 0
        Public OwnInfestation As Integer = 0
        Public OwnUsedMoves As New List(Of Integer)
        Public OwnMagicCoat As Integer = 0
        Public OwnConsumedItem As Item = Nothing
        Public OwnCudChewBerry As Item = Nothing 'Cud Chew Ability
        Public OwnCudChewIndex As Integer = -1
        Public OwnSmacked As Integer = 0 'Smack Down effect condition
        Public OwnPursuit As Boolean = False
        Public OwnMegaEvolved As Boolean = False
        Public OwnRoostUsed As Boolean = False 'If roost got used, this is true and will get set false and revert types at the end of a turn.
        Public OwnDestinyBond As Boolean = False 'If own Pokémon used destiny bond, this is true. If the opponent knocks the own Pokémon out, it will faint as well.
        Public OwnHealingWish As Boolean = False 'True, if healing wish got used. Heals the next switched in Pokémon.
        Public OwnGastroAcid As Boolean = False 'If own Pokémon is affected by Gastro Acid
        Public OwnTarShot As Boolean = False 'If own Pokémon is affected by Tar Shot

        Public OwnLastMove As Attack = Nothing 'Last move used
        Public OwnLastMoveFailed As Boolean = False 'keeping track of this for Stomping Tantrum
        Public OwnSpikes As Integer = 0 'Trap move counter
        Public OwnStealthRock As Integer = 0 'Trap move counter
        Public OwnStickyWeb As Integer = 0 'Trap move counter
        Public OwnToxicSpikes As Integer = 0 'Trap move counter
        Public OwnMist As Integer = 0 'Mist move counter
        Public OwnGuardSpec As Integer = 0 'Guard spec item counter
        Public OwnTurnCounts As Integer = 0 'Own turns
        Public OwnLightScreen As Integer = 0 'Lightscreen move counter
        Public OwnReflect As Integer = 0 'Reflect move counter
        Public OwnTailWind As Integer = 0 'Tailwind move counter
        Public OwnHealBlock As Integer = 0 'Healblock move counter
        Public OwnSafeguard As Integer = 0 'Safeguard move counter 
        Public OwnWish As Integer = 0 'Wish move counter 
        Public OwnFutureSightDamage As Integer = 0 'stored Futuresight move damage
        Public OwnFutureSightTurns As Integer = 0 'Turns until Futuresight hits
        Public OwnFutureSightID As Integer = 0 'Move ID for the Futuresight move

        Public OwnUsedRandomMove As Boolean = False 'Metronome for example
        Public OwnUsedMirrorMove As Boolean = False

        'Opp stuff
        Public OppSpikes As Integer = 0
        Public OppStealthRock As Integer = 0
        Public OppStickyWeb As Integer = 0
        Public OppToxicSpikes As Integer = 0
        Public OppMist As Integer = 0
        Public OppGuardSpec As Integer = 0
        Public OppTurnCounts As Integer = 0
        Public OppLastMove As Attack = Nothing
        Public OppLastMoveFailed As Boolean = False
        Public OppLightScreen As Integer = 0
        Public OppReflect As Integer = 0
        Public OppTailWind As Integer = 0
        Public OppSleepTurns As Integer = 0
        Public OppTruantRound As Integer = 0
        Public OppImprison As Integer = 0
        Public OppHealBlock As Integer = 0
        Public OppTaunt As Integer = 0
        Public OppTelekinesis As Integer = 0
        Public OppRageCounter As Integer = 0
        Public OppUproar As Integer = 0
        Public OppFocusEnergy As Integer = 0
        Public OppFlashFire As Integer = 0
        Public OppEndure As Integer = 0
        Public OppProtectCounter As Integer = 0
        Public OppKingsShieldCounter As Integer = 0
        Public OppSpikyShieldCounter As Integer = 0
        Public OppDetectCounter As Integer = 0
        Public OppBanefulBunkerCounter As Integer = 0
        Public OppCraftyShieldCounter As Integer = 0
        Public OppMatBlockCounter As Integer = 0
        Public OppWideGuardCounter As Integer = 0
        Public OppQuickGuardCounter As Integer = 0
        Public OppIngrain As Integer = 0
        Public OppSubstitute As Integer = 0
        Public OppSafeguard As Integer = 0
        Public OppLuckyChant As Integer = 0
        Public OppWish As Integer = 0
        Public OppMagnetRise As Integer = 0
        Public OppAquaRing As Integer = 0
        Public OppPoisonCounter As Integer = 0
        Public OppNightmare As Integer = 0
        Public OppCurse As Integer = 0
        Public OppOutrage As Integer = 0
        Public OppThrash As Integer = 0
        Public OppPetalDance As Integer = 0
        Public OppEncore As Integer = 0
        Public OppEncoreMove As Attack
        Public OppEmbargo As Integer = 0
        Public OppYawn As Integer = 0
        Public OppFutureSightDamage As Integer = 0
        Public OppFutureSightTurns As Integer = 0
        Public OppFutureSightID As Integer = 0
        Public OppPerishSongCount As Integer = 0
        Public OppConfusionTurns As Integer = 0
        Public TempOppConfusionTurns As Integer = 0
        Public OppTorment As Integer = 0
        Public OppTormentMove As Attack = Nothing
        Public OppMetronomeItemCount As Integer = 0
        Public OppChoiceMove As Attack = Nothing
        Public OppRolloutCounter As Integer = 0
        Public OppIceBallCounter As Integer = 0
        Public OppRecharge As Integer = 0
        Public OppDefenseCurl As Integer = 0
        Public OppCharge As Integer = 0
        Public OppPayDayCounter As Integer = 0
        Public OppRazorWindCounter As Integer = 0
        Public OppSkullBashCounter As Integer = 0
        Public OppSkyAttackCounter As Integer = 0
        Public OppMinimize As Integer = 0
        Public OppLastDamage As Integer = 0
        Public OppLeechSeed As Integer = 0
        Public OppSolarBeam As Integer = 0
        Public OppSolarBlade As Integer = 0
        Public OppLockOn As Integer = 0
        Public OppBideCounter As Integer = 0
        Public OppRageFistPower As Integer = 0
        Public OppBideDamage As Integer = 0
        Public OppLansatBerry As Integer = 0
        Public OppCustapBerry As Integer = 0
        Public OppTrappedCounter As Integer = 0
        Public OppForesight As Integer = 0
        Public OppOdorSleuth As Integer = 0
        Public OppMiracleEye As Integer = 0
        Public OppProtectMovesCount As Integer = 0
        Public OppFuryCutter As Integer = 0
        Public OppEchoedVoice As Integer = 0
        Public OppPokemonTurns As Integer = 0
        Public OppStockpileCount As Integer = 0
        Public OppIceBurnCounter As Integer = 0
        Public OppFreezeShockCounter As Integer = 0
        Public OppWaterPledge As Integer = 0
        Public OppGrassPledge As Integer = 0
        Public OppFirePledge As Integer = 0
        Public OppPokemonDamagedThisTurn As Boolean = False
        Public OppPokemonDamagedLastTurn As Boolean = False
        Public OppMagicCoat As Integer = 0
        Public OppConsumedItem As Item = Nothing
        Public OppCudChewBerry As Item = Nothing
        Public OppCudChewIndex As Integer = -1
        Public OppSmacked As Integer = 0
        Public OppPursuit As Boolean = False
        Public OppMegaEvolved As Boolean = False
        Public OppRoostUsed As Boolean = False
        Public OppDestinyBond As Boolean = False
        Public OppHealingWish As Boolean = False
        Public OppGastroAcid As Boolean = False
        Public OppTarShot As Boolean = False 'If opponent Pokémon is affected by Tar Shot

        Public OppFlyCounter As Integer = 0
        Public OppDigCounter As Integer = 0
        Public OppBounceCounter As Integer = 0
        Public OppDiveCounter As Integer = 0
        Public OppShadowForceCounter As Integer = 0
        Public OppPhantomForceCounter As Integer = 0
        Public OppSkyDropCounter As Integer = 0
        Public OppGeomancyCounter As Integer = 0

        Public OppWrap As Integer = 0
        Public OppWhirlpool As Integer = 0
        Public OppBind As Integer = 0
        Public OppClamp As Integer = 0
        Public OppFireSpin As Integer = 0
        Public OppMagmaStorm As Integer = 0
        Public OppSandTomb As Integer = 0
        Public OppInfestation As Integer = 0

        Public OppUsedRandomMove As Boolean = False
        Public OppUsedMirrorMove As Boolean = False

        Public OppUsedMoves As New List(Of Integer)

        'Global Temporary Variables
        Public TempTripleKick As Integer = 0 'If Triple Kick is used, this will be increased every time it's hit

        'Weather
        Private _weather As BattleWeather.WeatherTypes = BattleWeather.WeatherTypes.Clear
        Public WeatherRounds As Integer = 0

        Public Property Weather() As BattleWeather.WeatherTypes
            Get
                Return Me._weather
            End Get
            Set(value As BattleWeather.WeatherTypes)
                Me._weather = value
                Screen.Level.World.CurrentMapWeather = BattleWeather.GetWorldWeather(value)
            End Set
        End Property

        'Field stuff
        Public TrickRoom As Integer = 0
        Public Gravity As Integer = 0
        Public MudSport As Integer = 0
        Public WaterSport As Integer = 0
        Public Rounds As Integer = 0
        Public AmuletCoin As Integer = 0

        Public ElectricTerrain As Integer = 0
        Public GrassyTerrain As Integer = 0
        Public MistyTerrain As Integer = 0
        Public PsychicTerrain As Integer = 0

        'Special stuff
        Public RunTries As Integer = 0
        Public UsedPokemon As New List(Of Integer)
        Public StolenItemIDs As New List(Of String)
        Public DefeatedTrainerPokemon As Boolean = False
        Public RoamingFled As Boolean = False

        'Moves that swap out Pokémon
        Public OwnSwapIndex As Integer = -1
        Public OppSwapIndex As Integer = -1

        'BatonPassTemp:
        Public OwnUsedBatonPass As Boolean = False
        Public OwnBatonPassStats As List(Of Integer)
        Public OwnBatonPassConfusion As Boolean = False
        Public OwnBatonPassIndex As Integer = -1

        Public OppUsedBatonPass As Boolean = False
        Public OppBatonPassStats As List(Of Integer)
        Public OppBatonPassConfusion As Boolean = False
        Public OppBatonPassIndex As Integer = -1

        Public Function CanUseItem(ByVal own As Boolean) As Boolean
            Dim embargo As Integer = OwnEmbargo
            If own = False Then
                embargo = OppEmbargo
            End If
            If embargo > 0 Then
                Return False
            End If
            Return True
        End Function

        Public Function CanUseOwnItem(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If
            If p.Ability.Name.ToLower() = "klutz" Then
                Return False
            End If
            Return True
        End Function

        ''' <summary>
        ''' Checks if a Pokémon can use its ability.
        ''' </summary>
        ''' <param name="own">Which Pokémon?</param>
        ''' <param name="BattleScreen">The BattleScreen reference.</param>
        ''' <param name="CheckType">0: Supression abilities, 1: GastroAcid, 2: both</param>
        Public Function CanUseAbility(ByVal own As Boolean, ByVal BattleScreen As BattleScreen, Optional ByVal CheckType As Integer = 0) As Boolean
            If CheckType = 0 Or CheckType = 2 Then
                Dim p As Pokemon = BattleScreen.OppPokemon
                If own = False Then
                    p = BattleScreen.OwnPokemon
                End If

                Dim supressAbilities() As String = {"mold breaker", "turboblaze", "teravolt"}

                If supressAbilities.Contains(p.Ability.Name.ToLower()) = True Then
                    Return False
                End If
            End If
            If CheckType = 1 Or CheckType = 2 Then
                If own = True Then
                    If OwnGastroAcid = True Then
                        Return False
                    End If
                Else
                    If OppGastroAcid = True Then
                        Return False
                    End If
                End If
            End If

            Return True
        End Function

        Public Function IsGrounded(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim grounded As Boolean = True
            If own = True Then
                If p.Type1.Type = Element.Types.Flying Or p.Type2.Type = Element.Types.Flying Or p.Ability.Name.ToLower() = "levitate" And BattleScreen.FieldEffects.CanUseAbility(True, BattleScreen) = True Then
                    grounded = False
                End If
                If BattleScreen.FieldEffects.Gravity > 0 Or BattleScreen.FieldEffects.OwnSmacked > 0 Or BattleScreen.FieldEffects.OwnIngrain > 0 Then
                    grounded = True
                Else
                    If BattleScreen.FieldEffects.OwnTelekinesis > 0 Or BattleScreen.FieldEffects.OwnMagnetRise > 0 Then
                        grounded = False
                    End If
                    If Not p.Item Is Nothing Then
                        If p.Item.OriginalName.ToLower() = "air balloon" And BattleScreen.FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                            grounded = False
                        End If
                        If p.Item.OriginalName.ToLower() = "iron ball" And BattleScreen.FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                            grounded = True
                        End If
                    End If
                End If
                If OwnBounceCounter > 0 Or OwnDigCounter > 0 Or OwnDiveCounter > 0 Or OwnFlyCounter > 0 Or OwnPhantomForceCounter > 0 Or OwnShadowForceCounter > 0 Or OwnSkyDropCounter > 0 Then
                    grounded = False
                End If
            Else
                p = BattleScreen.OppPokemon
                If p.Type1.Type = Element.Types.Flying Or (p.Type2 IsNot Nothing AndAlso p.Type2.Type = Element.Types.Flying) Or p.Ability.Name.ToLower() = "levitate" And BattleScreen.FieldEffects.CanUseAbility(False, BattleScreen) = True Then
                    grounded = False
                End If
                If BattleScreen.FieldEffects.Gravity > 0 Or BattleScreen.FieldEffects.OppSmacked > 0 Or BattleScreen.FieldEffects.OppIngrain > 0 Then
                    grounded = True
                Else
                    If BattleScreen.FieldEffects.OppTelekinesis > 0 Or BattleScreen.FieldEffects.OppMagnetRise > 0 Then
                        grounded = False
                    End If
                    If Not p.Item Is Nothing Then
                        If p.Item.OriginalName.ToLower() = "air balloon" And BattleScreen.FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                            grounded = False
                        End If
                        If p.Item.OriginalName.ToLower() = "iron ball" And BattleScreen.FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                            grounded = True
                        End If
                    End If
                End If
                If OppBounceCounter > 0 Or OppDigCounter > 0 Or OppDiveCounter > 0 Or OppFlyCounter > 0 Or OppPhantomForceCounter > 0 Or OppShadowForceCounter > 0 Or OppSkyDropCounter > 0 Then
                    grounded = False
                End If
            End If

            If grounded = True Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function GetPokemonWeight(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Single
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim weigth As Single = p.PokedexEntry.Weight

            If p.Ability.Name.ToLower() = "light metal" And CanUseAbility(own, BattleScreen) = True Then
                weigth /= 2
            End If

            If p.Ability.Name.ToLower() = "heavy metal" And CanUseAbility(own, BattleScreen) = True Then
                weigth *= 2
            End If

            Return weigth
        End Function

        Public Function MovesFirst(ByVal own As Boolean) As Boolean
            If own = True Then
                If OwnTurnCounts > OppTurnCounts Then
                    Return True
                Else
                    Return False
                End If
            Else
                If OppTurnCounts > OwnTurnCounts Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

    End Class

End Namespace
