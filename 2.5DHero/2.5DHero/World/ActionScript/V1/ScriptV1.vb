Public Class ScriptV1

    Public Enum ScriptTypes As Integer
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
        Level = 33

        SwitchWhen = 34
        SwitchEndWhen = 35
        SwitchIf = 36
        SwitchThen = 37
        SwitchElse = 38
        SwitchEndIf = 39
        SwitchEnd = 40
    End Enum

    Public ScriptType As ScriptTypes = ScriptTypes.Text

    Public Value As String = ""

    Public started As Boolean = False
    Public IsReady As Boolean = False
    Public CanContinue As Boolean = False

    Public Sub Initialize(ByVal Line As String)
        If Line.StartsWith("@") = True Then
            Line = Line.Remove(0, 1)

            Dim Script As String = Line
            Dim Command As String = ""

            If Line.Contains(":") = True Then
                Script = Line.Remove(Line.IndexOf(":"))
                Command = Line.Remove(0, Line.IndexOf(":") + 1)
            End If

            Select Case Script
                Case "Move"
                    If Command.StartsWith("Async,") = True Then
                        Command = Command.Remove(0, 6)
                        Me.ScriptType = ScriptTypes.MoveAsync
                    ElseIf Command.StartsWith("Player,") = True Then
                        Command = Command.Remove(0, 7)
                        Me.ScriptType = ScriptTypes.MovePlayer
                    Else
                        Me.ScriptType = ScriptTypes.Move
                    End If

                    Me.Value = Command
                Case "Turn"
                    If Command.StartsWith("Player,") = True Then
                        Command = Command.Remove(0, 7)
                        Me.ScriptType = ScriptTypes.TurnPlayer
                    Else
                        Me.ScriptType = ScriptTypes.Turn
                    End If

                    Me.Value = Command
                Case "Warp"
                    If Command.StartsWith("Player,") = True Then
                        Command = Command.Remove(0, 7)
                        Me.ScriptType = ScriptTypes.WarpPlayer
                    Else
                        Me.ScriptType = ScriptTypes.Warp
                    End If

                    Me.Value = Command
                Case "Heal"
                    Me.ScriptType = ScriptTypes.Heal
                    Me.Value = Command
                Case "ViewPokemonImage"
                    Me.ScriptType = ScriptTypes.ViewPokemonImage
                    Me.Value = Command
                Case "GiveItem"
                    Me.ScriptType = ScriptTypes.GiveItem
                    Me.Value = Command
                Case "RemoveItem"
                    Me.ScriptType = ScriptTypes.RemoveItem
                    Me.Value = Command
                Case "GetBadge"
                    Me.ScriptType = ScriptTypes.GetBadge
                    Me.Value = Command



                Case "Action"
                    Me.ScriptType = ScriptTypes.Action
                    Me.Value = Command
                Case "Music"
                    Me.ScriptType = ScriptTypes.Music
                    Me.Value = Command
                Case "Sound"
                    Me.ScriptType = ScriptTypes.Sound
                    Me.Value = Command
                Case "Text"
                    Me.ScriptType = ScriptTypes.Text
                    Me.Value = Command
                Case "Options"
                    Me.ScriptType = ScriptTypes.Options
                    Me.Value = Command
                Case "Wait"
                    Me.ScriptType = ScriptTypes.Wait
                    Me.Value = Command
                Case "Register"
                    Me.ScriptType = ScriptTypes.Register
                    Me.Value = Command
                Case "Unregister"
                    Me.ScriptType = ScriptTypes.Unregister
                    Me.Value = Command
                Case "NPC"
                    Me.ScriptType = ScriptTypes.NPC
                    Me.Value = Command
                Case "Achievement"
                    Me.ScriptType = ScriptTypes.Achievement
                    Me.Value = Command
                Case "Trainer"
                    Me.ScriptType = ScriptTypes.Trainer
                    Me.Value = Command
                Case "Battle"
                    Me.ScriptType = ScriptTypes.Battle
                    Me.Value = Command
                Case "Script"
                    Me.ScriptType = ScriptTypes.Script
                    Me.Value = Command
                Case "Bulb", "MessageBulb"
                    Me.ScriptType = ScriptTypes.MessageBulb
                    Me.Value = Command
                Case "Camera"
                    Me.ScriptType = ScriptTypes.Camera
                    Me.Value = Command
                Case "Pokemon"
                    Me.ScriptType = ScriptTypes.Pokemon
                    Me.Value = Command
                Case "Player"
                    Me.ScriptType = ScriptTypes.Player
                    Me.Value = Command
                Case "Entity"
                    Me.ScriptType = ScriptTypes.Entity
                    Me.Value = Command
                Case "Environment"
                    Me.ScriptType = ScriptTypes.Environment
                    Me.Value = Command
                Case "Level"
                    Me.ScriptType = ScriptTypes.Level
                    Me.Value = Command
            End Select
        ElseIf Line.StartsWith(":") = True Then
            Line = Line.Remove(0, 1)

            Dim Script As String = ""
            Dim Command As String = ""

            If Line.Contains(":") = True Then
                Script = Line.Remove(Line.IndexOf(":"))
                Command = Line.Remove(0, Line.IndexOf(":") + 1)
            Else
                Script = Line
                Command = ""
            End If

            Select Case Script
                Case "if"
                    Me.ScriptType = ScriptTypes.SwitchIf
                    Me.Value = Command
                Case "when"
                    Me.ScriptType = ScriptTypes.SwitchWhen
                    Me.Value = Command
                Case "then"
                    Me.ScriptType = ScriptTypes.SwitchThen
                Case "else"
                    Me.ScriptType = ScriptTypes.SwitchElse
                Case "endif"
                    Me.ScriptType = ScriptTypes.SwitchEndIf
                Case "endwhen"
                    Me.ScriptType = ScriptTypes.SwitchEndWhen
                Case "end"
                    Me.ScriptType = ScriptTypes.SwitchEnd
                Case "select"
                    Me.ScriptType = ScriptTypes.SelectCase
                    Me.Value = Command
            End Select
        End If
    End Sub

    Public Sub Update()
        Select Case Me.ScriptType
            Case ScriptTypes.Text
                Me.DoText()
            Case ScriptTypes.Wait
                Me.DoWait()
            Case ScriptTypes.Move
                Me.Move()
            Case ScriptTypes.MoveAsync
                Me.MoveAsync()
            Case ScriptTypes.MovePlayer
                Me.MovePlayer()
            Case ScriptTypes.Register
                Me.Register()
            Case ScriptTypes.Unregister
                Me.Unregister()
            Case ScriptTypes.Turn
                Me.Turn()
            Case ScriptTypes.TurnPlayer
                Me.TurnPlayer()
            Case ScriptTypes.Warp
                Me.Warp()
            Case ScriptTypes.WarpPlayer
                Me.WarpPlayer()
            Case ScriptTypes.Heal
                Me.Heal()
            Case ScriptTypes.Action
                Me.DoAction()
            Case ScriptTypes.Music
                Me.DoMusic()
            Case ScriptTypes.Sound
                Me.DoSound()
            Case ScriptTypes.ViewPokemonImage
                Me.ViewPokemonImage()
            Case ScriptTypes.NPC
                Me.DoNPC()
            Case ScriptTypes.Achievement
                Me.GetAchievement()
            Case ScriptTypes.GiveItem
                Me.GiveItem()
            Case ScriptTypes.RemoveItem
                Me.RemoveItem()
            Case ScriptTypes.Trainer
                Me.DoTrainerBattle()
            Case ScriptTypes.Battle
                Me.DoBattle()
            Case ScriptTypes.Script
                Me.DoScript()
            Case ScriptTypes.MessageBulb
                Me.DoMessageBulb()
            Case ScriptTypes.Camera
                Me.DoCamera()
            Case ScriptTypes.GetBadge
                Me.GetBadge()
            Case ScriptTypes.Pokemon
                Me.DoPokemon()
            Case ScriptTypes.Player
                Me.DoPlayer()
            Case ScriptTypes.Entity
                Me.DoEntity()
            Case ScriptTypes.Environment
                Me.DoEnvironment()
            Case ScriptTypes.Level
                Me.DoLevel()

            Case ScriptTypes.SwitchIf
                Me.DoIf()
            Case ScriptTypes.SwitchThen
                Me.IsReady = True
            Case ScriptTypes.SwitchElse
                Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
                oS.ActionScript.ChooseIf(True)

                Me.IsReady = True
            Case ScriptTypes.SwitchEndIf
                Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
                oS.ActionScript.ChooseIf(True)

                Me.IsReady = True

            Case ScriptTypes.SwitchWhen
                If ActionScript.CSL().WaitingEndWhen(ActionScript.CSL().WhenIndex) = True Then
                    Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
                    oS.ActionScript.Switch("")
                End If
                Me.IsReady = True
            Case ScriptTypes.SwitchEndWhen
                Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
                oS.ActionScript.Switch("")
                Me.IsReady = True

            Case ScriptTypes.SwitchEnd
                Me.EndScript()
            Case ScriptTypes.Options
                Me.DoOptions()
            Case ScriptTypes.SelectCase
                Me.DoSelect()
        End Select
    End Sub

