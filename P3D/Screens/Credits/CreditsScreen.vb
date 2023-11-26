Public Class CreditsScreen

    Inherits Screen

    Private Const COPYRIGHTYEAR As String = "2023"

    Dim CreditsPages As New List(Of CreditsPage)
    Dim CurrentPageIndex As Integer = 0

    Dim CameraLevels As New List(Of CameraLevel)
    Dim CurrentCameraLevelIndex As Integer = 0
    Dim ExecutedCameraLevel As Boolean = False

    Dim TheEnd As Boolean = False
    Dim FadeAlpha As Integer = 0

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
        Me.CanMuteAudio = True
        Me.CanTakeScreenshot = True

        Screen.TextBox.Showing = False
        Screen.PokemonImageView.Showing = False
        Screen.ImageView.Showing = False
        Screen.ChooseBox.Showing = False

        Effect = New BasicEffectWithAlphaTest(Core.GraphicsDevice)
        Effect.FogEnabled = True
        SkyDome = New SkyDome()
        Camera = New CreditsCamera()

        InitializeCreditsPages(ending)
        InitializeCameraLevels(ending)
        If CameraLevels IsNot Nothing AndAlso CameraLevels.Count > 0 Then
            Level = New Level()
            ExecuteCameraLevel()
        End If

        MusicManager.Play("credits", True, False)
    End Sub

    ' The text that shows during the credits:
    ' Shouldn't be more than 23 pages total and no more than 13 people per page
    Private Sub InitializeCreditsPages(ByVal ending As String)
        CreditsPages.Add(New CreditsPage("Pokémon3D Staff", Color.White, Color.Black))
        CreditsPages.Add(New CreditsPage("Pokémon", Color.White, Color.Black, {"made by", "Nintendo", "Game Freak", "The Pokémon Company"}.ToList()))
        If GameModeManager.ActiveGameMode.IsDefaultGamemode = True Then
            CreditsPages.Add(New CreditsPage("Pokémon 3D", Color.White, Color.Black, {"Trademark (TM) 2012 - " & COPYRIGHTYEAR, "made by Kolben Games"}.ToList()))
        Else
            CreditsPages.Add(New CreditsPage("Pokémon 3D", Color.White, Color.Black, {"Trademark (TM) 2012 - " & COPYRIGHTYEAR, "made by Kolben Games", "", "GameMode made by", GameModeManager.ActiveGameMode.Author}.ToList()))
        End If
        CreditsPages.Add(New CreditsPage("Pokémon 3D Team", Color.White, Color.Black, {"Benjamin Smith", """Aragas""", """Fanta""", "Jorge Luis Espinoza", "Conner Joseph Brewster", """The Omega Ghost""", "Daniel S. Billing", "Jasper ""JappaWakka"" Speelman"}.ToList()))
        CreditsPages.Add(New CreditsPage("Director", Color.White, Color.Black, {"Benjamin Smith"}.ToList()))
        CreditsPages.Add(New CreditsPage("Initial Development", Color.White, Color.Black, {"""nilllzz""", "Jason Houston", "Daniel S. Billing", "Benjamin Smith", "Hunter Graves"}.ToList()))
        CreditsPages.Add(New CreditsPage("Programming", Color.White, Color.Black, {"Benjamin Smith", "Jorge Luis Espinoza", """Aragas""", "Jasper ""JappaWakka"" Speelman"}.ToList()))
        CreditsPages.Add(New CreditsPage("Script System Development", Color.White, Color.Black, {"Benjamin Smith", """Aragas""", "Yong Jian Ming", "Jasper ""JappaWakka"" Speelman"}.ToList()))
        CreditsPages.Add(New CreditsPage("Website Host/Server Development", Color.White, Color.Black, {"Daniel S. Billing", """Aragas""", """iErws[GR]"""}.ToList()))

        If Core.Player.IsGameJoltSave = True Then
            CreditsPages.Add(New CreditsPage("GameJolt Service/API Programming", Color.White, Color.Black, {"David DeCarmine", """nilllzz"""}.ToList()))
        End If

        If GameModeManager.ActiveGameMode.IsDefaultGamemode = False AndAlso File.Exists(GameModeManager.GetContentFilePath("Data\Credits.dat")) Then
            Dim Credits() As String = System.IO.File.ReadAllLines(GameModeManager.GetContentFilePath("Data\Credits.dat"))
            For Each Line As String In Credits
                If Line.Contains("|") = True AndAlso Line.StartsWith("#") = False AndAlso Line.StartsWith("Credits") = True Then
                    If Line.GetSplit(1, "|").ToLower = ending.ToLower OrElse Line.GetSplit(1, "|") = "" Then
                        Dim creditTitle As String = Line.GetSplit(2, "|")
                        Dim creditNames As List(Of String) = Line.GetSplit(3, "|").Split(",").ToList
                        CreditsPages.Add(New CreditsPage(creditTitle, Color.White, Color.Black, creditNames))
                    End If
                End If
            Next
        Else
            CreditsPages.Add(New CreditsPage("Graphic Design", Color.White, Color.Black, {"Benjamin Smith", """The Omega Ghost""", "Jasper ""JappaWakka"" Speelman", """Godeken""", "Caleb Coleman", "Miguel Nunez", "Grant Garrett", """Anvil555""", """princess-phoenix""", """AgentPaperCraft"""}.ToList())) '10
            CreditsPages.Add(New CreditsPage("Map Design", Color.White, Color.Black, {"Benjamin Smith", """Fanta""", "Conner Joseph Brewster"}.ToList()))
            CreditsPages.Add(New CreditsPage("ActionScript", Color.White, Color.Black, {"Benjamin Smith", """Fanta""", "Conner Joseph Brewster", "Jasper ""JappaWakka"" Speelman"}.ToList()))
            CreditsPages.Add(New CreditsPage("Community Staff", Color.White, Color.Black, {"Conner Joseph Brewster", "Benjamin Smith", "Daniel S. Billing", """Fanta""", """MamaLeef""", "Tim ten Brink"}.ToList()))
            CreditsPages.Add(New CreditsPage("0.59 QA Team", Color.White, Color.Black, {"Tim ten Brink", """iErws[GR]""", """Runaryu/agravedigger""", """AlexCorruptor""", """HantomPro""", """Lexichu_"""}.ToList())) '6
            CreditsPages.Add(New CreditsPage("Past QA Team", Color.White, Color.Black, {"Tim Drescher", "Daniel Steinborn", "Marc Boisvert-Dupras", "Matt Chambers", "William Hunn", "Torben Carrington", """Sanio""", """Vanilla"""}.ToList())) '8
            CreditsPages.Add(New CreditsPage("Legacy Contributors", Color.White, Color.Black, {"Yong Jian Ming", "Andrew Leach", "Manuel Lampe", "Robert Nobbmann", "Maximilian Schröder", "Jan Mika Eine"}.ToList()))
            CreditsPages.Add(New CreditsPage("Special Thanks", Color.White, Color.Black, {"""MunchingOrange""", """TheFlamingSpade""", """SlyFoxHound""", """ArsenioDev""", """TrUShade""", """Isaaking6""", """ParadiseGamer13"""}.ToList()))
            CreditsPages.Add(New CreditsPage("Special Thanks", Color.White, Color.Black, {"Davey Van Raaij", "Diego López", "The GameJolt Team", "The AppSharp Team", "The Smogon University Sprite Project Team"}.ToList()))
            CreditsPages.Add(New CreditsPage("", Color.White, Color.Black, {"And probably a lot more.", "Especially all the awesome people from", "the discord server and", "the pokemon3d.net community.", "Thanks for helping and playing this great game."}.ToList()))
            CreditsPages.Add(New CreditsPage("", Color.White, Color.Black))
            CreditsPages.Add(New CreditsPage("", Color.White, Color.Black))
            CreditsPages.Add(New CreditsPage("THE END", Color.White, Color.Black, {"Thank you for playing!"}.ToList()))
        End If
    End Sub

    ' Credit camera starting positions:
    Private Sub InitializeCameraLevels(ByVal ending As String)

        If GameModeManager.ActiveGameMode.IsDefaultGamemode = False AndAlso File.Exists(GameModeManager.GetContentFilePath("Data\Credits.dat")) Then
            Dim Credits() As String = System.IO.File.ReadAllLines(GameModeManager.GetContentFilePath("Data\Credits.dat"))
            For Each Line As String In Credits
                If Line.Contains("|") = True And Line.StartsWith("#") = False AndAlso Line.StartsWith("Background") = True Then
                    If Line.GetSplit(1, "|").ToLower = ending.ToLower OrElse Line.GetSplit(1, "|") = "" Then
                        Dim MapPath As String = Line.GetSplit(2, "|")

                        Dim StartPosition() As String = Line.GetSplit(3, "|").Split(",")
                        Dim StartVector As New Vector3(CSng(StartPosition(0).InsertDecSeparator), CSng(StartPosition(1).InsertDecSeparator), CSng(StartPosition(2).InsertDecSeparator))

                        Dim EndPosition() As String = Line.GetSplit(4, "|").Split(",")
                        Dim EndVector As New Vector3(CSng(EndPosition(0).InsertDecSeparator), CSng(EndPosition(1).InsertDecSeparator), CSng(EndPosition(2).InsertDecSeparator))

                        Dim CameraSpeed As Single = CSng(Line.GetSplit(5, "|").InsertDecSeparator) * 0.04F
                        Dim CameraYaw As Single = CSng(Line.GetSplit(6, "|").InsertDecSeparator)
                        Dim CameraPitch As Single = CSng(Line.GetSplit(7, "|").InsertDecSeparator)
                        CameraLevels.Add(New CameraLevel(MapPath, EndVector, StartVector, CameraSpeed, CameraYaw, CameraPitch))
                    End If
                End If
            Next
        Else
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
        End If

    End Sub

    Private Sub ExecuteCameraLevel()
        If CameraLevels IsNot Nothing AndAlso CameraLevels.Count > 0 Then
            If Me.ExecutedCameraLevel = False Then
                Me.ExecutedCameraLevel = True
                CameraLevels(CurrentCameraLevelIndex).Apply(CType(Camera, CreditsCamera))
            End If
        End If
    End Sub

    Public Overrides Sub Draw()
        If CameraLevels IsNot Nothing AndAlso CameraLevels.Count > 0 Then
            Level.Draw()
        Else
            Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width, Core.windowSize.Height), New Color(0, 0, 0))
        End If

        If TheEnd = True Then
            If FadeAlpha < 255 Then
                FadeAlpha += 5
                If FadeAlpha >= 255 Then
                    FadeAlpha = 255
                End If
            End If

            Dim ContinueString As String = Localization.GetString("credits_continue", "Press [Enter] to continue.")
            Core.SpriteBatch.DrawString(FontManager.InGameFont, ContinueString, New Vector2(CInt(windowSize.Width / 2 - FontManager.InGameFont.MeasureString(ContinueString).X / 2 + 2), CInt(windowSize.Height - 160 + 2)), New Color(Color.Black, FadeAlpha))
            Core.SpriteBatch.DrawString(FontManager.InGameFont, ContinueString, New Vector2(CInt(windowSize.Width / 2 - FontManager.InGameFont.MeasureString(ContinueString).X / 2), CInt(windowSize.Height - 160)), New Color(Color.White, FadeAlpha))

            CreditsPages(CreditsPages.Count - 1).Draw()
        Else
            CreditsPages(CurrentPageIndex).Draw()
        End If
    End Sub

    Public Overrides Sub Update()
        If CameraLevels IsNot Nothing AndAlso CameraLevels.Count > 0 Then
            Camera.Update()
            Level.Update()
        End If

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
                SoundManager.PlaySound("select")
                Core.SetScreen(New TransitionScreen(Me, SavedOverworld.OverworldScreen, Color.Black, False, AddressOf ChangeSavedScreen))
            End If
        End If

        If CameraLevels IsNot Nothing AndAlso CameraLevels.Count > 0 Then
            If CType(Camera, CreditsCamera).IsReady = True And TheEnd = False Then
                Me.CurrentCameraLevelIndex += 1
                If Me.CurrentCameraLevelIndex > Me.CameraLevels.Count - 1 Then
                    Me.CurrentCameraLevelIndex = 0
                End If
                Me.ExecutedCameraLevel = False
            End If

            ExecuteCameraLevel()
        End If
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

            Dim Title As String = _title
            If Localization.TokenExists("credits_title_" & _title) Then
                Title = Localization.GetString("credits_title_" & _title, _title)
            End If
            Dim posTitle As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.InGameFont.MeasureString(Title).X / 2), CInt(100))

            Core.SpriteBatch.DrawString(FontManager.InGameFont, Title, New Vector2(posTitle.X + 2, posTitle.Y + 2), AColor(Me._color2))
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Title, posTitle, AColor(Me._color))

            For i = 0 To _rows.Count - 1
                Dim line As String = _rows(i)
                If Localization.TokenExists("credits_line_" & _rows(i)) Then
                    line = Localization.GetString("credits_line_" & _rows(i), _rows(i))
                End If
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


End Class