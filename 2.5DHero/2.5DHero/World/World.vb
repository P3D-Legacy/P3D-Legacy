Public Class World

    Private Shared _regionWeather As Weathers = Weathers.Clear
    Private Shared _regionWeatherSet As Boolean = False
    Public Shared IsAurora As Boolean = False

    Public Enum Seasons As Integer
        Winter = 0
        Spring = 1
        Summer = 2
        Fall = 3
    End Enum

    Public Enum Weathers As Integer
        Clear = 0
        Rain = 1
        Snow = 2
        Underwater = 3
        Sunny = 4
        Fog = 5
        Thunderstorm = 6
        Sandstorm = 7
        Ash = 8
        Blizzard = 9
    End Enum

    Public Enum EnvironmentTypes As Integer
        Outside = 0
        Inside = 1
        Cave = 2
        Dark = 3
        Underwater = 4
        Forest = 5
    End Enum

    Public Enum DayTime As Integer
        Night = 0
        Morning = 1
        Day = 2
        Evening = 3
    End Enum

    Public Shared ReadOnly Property WeekOfYear() As Integer
        Get
            Return CInt(((My.Computer.Clock.LocalTime.DayOfYear - (My.Computer.Clock.LocalTime.DayOfWeek - 1)) / 7) + 1)
        End Get
    End Property

    Public Shared ReadOnly Property CurrentSeason() As Seasons
        Get
            If NeedServerObject() = True Then
                Return ServerSeason
            End If
            Select Case WeekOfYear Mod 4
                Case 1
                    Return Seasons.Winter
                Case 2
                    Return Seasons.Spring
                Case 3
                    Return Seasons.Summer
                Case 0
                    Return Seasons.Fall
            End Select
            Return Seasons.Summer
        End Get
    End Property

    Public Shared ReadOnly Property GetTime() As DayTime
        Get
            Dim time As DayTime = DayTime.Day

            Dim Hour As Integer = My.Computer.Clock.LocalTime.Hour
            If NeedServerObject() = True Then
                Dim data() As String = ServerTimeData.Split(CChar(","))
                Hour = CInt(data(0))
            End If

            Select Case CurrentSeason
                Case Seasons.Winter
                    If Hour > 18 Or Hour < 7 Then
                        time = DayTime.Night
                    ElseIf Hour > 6 And Hour < 11 Then
                        time = DayTime.Morning
                    ElseIf Hour > 10 And Hour < 17 Then
                        time = DayTime.Day
                    ElseIf Hour > 16 And Hour < 19 Then
                        time = DayTime.Evening
                    End If
                Case Seasons.Spring
                    If Hour > 19 Or Hour < 5 Then
                        time = DayTime.Night
                    ElseIf Hour > 4 And Hour < 10 Then
                        time = DayTime.Morning
                    ElseIf Hour > 9 And Hour < 17 Then
                        time = DayTime.Day
                    ElseIf Hour > 16 And Hour < 20 Then
                        time = DayTime.Evening
                    End If
                Case Seasons.Summer
                    If Hour > 20 Or Hour < 4 Then
                        time = DayTime.Night
                    ElseIf Hour > 3 And Hour < 9 Then
                        time = DayTime.Morning
                    ElseIf Hour > 8 And Hour < 19 Then
                        time = DayTime.Day
                    ElseIf Hour > 18 And Hour < 21 Then
                        time = DayTime.Evening
                    End If
                Case Seasons.Fall
                    If Hour > 19 Or Hour < 6 Then
                        time = DayTime.Night
                    ElseIf Hour > 5 And Hour < 10 Then
                        time = DayTime.Morning
                    ElseIf Hour > 9 And Hour < 18 Then
                        time = DayTime.Day
                    ElseIf Hour > 17 And Hour < 20 Then
                        time = DayTime.Evening
                    End If
            End Select

            Return time
        End Get
    End Property

    Public Shared Sub SetRenderDistance(ByVal EnvironmentType As EnvironmentTypes, ByVal Weather As Weathers)
        If Weather = Weathers.Fog Then
            Screen.Effect.FogStart = -40
            Screen.Effect.FogEnd = 12

            Screen.Camera.FarPlane = 15
            GoTo endsub
        End If

        If Weather = Weathers.Blizzard Then
            Screen.Effect.FogStart = -40
            Screen.Effect.FogEnd = 20

            Screen.Camera.FarPlane = 24
            GoTo endsub
        End If

        If Weather = Weathers.Thunderstorm Then
            Screen.Effect.FogStart = -40
            Screen.Effect.FogEnd = 20

            Screen.Camera.FarPlane = 24
            GoTo endsub
        End If

        Select Case EnvironmentType
            Case EnvironmentTypes.Cave, EnvironmentTypes.Dark, EnvironmentTypes.Forest
                Select Case Core.GameOptions.RenderDistance
                    Case 0
                        Screen.Effect.FogStart = -2
                        Screen.Effect.FogEnd = 19

                        Screen.Camera.FarPlane = 20
                    Case 1
                        Screen.Effect.FogStart = -2
                        Screen.Effect.FogEnd = 39

                        Screen.Camera.FarPlane = 40
                    Case 2
                        Screen.Effect.FogStart = -2
                        Screen.Effect.FogEnd = 59

                        Screen.Camera.FarPlane = 60
                    Case 3
                        Screen.Effect.FogStart = -5
                        Screen.Effect.FogEnd = 79

                        Screen.Camera.FarPlane = 80
                    Case 4
                        Screen.Effect.FogStart = -20
                        Screen.Effect.FogEnd = 99

                        Screen.Camera.FarPlane = 100
                End Select
            Case EnvironmentTypes.Inside
                Select Case Core.GameOptions.RenderDistance
                    Case 0
                        Screen.Effect.FogStart = 16
                        Screen.Effect.FogEnd = 19

                        Screen.Camera.FarPlane = 20
                    Case 1
                        Screen.Effect.FogStart = 36
                        Screen.Effect.FogEnd = 39

                        Screen.Camera.FarPlane = 40
                    Case 2
                        Screen.Effect.FogStart = 56
                        Screen.Effect.FogEnd = 59

                        Screen.Camera.FarPlane = 60
                    Case 3
                        Screen.Effect.FogStart = 76
                        Screen.Effect.FogEnd = 79

                        Screen.Camera.FarPlane = 80
                    Case 4
                        Screen.Effect.FogStart = 96
                        Screen.Effect.FogEnd = 99

                        Screen.Camera.FarPlane = 100
                End Select
            Case EnvironmentTypes.Outside
                Select Case World.GetTime
                    Case DayTime.Night
                        Select Case Core.GameOptions.RenderDistance
                            Case 0
                                Screen.Effect.FogStart = -2
                                Screen.Effect.FogEnd = 19

                                Screen.Camera.FarPlane = 20
                            Case 1
                                Screen.Effect.FogStart = -2
                                Screen.Effect.FogEnd = 39

                                Screen.Camera.FarPlane = 40
                            Case 2
                                Screen.Effect.FogStart = -2
                                Screen.Effect.FogEnd = 59

                                Screen.Camera.FarPlane = 60
                            Case 3
                                Screen.Effect.FogStart = -5
                                Screen.Effect.FogEnd = 79

                                Screen.Camera.FarPlane = 80
                            Case 4
                                Screen.Effect.FogStart = -20
                                Screen.Effect.FogEnd = 99

                                Screen.Camera.FarPlane = 100
                        End Select
                    Case DayTime.Morning
                        Select Case Core.GameOptions.RenderDistance
                            Case 0
                                Screen.Effect.FogStart = 16
                                Screen.Effect.FogEnd = 19

                                Screen.Camera.FarPlane = 20
                            Case 1
                                Screen.Effect.FogStart = 36
                                Screen.Effect.FogEnd = 39

                                Screen.Camera.FarPlane = 40
                            Case 2
                                Screen.Effect.FogStart = 56
                                Screen.Effect.FogEnd = 59

                                Screen.Camera.FarPlane = 60
                            Case 3
                                Screen.Effect.FogStart = 76
                                Screen.Effect.FogEnd = 79

                                Screen.Camera.FarPlane = 80
                            Case 4
                                Screen.Effect.FogStart = 96
                                Screen.Effect.FogEnd = 99

                                Screen.Camera.FarPlane = 100
                        End Select
                    Case DayTime.Day
                        Select Case Core.GameOptions.RenderDistance
                            Case 0
                                Screen.Effect.FogStart = 16
                                Screen.Effect.FogEnd = 19

                                Screen.Camera.FarPlane = 20
                            Case 1
                                Screen.Effect.FogStart = 36
                                Screen.Effect.FogEnd = 39

                                Screen.Camera.FarPlane = 40
                            Case 2
                                Screen.Effect.FogStart = 56
                                Screen.Effect.FogEnd = 59

                                Screen.Camera.FarPlane = 60
                            Case 3
                                Screen.Effect.FogStart = 76
                                Screen.Effect.FogEnd = 79

                                Screen.Camera.FarPlane = 80
                            Case 4
                                Screen.Effect.FogStart = 96
                                Screen.Effect.FogEnd = 99

                                Screen.Camera.FarPlane = 100
                        End Select
                    Case DayTime.Evening
                        Select Case Core.GameOptions.RenderDistance
                            Case 0
                                Screen.Effect.FogStart = 0
                                Screen.Effect.FogEnd = 19

                                Screen.Camera.FarPlane = 20
                            Case 1
                                Screen.Effect.FogStart = 0
                                Screen.Effect.FogEnd = 39

                                Screen.Camera.FarPlane = 40
                            Case 2
                                Screen.Effect.FogStart = 0
                                Screen.Effect.FogEnd = 59

                                Screen.Camera.FarPlane = 60
                            Case 3
                                Screen.Effect.FogStart = 0
                                Screen.Effect.FogEnd = 79

                                Screen.Camera.FarPlane = 80
                            Case 4
                                Screen.Effect.FogStart = 0
                                Screen.Effect.FogEnd = 99

                                Screen.Camera.FarPlane = 100
                        End Select
                End Select
            Case EnvironmentTypes.Underwater
                Select Case Core.GameOptions.RenderDistance
                    Case 0
                        Screen.Effect.FogStart = 0
                        Screen.Effect.FogEnd = 19

                        Screen.Camera.FarPlane = 20
                    Case 1
                        Screen.Effect.FogStart = 0
                        Screen.Effect.FogEnd = 39

                        Screen.Camera.FarPlane = 40
                    Case 2
                        Screen.Effect.FogStart = 0
                        Screen.Effect.FogEnd = 59

                        Screen.Camera.FarPlane = 60
                    Case 3
                        Screen.Effect.FogStart = 0
                        Screen.Effect.FogEnd = 79

                        Screen.Camera.FarPlane = 80
                    Case 4
                        Screen.Effect.FogStart = 0
                        Screen.Effect.FogEnd = 99

                        Screen.Camera.FarPlane = 100
                End Select
        End Select

        If Core.GameOptions.RenderDistance >= 5 Then
            Screen.Effect.FogStart = 999
            Screen.Effect.FogEnd = 1000

            Screen.Camera.FarPlane = 1000
        End If

