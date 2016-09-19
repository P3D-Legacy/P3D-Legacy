Namespace BattleSystem

    ''' <summary>
    ''' Represents a Pokémon's move.
    ''' </summary>
    Public Class Attack

        Implements ICopyAble

#Region "Fields"

        Public Const MOVE_COUNT As Integer = 560

        Public Enum Categories
            Physical
            Special
            Status
        End Enum

        Public Enum ContestCategories
            Tough
            Smart
            Beauty
            Cool
            Cute
        End Enum

        ''' <summary>
        ''' The target for an attack.
        ''' </summary>
        Public Enum Targets
            OneAdjacentTarget 'One adjacent target, excluding itself.
            OneAdjacentFoe 'One adjacent foe.
            OneAdjacentAlly 'One adjacent ally, excluding itself.

            OneTarget 'One target, excluding itself.
            OneFoe 'One Foe.
            OneAlly 'One ally, excluding itself.

            Self 'Only self

            AllAdjacentTargets 'All adjacent targets, exluding itself
            AllAdjacentFoes 'All adjacent foes
            AllAdjacentAllies 'All adjacent allies, excluding itself.

            AllTargets 'All Targets, excluding itself.
            AllFoes 'All Foes
            AllAllies 'All allies, excluding itself.

            All 'All Pokémon, including itself
        End Enum

        Public Enum AIField
            [Nothing]

            Damage

            Poison
            Burn
            Paralysis
            Sleep
            Freeze
            Confusion

            ConfuseOwn

            CanPoison
            CanBurn
            CanParalyse
            CanSleep
            CanFreeze
            CanConfuse

            RaiseAttack
            RaiseDefense
            RaiseSpAttack
            RaiseSpDefense
            RaiseSpeed
            RaiseAccuracy
            RaiseEvasion

            LowerAttack
            LowerDefense
            LowerSpAttack
            LowerSpDefense
            LowerSpeed
            LowerAccuracy
            LowerEvasion

            CanRaiseAttack
            CanRaiseDefense
            CanRaiseSpAttack
            CanRaiseSpDefense
            CanRaiseSpeed
            CanRaiseAccuracy
            CanRauseEvasion

            CanLowerAttack
            CanLowerDefense
            CanLowerSpAttack
            CanLowerSpDefense
            CanLowerSpeed
            CanLowerAccuracy
            CanLowerEvasion

            Flinch
            CanFlinch

            Infatuation

            Trap
            OHKO
            MultiTurn
            Recoil

            Healing
            CureStatus
            Support
            Recharge
            HighPriority
            Absorbing
            Selfdestruct
            ThrawOut
            CannotMiss
            RemoveReflectLightscreen
        End Enum

        '#Definitions
        Public Type As Element = New Element("Normal")

        Public Property ID() As Integer
            Get
                Return Me._ID
            End Get
            Set(value As Integer)
                Me._ID = value
            End Set
        End Property

        Public Property Power() As Integer
            Get
                Return Me._power
            End Get
            Set(value As Integer)
                Me._power = value
            End Set
        End Property

        Public Property Accuracy() As Integer
            Get
                Return Me._accuracy
            End Get
            Set(value As Integer)
                Me._accuracy = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(value As String)
                Me._name = value
            End Set
        End Property

        Private _ID As Integer = 1
        Public OriginalID As Integer = 1 'Original MoveID, remove when not needed anymore. This stores the original move ID when the move isn't programmed yet.
        Public IsDefaultMove As Boolean = False 'if Pound gets loaded instead of the correct move, this is true.

        Public GameModeFunction As String = "" 'A GameMode can specify a pre defined function for a move.
        Public IsGameModeMove As Boolean = False

        Private _power As Integer = 40
        Private _accuracy As Integer = 100
        Private _name As String = "Pound"

        Public OriginalPP As Integer = 35
        Public Category As Categories = Categories.Physical
        Public ContestCategory As ContestCategories = ContestCategories.Tough
        Public Description As String = "Pounds with forelegs or tail."
        Public CriticalChance As Integer = 1
        Public IsHMMove As Boolean = False
        Public Target As Targets = Targets.OneAdjacentTarget
        Public Priority As Integer = 0
        Public TimesToAttack As Integer = 1
        Public EffectChances As New List(Of Integer)
        '#End

        '#SpecialDefinitions
        Public MakesContact As Boolean = True
        Public ProtectAffected As Boolean = True
        Public MagicCoatAffected As Boolean = False
        Public SnatchAffected As Boolean = False
        Public MirrorMoveAffected As Boolean = True
        Public KingsrockAffected As Boolean = True
        Public CounterAffected As Boolean = True
        Public DisabledWhileGravity As Boolean = False
        Public UseEffectiveness As Boolean = True
        Public IsHealingMove As Boolean = False
        Public RemovesFrozen As Boolean = False
        Public IsRecoilMove As Boolean = False
        Public IsPunchingMove As Boolean = False
        Public ImmunityAffected As Boolean = True
        Public IsDamagingMove As Boolean = True
        Public IsProtectMove As Boolean = False
        Public IsSoundMove As Boolean = False
        Public HasSecondaryEffect As Boolean = False
        Public IsAffectedBySubstitute As Boolean = True
        Public IsOneHitKOMove As Boolean = False
        Public IsWonderGuardAffected As Boolean = True
        Public UseAccEvasion As Boolean = True
        Public CanHitInMidAir As Boolean = False
        Public CanHitUnderground As Boolean = False
        Public CanHitUnderwater As Boolean = False
        Public CanHitSleeping As Boolean = True
        Public CanGainSTAB As Boolean = True
        Public IsPowderMove As Boolean = False
        Public IsTrappingMove As Boolean = False
        Public IsPulseMove As Boolean = False
        Public IsBulletMove As Boolean = False
        Public IsJawMove As Boolean = False
        Public UseOppDefense As Boolean = True
        Public UseOppEvasion As Boolean = True

        Public FocusOppPokemon As Boolean = True
        '#End

        Public CurrentPP As Integer = 0
        Public MaxPP As Integer = 0
        Public Disabled As Integer = 0

        Public AIField1 As AIField = AIField.Damage
        Public AIField2 As AIField = AIField.Nothing
        Public AIField3 As AIField = AIField.Nothing

