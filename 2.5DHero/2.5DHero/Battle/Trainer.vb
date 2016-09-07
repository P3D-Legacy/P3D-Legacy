Imports net.Pokemon3D.Game.ScriptVersion2

Public Class Trainer

    Public AILevel As Integer = 0
    Public SignatureMoves As New List(Of BattleSystem.Attack)
    Public Pokemons As New List(Of Pokemon)
    Public TrainerType As String = "Youngster"
    Public TrainerType2 As String = "Youngster"
    Public Name As String = "Joey"
    Public Name2 As String = "Joey"
    Public Money As Integer = 84
    Public SpriteName As String = "14"
    Public SpriteName2 As String = "14"
    Public Region As String = "Johto"
    Public Music As String = "Trainer"
    Public TrainerFile As String = ""
    Public DoubleTrainer As Boolean = False
    Public Items As New List(Of Item)
    Public Gender As Integer = -1
    Public IntroType As Integer = 10
    Public GameJoltID As String = ""

    Public VSImageOrigin As String = "VSIntro"
    Public VSImagePosition As Vector2 = New Vector2(0, 0)
    Public VSImageSize As Size = New Size(61, 54)
    Public BarImagePosition As Vector2 = New Vector2(0, 0)

    Public OutroMessage As String = "TRAINER_DEFAULT_MESSAGE"
    Public OutroMessage2 As String = "TRAINER_DEFAULT_MESSAGE"

    Public IntroMessage As String = "TRAINER_DEFAULT_MESSAGE"
    Public DefeatMessage As String = "TRAINER_DEFAULT_MESSAGE"

    Public Shared FrontierTrainer As Integer = -1

    Public Function IsBeaten() As Boolean
        Return ActionScript.IsRegistered("trainer_" & TrainerFile)
    End Function

    Public Shared Function IsBeaten(ByVal CheckTrainerFile As String) As Boolean
        Return ActionScript.IsRegistered("trainer_" & CheckTrainerFile)
    End Function

    Public Sub New()
    End Sub

    Public Sub New(ByVal TrainerFile As String)
        Me.TrainerFile = TrainerFile

        Dim path As String = GameModeManager.GetScriptPath("Trainer\" & TrainerFile & ".trainer")
        Security.FileValidation.CheckFileValid(path, False, "Trainer.vb")

        Dim Data() As String = System.IO.File.ReadAllLines(path)

        If Data(0) = "[TRAINER FORMAT]" Then
            LoadTrainer(Data)
        Else
            LoadTrainerLegacy(Data)
        End If
    End Sub

    Private Sub LoadTrainerLegacy(ByVal Data() As String)
        Dim newData As New List(Of String)
        Dim sevenData As List(Of String) = Data(7).Split(CChar("|")).ToList()

        newData.Add("Name|" & Data(2))
        newData.Add("TrainerClass|" & Data(1))
        newData.Add("Money|" & Data(0))
        newData.Add("IntroMessage|" & Data(3))
        newData.Add("OutroMessage|" & Data(4))
        newData.Add("DefeatMessage|" & Data(5))
        newData.Add("TextureID|" & Data(6))
        newData.Add("Region|" & sevenData(0))

        Me.Region = sevenData(0)
        Me.Music = sevenData(1)

        newData.Add("IniMusic|" & GetIniMusicName())
        newData.Add("DefeatMusic|" & GetDefeatMusic())
        newData.Add("BattleMusic|" & GetBattleMusicName())

        newData.Add("Pokemon1|" & Data(8).Remove(0, 2))
        newData.Add("Pokemon2|" & Data(9).Remove(0, 2))
        newData.Add("Pokemon3|" & Data(10).Remove(0, 2))
        newData.Add("Pokemon4|" & Data(11).Remove(0, 2))
        newData.Add("Pokemon5|" & Data(12).Remove(0, 2))
        newData.Add("Pokemon6|" & Data(13).Remove(0, 2))

        If Data.Length > 14 Then
            newData.Add("Items|" & Data(14))
        End If
        If Data.Length > 15 Then
            newData.Add("AI|" & Data(15))
        End If
        If Data.Length > 16 Then
            newData.Add("Gender|" & Data(16))
        End If

        Dim sequenceData As String = "Blue,Blue"
        If sevenData.Count = 3 Then
            sequenceData = sevenData(2) & ",Blue"
        ElseIf sevenData.Count > 3 Then
            sequenceData = sevenData(2) & "," & sevenData(3)
        End If
        newData.Add("IntroSequence|" & sequenceData)

        Logger.Log(Logger.LogTypes.Warning, "Trainer.vb: Converted legacy trainer file! Generated new trainer data:")
        Logger.Log(Logger.LogTypes.Message, newData.ToArray().ArrayToString())

        LoadTrainer(newData.ToArray())
    End Sub

    Private Sub LoadTrainer(ByVal Data() As String)
        Dim PokeLines As New List(Of String)
        Dim isDoubleTrainerValid As Integer = 0
        Dim vsdata As String = "blue"
        Dim bardata As String = "blue"

        For Each line As String In Data
            If line.Contains("|") = True Then
                Dim pointer As String = line.Remove(line.IndexOf("|"))
                Dim value As String = line.Remove(0, line.IndexOf("|") + 1)

                Select Case pointer.ToLower()
                    Case "name"
                        Me.Name = ScriptCommander.Parse(value).ToString()
                        If Me.Name.Contains(",") = True Then
                            Me.Name2 = Me.Name.GetSplit(1)
                            Me.Name = Me.Name.GetSplit(0)

                            isDoubleTrainerValid += 1
                        End If
                    Case "trainerclass"
                        Me.TrainerType = ScriptCommander.Parse(value).ToString()
                        If Me.TrainerType.Contains(",") = True Then
                            Me.TrainerType2 = TrainerType.GetSplit(1)
                            Me.TrainerType = TrainerType.GetSplit(0)

                            isDoubleTrainerValid += 1
                        End If
                    Case "money"
                        Me.Money = CInt(ScriptCommander.Parse(value).ToString())
                    Case "intromessage"
                        Me.IntroMessage = ScriptCommander.Parse(value).ToString()
                    Case "outromessage"
                        Me.OutroMessage = ScriptCommander.Parse(value).ToString()
                        If Me.OutroMessage.Contains("|") = True Then
                            Me.OutroMessage2 = OutroMessage.GetSplit(1, "|")
                            Me.OutroMessage = OutroMessage.GetSplit(0, "|")

                            isDoubleTrainerValid += 1
                        End If
                    Case "defeatmessage"
                        Me.DefeatMessage = ScriptCommander.Parse(value).ToString()
                    Case "textureid"
                        Me.SpriteName = ScriptCommander.Parse(value).ToString()
                        If Me.SpriteName.Contains(",") = True Then
                            Me.SpriteName2 = Me.SpriteName.GetSplit(1)
                            Me.SpriteName = Me.SpriteName.GetSplit(0)

                            isDoubleTrainerValid += 1
                        End If
                    Case "region"
                        Me.Region = ScriptCommander.Parse(value).ToString()
                    Case "inimusic"
                        Me.IniMusic = ScriptCommander.Parse(value).ToString()
                    Case "defeatmusic"
                        Me.DefeatMusic = ScriptCommander.Parse(value).ToString()
                    Case "battlemusic"
                        Me.BattleMusic = ScriptCommander.Parse(value).ToString()
                    Case "insightmusic"
                        Me.InSightMusic = ScriptCommander.Parse(value).ToString()
                    Case "pokemon1", "pokemon2", "pokemon3", "pokemon4", "pokemon5", "pokemon6"
                        If value <> "" Then
                            PokeLines.Add(value)
                        End If
                    Case "items"
                        If value <> "" Then
                            Dim itemData() As String = ScriptCommander.Parse(value).ToString().Split(CChar(","))
                            For Each ItemID As String In itemData
                                Items.Add(Item.GetItemByID(CInt(ItemID)))
                            Next
                        End If
                    Case "gender"
                        Dim GenderInt As Integer = CInt(ScriptCommander.Parse(value).ToString())

                        Me.Gender = CInt(MathHelper.Clamp(GenderInt, -1, 1))
                    Case "ai"
                        Me.AILevel = CInt(ScriptCommander.Parse(value).ToString())
                    Case "introsequence"
                        value = ScriptCommander.Parse(value).ToString()

                        If value.Contains(",") = True Then
                            vsdata = value.GetSplit(0)
                            bardata = value.GetSplit(1)
                        Else
                            vsdata = value
                        End If
                    Case "introtype"
                        Me.IntroType = CInt(ScriptCommander.Parse(value).ToString())
                End Select
            End If
        Next

        For Each PokeLine As String In PokeLines
            Dim PokeData As String = PokeLine.GetSplit(1, "|")
            If PokeData <> "" Then
                If ScriptCommander.Parse(PokeData).ToString().StartsWith("{") = True Then
                    PokeData = ScriptCommander.Parse(PokeData).ToString().Replace("§", ",")
                End If
                If PokeData.StartsWith("{") = True And PokeData.EndsWith("}") = True Then
                    Dim p As Pokemon = Pokemon.GetPokemonByData(PokeData)

                    If Core.Player.DifficultyMode > 0 Then
                        Dim level As Integer = p.Level

                        Dim addLevel As Integer = 0
                        If Core.Player.DifficultyMode = 1 Then
                            addLevel = CInt(Math.Ceiling(level / 10))
                        ElseIf Core.Player.DifficultyMode = 2 Then
                            addLevel = CInt(Math.Ceiling(level / 5))
                        End If

                        While level + addLevel > p.Level
                            p.LevelUp(False)
                            p.Experience = p.NeedExperience(p.Level)
                        End While
                        p.HP = p.MaxHP
                    End If

                    Pokemons.Add(p)
                Else
                    Dim firstPart As String = ""
                    Dim secondPart As String = ""
                    Dim endedFirstPart As Boolean = False
                    Dim readData As String = PokeData
                    Dim openTag As Boolean = False

                    While readData.Length > 0
                        Select Case readData(0).ToString()
                            Case "<"
                                openTag = True
                            Case ">"
                                openTag = False
                            Case ","
                                If openTag = False Then
                                    endedFirstPart = True
                                End If
                        End Select

                        If readData(0).ToString() <> "," Or openTag = True Then
                            If endedFirstPart = True Then
                                secondPart &= readData(0).ToString()
                            Else
                                firstPart &= readData(0).ToString()
                            End If
                        End If

                        readData = readData.Remove(0, 1)
                    End While

                    Dim ID As Integer = ScriptConversion.ToInteger(ScriptCommander.Parse(firstPart))
                    Dim Level As Integer = ScriptConversion.ToInteger(ScriptCommander.Parse(secondPart))

                    Dim addLevel As Integer = 0
                    If Core.Player.DifficultyMode = 1 Then
                        addLevel = CInt(Math.Ceiling(Level / 10))
                    ElseIf Core.Player.DifficultyMode = 2 Then
                        addLevel = CInt(Math.Ceiling(Level / 5))
                    End If
                    Level += addLevel

                    Dim maxLevel As Integer = CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100"))

                    If Level > maxLevel Then
                        Level = maxLevel
                    End If

                    Dim p As Pokemon = Nothing

                    If FrontierTrainer > -1 Then
                        p = FrontierSpawner.GetPokemon(Level, FrontierTrainer, Nothing)
                    Else
                        p = Pokemon.GetPokemonByID(ID)
                        p.Generate(Level, True)
                    End If

                    If p.IsGenderless = False Then
                        Select Case Me.Gender
                            Case 0
                                If p.IsMale > 0.0F Then
                                    p.Gender = Pokemon.Genders.Male
                                End If
                            Case 1
                                If p.IsMale < 100.0F Then
                                    p.Gender = Pokemon.Genders.Female
                                End If
                        End Select
                    End If

                    Pokemons.Add(p)
                End If
            End If
        Next

        If isDoubleTrainerValid = 4 Then
            Me.DoubleTrainer = True
        End If

        SetIniImage(vsdata, bardata)
        FrontierTrainer = -1
    End Sub

    Private Sub SetIniImage(ByVal vsType As String, ByVal barType As String)
        Select Case vsType.ToLower()
            Case "blue", "0"
                Me.VSImagePosition = New Vector2(0, 0)
            Case "orange", "1"
                Me.VSImagePosition = New Vector2(1, 0)
            Case "green", "2"
                Me.VSImagePosition = New Vector2(0, 1)
            Case "3"
                Me.VSImagePosition = New Vector2(1, 1)
            Case "4"
                Me.VSImagePosition = New Vector2(0, 2)
            Case "5"
                Me.VSImagePosition = New Vector2(1, 2)
            Case "6"
                Me.VSImagePosition = New Vector2(0, 3)
            Case "7"
                Me.VSImagePosition = New Vector2(1, 3)
            Case "8"
                Me.VSImagePosition = New Vector2(0, 4)
            Case "9"
                Me.VSImagePosition = New Vector2(1, 4)
            Case "red", "10"
                Me.VSImagePosition = New Vector2(0, 5)
            Case "11"
                Me.VSImagePosition = New Vector2(1, 5)
            Case "battlefrontier"
                Me.VSImagePosition = New Vector2(0, 0)
                Me.VSImageOrigin = "battlefrontier"
                Me.VSImageSize = New Size(275, 275)
            Case Else
                If IsNumeric(vsType) = True Then
                    If CInt(vsType) > 11 Then
                        Dim x As Integer = CInt(vsType)
                        Dim y As Integer = 0
                        While x > 1
                            x -= 2
                            y += 1
                        End While
                        Me.VSImagePosition = New Vector2(x, y)
                    End If
                End If
        End Select
        Select Case barType.ToLower()
            Case "blue", "0"
                Me.BarImagePosition = New Vector2(0, 0)
            Case "orange", "1"
                Me.BarImagePosition = New Vector2(1, 0)
            Case "lightgreen", "2"
                Me.BarImagePosition = New Vector2(0, 1)
            Case "gray", "3"
                Me.BarImagePosition = New Vector2(1, 1)
            Case "violet", "4"
                Me.BarImagePosition = New Vector2(0, 2)
            Case "green", "5"
                Me.BarImagePosition = New Vector2(1, 2)
            Case "yellow", "6"
                Me.BarImagePosition = New Vector2(0, 3)
            Case "brown", "7"
                Me.BarImagePosition = New Vector2(1, 3)
            Case "lightblue", "8"
                Me.BarImagePosition = New Vector2(0, 4)
            Case "lightgray", "9"
                Me.BarImagePosition = New Vector2(1, 4)
            Case "red", "10"
                Me.BarImagePosition = New Vector2(0, 5)
            Case "11"
                Me.BarImagePosition = New Vector2(1, 5)
            Case Else
                If IsNumeric(barType) = True Then
                    If CInt(barType) > 11 Then
                        Dim x As Integer = CInt(barType)
                        Dim y As Integer = 0
                        While x > 1
                            x -= 2
                            y += 1
                        End While
                        Me.BarImagePosition = New Vector2(x, y)
                    End If
                End If
        End Select
    End Sub

    Private IniMusic As String = ""
    Private DefeatMusic As String = ""
    Private BattleMusic As String = ""
    Private InSightMusic As String = "trainer_encounter"

    Public Function GetIniMusicName() As String
        If IniMusic <> "" Then
            Return IniMusic
        End If

        Dim middle As String = "trainer"

        Select Case Me.Music.ToLower()
            Case "rival"
                middle = "rival"
            Case "leader"
                middle = "leader"
            Case "rocket"
                middle = "rocket"
        End Select

        Return Region & "_" & middle & "_intro"
    End Function

    Public Function GetDefeatMusic() As String
        If DefeatMusic <> "" Then
            Return DefeatMusic
        End If

        Dim pre As String = "trainer"

        Select Case Me.Music.ToLower()
            Case "leader"
                pre = "leader"
        End Select

        Return pre & "_defeat"
    End Function

    Public Function GetBattleMusicName() As String
        If BattleMusic <> "" Then
            Return BattleMusic
        End If

        Return Me.Region.ToLower() & "_" & Me.Music.ToLower()
    End Function

    Public Function GetInSightMusic() As String
        Return InSightMusic
    End Function

    Public Function HasBattlePokemon() As Boolean
        For Each Pokemon As Pokemon In Pokemons
            If Pokemon.Status <> net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted And Pokemon.HP > 0 Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Sub TrainerItemUse(ByVal ItemID As Integer)
        For i = 0 To Items.Count - 1
            Dim item As Item = Items(i)
            If item.ID = ItemID Then
                Me.Items.RemoveAt(i)
                Exit For
            End If
        Next
    End Sub

    Public Function CountUseablePokemon() As Integer
        Dim i As Integer = 0

        For Each p As Pokemon In Me.Pokemons
            If p.HP > 0 And p.Status <> Pokemon.StatusProblems.Fainted Then
                i += 1
            End If
        Next

        Return i
    End Function

End Class