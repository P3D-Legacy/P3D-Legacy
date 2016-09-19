Public Class OverworldCamera

    Inherits Camera

#Region "Fields"

    Public oldX, oldY As Single

    Private _thirdPerson As Boolean = False
    Private _playerFacing As Integer = 0 'relative to the world, 0 means the player faces north, the camera might face in a different direction.

    Private _freeCameraMode As Boolean = False
    Private _cPosition As Vector3 = Vector3.Zero 'Actual camera position.
    Private _isFixed As Boolean = False
    Private _aimDirection As Integer = -1 'The direction the camera is aiming to face.

    Private _bobbingTemp As Single = 0F
    Private _tempDirectionPressed As Integer = -1
    Private _tempAmountOfSteps As Integer = 0 'Temp value to store what amount of steps have been walked.

    Private _waitForThirdPersonTurning As Integer = 0
    Private _notPressedThirdPersonDirectionButton As Integer = 0

    Private _scrollSpeed As Single = 0F
    Private _scrollDirection As Integer = 1
    Private _moved As Single = 0F

    Public LastStepPosition As Vector3 = New Vector3(0, -2, 0)
    Public YawLocked As Boolean = False
    Public ThirdPersonOffset As Vector3 = New Vector3(0F, 0.3F, 1.5F)

#End Region

#Region "Properties"

    ''' <summary>
    ''' Usually true, but if the player walks against an entitity that forces him to move up, set this to false so that the visual/audio feedback of walking against something don't appear.
    ''' </summary>
    Public Property DidWalkAgainst() As Boolean
        Get
            Return _didWalkAgainst
        End Get
        Set(value As Boolean)
            _didWalkAgainst = value
        End Set
    End Property

    Public Overrides ReadOnly Property IsMoving() As Boolean
        Get
            Return _moved > 0F
        End Get
    End Property

    Public ReadOnly Property FreeCameraMode() As Boolean
        Get
            Return _freeCameraMode
        End Get
    End Property

    Public ReadOnly Property ThirdPerson() As Boolean
        Get
            Return _thirdPerson
        End Get
    End Property

    Public ReadOnly Property CPosition() As Vector3
        Get
            Return _cPosition
        End Get
    End Property

    Public Property Fixed() As Boolean
        Get
            Return _isFixed
        End Get
        Set(value As Boolean)
            _isFixed = value
        End Set
    End Property

#End Region

#Region "CameraFocus"

    Public Enum CameraFocusTypes
        Player
        NPC
        Entity
    End Enum

    Private _cameraFocusType As CameraFocusTypes = CameraFocusTypes.Player
    Private _cameraFocusID As Integer = -1 'For NPC: NPCID; For Entity: EntityID

    Public Property CameraFocusType() As CameraFocusTypes
        Get
            Return _cameraFocusType
        End Get
        Set(value As CameraFocusTypes)
            _cameraFocusType = value
            If _thirdPerson = True Then
                SetThirdPerson(True, False)
                UpdateThirdPersonCamera()
            End If
        End Set
    End Property

    Public Property CameraFocusID() As Integer
        Get
            Return _cameraFocusID
        End Get
        Set(value As Integer)
            _cameraFocusID = value
            If _thirdPerson = True Then
                SetThirdPerson(True, False)
                UpdateThirdPersonCamera()
            End If
        End Set
    End Property

    Public Sub SetupFocus(ByVal FocusType As CameraFocusTypes, ByVal ID As Integer)
        _cameraFocusType = FocusType
        _cameraFocusID = ID
        If _thirdPerson = True Then
            SetThirdPerson(True, False)
            UpdateThirdPersonCamera()
        End If
    End Sub

#End Region

    ''' <summary>
    ''' If the camera is pointing straight north, east, south or west.
    ''' </summary>
    Public Function IsPointingToNormalDirection() As Boolean
        Return (Yaw = 0F Or Yaw = MathHelper.Pi * 0.5F Or Yaw = MathHelper.Pi Or Yaw = MathHelper.Pi * 1.5F)
    End Function

    Public Sub SetAimDirection(ByVal direction As Integer)
        _aimDirection = direction
    End Sub

    Public Sub New()
        MyBase.New("Overworld")

        Position = Core.Player.startPosition
        _thirdPerson = Core.Player.startThirdPerson
        RotationSpeed = CSng(Core.Player.startRotationSpeed / 10000)
        FOV = Core.Player.startFOV
        Yaw = Core.Player.startRotation
        _freeCameraMode = Core.Player.startFreeCameraMode

        Pitch = 0.0F

        CreateProjectionMatrix()
        UpdateViewMatrix()
        UpdateFrustum()
    End Sub

    Private Sub CreateProjectionMatrix()
        Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), GraphicsDevice.Viewport.AspectRatio, 0.01, FarPlane)
    End Sub


