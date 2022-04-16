Public Class SkyDome

    Private SkydomeModel As Model
    Public TextureUp As Texture2D
    Public TextureDown As Texture2D
    Dim TextureSun As Texture2D
    Dim TextureMoon As Texture2D

    Public Yaw As Single = 0

    Const FASTTIMECYCLE As Boolean = False

    Public Sub New()
        SkydomeModel = Core.Content.Load(Of Model)("SkyDomeResource\SkyDome")

        TextureUp = TextureManager.GetTexture("SkyDomeResource\Sky_Day")
        TextureDown = TextureManager.GetTexture("SkyDomeResource\Stars")
        TextureSun = TextureManager.GetTexture("SkyDomeResource\sun")
        TextureMoon = TextureManager.GetTexture("SkyDomeResource\moon")

        SetLastColor()
    End Sub

    Public Sub Update()
        Yaw += 0.0001F
        While Yaw > MathHelper.TwoPi
            Yaw -= MathHelper.TwoPi
        End While
        SetLastColor()

        If FASTTIMECYCLE = True Then
            Second += 60
            If Second = 60 Then
                Second = 0
                Minute += 1
                If Minute = 60 Then
                    Minute = 0
                    Hour += 1
                    If Hour = 24 Then
                        Hour = 0
                    End If
                End If
            End If
        End If
    End Sub

    Dim Hour As Integer = 0
    Dim Minute As Integer = 0
    Dim Second As Integer = 0

    Public Sub Draw(ByVal FOV As Single)
        If Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Outside Then
            If World.GetWeatherFromWeatherType(Screen.Level.WeatherType) <> World.Weathers.Fog Then ' Don't render the sky if the weather is set to Fog.
                RenderHalf(FOV, Yaw, 0.0F, True, GetSkyTexture(), 20, 1.0F) ' Draw the sky
                RenderHalf(FOV, MathHelper.TwoPi, 0.0F, True, TextureDown, 18, GetStarsAlpha()) ' Draw the stars.
                If GetSunAlpha() > 0 Then
                    RenderHalf(FOV, MathHelper.TwoPi, 0.0F, True, TextureSun, 16, 1.0F) ' Draw the Sun.
                Else
                    RenderHalf(FOV, MathHelper.TwoPi, 0.0F, True, TextureMoon, 16, 1.0F) ' Draw the Moon.
                End If
                RenderHalf(FOV, MathHelper.TwoPi - Yaw * 2, 0.0F, True, GetCloudsTexture(), 12, GetCloudAlpha) ' Draw the clouds.
            End If
        Else
            If Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Cave Or Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Forest Then
                RenderHalf(FOV, MathHelper.TwoPi, 0.0F, True, TextureUp, 20, 1.0F) ' Draw the sky
            Else
                RenderHalf(FOV, Yaw, 0.0F, True, TextureUp, 16, 1.0F) ' Draw the sky
                RenderHalf(FOV, MathHelper.TwoPi, 0.0F, True, TextureSun, 12, GetSunAlpha()) ' Draw the Sun.
                RenderHalf(FOV, MathHelper.TwoPi - Yaw, 0.0F, True, TextureManager.GetTexture("SkyDomeResource\Clouds_Day"), 8, GetCloudAlpha()) ' Draw the clouds.
            End If
            If Not TextureDown Is Nothing Then
                RenderHalf(FOV, Yaw, 0.0F, False, TextureDown, 16, 1.0F)
            End If
        End If
    End Sub

    Private Sub RenderHalf(ByVal FOV As Single, ByVal useYaw As Single, ByVal usePitch As Single, ByVal up As Boolean, ByVal texture As Texture2D, ByVal scale As Single, ByVal alpha As Single)
        Dim Roll As Single = 0.0F
        If up = False Then
            Roll = Math.PI
        End If

        Dim previousBlendState = Core.GraphicsDevice.BlendState
        Core.GraphicsDevice.BlendState = BlendState.NonPremultiplied

        For Each ModelMesh As ModelMesh In SkydomeModel.Meshes
            For Each BasicEffect As BasicEffect In ModelMesh.Effects
                BasicEffect.World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(New Vector3(Screen.Camera.Position.X, Screen.Camera.Position.Y - 2, Screen.Camera.Position.Z)) * Matrix.CreateFromYawPitchRoll(useYaw, usePitch, Roll)

                BasicEffect.View = Screen.Camera.View
                BasicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), Core.GraphicsDevice.Viewport.AspectRatio, 0.01, 10000)

                BasicEffect.TextureEnabled = True
                BasicEffect.Texture = texture
                BasicEffect.Alpha = alpha

                If BasicEffect.Texture Is TextureDown Then
                    BasicEffect.DiffuseColor = New Vector3(1)
                Else
                    Select Case Screen.Level.World.CurrentMapWeather
                        Case World.Weathers.Clear, World.Weathers.Sunny
                            BasicEffect.DiffuseColor = New Vector3(1)
                        Case World.Weathers.Rain
                            BasicEffect.DiffuseColor = New Vector3(0.4, 0.4, 0.7)
                        Case World.Weathers.Snow
                            BasicEffect.DiffuseColor = New Vector3(0.8)
                        Case World.Weathers.Underwater
                            BasicEffect.DiffuseColor = New Vector3(0.1, 0.3, 0.9)
                        Case World.Weathers.Fog
                            BasicEffect.DiffuseColor = New Vector3(0.7, 0.7, 0.8)
                        Case World.Weathers.Sandstorm
                            BasicEffect.DiffuseColor = New Vector3(0.8, 0.5, 0.2)
                        Case World.Weathers.Ash
                            BasicEffect.DiffuseColor = New Vector3(0.5, 0.5, 0.5)
                        Case World.Weathers.Blizzard
                            BasicEffect.DiffuseColor = New Vector3(0.6, 0.6, 0.6)
                    End Select
                End If

                If BasicEffect.DiffuseColor <> New Vector3(1) Then
                    BasicEffect.DiffuseColor = GetWeatherColorMultiplier(BasicEffect.DiffuseColor)
                End If
            Next

            ModelMesh.Draw()
        Next

        Core.GraphicsDevice.BlendState = previousBlendState

    End Sub

    Shared DaycycleTextureData() As Color = Nothing
    Shared DaycycleTexture As Texture2D = Nothing
    Shared LastSkyColor As Color = New Color(0, 0, 0, 0)
    Shared LastEntityColor As Color = New Color(0, 0, 0, 0)

    Public Shared Function GetDaytimeColor(ByVal shader As Boolean) As Color
        If shader = True Then
            Return LastEntityColor
        Else
            If World.IsAurora = True Then
                Return New Color(64, 101, 164)
            End If
            Select Case Screen.Level.DayTime
                Case 1
                    Return New Color(48, 200, 248)
                Case 2
                    Return New Color(40, 88, 136)
                Case 3
                    Return New Color(168, 224, 248)
                Case 4
                    Return New Color(192, 152, 184)
            End Select
        End If
    End Function

    Private Sub SetLastColor()
        If DaycycleTextureData Is Nothing Then
            Dim DaycycleTexture As Texture2D = TextureManager.GetTexture("SkyDomeResource\daycycle")
            DaycycleTextureData = New Color(DaycycleTexture.Width * DaycycleTexture.Height - 1) {}
            DaycycleTexture.GetData(DaycycleTextureData)
            SkyDome.DaycycleTexture = DaycycleTexture
        End If

        Dim pixel As Integer = GetTimeValue()

        Dim pixelColor As Color = DaycycleTextureData(pixel)
        If pixelColor <> LastSkyColor Then
            LastSkyColor = pixelColor
            LastEntityColor = DaycycleTextureData((pixel + DaycycleTexture.Width).Clamp(0, DaycycleTexture.Width * DaycycleTexture.Height - 1))
        End If
    End Sub

    Private Function GetCloudAlpha() As Single
        If Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Outside And World.IsAurora = False Then
            Return 1.0F
        Else
            Return 0.0F
        End If
    End Function

    Private Function GetStarsAlpha() As Single
        If Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Outside And World.IsAurora = False Then
            Select Case Screen.Level.DayTime
                Case 1
                    Return 0.0F
                Case 2
                    Return 1.0F
                Case 3
                    Return 0.0F
                Case 4
                    Return 0.0F
                Case Else
                    Return 0.0F
            End Select
        Else
            Return 0.0F
        End If
    End Function

    Private Function GetSunAlpha() As Single
        If Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Outside And World.IsAurora = False Then
            Select Case Screen.Level.DayTime
                Case 1
                    Return 1.0F
                Case 2
                    Return 0.0F
                Case 3
                    Return 1.0F
                Case 4
                    Return 0.0F
                Case Else
                    Return 0.0F
            End Select
        Else
            Return 0.0F
        End If
    End Function

    Private Function GetCloudsTexture() As Texture2D
        Dim time As World.DayTimes = World.GetTime

        Select Case Screen.Level.World.CurrentMapWeather
            Case World.Weathers.Rain, World.Weathers.Blizzard, World.Weathers.Thunderstorm, World.Weathers.Snow
                Return TextureManager.GetTexture("SkyDomeResource\Clouds_Weather")
            Case World.Weathers.Clear
                Select Case Screen.Level.DayTime
                    Case 1
                        Return TextureManager.GetTexture("SkyDomeResource\Clouds_Day")
                    Case 2
                        Return TextureManager.GetTexture("SkyDomeResource\Clouds_Night")
                    Case 3
                        Return TextureManager.GetTexture("SkyDomeResource\Clouds_Morning")
                    Case 4
                        Return TextureManager.GetTexture("SkyDomeResource\Clouds_Evening")
                End Select
                If time = World.DayTimes.Morning Then
                    Return TextureManager.GetTexture("SkyDomeResource\Clouds_Morning")
                ElseIf time = World.DayTimes.Day Then
                    Return TextureManager.GetTexture("SkyDomeResource\Clouds_Day")
                ElseIf time = World.DayTimes.Evening Then
                    Return TextureManager.GetTexture("SkyDomeResource\Clouds_Evening")
                Else
                    Return TextureManager.GetTexture("SkyDomeResource\Clouds_Night")
                End If
        End Select
        Return Nothing
    End Function
    Private Function GetSkyTexture() As Texture2D
        If World.IsAurora Then
            Return TextureManager.GetTexture("SkyDomeResource\AuroraBorealis")
        End If
        Select Case Screen.Level.DayTime
            Case 1
                Return TextureManager.GetTexture("SkyDomeResource\Sky_Day")
            Case 2
                Return TextureManager.GetTexture("SkyDomeResource\Sky_Night")
            Case 3
                Return TextureManager.GetTexture("SkyDomeResource\Sky_Morning")
            Case 4
                Return TextureManager.GetTexture("SkyDomeResource\Sky_Evening")
        End Select
        Dim time As World.DayTimes = World.GetTime
        Select Case Screen.Level.World.CurrentMapWeather
            Case World.Weathers.Clear
                If time = World.DayTimes.Morning Then
                    Return TextureManager.GetTexture("SkyDomeResource\Sky_Morning")
                ElseIf time = World.DayTimes.Day Then
                    Return TextureManager.GetTexture("SkyDomeResource\Sky_Day")
                ElseIf time = World.DayTimes.Evening Then
                    Return TextureManager.GetTexture("SkyDomeResource\Sky_Evening")
                Else
                    Return TextureManager.GetTexture("SkyDomeResource\Sky_Night")
                End If
        End Select
        Return TextureUp
    End Function

    Public Function GetWeatherColorMultiplier(ByVal v As Vector3) As Vector3
        Dim progress As Integer = GetTimeValue()

        Dim p As Single = 0.0F

        If progress < 720 Then
            p = CSng((720 - progress) / 720)
        Else
            p = CSng((progress - 720) / 720)
        End If

        Return New Vector3(v.X + ((1 - v.X) * p),
                           v.Y + ((1 - v.Y) * p),
                           v.Z + ((1 - v.Z) * p))
    End Function

    Private Function GetTimeValue() As Integer
        If FASTTIMECYCLE = True Then
            Return Hour * 60 + Minute
        Else
            If World.IsMainMenu Then
                Return 720
            End If
            Return World.MinutesOfDay
        End If
    End Function

End Class