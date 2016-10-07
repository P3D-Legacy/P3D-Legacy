Public Class NPC

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

    Public HasPokemonTexture As Boolean = False

    Public IsTrainer As Boolean = False
    Public TrainerSight As Integer = 1
    Public TrainerBeaten As Boolean = False
    Public TrainerChecked As Boolean = False

    Dim AnimateIdle As Boolean = True
    Dim AnimationX As Integer = 1
    Const AnimationDelayLenght As Single = 1.1F
    Dim AnimationDelay As Single = AnimationDelayLenght

    Public Movement As Movements = Movements.Still
    Public MoveRectangles As New List(Of Rectangle)
    Public TurningDelay As Single = 2.0F

    Public MoveY As Single = 0.0F
    Public MoveAsync As Boolean = False

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
            Me.AdditionalValue = Me.AdditionalValue.GetSplit(1, "|")
        End If

        Me.DropUpdateUnlessDrawn = False
    End Sub

    Public Sub SetupSprite(ByVal UseTextureID As String, ByVal GameJoltID As String, ByVal UseGameJoltID As Boolean)
        Me.TextureID = UseTextureID

        Dim texturePath As String = "Textures\NPC\"
        HasPokemonTexture = False
        If Me.TextureID.StartsWith("[POKEMON|N]") = True Or Me.TextureID.StartsWith("[Pokémon|N]") = True Then
            Me.TextureID = Me.TextureID.Remove(0, 11)
            texturePath = "Pokemon\Overworld\Normal\"
            HasPokemonTexture = True
        ElseIf Me.TextureID.StartsWith("[POKEMON|S]") = True Or Me.TextureID.StartsWith("[Pokémon|S]") = True Then
            Me.TextureID = Me.TextureID.Remove(0, 11)
            texturePath = "Pokemon\Overworld\Shiny\"
            HasPokemonTexture = True
        End If

        Dim PokemonAddition As String = ""

        If UseTextureID.StartsWith("Pokemon\Overworld\") = True Then
            texturePath = ""
            HasPokemonTexture = True
            If IsNumeric(TextureID) = True Then
                PokemonAddition = PokemonForms.GetDefaultOverworldSpriteAddition(CInt(TextureID))
            End If
        End If

        If UseGameJoltID = True And Core.Player.IsGameJoltSave = True And GameJolt.API.LoggedIn = True AndAlso Not GameJolt.Emblem.GetOnlineSprite(GameJoltID) Is Nothing Then
            Me.Texture = GameJolt.Emblem.GetOnlineSprite(GameJoltID)
        Else
            Me.Texture = net.Pokemon3D.Game.TextureManager.GetTexture(texturePath & Me.TextureID & PokemonAddition)
        End If

        Me.FrameSize = New Vector2(CInt(Me.Texture.Width / 3), CInt(Me.Texture.Height / 4))

        If HasPokemonTexture = True Then
            Me.FrameSize = New Vector2(Me.FrameSize.X, Me.FrameSize.Y)
        End If
        If Me.Movement = Movements.Pokeball Then
            Me.FrameSize = New Vector2(32, 32)
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
        Else
            Core.Player.NPCData &= vbNewLine & Data
        End If
    End Sub

    Public Shared Sub RemoveNPCData(ByVal file As String, ByVal ID As Integer, ByVal action As String, ByVal addition As String)
        Dim Data As String = "{" & file & "|" & ID & "|" & action & "|" & addition & "}"

        Dim NData() As String = Core.Player.NPCData.SplitAtNewline()
        Dim nList As List(Of String) = NData.ToList()
        If nList.Contains(Data) = True Then
            nList.Remove(Data)
        End If
        NData = nList.ToArray()

        Data = ""
        For i = 0 To NData.Count - 1
            If i <> 0 Then
                Data &= vbNewLine
            End If

            Data &= NData(i)
        Next

        Core.Player.NPCData = Data
    End Sub

    Public Shared Sub RemoveNPCData(ByVal FullData As String)
        Dim Data() As String = FullData.Split(CChar("|"))

        RemoveNPCData(Data(0), CInt(Data(1)), Data(2), Data(3))
    End Sub

#End Region

    Private Function GetAnimationX() As Integer
        If Me.HasPokemonTexture = True Then
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
        End If
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
        Return 1
    End Function

    Private Sub ChangeTexture()
        If Not Me.Texture Is Nothing Then
            Dim r As New Rectangle(0, 0, 0, 0)
            Dim cameraRotation As Integer = Me.getCameraRotation()
            Dim spriteIndex As Integer = Me.faceRotation - cameraRotation

            If spriteIndex < 0 Then
                spriteIndex += 4
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

                Textures(0) = TextureManager.GetTexture(Me.Texture, r, 1)
            End If
        End If
    End Sub

    Public Sub ActivateScript()
        Dim oScreen As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
        If oScreen.ActionScript.IsReady = True Then
            SoundManager.PlaySound("select")
            Select Case Me.ActionValue
                Case 0
                    oScreen.ActionScript.StartScript(Me.AdditionalValue, 1)
                Case 1
                    oScreen.ActionScript.StartScript(Me.AdditionalValue, 0)
                Case 3
                    oScreen.ActionScript.StartScript(Me.AdditionalValue.Replace("<br>", vbNewLine), 2)
                Case Else
                    oScreen.ActionScript.StartScript(Me.AdditionalValue, 0)
            End Select
        End If
    End Sub

    Public Sub CheckInSight()
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
                                Dim InSightMusic As String = "nomusic"

                                If Me.IsTrainer = True Then
                                    Dim trainerFilePath As String = GameModeManager.GetScriptPath(Me.AdditionalValue & ".dat")
                                    Security.FileValidation.CheckFileValid(trainerFilePath, False, "NPC.vb")

                                    Dim trainerContent() As String = System.IO.File.ReadAllLines(trainerFilePath)
                                    For Each line As String In trainerContent
                                        If line.StartsWith("@Trainer:") = True Then
                                            Dim trainerID As String = line.GetSplit(1, ":")
                                            If Trainer.IsBeaten(trainerID) = True Then
                                                Exit Sub
                                            Else
                                                Dim t As New Trainer(trainerID)
                                                InSightMusic = t.GetInSightMusic()
                                            End If
                                        ElseIf line.ToLower().StartsWith("@battle.starttrainer(") = True Then
                                            Dim trainerID As String = line.Remove(line.Length - 1, 1).Remove(0, "@battle.starttrainer(".Length)
                                            If Trainer.IsBeaten(trainerID) = True Then
                                                Exit Sub
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
                                Dim turns As Integer = needFacing - Screen.Camera.GetPlayerFacingDirection()
                                If turns < 0 Then
                                    turns = 4 - turns.ToPositive()
                                End If

                                CType(Core.CurrentScreen, OverworldScreen).TrainerEncountered = True
                                If InSightMusic <> "nomusic" And InSightMusic <> "" Then
                                    MusicManager.PlayMusic(InSightMusic, True, 0.0F, 0.0F)
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

                                Dim s As String = "version=2" & vbNewLine &
                                    "@player.turn(" & turns & ")" & vbNewLine

                                With CType(Screen.Camera, OverworldCamera)
                                    If CType(Screen.Camera, OverworldCamera).ThirdPerson = True And IsOnScreen() = False Then
                                        s &= "@camera.setfocus(npc," & Me.NPCID & ")" & vbNewLine
                                        Dim cPosition = .ThirdPersonOffset.X.ToString() & "," & .ThirdPersonOffset.Y.ToString() & "," & .ThirdPersonOffset.Z.ToString()
                                        s &= "@entity.showmessagebulb(1|" & Me.Position.X + offset.X & "|" & Me.Position.Y + 0.7F & "|" & Me.Position.Z + offset.Y & ")" & vbNewLine &
                                             "@npc.move(" & Me.NPCID & "," & distance - 1 & ")" & vbNewLine &
                                             "@camera.resetfocus" & vbNewLine &
                                             "@camera.setposition(" & cPosition & ")" & vbNewLine &
                                             "@script.start(" & Me.AdditionalValue & ")" & vbNewLine &
                                             ":end"
                                    Else
                                        s &= "@entity.showmessagebulb(1|" & Me.Position.X + offset.X & "|" & Me.Position.Y + 0.7F & "|" & Me.Position.Z + offset.Y & ")" & vbNewLine &
                                        "@npc.move(" & Me.NPCID & "," & distance - 1 & ")" & vbNewLine &
                                        "@script.start(" & Me.AdditionalValue & ")" & vbNewLine &
                                        ":end"
                                    End If
                                End With


                                Screen.Level.OwnPlayer.Opacity = 0.5F
                                CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2)
                                ActionScript.IsInsightScript = True
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Public Overrides Sub ClickFunction()
        Dim newHeading As Integer = Screen.Camera.GetPlayerFacingDirection() - 2
        If newHeading < 0 Then
            newHeading += 4
        End If
        Me.faceRotation = newHeading

        If Me.Moved = 0.0F Then
            ActivateScript()
        End If
    End Sub

    Public Overrides Sub Update()
        NPCMovement()
        Move()

        ChangeTexture()

        MyBase.Update()
    End Sub

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
        End If

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, True)
    End Sub