#End Region

        ''' <summary>
        ''' Returns a new instance of AttackV2 based on the input ID.
        ''' </summary>
        ''' <param name="ID">The ID of the Move to return.</param>
        Public Shared Function GetAttackByID(ByVal ID As Integer) As Attack
            Dim returnMoveFunc = AttackList.Attacks(ID)
            Dim returnMove As Attack

            If returnMoveFunc IsNot Nothing
                returnMove = returnMoveFunc()
            End If

            If GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                returnMove = New Moves.Special.TheDerpMove()
            Else
                returnMove = New Moves.Normal.Pound()
            End If

            'Try to load a GameMode move.
            Dim gameModeMove As Attack = GameModeAttackLoader.GetAttackByID(ID)
            If GameModeManager.ActiveGameMode IsNot Nothing AndAlso gameModeMove IsNot Nothing AndAlso GameModeManager.ActiveGameMode.IsDefaultGamemode = False Then
                returnMove = CType(gameModeMove.Copy(), Attack)
            Else
                returnMove = New Moves.Normal.Pound()
                returnMove.IsDefaultMove = True
            End If

            returnMove.OriginalID = ID
            Return returnMove
        End Function

        Public Function GetEffectChance(ByVal i As Integer, ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Dim chance As Integer = Me.EffectChances(i)

            If Me.HasSecondaryEffect = True Then
                Dim p As Pokemon = BattleScreen.OwnPokemon
                If own = False Then
                    p = BattleScreen.OppPokemon
                End If

                If p.Ability.Name.ToLower() = "serene grace" Then
                    chance *= 2
                End If

                Dim waterPledge As Integer = BattleScreen.FieldEffects.OwnWaterPledge
                If own = False Then
                    waterPledge = BattleScreen.FieldEffects.OppWaterPledge
                End If
                If waterPledge > 0 Then
                    chance *= 2
                End If
            End If

            chance = chance.Clamp(0, 100)

            Return chance
        End Function

        ''' <summary>
        ''' Gets called prior to using the attack.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub PreAttack(ByVal Own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING HERE
        End Sub

        ''' <summary>
        ''' If the move fails prior to using it. Return True for failing.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function MoveFailBeforeAttack(ByVal Own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            'DO NOTHING HERE
            Return False
        End Function

        ''' <summary>
        ''' Returns the BasePower of this move. Defaults to Power.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetBasePower(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Return Me.Power
        End Function

        ''' <summary>
        ''' Returns the calculated damage of this move.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetDamage(ByVal Critical As Boolean, ByVal Own As Boolean, ByVal targetPokemon As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Return BattleCalculation.CalculateDamage(Me, Critical, Own, targetPokemon, BattleScreen)
        End Function

        ''' <summary>
        ''' Returns how many times this move is getting used in a row.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetTimesToAttack(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Return Me.TimesToAttack
        End Function

        ''' <summary>
        ''' Event that occurs when the move connects.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveHits(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If Me.IsGameModeMove = True Then
                AttackSpecialFunctions.ExecuteAttackFunction(Me, own, BattleScreen)
            Else
                'DO NOTHING HERE (will do secondary effect if moves overrides it)
            End If
        End Sub

        Public Overridable Sub MoveRecoil(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING HERE (will do recoil if moves overrides it)
        End Sub

        ''' <summary>
        ''' Event that occurs when the move misses its target.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveMisses(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING HERE
        End Sub

        ''' <summary>
        ''' If the move gets blocked by a protection move.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveProtectedDetected(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING HERE
        End Sub

        ''' <summary>
        ''' Event that occurs when the move has no effect on the target.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveHasNoEffect(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING HERE
        End Sub

        ''' <summary>
        ''' Returns the type of the move. Defaults to the Type field.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetAttackType(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Element
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.Ability.Name.ToLower() = "normalize" Then
                Return New Element(Element.Types.Normal)
            End If

            If Me.Type.Type = Element.Types.Normal Then
                If p.Ability.Name.ToLower() = "pixilate" Then
                    Return New Element(Element.Types.Fairy)
                End If
                If p.Ability.Name.ToLower() = "refrigerate" Then
                    Return New Element(Element.Types.Ice)
                End If
                If p.Ability.Name.ToLower() = "aerilate" Then
                    Return New Element(Element.Types.Flying)
                End If
            End If

            Return Me.Type
        End Function

        ''' <summary>
        ''' Returns the accuracy of this move.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetAccuracy(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Return Me.Accuracy
        End Function

        ''' <summary>
        ''' If the PP of this move should get deducted when using it. Default is True.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function DeductPP(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            Return True
        End Function

        ''' <summary>
        ''' If the Accuracy and Evasion parameters of Pokémon and moves should get used for this attack.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetUseAccEvasion(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            Return Me.UseAccEvasion
        End Function

        ''' <summary>
        ''' Event that occurs when the move gets selected in the menu.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveSelected(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING
        End Sub

        ''' <summary>
        ''' Event that occurs before this move deals damage.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub BeforeDealingDamage(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING
        End Sub

        ''' <summary>
        ''' Event that occurs when this move's damage gets absorbed by a substitute.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub AbsorbedBySubstitute(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING
        End Sub

        ''' <summary>
        ''' Event that occurs when the Soundproof ability blocks this move.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveFailsSoundproof(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING
        End Sub

        ''' <summary>
        ''' Returns the attack stat of a Pokémon (Physical or Special).
        ''' </summary>
        ''' <param name="p">The Pokémon that used the move.</param>
        Public Overridable Function GetUseAttackStat(ByVal p As Pokemon) As Integer
            If Me.Category = Categories.Physical Then
                Return p.Attack
            Else
                Return p.SpAttack
            End If
        End Function

        ''' <summary>
        ''' Returns the defense stat of a Pokémon (Physical or Special).
        ''' </summary>
        ''' <param name="p">The Pokémon that used the move.</param>
        Public Overridable Function GetUseDefenseStat(ByVal p As Pokemon) As Integer
            If Me.Category = Categories.Physical Then
                Return p.Defense
            Else
                Return p.SpDefense
            End If
        End Function

        ''' <summary>
        ''' If the AI is allowed to use this move.
        ''' </summary>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Function AIUseMove(ByVal BattleScreen As BattleScreen) As Boolean
            Return True
        End Function

#Region "Animation"

        Public Sub UserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen)
            If Core.Player.ShowBattleAnimations = 1 Then
                Me.InternalUserPokemonMoveAnimation(BattleScreen)
            End If
        End Sub

        Public Overridable Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen)
            'Override this method in the attack class to insert the move animation query objects into the queue.
        End Sub

        Public Sub OpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen)
            If Core.Player.ShowBattleAnimations = 1 Then
                Me.InternalOpponentPokemonMoveAnimation(BattleScreen)
            End If
        End Sub

        Public Overridable Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen)
            'Override this method in the attack class to insert the move animation query objects into the queue.
        End Sub

#End Region

        ''' <summary>
        ''' Returns a copy of this move.
        ''' </summary>
        Public Function Copy() As Object Implements ICopyAble.Copy
            Dim m As Attack

            If Me.IsGameModeMove = True Then
                m = GameModeAttackLoader.GetAttackByID(Me.ID)
            Else
                m = GetAttackByID(Me.ID)
            End If

            'Set definition properties:
            m.OriginalPP = Me.OriginalPP
            m.CurrentPP = Me.CurrentPP
            m.MaxPP = Me.MaxPP
            m.OriginalID = Me.OriginalID

            Return m
        End Function

        ''' <summary>
        ''' Builds an instance of AttackV2 with PP and MaxPP set.
        ''' </summary>
        ''' <param name="InputData">Data in the format "ID,MaxPP,CurrentPP"</param>
        Public Shared Function ConvertStringToAttack(ByVal InputData As String) As Attack
            If InputData <> "" Then
                Dim Data() As String = InputData.Split(CChar(","))
                Dim a As Attack = GetAttackByID(CInt(Data(0)))

                If Not a Is Nothing Then
                    a.MaxPP = CInt(Data(1))
                    a.CurrentPP = CInt(Data(2))
                End If

                Return a
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Raises the PP of the move by one stage.
        ''' </summary>
        Public Function RaisePP() As Boolean
            Select Case Me.OriginalPP
                Case 5
                    Select Case Me.MaxPP
                        Case 5, 6, 7
                            Me.CurrentPP += 1
                            Me.MaxPP += 1
                            Return True
                    End Select
                Case 10
                    Select Case Me.MaxPP
                        Case 10, 12, 14
                            Me.CurrentPP += 2
                            Me.MaxPP += 2
                            Return True
                    End Select
                Case 15
                    Select Case Me.MaxPP
                        Case 15, 18, 21
                            Me.CurrentPP += 3
                            Me.MaxPP += 3
                            Return True
                    End Select
                Case 20
                    Select Case Me.MaxPP
                        Case 20, 24, 28
                            Me.CurrentPP += 4
                            Me.MaxPP += 4
                            Return True
                    End Select
                Case 25
                    Select Case Me.MaxPP
                        Case 25, 30, 35
                            Me.CurrentPP += 5
                            Me.MaxPP += 5
                            Return True
                    End Select
                Case 30
                    Select Case Me.MaxPP
                        Case 30, 36, 42
                            Me.CurrentPP += 6
                            Me.MaxPP += 6
                            Return True
                    End Select
                Case 35
                    Select Case Me.MaxPP
                        Case 35, 42, 49
                            Me.CurrentPP += 7
                            Me.MaxPP += 7
                            Return True
                    End Select
                Case 40
                    Select Case Me.MaxPP
                        Case 40, 48, 56
                            Me.CurrentPP += 8
                            Me.MaxPP += 8
                            Return True
                    End Select
            End Select

            Me.CurrentPP = CInt(MathHelper.Clamp(Me.CurrentPP, 0, Me.MaxPP))

            Return False
        End Function

        ''' <summary>
        ''' Returns the texture representing the category of this move.
        ''' </summary>
        Public Function GetDamageCategoryImage() As Texture2D
            Dim r As New Rectangle(0, 0, 0, 0)

            Select Case Me.Category
                Case Categories.Physical
                    r = New Rectangle(115, 0, 28, 14)
                Case Categories.Special
                    r = New Rectangle(115, 14, 28, 14)
                Case Categories.Status
                    r = New Rectangle(115, 28, 28, 14)
            End Select

            Return TextureManager.GetTexture("GUI\Menus\Types", r, "")
        End Function

        ''' <summary>
        ''' Returns a saveable string.
        ''' </summary>
        Public Overrides Function ToString() As String
            Return Me.OriginalID.ToString() & "," & Me.MaxPP.ToString() & "," & Me.CurrentPP.ToString()
        End Function

    End Class

End Namespace
