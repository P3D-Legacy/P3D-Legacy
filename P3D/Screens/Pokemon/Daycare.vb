﻿Public Class Daycare

    Public Shared Function ProduceEgg(ByVal daycareID As Integer) As Pokemon
        Dim parent1 As Pokemon = Nothing
        Dim parent2 As Pokemon = Nothing

        Dim EggID As Integer = 0

        For Each line As String In Core.Player.DaycareData.SplitAtNewline()
            If line.StartsWith(daycareID.ToString() & "|0|") = True Then
                Dim data As String = line.Remove(0, line.IndexOf("{"))
                parent1 = P3D.Pokemon.GetPokemonByData(data)
            ElseIf line.StartsWith(daycareID.ToString() & "|1|") = True Then
                Dim data As String = line.Remove(0, line.IndexOf("{"))
                parent2 = P3D.Pokemon.GetPokemonByData(data)
            ElseIf line.StartsWith(daycareID.ToString() & "|Egg|") = True Then
                EggID = CInt(line.Split(CChar("|"))(2))
            End If
        Next

        Dim DittoAsParent As Integer = 0
        If parent1.EggGroup1 = Pokemon.EggGroups.Ditto Or parent1.EggGroup2 = Pokemon.EggGroups.Ditto Then
            DittoAsParent = 1
        ElseIf parent2.EggGroup1 = Pokemon.EggGroups.Ditto Or parent2.EggGroup2 = Pokemon.EggGroups.Ditto Then
            DittoAsParent = 2
        End If

        If Not parent1 Is Nothing And Not parent2 Is Nothing And EggID <> 0 Then
            Dim p As Pokemon = Pokemon.GetPokemonByID(EggID)
            Dim OptionalAdditionalData As String = "xXx"

            ' Regional Form check
            If Screen.Level.RegionalForm.Contains(CChar(",")) Then
                For Each r As String In Screen.Level.RegionalForm.Split(CChar(","))
                    If p.RegionalForms.Contains(r.ToLower()) Then
                        p.AdditionalData = r.ToLower()
                    End If
                Next
            Else
                If p.RegionalForms.ToLower.Contains(Screen.Level.RegionalForm.ToLower()) Then
                    p.AdditionalData = Screen.Level.RegionalForm.ToLower()
                End If
            End If

            ' Form inheritance
            Select Case DittoAsParent
                Case 0
                    If parent1.Gender = P3D.Pokemon.Genders.Female Then
                        If parent1.Item IsNot Nothing AndAlso parent1.Item.OriginalName.ToLower() = "everstone" Then
                            p.AdditionalData = parent1.AdditionalData
                        ElseIf parent2.Number = parent1.Number And parent2.Item IsNot Nothing AndAlso parent2.Item.OriginalName.ToLower() = "everstone" Then
                            p.AdditionalData = parent2.AdditionalData
                        End If
                    Else
                        If parent2.Item IsNot Nothing AndAlso parent2.Item.OriginalName.ToLower() = "everstone" Then
                            p.AdditionalData = parent2.AdditionalData
                        ElseIf parent1.Number = parent2.Number And parent1.Item IsNot Nothing AndAlso parent1.Item.OriginalName.ToLower() = "everstone" Then
                            p.AdditionalData = parent1.AdditionalData
                        End If
                    End If
                Case 1
                    If parent2.Item IsNot Nothing AndAlso parent2.Item.OriginalName.ToLower() = "everstone" Then
                        p.AdditionalData = parent2.AdditionalData
                    End If
                Case 2
                    If parent1.Item IsNot Nothing AndAlso parent1.Item.OriginalName.ToLower() = "everstone" Then
                        p.AdditionalData = parent1.AdditionalData
                    End If
            End Select

            If p.AdditionalData <> "" Then
                OptionalAdditionalData = p.AdditionalData
            End If

            p.Generate(1, True, OptionalAdditionalData)
            p.EggSteps = 1
            p.SetCatchInfos(Item.GetItemByID(5.ToString), Localization.GetString("CatchMethod_Obtained", "Obtained at"))
            p.CatchBall = Item.GetItemByID(GetEggPokeballID({parent1, parent2}.ToList()))

            p.ReloadDefinitions()
            p.CalculateStats()

            ' Adding Egg Moves:
            Dim EggMoves As New List(Of BattleSystem.Attack)

            ' Level-Up moves:
            If DittoAsParent = 0 Then
                For Each m1 As BattleSystem.Attack In parent1.Attacks
                    For Each m2 As BattleSystem.Attack In parent2.Attacks
                        If m1.ID = m2.ID Then
                            Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(m1.ID)
                            EggMoves.Add(newAttack)
                        End If
                    Next
                Next
            End If

            ' Technical/Hidden Machine moves:
            Dim male As Integer = -1
            If parent1.Gender = P3D.Pokemon.Genders.Male Then
                male = 0
            End If
            If parent2.Gender = P3D.Pokemon.Genders.Male Then
                male = 1
            End If
            If male > -1 Then
                Dim cParent As Pokemon = Nothing
                Select Case male
                    Case 0
                        cParent = parent1
                    Case 1
                        cParent = parent2
                End Select
                For Each aList As List(Of BattleSystem.Attack) In p.AttackLearns.Values
                    For Each THMMove As BattleSystem.Attack In aList
                        For Each m1 As BattleSystem.Attack In cParent.Attacks
                            If m1.ID = THMMove.ID Then
                                Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(m1.ID)
                                EggMoves.Add(newAttack)
                            End If
                        Next

                    Next
                Next

            End If

            ' Egg Moves:
            male = -1
            If parent1.Gender = P3D.Pokemon.Genders.Male Then
                male = 0
            End If
            If parent2.Gender = P3D.Pokemon.Genders.Male Then
                male = 1
            End If
            If male > -1 Then
                For Each BreedMove As Integer In p.EggMoves
                    For Each m1 As BattleSystem.Attack In parent1.Attacks
                        If m1.ID = BreedMove Then
                            GameJolt.Emblem.AchieveEmblem("eggsplosion")

                            Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(m1.ID)
                            EggMoves.Add(newAttack)
                        End If
                    Next
                    For Each m1 As BattleSystem.Attack In parent2.Attacks
                        If m1.ID = BreedMove Then
                            GameJolt.Emblem.AchieveEmblem("eggsplosion")

                            Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(m1.ID)
                            EggMoves.Add(newAttack)
                        End If
                    Next
                Next
            End If
            ' Volt Tackle for Pikachu:
            If (parent1.Item IsNot Nothing AndAlso parent1.Item.Name.ToLower = "light ball") OrElse (parent2.Item IsNot Nothing AndAlso parent2.Item.Name.ToLower = "light ball") Then
                Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(344)
                EggMoves.Add(newAttack)
            End If

            Dim learnMoves As New List(Of BattleSystem.Attack)
            If EggMoves.Count <= 4 Then
                learnMoves.AddRange(EggMoves.ToArray())
            Else
                For i = EggMoves.Count - 4 To EggMoves.Count - 1
                    learnMoves.Add(EggMoves(i))
                Next
            End If

            While p.Attacks.Count + learnMoves.Count > 4
                p.Attacks.RemoveAt(0)
            End While

            For Each learnMove As BattleSystem.Attack In learnMoves
                Dim hasAttack As Boolean = False
                For Each m As BattleSystem.Attack In p.Attacks
                    If m.ID = learnMove.ID Then
                        hasAttack = True
                        Exit For
                    End If
                Next
                If hasAttack = False Then
                    p.Attacks.Add(learnMove)
                End If
            Next

            ' Inherit stats:
            Dim IV1 As New List(Of String)
            Dim IV2 As New List(Of String)

            Dim EVStat1 As String = ""
            Dim EVStat2 As String = ""

            Dim DKnot As Boolean = False

            Dim EVItems() As String = {"power weight", "power bracer", "power belt", "power lens", "power band", "power anklet", "destiny knot"}
            If Not parent1.Item Is Nothing Then
                If EVItems.Contains(parent1.Item.OriginalName.ToLower()) = True Then
                    Select Case parent1.Item.OriginalName.ToLower()
                        Case "power weight"
                            EVStat1 = "HP"
                        Case "power bracer"
                            EVStat1 = "Attack"
                        Case "power belt"
                            EVStat1 = "Defense"
                        Case "power lens"
                            EVStat1 = "Special Attack"
                        Case "power band"
                            EVStat1 = "Special Defense"
                        Case "power anklet"
                            EVStat1 = "Speed"
                        Case "destiny knot"
                            DKnot = True
                    End Select
                End If
            End If
            If Not parent2.Item Is Nothing Then
                If EVItems.Contains(parent2.Item.OriginalName.ToLower()) = True Then
                    Select Case parent2.Item.OriginalName.ToLower()
                        Case "power weight"
                            EVStat2 = "HP"
                        Case "power bracer"
                            EVStat2 = "Attack"
                        Case "power belt"
                            EVStat2 = "Defense"
                        Case "power lens"
                            EVStat2 = "Special Attack"
                        Case "power band"
                            EVStat2 = "Special Defense"
                        Case "power anklet"
                            EVStat2 = "Speed"
                        Case "destiny knot"
                            DKnot = True
                    End Select
                End If
            End If

            If EVStat1 <> "" And EVStat2 = "" Then
                IV1.Add(EVStat1)
            ElseIf EVStat1 = "" And EVStat2 <> "" Then
                IV2.Add(EVStat2)
            ElseIf EVStat1 <> "" And EVStat2 <> "" Then
                If Core.Random.Next(0, 2) = 0 Then
                    IV1.Add(EVStat1)
                Else
                    IV2.Add(EVStat2)
                End If
            End If

            Dim InheritIV As Integer = 3
            If DKnot = True Then
                InheritIV = 5
            End If

            While IV1.Count + IV2.Count < InheritIV
                Dim newStat As String = ""
                While newStat = "" Or IV1.Contains(newStat) = True Or IV2.Contains(newStat) = True
                    Select Case Core.Random.Next(0, 6)
                        Case 0
                            newStat = "Attack"
                        Case 1
                            newStat = "Defense"
                        Case 2
                            newStat = "Special Attack"
                        Case 3
                            newStat = "Special Defense"
                        Case 4
                            newStat = "Speed"
                        Case 5
                            newStat = "HP"
                    End Select
                End While

                Select Case Core.Random.Next(0, 2)
                    Case 0
                        IV1.Add(newStat)
                    Case 1
                        IV2.Add(newStat)
                End Select
            End While

            For Each IV As String In IV1
                Select Case IV
                    Case "HP"
                        p.IVHP = parent1.IVHP
                    Case "Attack"
                        p.IVAttack = parent1.IVAttack
                    Case "Defense"
                        p.IVDefense = parent1.IVDefense
                    Case "Special Attack"
                        p.IVSpAttack = parent1.IVSpAttack
                    Case "Special Defense"
                        p.IVSpDefense = parent1.IVSpDefense
                    Case "Speed"
                        p.IVSpeed = parent1.IVSpeed
                End Select
            Next
            For Each IV As String In IV2
                Select Case IV
                    Case "HP"
                        p.IVHP = parent2.IVHP
                    Case "Attack"
                        p.IVAttack = parent2.IVAttack
                    Case "Defense"
                        p.IVDefense = parent2.IVDefense
                    Case "Special Attack"
                        p.IVSpAttack = parent2.IVSpAttack
                    Case "Special Defense"
                        p.IVSpDefense = parent2.IVSpDefense
                    Case "Speed"
                        p.IVSpeed = parent2.IVSpeed
                End Select
            Next

            ' Natures:
            Dim EStone1 As Boolean = False
            Dim EStone2 As Boolean = False

            If Not parent1.Item Is Nothing Then
                If parent1.Item.OriginalName.ToLower() = "everstone" Then
                    EStone1 = True
                End If
            End If
            If Not parent2.Item Is Nothing Then
                If parent2.Item.OriginalName.ToLower() = "everstone" Then
                    EStone2 = True
                End If
            End If

            If EStone1 = True And EStone2 = False Then
                p.Nature = parent1.Nature
            ElseIf EStone1 = False And EStone2 = True Then
                p.Nature = parent2.Nature
            ElseIf EStone1 = True And EStone2 = True Then
                If Core.Random.Next(0, 2) = 0 Then
                    p.Nature = parent1.Nature
                Else
                    p.Nature = parent2.Nature
                End If
            End If

            ' Abilities:
            If DittoAsParent = 0 Then
                Dim female As Pokemon = parent1
                If parent2.Gender = P3D.Pokemon.Genders.Female Then
                    female = parent2
                End If

                If Core.Random.Next(0, 100) < 80 Then
                    p.Ability = female.Ability
                End If
            End If

            ' Hidden Ability:
            If DittoAsParent <> 0 Then
                If DittoAsParent = 1 Then
                    If parent2.IsUsingHiddenAbility = True And p.HasHiddenAbility = True And Core.Random.Next(0, 100) < 80 Then
                        p.Ability = p.HiddenAbility
                    End If
                Else
                    If parent1.IsUsingHiddenAbility = True And p.HasHiddenAbility = True And Core.Random.Next(0, 100) < 80 Then
                        p.Ability = p.HiddenAbility
                    End If
                End If
            Else
                Dim female As Pokemon = parent1
                If parent2.Gender = P3D.Pokemon.Genders.Female Then
                    female = parent2
                End If

                If female.IsUsingHiddenAbility = True And p.HasHiddenAbility = True And Core.Random.Next(0, 100) < 80 Then
                    p.Ability = p.HiddenAbility
                End If
            End If

            ' Shiny:
            Dim Shiny1 As Boolean = parent1.IsShiny
            Dim Shiny2 As Boolean = parent2.IsShiny

            Dim chances As List(Of Integer) = {1, Pokemon.MasterShinyRate}.ToList()

            If Shiny1 = True And Shiny2 = True Then
                chances = {12, Pokemon.MasterShinyRate}.ToList() '12/base rate odds
            ElseIf Shiny1 = True Or Shiny2 = True Then
                chances = {6, Pokemon.MasterShinyRate}.ToList() '6/base rate odds
            End If

            If Core.Random.Next(0, chances(1)) < chances(0) Then
                p.IsShiny = True
            Else
                p.IsShiny = False
            End If

            If p.HP > p.MaxHP Then
                p.HP = p.MaxHP
            End If

            Return p
        End If

        Return Nothing
    End Function

    Public Shared Function CanBreed(ByVal Pokemon As List(Of Pokemon), Optional ByVal multiplier As Boolean = True) As Integer
        Dim chance As Integer = 0

        If Pokemon.Count = 2 Then
            Dim p1 As Pokemon = Pokemon(0)
            Dim p2 As Pokemon = Pokemon(1)

            If p1.CanBreed = False Or p2.CanBreed = False Then
                Return 0
            End If

            If p1.EggGroup1 = P3D.Pokemon.EggGroups.Undiscovered Or p1.EggGroup2 = P3D.Pokemon.EggGroups.Undiscovered Or p2.EggGroup1 = P3D.Pokemon.EggGroups.Undiscovered Or p2.EggGroup2 = P3D.Pokemon.EggGroups.Undiscovered Then
                Return 0
            End If

            If p1.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                If p2.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                    Return 0
                End If
            End If

            If p2.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                If p1.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                    Return 0
                End If
            End If

            If p1.IsGenderless = True Then
                If p2.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                    chance = -1
                Else
                    If p1.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                        chance = -1
                    End If
                End If
            ElseIf p2.IsGenderless = True Then
                If p1.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                    chance = -1
                Else
                    If p2.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                        chance = -1
                    End If
                End If
            ElseIf p1.IsGenderless = False And p2.IsGenderless = False Then
                If p1.Gender <> p2.Gender Then
                    If p1.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p2.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                        chance = -1
                    Else
                        If p1.EggGroup1 = p2.EggGroup1 And p1.EggGroup1 <> P3D.Pokemon.EggGroups.None Or
                            p1.EggGroup2 = p2.EggGroup1 And p1.EggGroup2 <> P3D.Pokemon.EggGroups.None Or
                            p1.EggGroup1 = p2.EggGroup2 And p1.EggGroup1 <> P3D.Pokemon.EggGroups.None Or
                            p1.EggGroup2 = p2.EggGroup2 And p1.EggGroup2 <> P3D.Pokemon.EggGroups.None Then
                            chance = -1
                        End If
                    End If
                End If
            End If

            If chance = -1 Then
                If p1.Number = p2.Number And p1.OT <> p2.OT Then
                    chance = 70
                End If
                If p1.Number = p2.Number And p1.OT = p2.OT Then
                    chance = 50
                End If
                If p1.Number <> p2.Number And p1.OT <> p2.OT Then
                    chance = 50
                End If
                If p1.Number <> p2.Number And p1.OT = p2.OT Then
                    chance = 20
                End If
            End If
        End If

        If chance > 0 And multiplier = True Then
            If Core.Player.Inventory.GetItemAmount(241.ToString) > 0 Then
                chance = CInt(chance * 1.3F)
            End If
        End If

        Return chance
    End Function

    Public Shared Function CanBreed(ByVal daycareID As Integer, Optional ByVal multiplier As Boolean = True) As Integer
        Dim l As New List(Of Pokemon)

        For Each line As String In Core.Player.DaycareData.SplitAtNewline()
            If line.StartsWith(daycareID.ToString() & "|") = True Then
                Dim data As String = line.Remove(0, line.IndexOf("{"))
                Dim p As Pokemon = P3D.Pokemon.GetPokemonByData(data)
                l.Add(p)
            End If
        Next

        Return CanBreed(l, multiplier)
    End Function

    Public Shared Function CanBreed(ByVal Pokemon As Dictionary(Of Integer, Pokemon), Optional ByVal multiplier As Boolean = True) As Integer
        Dim l As New List(Of Pokemon)
        For i = 0 To Pokemon.Count - 1
            l.Add(Pokemon.Values(i))
        Next
        Return CanBreed(l, multiplier)
    End Function

    Private Shared Function GetEggPokemonID(ByVal Pokemon As Dictionary(Of Integer, Pokemon)) As String
        If Pokemon.Count = 2 Then
            Dim p1 As Pokemon = Pokemon.Values(0)
            Dim p2 As Pokemon = Pokemon.Values(1)

            If p1.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                Dim dexID As String = PokemonForms.GetPokemonDataFileName(p2.Number, p2.AdditionalData)
                Return dexID
            End If
            If p2.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                Dim dexID As String = PokemonForms.GetPokemonDataFileName(p1.Number, p1.AdditionalData)
                Return dexID
            End If

            If p1.Gender = P3D.Pokemon.Genders.Female Then
                Dim dexID As String = PokemonForms.GetPokemonDataFileName(p1.Number, p1.AdditionalData)
                Return dexID
            End If
            If p2.Gender = P3D.Pokemon.Genders.Female Then
                Dim dexID As String = PokemonForms.GetPokemonDataFileName(p2.Number, p2.AdditionalData)
                Return dexID
            End If
        End If

        Return 0.ToString
    End Function

    Private Shared Function GetEggPokeballID(ByVal Pokemon As List(Of Pokemon)) As String
        Dim ballID As String = 5.ToString

        If Pokemon.Count = 2 Then

            Dim p1 As Pokemon = Pokemon(0)
            Dim p2 As Pokemon = Pokemon(1)

            Dim CatchBallID1 As String
            Dim CatchBallID2 As String
            If p1.CatchBall.IsGameModeItem Then
                CatchBallID1 = p1.CatchBall.gmID
            Else
                CatchBallID1 = p1.CatchBall.ID.ToString
            End If
            If p2.CatchBall.IsGameModeItem Then
                CatchBallID2 = p2.CatchBall.gmID
            Else
                CatchBallID2 = p2.CatchBall.ID.ToString
            End If

            ' First Pokémon is Ditto:
            If p1.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                ' If the first Pokémon is Ditto, then the other Pokémon must be female to inherit the Poké Ball.
                If p2.Gender = P3D.Pokemon.Genders.Female Then
                    ballID = CatchBallID2
                End If
            End If
            'Second Pokémon is Ditto.
            If p2.EggGroup1 = P3D.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = P3D.Pokemon.EggGroups.Ditto Then
                'If the second Pokémon is Ditto, then the other Pokémon must be female to inherit the Poké Ball.
                If p1.Gender = P3D.Pokemon.Genders.Female Then
                    ballID = CatchBallID1
                End If
            End If
            ' No Pokémon is Ditto:
            If p1.EggGroup1 <> P3D.Pokemon.EggGroups.Ditto And p1.EggGroup2 <> P3D.Pokemon.EggGroups.Ditto And p2.EggGroup1 <> P3D.Pokemon.EggGroups.Ditto And p2.EggGroup2 <> P3D.Pokemon.EggGroups.Ditto Then
                ' First Pokémon is female:
                If p1.Gender = P3D.Pokemon.Genders.Female Then
                    ballID = CatchBallID1
                End If
                ' Second Pokémon is female:
                If p2.Gender = P3D.Pokemon.Genders.Female Then
                    ballID = CatchBallID2
                End If
            End If

            ' Check for: Master Ball, Cherish Ball: Set to Poké Ball
            If ballID = 1.ToString Or ballID = 45.ToString Then
                ballID = 5.ToString
            End If
        End If

        ' Return BallID (Standard is 5 for Pokéball):
        Return ballID
    End Function

    Public Shared Sub ObtainEgg()
        Dim Data() As String = Core.Player.DaycareData.SplitAtNewline()

        Dim IDs As New List(Of Integer)

        For Each line As String In Data
            If line <> "" And line.Contains("|") = True Then
                Dim newID As Integer = CInt(line.GetSplit(0, "|"))
                If IDs.Contains(newID) = False Then
                    IDs.Add(newID)
                End If
            End If
        Next

        Logger.Debug("Daycare circle complete!")

        For Each DaycareID As Integer In IDs
            Logger.Debug("Daycare ID: " & DaycareID)

            Dim Pokemon As New Dictionary(Of Integer, Pokemon)

            Dim hasEgg As Boolean = False
            Dim EggID As Integer = 0

            For Each line As String In Data
                If line.StartsWith(DaycareID & "|") = True Then
                    If line.GetSplit(1, "|") = "Egg" Then
                        hasEgg = True
                        EggID = CInt(line.GetSplit(2, "|"))
                    Else
                        Dim PlaceID As Integer = CInt(line.GetSplit(1, "|"))
                        Dim startStep As Integer = CInt(line.GetSplit(2, "|"))
                        Dim Level As Integer = CInt(line.GetSplit(3, "|"))
                        Dim PokemonData As String = line.GetSplit(4, "|")

                        If Pokemon.ContainsKey(PlaceID) = False Then
                            Dim p As Pokemon = P3D.Pokemon.GetPokemonByData(PokemonData)

                            Pokemon.Add(PlaceID, p)
                        End If
                    End If
                End If
            Next

            Logger.Debug("Pokémon count: " & Pokemon.Count)
            Logger.Debug("Has Egg: " & hasEgg.ToString())

            If hasEgg = False Then
                Dim breedChance As Integer = CanBreed(Pokemon)
                If breedChance > 0 Then
                    Logger.Debug("Breed chance: " & breedChance)
                    If Core.Random.Next(0, 100) < breedChance Then
                        Dim DexID As String = GetEggPokemonID(Pokemon)
                        Dim parentID As Integer = CInt(DexID.GetSplit(0, "_"))
                        Dim parentAD As String = ""
                        If DexID.Contains("_") = True Then
                            parentAD = DexID.GetSplit(1, "_")
                        End If

                        Dim newEggID As String = P3D.Pokemon.GetPokemonByID(parentID, parentAD).EggPokemon
                        Dim s As String = DaycareID.ToString() & "|Egg|" & newEggID.ToString()

                        Logger.Debug("Egg created!" & Environment.NewLine & "EggID: " & newEggID)
                        TriggerCall(DaycareID)

                        Dim oldData As String = Core.Player.DaycareData
                        If oldData <> "" Then
                            oldData &= Environment.NewLine
                        End If
                        oldData &= s
                        Core.Player.DaycareData = oldData
                    End If
                Else
                    Logger.Debug("Pokémon in Daycare " & DaycareID & " cannot breed.")
                End If
            End If
        Next
    End Sub

    Public Shared Sub TriggerCall(ByVal daycareID As Integer)
        If ActionScript.IsRegistered("daycare_callid_" & daycareID.ToString()) = True Then
            Dim c() As Object = ActionScript.GetRegisterValue("daycare_callid_" & daycareID.ToString())
            If Not c(0) Is Nothing And Not c(1) Is Nothing Then
                Dim callID As String = CStr(c(0))

                GameJolt.PokegearScreen.CallID(callID, True, False)
            Else
                Logger.Debug("Cannot initialize call for Daycare ID " & daycareID.ToString() & ".")
            End If
        Else
            Logger.Debug("Cannot initialize call for Daycare ID " & daycareID.ToString() & ".")
        End If
    End Sub

    Public Shared Sub EggCircle()
        Core.Player.PlayerTemp.DayCareCycle = 256
        ObtainEgg()
    End Sub

End Class