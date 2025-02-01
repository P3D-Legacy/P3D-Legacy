Namespace BattleSystem

    ''' <summary>
    ''' Provides an interface to load additional GameMode moves.
    ''' </summary>
    Public Class GameModeAttackLoader

        'The default relative path to load moves from (Content folder).
        Const PATH As String = "Data\Moves\"

        'List of loaded moves.
        Shared LoadedMoves As New List(Of Attack)

        ''' <summary>
        ''' Load the attack list for the loaded GameMode.
        ''' </summary>
        ''' <remarks>The game won't try to load the list if the default GameMode is selected.</remarks>
        Public Shared Sub Load()
            LoadedMoves.Clear()

            If GameModeManager.ActiveGameMode.IsDefaultGamemode = False Then
                If System.IO.Directory.Exists(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & "\" & PATH) = True Then
                    For Each file As String In System.IO.Directory.GetFiles(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & PATH, "*.dat")
                        LoadMove(file)
                    Next
                End If
            End If

            If LoadedMoves.Count > 0 Then
                Logger.Debug("Loaded " & LoadedMoves.Count.ToString() & " GameMode move(s).")
            End If
        End Sub

        ''' <summary>
        ''' Loads a move from a file.
        ''' </summary>
        ''' <param name="file">The file to load the move from.</param>
        Private Shared Sub LoadMove(ByVal file As String)
            Dim move As New Attack() 'Load the default Pound move.
            move.IsGameModeMove = True

            Dim content() As String = System.IO.File.ReadAllLines(file)

            Dim key As String = ""
            Dim value As String = ""

            Dim setID As Boolean = False 'Controls if the move sets its ID.
            Dim nonCommentLines As Integer = 0

            Try
                'Go through lines of the file and set the properties depending on the content.
                'Lines starting with # are comments.
                For Each l As String In content
                    If l.Contains("|") = True And l.StartsWith("#") = False Then
                        nonCommentLines += 1
                        key = l.Remove(l.IndexOf("|"))
                        value = l.Remove(0, l.IndexOf("|") + 1)

                        Select Case key.ToLower()
                            Case "copymove"
                                If nonCommentLines = 1 Then
                                    move.gmCopyMove = CInt(value)

                                    Dim _attack As Attack = Attack.GetAttackByID(move.gmCopyMove)
                                    move.Type = _attack.Type
                                    move.Power = _attack.Power
                                    move.Accuracy = _attack.Accuracy
                                    move.Name = _attack.Name
                                    move.OriginalPP = _attack.OriginalPP
                                    move.CurrentPP = _attack.CurrentPP
                                    move.MaxPP = _attack.MaxPP
                                    move.Category = _attack.Category
                                    move.ContestCategory = _attack.ContestCategory
                                    move.Description = _attack.Description
                                    move.CriticalChance = _attack.CriticalChance
                                    move.IsHMMove = _attack.IsHMMove
                                    move.Target = _attack.Target
                                    move.Priority = _attack.Priority
                                    move.TimesToAttack = _attack.TimesToAttack
                                    move.EffectChances = _attack.EffectChances
                                    move.MakesContact = _attack.MakesContact
                                    move.HasSecondaryEffect = _attack.HasSecondaryEffect
                                    move.IsHealingMove = _attack.IsHealingMove
                                    move.IsDamagingMove = _attack.IsDamagingMove
                                    move.IsProtectMove = _attack.IsProtectMove
                                    move.IsOneHitKOMove = _attack.IsOneHitKOMove
                                    move.IsRecoilMove = _attack.IsRecoilMove
                                    move.IsTrappingMove = _attack.IsTrappingMove
                                    move.RemovesOwnFrozen = _attack.RemovesOwnFrozen
                                    move.RemovesOppFrozen = _attack.RemovesOppFrozen
                                    move.SwapsOutOwnPokemon = _attack.SwapsOutOwnPokemon
                                    move.SwapsOutOppPokemon = _attack.SwapsOutOppPokemon
                                    move.ProtectAffected = _attack.ProtectAffected
                                    move.MagicCoatAffected = _attack.MagicCoatAffected
                                    move.SnatchAffected = _attack.SnatchAffected
                                    move.MirrorMoveAffected = _attack.MirrorMoveAffected
                                    move.KingsrockAffected = _attack.KingsrockAffected
                                    move.CounterAffected = _attack.CounterAffected
                                    move.IsAffectedBySubstitute = _attack.IsAffectedBySubstitute
                                    move.ImmunityAffected = _attack.ImmunityAffected
                                    move.IsWonderGuardAffected = _attack.IsWonderGuardAffected
                                    move.DisabledWhileGravity = _attack.DisabledWhileGravity
                                    move.UseEffectiveness = _attack.UseEffectiveness
                                    move.UseAccEvasion = _attack.UseAccEvasion
                                    move.CanHitInMidAir = _attack.CanHitInMidAir
                                    move.CanHitUnderground = _attack.CanHitUnderground
                                    move.CanHitUnderwater = _attack.CanHitUnderwater
                                    move.CanHitSleeping = _attack.CanHitSleeping
                                    move.CanGainSTAB = _attack.CanGainSTAB
                                    move.UseOppDefense = _attack.UseOppDefense
                                    move.UseOppEvasion = _attack.UseOppEvasion
                                    move.IsPulseMove = _attack.IsPulseMove
                                    move.IsBulletMove = _attack.IsBulletMove
                                    move.IsJawMove = _attack.IsJawMove
                                    move.IsDanceMove = _attack.IsDanceMove
                                    move.IsExplosiveMove = _attack.IsExplosiveMove
                                    move.IsPowderMove = _attack.IsPowderMove
                                    move.IsPunchingMove = _attack.IsPunchingMove
                                    move.IsSlicingMove = _attack.IsSlicingMove
                                    move.IsSoundMove = _attack.IsSoundMove
                                    move.IsWindMove = _attack.IsWindMove
                                    move.FocusOppPokemon = _attack.FocusOppPokemon
                                    move.Disabled = _attack.Disabled
                                    move.AIField1 = _attack.AIField1
                                    move.AIField2 = _attack.AIField2
                                    move.AIField3 = _attack.AIField3
                                Else
                                    Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeAttackLoader.vb: The CopyMove property should be the first property set in the move definition file. It is currently property number " & nonCommentLines.ToString & ".")
                                End If
                            Case "id"
                                move.ID = CInt(value)
                                move.OriginalID = CInt(value)
                                setID = True
                            Case "pp"
                                move.CurrentPP = CInt(value)
                                move.MaxPP = CInt(value)
                                move.OriginalPP = CInt(value)
                            Case "function", "movehits"
                                If move.GameModeFunction = "" Then
                                    move.GameModeFunction = value
                                Else
                                    Dim OldFunctionList = move.GameModeFunction
                                    move.GameModeFunction = OldFunctionList & "|" & value
                                End If
                            Case "multiplier", "getbasepower"
                                If move.GameModeBasePower = "" Then
                                    move.GameModeBasePower = value
                                Else
                                    Dim OldBasePowerCalculationList = move.GameModeBasePower
                                    move.GameModeBasePower = OldBasePowerCalculationList & "|" & value
                                End If
                            Case "power", "basepower"
                                move.Power = CInt(value)
                            Case "accuracy", "acc"
                                move.Accuracy = CInt(value)
                            Case "type"
                                If StringHelper.IsNumeric(value) = False Then
                                    move.Type = GameModeElementLoader.GetElementByName(value)
                                Else
                                    move.Type = GameModeElementLoader.GetElementByID(CInt(value))
                                End If
                            Case "category"
                                Select Case value.ToLower()
                                    Case "physical"
                                        move.Category = Attack.Categories.Physical
                                    Case "special"
                                        move.Category = Attack.Categories.Special
                                    Case "status"
                                        move.Category = Attack.Categories.Status
                                End Select
                            Case "contestcategory"
                                Select Case value.ToLower()
                                    Case "tough"
                                        move.ContestCategory = Attack.ContestCategories.Tough
                                    Case "smart"
                                        move.ContestCategory = Attack.ContestCategories.Smart
                                    Case "beauty"
                                        move.ContestCategory = Attack.ContestCategories.Beauty
                                    Case "cool"
                                        move.ContestCategory = Attack.ContestCategories.Cool
                                    Case "cute"
                                        move.ContestCategory = Attack.ContestCategories.Cute
                                End Select
                            Case "name"
                                move.Name = value
                            Case "description"
                                move.Description = value
                            Case "criticalchance", "critical"
                                move.CriticalChance = CInt(value)
                            Case "hmmove", "ishmmove"
                                move.IsHMMove = CBool(value)
                            Case "priority"
                                move.Priority = CInt(value)
                            Case "timestoattack", "tta"
                                move.gmTimesToAttack = value
                            Case "makescontact", "contact"
                                move.MakesContact = CBool(value)
                            Case "protectaffected"
                                move.ProtectAffected = CBool(value)
                            Case "magiccoataffected"
                                move.MagicCoatAffected = CBool(value)
                            Case "snatchaffected"
                                move.SnatchAffected = CBool(value)
                            Case "mirrormoveaffected"
                                move.MirrorMoveAffected = CBool(value)
                            Case "kingsrockaffected"
                                move.KingsrockAffected = CBool(value)
                            Case "counteraffected"
                                move.CounterAffected = CBool(value)
                            Case "disabledduringgravity", "disabledwhilegravity"
                                move.DisabledWhileGravity = CBool(value)
                            Case "useeffectiveness"
                                move.UseEffectiveness = CBool(value)
                            Case "ishealingmove"
                                move.IsHealingMove = CBool(value)
                            Case "removesownfrozen", "removesfrozen"
                                move.RemovesOwnFrozen = CBool(value)
                            Case "removesoppfrozen"
                                move.RemovesOppFrozen = CBool(value)
                            Case "isrecoilmove"
                                move.IsRecoilMove = CBool(value)
                            Case "ispunchingmove"
                                move.IsPunchingMove = CBool(value)
                            Case "immunityaffected"
                                move.ImmunityAffected = CBool(value)
                            Case "isdamagingmove"
                                move.IsDamagingMove = CBool(value)
                            Case "isprotectmove"
                                move.IsProtectMove = CBool(value)
                            Case "issoundmove"
                                move.IsSoundMove = CBool(value)
                            Case "isaffectedbysubstitute"
                                move.IsAffectedBySubstitute = CBool(value)
                            Case "isonehitkomove"
                                move.IsOneHitKOMove = CBool(value)
                            Case "iswonderguardaffected"
                                move.IsWonderGuardAffected = CBool(value)
                            Case "useaccevasion"
                                move.UseAccEvasion = CBool(value)
                            Case "canhitinmidair"
                                move.CanHitInMidAir = CBool(value)
                            Case "canhitunderground"
                                move.CanHitUnderground = CBool(value)
                            Case "canhitunderwater"
                                move.CanHitUnderwater = CBool(value)
                            Case "canhitsleeping"
                                move.CanHitSleeping = CBool(value)
                            Case "cangainstab"
                                move.CanGainSTAB = CBool(value)
                            Case "ispowdermove"
                                move.IsPowderMove = CBool(value)
                            Case "istrappingmove"
                                move.IsTrappingMove = CBool(value)
                            Case "ispulsemove"
                                move.IsPulseMove = CBool(value)
                            Case "isbulletmove"
                                move.IsBulletMove = CBool(value)
                            Case "isjawmove"
                                move.IsJawMove = CBool(value)
                            Case "useoppdefense"
                                move.UseOppDefense = CBool(value)
                            Case "useoppevasion"
                                move.UseOppEvasion = CBool(value)
                            Case "deductpp"
                                move.gmDeductPP = CBool(value)
                            Case "aifield1", "aifield2", "aifield3"
                                Dim AIFieldType As Attack.AIField = Attack.AIField.Nothing
                                Select Case value
                                    Case "damage"
                                        AIFieldType = Attack.AIField.Damage
                                    Case "poison"
                                        AIFieldType = Attack.AIField.Poison
                                    Case "burn"
                                        AIFieldType = Attack.AIField.Burn
                                    Case "paralysis"
                                        AIFieldType = Attack.AIField.Paralysis
                                    Case "sleep"
                                        AIFieldType = Attack.AIField.Sleep
                                    Case "freeze"
                                        AIFieldType = Attack.AIField.Freeze
                                    Case "confusion"
                                        AIFieldType = Attack.AIField.Confusion
                                    Case "confuseown"
                                        AIFieldType = Attack.AIField.ConfuseOwn
                                    Case "canpoison"
                                        AIFieldType = Attack.AIField.CanPoison
                                    Case "canburn"
                                        AIFieldType = Attack.AIField.CanBurn
                                    Case "canparalyze"
                                        AIFieldType = Attack.AIField.CanParalyze
                                    Case "cansleep"
                                        AIFieldType = Attack.AIField.CanSleep
                                    Case "canfreeze"
                                        AIFieldType = Attack.AIField.CanFreeze
                                    Case "canconfuse"
                                        AIFieldType = Attack.AIField.CanConfuse
                                    Case "raiseattack"
                                        AIFieldType = Attack.AIField.RaiseAttack
                                    Case "raisedefense"
                                        AIFieldType = Attack.AIField.RaiseDefense
                                    Case "raisespattack"
                                        AIFieldType = Attack.AIField.RaiseSpAttack
                                    Case "raisespdefense"
                                        AIFieldType = Attack.AIField.RaiseSpDefense
                                    Case "raisespeed"
                                        AIFieldType = Attack.AIField.RaiseSpeed
                                    Case "raiseaccuracy"
                                        AIFieldType = Attack.AIField.RaiseAccuracy
                                    Case "raiseevasion"
                                        AIFieldType = Attack.AIField.RaiseEvasion
                                    Case "lowerattack"
                                        AIFieldType = Attack.AIField.LowerAttack
                                    Case "lowerdefense"
                                        AIFieldType = Attack.AIField.LowerDefense
                                    Case "lowerspattack"
                                        AIFieldType = Attack.AIField.LowerSpAttack
                                    Case "lowerspdefense"
                                        AIFieldType = Attack.AIField.LowerSpDefense
                                    Case "lowerspeed"
                                        AIFieldType = Attack.AIField.LowerSpeed
                                    Case "loweraccuracy"
                                        AIFieldType = Attack.AIField.LowerAccuracy
                                    Case "lowerevasion"
                                        AIFieldType = Attack.AIField.LowerEvasion
                                    Case "canraiseattack"
                                        AIFieldType = Attack.AIField.CanRaiseAttack
                                    Case "canraisedefense"
                                        AIFieldType = Attack.AIField.CanRaiseDefense
                                    Case "canraisespattack"
                                        AIFieldType = Attack.AIField.CanRaiseSpAttack
                                    Case "canraisespdefense"
                                        AIFieldType = Attack.AIField.CanRaiseSpDefense
                                    Case "canraisespeed"
                                        AIFieldType = Attack.AIField.CanRaiseSpeed
                                    Case "canraiseaccuracy"
                                        AIFieldType = Attack.AIField.CanRaiseAccuracy
                                    Case "canrauseevasion"
                                        AIFieldType = Attack.AIField.CanRauseEvasion
                                    Case "canlowerattack"
                                        AIFieldType = Attack.AIField.CanLowerAttack
                                    Case "canlowerdefense"
                                        AIFieldType = Attack.AIField.CanLowerDefense
                                    Case "canlowerspattack"
                                        AIFieldType = Attack.AIField.CanLowerSpAttack
                                    Case "canlowerspdefense"
                                        AIFieldType = Attack.AIField.CanLowerSpDefense
                                    Case "canlowerspeed"
                                        AIFieldType = Attack.AIField.CanLowerSpeed
                                    Case "canloweraccuracy"
                                        AIFieldType = Attack.AIField.CanLowerAccuracy
                                    Case "canlowerevasion"
                                        AIFieldType = Attack.AIField.CanLowerEvasion
                                    Case "flinch"
                                        AIFieldType = Attack.AIField.Flinch
                                    Case "canflinch"
                                        AIFieldType = Attack.AIField.CanFlinch
                                    Case "infatuation"
                                        AIFieldType = Attack.AIField.Infatuation
                                    Case "trap"
                                        AIFieldType = Attack.AIField.Trap
                                    Case "ohko"
                                        AIFieldType = Attack.AIField.OHKO
                                    Case "multiturn"
                                        AIFieldType = Attack.AIField.MultiTurn
                                    Case "recoil"
                                        AIFieldType = Attack.AIField.Recoil
                                    Case "healing"
                                        AIFieldType = Attack.AIField.Healing
                                    Case "curestatus"
                                        AIFieldType = Attack.AIField.CureStatus
                                    Case "support"
                                        AIFieldType = Attack.AIField.Support
                                    Case "recharge"
                                        AIFieldType = Attack.AIField.Recharge
                                    Case "highpriority"
                                        AIFieldType = Attack.AIField.HighPriority
                                    Case "absorbing"
                                        AIFieldType = Attack.AIField.Absorbing
                                    Case "selfdestruct"
                                        AIFieldType = Attack.AIField.Selfdestruct
                                    Case "thrawout"
                                        AIFieldType = Attack.AIField.ThrawOut
                                    Case "cannotmiss"
                                        AIFieldType = Attack.AIField.CannotMiss
                                    Case "removereflectlightscreen"
                                        AIFieldType = Attack.AIField.RemoveReflectLightscreen
                                End Select
                                If AIFieldType <> Attack.AIField.Nothing Then
                                    If key.EndsWith("1") Then
                                        move.AIField1 = AIFieldType
                                    ElseIf key.EndsWith("2") Then
                                        move.AIField2 = AIFieldType
                                    ElseIf key.EndsWith("3") Then
                                        move.AIField3 = AIFieldType
                                    End If
                                End If
                            Case "usemoveanims"
                                move.gmUseMoveAnims = Attack.GetAttackByID(CInt(value))
                            Case "userandommove"
                                move.gmUseRandomMove = True
                                Dim movelist As New List(Of Integer)
                                If value <> "" Then
                                    Dim stringList As List(Of String) = value.Split(";").ToList
                                    For a = 0 To stringList.Count - 1
                                        movelist.Add(CInt(stringList(a)))
                                    Next
                                Else
                                    Dim forbiddenIDs As List(Of Integer) = {68, 102, 118, 119, 144, 165, 166, 168, 173, 182, 194, 197, 203, 214, 243, 264, 266, 267, 270, 271, 274, 289, 343, 364, 382, 383, 415, 448, 476, 469, 495, 501, 511, 516, 546, 547, 548, 553, 554, 555, 557}.ToList()
                                    For a = 1 To Attack.MOVE_COUNT + 1
                                        If forbiddenIDs.Contains(a) = False Then
                                            movelist.Add(a)
                                        End If
                                    Next
                                End If
                                move.gmRandomMoveList = moveList
                        End Select
                    End If
                Next
            Catch ex As Exception
                'If an error occurs loading a move, log the error.
                Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeAttackLoader.vb: Error loading GameMode move from file """ & file & """: " & ex.Message & "; Last Key/Value pair successfully loaded: " & key & "|" & value)
            End Try

            If nonCommentLines > 0 Then
                If setID = True Then
                    If move.ID >= 1000 Then
                        Dim testMove As Attack = Attack.GetAttackByID(move.ID)
                        If testMove.IsDefaultMove = True Then
                            If Localization.TokenExists("move_name_" & move.ID.ToString) = True Then
                                move.Name = Localization.GetString("move_name_" & move.ID.ToString)
                            End If
                            LoadedMoves.Add(move) 'Add the move.
                        Else
                            Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeAttackLoader.vb: User defined moves are not allowed to have an ID of an already existing move or an ID below 1000. The ID for the move loaded from """ & file & """ has the ID " & move.ID.ToString() & ", which is the ID of an already existing move (" & testMove.Name & ").")
                        End If
                    Else
                        Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeAttackLoader.vb: User defined moves are not allowed to have an ID of an already existing move or an ID below 1000. The ID for the move loaded from """ & file & """ has the ID " & move.ID.ToString() & ", which is smaller than 1000.")
                    End If
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeAttackLoader.vb: User defined moves must set their ID through the ""ID"" property, however the move loaded from """ & file & """ has no ID set so it will be ignored.")
                End If
            Else
                Debug.Print("GameModeAttackLoader.vb: The move loaded from """ & file & """ has no valid lines so it will be ignored.")
            End If
        End Sub

        ''' <summary>
        ''' Returns a custom move based on its ID.
        ''' </summary>
        ''' <param name="ID">The ID of the custom move.</param>
        ''' <returns>Returns a move or nothing.</returns>
        Public Shared Function GetAttackByID(ByVal ID As Integer) As Attack
            For Each m As Attack In LoadedMoves
                If m.ID = ID Then
                    Return m
                End If
            Next
            Return Nothing
        End Function

    End Class

End Namespace