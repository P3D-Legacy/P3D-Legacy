﻿Public Class NPC

    Inherits Entity

    Const STANDARD_SPEED As Single = 0.04F

    Public Enum Movements
        Still
        Looking
        FacePlayer
        Walk
        Straight
        Turning
        Pokeball
    End Enum

    Public Name As String
    Public NPCID As Integer
    Public faceRotation As Integer
    Public TextureID As String
    Dim lastRectangle As New Rectangle(0, 0, 0, 0)
    Dim FrameSize As New Vector2(32, 32)
    Dim Texture As Texture2D
    Public ActivationValue As Integer = 0

    Public IsTrainer As Boolean = False
    Public TrainerSight As Integer = 1
    Public TrainerBeaten As Boolean = False
    Public TrainerChecked As Boolean = False
    Public TrainerCheckCollision As Boolean = True

    Dim AnimateIdle As Boolean = True
    Dim AnimationX As Integer = 1
    Dim AnimationX_Offset As Integer = 0
    Const AnimationDelayLength As Single = 1.1F
    Dim AnimationDelay As Single = AnimationDelayLength

    Public Movement As Movements = Movements.Still
    Public MoveRectangles As New List(Of Rectangle)
    Public TurningDelay As Single = 2.0F
    Public StraightDirection As Integer = -1

    Public MoveY As Single = 0.0F
    Public MoveAsync As Boolean = False
    Public Interacted As Boolean = False

    Public Overloads Sub Initialize(ByVal TextureID As String, ByVal Rotation As Integer, ByVal Name As String, ByVal ID As Integer, ByVal AnimateIdle As Boolean, ByVal Movement As String, ByVal MoveRectangles As List(Of Rectangle))
        MyBase.Initialize()

        Me.Name = Name
        Me.NPCID = ID
        Me.faceRotation = Rotation
        Me.TextureID = TextureID
        Me.MoveRectangles = MoveRectangles
        Me.AnimateIdle = AnimateIdle

        ApplyNPCData()

        Select Case Movement.ToLower()
            Case "pokeball"
                Me.Movement = Movements.Pokeball
            Case "still"
                Me.Movement = Movements.Still
            Case "looking"
                Me.Movement = Movements.Looking
            Case "faceplayer"
                Me.Movement = Movements.FacePlayer
            Case "walk"
                Me.Movement = Movements.Walk
            Case "straight"
                Me.Movement = Movements.Straight
            Case "turning"
                Me.Movement = Movements.Turning
        End Select

        SetupSprite(Me.TextureID, "", False)

        Me.NeedsUpdate = True
        Me.CreateWorldEveryFrame = True

        If ActionValue = 2 Then
            Me.IsTrainer = True

            Me.TrainerSight = CInt(Me.AdditionalValue.GetSplit(0, "|"))
            If Me.AdditionalValue.Split("|").Count > 2 Then
                Me.TrainerCheckCollision = CBool(Me.AdditionalValue.GetSplit(2, "|"))
            End If
            Me.AdditionalValue = Me.AdditionalValue.GetSplit(1, "|")
        End If

        Me.DropUpdateUnlessDrawn = False
    End Sub

    Public Sub SetupSprite(ByVal UseTextureID As String, ByVal GameJoltID As String, ByVal UseGameJoltID As Boolean)
        Me.TextureID = UseTextureID

        Dim texturePath As String = "Textures\NPC\"
        If Me.TextureID.StartsWith("[POKEMON|N]") = True Or Me.TextureID.StartsWith("[POKEMON|N]") = True Then
            Me.TextureID = Me.TextureID.Remove(0, 11)
            texturePath = "Pokemon\Overworld\Normal\"
        ElseIf Me.TextureID.StartsWith("[POKEMON|S]") = True Or Me.TextureID.StartsWith("[POKEMON|S]") = True Then
            Me.TextureID = Me.TextureID.Remove(0, 11)
            texturePath = "Pokemon\Overworld\Shiny\"
        End If

        If Me.TextureID.ToLower = "<player.skin>" Then
            Me.TextureID = Core.Player.Skin
        End If

        If Me.TextureID.ToLower = "<rival.skin>" Then
            Me.TextureID = Core.Player.RivalSkin
        End If

        Dim PokemonAddition As String = ""

        If UseTextureID.StartsWith("Pokemon\Overworld\") = True OrElse UseTextureID.StartsWith("Pokemon\Battle\") = True Then
            texturePath = ""
            If StringHelper.IsNumeric(TextureID) = True Then
                PokemonAddition = PokemonForms.GetDefaultOverworldSpriteAddition(CInt(TextureID))
            End If
        End If

        If StringHelper.IsNumeric(Me.TextureID) = False Then
            If System.IO.File.Exists(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & texturePath & Me.TextureID & PokemonAddition & ".png") = False Then
                If Me.TextureID.Contains("_") Then
                    Me.TextureID = Me.TextureID.GetSplit(0, "_")
                ElseIf Me.TextureID.Contains("-") Then
                    Me.TextureID = Me.TextureID.GetSplit(0, "-")
                ElseIf Me.TextureID.Contains(";") Then
                    Dim p As Pokemon = Pokemon.GetPokemonByID(CInt(Me.TextureID.GetSplit(0, ";")), Me.TextureID.GetSplit(1, ";"), True)
                    Dim formaddition = PokemonForms.GetOverworldAddition(p)
                    Me.TextureID = Me.TextureID.GetSplit(0, ";") & formaddition
                End If
            End If
        End If

        If UseGameJoltID = True And Core.Player.IsGameJoltSave = True And GameJolt.API.LoggedIn = True AndAlso Not GameJolt.Emblem.GetOnlineSprite(GameJoltID) Is Nothing Then
            Me.Texture = GameJolt.Emblem.GetOnlineSprite(GameJoltID)
        Else
            Me.Texture = P3D.TextureManager.GetTexture(texturePath & Me.TextureID & PokemonAddition)
        End If

        If Me.Texture.Width = Me.Texture.Height / 2 Then
            Me.FrameSize = New Vector2(CInt(Me.Texture.Width / 2), CInt(Me.Texture.Height / 4))
        ElseIf Me.Texture.Width = Me.Texture.Height Then
            Me.FrameSize = New Vector2(CInt(Me.Texture.Width / 4), CInt(Me.Texture.Height / 4))
        Else
            Me.FrameSize = New Vector2(CInt(Me.Texture.Width / 3), CInt(Me.Texture.Height / 4))
        End If

        If Me.Movement = Movements.Pokeball Then
            Me.FrameSize = New Vector2(Me.Texture.Width, Me.Texture.Height)
        End If

        lastRectangle = New Rectangle(0, 0, 0, 0)

        Me.ChangeTexture()
    End Sub

#Region "NPCData"

    Private Sub ApplyNPCData()
        If Core.Player.NPCData <> "" Then
            Dim Data() As String = Core.Player.NPCData.SplitAtNewline()

            For Each line As String In Data
                line = line.Remove(0, 1)
                line = line.Remove(line.Length - 1, 1)

                Dim file As String = line.GetSplit(0, "|")
                Dim ID As Integer = CInt(line.GetSplit(1, "|"))
                Dim action As String = line.GetSplit(2, "|")
                Dim addition As String = line.GetSplit(3, "|")

                If Me.NPCID = ID And Me.MapOrigin.ToLower() = file.ToLower() Then
                    Select Case action.ToLower()
                        Case "position"
                            Dim PositionData() As String = addition.Split(CChar(","))
                            Me.Position = New Vector3(CSng(PositionData(0).Replace(".", GameController.DecSeparator)) + Offset.X, CSng(PositionData(1).Replace(".", GameController.DecSeparator)) + Offset.Y, CSng(PositionData(2).Replace(".", GameController.DecSeparator)) + Offset.Z)
                        Case "remove"
                            Me.CanBeRemoved = True
                    End Select
                End If
            Next
        End If
    End Sub

    Public Shared Sub AddNPCData(ByVal Data As String)
        Data = "{" & Data & "}"

        If Core.Player.NPCData = "" Then
            Core.Player.NPCData = Data
        ElseIf Core.Player.NPCData.Contains(Data) = False Then
            Core.Player.NPCData &= Environment.NewLine & Data
        End If
    End Sub

    Public Shared Sub RemoveNPCData(ByVal file As String, ByVal ID As Integer, ByVal action As String, Optional ByVal addition As String = "")
        Dim Data As String
        If addition = "" Then
            Data = "{" & file & "|" & ID & "|" & action & "}"
        Else
            Data = "{" & file & "|" & ID & "|" & action & "|" & addition & "}"
        End If


        Dim NData() As String = Core.Player.NPCData.SplitAtNewline()
        Dim nList As List(Of String) = NData.ToList()
        If nList.Contains(Data) = True Then
            nList.Remove(Data)
        End If
        NData = nList.ToArray()

        Data = ""
        For i = 0 To NData.Count - 1
            If i <> 0 Then
                Data &= Environment.NewLine
            End If

            Data &= NData(i)
        Next

        Core.Player.NPCData = Data
    End Sub

    Public Shared Sub RemoveNPCData(ByVal FullData As String)
        Dim Data() As String = FullData.Split(CChar("|"))
        If Data.Count > 3 Then
            RemoveNPCData(Data(0), CInt(Data(1)), Data(2), Data(3))
        Else
            RemoveNPCData(Data(0), CInt(Data(1)), Data(2))
        End If

    End Sub

#End Region

    Private Function GetAnimationX() As Integer
        If Me.Texture.Width = Me.Texture.Height / 2 Then
            Select Case AnimationX
                Case 1
                    Return 0
                Case 2
                    Return 1
                Case 3
                    Return 0
                Case 4
                    Return 1
            End Select
        ElseIf Me.Texture.Width = Me.Texture.Height Then
            Select Case AnimationX
                Case 1
                    Return 0
                Case 2
                    Return 1
                Case 3
                    Return 2
                Case 4
                    Return 3
            End Select
        Else
            Select Case AnimationX
                Case 1
                    Return 0
                Case 2
                    Return 1
                Case 3
                    Return 0
                Case 4
                    Return 2
            End Select
        End If
        Return 0
    End Function

    Private Sub ChangeTexture()
        If Not Me.Texture Is Nothing Then
            Dim r As New Rectangle(0, 0, 0, 0)
            Dim cameraRotation As Integer = Me.getCameraRotation()
            Dim spriteIndex As Integer = Me.faceRotation - cameraRotation

            If spriteIndex < 0 Then
                spriteIndex += 4
            End If
            If spriteIndex > 3 Then
                spriteIndex -= 4
            End If

            Dim x As Integer = 0
            If Me.Moved > 0.0F Or AnimateIdle = True Then
                x = CInt(FrameSize.X) * GetAnimationX()
            End If

            If Me.Movement = Movements.Pokeball Then
                spriteIndex = 0
                x = 0
            End If

            Dim y As Integer = CInt(FrameSize.Y) * spriteIndex

            Dim xFrameSize As Integer = CInt(Me.FrameSize.X)
            Dim yFrameSize As Integer = CInt(Me.FrameSize.Y)

            If x < 0 Then
                x = 0
            End If

            If y < 0 Then
                y = 0
            End If

            If x + xFrameSize > Me.Texture.Width Then
                xFrameSize = Me.Texture.Width - x
            End If

            If y + yFrameSize > Me.Texture.Height Then
                yFrameSize = Me.Texture.Height - y
            End If

            r = New Rectangle(x, y, xFrameSize, yFrameSize)

            If r <> lastRectangle Then
                lastRectangle = r
                If Me.Texture IsNot Nothing Then
                    Textures(0) = TextureManager.GetTexture(Me.Texture, r, 1)
                End If
            End If
        End If
    End Sub

    Public Sub ActivateScript()
        If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            Dim oScreen As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
            If oScreen.ActionScript.IsReady = True Then
                SoundManager.PlaySound("select")
                Select Case Me.ActionValue
                    Case 0
                        oScreen.ActionScript.StartScript(Me.AdditionalValue, 1,,, "NPCInteract")
                    Case 1
                        oScreen.ActionScript.StartScript(Me.AdditionalValue, 0,,, "NPCInteract")
                    Case 3
                        oScreen.ActionScript.StartScript(Me.AdditionalValue.Replace("<br>", Environment.NewLine), 2,,, "NPCInteract")
                    Case Else
                        oScreen.ActionScript.StartScript(Me.AdditionalValue, 0,,, "NPCInteract")
                End Select
            End If
        End If
    End Sub

    Public Function CheckInSight() As Boolean
        If Me.TrainerSight > -1 And Screen.Level.PokemonEncounterData.EncounteredPokemon = False And Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            If CInt(Me.Position.Y) = CInt(Screen.Camera.Position.Y) And Screen.Camera.IsMoving() = False Then
                If Moved = 0.0F And Me.CanBeRemoved = False Then
                    If Screen.Camera.Position.X = CInt(Me.Position.X) Or CInt(Me.Position.Z) = Screen.Camera.Position.Z Then
                        Dim distance As Integer = 0
                        Dim correctFacing As Boolean = False

                        If Screen.Camera.Position.X = CInt(Me.Position.X) Then
                            distance = CInt(CInt(Me.Position.Z) - Screen.Camera.Position.Z)

                            If distance > 0 Then
                                If Me.faceRotation = 0 Then
                                    correctFacing = True
                                End If
                            Else
                                If Me.faceRotation = 2 Then
                                    correctFacing = True
                                End If
                            End If
                        ElseIf Screen.Camera.Position.Z = CInt(Me.Position.Z) Then
                            distance = CInt(CInt(Me.Position.X) - Screen.Camera.Position.X)

                            If distance > 0 Then
                                If Me.faceRotation = 1 Then
                                    correctFacing = True
                                End If
                            Else
                                If Me.faceRotation = 3 Then
                                    correctFacing = True
                                End If
                            End If
                        End If

                        If correctFacing = True Then
                            distance = distance.ToPositive()

                            If distance <= Me.TrainerSight Then

                                Dim canReach As Boolean = True
                                If TrainerCheckCollision = True Then
                                    Select Case faceRotation
                                        Case 0
                                            For i = CInt(Me.Position.Z - distance) To CInt(Me.Position.Z - 1)
                                                If CheckCollision(New Vector3(Me.Position.X, Me.Position.Y, i), False) = False Then
                                                    canReach = False
                                                    Exit For
                                                End If
                                            Next
                                        Case 2
                                            For i = CInt(Me.Position.Z + 1) To CInt(Me.Position.Z + distance)
                                                If CheckCollision(New Vector3(Me.Position.X, Me.Position.Y, i), False) = False Then
                                                    canReach = False
                                                    Exit For
                                                End If
                                            Next
                                        Case 1
                                            For i = CInt(Me.Position.X - distance) To CInt(Me.Position.X - 1)
                                                If CheckCollision(New Vector3(i, Me.Position.Y, Me.Position.Z), False) = False Then
                                                    canReach = False
                                                    Exit For
                                                End If
                                            Next
                                        Case 3
                                            For i = CInt(Me.Position.X + 1) To CInt(Me.Position.X + distance)
                                                If CheckCollision(New Vector3(i, Me.Position.Y, Me.Position.Z), False) = False Then
                                                    canReach = False
                                                    Exit For
                                                End If
                                            Next
                                    End Select

                                End If
                                If canReach = True Then

                                    Dim InSightMusic As String = "nomusic"

                                    If Me.IsTrainer = True Then
                                        Dim trainerFilePath As String = GameModeManager.GetScriptPath(Me.AdditionalValue & ".dat")
                                        Security.FileValidation.CheckFileValid(trainerFilePath, False, "NPC.vb")

                                        Dim trainerContent() As String = System.IO.File.ReadAllLines(trainerFilePath)
                                        For Each line As String In trainerContent
                                            Dim l As String = line.Trim()
                                            If l.ToLower.StartsWith("@trainer:") = True Then
                                                Dim trainerID As String = l.GetSplit(1, ":")
                                                If Trainer.IsBeaten(trainerID) = True Then
                                                    Return False
                                                    Exit Function
                                                Else
                                                    Dim t As New Trainer(trainerID)
                                                    InSightMusic = t.GetInSightMusic()
                                                End If
                                            ElseIf l.ToLower.StartsWith("@battle.starttrainer(") = True Then
                                                Dim trainerID As String = l.Remove(l.Length - 1, 1).Remove(0, "@battle.starttrainer(".Length)
                                                If Trainer.IsBeaten(trainerID) = True Then
                                                    Return False
                                                    Exit Function
                                                Else
                                                    Dim t As New Trainer(trainerID)
                                                    InSightMusic = t.GetInSightMusic()
                                                End If
                                            End If
                                        Next
                                    End If

                                    Dim needFacing As Integer = 0
                                    Select Case Me.faceRotation
                                        Case 0
                                            needFacing = 2
                                        Case 1
                                            needFacing = 3
                                        Case 2
                                            needFacing = 0
                                        Case 3
                                            needFacing = 1
                                    End Select

                                    CType(Core.CurrentScreen, OverworldScreen).TrainerEncountered = True
                                    If InSightMusic <> "nomusic" And InSightMusic <> "" Then
                                        MusicManager.Play(InSightMusic, True, 0.0F)
                                    End If
                                    Screen.Camera.StopMovement()
                                    Me.Movement = Movements.Still

                                    Dim offset As New Vector2(0, 0)
                                    Select Case Me.faceRotation
                                        Case 0
                                            offset.Y = -0.01F
                                        Case 1
                                            offset.X = -0.01F
                                        Case 2
                                            offset.Y = 0.01F
                                        Case 3
                                            offset.X = 0.01F
                                    End Select

                                    Dim s As String = "version=2" & Environment.NewLine &
                                        "@player.turnto(" & needFacing & ")" & Environment.NewLine

                                    With CType(Screen.Camera, OverworldCamera)
                                        If CType(Screen.Camera, OverworldCamera).ThirdPerson = True And IsOnScreen() = False Then
                                            Dim cPosition = .ThirdPersonOffset.X.ToString().ReplaceDecSeparator & "," & .ThirdPersonOffset.Y.ToString().ReplaceDecSeparator & "," & .ThirdPersonOffset.Z.ToString().ReplaceDecSeparator
                                            s &= "@camera.setfocus(npc," & Me.NPCID & ")" & Environment.NewLine &
                                                 "@sound.play(Emote_Exclamation)" & Environment.NewLine &
                                                 "@entity.showmessagebulb(1|" & CStr(Me.Position.X + offset.X & "|" & Me.Position.Y + 0.7F & "|" & Me.Position.Z + offset.Y & ")").ReplaceDecSeparator & Environment.NewLine &
                                                 "@npc.move(" & Me.NPCID & "," & distance - 1 & ")" & Environment.NewLine &
                                                 "@script.start(" & Me.AdditionalValue & ")" & Environment.NewLine &
                                                 "@camera.resetfocus" & Environment.NewLine &
                                                 "@camera.setposition(" & cPosition & ")" & Environment.NewLine &
                                                 ":end"
                                        Else
                                            s &= "@sound.play(Emote_Exclamation)" & Environment.NewLine &
                                            "@entity.showmessagebulb(1|" & CStr(Me.Position.X + offset.X & "|" & Me.Position.Y + 0.7F & "|" & Me.Position.Z + offset.Y & ")").ReplaceDecSeparator & Environment.NewLine &
                                            "@npc.move(" & Me.NPCID & "," & distance - 1 & ")" & Environment.NewLine &
                                            "@script.start(" & Me.AdditionalValue & ")" & Environment.NewLine &
                                            ":end"
                                        End If
                                    End With

                                    CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2,,, "NPCInSight")
                                    ActionScript.IsInSightScript = True
                                    Return True
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
        Return False

    End Function

    Public Overrides Sub ClickFunction()
        If Screen.TextBox.reDelay = 0.0F Then
            If Me.Movement = Movements.Straight Then
                StraightDirection = Me.faceRotation
            End If
            If Me.Moved = 0.0F Then
                Dim newHeading As Integer = Screen.Camera.GetPlayerFacingDirection() - 2
                If newHeading < 0 Then
                    newHeading += 4
                End If
                Me.faceRotation = newHeading

                ActivateScript()
            ElseIf Me.Moved < 0.3F Then
                Interacted = True
            End If
        End If
    End Sub

    Public Overrides Sub Update()
        NPCMovement()
        Move()

        ChangeTexture()

        MyBase.Update()
    End Sub

    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) As Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.2F
    End Function

    Public Overrides Sub UpdateEntity()
        If Me.Model Is Nothing Then
            If Me.Rotation.Y <> Screen.Camera.Yaw Then
                Me.Rotation.Y = Screen.Camera.Yaw
            End If
        Else
            Dim ChangeRotation As Integer = faceRotation + 2
            If ChangeRotation > 3 Then
                ChangeRotation -= 4
            End If
            If Me.Rotation.Y <> ChangeRotation Then
                Me.Rotation.Y = GetRotationFromInteger(ChangeRotation).Y
            End If
        End If
        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Dim state = GraphicsDevice.DepthStencilState
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead
            Draw(Me.BaseModel, Me.Textures, True)
            GraphicsDevice.DepthStencilState = state
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