#Region "NewScripts"

    Public Sub EndScript()
        ActionScript.ScriptLevelIndex -= 1
        If ActionScript.ScriptLevelIndex = -1 Then
            Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
            oS.ActionScript.Scripts.Clear()
            oS.ActionScript.reDelay = 1.0F
            Me.IsReady = True
            Screen.TextBox.reDelay = 1.0F
            ActionScript.TempInputDirection = -1
        End If
    End Sub

    Private Sub Register()
        ActionScript.RegisterID(Me.Value)

        Me.IsReady = True
    End Sub

    Private Sub Unregister()
        ActionScript.UnregisterID(Me.Value)

        Me.IsReady = True
    End Sub

    Private Sub GetAchievement()
        Dim IndiciesData As String = Value.GetSplit(0, "|")
        IndiciesData = IndiciesData.Remove(0, 1)
        IndiciesData = IndiciesData.Remove(IndiciesData.Length - 1, 1)
        Dim StringIndicies() As String = IndiciesData.Split(CChar(","))
        Dim Indicies As New List(Of Integer)
        For i = 0 To StringIndicies.Count - 1
            Indicies.Add(CInt(StringIndicies(i)))
        Next

        Dim aInput As String = Value.GetSplit(1, "|")

        Me.IsReady = True
    End Sub

    Private Sub DoScript()
        If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(Value, 0)
        End If
    End Sub

    Private Sub DoCamera()
        Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)

        If c.ThirdPerson = True Then
            Dim action As String = Value.GetSplit(0)
            Select Case action.ToLower()
                Case "set"
                    Dim x As Single = CSng(Value.GetSplit(1).Replace(".", GameController.DecSeparator))
                    Dim y As Single = CSng(Value.GetSplit(2).Replace(".", GameController.DecSeparator))
                    Dim z As Single = CSng(Value.GetSplit(3).Replace(".", GameController.DecSeparator))
                    Dim yaw As Single = CSng(Value.GetSplit(4).Replace(".", GameController.DecSeparator))
                    Dim pitch As Single = CSng(Value.GetSplit(5).Replace(".", GameController.DecSeparator))

                    c.ThirdPersonOffset = New Vector3(x, y, z)
                    c.Yaw = yaw
                    c.Pitch = pitch
                Case "reset"
                    c.ThirdPersonOffset = New Vector3(0.0F, 0.3F, 1.5F)
                Case "yaw"
                    Dim yaw As Single = CSng(Value.GetSplit(1).Replace(",", ".").Replace(".", GameController.DecSeparator))

                    c.Yaw = yaw
                Case "pitch"
                    Dim pitch As Single = CSng(Value.GetSplit(1).Replace(",", ".").Replace(".", GameController.DecSeparator))

                    c.Pitch = pitch
                Case "position"
                    Dim x As Single = CSng(Value.GetSplit(1).Replace(".", GameController.DecSeparator))
                    Dim y As Single = CSng(Value.GetSplit(2).Replace(".", GameController.DecSeparator))
                    Dim z As Single = CSng(Value.GetSplit(3).Replace(".", GameController.DecSeparator))

                    c.ThirdPersonOffset = New Vector3(x, y, z)
                Case "x"
                    Dim x As Single = CSng(Value.GetSplit(1).Replace(".", GameController.DecSeparator))

                    c.ThirdPersonOffset.X = x
                Case "y"
                    Dim y As Single = CSng(Value.GetSplit(1).Replace(".", GameController.DecSeparator))

                    c.ThirdPersonOffset.Y = y
                Case "z"
                    Dim z As Single = CSng(Value.GetSplit(1).Replace(".", GameController.DecSeparator))

                    c.ThirdPersonOffset.Z = z
            End Select

            c.UpdateThirdPersonCamera()
            c.UpdateFrustum()
            c.UpdateViewMatrix()
            Screen.Level.UpdateEntities()
            Screen.Level.Entities = (From e In Screen.Level.Entities Order By e.CameraDistance Descending).ToList()
            Screen.Level.UpdateEntities()
        End If

        Me.IsReady = True
    End Sub

    Private Sub DoText()
        Screen.TextBox.reDelay = 0.0F
        Screen.TextBox.Show(Me.Value, {})
        Me.IsReady = True
    End Sub

    Private Sub DoSelect()
        Dim condition As String = ""
        Dim check As String = Value

        If Value.Contains("(") = True And Value.Contains(")") = True Then
            condition = Value.Remove(0, Value.IndexOf("(") + 1)
            condition = condition.Remove(condition.LastIndexOf(")"))
            check = Value.Remove(Value.IndexOf("("))
        End If

        Select Case check.ToLower()
            Case "random"
                check = CStr(Core.Random.Next(1, CInt(condition) + 1))
        End Select

        Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)

        ActionScript.CSL().WhenIndex += 1

        oS.ActionScript.Switch(check)

        Me.IsReady = True
    End Sub

    Private Sub DoOptions()
        Screen.TextBox.Showing = True
        Dim Options() As String = Me.Value.Split(CChar(","))

        For i = 0 To Options.Count - 1
            If i <= Options.Count - 1 Then
                Dim flag = Options(i)
                Dim removeFlag As Boolean = False

                Select Case flag
                    Case "[TEXT=FALSE]"
                        removeFlag = True
                        Screen.TextBox.Showing = False
                End Select

                If removeFlag = True Then
                    Dim l As List(Of String) = Options.ToList()
                    l.RemoveAt(i)
                    Options = l.ToArray()
                    i -= 1
                End If
            End If
        Next
        Screen.ChooseBox.Show(Options, 0, True)

        ActionScript.CSL().WhenIndex += 1

        Me.IsReady = True
    End Sub

    Private Sub DoIf()
        Dim condition As String = ""
        Dim check As String = Value

        If Value.Contains("(") = True And Value.Contains(")") = True Then
            condition = Value.Remove(0, Value.IndexOf("(") + 1)
            condition = condition.Remove(condition.LastIndexOf(")"))
            check = Value.Remove(Value.IndexOf("("))
        End If

        Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)

        Dim T As Boolean = False
        Dim inverse As Boolean = False

        If check.StartsWith("not ") = True Then
            check = check.Remove(0, 4)
            inverse = True
        End If

        Select Case check.ToLower()
            Case "register"
                T = ActionScript.IsRegistered(condition)
            Case "daytime"
                If CInt(condition) = World.GetTime() Then
                    T = True
                End If
            Case "freeplaceinparty"
                If Core.Player.Pokemons.Count < 6 Then
                    T = True
                End If
            Case "season"
                If CInt(condition) = CInt(World.CurrentSeason) Then
                    T = True
                End If
            Case "nopokemon"
                If Core.Player.Pokemons.Count = 0 Then
                    T = True
                End If
            Case "countpokemon"
                If Core.Player.Pokemons.Count = CInt(condition) Then
                    T = True
                End If
            Case "day"
                If CInt(condition) = My.Computer.Clock.LocalTime.DayOfWeek Then
                    T = True
                End If
            Case "aurora"
                If condition = "0" Then
                    T = Not World.IsAurora
                Else
                    T = World.IsAurora
                End If
            Case "random"
                If Core.Random.Next(0, CInt(condition)) = 0 Then
                    T = True
                End If
            Case "position"
                Dim PositionValues() As String = condition.Split(CChar(","))
                Dim checkPosition As New Vector3(CInt(Screen.Camera.Position.X), CInt(Screen.Camera.Position.Y), CInt(Screen.Camera.Position.Z))

                If PositionValues(0).ToLower() <> "player" Then
                    Dim targetID As Integer = CInt(PositionValues(0))
                    checkPosition = Screen.Level.GetNPC(targetID).Position
                End If

                Dim p As New Vector3(CSng(PositionValues(1)), CSng(PositionValues(2)), CSng(PositionValues(3)))

                If p = checkPosition Then
                    T = True
                Else
                    T = False
                End If
            Case "weather"
                If CInt(condition) = Screen.Level.World.CurrentMapWeather Then
                    T = True
                Else
                    T = False
                End If
            Case "regionweather"
                If CInt(condition) = World.GetCurrentRegionWeather() Then
                    T = True
                Else
                    T = False
                End If
            Case "hasbadge"
                If Core.Player.Badges.Contains(CInt(condition)) = True Then
                    T = True
                Else
                    T = False
                End If
            Case "hasitem"
                If Core.Player.Inventory.GetItemAmount(CInt(condition)) > 0 Then
                    T = True
                Else
                    T = False
                End If
            Case "haspokemon"
                For Each p As Pokemon In Core.Player.Pokemons
                    If p.Number = CInt(condition) Then
                        T = True
                        Exit For
                    End If
                Next
        End Select

        If inverse = True Then
            T = Not T
        End If

        ActionScript.CSL().WaitingEndIf(ActionScript.CSL().IfIndex + 1) = False
        ActionScript.CSL().CanTriggerElse(ActionScript.CSL().IfIndex + 1) = False

        oS.ActionScript.ChooseIf(T)

        Me.IsReady = True
    End Sub

    Private Sub DoWait()
        If CInt(Me.Value) > 0 Then
            Me.Value = CStr(CInt(Me.Value) - 1)
        End If
        If CInt(Me.Value) <= 0 Then
            Me.IsReady = True
        End If
    End Sub

    Private Sub DoAction()
        Select Case True
            Case Me.Value.ToLower() = "storagesystem"
                Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New StorageSystemScreen(Core.CurrentScreen), Color.Black, False))
            Case Me.Value.ToLower() = "apricornkurt"
                Core.SetScreen(New ApricornScreen(Core.CurrentScreen, "Kurt"))
            Case Me.Value.ToLower().StartsWith("trade(")
                Me.Value = Me.Value.Remove(0, Me.Value.IndexOf("(") + 1)
                Me.Value = Me.Value.Remove(Me.Value.Length - 1, 1)

                Dim storeData As String = CStr(Value.GetSplit(0))
                Dim canBuy As Boolean = CBool(Value.GetSplit(1))
                Dim canSell As Boolean = CBool(Value.GetSplit(2))

                Dim currencyIndicator As String = "P"

                If Value.CountSplits() > 3 Then
                    currencyIndicator = Value.GetSplit(3)
                End If

                Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New TradeScreen(Core.CurrentScreen, storeData, canBuy, canSell, currencyIndicator), Color.Black, False))
            Case Me.Value.ToLower().StartsWith("getpokemon(")
                Me.Value = Me.Value.Remove(0, Me.Value.IndexOf("(") + 1)
                Me.Value = Me.Value.Remove(Me.Value.Length - 1, 1)

                Dim commas As Integer = 0
                For Each c As Char In Value
                    If c = "," Then
                        commas += 1
                    End If
                Next

                Dim PokemonID As Integer = CInt(Me.Value.GetSplit(0))
                Dim Level As Integer = CInt(Me.Value.GetSplit(1))

                Dim catchMethod As String = "random reason"
                If commas > 1 Then
                    catchMethod = Me.Value.GetSplit(2)
                End If

                Dim catchBall As Item = Item.GetItemByID(1)
                If commas > 2 Then
                    catchBall = Item.GetItemByID(CInt(Me.Value.GetSplit(3)))
                End If

                Dim catchLocation As String = Screen.Level.MapName
                If commas > 3 Then
                    catchLocation = Me.Value.GetSplit(4)
                End If

                Dim isEgg As Boolean = False
                If commas > 4 Then
                    isEgg = CBool(Me.Value.GetSplit(5))
                End If

                Dim catchTrainer As String = Core.Player.Name
                If commas > 5 And Me.Value.GetSplit(6) <> "<playername>" Then
                    catchTrainer = Me.Value.GetSplit(6)
                End If

                Dim Pokemon As Pokemon = Pokemon.GetPokemonByID(PokemonID)
                Pokemon.Generate(Level, True)

                Pokemon.CatchTrainerName = catchTrainer
                Pokemon.OT = Core.Player.OT

                Pokemon.CatchLocation = catchLocation
                Pokemon.CatchBall = catchBall
                Pokemon.CatchMethod = catchMethod

                If isEgg = True Then
                    Pokemon.EggSteps = 1
                    Pokemon.SetCatchInfos(Item.GetItemByID(5), "obtained at")
                Else
                    Pokemon.EggSteps = 0
                End If

                Core.Player.Pokemons.Add(Pokemon)

                Dim pokedexType As Integer = 2
                If Pokemon.IsShiny = True Then
                    pokedexType = 3
                End If

                Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, Pokemon.Number, pokedexType)
            Case Me.Value.ToLower().StartsWith("townmap,")
                Dim startRegion As String = Me.Value.GetSplit(1)
                Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New MapScreen(Core.CurrentScreen, startRegion, {"view"}), Color.Black, False))
            Case Me.Value.ToLower() = "opendonation"
                Core.SetScreen(New DonationScreen(Core.CurrentScreen))
            Case Me.Value.ToLower() = "receivepokedex"
                Core.Player.hasPokedex = True
                For Each p As Pokemon In Core.Player.Pokemons
                    Dim i As Integer = 2
                    If p.IsShiny = True Then
                        i = 3
                    End If
                    Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, p.Number, i)
                Next
            Case Me.Value.ToLower() = "receivepokegear"
                Core.Player.hasPokegear = True
            Case Me.Value.ToLower().StartsWith("renamepokemon(") = True
                Me.Value = Me.Value.Remove(0, Me.Value.IndexOf("(") + 1)
                Me.Value = Me.Value.Remove(Me.Value.Length - 1, 1)

                Dim index As String = Me.Value
                Dim renameOTcheck As Boolean = False
                Dim canRename As Boolean = True

                If Me.Value.Contains(",") = True Then
                    index = Me.Value.GetSplit(0)
                    renameOTcheck = CBool(Me.Value.GetSplit(1))
                End If

                Dim PokemonIndex As Integer = 0
                If IsNumeric(index) = True Then
                    PokemonIndex = CInt(index)
                Else
                    If index.ToLower() = "last" Then
                        PokemonIndex = Core.Player.Pokemons.Count - 1
                    End If
                End If

                If renameOTcheck = True Then
                    If Core.Player.Pokemons(PokemonIndex).OT <> Core.Player.OT Then
                        canRename = False
                    End If
                End If

                If canRename = True Then
                    Core.SetScreen(New NameObjectScreen(Core.CurrentScreen, Core.Player.Pokemons(PokemonIndex)))
                Else
                    Screen.TextBox.Show("I cannot rename this~Pokémon because the~OT is different!*Did you receive it in~a trade or something?", {}, True, False)
                End If
            Case Me.Value.ToLower() = "renamerival"
                Core.SetScreen(New NameObjectScreen(Core.CurrentScreen, TextureManager.GetTexture("NPC\4", New Rectangle(0, 64, 32, 32)), False, False, "rival", "Silver", AddressOf Script.NameRival))
            Case Me.Value.ToLower().StartsWith("playcry(")
                Me.Value = Me.Value.Remove(0, Me.Value.IndexOf("(") + 1)
                Me.Value = Me.Value.Remove(Me.Value.Length - 1, 1)

                Dim p As Pokemon = Pokemon.GetPokemonByID(CInt(Value))
                p.PlayCry()
            Case Me.Value.ToLower().StartsWith("showOpokemon(")
                Me.Value = Me.Value.Remove(0, Me.Value.IndexOf("(") + 1)
                Me.Value = Me.Value.Remove(Me.Value.Length - 1, 1)

                Screen.Level.OverworldPokemon.Visible = CBool(Value)
            Case Me.Value.ToLower() = "togglethirdperson"
                If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                    Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)

                    c.SetThirdPerson(Not c.ThirdPerson, False)
                    c.UpdateFrustum()
                    c.UpdateViewMatrix()
                    Screen.Level.UpdateEntities()
                    Screen.Level.Entities = (From e In Screen.Level.Entities Order By e.CameraDistance Descending).ToList()
                    Screen.Level.UpdateEntities()
                End If
            Case Me.Value.ToLower() = "activatethirdperson"
                If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                    Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)

                    c.SetThirdPerson(True, False)
                    c.UpdateFrustum()
                    c.UpdateViewMatrix()
                    Screen.Level.UpdateEntities()
                    Screen.Level.Entities = (From e In Screen.Level.Entities Order By e.CameraDistance Descending).ToList()
                    Screen.Level.UpdateEntities()
                End If
            Case Me.Value.ToLower() = "deactivatethirdperson"
                If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                    Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)

                    c.SetThirdPerson(False, False)
                    c.UpdateFrustum()
                    c.UpdateViewMatrix()
                    Screen.Level.UpdateEntities()
                    Screen.Level.Entities = (From e In Screen.Level.Entities Order By e.CameraDistance Descending).ToList()
                    Screen.Level.UpdateEntities()
                End If
            Case Me.Value.ToLower().StartsWith("setfont(")
                Me.Value = Me.Value.Remove(0, Me.Value.IndexOf("(") + 1)
                Me.Value = Me.Value.Remove(Me.Value.Length - 1, 1)

                Select Case Me.Value.ToLower()
                    Case "standard"
                        Screen.TextBox.TextFont = FontManager.GetFontContainer("textfont")
                    Case "unown"
                        Screen.TextBox.TextFont = FontManager.GetFontContainer("unown")
                End Select
            Case Me.Value.ToLower().StartsWith("setrenderdistance(")
                Me.Value = Me.Value.Remove(0, Me.Value.IndexOf("(") + 1)
                Me.Value = Me.Value.Remove(Me.Value.Length - 1, 1)

                Select Case Me.Value.ToLower()
                    Case "0", "tiny"
                        Core.GameOptions.RenderDistance = 0
                    Case "1", "small"
                        Core.GameOptions.RenderDistance = 1
                    Case "2", "normal"
                        Core.GameOptions.RenderDistance = 2
                    Case "3", "far"
                        Core.GameOptions.RenderDistance = 3
                    Case "4", "extreme"
                        Core.GameOptions.RenderDistance = 4
                End Select

                Screen.Level.World = New World(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
            Case Me.Value.ToLower().StartsWith("wearskin(")
                Me.Value = Me.Value.Remove(0, Me.Value.IndexOf("(") + 1)
                Me.Value = Me.Value.Remove(Me.Value.Length - 1, 1)

                With Screen.Level.OwnPlayer
                    Dim TextureID As String = Value

                    Logger.Debug(TextureID)

                    .Texture = net.Pokemon3D.Game.TextureManager.GetTexture("Textures\NPC\" & TextureID)
                    .SkinName = TextureID

                    .UpdateEntity()
                End With
            Case Me.Value.ToLower() = "toggledarkness"
                Screen.Level.IsDark = Not Screen.Level.IsDark
            Case Me.Value.ToLower().StartsWith("globalhub"), Me.Value.ToLower().StartsWith("friendhub")
                If GameJolt.API.LoggedIn = True And Core.Player.IsGamejoltSave = True Or GameController.IS_DEBUG_ACTIVE = True Then
                    If GameJolt.LogInScreen.UserBanned(Core.GameJoltSave.GameJoltID) = False Then
                        Core.SetScreen(New TransitionScreen(Core.CurrentScreen, New GameJolt.GTSMainScreen(Core.CurrentScreen), Color.Black, False))
                    Else
                        Screen.TextBox.Show("This GameJolt account~(" & Core.GameJoltSave.GameJoltID & ") is banned~from the GTS!", {}, False, False, Color.Red)
                    End If
                Else
                    Screen.TextBox.Show("You are not using~your GameJolt profile.*Please connect to GameJolt~and switch to the GameJolt~profile to enable the GTS.*You can do this by going~back to the main menu~and choosing ""Play online"".", {}, False, False, Color.Red)
                End If
            Case Me.Value.ToLower().StartsWith("gamejoltlogin")
                Core.SetScreen(New GameJolt.LogInScreen(Core.CurrentScreen))
            Case Me.Value.ToLower().StartsWith("readpokemon(") = True
                Me.Value = Me.Value.Remove(0, Me.Value.IndexOf("(") + 1)
                Me.Value = Me.Value.Remove(Me.Value.Length - 1, 1)

                Dim p As Pokemon = Core.Player.Pokemons(CInt(Value))

                Dim message As String = "Hm... I see your~" & p.GetDisplayName()
                Dim addmessage As String = "~is very stable with~"

                If p.EVAttack > p.EVDefense And p.EVAttack > p.EVHP And p.EVAttack > p.EVSpAttack And p.EVAttack > p.EVSpDefense And p.EVAttack > p.EVSpeed Then
                    addmessage &= "performing physical moves."
                End If
                If p.EVDefense > p.EVAttack And p.EVDefense > p.EVHP And p.EVDefense > p.EVSpAttack And p.EVDefense > p.EVSpDefense And p.EVDefense > p.EVSpeed Then
                    addmessage &= "taking hits."
                End If
                If p.EVHP > p.EVAttack And p.EVHP > p.EVDefense And p.EVHP > p.EVSpAttack And p.EVHP > p.EVSpDefense And p.EVHP > p.EVSpeed Then
                    addmessage &= "taking damage."
                End If
                If p.EVSpAttack > p.EVAttack And p.EVSpAttack > p.EVDefense And p.EVSpAttack > p.EVHP And p.EVSpAttack > p.EVSpDefense And p.EVSpAttack > p.EVSpeed Then
                    addmessage &= "performing complex strategies."
                End If
                If p.EVSpDefense > p.EVAttack And p.EVSpDefense > p.EVDefense And p.EVSpDefense > p.EVHP And p.EVSpDefense > p.EVSpAttack And p.EVSpDefense > p.EVSpeed Then
                    addmessage &= "breaking strategies."
                End If
                If p.EVSpeed > p.EVAttack And p.EVSpeed > p.EVDefense And p.EVSpeed > p.EVHP And p.EVSpeed > p.EVSpAttack And p.EVSpeed > p.EVSpDefense Then
                    addmessage &= "speeding the others out."
                End If

                If addmessage = "~is very stable with~" Then
                    addmessage = "~is very well balanced."
                End If

                message &= addmessage

                message &= "*...~...*What that means?~I am not sure..."

                Screen.TextBox.Show(message, {}, False, False)
            Case Me.Value.ToLower.StartsWith("achieveemblem(") = True
                Me.Value = Me.Value.Remove(0, Me.Value.IndexOf("(") + 1)
                Me.Value = Me.Value.Remove(Me.Value.Length - 1, 1)

                GameJolt.Emblem.AchieveEmblem(Me.Value)
        End Select

        Me.IsReady = True
    End Sub

    Private Sub DoMusic()
        MusicManager.PlayMusic(Me.Value, True)

        If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            Screen.Level.MusicLoop = Me.Value
        End If

        Me.IsReady = True
    End Sub

    Private Sub DoSound()
        Dim sound As String = Me.Value
        Dim stopMusic As Boolean = False

        If Me.Value.Contains(",") = True Then
            sound = Me.Value.GetSplit(0)
            stopMusic = CBool(Me.Value.GetSplit(1))
        End If

        SoundManager.PlaySound(sound, stopMusic)

        Me.IsReady = True
    End Sub

    Private Sub DoMessageBulb()
        If Me.started = False Then
            Me.started = True
            Dim Data() As String = Me.Value.Split(CChar("|"))

            Dim ID As Integer = CInt(Data(0))
            Dim Position As New Vector3(CSng(Data(1).Replace(".", GameController.DecSeparator)), CSng(Data(2).Replace(".", GameController.DecSeparator)), CSng(Data(3).Replace(".", GameController.DecSeparator)))

            Dim noType As MessageBulb.NotifcationTypes = MessageBulb.NotifcationTypes.Waiting
            Select Case ID
                Case 0
                    noType = MessageBulb.NotifcationTypes.Waiting
                Case 1
                    noType = MessageBulb.NotifcationTypes.Exclamation
                Case 2
                    noType = MessageBulb.NotifcationTypes.Shouting
                Case 3
                    noType = MessageBulb.NotifcationTypes.Question
                Case 4
                    noType = MessageBulb.NotifcationTypes.Note
                Case 5
                    noType = MessageBulb.NotifcationTypes.Heart
                Case 6
                    noType = MessageBulb.NotifcationTypes.Unhappy
                Case 7
                    noType = MessageBulb.NotifcationTypes.Happy
                Case 8
                    noType = MessageBulb.NotifcationTypes.Friendly
                Case 9
                    noType = MessageBulb.NotifcationTypes.Poisoned
                Case Else
                    noType = MessageBulb.NotifcationTypes.Exclamation
            End Select

            Screen.Level.Entities.Add(New MessageBulb(Position, noType))
        End If

        Dim contains As Boolean = False
        Screen.Level.Entities = (From e In Screen.Level.Entities Order By e.CameraDistance Descending).ToList()
        For Each e As Entity In Screen.Level.Entities
            If e.EntityID = "MessageBulb" Then
                e.Update()
                contains = True
            End If
        Next
        If contains = False Then
            Me.IsReady = True
        Else
            For i = 0 To Screen.Level.Entities.Count - 1
                If i <= Screen.Level.Entities.Count - 1 Then
                    If Screen.Level.Entities(i).CanBeRemoved = True Then
                        Screen.Level.Entities.RemoveAt(i)
                        i -= 1
                    End If
                Else
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Sub DoBattle()
        Dim ActionValue As String = Value.GetSplit(0)
        Select Case ActionValue.ToLower()
            Case "trainer"
                Dim ID As String = Value.GetSplit(1)
                Dim t As New Trainer(ID)

                If Value.CountSeperators(",") > 1 Then
                    For Each v As String In Value.Split(CChar(","))
                        Select Case v
                            Case "generate_pokemon_tower"
                                Dim level As Integer = 0
                                For Each p As Pokemon In Core.Player.Pokemons
                                    If p.Level > level Then
                                        level = p.Level
                                    End If
                                Next

                                While CStr(level)(CStr(level).Length - 1) <> "0"
                                    level += 1
                                End While
                        End Select
                    Next
                End If

                Dim b As New BattleSystem.BattleScreen(t, Core.CurrentScreen, 0)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))
            Case "wild"
                Dim ID As Integer = CInt(Value.GetSplit(1))
                Dim Level As Integer = CInt(Value.GetSplit(2))

                Dim p As Pokemon = Pokemon.GetPokemonByID(ID)
                p.Generate(Level, True)

                Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, p.Number, 1)

                Dim b As New BattleSystem.BattleScreen(p, Core.CurrentScreen, 0)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, Core.Random.Next(0, 10)))
        End Select
        Me.IsReady = True
    End Sub

    Private Sub DoTrainerBattle()
        Dim t As New Trainer(Value)
        If t.IsBeaten() = False Then
            If started = False Then
                CType(Core.CurrentScreen, OverworldScreen).TrainerEncountered = True
                MusicManager.PlayMusic(t.GetInSightMusic(), True)
                If t.IntroMessage <> "" Then
                    Screen.TextBox.reDelay = 0.0F
                    Screen.TextBox.Show(t.IntroMessage, {})
                End If
                started = True
            End If

            If Screen.TextBox.Showing = False Then
                CType(Core.CurrentScreen, OverworldScreen).TrainerEncountered = False

                Dim b As New BattleSystem.BattleScreen(New Trainer(Value), Core.CurrentScreen, 0)
                Core.SetScreen(New BattleIntroScreen(Core.CurrentScreen, b, t, t.GetIniMusicName(), t.IntroType))
            End If
        Else
            Screen.TextBox.reDelay = 0.0F
            Screen.TextBox.Show(t.DefeatMessage, {})

            Me.IsReady = True
        End If

        If Screen.TextBox.Showing = False Then
            Me.IsReady = True
        End If
    End Sub

    Private Sub DoPokemon()
        Dim command As String = Value
        Dim argument As String = ""

        If command.Contains("(") = True And command.EndsWith(")") = True Then
            argument = command.Remove(0, command.IndexOf("(") + 1)
            argument = argument.Remove(argument.Length - 1, 1)
            command = command.Remove(command.IndexOf("("))
        End If

        Select Case command.ToLower()
            Case "cry"
                Dim PokemonID As Integer = CInt(argument)

                Dim p As Pokemon = Pokemon.GetPokemonByID(PokemonID)
                p.PlayCry()
            Case "remove"
                Dim index As Integer = CInt(argument)
                If Core.Player.Pokemons.Count - 1 >= index Then
                    Logger.Debug("Remove Pokémon (" & Core.Player.Pokemons(index).GetDisplayName() & ") at index " & index)
                    Core.Player.Pokemons.RemoveAt(index)
                End If
            Case "add"
                Dim commas As Integer = 0
                For Each c As Char In argument
                    If c = "," Then
                        commas += 1
                    End If
                Next

                Dim PokemonID As Integer = CInt(argument.GetSplit(0))
                Dim Level As Integer = CInt(argument.GetSplit(1))

                Dim catchMethod As String = "random reason"
                If commas > 1 Then
                    catchMethod = argument.GetSplit(2)
                End If

                Dim catchBall As Item = Item.GetItemByID(1)
                If commas > 2 Then
                    catchBall = Item.GetItemByID(CInt(argument.GetSplit(3)))
                End If

                Dim catchLocation As String = Screen.Level.MapName
                If commas > 3 Then
                    catchLocation = argument.GetSplit(4)
                End If

                Dim isEgg As Boolean = False
                If commas > 4 Then
                    isEgg = CBool(argument.GetSplit(5))
                End If

                Dim catchTrainer As String = Core.Player.Name
                If commas > 5 And argument.GetSplit(6) <> "<playername>" Then
                    catchTrainer = argument.GetSplit(6)
                End If

                Dim Pokemon As Pokemon = Pokemon.GetPokemonByID(PokemonID)
                Pokemon.Generate(Level, True)

                Pokemon.CatchTrainerName = catchTrainer
                Pokemon.OT = Core.Player.OT

                Pokemon.CatchLocation = catchLocation
                Pokemon.CatchBall = catchBall
                Pokemon.CatchMethod = catchMethod

                If isEgg = True Then
                    Pokemon.EggSteps = 1
                    Pokemon.SetCatchInfos(Item.GetItemByID(5), "obtained at")
                Else
                    Pokemon.EggSteps = 0
                End If

                Core.Player.Pokemons.Add(Pokemon)

                Dim pokedexType As Integer = 2
                If Pokemon.IsShiny = True Then
                    pokedexType = 3
                End If

                Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, Pokemon.Number, pokedexType)
            Case "setadditionalvalue"
                Dim Index As Integer = CInt(argument.GetSplit(0, ","))
                Dim AdditionalValue As String = argument.GetSplit(1, ",")

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    Core.Player.Pokemons(Index).AdditionalData = AdditionalValue
                End If
            Case "setnickname"
                Dim Index As Integer = CInt(argument.GetSplit(0, ","))
                Dim NickName As String = argument.GetSplit(1, ",")

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    Core.Player.Pokemons(Index).NickName = NickName
                End If
            Case "setstat"
                Dim Index As Integer = CInt(argument.GetSplit(0, ","))
                Dim stat As String = argument.GetSplit(1, ",")
                Dim statValue As Integer = CInt(argument.GetSplit(2, ","))

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    With Core.Player.Pokemons(Index)
                        Select Case stat.ToLower()
                            Case "maxhp", "hp"
                                .MaxHP = statValue
                            Case "chp"
                                .HP = statValue
                            Case "atk", "attack"
                                .Attack = statValue
                            Case "def", "defense"
                                .Defense = statValue
                            Case "spatk", "specialattack", "spattack"
                                .SpAttack = statValue
                            Case "spdef", "specialdefense", "spdefense"
                                .SpDefense = statValue
                            Case "speed"
                                .Speed = statValue
                        End Select
                    End With
                End If
            Case "clear"
                Core.Player.Pokemons.Clear()
            Case "removeattack"
                Dim Index As Integer = CInt(argument.GetSplit(0, ","))
                Dim attackIndex As Integer = CInt(argument.GetSplit(1, ","))

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    Dim p As Pokemon = Core.Player.Pokemons(Index)

                    If p.Attacks.Count - 1 >= attackIndex Then
                        p.Attacks.RemoveAt(attackIndex)
                    End If
                End If
            Case "clearattacks"
                Dim Index As Integer = CInt(argument)

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    Core.Player.Pokemons(Index).Attacks.Clear()
                End If
            Case "addattack"
                Dim Index As Integer = CInt(argument.GetSplit(0, ","))
                Dim attackID As Integer = CInt(argument.GetSplit(1, ","))

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    Dim p As Pokemon = Core.Player.Pokemons(Index)

                    If p.Attacks.Count < 4 Then
                        Dim newAttack As BattleSystem.Attack = BattleSystem.Attack.GetAttackByID(attackID)
                        p.Attacks.Add(newAttack)
                    End If
                End If
            Case "removeattack"
                Dim Index As Integer = CInt(argument.GetSplit(0, ","))
                Dim attackIndex As Integer = CInt(argument.GetSplit(1, ","))

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    Dim p As Pokemon = Core.Player.Pokemons(Index)

                    If p.Attacks.Count - 1 >= attackIndex Then
                        p.Attacks.RemoveAt(attackIndex)
                    End If
                End If
            Case "setshiny"
                Dim Index As Integer = CInt(argument.GetSplit(0, ","))
                Dim isShiny As Boolean = CBool(argument.GetSplit(1, ","))

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    Core.Player.Pokemons(Index).IsShiny = isShiny
                End If
            Case "changelevel"
                Dim Index As Integer = CInt(argument.GetSplit(0, ","))
                Dim newLevel As Integer = CInt(argument.GetSplit(1, ","))

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    Core.Player.Pokemons(Index).Level = newLevel
                End If
            Case "gainexp"
                Dim Index As Integer = CInt(argument.GetSplit(0, ","))
                Dim exp As Integer = CInt(argument.GetSplit(1, ","))

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    Core.Player.Pokemons(Index).Experience += exp
                End If
            Case "setnature"
                Dim Index As Integer = CInt(argument.GetSplit(0, ","))
                Dim Nature As Pokemon.Natures = Pokemon.ConvertIDToNature(CInt(argument.GetSplit(1, ",")))

                If Core.Player.Pokemons.Count - 1 >= Index Then
                    Core.Player.Pokemons(Index).Nature = Nature
                End If
            Case "npctrade"
                Dim splits() As String = argument.Split(CChar("|"))
                Script.SaveNPCTrade = splits

                Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Item.GetItemByID(5), AddressOf Script.DoNPCTrade, "Choose trade Pokémon", True))
                CType(Core.CurrentScreen, ChoosePokemonScreen).ExitedSub = AddressOf Script.ExitedNPCTrade
            Case "hide"
                Screen.Level.OverworldPokemon.Visible = False
        End Select

        Me.IsReady = True
    End Sub

    Private Sub DoNPC()
        Dim command As String = Value
        Dim argument As String = ""

        If command.Contains("(") = True And command.EndsWith(")") = True Then
            argument = command.Remove(0, command.IndexOf("(") + 1)
            argument = argument.Remove(argument.Length - 1, 1)
            command = command.Remove(command.IndexOf("("))
        End If

        Select Case command.ToLower()
            Case "remove"
                Dim targetNPC As Entity = Screen.Level.GetNPC(CInt(argument))
                Screen.Level.Entities.Remove(targetNPC)
                Me.IsReady = True
            Case "position", "warp"
                Dim targetNPC As NPC = Screen.Level.GetNPC(CInt(argument.GetSplit(0)))

                Dim PositionData() As String = argument.Split(CChar(","))
                targetNPC.Position = New Vector3(CSng(PositionData(1).Replace(".", GameController.DecSeparator)), CSng(PositionData(2).Replace(".", GameController.DecSeparator)), CSng(PositionData(3).Replace(".", GameController.DecSeparator)))
                targetNPC.CreatedWorld = False
                Me.IsReady = True
            Case "register"
                NPC.AddNPCData(argument)
                Me.IsReady = True
            Case "unregister"
                NPC.RemoveNPCData(argument)
                Me.IsReady = True
            Case "wearskin"
                Dim textureID As String = argument.GetSplit(1)
                Dim targetNPC As NPC = Screen.Level.GetNPC(CInt(argument.GetSplit(0)))

                targetNPC.SetupSprite(textureID, "", False)
                Me.IsReady = True
            Case "move"
                Dim targetNPC As NPC = Screen.Level.GetNPC(CInt(argument.GetSplit(0)))
                Dim steps As Integer = CInt(argument.GetSplit(1))

                Screen.Level.UpdateEntities()
                If started = False Then
                    targetNPC.Moved += steps
                    started = True
                Else
                    If targetNPC.Moved <= 0.0F Then
                        Me.IsReady = True
                    End If
                End If
            Case "turn"
                Dim targetNPC As NPC = Screen.Level.GetNPC(CInt(argument.GetSplit(0)))

                targetNPC.faceRotation = CInt(argument.GetSplit(1))
                targetNPC.Update()
                targetNPC.UpdateEntity()
                Me.IsReady = True
            Case Else
                Me.IsReady = True
        End Select
    End Sub

    Private Sub DoPlayer()
        Dim command As String = Value
        Dim argument As String = ""

        If command.Contains("(") = True And command.EndsWith(")") = True Then
            argument = command.Remove(0, command.IndexOf("(") + 1)
            argument = argument.Remove(argument.Length - 1, 1)
            command = command.Remove(command.IndexOf("("))
        End If

        Select Case command.ToLower()
            Case "wearskin"
                With Screen.Level.OwnPlayer
                    Dim TextureID As String = argument
                    .SetTexture(TextureID, False)

                    .UpdateEntity()
                End With

                Me.IsReady = True
            Case "move"
                If Me.started = False Then
                    Screen.Camera.Move(CSng(argument))
                    started = True
                    Screen.Level.OverworldPokemon.Visible = False
                Else
                    Screen.Level.UpdateEntities()
                    Screen.Camera.Update()
                    If Screen.Camera.IsMoving() = False Then
                        Me.IsReady = True
                    End If
                End If
            Case "turn"
                If Me.started = False Then
                    Screen.Camera.Turn(CInt(argument))
                    started = True
                    Screen.Level.OverworldPokemon.Visible = False
                Else
                    Screen.Camera.Update()
                    Screen.Level.UpdateEntities()
                    If Screen.Camera.Turning = False Then
                        Me.IsReady = True
                    End If
                End If
            Case "turnto"
                If Me.started = False Then
                    Dim turns As Integer = CInt(argument) - Screen.Camera.GetPlayerFacingDirection() 
                    If turns < 0 Then
                        turns = turns + 4
                    End If

                    If turns > 0 Then
                        Screen.Camera.Turn(turns)
                        started = True
                        Screen.Level.OverworldPokemon.Visible = False
                    Else
                        Me.IsReady = True
                    End If
                Else
                    Screen.Camera.Update()
                    Screen.Level.UpdateEntities()
                    If Screen.Camera.Turning = False Then
                        Me.IsReady = True
                    End If
                End If
            Case "warp"
                Dim commas As Integer = 0
                For Each c As Char In argument
                    If c = "," Then
                        commas += 1
                    End If
                Next

                Select Case commas
                    Case 4
                        Screen.Level.WarpData.WarpDestination = argument.GetSplit(0)
                        Screen.Level.WarpData.WarpPosition = New Vector3(CSng(argument.GetSplit(1)), CSng(argument.GetSplit(2).Replace(".", GameController.DecSeparator)), CSng(argument.GetSplit(3)))
                        Screen.Level.WarpData.WarpRotations = CInt(argument.GetSplit(4))
                        Screen.Level.WarpData.DoWarpInNextTick = True
                        Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
                    Case 2
                        Screen.Camera.Position = New Vector3(CSng(argument.GetSplit(0)), CSng(argument.GetSplit(1).Replace(".", GameController.DecSeparator)), CSng(argument.GetSplit(2)))
                End Select

                Screen.Level.OverworldPokemon.warped = True
                Screen.Level.OverworldPokemon.Visible = False

                Me.IsReady = True
            Case "stopmovement"
                Screen.Camera.StopMovement()

                Me.IsReady = True
            Case "money"
                Core.Player.Money += CInt(argument)

                Me.IsReady = True
            Case "setmovement"
                Dim movements() As String = argument.Split(CChar(","))

                Screen.Camera.PlannedMovement = New Vector3(CInt(movements(0)),
                                                            CInt(movements(1)),
                                                            CInt(movements(2)))
                Me.IsReady = True
            Case Else
                Me.IsReady = True
        End Select
    End Sub

    Private Sub DoEntity()
        Dim command As String = Value
        Dim argument As String = ""

        If command.Contains("(") = True And command.EndsWith(")") = True Then
            argument = command.Remove(0, command.IndexOf("(") + 1)
            argument = argument.Remove(argument.Length - 1, 1)
            command = command.Remove(command.IndexOf("("))
        End If

        Dim entID As Integer = CInt(argument.GetSplit(0))
        Dim ent As Entity = Screen.Level.GetEntity(entID)

        If Not ent Is Nothing Then
            Select Case command.ToLower()
                Case "warp"
                    Dim PositionList As List(Of String) = argument.Split(CChar(",")).ToList()
                    Dim newPosition As Vector3 = New Vector3(CSng(PositionList(1).Replace(".", GameController.DecSeparator)), CSng(PositionList(2).Replace(".", GameController.DecSeparator)), CSng(PositionList(3).Replace(".", GameController.DecSeparator)))

                    ent.Position = newPosition
                    ent.CreatedWorld = False
                Case "scale"
                    Dim ScaleList As List(Of String) = argument.Split(CChar(",")).ToList()
                    Dim newScale As Vector3 = New Vector3(CSng(ScaleList(1).Replace(".", GameController.DecSeparator)), CSng(ScaleList(2).Replace(".", GameController.DecSeparator)), CSng(ScaleList(3).Replace(".", GameController.DecSeparator)))

                    ent.Scale = newScale
                    ent.CreatedWorld = False
                Case "remove"
                    ent.CanBeRemoved = True
                Case "setid"
                    ent.ID = CInt(argument.GetSplit(1))
                Case "opacity"
                    ent.NormalOpacity = CSng(CInt(argument.GetSplit(1)) / 100)
                Case "visible"
                    ent.Visible = CBool(argument.GetSplit(1))
                    'Case "move"
                Case "setadditionalvalue"
                    ent.AdditionalValue = argument.GetSplit(1)
                Case "collision"
                    ent.Collision = CBool(argument.GetSplit(1))

            End Select
        End If

        Me.IsReady = True
    End Sub

    Private Sub DoEnvironment()
        Dim command As String = Value
        Dim argument As String = ""

        If command.Contains("(") = True And command.EndsWith(")") = True Then
            argument = command.Remove(0, command.IndexOf("(") + 1)
            argument = argument.Remove(argument.Length - 1, 1)
            command = command.Remove(command.IndexOf("("))
        End If

        Select Case argument.ToLower()
            Case "changeweathertype"
                Screen.Level.WeatherType = CInt(argument)
                Screen.Level.World = New World(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
            Case "changeenvironmenttype"
                Screen.Level.EnvironmentType = CInt(argument)
                Screen.Level.World = New World(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
            Case "canfly"
                Screen.Level.CanFly = CBool(argument)
            Case "candig"
                Screen.Level.CanDig = CBool(argument)
            Case "canteleport"
                Screen.Level.CanTeleport = CBool(argument)
            Case "wildpokemongrass"
                Screen.Level.WildPokemonGrass = CBool(argument)
            Case "wildpokemonwater"
                Screen.Level.WildPokemonWater = CBool(argument)
            Case "wildpokemoneverywhere"
                Screen.Level.WildPokemonFloor = CBool(argument)
            Case "isdark"
                Screen.Level.IsDark = CBool(argument)
            Case "resetwalkedsteps"
                Screen.Level.WalkedSteps = 0
        End Select

        Me.IsReady = True
    End Sub

    Private Sub DoLevel()
        Dim command As String = Value
        Dim argument As String = ""

        If command.Contains("(") = True And command.EndsWith(")") = True Then
            argument = command.Remove(0, command.IndexOf("(") + 1)
            argument = argument.Remove(argument.Length - 1, 1)
            command = command.Remove(command.IndexOf("("))
        End If

        Select Case command.ToLower()
            Case "update"
                Screen.Level.Update()
                Screen.Level.UpdateEntities()
                Screen.Camera.Update()
        End Select

        Me.IsReady = True
    End Sub

#End Region

    Private Sub ViewPokemonImage()
        Dim PokemonID As Integer = CInt(Me.Value.GetSplit(0))
        Dim Shiny As Boolean = CBool(Me.Value.GetSplit(1))
        Dim Front As Boolean = CBool(Me.Value.GetSplit(2))

        Screen.PokemonImageView.Show(PokemonID, Shiny, Front)
        Me.IsReady = True
    End Sub

    Private Sub Move()
        Dim targetID As Integer = CInt(Me.Value.GetSplit(0))
        Dim targetNPC As NPC = Screen.Level.GetNPC(targetID)
        Dim moved As Integer = CInt(Me.Value.GetSplit(1))

        Screen.Level.UpdateEntities()
        If started = False Then
            targetNPC.Moved += moved
            started = True
        Else
            If targetNPC.Moved <= 0.0F Then
                Me.IsReady = True
            End If
        End If
    End Sub

    Private Sub MoveAsync()
        Dim targetID As Integer = CInt(Me.Value.GetSplit(0))
        Dim targetNPC As NPC = Screen.Level.GetNPC(targetID)
        Dim moved As Integer = CInt(Me.Value.GetSplit(1))

        targetNPC.Moved += moved
        started = True
        Me.IsReady = True
    End Sub

    Private Sub MovePlayer()
        If Me.started = False Then
            Screen.Camera.Move(CSng(Value))
            started = True
            Screen.Level.OverworldPokemon.Visible = False
        Else
            Screen.Level.UpdateEntities()
            Screen.Camera.Update()
            If Screen.Camera.IsMoving() = False Then
                Me.IsReady = True
            End If
        End If
    End Sub

    Private Sub Turn()
        Dim targetID As Integer = CInt(Value.GetSplit(0))
        Dim targetNPC As NPC = Screen.Level.GetNPC(targetID)

        targetNPC.faceRotation = CInt(Value.GetSplit(1))
        targetNPC.Update()
        targetNPC.UpdateEntity()
        Me.IsReady = True
    End Sub

    Private Sub Warp()
        Dim targetID As Integer = CInt(Value.GetSplit(0))
        Dim targetNPC As NPC = Screen.Level.GetNPC(targetID)

        Dim targetPosition As New Vector3(CInt(Value.GetSplit(1)), CInt(Value.GetSplit(2)), CInt(Value.GetSplit(3)))
        targetNPC.Position = targetPosition
        Logger.Debug(targetNPC.Position.ToString())
        targetNPC.Update()

        Me.IsReady = True
    End Sub

    Private Sub WarpPlayer()
        Dim commas As Integer = 0
        For Each c As Char In Value
            If c = "," Then
                commas += 1
            End If
        Next

        Select Case commas
            Case 4
                Screen.Level.WarpData.WarpDestination = Me.Value.GetSplit(0)
                Screen.Level.WarpData.WarpPosition = New Vector3(CSng(Me.Value.GetSplit(1)), CSng(Me.Value.GetSplit(2).Replace(".", GameController.DecSeparator)), CSng(Me.Value.GetSplit(3)))
                Screen.Level.WarpData.WarpRotations = CInt(Me.Value.GetSplit(4))
                Screen.Level.WarpData.DoWarpInNextTick = True
                Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
            Case 2
                Screen.Camera.Position = New Vector3(CSng(Me.Value.GetSplit(0)), CSng(Me.Value.GetSplit(1).Replace(".", GameController.DecSeparator)), CSng(Me.Value.GetSplit(2)))
        End Select

        Screen.Level.OverworldPokemon.Visible = False

        Me.IsReady = True
    End Sub

    Private Sub Heal()
        If Me.Value = "" Then
            Core.Player.HealParty()
        Else
            Dim Data() As String = Me.Value.Split(CChar(","))
            Dim Members As New List(Of Integer)
            For Each Member As String In Data
                Members.Add(CInt(Member))
            Next
            Core.Player.HealParty(Members.ToArray())
        End If

        Me.IsReady = True
    End Sub

    Private Sub TurnPlayer()
        If Me.started = False Then
            Screen.Camera.Turn(CInt(Value))
            started = True
            Screen.Level.OverworldPokemon.Visible = False
        Else
            Screen.Camera.Update()
            Screen.Level.UpdateEntities()
            If Screen.Camera.Turning = False Then
                Me.IsReady = True
            End If
        End If
    End Sub

    Private Sub GiveItem()
        Dim ItemID As Integer = CInt(Me.Value.GetSplit(0))
        Dim Item As Item = Item.GetItemByID(ItemID)

        Dim Amount As Integer = CInt(Me.Value.GetSplit(1))

        Dim Message As String = ""
        If Amount = 1 Then
            Message = "Received the~" & Item.Name & ".*" & Core.Player.Name & " stored it in the~" & Item.ItemType.ToString() & " pocket."
        Else
            Message = "Received " & Amount & "~" & Item.PluralName & ".*" & Core.Player.Name & " stored them~in the " & Item.ItemType.ToString() & " pocket."
        End If

        Core.Player.Inventory.AddItem(ItemID, Amount)
        SoundManager.PlaySound("item_found", True)

        Screen.TextBox.reDelay = 0.0F
        Screen.TextBox.Show(Message, {})

        Me.IsReady = True
    End Sub

    Private Sub RemoveItem()
        Dim ItemID As Integer = CInt(Me.Value.GetSplit(0))
        Dim Item As Item = Item.GetItemByID(ItemID)

        Dim Amount As Integer = CInt(Me.Value.GetSplit(1))

        Dim Message As String = ""
        If Amount = 1 Then
            Message = "<playername> handed over the~" & Item.Name & "!"
        Else
            Message = "<playername> handed over the~" & Item.PluralName & "!"
        End If

        Core.Player.Inventory.RemoveItem(ItemID, Amount)

        Screen.TextBox.reDelay = 0.0F
        Screen.TextBox.Show(Message, {})

        Me.IsReady = True
    End Sub

    Private Sub GetBadge()
        If IsNumeric(Value) = True Then
            If Core.Player.Badges.Contains(CInt(Value)) = False Then
                Core.Player.Badges.Add(CInt(Value))
                SoundManager.PlaySound("badge_acquired", True)
                Screen.TextBox.Show(Core.Player.Name & " received the~" & Badge.GetBadgeName(CInt(Value)) & "badge.", {}, False, False)

                Core.Player.AddPoints(10, "Got a badge (V1 script!).")
            End If
        Else
            Throw New Exception("Invalid argument exception")
        End If

        Me.IsReady = True
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

End Class