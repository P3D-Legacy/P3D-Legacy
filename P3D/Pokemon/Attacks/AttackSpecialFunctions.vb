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
            Dim functions() As String = Move.GameModeFunction.Split("|")
            Dim Trigger As String = ""
            For i = 0 To functions.Count - 1
                Dim f As String = functions(i)
                Dim fMain As String = f
                Dim fSub As String = ""

                Select Case f.GetSplit(0, ",").ToLower
                    Case "settrigger"
                        Trigger = f.GetSplit(1, ",") & "," & f.GetSplit(2, ",")
                    Case "removetrigger"
                        Trigger = ""
                End Select

                If Trigger <> "" Then
                    Dim Target As Boolean = CBool(Trigger.GetSplit(0, ","))
                    Dim Triggers() As String = Trigger.GetSplit(1, ",").Split(";")
                    Dim Success As Boolean = False
                    For t = 0 To Triggers.Count - 1
                        Select Case Triggers(t).ToLower()
                            Case "burn"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Burn Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.Burn Then
                                        Success = True
                                    End If
                                End If
                            Case "freeze"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Freeze Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.Freeze Then
                                        Success = True
                                    End If
                                End If
                            Case "paralyze"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Paralyzed Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.Paralyzed Then
                                        Success = True
                                    End If
                                End If
                            Case "poison"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Poison Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.Poison Then
                                        Success = True
                                    End If
                                End If
                            Case "toxic", "badpoison"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.BadPoison Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.BadPoison Then
                                        Success = True
                                    End If
                                End If
                            Case "anypoison"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Poison OrElse BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.BadPoison Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.Poison OrElse BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.BadPoison Then
                                        Success = True
                                    End If
                                End If
                            Case "sleep"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status = Pokemon.StatusProblems.Sleep Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status = Pokemon.StatusProblems.Sleep Then
                                        Success = True
                                    End If
                                End If
                            Case "noburn"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status <> Pokemon.StatusProblems.Burn Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status <> Pokemon.StatusProblems.Burn Then
                                        Success = True
                                    End If
                                End If
                            Case "nofreeze"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status <> Pokemon.StatusProblems.Freeze Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status <> Pokemon.StatusProblems.Freeze Then
                                        Success = True
                                    End If
                                End If
                            Case "noparalyze"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status <> Pokemon.StatusProblems.Paralyzed Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status <> Pokemon.StatusProblems.Paralyzed Then
                                        Success = True
                                    End If
                                End If
                            Case "nopoison"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status <> Pokemon.StatusProblems.Poison Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status <> Pokemon.StatusProblems.Poison Then
                                        Success = True
                                    End If
                                End If
                            Case "notoxic", "nobadpoison"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status <> Pokemon.StatusProblems.BadPoison Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status <> Pokemon.StatusProblems.BadPoison Then
                                        Success = True
                                    End If
                                End If
                            Case "nopoison"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status <> Pokemon.StatusProblems.Poison AndAlso BattleScreen.OppPokemon.Status <> Pokemon.StatusProblems.BadPoison Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status <> Pokemon.StatusProblems.Poison AndAlso BattleScreen.OwnPokemon.Status <> Pokemon.StatusProblems.BadPoison Then
                                        Success = True
                                    End If
                                End If
                            Case "nosleep"
                                If Target = True Then
                                    If BattleScreen.OppPokemon.Status <> Pokemon.StatusProblems.Sleep Then
                                        Success = True
                                    End If
                                Else
                                    If BattleScreen.OwnPokemon.Status <> Pokemon.StatusProblems.Sleep Then
                                        Success = True
                                    End If
                                End If
                        End Select
                    Next
                    If Success = True Then
                        Trigger = ""
                    End If
                Else
                    If f.Contains(",") = True Then
                        fMain = f.GetSplit(0, ",")
                        fSub = f.GetSplit(1, ",")
                        Select Case fMain.ToLower()
                            Case "cameraangle"
                                Dim Direction As Integer = CInt(fSub)
                                Select Case Direction
                                    Case 0
                                        BattleScreen.Battle.ChangeCameraAngle(0, own, BattleScreen)
                                    Case 1
                                        BattleScreen.Battle.ChangeCameraAngle(1, True, BattleScreen)
                                    Case 2
                                        BattleScreen.Battle.ChangeCameraAngle(2, True, BattleScreen)
                                End Select
                            Case "message", "text"
                                Dim OppPokemon As Pokemon = BattleScreen.OppPokemon
                                Dim OwnPokemon As Pokemon = BattleScreen.OwnPokemon
                                If own = False Then
                                    OwnPokemon = BattleScreen.OppPokemon
                                    OppPokemon = BattleScreen.OwnPokemon
                                End If

                                fSub = Localization.GetString(fSub, fSub).Replace("[OPPNAME]", OppPokemon.GetDisplayName).Replace("[OWNNAME]", OwnPokemon.GetDisplayName)
                                BattleScreen.BattleQuery.Add(New TextQueryObject(fSub))
                            Case "raisestat", "increasestat"
                                Dim Stat As String = f.GetSplit(1, ",")
                                Dim Target As Boolean = own
                                Dim Message As String = ""
                                Dim RaiseAmount As Integer = 1

                                If f.Split(CChar(",")).Count > 2 Then
                                    Target = CBool(f.GetSplit(2, ","))
                                    If f.Split(CChar(",")).Count > 3 Then
                                        Message = f.GetSplit(3, ",")
                                        If f.Split(CChar(",")).Count > 4 Then
                                            If CInt(f.GetSplit(4, ",")) > 0 Then
                                                RaiseAmount = CInt(f.GetSplit(4, ","))
                                            End If
                                        End If
                                    End If
                                End If
                                BattleScreen.Battle.RaiseStat(Target, own, BattleScreen, Stat, RaiseAmount, Message, "move:" & Move.Name, True)
                            Case "lowerstat", "decreasestat"
                                Dim Stat As String = f.GetSplit(1, ",")
                                Dim Message As String = ""
                                Dim Target As Boolean = own
                                Dim LowerAmount As Integer = 1
                                If f.Split(CChar(",")).Count > 2 Then
                                    Target = CBool(f.GetSplit(2, ","))
                                    If f.Split(CChar(",")).Count > 3 Then
                                        Message = f.GetSplit(3, ",")
                                        If f.Split(CChar(",")).Count > 4 Then
                                            If CInt(f.GetSplit(4, ",")) > 0 Then
                                                LowerAmount = CInt(f.GetSplit(4, ","))
                                            End If
                                        End If
                                    End If
                                End If
                                BattleScreen.Battle.LowerStat(Target, own, BattleScreen, Stat, LowerAmount, Message, "move:" & Move.Name, True)
                            Case "reducehp", "drainhp", "damage"
                                Dim Target As Boolean = CBool(f.GetSplit(1, ","))
                                Dim HPAmount As Integer = 0
                                Dim Message As String = ""

                                If f.Split(CChar(",")).Count > 2 Then
                                    HPAmount = CInt(f.GetSplit(2, ","))
                                    If f.Split(CChar(",")).Count > 3 Then
                                        Message = f.GetSplit(3, ",")
                                    End If
                                End If
                                BattleScreen.Battle.ReduceHP(HPAmount, Target, own, BattleScreen, Message, "move:" & Move.Name.ToLower)
                            Case "gainhp", "increasehp", "heal"
                                Dim Target As Boolean = CBool(f.GetSplit(1, ","))
                                Dim HPAmount As Integer = 0
                                Dim Message As String = ""

                                If f.Split(CChar(",")).Count > 2 Then
                                    HPAmount = CInt(f.GetSplit(2, ","))
                                    If f.Split(CChar(",")).Count > 3 Then
                                        Message = f.GetSplit(3, ",")
                                    End If
                                End If
                                BattleScreen.Battle.GainHP(HPAmount, Target, own, BattleScreen, Message, "move:" & Move.Name.ToLower)
                            Case "endbattle"
                                Dim Blackout As Boolean = False

                                If f.Split(CChar(",")).Count > 2 Then
                                    Blackout = CBool(fSub)
                                End If
                                BattleScreen.BattleQuery.Add(New EndBattleQueryObject(Blackout))
                            Case "faint"
                                Dim Target As Boolean = CBool(f.GetSplit(1, ","))
                                Dim Message As String = ""
                                Dim NextPokemonIndex As Integer = -1

                                Dim OppPokemon As Pokemon = BattleScreen.OppPokemon
                                Dim OwnPokemon As Pokemon = BattleScreen.OwnPokemon
                                If Target = False Then
                                    OwnPokemon = BattleScreen.OppPokemon
                                    OppPokemon = BattleScreen.OwnPokemon
                                End If

                                If Target = True Then
                                    If f.Split(CChar(",")).Count > 3 Then
                                        If f.GetSplit(3, ",").StartsWith("~+") Then
                                            NextPokemonIndex = BattleScreen.OppPokemonIndex + CInt(f.GetSplit(3, ",").Remove(0, 2))
                                        End If
                                        If f.GetSplit(3, ",").StartsWith("~-") Then
                                            NextPokemonIndex = BattleScreen.OppPokemonIndex - CInt(f.GetSplit(3, ",").Remove(0, 2))
                                        End If
                                    End If
                                End If
                                BattleScreen.NextPokemonIndex = NextPokemonIndex

                                If f.Split(CChar(",")).Count > 2 Then
                                    Message = f.GetSplit(2, ",").Replace("[OPPNAME]", OppPokemon.GetDisplayName).Replace("[OWNNAME]", OwnPokemon.GetDisplayName)
                                End If
                                BattleScreen.Battle.FaintPokemon(Not Target, BattleScreen, Message)
                            Case "switch"
                                Dim Target As Boolean = CBool(f.GetSplit(1, ","))
                                Dim SwitchTo As Integer = 0
                                Dim Message As String = ""

                                If f.Split(CChar(",")).Count > 3 Then
                                    Message = f.GetSplit(3, ",")
                                End If
                                If Target = True Then
                                    If f.GetSplit(2, ",").StartsWith("~+") Then
                                        SwitchTo = BattleScreen.OppPokemonIndex + CInt(f.GetSplit(2, ",").Remove(0, 2))
                                    End If
                                    If f.GetSplit(2, ",").StartsWith("~-") Then
                                        SwitchTo = BattleScreen.OppPokemonIndex - CInt(f.GetSplit(2, ",").Remove(0, 2))
                                    End If
                                    BattleScreen.Battle.SwitchOutOpp(BattleScreen, SwitchTo, Message)
                                Else
                                    If f.GetSplit(2, ",").StartsWith("~+") Then
                                        SwitchTo = BattleScreen.OwnPokemonIndex + CInt(f.GetSplit(2, ",").Remove(0, 2))
                                    End If
                                    If f.GetSplit(2, ",").StartsWith("~-") Then
                                        SwitchTo = BattleScreen.OwnPokemonIndex - CInt(f.GetSplit(2, ",").Remove(0, 2))
                                    End If
                                    BattleScreen.Battle.SwitchOutOwn(BattleScreen, SwitchTo, -1, Message)
                                End If
                            Case Else
                                fSub = CInt(f.GetSplit(1, ",")).Clamp(0, 100).ToString
                        End Select
                    Else
                        Select Case f.ToLower()
                            Case "endround"
                                Dim cq1 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, True, 16)
                                Dim cq2 As ScreenFadeQueryObject = New ScreenFadeQueryObject(ScreenFadeQueryObject.FadeTypes.Vertical, Color.Black, False, 16)
                                cq2.PassThis = True
                                BattleScreen.BattleQuery.AddRange({cq1, cq2})
                                BattleScreen.Battle.StartRound(BattleScreen)
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
                End If
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