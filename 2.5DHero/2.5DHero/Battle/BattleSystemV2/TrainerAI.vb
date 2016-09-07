Namespace BattleSystem

    Public Class TrainerAI

        'Trainer AI Levels:---------------------------------------------------------------------------------------------------------------------
        'Normal Trainers: 0-20 (0 being not very smart trainers like Bugcatchers, 20 being Cooltrainers, get higher levels when rematches occur)
        '(Johto) Gym Leaders: 10-40 (10 for Falkner, 40 for Clair)
        '(Kanto) Gym Leaders: All 60
        'Elite 4: 60
        'Lance: 100
        'Elder Li: 10
        'Rival: 40
        'Team Rocket Grunts: 5
        'Team Rocket Admins: 20

        Public Shared Function GetAIMove(ByVal BattleScreen As BattleScreen, ByVal OwnStep As Battle.RoundConst) As Battle.RoundConst
            Dim p As Pokemon = BattleScreen.OppPokemon
            Dim op As Pokemon = BattleScreen.OwnPokemon

            Dim m As List(Of Attack) = p.Attacks.ToList()
            For i = m.Count - 1 To 0 Step -1
                If m(i).CurrentPP <= 0 Then
                    m.RemoveAt(i)
                End If
            Next
            If m.Count = 0 Then
                Return New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Move, .Argument = Attack.GetAttackByID(165)}
            End If

            'Switching:
            ' - check for opponent moves: if super effective: 1.5x: 25%, 2x: 50%, 4x: 75%: check for pokemon in party that reacts to every move with less the detected effectiveness k
            ' - check for own moves: if only 0x: check for other pokemon in party (best fitting) and switch in k
            ' - check for own moves: if all moves below 1x: 50%: check for other pokemon in party that has at least one move above or equal 1x
            ' - own pokemon got cursed: 75% k
            ' - own pokemon got confused: 50% k
            ' - if has pokemon in team that can stand the attack coming in better than the current one: 25%: switch into that pokemon reflecting that move

            'Use Item:
            '(Useable items: Potion,Super Point,Hyper Potion,Full Heal,Full Restore,Burn Heal,Antidote,Paralyze heal,Awakening,Ice Heal,Revive,Max Revive,Max Potion)
            ' - Use potions if fills to at least half + own hp is below 40% (chance 60% + (40 - ownhp%)), always use best potion (normal->super->hyper->max) k
            ' - Use Full Heal when: Confused, Burned, Frozen, Asleep, Paralyzed, Poisoned and HP is above 25% (100%), use above status restore items. k
            ' - ^ same with Full Restore but without HP check, use above everything k
            ' - Use Status restore items if own HP is above 25%, chance: 60% k
            ' - Use revive when at least 50% of team is beaten, use on highest leveled pokemon (max over normal) (50%) k

            'Definite move:
            ' - If own pokemon is asleep, try to use Snore (100%) k
            ' - If own pokemon is frozen and has a move to thraw out -> use that move (100%) k
            ' - Fake Out if first turn -> try to inflict flinch (100%) k
            ' - use high priority move if opponent is nearly defeated (<= 15%) (100%) k
            ' - use attacking move if speed is higher than opp speed and opponent has low health (<= 30%) (75%) k
            ' - use recover moves when HP lower than 50% (75%) k
            ' - try to paralyse (75%) k
            ' - try to confuse (75%) k
            ' - try to burn (75%) k
            ' - try to sleep (75%) k
            ' - try to freeze (75%) k
            ' - try to poison (50%) k
            ' - try to set up leech seed (75%) k
            ' - try to use FocusEnergy (50%) k
            ' - Set up spikes (50%)
            ' - setup safeguard (20%)
            ' - setup sandstorm, when at least half of the team is immune (50%)
            ' - setup hailstorm, when at least half of the team is immune (50%)
            ' - higher sp atk than atk, not boosted stat, higher spatk than spdef: use sp atk boost (50%) k
            ' - higher atk than sp atk, not boosted stat, higher atk than def: use atk boost (50%) k
            ' - not lowered opp def stat, higher atk than spatk, higher def than atk: use def lowering move  (50%) k
            ' - not lowered opp sp def stat, higher sp atk than spdef, higher sp def than sp atk: use sp def lowering move  (50%) k
            ' - when opp has lower atk than def/lower spatk than spdef and can heal stat: use stat healing move (50%) k
            ' - if entry hazards are on the field and more than one pokemon left: use Rapid Spin (100%)
            ' - if not done before, boost own evasion (50%) k
            ' - if not done before, lower opp accuracy (75%) k
            ' - use Psych up when the opponent total stat change compared to own is at least +2 (50%)
            ' - has Reflect/Light Screen: Uses the one that fits better for the own stat (75%) k
            ' - when having a move with accuracy lower than 60 and Mind Reader/Lock On -> Use Mind Reader/Lock On (50%)
            ' - Use fascade, if burned, paralysed or poisoned (50%)
            ' - use smelling salt if opponent is paralysed (100%)

            'Special Moveset combos:
            ' - Defense Curl + Rollout k
            ' - Sunny Day + Solar Beam / Sunny weather + Solar Beam
            ' - Raindance + Thunder / Rain weather + Thunder / Sunny weather + Thunder
            ' - Sleep inducting move + Dream Eater / Use Dream Eater if target is asleep
            ' - Flash + Ride
            ' - Ghost Pokemon and Curse
            ' - Use Flail/Reversal when HP is really low
            ' - Conversion2/Conversion
            ' - Protect/Detect: when opp used a strong move
            ' - use Uproar if not set up and detect opponent with sleep moves
            ' - Stockpile + Swallow/SpitUp
            ' - Thunderwave + Smelling Salt

            'After choosing attacking moves:
            ' - not use belly drum/substitute with too low HP
            ' - not use any move that doesnt work due to type disadventage (ghost-normal, normal-ghost, fighting-ghost, dragon-fairy)

            'Choose attack move:
            ' - look for abilities that absorb move types
            ' - base power, accuracy, effectiveness, STAB, PP Left, attack stats (atk/spatk), EV, IV, defense stat (def/spdef)
            ' - never use splash
            ' - use never-miss attack when own accuracy is low/opp evasion is high

            '-------------------------------------AI BEGIN------------------------------------------------------------------------------------'

            '-------------------------------------Random move depending on difficulty---------------------------------------------------------'

            'Only applies if trainer has an AI level below 20:
            If BattleScreen.Trainer.AILevel < 20 Then
                If Core.Player.DifficultyMode = 0 Then
                    'Chance of 35% that the trainer is using a random move:
                    If Core.Random.Next(0, 100) < 35 Then
                        Return ProduceOppStep(m, Core.Random.Next(0, m.Count))
                    End If
                ElseIf Core.Player.DifficultyMode = 1 Then
                    'Chance of 18% that the trainer is using a random move:
                    If Core.Random.Next(0, 100) < 18 Then
                        Return ProduceOppStep(m, Core.Random.Next(0, m.Count))
                    End If
                End If
            End If

            '-------------------------------------Switching-----------------------------------------------------------------------------------'

            'Only applies if trainer has an AI level above or equal 40:
            If BattleScreen.Trainer.AILevel >= 40 Then
                If BattleCalculation.CanSwitch(BattleScreen, False) = True Then
                    If BattleScreen.Trainer.Pokemons.Count > 0 Then
                        Dim living As Integer = 0
                        For Each cP As Pokemon In BattleScreen.Trainer.Pokemons
                            If cP.HP > 0 And cP.Status <> Pokemon.StatusProblems.Fainted Then
                                living += 1
                            End If
                        Next
                        If living > 1 Then
                            'check for opponent moves: if super effective: 1.5x: 25%, 2x: 50%, 4x: 75%: check for pokemon in party that reacts to every move with less the detected effectiveness
                            Dim maxOpponentEff As Single = 0.0F
                            For Each Attack As BattleSystem.Attack In op.Attacks
                                Dim effectiveness As Single = BattleCalculation.CalculateEffectiveness(Attack.GetAttackByID(Attack.ID), BattleScreen, op, p, True)
                                If effectiveness > maxOpponentEff Then
                                    maxOpponentEff = effectiveness
                                End If
                            Next
                            If maxOpponentEff > 1.0F Then
                                Dim chance As Integer = 0

                                Select Case maxOpponentEff
                                    Case 1.25F
                                        chance = 10
                                    Case 1.5F
                                        chance = 25
                                    Case 2.0F
                                        chance = 35
                                    Case 4.0F
                                        chance = 50
                                End Select

                                If RPercent(chance) = True Then
                                    Dim lessTeamPs As New List(Of Integer)

                                    For i = 0 To BattleScreen.Trainer.Pokemons.Count - 1
                                        If i <> BattleScreen.OppPokemonIndex Then
                                            Dim TeamP As Pokemon = BattleScreen.Trainer.Pokemons(i)
                                            If TeamP.HP > 0 And TeamP.Status <> Pokemon.StatusProblems.Fainted Then
                                                Dim alwaysLess As Boolean = True
                                                For Each Attack As BattleSystem.Attack In op.Attacks
                                                    Dim effectiveness As Single = BattleCalculation.CalculateEffectiveness(Attack.GetAttackByID(Attack.ID), BattleScreen, op, TeamP, True)

                                                    If effectiveness >= maxOpponentEff Then
                                                        alwaysLess = False
                                                        Exit For
                                                    End If
                                                Next
                                                If alwaysLess = True Then
                                                    lessTeamPs.Add(i)
                                                End If
                                            End If
                                        End If
                                    Next

                                    If lessTeamPs.Count > 0 Then
                                        Return ProduceOppStep(lessTeamPs(Core.Random.Next(0, lessTeamPs.Count)))
                                    End If
                                End If
                            End If

                            'check for own moves: if only 0x: check for other pokemon in party (best fitting) and switch in
                            Dim only0 As Boolean = True
                            For Each Attack As BattleSystem.Attack In p.Attacks
                                Dim effectiveness As Single = BattleCalculation.CalculateEffectiveness(Attack.GetAttackByID(Attack.ID), BattleScreen, p, op, False)
                                If effectiveness <> 0.0F Then
                                    only0 = False
                                    Exit For
                                End If
                            Next
                            If only0 = True Then
                                Dim switchList As New List(Of Integer)
                                For i = 0 To BattleScreen.Trainer.Pokemons.Count - 1
                                    If i <> BattleScreen.OppPokemonIndex Then
                                        Dim TeamP As Pokemon = BattleScreen.Trainer.Pokemons(i)
                                        If TeamP.HP > 0 And TeamP.Status <> Pokemon.StatusProblems.Fainted Then
                                            switchList.Add(i)
                                        End If
                                    End If
                                Next
                                If switchList.Count > 0 Then
                                    Return ProduceOppStep(switchList(Core.Random.Next(0, switchList.Count)))
                                End If
                            End If

                            'own pokemon got cursed: 75%
                            If BattleScreen.FieldEffects.OppCurse > 0 Then
                                If RPercent(75) = True Then
                                    Dim newSwitchIndex As Integer = 0
                                    Dim canSwitchTo As New List(Of Integer)
                                    For i = 0 To BattleScreen.Trainer.Pokemons.Count - 1
                                        Dim TeamP As Pokemon = BattleScreen.Trainer.Pokemons(i)
                                        If TeamP.HP > 0 And TeamP.Status <> Pokemon.StatusProblems.Fainted And i <> BattleScreen.OppPokemonIndex Then
                                            canSwitchTo.Add(i)
                                        End If
                                    Next

                                    If canSwitchTo.Count > 0 Then
                                        newSwitchIndex = canSwitchTo(Core.Random.Next(0, canSwitchTo.Count))

                                        Return ProduceOppStep(newSwitchIndex)
                                    End If
                                End If
                            End If

                            'own pokemon got confused: 50%
                            If p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                                If RPercent(50) = True Then
                                    Dim newSwitchIndex As Integer = 0
                                    Dim canSwitchTo As New List(Of Integer)
                                    For i = 0 To BattleScreen.Trainer.Pokemons.Count - 1
                                        Dim TeamP As Pokemon = BattleScreen.Trainer.Pokemons(i)
                                        If TeamP.HP > 0 And TeamP.Status <> Pokemon.StatusProblems.Fainted And i <> BattleScreen.OppPokemonIndex Then
                                            canSwitchTo.Add(i)
                                        End If
                                    Next

                                    If canSwitchTo.Count > 0 Then
                                        newSwitchIndex = canSwitchTo(Core.Random.Next(0, canSwitchTo.Count))

                                        Return ProduceOppStep(newSwitchIndex)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            '-------------------------------------Items---------------------------------------------------------------------------------------'

            'same with Full Restore but without HP check, use above everything
            If p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Or p.Status <> Pokemon.StatusProblems.None And p.Status <> Pokemon.StatusProblems.Fainted Then
                If TrainerHasItem(14, BattleScreen) = True Then
                    If Not (TrainerHasItem(38, BattleScreen) = True And p.HP = p.MaxHP) = True Then
                        Return ProduceOppStep(14, -1)
                    End If
                End If
            End If

            'Use potions if fills to at least half + own hp is below 40% (chance 60% + (40 - ownhp%)), always use best potion (normal->super->hyper->max)
            If p.HP <= CInt((p.MaxHP / 100) * 40) Then
                Dim potion As Integer = GetBestPotion(BattleScreen)
                If potion > -1 Then
                    If TrainerHasItem(potion, BattleScreen) = True Then
                        Dim chance As Integer = GetPokemonValue(BattleScreen, p) + (40 - CInt((p.HP / p.MaxHP) * 100))
                        If RPercent(chance) = True Then
                            Dim HP As Integer = GetPotionHealHP(p, potion)
                            If HP >= CInt(Math.Ceiling(p.MaxHP / 2)) Then
                                Return ProduceOppStep(potion, -1)
                            End If
                        End If
                    End If
                End If
            End If

            'Use Full Heal when: Confused, Burned, Frozen, Asleep, Paralyzed, Poisoned and HP is above 25% (100%), use above status restore items.
            If p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Or p.Status <> Pokemon.StatusProblems.None And p.Status <> Pokemon.StatusProblems.Fainted Then
                If p.HP >= CInt((p.MaxHP / 100) * 25) Then
                    If TrainerHasItem(38, BattleScreen) = True Then
                        Return ProduceOppStep(38, -1)
                    End If
                End If
            End If

            'Use Status restore items if own HP is above 25%, chance: 60%
            If p.HP >= CInt((p.MaxHP / 100) * 25) Then
                If p.Status <> Pokemon.StatusProblems.None And p.Status <> Pokemon.StatusProblems.Fainted Then
                    If RPercent(60) = True Then
                        Select Case p.Status
                            Case Pokemon.StatusProblems.Poison, Pokemon.StatusProblems.BadPoison
                                If TrainerHasItem(9, BattleScreen) = True Then
                                    Return ProduceOppStep(9, -1)
                                End If
                            Case Pokemon.StatusProblems.Burn
                                If TrainerHasItem(10, BattleScreen) = True Then
                                    Return ProduceOppStep(10, -1)
                                End If
                            Case Pokemon.StatusProblems.Freeze
                                If TrainerHasItem(11, BattleScreen) = True Then
                                    Return ProduceOppStep(11, -1)
                                End If
                            Case Pokemon.StatusProblems.Sleep
                                If TrainerHasItem(12, BattleScreen) = True Then
                                    Return ProduceOppStep(12, -1)
                                End If
                            Case Pokemon.StatusProblems.Paralyzed
                                If TrainerHasItem(13, BattleScreen) = True Then
                                    Return ProduceOppStep(13, -1)
                                End If
                        End Select
                    End If
                End If
            End If

            'Use revive when at least 50% of team is beaten, use on highest leveled pokemon (max over normal) (50%)
            If RPercent(50) = True And (TrainerHasItem(39, BattleScreen) = True Or TrainerHasItem(40, BattleScreen) = True) = True Then
                Dim beaten As Integer = 0
                For Each TeamP As Pokemon In BattleScreen.Trainer.Pokemons
                    If TeamP.HP <= 0 Or TeamP.Status = Pokemon.StatusProblems.Fainted Or TeamP.IsEgg() = True Then
                        beaten += 1
                    End If
                Next
                If beaten >= CInt(Math.Floor(BattleScreen.Trainer.Pokemons.Count / 2)) Then
                    Dim highestLevel As Integer = -1
                    For i = 0 To BattleScreen.Trainer.Pokemons.Count - 1
                        Dim TeamP As Pokemon = BattleScreen.Trainer.Pokemons(i)
                        If TeamP.IsEgg() = False Then
                            If TeamP.HP <= 0 Or TeamP.Status = Pokemon.StatusProblems.Fainted Then
                                If highestLevel = -1 OrElse BattleScreen.Trainer.Pokemons(highestLevel).Level < TeamP.Level Then
                                    highestLevel = i
                                End If
                            End If
                        End If
                    Next
                    If highestLevel > -1 Then
                        Dim bestRevive As Integer = GetBestRevive(BattleScreen)
                        Return ProduceOppStep(bestRevive, highestLevel)
                    End If
                End If
            End If

            '-------------------------------------Moves---------------------------------------------------------------------------------------'

            'If own pokemon is asleep, try to use Sleep Talk (100%)
            If p.Status = Pokemon.StatusProblems.Sleep And BattleScreen.FieldEffects.OppSleepTurns > 1 And HasMove(m, 214) = True Then
                Return ProduceOppStep(m, IDtoMoveIndex(m, 214))
            End If

            'If own pokemon is asleep, try to use Snore (100%)
            If p.Status = Pokemon.StatusProblems.Sleep And BattleScreen.FieldEffects.OppSleepTurns > 1 And HasMove(m, 173) = True Then
                Return ProduceOppStep(m, IDtoMoveIndex(m, 173))
            End If

            'If own pokemon is frozen and has a move to thraw out -> use that move
            If p.Status = Pokemon.StatusProblems.Freeze Then
                Dim chosenMove As Integer = MoveAI(m, Attack.AIField.ThrawOut)
                If chosenMove > -1 Then
                    Return ProduceOppStep(m, chosenMove)
                End If
            End If

            'Fake Out if first turn -> try to inflict flinch (100%)
            If HasMove(m, 252) = True Then
                If op.Ability.Name.ToLower() <> "inner focus" Then
                    Dim turns As Integer = BattleScreen.FieldEffects.OppPokemonTurns
                    If turns = 0 Then
                        Return ProduceOppStep(m, IDtoMoveIndex(m, 252))
                    End If
                End If
            End If

            'use high priority move if opponent is nearly defeated (<= 15%) (100%)
            If op.HP <= CInt((op.MaxHP / 100) * 15) Then
                Dim chosenMove As Integer = MoveAI(m, Attack.AIField.HighPriority)
                If chosenMove > -1 Then
                    If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                        Return ProduceOppStep(m, chosenMove)
                    End If
                End If
            End If

            'use attacking move if speed is higher than opp speed and opponent has low health (<= 30%) (75%)
            If p.Speed > op.Speed And op.HP <= CInt((op.MaxHP / 100) * 30) Then
                Dim chosenMove As Integer = GetAttackingMove(BattleScreen, m)
                If chosenMove > -1 Then
                    If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                        Return ProduceOppStep(m, chosenMove)
                    End If
                End If
            End If

            'use recover moves when HP lower than 50% (50%):
            If p.HP <= CInt(p.MaxHP / 2) Then
                Dim chance As Integer = 50
                'make chance higher when opp has a status condition:
                If op.Status = Pokemon.StatusProblems.Freeze Or op.Status = Pokemon.StatusProblems.Paralyzed Or op.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                    chance = 75
                End If
                If op.Status = Pokemon.StatusProblems.Sleep Then
                    chance = 100
                End If

                If RPercent(chance) = True Then
                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.Healing)
                    If chosenMove > -1 Then
                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                            Return ProduceOppStep(m, chosenMove)
                        End If
                    End If
                End If
            End If

            'Try to use Brick Break move if opponent has LightScreen/Reflect up:
            If op.IsType(Element.Types.Ghost) = False Then
                If BattleScreen.FieldEffects.OwnReflect > 0 Or BattleScreen.FieldEffects.OwnLightScreen > 0 Then
                    If RPercent(80) = True Then
                        Dim chosenMove As Integer = MoveAI(m, Attack.AIField.RemoveReflectLightscreen)
                        If chosenMove > -1 Then
                            Return ProduceOppStep(m, chosenMove)
                        End If
                    End If
                End If
            End If

            'try to inflict status:
            'check if substitute is up:
            If BattleScreen.FieldEffects.OwnSubstitute = 0 Then
                'check no status already applies:
                If op.Status = Pokemon.StatusProblems.None Then

                    'try to paralyse (75%)
                    If op.Status <> Pokemon.StatusProblems.Paralyzed Then
                        If op.Type1.Type <> Element.Types.Electric And op.Type2.Type <> Element.Types.Electric Then
                            If op.Ability.Name.ToLower() <> "limber" Then
                                If RPercent(75) = True Then
                                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.Paralysis)
                                    If chosenMove > -1 Then
                                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                                            Return ProduceOppStep(m, chosenMove)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    'try to burn (75%)
                    If op.Status <> Pokemon.StatusProblems.Burn Then
                        If op.Type1.Type <> Element.Types.Fire And op.Type2.Type <> Element.Types.Fire Then
                            If op.Ability.Name.ToLower() <> "water veil" Then
                                If RPercent(75) = True Then
                                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.Burn)
                                    If chosenMove > -1 Then
                                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                                            Return ProduceOppStep(m, chosenMove)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    'try to sleep (75%)
                    If op.Status <> Pokemon.StatusProblems.Sleep Then
                        If op.Ability.Name.ToLower() <> "vital spirit" Then
                            If op.Ability.Name.ToLower() <> "insomnia" Then
                                If RPercent(75) = True Then
                                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.Sleep)
                                    If chosenMove > -1 Then
                                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                                            Return ProduceOppStep(m, chosenMove)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                    'try to poison (50%)
                    If op.Status <> Pokemon.StatusProblems.BadPoison And op.Status <> Pokemon.StatusProblems.Poison Then
                        If op.Type1.Type <> Element.Types.Steel And op.Type1.Type <> Element.Types.Poison And op.Type2.Type <> Element.Types.Steel And op.Type2.Type <> Element.Types.Poison Then
                            If op.Ability.Name.ToLower() <> "immunity" Then
                                If RPercent(50) = True Then
                                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.Poison)
                                    If chosenMove > -1 Then
                                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                                            Return ProduceOppStep(m, chosenMove)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If

                End If
            End If

            'try to confuse (75%)
            'check if opp isnt confused already:
            If op.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = False Then
                If op.Ability.Name.ToLower() <> "own tempo" Then
                    If RPercent(75) = True Then
                        Dim chosenMove As Integer = MoveAI(m, Attack.AIField.Confusion)
                        If chosenMove > -1 Then
                            If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                                Return ProduceOppStep(m, chosenMove)
                            End If
                        End If
                    End If
                End If
            End If

            'try to set up leech seed (75%)
            If op.IsType(Element.Types.Grass) = False Then
                If BattleScreen.FieldEffects.OwnLeechSeed = 0 Then
                    If HasMove(m, 73) = True Then
                        If RPercent(75) = True Then
                            Return ProduceOppStep(m, IDtoMoveIndex(m, 73))
                        End If
                    End If
                End If
            End If

            'try to use FocusEnergy (50%)
            If BattleScreen.FieldEffects.OppFocusEnergy = 0 Then
                If HasMove(m, 116) = True Then
                    If RPercent(50) = True Then
                        Return ProduceOppStep(m, IDtoMoveIndex(m, 116))
                    End If
                End If
            End If

            'higher sp atk than atk, not boosted stat, higher spatk than spdef: use sp atk boost (50%)
            If p.SpAttack > p.Attack And p.StatSpAttack <= 0 And p.SpAttack > p.SpDefense Then
                If RPercent(50) = True Then
                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.RaiseSpAttack)
                    If chosenMove > -1 Then
                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                            Return ProduceOppStep(m, chosenMove)
                        End If
                    End If
                End If
            End If

            'higher atk than sp atk, not boosted stat, higher atk than def: use atk boost (50%)
            If p.Attack > p.SpAttack And p.StatAttack <= 0 And p.Attack > p.Defense Then
                If RPercent(50) = True Then
                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.RaiseAttack)
                    If chosenMove > -1 Then
                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                            Return ProduceOppStep(m, chosenMove)
                        End If
                    End If
                End If
            End If

            'not lowered opp def stat, higher atk than spatk, higher def than atk: use def lowering move  (50%)
            If p.Attack > p.SpAttack And p.Defense > p.Attack And op.StatDefense >= 0 Then
                If RPercent(50) = True Then
                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.LowerDefense)
                    If chosenMove > -1 Then
                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                            Return ProduceOppStep(m, chosenMove)
                        End If
                    End If
                End If
            End If

            'not lowered opp spdef stat, higher spatk than atk, higher spdef than spatk: use spdef lowering move (50%)
            If p.SpAttack > p.Attack And p.SpDefense > p.SpAttack And op.StatSpDefense >= 0 Then
                If RPercent(50) = True Then
                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.LowerSpDefense)
                    If chosenMove > -1 Then
                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                            Return ProduceOppStep(m, chosenMove)
                        End If
                    End If
                End If
            End If


            'when opp has lower atk than def/lower spatk than spdef and can heal stat: use stat healing move (50%)
            If op.SpAttack > op.Attack And op.SpAttack < op.SpDefense Or op.Attack > op.SpAttack And op.Attack < op.Defense Then
                If p.Status = Pokemon.StatusProblems.BadPoison Or p.Status = Pokemon.StatusProblems.Burn Or p.Status = Pokemon.StatusProblems.Freeze Or p.Status = Pokemon.StatusProblems.Paralyzed Or p.Status = Pokemon.StatusProblems.Poison Or p.Status = Pokemon.StatusProblems.Sleep Then
                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.CureStatus)
                    If chosenMove > -1 Then
                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                            Return ProduceOppStep(m, chosenMove)
                        End If
                    End If
                End If
            End If

            'if not done before, boost own evasion (50%) 
            If p.Evasion <= 0 Then
                If RPercent(50) = True Then
                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.RaiseEvasion)
                    If chosenMove > -1 Then
                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                            Return ProduceOppStep(m, chosenMove)
                        End If
                    End If
                End If
            End If

            'if not done before, boost own accuracy (75%) 
            If p.Evasion <= 0 Then
                If RPercent(75) = True Then
                    Dim chosenMove As Integer = MoveAI(m, Attack.AIField.RaiseEvasion)
                    If chosenMove > -1 Then
                        If CheckForTypeIneffectiveness(BattleScreen, m, chosenMove) = True Then
                            Return ProduceOppStep(m, chosenMove)
                        End If
                    End If
                End If
            End If

            'Use LightScreen/Reflect (75%):
            If RPercent(75) = True Then
                If HasMove(m, 113) = True And op.SpAttack > op.Attack And BattleScreen.FieldEffects.OppLightScreen = 0 Then
                    Return ProduceOppStep(m, IDtoMoveIndex(m, 113))
                End If
                If HasMove(m, 115) = True And op.Attack > op.SpAttack And BattleScreen.FieldEffects.OppReflect = 0 Then
                    Return ProduceOppStep(m, IDtoMoveIndex(m, 115))
                End If
            End If

            'Special Moveset combos:
            ' - Defense Curl + Rollout
            If HasMove(m, 205) = True And HasMove(m, 111) = True Then
                If BattleScreen.FieldEffects.OppDefenseCurl = 0 Then
                    Return ProduceOppStep(m, IDtoMoveIndex(m, 111))
                Else
                    Return ProduceOppStep(m, IDtoMoveIndex(m, 205))
                End If
            End If

            'Determine best attacking move:

            Dim attackDic As New Dictionary(Of Integer, Integer)
            For i = 0 To m.Count - 1
                If MoveHasAIField(m(i), Attack.AIField.Damage) = True Then
                    attackDic.Add(i, 0)
                End If
            Next
            'If has more than 0 attacking moves:
            If attackDic.Count > 0 Then
                For i = 0 To attackDic.Count - 1
                    Dim key As Integer = attackDic.Keys(i)
                    Dim cMove As Attack = m(key)
                    Dim value As Integer = 0

                    Dim effectiveness As Single = BattleCalculation.CalculateEffectiveness(False, cMove, BattleScreen)

                    'Base power
                    If effectiveness <> 0.0F Then
                        value += CInt(cMove.GetBasePower(False, BattleScreen) * effectiveness)
                    End If

                    'Accuracy
                    value += cMove.GetAccuracy(False, BattleScreen)

                    'STAB
                    If p.IsType(cMove.Type.Type) = True Then
                        value += 35
                    End If

                    'Ignore PP left for now...

                    'Attack stats:
                    If cMove.Category = Attack.Categories.Physical Then
                        value += p.StatAttack * 15
                    Else 'Special
                        value += p.StatSpAttack * 15
                    End If
                    If cMove.UseOppDefense = True Then
                        If cMove.Category = Attack.Categories.Physical Then
                            value -= op.StatDefense * 15
                        Else 'Special
                            value -= p.StatSpDefense * 15
                        End If
                    End If

                    'Dry skin: +fire
                    If cMove.GetAttackType(False, BattleScreen).Type = Element.Types.Fire And op.Ability.Name.ToLower() = "dry skin" Then
                        value += 25
                    End If

                    'Use never-miss attack when own accuracy is low/opp evasion is high
                    If cMove.Accuracy = 0 Or cMove.GetUseAccEvasion(False, BattleScreen) = False Then
                        If p.Accuracy < 0 Then
                            value += CInt(Math.Abs(p.Accuracy * 15))
                        End If
                        If op.Evasion > 0 Then
                            value += op.Evasion * 15
                        End If
                    End If

                    'If the pokemon has other options, dont use selfdestruct or explosion too often:
                    If cMove.ID = 120 Or cMove.ID = 153 Then
                        If Core.Random.Next(0, 8) <> 0 Then
                            value = Core.Random.Next(50, 100)
                        End If
                    End If

                    'Add randomness value:
                    value += Core.Random.Next(-35, 35)

                    If value < 0 Then
                        value = 0
                    End If

                    'Effectiveness:
                    If effectiveness = 0.0F Then
                        value = 0
                    End If

                    'Don't use moves that get absorbed by abilities:
                    If cMove.GetAttackType(False, BattleScreen).Type = Element.Types.Water And op.Ability.Name.ToLower() = "water absorb" Then
                        value = 0
                    End If
                    If cMove.GetAttackType(False, BattleScreen).Type = Element.Types.Electric And op.Ability.Name.ToLower() = "volt absorb" Then
                        value = 0
                    End If
                    If cMove.GetAttackType(False, BattleScreen).Type = Element.Types.Electric And op.Ability.Name.ToLower() = "motor drive" Then
                        value = 0
                    End If
                    If cMove.GetAttackType(False, BattleScreen).Type = Element.Types.Grass And op.Ability.Name.ToLower() = "sap sipper" Then
                        value = 0
                    End If
                    If cMove.GetAttackType(False, BattleScreen).Type = Element.Types.Fire And op.Ability.Name.ToLower() = "flash fire" Then
                        value = 0
                    End If
                    If cMove.GetAttackType(False, BattleScreen).Type = Element.Types.Water And op.Ability.Name.ToLower() = "dry skin" Then
                        value = 0
                    End If

                    'Special moves to not use:
                    If cMove.ID = 150 Then 'Never use splash
                        value = 0
                    End If
                    If cMove.ID = 214 And p.Status <> Pokemon.StatusProblems.Sleep Then 'Never use Sleep Talk when user is not sleeping.
                        value = 0
                    End If
                    If cMove.ID = 171 And op.Status <> Pokemon.StatusProblems.Sleep Then 'Never use nightmare when opponent is not sleeping.
                        value = 0
                    End If
                    If cMove.ID = 138 And op.Status <> Pokemon.StatusProblems.Sleep Then 'Never use Dream eater when opponent is not sleeping
                        value = 0
                    End If

                    attackDic(key) = value
                Next

                'Debug feature: Enable if not sure
                'Logger.Debug("RESULTS:")
                'For t = 0 To attackDic.Count - 1
                '    Logger.Debug(t.ToString() & ": " & m(attackDic.Keys(t)).Name & "; VALUE: " & attackDic.Values(t))
                'Next

                Dim index As Integer = 0
                If attackDic.Count > 1 Then
                    If attackDic.Values(1) > attackDic.Values(index) Then
                        index = 1
                    End If
                End If
                If attackDic.Count > 2 Then
                    If attackDic.Values(2) > attackDic.Values(index) Then
                        index = 2
                    End If
                End If
                If attackDic.Count > 3 Then
                    If attackDic.Values(3) > attackDic.Values(index) Then
                        index = 3
                    End If
                End If

                'Dis too:
                'Logger.Debug("CHOSEN: " & m(attackDic.Keys(index)).Name & "; VALUE: " & attackDic.Values(index))

                Return ProduceOppStep(m, attackDic.Keys(index))
            End If

            'once everything else failed, choose one of the attacking moves:
            Dim chosenAttackMove As Integer = MoveAI(m, Attack.AIField.Damage)
            If chosenAttackMove > -1 Then
                Return ProduceOppStep(m, chosenAttackMove)
            End If

            'catch crash: return random move:
            Return ProduceOppStep(m, Core.Random.Next(0, m.Count))
        End Function

        Private Shared Function HasOtherAttackingMoveThanExplosion(ByVal m As List(Of Attack), ByVal leaveOutIndex As Integer) As Boolean
            For i = 0 To m.Count - 1
                If i <> leaveOutIndex Then
                    If m(i).ID <> 121 And m(i).ID <> 153 Then
                        Return True
                    End If
                End If
            Next
            Return False
        End Function

        Private Shared Function GetAttackingMove(ByVal BattleScreen As BattleScreen, ByVal m As List(Of Attack)) As Integer
            Return 0
        End Function

        Private Shared Function CheckForTypeIneffectiveness(ByVal BattleScreen As BattleScreen, ByVal m As List(Of Attack), ByVal i As Integer) As Boolean
            Dim move As Attack = m(i)

            If move.ImmunityAffected = True Then
                Dim effectiveness As Single = BattleCalculation.CalculateEffectiveness(False, move, BattleScreen)
                If effectiveness = 0.0F Then
                    Return False
                End If
            End If
            Return True
        End Function

        Private Shared Function RPercent(ByVal p As Integer) As Boolean
            If p >= 100 Then
                Return True
            End If
            If Core.Random.Next(0, 100) < p Then
                Return True
            End If
            Return False
        End Function

        Private Shared Function MoveAI(ByVal m As List(Of Attack), ByVal AIType As Attack.AIField) As Integer
            Dim validMoves As New List(Of Integer)
            For i = 0 To m.Count - 1
                If m(i).AIField1 = AIType Or m(i).AIField2 = AIType Or m(i).AIField3 = AIType Then
                    validMoves.Add(i)
                End If
            Next

            If validMoves.Count > 0 Then
                Return validMoves(Core.Random.Next(0, validMoves.Count))
            End If

            Return -1
        End Function

        Private Shared Function MoveHasAIField(ByVal m As Attack, ByVal AIType As Attack.AIField) As Boolean
            Return m.AIField1 = AIType Or m.AIField2 = AIType Or m.AIField3 = AIType
        End Function

        Private Shared Function HasMove(ByVal m As List(Of Attack), ByVal ID As Integer) As Boolean
            For Each move As Attack In m
                If move.ID = ID Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Shared Function IDtoMoveIndex(ByVal m As List(Of Attack), ByVal ID As Integer) As Integer
            For i = 0 To m.Count - 1
                If m(i).ID = ID Then
                    Return i
                End If
            Next
            Return -1
        End Function

