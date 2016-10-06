Public Class BattleIntroScreen

    Inherits Screen

    Dim oldX, oldY As Integer

    Dim AnimationType As Integer
    Dim Animations As New List(Of Rectangle)

    Public OldScreen As Screen
    Public NewScreen As Screen

    Dim ready As Boolean = False
    Dim value As Integer = 0

    Dim Trainer As Trainer

    Dim minDelay As Single = 16.0F
    Dim startTime As Date
    Dim duration As TimeSpan

    Public MusicLoop As String = ""

    Public Sub New(ByVal OldScreen As Screen, ByVal NewScreen As Screen, ByVal IntroType As Integer)
        Dim musicLoop As String = Screen.Level.CurrentRegion.Split(CChar(","))(0) & "_wild_intro"

        If BattleSystem.BattleScreen.RoamingBattle = True Then
            If BattleSystem.BattleScreen.RoamingPokemonStorage.MusicLoop <> "" Then
                musicLoop = BattleSystem.BattleScreen.RoamingPokemonStorage.MusicLoop & "_intro"
            End If
        End If

        If MusicManager.SongExists(musicLoop) = False Then
            musicLoop = "johto_wild_intro"
        End If
        musicLoop = musicLoop

        Me.Constructor(OldScreen, NewScreen, Nothing, musicLoop, IntroType)
    End Sub

    Public Sub New(ByVal OldScreen As Screen, ByVal NewScreen As Screen, ByVal IntroType As Integer, ByVal MusicLoop As String)
        If MusicLoop = "" Then
            MusicLoop = Screen.Level.CurrentRegion.Split(CChar(","))(0) & "_wild_intro"
            If MusicManager.SongExists(MusicLoop) = False Then
                If BattleSystem.BattleScreen.RoamingBattle = True Then
                    If BattleSystem.BattleScreen.RoamingPokemonStorage.MusicLoop <> "" Then
                        MusicLoop = BattleSystem.BattleScreen.RoamingPokemonStorage.MusicLoop & "_intro"
                    End If
                End If
                If MusicManager.SongExists(MusicLoop) = False Then
                    MusicLoop = "johto_wild_intro"
                End If
            End If
            MusicLoop = MusicLoop
        Else
            MusicLoop = MusicLoop
        End If

        Me.Constructor(OldScreen, NewScreen, Nothing, MusicLoop, IntroType)
    End Sub

    Public Sub New(ByVal OldScreen As Screen, ByVal NewScreen As Screen, ByVal Trainer As Trainer, ByVal MusicLoop As String, ByVal IntroType As Integer)
        Me.Constructor(OldScreen, NewScreen, Trainer, MusicLoop, IntroType)
    End Sub

    Private Sub Constructor(ByVal OldScreen As Screen, ByVal NewScreen As Screen, ByVal Trainer As Trainer, ByVal MusicLoop As String, ByVal IntroType As Integer)
        Me.OldScreen = OldScreen
        Me.NewScreen = NewScreen
        Me.CanChat = False
        Me.CanBePaused = False
        Me.Trainer = Trainer
        Me.MusicLoop = MusicLoop

        Me.AnimationType = IntroType
        If Screen.Level.IsDark = True And Me.AnimationType > 4 Then
            Me.AnimationType -= 5
        End If

        Me.Identification = Identifications.BattleIniScreen
    End Sub

    Public Overrides Sub Draw()
        OldScreen.Draw()

        Dim c As Color = Color.Black
        If AnimationType > 4 Then
            c = Color.White
        End If

        Select Case AnimationType
            Case 0, 1, 2, 3, 5, 6, 7, 8
                For Each Animation As Rectangle In Animations
                    Canvas.DrawRectangle(Animation, c)
                Next
            Case 4, 9
                For Each Animation As Rectangle In Animations
                    Canvas.DrawBorder(value, Animation, c)
                Next
            Case 10
                DrawTrainerIntro()
            Case 11
                DrawFaceshotIntro()
            Case 12
                DrawBlurIntro()
        End Select
    End Sub

    Dim blurTexture As Texture2D = Nothing
    Dim blurLayers As New List(Of Rectangle)
    Dim whiteLayers As New List(Of Rectangle)
    Dim blurDelay As Single = 0.5F
    Dim currentBlurPosition As New Vector2(0.0F)
    Dim currentBlurIntensity As Single = 10.0F
    Dim currentBlurZoom As Integer = 0

    Private Sub DrawBlurIntro()
        If Not blurTexture Is Nothing Then
            Dim startIndex As Integer = 0
            If blurLayers.Count > 10 Then
                startIndex = blurLayers.Count - 10
            End If

            For i = startIndex To blurLayers.Count - 1
                Dim usedZoom As Integer = blurLayers(i).Width - Core.windowSize.Width

                Dim rot As Single = CSng(Math.Sin(usedZoom * 0.01F) * 0.1F)

                Dim r As Rectangle = blurLayers(i)
                Dim origin As New Vector2(Core.windowSize.Width / 2.0F, Core.windowSize.Height / 2.0F)

                Core.SpriteBatch.Draw(blurTexture, New Rectangle(r.X + CInt(r.Width / 2), r.Y + CInt(r.Height / 2), r.Width, r.Height), Nothing, New Color(255, 255, 255, 100), rot, origin, SpriteEffects.None, 0.0F)
            Next

            For i = 0 To Me.whiteLayers.Count - 1
                Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width, Core.windowSize.Height), New Color(255, 255, 255, 50))
            Next
        End If
    End Sub

    Private Sub UpdateBlurIntro()
        If blurTexture Is Nothing Then
            Dim r As New RenderTarget2D(Core.GraphicsDevice, Core.windowSize.Width, Core.windowSize.Height)
            Core.GraphicsDevice.SetRenderTarget(r)

            Core.Draw()

            Core.GraphicsDevice.SetRenderTarget(Nothing)

            blurTexture = r
        End If

        blurDelay -= 0.1F
        If blurDelay <= 0.0F Then
            blurDelay = 0.5F
            If Core.Random.Next(0, 75) < Me.currentBlurIntensity Then
                Me.whiteLayers.Add(Core.windowSize)
            End If
            Dim v As New Vector2(Core.Random.Next(CInt(currentBlurPosition.X - currentBlurIntensity), CInt(currentBlurPosition.X + currentBlurIntensity)), Core.Random.Next(CInt(currentBlurPosition.Y - currentBlurIntensity), CInt(currentBlurPosition.Y + currentBlurIntensity)))

            Me.blurLayers.Add(New Rectangle(CInt(v.X - (currentBlurZoom / 2)), CInt(v.Y - (currentBlurZoom / 2)), Core.windowSize.Width + currentBlurZoom, Core.windowSize.Height + currentBlurZoom))

            Me.currentBlurIntensity += 2
            Me.currentBlurZoom += 55
        End If

        If Me.currentBlurIntensity = 80 Or SongOver() = True Then
            ready = True
        End If
    End Sub

    Dim animationAfterReady As Integer = 0
    Private Sub DrawTrainerIntro()
        Dim barPosition As Vector2 = New Vector2(Trainer.BarImagePosition.X * 128, Trainer.BarImagePosition.Y * 128)
        Dim VSPosition As Vector2 = New Vector2(Trainer.VSImagePosition.X * 128, Trainer.VSImagePosition.Y * 128 + 64)

        If Trainer.VSImageOrigin <> "VSIntro" Then
            VSPosition.Y -= 64
        End If

        Dim t1 As Texture2D = TextureManager.GetTexture("GUI\Intro\VSIntro", New Rectangle(CInt(barPosition.X), CInt(barPosition.Y), 128, 64), "")
        Dim t2 As Texture2D = TextureManager.GetTexture("GUI\Intro\" & Trainer.VSImageOrigin, New Rectangle(CInt(VSPosition.X), CInt(VSPosition.Y), Trainer.VSImageSize.Width, Trainer.VSImageSize.Height), "")
        Dim t3 As Texture2D = TextureManager.GetTexture("NPC\" & Trainer.SpriteName, New Rectangle(0, 64, 32, 32))
        Dim t4 As Texture2D = Nothing
        If Trainer.DoubleTrainer = True Then
            t4 = TextureManager.GetTexture("NPC\" & Trainer.SpriteName2, New Rectangle(0, 64, 32, 32))
        End If

        If Trainer.GameJoltID <> "" Then
            If GameJolt.Emblem.HasDownloadedSprite(Trainer.GameJoltID) = True Then
                Dim t As Texture2D = GameJolt.Emblem.GetOnlineSprite(Trainer.GameJoltID)
                If Not t Is Nothing Then
                    Dim spriteSize As New Vector2(t.Width / 3.0F, t.Height / 4.0F)
                    t3 = TextureManager.GetTexture(t, New Rectangle(0, CInt(spriteSize.Y * 2), CInt(spriteSize.X), CInt(spriteSize.Y)))
                End If
            End If
        End If

        Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width, CInt(((value / 1140) * Core.windowSize.Height * 1.5F))), Color.Black)

        For i = -256 To Core.windowSize.Width Step 256
            Dim offset As Integer = value + (animationAfterReady * 7)
            While offset >= 256
                offset -= 256
            End While

            Core.SpriteBatch.Draw(t1, New Rectangle(CInt(i + offset), CInt(Core.windowSize.Height / 2 - 64), 256, 128), Color.White)
        Next

        If Trainer.DoubleTrainer = True Then
            Dim t As String = ReplaceIntroName(Trainer.TrainerType) & " " & ReplaceIntroName(Trainer.Name) & " & " & ReplaceIntroName(Trainer.TrainerType2) & " " & ReplaceIntroName(Trainer.Name2)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(Core.windowSize.Width - FontManager.InGameFont.MeasureString(t).X - 50, CInt(Core.windowSize.Height / 2 + 20)), Color.White)

            Core.SpriteBatch.Draw(t3, New Rectangle(Core.windowSize.Width - 540, CInt(Core.windowSize.Height / 2 - 230), 256, 256), Color.White)
            Core.SpriteBatch.Draw(t4, New Rectangle(Core.windowSize.Width - 280, CInt(Core.windowSize.Height / 2 - 230), 256, 256), Color.White)
        Else
            Dim t As String = ReplaceIntroName(Trainer.TrainerType) & " " & ReplaceIntroName(Trainer.Name)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(Core.windowSize.Width - FontManager.InGameFont.MeasureString(t).X - 50, CInt(Core.windowSize.Height / 2 + 20)), Color.White)

            Core.SpriteBatch.Draw(t3, New Rectangle(Core.windowSize.Width - 310, CInt(Core.windowSize.Height / 2 - 230), 256, 256), Color.White)
        End If
        Core.SpriteBatch.Draw(t2, New Rectangle(420 - CInt(CInt(1.29 * value) / 3), CInt(Core.windowSize.Height / 2 - 20) - CInt(CInt(1 * value) / 3), CInt(1.12 * CInt(value / 1.5F)), 1 * CInt(value / 1.5F)), Color.White)
    End Sub

    Private Function ReplaceIntroName(ByVal Name As String) As String
        Dim n As String = Name.Replace("<rivalname>", Core.Player.RivalName)
        n = n.Replace("<playername>", Core.Player.Name)
        n = n.Replace("<player.name>", Core.Player.Name)
        n = n.Replace("[POKE]", "Poké")

        Return n
    End Function

    Dim blackPosition As Integer = 0
    Dim trainerPosition As Integer = 0
    Dim barOffset As Integer = 0
    Dim textPosition As Integer = 0

    Private Sub DrawFaceshotIntro()
        Dim barPosition As Vector2 = New Vector2(Trainer.BarImagePosition.X * 128, Trainer.BarImagePosition.Y * 128)
        Dim VSPosition As Vector2 = New Vector2(Trainer.VSImagePosition.X * 128, Trainer.VSImagePosition.Y * 128 + 64)

        Dim t1 As Texture2D = TextureManager.GetTexture("GUI\Intro\VSIntro", New Rectangle(CInt(barPosition.X), CInt(barPosition.Y), 128, 64), "")
        Dim t2 As Texture2D = TextureManager.GetTexture("GUI\Intro\VSIntro", New Rectangle(CInt(VSPosition.X), CInt(VSPosition.Y), 61, 54), "")
        Dim t3 As Texture2D = TextureManager.GetTexture("NPC\" & Trainer.SpriteName, New Rectangle(0, 64, 32, 32))
        Dim t4 As Texture2D = Nothing
        If Trainer.DoubleTrainer = True Then
            t4 = TextureManager.GetTexture("NPC\" & Trainer.SpriteName2, New Rectangle(0, 64, 32, 32))
        End If

        If Trainer.GameJoltID <> "" Then
            If GameJolt.Emblem.HasDownloadedSprite(Trainer.GameJoltID) = True Then
                Dim t As Texture2D = GameJolt.Emblem.GetOnlineSprite(Trainer.GameJoltID)
                If Not t Is Nothing Then
                    Dim spriteSize As New Vector2(t.Width / 3.0F, t.Height / 4.0F)
                    t3 = TextureManager.GetTexture(t, New Rectangle(0, CInt(spriteSize.Y * 2), CInt(spriteSize.X), CInt(spriteSize.Y)))
                End If
            End If
        End If

        For i = -512 To Core.windowSize.Width Step 512
            Dim offset As Integer = barOffset + (animationAfterReady * 7)
            While offset >= 512
                offset -= 512
            End While

            Core.SpriteBatch.Draw(t1, New Rectangle(CInt(i + offset), CInt(Core.windowSize.Height / 2 - 128), 512, 256), Color.White)
        Next

        Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width, blackPosition), Color.Black)
        Core.SpriteBatch.Draw(t3, New Rectangle(Core.windowSize.Width - trainerPosition, CInt(Core.windowSize.Height / 2) - 96, 256, 224), New Rectangle(0, 0, 32, 28), Color.White)
        Core.SpriteBatch.Draw(t2, New Rectangle(trainerPosition - 61 * 4, CInt(Core.windowSize.Height / 2) - 96, 61 * 4, 54 * 4), Color.White)
        Canvas.DrawRectangle(New Rectangle(0, Core.windowSize.Height - blackPosition, Core.windowSize.Width, blackPosition), Color.Black)

        If textPosition > 0 Then
            Dim tWidth As Integer = CInt(FontManager.InGameFont.MeasureString(Trainer.TrainerType).X * 3.0F)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Trainer.TrainerType, New Vector2((textPosition - tWidth).Clamp(-tWidth, CInt(Core.windowSize.Width / 2 - tWidth / 2)), 50), Color.White, 0.0F, New Vector2(0), 3.0F, SpriteEffects.None, 0.0F)
            If textPosition > 300 Then
                tWidth = CInt(FontManager.InGameFont.MeasureString(Trainer.Name).X * 3.0F)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, Trainer.Name, New Vector2((Core.windowSize.Width - (textPosition - 300)).Clamp(CInt(Core.windowSize.Width / 2 - tWidth / 2), Core.windowSize.Width), Core.windowSize.Height - 180), Color.White, 0.0F, New Vector2(0), 3.0F, SpriteEffects.None, 0.0F)
            End If
        End If
    End Sub

    Public Overrides Sub Update()
        If minDelay > 0.0F Then
            minDelay -= 0.1F
            If minDelay <= 0.0F Then
                minDelay = 0.0F
            End If
        End If

        If ready = True Then
            animationAfterReady += 1
            If Me.minDelay = 0.0F And SongOver() Then
                Core.SetScreen(Me.NewScreen)
                If Me.NewScreen.GetType() Is GetType(BattleSystem.BattleScreen) Then
                    MediaPlayer.IsRepeating = True

                    Dim b As BattleSystem.BattleScreen = CType(Me.NewScreen, BattleSystem.BattleScreen)

                    If b.IsPVPBattle = True Then
                        b.InitializePVP(b.Trainer, b.OverworldScreen)
                    Else
                        If b.IsTrainerBattle = True Then
                            b.InitializeTrainer(b.Trainer, b.OverworldScreen, b.defaultMapType)
                        Else
                            If Screen.Level.IsSafariZone = True Then
                                b.InitializeSafari(b.WildPokemon, b.OverworldScreen, b.defaultMapType)
                            Else
                                If Screen.Level.IsBugCatchingContest = True Then
                                    b.InitializeBugCatch(b.WildPokemon, b.OverworldScreen, b.defaultMapType)
                                Else
                                    b.InitializeWild(b.WildPokemon, b.OverworldScreen, b.defaultMapType)
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Else
            Select Case AnimationType
                Case 0, 5
                    UpdateRectangleIntro()
                Case 1, 6
                    UpdateHorizontalBars()
                Case 2, 7
                    UpdateVerticalBars()
                Case 3, 8
                    UpdateBlockIn()
                Case 4, 9
                    UpdateBlockOut()
                Case 10
                    UpdateTrainerVS()
                Case 11
                    UpdateFaceshotIntro()
                Case 12
                    UpdateBlurIntro()
            End Select
        End If

        ResetCursor()
    End Sub

    Private Sub UpdateFaceshotIntro()
        Me.barOffset += 14
        Me.blackPosition = (Me.blackPosition + 6).Clamp(0, CInt(Core.windowSize.Height / 2 - 128))
        If blackPosition >= CInt(Core.windowSize.Height / 2 - 128) Then
            trainerPosition = (trainerPosition + 16).Clamp(0, 420)
            If trainerPosition >= 420 Then
                textPosition += CInt(Math.Ceiling(Core.windowSize.Width / 75))
                If textPosition >= CInt(Core.windowSize.Width / 2 - CInt(FontManager.InGameFont.MeasureString(Trainer.Name).X) / 2) + 1200 Then
                    Me.ready = True
                End If
            End If
        End If
    End Sub

    Private Sub UpdateRectangleIntro()
        Dim rectangleSize As Integer = Me.GetRectangleSize()

        Dim fullRecs As Integer = CInt(Math.Ceiling(Core.windowSize.Height / rectangleSize)) * CInt(Math.Ceiling(Core.windowSize.Width / rectangleSize))
        Dim currentRecs As Integer = Animations.Count

        If fullRecs > currentRecs Then
            Dim validPosition As Boolean = False
            Dim Pos As Vector2

            While validPosition = False
                Pos = New Vector2(Core.Random.Next(0, CInt(Math.Ceiling((Core.windowSize.Width - rectangleSize) / rectangleSize)) + 1) * rectangleSize, Core.Random.Next(0, CInt(Math.Ceiling((Core.windowSize.Height - rectangleSize) / rectangleSize)) + 1) * rectangleSize)

                validPosition = True

                If Animations.Count > 0 Then
                    For Each R As Rectangle In Animations
                        If R.X = Pos.X And R.Y = Pos.Y Then
                            validPosition = False
                            Exit For
                        End If
                    Next
                End If
            End While

            Animations.Add(New Rectangle(CInt(Pos.X), CInt(Pos.Y), rectangleSize, rectangleSize))
        Else
            ready = True
        End If
    End Sub

    Private Function GetRectangleSize() As Integer
        Dim blocksOnScreen As Double = 81.6

        Dim pixelAmount As Double = Core.windowSize.Width * Core.windowSize.Height

        Dim perRectanglePixels As Double = pixelAmount / blocksOnScreen

        Return CInt(Math.Ceiling(Math.Sqrt(perRectanglePixels)))
    End Function

    Private Sub UpdateHorizontalBars()
        If Animations.Count < 20 Then
            If Core.Random.Next(0, 4) = 0 Then
                Dim validPosition As Boolean = False
                Dim Pos As Vector2

                While validPosition = False
                    Pos = New Vector2(0, CInt(Core.windowSize.Height / 20) * Core.Random.Next(0, 20))

                    validPosition = True

                    If Animations.Count > 0 Then
                        For Each R As Rectangle In Animations
                            If R.X = Pos.X And R.Y = Pos.Y Then
                                validPosition = False
                                Exit For
                            End If
                        Next
                    End If
                End While

                Animations.Add(New Rectangle(CInt(Pos.X), CInt(Pos.Y), Core.windowSize.Width, CInt(Core.windowSize.Height / 20)))
            End If
        Else
            ready = True
        End If
    End Sub

    Private Sub UpdateVerticalBars()
        If Animations.Count < 20 Then
            If Core.Random.Next(0, 4) = 0 Then
                Dim validPosition As Boolean = False
                Dim Pos As Vector2

                While validPosition = False
                    Pos = New Vector2(CInt(Core.windowSize.Width / 20) * Core.Random.Next(0, 20), 0)

                    validPosition = True

                    If Animations.Count > 0 Then
                        For Each R As Rectangle In Animations
                            If R.X = Pos.X And R.Y = Pos.Y Then
                                validPosition = False
                                Exit For
                            End If
                        Next
                    End If
                End While

                Animations.Add(New Rectangle(CInt(Pos.X), CInt(Pos.Y), CInt(Core.windowSize.Width / 20), Core.windowSize.Height))
            End If
        Else
            ready = True
        End If
    End Sub

    Private Sub UpdateBlockIn()
        If Animations.Count = 0 Then
            Animations.Add(New Rectangle(CInt(Core.windowSize.Width / 2) - 10, CInt(Core.windowSize.Height / 2) - 10, 20, 20))
        Else
            If Animations(0).Width >= Core.windowSize.Width Then
                ready = True
            Else
                Dim a As Rectangle = Animations(0)

                a.X -= 7
                a.Y -= 7

                a.Width += 14
                a.Height += 14

                Animations.RemoveAt(0)
                Animations.Add(a)
            End If
        End If
    End Sub

    Private Sub UpdateBlockOut()
        If Animations.Count = 0 Then
            Animations.Add(New Rectangle(0, 0, Core.windowSize.Width, Core.windowSize.Height))
        Else
            If value >= Core.windowSize.Height / 2 Then
                ready = True
            Else
                value += CInt(Math.Ceiling(Core.windowSize.Height / 300))
            End If
        End If
    End Sub

    Private Sub UpdateTrainerVS()
        value += 7
        If value >= 1140 Then
            ready = True
        End If
    End Sub

    Public Sub ResetCursor()
        If Core.GameInstance.IsActive = True Then
            Mouse.SetPosition(CInt(Core.windowSize.Width / 2), CInt(Core.windowSize.Height / 2))
            oldX = CInt(Core.windowSize.Width / 2)
            oldY = CInt(Core.windowSize.Height / 2)
        End If
    End Sub

    Public Overrides Sub ChangeTo()
        Player.Temp.IsInBattle = True
        Player.Temp.BeforeBattlePosition = Screen.Camera.Position
        Player.Temp.BeforeBattleLevelFile = Screen.Level.LevelFile
        Player.Temp.BeforeBattleFacing = Screen.Camera.GetPlayerFacingDirection() 
        MusicManager.PlayMusic(MusicLoop)
        MediaPlayer.IsRepeating = False

        Dim s As MusicManager.CSong = MusicManager.GetSong(MusicLoop, True)
        If Not s Is Nothing Then
            Me.duration = s.Song.Duration
        Else
            Me.duration = New TimeSpan(0)
        End If

        Me.startTime = Date.Now
    End Sub

    Private Function SongOver() As Boolean
        Return startTime + duration < Date.Now
    End Function

    'Protected Overrides Sub Finalize()
    '    If blurTexture IsNot Nothing
    '        blurTexture.Dispose()
    '    End If
    'End Sub
End Class