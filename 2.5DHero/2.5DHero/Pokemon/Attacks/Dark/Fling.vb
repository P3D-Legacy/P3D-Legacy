Namespace BattleSystem.Moves.Dark

    Public Class Fling

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Dark)
            Me.ID = 374
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = "Fling"
            Me.Description = "The user flings its held item at the target to attack. This move's power and effects depend on the item."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = True
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End

            Me.AIField1 = AIField.Damage
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Function MoveFailBeforeAttack(Own As Boolean, BattleScreen As BattleScreen) As Boolean
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If Own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.Item Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function GetBasePower(own As Boolean, BattleScreen As BattleScreen) As Integer
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.Item Is Nothing Then
                Return 10
            Else
                Return p.Item.FlingDamage
            End If
        End Function

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If Not p.Item Is Nothing Then
                'Clear prior effect chances to add the chance depending on the item.
                Me.EffectChances.Clear()

                Select Case p.Item.Name.ToLower()
                    Case "flame orb" 'cause burn
                        Me.EffectChances.Add(30)
                        If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                            BattleScreen.Battle.InflictBurn(Not own, own, BattleScreen, "", "move:fling")
                        End If
                    Case "king's rock", "razor fang" 'cause flinch
                        Me.EffectChances.Add(30)
                        If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                            BattleScreen.Battle.InflictFlinch(Not own, own, BattleScreen, "", "move:fling")
                        End If
                    Case "light ball" 'cause paralysis
                        Me.EffectChances.Add(30)
                        If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                            BattleScreen.Battle.InflictParalysis(Not own, own, BattleScreen, "", "move:fling")
                        End If
                    Case "mental herb" 'cures infatuation
                        If p.HasVolatileStatus(Pokemon.VolatileStatus.Infatuation) = True Then
                            Me.EffectChances.Add(10)
                            If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                                p.RemoveVolatileStatus(Pokemon.VolatileStatus.Infatuation)
                                BattleScreen.BattleQuery.Add(New TextQueryObject("Cured the infatuation of " & p.GetDisplayName() & "."))
                            End If
                        End If
                    Case "poison barb" 'causes poison
                        Me.EffectChances.Add(70)
                        If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                            BattleScreen.Battle.InflictPoison(Not own, own, BattleScreen, False, "", "move:fling")
                        End If
                    Case "toxic orb" 'causes bad poison
                        Me.EffectChances.Add(30)
                        If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                            BattleScreen.Battle.InflictPoison(Not own, own, BattleScreen, True, "", "move:fling")
                        End If
                    Case "white herb" 'restores lowered stats
                        Me.EffectChances.Add(10)
                        If Core.Random.Next(0, 100) < Me.GetEffectChance(0, own, BattleScreen) Then
                            p.StatAttack = 0
                            p.StatDefense = 0
                            p.StatSpAttack = 0
                            p.StatSpDefense = 0
                            p.StatSpeed = 0
                            p.Accuracy = 0
                            p.Evasion = 0
                            BattleScreen.BattleQuery.Add(New TextQueryObject("Restored the stats of " & p.GetDisplayName() & "."))
                        End If
                End Select

                p.OriginalItem = CType(p.Item.Copy(), Item)
                p.Item = Nothing
            End If
        End Sub

    End Class

End Namespace