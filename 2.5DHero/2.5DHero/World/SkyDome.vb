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

        TextureUp = TextureManager.GetTexture("SkyDomeResource\Clouds")
        TextureDown = TextureManager.GetTexture("SkyDomeResource\Clouds")
        TextureSun = TextureManager.GetTexture("SkyDomeResource\sun")
        TextureMoon = TextureManager.GetTexture("SkyDomeResource\moon")

        SetLastColor()
    End Sub

    Public Sub Update()
        Yaw += 0.0002F
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

    Private Function GetUniversePitch() As Single
        If FASTTIMECYCLE = True Then
            Dim progress As Integer = Hour * 3600 + Minute * 60 + Second
            Return CSng((MathHelper.TwoPi / 100) * (progress / 86400 * 100))
        Else
            Dim progress As Integer = World.SecondsOfDay
            Return CSng((MathHelper.TwoPi / 100) * (progress / 86400 * 100))
        End If
    End Function

    Public Sub Draw(ByVal FOV As Single)
        If Core.GameOptions.GraphicStyle = 1 Then
            If Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Outside Then
                If World.GetWeatherFromWeatherType(Screen.Level.WeatherType) <> World.Weathers.Fog Then 'Don't render the sky if weather is set to Fog
                    RenderHalf(FOV, MathHelper.PiOver2, CSng(GetUniversePitch() + Math.PI), True, TextureSun, 100, Me.GetSunAlpha()) 'Draw sun
                    RenderHalf(FOV, MathHelper.PiOver2, CSng(GetUniversePitch()), True, TextureMoon, 100, GetStarsAlpha()) 'Draw moon
                    RenderHalf(FOV, MathHelper.PiOver2, CSng(GetUniversePitch()), True, TextureDown, 50, GetStarsAlpha()) 'Draw stars half 1
                    RenderHalf(FOV, MathHelper.PiOver2, CSng(GetUniversePitch()), False, TextureDown, 50, GetStarsAlpha()) 'Draw stars half 2
                    RenderHalf(FOV, MathHelper.TwoPi - Yaw, 0.0F, True, GetCloudsTexture(), 15, GetCloudAlpha()) 'Draw clouds back layer
                    RenderHalf(FOV, Yaw, 0.0F, True, TextureUp, 10, GetCloudAlpha()) 'Draw clouds front layer
                End If
            Else
                RenderHalf(FOV, Yaw, 0.0F, True, TextureUp, 8.0F, 1.0F)

                If Not TextureDown Is Nothing Then
                    RenderHalf(FOV, Yaw, 0.0F, False, TextureDown, 8.0F, 1.0F)
                End If
            End If
        End If
    End Sub

    Private Sub RenderHalf(ByVal FOV As Single, ByVal useYaw As Single, ByVal usePitch As Single, ByVal up As Boolean, ByVal texture As Texture2D, ByVal scale As Single, ByVal alpha As Single)
        Dim Roll As Single = 0.0F
        If up = False Then
            Roll = Math.PI
        End If

        Core.GraphicsDevice.BlendState = BlendState.AlphaBlend

        For Each ModelMesh As ModelMesh In SkydomeModel.Meshes
            For Each BasicEffect As BasicEffect In ModelMesh.Effects
                BasicEffect.World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(New Vector3(Screen.Camera.Position.X, -5, Screen.Camera.Position.Z)) * Matrix.CreateFromYawPitchRoll(useYaw, usePitch, Roll)

                BasicEffect.View = Screen.Camera.View
                BasicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), Core.GraphicsDevice.Viewport.AspectRatio, 0.01, 10000)

                BasicEffect.TextureEnabled = True
                BasicEffect.Texture = texture
                BasicEffect.Alpha = alpha

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

                If BasicEffect.DiffuseColor <> New Vector3(1) Then
                    BasicEffect.DiffuseColor = GetWeatherColorMultiplier(BasicEffect.DiffuseColor)
                End If
            Next

            ModelMesh.Draw()
        Next
    End Sub

    Shared DaycycleTextureData() As Color = Nothing
    Shared DaycycleTexture As Texture2D = Nothing
    Shared LastSkyColor As Color = New Color(0, 0, 0, 0)
    Shared LastEntityColor As Color = New Color(0, 0, 0, 0)

    Public Shared Function GetDaytimeColor(ByVal shader As Boolean) As Color
        If shader = True Then
            Return LastEntityColor
        Else
            Return LastSkyColor
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
        Select Case Screen.Level.World.CurrentMapWeather
            Case World.Weathers.Rain, World.Weathers.Blizzard, World.Weathers.Thunderstorm
                Return 0.6F
            Case World.Weathers.Snow, World.Weathers.Ash
                Return 0.4F
            Case World.Weathers.Clear
                Return 0.1F
        End Select
        Return 0.0F
    End Function

    Private Function GetStarsAlpha() As Single
        Dim progress As Integer = GetTimeValue()

        If progress < 360 Or progress > 1080 Then
            Dim dP As Integer = progress
            If dP < 360 Then
                dP = 720 - dP * 2
            ElseIf dP > 1080 Then
                dP = 720 - (1440 - dP) * 2
            End If

            Dim alpha As Single = CDec(dP / 720) * 0.7F
            Return alpha
        Else
            Return 0.0F
        End If
    End Function

    Private Function GetSunAlpha() As Single
        Dim progress As Integer = GetTimeValue()

        If progress >= 1080 And progress < 1140 Then
            'Between 6PM and 7PM, the sun fades away in 60 stages:
            Dim i As Single = progress - 1080
            Dim percent As Single = i / 60 * 100

            Return 1.0F - percent / 100.0F
        ElseIf progress >= 300 And progress < 360 Then
            'Between 5AM and 6AM, the sun fades in in 60 stages:
            Dim i As Single = progress - 300
            Dim percent As Single = i / 60 * 100

            Return percent / 100.0F
        ElseIf progress >= 1140 Or progress < 300 Then
            'Between 7PM and 5AM, the sun is invisible:
            Return 0.0F
        Else
            'Between 6AM and 6PM, the sun is fully visible:
            Return 1.0F
        End If
    End Function

    Private Function GetCloudsTexture() As Texture2D
        Select Case Screen.Level.World.CurrentMapWeather
            Case World.Weathers.Rain, World.Weathers.Blizzard, World.Weathers.Thunderstorm, World.Weathers.Snow
                Return TextureManager.GetTexture("SkyDomeResource\CloudsWeather")
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
            Return World.MinutesOfDay
        End If
    End Function

End Class