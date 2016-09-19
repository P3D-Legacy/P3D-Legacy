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
        Public Shared Sub ExecuteAttackFunction(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            Dim functions() As String = Move.GameModeFunction.Split(CChar(","))

            For Each f As String In functions
                Select Case f.ToLower()
                    Case "paralyze"
                        Paralyze(Move, own, BattleScreen)
                    Case "poison"
                        Poison(Move, own, BattleScreen)
                    Case "toxic", "badpoison"
                        BadPoison(Move, own, BattleScreen)
                    Case "burn"
                        Burn(Move, own, BattleScreen)
                    Case "freeze"
                        Burn(Move, own, BattleScreen)
                    Case "sleep"
                        Sleep(Move, own, BattleScreen)
                End Select
            Next
        End Sub

        Private Shared Function GetEffectChanceResult(ByVal move As Attack, ByVal chance As Integer) As Boolean
            If move.Category = Attack.Categories.Special Then
                Return True
            Else
                Return Core.Random.Next(0, 100) < chance
            End If
        End Function

        Private Shared Sub Paralyze(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If GetEffectChanceResult(Move, 30) = True Then
                If BattleScreen.Battle.InflictParalysis(Not own, own, BattleScreen, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

        Private Shared Sub Burn(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If GetEffectChanceResult(Move, 30) = True Then
                If BattleScreen.Battle.InflictBurn(Not own, own, BattleScreen, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

        Private Shared Sub Sleep(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If GetEffectChanceResult(Move, 30) = True Then
                If BattleScreen.Battle.InflictSleep(Not own, own, BattleScreen, -1, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

        Private Shared Sub Freeze(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If GetEffectChanceResult(Move, 15) = True Then
                If BattleScreen.Battle.InflictFreeze(Not own, own, BattleScreen, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

        Private Shared Sub Poison(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If GetEffectChanceResult(Move, 40) = True Then
                If BattleScreen.Battle.InflictPoison(Not own, own, BattleScreen, False, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

        Private Shared Sub BadPoison(ByVal Move As Attack, ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If GetEffectChanceResult(Move, 25) = True Then
                If BattleScreen.Battle.InflictPoison(Not own, own, BattleScreen, True, "", "move:" & Move.Name.ToLower()) = False Then
                    If Move.Category = Attack.Categories.Status Then BattleScreen.BattleQuery.Add(New TextQueryObject(Move.Name & " failed!"))
                End If
            End If
        End Sub

    End Class

End Namespace