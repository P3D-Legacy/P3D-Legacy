Namespace BattleSystem

    Public Class BattleCalculation

        Public Shared Function MovesFirst(ByVal BattleScreen As BattleScreen) As Boolean
            'Determine strike first with speed:
            Dim ownSpeed As Integer = DetermineBattleSpeed(True, BattleScreen)
            Dim oppSpeed As Integer = DetermineBattleSpeed(False, BattleScreen)

            If ownSpeed > oppSpeed Then 'Faster than opponent
                Return True
            ElseIf ownSpeed < oppSpeed Then 'Slower than opponent
                Return False
            Else 'Speed is equal, decide randomly
                If Core.Random.Next(0, 2) = 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

        Public Shared Function AttackFirst(ByVal ownAttack As Attack, ByVal oppAttack As Attack, ByVal BattleScreen As BattleScreen) As Boolean
            Dim ownPokemon As Pokemon = BattleScreen.OwnPokemon
            Dim oppPokemon As Pokemon = BattleScreen.OppPokemon

            Dim ownPriority As Integer = ownAttack.Priority
            Dim oppPriority As Integer = oppAttack.Priority

            If ownPokemon.Ability.Name.ToLower() = "prankster" Then
                If ownAttack.Category = Attack.Categories.Status Then
                    ownPriority += 1
                End If
            End If

            If oppPokemon.Ability.Name.ToLower() = "prankster" Then
                If oppAttack.Category = Attack.Categories.Status Then
                    oppPriority += 1
                End If
            End If

            If ownPokemon.Ability.Name.ToLower() = "gale wings" And ownAttack.Type.Type = Element.Types.Flying Then
                ownPriority += 1
            End If

            If oppPokemon.Ability.Name.ToLower() = "gale wings" And oppAttack.Type.Type = Element.Types.Flying Then
                oppPriority += 1
            End If

            If BattleScreen.FieldEffects.OwnCustapBerry > 0 Then
                ownPriority += 1
                BattleScreen.FieldEffects.OwnCustapBerry = 0
            End If

            If BattleScreen.FieldEffects.OppCustapBerry > 0 Then
                oppPriority += 1
                BattleScreen.FieldEffects.OppCustapBerry = 0
            End If

            'If the priority of your attack is higher, then attack first:
            If ownPriority > oppPriority Then
                Return True
            ElseIf oppPriority > ownPriority Then  'Same thing for opp's attack
                Return False
            Else 'Same priority
                'Quickclaw's 20% chance of hitting first:
                Dim Claw As Integer = 0

                If Not ownPokemon.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                    If ownPokemon.Item.ID = 73 Then
                        If Core.Random.Next(0, 100) < 20 Then
                            Claw += 1 'Own claw succeeded
                        End If
                    End If
                End If

                If Not oppPokemon.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                    If oppPokemon.Item.ID = 73 Then
                        If Core.Random.Next(0, 100) < 20 Then
                            Claw += 10 'Opp's claw succeeded
                        End If
                    End If
                End If

                Select Case Claw
                    Case 1 'You succeed claw
                        Return True
                    Case 10 'Opp succeed claw 
                        Return False
                    Case 11 'Both succeed claw, decide randomly
                        If Core.Random.Next(0, 2) = 0 Then
                            Return True
                        Else
                            Return False
                        End If
                End Select

                Dim Tail As Integer = 0 'Uses effect of Lagging Tail
                If Not ownPokemon.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                    If ownPokemon.Item.Name = "Lagging Tail" Then
                        Tail += 1
                    End If
                End If
                If Not oppPokemon.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                    If oppPokemon.Item.Name = "Lagging Tail" Then
                        Tail += 10
                    End If
                End If
                Select Case Tail
                    Case 1
                        Return False 'Your laggin tail effect works > You go last
                    Case 10
                        Return True 'Opponents lagging tail effect works > He goes last
                    Case 11
                        If Core.Random.Next(0, 2) = 0 Then 'Both laggin Tail works. > Random determination.
                            Return True
                        Else
                            Return False
                        End If
                End Select

                Dim Full As Integer = 0 'Uses effect of Full Incense
                If Not ownPokemon.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                    If ownPokemon.Item.Name = "Full Incense" Then
                        Full += 1
                    End If
                End If
                If Not oppPokemon.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                    If oppPokemon.Item.Name = "Full Incense" Then
                        Full += 10
                    End If
                End If
                Select Case Full
                    Case 1
                        Return False 'Your Full incense effect works > You go last
                    Case 10
                        Return True 'Opponents full incense effect works > He goes last
                    Case 11
                        If Core.Random.Next(0, 2) = 0 Then 'Both full incense works. > Random determination.
                            Return True
                        Else
                            Return False
                        End If
                End Select

                If ownPokemon.Ability.Name.ToLower() = "stall" And oppPokemon.Ability.Name.ToLower() <> "stall" Then
                    Return False
                End If
                If oppPokemon.Ability.Name.ToLower() = "stall" And ownPokemon.Ability.Name.ToLower() <> "stall" Then
                    Return True
                End If

                Dim _first As Boolean = False
                'Determine strike first with speed:
                Dim ownSpeed As Integer = DetermineBattleSpeed(True, BattleScreen)
                Dim oppSpeed As Integer = DetermineBattleSpeed(False, BattleScreen)

                If ownSpeed > oppSpeed Then 'Faster than opponent
                    _first = True
                ElseIf ownSpeed < oppSpeed Then 'Slower than opponent
                    _first = False
                Else 'Speed is equal, decide randomly
                    If Core.Random.Next(0, 2) = 0 Then
                        _first = True
                    Else
                        _first = False
                    End If
                End If
                If BattleScreen.FieldEffects.TrickRoom > 0 Then
                    _first = Not _first
                End If
                Return _first
            End If
        End Function

        Public Shared Function DetermineBattleSpeed(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            Dim speed As Integer = CInt(p.Speed * GetMultiplierFromStat(p.StatSpeed)) 'Calculate the speed's basic value from the speed and the speed stat

            If own = True Then
               If BattleScreen.IsPVPBattle = False Then  
                 If Core.Player.Badges.Contains(3) = True Then
                        speed = CInt(speed + (speed * (1 / 8))) 'Add 1/8 of the speed if the player has the 3rd badge and it's not a PvP battle
                    End If
                End If    
            End If

            If p.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Paralyzed And p.Ability.Name.ToLower() <> "quick feet" Then
                speed = CInt(speed / 4) 'Divide the speed by 4 if the Pokemon is paralyzed.
            End If

            If Not p.Item Is Nothing Then
                If p.Item.Name = "Choice Scarf" Then
                    speed = CInt(speed * 1.5F)
                End If

                Dim SlowDownItems As List(Of String) = {"iron ball", "macho brace", "power bracer", "power belt", "power lens", "power band", "power anklet", "power weight"}.ToList()
                If SlowDownItems.Contains(p.Item.Name.ToLower()) = True And BattleScreen.FieldEffects.CanUseItem(own) = True Then
                    speed = CInt(speed / 2)
                End If

                If own = True Then
                    If BattleScreen.FieldEffects.OwnTailWind > 0 Then
                        speed *= 2
                    End If
                Else
                    If BattleScreen.FieldEffects.OppTailWind > 0 Then
                        speed *= 2
                    End If
                End If

                If p.Number = 132 Then
                    If Not p.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(own) = True Then
                        If p.Item.Name = "Quick Powder" Then
                            speed *= 2
                        End If
                    End If
                End If
            End If

            Select Case p.Ability.Name.ToLower()
                Case "swift swim"
                    If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                        speed *= 2
                    End If
                Case "chlorophyll"
                    If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                        speed *= 2
                    End If
                Case "sand rush"
                    If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sandstorm Then
                        speed *= 2
                    End If
            End Select

            Dim grassPledge As Integer = BattleScreen.FieldEffects.OppGrassPledge
            If own = False Then
                grassPledge = BattleScreen.FieldEffects.OwnGrassPledge
            End If

            If grassPledge > 0 Then
                speed = CInt(speed / 2)
            End If

            If p.Ability.Name.ToLower() = "quick feet" Then
                If p.Status = Pokemon.StatusProblems.Paralyzed Or p.Status = Pokemon.StatusProblems.Burn Or p.Status = Pokemon.StatusProblems.Poison Or p.Status = Pokemon.StatusProblems.Sleep Or p.Status = Pokemon.StatusProblems.Freeze Then
                    speed = CInt(speed * 1.5F)
                End If
            End If

            If p.Ability.Name.ToLower() = "slow start" Then
                If own = True Then
                    If BattleScreen.FieldEffects.OwnTurnCounts < 5 Then
                        speed = CInt(speed / 2)
                    End If
                Else
                    If BattleScreen.FieldEffects.OppTurnCounts < 5 Then
                        speed = CInt(speed / 2)
                    End If
                End If
            End If

            speed = speed.Clamp(1, 999)

            Return speed
        End Function

        ''' <summary>
        ''' Outcome: 0=true/>1=false:1=sleeptalk/snore 2=other move 3=start sleep 4=X wont obey 5=X wont obey 6=X turned away 7=X is loafing around 8=X pretended to not notice
        ''' </summary>
        Public Shared Function ObedienceCheck(ByVal UsedAttack As Attack, BattleScreen As BattleScreen) As Integer
            Dim p As Pokemon = BattleScreen.OwnPokemon

            If p.OT = Core.Player.OT Then
                Return 0
            End If

            If BattleScreen.IsPVPBattle = True Then
                Return 0
            End If

            Dim badgeLevel As Integer = Badge.GetLevelCap()

            If badgeLevel > -1 And p.Level > badgeLevel And p.OT <> Core.Player.OT Then
                Dim r As Integer = Core.Random.Next(0, 256)
                Dim A As Integer = CInt((p.Level + badgeLevel * r) / 256)
                If A < badgeLevel Then
                    Return 0
                Else
                    BattleScreen.FieldEffects.OwnRageCounter = 0

                    Dim outcome As Integer = 0

                    If UsedAttack.Name.ToLower() = "snore" Or UsedAttack.Name.ToLower() = "sleep talk" Then
                        Return 1
                    Else
                        r = Core.Random.Next(0, 256)
                        Dim B As Integer = CInt((p.Level + badgeLevel) * r / 256)
                        If B < badgeLevel Then
                            Return 2
                        Else
                            Dim C As Integer = p.Level - badgeLevel
                            r = Core.Random.Next(0, 256)

                            Dim uproar As Integer = BattleScreen.FieldEffects.OwnUproar

                            If p.Ability.Name.ToLower() <> "insomnia" And p.Ability.Name.ToLower() <> "vital spirit" And uproar = 0 Then
                                If r < C Then
                                    Return 3
                                End If
                            Else
                                If r - C < C Then
                                    Return 4
                                Else
                                    Return 4 + Core.Random.Next(1, 5)
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                Return 0
            End If
            Return 0
        End Function

        Public Shared Function AccuracyCheck(ByVal UsedAttack As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If UsedAttack.Accuracy <= 0 Then
                Return True
            End If

            If p.Ability.Name.ToLower() = "no guard" Or op.Ability.Name.ToLower() = "no guard" Then
                Return True
            End If

            Dim result As Single = 1.0F

            Dim INIT As Integer = UsedAttack.GetAccuracy(own, BattleScreen)
            If op.Ability.Name.ToLower() = "wonder skin" And UsedAttack.Category = Attack.Categories.Status And UsedAttack.GetAccuracy(own, BattleScreen) > 0 And BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                INIT = 50
            End If

            If INIT < 0 Then
                INIT = 1
            End If

            Dim evasion As Integer = op.Evasion
            Dim accuracy As Integer = p.Accuracy

            If UsedAttack.UseOppEvasion = False Then
                evasion = 0
            End If

            Dim ACC As Integer = accuracy - evasion
            ACC = ACC.Clamp(-6, 6)

            Dim ACCM As Single = GetMultiplierFromAccEvasion(ACC)

            If UsedAttack.GetUseAccEvasion(own, BattleScreen) = False Then
                ACCM = 1.0F
            End If

            result = INIT * ACCM

            If Not op.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(Not own) = True Then
                If op.Item.Name.ToLower() = "brightpowder" Or op.Item.Name.ToLower() = "lax incense" Then
                    result *= 0.9F
                End If
            End If

            If Not p.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(own) = True Then
                Select Case p.Item.Name.ToLower()
                    Case "wide lens"
                        result *= 1.1F
                    Case "zoom lens"
                        Dim ownTurns As Integer = BattleScreen.FieldEffects.OwnTurnCounts
                        Dim oppTurns As Integer = BattleScreen.FieldEffects.OppTurnCounts
                        If own = False Then
                            ownTurns = BattleScreen.FieldEffects.OppTurnCounts
                            oppTurns = BattleScreen.FieldEffects.OwnTurnCounts
                        End If

                        If ownTurns < oppTurns Then
                            result *= 1.2F
                        End If
                End Select
            End If

            Select Case p.Ability.Name.ToLower()
                Case "compoundeyes"
                    result *= 1.3F
                Case "victory star"
                    result *= 1.1F
                Case "hustle"
                    If UsedAttack.Category = Attack.Categories.Physical Then
                        result *= 0.8F
                    End If
            End Select

            Select Case op.Ability.Name.ToLower()
                Case "sand veil"
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sandstorm Then
                            result *= 0.8F
                        End If
                    End If
                Case "snow cloak"
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Hailstorm Then
                            result *= 0.8F
                        End If
                    End If
                Case "tangled feet"
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        If op.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                            result *= 0.5F
                        End If
                    End If
            End Select

            If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Foggy Then
                result *= 0.6F
            End If

            If BattleScreen.FieldEffects.Gravity > 0 Then
                result *= CSng(10 / 6)
            End If

            Dim F As Integer = CInt(result).Clamp(0, 100)
            Dim R As Integer = Core.Random.Next(0, 100)

            If R < F Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function IsCriticalHit(ByVal UsedAttack As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            Dim C As Integer = 0

            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If UsedAttack.CriticalChance < 1 Then
                Return False
            End If

            If op.Ability.Name.ToLower() = "battle armor" Or op.Ability.Name.ToLower() = "shell armor" Then
                If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                    Return False
                End If
            End If

            Dim luckyChant As Integer = BattleScreen.FieldEffects.OppLuckyChant
            If own = False Then
                luckyChant = BattleScreen.FieldEffects.OwnLuckyChant
            End If
            If luckyChant > 0 Then
                Return False
            End If

            If p.Ability.Name.ToLower() = "super luck" Then
                C += 1
            End If

            Dim focusEnergy As Integer = BattleScreen.FieldEffects.OwnFocusEnergy
            If own = False Then
                focusEnergy = BattleScreen.FieldEffects.OppFocusEnergy
            End If

            If focusEnergy > 0 Then
                C += 2
            End If

            If UsedAttack.CriticalChance > 1 Then
                C += 1
            End If

            Dim lansat As Integer = BattleScreen.FieldEffects.OwnLansatBerry
            If own = False Then
                lansat = BattleScreen.FieldEffects.OppLansatBerry
            End If

            If lansat > 0 Then
                C += 2
            End If

            If Not p.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(own) = True Then
                Select Case p.Item.Name.ToLower()
                    Case "lucky punch"
                        If p.Number = 113 Then
                            C += 2
                        End If
                    Case "stick"
                        If p.Number = 83 Then
                            C += 2
                        End If
                    Case "scope lens"
                        C += 1
                    Case "razor claw"
                        C += 1
                End Select
            End If

            Dim chance As Integer = 16
            Select Case C
                Case 0
                    chance = 16
                Case 1
                    chance = 8
                Case 2
                    chance = 4
                Case 3
                    chance = 3
                Case Else
                    chance = 2
            End Select

            If Core.Random.Next(0, chance) = 0 Then
                Return True
            End If
            If UsedAttack.ID = 524 Then
                Return True
            End If
            
            Return False
        End Function

        Public Shared Function CanRun(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            If BattleScreen.BattleMode = BattleScreen.BattleModes.Safari Then
                Return True
            End If

            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.Ability.Name.ToLower() = "run away" Then
                Return True
            End If

            If op.Ability.Name.ToLower() = "shadow tag" And p.Ability.Name.ToLower() <> "shadow tag" Then
                Return False
            End If

            If op.Ability.Name.ToLower() = "arena trap" Then
                Dim magnetRise As Integer = BattleScreen.FieldEffects.OwnMagnetRise
                If own = False Then
                    magnetRise = BattleScreen.FieldEffects.OppMagnetRise
                End If

                If p.Type1.Type <> Element.Types.Flying And p.Type2.Type <> Element.Types.Flying And p.Ability.Name.ToLower() <> "levitate" And magnetRise = 0 Then
                    Return False
                End If
            End If

            If op.Ability.Name.ToLower() = "magnet pull" Then
                If p.Type1.Type = Element.Types.Steel Or p.Type2.Type = Element.Types.Steel Then
                    Return False
                End If
            End If

            Dim ingrain As Integer = BattleScreen.FieldEffects.OwnIngrain
            If own = False Then
                ingrain = BattleScreen.FieldEffects.OppIngrain
            End If
            If ingrain > 0 Then
                Return False
            End If

            If p.Speed > op.Speed Then
                Return True
            Else
                If Not p.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(own) = True Then
                    If p.Item.Name.ToLower() = "smoke ball" Then
                        BattleScreen.FieldEffects.RunTries += 1
                        Return True
                    End If
                End If

                Dim A As Integer = p.Speed
                Dim B As Integer = op.Speed
                If B = 0 Then
                    B = 1
                End If
                Dim C As Integer = BattleScreen.FieldEffects.RunTries
                Dim X As Integer = (CInt(A * 128 / B) + (30 * C)) Mod 256
                Dim R As Integer = Core.Random.Next(0, 256)

                If R < X Then
                    Return True
                Else
                    BattleScreen.FieldEffects.RunTries += 1
                    Return False
                End If
            End If
        End Function

        Public Shared Function CalculateEffectiveness(ByVal move As Attack, ByVal BattleScreen As BattleScreen, ByVal p As Pokemon, ByVal op As Pokemon, ByVal own As Boolean) As Single
            Dim Type1 As Single = ReverseTypeEffectiveness(Element.GetElementMultiplier(move.GetAttackType(own, BattleScreen), op.Type1))
            Dim Type2 As Single = ReverseTypeEffectiveness(Element.GetElementMultiplier(move.GetAttackType(own, BattleScreen), op.Type2))
            Dim effectiveness As Single = Type1 * Type2

            'Freeze Dry
            If move.ID = 573 Then 
                If op.Type1.Type = Element.Types.Water Or op.Type2.Type = Element.Types.Water Then
                    effectiveness *= 4
                End If
            End If

            Dim _targetHasIronBall As Boolean = False
            If Not op.Item Is Nothing Then
                If op.Item.Name.ToLower() = "iron ball" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                    _targetHasIronBall = True
                End If
            End If

            If op.Ability.Name.ToLower() = "levitate" And move.GetAttackType(own, BattleScreen).Type = Element.Types.Ground And BattleScreen.FieldEffects.Gravity = 0 And _targetHasIronBall = False Then
                If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                    effectiveness = 0.0F
                End If
            End If

            Dim ingrain As Integer = BattleScreen.FieldEffects.OppIngrain
            If own = False Then
                ingrain = BattleScreen.FieldEffects.OwnIngrain
            End If

            If move.GetAttackType(own, BattleScreen).Type = Element.Types.Ground Then
                If BattleScreen.FieldEffects.Gravity = 0 And ingrain = 0 And _targetHasIronBall = False Then
                    Dim magnetRise As Integer = BattleScreen.FieldEffects.OppMagnetRise
                    If own = False Then
                        magnetRise = BattleScreen.FieldEffects.OwnMagnetRise
                    End If
                    If magnetRise > 0 Then
                        effectiveness = 0.0F
                    End If
                End If
            End If

            If p.Ability.Name.ToLower() = "scrappy" Then
                If op.Type1.Type = Element.Types.Ghost Or op.Type2.Type = Element.Types.Ghost Then
                    If effectiveness = 0 Then
                        effectiveness = 1.0F
                    End If
                End If
            End If

            If Not op.Item Is Nothing Then
                If op.Item.Name.ToLower() = "ring target" And BattleScreen.FieldEffects.CanUseItem(Not own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Not own, BattleScreen) = True Then
                    If Type1 = 0 Then
                        effectiveness = Type2
                    End If
                    If Type2 = 0 Then
                        effectiveness = Type1
                    End If
                    If effectiveness = 0 Then
                        effectiveness = 1.0F
                    End If
                End If
            End If

            If move.ImmunityAffected = False And effectiveness = 0 Then
                If Type1 = 0.0F Then
                    effectiveness = Type2
                End If
                If Type2 = 0.0F Then
                    effectiveness = Type1
                End If
                If effectiveness = 0.0F Then
                    effectiveness = 1.0F
                End If
            End If

            If op.IsType(Element.Types.Ghost) = True Then
                Dim foresight As Integer = BattleScreen.FieldEffects.OppForesight
                Dim odorSleught As Integer = BattleScreen.FieldEffects.OppOdorSleuth
                If own = False Then
                    foresight = BattleScreen.FieldEffects.OwnForesight
                    odorSleught = BattleScreen.FieldEffects.OwnOdorSleuth
                End If

                If foresight > 0 Or odorSleught > 0 Then
                    If move.Type.Type = Element.Types.Normal Or move.Type.Type = Element.Types.Fighting Then
                        If Type1 = 0.0F Then
                            effectiveness = Type2
                        End If
                        If Type2 = 0.0F Then
                            effectiveness = Type1
                        End If
                        If effectiveness = 0.0F Then
                            effectiveness = 1.0F
                        End If
                    End If
                End If
            End If

            If op.IsType(Element.Types.Dark) = True Then
                Dim miracleEye As Integer = BattleScreen.FieldEffects.OppMiracleEye
                If own = False Then
                    miracleEye = BattleScreen.FieldEffects.OwnMiracleEye
                End If

                If miracleEye > 0 Then
                    If move.Type.Type = Element.Types.Psychic Then
                        If Type1 = 0.0F Then
                            effectiveness = Type2
                        End If
                        If Type2 = 0.0F Then
                            effectiveness = Type1
                        End If
                        If effectiveness = 0.0F Then
                            effectiveness = 1.0F
                        End If
                    End If
                End If
            End If

            If move.UseEffectiveness = False And effectiveness <> 0.0F Then
                Return 1.0F
            End If

            Return effectiveness
        End Function

        Public Shared Function CalculateEffectiveness(ByVal own As Boolean, ByVal move As Attack, ByVal BattleScreen As BattleScreen) As Single
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Return CalculateEffectiveness(move, BattleScreen, p, op, own)
        End Function

        Public Shared Function GainExp(ByVal p As Pokemon, ByVal BattleScreen As BattleScreen, ByVal PokemonList As List(Of Integer)) As Integer
            Dim op As Pokemon = BattleScreen.OppPokemon

            Dim a As Double = 1D
            If BattleScreen.IsTrainerBattle = True Then
                a = 1.5D
            End If

            Dim b As Double = op.BaseExperience

            Dim t As Double = 1D
            If p.OT <> Core.Player.OT Then
                t = 1.5D
            End If

            Dim e As Double = 1D
            If Not p.Item Is Nothing Then
                If p.Item.Name.ToLower() = "lucky egg" Then
                    e = 1.5D
                End If
            End If

            Dim L As Double = op.Level
            Dim Lp As Double = p.Level

            Dim s As Double = PokemonList.Count

            Dim expShares As Integer = 0
            For Each po As Pokemon In Core.Player.Pokemons
                If Not po.Item Is Nothing Then
                    If po.Item.Name.ToLower() = "exp share" Then
                        expShares += 1
                    End If
                End If
            Next

            If expShares > 0 Then
                If Not p.Item Is Nothing Then
                    If p.Item.Name.ToLower() = "exp share" Then
                        s = 2D
                    Else
                        s = (PokemonList.Count * 2D) * expShares
                    End If
                End If
            End If

            Dim EXP As Integer = CInt((((a * b * L) / (5 * s)) * (((2 * L + 10) ^ 2.5D) / ((L + Lp + 10) ^ 2.5D)) + 1) * t * e * 1)

            If EXP < 2 Then
                EXP = 2
            End If

            For Each mysteryEvent As MysteryEventScreen.MysteryEvent In MysteryEventScreen.ActivatedMysteryEvents
                If mysteryEvent.EventType = MysteryEventScreen.EventTypes.EXPMultiplier Then
                    EXP = CInt(EXP * CSng(mysteryEvent.Value.Replace(".", GameController.DecSeparator)))
                End If
            Next

            Return EXP
        End Function

        Public Shared Function SafariRound(ByVal BattleScreen As BattleScreen) As Battle.RoundConst
            If BattleScreen.PokemonSafariStatus > 0 Then
                BattleScreen.PokemonSafariStatus -= 1
            ElseIf BattleScreen.PokemonSafariStatus < 0 Then
                BattleScreen.PokemonSafariStatus += 1
            End If

            Dim flee As Boolean = False

            Dim X As Single = BattleScreen.OppPokemon.Speed Mod 256
            X *= 2
            If X > 255 Then
                flee = True
            End If

            If flee = False Then
                If BattleScreen.PokemonSafariStatus < 0 Then
                    X *= 2
                ElseIf BattleScreen.PokemonSafariStatus > 0 Then
                    X /= 4
                End If
            End If

            Dim R As Integer = Core.Random.Next(0, 256)
            If R < X Then
                flee = True
            End If

            If flee = True And CanSwitch(BattleScreen, False) = True Then
                Return New Battle.RoundConst() With {.StepType = Battle.RoundConst.StepTypes.Flee, .Argument = BattleScreen.OppPokemon.GetDisplayName() & " fled!"}
            Else
                If BattleScreen.PokemonSafariStatus < 0 Then
                    Return New Battle.RoundConst() With {.StepType = Battle.RoundConst.StepTypes.Text, .Argument = BattleScreen.OppPokemon.GetDisplayName() & " is angry!"}
                ElseIf BattleScreen.PokemonSafariStatus > 0 Then
                    Return New Battle.RoundConst() With {.StepType = Battle.RoundConst.StepTypes.Text, .Argument = BattleScreen.OppPokemon.GetDisplayName() & " is eating!"}
                Else
                    Return New Battle.RoundConst() With {.StepType = Battle.RoundConst.StepTypes.Text, .Argument = BattleScreen.OppPokemon.GetDisplayName() & " is watching carefully!"}
                End If
            End If
        End Function

        Public Shared Function CanSwitch(ByVal BattleScreen As BattleScreen, ByVal own As Boolean) As Boolean
            'Abilities: Shadow Tag, Arena Trap, Magnet Pull
            'Items
            'Used Moves + Trapping Moves: Block, Mean Look, Spider Web, Ingrain
            'Trapping: Bind, Clamp, Fire Spin, Infestation, Magma Storm, Sand Tomb, Whirlpool, Wrap

            If own = True Then
                If BattleScreen.OppPokemon.Ability.Name.ToLower() = "shadow tag" And BattleScreen.OwnPokemon.Ability.Name.ToLower() <> "shadow tag" Then
                    Return False
                End If
                If BattleScreen.FieldEffects.OwnTrappedCounter > 0 Then
                    Return False
                End If

                If BattleScreen.OppPokemon.Ability.Name.ToLower() = "arena trap" Then
                    Dim magnetRise As Integer = BattleScreen.FieldEffects.OwnMagnetRise

                    If BattleScreen.OwnPokemon.IsType(Element.Types.Flying) = False And BattleScreen.OwnPokemon.Ability.Name.ToLower() <> "levitate" And magnetRise = 0 Then
                        Return False
                    End If
                End If

                If BattleScreen.OppPokemon.Ability.Name.ToLower() = "magnet pull" And BattleScreen.OwnPokemon.IsType(Element.Types.Ghost) = False And BattleScreen.OwnPokemon.IsType(Element.Types.Steel) = True Then
                    Return False
                End If

                With BattleScreen.FieldEffects
                    If .OwnWrap > 0 Or .OwnBind > 0 Or .OwnClamp > 0 Or .OwnFireSpin > 0 Or .OwnMagmaStorm > 0 Or .OwnSandTomb > 0 Or .OwnWhirlpool > 0 Or .OwnInfestation > 0 Then
                        Return False
                    End If
                End With

                If BattleScreen.FieldEffects.OwnIngrain > 0 Then
                    Return False
                End If
            Else
                If BattleScreen.OwnPokemon.Ability.Name.ToLower() = "shadow tag" And BattleScreen.OppPokemon.Ability.Name.ToLower() <> "shadow tag" Then
                    Return False
                End If
                If BattleScreen.FieldEffects.OppTrappedCounter > 0 Then
                    Return False
                End If

                If BattleScreen.OwnPokemon.Ability.Name.ToLower() = "arena trap" Then
                    Dim magnetRise As Integer = BattleScreen.FieldEffects.OppMagnetRise

                    If BattleScreen.OppPokemon.IsType(Element.Types.Flying) = False And BattleScreen.OppPokemon.Ability.Name.ToLower() <> "levitate" And magnetRise = 0 Then
                        Return False
                    End If
                End If

                If BattleScreen.OwnPokemon.Ability.Name.ToLower() = "magnet pull" And BattleScreen.OppPokemon.IsType(Element.Types.Ghost) = False And BattleScreen.OppPokemon.IsType(Element.Types.Steel) = True Then
                    Return False
                End If

                With BattleScreen.FieldEffects
                    If .OppWrap > 0 Or .OppBind > 0 Or .OppClamp > 0 Or .OppFireSpin > 0 Or .OppMagmaStorm > 0 Or .OppSandTomb > 0 Or .OppWhirlpool > 0 Or .OppInfestation > 0 Then
                        Return False
                    End If
                End With

                If BattleScreen.FieldEffects.OppIngrain > 0 Then
                    Return False
                End If
            End If

            Return True
        End Function

        Public Shared Function CalculateDamage(ByVal Attack As Attack, ByVal Critical As Boolean, ByVal Own As Boolean, ByVal targetPokemon As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Dim p As Pokemon = Nothing
            Dim Op As Pokemon = Nothing
            If Own = True Then
                p = BattleScreen.OwnPokemon
            Else
                p = BattleScreen.OppPokemon
            End If
            If targetPokemon = True Then
                Op = BattleScreen.OwnPokemon
            Else
                Op = BattleScreen.OppPokemon
            End If

            Dim damage As Integer = 0

            Dim Level As Integer = p.Level

            'BASE POWER:
            Dim BasePower As Integer = 1

            Dim HH As Single = 1.0F 'Helping hand = 1.5
            Dim BP As Single = Attack.GetBasePower(Own, BattleScreen)
            Dim IT As Single = 1.0F
            Dim CHG As Single = 1.0F
            Dim MS As Single = 1.0F
            Dim WS As Single = 1.0F
            Dim UA As Single = 1.0F
            Dim FA As Single = 1.0F

            'IT (Item attack power modifier)
            If Not p.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Own, BattleScreen) = True Then
                Select Case p.Item.Name.ToLower()
                    Case "muscle band"
                        If Attack.Category = Attack.Categories.Physical Then
                            IT = 1.1F
                        End If
                    Case "wise glasses"
                        If Attack.Category = Attack.Categories.Special Then
                            IT = 1.1F
                        End If
                    Case "adamant orb"
                        If p.Number = 483 Then
                            If Attack.Type.Type = Element.Types.Dragon Or Attack.Type.Type = Element.Types.Steel Then
                                IT = 1.2F
                            End If
                        End If
                    Case "lustrous orb"
                        If p.Number = 484 Then
                            If Attack.Type.Type = Element.Types.Dragon Or Attack.Type.Type = Element.Types.Water Then
                                IT = 1.2F
                            End If
                        End If
                    Case "griseous orb"
                        If p.Number = 487 Then
                            If Attack.Type.Type = Element.Types.Dragon Or Attack.Type.Type = Element.Types.Ghost Then
                                IT = 1.2F
                            End If
                        End If
                    Case Else
                        IT = 1.0F
                End Select
                Select Case p.Item.ID
                    Case 98, 270 'Black Belt, Fist Plate
                        If Attack.Type.Type = Element.Types.Fighting Then
                            IT = 1.2F
                        End If
                    Case 102, 268 'Black Glasses, Dread Plate
                        If Attack.Type.Type = Element.Types.Dark Then
                            IT = 1.2F
                        End If
                    Case 138, 271 'Charcoal, Flame Plate
                        If Attack.Type.Type = Element.Types.Fire Then
                            IT = 1.2F
                        End If
                    Case 144, 267 'Dragon Fang, Draco Plate
                        If Attack.Type.Type = Element.Types.Dragon Then
                            IT = 1.2F
                        End If
                    Case 125, 281, 286 'Hard Stone, Stone Plate, Rock Incense
                        If Attack.Type.Type = Element.Types.Rock Then
                            IT = 1.2F
                        End If
                    Case 108, 283 'Magnet, Zap Plate
                        If Attack.Type.Type = Element.Types.Electric Then
                            IT = 1.2F
                        End If
                    Case 143, 274 'Metal Coat, Iron Plate
                        If Attack.Type.Type = Element.Types.Steel Then
                            IT = 1.2F
                        End If
                    Case 117, 275, 287 'Miracle Seed, Meadow Plate, Rose Incense
                        If Attack.Type.Type = Element.Types.Grass Then
                            IT = 1.2F
                        End If
                    Case 95, 145, 264, 279 'Mystic Water, Wave Incense, Sea Incense, Splash Plate
                        If Attack.Type.Type = Element.Types.Water Then
                            IT = 1.2F
                        End If
                    Case 107, 272 'NeverMeltIce, Icicle Plate
                        If Attack.Type.Type = Element.Types.Ice Then
                            IT = 1.2F
                        End If
                    Case 104, 277 'Pink Bow, Pixie Plate
                        If Attack.Type.Type = Element.Types.Fairy Then
                            IT = 1.2F
                        End If
                    Case 81, 282 'Poison Barb, Toxic Plate
                        If Attack.Type.Type = Element.Types.Poison Then
                            IT = 1.2F
                        End If
                    Case 77, 278 'Sharp Beak, Sky Plate
                        If Attack.Type.Type = Element.Types.Flying Then
                            IT = 1.2F
                        End If
                    Case 88, 273 'Silver powder, Insect Plate
                        If Attack.Type.Type = Element.Types.Bug Then
                            IT = 1.2F
                        End If
                    Case 76, 269 'Soft Sand, Earth Plate
                        If Attack.Type.Type = Element.Types.Ground Then
                            IT = 1.2F
                        End If
                    Case 113, 280 'Spell Tag, Spooky Plate
                        If Attack.Type.Type = Element.Types.Ghost Then
                            IT = 1.2F
                        End If
                    Case 96, 263, 276 'Twisted Spoon, Odd Incense, Mind Plate
                        If Attack.Type.Type = Element.Types.Psychic Then
                            IT = 1.2F
                        End If
                End Select
            End If
            'CHG (If used Charge)
            Dim lastMove As Attack = BattleScreen.FieldEffects.OwnLastMove
            If Own = False Then
                lastMove = BattleScreen.FieldEffects.OppLastMove
            End If
            If Not lastMove Is Nothing Then
                If lastMove.Name.ToLower() = "charge" And Attack.Type.Type = Element.Types.Electric Then
                    CHG = 2.0F
                End If
            End If
            'MS
            If BattleScreen.FieldEffects.MudSport > 0 And Attack.Type.Type = Element.Types.Electric Then
                MS = 0.5F
            End If
            'WA
            If BattleScreen.FieldEffects.WaterSport > 0 And Attack.Type.Type = Element.Types.Fire Then
                WS = 0.5F
            End If
            'UA (User Ability)
            Select Case p.Ability.Name.ToLower()
                Case "rivalry"
                    If p.Gender <> Pokemon.Genders.Genderless And Op.Gender <> Pokemon.Genders.Genderless Then
                        If p.Gender = Op.Gender Then
                            UA = 1.25F
                        Else
                            UA = 0.75F
                        End If
                    End If
                Case "reckless"
                    If Attack.IsRecoilMove = True Then
                        UA = 1.2F
                    End If
                Case "iron fist"
                    If Attack.IsPunchingMove = True Then
                        UA = 1.2F
                    End If
                Case "blaze"
                    If p.HP < CInt(Math.Floor(p.MaxHP / 3)) And Attack.Type.Type = Element.Types.Fire Then
                        UA = 1.5F
                    End If
                Case "overgrow"
                    If p.HP < CInt(Math.Floor(p.MaxHP / 3)) And Attack.Type.Type = Element.Types.Grass Then
                        UA = 1.5F
                    End If
                Case "torrent"
                    If p.HP < CInt(Math.Floor(p.MaxHP / 3)) And Attack.Type.Type = Element.Types.Water Then
                        UA = 1.5F
                    End If
                Case "swarm"
                    If p.HP < CInt(Math.Floor(p.MaxHP / 3)) And Attack.Type.Type = Element.Types.Bug Then
                        UA = 1.5F
                    End If
                Case "technician"
                    If Attack.GetBasePower(Own, BattleScreen) <= 60 Then
                        UA = 1.5F
                    End If
                Case "sheer force"
                    If Attack.HasSecondaryEffect = True Then
                        UA = 1.3F
                    End If
                Case "analytic"
                    If Own = True Then
                        If BattleScreen.FieldEffects.OwnTurnCounts < BattleScreen.FieldEffects.OppTurnCounts Then
                            UA = 1.3F
                        End If
                    Else
                        If BattleScreen.FieldEffects.OwnTurnCounts > BattleScreen.FieldEffects.OppTurnCounts Then
                            UA = 1.3F
                        End If
                    End If
                Case "sand force"
                    If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sandstorm Then
                        If Attack.Type.Type = Element.Types.Rock Or Attack.Type.Type = Element.Types.Ground Or Attack.Type.Type = Element.Types.Steel Then
                            UA = 1.3F
                        End If
                    End If
                Case "strong jaw"
                    If Attack.IsJawMove = True Then
                        UA = 1.5F
                    End If
                Case "fur coat"
                    If Attack.Category = Attack.Categories.Physical Then
                        UA = 0.5F
                    End If
                Case "refrigerate"
                    If Attack.Type.Type = Element.Types.Normal Then
                        UA = 1.3F
                    End If
                Case "pixilate"
                    If Attack.Type.Type = Element.Types.Normal Then
                        UA = 1.3F
                    End If
                Case "aerilate"
                    If Attack.Type.Type = Element.Types.Normal Then
                        UA = 1.3F
                    End If
                Case "mega launcher"
                    If Attack.IsPulseMove = True Then
                        UA = 1.5F
                    End If
                Case "tough claws"
                    If Attack.MakesContact = True Then
                        UA = 1.3F
                    End If
                Case "dark aura"
                    If Attack.Type.Type = Element.Types.Dark Then
                        UA = 1.3F
                    End If
                Case "fairy aura"
                    If Attack.Type.Type = Element.Types.Fairy Then
                        UA = 1.3F
                    End If
                Case Else
                    UA = 1.0F
            End Select
            
            'FA (Foe ability)
            Select Case Op.Ability.Name.ToLower()
                Case "thick fat"
                    If BattleScreen.FieldEffects.CanUseAbility(Not Own, BattleScreen) = True Then
                        If Attack.Type.Type = Element.Types.Fire Or Attack.Type.Type = Element.Types.Ice Then
                            FA = 0.5F
                        End If
                    End If
                Case "heatproof"
                    If BattleScreen.FieldEffects.CanUseAbility(Own, BattleScreen) = True Then
                        If Attack.Type.Type = Element.Types.Fire Then
                            FA = 0.5F
                        End If
                    End If
                Case "dry skin"
                    If BattleScreen.FieldEffects.CanUseAbility(Not Own, BattleScreen) = True Then
                        If Attack.Type.Type = Element.Types.Fire Then
                            FA = 1.25F
                        End If
                    End If
                Case "multiscale"
                    If Op.HP = Op.MaxHP Then
                        FA = 0.5F
                    End If
                Case "dark aura"
                    If Attack.Type.Type = Element.Types.Dark Then
                        FA = 1.3F
                    End If
                Case "fairy aura"
                    If Attack.Type.Type = Element.Types.Fairy Then
                        FA = 1.3F
                    End If
                Case Else
                    FA = 1.0F
            End Select

            BasePower = CInt(Math.Floor(HH * BP * IT * CHG * MS * WS * UA * FA))

            'ATK:
            Dim Atk As Integer = 1

            Dim AStat As Single = 1.0F
            Dim ASM As Single = 1.0F
            Dim AM As Single = 1.0F
            Dim IM As Single = 1.0F

            If Attack.Category = Attack.Categories.Physical Then
                If Attack.ID = 492 Then
                    AStat = Attack.GetUseAttackStat(Op) 'When the move is Foul Play
                    ASM = GetMultiplierFromStat(Op.StatAttack)
                Else
                    AStat = Attack.GetUseAttackStat(p)
                    ASM = GetMultiplierFromStat(p.StatAttack)
                End If

                If BattleScreen.FieldEffects.CanUseAbility(Not Own, BattleScreen) = True Then
                    If Op.Ability.Name.ToLower() = "unaware" Then
                        ASM = 1.0F
                    End If
                End If

                Select Case p.Ability.Name.ToLower()
                    Case "pure power"
                        AM = 2.0F
                    Case "huge power"
                        AM = 2.0F
                    Case "flower gift"
                        If BattleScreen.FieldEffects.CanUseAbility(Own, BattleScreen) = True Then
                            If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                                AM = 1.5F
                            End If
                        End If
                    Case "guts"
                        If p.Status = Pokemon.StatusProblems.Paralyzed Or p.Status = Pokemon.StatusProblems.Poison Or p.Status = Pokemon.StatusProblems.Burn Or p.Status = Pokemon.StatusProblems.Sleep Or p.Status = Pokemon.StatusProblems.BadPoison Then
                            AM = 1.5F
                        End If
                    Case "hustle"
                        AM = 1.5F
                    Case "slow start"
                        If Own = True Then
                            If BattleScreen.FieldEffects.OwnTurnCounts < 5 Then
                                AM = 0.5F
                            End If
                        Else
                            If BattleScreen.FieldEffects.OppTurnCounts < 5 Then
                                AM = 0.5F
                            End If
                        End If
                    Case "toxic boost"
                        If p.Status = Pokemon.StatusProblems.Poison Or p.Status = Pokemon.StatusProblems.BadPoison Then
                            AM = 1.5F
                        End If
                    Case "defeatist"
                        If p.HP <= CInt(p.MaxHP / 2) Then
                            AM = 0.5F
                        End If
                    Case Else
                        AM = 1.0F
                End Select

                If Not p.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Own, BattleScreen) = True Then
                    Select Case p.Item.Name.ToLower()
                        Case "choice band"
                            IM = 1.5F
                        Case "light ball"
                            If p.Number = 25 Then
                                IM = 2.0F
                            End If
                        Case "thick club"
                            If p.Number = 104 Or p.Number = 105 Then
                                IM = 2.0F
                            End If
                        Case Else
                            IM = 1.0F
                    End Select
                End If
            ElseIf Attack.Category = Attack.Categories.Special Then
                AStat = Attack.GetUseAttackStat(p)
                ASM = GetMultiplierFromStat(p.StatSpAttack)

                If Op.Ability.Name.ToLower() = "unaware" Then
                    ASM = 1.0F
                End If

                Select Case p.Ability.Name.ToLower()
                    Case "plus"
                        AM = 1.0F
                    Case "minus"
                        AM = 1.0F
                    Case "solar power"
                        If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                            AM = 1.5F
                        End If
                    Case "flare boost"
                        If Op.Status = Pokemon.StatusProblems.Burn Then
                            AM = 1.5F
                        End If
                    Case "defeatist"
                        If p.HP <= CInt(p.MaxHP / 2) Then
                            AM = 0.5F
                        End If
                    Case Else
                        AM = 1.0F
                End Select

                If Not p.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Own, BattleScreen) = True Then
                    Select Case p.Item.Name.ToLower()
                        Case "choice specs"
                            IM = 1.5F
                        Case "light ball"
                            If p.Number = 25 Then
                                IM = 2.0F
                            End If
                        Case "soul dew"
                            If p.Number = 380 Or p.Number = 381 Then
                                IM = 1.5F
                            End If
                        Case "deepseatooth"
                            If p.Number = 366 Then
                                IM = 2.0F
                            End If
                        Case "metronome"
                            Dim lastAttack As Attack = Nothing
                            If Own = False Then
                                If Not BattleScreen.FieldEffects.OppLastMove Is Nothing Then
                                    lastAttack = BattleScreen.FieldEffects.OppLastMove
                                End If
                            Else
                                If Not BattleScreen.FieldEffects.OwnLastMove Is Nothing Then
                                    lastAttack = BattleScreen.FieldEffects.OwnLastMove
                                End If
                            End If
                            If Not lastAttack Is Nothing Then
                                If lastAttack.ID = Attack.ID Then
                                    Dim multi As Integer = 1
                                    If Own = True Then
                                        BattleScreen.FieldEffects.OwnMetronomeItemCount += 1
                                        multi = BattleScreen.FieldEffects.OwnMetronomeItemCount
                                    Else
                                        BattleScreen.FieldEffects.OppMetronomeItemCount += 1
                                        multi = BattleScreen.FieldEffects.OppMetronomeItemCount
                                    End If
                                    multi = multi.Clamp(1, 10)
                                    IM = 1.0F + CSng(multi / 10)
                                Else
                                    If Own = True Then
                                        BattleScreen.FieldEffects.OwnMetronomeItemCount = 0
                                    Else
                                        BattleScreen.FieldEffects.OppMetronomeItemCount = 0
                                    End If
                                    IM = 1.0F
                                End If
                            Else
                                If Own = True Then
                                    BattleScreen.FieldEffects.OwnMetronomeItemCount = 0
                                Else
                                    BattleScreen.FieldEffects.OppMetronomeItemCount = 0
                                End If
                                IM = 1.0F
                            End If
                        Case Else
                            IM = 1.0F
                    End Select
                End If
            End If
            
            'Critical hit interaction with attack stat change
            If ASM < 1.0F AndAlso Critical = True Then
                ASM = 1.0F
            End If

            Atk = CInt(Math.Floor(AStat * ASM * AM * IM))
            'DEF
            Dim Def As Integer = 1

            Dim DStat As Single = 1.0F
            Dim DSM As Single = 1.0F
            Dim SX As Single = 1.0F
            Dim DMod As Single = 1.0F

            If Attack.Category = Attack.Categories.Physical OrElse Attack.ID = 473 OrElse Attack.ID = 548 Then 'Psyshock and Secret Sword.
                DStat = Attack.GetUseDefenseStat(Op)
                DSM = GetMultiplierFromStat(Op.StatDefense)

                If p.Ability.Name.ToLower() = "unaware" Then
                    DSM = 1.0F
                End If

                If Attack.Name.ToLower() = "selfdestruct" Or Attack.Name.ToLower() = "explosion" Then
                    SX = 0.5F
                End If

                If Not Op.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(Not Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Not Own, BattleScreen) = True Then
                    Select Case Op.Item.Name.ToLower()
                        Case "metal powder"
                            If Op.Number = 132 Then
                                DMod = 1.5F
                            End If
                    End Select
                End If

                If Op.Ability.Name.ToLower() = "marvel scale" Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not Own, BattleScreen) = True Then
                        If Op.Status = Pokemon.StatusProblems.Paralyzed Or Op.Status = Pokemon.StatusProblems.Poison Or Op.Status = Pokemon.StatusProblems.Burn Or Op.Status = Pokemon.StatusProblems.Sleep Or Op.Status = Pokemon.StatusProblems.Freeze Then
                            DMod = 1.5F
                        End If
                    End If
                End If
            ElseIf Attack.Category = Attack.Categories.Special Then
                DStat = Attack.GetUseDefenseStat(Op)
                DSM = GetMultiplierFromStat(Op.StatSpDefense)

                If p.Ability.Name.ToLower() = "unaware" Or Attack.UseOppDefense = False Then
                    DSM = 1.0F
                End If

                If Attack.Name.ToLower() = "selfdestruct" Or Attack.Name.ToLower() = "explosion" Then
                    SX = 0.5F
                End If

                If Op.Ability.Name.ToLower() = "flower gift" Then
                    If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                        DMod = 1.5F
                    End If
                End If

                If Not Op.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(Not Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Not Own, BattleScreen) = True Then
                    Select Case Op.Item.Name.ToLower()
                        Case "soul dew"
                            If p.Number = 380 Or p.Number = 381 Then
                                DMod = 1.5F
                            End If
                        Case "metal powder"
                            If Op.Number = 132 Then
                                DMod = 1.5F
                            End If
                        Case "deepseascale"
                            If p.Number = 366 Then
                                DMod = 2.0F
                            End If
                    End Select
                End If

                If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sandstorm Then
                    If Op.Type1.Type = Element.Types.Rock Or Op.Type2.Type = Element.Types.Rock Then
                        DMod = 1.5F
                    End If
                End If
            End If
            
            If DSM > 1.0F AndAlso Critical = True Then
                DSM = 1.0F
            End If
            
            'Sacred Sword ignores defense stat changes
            If Attack.ID = 533 Then
                DSM = 1.0F
            End If
            
            Def = CInt(Math.Floor(DStat * DSM * DMod * SX))

            If Def <= 0 Then
                Def = 1
            End If

            'Mod1
            Dim Mod1 As Single = 1.0F

            Dim BRN As Single = 1.0F
            Dim RL As Single = 1.0F
            Dim TVT As Single = 1.0F
            Dim SR As Single = 1.0F
            Dim FF As Single = 1.0F

            If Attack.Category = Attack.Categories.Physical Then
                If p.Ability.Name.ToLower() <> "guts" And p.Status = Pokemon.StatusProblems.Burn Then
                    BRN = 0.5F
                End If
            End If

            Dim CritSwap As Boolean = False
            If Core.Random.Next(0, 3) = 0 And Critical = True Then
                CritSwap = True
            End If
            If CritSwap = False Then
                If Attack.Category = Attack.Categories.Physical Then
                    If p.Ability.Name.ToLower() <> "infiltrator" Then
                        If Own = True Then
                            If BattleScreen.FieldEffects.OppReflect > 0 Then
                                RL = 0.5F
                            End If
                        Else
                            If BattleScreen.FieldEffects.OwnReflect > 0 Then
                                RL = 0.5F
                            End If
                        End If
                    End If
                ElseIf Attack.Category = Attack.Categories.Special Then
                    If p.Ability.Name.ToLower() <> "infiltrator" Then
                        If Own = True Then
                            If BattleScreen.FieldEffects.OppLightScreen > 0 Then
                                RL = 0.5F
                            End If
                        Else
                            If BattleScreen.FieldEffects.OwnLightScreen > 0 Then
                                RL = 0.5F
                            End If
                        End If
                    End If
                End If
            End If

            Select Case Attack.Type.Type
                Case Element.Types.Fire
                    If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                        SR = 1.5F
                    ElseIf BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                        SR = 0.5F
                    End If
                Case Element.Types.Water
                    If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                        SR = 0.5F
                    ElseIf BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                        SR = 1.5F
                    End If
                Case Element.Types.Ice
                    If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Snow Then
                        SR = 1.5F
                    End If
            End Select

            If p.Ability.Name.ToLower() = "flash fire" Then
                If BattleScreen.FieldEffects.CanUseAbility(Own, BattleScreen) = True Then
                    If Own = True Then
                        If Not BattleScreen.FieldEffects.OppLastMove Is Nothing Then
                            If BattleScreen.FieldEffects.OppLastMove.Type.Type = Element.Types.Fire And Attack.Type.Type = Element.Types.Fire Then
                                FF = 1.5F
                            End If
                        End If
                    Else
                        If Not BattleScreen.FieldEffects.OwnLastMove Is Nothing Then
                            If BattleScreen.FieldEffects.OwnLastMove.Type.Type = Element.Types.Fire And Attack.Type.Type = Element.Types.Fire Then
                                FF = 1.5F
                            End If
                        End If
                    End If
                End If
            End If

            Mod1 = BRN * RL * TVT * SR * FF

            'CH
            Dim CH As Single = 1.0F
            If Critical = True Then
                If p.Ability.Name.ToLower() = "sniper" Then
                    CH = 2.25
                Else
                    CH = 1.5
                End If
            End If

            'Mod2

            Dim Mod2 As Single = 1.0F

            If Not p.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Own, BattleScreen) = True Then
                If p.Item.Name.ToLower() = "life orb" Then
                    Mod2 = 1.3F
                End If
            End If
            If Attack.Name.ToLower() = "me first" Then
                Mod2 = 1.5F
            End If

            'R
            Dim R As Integer = Core.Random.Next(85, 101)
            If R = 0 Then
                R = 1
            End If

            'STAB
            Dim STAB As Single = 1.0F
            If Attack.CanGainSTAB = True Then
                If Attack.Type.Type = p.Type1.Type Or Attack.Type.Type = p.Type2.Type Then
                    If p.Ability.Name.ToLower() = "adaptability" Then
                        STAB = 2.0F
                    Else
                        STAB = 1.5F
                    End If
                End If
            End If

            'type1
            Dim Type1 As Single = Element.GetElementMultiplier(Attack.Type, Op.Type1)
            If p.Ability.Name.ToLower() = "refrigerate" And Attack.Type.Type = Element.Types.Normal Then
                Type1 = Element.GetElementMultiplier(New Element(Element.Types.Ice), Op.Type1)
            End If
            If p.Ability.Name.ToLower() = "pixilate" And Attack.Type.Type = Element.Types.Normal Then
                Type1 = Element.GetElementMultiplier(New Element(Element.Types.Fairy), Op.Type1)
            End If
            If p.Ability.Name.ToLower() = "aerilate" And Attack.Type.Type = Element.Types.Normal Then
                Type1 = Element.GetElementMultiplier(New Element(Element.Types.Flying), Op.Type1)
            End If
            Type1 = ReverseTypeEffectiveness(Type1)

            'type2
            Dim Type2 As Single = Element.GetElementMultiplier(Attack.Type, Op.Type2)
            If p.Ability.Name.ToLower() = "refrigerate" And Attack.Type.Type = Element.Types.Normal Then
                Type2 = Element.GetElementMultiplier(New Element(Element.Types.Ice), Op.Type2)
            End If
            If p.Ability.Name.ToLower() = "pixilate" And Attack.Type.Type = Element.Types.Normal Then
                Type2 = Element.GetElementMultiplier(New Element(Element.Types.Fairy), Op.Type2)
            End If
            If p.Ability.Name.ToLower() = "aerilate" And Attack.Type.Type = Element.Types.Normal Then
                Type2 = Element.GetElementMultiplier(New Element(Element.Types.Flying), Op.Type2)
            End If
            Type2 = ReverseTypeEffectiveness(Type2)

            'Mod3
            Dim Mod3 As Single = 1.0F

            Dim SRF As Single = 1.0F
            Dim EB As Single = 1.0F
            Dim TL As Single = 1.0F
            Dim TRB As Single = 1.0F

            Dim TypeA As Single = Type1 * Type2

            If Not Op.Item Is Nothing Then
                If Op.Item.Name.ToLower() = "ring target" And BattleScreen.FieldEffects.CanUseItem(Not Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Not Own, BattleScreen) = True Then
                    If Type1 = 0 Then
                        TypeA = Type2
                    End If
                    If Type2 = 0 Then
                        TypeA = Type1
                    End If
                    If TypeA = 0 Then
                        TypeA = 1.0F
                    End If
                End If
            End If

            If TypeA > 1.0F Then
                If Op.Ability.Name.ToLower() = "solid rock" Or Op.Ability.Name.ToLower() = "filter" Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not Own, BattleScreen) = True Then
                        SRF = 0.75F
                    End If
                End If

                If Not p.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Own, BattleScreen) = True Then
                    If p.Item.Name.ToLower() = "expert belt" Then
                        EB = 1.2F
                    End If
                End If

                If Not Op.Item Is Nothing Then
                    If BattleScreen.FieldEffects.CanUseItem(Not Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Not Own, BattleScreen) = True Then
                        Select Case Op.Item.Name.ToLower()
                            Case "occa"
                                If Attack.Type.Type = Element.Types.Fire Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Occa Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:occa") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "passho"
                                If Attack.Type.Type = Element.Types.Water Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Passho Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:passho") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "wacan"
                                If Attack.Type.Type = Element.Types.Electric Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Wacan Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:wacan") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "rindo"
                                If Attack.Type.Type = Element.Types.Grass Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Rindo Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:rindo") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "yache"
                                If Attack.Type.Type = Element.Types.Ice Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Yache Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:yache") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "chople"
                                If Attack.Type.Type = Element.Types.Fighting Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Chople Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:chople") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "kebia"
                                If Attack.Type.Type = Element.Types.Poison Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Kebia Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:kebia") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "shuca"
                                If Attack.Type.Type = Element.Types.Ground Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Shuca Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:shuca") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "coba"
                                If Attack.Type.Type = Element.Types.Flying Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Coba Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:coba") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "payapa"
                                If Attack.Type.Type = Element.Types.Psychic Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Payapa Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:payapa") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "tanga"
                                If Attack.Type.Type = Element.Types.Bug Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Tanga Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:tanga") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "charti"
                                If Attack.Type.Type = Element.Types.Rock Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Charti Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:charti") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "kasib"
                                If Attack.Type.Type = Element.Types.Ghost Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Kasib Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:kasib") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "haban"
                                If Attack.Type.Type = Element.Types.Dragon Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Haban Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:haban") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "colbur"
                                If Attack.Type.Type = Element.Types.Dark Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Colbur Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:corlbur") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                            Case "babiri"
                                If Attack.Type.Type = Element.Types.Steel Then
                                    If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Babiri Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:babiri") = True Then
                                        TRB = 0.5F
                                    End If
                                End If
                        End Select
                    End If
                End If
            End If

            If Not Op.Item Is Nothing Then
                If BattleScreen.FieldEffects.CanUseItem(Not Own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Not Own, BattleScreen) = True Then
                    If Op.Item.Name.ToLower() = "chilan" Then
                        If Attack.Type.Type = Element.Types.Normal Then
                            If BattleScreen.Battle.RemoveHeldItem(Not Own, Not Own, BattleScreen, "The Chilan Berry weakened the effect of " & Attack.Name & " on " & Op.GetDisplayName() & "!", "berry:chilan") = True Then
                                TRB = 0.5F
                            End If
                        End If
                    End If
                End If
            End If

            If TypeA < 1.0F Then
                If p.Ability.Name.ToLower() = "tinted lense" Then
                    TL = 2.0F
                End If
            End If

            Mod3 = SRF * EB * TL * TRB

            damage = CInt(Math.Floor((((((((Level * 2 / 5) + 2) * BasePower * Atk / 50) / Def) * Mod1) + 2) * CH * Mod2 * R / 100) * STAB * Type1 * Type2 * Mod3))

            If p.Ability.Name.ToLower() = "multiscale" And p.HP = p.MaxHP And BattleScreen.FieldEffects.CanUseAbility(Own, BattleScreen) = True Then
                damage = CInt(damage / 2)
            End If

            If Attack.IsOneHitKOMove = True Then
                damage = Op.HP
            End If

            If damage <= 0 Then
                damage = 1
            End If

            Return damage
        End Function

        Public Shared Function ReverseTypeEffectiveness(ByVal effectiveness As Single) As Single
            If BattleScreen.IsInverseBattle = True Then
                Select Case effectiveness
                    Case 0.5F, 0.0F
                        Return 2.0F
                    Case 1.0F
                        Return 1.0F
                    Case 2.0F
                        Return 0.5F
                End Select
                Return 1.0F
            Else
                Return effectiveness
            End If
        End Function

        ''' <summary>
        ''' Calculates the usable multiplier from a stat reduction/raise.
        ''' </summary>
        ''' <param name="StatValue">The relative stat reduction/raise of the stat.</param>
        Public Shared Function GetMultiplierFromStat(ByVal StatValue As Integer) As Single
            Select Case StatValue
                Case -6
                    Return CSng(2 / 8)
                Case -5
                    Return CSng(2 / 7)
                Case -4
                    Return CSng(2 / 6)
                Case -3
                    Return CSng(2 / 5)
                Case -2
                    Return CSng(2 / 4)
                Case -1
                    Return CSng(2 / 3)
                Case 0
                    Return CSng(2 / 2)
                Case 1
                    Return CSng(3 / 2)
                Case 2
                    Return CSng(4 / 2)
                Case 3
                    Return CSng(5 / 2)
                Case 4
                    Return CSng(6 / 2)
                Case 5
                    Return CSng(7 / 2)
                Case 6
                    Return CSng(8 / 2)
                Case Else
                    Return 1.0F
            End Select
        End Function
        
        Public Shared Function GetMultiplierFromAccEvasion(ByVal StatValue As Integer) As Single
            Select Case StatValue
                Case -6
                    Return CSng(3 / 9)
                Case -5
                    Return CSng(3 / 8)
                Case -4
                    Return CSng(3 / 7)
                Case -3
                    Return CSng(3 / 6)
                Case -2
                    Return CSng(3 / 5)
                Case -1
                    Return CSng(3 / 4)
                Case 0
                    Return CSng(3 / 3)
                Case 1
                    Return CSng(4 / 3)
                Case 2
                    Return CSng(5 / 3)
                Case 3
                    Return CSng(6 / 3)
                Case 4
                    Return CSng(7 / 3)
                Case 5
                    Return CSng(8 / 3)
                Case 6
                    Return CSng(9 / 3)
                Case Else
                    Return 1.0F
            End Select
        End Function
    End Class

End Namespace
