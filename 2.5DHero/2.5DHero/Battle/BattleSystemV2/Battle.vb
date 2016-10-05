Namespace BattleSystem

    Public Class Battle

        Public Shared Won As Boolean = False
        Public Shared Fled As Boolean = False

#Region "StartRound"

        Public Structure RoundConst
            Public Enum StepTypes
                Move
                Item
                Switch
                Text
                Flee
            End Enum

            Dim StepType As StepTypes
            Dim Argument As Object
        End Structure

        Public OwnStep As RoundConst
        Public OppStep As RoundConst

        ''' <summary>
        ''' Returns the move of a Pokémon with a specified ID.
        ''' </summary>
        Private Function GetPokemonMoveFromID(ByVal Pokemon As Pokemon, ByVal MoveID As Integer) As Attack
            For Each a As Attack In Pokemon.Attacks
                If a.ID = MoveID Then
                    Return a
                End If
            Next
            Return Pokemon.Attacks(0)
        End Function

        ''' <summary>
        ''' If the battle is a remote battle, then start this function once client input is received.
        ''' </summary>
        Public Sub StartMultiTurnAction(ByVal BattleScreen As BattleScreen)
            'Recharge:
            If BattleScreen.FieldEffects.OwnRecharge > 0 Then
                BattleScreen.FieldEffects.OwnRecharge -= 1
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Text, .Argument = BattleScreen.OwnPokemon.GetDisplayName() & " needs to recharge!"})
                Exit Sub
            End If

            'Rollout
            If BattleScreen.FieldEffects.OwnRolloutCounter > 0 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 205)})
                Exit Sub
            End If

            'IceBall
            If BattleScreen.FieldEffects.OwnIceBallCounter > 0 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 301)})
                Exit Sub
            End If

            'Fly:
            If BattleScreen.FieldEffects.OwnFlyCounter >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 19)})
                Exit Sub
            End If

            'Dig:
            If BattleScreen.FieldEffects.OwnDigCounter >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 91)})
                Exit Sub
            End If

            'Outrage:
            If BattleScreen.FieldEffects.OwnOutrage >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 200)})
                Exit Sub
            End If

            'Thrash:
            If BattleScreen.FieldEffects.OwnThrash >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 37)})
                Exit Sub
            End If

            'Petal Dance:
            If BattleScreen.FieldEffects.OwnPetalDance >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 80)})
                Exit Sub
            End If

            'Bounce:
            If BattleScreen.FieldEffects.OwnBounceCounter >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 340)})
                Exit Sub
            End If

            'Dive:
            If BattleScreen.FieldEffects.OwnDiveCounter >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 291)})
                Exit Sub
            End If

            'If Shadow Force gets programmed, put this in.
            'If BattleScreen.FieldEffects.OwnShadowForceCounter = 1 Then
            '    InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, xxx)})
            '    Exit Sub
            'End If

            'If Sky Drop gets programmed, put this in.
            'If BattleScreen.FieldEffects.OwnSkyDropCounter = 1 Then
            '    InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, xxx)})
            '    Exit Sub
            'End If

            'Solar Beam:
            If BattleScreen.FieldEffects.OwnSolarBeam >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 76)})
                Exit Sub
            End If

            'Sky Attack:
            If BattleScreen.FieldEffects.OwnSkyAttackCounter >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 143)})
                Exit Sub
            End If

            'Skull Bash:
            If BattleScreen.FieldEffects.OwnSkullBashCounter >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 130)})
                Exit Sub
            End If

            'RazorWind:
            If BattleScreen.FieldEffects.OwnRazorWindCounter >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 13)})
                Exit Sub
            End If

            'Uproar:
            If BattleScreen.FieldEffects.OwnUproar >= 1 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 253)})
                Exit Sub
            End If

            'Bide:
            If BattleScreen.FieldEffects.OwnBideCounter > 0 Then
                SelectedMoveOwn = False
                DeleteHostQuery(BattleScreen)
                InitializeRound(BattleScreen, New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OwnPokemon, 117)})
                Exit Sub
            End If

            'Todo: Leppa Berry
            'Todo: Taunt

            If BattleScreen.IsRemoteBattle = True Then
                BattleScreen.BattleMenu.Visible = True
            End If
        End Sub

        Private Sub DeleteHostQuery(ByVal BattleScreen As BattleScreen)
            If BattleScreen.IsRemoteBattle Then
                BattleScreen.BattleQuery.Clear()
                BattleScreen.TempPVPBattleQuery.Clear()
            End If
        End Sub

        Public Sub StartRound(ByVal BattleScreen As BattleScreen)
            BattleScreen.BattleMenu.MenuState = BattleMenu.MenuStates.Main
            SelectedMoveOwn = True
            SelectedMoveOpp = True

            If Not BattleScreen.IsRemoteBattle Then
                StartMultiTurnAction(BattleScreen)
            End If

            'Going to menu:
            BattleScreen.BattleQuery.Add(New ToggleMenuQueryObject(False))

            If BattleScreen.IsRemoteBattle AndAlso BattleScreen.IsHost Then
                BattleScreen.BattleQuery.Add(New TriggerNewRoundPVPQueryObject())

                BattleScreen.SendHostQuery()
            End If

            For i = 0 To 99
                BattleScreen.InsertCasualCameramove()
            Next
        End Sub

        Public Function GetOppStep(ByVal BattleScreen As BattleScreen, ByVal OwnStep As RoundConst) As RoundConst
            If BattleScreen.RoamingBattle Then
                BattleScreen.FieldEffects.RoamingFled = False
                If BattleCalculation.CanSwitch(BattleScreen, False) Then
                    Return New RoundConst() With {.StepType = RoundConst.StepTypes.Flee, .Argument = BattleScreen.OppPokemon.GetDisplayName() & " fled!"}
                End If
            End If

            If BattleScreen.BattleMode = BattleScreen.BattleModes.Safari Then
                Return BattleCalculation.SafariRound(BattleScreen)
            End If

            If BattleScreen.FieldEffects.OppRecharge > 0 Then
                SelectedMoveOpp = False
                BattleScreen.FieldEffects.OppRecharge -= 1
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Text, .Argument = BattleScreen.OppPokemon.GetDisplayName() & " needs to recharge!"}
            End If

            'Rollout
            If BattleScreen.FieldEffects.OppRolloutCounter > 0 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 205)}
            End If

            'IceBall
            If BattleScreen.FieldEffects.OppIceBallCounter > 0 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 301)}
            End If

            'Fly:
            If BattleScreen.FieldEffects.OppFlyCounter >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 19)}
            End If

            'Dig:
            If BattleScreen.FieldEffects.OppDigCounter >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 91)}
            End If

            'Outrage:
            If BattleScreen.FieldEffects.OppOutrage >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 200)}
            End If

            'Thrash:
            If BattleScreen.FieldEffects.OppThrash >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 37)}
            End If

            'Petal Dance:
            If BattleScreen.FieldEffects.OppPetalDance >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 80)}
            End If

            'Bounce:
            If BattleScreen.FieldEffects.OppBounceCounter >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 340)}
            End If

            'Dive:
            If BattleScreen.FieldEffects.OppDiveCounter = 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 291)}
            End If

            ''Shadow Force:
            'If BattleScreen.FieldEffects.OppShadowForceCounter = 1 Then
            '    Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = (19).ToString()}
            'End If

            ''Sky Drop:
            'If BattleScreen.FieldEffects.OppSkyDropCounter = 1 Then
            '    Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = (19).ToString()}
            'End If

            'Solar Beam:
            If BattleScreen.FieldEffects.OppSolarBeam >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 76)}
            End If

            'Sky Attack:
            If BattleScreen.FieldEffects.OppSkyAttackCounter >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 143)}
            End If

            'Skull Bash:
            If BattleScreen.FieldEffects.OppSkullBashCounter >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 130)}
            End If

            'RazorWind:
            If BattleScreen.FieldEffects.OppRazorWindCounter >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 13)}
            End If

            'Uproar:
            If BattleScreen.FieldEffects.OppUproar >= 1 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 253)}
            End If

            'Bide:
            If BattleScreen.FieldEffects.OppBideCounter > 0 Then
                SelectedMoveOpp = False
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, 117)}
            End If

            'PVP action:
            If BattleScreen.IsRemoteBattle AndAlso BattleScreen.IsHost Then
                BattleScreen.OppStatistics.Turns += 1
                BattleScreen.OwnStatistics.Turns += 1
                If BattleScreen.ReceivedInput.StartsWith("MOVE|") OrElse BattleScreen.ReceivedInput.StartsWith("MEGA|") Then
                    BattleScreen.OppStatistics.Moves += 1
                    If BattleScreen.ReceivedInput.StartsWith("MEGA|") Then
                        BattleScreen.IsMegaEvolvingOpp = True
                    End If
                    Dim moveID As Integer = CInt(BattleScreen.ReceivedInput.Remove(0, 5))
                        Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = GetPokemonMoveFromID(BattleScreen.OppPokemon, moveID)}
                    ElseIf BattleScreen.ReceivedInput.StartsWith("SWITCH|") Then
                        BattleScreen.OppStatistics.Switches += 1
                        Dim switchID As Integer = CInt(BattleScreen.ReceivedInput.Remove(0, 7))
                        Return New RoundConst() With {.StepType = RoundConst.StepTypes.Switch, .Argument = switchID.ToString()}
                    ElseIf BattleScreen.ReceivedInput.StartsWith("TEXT|") Then
                        Dim text As String = BattleScreen.ReceivedInput.Remove(0, 5)
                    Return New RoundConst() With {.StepType = RoundConst.StepTypes.Text, .Argument = text}
                End If
            End If

            'AI move here:
            If BattleScreen.IsTrainerBattle AndAlso Not BattleScreen.IsRemoteBattle Then
                Return TrainerAI.GetAIMove(BattleScreen, OwnStep)
            Else
                Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = BattleScreen.OppPokemon.Attacks(Core.Random.Next(0, BattleScreen.OppPokemon.Attacks.Count))}
            End If
        End Function

        Private Function GetAttack(ByVal BattleScreen As BattleScreen, ByVal own As Boolean, ByVal move As Attack) As RoundConst
            'TODO: Reset rage counters

            Select Case move.Name.ToLower()
                Case "metronome"
                    If move.CurrentPP > 0 Then
                        move.CurrentPP -= 1
                    End If

                    Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = Moves.Normal.Metronome.GetMetronomeMove()}
                Case "mirror move"
                    If move.CurrentPP > 0 Then
                        move.CurrentPP -= 1
                    End If

                    Dim id As Integer = -1
                    If own = True Then
                        If Not BattleScreen.FieldEffects.OppLastMove Is Nothing AndAlso BattleScreen.FieldEffects.OppLastMove.MirrorMoveAffected = True Then
                            id = BattleScreen.FieldEffects.OppLastMove.ID
                        End If
                    Else
                        If Not BattleScreen.FieldEffects.OwnLastMove Is Nothing AndAlso BattleScreen.FieldEffects.OwnLastMove.MirrorMoveAffected = True Then
                            id = BattleScreen.FieldEffects.OwnLastMove.ID
                        End If
                    End If

                    If id <> -1 Then
                        Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = Attack.GetAttackByID(id)}
                    Else
                        Return New RoundConst() With {.StepType = RoundConst.StepTypes.Text, .Argument = "Mirror Move failed!"}
                    End If
                Case "struggle"
                    Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = move}
                Case Else
                    Return New RoundConst() With {.StepType = RoundConst.StepTypes.Move, .Argument = move}
            End Select
        End Function

        Public SelectedMoveOwn As Boolean = True
        Public SelectedMoveOpp As Boolean = True

        'Does the MegaEvolution
        Sub DoMegaEvolution(ByVal BattleScreen As BattleScreen, ByVal own As Boolean)

            Dim p As Pokemon = BattleScreen.OwnPokemon
            If Not own Then
                p = BattleScreen.OppPokemon
            End If
            'Transform a Pokemon into it's Mega Evolution
            Dim _base As String = p.GetDisplayName()
            If p.AdditionalData = "" Then
                Select Case p.Item.ID
                    Case 516, 529
                        p.AdditionalData = "mega_x"
                    Case 517, 530
                        p.AdditionalData = "mega_y"
                    Case Else
                        p.AdditionalData = "mega"
                End Select
                p.ReloadDefinitions()
                p.CalculateStats()
                p.LoadMegaAbility()
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(own, ToggleEntityQueryObject.BattleEntities.OwnPokemon, PokemonForms.GetOverworldSpriteName(p), 0, 1, -1, -1))
                BattleScreen.BattleQuery.Add(New TextQueryObject(_base & " has Mega Evolved!"))
            End If
        End Sub

        'Checks if any pokemon is mega evolving, order based on speed
        Sub MegaEvolCheck(ByVal BattleScreen As BattleScreen)
            If BattleCalculation.MovesFirst(BattleScreen) Then
                If BattleScreen.IsMegaEvolvingOwn Then
                    DoMegaEvolution(BattleScreen, True)
                End If
                If BattleScreen.IsMegaEvolvingOpp Then
                    DoMegaEvolution(BattleScreen, False)
                End If
            Else
                If BattleScreen.IsMegaEvolvingOpp Then
                    DoMegaEvolution(BattleScreen, False)
                End If
                If BattleScreen.IsMegaEvolvingOwn Then
                    DoMegaEvolution(BattleScreen, True)
                End If

            End If
            BattleScreen.IsMegaEvolvingOwn = False
            BattleScreen.IsMegaEvolvingOpp = False
        End Sub



        Public Sub InitializeRound(ByVal BattleScreen As BattleScreen, ByVal OwnStep As RoundConst)
            If BattleHasEnded(BattleScreen) Then
                Exit Sub
            End If

            Dim OppStep As RoundConst = GetOppStep(BattleScreen, OwnStep)
            If OwnStep.StepType = RoundConst.StepTypes.Move Then
                OwnStep = GetAttack(BattleScreen, True, CType(OwnStep.Argument, Attack))
            Else
                BattleScreen.IsMegaEvolvingOwn = False
            End If
            If OppStep.StepType = RoundConst.StepTypes.Move Then
                OppStep = GetAttack(BattleScreen, False, CType(OppStep.Argument, Attack))
            Else
                BattleScreen.IsMegaEvolvingOpp = False
            End If

            'Move,Move
            If OwnStep.StepType = RoundConst.StepTypes.Move And OppStep.StepType = RoundConst.StepTypes.Move Then
                BattleScreen.FieldEffects.OwnUsedMoves.Add(CType(OwnStep.Argument, Attack).ID)
                BattleScreen.FieldEffects.OppUsedMoves.Add(CType(OppStep.Argument, Attack).ID)

                Dim ownMove As Attack = CType(OwnStep.Argument, Attack)
                Dim oppMove As Attack = CType(OppStep.Argument, Attack)

                If SelectedMoveOwn = True Then ownMove.MoveSelected(True, BattleScreen)
                If SelectedMoveOpp = True Then oppMove.MoveSelected(False, BattleScreen)

                Dim first As Boolean = BattleCalculation.AttackFirst(ownMove, oppMove, BattleScreen)
                MegaEvolCheck(BattleScreen)

                If first Then
                    DoAttackRound(BattleScreen, first, ownMove)
                    EndRound(BattleScreen, 1)
                    DoAttackRound(BattleScreen, Not first, oppMove)
                    EndRound(BattleScreen, 2)
                Else
                    DoAttackRound(BattleScreen, first, oppMove)
                    EndRound(BattleScreen, 2)
                    DoAttackRound(BattleScreen, Not first, ownMove)
                    EndRound(BattleScreen, 1)
                End If
            End If

            'Move,Text
            If OwnStep.StepType = RoundConst.StepTypes.Move And OppStep.StepType = RoundConst.StepTypes.Text Then
                MegaEvolCheck(BattleScreen)

                ChangeCameraAngel(0, True, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OppStep.Argument)))
                EndRound(BattleScreen, 2)

                BattleScreen.FieldEffects.OwnUsedMoves.Add(CType(OwnStep.Argument, Attack).ID)
                Dim ownMove As Attack = CType(OwnStep.Argument, Attack)

                If SelectedMoveOwn = True Then ownMove.MoveSelected(True, BattleScreen)

                DoAttackRound(BattleScreen, True, ownMove)
                EndRound(BattleScreen, 1)
            End If

            'Move,Item
            If OwnStep.StepType = RoundConst.StepTypes.Move And OppStep.StepType = RoundConst.StepTypes.Item Then
                MegaEvolCheck(BattleScreen)

                OpponentUseItem(BattleScreen, CInt(CStr(OppStep.Argument).Split(CChar(","))(0)), CInt(CStr(OppStep.Argument).Split(CChar(","))(1)))
                EndRound(BattleScreen, 2)

                BattleScreen.FieldEffects.OwnUsedMoves.Add(CType(OwnStep.Argument, Attack).ID)
                Dim ownMove As Attack = CType(OwnStep.Argument, Attack)
                If SelectedMoveOwn = True Then ownMove.MoveSelected(True, BattleScreen)
                DoAttackRound(BattleScreen, True, ownMove)
                EndRound(BattleScreen, 1)
            End If

            'Move,Switch
            If OwnStep.StepType = RoundConst.StepTypes.Move And OppStep.StepType = RoundConst.StepTypes.Switch Then
                MegaEvolCheck(BattleScreen)

                If CType(OwnStep.Argument, Attack).ID = 228 Then 'Pursuit is used by own pokemon and opponent tries to switch.
                    BattleScreen.FieldEffects.OwnPursuit = True
                    BattleScreen.FieldEffects.OwnUsedMoves.Add(CType(OwnStep.Argument, Attack).ID)
                    Dim ownMove As Attack = CType(OwnStep.Argument, Attack)
                    If SelectedMoveOwn = True Then ownMove.MoveSelected(True, BattleScreen)
                    Me.DoAttackRound(BattleScreen, True, ownMove)
                    EndRound(BattleScreen, 1)

                    SwitchOutOpp(BattleScreen, CInt(OppStep.Argument))
                    EndRound(BattleScreen, 2)
                Else
                    SwitchOutOpp(BattleScreen, CInt(OppStep.Argument))
                    EndRound(BattleScreen, 2)

                    BattleScreen.FieldEffects.OwnUsedMoves.Add(CType(OwnStep.Argument, Attack).ID)
                    Dim ownMove As Attack = CType(OwnStep.Argument, Attack)
                    If SelectedMoveOwn = True Then ownMove.MoveSelected(True, BattleScreen)
                    DoAttackRound(BattleScreen, True, ownMove)
                    EndRound(BattleScreen, 1)
                End If
            End If

            'Move,Flee
            If OwnStep.StepType = RoundConst.StepTypes.Move And OppStep.StepType = RoundConst.StepTypes.Flee Then
                MegaEvolCheck(BattleScreen)

                BattleScreen.FieldEffects.OwnUsedMoves.Add(CType(OwnStep.Argument, Attack).ID)
                Dim ownMove As Attack = CType(OwnStep.Argument, Attack)

                If SelectedMoveOwn = True Then ownMove.MoveSelected(True, BattleScreen)

                DoAttackRound(BattleScreen, True, ownMove)
                EndRound(BattleScreen, 1)

                If BattleScreen.OppPokemon.HP > 0 Then
                    If BattleCalculation.CanSwitch(BattleScreen, False) = True Then
                        ChangeCameraAngel(0, True, BattleScreen)
                        Won = True
                        BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\running", False))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OppStep.Argument)))
                        BattleScreen.BattleQuery.Add(New RoamingPokemonFledQueryObject())
                        BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                    Else
                        ChangeCameraAngel(2, True, BattleScreen)

                        BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OppPokemon.GetDisplayName() & " is trapped!"))
                        EndRound(BattleScreen, 2)
                    End If
                End If
            End If

            'Text,Move
            If OwnStep.StepType = RoundConst.StepTypes.Text And OppStep.StepType = RoundConst.StepTypes.Move Then
                MegaEvolCheck(BattleScreen)

                ChangeCameraAngel(0, True, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OwnStep.Argument)))
                EndRound(BattleScreen, 1)

                Dim oppMove As Attack = CType(OppStep.Argument, Attack)
                BattleScreen.FieldEffects.OppUsedMoves.Add(oppMove.ID)
                If SelectedMoveOpp = True Then oppMove.MoveSelected(False, BattleScreen)
                DoAttackRound(BattleScreen, False, oppMove)
                EndRound(BattleScreen, 2)
            End If

            'Text,Text
            If OwnStep.StepType = RoundConst.StepTypes.Text And OppStep.StepType = RoundConst.StepTypes.Text Then
                ChangeCameraAngel(0, True, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OwnStep.Argument)))
                EndRound(BattleScreen, 1)
                BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OppStep.Argument)))
                EndRound(BattleScreen, 2)
            End If

            'Text,Item
            If OwnStep.StepType = RoundConst.StepTypes.Text And OppStep.StepType = RoundConst.StepTypes.Item Then
                ChangeCameraAngel(0, True, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OwnStep.Argument)))
                EndRound(BattleScreen, 1)

                ChangeCameraAngel(2, True, BattleScreen)
                OpponentUseItem(BattleScreen, CInt(CStr(OppStep.Argument).Split(CChar(","))(0)), CInt(CStr(OppStep.Argument).Split(CChar(","))(1)))
                EndRound(BattleScreen, 2)
            End If

            'Text,Switch
            If OwnStep.StepType = RoundConst.StepTypes.Text And OppStep.StepType = RoundConst.StepTypes.Switch Then
                ChangeCameraAngel(0, True, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OwnStep.Argument)))
                EndRound(BattleScreen, 1)

                ChangeCameraAngel(2, True, BattleScreen)
                SwitchOutOpp(BattleScreen, CInt(OppStep.Argument))
                EndRound(BattleScreen, 2)
            End If

            'Text,Flee
            If OwnStep.StepType = RoundConst.StepTypes.Text And OppStep.StepType = RoundConst.StepTypes.Flee Then
                ChangeCameraAngel(0, True, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OwnStep.Argument)))

                If BattleScreen.OppPokemon.HP > 0 Then
                    If BattleCalculation.CanSwitch(BattleScreen, False) = True Then
                        ChangeCameraAngel(0, True, BattleScreen)
                        Won = True
                        BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\running", False))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OppStep.Argument)))
                        BattleScreen.BattleQuery.Add(New RoamingPokemonFledQueryObject())
                        BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                    Else
                        ChangeCameraAngel(2, True, BattleScreen)

                        BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OppPokemon.GetDisplayName() & " is trapped!"))
                        EndRound(BattleScreen, 2)
                    End If
                End If
            End If

            'Switch,Move
            If OwnStep.StepType = RoundConst.StepTypes.Switch And OppStep.StepType = RoundConst.StepTypes.Move Then
                MegaEvolCheck(BattleScreen)

                If BattleCalculation.CanSwitch(BattleScreen, True) = True Then
                    If CType(OppStep.Argument, Attack).ID = 228 Then 'Opp uses pursuit while own tries to switch.
                        BattleScreen.FieldEffects.OppPursuit = True
                        Dim oppMove As Attack = CType(OppStep.Argument, Attack)
                        BattleScreen.FieldEffects.OppUsedMoves.Add(oppMove.ID)
                        If SelectedMoveOpp = True Then oppMove.MoveSelected(False, BattleScreen)
                        DoAttackRound(BattleScreen, False, oppMove)
                        EndRound(BattleScreen, 2)

                        SwitchOutOwn(BattleScreen, CInt(OwnStep.Argument), -1)
                        EndRound(BattleScreen, 1)
                    Else
                        SwitchOutOwn(BattleScreen, CInt(OwnStep.Argument), -1)
                        EndRound(BattleScreen, 1)

                        Dim oppMove As Attack = CType(OppStep.Argument, Attack)
                        BattleScreen.FieldEffects.OppUsedMoves.Add(oppMove.ID)
                        If SelectedMoveOpp = True Then oppMove.MoveSelected(False, BattleScreen)
                        DoAttackRound(BattleScreen, False, oppMove)
                        EndRound(BattleScreen, 2)
                    End If
                Else
                    ChangeCameraAngel(0, True, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & " is trapped!"))

                    BattleScreen.FieldEffects.OppUsedMoves.Add(CInt(OppStep.Argument))
                    Dim oppMove As Attack = Attack.GetAttackByID(CInt(OppStep.Argument))
                    If SelectedMoveOpp = True Then oppMove.MoveSelected(False, BattleScreen)
                    DoAttackRound(BattleScreen, False, oppMove)
                    EndRound(BattleScreen, 2)
                End If
            End If

            'Switch,Text
            If OwnStep.StepType = RoundConst.StepTypes.Switch And OppStep.StepType = RoundConst.StepTypes.Text Then
                If BattleCalculation.CanSwitch(BattleScreen, True) = True Then
                    SwitchOutOwn(BattleScreen, CInt(OwnStep.Argument), -1)
                    EndRound(BattleScreen, 1)

                    ChangeCameraAngel(0, True, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OppStep.Argument)))
                    EndRound(BattleScreen, 2)
                Else
                    ChangeCameraAngel(0, True, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & " is trapped!"))

                    ChangeCameraAngel(0, True, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OppStep.Argument)))
                    EndRound(BattleScreen, 2)
                End If
            End If

            'Switch,Item
            If OwnStep.StepType = RoundConst.StepTypes.Switch And OppStep.StepType = RoundConst.StepTypes.Item Then
                If BattleCalculation.CanSwitch(BattleScreen, True) = True Then
                    SwitchOutOwn(BattleScreen, CInt(OwnStep.Argument), -1)
                    EndRound(BattleScreen, 1)

                    ChangeCameraAngel(2, True, BattleScreen)
                    OpponentUseItem(BattleScreen, CInt(CStr(OppStep.Argument).Split(CChar(","))(0)), CInt(CStr(OppStep.Argument).Split(CChar(","))(1)))
                    EndRound(BattleScreen, 2)
                Else
                    ChangeCameraAngel(0, True, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & " is trapped!"))

                    ChangeCameraAngel(2, True, BattleScreen)
                    OpponentUseItem(BattleScreen, CInt(CStr(OppStep.Argument).Split(CChar(","))(0)), CInt(CStr(OppStep.Argument).Split(CChar(","))(1)))
                    EndRound(BattleScreen, 2)
                End If
            End If

            'Switch,Switch
            If OwnStep.StepType = RoundConst.StepTypes.Switch And OppStep.StepType = RoundConst.StepTypes.Switch Then
                If BattleCalculation.CanSwitch(BattleScreen, True) = True Then
                    SwitchOutOwn(BattleScreen, CInt(OwnStep.Argument), -1)
                    EndRound(BattleScreen, 1)

                    ChangeCameraAngel(2, True, BattleScreen)
                    SwitchOutOpp(BattleScreen, CInt(OppStep.Argument))
                    EndRound(BattleScreen, 2)
                Else
                    ChangeCameraAngel(0, True, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & " is trapped!"))

                    ChangeCameraAngel(2, True, BattleScreen)
                    SwitchOutOpp(BattleScreen, CInt(OppStep.Argument))
                    EndRound(BattleScreen, 2)
                End If
            End If

            'Switch,Flee
            If OwnStep.StepType = RoundConst.StepTypes.Switch And OppStep.StepType = RoundConst.StepTypes.Flee Then
                If BattleCalculation.CanSwitch(BattleScreen, True) = True Then
                    SwitchOutOwn(BattleScreen, CInt(OwnStep.Argument), -1)
                    EndRound(BattleScreen, 1)

                    ChangeCameraAngel(0, True, BattleScreen)
                    Won = True
                    BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OppStep.Argument)))
                    BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                Else
                    ChangeCameraAngel(0, True, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & " is trapped!"))
                End If

                If BattleScreen.OppPokemon.HP > 0 Then
                    If BattleCalculation.CanSwitch(BattleScreen, False) = True Then
                        ChangeCameraAngel(0, True, BattleScreen)
                        Won = True
                        BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\running", False))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OppStep.Argument)))
                        BattleScreen.BattleQuery.Add(New RoamingPokemonFledQueryObject())
                        BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                    Else
                        ChangeCameraAngel(2, True, BattleScreen)

                        BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OppPokemon.GetDisplayName() & " is trapped!"))
                        EndRound(BattleScreen, 2)
                    End If
                End If
            End If

            'Item,Move
            If OwnStep.StepType = RoundConst.StepTypes.Item And OppStep.StepType = RoundConst.StepTypes.Move Then
                MegaEvolCheck(BattleScreen)

                EndRound(BattleScreen, 1)

                Dim oppMove As Attack = CType(OppStep.Argument, Attack)
                BattleScreen.FieldEffects.OppUsedMoves.Add(oppMove.ID)
                If SelectedMoveOpp = True Then oppMove.MoveSelected(False, BattleScreen)
                DoAttackRound(BattleScreen, False, oppMove)
                EndRound(BattleScreen, 2)
            End If

            'Item,Text
            If OwnStep.StepType = RoundConst.StepTypes.Item And OppStep.StepType = RoundConst.StepTypes.Text Then
                EndRound(BattleScreen, 1)
                ChangeCameraAngel(0, True, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OppStep.Argument)))
                EndRound(BattleScreen, 2)
            End If

            'Item,Switch
            If OwnStep.StepType = RoundConst.StepTypes.Item And OppStep.StepType = RoundConst.StepTypes.Switch Then
                EndRound(BattleScreen, 1)

                ChangeCameraAngel(2, True, BattleScreen)
                SwitchOutOpp(BattleScreen, CInt(OppStep.Argument))
                EndRound(BattleScreen, 2)
            End If

            'Item,Item
            If OwnStep.StepType = RoundConst.StepTypes.Item And OppStep.StepType = RoundConst.StepTypes.Item Then
                EndRound(BattleScreen, 1)

                ChangeCameraAngel(2, True, BattleScreen)
                OpponentUseItem(BattleScreen, CInt(CStr(OppStep.Argument).Split(CChar(","))(0)), CInt(CStr(OppStep.Argument).Split(CChar(","))(1)))
                EndRound(BattleScreen, 2)
            End If

            'Item,Flee
            If OwnStep.StepType = RoundConst.StepTypes.Item And OppStep.StepType = RoundConst.StepTypes.Flee Then
                EndRound(BattleScreen, 1)

                If BattleScreen.OppPokemon.HP > 0 Then
                    If BattleCalculation.CanSwitch(BattleScreen, False) = True Then
                        ChangeCameraAngel(0, True, BattleScreen)
                        Won = True
                        BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\running", False))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(CStr(OppStep.Argument)))
                        BattleScreen.BattleQuery.Add(New RoamingPokemonFledQueryObject())
                        BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                    Else
                        ChangeCameraAngel(2, True, BattleScreen)

                        BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OppPokemon.GetDisplayName() & " is trapped!"))
                        EndRound(BattleScreen, 2)
                    End If
                End If
            End If

            EndRound(BattleScreen, 0)
        End Sub

        Private Function BattleHasEnded(ByVal BattleScreen As BattleScreen) As Boolean
            For Each b As QueryObject In BattleScreen.BattleQuery
                If b.QueryType = QueryObject.QueryTypes.EndBattle Then
                    Return True
                End If
            Next
            Return False
        End Function

        Private Sub OpponentUseItem(ByVal BattleScreen As BattleScreen, ByVal ItemID As Integer, ByVal target As Integer)
            Dim p As Pokemon = BattleScreen.OppPokemon

            If target <> -1 Then
                p = BattleScreen.Trainer.Pokemons(target)
            End If

            'Potion,Super Point,Hyper Potion,Full Heal,Full Restore,Burn Heal,Antidote,Paralyze heal,Awakening,Ice Heal,Revive,Max Revive,Max Potion
            Select Case ItemID
                Case 18 'Potion
                    Me.GainHP(20, False, False, BattleScreen, BattleScreen.Trainer.Name & " used a Potion on " & p.GetDisplayName() & "!", "item:potion")
                Case 17 'Super Potion
                    Me.GainHP(50, False, False, BattleScreen, BattleScreen.Trainer.Name & " used a Super Potion on " & p.GetDisplayName() & "!", "item:superpotion")
                Case 16 'Hyper Potion
                    Me.GainHP(100, False, False, BattleScreen, BattleScreen.Trainer.Name & " used a Hyper Potion on " & p.GetDisplayName() & "!", "item:hyperpotion")
                Case 15 'Max Potion
                    Me.GainHP(p.MaxHP, False, False, BattleScreen, BattleScreen.Trainer.Name & " used a Max Potion on " & p.GetDisplayName() & "!", "item:maxpotion")
                Case 14 'Full Restore
                    Me.GainHP(p.MaxHP, False, False, BattleScreen, BattleScreen.Trainer.Name & " used a Full Restore on " & p.GetDisplayName() & "!", "item:fullrestore")
                    Me.CureStatusProblem(False, False, BattleScreen, "", "item:fullrestore")
                Case 38 'Full Heal
                    Me.CureStatusProblem(False, False, BattleScreen, BattleScreen.Trainer.Name & " used a Full Heal on " & p.GetDisplayName() & "!", "item:fullheal")
                Case 9 'Antidote
                    Me.CureStatusProblem(False, False, BattleScreen, BattleScreen.Trainer.Name & " used an Antidote on " & p.GetDisplayName() & "!", "item:antidote")
                Case 10 'Burn Heal
                    Me.CureStatusProblem(False, False, BattleScreen, BattleScreen.Trainer.Name & " used a Burn Heal on " & p.GetDisplayName() & "!", "item:burnheal")
                Case 11 'Ice Heal
                    Me.CureStatusProblem(False, False, BattleScreen, BattleScreen.Trainer.Name & " used an Ice Heal on " & p.GetDisplayName() & "!", "item:iceheal")
                Case 12 'Awakening
                    Me.CureStatusProblem(False, False, BattleScreen, BattleScreen.Trainer.Name & " used an Awakening on " & p.GetDisplayName() & "!", "item:awakening")
                Case 13 'Paralyze Heal
                    Me.CureStatusProblem(False, False, BattleScreen, BattleScreen.Trainer.Name & " used a Paralyze Heal on " & p.GetDisplayName() & "!", "item:paralyzeheal")
                Case 39 'Revive
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.Trainer.Name & " used a Revive on " & p.GetDisplayName() & "!"))
                    p.Status = Pokemon.StatusProblems.None
                    p.HP = CInt(Math.Ceiling(p.MaxHP / 2))
                Case 40 'Max Revive
                    BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.Trainer.Name & " used a Revive on " & p.GetDisplayName() & "!"))
                    p.Status = Pokemon.StatusProblems.None
                    p.HP = p.MaxHP
            End Select

            BattleScreen.Trainer.TrainerItemUse(ItemID)
        End Sub