#Region "Movement and Camera"

    Private Sub NPCMovement()
        If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            If CType(Core.CurrentScreen, OverworldScreen).ActionScript.IsReady = False Then
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
                    If Core.Random.Next(0, 120) = 0 Then
                        If Core.Random.Next(0, 3) = 0 Then
                            Dim newRotation As Integer = Me.faceRotation
                            While newRotation = Me.faceRotation
                                newRotation = Core.Random.Next(0, 4)
                            End While
                            Me.faceRotation = newRotation
                        End If
                        Dim contains As Boolean = False
                        Dim newPosition As Vector3 = (GetMove() / Speed) + Me.Position
                        If CheckCollision(newPosition) = True Then
                            For Each r As Rectangle In Me.MoveRectangles
                                If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                    contains = True
                                    Exit For
                                End If
                            Next
                            If contains = True Then
                                Moved = 1.0F
                            End If
                        End If
                    End If
                End If
            Case Movements.Straight
                If Me.Moved = 0.0F Then
                    If Core.Random.Next(0, 15) = 0 Then
                        Dim newRotation As Integer = Me.faceRotation
                        While newRotation = Me.faceRotation
                            newRotation = Core.Random.Next(0, 4)
                        End While
                        Me.faceRotation = newRotation
                    End If
                    Dim contains As Boolean = False
                    Dim newPosition As Vector3 = (GetMove() / Speed) + Me.Position
                    If CheckCollision(newPosition) = True Then
                        For Each r As Rectangle In Me.MoveRectangles
                            If r.Contains(New Point(CInt(newPosition.X), CInt(newPosition.Z))) = True Then
                                contains = True
                                Exit For
                            End If
                        Next
                        If contains = True Then
                            Moved = 1.0F
                        End If
                    End If
                End If
        End Select
    End Sub

    Private Function CheckCollision(ByVal newPosition As Vector3) As Boolean
        newPosition = New Vector3(CInt(newPosition.X), CInt(newPosition.Y), CInt(newPosition.Z))

        Dim interactPlayer As Boolean = True

        If Screen.Camera.IsMoving() = False Then
            If CInt(Screen.Camera.Position.X) <> newPosition.X Or CInt(Screen.Camera.Position.Z) <> newPosition.Z Then
                If CInt(Screen.Level.OverworldPokemon.Position.X) <> newPosition.X Or CInt(Screen.Level.OverworldPokemon.Position.Z) <> newPosition.Z Then
                    interactPlayer = False
                End If
            End If
        End If

        If interactPlayer = True Then
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

    Private Sub Move()
        If Moved > 0.0F Then
            Me.Position += GetMove()

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
                AnimationDelay = AnimationDelayLenght
                AnimationX += 1
                If AnimationX > 4 Then
                    AnimationX = 1
                End If
            End If

            If Moved <= 0.0F Then
                MoveAsync = False
                Moved = 0.0F
                MoveY = 0.0F
                AnimationX = 1
                AnimationDelay = AnimationDelayLenght
                Me.Position = New Vector3(CInt(Me.Position.X), CInt(Me.Position.Y), CInt(Me.Position.Z))
                ChangeTexture()
                ApplyShaders()
                Speed = NPC.STANDARD_SPEED
            End If
        Else
            If Me.AnimateIdle = True Then
                Me.AnimationDelay -= 0.1F
                If AnimationDelay <= 0.0F Then
                    AnimationDelay = AnimationDelayLenght
                    AnimationX += 1
                    If AnimationX > 4 Then
                        AnimationX = 1
                    End If
                End If
            End If
        End If
    End Sub

    Private Function GetMove() As Vector3
        Dim moveVector As Vector3
        Select Case Me.faceRotation
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