#Region "Update"

    Public Overrides Sub Update()
        Ray = CreateRay()

        PlayerMovement()

        ScrollThirdPersonCamera()

        LockCamera()

        CheckEntities()

        AimCamera()

        ControlCamera()

        UpdateThirdPersonCamera()

        SetSpeed()

        ControlThirdPersonCamera()

        UpdateViewMatrix()
        UpdateFrustum()
        ResetCursor()
    End Sub

    'Control camera with the cursor:
    Private Sub ControlCamera()
        Dim mState As MouseState = Mouse.GetState()
        Dim gState As GamePadState = GamePad.GetState(PlayerIndex.One)

        Dim dx As Single = mState.X - oldX
        If gState.ThumbSticks.Right.X <> 0.0F And Core.GameOptions.GamePadEnabled = True Then
            dx = gState.ThumbSticks.Right.X * 50.0F
        End If

        Dim dy As Single = mState.Y - oldY
        If gState.ThumbSticks.Right.Y <> 0.0F And Core.GameOptions.GamePadEnabled = True Then
            dy = gState.ThumbSticks.Right.Y * 40.0F * -1.0F
        End If

        If _isFixed = False Then
            If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                Dim OS As OverworldScreen = CType(CurrentScreen, OverworldScreen)

                If _freeCameraMode = True And OS.ActionScript.IsReady = True Then
                    If YawLocked = False Then
                        Yaw += -RotationSpeed * 0.75F * dx
                    End If
                End If
            End If

            Pitch += -RotationSpeed * dy
        End If

        ClampYaw()
        ClampPitch()
    End Sub

    Private Sub LockCamera()
        If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            Dim OS As OverworldScreen = CType(CurrentScreen, OverworldScreen)

            If OS.ActionScript.IsReady = True Then
                If (KeyBoardHandler.KeyPressed(KeyBindings.CameraLockKey) = True Or ControllerHandler.ButtonPressed(Buttons.RightStick)) = True And _moved = 0.0F And YawLocked = False Then
                    _freeCameraMode = Not _freeCameraMode

                    If _freeCameraMode = False Then
                        Core.GameMessage.ShowMessage(Localization.GetString("game_message_free_camera_off"), 12, FontManager.MainFont, Color.White)
                    Else
                        Core.GameMessage.ShowMessage(Localization.GetString("game_message_free_camera_on"), 12, FontManager.MainFont, Color.White)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ScrollThirdPersonCamera()
        If _isFixed = False Then
            If Controls.Down(True, False, True, False, False, False) = True Then
                If _scrollSpeed = 0.0F Or _scrollDirection <> 1 Then
                    _scrollSpeed = 0.01F
                End If

                _scrollDirection = 1
                _scrollSpeed += _scrollSpeed.Clamp(0, 0.01)
            End If

            If Controls.Up(True, False, True, False, False, False) = True Then
                If _scrollSpeed = 0.0F Or _scrollDirection <> -1 Then
                    _scrollSpeed = 0.01F
                End If

                _scrollDirection = -1
                _scrollSpeed += _scrollSpeed.Clamp(0, 0.01)
            End If

            _scrollSpeed = _scrollSpeed.Clamp(0, 0.08)

            If _scrollSpeed > 0.0F Then
                ThirdPersonOffset.Y += _scrollSpeed * _scrollDirection
                ThirdPersonOffset.Z += _scrollSpeed * _scrollDirection

                If GameController.IS_DEBUG_ACTIVE = False And Core.Player.SandBoxMode = False Then
                    ThirdPersonOffset.Y = ThirdPersonOffset.Y.Clamp(0, 1.32F)
                    ThirdPersonOffset.Z = ThirdPersonOffset.Z.Clamp(-0.1, 2.7F)
                End If

                _scrollSpeed -= 0.001F
                If _scrollSpeed <= 0.0F Then
                    _scrollSpeed = 0.0F
                End If
            End If
        End If
    End Sub

    Public Sub UpdateThirdPersonCamera()
        If _isFixed = False Then
            If KeyBoardHandler.KeyPressed(KeyBindings.PerspectiveSwitchKey) = True Or ControllerHandler.ButtonPressed(Buttons.LeftShoulder) = True Then
                Dim actionscriptReady As Boolean = True
                If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                    actionscriptReady = CType(CurrentScreen, OverworldScreen).ActionScript.IsReady
                End If
                If actionscriptReady = True Then
                    SetThirdPerson(Not _thirdPerson, True)
                End If
            End If

            Dim usePosition As Vector3 = Position
            Select Case _cameraFocusType
                Case CameraFocusTypes.Entity
                    Dim entList = (From ent As Entity In Screen.Level.Entities Select ent Where ent.ID = _cameraFocusID)
                    If entList.Count() > 0 Then
                        usePosition = entList(0).Position
                        usePosition.Y += 0.1F
                    End If
                Case CameraFocusTypes.NPC
                    Dim entList = (From ent As Entity In Screen.Level.Entities Select ent Where ent.GetType() = GetType(NPC) AndAlso CType(ent, NPC).NPCID = _cameraFocusID)
                    If entList.Count() > 0 Then
                        usePosition = entList(0).Position
                        usePosition.Y += 0.1F
                    End If
            End Select

            If _thirdPerson = True Then
                Dim rotationMatrix As Matrix = Matrix.CreateRotationY(Yaw)
                Dim offset As Vector3 = ThirdPersonOffset

                Dim transformedOffset As Vector3 = Vector3.Transform(offset, rotationMatrix)

                Dim diff As Vector3 = _cPosition - (usePosition + transformedOffset)
                For Each p As Particle In (From ent In Screen.Level.Entities Where ent.GetType() = GetType(Particle) Select ent)
                    p.MoveWithCamera(diff)
                Next

                _cPosition = usePosition + transformedOffset
            Else
                _cPosition = usePosition
            End If
        End If
    End Sub

    Public Sub SetThirdPerson(ByVal isThirdPerson As Boolean, ByVal showMessage As Boolean)
        If _thirdPerson <> isThirdPerson Then
            If isThirdPerson = True And _thirdPerson = False Then
                _playerFacing = GetFacingDirection()
            End If

            _thirdPerson = isThirdPerson
            ThirdPersonOffset = New Vector3(0F, 0.3F, 1.5F)
            If _thirdPerson = True Then
                Screen.Level.OwnPlayer.Opacity = 1.0F
                If showMessage = True Then
                    Core.GameMessage.ShowMessage(Localization.GetString("game_message_third_person_on"), 12, FontManager.MainFont, Color.White)
                End If
            Else
                Yaw = GetAimYawFromDirection(_playerFacing)
                If showMessage = True Then
                    Core.GameMessage.ShowMessage(Localization.GetString("game_message_third_person_off"), 12, FontManager.MainFont, Color.White)
                End If
            End If
        End If
    End Sub

    Public Sub UpdateFrustum()
        Dim rotation As Matrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw)

        Dim fPosition As New Vector3(_cPosition.X, _cPosition.Y + GetBobbing(), _cPosition.Z)

        Dim transformed As Vector3 = Vector3.Transform(New Vector3(0, 0, -1), rotation)
        Dim lookAt As Vector3 = fPosition + transformed

        BoundingFrustum = New BoundingFrustum(Matrix.CreateLookAt(fPosition, lookAt, Vector3.Up) * Projection)
    End Sub

    Public Sub UpdateViewMatrix()
        Dim rotation As Matrix = Matrix.CreateRotationX(Pitch) * Matrix.CreateRotationY(Yaw)

        Dim transformed As Vector3 = Vector3.Transform(New Vector3(0, 0, -1), rotation)
        Dim lookAt As Vector3 = New Vector3(_cPosition.X, _cPosition.Y + GetBobbing(), _cPosition.Z) + transformed

        View = Matrix.CreateLookAt(_cPosition, lookAt, Vector3.Up)
    End Sub

    Public Sub ResetCursor()
        If GameInstance.IsActive = True Then
            Mouse.SetPosition(CInt(windowSize.Width / 2), CInt(windowSize.Height / 2))
            oldX = CInt(windowSize.Width / 2)
            oldY = CInt(windowSize.Height / 2)
        End If
    End Sub

#End Region

#Region "CameraMethods"

    Private Sub SetSpeed()
        If CurrentScreen.Identification = Screen.Identifications.OverworldScreen AndAlso CType(CurrentScreen, OverworldScreen).ActionScript.IsReady = False Then
            Speed = 0.04F
        Else
            If Screen.Level.Riding = True Then
                Speed = 0.08F
            Else
                If Core.Player.IsRunning() = True Then
                    Speed = 0.06F
                Else
                    Speed = 0.04F
                End If
            End If
        End If
        Screen.Level.OverworldPokemon.MoveSpeed = Speed
    End Sub

    Private Function CreateRay() As Ray
        Dim centerX As Integer = CInt(windowSize.Width / 2)
        Dim centerY As Integer = CInt(windowSize.Height / 2)

        Dim nearSource As Vector3 = New Vector3(centerX, centerY, 0)
        Dim farSource As Vector3 = New Vector3(centerX, centerY, 1)

        Dim nearPoint As Vector3 = GraphicsDevice.Viewport.Unproject(nearSource, Projection, View, Matrix.Identity)
        Dim farPoint As Vector3 = GraphicsDevice.Viewport.Unproject(farSource, Projection, View, Matrix.Identity)

        Dim direction As Vector3 = farPoint - nearPoint
        direction.Normalize()

        Return New Ray(nearPoint, direction)
    End Function

    'Aims the camera to the set aim.
    Public Sub AimCamera()
        If _aimDirection > -1 And Turning = True Then
            Dim yawAim As Single = GetAimYawFromDirection(_aimDirection)

            Dim clockwise As Boolean = True

            If Yaw >= 0F And Yaw < MathHelper.Pi * 0.5F Then
                Select Case _aimDirection
                    Case 0
                        clockwise = True
                    Case 1
                        clockwise = False
                    Case 2
                        clockwise = False
                    Case 3
                        clockwise = True
                        yawAim -= MathHelper.TwoPi
                End Select
            ElseIf Yaw >= MathHelper.Pi * 0.5F And Yaw < MathHelper.Pi Then
                Select Case _aimDirection
                    Case 0
                        clockwise = True
                    Case 1
                        clockwise = True
                    Case 2
                        clockwise = False
                    Case 3
                        clockwise = False
                End Select
            ElseIf Yaw >= MathHelper.Pi And Yaw < MathHelper.Pi * 1.5F Then
                Select Case _aimDirection
                    Case 0
                        clockwise = False
                        yawAim += MathHelper.TwoPi
                    Case 1
                        clockwise = True
                    Case 2
                        clockwise = True
                    Case 3
                        clockwise = False
                End Select
            ElseIf Yaw >= MathHelper.Pi * 1.5F And Yaw < MathHelper.TwoPi Then
                Select Case _aimDirection
                    Case 0
                        clockwise = False
                        yawAim += MathHelper.TwoPi
                    Case 1
                        clockwise = False
                        yawAim += MathHelper.TwoPi
                    Case 2
                        clockwise = True
                    Case 3
                        clockwise = True
                End Select
            End If

            If clockwise = True Then
                ClampYaw()
                Yaw -= RotationSpeed * 35.0F
                If Yaw <= yawAim Then
                    Turning = False
                    _aimDirection = -1
                    Yaw = yawAim
                    ClampYaw()
                End If
            Else
                ClampYaw()
                Yaw += RotationSpeed * 35.0F
                If Yaw >= yawAim Then
                    Turning = False
                    _aimDirection = -1
                    Yaw = yawAim
                    ClampYaw()
                End If
            End If
        End If
    End Sub

    Private Sub ClampYaw()
        While Yaw < 0F
            Yaw += MathHelper.TwoPi
        End While
        While Yaw >= MathHelper.TwoPi
            Yaw -= MathHelper.TwoPi
        End While
    End Sub

    Private Sub ClampPitch()
        Pitch = MathHelper.Clamp(Pitch, -1.5f, 1.5f)
    End Sub

    'Changes the camera's pitch so you can see the stuff that is in front of you. Used when textboxes appear.
    Public Sub PitchForward()
        Dim aim As Single = -0.1F
        If ThirdPerson = True Then
            aim = -0.25F
        End If

        If Pitch > aim Then
            Pitch -= RotationSpeed * 35.0F
            If Pitch < aim Then
                Pitch = aim
            End If
        ElseIf Pitch < aim Then
            Pitch += RotationSpeed * 35.0F
            If Pitch > aim Then
                Pitch = aim
            End If
        End If
    End Sub

    Private Function GetBobbing() As Single
        If Core.GameOptions.ViewBobbing = False Then
            Return 0.0F
        End If
        If Screen.Level.Riding = True Then
            Return CSng(Math.Sin(_bobbingTemp) * 0.012)
        Else
            If Core.Player.IsRunning() = True Then
                Return CSng(Math.Sin(_bobbingTemp) * 0.008)
            Else
                Return CSng(Math.Sin(_bobbingTemp) * 0.004)
            End If
        End If
    End Function