#End Region

        Public Sub DoAttackRound(ByVal BattleScreen As BattleScreen, ByVal own As Boolean, ByVal moveUsed As Attack)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon

            If Not own Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            'Transform Aegislash with Stance Change ability.
            If p.Ability.Name.ToLower() = "stance change" AndAlso p.Number = 681 Then
                If p.AdditionalData = "" Then
                    If moveUsed.IsDamagingMove Then
                        p.AdditionalData = "blade"
                        p.ReloadDefinitions()
                        p.CalculateStats()
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(own, ToggleEntityQueryObject.BattleEntities.OwnPokemon, PokemonForms.GetOverworldSpriteName(p), 0, 1, -1, -1))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " transformed into Blade Forme!"))
                    End If
                Else
                    If moveUsed.ID = 588 Then
                        p.AdditionalData = ""
                        p.ReloadDefinitions()
                        p.CalculateStats()
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(own, ToggleEntityQueryObject.BattleEntities.OwnPokemon, PokemonForms.GetOverworldSpriteName(p), 0, 1, -1, -1))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " transformed into Shield Forme!"))
                    End If
                End If
            End If

            'Turn count
            If own Then
                BattleScreen.FieldEffects.OwnTurnCounts += 1
            Else
                BattleScreen.FieldEffects.OppTurnCounts += 1
            End If

            'Reset Destiny Bond:
            If moveUsed.ID <> 194 Then
                If own Then
                    BattleScreen.FieldEffects.OwnDestinyBond = False
                Else
                    BattleScreen.FieldEffects.OppDestinyBond = False
                End If
            End If

            If p.HP <= 0 Then
                Exit Sub
            End If

            Dim q As CameraQueryObject = CType(BattleScreen.FocusBattle(), CameraQueryObject) : q.ApplyCurrentCamera = True : BattleScreen.BattleQuery.Add(q)
            BattleScreen.BattleQuery.Add(New DelayQueryObject(20))

            If p.Status = Pokemon.StatusProblems.Freeze Then
                If Core.Random.Next(0, 100) < 20 Then
                    CureStatusProblem(own, own, BattleScreen, p.GetDisplayName() & " thrawed out.", "own defrost")
                Else
                    BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_ice1", False))
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is frozen solid!"))
                    Exit Sub
                End If
            End If

            If p.Status = Pokemon.StatusProblems.Sleep Then
                Dim sleepTurns As Integer = BattleScreen.FieldEffects.OwnSleepTurns
                If Not own Then
                    sleepTurns = BattleScreen.FieldEffects.OppSleepTurns
                End If

                ' If using SleepTalk, then store the last move used as sleeptalk for the upcoming attack.
                If moveUsed.ID = 214 Then
                    If sleepTurns > 0 Then
                        If own Then
                            BattleScreen.FieldEffects.OwnLastMove = moveUsed
                        Else
                            BattleScreen.FieldEffects.OppLastMove = moveUsed
                        End If
                    Else
                        CureStatusProblem(own, own, BattleScreen, p.GetDisplayName() & " woke up!", "sleepturns")
                        BattleScreen.BattleQuery.Add(New TextQueryObject("Sleep Talk failed!"))
                        Exit Sub
                    End If
                Else
                    If (own AndAlso BattleScreen.FieldEffects.OwnLastMove.ID = 214) OrElse (Not own AndAlso BattleScreen.FieldEffects.OppLastMove.ID = 214) Then
                        If own Then
                            BattleScreen.FieldEffects.OwnLastMove = moveUsed
                        Else
                            BattleScreen.FieldEffects.OppLastMove = moveUsed
                        End If
                    Else
                        If sleepTurns > 0 Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is fast asleep."))
                            Exit Sub
                        Else
                            CureStatusProblem(own, own, BattleScreen, p.GetDisplayName() & " woke up!", "sleepturns")
                        End If
                    End If
                End If
            End If

            If p.Ability.Name.ToLower() = "truant" Then
                Dim truantTurn As Integer = BattleScreen.FieldEffects.OwnTruantRound
                If Not own Then
                    truantTurn = BattleScreen.FieldEffects.OppTruantRound
                End If
                If truantTurn = 1 Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " loafes around."))
                    Exit Sub
                End If
            End If

            If moveUsed.Disabled > 0 Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(moveUsed.Name & " is disabled!"))
                Exit Sub
            End If

            Dim imprisoned As Integer = BattleScreen.FieldEffects.OwnImprison
            If own = False Then
                imprisoned = BattleScreen.FieldEffects.OppImprison
            End If
            If imprisoned > 0 Then
                Dim hasMove As Boolean = False
                For Each a As BattleSystem.Attack In op.Attacks
                    If a.ID = moveUsed.ID Then
                        hasMove = True
                        Exit For
                    End If
                Next
                If hasMove = True Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s move is sealed by " & op.GetDisplayName() & "!"))
                    Exit Sub
                End If
            End If

            Dim healBlock As Integer = BattleScreen.FieldEffects.OppHealBlock
            If own = False Then
                healBlock = BattleScreen.FieldEffects.OwnHealBlock
            End If
            If healBlock > 0 Then
                If moveUsed.IsHealingMove = True Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " was prevented from healing!"))
                    Exit Sub
                End If
            End If

            If p.HP > 0 And p.Status <> Pokemon.StatusProblems.Fainted Then
                If op.Ability.Name.ToLower() = "cacophony" And moveUsed.IsSoundMove = True Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " prevented the sound-based move with Cacophony!"))
                        moveUsed.MoveFailsSoundproof(own, BattleScreen)
                        Exit Sub
                    End If
                End If

                If op.Ability.Name.ToLower() = "soundproof" And moveUsed.IsSoundMove = True Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " prevented the sound-based move with Soundproof!"))
                        moveUsed.MoveFailsSoundproof(own, BattleScreen)
                        Exit Sub
                    End If
                End If

                If op.Ability.Name.ToLower() = "sturdy" And moveUsed.IsOneHitKOMove = True Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Sturdy prevented any damage from the 1-Hit-KO move."))
                    Exit Sub
                End If
            End If

            If p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                Dim confusionTurns As Integer = 0
                If own = True Then
                    confusionTurns = BattleScreen.FieldEffects.OwnConfusionTurns
                    BattleScreen.FieldEffects.OwnConfusionTurns -= 1
                Else
                    confusionTurns = BattleScreen.FieldEffects.OppConfusionTurns
                    BattleScreen.FieldEffects.OppConfusionTurns -= 1
                End If
                If confusionTurns = 0 Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is no longer confused!"))
                    p.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is confused!"))
                    If Core.Random.Next(0, 2) = 0 Then
                        Dim a As Attack = New ConfusionAttack()
                        Dim damage As Integer = BattleCalculation.CalculateDamage(a, False, True, True, BattleScreen)
                        ReduceHP(damage, own, own, BattleScreen, p.GetDisplayName() & " hurt itself in confusion.", "confusiondamage")
                        Exit Sub
                    End If
                End If
            End If

            If op.HP > 0 And op.Status <> Pokemon.StatusProblems.Fainted Then
                If Not op.Item Is Nothing Then
                    If op.Item.Name.ToLower() = "king's rock" Or op.Item.Name.ToLower() = "razor fang" And BattleScreen.FieldEffects.CanUseItem(Not own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Not own, BattleScreen) = True Then
                        If Core.Random.Next(0, 100) < 10 Then
                            p.AddVolatileStatus(Pokemon.VolatileStatus.Flinch)
                        End If
                    End If
                End If
            End If

            moveUsed.PreAttack(own, BattleScreen)

            If p.HasVolatileStatus(Pokemon.VolatileStatus.Flinch) = True Then
                p.RemoveVolatileStatus(Pokemon.VolatileStatus.Flinch)
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " flinched and couldn't move!"))
                If p.Ability.Name.ToLower() = "steadfast" Then
                    RaiseStat(own, Not own, BattleScreen, "Speed", 1, "", "steadfast")
                End If
                Exit Sub
            End If

            Dim taunt As Integer = BattleScreen.FieldEffects.OwnTaunt
            If own = False Then
                taunt = BattleScreen.FieldEffects.OppTaunt
            End If
            If taunt > 0 Then
                If moveUsed.Category = Attack.Categories.Status Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " move was prevented due to Taunt!"))
                    Exit Sub
                End If
            End If

            Dim gravity As Integer = BattleScreen.FieldEffects.Gravity
            If gravity > 0 Then
                If moveUsed.DisabledWhileGravity = True Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " move was prevented due to Gravity!"))
                    Exit Sub
                End If
            End If

            If op.HP > 0 And op.Status <> Pokemon.StatusProblems.Fainted Then
                If p.HasVolatileStatus(Pokemon.VolatileStatus.Infatuation) = True Then
                    If Core.Random.Next(0, 2) = 0 Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is in love with " & op.GetDisplayName() & "!"))
                        Exit Sub
                    End If
                End If
            End If

            If p.Status = Pokemon.StatusProblems.Paralyzed Then
                If Core.Random.Next(0, 4) = 0 Then
                    BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_thundershock2", False))
                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is fully paralyzed!" & vbNewLine & "It cannot move!"))
                    Exit Sub
                End If
            End If

            If p.Status = Pokemon.StatusProblems.Freeze Then
                If moveUsed.RemovesFrozen = True Then
                    CureStatusProblem(own, own, BattleScreen, p.GetDisplayName() & " got defrosted by " & moveUsed.Name & ".", "defrostmove")
                End If
            End If

            If own = True Then
                BattleScreen.FieldEffects.OwnLastMove = moveUsed
            Else
                BattleScreen.FieldEffects.OppLastMove = moveUsed
            End If

            If own = True Then
                Dim ObedienceCheck As Integer = BattleCalculation.ObedienceCheck(moveUsed, BattleScreen)
                If ObedienceCheck > 0 Then
                    Select Case ObedienceCheck
                        Case 1
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " ignores orders while asleep!"))
                            Exit Sub
                        Case 2
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " ignores orders!"))
                            moveUsed = CType(GetAttack(BattleScreen, own, p.Attacks(Core.Random.Next(0, p.Attacks.Count))).Argument, Attack)
                            Exit Sub
                        Case 3
                            InflictSleep(own, own, BattleScreen, -1, p.GetDisplayName() & " began to nap!", "obeynap")
                            Exit Sub
                        Case 4
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " won't obey!"))
                            Exit Sub
                        Case 5
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " won't obey!"))
                            Exit Sub
                        Case 6
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " turned away!"))
                            Exit Sub
                        Case 7
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is loafing around!"))
                            Exit Sub
                        Case 8
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " pretended to not notice!"))
                            Exit Sub
                    End Select
                End If
            End If

            Dim substitute As Integer = BattleScreen.FieldEffects.OppSubstitute
            If own = False Then
                substitute = BattleScreen.FieldEffects.OwnSubstitute
            End If

            Dim AllDamage As Integer = 0
            Dim KOED As Boolean = False
            Dim DirectKOED As Boolean = False

            ChangeCameraAngel(1, own, BattleScreen)
            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " used " & moveUsed.Name & "!"))

            If moveUsed.DeductPP(own, BattleScreen) = True Then
                moveUsed.CurrentPP -= 1
                If op.Ability.Name.ToLower() = "pressure" And moveUsed.CurrentPP > 0 Then
                    moveUsed.CurrentPP -= 1
                End If
            End If

            'If there's no opponent (opponent is fainted), skip to end of turn:
            If moveUsed.Target <> Attack.Targets.Self And moveUsed.Target <> Attack.Targets.All Then
                If op.HP <= 0 Or op.Status = Pokemon.StatusProblems.Fainted Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject("But there was no target..."))
                    Exit Sub
                End If
            End If

            'Own Pokémon move animation! This displays any effects that should display on the user of the move.
            moveUsed.UserPokemonMoveAnimation(BattleScreen)

            If moveUsed.Target <> Attack.Targets.Self And moveUsed.FocusOppPokemon = True Then
                If own = True Then
                    Dim ca As QueryObject = BattleScreen.FocusOppPokemon()
                    CType(ca, CameraQueryObject).SetTargetToStart()
                    Dim fa1 As New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.CloseLeft, Color.Black, True, 110)
                    Dim fa2 As New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.CloseRight, Color.Black, False, 110)
                    BattleScreen.BattleQuery.AddRange({fa1, ca, fa2})
                Else
                    Dim ca As QueryObject = BattleScreen.FocusOwnPokemon()
                    CType(ca, CameraQueryObject).SetTargetToStart()
                    Dim fa1 As New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.CloseRight, Color.Black, True, 110)
                    Dim fa2 As New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.CloseLeft, Color.Black, False, 110)
                    BattleScreen.BattleQuery.AddRange({fa1, ca, fa2})
                End If
                'ChangeCameraAngel(2, own, BattleScreen)
            End If

            Dim DoesNotMiss As Boolean = BattleCalculation.AccuracyCheck(moveUsed, own, BattleScreen)

            Dim lockon As Integer = BattleScreen.FieldEffects.OwnLockOn
            If own = False Then
                lockon = BattleScreen.FieldEffects.OppLockOn
            End If

            If lockon > 0 Then
                DoesNotMiss = True
            Else
                If DoesNotMiss = True And moveUsed.Target <> Attack.Targets.Self Then 'Dig check
                    Dim dig As Integer = BattleScreen.FieldEffects.OppDigCounter
                    If own = False Then
                        dig = BattleScreen.FieldEffects.OwnDigCounter
                    End If

                    If dig > 0 And moveUsed.CanHitUnderground = False Then
                        DoesNotMiss = False
                    End If
                End If

                If DoesNotMiss = True And moveUsed.Target <> Attack.Targets.Self Then 'Fly check
                    Dim fly As Integer = BattleScreen.FieldEffects.OppFlyCounter
                    If own = False Then
                        fly = BattleScreen.FieldEffects.OwnFlyCounter
                    End If

                    If fly > 0 And moveUsed.CanHitInMidAir = False Then
                        DoesNotMiss = False
                    End If
                End If

                If DoesNotMiss = True And moveUsed.Target <> Attack.Targets.Self Then 'bounce check
                    Dim bounce As Integer = BattleScreen.FieldEffects.OppBounceCounter
                    If own = False Then
                        bounce = BattleScreen.FieldEffects.OwnBounceCounter
                    End If

                    If bounce > 0 And moveUsed.CanHitInMidAir = False Then
                        DoesNotMiss = False
                    End If
                End If

                If DoesNotMiss = True And moveUsed.Target <> Attack.Targets.Self Then 'dive check
                    Dim dive As Integer = BattleScreen.FieldEffects.OppDiveCounter
                    If own = False Then
                        dive = BattleScreen.FieldEffects.OwnDiveCounter
                    End If

                    If dive > 0 And moveUsed.CanHitInMidAir = False Then
                        DoesNotMiss = False
                    End If
                End If

                If DoesNotMiss = True And moveUsed.Target <> Attack.Targets.Self Then 'shadowforce check
                    Dim shadowforce As Integer = BattleScreen.FieldEffects.OppShadowForceCounter
                    If own = False Then
                        shadowforce = BattleScreen.FieldEffects.OwnShadowForceCounter
                    End If

                    If shadowforce > 0 Then
                        DoesNotMiss = False
                    End If
                End If

                If DoesNotMiss = True And moveUsed.Target <> Attack.Targets.Self Then 'sky drop check
                    Dim skydrop As Integer = BattleScreen.FieldEffects.OppSkyDropCounter
                    If own = False Then
                        skydrop = BattleScreen.FieldEffects.OwnSkyDropCounter
                    End If

                    If skydrop > 0 And moveUsed.CanHitInMidAir = False Then
                        DoesNotMiss = False
                    End If
                End If
            End If

            If DoesNotMiss = True Then
                Dim effectiveness As Single = BattleCalculation.CalculateEffectiveness(own, moveUsed, BattleScreen)

                Dim oppHealblock As Integer = BattleScreen.FieldEffects.OwnHealBlock
                If own = False Then
                    oppHealblock = BattleScreen.FieldEffects.OppHealBlock
                End If
                Dim moveWorks As Boolean = True

                If moveUsed.MoveFailBeforeAttack(own, BattleScreen) = True Then
                    moveWorks = False
                End If

                If op.Ability.Name.ToLower() = "volt absorb" And moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Electric And moveWorks = True And moveUsed.Category <> Attack.Categories.Status Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        moveWorks = False
                        If oppHealblock > 0 Then
                            ReduceHP(CInt(op.MaxHP / 4), Not own, own, BattleScreen, "Healblock blocked Volt Absorb!", "healblock")
                        Else
                            If op.HP = op.MaxHP Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s Volt Absorb made " & moveUsed.Name & " useless!"))
                            Else
                                GainHP(CInt(op.MaxHP / 4), Not own, Not own, BattleScreen, op.GetDisplayName() & "'s Volt Absorb absorbed the attack!", "volatabsorb")
                            End If
                        End If
                    End If
                End If
                If op.Ability.Name.ToLower() = "motor drive" And moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Electric And moveWorks = True Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        moveWorks = False
                        ChangeCameraAngel(2, own, BattleScreen)
                        If op.StatSpeed = 6 Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s Motor Drive made " & moveUsed.Name & " useless!"))
                        Else
                            RaiseStat(Not own, Not own, BattleScreen, "Speed", 1, op.GetDisplayName() & "'s Motor Drive absorbed the attack!", "motordrive")
                        End If
                    End If
                End If
                If op.Ability.Name.ToLower() = "water absorb" And moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Water And moveWorks = True And moveUsed.Category <> Attack.Categories.Status Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        moveWorks = False
                        If oppHealblock > 0 Then
                            ReduceHP(CInt(op.MaxHP / 4), Not own, own, BattleScreen, "Healblock blocked Water Absorb!", "healblock")
                        Else
                            If op.HP = op.MaxHP Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s Water Absorb made " & moveUsed.Name & " useless!"))
                            Else
                                GainHP(CInt(op.MaxHP / 4), Not own, Not own, BattleScreen, op.GetDisplayName() & "'s Water Absorb absorbed the attack!", "waterabsorb")
                            End If
                        End If
                    End If
                End If
                If op.Ability.Name.ToLower() = "dry skin" And moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Water And moveWorks = True And moveUsed.Category <> Attack.Categories.Status Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        moveWorks = False
                        If oppHealblock > 0 Then
                            ReduceHP(CInt(op.MaxHP / 4), Not own, own, BattleScreen, "Healblock blocked Dry Skin!", "healblock")
                        Else
                            If op.HP = op.MaxHP Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s Dry Skin made " & moveUsed.Name & " useless!"))
                            Else
                                GainHP(CInt(op.MaxHP / 4), Not own, Not own, BattleScreen, op.GetDisplayName() & "'s Dry Skin absorbed the attack!", "dryskin")
                            End If
                        End If
                    End If
                End If
                If op.Ability.Name.ToLower() = "sap sipper" And moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Grass And moveWorks = True And moveUsed.Category <> Attack.Categories.Status Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        moveWorks = False
                        ChangeCameraAngel(2, own, BattleScreen)
                        If op.StatAttack = 6 Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s Sap Sipper made " & moveUsed.Name & " useless!"))
                        Else
                            RaiseStat(Not own, Not own, BattleScreen, "Attack", 1, op.GetDisplayName() & "'s Sap Sipper absorbed the attack!", "sapsnipper")
                        End If
                    End If
                End If
                If op.Ability.Name.ToLower() = "storm drain" And moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Water And moveWorks = True And moveUsed.Category <> Attack.Categories.Status Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        moveWorks = False
                        ChangeCameraAngel(2, own, BattleScreen)
                        If op.StatAttack = 6 Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s Storm Drain made " & moveUsed.Name & " useless!"))
                        Else
                            RaiseStat(Not own, Not own, BattleScreen, "Special Attack", 1, op.GetDisplayName() & "'s Storm Drain absorbed the attack!", "stormdrain")
                        End If
                    End If
                End If
                If op.Ability.Name.ToLower() = "lightningrod" And moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Electric And moveWorks = True And moveUsed.Category <> Attack.Categories.Status Then
                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                        moveWorks = False
                        ChangeCameraAngel(2, own, BattleScreen)
                        If op.StatSpAttack = 6 Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s Lightningrod made " & moveUsed.Name & " useless!"))
                        Else
                            RaiseStat(Not own, Not own, BattleScreen, "Special Attack", 1, op.GetDisplayName() & "'s Lightningrod absorbed the attack!", "lightningrod")
                        End If
                    End If
                End If

                If op.Type1.Type = Element.Types.Grass Or op.Type2.Type = Element.Types.Grass Then
                    If moveUsed.IsPowderMove = True Then
                        moveWorks = False
                        BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " is not affected by " & moveUsed.Name & "!"))
                    End If
                End If

                If op.Type1.Type = Element.Types.Ghost Or op.Type2.Type = Element.Types.Ghost Then
                    If moveUsed.IsTrappingMove = True Then
                        moveWorks = False
                        BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " is not affected by " & moveUsed.Name & "!"))
                    End If
                End If

                If op.Ability.Name.ToLower() = "bulletproof" And moveUsed.IsBulletMove = True Then
                    moveWorks = False
                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " prevents damage with its Bulletproof ability!"))
                End If

                If moveWorks = True Then
                    If op.HP > 0 And op.Status <> Pokemon.StatusProblems.Fainted Then
                        Dim protect As Integer = BattleScreen.FieldEffects.OppProtectCounter
                        If own = False Then
                            protect = BattleScreen.FieldEffects.OwnProtectCounter
                        End If
                        If protect > 0 And moveUsed.ProtectAffected = True Then
                            Dim protectWorks As Boolean = True
                            If p.Ability.Name.ToLower() = "no guard" Then
                                If Core.Random.Next(0, 100) < (100 - moveUsed.GetAccuracy(own, BattleScreen)) Then
                                    protectWorks = False
                                End If
                            End If

                            If protectWorks = True Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " protected itself!"))
                                Exit Sub
                            End If
                        End If

                        Dim detect As Integer = BattleScreen.FieldEffects.OppDetectCounter
                        If own = False Then
                            detect = BattleScreen.FieldEffects.OwnDetectCounter
                        End If
                        If detect > 0 And moveUsed.ProtectAffected = True Then
                            Dim detectWorks As Boolean = True
                            If p.Ability.Name.ToLower() = "no guard" Then
                                If Core.Random.Next(0, 100) < (100 - moveUsed.GetAccuracy(own, BattleScreen)) Then
                                    detectWorks = False
                                End If
                            End If

                            If detectWorks = True Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " protected itself!"))
                                Exit Sub
                            End If
                        End If

                        Dim kingsshield As Integer = BattleScreen.FieldEffects.OppKingsShieldCounter
                        If own = False Then
                            kingsshield = BattleScreen.FieldEffects.OwnKingsShieldCounter
                        End If
                        If kingsshield > 0 And moveUsed.ProtectAffected = True And moveUsed.Category <> Attack.Categories.Status Then
                            Dim kingsshieldWorks As Boolean = True
                            If p.Ability.Name.ToLower() = "no guard" Then
                                If Core.Random.Next(0, 100) < (100 - moveUsed.GetAccuracy(own, BattleScreen)) Then
                                    kingsshieldWorks = False
                                End If
                            End If

                            If kingsshieldWorks = True Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " protected itself!"))

                                If moveUsed.MakesContact = True Then
                                    Me.LowerStat(own, Not own, BattleScreen, "Attack", 2, "", "move:kingsshield")
                                End If

                                Exit Sub
                            End If
                        End If
                    End If

                    'Protean Ability:
                    If p.Ability.Name.ToLower() = "protean" Then
                        If p.Type1.Type <> moveUsed.Type.Type Or p.Type2.Type <> Element.Types.Blank Then
                            p.Type1.Type = moveUsed.Type.Type
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s type changed to " & p.Type1.ToString() & " due to Protean."))
                        End If
                    End If

                    'Opp Pokémon move animation! This displays the move effects that target the other Pokémon and appear after the camera switched around.
                    moveUsed.OpponentPokemonMoveAnimation(BattleScreen)

                    If moveUsed.IsDamagingMove = True Then
                        ChangeCameraAngel(2, own, BattleScreen)
                        If op.Ability.Name.ToLower() = "wonder guard" And effectiveness <= 1.0F And BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True And moveUsed.IsWonderGuardAffected = True Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "s Wonder Guard blocked the attack!"))
                            Exit Sub
                        End If

                        Dim Hits As Integer = 0
                        Dim TimesToAttack As Integer = moveUsed.GetTimesToAttack(own, BattleScreen)

                        For i = 1 To TimesToAttack
                            Dim Critical As Boolean = BattleCalculation.IsCriticalHit(moveUsed, own, BattleScreen)
                            Dim Damage As Integer = moveUsed.GetDamage(Critical, own, Not own, BattleScreen)

                            If effectiveness <> 0 Then
                                Dim sturdyWorked As Boolean = False

                                If substitute = 0 Then
                                    If op.HP = op.MaxHP And op.MaxHP <= Damage Then
                                        DirectKOED = True
                                    Else
                                        DirectKOED = False
                                    End If

                                    If DirectKOED = True And op.Ability.Name.ToLower() = "sturdy" And BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                                        DirectKOED = False
                                        sturdyWorked = True
                                        Damage = op.MaxHP - 1
                                    End If

                                    If op.HP <= Damage Then
                                        KOED = True
                                    Else
                                        KOED = False
                                    End If
                                End If

                                AllDamage += Damage

                                If substitute = 0 Then
                                    If own = True Then
                                        Dim didDamage As Integer = Damage
                                        If didDamage > op.HP Then
                                            didDamage = op.HP
                                        End If

                                        BattleScreen.FieldEffects.OwnLastDamage = didDamage

                                        If BattleScreen.FieldEffects.OppBideCounter > 0 Then
                                            BattleScreen.FieldEffects.OppBideDamage += didDamage
                                        End If
                                    Else
                                        Dim didDamage As Integer = Damage
                                        If didDamage > op.HP Then
                                            didDamage = op.HP
                                        End If

                                        BattleScreen.FieldEffects.OppLastDamage = didDamage

                                        If BattleScreen.FieldEffects.OwnBideCounter > 0 Then
                                            BattleScreen.FieldEffects.OwnBideDamage += didDamage
                                        End If
                                    End If
                                End If

                                moveUsed.BeforeDealingDamage(own, BattleScreen)

                                If substitute = 0 Then
                                    Dim endure As Integer = BattleScreen.FieldEffects.OppEndure
                                    If own = False Then
                                        endure = BattleScreen.FieldEffects.OwnEndure
                                    End If

                                    Dim endureWorked As Boolean = False

                                    If endure > 0 And effectiveness <> 0 Then
                                        If Damage > op.HP Then
                                            Damage = op.HP - 1
                                            endureWorked = True
                                        End If
                                    End If

                                    If own = True Then
                                        BattleScreen.FieldEffects.OppPokemonDamagedThisTurn = True
                                    Else
                                        BattleScreen.FieldEffects.OwnPokemonDamagedThisTurn = True
                                    End If

                                    Dim sound As String = "Battle\Damage\normaldamage"
                                    If effectiveness > 1.0F Then
                                        sound = "Battle\Damage\super_effective"
                                    End If
                                    If effectiveness < 1.0F And effectiveness <> 0.0F Then
                                        sound = "Battle\Damage\not_effective"
                                    End If

                                    ReduceHP(Damage, Not own, own, BattleScreen, "", "battledamage", sound)

                                    If sturdyWorked = True Then
                                        BattleScreen.BattleQuery.Add(New TextQueryObject("Sturdy prevented " & op.GetDisplayName() & " from fainting!"))
                                    End If
                                    If endureWorked = True Then
                                        BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " endured the attack."))
                                    End If
                                Else
                                    If own = True Then
                                        BattleScreen.FieldEffects.OppSubstitute -= Damage
                                        BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OppPokemon.GetDisplayName() & "'s substitute took the damage!"))
                                        If BattleScreen.FieldEffects.OppSubstitute <= 0 Then
                                            BattleScreen.FieldEffects.OppSubstitute = 0
                                            BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(False, ToggleEntityQueryObject.BattleEntities.OwnPokemon, PokemonForms.GetOverworldSpriteName(BattleScreen.OppPokemon), 0, 1, -1, -1))
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OppPokemon.GetDisplayName() & " substitute broke!"))
                                            Exit For
                                        End If
                                    Else
                                        BattleScreen.FieldEffects.OwnSubstitute -= Damage
                                        BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & "'s substitute took the damage!"))
                                        If BattleScreen.FieldEffects.OwnSubstitute <= 0 Then
                                            BattleScreen.FieldEffects.OwnSubstitute = 0
                                            BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OwnPokemon, PokemonForms.GetOverworldSpriteName(BattleScreen.OwnPokemon), 0, 1, -1, -1))
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.OwnPokemon.GetDisplayName() & " substitute broke!"))
                                            Exit For
                                        End If
                                    End If
                                End If
                            End If

                            If effectiveness > 1.0F Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject("It's super effective!"))
                                If BattleScreen.IsRemoteBattle = True And BattleScreen.IsHost = True Then
                                    If own = True Then
                                        BattleScreen.OwnStatistics.SuperEffective += 1
                                    Else
                                        BattleScreen.OppStatistics.SuperEffective += 1
                                    End If
                                End If
                            ElseIf effectiveness < 1.0F And effectiveness <> 0.0F Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject("It's not very effective..."))
                                If BattleScreen.IsRemoteBattle = True And BattleScreen.IsHost = True Then
                                    If own = True Then
                                        BattleScreen.OwnStatistics.NotVeryEffective += 1
                                    Else
                                        BattleScreen.OppStatistics.NotVeryEffective += 1
                                    End If
                                End If
                            ElseIf effectiveness = 0.0F Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject("It has no effect..."))
                                If BattleScreen.IsRemoteBattle = True And BattleScreen.IsHost = True Then
                                    If own = True Then
                                        BattleScreen.OwnStatistics.NoEffect += 1
                                    Else
                                        BattleScreen.OppStatistics.NoEffect += 1
                                    End If
                                End If
                                Exit For
                            End If

                            If Critical = True And effectiveness <> 0 Then
                                If BattleScreen.IsRemoteBattle = True And BattleScreen.IsHost = True Then
                                    If own = True Then
                                        BattleScreen.OwnStatistics.Critical += 1
                                    Else
                                        BattleScreen.OppStatistics.Critical += 1
                                    End If
                                End If
                                BattleScreen.BattleQuery.Add(New TextQueryObject("It's a critical hit!"))

                                If op.Ability.Name.ToLower() = "anger point" And op.StatAttack < 6 And op.HP > 0 Then
                                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "s Anger Point maxed it's attack!"))
                                    op.StatAttack = 6
                                End If
                            End If

                            If effectiveness <> 0 Then
                                Dim canUseEffect As Boolean = True
                                If op.Ability.Name.ToLower() = "shield dust" And moveUsed.HasSecondaryEffect = True Then
                                    If BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                                        canUseEffect = False
                                    End If
                                End If
                                If p.Ability.Name.ToLower() = "sheer force" And moveUsed.HasSecondaryEffect = True Then
                                    canUseEffect = False
                                End If

                                If canUseEffect = True Then
                                    If substitute = 0 Or moveUsed.IsAffectedBySubstitute = False Then
                                        moveUsed.MoveHits(own, BattleScreen)
                                    End If
                                End If
                                moveUsed.MoveRecoil(own, BattleScreen)

                                If op.HP > 0 Then
                                    If own = True Then
                                        If BattleScreen.FieldEffects.OppRageCounter > 0 Then
                                            BattleScreen.FieldEffects.OppRageCounter += 1
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " built up its rage."))
                                        End If
                                    Else
                                        If BattleScreen.FieldEffects.OwnRageCounter > 0 Then
                                            BattleScreen.FieldEffects.OwnRageCounter += 1
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " built up its rage."))
                                        End If
                                    End If
                                End If
                            Else
                                moveUsed.MoveHasNoEffect(own, BattleScreen)
                            End If

                            'ABILITY SHIT GOES HERE:
                            Select Case op.Ability.Name.ToLower()
                                Case "color change"
                                    If op.HP > 0 Then
                                        If op.Type1.Type <> moveUsed.GetAttackType(own, BattleScreen).Type Or op.Type2.Type <> Element.Types.Blank Then
                                            ChangeCameraAngel(2, own, BattleScreen)
                                            op.OriginalType1 = op.Type1
                                            op.OriginalType2 = op.Type2

                                            op.Type1 = moveUsed.GetAttackType(own, BattleScreen)
                                            op.Type2.Type = Element.Types.Blank
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " changed it's color!"))
                                        End If
                                    End If
                                Case "rough skin"
                                    If moveUsed.MakesContact = True Then
                                        ReduceHP(CInt(Math.Floor(p.MaxHP / 16)), own, Not own, BattleScreen, p.GetDisplayName() & " was harmed by Rough Skin.", "roughskin")
                                    End If
                                Case "static"
                                    If moveUsed.MakesContact = True And p.Status = Pokemon.StatusProblems.None Then
                                        If Core.Random.Next(0, 100) < 30 Then
                                            InflictParalysis(own, Not own, BattleScreen, op.GetDisplayName() & "'s Static affects " & p.GetDisplayName() & "!", "static")
                                        End If
                                    End If
                                Case "effect spore"
                                    If moveUsed.MakesContact = True And p.Status = Pokemon.StatusProblems.None Then
                                        Dim R As Integer = Core.Random.Next(0, 100)
                                        If R < 30 Then
                                            If R < 9 Then
                                                InflictPoison(own, Not own, BattleScreen, False, op.GetDisplayName() & "'s Effect Spore affects " & p.GetDisplayName() & "!", "effectspore")
                                            ElseIf R >= 9 And R < 19 Then
                                                InflictParalysis(own, Not own, BattleScreen, op.GetDisplayName() & "'s Effect Spore affects " & p.GetDisplayName() & "!", "effectspore")
                                            Else
                                                InflictSleep(own, Not own, BattleScreen, -1, op.GetDisplayName() & "'s Effect Spore affects " & p.GetDisplayName() & "!", "effectspore")
                                            End If
                                        End If
                                    End If
                                Case "poison point"
                                    If moveUsed.MakesContact = True And p.Status = Pokemon.StatusProblems.None Then
                                        If Core.Random.Next(0, 100) < 30 Then
                                            InflictPoison(own, Not own, BattleScreen, False, op.GetDisplayName() & "'s Poison Point affects " & p.GetDisplayName() & "!", "poisonpoint")
                                        End If
                                    End If
                                Case "flame body"
                                    If moveUsed.MakesContact = True And p.Status = Pokemon.StatusProblems.None Then
                                        If Core.Random.Next(0, 100) < 30 Then
                                            InflictBurn(own, Not own, BattleScreen, op.GetDisplayName() & "'s Flame Body affects " & p.GetDisplayName() & "!", "flamebody")
                                        End If
                                    End If
                                Case "cute charm"
                                    If moveUsed.MakesContact = True And p.HasVolatileStatus(Pokemon.VolatileStatus.Infatuation) = False Then
                                        If Core.Random.Next(0, 100) < 30 Then
                                            InflictInfatuate(own, Not own, BattleScreen, op.GetDisplayName() & "'s Cute Charm affects " & p.GetDisplayName() & "!", "cutecharm")
                                        End If
                                    End If
                                Case "aftermath"
                                    If moveUsed.MakesContact = True Then
                                        If op.HP <= 0 Then
                                            ReduceHP(CInt(p.MaxHP / 4), own, Not own, BattleScreen, "Aftermath caused damage!", "aftermath")
                                        End If
                                    End If
                                Case "iron barbs"
                                    If moveUsed.MakesContact = True Then
                                        ReduceHP(CInt(p.MaxHP / 8), own, Not own, BattleScreen, "Iron Barbs caused damage!", "ironbarbs")
                                    End If
                                Case "cursed body"
                                    If moveUsed.Disabled = 0 Then
                                        If substitute = 0 Then
                                            If Core.Random.Next(0, 100) < 30 Then
                                                ChangeCameraAngel(2, own, BattleScreen)
                                                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s Cursed Body disabled " & moveUsed.Name & "!"))
                                                moveUsed.Disabled = Core.Random.Next(1, 6)
                                            End If
                                        End If
                                    End If
                                Case "mummy"
                                    If moveUsed.MakesContact = True Then
                                        If p.Ability.Name.ToLower() <> "multitype" And p.Ability.Name.ToLower() <> "mummy" Then
                                            p.OriginalAbility = p.Ability
                                            p.Ability = Ability.GetAbilityByID(152)
                                            ChangeCameraAngel(1, own, BattleScreen)
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s ability changed to Mummy!"))
                                        End If
                                    End If
                                Case "justified"
                                    If moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Dark Then
                                        RaiseStat(Not own, Not own, BattleScreen, "Attack", 1, op.GetDisplayName() & " became justified!", "justified")
                                    End If
                                Case "rattled"
                                    If moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Dark Or moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Bug Or moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Ghost Then
                                        RaiseStat(Not own, Not own, BattleScreen, "Speed", 1, op.GetDisplayName() & "'s Rattled affected it's clairaudience.", "rattled")
                                    End If
                                Case "gooey"
                                    If moveUsed.MakesContact = True Then
                                        LowerStat(own, Not own, BattleScreen, "Speed", 1, "Gooey slowed down " & p.GetDisplayName() & "!", "gooey")
                                    End If
                            End Select

                            Select Case p.Ability.Name.ToLower()
                                Case "poison touch"
                                    If moveUsed.MakesContact = True And op.Status = Pokemon.StatusProblems.None Then
                                        If Core.Random.Next(0, 100) < 30 Then
                                            InflictPoison(Not own, own, BattleScreen, False, p.GetDisplayName() & "'s Poison Touch affects " & op.GetDisplayName() & "!", "poisontouch")
                                        End If
                                    End If
                                Case "pickpocket", "magician"
                                    If moveUsed.MakesContact = True Then
                                        If Not op.Item Is Nothing And p.Item Is Nothing And substitute = 0 Then
                                            If RemoveHeldItem(Not own, own, BattleScreen, p.GetDisplayName() & " stole an item from " & op.GetDisplayName() & "!", "pickpocket") Then
                                                p.Item = BattleScreen.FieldEffects.OppLostItem
                                            End If
                                        End If
                                    End If
                                Case "moxie"
                                    If KOED = True Then
                                        RaiseStat(own, own, BattleScreen, "Attack", 1, p.GetDisplayName() & "'s Moxie got in effect!", "moxie")
                                    End If
                                Case "weak armor"
                                    If moveUsed.Category = Attack.Categories.Physical Then
                                        RaiseStat(own, own, BattleScreen, "Speed", 1, "Weak Armor causes the Speed to increase!", "weakarmor")
                                        LowerStat(own, own, BattleScreen, "Defense", 1, "Weak Armor causes the Defense to decrease!", "weakarmor")
                                    End If
                            End Select

                            If substitute = 0 And op.HP > 0 Then
                                If Not op.Item Is Nothing Then
                                    If BattleScreen.FieldEffects.CanUseItem(Not own) = True And BattleScreen.FieldEffects.CanUseOwnItem(Not own, BattleScreen) = True Then
                                        Select Case op.Item.Name.ToLower()
                                            Case "enigma"
                                                If RemoveHeldItem(Not own, Not own, BattleScreen, op.GetDisplayName() & " used the Enigma Berry to recover.", "berry:enigma") = True Then
                                                    GainHP(CInt(Math.Ceiling(op.MaxHP / 4)), Not own, Not own, BattleScreen, "", "berry:enigma")
                                                End If
                                            Case "jaboca"
                                                If moveUsed.Category = Attack.Categories.Physical Then
                                                    If AllDamage > 0 Then
                                                        If RemoveHeldItem(Not own, Not own, BattleScreen, "", "berry:jaboca") = True Then
                                                            InflictRecoil(own, Not own, BattleScreen, Nothing, CInt(Math.Ceiling(AllDamage / 2)), "The Jaboca Berry damaged " & p.GetDisplayName() & "!", "berry:jaboca")
                                                        End If
                                                    End If
                                                End If
                                            Case "rowap"
                                                If moveUsed.Category = Attack.Categories.Special Then
                                                    If AllDamage > 0 Then
                                                        If RemoveHeldItem(Not own, Not own, BattleScreen, "", "berry:rowap") = True Then
                                                            InflictRecoil(own, Not own, BattleScreen, Nothing, CInt(Math.Ceiling(AllDamage / 2)), "The Rowap Berry damaged " & p.GetDisplayName() & "!", "berry:rowap")
                                                        End If
                                                    End If
                                                End If
                                        End Select
                                    End If
                                End If
                            End If

                            Hits += 1

                            If op.HP <= 0 Then
                                Me.FaintPokemon(Not own, BattleScreen, "")

                                Dim destinyBond As Boolean = False
                                If own = True Then
                                    destinyBond = BattleScreen.FieldEffects.OppDestinyBond
                                Else
                                    destinyBond = BattleScreen.FieldEffects.OwnDestinyBond
                                End If

                                'Faint other pokemon:
                                If destinyBond = True Then
                                    ReduceHP(p.HP, own, Not own, BattleScreen, op.GetDisplayName() & " took its attacker with it!", "move:destinybond")

                                    Me.FaintPokemon(own, BattleScreen, "")
                                End If

                                Exit For
                            End If

                            If op.HP > 0 And effectiveness <> 0 Then
                                If moveUsed.GetAttackType(own, BattleScreen).Type = Element.Types.Fire Then
                                    If op.Status = Pokemon.StatusProblems.Freeze Then
                                        CureStatusProblem(Not own, own, BattleScreen, op.GetDisplayName() & " got defrosted by " & moveUsed.Name & ".", "defrostedfire")
                                    End If
                                End If
                            End If
                        Next

                        If (Hits > 1 Or TimesToAttack > 1) And effectiveness <> 0.0F Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Hit " & Hits & " times!"))
                        End If

                        'ABILITY SHIT GOES HERE

                        If Not p.Item Is Nothing Then
                            If p.Item.Name.ToLower() = "sticky barb" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                If p.Ability.Name.ToLower() <> "magic guard" Then
                                    ReduceHP(CInt(Math.Floor(p.MaxHP / 8)), True, True, BattleScreen, p.GetDisplayName() & " was harmed by Sticky Barb.", "stickybarb")
                                End If

                                If Core.Random.Next(0, 2) = 0 And moveUsed.MakesContact = True And op.Item Is Nothing And op.HP > 0 Then
                                    ChangeCameraAngel(2, own, BattleScreen)
                                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s Sticky Barb was passed over to " & op.GetDisplayName() & "."))
                                    op.Item = Item.GetItemByID(p.Item.ID)
                                    p.Item = Nothing
                                End If 'CODELINE OF EVIL
                            End If
                        End If

                        If p.HP > 0 Then
                            If Not p.Item Is Nothing And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                Select Case p.Item.Name.ToLower()
                                    Case "shell bell"
                                        If p.HP < p.MaxHP Then
                                            GainHP(CInt(AllDamage / 8), own, own, BattleScreen, p.GetDisplayName() & " gains some HP due to the Shell Bell.", "shellbell")
                                        End If
                                    Case "life orb"
                                        If p.Ability.Name.ToLower() <> "magic guard" Then
                                            ReduceHP(CInt(p.MaxHP / 10), own, own, BattleScreen, p.GetDisplayName() & " loses HP due to Life Orb.", "lifeorb")
                                        End If
                                End Select
                            End If
                        End If
                    Else
                        Dim lastMove As Attack = BattleScreen.FieldEffects.OppLastMove
                        If own = False Then
                            lastMove = BattleScreen.FieldEffects.OwnLastMove
                        End If
                        If moveUsed.SnatchAffected = True And Not lastMove Is Nothing AndAlso lastMove.ID = 289 Then 'Snatch
                            BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " snatched the " & p.GetDisplayName() & "'s move!"))

                            moveUsed.MoveHits(Not own, BattleScreen)
                        Else
                            Dim magicReflect As String = ""
                            If moveUsed.MagicCoatAffected = True Then
                                If op.Ability.Name.ToLower() = "magic bounce" And BattleScreen.FieldEffects.CanUseAbility(Not own, BattleScreen) = True Then
                                    magicReflect = "Magic Bounce"
                                Else
                                    If own = True And BattleScreen.FieldEffects.OppMagicCoat > 0 Or own = False And BattleScreen.FieldEffects.OwnMagicCoat > 0 Then
                                        magicReflect = "Magic Coat"
                                    End If
                                End If
                            End If

                            If magicReflect <> "" Then
                                BattleScreen.BattleQuery.Add(New TextQueryObject(magicReflect & " bounced the attack back!"))

                                'Animation (switched)

                                effectiveness = BattleCalculation.CalculateEffectiveness(Not own, moveUsed, BattleScreen)

                                Dim oppSubstitute As Integer = BattleScreen.FieldEffects.OwnSubstitute
                                If own = False Then
                                    oppSubstitute = BattleScreen.FieldEffects.OppSubstitute
                                End If

                                If effectiveness = 0.0F Then
                                    BattleScreen.BattleQuery.Add(New TextQueryObject("It has no effect..."))
                                    moveUsed.MoveHasNoEffect(Not own, BattleScreen)
                                Else
                                    If oppSubstitute = 0 Or moveUsed.IsAffectedBySubstitute = False Then
                                        moveUsed.MoveHits(Not own, BattleScreen)
                                    Else
                                        BattleScreen.BattleQuery.Add(New TextQueryObject("The substitute absorbed the move!"))
                                    End If
                                End If
                            Else
                                If effectiveness = 0.0F Then
                                    BattleScreen.BattleQuery.Add(New TextQueryObject("It has no effect..."))
                                    moveUsed.MoveHasNoEffect(own, BattleScreen)
                                Else
                                    If substitute = 0 Or moveUsed.IsAffectedBySubstitute = False Then
                                        moveUsed.MoveHits(own, BattleScreen)
                                    Else
                                        BattleScreen.BattleQuery.Add(New TextQueryObject("The substitute absorbed the move!"))
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                If moveUsed.Category = Attack.Categories.Status Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject("But it failed..."))
                Else
                    BattleScreen.BattleQuery.Add(New TextQueryObject("But it missed..."))
                End If
                moveUsed.MoveMisses(own, BattleScreen)
            End If
        End Sub

        ''' <summary>
        ''' Faints a Pokémon.
        ''' </summary>
        ''' <param name="own">true: faints own, false: faints opp</param>
        Public Sub FaintPokemon(ByVal own As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            p.HP = 0
            p.Status = Pokemon.StatusProblems.Fainted
            Me.ChangeCameraAngel(1, own, BattleScreen)
            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject(p.Number.ToString(), True))

            If message = "" Then
                message = p.GetDisplayName() & " fainted!"
            End If
            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
        End Sub

