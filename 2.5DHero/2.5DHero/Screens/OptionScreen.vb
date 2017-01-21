Public Class OptionScreen

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
    Dim ControlList As New List(Of Control)

    Shared YScroll As Integer = 0
    Dim CanYScroll As Boolean = False
    Dim YScrollLimit As Integer = 0

    Dim CurrentPath As String = "Options"

    Public Sub New(ByVal currentScreen As Screen)
        Me.Identification = Identifications.OptionScreen
        Me.PreScreen = currentScreen
        Me.CanChat = False
        Me.MouseVisible = True
        Me.CanBePaused = False

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
        Me.Muted = CInt(MediaPlayer.IsMuted.ToNumberString())
        If Core.GameOptions.LoadOffsetMaps = 0 Then
            Me.LoadOffsetMaps = 0
        Else
            Me.LoadOffsetMaps = 101 - Core.GameOptions.LoadOffsetMaps
        End If
        Me.ViewBobbing = Core.GameOptions.ViewBobbing
        Me.GamePadEnabled = Core.GameOptions.GamePadEnabled
        Me.PreferMultiSampling = Core.GraphicsManager.PreferMultiSampling

        InitializeControls()
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        Canvas.DrawImageBorder(TextureManager.GetTexture(TextureManager.GetTexture("GUI\Menus\Menu"), New Rectangle(0, 0, 48, 48)), 2, New Rectangle(60, 100, 800, 480))

        Core.SpriteBatch.DrawString(FontManager.InGameFont, Me.CurrentPath, New Vector2(80, 130), Color.Black)
        If savedOptions = False Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, Localization.GetString("option_screen_warning"), New Vector2(90 + FontManager.InGameFont.MeasureString(Localization.GetString("option_screen_title")).X, 138), Color.DarkRed)
        End If

        For Each C As Control In ControlList
            C.Draw()
        Next
    End Sub

    Public Overrides Sub Update()
        If CanYScroll = True Then
            If Controls.Down(True, False, True, False, False) = True Then
                YScroll += 1
            End If
            If Controls.Up(True, False, True, False, False) = True Then
                YScroll -= 1
            End If

            YScroll = YScroll.Clamp(0, YScrollLimit)
        Else
            YScroll = 0
        End If

        For i = 0 To ControlList.Count
            If i <= ControlList.Count - 1 Then
                ControlList(i).Update()
            End If
        Next

        If net.Pokemon3D.Game.Controls.Dismiss(True, True, True) = True Then
            Close()
        End If
    End Sub

    Private Sub InitializeControls()
        Me.ControlList.Clear()
        YScroll = 0

        Select Case Me.ScreenIndex
            Case 0 ' Main Options menu.
                Me.ControlList.Add(New CommandButton(New Vector2(100, 200), 6, "Game", AddressOf SwitchToGame))
                Me.ControlList.Add(New CommandButton(New Vector2(340, 200), 6, "Graphics", AddressOf SwitchToGraphics))
                Me.ControlList.Add(New CommandButton(New Vector2(580, 200), 6, "Battle", AddressOf SwitchToBattle))
                Me.ControlList.Add(New CommandButton(New Vector2(220, 320), 6, "Controls", AddressOf SwitchToControls))
                Me.ControlList.Add(New CommandButton(New Vector2(460, 320), 6, "Volume", AddressOf SwitchToVolume))

                Me.ControlList.Add(New CommandButton(New Vector2(120, 480), 6, "Apply", AddressOf Apply))
                Me.ControlList.Add(New CommandButton(New Vector2(630, 480), 6, "Close", AddressOf Close))
            Case 1 ' "Game" from the Options menu.
                Me.ControlList.Add(New ScrollBar(New Vector2(100, 200), 400, "Text Speed", Me.TextSpeed, 1, 3, AddressOf ChangeTextspeed))

                If CBool(GameModeManager.GetGameRuleValue("LockDifficulty", "0")) = False Then
                    Dim d As New Dictionary(Of Integer, String)
                    d.Add(0, "Easy")
                    d.Add(1, "Hard")
                    d.Add(2, "Super Hard")

                    Me.ControlList.Add(New ScrollBar(New Vector2(100, 250), 400, "Difficulty", Me.Difficulty, 0, 2, AddressOf ChangeDifficulty, d))
                End If

                Me.ControlList.Add(New ToggleButton(New Vector2(100, 300), 8, "View Bobbing", Me.ViewBobbing, AddressOf ToggleBobbing, {"Off", "On"}.ToList()))

                Me.ControlList.Add(New CommandButton(New Vector2(364, 480), 6, "Back", AddressOf SwitchToMain))
            Case 2 ' "Graphics" from the Options menu.
                Me.ControlList.Add(New ScrollBar(New Vector2(100, 200), 400, "Field of View", CInt(Me.FOV), 45, 120, AddressOf ChangeFOV))

                Dim d As New Dictionary(Of Integer, String)
                d.Add(0, "Tiny")
                d.Add(1, "Small")
                d.Add(2, "Normal")
                d.Add(3, "Far")
                d.Add(4, "Extreme")
                Me.ControlList.Add(New ScrollBar(New Vector2(100, 250), 400, "Render Distance", Me.RenderDistance, 0, 4, AddressOf ChangeRenderDistance, d))

                Dim d1 As New Dictionary(Of Integer, String)
                d1.Add(0, "Off")
                Me.ControlList.Add(New ScrollBar(New Vector2(100, 300), 400, "Offset Map Quality", Me.LoadOffsetMaps, 0, 100, AddressOf ChangeOffsetMaps, d1))

                Me.ControlList.Add(New ToggleButton(New Vector2(100, 350), 8, "Graphics", CBool(Me.GraphicStyle), AddressOf ToggleGraphicsStyle, {"Fast", "Fancy"}.ToList()))
                Me.ControlList.Add(New ToggleButton(New Vector2(400, 350), 8, "Multi Sampling", Me.PreferMultiSampling, AddressOf ToggleMultiSampling, {"Off", "On"}.ToList()))

                Me.ControlList.Add(New CommandButton(New Vector2(364, 480), 6, "Back", AddressOf SwitchToMain))
            Case 3 ' "Battle" from the Options menu.
                Me.ControlList.Add(New ToggleButton(New Vector2(100, 200), 8, "3D Models", CBool(ShowModels), AddressOf ToggleShowModels, {"Off", "On"}.ToList()))
                Me.ControlList.Add(New ToggleButton(New Vector2(400, 200), 8, "Animations", CBool(Me.ShowBattleAnimations), AddressOf ToggleAnimations, {"Off", "On"}.ToList()))
                Me.ControlList.Add(New ToggleButton(New Vector2(100, 320), 8, "Battle Style", CBool(Me.BattleStyle), AddressOf ToggleBattleStyle, {"Shift", "Set"}.ToList()))

                Me.ControlList.Add(New CommandButton(New Vector2(364, 480), 6, "Back", AddressOf SwitchToMain))
            Case 4 ' "Controls" from the Options menu.
                Dim d As New Dictionary(Of Integer, String)
                d.Add(1, "...Slow...")
                d.Add(12, "Standard")
                d.Add(38, "Super fast!")
                d.Add(50, "SPEED OF LIGHT!")
                Me.ControlList.Add(New ScrollBar(New Vector2(100, 200), 400, "Mouse Speed", Me.MouseSpeed, 1, 50, AddressOf ChangeMouseSpeed, d))
                Me.ControlList.Add(New CommandButton(New Vector2(100, 250), 9, "Reset Key Bindings", AddressOf ResetKeyBindings))
                Me.ControlList.Add(New ToggleButton(New Vector2(100, 370), 12, "Xbox 360 Gamepad", Me.GamePadEnabled, AddressOf ToggleXBOX360Controller, {"Disabled", "Enabled"}.ToList()))

                Me.ControlList.Add(New CommandButton(New Vector2(364, 480), 6, "Back", AddressOf SwitchToMain))
            Case 5 ' "Volume" from the Options menu.
                Me.ControlList.Add(New ScrollBar(New Vector2(100, 200), 400, "Music Volume", Me.Music, 0, 100, AddressOf ChangeMusicVolume))
                Me.ControlList.Add(New ScrollBar(New Vector2(100, 250), 400, "Sound Volume", Me.Sound, 0, 100, AddressOf ChangeSoundVolume))
                Me.ControlList.Add(New ToggleButton(New Vector2(100, 300), 8, "Muted", CBool(Me.Muted), AddressOf ToggleMute, {"No", "Yes"}.ToList()))

                Me.ControlList.Add(New CommandButton(New Vector2(364, 480), 6, "Back", AddressOf SwitchToMain))
        End Select
    End Sub

    Private Sub Apply()
        Save()
        Close()
    End Sub

    Private Sub Close()
        YScroll = 0
        Core.SetScreen(Me.PreScreen)
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
        Me.BattleStyle = 0
        Me.LoadOffsetMaps = 10
        Me.ViewBobbing = True
        Me.ShowModels = 1
        Me.Muted = 0
        Me.GamePadEnabled = True
        Me.PreferMultiSampling = True
    End Sub

    Private Sub Save()
        C.CreateNewProjection(Me.FOV)
        TextBox.TextSpeed = Me.TextSpeed
        C.RotationSpeed = CSng(Me.MouseSpeed / 10000)
        MusicManager.MasterVolume = CSng(Me.Music / 100)
        SoundManager.Volume = CSng(Me.Sound / 100)
        MusicManager.Mute(CBool(Me.Muted))
        SoundManager.Mute(CBool(Me.Muted))
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
            Me.Muted = CInt(MediaPlayer.IsMuted)
            InitializeControls()
        End If
    End Sub

