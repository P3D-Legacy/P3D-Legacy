Public Class NewOptionScreen

    Inherits Screen


    Dim TextSpeed As Integer = 2
    Dim MouseSpeed As Integer = 12
    Dim FOV As Single = 45.0F
    Dim C As OverworldCamera
    Dim Music As Integer = 50
    Dim Sound As Integer = 50
    Dim RenderDistance As Integer = 0
    Dim GraphicStyle As Integer = 1
    Dim ShowBattleAnimations As Integer = 0
    Dim DiagonalMovement As Boolean = True
    Dim Difficulty As Integer = 0
    Dim BattleStyle As Integer = 0
    Dim LoadOffsetMaps As Integer = 10
    Dim ViewBobbing As Boolean = True
    Dim ShowModels As Integer = 1
    Dim Muted As Integer = 0
    Dim GamePadEnabled As Boolean = True
    Dim PreferMultiSampling As Boolean = True

    Dim savedOptions As Boolean = True

    Dim ScreenIndex As Integer = 0
    Dim _nextIndex As Integer = 0
    Dim ControlList As New List(Of Control)

    'New stuff
    ''' <summary>
    ''' Texture from file: GUI\Menus\General
    ''' </summary>
    Private _texture As Texture2D
    ''' <summary>
    ''' Texture from file: GUI\Menus\Options
    ''' </summary>
    Private _menuTexture As Texture2D

    'Interface animation state values:
    Private _interfaceFade As Single = 0F
    Private _closing As Boolean = False
    Private _opening As Boolean = False
    Private _enrollY As Single = 0F
    Private _itemIntro As Single = 0F

    Private _pageFade As Single = 1.0F
    Private _pageOpening As Boolean = False
    Private _pageClosing As Boolean = False

    'cursor animation:
    Private _cursorPosition As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2) - 400 + 90, CInt(Core.windowSize.Height / 2) - 200 + 80)
    Private _cursorDestPosition As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2) - 400 + 90, CInt(Core.windowSize.Height / 2) - 200 + 80)

    Private _selectedScrollBar As Boolean = False


    Public Sub New(ByVal currentScreen As Screen)
        'New stuff
        _texture = TextureManager.GetTexture("GUI\Menus\General")
        _menuTexture = TextureManager.GetTexture("GUI\Menus\Options")

        '''
        Me.Identification = Identifications.OptionScreen
        Me.PreScreen = currentScreen
        Me.CanChat = False
        Me.MouseVisible = True
        Me.CanBePaused = False
        Me._opening = True
        SetFunctionality()
    End Sub

    Private Sub SetFunctionality()
        Me.C = CType(Screen.Camera, OverworldCamera)
        Me.FOV = C.FOV
        Me.TextSpeed = TextBox.TextSpeed
        Me.MouseSpeed = CInt(C.RotationSpeed * 10000)
        Me.Music = CInt(MusicManager.MasterVolume * 100)
        Me.Sound = CInt(SoundManager.Volume * 100)
        Me.RenderDistance = Core.GameOptions.RenderDistance
        Me.GraphicStyle = Core.GameOptions.GraphicStyle
        Me.ShowBattleAnimations = Core.Player.ShowBattleAnimations
        Me.DiagonalMovement = Core.Player.DiagonalMovement
        Me.Difficulty = Core.Player.DifficultyMode
        Me.BattleStyle = Core.Player.BattleStyle
        Me.ShowModels = CInt(Core.Player.ShowModelsInBattle)
		Me.Muted = CInt(MusicManager.Muted.ToNumberString())
		If Core.GameOptions.LoadOffsetMaps = 0 Then
            Me.LoadOffsetMaps = 0
        Else
            Me.LoadOffsetMaps = 101 - Core.GameOptions.LoadOffsetMaps
        End If
        Me.ViewBobbing = Core.GameOptions.ViewBobbing
        Me.GamePadEnabled = Core.GameOptions.GamePadEnabled
        Me.PreferMultiSampling = Core.GraphicsManager.PreferMultiSampling
    End Sub


    Public Overrides Sub Draw()
        PreScreen.Draw()
        DrawBackground()
        DrawCurrentPage()
        DrawCursor()
        DrawMessage()

        TextBox.Draw()
        ChooseBox.Draw()

    End Sub

    Private Sub DrawBackground()
        Dim mainBackgroundColor As Color = Color.White
        If _closing Then
            mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
        End If

        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        Canvas.DrawRectangle(New Rectangle(halfWidth - 400, halfHeight - 232, 260, 32), New Color(84, 198, 216, mainBackgroundColor.A))
        Canvas.DrawRectangle(New Rectangle(halfWidth - 140, halfHeight - 216, 16, 16), New Color(84, 198, 216, mainBackgroundColor.A))
        SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 140, halfHeight - 232, 16, 16), New Rectangle(80, 0, 16, 16), mainBackgroundColor)
        SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 124, halfHeight - 216, 16, 16), New Rectangle(80, 0, 16, 16), mainBackgroundColor)

        SpriteBatch.DrawString(FontManager.ChatFont, "Options", New Vector2(halfWidth - 390, halfHeight - 228), mainBackgroundColor)

        For y = 0 To CInt(_enrollY) Step 16
            For x = 0 To 800 Step 16
                SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 400 + x, halfHeight - 200 + y, 16, 16), New Rectangle(64, 0, 4, 4), mainBackgroundColor)
            Next
        Next

        Dim modRes As Integer = CInt(_enrollY) Mod 16
        If modRes > 0 Then
            For x = 0 To 800 Step 16
                SpriteBatch.Draw(_texture, New Rectangle(halfWidth - 400 + x, CInt(_enrollY + (halfHeight - 200)), 16, modRes), New Rectangle(64, 0, 4, 4), mainBackgroundColor)
            Next
        End If
    End Sub
    Private Sub DrawCursor()
        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(_cursorPosition.X) + 60, CInt(_cursorPosition.Y) - 35, 48, 48), New Rectangle(0, 0, 16, 16), New Color(255, 255, 255, CInt(255 * Me._interfaceFade)), 0.0F, Vector2.Zero, SpriteEffects.None, 0.0F)
    End Sub
    Private Sub DrawCurrentPage()
        For Each C As Control In ControlList
            C.Draw()
        Next
    End Sub
    Private Sub DrawMessage()

    End Sub

    Public Overrides Sub Update()
        'New stuff
        If _opening Then
            InitializeControls()
            _opening = False
        End If

        If _closing Then
            ' When the interface is closing, only update the closing animation
            ' Once the interface is completely closed, set to the previous screen.

            If _interfaceFade > 0F Then
                _interfaceFade = MathHelper.Lerp(0, _interfaceFade, 0.8F)
                If _interfaceFade < 0F Then
                    _interfaceFade = 0F
                End If
            End If
            If _enrollY > 0 Then
                _enrollY = MathHelper.Lerp(0, _enrollY, 0.8F)
                If _enrollY <= 0 Then
                    _enrollY = 0
                End If
            End If
            If _enrollY <= 2.0F Then
                'TODO: Set the interface state to PlayerTemp.
                SetScreen(PreScreen)
            End If
        Else
            'Update intro animation:
            Dim maxWindowHeight As Integer = 400
            If _enrollY < maxWindowHeight Then
                _enrollY = MathHelper.Lerp(maxWindowHeight, _enrollY, 0.8F)
                If _enrollY >= maxWindowHeight Then
                    _enrollY = maxWindowHeight
                End If
            End If
            If _interfaceFade < 1.0F Then
                _interfaceFade = MathHelper.Lerp(1.0F, _interfaceFade, 0.95F)
                If _interfaceFade > 1.0F Then
                    _interfaceFade = 1.0F
                End If
            End If
            If _itemIntro < 1.0F Then
                _itemIntro += 0.05F
                If _itemIntro > 1.0F Then
                    _itemIntro = 1.0F
                End If
            End If

            'Update the Dialogues:
            ChooseBox.Update()
            If ChooseBox.Showing = False Then
                TextBox.Update()
            End If



            If _pageClosing = True Then
                If _pageFade >= 0F Then
                    _pageFade -= 0.07F
                    If _pageFade <= 0F Then
                        _pageFade = 0F
                        _pageClosing = False
                        _pageOpening = True
                        ScreenIndex = _nextIndex
                        InitializeControls()
                    End If
                End If
            End If
            If _pageOpening = True Then
                If _pageFade <= 1.0F Then
                    _pageFade += 0.07F
                    If _pageFade >= 1.0F Then
                        _pageFade = 1.0F
                        _pageClosing = False
                        _pageOpening = False
                    End If
                End If
            End If


            If _cursorDestPosition.X <> _cursorPosition.X Or _cursorDestPosition.Y <> _cursorPosition.Y Then
                _cursorPosition.X = MathHelper.Lerp(_cursorDestPosition.X, _cursorPosition.X, 0.75F)
                _cursorPosition.Y = MathHelper.Lerp(_cursorDestPosition.Y, _cursorPosition.Y, 0.75F)

                If Math.Abs(_cursorDestPosition.X - _cursorPosition.X) < 0.1F Then
                    _cursorPosition.X = _cursorDestPosition.X
                End If
                If Math.Abs(_cursorDestPosition.Y - _cursorPosition.Y) < 0.1F Then
                    _cursorPosition.Y = _cursorDestPosition.Y
                End If
            End If
            If Not _selectedScrollBar Then
                If Controls.Up(True, True, False, True, True, True) = True Then
                    SetCursorPosition("up")
                End If
                If Controls.Down(True, True, False, True, True, True) = True Then
                    SetCursorPosition("down")
                End If
                If Controls.Right(True, True, True, True, True, True) = True Then
                    SetCursorPosition("right")
                End If
                If Controls.Left(True, True, True, True, True, True) = True Then
                    SetCursorPosition("left")
                End If


                If Controls.Dismiss() Then
                    SoundManager.PlaySound("select")
                    If Me.ScreenIndex = 0 Then
                        _closing = True
                    Else
                        SwitchToMain()
                    End If
                End If
            End If
            For i = 0 To ControlList.Count
                If i <= ControlList.Count - 1 Then
                    ControlList(i).Update(Me)
                End If
            Next
        End If
    End Sub
    Private Sub SetCursorPosition(ByVal direction As String)
        Dim pos = GetButtonPosition(direction)
        'Dim cPosition As Vector2 = New Vector2(pos.X + 180, pos.Y - 42)
        Dim cPosition As Vector2 = New Vector2(pos.X, pos.Y)
        _cursorDestPosition = cPosition
    End Sub

    Private Function GetButtonPosition(ByVal direction As String) As Vector2
        Dim EligibleControls As New List(Of Control)
        Dim currentControl As Control = Nothing

        For Each control As Control In ControlList
            If control._position = _cursorDestPosition Then
                currentControl = control
                Exit For
            End If
        Next

        For Each control As Control In ControlList
            Dim R2 As Vector2 = control._position
            Dim R1 As Vector2 = currentControl._position

            If R1 = R2 Then
                Continue For
            End If

            Select Case direction
                Case "up"
                    If Math.Abs(R2.X - R1.X) <= -(R2.Y - R1.Y) Then 'because Y axis points down 
                        EligibleControls.Add(control)
                    End If
                Case "down"
                    If Math.Abs(R2.X - R1.X) <= -(R1.Y - R2.Y) Then 'because Y axis points down 
                        EligibleControls.Add(control)
                    End If
                Case "right"
                    If Math.Abs(R2.Y - R1.Y) <= (R2.X - R1.X) Then
                        EligibleControls.Add(control)
                    End If
                Case "left"
                    If Math.Abs(R2.Y - R1.Y) <= (R1.X - R2.X) Then
                        EligibleControls.Add(control)
                    End If
            End Select
        Next

        Dim NextPosition As New Vector2(currentControl._position.X, currentControl._position.Y)

        Dim cDistance As Double = 99999D
        For Each control As Control In EligibleControls
            Dim R2 As Vector2 = control._position
            Dim R1 As Vector2 = currentControl._position
            Dim DeltaR As Vector2 = R2 - R1
            Dim Distance As Double = DeltaR.Length
            If Distance < cDistance Then
                NextPosition = control._position
                cDistance = Distance
            End If
        Next

        Return NextPosition
    End Function

    Private Sub InitializeControls()
        Me.ControlList.Clear()
        Me._selectedScrollBar = False

        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        Dim Delta_X As Integer = halfWidth - 400
        Dim Delta_Y As Integer = halfHeight - 200

        Select Case Me.ScreenIndex
            Case 0 ' Main Options menu.
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 90, Delta_Y + 80), 1, 64, "Game", AddressOf SwitchToGame))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 310, Delta_Y + 80), 1, 64, "Graphics", AddressOf SwitchToGraphics))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530, Delta_Y + 80), 1, 64, "Battle", AddressOf SwitchToBattle))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 90, Delta_Y + 167), 1, 64, "Controls", AddressOf SwitchToControls))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 310, Delta_Y + 167), 1, 64, "Volume", AddressOf SwitchToVolume))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530, Delta_Y + 167), 1, 64, "Advanced", AddressOf SwitchToAdvanced))

                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 90 + 24, Delta_Y + 327), 1, 48, "Apply", AddressOf Apply))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, "Close", AddressOf Close))

            Case 1 ' "Game" from the Options menu.
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 60), 400, "Text Speed", Me.TextSpeed, 1, 3, AddressOf ChangeTextspeed))

                If CBool(GameModeManager.GetGameRuleValue("LockDifficulty", "0")) = False Then
                    Dim d As New Dictionary(Of Integer, String)
                    d.Add(0, "Easy")
                    d.Add(1, "Hard")
                    d.Add(2, "Super Hard")

                    Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 120), 400, "Difficulty", Me.Difficulty, 0, 2, AddressOf ChangeDifficulty, d))
                End If

                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 180), 3, 64, "View Bobbing", Me.ViewBobbing, AddressOf ToggleBobbing, {"Off", "On"}.ToList()))

                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, "Back", AddressOf SwitchToMain))

            Case 2 ' "Graphics" from the Options menu.
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 40), 400, "Field of View", CInt(Me.FOV), 45, 120, AddressOf ChangeFOV))

                Dim d As New Dictionary(Of Integer, String)
                d.Add(0, "Tiny")
                d.Add(1, "Small")
                d.Add(2, "Normal")
                d.Add(3, "Far")
                d.Add(4, "Extreme")
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 100), 400, "Render Distance", Me.RenderDistance, 0, 4, AddressOf ChangeRenderDistance, d))

                Dim d1 As New Dictionary(Of Integer, String)
                d1.Add(0, "Off")
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 160), 400, "Offset Map Quality", Me.LoadOffsetMaps, 0, 100, AddressOf ChangeOffsetMaps, d1))

                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 220), 3, 64, "Graphics", CBool(Me.GraphicStyle), AddressOf ToggleGraphicsStyle, {"Fast", "Fancy"}.ToList()))
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 300), 3, 64, "Multi Sampling", Me.PreferMultiSampling, AddressOf ToggleMultiSampling, {"Off", "On"}.ToList()))

                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, "Back", AddressOf SwitchToMain))
            Case 3 ' "Battle" from the Options menu.
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100 + 20, Delta_Y + 100), 2, 64, "3D Models", CBool(ShowModels), AddressOf ToggleShowModels, {"Off", "On"}.ToList()))
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 400 + 20, Delta_Y + 100), 2, 64, "Animations", CBool(Me.ShowBattleAnimations), AddressOf ToggleAnimations, {"Off", "On"}.ToList()))
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100 + 20, Delta_Y + 200), 2, 64, "Battle Style", CBool(Me.BattleStyle), AddressOf ToggleBattleStyle, {"Shift", "Set"}.ToList()))

                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, "Back", AddressOf SwitchToMain))
            Case 4 ' "Controls" from the Options menu.
                Dim d As New Dictionary(Of Integer, String)
                d.Add(1, "...Slow...")
                d.Add(12, "Standard")
                d.Add(38, "Super fast!")
                d.Add(50, "SPEED OF LIGHT!")
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 60), 400, "Mouse Speed", Me.MouseSpeed, 1, 50, AddressOf ChangeMouseSpeed, d))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 100, Delta_Y + 200), 3, 64, "Reset Key Bindings", AddressOf ResetKeyBindings))
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 120), 5, 64, "Xbox 360 Gamepad", Me.GamePadEnabled, AddressOf ToggleXBOX360Controller, {"Disabled", "Enabled"}.ToList()))

                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, "Back", AddressOf SwitchToMain))
            Case 5 ' "Volume" from the Options menu.
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 60), 400, "Music Volume", Me.Music, 0, 100, AddressOf ChangeMusicVolume))
                Me.ControlList.Add(New ScrollBar(New Vector2(Delta_X + 100, Delta_Y + 120), 400, "Sound Volume", Me.Sound, 0, 100, AddressOf ChangeSoundVolume))
                Me.ControlList.Add(New ToggleButton(New Vector2(Delta_X + 100, Delta_Y + 200), 1, 64, "Muted", CBool(Me.Muted), AddressOf ToggleMute, {"No", "Yes"}.ToList()))

                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, "Back", AddressOf SwitchToMain))
            Case 6 ' "Advanced" from the Options menu.
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 100, Delta_Y + 100), 2, 64, "Reset Options", AddressOf Reset))
                Me.ControlList.Add(New CommandButton(New Vector2(Delta_X + 530 + 24, Delta_Y + 327), 1, 48, "Back", AddressOf SwitchToMain))
        End Select

        '_cursorPosition = ControlList(0)._position
        _cursorDestPosition = ControlList(0)._position
    End Sub

    Private Sub Apply()
        Save()
        Close()
    End Sub

    Private Sub Close()
        _closing = True
    End Sub

    Private Sub Reset()
        Me.FOV = 45.0F
        Me.TextSpeed = 2
        Me.MouseSpeed = 12
        Me.Music = 50
        Me.Sound = 50
        Me.RenderDistance = 2
        Me.GraphicStyle = 1
        Me.ShowBattleAnimations = 2
        Me.DiagonalMovement = False
        Me.Difficulty = 0
        Me.BattleStyle = 1
        Me.LoadOffsetMaps = 90
        Me.ViewBobbing = True
        Me.ShowModels = 1
        Me.Muted = 0
        Me.GamePadEnabled = True
        Me.PreferMultiSampling = True
        MusicManager.Muted = CBool(Me.Muted)
        SoundManager.Muted = CBool(Me.Muted)
    End Sub

    Private Sub Save()
        C.CreateNewProjection(Me.FOV)
        TextBox.TextSpeed = Me.TextSpeed
        C.RotationSpeed = CSng(Me.MouseSpeed / 10000)
        MusicManager.MasterVolume = CSng(Me.Music / 100)
        SoundManager.Volume = CSng(Me.Sound / 100)
        MusicManager.Muted = CBool(Me.Muted)
        SoundManager.Muted = CBool(Me.Muted)
        Core.GameOptions.RenderDistance = Me.RenderDistance
        Core.GameOptions.GraphicStyle = Me.GraphicStyle
        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
        Core.Player.ShowBattleAnimations = Me.ShowBattleAnimations
        Core.Player.DiagonalMovement = Me.DiagonalMovement
        Core.Player.DifficultyMode = Me.Difficulty
        Core.Player.BattleStyle = Me.BattleStyle
        Core.Player.ShowModelsInBattle = CBool(Me.ShowModels)
        Core.GameOptions.GamePadEnabled = Me.GamePadEnabled
        Core.GraphicsManager.PreferMultiSampling = Me.PreferMultiSampling
        If LoadOffsetMaps = 0 Then
            Core.GameOptions.LoadOffsetMaps = Me.LoadOffsetMaps
        Else
            Core.GameOptions.LoadOffsetMaps = 101 - Me.LoadOffsetMaps
        End If
        Core.GameOptions.ViewBobbing = Me.ViewBobbing
        Core.GameOptions.SaveOptions()

        SoundManager.PlaySound("save")

        Me.PreScreen.Update()
    End Sub

    Public Overrides Sub ToggledMute()
        If Me.ScreenIndex = 5 Then
			Me.Muted = CInt(MusicManager.Muted)
			InitializeControls()
        End If
    End Sub

#Region "ControlCommands"

#Region "Switch"

    Private Sub SwitchToMain()
        Me._nextIndex = 0
        Me._pageClosing = True
    End Sub

    Private Sub SwitchToGame()
        Me._nextIndex = 1
        Me._pageClosing = True
    End Sub

    Private Sub SwitchToGraphics()
        Me._nextIndex = 2
        Me._pageClosing = True
    End Sub

    Private Sub SwitchToBattle()
        Me._nextIndex = 3
        Me._pageClosing = True
    End Sub

    Private Sub SwitchToControls()
        Me._nextIndex = 4
        Me._pageClosing = True
    End Sub

    Private Sub SwitchToVolume()
        Me._nextIndex = 5
        Me._pageClosing = True
    End Sub

    Private Sub SwitchToAdvanced()
        Me._nextIndex = 6
        Me._pageClosing = True
    End Sub

#End Region

#Region "SettingsGraphics"

    Private Sub ChangeFOV(ByVal c As ScrollBar)
        Me.FOV = c.Value
    End Sub

    Private Sub ChangeRenderDistance(ByVal c As ScrollBar)
        Me.RenderDistance = c.Value
    End Sub

    Private Sub ToggleGraphicsStyle(ByVal c As ToggleButton)
        If c.Toggled = True Then
            Me.GraphicStyle = 1
        Else
            Me.GraphicStyle = 0
        End If
    End Sub

    Private Sub ChangeOffsetMaps(ByVal c As ScrollBar)
        Me.LoadOffsetMaps = c.Value
    End Sub

    Private Sub ToggleMultiSampling(ByVal c As ToggleButton)
        Me.PreferMultiSampling = Not Me.PreferMultiSampling
    End Sub

#End Region

#Region "SettingsGame"

    Private Sub ToggleBobbing(ByVal c As ToggleButton)
        Me.ViewBobbing = Not Me.ViewBobbing
    End Sub

    Private Sub ChangeTextspeed(ByVal c As ScrollBar)
        Me.TextSpeed = c.Value
    End Sub

    Private Sub ChangeDifficulty(ByVal c As ScrollBar)
        Me.Difficulty = c.Value
    End Sub

#End Region

#Region "SettingsBattle"

    Private Sub ToggleShowModels(ByVal c As ToggleButton)
        If Me.ShowModels = 0 Then
            Me.ShowModels = 1
        Else
            Me.ShowModels = 0
        End If
    End Sub

    Private Sub ToggleAnimations(ByVal c As ToggleButton)
        If Me.ShowBattleAnimations = 0 Then
            Me.ShowBattleAnimations = 1
        Else
            Me.ShowBattleAnimations = 0
        End If
    End Sub

    Private Sub ToggleBattleStyle(ByVal c As ToggleButton)
        If Me.BattleStyle = 0 Then
            Me.BattleStyle = 1
        Else
            Me.BattleStyle = 0
        End If
    End Sub

#End Region

#Region "SettingsControls"

    Private Sub ToggleXBOX360Controller(ByVal c As ToggleButton)
        Me.GamePadEnabled = Not Me.GamePadEnabled
    End Sub

    Private Sub ChangeMouseSpeed(ByVal c As ScrollBar)
        Me.MouseSpeed = c.Value
    End Sub

    Private Sub ResetKeyBindings(ByVal c As CommandButton)
        KeyBindings.CreateKeySave(True)
        KeyBindings.LoadKeys()
    End Sub

#End Region

#Region "SettingsVolume"

    Private Sub ChangeMusicVolume(ByVal c As ScrollBar)
        Me.Music = c.Value
        ApplyMusicChange()
    End Sub

    Private Sub ChangeSoundVolume(ByVal c As ScrollBar)
        Me.Sound = c.Value
        ApplyMusicChange()
    End Sub

    Private Sub ToggleMute(ByVal c As ToggleButton)
        If Me.Muted = 0 Then
            Me.Muted = 1
        Else
            Me.Muted = 0
        End If
        ApplyMusicChange()
    End Sub

    Private Sub ApplyMusicChange()
        MusicManager.Muted = CBool(Me.Muted)
        SoundManager.Muted = CBool(Me.Muted)
        MusicManager.MasterVolume = CSng(Me.Music / 100)
        SoundManager.Volume = CSng(Me.Sound / 100)
    End Sub

#End Region

#End Region

#Region "Controls"

    MustInherit Class Control

        Public MustOverride Sub Draw()
        Public MustOverride Sub Update(ByRef s As NewOptionScreen)
        Public _position As Vector2 = New Vector2(0)

        Sub New()

        End Sub
    End Class

    Class ToggleButton

        Inherits Control


        Private _buttonWidth As Integer = 1
        Private _size As Integer = 1
        Private _text As String = ""
        Private _toggled As Boolean = False

        Public Property Position As Vector2
            Get
                Return _position
            End Get
            Set(value As Vector2)
                Me._position = value
            End Set
        End Property

        Public Property ButtonWidth As Integer
            Get
                Return Me._buttonWidth
            End Get
            Set(value As Integer)
                Me._buttonWidth = value
            End Set
        End Property

        Public Property Size As Integer
            Get
                Return Me._size
            End Get
            Set(value As Integer)
                Me._size = value
            End Set
        End Property

        Public Property Text As String
            Get
                Return Me._text
            End Get
            Set(value As String)
                Me._text = value
            End Set
        End Property

        Public Property Toggled As Boolean
            Get
                Return Me._toggled
            End Get
            Set(value As Boolean)
                Me._toggled = value
            End Set
        End Property

        Public Delegate Sub OnToggle(ByVal T As ToggleButton)
        Public OnToggleTrigger As OnToggle

        Public Settings As New List(Of String)

        Public Sub New(ByVal TriggerSub As OnToggle)
            MyBase.New
            Me.OnToggleTrigger = TriggerSub
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal ButtonWidth As Integer, ByVal Size As Integer, ByVal Text As String, ByVal Toggled As Boolean, ByVal TriggerSub As OnToggle)
            Me.New(Position, Size, ButtonWidth, Text, Toggled, TriggerSub, New List(Of String))
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal ButtonWidth As Integer, ByVal Size As Integer, ByVal Text As String, ByVal Toggled As Boolean, ByVal TriggerSub As OnToggle, ByVal Settings As List(Of String))
            MyBase.New
            Me._position = Position
            Me._buttonWidth = ButtonWidth
            Me._size = Size
            Me._text = Text
            Me._toggled = Toggled
            Me.OnToggleTrigger = TriggerSub
            Me.Settings = Settings
        End Sub


        Public Overrides Sub Draw()
            Dim s As NewOptionScreen = CType(CurrentScreen, NewOptionScreen)
            Dim pos As Vector2 = Me.Position
            Dim c As Color = New Color(255, 255, 255, CInt(255 * s._interfaceFade * s._pageFade))

            Dim size As Integer = Me.Size

            Dim B As New Vector2
            Dim t As String = Me.Text
            Dim textColor As New Color
            If Toggled Then
                t &= ": " & Settings(1)
                B.X = 16
                B.Y = 32
                textColor = (New Color(255, 255, 255, CInt(255 * s._interfaceFade * s._pageFade)))
            Else
                t &= ": " & Settings(0)
                B.X = 16
                B.Y = 16
                textColor = (New Color(0, 0, 0, CInt(255 * s._interfaceFade * s._pageFade)))
            End If

            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X), CInt(pos.Y), size, size), New Rectangle(CInt(B.X), CInt(B.Y), 16, 16), c)
            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X) + size, CInt(pos.Y), size * ButtonWidth, size), New Rectangle(CInt(B.X) + 16, CInt(B.Y), 16, 16), c)
            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X) + size * (ButtonWidth + 1), CInt(pos.Y), size, size), New Rectangle(CInt(B.X), CInt(B.Y), 16, 16), c, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)
            Dim fontWidth As Integer = CInt(FontManager.MainFont.MeasureString(t).X * 1.25 * (size / 64.0F))
            Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt((pos.X + (size * (2 + ButtonWidth) - fontWidth) * 0.5F)), CInt(pos.Y) + CInt(16 * size / 64)), textColor, 0.0F, Vector2.Zero, 1.25F * CSng(size / 64), SpriteEffects.None, 0.0F)

        End Sub

        Public Overrides Sub Update(ByRef s As NewOptionScreen)
            Dim r As New Rectangle(CInt(_position.X), CInt(_position.Y), (2 + ButtonWidth) * Size, Size)

            If r.Contains(MouseHandler.MousePosition) = True Then
                If P3D.Controls.Accept(True, False, False) = True Then
                    Me._toggled = Not Me._toggled
                    OnToggleTrigger(Me)
                    SoundManager.PlaySound("select")
                End If
            End If

            If Controls.Accept(False, True, True) Then
                If Position = s._cursorDestPosition Then
                    Me._toggled = Not Me._toggled
                    OnToggleTrigger(Me)
                    SoundManager.PlaySound("select")
                End If
            End If
        End Sub
    End Class

    Class CommandButton

        Inherits Control

        Private _buttonWidth As Integer = 1
        Private _size As Integer = 1
        Private _text As String = ""

        Public Property Position As Vector2
            Get
                Return _position
            End Get
            Set(value As Vector2)
                Me._position = value
            End Set
        End Property

        Public Property ButtonWidth As Integer
            Get
                Return Me._buttonWidth
            End Get
            Set(value As Integer)
                Me._buttonWidth = value
            End Set
        End Property

        Public Property Size As Integer
            Get
                Return Me._size
            End Get
            Set(value As Integer)
                Me._size = value
            End Set
        End Property

        Public Property Text As String
            Get
                Return Me._text
            End Get
            Set(value As String)
                Me._text = value
            End Set
        End Property

        Public Delegate Sub OnClick(ByVal C As CommandButton)
        Public OnClickTrigger As OnClick

        Public Sub New(ByVal ClickSub As OnClick)
            MyBase.New
            Me.OnClickTrigger = ClickSub
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal ButtonWidth As Integer, ByVal Size As Integer, ByVal Text As String, ByVal ClickSub As OnClick)
            MyBase.New
            Me._position = Position
            Me._buttonWidth = ButtonWidth
            Me._size = Size
            Me._text = Text
            Me.OnClickTrigger = ClickSub
        End Sub


        Public Overrides Sub Draw()
            Dim s As NewOptionScreen = CType(CurrentScreen, NewOptionScreen)

            Dim pos As Vector2 = Me.Position
            Dim c As Color = New Color(255, 255, 255, CInt(255 * s._interfaceFade * s._pageFade))


            Core.SpriteBatch.Draw(s._texture, New Rectangle(CInt(pos.X), CInt(pos.Y), Size, Size), New Rectangle(16, 16, 16, 16), c)
            Core.SpriteBatch.Draw(s._texture, New Rectangle(CInt(pos.X) + Size, CInt(pos.Y), Size * ButtonWidth, Size), New Rectangle(32, 16, 16, 16), c)
            Core.SpriteBatch.Draw(s._texture, New Rectangle(CInt(pos.X) + Size * (ButtonWidth + 1), CInt(pos.Y), Size, Size), New Rectangle(16, 16, 16, 16), c, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

            Dim fontWidth As Integer = CInt(FontManager.MainFont.MeasureString(Text).X * 1.25 * (Size / 64.0F))
            Core.SpriteBatch.DrawString(FontManager.MainFont, Text, New Vector2(CInt((pos.X + (Size * (2 + ButtonWidth) - fontWidth) * 0.5F)), CInt(pos.Y) + CInt(16 * Size / 64)), New Color(0, 0, 0, CInt(255 * s._interfaceFade * s._pageFade)), 0.0F, Vector2.Zero, 1.25F * CSng(Size / 64), SpriteEffects.None, 0.0F)
        End Sub

        Public Overrides Sub Update(ByRef s As NewOptionScreen)
            Dim r As New Rectangle(CInt(_position.X), CInt(_position.Y), (2 + ButtonWidth) * Size, Size)


            If r.Contains(MouseHandler.MousePosition) = True Then
                If P3D.Controls.Accept(True, False, False) = True Then
                    OnClickTrigger(Me)
                End If
            End If

            If Controls.Accept(False, True, True) Then
                If Position = s._cursorDestPosition Then
                    OnClickTrigger(Me)
                    SoundManager.PlaySound("select")
                End If
            End If
        End Sub
    End Class

    Class ScrollBar

        Inherits Control

        Private _size As Integer = 0
        Private _value As Integer = 0
        Private _max As Integer = 0
        Private _min As Integer = 0
        Private _text As String = ""
        Private _drawPercentage As Boolean = False

        Public Property Position As Vector2
            Get
                Return _position
            End Get
            Set(value As Vector2)
                Me._position = value
            End Set
        End Property

        Public Property Size As Integer
            Get
                Return Me._size
            End Get
            Set(value As Integer)
                Me._size = value
            End Set
        End Property

        Public Property Value As Integer
            Get
                Return Me._value
            End Get
            Set(value As Integer)
                Me._value = value
            End Set
        End Property

        Public Property Max As Integer
            Get
                Return Me._max
            End Get
            Set(value As Integer)
                Me._max = value
            End Set
        End Property

        Public Property Min As Integer
            Get
                Return Me._min
            End Get
            Set(value As Integer)
                Me._min = value
            End Set
        End Property

        Public Property Text As String
            Get
                Return Me._text
            End Get
            Set(value As String)
                Me._text = value
            End Set
        End Property

        Public Property DrawPercentage As Boolean
            Get
                Return Me._drawPercentage
            End Get
            Set(value As Boolean)
                Me._drawPercentage = value
            End Set
        End Property

        Public Delegate Sub OnChange(ByVal S As ScrollBar)
        Public OnChangeTrigger As OnChange

        Public Settings As New Dictionary(Of Integer, String)

        Dim Selected As Boolean = False
        Dim Clicked As Boolean = False

        Public Sub New(ByVal ChangeSub As OnChange)
            MyBase.New
            Me.OnChangeTrigger = ChangeSub
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal Size As Integer, ByVal Text As String, ByVal Value As Integer, ByVal Min As Integer, ByVal Max As Integer, ByVal ChangeSub As OnChange)
            Me.New(Position, Size, Text, Value, Min, Max, ChangeSub, New Dictionary(Of Integer, String))
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal Size As Integer, ByVal Text As String, ByVal Value As Integer, ByVal Min As Integer, ByVal Max As Integer, ByVal ChangeSub As OnChange, ByVal Settings As Dictionary(Of Integer, String))
            MyBase.New
            Me._position = Position
            Me._size = Size
            Me._text = Text
            Me._value = Value
            Me._max = Max
            Me._min = Min
            Me.Settings = Settings
            Me.OnChangeTrigger = ChangeSub
        End Sub

        Public Overrides Sub Draw()
            Dim length As Integer = _size + 16
            Dim height As Integer = 36

            Dim s As NewOptionScreen = CType(CurrentScreen, NewOptionScreen)
            Dim pos As Vector2 = Me.Position
            Dim c As Color = New Color(255, 255, 255, CInt(255 * s._interfaceFade * s._pageFade))

            Dim BarRectangle1 As Rectangle
            Dim BarRectangle2 As Rectangle
            Dim SliderRectangle As Rectangle
            Dim TextColor As Color

            If Selected OrElse Clicked Then
                BarRectangle1 = New Rectangle(0, 60, 12, 12)
                BarRectangle2 = New Rectangle(12, 60, 12, 12)
                SliderRectangle = New Rectangle(6, 32, 6, 12)
                TextColor = New Color(25, 67, 91, CInt(255 * s._interfaceFade * s._pageFade))
            Else
                BarRectangle1 = New Rectangle(0, 48, 12, 12)
                BarRectangle2 = New Rectangle(12, 48, 12, 12)
                SliderRectangle = New Rectangle(0, 32, 6, 12)
                TextColor = New Color(0, 0, 0, CInt(255 * s._interfaceFade * s._pageFade))
            End If


            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X), CInt(pos.Y), height, height), BarRectangle1, c)
            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X) + 36, CInt(pos.Y), length - 72, height), BarRectangle2, c)
            Core.SpriteBatch.Draw(s._menuTexture, New Rectangle(CInt(pos.X) + length - 36, CInt(pos.Y), height, height), BarRectangle1, c, 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

            Core.SpriteBatch.Draw(s._menuTexture, GetSliderBox, SliderRectangle, c)


            Dim t As String = Text & ": "

            If Settings.ContainsKey(Value) = True Then
                t &= Settings(Value)
            Else
                If Me._drawPercentage = True Then
                    t &= CStr(Me._value / (Me._max - Me._min) * 100)
                Else
                    t &= Me._value.ToString()
                End If
            End If
            Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(Me.Position.X + CSng((Me.Size / 2) - (FontManager.MiniFont.MeasureString(t).X / 2)), Me._position.Y + 6 - 25), TextColor)
        End Sub

        Public Overrides Sub Update(ByRef s As NewOptionScreen)
            If MouseHandler.ButtonDown(MouseHandler.MouseButtons.LeftButton) Then
                If GetSliderBox().Contains(MouseHandler.MousePosition.X, MouseHandler.MousePosition.Y) And Clicked = False Then
                    Clicked = True
                    Selected = False
                    s._selectedScrollBar = False
                End If
                If Clicked = True Then
                    Dim x As Double = MouseHandler.MousePosition.X - Me._position.X
                    If x < 0 Then
                        x = 0D
                    End If
                    If x > Me.Size + 16 Then
                        x = Me.Size + 16
                    End If

                    Me.Value = CInt(x * ((Me._max - Min) / 100) * (100 / Me._size)) + Min
                    Me.Value = Value.Clamp(Min, Max)

                    OnChangeTrigger(Me)
                End If
            Else
                Clicked = False
                If Selected Then
                    If Controls.Dismiss(False, True, True) OrElse Controls.Accept(False, True, True) Then
                        Selected = False
                        s._selectedScrollBar = False
                    ElseIf Controls.Left(True) Then
                        Me.Value = Me.Value - 1
                        Me.Value = Value.Clamp(Min, Max)
                        OnChangeTrigger(Me)
                    ElseIf Controls.Right(True) Then
                        Me.Value = Me.Value + 1
                        Me.Value = Value.Clamp(Min, Max)
                        OnChangeTrigger(Me)
                    End If
                Else
                    If Controls.Accept(False, True, True) Then
                        If s._cursorDestPosition = Me.Position Then
                            Selected = True
                            s._selectedScrollBar = True
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function GetSliderBox() As Rectangle
            Dim x As Integer = CInt(((100 / (Me._max - Min)) * (Me._value - Min)) * (_size / 100))

            If Me._value = Min Then
                x = 0
            Else
                If x = 0 And _value > 0 Then
                    x = 1
                End If
            End If

            Return New Rectangle(x + CInt(Me._position.X), CInt(Me.Position.Y), 18, 36)
        End Function

    End Class

#End Region



End Class