#Region "Applystuff"

        Public Function CureStatusProblem(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If message <> "" Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(message))
            End If

            p.Status = Pokemon.StatusProblems.None
            Return True
        End Function

        Public Function InflictFlinch(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            If p.HasVolatileStatus(Pokemon.VolatileStatus.Flinch) = True Then
                Return False
            End If

            If p.Ability.Name.ToLower() = "inner focus" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " won't flinch because of its Inner Focus!"))
                Return False
            Else
                Dim substitute As Integer = BattleScreen.FieldEffects.OwnSubstitute
                If own = False Then
                    substitute = BattleScreen.FieldEffects.OppSubstitute
                End If
                If substitute > 0 Then
                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject("The substitute prevented flinching."))
                    Return False
                Else
                    'Works!
                    p.AddVolatileStatus(Pokemon.VolatileStatus.Flinch)
                    ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case ""
                            'Print nothing, too.
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                    End Select
                    Return True
                End If
            End If
        End Function

        Public Function InflictBurn(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            If p.Status = Pokemon.StatusProblems.Burn Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is already burned!"))
                Return False
            End If

            If p.Status <> Pokemon.StatusProblems.None Then
                Return False
            End If

            Dim substitute As Integer = BattleScreen.FieldEffects.OwnSubstitute
            If own = False Then
                substitute = BattleScreen.FieldEffects.OppSubstitute
            End If
            If substitute > 0 Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject("The substitute took the burn."))
                Return False
            Else
                If p.Type1.Type = Element.Types.Fire Or p.Type2.Type = Element.Types.Fire Then
                    Return False
                Else
                    If p.Ability.Name.ToLower() = "water veil" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        BattleScreen.BattleQuery.Add(New TextQueryObject("Water Veil prevented the burn."))
                        Return False
                    Else
                        If p.Ability.Name.ToLower.ToLower() = "leaf guard" And BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny And from <> own And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                            Me.ChangeCameraAngel(1, own, BattleScreen)
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Leaf Guard prevented the burn."))
                            Return False
                        Else
                            Dim safeGuard As Integer = BattleScreen.FieldEffects.OwnSafeguard
                            If own = False Then
                                safeGuard = BattleScreen.FieldEffects.OppSafeguard
                            End If
                            If safeGuard > 0 And op.Ability.Name.ToLower() <> "infiltrator" Then
                                Me.ChangeCameraAngel(1, own, BattleScreen)
                                BattleScreen.BattleQuery.Add(New TextQueryObject("Safeguard prevented the burn."))
                                Return False
                            Else
                                'Works!
                                p.Status = Pokemon.StatusProblems.Burn
                                ChangeCameraAngel(1, own, BattleScreen)
                                BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_ember", False))
                                Select Case message
                                    Case "" 'Print default message only
                                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " got burned!"))
                                    Case "-1" 'Print no message at all
                                        'Do nothing
                                    Case Else 'Print message given in 'message'
                                        BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " got burned!"))
                                End Select
                                If p.Ability.Name.ToLower() = "synchronize" And from <> own Then
                                    Me.InflictBurn(Not own, Not own, BattleScreen, "Synchronize passed over the burn.", "synchronize")
                                End If

                                If Not p.Item Is Nothing Then
                                    If p.Item.Name.ToLower() = "rawst" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                        If RemoveHeldItem(own, own, BattleScreen, "", "berry:rawst") = True Then
                                            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                                            CureStatusProblem(own, own, BattleScreen, "The Rawst Berry cured the burn of " & p.GetDisplayName() & "!", "berry:rawst")
                                        End If
                                    End If
                                End If

                                If Not p.Item Is Nothing Then
                                    If p.Item.Name.ToLower() = "lum" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                        If RemoveHeldItem(own, own, BattleScreen, "", "berry:lum") = True Then
                                            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                                            CureStatusProblem(own, own, BattleScreen, "The Lum Berry cured the burn of " & p.GetDisplayName() & "!", "berry:lum")
                                        End If
                                    End If
                                End If

                                Return True
                            End If
                        End If
                    End If
                End If
            End If
        End Function

        Public Function InflictFreeze(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            If p.Status = Pokemon.StatusProblems.Freeze Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is already frozen!"))
                Return False
            End If

            If p.Status <> Pokemon.StatusProblems.None Then
                Return False
            End If

            Dim substitute As Integer = BattleScreen.FieldEffects.OwnSubstitute
            If own = False Then
                substitute = BattleScreen.FieldEffects.OppSubstitute
            End If
            If substitute > 0 Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject("The substitute took the freeze effect."))
                Return False
            Else
                If BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject("The sunny weather prevented " & p.GetDisplayName() & " from freezing."))
                    Return False
                Else
                    If p.Type1.Type = Element.Types.Ice Or p.Type2.Type = Element.Types.Ice Then
                        If cause <> "move:triattack" And cause <> "move:secretpower" Then
                            Return False
                        End If
                    End If
                    If p.Ability.Name.ToLower() = "magma armor" Then
                        If BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                            If cause <> "move:triattack" And cause <> "move:secretpower" Then
                                Me.ChangeCameraAngel(1, own, BattleScreen)
                                BattleScreen.BattleQuery.Add(New TextQueryObject("Magma Armor prevented the freeze."))
                                Return False
                            End If
                        End If
                    End If
                    If p.Ability.Name.ToLower.ToLower() = "leaf guard" And BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny And from <> own And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        BattleScreen.BattleQuery.Add(New TextQueryObject("Leaf Guard prevented the freeze."))
                        Return False
                    Else
                        Dim safeGuard As Integer = BattleScreen.FieldEffects.OwnSafeguard
                        If own = False Then
                            safeGuard = BattleScreen.FieldEffects.OppSafeguard
                        End If
                        If safeGuard > 0 And op.Ability.Name.ToLower() <> "infiltrator" Then
                            Me.ChangeCameraAngel(1, own, BattleScreen)
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Safeguard prevented the freezing."))
                            Return False
                        Else
                            'Works!
                            p.Status = Pokemon.StatusProblems.Freeze
                            ChangeCameraAngel(1, own, BattleScreen)
                            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_ice1", False))
                            Select Case message
                                Case "" 'Print default message only
                                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " was frozen solid!"))
                                Case "-1" 'Print no message at all
                                    'Do nothing
                                Case Else 'Print message given in 'message'
                                    BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " was frozen solid!"))
                            End Select
                            If p.Ability.Name.ToLower() = "synchronize" And from <> own Then
                                Me.InflictFreeze(Not own, Not own, BattleScreen, "Synchronize passed over the freeze.", "synchronize")
                            End If

                            If Not p.Item Is Nothing Then
                                If p.Item.Name.ToLower() = "aspear" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:aspear") = True Then
                                        BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                                        CureStatusProblem(own, own, BattleScreen, "The Aspear Berry thraw out " & p.GetDisplayName() & "!", "berry:aspear")
                                    End If
                                End If
                            End If

                            If Not p.Item Is Nothing Then
                                If p.Item.Name.ToLower() = "lum" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:lum") = True Then
                                        BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                                        CureStatusProblem(own, own, BattleScreen, "The Lum Berry thraw out " & p.GetDisplayName() & "!", "berry:lum")
                                    End If
                                End If
                            End If

                            Return True
                        End If
                    End If
                End If
            End If
        End Function

        Public Function InflictParalysis(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            If p.Status = Pokemon.StatusProblems.Paralyzed Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is already paralyzed!"))
                Return False
            End If

            If p.Type1.Type = Element.Types.Electric Or p.Type2.Type = Element.Types.Electric Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is not affected by paralysis!"))
                Return False
            End If

            If p.Status <> Pokemon.StatusProblems.None Then
                Return False
            End If

            Dim substitute As Integer = BattleScreen.FieldEffects.OwnSubstitute
            If own = False Then
                substitute = BattleScreen.FieldEffects.OppSubstitute
            End If
            If substitute > 0 Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject("The substitute took the paralysis."))
                Return False
            Else
                If p.Ability.Name.ToLower() = "limber" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Limber prevented the paralysis."))
                    Return False
                Else
                    If p.Ability.Name.ToLower.ToLower() = "leaf guard" And BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny And from <> own And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        BattleScreen.BattleQuery.Add(New TextQueryObject("Leaf Guard prevented the paralysis."))
                        Return False
                    Else
                        Dim safeGuard As Integer = BattleScreen.FieldEffects.OwnSafeguard
                        If own = False Then
                            safeGuard = BattleScreen.FieldEffects.OppSafeguard
                        End If
                        If safeGuard > 0 And op.Ability.Name.ToLower() <> "infiltrator" Then
                            Me.ChangeCameraAngel(1, own, BattleScreen)
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Safeguard prevented the paralysis."))
                            Return False
                        Else
                            'Works!
                            p.Status = Pokemon.StatusProblems.Paralyzed
                            ChangeCameraAngel(1, own, BattleScreen)
                            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_thundershock2", False))
                            Select Case message
                                Case "" 'Print default message only
                                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is paralyzed!" & vbNewLine & "It can't move!"))
                                Case "-1" 'Print no message at all
                                    'Do nothing
                                Case Else 'Print message given in 'message'
                                    BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                                    BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is paralyzed!" & vbNewLine & "It can't move!"))
                            End Select
                            If p.Ability.Name.ToLower() = "synchronize" And from <> own Then
                                Me.InflictParalysis(Not own, Not own, BattleScreen, "Synchronize passed over the paralysis.", "synchronize")
                            End If

                            If Not p.Item Is Nothing Then
                                If p.Item.Name.ToLower() = "cheri" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:cheri") = True Then
                                        BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                                        CureStatusProblem(own, own, BattleScreen, "The Cheri Berry cured the paralysis of " & p.GetDisplayName() & "!", "berry:cheri")
                                    End If
                                End If
                            End If

                            If Not p.Item Is Nothing Then
                                If p.Item.Name.ToLower() = "lum" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:lum") = True Then
                                        BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                                        CureStatusProblem(own, own, BattleScreen, "The Lum Berry cured the paralyzis of " & p.GetDisplayName() & "!", "berry:lum")
                                    End If
                                End If
                            End If

                            Return True
                        End If
                    End If
                End If
            End If
        End Function

        Public Function InflictSleep(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal turnsPreset As Integer, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            Dim SleepTurns As Integer = turnsPreset
            If SleepTurns < 0 Then
                SleepTurns = Core.Random.Next(1, 4)
            End If

            If p.Ability.Name.ToLower() = "early bird" Then
                SleepTurns = CInt(Math.Floor(SleepTurns / 2))
            End If

            If p.Status = Pokemon.StatusProblems.Sleep Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is already asleep!"))
                Return False
            End If

            If p.Status <> Pokemon.StatusProblems.None Then
                If cause <> "move:rest" Then
                    Return False
                End If
            End If

            Dim substitute As Integer = BattleScreen.FieldEffects.OwnSubstitute
            If own = False Then
                substitute = BattleScreen.FieldEffects.OppSubstitute
            End If
            If substitute > 0 Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject("The substitute took the sleep effect."))
                Return False
            Else
                If p.Ability.Name.ToLower() = "vital spirit" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Vital Spirit prevented the sleep."))
                    Return False
                Else
                    If p.Ability.Name.ToLower() = "insomnia" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        BattleScreen.BattleQuery.Add(New TextQueryObject("Insomnia prevented the sleep."))
                        Return False
                    Else
                        If p.Ability.Name.ToLower() = "sweet veil" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                            Me.ChangeCameraAngel(1, own, BattleScreen)
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Sweet Veil prevented the sleep."))
                            Return False
                        Else
                            Dim uproar As Integer = BattleScreen.FieldEffects.OwnUproar
                            If own = False Then
                                uproar = BattleScreen.FieldEffects.OppUproar
                            End If
                            If uproar > 0 Then
                                Me.ChangeCameraAngel(1, own, BattleScreen)
                                BattleScreen.BattleQuery.Add(New TextQueryObject("The Uproar prevented the sleep."))
                                Return False
                            Else
                                Dim safeGuard As Integer = BattleScreen.FieldEffects.OwnSafeguard
                                If own = False Then
                                    safeGuard = BattleScreen.FieldEffects.OppSafeguard
                                End If
                                If safeGuard > 0 And op.Ability.Name.ToLower() <> "infiltrator" Then
                                    Me.ChangeCameraAngel(1, own, BattleScreen)
                                    BattleScreen.BattleQuery.Add(New TextQueryObject("Safeguard prevented the sleep."))
                                    Return False
                                Else
                                    If p.Ability.Name.ToLower.ToLower() = "leaf guard" And BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny And from <> own And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                                        Me.ChangeCameraAngel(1, own, BattleScreen)
                                        BattleScreen.BattleQuery.Add(New TextQueryObject("Leaf Guard prevented the sleep."))
                                        Return False
                                    Else
                                        'Works!
                                        If own = True Then
                                            BattleScreen.FieldEffects.OwnBideCounter = 0
                                            BattleScreen.FieldEffects.OwnBideDamage = 0
                                        Else
                                            BattleScreen.FieldEffects.OppBideCounter = 0
                                            BattleScreen.FieldEffects.OppBideDamage = 0
                                        End If

                                        ChangeCameraAngel(1, own, BattleScreen)
                                        If own = True Then
                                            BattleScreen.FieldEffects.OwnSleepTurns = SleepTurns
                                        Else
                                            BattleScreen.FieldEffects.OppSleepTurns = SleepTurns
                                        End If
                                        p.Status = Pokemon.StatusProblems.Sleep
                                        Select Case message
                                            Case "" 'Print default message only
                                                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " fell asleep!"))
                                            Case "-1" 'Print no message at all
                                                'Do nothing
                                            Case Else 'Print message given in 'message'
                                                BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                                                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " fell asleep!"))
                                        End Select
                                        If p.Ability.Name.ToLower() = "synchronize" And from <> own Then
                                            Me.InflictSleep(Not own, Not own, BattleScreen, SleepTurns, "Synchronize passed over the sleep.", "synchronize")
                                        End If

                                        If Not p.Item Is Nothing Then
                                            If p.Item.Name.ToLower() = "chesto" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                                If RemoveHeldItem(own, own, BattleScreen, "", "berry:chesto") = True Then
                                                    BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                                                    CureStatusProblem(own, own, BattleScreen, "The Chesto Berry woke up " & p.GetDisplayName() & "!", "berry:chesto")
                                                End If
                                            End If
                                        End If

                                        If Not p.Item Is Nothing Then
                                            If p.Item.Name.ToLower() = "lum" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                                If RemoveHeldItem(own, own, BattleScreen, "", "berry:lum") = True Then
                                                    BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                                                    CureStatusProblem(own, own, BattleScreen, "The Lum Berry woke up " & p.GetDisplayName() & "!", "berry:lum")
                                                End If
                                            End If
                                        End If

                                        Return True
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End Function

        Public Function InflictPoison(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal bad As Boolean, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            If p.Status = Pokemon.StatusProblems.Poison Or p.Status = Pokemon.StatusProblems.BadPoison Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is already poisoned!"))
                Return False
            End If

            If p.Status <> Pokemon.StatusProblems.None Then
                Return False
            End If

            Dim substitute As Integer = BattleScreen.FieldEffects.OwnSubstitute
            If own = False Then
                substitute = BattleScreen.FieldEffects.OppSubstitute
            End If
            If substitute > 0 Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject("The substitute took the poison."))
                Return False
            Else
                If p.Type1.Type = Element.Types.Steel Or p.Type1.Type = Element.Types.Poison Or p.Type2.Type = Element.Types.Steel Or p.Type2.Type = Element.Types.Poison Then
                    Return False
                Else
                    If p.Ability.Name.ToLower() = "immunity" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        BattleScreen.BattleQuery.Add(New TextQueryObject("Immunity prevented the sleep."))
                        Return False
                    Else
                        Dim safeGuard As Integer = BattleScreen.FieldEffects.OwnSafeguard
                        If own = False Then
                            safeGuard = BattleScreen.FieldEffects.OppSafeguard
                        End If
                        If safeGuard > 0 And op.Ability.Name.ToLower() <> "infiltrator" Then
                            Me.ChangeCameraAngel(1, own, BattleScreen)
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Safeguard prevented the poison."))
                            Return False
                        Else
                            If p.Ability.Name.ToLower.ToLower() = "leaf guard" And BattleScreen.FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny And from <> own And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                                Me.ChangeCameraAngel(1, own, BattleScreen)
                                BattleScreen.BattleQuery.Add(New TextQueryObject("Leaf Guard prevented the poison."))
                                Return False
                            Else
                                'Works!
                                ChangeCameraAngel(1, own, BattleScreen)
                                BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_poison", False))
                                If bad = True Then
                                    p.Status = Pokemon.StatusProblems.BadPoison
                                    Select Case message
                                        Case "" 'Print default message only
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is badly poisoned!"))
                                        Case "-1" 'Print no message at all
                                            'Do nothing
                                        Case Else 'Print message given in 'message'
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is badly poisoned!"))
                                    End Select
                                Else
                                    p.Status = Pokemon.StatusProblems.Poison
                                    Select Case message
                                        Case "" 'Print default message only
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is poisoned!"))
                                        Case "-1" 'Print no message at all
                                            'Do nothing
                                        Case Else 'Print message given in 'message'
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is poisoned"))
                                    End Select
                                End If
                                If p.Ability.Name.ToLower() = "synchronize" And from <> own Then
                                    Dim addBad As String = ""
                                    If bad = True Then
                                        addBad = " bad"
                                    End If

                                    Me.InflictPoison(Not own, Not own, BattleScreen, bad, "Synchronize passed over the" & bad & " poison.", "synchronize")
                                End If

                                If Not p.Item Is Nothing Then
                                    If p.Item.Name.ToLower() = "pecha" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                        If RemoveHeldItem(own, own, BattleScreen, "", "berry:pecha") = True Then
                                            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                                            CureStatusProblem(own, own, BattleScreen, "The Pecha Berry cured the poison of " & p.GetDisplayName() & "!", "berry:pecha")
                                        End If
                                    End If
                                End If

                                If Not p.Item Is Nothing Then
                                    If p.Item.Name.ToLower() = "lum" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                        If RemoveHeldItem(own, own, BattleScreen, "", "berry:lum") = True Then
                                            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                                            CureStatusProblem(own, own, BattleScreen, "The Lum Berry cured the poison of " & p.GetDisplayName() & "!", "berry:lum")
                                        End If
                                    End If
                                End If

                                Return True
                            End If
                        End If
                    End If
                End If
            End If
        End Function

        Public Function InflictConfusion(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            If p.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is already confused!"))
                Return False
            End If

            Dim confusionTurns As Integer = Core.Random.Next(1, 5)

            If p.Ability.Name.ToLower() = "own tempo" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject("Own Tempo prevented the confusion."))
                Return False
            Else
                'Works!
                p.AddVolatileStatus(Pokemon.VolatileStatus.Confusion)
                Select Case message
                    Case "" 'Print default message only
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is confused!"))
                    Case "-1" 'Print no message at all
                        'Do nothing
                    Case Else 'Print message given in 'message'
                        BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is confused!"))
                End Select
                If own = True Then
                    BattleScreen.FieldEffects.OwnConfusionTurns = confusionTurns
                Else
                    BattleScreen.FieldEffects.OppConfusionTurns = confusionTurns
                End If

                If Not p.Item Is Nothing Then
                    If p.Item.Name.ToLower() = "persim" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                        If RemoveHeldItem(own, own, BattleScreen, "", "berry:persim") = True Then
                            Me.ChangeCameraAngel(1, own, BattleScreen)
                            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
                            BattleScreen.BattleQuery.Add(New TextQueryObject("The Persim Berry cured the confusion of " & p.GetDisplayName() & "!"))
                            If own = True Then
                                BattleScreen.FieldEffects.OwnConfusionTurns = 0
                            Else
                                BattleScreen.FieldEffects.OppConfusionTurns = 0
                            End If
                            p.RemoveVolatileStatus(Pokemon.VolatileStatus.Confusion)
                        End If
                    End If
                End If

                Return True
            End If
        End Function

        Public Function RaiseStat(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal Stat As String, ByVal val As Integer, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            If op.HP > 0 Or op.Status <> Pokemon.StatusProblems.Fainted Then
                If from <> own Then
                    Dim mist As Integer = BattleScreen.FieldEffects.OwnMist
                    If own = False Then
                        mist = BattleScreen.FieldEffects.OppMist
                    End If
                    If mist > 0 And op.Ability.Name.ToLower() <> "infiltrator" Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The mist prevented the status change!"))
                        Return False
                    End If
                End If
            End If

            If p.Ability.Name.ToLower() = "contrary" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                If cause <> "contrary" Then
                    Return LowerStat(own, own, BattleScreen, Stat, val, message & vbNewLine & "Contrary reverted the stat change!", "contrary")
                End If        
            End If

            If p.Ability.Name.ToLower() = "simple" Then
                If BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                    val *= 2
                End If
            End If

            Dim statString As String = Stat.ToLower()
            Select Case statString
                Case "spdefense"
                    statString = "special attack"
                Case "spattack"
                    statString = "special defense"
            End Select

            Dim statC As Integer = 0
            Select Case Stat.ToLower()
                Case "attack"
                    statC = p.StatAttack
                Case "defense"
                    statC = p.StatDefense
                Case "special attack"
                    statC = p.StatSpAttack
                Case "special defense"
                    statC = p.StatSpDefense
                Case "speed"
                    statC = p.StatSpeed
                Case "evasion"
                    statC = p.Evasion
                Case "accuracy"
                    statC = p.Accuracy
            End Select
            If statC >= 6 Then
                'Cannot rise stat higher

                Me.ChangeCameraAngel(1, own, BattleScreen)
                Select Case message
                    Case "" 'Print default message only
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s " & statString & " cannot rise further."))
                    Case "-1" 'Print no message at all
                        'Do nothing
                    Case Else 'Print message given in 'message'
                        BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s " & statString & " cannot rise further."))
                End Select

                Return False
            Else
                If statC + val > 6 Then
                    val = 6 - statC
                End If
            End If
            
            '***SHOW STAT INCREASE ANIMATION HERE***
            
            Dim printMessage As String = p.GetDisplayName() & "'s " & statString
            Select Case val
                Case 2
                    printMessage &= " sharply rose!"
                Case 3
                    printMessage &= " rose drastically!"
                Case Else
                    printMessage &= " slightly rose."
            End Select

            Select Case statString
                Case "attack"
                    p.StatAttack += val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "defense"
                    p.StatDefense += val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "special attack"
                    p.StatSpAttack += val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "special defense"
                    p.StatSpDefense += val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "speed"
                    p.StatSpeed += val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "evasion"
                    p.Evasion += val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "accuracy"
                    p.Accuracy += val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
            End Select

            Logger.Log(Logger.LogTypes.Warning, "BattleV2.vb: Failed to indicate stat change: " & Stat.ToUpper() & "!")
            Return True
        End Function

        Public Function LowerStat(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal Stat As String, ByVal val As Integer, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            If op.HP > 0 And op.Status <> Pokemon.StatusProblems.Fainted Then
                If from <> own Then
                    Dim mist As Integer = BattleScreen.FieldEffects.OwnMist
                    If own = False Then
                        mist = BattleScreen.FieldEffects.OppMist
                    End If
                    If mist > 0 And op.Ability.Name.ToLower() <> "infiltrator" Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The Mist prevented the status change!"))
                        Return False
                    End If
                End If
            End If

            If p.Ability.Name.ToLower() = "contrary" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                If cause <> "contrary" Then
                    Return RaiseStat(own, own, BattleScreen, Stat, val, message & vbNewLine & "Contrary reverted the stat change!", "contrary")
                End If
            End If

            If p.Ability.Name.ToLower() = "simple" Then
                If BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                    val *= 2
                End If
            End If

            If p.Ability.Name.ToLower() = "clear body" Or p.Ability.Name.ToLower() = "white smoke" Then
                If BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                    If own <> from Then
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The " & p.Ability.Name & " prevented the status change!"))
                        Return False
                    End If
                End If
            End If
            
            
            
            Dim statString As String = Stat.ToLower()
            
             Select Case statString
                Case "attack"
                    If p.Ability.Name.ToLower() = "hyper cutter" And from <> own Then
                        If BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                            Me.ChangeCameraAngel(1, own, BattleScreen)
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Hyper Cutter prevented attack drop!"))
                            Return False
                        End If
                    End If
                  
                Case "defense"
                    If p.Ability.Name.ToLower() = "big pecks" And from <> own And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        BattleScreen.BattleQuery.Add(New TextQueryObject("Big Pecks prevented defense drop!"))
                        Return False
                    End If
               
                Case "accuracy"
                    If p.Ability.Name.ToLower() = "keen eye" And from <> own Then
                        If BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                            Me.ChangeCameraAngel(1, own, BattleScreen)
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Keen Eye prevented accuracy drop!"))
                            Return False
                        End If
                    End If
                End Select    
            
            Select Case statString
                Case "spdefense"
                    statString = "special defense"
                Case "spattack"
                    statString = "special attack"
            End Select

            Dim statC As Integer = 0
            Select Case Stat.ToLower()
                Case "attack"
                    statC = p.StatAttack
                Case "defense"
                    statC = p.StatDefense
                Case "special attack"
                    statC = p.StatSpAttack
                Case "special defense"
                    statC = p.StatSpDefense
                Case "speed"
                    statC = p.StatSpeed
                Case "evasion"
                    statC = p.Evasion
                Case "accuracy"
                    statC = p.Accuracy
            End Select
            If statC <= -6 Then
                'Cannot fall further

                Me.ChangeCameraAngel(1, own, BattleScreen)

                Select Case message
                    Case "" 'Print default message only
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s " & statString & " cannot fall further."))
                    Case "-1" 'Print no message at all
                        'Do nothing
                    Case Else 'Print message given in 'message'
                        BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s " & statString & " cannot fall further."))
                End Select

                Return False
            Else
                If statC - val < -6 Then
                    val = 6 + statC
                End If
            End If

            '***SHOW STAT DECREASE ANIMATION HERE***

            Dim printMessage As String = p.GetDisplayName() & "'s " & statString
            Select Case val
                Case 2
                    printMessage &= " sharply fell!"
                Case 3
                    printMessage &= " fell drastically!"
                Case Else
                    printMessage &= " slightly fell."
            End Select



            Select Case statString
                Case "attack"
                    p.StatAttack -= val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "defense"
                    p.StatDefense -= val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "special attack"
                    p.StatSpAttack -= val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "special defense"
                    p.StatSpDefense -= val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "speed"
                    p.StatSpeed -= val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "evasion"
                    p.Evasion -= val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select

                    Return True
                Case "accuracy"
                    p.Accuracy -= val

                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(printMessage))
                    End Select
                    
                If val > 0 Then
                    If p.Ability.Name.ToLower() = "defiant" And from <> own Then
                        RaiseStat(own, own, BattleScreen, "Attack", 2, p.GetDisplayName() & "'s Defiant raised its attack!", "defiant")
                    End If

                    If p.Ability.Name.ToLower() = "competitive" And from <> own Then
                        RaiseStat(own, own, BattleScreen, "Special Attack", 2, p.GetDisplayName() & "'s Competitive raised its Special Attack!", "competitive")
                    End If
                 End If
                    
                    Return True
            End Select

            Logger.Log(Logger.LogTypes.Warning, "BattleV2.vb: Failed to indicate stat change: " & Stat.ToUpper() & "!")
            Return True
        End Function

        Public Function InflictInfatuate(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            If p.HasVolatileStatus(Pokemon.VolatileStatus.Infatuation) = True Then
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is already infatuated."))
                Return False
            End If

            If p.Ability.Name.ToLower() = "oblivious" And BattleScreen.FieldEffects.CanUseAbility(own, BattleScreen) = True Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject("Oblivious prevented the infatuation."))
                Return False
            Else
                Me.ChangeCameraAngel(1, own, BattleScreen)
                Select Case message
                    Case "" 'Print default message only
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " fell in love."))
                    Case "-1" 'Print no message at all
                        'Do nothing
                    Case Else 'Print message given in 'message'
                        BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " fell in love."))
                End Select

                p.AddVolatileStatus(Pokemon.VolatileStatus.Infatuation)

                If Not p.Item Is Nothing Then
                    If p.Item.Name.ToLower() = "destiny knot" And from <> own And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                        Me.InflictInfatuate(Not own, Not own, BattleScreen, "Destiny Knot reflects the infatuation.", "destinyknot")
                    End If
                End If

                Return True
            End If
        End Function

        Public Sub InflictRecoil(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal MoveUsed As Attack, ByVal Damage As Integer, ByVal message As String, ByVal cause As String)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Exit Sub
            End If

            If p.Ability.Name.ToLower() = "rock head" And cause.StartsWith("move:") = True Then
                Me.ChangeCameraAngel(1, own, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject("Rock Head prevented the recoil damage of " & p.GetDisplayName() & "!"))
            Else
                If p.Ability.Name.ToLower() = "magic guard" Then
                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Magic Guard prevented the recoil damage of " & p.GetDisplayName() & "!"))
                Else
                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is damaged by recoil"))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is damaged by recoil"))
                    End Select
                    ReduceHP(Damage, own, from, BattleScreen, "", "recoildamage")
                End If
            End If
        End Sub

        Public Sub GainHP(ByVal HPAmount As Integer, ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.HP < p.MaxHP And p.HP > 0 And p.Status <> Pokemon.StatusProblems.Fainted Then
                If own = True Then
                    ChangeCameraAngel(1, True, BattleScreen)
                Else
                    ChangeCameraAngel(2, True, BattleScreen)
                End If

                If HPAmount > p.MaxHP - p.HP Then
                    HPAmount = p.MaxHP - p.HP
                End If

                If own = True Then
                    BattleScreen.BattleQuery.Add(New MathHPQueryObject(p.HP, p.MaxHP, -HPAmount, New Vector2(200, 256)))
                Else
                    BattleScreen.BattleQuery.Add(New MathHPQueryObject(p.HP, p.MaxHP, -HPAmount, New Vector2(300, 256)))
                End If

                If message <> "" Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                End If

                p.HP += HPAmount
                p.HP = p.HP.Clamp(0, p.MaxHP)
            End If
        End Sub

        Public Sub ReduceHP(ByVal HPAmount As Integer, ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String)
            Me.ReduceHP(HPAmount, own, from, BattleScreen, message, cause, "")
        End Sub

        Public Sub ReduceHP(ByVal HPAmount As Integer, ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String, ByVal sound As String)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.HP > 0 And p.Status <> Pokemon.StatusProblems.Fainted Then
                If own = True Then
                    ChangeCameraAngel(1, True, BattleScreen)
                Else
                    ChangeCameraAngel(2, True, BattleScreen)
                End If

                If sound <> "NOSOUND" Then
                    If sound = "" Then
                        sound = "Battle\Damage\normaldamage"
                    End If
                    BattleScreen.BattleQuery.Add(New PlaySoundQueryObject(sound, False, 0.0F))
                End If

                If own = True Then
                    BattleScreen.BattleQuery.Add(New MathHPQueryObject(p.HP, p.MaxHP, HPAmount, New Vector2(200, 256)))
                Else
                    BattleScreen.BattleQuery.Add(New MathHPQueryObject(p.HP, p.MaxHP, HPAmount, New Vector2(300, 256)))
                End If

                If message <> "" Then
                    BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                End If

                p.HP -= HPAmount
                p.HP = p.HP.Clamp(0, p.MaxHP)

                Dim ItemID As Integer = -1
                If Not p.Item Is Nothing Then
                    ItemID = p.Item.ID
                End If

                Dim lastMove As Attack = Nothing
                If own = True Then
                    lastMove = BattleScreen.FieldEffects.OppLastMove
                Else
                    lastMove = BattleScreen.FieldEffects.OwnLastMove
                End If
                If Not lastMove Is Nothing Then
                    Dim effectiveness As Single = BattleCalculation.CalculateEffectiveness(Not own, lastMove, BattleScreen)
                    If effectiveness > 1.0F Then
                        If Not p.Item Is Nothing Then
                            If p.Item.Name.ToLower() = "enigma" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                                If RemoveHeldItem(own, own, BattleScreen, "", "berry:enigma") = True Then
                                    UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                End If
                            End If
                        End If
                    End If
                End If

                If p.HP > 0 And p.HP < CInt(Math.Ceiling(p.MaxHP / 3)) Then
                    If Not p.Item Is Nothing Then
                        If p.Item.Name.ToLower() = "oran" And BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                            If RemoveHeldItem(own, own, BattleScreen, "", "berry:oran") = True Then
                                UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                            End If
                        End If
                    End If
                End If
                If p.HP > 0 And p.HP < CInt(Math.Ceiling(p.MaxHP / 2)) Then
                    If Not p.Item Is Nothing Then
                        If BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                            Select Case p.Item.Name.ToLower()
                                Case "sitrus"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:sitrus") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "figy"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:figy") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "wiki"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:wiki") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "mago"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:mago") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "aguav"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:aguav") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "iapapa"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:iapapa") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                            End Select
                        End If
                    End If
                End If
                If p.HP > 0 And p.HP < CInt(Math.Ceiling(p.MaxHP / 4)) Then
                    If Not p.Item Is Nothing Then
                        If BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                            Select Case p.Item.Name.ToLower()
                                Case "liechi"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:liechi") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "ganlon"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:ganlon") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "salac"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:salac") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "petaya"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:petaya") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "apicot"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:apicot") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "lansat"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:lansat") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "starf"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:starf") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "micle"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:micle") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                                Case "custap"
                                    If RemoveHeldItem(own, own, BattleScreen, "", "berry:custap") = True Then
                                        UseBerry(own, from, Item.GetItemByID(ItemID), BattleScreen, message, cause)
                                    End If
                            End Select
                        End If
                    End If
                End If
            End If
        End Sub

        Public Sub UseBerry(ByVal own As Boolean, ByVal from As Boolean, ByVal BerryItem As Item, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            Dim berry As Items.Berry = CType(BerryItem, Items.Berry)

            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("single_heal", False))
            Select Case BerryItem.Name.ToLower()
                Case "oran"
                    GainHP(10, own, from, BattleScreen, "The Oran Berry filled up " & p.GetDisplayName() & "'s HP!", "berry:oran")
                Case "sitrus"
                    GainHP(CInt(p.MaxHP / 4), own, own, BattleScreen, "The Sitrus Berry filled up " & p.GetDisplayName() & "'s HP!", "berry:sitrus")
                Case "figy"
                    Dim healHP As Integer = CInt(Math.Ceiling(p.MaxHP / 8))
                    GainHP(healHP, own, own, BattleScreen, "The Figy Berry filled up " & p.GetDisplayName() & "'s HP!", "berry:figy")
                    If berry.PokemonLikes(p) = False Then
                        InflictConfusion(own, own, BattleScreen, p.GetDisplayName() & " disliked the Figy Berry!", "berry:figy")
                    End If
                Case "wiki"
                    Dim healHP As Integer = CInt(Math.Ceiling(p.MaxHP / 8))
                    GainHP(healHP, own, own, BattleScreen, "The Wiki Berry filled up " & p.GetDisplayName() & "'s HP!", "berry:wiki")
                    If berry.PokemonLikes(p) = False Then
                        InflictConfusion(own, own, BattleScreen, p.GetDisplayName() & " disliked the Wiki Berry!", "berry:wiki")
                    End If
                Case "mago"
                    Dim healHP As Integer = CInt(Math.Ceiling(p.MaxHP / 8))
                    GainHP(healHP, own, own, BattleScreen, "The Mago Berry filled up " & p.GetDisplayName() & "'s HP!", "berry:mago")
                    If berry.PokemonLikes(p) = False Then
                        InflictConfusion(own, own, BattleScreen, p.GetDisplayName() & " disliked the Mago Berry!", "mago")
                    End If
                Case "aguav"
                    Dim healHP As Integer = CInt(Math.Ceiling(p.MaxHP / 8))
                    GainHP(healHP, own, own, BattleScreen, "The Aguav Berry filled up " & p.GetDisplayName() & "'s HP!", "berry:aguav")
                    If berry.PokemonLikes(p) = False Then
                        InflictConfusion(own, own, BattleScreen, p.GetDisplayName() & " disliked the Aguav Berry!", "aguav")
                    End If
                Case "iapapa"
                    Dim healHP As Integer = CInt(Math.Ceiling(p.MaxHP / 8))
                    GainHP(healHP, own, own, BattleScreen, "The Iapapa Berry filled up " & p.GetDisplayName() & "'s HP!", "berry:iapapa")
                    If berry.PokemonLikes(p) = False Then
                        InflictConfusion(own, own, BattleScreen, p.GetDisplayName() & " disliked the Iapapa Berry!", "berry:iapapa")
                    End If
                Case "liechi"
                    RaiseStat(own, own, BattleScreen, "Attack", 2, "The Liechi Berry raised " & p.GetDisplayName() & "'s power!", "berry:liechi")
                Case "ganlon"
                    RaiseStat(own, own, BattleScreen, "Defense", 2, "The Ganlon Berry raised " & p.GetDisplayName() & "'s power!", "berry:ganlon")
                Case "salac"
                    RaiseStat(own, own, BattleScreen, "Speed", 2, "The Salac Berry raised " & p.GetDisplayName() & "'s power!", "berry:salac")
                Case "petaya"
                    RaiseStat(own, own, BattleScreen, "Special Attack", 2, "The Petaya Berry raised " & p.GetDisplayName() & "'s power!", "berry:petaya")
                Case "apicot"
                    RaiseStat(own, own, BattleScreen, "Special Defense", 2, "The Apicot Berry raised " & p.GetDisplayName() & "'s power!", "berry:apicot")
                Case "lansat"
                    If own = True Then
                        BattleScreen.FieldEffects.OwnLansatBerry = 1
                    Else
                        BattleScreen.FieldEffects.OppLansatBerry = 1
                    End If
                    BattleScreen.BattleQuery.Add(New TextQueryObject("The Lansat Berry raised " & p.GetDisplayName() & "'s power!"))
                Case "starf"
                    Dim stat As String = "Attack"
                    Select Case Core.Random.Next(0, 7)
                        Case 0
                            stat = "Attack"
                        Case 1
                            stat = "Defense"
                        Case 2
                            stat = "Special Attack"
                        Case 3
                            stat = "Special Defense"
                        Case 4
                            stat = "Speed"
                        Case 5
                            stat = "Accuracy"
                        Case 6
                            stat = "Evasion"
                    End Select
                    RaiseStat(own, own, BattleScreen, stat, 2, "The Starf Berry raised " & p.GetDisplayName() & "'s power!", "berry:starf")
                Case "micle"
                    RaiseStat(own, own, BattleScreen, "Accuracy", 2, "The Micle Berry raised " & p.GetDisplayName() & "'s power!", "berry:micle")
                Case "custap"
                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    If own = True Then
                        BattleScreen.FieldEffects.OwnCustapBerry = 1
                    Else
                        BattleScreen.FieldEffects.OppCustapBerry = 1
                    End If
                    BattleScreen.BattleQuery.Add(New TextQueryObject("The Custap Berry gave " & p.GetDisplayName() & " a speed boost!"))
                Case "enigma"
                    GainHP(CInt(p.MaxHP / 4), own, own, BattleScreen, "The Enigma Berry filled up " & p.GetDisplayName() & "'s HP!", "berry:enigma")
            End Select

            If p.Ability.Name.ToLower() = "cheek pouch" Then
                GainHP(CInt(p.MaxHP / 8), own, own, BattleScreen, "Cheek Pouch healed some HP.", "cheekpouch")
            End If
        End Sub

        Public Function RemoveHeldItem(ByVal own As Boolean, ByVal from As Boolean, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String, Optional ByVal TestFor As Boolean = False) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If p.Item Is Nothing Then
                Return False
            End If

            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If

            If p.Ability.Name.ToLower() = "sticky hold" And cause.StartsWith("berry:") = False Then
                If TestFor = False Then
                    Me.ChangeCameraAngel(1, own, BattleScreen)
                    BattleScreen.BattleQuery.Add(New TextQueryObject("Sticky Hold prevented the item loss."))
                End If
                Return False
            End If

            If TestFor = False Then
                Dim ItemID As Integer = p.Item.ID
                Dim lostItem As Item = Item.GetItemByID(ItemID)

                If own = True Then
                    BattleScreen.FieldEffects.OwnLostItem = lostItem
                Else
                    BattleScreen.FieldEffects.OppLostItem = lostItem
                End If

                p.Item = Nothing

                If p.Ability.Name.ToLower() = "unburden" Then
                    RaiseStat(own, own, BattleScreen, "Speed", 2, "Unburden raised the speed!", "unburden")
                End If

                Me.ChangeCameraAngel(1, own, BattleScreen)
                Select Case message
                    Case "" 'Print default message only
                        If cause.StartsWith("berry:") = True Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " ate the " & lostItem.Name & "berry!"))
                        Else
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " lost the item " & lostItem.Name & "!"))
                        End If
                    Case "-1" 'Print no message at all
                        'Do nothing
                    Case Else 'Print message given in 'message'
                        BattleScreen.BattleQuery.Add(New TextQueryObject(message))

                        If cause.StartsWith("berry:") = True Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " ate the " & lostItem.Name & "berry!"))
                        Else
                            BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " lost the item " & lostItem.Name & "!"))
                        End If
                End Select
            End If

            Return True
        End Function

        Public Sub ChangeWeather(ByVal own As Boolean, ByVal from As Boolean, ByVal newWeather As BattleWeather.WeatherTypes, ByVal turns As Integer, ByVal BattleScreen As BattleScreen, ByVal message As String, ByVal cause As String)
            If BattleScreen.FieldEffects.Weather <> newWeather Then
                If newWeather <> BattleWeather.WeatherTypes.Clear Then
                    Dim weatherRounds As Integer = turns
                    If weatherRounds = -1 Then
                        weatherRounds = 5
                    End If

                    Dim p As Pokemon = BattleScreen.OwnPokemon
                    Dim op As Pokemon = BattleScreen.OppPokemon
                    If own = False Then
                        p = BattleScreen.OppPokemon
                        op = BattleScreen.OwnPokemon
                    End If

                    If op.Ability.Name.ToLower() = "air lock" Or op.Ability.Name.ToLower() = "cloud nine" Then
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        Select Case message
                            Case "" 'Print default message only
                                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s " & op.Ability.Name & " prevented the weather change!"))
                            Case "-1" 'Print no message at all
                                'Do nothing
                            Case Else 'Print message given in 'message'
                                BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                                BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & "'s " & op.Ability.Name & " prevented the weather change!"))
                        End Select

                        ApplyForecast(BattleScreen)

                        Exit Sub
                    End If

                    If p.Ability.Name.ToLower() = "air lock" Or p.Ability.Name.ToLower() = "cloud nine" Then
                        Me.ChangeCameraAngel(1, own, BattleScreen)
                        Select Case message
                            Case "" 'Print default message only
                                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s " & p.Ability.Name & " prevented the weather change!"))
                            Case "-1" 'Print no message at all
                                'Do nothing
                            Case Else 'Print message given in 'message'
                                BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s " & p.Ability.Name & " prevented the weather change!"))
                        End Select

                        ApplyForecast(BattleScreen)

                        Exit Sub
                    End If

                    If Not p.Item Is Nothing Then
                        If BattleScreen.FieldEffects.CanUseItem(own) = True And BattleScreen.FieldEffects.CanUseOwnItem(own, BattleScreen) = True Then
                            Select Case p.Item.Name.ToLower()
                                Case "damp rock"
                                    If newWeather = BattleWeather.WeatherTypes.Rain Then
                                        weatherRounds += 3
                                    End If
                                Case "heat rock"
                                    If newWeather = BattleWeather.WeatherTypes.Sunny Then
                                        weatherRounds += 3
                                    End If
                                Case "icy rock"
                                    If newWeather = BattleWeather.WeatherTypes.Hailstorm Then
                                        weatherRounds += 3
                                    End If
                                Case "smooth rock"
                                    If newWeather = BattleWeather.WeatherTypes.Sandstorm Then
                                        weatherRounds += 3
                                    End If
                            End Select
                        End If
                    End If

                    BattleScreen.FieldEffects.Weather = newWeather
                    BattleScreen.FieldEffects.WeatherRounds = weatherRounds

                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject("The weather changed to " & newWeather.ToString() & "!"))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject("The weather changed to " & newWeather.ToString() & "!"))
                    End Select

                    ApplyForecast(BattleScreen)
                Else
                    Select Case message
                        Case "" 'Print default message only
                            BattleScreen.BattleQuery.Add(New TextQueryObject("The effects of weather disappeared."))
                        Case "-1" 'Print no message at all
                            'Do nothing
                        Case Else 'Print message given in 'message'
                            BattleScreen.BattleQuery.Add(New TextQueryObject(message))
                            BattleScreen.BattleQuery.Add(New TextQueryObject("The effects of weather disappeared."))
                    End Select

                    ApplyForecast(BattleScreen)
                End If
            End If
        End Sub

        Private Sub ApplyForecast(ByVal BattleScreen As BattleScreen)
            With BattleScreen
                Dim p As Pokemon = .OwnPokemon

                If p.Ability.Name.ToLower() = "forecast" Then
                    Select Case .FieldEffects.Weather
                        Case BattleWeather.WeatherTypes.Rain
                            p.OriginalType1 = p.Type1
                            p.OriginalType2 = p.Type2

                            p.Type1 = New Element(Element.Types.Water)
                            p.Type2 = New Element(Element.Types.Blank)

                            .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " transformed into a water type!"))
                        Case BattleWeather.WeatherTypes.Sunny
                            p.OriginalType1 = p.Type1
                            p.OriginalType2 = p.Type2

                            p.Type1 = New Element(Element.Types.Fire)
                            p.Type2 = New Element(Element.Types.Blank)

                            .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " transformed into a fire type!"))
                        Case BattleWeather.WeatherTypes.Hailstorm
                            p.OriginalType1 = p.Type1
                            p.OriginalType2 = p.Type2

                            p.Type1 = New Element(Element.Types.Ice)
                            p.Type2 = New Element(Element.Types.Blank)

                            .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " transformed into an ice type!"))
                    End Select
                End If

                p = .OppPokemon

                If p.Ability.Name.ToLower() = "forecast" Then
                    Select Case .FieldEffects.Weather
                        Case BattleWeather.WeatherTypes.Rain
                            p.OriginalType1 = p.Type1
                            p.OriginalType2 = p.Type2

                            p.Type1 = New Element(Element.Types.Water)
                            p.Type2 = New Element(Element.Types.Blank)

                            .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " transformed into a water type!"))
                        Case BattleWeather.WeatherTypes.Sunny
                            p.OriginalType1 = p.Type1
                            p.OriginalType2 = p.Type2

                            p.Type1 = New Element(Element.Types.Fire)
                            p.Type2 = New Element(Element.Types.Blank)

                            .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " transformed into a fire type!"))
                        Case BattleWeather.WeatherTypes.Hailstorm
                            p.OriginalType1 = p.Type1
                            p.OriginalType2 = p.Type2

                            p.Type1 = New Element(Element.Types.Ice)
                            p.Type2 = New Element(Element.Types.Blank)

                            .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " transformed into an ice type!"))
                    End Select
                End If
            End With
        End Sub

