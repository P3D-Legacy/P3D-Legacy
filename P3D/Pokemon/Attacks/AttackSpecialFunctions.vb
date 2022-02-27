Namespace BattleSystem

    ''' <summary>
    ''' A class to execute GameMode attack functions.
    ''' </summary>
    Public Class AttackSpecialFunctions

        ''' <summary>
        ''' Executes the attack function(s).
        ''' </summary>
        ''' <param name="Move">The move containing the attack function.</param>
        ''' <param name="own">Own toggle.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Shared Sub ExecuteMoveHitsFunction(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            Dim functions() As String = Move.GameModeFunction.Split(CChar("|"))
            For i = 0 To functions.Count - 1
                Dim f As String = functions(i)
                Dim fMain As String = f
                Dim fSub As String = ""
                If f.Contains(",") = True Then
                    fMain = f.GetSplit(0, ",")
                    Select Case fMain.ToLower()
                        Case "message"
                            fSub = Localization.GetString(f.GetSplit(1, ","), f.GetSplit(1, ","))
                            BattleScreen.BattleQuery.Add(New TextQueryObject(fSub))
                        Case "reducehp", "drainhp", "damage"
                            Dim HPAmount As Integer = CInt(fSub.GetSplit(0, ","))
                            Dim Message As String = ""
                            Dim Target As Boolean = True
                            If fSub.Split(CChar(",")).Count > 1 Then
                                Target = CBool(fSub.GetSplit(1, ","))
                                If fSub.Split(CChar(",")).Count > 2 Then
                                    Message = fSub.GetSplit(2, ",")
                                End If
                            End If
                            BattleScreen.Battle.ReduceHP(HPAmount, Not Target, Target, BattleScreen, Message, Move.Name.ToLower)
                        Case "gainhp", "increasehp", "heal"
                            Dim HPAmount As Integer = CInt(fSub.GetSplit(0, ","))
                            Dim Message As String = ""
                            Dim Target As Boolean = True
                            If fSub.Split(CChar(",")).Count > 1 Then
                                Target = CBool(fSub.GetSplit(1, ","))
                                If fSub.Split(CChar(",")).Count > 2 Then
                                    Message = fSub.GetSplit(2, ",")
                                End If
                            End If
                            BattleScreen.Battle.GainHP(HPAmount, Target, Target, BattleScreen, Message, Move.Name.ToLower)
                        Case "endbattle"
                            Dim Blackout As Boolean = False
                            If fSub.Split(CChar(",")).Count > 1 Then
                                Blackout = CBool(fSub.GetSplit(1, ","))
                            End If
                            BattleScreen.EndBattle(Blackout)
                        Case Else
                            fSub = CInt(f.GetSplit(1, ",")).Clamp(0, 100).ToString
                    End Select
                Else
                    Select Case f.ToLower()
                        Case "endbattle"
                            BattleScreen.EndBattle(False)
                        Case "freeze"
                            fSub = "15"
                        Case "poison"
                            fSub = "40"
                        Case "toxic", "badpoison"
                            fSub = "25"
                        Case Else
                            fSub = "30"
                    End Select
                End If

                Select Case fMain.ToLower()
                    Case "paralyze"
                        Paralyze(Move, own, BattleScreen, CInt(fSub))
                    Case "poison"
                        Poison(Move, own, BattleScreen, CInt(fSub))
                    Case "toxic", "badpoison"
                        BadPoison(Move, own, BattleScreen, CInt(fSub))
                    Case "burn"
                        Burn(Move, own, BattleScreen, CInt(fSub))
                    Case "freeze"
                        Freeze(Move, own, BattleScreen, CInt(fSub))
                    Case "sleep"
                        Sleep(Move, own, BattleScreen, CInt(fSub))
                End Select
                i += 1
            Next
        End Sub

        Private Shared Function GetEffectChanceResult(ByVal move As Attack, ByVal chance As Integer) As Boolean
            If move.Category = Attack.Categories.Special Then
                Return True
            Else
                Return Core.Random.Next(0, 100) <= chance
            End If
        End Function

        Private Shared Sub Paralyze(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen, Chance As Integer)
            If GetEffectChanceResult(Move, Chance) = True Then
                If BattleScreen.Battle.InflictParalysis(Not own, own, BattleScreen, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

        Private Shared Sub Burn(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen, Chance As Integer)
            If GetEffectChanceResult(Move, Chance) = True Then
                If BattleScreen.Battle.InflictBurn(Not own, own, BattleScreen, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

        Private Shared Sub Sleep(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen, Chance As Integer)
            If GetEffectChanceResult(Move, Chance) = True Then
                If BattleScreen.Battle.InflictSleep(Not own, own, BattleScreen, -1, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

        Private Shared Sub Freeze(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen, Chance As Integer)
            If GetEffectChanceResult(Move, Chance) = True Then
                If BattleScreen.Battle.InflictFreeze(Not own, own, BattleScreen, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

        Private Shared Sub Poison(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen, Chance As Integer)
            If GetEffectChanceResult(Move, Chance) = True Then
                If BattleScreen.Battle.InflictPoison(Not own, own, BattleScreen, False, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

        Private Shared Sub BadPoison(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen, Chance As Integer)
            If GetEffectChanceResult(Move, Chance) = True Then
                If BattleScreen.Battle.InflictPoison(Not own, own, BattleScreen, True, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace