Namespace BattleSystem

    Public Class FieldEffects

        'Own stuff
        Public OwnSleepTurns As Integer = 0 'Sleep turns
        Public OwnTruantRound As Integer = 0 'Truant move counter
        Public OwnImprison As Integer = 0 'Imprison move counter
        Public OwnTaunt As Integer = 0 'Taunt move counter
        Public OwnRageCounter As Integer = 0 'Rage move counter
        Public OwnUproar As Integer = 0 'Uproar move counter
        Public OwnFocusEnergy As Integer = 0 'Focus energy move counter
        Public OwnEndure As Integer = 0 'Endure move counter
        Public OwnProtectCounter As Integer = 0 'Protect move counter
        Public OwnKingsShieldCounter As Integer = 0 'Kings Shield move counter
        Public OwnDetectCounter As Integer = 0 'Detect move counter
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
        Public OwnLockOn As Integer = 0 'Counter for the moves lock-on and mind reader
        Public OwnBideCounter As Integer = 0 'Counter for the Bide move
        Public OwnBideDamage As Integer = 0 'Half of the damage dealt by bide
        Public OwnLansatBerry As Integer = 0 'Raise critical hit ration when Lansat got eaten
        Public OwnCustapBerry As Integer = 0 'Raises the attack speed once when Custap got eaten
        Public OwnTrappedCounter As Integer = 0 'If the pokemon is trapped (for example by Spider Web), this is =1
        Public OwnForesight As Integer = 0 'Own Ghost Pokémon can be hit by Normal/Fighting attacks
        Public OwnOdorSleuth As Integer = 0 'Same as Foresight
        Public OwnMiracleEye As Integer = 0 'Own Dark type Pokémon can be hit by Psychic type attacks
        Public OwnProtectMovesCount As Integer = 0 'Counts uses of protect moves
        Public OwnFuryCutter As Integer = 0 'Counter for the move fury cutter
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
        Public OwnSkyDropCounter As Integer = 0
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
        Public OwnLostItem As Item = Nothing
        Public OwnPursuit As Boolean = False
        Public OwnMegaEvolved As Boolean = False
        Public OwnRoostUsed As Boolean = False 'If roost got used, this is true and will get set false and revert types at the end of a turn.
        Public OwnDestinyBond As Boolean = False 'If own Pokémon used destiny bond, this is true. If the opponent knocks the own Pokémon out, it will faint as well.
        Public OwnHealingWish As Boolean = False 'True, if healing wish got used. Heals the next switched in Pokémon.
        Public OwnGastroAcid As Boolean = False 'If own Pokémon is affected by Gastro Acid

        Public OwnLastMove As Attack = Nothing 'Last move used
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

        'Opp stuff
        Public OppSpikes As Integer = 0
        Public OppStealthRock As Integer = 0
        Public OppStickyWeb As Integer = 0
        Public OppToxicSpikes As Integer = 0
        Public OppMist As Integer = 0
        Public OppGuardSpec As Integer = 0
        Public OppTurnCounts As Integer = 0
        Public OppLastMove As Attack = Nothing
        Public OppLightScreen As Integer = 0
        Public OppReflect As Integer = 0
        Public OppTailWind As Integer = 0
        Public OppSleepTurns As Integer = 0
        Public OppTruantRound As Integer = 0
        Public OppImprison As Integer = 0
        Public OppHealBlock As Integer = 0
        Public OppTaunt As Integer = 0
        Public OppRageCounter As Integer = 0
        Public OppUproar As Integer = 0
        Public OppFocusEnergy As Integer = 0
        Public OppEndure As Integer = 0
        Public OppProtectCounter As Integer = 0
        Public OppKingsShieldCounter As Integer = 0
        Public OppDetectCounter As Integer = 0
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
        Public OppLockOn As Integer = 0
        Public OppBideCounter As Integer = 0
        Public OppBideDamage As Integer = 0
        Public OppLansatBerry As Integer = 0
        Public OppCustapBerry As Integer = 0
        Public OppTrappedCounter As Integer = 0
        Public OppForesight As Integer = 0
        Public OppOdorSleuth As Integer = 0
        Public OppMiracleEye As Integer = 0
        Public OppProtectMovesCount As Integer = 0
        Public OppFuryCutter As Integer = 0
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
        Public OppLostItem As Item = Nothing
        Public OppPursuit As Boolean = False
        Public OppMegaEvolved As Boolean = False
        Public OppRoostUsed As Boolean = False
        Public OppDestinyBond As Boolean = False
        Public OppHealingWish As Boolean = False
        Public OppGastroAcid As Boolean = False

        Public OppFlyCounter As Integer = 0
        Public OppDigCounter As Integer = 0
        Public OppBounceCounter As Integer = 0
        Public OppDiveCounter As Integer = 0
        Public OppShadowForceCounter As Integer = 0
        Public OppSkyDropCounter As Integer = 0

        Public OppWrap As Integer = 0
        Public OppWhirlpool As Integer = 0
        Public OppBind As Integer = 0
        Public OppClamp As Integer = 0
        Public OppFireSpin As Integer = 0
        Public OppMagmaStorm As Integer = 0
        Public OppSandTomb As Integer = 0
        Public OppInfestation As Integer = 0

        Public OppUsedMoves As New List(Of Integer)

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

        'Special stuff
        Public RunTries As Integer = 0
        Public UsedPokemon As New List(Of Integer)
        Public StolenItemIDs As New List(Of Integer)
        Public DefeatedTrainerPokemon As Boolean = False
        Public RoamingFled As Boolean = False

        'BatonPassTemp:
        Public OwnUsedBatonPass As Boolean = False
        Public OwnBatonPassStats As List(Of Integer)
        Public OwnBatonPassConfusion As Boolean = False

        Public OppUsedBatonPass As Boolean = False
        Public OppBatonPassStats As List(Of Integer)
        Public OppBatonPassConfusion As Boolean = False

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