#End Region

        ''' <summary>
        ''' Switches camera to angel
        ''' </summary>
        ''' <param name="direction">0=main battle/1=own pokemon/2=opp pokemon</param>
        ''' <param name="own">If the code comes from the own player or not.</param>
        ''' <param name="BattleScreen">Battlescreen reference</param>
        ''' <param name="AddPVP">If the call should get added the PVP list or the own queue.</param>
        Public Sub ChangeCameraAngel(ByVal direction As Integer, ByVal own As Boolean, ByVal BattleScreen As BattleScreen, Optional ByVal AddPVP As Boolean = False)
            Dim q As CameraQueryObject = Nothing

            Select Case direction
                Case 0
                    q = CType(BattleScreen.FocusBattle(), CameraQueryObject)
                Case 1
                    If own = True Then
                        q = CType(BattleScreen.FocusOwnPokemon(), CameraQueryObject)
                    Else
                        q = CType(BattleScreen.FocusOppPokemon(), CameraQueryObject)
                    End If
                Case 2
                    If own = False Then
                        q = CType(BattleScreen.FocusOwnPokemon(), CameraQueryObject)
                    Else
                        q = CType(BattleScreen.FocusOppPokemon(), CameraQueryObject)
                    End If
            End Select
            If Not q Is Nothing Then
                q.ApplyCurrentCamera = True
                q.ReplacePVP = True

                If AddPVP = True Then
                    BattleScreen.TempPVPBattleQuery.Add(BattleScreen.BattleQuery.Count - 1, q)
                Else
                    BattleScreen.BattleQuery.Add(q)
                End If
            End If
            If BattleScreen.IsRemoteBattle = True And AddPVP = False Then
                Select Case direction
                    Case 0
                        ChangeCameraAngel(0, own, BattleScreen, True)
                    Case 1
                        ChangeCameraAngel(2, own, BattleScreen, True)
                    Case 2
                        ChangeCameraAngel(1, own, BattleScreen, True)
                End Select
            End If
        End Sub

#Region "RoundEnd"

        ''' <summary>
        ''' Ends a round (or a complete round)
        ''' </summary>
        ''' <param name="BattleScreen">Battlescreen</param>
        ''' <param name="type">0=complete;1=own;2=opp</param>
        Public Sub EndRound(ByVal BattleScreen As BattleScreen, ByVal type As Integer)
            With BattleScreen
                Select Case type
                    Case 0 'Complete round
                        'The fastest pokemon ends its round first
                        If BattleCalculation.MovesFirst(BattleScreen) = True Then
                            EndRoundOwn(BattleScreen)
                            EndRoundOpp(BattleScreen)
                        Else
                            EndRoundOpp(BattleScreen)
                            EndRoundOwn(BattleScreen)
                        End If
                        .FieldEffects.Rounds += 1
                        If .FieldEffects.WeatherRounds > 0 Then
                            .FieldEffects.WeatherRounds -= 1
                            If .FieldEffects.WeatherRounds = 0 Then
                                .FieldEffects.Weather = BattleWeather.WeatherTypes.Clear
                                .BattleQuery.Add(New TextQueryObject("The weather became clear again!"))
                            End If
                        End If
                        If .FieldEffects.TrickRoom > 0 Then
                            .FieldEffects.TrickRoom -= 1
                            If .FieldEffects.TrickRoom = 0 Then
                                .BattleQuery.Add(New TextQueryObject("The dimensions have returned to normal."))
                            End If
                        End If
                        If .FieldEffects.Gravity > 0 Then
                            .FieldEffects.Gravity -= 1
                            If .FieldEffects.Gravity = 0 Then
                                .BattleQuery.Add(New TextQueryObject("Gravity became normal again!"))
                            End If
                        End If

                        'Water Sport
                        If .FieldEffects.WaterSport > 0 Then
                            .FieldEffects.WaterSport -= 1
                            If .FieldEffects.WaterSport = 0 Then
                                .BattleQuery.Add(New TextQueryObject("Water Sport's effect ended."))
                            End If
                        End If

                        'Mud Sport
                        If .FieldEffects.MudSport > 0 Then
                            .FieldEffects.MudSport -= 1
                            If .FieldEffects.MudSport = 0 Then
                                .BattleQuery.Add(New TextQueryObject("Mud Sport's effect ended."))
                            End If
                        End If

                        'Remove flinch:
                        If .OwnPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Flinch) = True Then
                            .OwnPokemon.RemoveVolatileStatus(Pokemon.VolatileStatus.Flinch)
                        End If
                        If .OppPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Flinch) = True Then
                            .OppPokemon.RemoveVolatileStatus(Pokemon.VolatileStatus.Flinch)
                        End If

                        'Revert roost types:
                        If .FieldEffects.OwnRoostUsed = True Then
                            .OwnPokemon.Type1 = .OwnPokemon.OriginalType1
                            .OwnPokemon.Type2 = .OwnPokemon.OriginalType2

                            .FieldEffects.OwnRoostUsed = False
                        End If
                        If .FieldEffects.OppRoostUsed = True Then
                            .OppPokemon.Type1 = .OppPokemon.OriginalType1
                            .OppPokemon.Type2 = .OppPokemon.OriginalType2

                            .FieldEffects.OppRoostUsed = False
                        End If

                        'Clear learned attack counter:
                        LearnMovesQueryObject.ClearCache()

                        'Remove Magical Coat:
                        .FieldEffects.OwnMagicCoat = 0
                        .FieldEffects.OppMagicCoat = 0

                        'Reset Detect/Protect
                        .FieldEffects.OwnDetectCounter = 0 'Reset protect and detect
                        .FieldEffects.OwnProtectCounter = 0
                        .FieldEffects.OwnKingsShieldCounter = 0

                        If .FieldEffects.OwnEndure > 0 Then 'Stop endure
                            .FieldEffects.OwnEndure = 0
                        End If

                        .FieldEffects.OppDetectCounter = 0 'Reset protect and detect
                        .FieldEffects.OppProtectCounter = 0
                        .FieldEffects.OppKingsShieldCounter = 0

                        If .FieldEffects.OppEndure > 0 Then 'Stop endure
                            .FieldEffects.OppEndure = 0
                        End If

                        If .FieldEffects.OwnProtectMovesCount > 0 AndAlso Not .FieldEffects.OwnLastMove Is Nothing AndAlso .FieldEffects.OwnLastMove.IsProtectMove = False Then
                            .FieldEffects.OwnProtectMovesCount = 0
                        End If
                        If .FieldEffects.OppProtectMovesCount > 0 AndAlso Not .FieldEffects.OppLastMove Is Nothing AndAlso .FieldEffects.OppLastMove.IsProtectMove = False Then
                            .FieldEffects.OppProtectMovesCount = 0
                        End If

                        If .OwnPokemon.Status = Pokemon.StatusProblems.Fainted Or .OwnPokemon.HP <= 0 Then
                            .OwnPokemon.Status = Pokemon.StatusProblems.Fainted
                            SwitchOutOwn(BattleScreen, -1, -1)
                        End If
                        If .OppPokemon.Status = Pokemon.StatusProblems.Fainted Or .OppPokemon.HP <= 0 Then
                            .OppPokemon.Status = Pokemon.StatusProblems.Fainted
                            If BattleScreen.IsTrainerBattle = True Then
                                If BattleScreen.Trainer.HasBattlePokemon() = True Then
                                    BattleScreen.FieldEffects.DefeatedTrainerPokemon = True
                                End If
                            End If
                            SwitchOutOpp(BattleScreen, -1)
                        End If

                        ChangeCameraAngel(0, True, BattleScreen)

                        Dim cq1 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, True, 16)
                        Dim cq2 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 16)

                        cq2.PassThis = True

                        BattleScreen.BattleQuery.AddRange({cq1, cq2})

                        StartRound(BattleScreen)
                        BattleScreen.ClearMenuTime = True
                    Case 1 'Own round
                        EndTurnOwn(BattleScreen)
                    Case 2 'Opp round
                        EndTurnOpp(BattleScreen)
                End Select
            End With
        End Sub

        Private Function PlayerWonBattle(ByVal BattleScreen As BattleScreen) As Boolean
            If BattleScreen.BattleMode = BattleScreen.BattleModes.Safari Then
                Return False
            End If
            If BattleScreen.IsTrainerBattle = True Then
                Return Not BattleScreen.TrainerHasFightablePokemon()
            Else
                Return BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Fainted Or BattleScreen.OppPokemon.HP <= 0
            End If
        End Function

        Private Sub EndTurnOwn(ByVal BattleScreen As BattleScreen)
            With BattleScreen
                'Turn count
                .FieldEffects.OwnPokemonTurns += 1

                .FieldEffects.OwnLockOn = 0 'Reset lock-on

                'Remove temp pursuit counter:
                .FieldEffects.OwnPursuit = False

                If .FieldEffects.OwnSleepTurns > 0 Then 'Sleep turns
                    .FieldEffects.OwnSleepTurns -= 1
                End If

                If .FieldEffects.OwnCharge > 0 Then
                    .FieldEffects.OwnCharge -= 1 'Sets charge to 0
                End If
                If .OwnPokemon.HP > 0 Then
                    If Not .OwnPokemon.Item Is Nothing Then
                        If .OwnPokemon.Item.Name.ToLower() = "mental herb" Then
                            Dim usedMentalHerb As Boolean = False
                            If .OwnPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Infatuation) = True Then
                                .OwnPokemon.RemoveVolatileStatus(Pokemon.VolatileStatus.Infatuation)
                                .BattleQuery.Add(New TextQueryObject(.OwnPokemon.GetDisplayName() & " got healed from the infatuation" & vbNewLine & "due to Mental Herb!"))
                                usedMentalHerb = True
                            End If
                            If .FieldEffects.OwnTaunt > 0 Then
                                .FieldEffects.OwnTaunt = 0
                                .BattleQuery.Add(New TextQueryObject(.OwnPokemon.GetDisplayName() & " got healed from the taunt" & vbNewLine & "due to Mental Herb!"))
                                usedMentalHerb = True
                            End If
                            If .FieldEffects.OwnEncore > 0 Then
                                .FieldEffects.OwnEncore = 0
                                .BattleQuery.Add(New TextQueryObject(.OwnPokemon.GetDisplayName() & " got healed from the encore" & vbNewLine & "due to Mental Herb!"))
                                usedMentalHerb = True
                            End If
                            If .FieldEffects.OwnTorment > 0 Then
                                .FieldEffects.OwnTorment = 0
                                .BattleQuery.Add(New TextQueryObject(.OwnPokemon.GetDisplayName() & " got healed from the torment" & vbNewLine & "due to Mental Herb!"))
                                usedMentalHerb = True
                            End If
                            'Remove disable
                            If usedMentalHerb = True Then
                                .OwnPokemon.Item = Nothing
                            End If
                        End If
                        If .OwnPokemon.Item.Name.ToLower() = "white herb" Then
                            Dim hasNegativeStats As Boolean = False
                            With .OwnPokemon
                                If .StatAttack < 0 Then
                                    .StatAttack = 0
                                    hasNegativeStats = True
                                End If
                                If .StatDefense < 0 Then
                                    .StatDefense = 0
                                    hasNegativeStats = True
                                End If
                                If .StatSpAttack < 0 Then
                                    .StatSpAttack = 0
                                    hasNegativeStats = True
                                End If
                                If .StatSpDefense < 0 Then
                                    .StatSpDefense = 0
                                    hasNegativeStats = True
                                End If
                                If .StatSpeed < 0 Then
                                    .StatSpeed = 0
                                    hasNegativeStats = True
                                End If
                                If .Accuracy < 0 Then
                                    .Accuracy = 0
                                    hasNegativeStats = True
                                End If
                                If .Evasion < 0 Then
                                    .Evasion = 0
                                    hasNegativeStats = True
                                End If
                            End With
                            If hasNegativeStats = True Then
                                .BattleQuery.Add(New TextQueryObject(.OwnPokemon.GetDisplayName() & " negative stats got healed due to White Herb!"))
                                .OwnPokemon.Item = Nothing
                            End If
                        End If
                    End If
                End If

                .FieldEffects.OwnPokemonDamagedLastTurn = .FieldEffects.OwnPokemonDamagedThisTurn
                .FieldEffects.OwnPokemonDamagedThisTurn = False
            End With
        End Sub


        Private Sub EndRoundOwn(ByVal BattleScreen As BattleScreen)
            If Me.PlayerWonBattle(BattleScreen) = True Then
                Exit Sub
            End If
            ChangeCameraAngel(0, True, BattleScreen)

            With BattleScreen
                If .FieldEffects.OwnReflect > 0 Then 'Stop reflect
                    .FieldEffects.OwnReflect -= 1
                    If .FieldEffects.OwnReflect = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Own Reflect effect faded."))
                    End If
                End If

                If .FieldEffects.OwnLightScreen > 0 Then 'Stop light screen
                    .FieldEffects.OwnLightScreen -= 1
                    If .FieldEffects.OwnLightScreen = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Own Light Screen effect faded."))
                    End If
                End If

                If .FieldEffects.OwnMist > 0 Then 'Stop mist
                    .FieldEffects.OwnMist -= 1
                    If .FieldEffects.OwnMist = 0 Then
                        .BattleQuery.Add(New TextQueryObject("The mist on your side of the field faded!"))
                    End If
                End If

                If .FieldEffects.OwnSafeguard > 0 Then 'Stop safeguard
                    .FieldEffects.OwnSafeguard -= 1
                    If .FieldEffects.OwnSafeguard = 0 Then
                        .BattleQuery.Add(New TextQueryObject("The Safeguard effect wore off!"))
                    End If
                End If

                If .FieldEffects.OwnGuardSpec > 0 Then 'Stop guard spec
                    .FieldEffects.OwnGuardSpec -= 1
                    If .FieldEffects.OwnGuardSpec = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Own Guard Spec. wore off."))
                    End If
                End If

                If .FieldEffects.OwnTailWind > 0 Then 'Stop tail wind
                    .FieldEffects.OwnTailWind -= 1
                    If .FieldEffects.OwnTailWind = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Own Tail Wind effect faded."))
                    End If
                End If

                If .FieldEffects.OwnLuckyChant > 0 Then 'Stop lucky chant
                    .FieldEffects.OwnLuckyChant -= 1
                    If .FieldEffects.OwnLuckyChant = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Own Lucky Chant effect faded."))
                    End If
                End If

                If .FieldEffects.OwnWish > 0 Then 'Use wish
                    .FieldEffects.OwnWish -= 1
                    If .FieldEffects.OwnWish = 0 Then
                        If .FieldEffects.OppHealBlock = 0 Then
                            If .OwnPokemon.HP < .OwnPokemon.MaxHP And .OwnPokemon.HP > 0 Then
                                GainHP(CInt( .OwnPokemon.MaxHP / 2), True, True, BattleScreen, "A wish came true!", "wish")
                            End If
                        End If
                    End If
                End If

                'Weather
                'Sandstorm
                If .FieldEffects.Weather = BattleWeather.WeatherTypes.Sandstorm Then
                    If .OwnPokemon.Type1.Type <> Element.Types.Ground And .OwnPokemon.Type2.Type <> Element.Types.Ground And .OwnPokemon.Type1.Type <> Element.Types.Steel And .OwnPokemon.Type2.Type <> Element.Types.Steel And .OwnPokemon.Type1.Type <> Element.Types.Rock And .OwnPokemon.Type2.Type <> Element.Types.Rock Then
                        Dim sandAbilities() As String = {"sand veil", "sand rush", "sand force", "overcoat", "magic guard"}
                        If sandAbilities.Contains( .OwnPokemon.Ability.Name.ToLower()) = False Then
                            If .OwnPokemon.HP > 0 Then
                                Dim sandHP As Integer = CInt( .OwnPokemon.MaxHP / 16)
                                ReduceHP(sandHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " took damage from the sandstorm!", "sandstorm")
                            End If
                        End If
                    End If
                End If

                'Hailstorm
                If .FieldEffects.Weather = BattleWeather.WeatherTypes.Hailstorm Then
                    If .OwnPokemon.Type1.Type <> Element.Types.Ice And .OwnPokemon.Type2.Type <> Element.Types.Ice Then
                        Dim hailAbilities() As String = {"ice body", "snow cloak", "overcoat", "magic guard"}
                        If hailAbilities.Contains( .OwnPokemon.Ability.Name.ToLower()) = False Then
                            If .OwnPokemon.HP > 0 Then
                                Dim hailHP As Integer = CInt( .OwnPokemon.MaxHP / 16)
                                ReduceHP(hailHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " took damage from the hailstorm!", "sandstorm")
                            End If
                        End If
                    End If
                End If

                If .OwnPokemon.HP > 0 Then
                    Dim HPChange As Integer = 0 'Use DrySkin/Rain Dish/Hydration/Ice Body
                    Dim HPMessage As String = ""
                    Select Case .OwnPokemon.Ability.Name.ToLower()
                        Case "dry skin"
                            If .FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                                HPChange = -CInt( .OwnPokemon.MaxHP / 8)
                                HPMessage = "Dry Skin"
                            ElseIf .FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                                HPChange = CInt( .OwnPokemon.MaxHP / 8)
                                HPMessage = "Dry Skin"
                            End If
                        Case "rain dish"
                            If .FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                                HPChange = CInt( .OwnPokemon.MaxHP / 16)
                                HPMessage = "Rain Dish"
                            End If
                        Case "hydration"
                            If .FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                                If .OwnPokemon.Status = Pokemon.StatusProblems.BadPoison Or .OwnPokemon.Status = Pokemon.StatusProblems.Poison Or .OwnPokemon.Status = Pokemon.StatusProblems.Paralyzed Or .OwnPokemon.Status = Pokemon.StatusProblems.Freeze Or .OwnPokemon.Status = Pokemon.StatusProblems.Burn Or .OwnPokemon.Status = Pokemon.StatusProblems.Sleep Then
                                    CureStatusProblem(True, True, BattleScreen, "Hydration cured " & .OwnPokemon.GetDisplayName() & "'s status problem.", "hydration")
                                End If
                            End If
                        Case "ice body"
                            If .FieldEffects.Weather = BattleWeather.WeatherTypes.Hailstorm Then
                                HPChange = CInt( .OwnPokemon.MaxHP / 16)
                                HPMessage = "Ice Body"
                            End If
                    End Select
                    If HPChange > 0 Then
                        If .OwnPokemon.HP < .OwnPokemon.MaxHP Then
                            GainHP(HPChange, True, True, BattleScreen, .OwnPokemon.GetDisplayName() & " restored some HP due to " & HPMessage & ".", HPMessage.Replace(" ", "").ToLower())
                        End If
                    ElseIf HPChange < 0 Then
                        ReduceHP(HPChange, True, True, BattleScreen, .OwnPokemon.GetDisplayName() & " lost some HP due to " & HPMessage & ".", HPMessage.Replace(" ", "").ToLower())
                    End If
                End If

                If .FieldEffects.OwnIngrain > 0 And .OwnPokemon.HP < .OwnPokemon.MaxHP And .OwnPokemon.HP > 0 Then 'Ingrain effect
                    If .FieldEffects.OppHealBlock = 0 Then
                        Dim healHP As Integer = CInt(BattleScreen.OwnPokemon.MaxHP / 16)
                        If Not BattleScreen.OwnPokemon.Item Is Nothing Then
                            If .OwnPokemon.Item.Name.ToLower() = "big root" And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                healHP = CInt(healHP * 1.3F)
                            End If
                        End If
                        GainHP(healHP, True, True, BattleScreen, .OwnPokemon.GetDisplayName() & " gained health from the Ingrain.", "ignrain")
                    End If
                End If

                If .FieldEffects.OwnAquaRing > 0 And .OwnPokemon.HP < .OwnPokemon.MaxHP And .OwnPokemon.HP > 0 Then 'Aqua Ring effect
                    If .FieldEffects.OppHealBlock = 0 Then
                        Dim healHP As Integer = CInt(BattleScreen.OwnPokemon.MaxHP / 16)
                        If Not .OwnPokemon.Item Is Nothing Then
                            If .OwnPokemon.Item.Name.ToLower() = "big root" And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                healHP = CInt(healHP * 1.3F)
                            End If
                        End If
                        GainHP(healHP, True, True, BattleScreen, .OwnPokemon.GetDisplayName() & " gained health from the Aqua Ring.", "aquaring")
                    End If
                End If

                If .OwnPokemon.Ability.Name.ToLower() = "shed skin" And .OwnPokemon.HP > 0 Then 'Shed skin effect
                    If .OwnPokemon.Status = Pokemon.StatusProblems.BadPoison Or .OwnPokemon.Status = Pokemon.StatusProblems.Poison Or .OwnPokemon.Status = Pokemon.StatusProblems.Paralyzed Or .OwnPokemon.Status = Pokemon.StatusProblems.Freeze Or .OwnPokemon.Status = Pokemon.StatusProblems.Burn Or .OwnPokemon.Status = Pokemon.StatusProblems.Sleep Then
                        If Core.Random.Next(0, 100) < 33 Then
                            .BattleQuery.Add( .FocusOwnPokemon())
                            CureStatusProblem(True, True, BattleScreen, .OwnPokemon.GetDisplayName() & "'s Shed Skin cured its status problem.", "shedskin")
                        End If
                    End If
                End If

                If .OwnPokemon.Ability.Name.ToLower() = "speed boost" And .OwnPokemon.HP > 0 Then 'Speed boost/Own first
                    RaiseStat(True, True, BattleScreen, "Speed", 1, .OwnPokemon.GetDisplayName() & "'s Speed Boost raised its speed.", "speedboost")
                End If

                If .OwnPokemon.Ability.Name.ToLower() = "truant" Then 'Set truant round
                    If .FieldEffects.OwnTruantRound = 1 Then
                        .FieldEffects.OwnTruantRound = 0
                    Else
                        .FieldEffects.OwnTruantRound = 1
                    End If
                End If

                If Not .OwnPokemon.Item Is Nothing Then 'Black Sludge
                    If .OwnPokemon.Item.Name.ToLower() = "black sludge" And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                        If .OwnPokemon.Type1.Type = Element.Types.Poison Or .OwnPokemon.Type2.Type = Element.Types.Poison Then
                            If .OwnPokemon.HP < .OwnPokemon.MaxHP And .OwnPokemon.HP > 0 Then
                                GainHP(CInt( .OwnPokemon.MaxHP / 16), True, True, BattleScreen, .OwnPokemon.GetDisplayName() & " gained HP from Black Sludge!", "blacksludge")
                            End If
                        Else
                            If .OwnPokemon.HP > 0 Then
                                ReduceHP(CInt( .OwnPokemon.MaxHP / 8), True, True, BattleScreen, .OwnPokemon.GetDisplayName() & " lost HP due to Black Sludge!", "blacksludge")
                            End If
                        End If
                    End If
                End If

                If .OwnPokemon.HP < .OwnPokemon.MaxHP And .OwnPokemon.HP > 0 Then
                    If .FieldEffects.OppHealBlock = 0 Then
                        If Not .OwnPokemon.Item Is Nothing Then 'Leftovers
                            If .OwnPokemon.Item.Name.ToLower() = "leftovers" And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                GainHP(CInt( .OwnPokemon.MaxHP / 16), True, True, BattleScreen, .OwnPokemon.GetDisplayName() & " restored some HP from Leftovers!", "leftovers")
                            End If
                        End If
                    End If
                End If
                If .FieldEffects.OppLeechSeed > 0 Then 'LeechSeed (opponent seeded)
                    If .OppPokemon.HP > 0 And .OwnPokemon.HP > 0 And .OwnPokemon.HP < .OwnPokemon.MaxHP Then
                        Dim loseHP As Integer = CInt(Math.Ceiling(.OppPokemon.MaxHP / 8))
                        Dim currHP As Integer = .OppPokemon.HP

                        If loseHP > currHP Then
                            loseHP = currHP
                        End If

                        Dim addHP As Integer = loseHP
                        If Not .OwnPokemon.Item Is Nothing Then
                            If .OwnPokemon.Item.Name.ToLower() = "big root" And .FieldEffects.CanUseItem(True) = True And .FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                addHP += CInt(Math.Ceiling(addHP * (30 / 100)))
                            End If
                        End If

                        ReduceHP(loseHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " lost HP due to Leech Seed!", "leechseed")

                        If .FieldEffects.OwnHealBlock = 0 Then
                            GainHP(addHP, True, True, BattleScreen, "", "leechseed")
                        End If
                    End If
                End If
                If .OwnPokemon.HP > 0 Then
                    If .OwnPokemon.Ability.Name.ToLower() = "poison heal" Then
                        If .FieldEffects.OppHealBlock = 0 Then
                            If .OwnPokemon.Status = Pokemon.StatusProblems.Poison Then
                                GainHP(CInt( .OwnPokemon.MaxHP / 8), True, True, BattleScreen, "Poison Heal healed " & .OwnPokemon.GetDisplayName() & ".", "poison")
                            End If

                            If .OwnPokemon.Status = Pokemon.StatusProblems.BadPoison Then
                                .FieldEffects.OwnPoisonCounter += 1
                                GainHP(CInt( .OwnPokemon.MaxHP / 8), True, True, BattleScreen, "Poison Heal healed " & .OwnPokemon.GetDisplayName() & ".", "poison")
                            End If
                        End If
                    Else
                        If .OwnPokemon.Ability.Name.ToLower() <> "magic guard" Then
                            If .OwnPokemon.Status = Pokemon.StatusProblems.Poison Then 'Own Poison
                                BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_poison", False))
                                ReduceHP(CInt( .OwnPokemon.MaxHP / 8), True, True, BattleScreen, "The poison hurt " & .OwnPokemon.GetDisplayName() & ".", "poison")
                            End If

                            If .OwnPokemon.Status = Pokemon.StatusProblems.BadPoison Then 'Own Toxic
                                .FieldEffects.OwnPoisonCounter += 1
                                Dim multiplier As Double = ( .FieldEffects.OwnPoisonCounter / 16)
                                BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_poison", False))
                                ReduceHP(CInt( .OwnPokemon.MaxHP * multiplier), True, True, BattleScreen, "The toxic hurt " & .OwnPokemon.GetDisplayName() & ".", "badpoison")
                            End If
                        End If
                    End If
                End If

                If .OwnPokemon.HP > 0 Then 'Burn
                    If .OwnPokemon.Status = Pokemon.StatusProblems.Burn Then
                        If .OwnPokemon.Ability.Name.ToLower() <> "water veil" And .OwnPokemon.Ability.Name.ToLower() <> "magic guard" Then
                            Dim reduceAmount As Integer = CInt( .OwnPokemon.MaxHP / 8)
                            If .OwnPokemon.Ability.Name.ToLower() = "heatproof" Then
                                reduceAmount = CInt( .OwnPokemon.MaxHP / 16)
                            End If

                            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_ember", False))
                            ReduceHP(reduceAmount, True, True, BattleScreen, .OwnPokemon.GetDisplayName() & " is hurt by the burn.", "burn")
                        End If
                    End If
                End If

                If .FieldEffects.OwnNightmare > 0 Then 'Nightmare
                    If .OwnPokemon.Status = Pokemon.StatusProblems.Sleep And .OwnPokemon.HP > 0 Then
                        ReduceHP(CInt( .OwnPokemon.MaxHP / 4), True, False, BattleScreen, "The nightmare haunted " & .OwnPokemon.GetDisplayName() & "!", "nightmare")
                    Else
                        .FieldEffects.OwnNightmare = 0
                    End If
                End If

                If .FieldEffects.OwnCurse > 0 Then 'Curse
                    If .OwnPokemon.HP > 0 Then
                        ReduceHP(CInt(.OwnPokemon.MaxHP / 4), True, False, BattleScreen, "The curse haunted " & .OwnPokemon.GetDisplayName() & "!", "curse")
                    End If
                End If
                'Water/Fire/Grass pledge:
                If .FieldEffects.OwnWaterPledge > 0 Then
                    .FieldEffects.OwnWaterPledge -= 1
                    If .FieldEffects.OwnWaterPledge = 0 Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The rainbow faded!"))
                    End If
                End If
                If .FieldEffects.OwnGrassPledge > 0 Then
                    .FieldEffects.OwnGrassPledge -= 1
                    If .FieldEffects.OwnGrassPledge = 0 Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The swamp faded!"))
                    End If
                End If
                If .FieldEffects.OwnFirePledge > 0 Then
                    .FieldEffects.OwnFirePledge -= 1
                    If .FieldEffects.OwnFirePledge = 0 Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The fiery sea faded!"))
                    Else
                        If .OwnPokemon.HP > 0 Then
                            ReduceHP(CInt( .OwnPokemon.MaxHP / 8), True, False, BattleScreen, "The firey sea hurt " & .OwnPokemon.GetDisplayName() & "!", "firepledge")
                        End If
                    End If
                End If

                If .OwnPokemon.HP > 0 Then
                    If .FieldEffects.OwnWrap > 0 Then 'Wrap
                        .FieldEffects.OwnWrap -= 1
                        If .FieldEffects.OwnWrap = 0 Then
                            .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & " was freed from Wrap!"))
                        Else
                            Dim multiHP As Integer = CInt( .OwnPokemon.MaxHP / 8)
                            If Not .OppPokemon.Item Is Nothing And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                If .OppPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt( .OwnPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " is hurt by Wrap!", "wrap")
                        End If
                    End If
                    If .FieldEffects.OwnWhirlpool > 0 Then 'Whirlpool
                        .FieldEffects.OwnWhirlpool -= 1
                        If .FieldEffects.OwnWhirlpool = 0 Then
                            .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & " was freed from Whirlpool!"))
                        Else
                            Dim multiHP As Integer = CInt( .OwnPokemon.MaxHP / 8)
                            If Not .OppPokemon.Item Is Nothing And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                If .OppPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt( .OwnPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " is hurt by Whirlpool!", "whirlpool")
                        End If
                    End If
                    If .FieldEffects.OwnSandTomb > 0 Then 'Sand Tomb
                        .FieldEffects.OwnSandTomb -= 1
                        If .FieldEffects.OwnSandTomb = 0 Then
                            .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & " was freed from Sand Tomb!"))
                        Else
                            Dim multiHP As Integer = CInt( .OwnPokemon.MaxHP / 8)
                            If Not .OppPokemon.Item Is Nothing And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                If .OppPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt( .OwnPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " is hurt by Sand Tomb!", "sandtomb")
                        End If
                    End If
                    If .FieldEffects.OwnBind > 0 Then 'Bind
                        .FieldEffects.OwnBind -= 1
                        If .FieldEffects.OwnBind = 0 Then
                            .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & " was freed from Bind!"))
                        Else
                            Dim multiHP As Integer = CInt( .OwnPokemon.MaxHP / 8)
                            If Not .OppPokemon.Item Is Nothing And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                If .OppPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt( .OwnPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " is hurt by Bind!", "bind")
                        End If
                    End If
                    If .FieldEffects.OwnClamp > 0 Then 'Clamp
                        .FieldEffects.OwnClamp -= 1
                        If .FieldEffects.OwnClamp = 0 Then
                            .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & " was freed from Clamp!"))
                        Else
                            Dim multiHP As Integer = CInt( .OwnPokemon.MaxHP / 8)
                            If Not .OppPokemon.Item Is Nothing And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                If .OppPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt( .OwnPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " is hurt by Clamp!", "clamp")
                        End If
                    End If
                    If .FieldEffects.OwnFireSpin > 0 Then 'Fire Spin
                        .FieldEffects.OwnFireSpin -= 1
                        If .FieldEffects.OwnFireSpin = 0 Then
                            .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & " was freed from Fire Spin!"))
                        Else
                            Dim multiHP As Integer = CInt( .OwnPokemon.MaxHP / 8)
                            If Not .OppPokemon.Item Is Nothing And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                If .OppPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt( .OwnPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " is hurt by Fire Spin!", "firespin")
                        End If
                    End If
                    If .FieldEffects.OwnMagmaStorm > 0 Then 'Magma Storm
                        .FieldEffects.OwnMagmaStorm -= 1
                        If .FieldEffects.OwnMagmaStorm = 0 Then
                            .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & " was freed from Magma Storm!"))
                        Else
                            Dim multiHP As Integer = CInt( .OwnPokemon.MaxHP / 8)
                            If Not .OppPokemon.Item Is Nothing And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                If .OppPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt( .OwnPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " is hurt by Magma Storm!", "magmastorm")
                        End If
                    End If
                    If .FieldEffects.OwnInfestation > 0 Then 'Infestation
                        .FieldEffects.OwnInfestation -= 1
                        If .FieldEffects.OwnInfestation = 0 Then
                            .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & " was freed from Infestation!"))
                        Else
                            Dim multiHP As Integer = CInt( .OwnPokemon.MaxHP / 8)
                            If Not .OppPokemon.Item Is Nothing And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                If .OppPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt( .OwnPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " is hurt by Infestation!", "infestation")
                        End If
                    End If                    
                End If

                'Own bad dreams
                If .OppPokemon.Ability.Name.ToLower() = "bad dreams" And .OwnPokemon.HP > 0 And .OwnPokemon.Status = Pokemon.StatusProblems.Sleep Then
                    ReduceHP(CInt( .OwnPokemon.MaxHP / 8), True, False, BattleScreen, "The bad dreams haunted" & .OwnPokemon.GetDisplayName() & "!", "baddreams")
                End If

                If .FieldEffects.OwnOutrage > 0 And .OwnPokemon.HP > 0 Then 'Outrage
                    .FieldEffects.OwnOutrage -= 1
                    If .FieldEffects.OwnOutrage = 0 Then
                        InflictConfusion(True, True, BattleScreen, .OwnPokemon.GetDisplayName() & "'s Outrage stopped.", "outrage")
                    End If
                End If
                If .FieldEffects.OwnPetalDance > 0 And .OwnPokemon.HP > 0 Then 'Petaldance
                    .FieldEffects.OwnPetalDance -= 1
                    If .FieldEffects.OwnPetalDance = 0 Then
                        InflictConfusion(True, True, BattleScreen, .OwnPokemon.GetDisplayName() & "'s Petal Dance stopped.", "petaldance")
                    End If
                End If
                If .FieldEffects.OwnThrash > 0 And .OwnPokemon.HP > 0 Then 'Thrash
                    .FieldEffects.OwnThrash -= 1
                    If .FieldEffects.OwnThrash = 0 Then
                        InflictConfusion(True, True, BattleScreen, .OwnPokemon.GetDisplayName() & "'s Thrash stopped.", "thrash")
                    End If
                End If

                If .FieldEffects.OwnUproar > 0 And .OwnPokemon.HP > 0 Then 'Uproar
                    .FieldEffects.OwnUproar -= 1
                    If .FieldEffects.OwnUproar = 0 Then
                        .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & "'s uproar stopped."))
                    End If
                End If

                'Disable
                'For each move in moveset, reduce Disable count. If disable count = 0, print message.

                If .FieldEffects.OwnEncore > 0 And .OwnPokemon.HP > 0 Then 'Encore
                    .FieldEffects.OwnEncore -= 1
                    If .FieldEffects.OwnEncore = 0 Then
                        .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & "'s encore stopped."))
                    End If
                End If

                If .FieldEffects.OwnTaunt > 0 And .OwnPokemon.HP > 0 Then 'Taunt
                    .FieldEffects.OwnTaunt -= 1
                    If .FieldEffects.OwnTaunt = 0 Then
                        .BattleQuery.Add(New TextQueryObject("The own taunt effect wore off"))
                    End If
                End If

                If .FieldEffects.OwnMagnetRise > 0 And .OwnPokemon.HP > 0 Then 'Magnetrise
                    .FieldEffects.OwnMagnetRise -= 1
                    If .FieldEffects.OwnMagnetRise = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Own Magnet Rise effect faded."))
                    End If
                End If

                If .FieldEffects.OwnHealBlock > 0 Then 'Healblock
                    .FieldEffects.OwnHealBlock -= 1
                    If .FieldEffects.OwnHealBlock = 0 Then
                        .BattleQuery.Add(New TextQueryObject("The effect of the own heal block faded."))
                    End If
                End If

                If .FieldEffects.OwnEmbargo > 0 And .OwnPokemon.HP > 0 Then 'Embargo
                    .FieldEffects.OwnEmbargo -= 1
                    If .FieldEffects.OwnEmbargo = 0 Then
                        .BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & " is not under the Embargo effect anymore."))
                    End If
                End If

                If .FieldEffects.OwnYawn > 0 And .OwnPokemon.HP > 0 Then 'Yawn
                    If .OwnPokemon.Status <> Pokemon.StatusProblems.Sleep Then
                        .FieldEffects.OwnYawn = 0
                        If .OwnPokemon.Status = Pokemon.StatusProblems.None Then
                            InflictSleep(True, False, BattleScreen, -1, "", "yawn")
                        End If
                    End If
                End If

                Dim futureSight As String = "Future Sight" 'Future Sight/Doom Desire
                If .FieldEffects.OwnFutureSightID = 1 Then
                    futureSight = "Doom Desire"
                End If
                If .FieldEffects.OwnFutureSightTurns > 0 Then
                    .FieldEffects.OwnFutureSightTurns -= 1
                    If .FieldEffects.OwnFutureSightTurns = 0 Then
                        If .OppPokemon.HP > 0 Then
                            ReduceHP( .FieldEffects.OwnFutureSightDamage, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " took the " & futureSight & " attack!", futureSight.Replace(" ", "").ToLower())
                        Else
                            .BattleQuery.Add(New TextQueryObject("The " & futureSight & " failed!"))
                        End If
                    End If
                End If

                'Perish Song:
                If .FieldEffects.OwnPerishSongCount > 0 Then 'Perish Song
                    .FieldEffects.OwnPerishSongCount -= 1
                    If .OwnPokemon.HP > 0 Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject( .OwnPokemon.GetDisplayName() & "'s Perish Count is at " & .FieldEffects.OwnPerishSongCount.ToString() & "!"))
                        If .FieldEffects.OwnPerishSongCount = 0 Then
                            ReduceHP( .OwnPokemon.HP, True, False, BattleScreen, "", "move:perishsong")
                            Me.FaintPokemon(True, BattleScreen, .OwnPokemon.GetDisplayName() & " fainted due to Perish Song!")
                        End If
                    End If
                End If

                'ABILITY SHIT/ITEM SHIT GOES HERE:

                If .OwnPokemon.HP > 0 And .OwnPokemon.Status <> Pokemon.StatusProblems.Burn Then
                    If Not .OwnPokemon.Item Is Nothing Then
                        If .OwnPokemon.Item.Name.ToLower() = "flame orb" And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                            InflictBurn(True, True, BattleScreen, "Flame orb inflicts a burn!", "flameorb")
                        End If
                    End If
                End If

                If .OwnPokemon.HP > 0 And .OwnPokemon.Status <> Pokemon.StatusProblems.Poison And .OwnPokemon.Status <> Pokemon.StatusProblems.BadPoison Then
                    If Not .OwnPokemon.Item Is Nothing Then
                        If .OwnPokemon.Item.Name.ToLower() = "toxic orb" And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                            InflictPoison(True, True, BattleScreen, True, "Toxic orb inflicts a poisoning!", "toxicorb")
                        End If
                    End If
                End If

                If .OwnPokemon.HP > 0 Then
                    If .OwnPokemon.Ability.Name.ToLower() = "moody" Then
                        Dim cannotRaise As New List(Of Integer)
                        Dim cannotLower As New List(Of Integer)

                        If .OwnPokemon.StatAttack = 6 Then
                            cannotRaise.Add(0)
                        ElseIf .OwnPokemon.StatAttack = -6 Then
                            cannotLower.Add(0)
                        End If
                        If .OwnPokemon.StatDefense = 6 Then
                            cannotRaise.Add(1)
                        ElseIf .OwnPokemon.StatDefense = -6 Then
                            cannotLower.Add(1)
                        End If
                        If .OwnPokemon.StatSpAttack = 6 Then
                            cannotRaise.Add(2)
                        ElseIf .OwnPokemon.StatSpAttack = -6 Then
                            cannotLower.Add(2)
                        End If
                        If .OwnPokemon.StatSpDefense = 6 Then
                            cannotRaise.Add(3)
                        ElseIf .OwnPokemon.StatSpDefense = -6 Then
                            cannotLower.Add(3)
                        End If
                        If .OwnPokemon.StatSpeed = 6 Then
                            cannotRaise.Add(4)
                        ElseIf .OwnPokemon.StatSpeed = -6 Then
                            cannotLower.Add(4)
                        End If
                        If .OwnPokemon.Accuracy = 6 Then
                            cannotRaise.Add(5)
                        ElseIf .OwnPokemon.Accuracy = -6 Then
                            cannotLower.Add(5)
                        End If
                        If .OwnPokemon.Evasion = 6 Then
                            cannotRaise.Add(6)
                        ElseIf .OwnPokemon.Evasion = -6 Then
                            cannotLower.Add(6)
                        End If

                        If cannotRaise.Count < 7 Then
                            Dim statToRaise As Integer = Core.Random.Next(0, 7)
                            While cannotRaise.Contains(statToRaise) = True
                                statToRaise = Core.Random.Next(0, 7)
                            End While

                            Select Case statToRaise
                                Case 0
                                    RaiseStat(True, True, BattleScreen, "Attack", 2, "Moody raised a stat.", "moody")
                                Case 1
                                    RaiseStat(True, True, BattleScreen, "Defense", 2, "Moody raised a stat.", "moody")
                                Case 2
                                    RaiseStat(True, True, BattleScreen, "Special Attack", 2, "Moody raised a stat.", "moody")
                                Case 3
                                    RaiseStat(True, True, BattleScreen, "Special Defense", 2, "Moody raised a stat.", "moody")
                                Case 4
                                    RaiseStat(True, True, BattleScreen, "Speed", 2, "Moody raised a stat.", "moody")
                                Case 5
                                    RaiseStat(True, True, BattleScreen, "Accuracy", 2, "Moody raised a stat.", "moody")
                                Case 6
                                    RaiseStat(True, True, BattleScreen, "Evasion", 2, "Moody raised a stat.", "moody")
                            End Select

                            If cannotLower.Contains(statToRaise) = False Then
                                cannotLower.Add(statToRaise)
                            End If
                        End If

                        If cannotLower.Count < 7 Then
                            Dim statToLower As Integer = Core.Random.Next(0, 7)
                            While cannotLower.Contains(statToLower) = True
                                statToLower = Core.Random.Next(0, 7)
                            End While

                            Select Case statToLower
                                Case 0
                                    LowerStat(True, True, BattleScreen, "Attack", 1, "Moody lowered a stat.", "moody")
                                Case 1
                                    LowerStat(True, True, BattleScreen, "Defense", 1, "Moody lowered a stat.", "moody")
                                Case 2
                                    LowerStat(True, True, BattleScreen, "Special Attack", 1, "Moody lowered a stat.", "moody")
                                Case 3
                                    LowerStat(True, True, BattleScreen, "Special Defense", 1, "Moody lowered a stat.", "moody")
                                Case 4
                                    LowerStat(True, True, BattleScreen, "Speed", 1, "Moody lowered a stat.", "moody")
                                Case 5
                                    LowerStat(True, True, BattleScreen, "Accuracy", 1, "Moody lowered a stat.", "moody")
                                Case 6
                                    LowerStat(True, True, BattleScreen, "Evasion", 1, "Moody lowered a stat.", "moody")
                            End Select
                        End If
                    End If
                End If
            End With
        End Sub

        Private Sub EndTurnOpp(ByVal BattleScreen As BattleScreen)
            With BattleScreen
                'Turn count
                .FieldEffects.OppPokemonTurns += 1

                .FieldEffects.OppLockOn = 0 'Reset lock on

                'Remove temp pursuit counter:
                .FieldEffects.OppPursuit = False

                If .FieldEffects.OppSleepTurns > 0 Then 'Sleep turns
                    .FieldEffects.OppSleepTurns -= 1
                End If

                If .FieldEffects.OppCharge > 0 Then
                    .FieldEffects.OppCharge -= 1 'Sets charge to 0
                End If
                If .OppPokemon.HP > 0 Then
                    If Not .OppPokemon.Item Is Nothing Then
                        If .OppPokemon.Item.Name.ToLower() = "mental herb" Then
                            Dim usedMentalHerb As Boolean = False
                            If .OppPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Infatuation) = True Then
                                .OppPokemon.RemoveVolatileStatus(Pokemon.VolatileStatus.Infatuation)
                                .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " got healed from the infatuation" & vbNewLine & "due to Mental Herb!"))
                                usedMentalHerb = True
                            End If
                            If .FieldEffects.OppTaunt > 0 Then
                                .FieldEffects.OppTaunt = 0
                                .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " got healed from the taunt" & vbNewLine & "due to Mental Herb!"))
                                usedMentalHerb = True
                            End If
                            If .FieldEffects.OppEncore > 0 Then
                                .FieldEffects.OppEncore = 0
                                .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " got healed from the encore" & vbNewLine & "due to Mental Herb!"))
                                usedMentalHerb = True
                            End If
                            If .FieldEffects.OppTorment > 0 Then
                                .FieldEffects.OppTorment = 0
                                .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " got healed from the torment" & vbNewLine & "due to Mental Herb!"))
                                usedMentalHerb = True
                            End If
                            'Remove disable
                            If usedMentalHerb = True Then
                                .OppPokemon.Item = Nothing
                            End If
                        End If
                        If .OppPokemon.Item.Name.ToLower() = "white herb" Then
                            Dim hasNegativeStats As Boolean = False
                            With .OppPokemon
                                If .StatAttack < 0 Then
                                    .StatAttack = 0
                                    hasNegativeStats = True
                                End If
                                If .StatDefense < 0 Then
                                    .StatDefense = 0
                                    hasNegativeStats = True
                                End If
                                If .StatSpAttack < 0 Then
                                    .StatSpAttack = 0
                                    hasNegativeStats = True
                                End If
                                If .StatSpDefense < 0 Then
                                    .StatSpDefense = 0
                                    hasNegativeStats = True
                                End If
                                If .StatSpeed < 0 Then
                                    .StatSpeed = 0
                                    hasNegativeStats = True
                                End If
                                If .Accuracy < 0 Then
                                    .Accuracy = 0
                                    hasNegativeStats = True
                                End If
                                If .Evasion < 0 Then
                                    .Evasion = 0
                                    hasNegativeStats = True
                                End If
                            End With
                            If hasNegativeStats = True Then
                                .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " negative stats got healed" & vbNewLine & "due to White Herb!"))
                                .OppPokemon.Item = Nothing
                            End If
                        End If
                    End If
                End If
                .FieldEffects.OppPokemonDamagedLastTurn = .FieldEffects.OppPokemonDamagedThisTurn
                .FieldEffects.OppPokemonDamagedThisTurn = False
            End With
        End Sub

        Private Sub EndRoundOpp(ByVal BattleScreen As BattleScreen)
            ChangeCameraAngel(0, True, BattleScreen)

            With BattleScreen
                If .FieldEffects.OppReflect > 0 Then 'Stop reflect
                    .FieldEffects.OppReflect -= 1
                    If .FieldEffects.OppReflect = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Opponent's Reflect effect faded."))
                    End If
                End If

                If .FieldEffects.OppLightScreen > 0 Then 'Stop light screen
                    .FieldEffects.OppLightScreen -= 1
                    If .FieldEffects.OppLightScreen = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Opponent's Light Screen effect faded."))
                    End If
                End If

                If .FieldEffects.OppMist > 0 Then 'Stop mist
                    .FieldEffects.OppMist -= 1
                    If .FieldEffects.OppMist = 0 Then
                        .BattleQuery.Add(New TextQueryObject("The mist on the opponent's side of the field faded!"))
                    End If
                End If

                If .FieldEffects.OppSafeguard > 0 Then 'Stop safeguard
                    .FieldEffects.OppSafeguard -= 1
                    If .FieldEffects.OppSafeguard = 0 Then
                        .BattleQuery.Add(New TextQueryObject("The Safeguard effect wore off!"))
                    End If
                End If

                If .FieldEffects.OppGuardSpec > 0 Then 'Stop guard spec
                    .FieldEffects.OppGuardSpec -= 1
                    If .FieldEffects.OppGuardSpec = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Opponent's Guard Spec. wore off."))
                    End If
                End If

                If .FieldEffects.OppTailWind > 0 Then 'Stop tail wind
                    .FieldEffects.OppTailWind -= 1
                    If .FieldEffects.OppTailWind = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Opponent's Tail Wind effect faded."))
                    End If
                End If

                If .FieldEffects.OppLuckyChant > 0 Then 'Stop lucky chant
                    .FieldEffects.OppLuckyChant -= 1
                    If .FieldEffects.OppLuckyChant = 0 Then
                        .BattleQuery.Add(New TextQueryObject("Opponent's Lucky Chant effect faded."))
                    End If
                End If

                If .FieldEffects.OppWish > 0 Then 'Use wish
                    .FieldEffects.OppWish -= 1
                    If .FieldEffects.OppWish = 0 Then
                        If .FieldEffects.OwnHealBlock = 0 Then
                            If .OppPokemon.HP < .OppPokemon.MaxHP And .OppPokemon.HP > 0 Then
                                GainHP(CInt(.OppPokemon.MaxHP / 2), False, False, BattleScreen, "A wish came true!", "wish")
                            End If
                        End If
                    End If
                End If

                'Weather
                'Sandstorm
                If .FieldEffects.Weather = BattleWeather.WeatherTypes.Sandstorm Then
                    If .OppPokemon.Type1.Type <> Element.Types.Ground And .OppPokemon.Type2.Type <> Element.Types.Ground And .OppPokemon.Type1.Type <> Element.Types.Steel And .OppPokemon.Type2.Type <> Element.Types.Steel And .OppPokemon.Type1.Type <> Element.Types.Rock And .OppPokemon.Type2.Type <> Element.Types.Rock Then
                        Dim sandAbilities() As String = {"sand veil", "sand rush", "sand force", "overcoat", "magic guard"}
                        If sandAbilities.Contains(.OppPokemon.Ability.Name.ToLower()) = False Then
                            If .OppPokemon.HP > 0 Then
                                Dim sandHP As Integer = CInt(.OppPokemon.MaxHP / 16)
                                ReduceHP(sandHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " took damage from the sandstorm!", "sandstorm")
                            End If
                        End If
                    End If
                End If

                'Hailstorm
                If .FieldEffects.Weather = BattleWeather.WeatherTypes.Hailstorm Then
                    If .OppPokemon.Type1.Type <> Element.Types.Ice And .OppPokemon.Type2.Type <> Element.Types.Ice Then
                        Dim hailAbilities() As String = {"ice body", "snow cloak", "overcoat", "magic guard"}
                        If hailAbilities.Contains(.OppPokemon.Ability.Name.ToLower()) = False Then
                            If .OppPokemon.HP > 0 Then
                                Dim hailHP As Integer = CInt(.OppPokemon.MaxHP / 16)
                                ReduceHP(hailHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " took damage from the hailstorm!", "sandstorm")
                            End If
                        End If
                    End If
                End If

                If .OppPokemon.HP > 0 Then
                    Dim HPChange As Integer = 0 'Use DrySkin/Rain Dish/Hydration/Ice Body
                    Dim HPMessage As String = ""
                    Select Case .OppPokemon.Ability.Name.ToLower()
                        Case "dry skin"
                            If .FieldEffects.Weather = BattleWeather.WeatherTypes.Sunny Then
                                HPChange = -CInt(.OppPokemon.MaxHP / 8)
                                HPMessage = "Dry Skin"
                            ElseIf .FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                                HPChange = CInt(.OppPokemon.MaxHP / 8)
                                HPMessage = "Dry Skin"
                            End If
                        Case "rain dish"
                            If .FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                                HPChange = CInt(.OppPokemon.MaxHP / 16)
                                HPMessage = "Rain Dish"
                            End If
                        Case "hydration"
                            If .FieldEffects.Weather = BattleWeather.WeatherTypes.Rain Then
                                If .OppPokemon.Status = Pokemon.StatusProblems.BadPoison Or .OppPokemon.Status = Pokemon.StatusProblems.Poison Or .OppPokemon.Status = Pokemon.StatusProblems.Paralyzed Or .OppPokemon.Status = Pokemon.StatusProblems.Freeze Or .OppPokemon.Status = Pokemon.StatusProblems.Burn Or .OppPokemon.Status = Pokemon.StatusProblems.Sleep Then
                                    CureStatusProblem(False, False, BattleScreen, "Hydration cured " & .OppPokemon.GetDisplayName() & "'s status problem.", "hydration")
                                End If
                            End If
                        Case "ice body"
                            If .FieldEffects.Weather = BattleWeather.WeatherTypes.Hailstorm Then
                                HPChange = CInt(.OppPokemon.MaxHP / 16)
                                HPMessage = "Ice Body"
                            End If
                    End Select
                    If HPChange > 0 Then
                        If .OppPokemon.HP < .OppPokemon.MaxHP Then
                            GainHP(HPChange, False, False, BattleScreen, .OppPokemon.GetDisplayName() & " restored some HP due to " & HPMessage & ".", HPMessage.Replace(" ", "").ToLower())
                        End If
                    ElseIf HPChange < 0 Then
                        ReduceHP(HPChange, False, False, BattleScreen, .OppPokemon.GetDisplayName() & " lost some HP due to " & HPMessage & ".", HPMessage.Replace(" ", "").ToLower())
                    End If
                End If

                If .FieldEffects.OppIngrain > 0 And .OppPokemon.HP < .OppPokemon.MaxHP And .OppPokemon.HP > 0 Then 'Ingrain effect
                    If .FieldEffects.OwnHealBlock = 0 Then
                        Dim healHP As Integer = CInt(BattleScreen.OppPokemon.MaxHP / 16)
                        If Not BattleScreen.OppPokemon.Item Is Nothing Then
                            If .OppPokemon.Item.Name.ToLower() = "big root" And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                healHP = CInt(healHP * 1.3F)
                            End If
                        End If
                        GainHP(healHP, False, False, BattleScreen, .OppPokemon.GetDisplayName() & " gained health from the Ingrain.", "ingrain")
                    End If
                End If

                If .FieldEffects.OppAquaRing > 0 And .OppPokemon.HP < .OppPokemon.MaxHP And .OppPokemon.HP > 0 Then 'Aqua Ring effect
                    If .FieldEffects.OwnHealBlock = 0 Then
                        Dim healHP As Integer = CInt(BattleScreen.OppPokemon.MaxHP / 16)
                        If Not .OppPokemon.Item Is Nothing Then
                            If .OppPokemon.Item.Name.ToLower() = "big root" And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                healHP = CInt(healHP * 1.3F)
                            End If
                        End If
                        GainHP(healHP, False, False, BattleScreen, .OppPokemon.GetDisplayName() & " gained health from the Aqua Ring.", "aquaring")
                    End If
                End If

                If .OppPokemon.Ability.Name.ToLower() = "shed skin" And .OppPokemon.HP > 0 Then 'Shed skin effect
                    If .OppPokemon.Status = Pokemon.StatusProblems.BadPoison Or .OppPokemon.Status = Pokemon.StatusProblems.Poison Or .OppPokemon.Status = Pokemon.StatusProblems.Paralyzed Or .OppPokemon.Status = Pokemon.StatusProblems.Freeze Or .OppPokemon.Status = Pokemon.StatusProblems.Burn Or .OppPokemon.Status = Pokemon.StatusProblems.Sleep Then
                        If Core.Random.Next(0, 100) < 33 Then
                            .BattleQuery.Add(.FocusOppPokemon())
                            CureStatusProblem(False, False, BattleScreen, .OppPokemon.GetDisplayName() & "'s Shed Skin cured its status problem.", "shedskin")
                        End If
                    End If
                End If

                If .OppPokemon.Ability.Name.ToLower() = "speed boost" And .OppPokemon.HP > 0 Then 'Speed boost/opp first
                    RaiseStat(False, False, BattleScreen, "Speed", 1, .OppPokemon.GetDisplayName() & "'s Speed Boost raised its speed.", "speedboost")
                End If

                If .OppPokemon.Ability.Name.ToLower() = "truant" Then 'Set truant round
                    If .FieldEffects.OppTruantRound = 1 Then
                        .FieldEffects.OppTruantRound = 0
                    Else
                        .FieldEffects.OppTruantRound = 1
                    End If
                End If

                If Not .OppPokemon.Item Is Nothing Then 'Black Sludge
                    If .OppPokemon.Item.Name.ToLower() = "black sludge" And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                        If .OppPokemon.Type1.Type = Element.Types.Poison Or .OppPokemon.Type2.Type = Element.Types.Poison Then
                            If .OppPokemon.HP < .OppPokemon.MaxHP And .OppPokemon.HP > 0 Then
                                GainHP(CInt(.OppPokemon.MaxHP / 16), False, False, BattleScreen, .OppPokemon.GetDisplayName() & " gained HP from Black Sludge!", "blacksludge")
                            End If
                        Else
                            If .OppPokemon.HP > 0 Then
                                ReduceHP(CInt(.OppPokemon.MaxHP / 8), False, False, BattleScreen, .OppPokemon.GetDisplayName() & " lost HP due to Black Sludge!", "blacksludge")
                            End If
                        End If
                    End If
                End If

                If .OppPokemon.HP < .OppPokemon.MaxHP And .OppPokemon.HP > 0 Then
                    If .FieldEffects.OwnHealBlock = 0 Then
                        If Not .OppPokemon.Item Is Nothing Then 'Leftovers
                            If .OppPokemon.Item.Name.ToLower() = "leftovers" And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                GainHP(CInt(.OppPokemon.MaxHP / 16), False, False, BattleScreen, .OppPokemon.GetDisplayName() & " restored some HP from Leftovers!", "leftovers")
                            End If
                        End If
                    End If
                End If
                If .FieldEffects.OwnLeechSeed > 0 Then 'LeechSeed (Own pokemon seeded)
                    If .OwnPokemon.HP > 0 And .OppPokemon.HP > 0 And .OppPokemon.HP < .OppPokemon.MaxHP Then
                        Dim loseHP As Integer = CInt(Math.Ceiling(.OwnPokemon.MaxHP / 8))
                        Dim currHP As Integer = .OwnPokemon.HP

                        If loseHP > currHP Then
                            loseHP = currHP
                        End If

                        Dim addHP As Integer = loseHP
                        If Not .OppPokemon.Item Is Nothing Then
                            If .OppPokemon.Item.Name.ToLower() = "big root" And .FieldEffects.CanUseItem(False) = True And .FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                                addHP += CInt(Math.Ceiling(addHP * (30 / 100)))
                            End If
                        End If

                        ReduceHP(loseHP, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & " lost HP due to Leech Seed!", "leechseed")

                        If .FieldEffects.OppHealBlock = 0 Then
                            GainHP(addHP, False, False, BattleScreen, "", "leechseed")
                        End If
                    End If
                End If
                If .OppPokemon.HP > 0 Then
                    If .OppPokemon.Ability.Name.ToLower() = "poison heal" Then
                        If .FieldEffects.OwnHealBlock = 0 Then
                            If .OppPokemon.Status = Pokemon.StatusProblems.Poison Then
                                GainHP(CInt(.OppPokemon.MaxHP / 8), False, False, BattleScreen, "Poison Heal healed " & .OppPokemon.GetDisplayName() & ".", "poison")
                            End If

                            If .OppPokemon.Status = Pokemon.StatusProblems.BadPoison Then
                                .FieldEffects.OppPoisonCounter += 1
                                GainHP(CInt(.OppPokemon.MaxHP / 8), False, False, BattleScreen, "Poison Heal healed " & .OppPokemon.GetDisplayName() & ".", "poison")
                            End If
                        End If
                    Else
                        If .OppPokemon.Ability.Name.ToLower() <> "magic guard" Then
                            If .OppPokemon.Status = Pokemon.StatusProblems.Poison Then 'Opp Poison
                                BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_poison", False))
                                ReduceHP(CInt(.OppPokemon.MaxHP / 8), False, False, BattleScreen, "The poison hurt " & .OppPokemon.GetDisplayName() & ".", "poison")
                            End If

                            If .OppPokemon.Status = Pokemon.StatusProblems.BadPoison Then 'Opp Toxic
                                .FieldEffects.OppPoisonCounter += 1
                                Dim multiplier As Double = (.FieldEffects.OppPoisonCounter / 16)
                                BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_poison", False))
                                ReduceHP(CInt(.OppPokemon.MaxHP * multiplier), False, False, BattleScreen, "The toxic hurt " & .OppPokemon.GetDisplayName() & ".", "badpoison")
                            End If
                        End If
                    End If
                End If

                If .OppPokemon.HP > 0 Then 'Burn
                    If .OppPokemon.Status = Pokemon.StatusProblems.Burn Then
                        If .OppPokemon.Ability.Name.ToLower() <> "water veil" And .OppPokemon.Ability.Name.ToLower() <> "magic guard" Then
                            Dim reduceAmount As Integer = CInt(.OppPokemon.MaxHP / 8)
                            If .OppPokemon.Ability.Name.ToLower() = "heatproof" Then
                                reduceAmount = CInt(.OppPokemon.MaxHP / 16)
                            End If

                            BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\Effects\effect_ember", False))
                            ReduceHP(reduceAmount, False, False, BattleScreen, .OppPokemon.GetDisplayName() & " is hurt by the burn.", "burn")
                        End If
                    End If
                End If

                If .FieldEffects.OppNightmare > 0 Then 'Nightmare
                    If .OppPokemon.Status = Pokemon.StatusProblems.Sleep And .OppPokemon.HP > 0 Then
                        ReduceHP(CInt(.OppPokemon.MaxHP / 4), False, True, BattleScreen, "The nightmare haunted " & .OppPokemon.GetDisplayName() & "!", "nightmare")
                    Else
                        .FieldEffects.OwnNightmare = 0
                    End If
                End If

                If .FieldEffects.OppCurse > 0 Then 'Curse
                    If .OppPokemon.HP > 0 Then
                        ReduceHP(CInt(.OppPokemon.MaxHP / 4), False, True, BattleScreen, "The curse haunted " & .OppPokemon.GetDisplayName() & "!", "curse")
                    End If
                End If

                'Water/Fire/Grass pledge:
                If .FieldEffects.OppWaterPledge > 0 Then
                    .FieldEffects.OppWaterPledge -= 1
                    If .FieldEffects.OppWaterPledge = 0 Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The rainbow faded!"))
                    End If
                End If
                If .FieldEffects.OppGrassPledge > 0 Then
                    .FieldEffects.OppGrassPledge -= 1
                    If .FieldEffects.OppGrassPledge = 0 Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The swamp faded!"))
                    End If
                End If
                If .FieldEffects.OppFirePledge > 0 Then
                    .FieldEffects.OppFirePledge -= 1
                    If .FieldEffects.OppFirePledge = 0 Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject("The fiery sea faded!"))
                    Else
                        If .OppPokemon.HP > 0 Then
                            ReduceHP(CInt(.OppPokemon.MaxHP / 8), False, True, BattleScreen, "The firey sea hurt " & .OppPokemon.GetDisplayName() & "!", "firepledge")
                        End If
                    End If
                End If

                If .OppPokemon.HP > 0 Then
                    If .FieldEffects.OppWrap > 0 Then 'Wrap
                        .FieldEffects.OppWrap -= 1
                        If .FieldEffects.OppWrap = 0 Then
                            .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " was freed from Wrap!"))
                        Else
                            Dim multiHP As Integer = CInt(.OppPokemon.MaxHP / 8)
                            If Not .OwnPokemon.Item Is Nothing And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                If .OwnPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt(.OppPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " is hurt by Wrap!", "wrap")
                        End If
                    End If
                    If .FieldEffects.OppWhirlpool > 0 Then 'Whirlpool
                        .FieldEffects.OppWhirlpool -= 1
                        If .FieldEffects.OppWhirlpool = 0 Then
                            .BattleQuery.Add(New TextQueryObject(.OwnPokemon.GetDisplayName() & " was freed from Whirlpool!"))
                        Else
                            Dim multiHP As Integer = CInt(.OppPokemon.MaxHP / 8)
                            If Not .OwnPokemon.Item Is Nothing And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                If .OwnPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt(.OppPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " is hurt by Whirlpool!", "whirlpool")
                        End If
                    End If
                    If .FieldEffects.OppSandTomb > 0 Then 'Sand Tomb
                        .FieldEffects.OppSandTomb -= 1
                        If .FieldEffects.OppSandTomb = 0 Then
                            .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " was freed from Sand Tomb!"))
                        Else
                            Dim multiHP As Integer = CInt(.OppPokemon.MaxHP / 8)
                            If Not .OwnPokemon.Item Is Nothing And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                If .OwnPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt(.OppPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " is hurt by Sand Tomb!", "sandtomb")
                        End If
                    End If
                    If .FieldEffects.OppBind > 0 Then 'Bind
                        .FieldEffects.OppBind -= 1
                        If .FieldEffects.OppBind = 0 Then
                            .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " was freed from Bind!"))
                        Else
                            Dim multiHP As Integer = CInt(.OppPokemon.MaxHP / 8)
                            If Not .OwnPokemon.Item Is Nothing And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                If .OwnPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt(.OppPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " is hurt by Bind!", "bind")
                        End If
                    End If
                    If .FieldEffects.OppClamp > 0 Then 'Clamp
                        .FieldEffects.OppClamp -= 1
                        If .FieldEffects.OppClamp = 0 Then
                            .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " was freed from Clamp!"))
                        Else
                            Dim multiHP As Integer = CInt(.OppPokemon.MaxHP / 8)
                            If Not .OwnPokemon.Item Is Nothing And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                If .OwnPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt(.OppPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " is hurt by Clamp!", "clamp")
                        End If
                    End If
                    If .FieldEffects.OppFireSpin > 0 Then 'Fire Spin
                        .FieldEffects.OppFireSpin -= 1
                        If .FieldEffects.OppFireSpin = 0 Then
                            .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " was freed from Fire Spin!"))
                        Else
                            Dim multiHP As Integer = CInt(.OppPokemon.MaxHP / 8)
                            If Not .OwnPokemon.Item Is Nothing And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                If .OwnPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt(.OppPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " is hurt by Fire Spin!", "firespin")
                        End If
                    End If
                    If .FieldEffects.OppMagmaStorm > 0 Then 'Magma Storm
                        .FieldEffects.OppMagmaStorm -= 1
                        If .FieldEffects.OppMagmaStorm = 0 Then
                            .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " was freed from Magma Storm!"))
                        Else
                            Dim multiHP As Integer = CInt(.OppPokemon.MaxHP / 8)
                            If Not .OwnPokemon.Item Is Nothing And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                If .OwnPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt(.OppPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " is hurt by Magma Storm!", "magmastorm")
                        End If
                    End If
                    If .FieldEffects.OppInfestation > 0 Then 'Infestation
                        .FieldEffects.OppInfestation -= 1
                        If .FieldEffects.OppInfestation = 0 Then
                            .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " was freed from Infestation!"))
                        Else
                            Dim multiHP As Integer = CInt(.OppPokemon.MaxHP / 8)
                            If Not .OwnPokemon.Item Is Nothing And .FieldEffects.CanUseItem(True) = True And BattleScreen.FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                                If .OwnPokemon.Item.Name.ToLower() = "binding band" Then
                                    multiHP = CInt(.OppPokemon.MaxHP / 6)
                                End If
                            End If
                            ReduceHP(multiHP, False, True, BattleScreen, .OppPokemon.GetDisplayName() & " is hurt by Infestation!", "infestation")
                        End If
                    End If
                End If

                'Opp bad dreams
                If .OwnPokemon.Ability.Name.ToLower() = "bad dreams" And .OppPokemon.HP > 0 And .OppPokemon.Status = Pokemon.StatusProblems.Sleep Then
                    ReduceHP(CInt(.OppPokemon.MaxHP / 8), False, True, BattleScreen, "The bad dreams haunted" & .OppPokemon.GetDisplayName() & "!", "baddreams")
                End If

                If .FieldEffects.OppOutrage > 0 And .OppPokemon.HP > 0 Then 'Outrage
                    .FieldEffects.OppOutrage -= 1
                    If .FieldEffects.OppOutrage = 0 Then
                        InflictConfusion(False, False, BattleScreen, .OppPokemon.GetDisplayName() & "'s Outrage stopped.", "outrage")
                    End If
                End If
                If .FieldEffects.OppPetalDance > 0 And .OppPokemon.HP > 0 Then 'Petaldance
                    .FieldEffects.OppPetalDance -= 1
                    If .FieldEffects.OppPetalDance = 0 Then
                        InflictConfusion(False, False, BattleScreen, .OppPokemon.GetDisplayName() & "'s Petal Dance stopped.", "petaldance")
                    End If
                End If
                If .FieldEffects.OppThrash > 0 And .OppPokemon.HP > 0 Then 'Thrash
                    .FieldEffects.OppThrash -= 1
                    If .FieldEffects.OppThrash = 0 Then
                        InflictConfusion(False, False, BattleScreen, .OppPokemon.GetDisplayName() & "'s Thrash stopped.", "thrash")
                    End If
                End If

                If .FieldEffects.OppUproar > 0 And .OppPokemon.HP > 0 Then 'Uproar
                    .FieldEffects.OppUproar -= 1
                    If .FieldEffects.OppUproar = 0 Then
                        .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & "'s uproar stopped."))
                    End If
                End If

                'Disable
                'For each move in moveset, reduce Disable count. If disable count = 0, print message.

                If .FieldEffects.OppEncore > 0 And .OppPokemon.HP > 0 Then 'Encore
                    .FieldEffects.OppEncore -= 1
                    If .FieldEffects.OppEncore = 0 Then
                        .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & "'s encore stopped."))
                    End If
                End If

                If .FieldEffects.OppTaunt > 0 And .OppPokemon.HP > 0 Then 'Taunt
                    .FieldEffects.OppTaunt -= 1
                    If .FieldEffects.OppTaunt = 0 Then
                        .BattleQuery.Add(New TextQueryObject("The opponent's taunt effect wore off"))
                    End If
                End If

                If .FieldEffects.OppMagnetRise > 0 And .OppPokemon.HP > 0 Then 'Magnetrise
                    .FieldEffects.OppMagnetRise -= 1
                    If .FieldEffects.OppMagnetRise = 0 Then
                        .BattleQuery.Add(New TextQueryObject("The opponent's Magnet Rise effect faded."))
                    End If
                End If

                If .FieldEffects.OppHealBlock > 0 Then 'Healblock
                    .FieldEffects.OppHealBlock -= 1
                    If .FieldEffects.OppHealBlock = 0 Then
                        .BattleQuery.Add(New TextQueryObject("The effect of the opponent's heal block faded."))
                    End If
                End If

                If .FieldEffects.OppEmbargo > 0 And .OppPokemon.HP > 0 Then 'Embargo
                    .FieldEffects.OppEmbargo -= 1
                    If .FieldEffects.OppEmbargo = 0 Then
                        .BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & " is not under the Embargo effect anymore."))
                    End If
                End If

                If .FieldEffects.OppYawn > 0 And .OppPokemon.HP > 0 Then 'Yawn
                    If .OppPokemon.Status <> Pokemon.StatusProblems.Sleep Then
                        .FieldEffects.OppYawn = 0
                        If .OppPokemon.Status = Pokemon.StatusProblems.None Then
                            InflictSleep(False, True, BattleScreen, -1, "", "yawn")
                        End If
                    End If
                End If

                Dim futureSight As String = "Future Sight" 'Future Sight/Doom Desire
                If .FieldEffects.OppFutureSightID = 1 Then
                    futureSight = "Doom Desire"
                End If
                If .FieldEffects.OppFutureSightTurns > 0 Then
                    .FieldEffects.OppFutureSightTurns -= 1
                    If .FieldEffects.OppFutureSightTurns = 0 Then
                        If .OwnPokemon.HP > 0 Then
                            ReduceHP(.FieldEffects.OppFutureSightDamage, True, False, BattleScreen, .OwnPokemon.GetDisplayName() & "took the " & futureSight & " attack!", futureSight.Replace(" ", "").ToLower())
                        Else
                            .BattleQuery.Add(New TextQueryObject("The " & futureSight & " failed!"))
                        End If
                    End If
                End If

                If .FieldEffects.OppPerishSongCount > 0 Then 'Perish Song
                    .FieldEffects.OppPerishSongCount -= 1
                    If .OppPokemon.HP > 0 Then
                        BattleScreen.BattleQuery.Add(New TextQueryObject(.OppPokemon.GetDisplayName() & "'s Perish Count is at " & .FieldEffects.OppPerishSongCount.ToString() & "!"))
                        If .FieldEffects.OppPerishSongCount = 0 Then
                            ReduceHP(.OppPokemon.HP, False, True, BattleScreen, "", "move:perishsong")
                            Me.FaintPokemon(False, BattleScreen, .OppPokemon.GetDisplayName() & " fainted due to Perish Song!")
                        End If
                    End If
                End If

                'ABILITY SHIT/ITEM SHIT GOES HERE:

                If .OppPokemon.HP > 0 And .OppPokemon.Status <> Pokemon.StatusProblems.Burn Then
                    If Not .OppPokemon.Item Is Nothing Then
                        If .OppPokemon.Item.Name.ToLower() = "flame orb" And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                            InflictBurn(False, False, BattleScreen, "Flame orb inflicts a burn!", "flameorb")
                        End If
                    End If
                End If

                If .OppPokemon.HP > 0 And .OppPokemon.Status <> Pokemon.StatusProblems.Poison And .OppPokemon.Status <> Pokemon.StatusProblems.BadPoison Then
                    If Not .OppPokemon.Item Is Nothing Then
                        If .OppPokemon.Item.Name.ToLower() = "toxic orb" And .FieldEffects.CanUseItem(False) = True And BattleScreen.FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                            InflictPoison(False, False, BattleScreen, True, "Toxic orb inflicts a poisoning!", "toxicorb")
                        End If
                    End If
                End If

                If .OppPokemon.HP > 0 Then
                    If .OppPokemon.Ability.Name.ToLower() = "moody" Then
                        Dim cannotRaise As New List(Of Integer)
                        Dim cannotLower As New List(Of Integer)

                        If .OppPokemon.StatAttack = 6 Then
                            cannotRaise.Add(0)
                        ElseIf .OppPokemon.StatAttack = -6 Then
                            cannotLower.Add(0)
                        End If
                        If .OppPokemon.StatDefense = 6 Then
                            cannotRaise.Add(1)
                        ElseIf .OppPokemon.StatDefense = -6 Then
                            cannotLower.Add(1)
                        End If
                        If .OppPokemon.StatSpAttack = 6 Then
                            cannotRaise.Add(2)
                        ElseIf .OppPokemon.StatSpAttack = -6 Then
                            cannotLower.Add(2)
                        End If
                        If .OppPokemon.StatSpDefense = 6 Then
                            cannotRaise.Add(3)
                        ElseIf .OppPokemon.StatSpDefense = -6 Then
                            cannotLower.Add(3)
                        End If
                        If .OppPokemon.StatSpeed = 6 Then
                            cannotRaise.Add(4)
                        ElseIf .OppPokemon.StatSpeed = -6 Then
                            cannotLower.Add(4)
                        End If
                        If .OppPokemon.Accuracy = 6 Then
                            cannotRaise.Add(5)
                        ElseIf .OppPokemon.Accuracy = -6 Then
                            cannotLower.Add(5)
                        End If
                        If .OppPokemon.Evasion = 6 Then
                            cannotRaise.Add(6)
                        ElseIf .OppPokemon.Evasion = -6 Then
                            cannotLower.Add(6)
                        End If

                        If cannotRaise.Count < 7 Then
                            Dim statToRaise As Integer = Core.Random.Next(0, 7)
                            While cannotRaise.Contains(statToRaise) = True
                                statToRaise = Core.Random.Next(0, 7)
                            End While

                            Select Case statToRaise
                                Case 0
                                    RaiseStat(False, False, BattleScreen, "Attack", 2, "Moody raised a stat.", "moody")
                                Case 1
                                    RaiseStat(False, False, BattleScreen, "Defense", 2, "Moody raised a stat.", "moody")
                                Case 2
                                    RaiseStat(False, False, BattleScreen, "Special Attack", 2, "Moody raised a stat.", "moody")
                                Case 3
                                    RaiseStat(False, False, BattleScreen, "Special Defense", 2, "Moody raised a stat.", "moody")
                                Case 4
                                    RaiseStat(False, False, BattleScreen, "Speed", 2, "Moody raised a stat.", "moody")
                                Case 5
                                    RaiseStat(False, False, BattleScreen, "Accuracy", 2, "Moody raised a stat.", "moody")
                                Case 6
                                    RaiseStat(False, False, BattleScreen, "Evasion", 2, "Moody raised a stat.", "moody")
                            End Select

                            If cannotLower.Contains(statToRaise) = False Then
                                cannotLower.Add(statToRaise)
                            End If
                        End If

                        If cannotLower.Count < 7 Then
                            Dim statToLower As Integer = Core.Random.Next(0, 7)
                            While cannotLower.Contains(statToLower) = True
                                statToLower = Core.Random.Next(0, 7)
                            End While

                            Select Case statToLower
                                Case 0
                                    LowerStat(False, False, BattleScreen, "Attack", 1, "Moody lowered a stat.", "moody")
                                Case 1
                                    LowerStat(False, False, BattleScreen, "Defense", 1, "Moody lowered a stat.", "moody")
                                Case 2
                                    LowerStat(False, False, BattleScreen, "Special Attack", 1, "Moody lowered a stat.", "moody")
                                Case 3
                                    LowerStat(False, False, BattleScreen, "Special Defense", 1, "Moody lowered a stat.", "moody")
                                Case 4
                                    LowerStat(False, False, BattleScreen, "Speed", 1, "Moody lowered a stat.", "moody")
                                Case 5
                                    LowerStat(False, False, BattleScreen, "Accuracy", 1, "Moody lowered a stat.", "moody")
                                Case 6
                                    LowerStat(False, False, BattleScreen, "Evasion", 1, "Moody lowered a stat.", "moody")
                            End Select
                        End If
                    End If
                End If
            End With
        End Sub

