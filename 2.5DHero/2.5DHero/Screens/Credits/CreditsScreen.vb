Public Class CreditsScreen

    Inherits Screen

    Private Const COPYRIGHTYEAR As String = "2015"

    Dim CreditsPages As New List(Of CreditsPage)
    Dim CurrentPageIndex As Integer = 0

    Dim CameraLevels As New List(Of CameraLevel)
    Dim CurrentCameraLevelIndex As Integer = 0
    Dim ExecutedCameraLevel As Boolean = False

    Dim TheEnd As Boolean = False

    Public SavedOverworld As OverworldStorage

    Public Sub New(ByVal OverworldScreen As Screen)
        SavedOverworld = New OverworldStorage()
        SavedOverworld.SetToCurrentEnvironment()
    End Sub

    Public Sub InitializeScreen(ByVal ending As String)
        Me.Identification = Identifications.CreditsScreen
        Me.CanBePaused = False
        Me.MouseVisible = False
        Me.CanChat = False
        Me.CanDrawDebug = True
        Me.CanMuteMusic = True
        Me.CanTakeScreenshot = True

        Screen.TextBox.Showing = False
        Screen.PokemonImageView.Showing = False
        Screen.ChooseBox.Showing = False

        Effect = New BasicEffect(Core.GraphicsDevice)
        Effect.FogEnabled = True
        SkyDome = New SkyDome()
        Camera = New CreditsCamera()

        Level = New Level()
        InitializeCreditsPages(ending)
        InitializeCameraLevels(ending)

        ExecuteCameraLevel()

        MusicManager.PlayMusic("credits", True)
        MediaPlayer.IsRepeating = False
    End Sub

    Private Sub InitializeCreditsPages(ByVal ending As String)
        CreditsPages.Add(New CreditsPage("Pokémon3D staff", Color.White, Color.Black))
        CreditsPages.Add(New CreditsPage("Pokémon", Color.White, Color.Black, {"made by", "Nintendo", "Game Freak", "The Pokémon Company"}.ToList()))
        If GameModeManager.ActiveGameMode.IsDefaultGamemode = True Then
            CreditsPages.Add(New CreditsPage("Pokémon3D", Color.White, Color.Black, {"Trademark (TM) 2012 - " & COPYRIGHTYEAR, "made by Kolben Games"}.ToList()))
        Else
            CreditsPages.Add(New CreditsPage("Pokémon3D", Color.White, Color.Black, {"Trademark (TM) 2012 - " & COPYRIGHTYEAR, "made by Kolben Games", "", "GameMode made by", GameModeManager.ActiveGameMode.Author}.ToList()))
        End If
        CreditsPages.Add(New CreditsPage("Team Kolben", Color.White, Color.Black, {"Nils Drescher", "Andrew Leach", "Marc Boisvert-Dupras", "Grant Garrett", "Jason Houston", "Daniel Billing", "Benjamin Smith", "Hunter Graves"}.ToList()))
        CreditsPages.Add(New CreditsPage("Director", Color.White, Color.Black, {"Nils Drescher"}.ToList()))
        CreditsPages.Add(New CreditsPage("Programming", Color.White, Color.Black, {"Nils Drescher", "Jason Houston", "William Lang"}.ToList()))
        CreditsPages.Add(New CreditsPage("Graphics", Color.White, Color.Black, {"Nils Drescher", "Benjamin Smith", """Godeken""", "Caleb Coleman", "Robert Nobbmann", "Manuel Lampe", "Miguel Nunez", "Grant Garrett", """Anvil555"""}.ToList()))
        CreditsPages.Add(New CreditsPage("Map design", Color.White, Color.Black, {"Nils Drescher", "Benjamin Smith", "Hunter Graves", "Manuel Lampe", "Robert Nobbmann", "Maximilian Schröder", "Jan Mika Eine", "Jason Houston"}.ToList()))
        CreditsPages.Add(New CreditsPage("Actionscript", Color.White, Color.Black, {"Nils Drescher", "Benjamin Smith", "Hunter Graves", "Andrew Leach", "Jason Houston"}.ToList()))
        CreditsPages.Add(New CreditsPage("Script System development", Color.White, Color.Black, {"Nils Drescher", "Benjamin Smith", "Hunter Graves"}.ToList()))
        CreditsPages.Add(New CreditsPage("KolbenWindowWidget development", Color.White, Color.Black, {"Jason Houston"}.ToList()))
        CreditsPages.Add(New CreditsPage("LUA implementation", Color.White, Color.Black, {"Jason Houston"}.ToList()))
        CreditsPages.Add(New CreditsPage("Launcher programming", Color.White, Color.Black, {"Nils Drescher", "Daniel Billing", """ThuxCommix"""}.ToList()))
        CreditsPages.Add(New CreditsPage("Pokéditor Programming", Color.White, Color.Black, {"Nils Drescher", "Jason Houston", "Hunter Graves"}.ToList()))
        CreditsPages.Add(New CreditsPage("Website Host/Server maintenance", Color.White, Color.Black, {"Daniel Billing", "Daniel Laube"}.ToList()))
        CreditsPages.Add(New CreditsPage("GameJolt Service/API programming", Color.White, Color.Black, {"David DeCarmine", "Nils Drescher"}.ToList()))
        CreditsPages.Add(New CreditsPage("Debug testing", Color.White, Color.Black, {"Jan Mika Eine", "Tim Drescher", "Daniel Steinborn", "Andrew Leach", "Marc Boisvert-Dupras", "Matt Chambers", "Hunter Graves", "Benjamin Smith", "William Hunn", "Torben Carrington", """oXFantaXo"""}.ToList()))
        CreditsPages.Add(New CreditsPage("Special Thanks", Color.White, Color.Black, {"""MunchingOrange""", """TheFlamingSpade""", """SlyFoxHound""", """ArsenioDev""", """TrUShade""", """Isaaking6"""}.ToList()))
        CreditsPages.Add(New CreditsPage("Special Thanks", Color.White, Color.Black, {"Davey Van Raaij", "Diego López", "The GameJolt team", "The AppSharp team", "The Smogon University Team"}.ToList()))
        CreditsPages.Add(New CreditsPage("", Color.White, Color.Black, {"And probably a lot more.", "Especially all the awesome people from", "the pokemon3d.net community.", "Thanks for helping and playing this great game."}.ToList()))
        CreditsPages.Add(New CreditsPage("", Color.White, Color.Black))
        CreditsPages.Add(New CreditsPage("", Color.White, Color.Black))
        CreditsPages.Add(New CreditsPage("THE END", Color.White, Color.Black, {"Thank you for playing!"}.ToList()))
    End Sub

    Private Sub InitializeCameraLevels(ByVal ending As String)
        Select Case ending.ToLower()
            Case "johto"
                CameraLevels.Add(New CameraLevel("route29.dat", New Vector3(50, 2, 10), New Vector3(0, 2, 10), 0.04F, MathHelper.Pi * 1.5F, -0.3F))
                CameraLevels.Add(New CameraLevel("routes\route42.dat", New Vector3(50, 2, 10), New Vector3(0, 2, 10), 0.04F, MathHelper.Pi * 1.5F, -0.3F))
                CameraLevels.Add(New CameraLevel("route38.dat", New Vector3(34, 2, 10), New Vector3(0, 2, 10), 0.04F, MathHelper.Pi * 1.5F, -0.3F))
                CameraLevels.Add(New CameraLevel("routes\route45.dat", New Vector3(10, 2, 3), New Vector3(10, 2, 30), 0.04F, 0.0F, -0.3F))
                CameraLevels.Add(New CameraLevel("Ecruteak.dat", New Vector3(22, 0, 22), New Vector3(36, 14, 8), 0.04F, MathHelper.Pi * 1.7F, 0.3F))
                CameraLevels.Add(New CameraLevel("routes\route34.dat", New Vector3(10, 2, 3), New Vector3(10, 2, 40), 0.04F, 0.0F, -0.3F))
                CameraLevels.Add(New CameraLevel("route31.dat", New Vector3(29, 2, 10), New Vector3(0, 2, 10), 0.04F, MathHelper.Pi * 1.5F, -0.3F))
                CameraLevels.Add(New CameraLevel("routes\route43.dat", New Vector3(10, 2, 3), New Vector3(10, 2, 30), 0.04F, 0.0F, -0.3F))
                CameraLevels.Add(New CameraLevel("barktown.dat", New Vector3(20, 1.5, 14), New Vector3(20, 1.5, 28), 0.04F, 0.0F, -0.1F))
            Case "kanto"

        End Select
    End Sub

    Private Sub ExecuteCameraLevel()
        If Me.ExecutedCameraLevel = False Then
            Me.ExecutedCameraLevel = True
            CameraLevels(CurrentCameraLevelIndex).Apply(CType(Camera, CreditsCamera))
        End If
    End Sub

    Public Overrides Sub Draw()
        Level.Draw()

        If TheEnd = True Then
            CreditsPages(CreditsPages.Count - 1).Draw()
        Else
            CreditsPages(CurrentPageIndex).Draw()
        End If
    End Sub

    Public Overrides Sub Update()
        Camera.Update()
        Level.Update()

        CreditsPages(CurrentPageIndex).Update()

        If CreditsPages(CurrentPageIndex).IsReady = True And TheEnd = False Then
            CurrentPageIndex += 1
            If CurrentPageIndex = CreditsPages.Count - 1 Then
                TheEnd = True
            End If
        End If

        If TheEnd = True Then
            If CreditsPages(CurrentPageIndex).OnScreenTime >= 500 Then
                CreditsPages(CurrentPageIndex).AlwaysVisible = True
            End If
        End If

        If TheEnd = True Then
            If Controls.Accept(True, True) = True Then
                Core.SetScreen(New TransitionScreen(Me, SavedOverworld.OverworldScreen, Color.Black, False, AddressOf ChangeSavedScreen))
            End If
        End If

        If CType(Camera, CreditsCamera).IsReady = True And TheEnd = False Then
            Me.CurrentCameraLevelIndex += 1
            If Me.CurrentCameraLevelIndex > Me.CameraLevels.Count - 1 Then
                Me.CurrentCameraLevelIndex = 0
            End If
            Me.ExecutedCameraLevel = False
        End If

        ExecuteCameraLevel()
    End Sub

    Public Sub ChangeSavedScreen()
        Screen.Level = SavedOverworld.Level
        Screen.Camera = SavedOverworld.Camera
        Screen.Effect = SavedOverworld.Effect
        Screen.SkyDome = SavedOverworld.SkyDome
        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
    End Sub

    Class CreditsPage

        Private _title As String = "Test"
        Private _rows As New List(Of String)
        Private _color As Color = Color.White
        Private _color2 As Color = Color.Black
        Private _image As Texture2D = Nothing

        Private _onScreenTime As Integer = 0
        Private _alwaysVisible As Boolean = False

        Public Sub New(ByVal Title As String, ByVal Color1 As Color, ByVal Color2 As Color)
            Me.New(Title, Color1, Color2, New List(Of String))
        End Sub

        Public Sub New(ByVal Title As String, ByVal Color1 As Color, ByVal Color2 As Color, ByVal Rows As List(Of String))
            Me.New(Title, Color1, Color2, Rows, Nothing)
        End Sub

        Public Sub New(ByVal Title As String, ByVal Color1 As Color, ByVal Color2 As Color, ByVal Rows As List(Of String), ByVal Image As Texture2D)
            Me._title = Title
            Me._color = Color1
            Me._color2 = Color2
            Me._rows = Rows
            Me._image = Image
        End Sub

        Public Sub Draw()
            Dim alpha As Byte = GetAlphaValue()

            Dim posTitle As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.InGameFont.MeasureString(_title).X / 2), CInt(100))

            Core.SpriteBatch.DrawString(FontManager.InGameFont, Me._title, New Vector2(posTitle.X + 2, posTitle.Y + 2), AColor(Me._color2))
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Me._title, posTitle, AColor(Me._color))

            For i = 0 To _rows.Count - 1
                Dim line As String = _rows(i)

                Dim posLine As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(line).X / 2), CInt(200) + i * 35)

                Core.SpriteBatch.DrawString(FontManager.MainFont, line, New Vector2(posLine.X + 2, posLine.Y + 2), AColor(Me._color2))
                Core.SpriteBatch.DrawString(FontManager.MainFont, line, posLine, AColor(Me._color))
            Next
        End Sub

        Public Sub Update()
            Me._onScreenTime += 4
        End Sub

        Private Function GetAlphaValue() As Byte
            If Me._alwaysVisible = True Then
                Return 255
            End If
            If Me._onScreenTime < 255 Then
                Return CByte(Me._onScreenTime)
            End If
            If Me._onScreenTime > 1255 Then
                Return 0
            End If
            If Me._onScreenTime > 1000 Then
                Return CByte(255 - (Me._onScreenTime - 1000))
            End If
            Return 255
        End Function

        Private Function AColor(ByVal c As Color) As Color
            Return New Color(c.R, c.G, c.B, GetAlphaValue())
        End Function

        Public ReadOnly Property IsReady() As Boolean
            Get
                If Me._onScreenTime > 1255 Then
                    Return True
                End If
                Return False
            End Get
        End Property

        Public ReadOnly Property OnScreenTime() As Integer
            Get
                Return Me._onScreenTime
            End Get
        End Property

        Public Property AlwaysVisible As Boolean
            Get
                Return Me._alwaysVisible
            End Get
            Set(value As Boolean)
                Me._alwaysVisible = value
            End Set
        End Property

    End Class

    Class CameraLevel

        Private _levelfile As String = "testlevel.dat"
        Private _target As Vector3 = New Vector3(0)
        Private _startPosition As Vector3 = New Vector3(0)
        Private _speed As Single = 0.04F
        Private _yaw As Single = 0.0F
        Private _pitch As Single = 0.0F

        Public Sub New(ByVal LevelFile As String, ByVal Target As Vector3, ByVal StartPosition As Vector3, ByVal Speed As Single, ByVal Yaw As Single, ByVal Pitch As Single)
            Me._levelfile = LevelFile
            Me._target = Target
            Me._startPosition = StartPosition
            Me._speed = Speed
            Me._yaw = Yaw
            Me._pitch = Pitch
        End Sub

        Public Sub Apply(ByRef CreditsCamera As CreditsCamera)
            CreditsCamera.Speed = Me._speed
            CreditsCamera.Position = Me._startPosition
            CreditsCamera.Target = Me._target
            CreditsCamera.Yaw = Me._yaw
            CreditsCamera.Pitch = Me._pitch

            Screen.Level.Load(Me._levelfile)
        End Sub

    End Class

    Public Overrides Sub ChangeFrom()
        MediaPlayer.IsRepeating = True
    End Sub

    Public Overrides Sub ChangeTo()
        MediaPlayer.IsRepeating = False
    End Sub

End Class