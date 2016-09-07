Public Class Daycare

    Public Shared Function ProduceEgg(ByVal daycareID As Integer) As Pokemon
        Dim parent1 As Pokemon = Nothing
        Dim parent2 As Pokemon = Nothing

        Dim EggID As Integer = 0

        For Each line As String In Core.Player.DaycareData.SplitAtNewline()
            If line.StartsWith(daycareID.ToString() & "|0|") = True Then
                Dim data As String = line.Remove(0, line.IndexOf("{"))
                parent1 = net.Pokemon3D.Game.Pokemon.GetPokemonByData(data)
            ElseIf line.StartsWith(daycareID.ToString() & "|1|") = True Then
                Dim data As String = line.Remove(0, line.IndexOf("{"))
                parent2 = net.Pokemon3D.Game.Pokemon.GetPokemonByData(data)
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
            p.Generate(1, True)
            p.EggSteps = 1
            p.SetCatchInfos(Item.GetItemByID(5), "obtained at")
            p.CatchBall = Item.GetItemByID(GetEggPokeballID({parent1, parent2}.ToList()))

            'adding egg moves:
            Dim EggMoves As New List(Of BattleSystem.Attack)

            'Level-Up moves:
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

            'TM-HM Moves:
            Dim male As Integer = -1
            If parent1.Gender = net.Pokemon3D.Game.Pokemon.Genders.Male Then
                male = 0
            End If
            If parent2.Gender = net.Pokemon3D.Game.Pokemon.Genders.Male Then
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
                For Each THMMove As BattleSystem.Attack In p.AttackLearns.Values
                    For Each m1 As BattleSystem.Attack In cParent.Attacks
                        If m1.ID = THMMove.ID Then
                            Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(m1.ID)
                            EggMoves.Add(newAttack)
                        End If
                    Next
                Next
            End If

            'Egg Moves:
            male = -1
            If parent1.Gender = net.Pokemon3D.Game.Pokemon.Genders.Male Then
                male = 0
            End If
            If parent2.Gender = net.Pokemon3D.Game.Pokemon.Genders.Male Then
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
                For Each BreedMove As Integer In p.EggMoves
                    For Each m1 As BattleSystem.Attack In cParent.Attacks
                        If m1.ID = BreedMove Then
                            GameJolt.Emblem.AchieveEmblem("eggsplosion")

                            Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(m1.ID)
                            EggMoves.Add(newAttack)
                        End If
                    Next
                Next
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

            'Inherit stats:
            Dim IV1 As New List(Of String)
            Dim IV2 As New List(Of String)

            Dim EVStat1 As String = ""
            Dim EVStat2 As String = ""

            Dim EVItems() As String = {"power weight", "power bracer", "power belt", "power lens", "power band", "power anklet"}
            If Not parent1.Item Is Nothing Then
                If EVItems.Contains(parent1.Item.Name.ToLower()) = True Then
                    Select Case parent1.Item.Name.ToLower()
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
                    End Select
                End If
            End If
            If Not parent2.Item Is Nothing Then
                If EVItems.Contains(parent2.Item.Name.ToLower()) = True Then
                    Select Case parent2.Item.Name.ToLower()
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

            While IV1.Count + IV2.Count < 3
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

            'Natures:
            Dim EStone1 As Boolean = False
            Dim EStone2 As Boolean = False

            If Not parent1.Item Is Nothing Then
                If parent1.Item.Name.ToLower() = "everstone" Then
                    EStone1 = True
                End If
            End If
            If Not parent2.Item Is Nothing Then
                If parent2.Item.Name.ToLower() = "everstone" Then
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

            'Abilities:
            If DittoAsParent = 0 Then
                Dim female As Pokemon = parent1
                If parent2.Gender = net.Pokemon3D.Game.Pokemon.Genders.Female Then
                    female = parent2
                End If

                If Core.Random.Next(0, 100) < 80 Then
                    p.Ability = female.Ability
                End If
            End If

            'Hidden Ability'
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
                If parent2.Gender = net.Pokemon3D.Game.Pokemon.Genders.Female Then
                    female = parent2
                End If

                If female.IsUsingHiddenAbility = True And p.HasHiddenAbility = True And Core.Random.Next(0, 100) < 80 Then
                    p.Ability = p.HiddenAbility
                End If
            End If

            'Shiny:
            Dim Shiny1 As Boolean = parent1.IsShiny
            Dim Shiny2 As Boolean = parent2.IsShiny

            Dim chances As List(Of Integer) = {1, 8192}.ToList()

            If Shiny1 = True And Shiny2 = True Then
                chances = {12, 8192}.ToList()
            ElseIf Shiny1 = True Or Shiny2 = True Then
                chances = {6, 8192}.ToList()
            End If

            If Core.Random.Next(0, chances(1)) < chances(0) Then
                p.IsShiny = True
            Else
                p.IsShiny = False
            End If

            Return p
        End If

        Return Nothing
    End Function

    Public Shared Function CanBreed(ByVal Pokemon As List(Of Pokemon)) As Integer
        Dim chance As Integer = 0

        If Pokemon.Count = 2 Then
            Dim p1 As Pokemon = Pokemon(0)
            Dim p2 As Pokemon = Pokemon(1)

            If p1.CanBreed = False Or p2.CanBreed = False Then
                Return 0
            End If

            If p1.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Undiscovered Or p1.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Undiscovered Or p2.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Undiscovered Or p2.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Undiscovered Then
                Return 0
            End If

            If p1.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                If p2.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                    Return 0
                End If
            End If

            If p2.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                If p1.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                    Return 0
                End If
            End If

            If p1.IsGenderLess = True Then
                If p2.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                    chance = -1
                Else
                    If p1.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                        chance = -1
                    End If
                End If
            ElseIf p2.IsGenderLess = True Then
                If p1.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                    chance = -1
                Else
                    If p2.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                        chance = -1
                    End If
                End If
            ElseIf p1.IsGenderLess = False And p2.IsGenderLess = False Then
                If p1.Gender <> p2.Gender Then
                    If p1.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p2.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                        chance = -1
                    Else
                        If p1.EggGroup1 = p2.EggGroup1 And p1.EggGroup1 <> net.Pokemon3D.Game.Pokemon.EggGroups.None Or
                            p1.EggGroup2 = p2.EggGroup1 And p1.EggGroup2 <> net.Pokemon3D.Game.Pokemon.EggGroups.None Or
                            p1.EggGroup1 = p2.EggGroup2 And p1.EggGroup1 <> net.Pokemon3D.Game.Pokemon.EggGroups.None Or
                            p1.EggGroup2 = p2.EggGroup2 And p1.EggGroup2 <> net.Pokemon3D.Game.Pokemon.EggGroups.None Then
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

        If chance > 0 Then
            If Core.Player.Inventory.GetItemAmount(241) > 0 Then
                chance = CInt(chance * 1.3F)
            End If
        End If

        Return chance
    End Function

    Public Shared Function CanBreed(ByVal daycareID As Integer) As Integer
        Dim l As New List(Of Pokemon)

        For Each line As String In Core.Player.DaycareData.SplitAtNewline()
            If line.StartsWith(daycareID.ToString() & "|") = True Then
                Dim data As String = line.Remove(0, line.IndexOf("{"))
                Dim p As Pokemon = net.Pokemon3D.Game.Pokemon.GetPokemonByData(data)
                l.Add(p)
            End If
        Next

        Return CanBreed(l)
    End Function

    Public Shared Function CanBreed(ByVal Pokemon As Dictionary(Of Integer, Pokemon)) As Integer
        Dim l As New List(Of Pokemon)
        For i = 0 To Pokemon.Count - 1
            l.Add(Pokemon.Values(i))
        Next
        Return CanBreed(l)
    End Function

    Private Shared Function GetEggPokemonID(ByVal Pokemon As Dictionary(Of Integer, Pokemon)) As Integer
        If Pokemon.Count = 2 Then
            Dim p1 As Pokemon = Pokemon.Values(0)
            Dim p2 As Pokemon = Pokemon.Values(1)

            If p1.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                Return p2.Number
            End If
            If p2.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                Return p1.Number
            End If

            If p1.Gender = net.Pokemon3D.Game.Pokemon.Genders.Female Then
                Return p1.Number
            End If
            If p2.Gender = net.Pokemon3D.Game.Pokemon.Genders.Female Then
                Return p2.Number
            End If
        End If

        Return 0
    End Function

    Private Shared Function GetEggPokeballID(ByVal Pokemon As List(Of Pokemon)) As Integer
        Dim ballID As Integer = 5

        If Pokemon.Count = 2 Then
            Dim p1 As Pokemon = Pokemon(0)
            Dim p2 As Pokemon = Pokemon(1)

            'First Pokémon is Ditto.
            If p1.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p1.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                'If first Pokémon is  Ditto, then the other Pokémon must be female to inherit the ball.
                If p2.Gender = net.Pokemon3D.Game.Pokemon.Genders.Female Then
                    ballID = p2.CatchBall.ID
                End If
            End If
            'Second Pokémon is Ditto.
            If p2.EggGroup1 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Or p2.EggGroup2 = net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                'If second Pokémon is  Ditto, then the other Pokémon must be female to inherit the ball.
                If p1.Gender = net.Pokemon3D.Game.Pokemon.Genders.Female Then
                    ballID = p1.CatchBall.ID
                End If
            End If
            'No Pokémon is Ditto:
            If p1.EggGroup1 <> net.Pokemon3D.Game.Pokemon.EggGroups.Ditto And p1.EggGroup2 <> net.Pokemon3D.Game.Pokemon.EggGroups.Ditto And p2.EggGroup1 <> net.Pokemon3D.Game.Pokemon.EggGroups.Ditto And p2.EggGroup2 <> net.Pokemon3D.Game.Pokemon.EggGroups.Ditto Then
                'First Pokémon is female:
                If p1.Gender = net.Pokemon3D.Game.Pokemon.Genders.Female Then
                    ballID = p1.CatchBall.ID
                End If
                'Second Pokémon is female:
                If p2.Gender = net.Pokemon3D.Game.Pokemon.Genders.Female Then
                    ballID = p2.CatchBall.ID
                End If
            End If

            'Check for: Masterball, Cherish Ball: Set to Pokéball
            If ballID = 1 Or ballID = 45 Then
                ballID = 5
            End If
        End If

        'Return BallID (Standard is 5 for Pokéball)
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
                            Dim p As Pokemon = net.Pokemon3D.Game.Pokemon.GetPokemonByData(PokemonData)

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
                        Dim parentID As Integer = GetEggPokemonID(Pokemon)

                        Dim newEggID As Integer = net.Pokemon3D.Game.Pokemon.GetPokemonByID(parentID).EggPokemon
                        Dim s As String = DaycareID.ToString() & "|Egg|" & newEggID.ToString()

                        Logger.Debug("Egg created!" & vbNewLine & "EggID: " & newEggID)
                        TriggerCall(DaycareID)

                        Dim oldData As String = Core.Player.DaycareData
                        If oldData <> "" Then
                            oldData &= vbNewLine
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