#End Region

#Region "Switching"

        Public Sub SwitchOutOwn(ByVal BattleScreen As BattleScreen, ByVal SwitchInIndex As Integer, ByVal InsertIndex As Integer, Optional ByVal message As String = "")
            With BattleScreen
                ChangeCameraAngel(1, True, BattleScreen)

                'Natural cure cures status problems
                If .OwnPokemon.Ability.Name.ToLower() = "natural cure" Then
                    If .OwnPokemon.Status <> Pokemon.StatusProblems.Fainted And .OwnPokemon.Status <> Pokemon.StatusProblems.None Then
                        .OwnPokemon.Status = Pokemon.StatusProblems.None
                        .AddToQuery(InsertIndex, New TextQueryObject( .OwnPokemon.GetDisplayName() & "'s status problem got healed by Natural Cure"))
                    End If
                End If

                'save baton pass stuff:
                If .FieldEffects.OwnUsedBatonPass = True Then
                    .FieldEffects.OwnBatonPassStats = New List(Of Integer)
                    With .OwnPokemon
                        BattleScreen.FieldEffects.OwnBatonPassStats.AddRange({ .StatAttack, .StatDefense, .StatSpAttack, .StatSpDefense, .StatSpeed, .Evasion, .Accuracy})
                    End With
                    .FieldEffects.OwnBatonPassConfusion = .OwnPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True
                End If

                'Set the original objects of Pokemon
                .OwnPokemon.ResetTemp()

                'Remove volatiles
                .OwnPokemon.ClearAllVolatiles()

                'Resetting FieldEffects
                With .FieldEffects
                    .OwnSleepTurns = 0
                    .OwnTruantRound = 0
                    .OwnTaunt = 0
                    .OwnRageCounter = 0
                    .OwnUproar = 0
                    If .OwnUsedBatonPass = False Then .OwnFocusEnergy = 0
                    .OwnEndure = 0
                    .OwnProtectCounter = 0
                    .OwnDetectCounter = 0
                    .OwnKingsShieldCounter = 0
                    .OwnProtectMovesCount = 0
                    If .OwnUsedBatonPass = False Then .OwnIngrain = 0
                    If .OwnUsedBatonPass = False Then .OwnSubstitute = 0
                    If .OwnUsedBatonPass = False Then .OwnMagnetRise = 0
                    If .OwnUsedBatonPass = False Then .OwnAquaRing = 0
                    .OwnPoisonCounter = 0
                    .OwnNightmare = 0
                    If .OwnUsedBatonPass = False Then .OwnCurse = 0
                    .OwnOutrage = 0
                    .OwnThrash = 0
                    .OwnPetalDance = 0
                    .OwnEncore = 0
                    .OwnEncoreMove = Nothing
                    If .OwnUsedBatonPass = False Then .OwnEmbargo = 0
                    .OwnYawn = 0
                    If .OwnUsedBatonPass = False Then .OwnPerishSongCount = 0
                    .OwnConfusionTurns = 0
                    .OwnTorment = 0
                    .OwnTormentMove = Nothing
                    .OwnChoiceMove = Nothing
                    .OwnRecharge = 0
                    .OwnRolloutCounter = 0
                    .OwnIceBallCounter = 0
                    .OwnDefenseCurl = 0
                    .OwnCharge = 0
                    .OwnSolarBeam = 0
                    If .OwnUsedBatonPass = False Then .OwnLeechSeed = 0
                    If .OwnUsedBatonPass = False Then .OwnLockOn = 0
                    .OwnLansatBerry = 0
                    .OwnCustapBerry = 0
                    .OwnTrappedCounter = 0
                    .OwnFuryCutter = 0
                    .OwnPokemonTurns = 0
                    .OwnStockpileCount = 0
                    .OwnDestinyBond = False
                    .OwnGastroAcid = False

                    .OwnForesight = 0
                    .OwnOdorSleuth = 0
                    .OwnMiracleEye = 0

                    .OwnFlyCounter = 0
                    .OwnDigCounter = 0
                    .OwnBounceCounter = 0
                    .OwnDiveCounter = 0
                    .OwnShadowForceCounter = 0
                    .OwnSkyDropCounter = 0
                    .OwnSkyAttackCounter = 0
                    .OwnRazorWindCounter = 0
                    .OwnSkullBashCounter = 0

                    .OwnWrap = 0
                    .OwnWhirlpool = 0
                    .OwnBind = 0
                    .OwnClamp = 0
                    .OwnFireSpin = 0
                    .OwnMagmaStorm = 0
                    .OwnSandTomb = 0
                    .OwnInfestation = 0

                    .OwnBideCounter = 0
                    .OwnBideDamage = 0

                    .OwnRoostUsed = False
                End With

                .OwnPokemon.Ability.SwitchOut( .OwnPokemon)

                BattleScreen.AddToQuery(InsertIndex, New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OwnPokemon, 2, -1, -1, -1, -1))

                If Core.Player.CountFightablePokemon > 0 Then
                    SwitchInOwn(BattleScreen, SwitchInIndex, False, InsertIndex, message)
                Else
                    If BattleScreen.IsTrainerBattle = True Then
                        EndBattle(EndBattleReasons.LooseTrainer, BattleScreen, False)
                        If BattleScreen.IsRemoteBattle = True Then
                            EndBattle(EndBattleReasons.LooseTrainer, BattleScreen, True)
                        End If
                    Else
                        EndBattle(EndBattleReasons.LooseWild, BattleScreen, False)
                    End If
                End If
            End With
        End Sub

        Public Sub ApplyOwnBatonPass(ByVal BattleScreen As BattleScreen)
            If BattleScreen.FieldEffects.OwnUsedBatonPass = True Then
                BattleScreen.FieldEffects.OwnUsedBatonPass = False

                BattleScreen.OwnPokemon.StatAttack = BattleScreen.FieldEffects.OwnBatonPassStats(0)
                BattleScreen.OwnPokemon.StatDefense = BattleScreen.FieldEffects.OwnBatonPassStats(1)
                BattleScreen.OwnPokemon.StatSpAttack = BattleScreen.FieldEffects.OwnBatonPassStats(2)
                BattleScreen.OwnPokemon.StatSpDefense = BattleScreen.FieldEffects.OwnBatonPassStats(3)
                BattleScreen.OwnPokemon.StatSpeed = BattleScreen.FieldEffects.OwnBatonPassStats(4)
                BattleScreen.OwnPokemon.Evasion = BattleScreen.FieldEffects.OwnBatonPassStats(5)
                BattleScreen.OwnPokemon.Accuracy = BattleScreen.FieldEffects.OwnBatonPassStats(6)

                If BattleScreen.FieldEffects.OwnBatonPassConfusion = True Then
                    BattleScreen.FieldEffects.OwnBatonPassConfusion = False
                    BattleScreen.OwnPokemon.AddVolatileStatus(Pokemon.VolatileStatus.Confusion)
                End If
            End If
        End Sub

        Public Sub SwitchInOwn(ByVal BattleScreen As BattleScreen, ByVal NewPokemonIndex As Integer, ByVal FirstTime As Boolean, ByVal InsertIndex As Integer, Optional ByVal message As String = "")
            If FirstTime = False Then
                Dim insertMessage As String = message

                If insertMessage = "" Then
                    insertMessage = "Come back, " & BattleScreen.OwnPokemon.GetDisplayName() & "!"
                End If

                BattleScreen.AddToQuery(InsertIndex, New TextQueryObject(insertMessage))

                Dim index As Integer = NewPokemonIndex
                If index <= -1 Then
                    For i = 0 To Core.Player.Pokemons.Count - 1
                        If Core.Player.Pokemons(i).Status <> Pokemon.StatusProblems.Fainted And Core.Player.Pokemons(i).IsEgg() = False Then
                            index = i
                            Exit For
                        End If
                    Next
                End If
                BattleScreen.OwnPokemonIndex = index

                If BattleScreen.ParticipatedPokemon.Contains(BattleScreen.OwnPokemonIndex) = False Then
                    BattleScreen.ParticipatedPokemon.Add(BattleScreen.OwnPokemonIndex)
                End If

                BattleScreen.OwnPokemon = Core.Player.Pokemons(index)
                Me.ApplyOwnBatonPass(BattleScreen)

                Dim ownShiny As String = "N"
                If BattleScreen.OwnPokemon.IsShiny = True Then
                    ownShiny = "S"
                End If

                Dim ownModel As String = BattleScreen.GetModelName(True)

                If ownModel = "" Then
                    BattleScreen.AddToQuery(InsertIndex, New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OwnPokemon, PokemonForms.GetOverworldSpriteName(BattleScreen.OwnPokemon), 0, 1, -1, -1))
                Else
                    BattleScreen.AddToQuery(InsertIndex, New ToggleEntityQueryObject(True, ownModel, 1, 0, -1, -1))
                End If

                BattleScreen.AddToQuery(InsertIndex, New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OwnPokemon, 1, -1, -1, -1, -1))
                BattleScreen.BattleQuery.Add(New PlaySoundQueryObject(BattleScreen.OwnPokemon.Number.ToString(), True))
                BattleScreen.AddToQuery(InsertIndex, New TextQueryObject("Go, " & BattleScreen.OwnPokemon.GetDisplayName() & "!"))
            End If

            With BattleScreen
                If .FieldEffects.UsedPokemon.Contains(NewPokemonIndex) = False Then
                    .FieldEffects.UsedPokemon.Add(NewPokemonIndex)
                End If

                If Not .OwnPokemon.Item Is Nothing Then
                    If .OwnPokemon.Item.Name.ToLower() = "amulet coin" Or .OwnPokemon.Item.Name.ToLower() = "luck incense" Then
                        If .FieldEffects.CanUseItem(True) = True And .FieldEffects.CanUseOwnItem(True, BattleScreen) = True Then
                            BattleScreen.FieldEffects.AmuletCoin += 1
                        End If
                    End If
                End If

                Dim p As Pokemon = .OwnPokemon
                Dim op As Pokemon = .OppPokemon

                Dim spikeAffected As Boolean = True
                Dim rockAffected As Boolean = True

                If p.Type1.Type = Element.Types.Flying Or p.Type2.Type = Element.Types.Flying Or p.Ability.Name.ToLower() = "levitate" And BattleScreen.FieldEffects.CanUseAbility(True, BattleScreen) = True Then
                    spikeAffected = False
                End If

                If .FieldEffects.Gravity > 0 Then
                    spikeAffected = True
                Else
                    If Not p.Item Is Nothing Then
                        If p.Item.Name.ToLower() = "air ballon" And .FieldEffects.CanUseItem(False) = True And .FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                            spikeAffected = False
                        End If
                        If p.Item.Name.ToLower() = "iron ball" And .FieldEffects.CanUseItem(False) = True And .FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                            spikeAffected = True
                        End If
                    End If
                End If

                'Spikes
                If spikeAffected = True Then
                    If .FieldEffects.OppSpikes > 0 And (p.Ability.Name.ToLower() <> "magic guard" Or BattleScreen.FieldEffects.CanUseAbility(True, BattleScreen, 1) = False) Then
                        Dim spikeDamage As Double = 1D
                        Select Case .FieldEffects.OppSpikes
                            Case 1
                                spikeDamage = (p.MaxHP / 100) * 12.5D
                            Case 2
                                spikeDamage = (p.MaxHP / 100) * 16.7D
                            Case 3
                                spikeDamage = (p.MaxHP / 100) * 25D
                        End Select
                        ReduceHP(CInt(spikeDamage), True, False, BattleScreen, "The Spikes hurt " & p.GetDisplayName() & "!", "spikes")
                    End If
                End If
                
                'Sticky Web
                If spikeAffected = True Then
                    If .FieldEffects.OppStickyWeb > 0 Then
                        
                        LowerStat(True, True, BattleScreen, "Speed", 1, "Your pokemon was caught in a sticky web!", "sticky web")
    

                    End If
                End If

                'Toxic Spikes
                If spikeAffected = True Then
                    If .FieldEffects.OppToxicSpikes > 0 And p.Status = Pokemon.StatusProblems.None And p.Type1.Type <> Element.Types.Poison And p.Type2.Type <> Element.Types.Poison Then
                        Select Case .FieldEffects.OppToxicSpikes
                            Case 1
                                InflictPoison(True, False, BattleScreen, False, "The Toxic Spikes hurt " & p.GetDisplayName() & "!", "toxicspikes")
                            Case 2
                                InflictPoison(True, False, BattleScreen, True, "The Toxic Spikes hurt " & p.GetDisplayName() & "!", "toxicspikes")
                        End Select
                    End If
                    If .FieldEffects.OppToxicSpikes > 0 Then
                        If p.Type1.Type = Element.Types.Poison Or p.Type2.Type = Element.Types.Poison Then
                            .AddToQuery(InsertIndex, New TextQueryObject(p.GetDisplayName() & " removed the Toxic Spikes!"))
                            .FieldEffects.OppToxicSpikes = 0
                        End If
                    End If
                End If
                
                'Stealth Rock
                If rockAffected = True Then
                    If .FieldEffects.OppStealthRock > 0 And (p.Ability.Name.ToLower() <> "magic guard" Or BattleScreen.FieldEffects.CanUseAbility(True, BattleScreen, 1) = False) Then
                        Dim rocksDamage As Double = 1D

                        Dim effectiveness As Single = BattleCalculation.ReverseTypeEffectiveness(Element.GetElementMultiplier(New Element(Element.Types.Rock), p.Type1)) * BattleCalculation.ReverseTypeEffectiveness(Element.GetElementMultiplier(New Element(Element.Types.Rock), p.Type2))
                        Select Case effectiveness
                            Case 0.25F
                                rocksDamage = (p.MaxHP / 100) * 3.125D
                            Case 0.5F
                                rocksDamage = (p.MaxHP / 100) * 6.25D
                            Case 1.0F
                                rocksDamage = (p.MaxHP / 100) * 12.5D
                            Case 2.0F
                                rocksDamage = (p.MaxHP / 100) * 25D
                            Case 4.0F
                                rocksDamage = (p.MaxHP / 100) * 50D
                        End Select

                        ReduceHP(CInt(rocksDamage), True, False, BattleScreen, "The Stealth Rocks hurt " & p.GetDisplayName() & "!", "stealthrocks")
                    End If
                End If

                If BattleScreen.FieldEffects.CanUseAbility(True, BattleScreen, 1) = True Then
                    Select Case p.Ability.Name.ToLower()
                        Case "drizzle"
                            ChangeWeather(True, True, BattleWeather.WeatherTypes.Rain, 10000, BattleScreen, "Drizzle makes it rain!", "drizzle")
                        Case "cloud nine"
                            ChangeWeather(True, True, BattleWeather.WeatherTypes.Clear, 0, BattleScreen, "", "cloudnine")
                        Case "intimidate"
                            LowerStat(False, True, BattleScreen, "Attack", 1, p.GetDisplayName() & "'s Intimidate cuts " & op.GetDisplayName() & "'s attack!", "intimidate")
                        Case "trace"
                            If op.Ability.Name.ToLower() <> "multitype" And op.Ability.Name.ToLower() <> "illusion" Then
                                p.OriginalAbility = p.Ability
                                p.Ability = op.Ability
                                .AddToQuery(InsertIndex, New TextQueryObject(p.GetDisplayName() & " copied the ability " & op.Ability.Name & " from " & op.GetDisplayName() & "!"))
                            End If
                        Case "sand stream"
                            ChangeWeather(True, True, BattleWeather.WeatherTypes.Sandstorm, 10000, BattleScreen, "Sand Stream creates a sandstorm!", "sandstream")
                        Case "forecast"
                            ApplyForecast(BattleScreen)
                        Case "drought"
                            ChangeWeather(True, True, BattleWeather.WeatherTypes.Sunny, 10000, BattleScreen, "The sunlight turned harsh!", "drought")
                        Case "air lock"
                            ChangeWeather(True, True, BattleWeather.WeatherTypes.Clear, 0, BattleScreen, "", "airlock")
                        Case "download"
                            If op.Defense < op.SpDefense Then
                                RaiseStat(True, True, BattleScreen, "Attack", 1, "Download analysed the foe!", "download")
                            Else
                                RaiseStat(True, True, BattleScreen, "Special Attack", 1, "Download analysed the foe!", "download")
                            End If
                        Case "mold breaker"
                            .AddToQuery(InsertIndex, New TextQueryObject(p.GetDisplayName() & " breakes the mold!"))
                        Case "turbo blaze"
                            .AddToQuery(InsertIndex, New TextQueryObject(p.GetDisplayName() & " is radiating a blazing aura!"))
                        Case "teravolt"
                            .AddToQuery(InsertIndex, New TextQueryObject(p.GetDisplayName() & " is radiating a bursting aura!"))
                        Case "anticipation"
                            Dim doShudder As Boolean = False
                            'Check every move if it is: super effective/1hitko/explosion/selfdestruct
                            If doShudder = True Then
                                .AddToQuery(InsertIndex, New TextQueryObject(op.GetDisplayName() & " makes " & p.GetDisplayName() & " shudder!"))
                            End If
                        Case "forewarn"
                            Dim moves As New List(Of Attack)
                            'Add attacks with highest base power here
                            Dim move As Attack = Nothing
                            If moves.Count > 1 Then
                                move = moves(Core.Random.Next(0, moves.Count))
                            ElseIf moves.Count = 1 Then
                                move = moves(0)
                            End If
                            If Not move Is Nothing Then
                                .AddToQuery(InsertIndex, New TextQueryObject(op.GetDisplayName() & " makes " & p.GetDisplayName() & " shudder!"))
                            End If
                        Case "snow warning"
                            ChangeWeather(True, True, BattleWeather.WeatherTypes.Hailstorm, 10000, BattleScreen, "Snow Warning summoned a hailstorm!", "snowwarning")
                        Case "frisk"
                            If Not op.Item Is Nothing Then
                                .AddToQuery(InsertIndex, New TextQueryObject(op.GetDisplayName() & " is holding " & op.Item.Name & "."))
                            End If
                        Case "multitype"
                            p.OriginalType1 = p.Type1
                            p.OriginalType2 = p.Type2

                            p.Type1 = New Element(Element.Types.Normal)
                            p.Type2 = New Element(Element.Types.Blank)

                            If Not p.Item Is Nothing Then
                                Dim changeType As Boolean = False
                                Dim newType As Element = Nothing

                                Select Case p.Item.Name.ToLower()
                                    Case "draco plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Dragon)
                                    Case "dread plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Dark)
                                    Case "earth plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Ground)
                                    Case "fist plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Fighting)
                                    Case "flame plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Fire)
                                    Case "icicle plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Ice)
                                    Case "insect plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Bug)
                                    Case "iron plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Steel)
                                    Case "meadow plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Grass)
                                    Case "mind plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Psychic)
                                    Case "sky plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Flying)
                                    Case "splash plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Water)
                                    Case "spooky plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Ghost)
                                    Case "stone plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Rock)
                                    Case "toxic plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Poison)
                                    Case "zap plate"
                                        changeType = True
                                        newType = New Element(Element.Types.Electric)
                                End Select

                                If changeType = True Then
                                    p.Type1 = newType
                                    p.Type2 = New Element(Element.Types.Blank)
                                End If
                            End If

                            .AddToQuery(InsertIndex, New TextQueryObject(p.GetDisplayName() & "'s type changed to " & p.Type1.ToString() & "!"))
                        Case "imposter"
                            'Doing the ditto stuff!
                    End Select
                End If

                If .OwnPokemon.Status = Pokemon.StatusProblems.Sleep Then
                    .FieldEffects.OwnSleepTurns = Core.Random.Next(1, 4)
                End If

                If BattleScreen.FieldEffects.OwnHealingWish = True Then
                    BattleScreen.FieldEffects.OwnHealingWish = False

                    If .OwnPokemon.HP < .OwnPokemon.MaxHP Or .OwnPokemon.Status <> Pokemon.StatusProblems.None Then
                        GainHP( .OwnPokemon.MaxHP - .OwnPokemon.HP, True, True, BattleScreen, "The healing wish came true for " & .OwnPokemon.GetDisplayName() & "!", "move:healingwish")
                        CureStatusProblem(True, True, BattleScreen, "", "move:healingwish")
                    End If
                End If
            End With
        End Sub

        Public Sub SwitchOutOpp(ByVal BattleScreen As BattleScreen, ByVal index As Integer, Optional ByVal message As String = "")
            With BattleScreen
                'Natural cure cures status problems
                If .OppPokemon.Ability.Name.ToLower() = "natural cure" Then
                    If .OppPokemon.Status <> Pokemon.StatusProblems.Fainted And .OppPokemon.Status <> Pokemon.StatusProblems.None Then
                        .OppPokemon.Status = Pokemon.StatusProblems.None
                        .BattleQuery.Add(New TextQueryObject( .OppPokemon.GetDisplayName() & "'s status problem got healed by Natural Cure"))
                    End If
                End If

                'save baton pass stuff:
                If .FieldEffects.OppUsedBatonPass = True Then
                    .FieldEffects.OppBatonPassStats = New List(Of Integer)
                    With .OppPokemon
                        BattleScreen.FieldEffects.OppBatonPassStats.AddRange({ .StatAttack, .StatDefense, .StatSpAttack, .StatSpDefense, .StatSpeed, .Evasion, .Accuracy})
                    End With
                    .FieldEffects.OppBatonPassConfusion = .OppPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True
                End If

                'Set the original objects of Pokemon
                .OppPokemon.ResetTemp()

                'Remove volatiles
                .OppPokemon.ClearAllVolatiles()

                'Resetting FieldEffects
                With .FieldEffects
                    .OppSleepTurns = 0
                    .OppTruantRound = 0
                    .OppTaunt = 0
                    .OppRageCounter = 0
                    .OppUproar = 0
                    If .OppUsedBatonPass = False Then .OppFocusEnergy = 0
                    .OppEndure = 0
                    .OppProtectCounter = 0
                    .OppDetectCounter = 0
                    .OppKingsShieldCounter = 0
                    .OppProtectMovesCount = 0
                    If .OppUsedBatonPass = False Then .OppIngrain = 0
                    If .OppUsedBatonPass = False Then .OppSubstitute = 0
                    If .OppUsedBatonPass = False Then .OppMagnetRise = 0
                    If .OppUsedBatonPass = False Then .OppAquaRing = 0
                    .OppPoisonCounter = 0
                    .OppNightmare = 0
                    If .OppUsedBatonPass = False Then .OppCurse = 0
                    .OppOutrage = 0
                    .OppThrash = 0
                    .OppPetalDance = 0
                    .OppEncore = 0
                    .OppEncoreMove = Nothing
                    If .OppUsedBatonPass = False Then .OppEmbargo = 0
                    .OppYawn = 0
                    If .OppUsedBatonPass = False Then .OppPerishSongCount = 0
                    .OppConfusionTurns = 0
                    .OppTorment = 0
                    .OppTormentMove = Nothing
                    .OppChoiceMove = Nothing
                    .OppRecharge = 0
                    .OppRolloutCounter = 0
                    .OppIceBallCounter = 0
                    .OppDefenseCurl = 0
                    .OppCharge = 0
                    .OppSolarBeam = 0
                    If .OppUsedBatonPass = False Then .OppLeechSeed = 0
                    If .OppUsedBatonPass = False Then .OppLockOn = 0
                    .OppLansatBerry = 0
                    .OppCustapBerry = 0
                    .OppTrappedCounter = 0
                    .OppFuryCutter = 0
                    .OppPokemonTurns = 0
                    .OppStockpileCount = 0
                    .OppDestinyBond = False
                    .OppGastroAcid = False

                    .OppFlyCounter = 0
                    .OppDigCounter = 0
                    .OppBounceCounter = 0
                    .OppDiveCounter = 0
                    .OppShadowForceCounter = 0
                    .OppSkyDropCounter = 0
                    .OppSkyAttackCounter = 0
                    .OppRazorWindCounter = 0
                    .OppSkullBashCounter = 0

                    .OppForesight = 0
                    .OppOdorSleuth = 0
                    .OppMiracleEye = 0

                    .OppWrap = 0
                    .OppWhirlpool = 0
                    .OppBind = 0
                    .OppClamp = 0
                    .OppFireSpin = 0
                    .OppMagmaStorm = 0
                    .OppSandTomb = 0
                    .OppInfestation = 0

                    .OppBideCounter = 0
                    .OppBideDamage = 0

                    .OppRoostUsed = False
                End With
            End With

            BattleScreen.OppPokemon.Ability.SwitchOut(BattleScreen.OppPokemon)

            If BattleScreen.IsTrainerBattle = False Then
                ChangeCameraAngel(1, False, BattleScreen)
                BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OppPokemon, 2, -1, -1, -1, -1))

                EndBattle(EndBattleReasons.WinWild, BattleScreen, False)
            Else
                If BattleScreen.TrainerHasFightablePokemon() = True Then
                    If BattleScreen.OppPokemon.HP <= 0 Or BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Fainted Then
                        GainEXP(BattleScreen)
                    End If
                    BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OppPokemon, 2, -1, -1, -1, -1))

                    SwitchInOpp(BattleScreen, False, index)
                Else
                    GainEXP(BattleScreen)

                    ChangeCameraAngel(1, False, BattleScreen)
                    BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OppPokemon, 2, -1, -1, -1, -1))

                    If message = "" Then
                        message = BattleScreen.Trainer.Name & ": ""Come back, " & BattleScreen.OppPokemon.GetDisplayName() & "!"""
                    End If

                    BattleScreen.BattleQuery.Add(New TextQueryObject(message))

                    EndBattle(EndBattleReasons.WinTrainer, BattleScreen, False)
                    If BattleScreen.IsRemoteBattle = True Then
                        EndBattle(EndBattleReasons.WinTrainer, BattleScreen, True)
                    End If
                End If
            End If
        End Sub

        Public Sub ApplyOppBatonPass(ByVal BattleScreen As BattleScreen)
            If BattleScreen.FieldEffects.OppUsedBatonPass = True Then
                BattleScreen.FieldEffects.OppUsedBatonPass = False

                BattleScreen.OppPokemon.StatAttack = BattleScreen.FieldEffects.OppBatonPassStats(0)
                BattleScreen.OppPokemon.StatDefense = BattleScreen.FieldEffects.OppBatonPassStats(1)
                BattleScreen.OppPokemon.StatSpAttack = BattleScreen.FieldEffects.OppBatonPassStats(2)
                BattleScreen.OppPokemon.StatSpDefense = BattleScreen.FieldEffects.OppBatonPassStats(3)
                BattleScreen.OppPokemon.StatSpeed = BattleScreen.FieldEffects.OppBatonPassStats(4)
                BattleScreen.OppPokemon.Evasion = BattleScreen.FieldEffects.OppBatonPassStats(5)
                BattleScreen.OppPokemon.Accuracy = BattleScreen.FieldEffects.OppBatonPassStats(6)

                If BattleScreen.FieldEffects.OppBatonPassConfusion = True Then
                    BattleScreen.FieldEffects.OppBatonPassConfusion = False
                    BattleScreen.OppPokemon.AddVolatileStatus(Pokemon.VolatileStatus.Confusion)
                End If
            End If
        End Sub

        Public Sub SwitchInOpp(ByVal BattleScreen As BattleScreen, ByVal FirstTime As Boolean, ByVal index As Integer)
            If FirstTime = False Then
                ChangeCameraAngel(1, False, BattleScreen)
                BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.Trainer.Name & ": ""Come back, " & BattleScreen.OppPokemon.GetDisplayName() & "!"""))

                BattleScreen.SendInNewTrainerPokemon(index)
                Me.ApplyOppBatonPass(BattleScreen)

                If BattleScreen.ParticipatedPokemon.Contains(BattleScreen.OwnPokemonIndex) = False Then
                    BattleScreen.ParticipatedPokemon.Add(BattleScreen.OwnPokemonIndex)
                End If

                Dim oppShiny As String = "N"
                If BattleScreen.OppPokemon.IsShiny = True Then
                    oppShiny = "S"
                End If

                Dim oppModel As String = BattleScreen.GetModelName(False)

                If oppModel = "" Then
                    BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OppPokemon, PokemonForms.GetOverworldSpriteName(BattleScreen.OppPokemon), -1, -1, 0, 1))
                Else
                    BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(False, oppModel, -1, -1, 1, 0))
                End If

                BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(True, ToggleEntityQueryObject.BattleEntities.OppPokemon, 1, -1, -1, -1, -1))
                BattleScreen.BattleQuery.Add(New PlaySoundQueryObject(BattleScreen.OppPokemon.Number.ToString(), True))
                BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.Trainer.Name & ": ""Go, " & BattleScreen.OppPokemon.GetDisplayName() & "!"""))
            End If

            With BattleScreen
                Dim p As Pokemon = .OppPokemon
                Dim op As Pokemon = .OwnPokemon

                Dim spikeAffected As Boolean = True
                Dim rockAffected As Boolean = True

                If p.Type1.Type = Element.Types.Flying Or p.Type2.Type = Element.Types.Flying Or p.Ability.Name.ToLower() = "levitate" Then
                    spikeAffected = False
                End If

                If .FieldEffects.Gravity > 0 Then
                    spikeAffected = True
                Else
                    If Not p.Item Is Nothing Then
                        If p.Item.Name.ToLower() = "air ballon" And .FieldEffects.CanUseItem(False) = True And .FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                            spikeAffected = False
                        End If
                        If p.Item.Name.ToLower() = "iron ball" And .FieldEffects.CanUseItem(False) = True And .FieldEffects.CanUseOwnItem(False, BattleScreen) = True Then
                            spikeAffected = True
                        End If
                    End If
                End If

                If spikeAffected = True Then
                    If .FieldEffects.OwnSpikes > 0 And p.Ability.Name.ToLower() <> "magic guard" Then
                        Dim spikeDamage As Double = 1D
                        Select Case .FieldEffects.OwnSpikes
                            Case 1
                                spikeDamage = (p.MaxHP / 100) * 12.5D
                            Case 2
                                spikeDamage = (p.MaxHP / 100) * 16.7D
                            Case 3
                                spikeDamage = (p.MaxHP / 100) * 25D
                        End Select
                        ReduceHP(CInt(spikeDamage), False, True, BattleScreen, "The Spikes hurt " & p.GetDisplayName() & "!", "spikes")
                    End If
                End If
                'Sticky Web
                If spikeAffected = True Then
                    If .FieldEffects.OwnStickyWeb > 0 Then
                        
                        LowerStat(False, False, BattleScreen, "Speed", 1, "The opposing pokemon was caught in a sticky web!", "sticky web")
    

                    End If
                End If
                If spikeAffected = True Then
                    If .FieldEffects.OwnToxicSpikes > 0 And p.Status = Pokemon.StatusProblems.None And p.Type1.Type <> Element.Types.Poison And p.Type2.Type <> Element.Types.Poison Then
                        Select Case .FieldEffects.OwnToxicSpikes
                            Case 1
                                InflictPoison(False, True, BattleScreen, False, "The Toxic Spikes hurt " & p.GetDisplayName() & "!", "toxicspikes")
                            Case 2
                                InflictPoison(False, True, BattleScreen, True, "The Toxic Spikes hurt " & p.GetDisplayName() & "!", "toxicspikes")
                        End Select
                    End If
                    If .FieldEffects.OwnToxicSpikes > 0 Then
                        If p.Type1.Type = Element.Types.Poison Or p.Type2.Type = Element.Types.Poison Then
                            .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " removed the Toxic Spikes!"))
                            .FieldEffects.OwnToxicSpikes = 0
                        End If
                    End If
                End If

                If rockAffected = True Then
                    If .FieldEffects.OwnStealthRock > 0 And p.Ability.Name.ToLower() <> "magic guard" Then
                        Dim rocksDamage As Double = 1D

                        Dim effectiveness As Single = BattleCalculation.ReverseTypeEffectiveness(Element.GetElementMultiplier(New Element(Element.Types.Rock), p.Type1)) * BattleCalculation.ReverseTypeEffectiveness(Element.GetElementMultiplier(New Element(Element.Types.Rock), p.Type2))
                        Select Case effectiveness
                            Case 0.25F
                                rocksDamage = (p.MaxHP / 100) * 3.125D
                            Case 0.5F
                                rocksDamage = (p.MaxHP / 100) * 6.25D
                            Case 1.0F
                                rocksDamage = (p.MaxHP / 100) * 12.5D
                            Case 2.0F
                                rocksDamage = (p.MaxHP / 100) * 25D
                            Case 4.0F
                                rocksDamage = (p.MaxHP / 100) * 50D
                        End Select

                        ReduceHP(CInt(rocksDamage), False, True, BattleScreen, "The Stealth Rocks hurt " & p.GetDisplayName() & "!", "stealthrocks")
                    End If
                End If

                Select Case p.Ability.Name.ToLower()
                    Case "drizzle"
                        ChangeWeather(False, False, BattleWeather.WeatherTypes.Rain, 10000, BattleScreen, "Drizzle makes it rain!", "drizzle")
                    Case "cloud nine"
                        ChangeWeather(False, False, BattleWeather.WeatherTypes.Clear, 0, BattleScreen, "", "cloudnine")
                    Case "intimidate"
                        LowerStat(True, False, BattleScreen, "Attack", 1, p.GetDisplayName() & "'s Intimidate cuts " & op.GetDisplayName() & "'s attack!", "intimidate")
                    Case "trace"
                        If op.Ability.Name.ToLower() <> "multitype" And op.Ability.Name.ToLower() <> "illusion" Then
                            p.OriginalAbility = p.Ability
                            p.Ability = op.Ability
                            .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " copied the ability " & op.Ability.Name & " from " & op.GetDisplayName() & "!"))
                        End If
                    Case "sand stream"
                        ChangeWeather(False, False, BattleWeather.WeatherTypes.Sandstorm, 10000, BattleScreen, "Sand Stream creates a sandstorm!", "sandstream")
                    Case "forecast"
                        ApplyForecast(BattleScreen)
                    Case "drought"
                        ChangeWeather(False, False, BattleWeather.WeatherTypes.Sunny, 10000, BattleScreen, "The sunlight turned harsh!", "drought")
                    Case "air lock"
                        ChangeWeather(False, False, BattleWeather.WeatherTypes.Clear, 0, BattleScreen, "", "airlock")
                    Case "download"
                        If op.Defense < op.SpDefense Then
                            RaiseStat(False, False, BattleScreen, "Attack", 1, "Download analysed the foe!", "download")
                        Else
                            RaiseStat(False, False, BattleScreen, "Special Attack", 1, "Download analysed the foe!", "download")
                        End If
                    Case "mold breaker"
                        .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " breakes the mold!"))
                    Case "turbo blaze"
                        .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is radiating a blazing aura!"))
                    Case "teravolt"
                        .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " is radiating a bursting aura!"))
                    Case "anticipation"
                        Dim doShudder As Boolean = False
                        'Check every move if it is: super effective/1hitko/explosion/selfdestruct
                        If doShudder = True Then
                            .BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " makes " & p.GetDisplayName() & " shudder!"))
                        End If
                    Case "forewarn"
                        Dim moves As New List(Of Attack)
                        'Add attacks with highest base power here
                        Dim move As Attack = Nothing
                        If moves.Count > 1 Then
                            move = moves(Core.Random.Next(0, moves.Count))
                        ElseIf moves.Count = 1 Then
                            move = moves(0)
                        End If
                        If Not move Is Nothing Then
                            .BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " makes " & p.GetDisplayName() & " shudder!"))
                        End If
                    Case "snow warning"
                        ChangeWeather(False, False, BattleWeather.WeatherTypes.Hailstorm, 10000, BattleScreen, "Snow Warning summoned a hailstorm!", "snowwarning")
                    Case "frisk"
                        If Not op.Item Is Nothing Then
                            .BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " is holding " & op.Item.Name & "."))
                        End If
                    Case "multitype"
                        p.OriginalType1 = p.Type1
                        p.OriginalType2 = p.Type2

                        p.Type1 = New Element(Element.Types.Normal)
                        p.Type2 = New Element(Element.Types.Blank)

                        If Not p.Item Is Nothing Then
                            Dim changeType As Boolean = False
                            Dim newType As Element = Nothing

                            Select Case p.Item.Name.ToLower()
                                Case "draco plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Dragon)
                                Case "dread plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Dark)
                                Case "earth plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Ground)
                                Case "fist plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Fighting)
                                Case "flame plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Fire)
                                Case "icicle plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Ice)
                                Case "insect plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Bug)
                                Case "iron plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Steel)
                                Case "meadow plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Grass)
                                Case "mind plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Psychic)
                                Case "sky plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Flying)
                                Case "splash plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Water)
                                Case "spooky plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Ghost)
                                Case "stone plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Rock)
                                Case "toxic plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Poison)
                                Case "zap plate"
                                    changeType = True
                                    newType = New Element(Element.Types.Electric)
                            End Select

                            If changeType = True Then
                                p.Type1 = newType
                                p.Type2 = New Element(Element.Types.Blank)
                            End If
                        End If

                        .BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & "'s type changed to " & p.Type1.ToString() & "!"))
                    Case "imposter"
                        'Doing the ditto stuff!
                End Select

                If .OppPokemon.Status = Pokemon.StatusProblems.Sleep Then
                    .FieldEffects.OppSleepTurns = Core.Random.Next(1, 4)
                End If

                If BattleScreen.FieldEffects.OppHealingWish = True Then
                    BattleScreen.FieldEffects.OppHealingWish = False

                    If .OppPokemon.HP < .OppPokemon.MaxHP Or .OppPokemon.Status <> Pokemon.StatusProblems.None Then
                        GainHP( .OppPokemon.MaxHP - .OppPokemon.HP, False, False, BattleScreen, "The healing wish came true for " & .OppPokemon.GetDisplayName() & "!", "move:healingwish")
                        CureStatusProblem(False, False, BattleScreen, "", "move:healingwish")
                    End If
                End If
            End With
        End Sub