#Region "Movement and Camera"

    Private Sub NPCMovement()
        Dim s As Screen = CurrentScreen
        While Not s.PreScreen Is Nothing And s.Identification <> Screen.Identifications.OverworldScreen
            s = s.PreScreen
        End While
        If s.Identification = Screen.Identifications.OverworldScreen Then
            If CType(s, OverworldScreen).ActionScript.IsReady = False Then
                Exit Sub
            End If
        End If
        Select Case Me.Movement
            Case Movements.Still, Movements.Pokeball
                'do nothing
            Case Movements.Turning
                If Me.TurningDelay > 0.0F Then
                    TurningDelay -= 0.1F
                    If TurningDelay <= 0.0F Then
                        Me.TurningDelay = 3.0F

                        Me.faceRotation += 1
                        If Me.faceRotation = 4 Then
                            Me.faceRotation = 0
                        End If
                        If Me.IsTrainer = True Then
                            CheckInSight()
                        End If
                    End If
                End If
            Case Movements.Looking
                If Me.Moved = 0.0F Then
                    If Core.Random.Next(0, 220) = 0 Then
                        Dim newRotation As Integer = Me.faceRotation
                        While newRotation = Me.faceRotation
                            newRotation = Core.Random.Next(0, 4)
                        End While
                        Me.faceRotation = newRotation
                        If Me.IsTrainer = True Then
                            CheckInSight()
                        End If
                    End If
                End If
            Case Movements.FacePlayer
                If Me.Moved = 0.0F Then
                    Dim oldRotation As Integer = Me.faceRotation

                    If Screen.Camera.Position.X = Me.Position.X Or Screen.Camera.Position.Z = Me.Position.Z Then
                        If Me.Position.X < Screen.Camera.Position.X Then
                            Me.faceRotation = 3
                        ElseIf Me.Position.X > Screen.Camera.Position.X Then
                            Me.faceRotation = 1
                        End If
                        If Me.Position.Z < Screen.Camera.Position.Z Then
                            Me.faceRotation = 2
                        ElseIf Me.Position.Z > Screen.Camera.Position.Z Then
                            Me.faceRotation = 0
                        End If
                    End If

                    If oldRotation <> Me.faceRotation And Me.IsTrainer = True Then
                        CheckInSight()
                    End If
                End If
            Case Movements.Walk
                If Me.Moved = 0.0F Then
                    If Core.Random.Next(0, 129) = 0 Then
                        Dim newRotation As Integer = Me.faceRotation
                        If Core.Random.Next(0, 3) = 0 Then
                            While newRotation = Me.faceRotation
                                newRotation = Core.Random.Next(0, 4)
                            End While
                        End If
                        Dim newPosition As Vector3 = (GetMove(newRotation) / Speed) + Me.Position
                        Dim canMove As Boolean = False

                        For Each r As Rectangle In Me.MoveRectangles
                            If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True AndAlso CheckCollision(newPosition) = True Then
                                canMove = True
                                Exit For
                            End If
                        Next
                        Dim CanRotate As New List(Of Integer)
                        While canMove = False
                            newRotation += 1
                            If newRotation > 3 Then
                                newRotation -= 4
                            End If
                            newPosition = (GetMove(newRotation) / Speed) + Me.Position
                            For Each r As Rectangle In Me.MoveRectangles
                                If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                    If CheckCollision(newPosition) = True Then
                                        canMove = True
                                        Exit While
                                    Else
                                        If CanRotate.Contains(newRotation) = False AndAlso newRotation <> Me.faceRotation Then
                                            CanRotate.Add(newRotation)
                                        End If
                                    End If
                                End If
                            Next
                            If newRotation = Me.faceRotation Then
                                Exit While
                            End If
                        End While

                        Me.faceRotation = newRotation

                        If canMove = True Then
                            Moved = 1.0F
                        Else
                            If CanRotate.Count > 0 Then
                                Me.faceRotation = CanRotate(Random.Next(0, CanRotate.Count))
                            End If
                        End If
                    End If
                End If
            Case Movements.Straight
                If Me.Moved = 0.0F Then
                    ''newRotation = rotation that is used for checking a possible position and for setting the new faceRotation
                    Dim newRotation As Integer = Me.faceRotation
                    ''frontRotation = original faceRotation
                    Dim frontRotation As Integer = Me.faceRotation

                    ''for the check if the NPC can rotate when it can't walk
                    Dim CanRotate As Boolean = False
                    Dim OnlyRotateRotation = Me.faceRotation

                    Dim contains As Boolean = False
                    Dim newPosition As Vector3 = (GetMove(newRotation) / Speed) + Me.Position
                    Dim blocked As Boolean = False

                    If CheckBlockedByPlayerOrNPC(newPosition) = True AndAlso StraightDirection <> -1 Then
                        newRotation = Me.StraightDirection
                        frontRotation = Me.StraightDirection
                        newPosition = (GetMove(newRotation) / Speed) + Me.Position
                        StraightDirection = -1
                    End If

                    If CheckBlockedByPlayerOrNPC(newPosition) = False Then
                        If CheckCollision(newPosition) = True Then
                            For Each r As Rectangle In Me.MoveRectangles
                                If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                    contains = True
                                    Exit For
                                End If
                            Next
                        End If
                    Else
                        For Each r As Rectangle In Me.MoveRectangles
                            If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                CanRotate = True
                                OnlyRotateRotation = newRotation
                                Exit For
                            End If
                        Next
                    End If
                    If contains = True Then
                        '' Only change faceRotation when it's possible to move
                        Me.faceRotation = newRotation
                        Moved = 1.0F
                    Else
                        '' If not possible to move forward, check right
                        newRotation = frontRotation + 1
                        If newRotation > 3 Then
                            newRotation = newRotation - 4
                        End If
                        newPosition = (GetMove(newRotation) / Speed) + Me.Position
                        If CheckBlockedByPlayerOrNPC(newPosition) = False Then
                            If CheckCollision(newPosition) = True Then
                                For Each r As Rectangle In Me.MoveRectangles
                                    If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                        contains = True
                                        Exit For
                                    End If
                                Next
                            End If
                        Else
                            For Each r As Rectangle In Me.MoveRectangles
                                If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                    CanRotate = True
                                    OnlyRotateRotation = newRotation
                                    Exit For
                                End If
                            Next
                        End If
                        If contains = True Then
                            '' Only change faceRotation when it's possible to move
                            Me.faceRotation = newRotation
                            Moved = 1.0F
                        Else
                            '' If not possible to move to the right, check left
                            newRotation = frontRotation - 1
                            If newRotation < 0 Then
                                newRotation = newRotation + 4
                            End If
                            newPosition = (GetMove(newRotation) / Speed) + Me.Position
                            If CheckBlockedByPlayerOrNPC(newPosition) = False Then
                                If CheckCollision(newPosition) = True Then
                                    For Each r As Rectangle In Me.MoveRectangles
                                        If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                            contains = True
                                            Exit For
                                        End If
                                    Next
                                End If
                            Else
                                For Each r As Rectangle In Me.MoveRectangles
                                    If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                        CanRotate = True
                                        OnlyRotateRotation = newRotation
                                        Exit For
                                    End If
                                Next
                            End If
                            If contains = True Then
                                '' Only change faceRotation when it's possible to move
                                Me.faceRotation = newRotation
                                Moved = 1.0F
                            Else
                                '' If not possible to move to the left, check behind
                                newRotation = frontRotation + 2
                                If newRotation > 3 Then
                                    newRotation = newRotation - 4
                                End If
                                newPosition = (GetMove(newRotation) / Speed) + Me.Position
                                If CheckBlockedByPlayerOrNPC(newPosition) = False Then
                                    If CheckCollision(newPosition) = True Then
                                        For Each r As Rectangle In Me.MoveRectangles
                                            If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                                contains = True
                                                Exit For
                                            End If
                                        Next
                                    End If
                                Else
                                    For Each r As Rectangle In Me.MoveRectangles
                                        If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                            CanRotate = True
                                            OnlyRotateRotation = newRotation
                                            Exit For
                                        End If
                                    Next
                                End If
                                If contains = True Then
                                    '' Only change faceRotation when it's possible to move
                                    Me.faceRotation = newRotation
                                    Moved = 1.0F
                                Else
                                    If CanRotate = True Then
                                        Me.faceRotation = OnlyRotateRotation
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
        End Select
    End Sub

    Private Function CheckCollision(ByVal newPosition As Vector3, Optional CheckPlayer As Boolean = True) As Boolean
        newPosition = New Vector3(CInt(newPosition.X), CInt(newPosition.Y), CInt(newPosition.Z))
        Dim oldPosition As Vector3 = Me.Position

        Dim blocked As Boolean = False

        If CheckPlayer = True Then
            '' check if player or a following Pokémon is not in the way
            If Screen.Camera.IsMoving() = False Then
                If CInt(Screen.Camera.Position.X) = newPosition.X And CInt(Screen.Camera.Position.Y) = newPosition.Y And CInt(Screen.Camera.Position.Z) = newPosition.Z Then
                    blocked = True
                End If
                If Screen.Level.OverworldPokemon.IsVisible = True Then
                    If CInt(Screen.Level.OverworldPokemon.Position.X) = newPosition.X And CInt(Screen.Level.OverworldPokemon.Position.Y) = newPosition.Y And CInt(Screen.Level.OverworldPokemon.Position.Z) = newPosition.Z Then
                        blocked = True
                    End If
                End If
            Else
                Dim cameraNewPosition As Vector3 = CType(Screen.Camera, OverworldCamera).LastStepPosition + Screen.Camera.PlannedMovement()
                Dim cameraOldPosition As Vector3 = CType(Screen.Camera, OverworldCamera).LastStepPosition

                If CInt(cameraNewPosition.X) = newPosition.X And CInt(cameraNewPosition.Y) = newPosition.Y And CInt(cameraNewPosition.Z) = newPosition.Z Then
                    blocked = True
                End If
                If Screen.Level.OverworldPokemon.IsVisible = True Then
                    If CInt(Screen.Level.OverworldPokemon.Position.X) = newPosition.X And CInt(Screen.Level.OverworldPokemon.Position.Y) = newPosition.Y And CInt(Screen.Level.OverworldPokemon.Position.Z) = newPosition.Z OrElse
                 CInt(cameraOldPosition.X) = newPosition.X And CInt(cameraOldPosition.Y) = newPosition.Y And CInt(cameraOldPosition.Z) = newPosition.Z Then
                        blocked = True
                    End If
                End If
            End If
            '' check if a NetworkPlayer is not in the way
            For Each Player As NetworkPlayer In Screen.Level.NetworkPlayers
                If CInt(Player.Position.X) = newPosition.X And CInt(Player.Position.Y) = newPosition.Y And CInt(Player.Position.Z) = newPosition.Z Then
                    blocked = True
                    Exit For
                End If
            Next
            '' check if a NetworkPokémon is not in the way
            For Each Pokemon As NetworkPokemon In Screen.Level.NetworkPokemon
                If CInt(Pokemon.Position.X) = newPosition.X And CInt(Pokemon.Position.Y) = newPosition.Y And CInt(Pokemon.Position.Z) = newPosition.Z Then
                    blocked = True
                    Exit For
                End If
            Next
        End If

        '' check if an NPC is not in the way
        For Each NPC As NPC In Screen.Level.GetNPCs()
            If CInt(NPC.Position.X) = newPosition.X And CInt(NPC.Position.Y) = newPosition.Y And CInt(NPC.Position.Z) = newPosition.Z And NPC.NPCID <> Me.NPCID Then
                blocked = True
                Exit For
            End If
        Next

        If blocked = True Then
            Return False
        End If

        Dim HasFloor As Boolean = False

        Dim Position2D As Vector3 = New Vector3(newPosition.X, newPosition.Y - 0.1F, newPosition.Z)
        For Each Floor As Entity In Screen.Level.Floors
            If Floor.boundingBox.Contains(Position2D) = ContainmentType.Contains Then
                HasFloor = True
            End If
        Next

        If HasFloor = False Then
            Return False
        End If

        For Each Entity As Entity In Screen.Level.Entities
            If Entity.boundingBox.Contains(newPosition) = ContainmentType.Contains Then
                If Entity.Collision = True Then
                    Return False
                End If
            End If
        Next

        Return True
    End Function
    Private Function CheckBlockedByPlayerOrNPC(ByVal newPosition As Vector3) As Boolean
        newPosition = New Vector3(CInt(newPosition.X), CInt(newPosition.Y), CInt(newPosition.Z))

        Dim blocked As Boolean = False

        '' check if player or a following Pokémon is not in the way
        If Screen.Camera.IsMoving() = False Then
            If CInt(Screen.Camera.Position.X) = newPosition.X And CInt(Screen.Camera.Position.Y) = newPosition.Y And CInt(Screen.Camera.Position.Z) = newPosition.Z Then
                blocked = True
            End If
            If Screen.Level.OverworldPokemon.IsVisible = True Then
                If CInt(Screen.Level.OverworldPokemon.Position.X) = newPosition.X And CInt(Screen.Level.OverworldPokemon.Position.Y) = newPosition.Y And CInt(Screen.Level.OverworldPokemon.Position.Z) = newPosition.Z Then
                    blocked = True
                End If
            End If
        Else
            Dim cameraNewPosition As Vector3 = CType(Screen.Camera, OverworldCamera).LastStepPosition + Screen.Camera.PlannedMovement()
            Dim cameraOldPosition As Vector3 = CType(Screen.Camera, OverworldCamera).LastStepPosition

            If CInt(cameraNewPosition.X) = newPosition.X And CInt(cameraNewPosition.Y) = newPosition.Y And CInt(cameraNewPosition.Z) = newPosition.Z Then
                blocked = True
            End If
            If Screen.Level.OverworldPokemon.IsVisible = True Then
                If CInt(Screen.Level.OverworldPokemon.Position.X) = newPosition.X And CInt(Screen.Level.OverworldPokemon.Position.Y) = newPosition.Y And CInt(Screen.Level.OverworldPokemon.Position.Z) = newPosition.Z OrElse
                CInt(cameraOldPosition.X) = newPosition.X And CInt(cameraOldPosition.Y) = newPosition.Y And CInt(cameraOldPosition.Z) = newPosition.Z Then
                    blocked = True
                End If
            End If
        End If
        '' check if a NetworkPlayer is not in the way
        For Each Player As NetworkPlayer In Screen.Level.NetworkPlayers
            If CInt(Player.Position.X) = newPosition.X And CInt(Player.Position.Y) = newPosition.Y And CInt(Player.Position.Z) = newPosition.Z Then
                blocked = True
                Exit For
            End If
        Next
        '' check if a NetworkPokémon is not in the way
        For Each Pokemon As NetworkPokemon In Screen.Level.NetworkPokemon
            If CInt(Pokemon.Position.X) = newPosition.X And CInt(Pokemon.Position.Y) = newPosition.Y And CInt(Pokemon.Position.Z) = newPosition.Z Then
                blocked = True
                Exit For
            End If
        Next
        Return blocked
    End Function

    Private Sub Move()
        If Moved > 0.0F Then
            If Not isDancing Then
                Me.Position += GetMove()
            End If

            If Me.Speed < 0 Then
                Moved += Me.Speed
            Else
                Moved -= Me.Speed
            End If

            If Me.MoveY < 0.0F Then
                Me.MoveY += Me.Speed
                If MoveY >= 0.0F Then
                    Me.MoveY = 0.0F
                End If
            ElseIf Me.MoveY > 0.0F Then
                Me.MoveY -= Me.Speed
                If MoveY <= 0.0F Then
                    Me.MoveY = 0.0F
                End If
            End If

            Me.AnimationDelay -= CSng(0.13 * (Math.Abs(Me.Speed) / NPC.STANDARD_SPEED))
            If AnimationDelay <= 0.0F Then
                AnimationDelay = AnimationDelayLength
                AnimationX += 1

                If AnimationX > 4 Then
                    AnimationX = 1
                End If
            End If

            If Moved <= 0.0F Then
                MoveAsync = False
                If Not isDancing Then
                    Me.Position = New Vector3(CInt(Me.Position.X), CSng(Me.Position.Y), CInt(Me.Position.Z))
                Else
                    isDancing = False
                End If
                Moved = 0.0F
                MoveY = 0.0F
                If Me.Movement = Movements.Straight AndAlso Me.Texture.Width <> Me.Texture.Height Then
                    If AnimationX_Offset = 0 Then
                        AnimationX_Offset = 2
                    Else
                        AnimationX_Offset = 0
                    End If
                Else
                    AnimationX_Offset = 0
                End If
                AnimationX = 1 + AnimationX_Offset
                AnimationDelay = AnimationDelayLength
                ChangeTexture()
                ApplyShaders()
                Speed = NPC.STANDARD_SPEED

                If Interacted = True Then
                    Dim newHeading As Integer = Screen.Camera.GetPlayerFacingDirection() - 2
                    If newHeading < 0 Then
                        newHeading += 4
                    End If
                    Me.faceRotation = newHeading

                    ActivateScript()
                    Interacted = False
                End If
            End If
        Else
            If Me.AnimateIdle = True Then
                Me.AnimationDelay -= 0.1F
                If AnimationDelay <= 0.0F Then
                    AnimationDelay = AnimationDelayLength
                    AnimationX += 1
                    If AnimationX > 4 Then
                        AnimationX = 1
                    End If
                End If
            End If
        End If
    End Sub

    Private Function GetMove(Optional ByVal rotation As Integer = 6) As Vector3
        Dim moveVector As Vector3
        Dim functionRotation As Integer
        If rotation = 6 Then
            functionRotation = Me.faceRotation
        Else
            functionRotation = rotation
        End If
        Select Case functionRotation
            Case 0
                moveVector = New Vector3(0, 0, -1) * Speed
            Case 1
                moveVector = New Vector3(-1, 0, 0) * Speed
            Case 2
                moveVector = New Vector3(0, 0, 1) * Speed
            Case 3
                moveVector = New Vector3(1, 0, 0) * Speed
        End Select
        If MoveY <> 0.0F Then
            Dim multi As Single = Me.Speed
            If multi < 0.0F Then
                multi *= -1
            End If
            If MoveY > 0 Then
                moveVector.Y = multi * 1
            Else
                moveVector.Y = multi * -1
            End If
        End If
        Return moveVector
    End Function

    Private Function getCameraRotation() As Integer
        Dim cameraRotation As Integer = 0
        Dim c As Camera = Screen.Camera

        Dim Yaw As Single = c.Yaw

        While Yaw < 0
            Yaw += MathHelper.TwoPi
        End While

        If Yaw <= MathHelper.Pi * 0.25F Or Yaw > MathHelper.Pi * 1.75F Then
            cameraRotation = 0
        End If
        If Yaw <= MathHelper.Pi * 0.75F And Yaw > MathHelper.Pi * 0.25F Then
            cameraRotation = 1
        End If
        If Yaw <= MathHelper.Pi * 1.25F And Yaw > MathHelper.Pi * 0.75F Then
            cameraRotation = 2
        End If
        If Yaw <= MathHelper.Pi * 1.75F And Yaw > MathHelper.Pi * 1.25F Then
            cameraRotation = 3
        End If

        Return cameraRotation
    End Function

    Private Sub ApplyShaders()
        Me.Shaders.Clear()
        For Each Shader As Shader In Screen.Level.Shaders
            Shader.ApplyShader({Me})
        Next
    End Sub

    Friend Function InCameraFocus() As Boolean
        If Screen.Camera.Name = "Overworld" Then
            Dim c = CType(Screen.Camera, OverworldCamera)

            If c.CameraFocusType = OverworldCamera.CameraFocusTypes.NPC Then
                If c.CameraFocusID = Me.NPCID Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Function IsOnScreen() As Boolean
        Return Screen.Camera.BoundingFrustum.Contains(Me.Position) <> ContainmentType.Disjoint
    End Function

#End Region

End Class