#End Region

#Region "PlayerMethods"

    Private Sub PlayerMovement()
        If _moved > 0.0F And Turning = False Then
            Dim v As Vector3 = PlannedMovement * Speed

            Position += v

            _moved -= Speed
            If _moved <= 0.0F Then
                StopMovement()

                Position.X = Position.X.ToInteger()
                Position.Y = Position.Y.ToInteger() + 0.1F
                Position.Z = Position.Z.ToInteger()

                'If surfing, the player is set to Y = 0.0
                If Screen.Level.Surfing = True Then
                    Position.Y = CSng(Math.Floor(Position.Y))
                End If

                If _tempDirectionPressed > -1 Then
                    Turn(_tempDirectionPressed)
                End If
                _tempDirectionPressed = -1
                Screen.Level.OwnPlayer.DoAnimation = True

                If Core.GameOptions.GraphicStyle > 0 Then
                    If World.NoParticlesList.Contains(Screen.Level.World.CurrentMapWeather) = False Then
                        World.GenerateParticles(-1, Screen.Level.World.CurrentMapWeather)
                    End If
                End If

                Core.Player.TakeStep(_tempAmountOfSteps)
                _tempAmountOfSteps = 0

                LastStepPosition = Position
            End If

            If Screen.Level.Surfing = False And _thirdPerson = False And _cameraFocusType = CameraFocusTypes.Player Then
                _bobbingTemp += 0.25F
            End If
        End If

        Dim isActionscriptReady As Boolean = False
        Dim OS As OverworldScreen = Nothing
        If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            OS = CType(CurrentScreen, OverworldScreen)
            isActionscriptReady = OS.ActionScript.IsReady
        End If

        If isActionscriptReady = True And Screen.Level.CanMove() = True Then
            If _thirdPerson = False And _cameraFocusType = CameraFocusTypes.Player Then
                FirstPersonMovement()
            Else
                ThirdPersonMovement()
            End If
        End If

        If Screen.Level.Surfing = True Then
            Screen.Level.OwnPlayer.Opacity = 1.0F
        End If

        If _bumpSoundDelay > 0 Then
            _bumpSoundDelay -= 1
        End If
    End Sub

    Private Sub FirstPersonMovement()
        Dim pressedDirection As Integer = -1
        If YawLocked = False And Turning = False Then
            If (KeyBoardHandler.KeyDown(KeyBindings.LeftMoveKey) = True Or ControllerHandler.ButtonDown(Buttons.RightThumbstickLeft) = True) And Turning = False Then
                If _freeCameraMode = True Then
                    Yaw += RotationSpeed * 35.0F

                    ClampYaw()
                Else
                    pressedDirection = 1
                End If
            End If
            If (KeyBoardHandler.KeyDown(KeyBindings.BackwardMoveKey) = True Or ControllerHandler.ButtonDown(Buttons.LeftThumbstickDown) = True) And Turning = False Then
                If _freeCameraMode = True Then
                    If _moved <= 0F Then
                        Turn(2)
                    End If
                Else
                    pressedDirection = 2
                End If
            End If
            If (KeyBoardHandler.KeyDown(KeyBindings.RightMoveKey) = True Or ControllerHandler.ButtonDown(Buttons.RightThumbstickRight) = True) And Turning = False Then
                If _freeCameraMode = True Then
                    Yaw -= RotationSpeed * 35.0F

                    ClampYaw()
                Else
                    pressedDirection = 3
                End If
            End If

            If _freeCameraMode = False And pressedDirection > -1 Then
                If _moved <= 0F Then
                    Turn(pressedDirection)
                Else
                    _tempDirectionPressed = pressedDirection
                End If
            End If

            ClampYaw()

            If (KeyBoardHandler.KeyDown(KeyBindings.ForwardMoveKey) = True Or ControllerHandler.ButtonDown(Buttons.LeftThumbstickUp) = True) And Turning = False Then
                MoveForward()
            End If
        End If
    End Sub

    Private Sub ThirdPersonMovement()
        If _moved <= 0F Then
            Dim doMove As Boolean = False
            Dim newPlayerFacing As Integer = -1

            If IsThirdPersonMoveButtonDown(0) = True Then
                newPlayerFacing = GetFacingDirection() + 0
                doMove = True
            ElseIf IsThirdPersonMoveButtonDown(1) = True Then
                newPlayerFacing = GetFacingDirection() + 1
                doMove = True
            ElseIf IsThirdPersonMoveButtonDown(2) = True Then
                newPlayerFacing = GetFacingDirection() + 2
                doMove = True
            ElseIf IsThirdPersonMoveButtonDown(3) = True Then
                newPlayerFacing = GetFacingDirection() + 3
                doMove = True
            End If

            While newPlayerFacing > 3
                newPlayerFacing -= 4
            End While

            If doMove = True Then
                If newPlayerFacing <> _playerFacing Then
                    If IsThirdPersonMoveButtonDown(_playerFacing) = False And Core.Player.IsRunning() = False And _notPressedThirdPersonDirectionButton >= 3 Then
                        _waitForThirdPersonTurning = 5
                    End If
                    _playerFacing = newPlayerFacing
                    Screen.Level.OwnPlayer.Opacity = 1.0F
                Else
                    If _waitForThirdPersonTurning > 0 Then
                        _waitForThirdPersonTurning -= 1
                    Else
                        MoveForward()
                    End If
                End If
                _notPressedThirdPersonDirectionButton = 0
            Else
                If _notPressedThirdPersonDirectionButton < 3 Then
                    _notPressedThirdPersonDirectionButton += 1
                End If
            End If
        Else
            _waitForThirdPersonTurning = 0
            _notPressedThirdPersonDirectionButton = 0
        End If
    End Sub

    Private Function IsThirdPersonMoveButtonDown(ByVal facing As Integer) As Boolean
        Select Case facing
            Case 0
                Return KeyBoardHandler.KeyDown(KeyBindings.ForwardMoveKey) = True Or ControllerHandler.ButtonDown(Buttons.LeftThumbstickUp) = True Or ControllerHandler.ButtonDown(Buttons.DPadUp) = True
            Case 1
                Return KeyBoardHandler.KeyDown(KeyBindings.LeftMoveKey) = True Or ControllerHandler.ButtonDown(Buttons.LeftThumbstickLeft) = True Or ControllerHandler.ButtonDown(Buttons.DPadLeft) = True
            Case 2
                Return KeyBoardHandler.KeyDown(KeyBindings.BackwardMoveKey) = True Or ControllerHandler.ButtonDown(Buttons.LeftThumbstickDown) = True Or ControllerHandler.ButtonDown(Buttons.DPadDown) = True
            Case 3
                Return KeyBoardHandler.KeyDown(KeyBindings.RightMoveKey) = True Or ControllerHandler.ButtonDown(Buttons.LeftThumbstickRight) = True Or ControllerHandler.ButtonDown(Buttons.DPadRight) = True
        End Select
        Return False
    End Function

    Private _bumpSoundDelay As Integer = 35
    Private _didWalkAgainst As Boolean = True

    Private Sub MoveForward()
        If _moved <= 0F Then
            _didWalkAgainst = True
            If CheckCollision(GetForwardMovedPosition()) = False Then
                Screen.Level.OwnPlayer.Opacity = 1.0F

                Dim walkSteps As Integer = GetIceSteps(GetForwardMovedPosition())
                Screen.Level.OwnPlayer.DoAnimation = (walkSteps <= 1)

                Move(walkSteps)
                If _thirdPerson = False Then
                    _bumpSoundDelay = 35
                End If
            Else
                'Walked against something, set player transparent
                If Screen.Level.Surfing = False Then
                    If _thirdPerson = True Then
                        If _didWalkAgainst = True Then
                            Screen.Level.OwnPlayer.Opacity = 0.5F
                        End If
                        If _bumpSoundDelay = 0 Then
                            If _didWalkAgainst = True Then
                                SoundManager.PlaySound("bump")
                            End If
                            _bumpSoundDelay = 35
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Public Function CheckCollision(ByVal newPosition As Vector3) As Boolean
        Dim cannotWalk As Boolean = True
        Dim setSurfFalse As Boolean = False
        Dim Position2D As Vector3 = New Vector3(newPosition.X, newPosition.Y - 0.1F, newPosition.Z)
        For Each Floor As Entity In Screen.Level.Floors
            If Floor.boundingBox.Contains(Position2D) = ContainmentType.Contains Then
                cannotWalk = False
                setSurfFalse = True
            End If
        Next

        If cannotWalk = False Then
            For Each Entity As Entity In Screen.Level.Entities
                If Entity.boundingBox.Contains(newPosition) = ContainmentType.Contains Then
                    If cannotWalk = False Then
                        If Entity.Collision = True Then
                            cannotWalk = Entity.WalkAgainstFunction()
                        Else
                            cannotWalk = Entity.WalkIntoFunction()
                        End If
                    End If
                ElseIf Entity.boundingBox.Contains(New Vector3(newPosition.X, newPosition.Y - 1, newPosition.Z)) = ContainmentType.Contains Then
                    Entity.WalkOntoFunction()
                End If
            Next
        Else
            For Each Entity As Entity In Screen.Level.Entities
                If Entity.boundingBox.Contains(New Vector3(newPosition.X, newPosition.Y - 1, newPosition.Z)) = ContainmentType.Contains Then
                    Entity.WalkOntoFunction()
                End If
                If Screen.Level.Surfing = True Then
                    If Entity.boundingBox.Contains(newPosition) = ContainmentType.Contains Then
                        If Entity.Collision = True Then
                            Entity.WalkAgainstFunction()
                        Else
                            Entity.WalkIntoFunction()
                        End If
                    End If
                End If
            Next
        End If

        If cannotWalk = False And setSurfFalse = True Then
            If Screen.Level.Surfing = True Then
                Screen.Level.Surfing = False
                Screen.Level.OwnPlayer.SetTexture(Core.Player.TempSurfSkin, True)
                Core.Player.Skin = Core.Player.TempSurfSkin

                Screen.Level.OverworldPokemon.warped = True
                Screen.Level.OverworldPokemon.Visible = False

                If Screen.Level.IsRadioOn = False OrElse GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) = False Then
                    MusicManager.PlayMusic(Screen.Level.MusicLoop)
                End If
            End If
        End If

        'DebugFeature:
        If GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
            If KeyBoardHandler.KeyDown(Keys.LeftControl) = True Then
                cannotWalk = False
            End If
        End If

        Return cannotWalk
    End Function

    Private Function GetIceSteps(ByVal newPosition As Vector3) As Integer
        Dim Position2D As Vector3 = New Vector3(newPosition.X, newPosition.Y - 0.1F, newPosition.Z)
        For Each Floor As Entity In Screen.Level.Floors
            If Floor.boundingBox.Contains(Position2D) = ContainmentType.Contains Then
                If CType(Floor, Floor).IsIce = True Then
                    Return CType(Floor, Floor).GetIceFloors()
                End If
            End If
        Next

        Return 1
    End Function

    Public Overrides Function GetForwardMovedPosition() As Vector3
        Return Position + GetMoveDirection()
    End Function

    Public Overrides Function GetMoveDirection() As Vector3
        Dim v As Vector3 = PlannedMovement

        Select Case GetPlayerFacingDirection()
            Case 0 'North
                If v.Z = 0F Then
                    v.Z = -1.0F
                End If
            Case 1 'West
                If v.X = 0F Then
                    v.X = -1.0F
                End If
            Case 2 'South
                If v.Z = 0F Then
                    v.Z = 1.0F
                End If
            Case 3 'East
                If v.X = 0F Then
                    v.X = 1.0F
                End If
        End Select

        Return v
    End Function

    Public Overrides Function GetPlayerFacingDirection() As Integer
        If _thirdPerson = False And _cameraFocusType = CameraFocusTypes.Player Then
            Return GetFacingDirection()
        Else
            Return _playerFacing
        End If
    End Function

    Public Function GetAimYawFromDirection(ByVal direction As Integer) As Single
        Select Case direction
            Case 0
                Return 0F
            Case 1
                Return MathHelper.Pi * 0.5F
            Case 2
                Return MathHelper.Pi
            Case 3
                Return MathHelper.Pi * 1.5F
        End Select
        Return 0F
    End Function

    Public Overrides Sub Turn(ByVal turns As Integer)
        If turns > 0 Then
            If _thirdPerson = True Then
                _playerFacing += turns
                While _playerFacing > 3
                    _playerFacing -= 4
                End While
                Screen.Level.OwnPlayer.Opacity = 1.0F
            Else
                Dim facing As Integer = GetFacingDirection()
                facing += turns

                While facing > 3
                    facing -= 4
                End While

                Turning = True
                _aimDirection = facing
            End If
        End If
    End Sub

    Public Overrides Sub InstantTurn(ByVal turns As Integer)
        If turns > 0 Then
            If _thirdPerson = True Then
                'Set the camera so the player would't walk into a different direction when holding down the walk button.
                Yaw += GetAimYawFromDirection(turns)
                ClampYaw()

                _playerFacing += turns
                While _playerFacing > 3
                    _playerFacing -= 4
                End While
                Screen.Level.OwnPlayer.Opacity = 1.0F
            Else
                Dim newFacing As Integer = GetFacingDirection() + turns
                While newFacing > 3
                    newFacing -= 4
                End While
                Yaw = GetAimYawFromDirection(newFacing)
            End If
        End If
    End Sub

    Private Sub CheckEntities()
        If Controls.Accept() = True Then
            If _moved = 0F And Turning = False Then
                Dim checkPosition As Vector3 = GetForwardMovedPosition()
                checkPosition.Y -= 0.1F

                For i = 0 To Screen.Level.Entities.Count - 1
                    If i <= Screen.Level.Entities.Count - 1 Then
                        Dim result As Single? = Screen.Level.Entities(i).boundingBox.Intersects(Ray)
                        Dim RayIntersects As Boolean = True
                        If result.HasValue = True Then
                            Dim minValue As Single = 1.3F
                            If _thirdPerson = True Then
                                minValue += 1.8F
                            End If

                            If result.Value < minValue Then
                                RayIntersects = True
                            End If
                        End If

                        If RayIntersects = True And Screen.Level.Entities(i).boundingBox.Contains(checkPosition) = ContainmentType.Contains Then
                            Screen.Level.Entities(i).ClickFunction()
                        End If
                    Else
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Public Overrides Sub Move(Steps As Single)
        If Steps <> 0F Then
            _moved += Steps
            _tempAmountOfSteps += CInt(Math.Ceiling(Steps))
            If _setPlannedMovement = False Then
                _plannedMovement = GetMoveDirection()
            End If
        End If
    End Sub

    Public Overrides Sub StopMovement()
        _moved = 0F
        _plannedMovement = Vector3.Zero
        _setPlannedMovement = False
    End Sub

    Private Sub ControlThirdPersonCamera()
        If GameController.IS_DEBUG_ACTIVE = True Then
            If Controls.CtrlPressed() = True Then
                If KeyBoardHandler.KeyDown(KeyBindings.UpKey) = True Then
                    ThirdPersonOffset.Y += Speed
                End If
                If KeyBoardHandler.KeyDown(KeyBindings.DownKey) = True Then
                    ThirdPersonOffset.Y -= Speed
                End If
            Else
                If KeyBoardHandler.KeyDown(KeyBindings.UpKey) = True Then
                    ThirdPersonOffset.Z -= Speed
                End If
                If KeyBoardHandler.KeyDown(KeyBindings.DownKey) = True Then
                    ThirdPersonOffset.Z += Speed
                End If
                If KeyBoardHandler.KeyDown(KeyBindings.RightKey) = True Then
                    ThirdPersonOffset.X += Speed
                End If
                If KeyBoardHandler.KeyDown(KeyBindings.LeftKey) = True Then
                    ThirdPersonOffset.X -= Speed
                End If
            End If
        End If
    End Sub

#End Region

End Class