#End Region

#Region "EndBattle"

        Enum EndBattleReasons
            WinWild
            LooseWild
            WinTrainer
            LooseTrainer
            WinPvP
            LoosePvP
        End Enum

        Public Sub EndBattle(ByVal reason As EndBattleReasons, ByVal BattleScreen As BattleScreen, ByVal AddPVP As Boolean)
            If AddPVP = True Then
                Select Case reason
                    Case EndBattleReasons.WinTrainer 'Lost
                        Dim q As New CameraQueryObject(New Vector3(12, 0, 13), Screen.Camera.Position, 0.03F, 0.03F, (MathHelper.Pi * 0.5F), Screen.Camera.Yaw, 0.0F, Screen.Camera.Pitch, 0.02F, 0.02F)
                        q.ApplyCurrentCamera = True
                        BattleScreen.TempPVPBattleQuery.Add(BattleScreen.BattleQuery.Count - 5, q)

                        BattleScreen.TempPVPBattleQuery.Add(BattleScreen.BattleQuery.Count - 4, New TextQueryObject("You lost the battle!"))
                        BattleScreen.TempPVPBattleQuery.Add(BattleScreen.BattleQuery.Count - 3, New TextQueryObject(""))
                        BattleScreen.TempPVPBattleQuery.Add(BattleScreen.BattleQuery.Count - 2, New TextQueryObject(""))

                        BattleScreen.TempPVPBattleQuery.Add(BattleScreen.BattleQuery.Count - 1, New EndBattleQueryObject(True))
                    Case EndBattleReasons.LooseTrainer 'Won
                        Dim q As New CameraQueryObject(New Vector3(15, 0, 13), Screen.Camera.Position, 0.03F, 0.03F, -(MathHelper.Pi * 0.5F), Screen.Camera.Yaw, 0.0F, Screen.Camera.Pitch, 0.02F, 0.02F)
                        q.ApplyCurrentCamera = True
                        BattleScreen.TempPVPBattleQuery.Add(BattleScreen.BattleQuery.Count - 3, q)

                        BattleScreen.TempPVPBattleQuery.Add(BattleScreen.BattleQuery.Count - 2, New TextQueryObject("Pokémon Trainer " & Core.Player.Name & " was defeated!"))

                        BattleScreen.TempPVPBattleQuery.Add(BattleScreen.BattleQuery.Count - 1, New EndBattleQueryObject(True))
                End Select
            Else
                Select Case reason
                    Case EndBattleReasons.WinWild
                        Won = True
                        Core.Player.AddPoints(1, "Won against wild Pokémon.")

                        BattleScreen.BattleQuery.Add(New PlayMusicQueryObject("wild_defeat"))
                        ChangeCameraAngel(1, True, BattleScreen)

                        GainEXP(BattleScreen)

                        If BattleScreen.FieldEffects.OwnPayDayCounter > 0 Then
                            Core.Player.Money += BattleScreen.FieldEffects.OwnPayDayCounter
                            BattleScreen.BattleQuery.Add(New TextQueryObject(Core.Player.Name & " picked up $" & BattleScreen.FieldEffects.OwnPayDayCounter & "!"))
                        End If

                        BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                    Case EndBattleReasons.WinTrainer
                        Won = True
                        Core.Player.AddPoints(3, "Won against trainer.")

                        Core.Player.Money += BattleScreen.GetTrainerMoney()

                        BattleScreen.BattleQuery.Add(New PlayMusicQueryObject(BattleScreen.Trainer.GetDefeatMusic()))

                        Dim q As New CameraQueryObject(New Vector3(15, 0, 13), Screen.Camera.Position, 0.03F, 0.03F, -(MathHelper.Pi * 0.5F), Screen.Camera.Yaw, 0.0F, Screen.Camera.Pitch, 0.04F, 0.02F)
                        q.ApplyCurrentCamera = True
                        BattleScreen.BattleQuery.Add(q)

                        BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.Trainer.TrainerType & " " & BattleScreen.Trainer.Name & " was defeated!"))
                        BattleScreen.BattleQuery.Add(New TextQueryObject(BattleScreen.Trainer.OutroMessage))

                        If BattleScreen.GetTrainerMoney() > 0 Then
                            BattleScreen.BattleQuery.Add(New TextQueryObject(Core.Player.Name & " got $" & BattleScreen.GetTrainerMoney() & "!"))
                        End If

                        BattleScreen.BattleQuery.Add(New EndBattleQueryObject(False))
                    Case EndBattleReasons.LooseTrainer, EndBattleReasons.LooseWild
                        Won = False
                        Dim q As New CameraQueryObject(New Vector3(12, 0, 13), Screen.Camera.Position, 0.03F, 0.03F, (MathHelper.Pi * 0.5F), Screen.Camera.Yaw, 0.0F, Screen.Camera.Pitch, 0.02F, 0.02F)
                        q.ApplyCurrentCamera = True
                        BattleScreen.BattleQuery.Add(q)

                        BattleScreen.BattleQuery.Add(New TextQueryObject("You lost the battle!"))

                        BattleScreen.BattleQuery.Add(New EndBattleQueryObject(True))
                End Select
            End If
        End Sub

        Private Sub GainEXP(ByVal BattleScreen As BattleScreen)
            If BattleScreen.IsPVPBattle = False And BattleScreen.CanReceiveEXP = True Then
                Dim expPokemon As New List(Of Integer)
                For Each i As Integer In BattleScreen.ParticipatedPokemon
                    If Core.Player.Pokemons(i).Status <> Pokemon.StatusProblems.Fainted And Core.Player.Pokemons(i).Level < CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) And Core.Player.Pokemons(i).IsEgg() = False Then
                        expPokemon.Add(i)
                    End If
                Next

                For i = 0 To Core.Player.Pokemons.Count - 1
                    If expPokemon.Contains(i) = False And Not Core.Player.Pokemons(i).Item Is Nothing AndAlso Core.Player.Pokemons(i).Item.Name.ToLower() = "exp share" AndAlso Core.Player.Pokemons(i).Status <> Pokemon.StatusProblems.Fainted AndAlso Core.Player.Pokemons(i).IsEgg() = False Then
                        expPokemon.Add(i)
                    End If
                Next

                If expPokemon.Count > 0 Then
                    Me.ChangeCameraAngel(1, True, BattleScreen)
                End If

                For i = 0 To expPokemon.Count - 1
                    Dim PokeIndex As Integer = expPokemon(i)

                    If Core.Player.Pokemons(PokeIndex).Level < CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) Then
                        Dim EXP As Integer = BattleCalculation.GainExp(Core.Player.Pokemons(PokeIndex), BattleScreen, expPokemon)
                        BattleScreen.BattleQuery.Add(New TextQueryObject(Core.Player.Pokemons(PokeIndex).GetDisplayName() & " gained " & EXP & " experience points."))

                        Dim originalLevel As Integer = Core.Player.Pokemons(PokeIndex).Level
                        Dim moveLevel As Integer = originalLevel

                        For e = 1 To EXP
                            Dim oldStats() As Integer
                            With Core.Player.Pokemons(PokeIndex)
                                oldStats = { .MaxHP, .Attack, .Defense, .SpAttack, .SpDefense, .Speed}
                            End With
                            Core.Player.Pokemons(PokeIndex).GetExperience(1, False)

                            If moveLevel < Core.Player.Pokemons(PokeIndex).Level Then
                                moveLevel = Core.Player.Pokemons(PokeIndex).Level

                                Core.Player.AddPoints(CInt(Math.Sqrt(Core.Player.Pokemons(PokeIndex).Level)).Clamp(1, 10), "Leveled up a Pokémon to level " & moveLevel.ToString() & ".")
                                Core.Player.Pokemons(PokeIndex).ChangeFriendShip(Pokemon.FriendShipCauses.LevelUp)

                                Core.Player.Pokemons(PokeIndex).hasLeveledUp = True
                                BattleScreen.BattleQuery.Add(New PlaySoundQueryObject("Battle\exp_max", False))
                                BattleScreen.BattleQuery.Add(New TextQueryObject(Core.Player.Pokemons(PokeIndex).GetDisplayName() & " reached level " & moveLevel & "!"))
                                BattleScreen.BattleQuery.Add(New DisplayLevelUpQueryObject(Core.Player.Pokemons(PokeIndex), oldStats))
                                If Core.Player.Pokemons(PokeIndex).AttackLearns.ContainsKey(moveLevel) = True Then
                                    BattleScreen.BattleQuery.Add(New LearnMovesQueryObject(Core.Player.Pokemons(PokeIndex), Core.Player.Pokemons(PokeIndex).AttackLearns(moveLevel), BattleScreen))
                                End If
                            End If
                        Next
                    End If

                    Core.Player.Pokemons(PokeIndex).GainEffort(BattleScreen.OppPokemon)
                Next
            End If

            BattleScreen.ParticipatedPokemon.Clear()
        End Sub

#End Region

    End Class

End Namespace