endsub:
        Screen.Camera.CreateNewProjection(Screen.Camera.FOV)
    End Sub

    Private Shared Function GetRegionWeather(ByVal Season As Seasons) As Weathers
        Dim r As Integer = Core.Random.Next(0, 100)

        Select Case Season
            Case Seasons.Winter
                If r < 20 Then
                    Return Weathers.Rain
                ElseIf r >= 20 And r < 50 Then
                    Return Weathers.Clear
                Else
                    Return Weathers.Snow
                End If
            Case Seasons.Spring
                If r < 5 Then
                    Return Weathers.Snow
                ElseIf r >= 5 And r < 40 Then
                    Return Weathers.Rain
                Else
                    Return Weathers.Clear
                End If
            Case Seasons.Summer
                If r < 10 Then
                    Return Weathers.Rain
                Else
                    Return Weathers.Clear
                End If
            Case Seasons.Fall
                If r < 5 Then
                    Return Weathers.Snow
                ElseIf r >= 5 And r < 80 Then
                    Return Weathers.Rain
                Else
                    Return Weathers.Clear
                End If
        End Select

        Return Weathers.Clear
    End Function

    Public CurrentMapWeather As Weathers = Weathers.Clear

    Public EnvironmentType As EnvironmentTypes = EnvironmentTypes.Outside
    Public UseLightning As Boolean = False

    Public Sub New(ByVal EnvironmentType As Integer, ByVal WeatherType As Integer)
        Initialize(EnvironmentType, WeatherType)
    End Sub

    Public Shared Function GetWeatherFromWeatherType(ByVal WeatherType As Integer) As Weathers
        Select Case WeatherType
            Case 0 'RegionWeather
                Return World.GetCurrentRegionWeather()
            Case 1 'Clear
                Return Weathers.Clear
            Case 2 'Rain
                Return Weathers.Rain
            Case 3 'Snow
                Return Weathers.Snow
            Case 4 'Underwater
                Return Weathers.Underwater
            Case 5 'Sunny
                Return Weathers.Sunny
            Case 6 'Fog
                Return Weathers.Fog
            Case 7 'Sandstorm
                Return Weathers.Sandstorm
            Case 8
                Return Weathers.Ash
            Case 9
                Return Weathers.Blizzard
            Case 10
                Return Weathers.Thunderstorm
        End Select
        Return Weathers.Clear
    End Function

    Public Shared Function GetWeatherTypeFromWeather(ByVal Weather As Weathers) As Integer
        Select Case Weather
            Case Weathers.Clear
                Return 1
            Case Weathers.Rain
                Return 2
            Case Weathers.Snow
                Return 3
            Case Weathers.Underwater
                Return 4
            Case Weathers.Sunny
                Return 5
            Case Weathers.Fog
                Return 6
            Case Weathers.Sandstorm
                Return 7
            Case Weathers.Ash
                Return 8
            Case Weathers.Blizzard
                Return 9
            Case Weathers.Thunderstorm
                Return 10
            Case Else
                Return 0
        End Select
    End Function

    Public Sub Initialize(ByVal EnvironmentType As Integer, ByVal WeatherType As Integer)
        If _regionWeatherSet = False Then
            World._regionWeather = World.GetRegionWeather(World.CurrentSeason)
            World._regionWeatherSet = True
        End If

        Me.CurrentMapWeather = GetWeatherFromWeatherType(WeatherType)

        Select Case EnvironmentType
            Case 0 'Overworld
                Me.EnvironmentType = EnvironmentTypes.Outside
                Me.UseLightning = True
            Case 1 'Day always
                Me.EnvironmentType = EnvironmentTypes.Inside
                Me.UseLightning = False
            Case 2 'Cave
                Me.EnvironmentType = EnvironmentTypes.Cave
                If WeatherType = 0 Then
                    Me.CurrentMapWeather = Weathers.Clear
                End If
                Me.UseLightning = False
            Case 3 'Night always
                Me.EnvironmentType = EnvironmentTypes.Dark
                If WeatherType = 0 Then
                    Me.CurrentMapWeather = Weathers.Clear
                End If
                Me.UseLightning = False
            Case 4 'Underwater
                Me.EnvironmentType = EnvironmentTypes.Underwater
                If WeatherType = 0 Then
                    Me.CurrentMapWeather = Weathers.Underwater
                End If
                Me.UseLightning = True
            Case 5 'Forest
                Me.EnvironmentType = EnvironmentTypes.Forest
                Me.UseLightning = True
        End Select

        SetWeatherLevelColor()
        ChangeEnvironment()
        SetRenderDistance(Me.EnvironmentType, Me.CurrentMapWeather)
    End Sub

    Private Sub SetWeatherLevelColor()
        Select Case CurrentMapWeather
            Case Weathers.Clear
                Screen.Effect.DiffuseColor = New Vector3(1)
            Case Weathers.Rain, Weathers.Thunderstorm
                Screen.Effect.DiffuseColor = New Vector3(0.4, 0.4, 0.7)
            Case Weathers.Snow
                Screen.Effect.DiffuseColor = New Vector3(0.8)
            Case Weathers.Underwater
                Screen.Effect.DiffuseColor = New Vector3(0.1, 0.3, 0.9)
            Case Weathers.Sunny
                Screen.Effect.DiffuseColor = New Vector3(1.6, 1.3, 1.3)
            Case Weathers.Fog
                Screen.Effect.DiffuseColor = New Vector3(0.5, 0.5, 0.6)
            Case Weathers.Sandstorm
                Screen.Effect.DiffuseColor = New Vector3(0.8, 0.5, 0.2)
            Case Weathers.Ash
                Screen.Effect.DiffuseColor = New Vector3(0.5, 0.5, 0.5)
            Case Weathers.Blizzard
                Screen.Effect.DiffuseColor = New Vector3(0.6, 0.6, 0.6)
        End Select

        Screen.Effect.DiffuseColor = Screen.SkyDome.GetWeatherColorMultiplier(Screen.Effect.DiffuseColor)
    End Sub

    Private Function GetWeatherBackgroundColor(ByVal defaultColor As Color) As Color
        Dim v As Vector3 = Vector3.One

        Select Case CurrentMapWeather
            Case World.Weathers.Clear, Weathers.Sunny
                v = New Vector3(1)
            Case World.Weathers.Rain, Weathers.Thunderstorm
                v = New Vector3(0.4, 0.4, 0.7)
            Case World.Weathers.Snow
                v = New Vector3(0.8)
            Case World.Weathers.Underwater
                v = New Vector3(0.1, 0.3, 0.9)
            Case World.Weathers.Fog
                v = New Vector3(0.7, 0.7, 0.8)
            Case World.Weathers.Sandstorm
                v = New Vector3(0.8, 0.5, 0.2)
            Case Weathers.Ash
                v = New Vector3(0.5, 0.5, 0.5)
            Case Weathers.Blizzard
                v = New Vector3(0.6, 0.6, 0.6)
        End Select

        Dim colorV As Vector3 = defaultColor.ToVector3 * Screen.SkyDome.GetWeatherColorMultiplier(v)
        Return colorV.ToColor()
    End Function

    Private Sub ChangeEnvironment()
        Select Case Me.EnvironmentType
            Case EnvironmentTypes.Outside
                Core.BackgroundColor = GetWeatherBackgroundColor(SkyDome.GetDaytimeColor(False))
                Screen.Effect.FogColor = Core.BackgroundColor.ToVector3()
                If IsAurora = True Then
                    Screen.SkyDome.TextureUp = TextureManager.GetTexture("SkyDomeResource\AuroraBoralis")
                Else
                    Screen.SkyDome.TextureUp = TextureManager.GetTexture("SkyDomeResource\Clouds1")
                End If
                Screen.SkyDome.TextureDown = TextureManager.GetTexture("SkyDomeResource\Stars")
            Case EnvironmentTypes.Inside
                Core.BackgroundColor = GetWeatherBackgroundColor(New Color(173, 216, 255))
                Screen.Effect.FogColor = Core.BackgroundColor.ToVector3()
                Screen.SkyDome.TextureUp = TextureManager.GetTexture("SkyDomeResource\Clouds")
                Screen.SkyDome.TextureDown = Nothing
            Case EnvironmentTypes.Dark
                Core.BackgroundColor = GetWeatherBackgroundColor(New Color(29, 29, 50))
                Screen.Effect.FogColor = Core.BackgroundColor.ToVector3()
                Screen.SkyDome.TextureUp = TextureManager.GetTexture("SkyDomeResource\Dark")
                Screen.SkyDome.TextureDown = Nothing
            Case EnvironmentTypes.Cave
                Core.BackgroundColor = GetWeatherBackgroundColor(New Color(34, 19, 12))
                Screen.Effect.FogColor = Core.BackgroundColor.ToVector3()
                Screen.SkyDome.TextureUp = TextureManager.GetTexture("SkyDomeResource\Cave")
                Screen.SkyDome.TextureDown = Nothing
            Case EnvironmentTypes.Underwater
                Core.BackgroundColor = GetWeatherBackgroundColor(New Color(19, 54, 117))
                Screen.Effect.FogColor = Core.BackgroundColor.ToVector3()
                Screen.SkyDome.TextureUp = TextureManager.GetTexture("SkyDomeResource\Underwater")
                Screen.SkyDome.TextureDown = TextureManager.GetTexture("SkyDomeResource\UnderwaterGround")
            Case EnvironmentTypes.Forest
                Core.BackgroundColor = GetWeatherBackgroundColor(New Color(30, 66, 21))
                Screen.Effect.FogColor = Core.BackgroundColor.ToVector3()
                Screen.SkyDome.TextureUp = TextureManager.GetTexture("SkyDomeResource\Forest")
                Screen.SkyDome.TextureDown = Nothing
        End Select
    End Sub

    Private Shared WeatherOffset As New Vector2(0, 0)
    Private Shared ObjectsList As New List(Of Rectangle)

    Public Shared NoParticlesList() As Weathers = {Weathers.Clear, Weathers.Sunny, Weathers.Fog}

    Public Shared Sub DrawWeather(ByVal MapWeather As Weathers)
        If NoParticlesList.Contains(MapWeather) = False Then
            If Core.GameOptions.GraphicStyle = 1 Then
                Dim identifications() As Screen.Identifications = {Screen.Identifications.OverworldScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleScreen, Screen.Identifications.BattleCatchScreen}
                If identifications.Contains(Core.CurrentScreen.Identification) = True Then
                    If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                        If Screen.TextBox.Showing = False Then
                            GenerateParticles(0, MapWeather)
                        End If
                    Else
                        GenerateParticles(0, MapWeather)
                    End If
                End If
            Else
                Dim T As Texture2D = Nothing

                Dim size As Integer = 128
                Dim opacity As Integer = 30

                Select Case MapWeather
                    Case Weathers.Rain
                        T = TextureManager.GetTexture("Textures\Weather\rain")

                        WeatherOffset.X += 8
                        WeatherOffset.Y += 16
                    Case Weathers.Thunderstorm
                        T = TextureManager.GetTexture("Textures\Weather\rain")

                        WeatherOffset.X += 12
                        WeatherOffset.Y += 20

                        opacity = 50
                    Case Weathers.Snow
                        T = TextureManager.GetTexture("Textures\Weather\snow")

                        WeatherOffset.X += 1
                        WeatherOffset.Y += 1
                    Case Weathers.Blizzard
                        T = TextureManager.GetTexture("Textures\Weather\snow")

                        WeatherOffset.X += 8
                        WeatherOffset.Y += 2

                        opacity = 80
                    Case Weathers.Sandstorm
                        T = TextureManager.GetTexture("Textures\Weather\sand")

                        WeatherOffset.X += 4
                        WeatherOffset.Y += 1

                        opacity = 80
                        size = 48
                    Case Weathers.Underwater
                        T = TextureManager.GetTexture("Textures\Weather\bubble")

                        If Core.Random.Next(0, 100) = 0 Then
                            ObjectsList.Add(New Rectangle(Core.Random.Next(0, Core.windowSize.Width - 32), Core.windowSize.Height, 32, 32))
                        End If

                        For i = 0 To ObjectsList.Count - 1
                            Dim r As Rectangle = ObjectsList(i)
                            ObjectsList(i) = New Rectangle(r.X, r.Y - 2, r.Width, r.Height)

                            Core.SpriteBatch.Draw(T, ObjectsList(i), New Color(255, 255, 255, 150))
                        Next
                    Case Weathers.Ash
                        T = TextureManager.GetTexture("Textures\Weather\ash2")

                        WeatherOffset.Y += 1
                        opacity = 65
                        size = 48
                End Select

                If WeatherOffset.X >= size Then
                    WeatherOffset.X = 0
                End If
                If WeatherOffset.Y >= size Then
                    WeatherOffset.Y = 0
                End If

                Select Case MapWeather
                    Case Weathers.Rain, Weathers.Snow, Weathers.Sandstorm, Weathers.Ash, Weathers.Blizzard, Weathers.Thunderstorm
                        For x = -size To Core.windowSize.Width Step size
                            For y = -size To Core.windowSize.Height Step size
                                Core.SpriteBatch.Draw(T, New Rectangle(CInt(x + WeatherOffset.X), CInt(y + WeatherOffset.Y), size, size), New Color(255, 255, 255, opacity))
                            Next
                        Next
                End Select
            End If
        End If
    End Sub

    Public Shared Sub GenerateParticles(ByVal chance As Integer, ByVal MapWeather As Weathers)
        If MapWeather = Weathers.Thunderstorm Then
            If Core.Random.Next(0, 250) = 0 Then
                Dim pitch As Single = -(Core.Random.Next(8, 11) / 10.0F)
                Debug.Print(pitch.ToString())
                SoundManager.PlaySound("Battle\Effects\effect_thunderbolt", pitch, 0F, SoundManager.Volume, False)
            End If
        End If

        If LevelLoader.IsBusy = False Then
            Dim validScreen() As Screen.Identifications = {Screen.Identifications.OverworldScreen, Screen.Identifications.BattleScreen, Screen.Identifications.BattleCatchScreen, Screen.Identifications.MainMenuScreen}
            If validScreen.Contains(Core.CurrentScreen.Identification) = True Then
                If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                    If CType(Core.CurrentScreen, OverworldScreen).ActionScript.IsReady = False Then
                        Exit Sub
                    End If
                End If

                Dim T As Texture2D = Nothing

                Dim speed As Single
                Dim scale As New Vector3(1)
                Dim range As Integer = 3

                Select Case MapWeather
                    Case Weathers.Rain
                        speed = 0.1F
                        T = TextureManager.GetTexture("Textures\Weather\rain3")
                        If chance > -1 Then chance = 3
                        scale = New Vector3(0.03F, 0.06F, 0.1F)
                    Case Weathers.Thunderstorm
                        speed = 0.15F
                        Select Case Core.Random.Next(0, 4)
                            Case 0
                                T = TextureManager.GetTexture("Textures\Weather\rain2")
                                scale = New Vector3(0.1F, 0.1F, 0.1F)
                            Case Else
                                T = TextureManager.GetTexture("Textures\Weather\rain3")
                                scale = New Vector3(0.03F, 0.06F, 0.1F)
                        End Select
                        If chance > -1 Then chance = 1
                    Case Weathers.Snow
                        speed = 0.02F
                        T = TextureManager.GetTexture("Textures\Weather\snow2")
                        If chance > -1 Then chance = 5
                        scale = New Vector3(0.03F, 0.03F, 0.1F)
                    Case Weathers.Underwater
                        speed = -0.02F
                        T = TextureManager.GetTexture("Textures\Weather\bubble")
                        If chance > -1 Then chance = 60
                        scale = New Vector3(0.5F)
                        range = 1
                    Case Weathers.Sandstorm
                        speed = 0.1F
                        T = TextureManager.GetTexture("Textures\Weather\sand")
                        If chance > -1 Then chance = 4
                        scale = New Vector3(0.03F, 0.03F, 0.1F)
                    Case Weathers.Ash
                        speed = 0.02F
                        T = TextureManager.GetTexture("Textures\Weather\ash")
                        If chance > -1 Then chance = 20
                        scale = New Vector3(0.03F, 0.03F, 0.1F)
                    Case Weathers.Blizzard
                        speed = 0.1F
                        T = TextureManager.GetTexture("Textures\Weather\snow")
                        If chance > -1 Then chance = 1
                        scale = New Vector3(0.12F, 0.12F, 0.1F)
                End Select

                If chance = -1 Then
                    chance = 1
                End If

                Dim cameraPosition As Vector3 = Screen.Camera.Position
                If Screen.Camera.Name = "Overworld" Then
                    cameraPosition = CType(Screen.Camera, OverworldCamera).CPosition
                End If

                If Core.Random.Next(0, chance) = 0 Then
                    For x = cameraPosition.X - range To cameraPosition.X + range
                        For z = cameraPosition.Z - range To cameraPosition.Z + range
                            If z <> 0 Or x <> 0 Then
                                Dim rY As Single = CSng(Core.Random.Next(0, 40) / 10) - 2.0F
                                Dim rX As Single = CSng(Core.Random.NextDouble()) - 0.5F
                                Dim rZ As Single = CSng(Core.Random.NextDouble()) - 0.5F
                                Dim p As Particle = New Particle(New Vector3(x + rX, cameraPosition.Y + 1.8F + rY, z + rZ), {T}, {0, 0}, Core.Random.Next(0, 2), scale, BaseModel.BillModel, New Vector3(1))
                                p.MoveSpeed = speed
                                If MapWeather = Weathers.Rain Then
                                    p.Opacity = 0.7F
                                End If
                                If MapWeather = Weathers.Thunderstorm Then
                                    p.Opacity = 1.0F
                                End If
                                If MapWeather = Weathers.Underwater Then
                                    p.Position.Y = 0.0F
                                    p.Destination = 10
                                    p.Behavior = Particle.Behaviors.Rising
                                End If
                                If MapWeather = Weathers.Sandstorm Then
                                    p.Behavior = Particle.Behaviors.LeftToRight
                                    p.Destination = cameraPosition.X + 5
                                    p.Position.X -= 2
                                End If
                                If MapWeather = Weathers.Blizzard Then
                                    p.Opacity = 1.0F
                                End If
                                Screen.Level.Entities.Add(p)
                            End If
                        Next
                    Next
                End If
            End If
        End If
    End Sub

    Private Shared SeasonTextureBuffer As New Dictionary(Of Texture2D, Texture2D)
    Private Shared BufferSeason As Seasons = Seasons.Fall

    Public Shared Function GetSeasonTexture(ByVal seasonTexture As Texture2D, ByVal T As Texture2D) As Texture2D
        If BufferSeason <> CurrentSeason Then
            BufferSeason = CurrentSeason
            SeasonTextureBuffer.Clear()
        End If

        If IsNothing(T) = False Then
            If SeasonTextureBuffer.ContainsKey(T) = True Then
                Return SeasonTextureBuffer(T)
            End If

            Dim x As Integer = 0
            Dim y As Integer = 0
            Select Case CurrentSeason
                Case Seasons.Winter
                    x = 0
                    y = 0
                Case Seasons.Spring
                    x = 2
                    y = 0
                Case Seasons.Summer
                    x = 0
                    y = 2
                Case Seasons.Fall
                    x = 2
                    y = 2
            End Select

            Dim inputColors() As Color = {New Color(0, 0, 0), New Color(85, 85, 85), New Color(170, 170, 170), New Color(255, 255, 255)}.Reverse().ToArray()
            Dim outputColors As New List(Of Color)

            Dim Data(3) As Color
            seasonTexture.GetData(0, New Rectangle(x, y, 2, 2), Data, 0, 4)

            SeasonTextureBuffer.Add(T, T.ReplaceColors(inputColors, Data))
            Return SeasonTextureBuffer(T)
        End If
        Return Nothing
    End Function

    Public Shared ServerSeason As Seasons = Seasons.Spring
    Public Shared ServerWeather As Weathers = Weathers.Clear
    Public Shared ServerTimeData As String = "0,0,0" 'Format: Hour,Minute,Second
    Public Shared LastServerDataReceived As Date = Date.Now

    Public Shared ReadOnly Property SecondsOfDay() As Integer
        Get
            If NeedServerObject() = True Then
                Dim data() As String = ServerTimeData.Split(CChar(","))
                Dim hours As Integer = CInt(data(0))
                Dim minutes As Integer = CInt(data(1))
                Dim seconds As Integer = CInt(data(2))

                seconds += CInt(Math.Abs(DateDiff(DateInterval.Second, LastServerDataReceived, Date.Now)))

                Return hours * 3600 + minutes * 60 + seconds
            Else
                Return My.Computer.Clock.LocalTime.Hour * 3600 + My.Computer.Clock.LocalTime.Minute * 60 + My.Computer.Clock.LocalTime.Second
            End If
        End Get
    End Property

    Public Shared ReadOnly Property MinutesOfDay() As Integer
        Get
            If NeedServerObject() = True Then
                Dim data() As String = ServerTimeData.Split(CChar(","))
                Dim hours As Integer = CInt(data(0))
                Dim minutes As Integer = CInt(data(1))

                minutes += CInt(Math.Abs(DateDiff(DateInterval.Minute, LastServerDataReceived, Date.Now)))

                Return hours * 60 + minutes
            Else
                Return My.Computer.Clock.LocalTime.Hour * 60 + My.Computer.Clock.LocalTime.Minute
            End If
        End Get
    End Property

    Private Shared Function NeedServerObject() As Boolean
        Return JoinServerScreen.Online = True And ConnectScreen.Connected = True
    End Function

    ''' <summary>
    ''' Returns the region weather and gets the server weather if needed.
    ''' </summary>
    Public Shared Function GetCurrentRegionWeather() As Weathers
        If NeedServerObject() = True Then
            Return ServerWeather
        Else
            Return _regionWeather
        End If
    End Function

    Public Shared Property RegionWeather() As Weathers
        Get
            Return _regionWeather
        End Get
        Set(value As Weathers)
            _regionWeather = value
        End Set
    End Property

    Public Shared Property RegionWeatherSet() As Boolean
        Get
            Return _regionWeatherSet
        End Get
        Set(value As Boolean)
            _regionWeatherSet = value
        End Set
    End Property

End Class
