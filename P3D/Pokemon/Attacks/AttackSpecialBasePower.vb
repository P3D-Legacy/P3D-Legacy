Namespace BattleSystem

    ''' <summary>
    ''' A class to execute GameMode attack functions.
    ''' </summary>
    Public Class AttackSpecialBasePower

        ''' <summary>
        ''' Executes the attack function(s).
        ''' </summary>
        ''' <param name="Move">The move containing the attack function.</param>
        ''' <param name="own">Own toggle.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Shared Function GetGameModeBasePower(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            If Move.GameModeBasePower <> "" Then
                Dim basePowerCalcs() As String = Move.GameModeBasePower.Split("|")
                Dim applyMultiplier As Single = 1
                For i = 0 To basePowerCalcs.Count - 1
                    Dim b As String = basePowerCalcs(i)
                    Dim bMain As String = b.Remove(b.IndexOf(","))
                    Dim bSub As String = b.Remove(0, b.IndexOf(",") + 1)

                    Select Case bMain.ToLower
                        Case "status"
                            Dim Target As Boolean = own
                            Dim Status As String = bSub.GetSplit(0, ",")
                            Dim Multiplier As Single = 1
                            If bSub.Split(",").Count > 1 Then
                                Multiplier = CSng(bSub.GetSplit(1, ",").InsertDecSeparator)
                                If bSub.Split(",").Count > 2 Then
                                    If own = True Then
                                        Target = Not CBool(bSub.GetSplit(2, ","))
                                    Else
                                        Target = CBool(bSub.GetSplit(2, ","))
                                    End If

                                End If
                            End If

                            Dim Success As Boolean = False

                            Select Case Status.ToLower()
                                Case "confuse"
                                    If Target = True Then
                                        If BattleScreen.OppPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                                            Success = True
                                        End If
                                    Else
                                        If BattleScreen.OwnPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = True Then
                                            Success = True
                                        End If
                                    End If
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
                                Case "noconfuse"
                                    If Target = True Then
                                        If BattleScreen.OppPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = False Then
                                            Success = True
                                        End If
                                    Else
                                        If BattleScreen.OwnPokemon.HasVolatileStatus(Pokemon.VolatileStatus.Confusion) = False Then
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
                            If Success = True Then
                                applyMultiplier *= Multiplier
                            End If
                        Case "underground"
                            Dim Target As Boolean = own
                            Dim Multiplier As Single = 1
                            Dim Success As Boolean = False
                            If bSub.Split(",").Count > 0 Then
                                Multiplier = CSng(bSub.GetSplit(0, ",").InsertDecSeparator)
                                If bSub.Split(",").Count > 1 Then
                                    If own = True Then
                                        Target = Not CBool(bSub.GetSplit(1, ","))
                                    Else
                                        Target = CBool(bSub.GetSplit(1, ","))
                                    End If
                                End If
                            End If
                            If Target = True Then
                                If BattleScreen.FieldEffects.OppDigCounter > 0 Then
                                    Success = True
                                End If
                            Else
                                If BattleScreen.FieldEffects.OwnDigCounter > 0 Then
                                    Success = True
                                End If
                            End If
                            If Success = True Then
                                applyMultiplier *= Multiplier
                            End If
                        Case "inmidair"
                            Dim Target As Boolean = own
                            Dim Multiplier As Single = 1
                            Dim Success As Boolean = False
                            If bSub.Split(",").Count > 0 Then
                                Multiplier = CSng(bSub.GetSplit(0, ",").InsertDecSeparator)
                                If own = True Then
                                    Target = Not CBool(bSub.GetSplit(1, ","))
                                Else
                                    Target = CBool(bSub.GetSplit(1, ","))
                                End If
                            End If
                            If Target = True Then
                                If BattleScreen.FieldEffects.OppFlyCounter > 0 Then
                                    Success = True
                                End If
                                If BattleScreen.FieldEffects.OppBounceCounter > 0 Then
                                    Success = True
                                End If
                                If BattleScreen.FieldEffects.OppSkyDropCounter > 0 Then
                                    Success = True
                                End If
                            Else
                                If BattleScreen.FieldEffects.OwnFlyCounter > 0 Then
                                    Success = True
                                End If
                                If BattleScreen.FieldEffects.OwnBounceCounter > 0 Then
                                    Success = True
                                End If
                                If BattleScreen.FieldEffects.OwnSkyDropCounter > 0 Then
                                    Success = True
                                End If
                            End If
                            If Success = True Then
                                applyMultiplier *= Multiplier
                            End If
                        Case "underwater"
                            Dim Target As Boolean = own
                            Dim Multiplier As Single = 1
                            Dim Success As Boolean = False
                            If bSub.Split(",").Count > 0 Then
                                Multiplier = CSng(bSub.GetSplit(0, ",").InsertDecSeparator)
                                If own = True Then
                                    Target = Not CBool(bSub.GetSplit(1, ","))
                                Else
                                    Target = CBool(bSub.GetSplit(1, ","))
                                End If
                            End If
                            If Target = True Then
                                If BattleScreen.FieldEffects.OppDiveCounter > 0 Then
                                    Success = True
                                End If
                            Else
                                If BattleScreen.FieldEffects.OwnDiveCounter > 0 Then
                                    Success = True
                                End If
                            End If
                            If Success = True Then
                                applyMultiplier *= Multiplier
                            End If
                    End Select
                Next
                Return CInt(Move.Power * applyMultiplier)
            Else
                Return Move.Power
            End If
        End Function

    End Class

End Namespace