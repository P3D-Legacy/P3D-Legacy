Public Class NewTrainerScreen

    Inherits Screen

    Private _backTexture As Texture2D
    Private _paperClipTexture As Texture2D
    Private _charTexture As Texture2D
    Private _papersTexture As Texture2D

    Private _isIntro As Boolean = True
    Private _introY As Single = -100.0F
    Private _rotation As Single = 0F
    Private _badgeRegionIndex As Integer = 0
    Private _badgeIndex As Integer = 0

    Private _spriteBatch As SpriteBatch
    Private _textBatch As SpriteBatch
    Private _charBatch As SpriteBatch
    Private _cardBatch As SpriteBatch

    Private _badgeAnimation As BadgeAnimation = New BadgeAnimation()

    Private target As RenderTarget2D
    Private target2 As RenderTarget2D

    Private Class BadgeAnimation
        Public _shakeV As Single
        Public _shakeLeft As Boolean
        Public _shakeCount As Integer
    End Class

    Public Sub New(ByVal currentScreen As Screen)
        Me.Identification = Identifications.TrainerScreen
        Me.PreScreen = currentScreen

        ''Requires file restructure
        '_backTexture = Content.Load(Of Texture2D)("SharedResources\Textures\UI\TrainerCard\Back")
        '_paperClipTexture = Content.Load(Of Texture2D)("SharedResources\Textures\UI\TrainerCard\Paperclip")
        '_papersTexture = Content.Load(Of Texture2D)("SharedResources\Textures\UI\TrainerCard\Papers")

        _backTexture = TextureManager.LoadDirect("Textures\UI\TrainerCard\Back.png")
        _paperClipTexture = TextureManager.LoadDirect("Textures\UI\TrainerCard\Paperclip.png")
        _papersTexture = TextureManager.LoadDirect("Textures\UI\TrainerCard\Papers.png")
        target = New RenderTarget2D(GraphicsDevice, _backTexture.Width, _backTexture.Height + _paperClipTexture.Height, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
        target2 = New RenderTarget2D(GraphicsDevice, Core.windowSize.Width, Core.windowSize.Height, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)

        If Screen.Level.Surfing = True Then
            _charTexture = TextureManager.GetTexture("Textures\NPC\" & Core.Player.TempSurfSkin)
        Else
            If Screen.Level.Riding = True Then
                _charTexture = TextureManager.GetTexture("Textures\NPC\" & Core.Player.TempRideSkin)
            Else
                _charTexture = Screen.Level.OwnPlayer.Texture
            End If
        End If

        Dim frameSize As Size = New Size(CInt(_charTexture.Width / 3), CInt(_charTexture.Height / 4))
        _charTexture = TextureManager.GetTexture(_charTexture, New Rectangle(0, frameSize.Width * 2, frameSize.Width, frameSize.Height))

        _spriteBatch = New SpriteBatch(GraphicsDevice)
        _textBatch = New SpriteBatch(GraphicsDevice)
        _charBatch = New SpriteBatch(GraphicsDevice)
        _cardBatch = New SpriteBatch(GraphicsDevice)

        Dim hasKanto As Boolean = True
        For i = 1 To 8
            If Core.Player.Badges.Contains(i) = False Then
                hasKanto = False
            End If
        Next
        Dim hasJohto As Boolean = True
        For i = 9 To 16
            If Core.Player.Badges.Contains(i) = False Then
                hasJohto = False
            End If
        Next

        If hasKanto = True Then
            GameJolt.Emblem.AchieveEmblem("kanto")
        End If
        If hasJohto = True Then
            GameJolt.Emblem.AchieveEmblem("johto")
        End If
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        'Dim target As New RenderTarget2D(GraphicsDevice, _backTexture.Width, _backTexture.Height + _paperClipTexture.Height)
        'Dim target2 As New RenderTarget2D(GraphicsDevice, Core.windowSize.Width, Core.windowSize.Height)

        GraphicsDevice.SetRenderTarget(target)
        GraphicsDevice.Clear(Color.Transparent)

        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise)
        _textBatch.Begin()
        _charBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise)

        _spriteBatch.Draw(_backTexture, New Rectangle(0, _paperClipTexture.Height, _backTexture.Width, _backTexture.Height), Color.White)
        _textBatch.Draw(_papersTexture, New Vector2(48, 8 + _paperClipTexture.Height), Color.White)
        _charBatch.Draw(_paperClipTexture, New Rectangle(85, 50, 47, 88), Color.White)

        _charBatch.Draw(_charTexture, New Rectangle(70, 36 + _paperClipTexture.Height, 128, 128), Color.White)

        If Core.Player.IsGameJoltSave Then
            _spriteBatch.Draw(GameJolt.Emblem.GetEmblemBackgroundTexture(GameJoltSave.Emblem), New Rectangle(-10, 290, 256, 64), Nothing, Color.White, 0F, Vector2.Zero, SpriteEffects.None, 0F)
            Canvas.DrawRectangle(_textBatch, New Rectangle(-10, 300, 190, 30), New Color(0, 0, 0, 150))

            Dim emblemName = GameJoltSave.Emblem
            _textBatch.DrawString(FontManager.MiniFont, emblemName(0).ToString().ToUpper() & emblemName.Substring(1, emblemName.Length - 1), New Vector2(0, 305), Color.White)

            DrawLevelProgress()
        End If

        _textBatch.DrawString(FontManager.MainFont, "Trainer Card", New Vector2(260, 100), Color.Black, 0F, Vector2.Zero, 1.5F, SpriteEffects.None, 0F)

        _textBatch.DrawString(FontManager.MainFont, "Name: ", New Vector2(270, 160), Color.Black, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)
        _textBatch.DrawString(FontManager.MainFont, "Money: ", New Vector2(270, 190), Color.Black, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)
        _textBatch.DrawString(FontManager.MainFont, "OT: ", New Vector2(270, 220), Color.Black, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)
        _textBatch.DrawString(FontManager.MainFont, "Time: ", New Vector2(270, 250), Color.Black, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)
        _textBatch.DrawString(FontManager.MainFont, "Points: ", New Vector2(270, 280), Color.Black, 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)

        _textBatch.DrawString(FontManager.MiniFont, Core.Player.Name, New Vector2(390, 165), New Color(80, 80, 80), 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)

        _textBatch.DrawString(FontManager.TextFont, "$", New Vector2(390, 195), New Color(80, 80, 80), 0F, Vector2.Zero, 1.1F, SpriteEffects.None, 0F)
        _textBatch.DrawString(FontManager.MiniFont, Core.Player.Money.ToString(), New Vector2(400, 195), New Color(80, 80, 80), 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)

        _textBatch.DrawString(FontManager.MiniFont, Core.Player.OT, New Vector2(390, 225), New Color(80, 80, 80), 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)

        _textBatch.DrawString(FontManager.MiniFont, TimeHelpers.GetDisplayTime(TimeHelpers.GetCurrentPlayTime(), True), New Vector2(390, 255), New Color(80, 80, 80), 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)

        Dim points = Core.Player.Points
        If Core.Player.IsGameJoltSave Then
            points = GameJoltSave.Points
        End If

        _textBatch.DrawString(FontManager.MiniFont, points.ToString(), New Vector2(390, 285), New Color(80, 80, 80), 0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0F)

        DrawBadges()

        _spriteBatch.End()
        _textBatch.End()
        _charBatch.End()

        GraphicsDevice.SetRenderTarget(target2)
        GraphicsDevice.Clear(Color.Transparent)

        _cardBatch.Begin()

        _cardBatch.Draw(target, New Rectangle(CInt(Core.ScreenSize.Width / 2 - target.Width / 2) + 42, CInt(60 + _introY), target.Width, target.Height), Nothing, Color.White, _rotation, Vector2.Zero, SpriteEffects.None, 0F)

        _cardBatch.End()

        GraphicsDevice.SetRenderTarget(Nothing)

        SpriteBatch.Draw(target2, New Vector2(0, 0), Color.White)
    End Sub

    Private Sub DrawBadges()
        _textBatch.DrawString(FontManager.MiniFont, Localization.GetString("trainer_screen_collected_badges") & ": " & Core.Player.Badges.Count, New Vector2(50, 360), Color.Black)

        Dim selectedRegion As String = Badge.GetRegion(_badgeRegionIndex)
        Dim badgesCount As Integer = Badge.GetBadgesCount(selectedRegion)
        Dim badgeName As String = ""

        For i = 0 To badgesCount - 1
            Dim badgeID As Integer = Badge.GetBadgeID(selectedRegion, i)

            Dim c As Color = Color.White
            Dim t As String = Badge.GetBadgeName(badgeID) & Localization.GetString("trainer_screen_badge")
            Dim shake As Single = 0F
            If Badge.PlayerHasBadge(badgeID) = False Then
                c = Color.Black
                t = Localization.GetString("trainer_screen_empty_badge")
            End If

            If i = CInt(_badgeIndex) Then
                badgeName = t
                shake = _badgeAnimation._shakeV
            End If
            _spriteBatch.Draw(Badge.GetBadgeTexture(badgeID), New Rectangle(16 + (i + 1) * 64, 412, 50, 50), Nothing, c, shake, New Vector2(25, 25), SpriteEffects.None, 0F)
        Next

        _textBatch.DrawString(FontManager.MiniFont, badgeName, New Vector2(555 - FontManager.MiniFont.MeasureString(badgeName).X.ToInteger(), 360), Color.Black)
    End Sub

    Private Sub DrawLevelProgress()
        Dim currentLevel As Integer = GameJolt.Emblem.GetPlayerLevel(Core.GameJoltSave.Points)

        Dim needPoints As Integer = 0
        Dim totalNeedPoints As Integer = 0

        Dim hasPoints As Integer = Core.GameJoltSave.Points
        Dim needPointsCurrentLevel As Integer = GameJolt.Emblem.GetPointsForLevel(currentLevel)
        Dim needPointsNextLevel As Integer = GameJolt.Emblem.GetPointsForLevel(currentLevel + 1)

        totalNeedPoints = needPointsNextLevel - needPointsCurrentLevel
        needPoints = totalNeedPoints - (hasPoints - needPointsCurrentLevel) + 1

        Dim hasPointsThisLevel As Integer = totalNeedPoints - needPoints

        Dim nextSprite As Texture2D = GameJolt.Emblem.GetPlayerSprite(currentLevel + 1, Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Gender)

        _spriteBatch.Draw(nextSprite, New Rectangle(570, 310, 32, 32), New Rectangle(0, 64, 32, 32), Color.White)

        Dim value = hasPointsThisLevel / totalNeedPoints * 310

        If currentLevel >= 100 Then
            value = 310
            needPoints = 0
        End If
        Canvas.DrawRectangle(_spriteBatch, New Rectangle(260, 312, 310, 32), New Color(0, 0, 0, 180))
        Canvas.DrawRectangle(_spriteBatch, New Rectangle(260, 316, CInt(value), 24), New Color(255, 165, 0))
        Canvas.DrawRectangle(_spriteBatch, New Rectangle(260, 316, CInt(value), 8), New Color(255, 203, 108))

        Dim nxtLvl As Integer = currentLevel + 1
        If currentLevel = 100 Then
            nxtLvl = 100
        End If
        Dim rankStr = "Rank: " & nxtLvl

        _textBatch.DrawString(FontManager.MiniFont, rankStr, New Vector2(600 - FontManager.MiniFont.MeasureString(rankStr).X.ToInteger(), 290), Color.Black)

        If needPoints = 1 Then
            _textBatch.DrawString(FontManager.MiniFont, "Need " & needPoints & " point", New Vector2(280, 318), Color.Black)
        Else
            _textBatch.DrawString(FontManager.MiniFont, "Need " & needPoints & " points", New Vector2(280, 318), Color.Black)
        End If

        'If totalNeedPoints > 0 Then
        '    Canvas.DrawScrollBar(New Vector2(140, 380), totalNeedPoints, hasPointsThisLevel, 0, New Size(320, 16), True, Color.Black, New Color(255, 165, 0))
        '    Canvas.DrawScrollBar(New Vector2(140, 380), totalNeedPoints, hasPointsThisLevel, 0, New Size(320, 6), True, Color.Black, New Color(255, 203, 108))
        'Else
        '    Canvas.DrawRectangle(New Rectangle(140, 380, 320, 16), Color.Black)
        'End If

        'Core.SpriteBatch.DrawString(FontManager.MiniFont, "Rank: " & currentLevel, New Vector2(100, 400), Color.Black)
        'Core.SpriteBatch.DrawString(FontManager.MiniFont, "Rank: " & currentLevel + 1, New Vector2(430, 400), Color.Black)

    End Sub

    Public Overrides Sub Update()
        If _isIntro Then
            If _rotation < 0.12F Then
                _rotation = MathHelper.Lerp(_rotation, 0.12F, 0.1F)
                _introY = MathHelper.Lerp(_introY, 0, 0.1F)
                If _rotation + 0.01F >= 0.12F Then
                    _isIntro = False
                End If
            End If
        Else
            If Controls.Dismiss() Then
                SetScreen(PreScreen)
            End If
        End If

        If Controls.Right(True, False, False, True, True, False) = True Then
            _badgeIndex += 1
        End If
        If Controls.Left(True, False, False, True, True, False) = True Then
            _badgeIndex -= 1
        End If
        If Controls.Up(True, False, False, True, True, False) = True Then
            _badgeRegionIndex -= 1
        End If
        If Controls.Down(True, False, False, True, True, False) = True Then
            _badgeRegionIndex += 1
        End If

        _badgeRegionIndex = _badgeRegionIndex.Clamp(0, Badge.GetRegionCount() - 1)
        _badgeIndex = _badgeIndex.Clamp(0, Badge.GetBadgesCount(Badge.GetRegion(_badgeRegionIndex)) - 1)

        If Core.Player.IsGameJoltSave = True Then
            Dim EmblemName As String = Core.GameJoltSave.Emblem
            Dim newIndex As Integer = Core.GameJoltSave.AchievedEmblems.IndexOf(EmblemName)
            If Controls.Up(True, True, False, False, False, True) OrElse Controls.Left(True, True, False, False, False, True) Or ControllerHandler.ButtonPressed(Buttons.LeftShoulder) Then
                newIndex -= 1
            End If
            If Controls.Down(True, True, False, False, False, True) OrElse Controls.Right(True, True, False, False, False, True) Or ControllerHandler.ButtonPressed(Buttons.RightShoulder) Then
                newIndex += 1
            End If
            If newIndex >= 0 And newIndex < Core.GameJoltSave.AchievedEmblems.Count Then
                Core.GameJoltSave.Emblem = Core.GameJoltSave.AchievedEmblems(newIndex)
            End If
        End If

        If _badgeAnimation._shakeLeft Then
            _badgeAnimation._shakeV -= 0.035F
            If _badgeAnimation._shakeV <= -0.4F Then
                _badgeAnimation._shakeCount -= 1
                _badgeAnimation._shakeLeft = False
            End If
        Else
            _badgeAnimation._shakeV += 0.035F
            If _badgeAnimation._shakeV >= 0.4F Then
                _badgeAnimation._shakeCount -= 1
                _badgeAnimation._shakeLeft = True
            End If
        End If
    End Sub

End Class