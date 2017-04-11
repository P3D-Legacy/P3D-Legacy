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
                For Each file As String In System.IO.Directory.GetFiles(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & "\" & PATH, "*.dat")
                    LoadMove(file)
                Next
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

            Try
                'Go through lines of the file and set the properties depending on the content.
                'Lines starting with # are comments.
                For Each l As String In content
                    If l.Contains("|") = True And l.StartsWith("#") = False Then
                        key = l.Remove(l.IndexOf("|"))
                        value = l.Remove(0, l.IndexOf("|") + 1)

                        Select Case key.ToLower()
                            Case "id"
                                move.ID = CInt(value)
                                move.OriginalID = CInt(value)
                                setID = True
                            Case "pp"
                                move.CurrentPP = CInt(value)
                                move.MaxPP = CInt(value)
                                move.OriginalPP = CInt(value)
                            Case "function"
                                move.GameModeFunction = value
                            Case "power", "basepower"
                                move.Power = CInt(value)
                            Case "accuracy", "acc"
                                move.Accuracy = CInt(value)
                            Case "type"
                                move.Type = New Element(value)
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
                                move.TimesToAttack = CInt(value)

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
                            Case "removesfrozen"
                                move.RemovesFrozen = CBool(value)
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
                        End Select
                    End If
                Next
            Catch ex As Exception
                'If an error occurs loading a move, log the error.
                Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeAttackLoader.vb: Error loading GameMode move from file """ & file & """: " & ex.Message & "; Last Key/Value pair successfully loaded: " & key & "|" & value)
            End Try

            If setID = True Then
                If move.ID >= 1000 Then
                    Dim testMove As Attack = Attack.GetAttackByID(move.ID)
                    If testMove.IsDefaultMove = True Then
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