#Region "ProduceSteps"

        Private Shared Function ProduceOppStep(ByVal m As List(Of Attack), ByVal i As Integer) As Battle.RoundConst
            While i > m.Count - 1
                i -= 1
            End While

            If m.Count = 0 Then
                Throw New Exception("An empty move array got passed in!")
            End If

            Return New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Move, .Argument = m(i)}
        End Function

        Private Shared Function ProduceOppStep(ByVal ItemID As Integer, ByVal target As Integer) As Battle.RoundConst
            Return New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Item, .Argument = ItemID.ToString() & "," & target.ToString()}
        End Function

        Private Shared Function ProduceOppStep(ByVal SwitchID As Integer) As Battle.RoundConst
            Return New Battle.RoundConst With {.StepType = Battle.RoundConst.StepTypes.Switch, .Argument = SwitchID.ToString()}
        End Function

#End Region

#Region "Item"

        Private Shared Function TrainerHasItem(ByVal ItemID As Integer, ByVal BattleScreen As BattleScreen) As Boolean
            For Each Item As Item In BattleScreen.Trainer.Items
                If Item.ID = ItemID Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Shared Function GetBestPotion(ByVal BattleScreen As BattleScreen) As Integer
            Dim potionRange As List(Of Integer) = {18, 17, 16, 15, 14}.ToList()
            Dim bestPotion As Integer = -1
            For Each I As Item In BattleScreen.Trainer.Items
                If potionRange.Contains(I.ID) = True Then
                    If potionRange.IndexOf(I.ID) > bestPotion Then
                        bestPotion = potionRange.IndexOf(I.ID)
                    End If
                End If
            Next

            If bestPotion = -1 Then
                Return -1
            Else
                Return potionRange(bestPotion)
            End If
        End Function

        Private Shared Function GetBestRevive(ByVal BattleScreen As BattleScreen) As Integer
            For Each Item As Item In BattleScreen.Trainer.Items
                If Item.ID = 40 Then
                    Return 40
                End If
            Next
            Return 39
        End Function

        Private Shared Function GetPotionHealHP(ByVal p As Pokemon, ByVal ItemID As Integer) As Integer
            Select Case ItemID
                Case 18
                    Return 20
                Case 17
                    Return 50
                Case 16
                    Return 200
                Case 15
                    Return p.MaxHP
                Case 14
                    Return p.MaxHP
            End Select
            Return 0
        End Function

#End Region

        Private Shared Function GetPokemonValue(ByVal BattleScreen As BattleScreen, ByVal p As Pokemon) As Integer
            Dim total As Integer = 0
            For Each TeamP As Pokemon In BattleScreen.Trainer.Pokemons
                total += TeamP.BaseHP + TeamP.BaseAttack + TeamP.BaseDefense + TeamP.BaseSpAttack + TeamP.BaseSpDefense + TeamP.BaseSpeed
            Next
            Dim pTotal As Integer = p.BaseHP + p.BaseAttack + p.BaseDefense + p.BaseSpAttack + p.BaseSpDefense + p.BaseSpeed

            Dim percent As Integer = CInt((pTotal / total) * 100)
            Return percent
        End Function

    End Class

End Namespace