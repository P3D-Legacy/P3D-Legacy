Public Class Script

    Public Enum ScriptTypes As Integer
        'V1:
        Move = 0
        MoveAsync = 1
        MovePlayer = 2
        Turn = 3
        TurnPlayer = 4
        Warp = 5
        WarpPlayer = 6
        Heal = 7
        ViewPokemonImage = 8
        GiveItem = 9
        RemoveItem = 10
        GetBadge = 11

        Pokemon = 12
        NPC = 13
        Player = 14
        Text = 15
        Options = 16
        SelectCase = 17
        Wait = 18
        Camera = 19
        Battle = 20
        Script = 21
        Trainer = 22
        Achievement = 23
        Action = 24
        Music = 25
        Sound = 26
        Register = 27
        Unregister = 28
        MessageBulb = 29
        Entity = 30
        Environment = 31
        Value = 32
        Level = 33

        SwitchWhen = 34
        SwitchEndWhen = 35
        SwitchIf = 36
        SwitchThen = 37
        SwitchElse = 38
        SwitchEndIf = 39
        SwitchEnd = 40

        'V2:
        Command = 100

        [if] = 101
        [when] = 102
        [then] = 103
        [else] = 104
        [endif] = 105
        [end] = 106
        [select] = 107
        [endwhen] = 108
    End Enum

    Public ScriptV1 As New ScriptV1()
    Public ScriptV2 As New ScriptV2()

    Public ScriptLine As String = ""
    Public Level As Integer = 0

    Public Property Value() As String
        Get
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    Return ScriptV1.Value
                Case 2
                    Return ScriptV2.Value
            End Select
            Return ""
        End Get
        Set(value As String)
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    ScriptV1.Value = value
                Case 2
                    ScriptV2.Value = value
            End Select
        End Set
    End Property

    Public Property ScriptType() As ScriptTypes
        Get
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    Return CType(ScriptV1.ScriptType, ScriptTypes)
                Case 2
                    Return CType(ScriptV2.ScriptType, ScriptTypes)
            End Select
            Return 0
        End Get
        Set(value As ScriptTypes)
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    ScriptV1.ScriptType = CType(value, ScriptV1.ScriptTypes)
                Case 2
                    ScriptV2.ScriptType = CType(value, ScriptV2.ScriptTypes)
            End Select
        End Set
    End Property

    Public Property started() As Boolean
        Get
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    Return ScriptV1.started
                Case 2
                    Return ScriptV2.started
            End Select
            Return False
        End Get
        Set(value As Boolean)
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    ScriptV1.started = value
                Case 2
                    ScriptV2.started = value
            End Select
        End Set
    End Property

    Public Property IsReady() As Boolean
        Get
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    Return ScriptV1.IsReady
                Case 2
                    Return ScriptV2.IsReady
            End Select
            Return False
        End Get
        Set(value As Boolean)
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    ScriptV1.IsReady = value
                Case 2
                    ScriptV2.IsReady = value
            End Select
        End Set
    End Property

    Public Property CanContinue() As Boolean
        Get
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    Return ScriptV1.CanContinue
                Case 2
                    Return ScriptV2.CanContinue
            End Select
            Return False
        End Get
        Set(value As Boolean)
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    ScriptV1.CanContinue = value
                Case 2
                    ScriptV2.CanContinue = value
            End Select
        End Set
    End Property

    Public Sub New(ByVal Line As String, ByVal Level As Integer)
        Me.Level = Level
        Me.ScriptLine = Line

        Select Case ActionScript.CSL().ScriptVersion
            Case 1
                ScriptV1.Initialize(Line)
            Case 2
                If Line <> "" Then
                    ScriptV2.Initialize(Line)
                End If
        End Select
    End Sub

    Public Sub Update()
        If Me.Level = ActionScript.ScriptLevelIndex Then
            Select Case ActionScript.CSL().ScriptVersion
                Case 1
                    ScriptV1.Update()
                Case 2
                    ScriptV2.Update()
            End Select
        Else
            Me.IsReady = True
        End If
    End Sub

    Public Shared Sub NameRival(ByVal name As String)
        Core.Player.RivalName = name
    End Sub

    Public Shared SaveNPCTrade() As String

    Public Shared Sub ExitedNPCTrade()
        Dim message2 As String = SaveNPCTrade(14)
        Screen.TextBox.Show(message2, {}, False, False)
    End Sub

    Public Shared Sub DoNPCTrade(ByVal pokeIndex As Integer)
        Core.SetScreen(Core.CurrentScreen.PreScreen)

        Dim ownPokemon As Pokemon = Core.Player.Pokemons(pokeIndex)

        Dim ownPokeID As Integer = ScriptConversion.ToInteger(Script.SaveNPCTrade(0))
        Dim oppPokeID As Integer = ScriptConversion.ToInteger(Script.SaveNPCTrade(1))

        Dim oppPokemon As Pokemon = Pokemon.GetPokemonByID(oppPokeID)

        Dim Level As Integer = ownPokemon.Level

        If IsNumeric(Script.SaveNPCTrade(2)) = True Then
            Level = ScriptConversion.ToInteger(Script.SaveNPCTrade(2))
        End If

        oppPokemon.Generate(Level, True)

        Dim Gender As Pokemon.Genders = ownPokemon.Gender

        If IsNumeric(Script.SaveNPCTrade(3)) = True Then
            Dim genderID As Integer = ScriptConversion.ToInteger(Script.SaveNPCTrade(3))
            If genderID = -1 Then
                genderID = Core.Random.Next(0, 2)
            End If

            Select Case genderID
                Case 0
                    Gender = Pokemon.Genders.Male
                Case 1
                    Gender = Pokemon.Genders.Female
                Case 2
                    Gender = Pokemon.Genders.Genderless
                Case Else
                    Gender = Pokemon.Genders.Male
            End Select
        End If

        oppPokemon.Gender = Gender

        If Script.SaveNPCTrade(4) <> "" Then
            oppPokemon.Attacks.Clear()
            Dim attacks() As String = {Script.SaveNPCTrade(4)}
            If Script.SaveNPCTrade(4).Contains(",") = True Then
                attacks = Script.SaveNPCTrade(4).Split(CChar(","))
            End If
            For Each attackID As String In attacks
                If oppPokemon.Attacks.Count < 4 Then
                    oppPokemon.Attacks.Add(BattleSystem.Attack.GetAttackByID(ScriptConversion.ToInteger(attackID)))
                End If
            Next
        End If

        If Script.SaveNPCTrade(5) <> "" Then
            oppPokemon.IsShiny = CBool(Script.SaveNPCTrade(5))
        End If

        oppPokemon.OT = Script.SaveNPCTrade(6)
        oppPokemon.CatchTrainerName = Script.SaveNPCTrade(7)
        oppPokemon.CatchBall = Item.GetItemByID(ScriptConversion.ToInteger(Script.SaveNPCTrade(8)))

        Dim itemID As String = Script.SaveNPCTrade(9)
        If IsNumeric(itemID) = True Then
            oppPokemon.Item = Item.GetItemByID(ScriptConversion.ToInteger(itemID))
        End If

        oppPokemon.CatchLocation = Script.SaveNPCTrade(10)
        oppPokemon.CatchMethod = Script.SaveNPCTrade(11)
        oppPokemon.NickName = Script.SaveNPCTrade(12)

        Dim message1 As String = Script.SaveNPCTrade(13)
        Dim message2 As String = Script.SaveNPCTrade(14)

        Dim register As String = Script.SaveNPCTrade(15)

        If ownPokeID = ownPokemon.Number Then
            Core.Player.Pokemons.RemoveAt(pokeIndex)
            Core.Player.Pokemons.Add(oppPokemon)

            Dim pokedexType As Integer = 2
            If oppPokemon.IsShiny = True Then
                pokedexType = 3
            End If

            Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, oppPokemon.Number, pokedexType)

            If register <> "" Then
                ActionScript.RegisterID(register)
            End If

            Core.Player.AddPoints(10, "Traded with NPC.")

            SoundManager.PlaySound("success_small")
            Screen.TextBox.Show(message1 & "*" & Core.Player.Name & " traded~" & oppPokemon.OriginalName & " for~" & ownPokemon.OriginalName & "!", {}, False, False)
        Else
            Screen.TextBox.Show(message2, {}, False, False)
        End If
    End Sub

    Public Function Clone() As Script
        Return New Script(Me.ScriptLine, Me.Level)
    End Function

    Public Shared Function ParseArguments(ByVal inputString As String, Optional ByVal SeparatorChar As Char = CChar(",")) As List(Of String)
        Dim arguments As New List(Of String)
        Dim stringDeclaration As Boolean = False
        Dim data As String = inputString
        Dim cArg As String = ""

        While data.Length > 0
            Select Case data(0)
                Case SeparatorChar
                    If stringDeclaration = True Then
                        cArg &= data(0).ToString()
                    Else
                        arguments.Add(cArg)
                        cArg = ""
                    End If
                Case CChar("""")
                    If stringDeclaration = False Then
                        stringDeclaration = True
                    Else
                        If data.Length = 1 OrElse data(1) <> data(0) Then
                            stringDeclaration = Not stringDeclaration
                        ElseIf data.Length > 1 AndAlso data(1) = data(0) Then
                            If stringDeclaration = True Then
                                cArg &= """"
                            End If
                            data = data.Remove(0, 1)
                        End If
                    End If
                Case Else
                    cArg &= data(0).ToString()
            End Select

            data = data.Remove(0, 1)
        End While

        arguments.Add(cArg)

        Return arguments
    End Function

End Class