#Region "ControlCommands"

#Region "Switch"

    Private Sub SwitchToMain()
        CurrentPath = "Options"
        Me.ScreenIndex = 0
        CanYScroll = False
        YScrollLimit = 0
        InitializeControls()
    End Sub

    Private Sub SwitchToGame()
        CurrentPath = "Options > Game"
        Me.ScreenIndex = 1
        CanYScroll = False
        YScrollLimit = 0
        InitializeControls()
    End Sub

    Private Sub SwitchToGraphics()
        CurrentPath = "Options > Graphics"
        Me.ScreenIndex = 2
        CanYScroll = False
        YScrollLimit = 0
        InitializeControls()
    End Sub

    Private Sub SwitchToBattle()
        CurrentPath = "Options > Battle"
        Me.ScreenIndex = 3
        CanYScroll = False
        YScrollLimit = 0
        InitializeControls()
    End Sub

    Private Sub SwitchToControls()
        CurrentPath = "Options > Controls"
        Me.ScreenIndex = 4
        CanYScroll = False
        YScrollLimit = 0
        InitializeControls()
    End Sub

    Private Sub SwitchToVolume()
        CurrentPath = "Options > Volume"
        Me.ScreenIndex = 5
        CanYScroll = False
        YScrollLimit = 0
        InitializeControls()
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
        MusicManager.Mute(CBool(Me.Muted))
        SoundManager.Mute(CBool(Me.Muted))
        MusicManager.MasterVolume = CSng(Me.Music / 100)
        SoundManager.Volume = CSng(Me.Sound / 100)
    End Sub

#End Region

#End Region

#Region "Controls"

    MustInherit Class Control

        Public MustOverride Sub Draw()
        Public MustOverride Sub Update()

    End Class

    Class ToggleButton

        Inherits Control

        Private _position As Vector2 = New Vector2(0)
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
            Me.OnToggleTrigger = TriggerSub
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal Size As Integer, ByVal Text As String, ByVal Toggled As Boolean, ByVal TriggerSub As OnToggle)
            Me.New(Position, Size, Text, Toggled, TriggerSub, New List(Of String))
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal Size As Integer, ByVal Text As String, ByVal Toggled As Boolean, ByVal TriggerSub As OnToggle, ByVal Settings As List(Of String))
            Me._position = Position
            Me._size = Size
            Me._text = Text
            Me._toggled = Toggled
            Me.OnToggleTrigger = TriggerSub
            Me.Settings = Settings
        End Sub


        Public Overrides Sub Draw()
            If _toggled = True Then
                Canvas.DrawImageBorder(TextureManager.GetTexture(TextureManager.GetTexture("GUI\Menus\Menu"), New Rectangle(0, 48, 48, 48)), 2, New Rectangle(CInt(_position.X), CInt(_position.Y) + YScroll, 32 * _size, 64))
            Else
                Canvas.DrawImageBorder(TextureManager.GetTexture(TextureManager.GetTexture("GUI\Menus\Menu"), New Rectangle(0, 0, 48, 48)), 2, New Rectangle(CInt(_position.X), CInt(_position.Y) + YScroll, 32 * _size, 64))
            End If

            Dim t As String = Me.Text
            If Settings.Count = 2 Then
                If Toggled = True Then
                    t &= ": " & Settings(1)
                Else
                    t &= ": " & Settings(0)
                End If
            End If

            Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(CInt(_position.X) + CInt(((Me._size * 32 + 20) / 2) - (FontManager.InGameFont.MeasureString(t).X / 2)), CInt(_position.Y) + 32 + YScroll), Color.Black)
        End Sub

        Public Overrides Sub Update()
            Dim r As New Rectangle(CInt(_position.X), CInt(_position.Y) + YScroll, 32 * _size, 96)

            If r.Contains(MouseHandler.MousePosition) = True Then
                If net.Pokemon3D.Game.Controls.Accept(True, False, False) = True Then
                    Me._toggled = Not Me._toggled
                    OnToggleTrigger(Me)
                End If
            End If
        End Sub
    End Class

    Class CommandButton

        Inherits Control

        Private _position As Vector2 = New Vector2(0)
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
            Me.OnClickTrigger = ClickSub
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal Size As Integer, ByVal Text As String, ByVal ClickSub As OnClick)
            Me._position = Position
            Me._size = Size
            Me._text = Text
            Me.OnClickTrigger = ClickSub
        End Sub


        Public Overrides Sub Draw()
            Dim r As New Rectangle(CInt(_position.X), CInt(_position.Y) + YScroll, Me._size * 32 + 32, 96)
            If r.Contains(MouseHandler.MousePosition) = True Then
                Canvas.DrawImageBorder(TextureManager.GetTexture(TextureManager.GetTexture("GUI\Menus\Menu"), New Rectangle(48, 0, 48, 48)), 2, New Rectangle(CInt(_position.X), CInt(_position.Y) + YScroll, 32 * _size, 64))
            Else
                Canvas.DrawImageBorder(TextureManager.GetTexture(TextureManager.GetTexture("GUI\Menus\Menu"), New Rectangle(0, 0, 48, 48)), 2, New Rectangle(CInt(_position.X), CInt(_position.Y) + YScroll, 32 * _size, 64))
            End If

            Core.SpriteBatch.DrawString(FontManager.InGameFont, Me._text, New Vector2(CInt(_position.X) + CInt(((Me._size * 32 + 20) / 2) - (FontManager.InGameFont.MeasureString(Me._text).X / 2)), CInt(_position.Y) + 32 + YScroll), Color.Black)
        End Sub

        Public Overrides Sub Update()
            Dim r As New Rectangle(CInt(_position.X), CInt(_position.Y) + YScroll, 32 * _size + 32, 96)

            If r.Contains(MouseHandler.MousePosition) = True Then
                If net.Pokemon3D.Game.Controls.Accept(True, False, False) = True Then
                    OnClickTrigger(Me)
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
        Private _position As Vector2 = New Vector2(0)
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

        Dim clicked As Boolean = False

        Public Sub New(ByVal ChangeSub As OnChange)
            Me.OnChangeTrigger = ChangeSub
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal Size As Integer, ByVal Text As String, ByVal Value As Integer, ByVal Min As Integer, ByVal Max As Integer, ByVal ChangeSub As OnChange)
            Me.New(Position, Size, Text, Value, Min, Max, ChangeSub, New Dictionary(Of Integer, String))
        End Sub

        Public Sub New(ByVal Position As Vector2, ByVal Size As Integer, ByVal Text As String, ByVal Value As Integer, ByVal Min As Integer, ByVal Max As Integer, ByVal ChangeSub As OnChange, ByVal Settings As Dictionary(Of Integer, String))
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
            Dim height As Integer = 32

            Canvas.DrawRectangle(New Rectangle(CInt(_position.X) - 2, CInt(_position.Y) - 2 + YScroll, length + 4, height + 4), Color.Black)

            Dim c As Color = Color.White
            If Me.clicked = True Then
                c = New Color(101, 142, 255)
            End If

            Canvas.DrawRectangle(New Rectangle(CInt(_position.X), CInt(_position.Y) + YScroll, length, height), c)

            Canvas.DrawRectangle(GetSliderRectangle, Color.Black)

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

            c = Color.Gray
            If Me.clicked = True Then
                c = Color.White
            End If

            Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(Me.Position.X + CSng((Me.Size / 2) - (FontManager.MiniFont.MeasureString(t).X / 2)), Me._position.Y + 6), c)
        End Sub

        Public Overrides Sub Update()
            If MouseHandler.ButtonDown(MouseHandler.MouseButtons.LeftButton) = True Then
                If GetSliderRectangle().Contains(MouseHandler.MousePosition.X, MouseHandler.MousePosition.Y) = True And clicked = False Then
                    clicked = True
                End If
            Else
                clicked = False
            End If

            If clicked = True Then
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
        End Sub

        Private Function GetSliderRectangle() As Rectangle
            Dim x As Integer = CInt(((100 / (Me._max - Min)) * (Me._value - Min)) * (_size / 100))

            If Me._value = Min Then
                x = 0
            Else
                If x = 0 And _value > 0 Then
                    x = 1
                End If
            End If

            Return New Rectangle(x + CInt(Me._position.X), CInt(Me.Position.Y) + YScroll, 16, 32)
        End Function

    End Class

#End Region
End Class