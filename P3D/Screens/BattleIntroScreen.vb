﻿Public Class BattleIntroScreen

    Inherits Screen

    Dim oldX, oldY As Integer

    Dim AnimationType As Integer
    Dim Animations As New List(Of Rectangle)

    Public OldScreen As Screen
    Public NewScreen As Screen

    Dim ready As Boolean = False
    Dim value As Integer = 0

    Dim Trainer As Trainer

    Dim minDelay As Single = 4.0F
    Dim startTime As Date
    Dim duration As TimeSpan

    Public Enum BattleType As Integer
        PVP = 0
        TRAINER = 1
        SAFARI = 2
        BUG_CATCHING = 3
        ROAMING = 4
        WILD = 5
    End Enum

    Public MusicLoop As String = ""

    Public Sub New(ByVal OldScreen As Screen, ByVal NewScreen As Screen, ByVal IntroType As Integer)
        Dim musicLoop As String = Screen.Level.CurrentRegion.Split(CChar(","))(0) & "_wild_intro"
        If Not BattleSystem.BattleScreen.CustomBattleMusic = "" OrElse MusicManager.SongExists(BattleSystem.BattleScreen.CustomBattleMusic) = True Then
            musicLoop = BattleSystem.BattleScreen.CustomBattleMusic & "_intro"
        Else
            If BattleSystem.BattleScreen.RoamingBattle = True Then
                If BattleSystem.BattleScreen.RoamingPokemonStorage.MusicLoop <> "" Then
                    musicLoop = BattleSystem.BattleScreen.RoamingPokemonStorage.MusicLoop & "_intro"
                End If
            End If
        End If

        If ShouldPlayNightTheme(musicLoop) Then
            musicLoop = musicLoop & "_night"
        End If

        If MusicManager.SongExists(musicLoop) = False Then
            musicLoop = "johto_wild_intro"
        End If

        Me.Constructor(OldScreen, NewScreen, Nothing, musicLoop, IntroType)
    End Sub

    Public Sub New(ByVal OldScreen As Screen, ByVal NewScreen As Screen, ByVal IntroType As Integer, ByVal MusicLoop As String)
        If MusicLoop = "" Then
            MusicLoop = Screen.Level.CurrentRegion.Split(CChar(","))(0) & "_wild_intro"
            If MusicManager.SongExists(MusicLoop) = True Then
                If BattleSystem.BattleScreen.RoamingBattle = True Then
                    If BattleSystem.BattleScreen.RoamingPokemonStorage.MusicLoop <> "" Then
                        MusicLoop = BattleSystem.BattleScreen.RoamingPokemonStorage.MusicLoop & "_intro"
                    End If
                End If
            End If

            If ShouldPlayNightTheme(MusicLoop) Then
                MusicLoop = MusicLoop & "_night"
            End If

            If MusicManager.SongExists(MusicLoop) = False Then
                MusicLoop = "johto_wild_intro"
            End If
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
        Dim TrainerTexture1 As Texture2D = TextureManager.GetTexture("Textures\NPC\" & Trainer.SpriteName)
        Dim TrainerTexture2 As Texture2D = Nothing
        Dim Trainer1FrameSize As Size = Nothing
        Dim Trainer2FrameSize As Size = Nothing

        If Trainer.VSImageOrigin <> "VSIntro" Then
            VSPosition.Y -= 64
        End If
        If TrainerTexture1.Width = TrainerTexture1.Height / 2 Then
            Trainer1FrameSize = New Size(CInt(TrainerTexture1.Width / 2), CInt(TrainerTexture1.Height / 4))
        ElseIf TrainerTexture1.Width = TrainerTexture1.Height Then
            Trainer1FrameSize = New Size(CInt(TrainerTexture1.Width / 4), CInt(TrainerTexture1.Height / 4))
        Else
            Trainer1FrameSize = New Size(CInt(TrainerTexture1.Width / 3), CInt(TrainerTexture1.Height / 4))
        End If
        Dim t1 As Texture2D = TextureManager.GetTexture("GUI\Intro\VSIntro", New Rectangle(CInt(barPosition.X), CInt(barPosition.Y), 128, 64), "")
        Dim t2 As Texture2D = TextureManager.GetTexture("GUI\Intro\" & Trainer.VSImageOrigin, New Rectangle(CInt(VSPosition.X), CInt(VSPosition.Y), Trainer.VSImageSize.Width, Trainer.VSImageSize.Height), "")
        Dim t3 As Texture2D = TextureManager.GetTexture("NPC\" & Trainer.SpriteName, New Rectangle(0, Trainer1FrameSize.Height * 2, Trainer1FrameSize.Width, Trainer1FrameSize.Height))
        Dim t4 As Texture2D = Nothing
        If Trainer.DoubleTrainer = True Then
            TrainerTexture2 = TextureManager.GetTexture("Textures\NPC\" & Trainer.SpriteName2)
            If TrainerTexture1.Width = TrainerTexture1.Height / 2 Then
                Trainer2FrameSize = New Size(CInt(TrainerTexture2.Width / 2), CInt(TrainerTexture2.Height / 4))
            ElseIf TrainerTexture1.Width = TrainerTexture1.Height Then
                Trainer2FrameSize = New Size(CInt(TrainerTexture2.Width / 4), CInt(TrainerTexture2.Height / 4))
            Else
                Trainer2FrameSize = New Size(CInt(TrainerTexture2.Width / 3), CInt(TrainerTexture2.Height / 4))
            End If
            t4 = TextureManager.GetTexture("NPC\" & Trainer.SpriteName2, New Rectangle(0, Trainer2FrameSize.Height * 2, Trainer2FrameSize.Width, Trainer2FrameSize.Height))
        End If

        If Trainer.GameJoltID <> "" Then
            If GameJolt.Emblem.HasDownloadedSprite(Trainer.GameJoltID) = True Then
                Dim t As Texture2D = GameJolt.Emblem.GetOnlineSprite(Trainer.GameJoltID)
                If Not t Is Nothing Then
                    Dim spriteSize As Vector2
                    If t.Width = t.Height / 2 Then
                        spriteSize = New Vector2(CInt(t.Width / 2), CInt(t.Height / 4))
                    ElseIf t.Width = t.Height Then
                        spriteSize = New Vector2(CInt(t.Width / 4), CInt(t.Height / 4))
                    Else
                        spriteSize = New Vector2(CInt(t.Width / 3), CInt(t.Height / 4))
                    End If
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
            Core.SpriteBatch.Draw(t3, New Rectangle(Core.windowSize.Width - 540, CInt(Core.windowSize.Height / 2 - 105 - (MathHelper.Min(t3.Height * 10, 256) / 2)), MathHelper.Min(t3.Width * 10, 256), MathHelper.Min(t3.Height * 10, 256)), Color.White)
            Core.SpriteBatch.Draw(t4, New Rectangle(Core.windowSize.Width - 280, CInt(Core.windowSize.Height / 2 - 105 - (MathHelper.Min(t4.Height * 10, 256) / 2)), MathHelper.Min(t4.Width * 10, 256), MathHelper.Min(t4.Height * 10, 256)), Color.White)

            Dim t As String = ReplaceIntroName(Trainer.TrainerType) & " " & ReplaceIntroName(Trainer.Name) & " & " & ReplaceIntroName(Trainer.TrainerType2) & " " & ReplaceIntroName(Trainer.Name2)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(Core.windowSize.Width - FontManager.InGameFont.MeasureString(t).X - 50 + 2, CInt(Core.windowSize.Height / 2 + 20 + 2)), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(Core.windowSize.Width - FontManager.InGameFont.MeasureString(t).X - 50, CInt(Core.windowSize.Height / 2 + 20)), Color.White)

        Else
            Core.SpriteBatch.Draw(t3, New Rectangle(Core.windowSize.Width - 310, CInt(Core.windowSize.Height / 2 - 105 - (MathHelper.Min(t3.Height * 10, 256) / 2)), MathHelper.Min(t3.Width * 10, 256), MathHelper.Min(t3.Height * 10, 256)), Color.White)

            Dim t As String = ReplaceIntroName(Trainer.TrainerType) & " " & ReplaceIntroName(Trainer.Name)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(Core.windowSize.Width - FontManager.InGameFont.MeasureString(t).X - 50 + 2, CInt(Core.windowSize.Height / 2 + 20 + 2)), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(Core.windowSize.Width - FontManager.InGameFont.MeasureString(t).X - 50, CInt(Core.windowSize.Height / 2 + 20)), Color.White)

        End If
        Core.SpriteBatch.Draw(t2, New Rectangle(480 - CInt(CInt(1.29 * value) / 3), CInt(Core.windowSize.Height / 2 - 20) - CInt(CInt(1 * value) / 3), CInt(1.12 * CInt(value / 1.5F)), 1 * CInt(value / 1.5F)), Color.White)
    End Sub

    Private Function ReplaceIntroName(ByVal Name As String) As String
        Dim n As String = Name.Replace("<rivalname>", Core.Player.RivalName)
        n = n.Replace("<rival.name>", Core.Player.RivalName)
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
        Dim TrainerTexture1 As Texture2D = TextureManager.GetTexture("Textures\NPC\" & Trainer.SpriteName)
        Dim Trainer1FrameSize As Size = Nothing

        If TrainerTexture1.Width = TrainerTexture1.Height / 2 Then
            Trainer1FrameSize = New Size(CInt(TrainerTexture1.Width / 2), CInt(TrainerTexture1.Height / 4))
        ElseIf TrainerTexture1.Width = TrainerTexture1.Height Then
            Trainer1FrameSize = New Size(CInt(TrainerTexture1.Width / 4), CInt(TrainerTexture1.Height / 4))
        Else
            Trainer1FrameSize = New Size(CInt(TrainerTexture1.Width / 3), CInt(TrainerTexture1.Height / 4))
        End If

        Dim t1 As Texture2D = TextureManager.GetTexture("GUI\Intro\VSIntro", New Rectangle(CInt(barPosition.X), CInt(barPosition.Y), 128, 64), "")
        Dim t2 As Texture2D = TextureManager.GetTexture("GUI\Intro\VSIntro", New Rectangle(CInt(VSPosition.X), CInt(VSPosition.Y), 64, 64), "")
        Dim t3 As Texture2D = TextureManager.GetTexture(TrainerTexture1, New Rectangle(0, Trainer1FrameSize.Height * 2, Trainer1FrameSize.Width, Trainer1FrameSize.Height))

        If Trainer.GameJoltID <> "" Then
            If GameJolt.Emblem.HasDownloadedSprite(Trainer.GameJoltID) = True Then
                Dim t As Texture2D = GameJolt.Emblem.GetOnlineSprite(Trainer.GameJoltID)
                If Not t Is Nothing Then
                    Dim spriteSize As Vector2
                    If t.Width = t.Height / 2 Then
                        spriteSize = New Vector2(CInt(t.Width / 2), CInt(t.Height / 4))
                    ElseIf t.Width = t.Height Then
                        spriteSize = New Vector2(CInt(t.Width / 4), CInt(t.Height / 4))
                    Else
                        spriteSize = New Vector2(CInt(t.Width / 3), CInt(t.Height / 4))
                    End If
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
        Canvas.DrawRectangle(New Rectangle(0, Core.windowSize.Height - blackPosition, Core.windowSize.Width, blackPosition), Color.Black)
        Core.SpriteBatch.Draw(t3, New Rectangle(CInt(Core.windowSize.Width - trainerPosition), CInt(Core.windowSize.Height / 2 + 128 - (CInt(MathHelper.Min(t3.Height * 10, 256) * 0.875))), MathHelper.Min(t3.Width * 10, 256), CInt(MathHelper.Min(t3.Height * 10, 256) * 0.875)), New Rectangle(0, 0, t3.Width, CInt(t3.Height * 0.875)), Color.White)
        Core.SpriteBatch.Draw(t2, New Rectangle(CInt(trainerPosition * 1.5 - 64 * 7), CInt(Core.windowSize.Height / 2) - 192, 64 * 7, 64 * 7), Color.White)

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
        Me.blackPosition = (Me.blackPosition + 10).Clamp(0, CInt(Core.windowSize.Height / 2 - 128))
        If blackPosition >= CInt(Core.windowSize.Height / 2 - 128) Then
            trainerPosition = (trainerPosition + 20).Clamp(0, 420)
            If trainerPosition >= 420 Then
                textPosition += CInt(Math.Ceiling(Core.windowSize.Width / 50))
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
            Animations.Add(New Rectangle(CInt(Core.windowSize.Width / 2 - (Core.windowSize.Width / 100 / 2)), CInt(Core.windowSize.Height / 2 - (Core.windowSize.Height / 100 / 2)), CInt(Core.windowSize.Width / 100), CInt(Core.windowSize.Height / 100)))
        Else
            Dim Speed As Integer = CInt(Me.duration.TotalMilliseconds / Core.windowSize.Height * 4)
            If Animations(0).Height >= Core.windowSize.Height + 128 Then
                ready = True
            End If
            Dim a As Rectangle = Animations(0)

            a.X -= CInt(Speed)
            a.Y -= CInt(Speed / 16 * 9)

            a.Width += Speed * 2
            a.Height += CInt(Speed * 2 / 16 * 9)

            Animations.RemoveAt(0)
            Animations.Add(a)
        End If
    End Sub

    Private Sub UpdateBlockOut()
        If Animations.Count = 0 Then
            Animations.Add(New Rectangle(0, 0, Core.windowSize.Width, Core.windowSize.Height))
        Else
            If value >= Core.windowSize.Height / 2 + 4 Then
                ready = True
            Else
                value += CInt(Math.Ceiling(Me.duration.TotalMilliseconds / Core.windowSize.Height * 3))
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
        Dim b As BattleSystem.BattleScreen = CType(Me.NewScreen, BattleSystem.BattleScreen)

        MusicManager.Playlist.Clear()
        MusicManager.outputDevice.Stop()
        If BattleSystem.BattleScreen.CustomBattleMusic = "" OrElse MusicManager.SongExists(BattleSystem.BattleScreen.CustomBattleMusic) = False Then
            Dim battleType = BattleIntroScreen.BattleType.WILD
            If b.IsPVPBattle = True Then
                battleType = BattleIntroScreen.BattleType.PVP
            Else
                If b.IsTrainerBattle = True Then
                    battleType = BattleIntroScreen.BattleType.TRAINER
                ElseIf Screen.Level.IsSafariZone = True Then
                    battleType = BattleIntroScreen.BattleType.SAFARI
                ElseIf Screen.Level.IsBugCatchingContest = True Then
                    battleType = BattleIntroScreen.BattleType.BUG_CATCHING
                Else
                    If BattleSystem.BattleScreen.RoamingBattle = True Then
                        battleType = BattleIntroScreen.BattleType.ROAMING
                    Else
                        battleType = BattleIntroScreen.BattleType.WILD
                    End If
                End If
            End If

            Dim loopSong = GetLoopSong(battleType)
            MusicManager.Play(MusicLoop, True, 0.0F, True, loopSong)
        Else
            MusicManager.Play(MusicLoop, True, 0.0F, True, BattleSystem.BattleScreen.CustomBattleMusic)
        End If
        If Not MusicLoop Is Nothing Then
            If MusicManager.GetSong(MusicLoop).Duration.TotalSeconds <= 1 Then
                Me.duration = New TimeSpan(0)
                minDelay = 0
                ready = True
            Else
                Me.duration = MusicManager.GetSong(MusicLoop).Duration
            End If
        Else
            Me.duration = New TimeSpan(0)
            minDelay = 0
            ready = True
        End If
        Me.startTime = Date.Now
    End Sub

    Private Function GetLoopSong(battleType As BattleIntroScreen.BattleType) As String
        'pvp battle
        'trainer battle
        'safari zone
        'bug catching contest
        'roaming battle
        'wild pokemon

        Dim fallbackLoopSong = "johto_wild"
        Dim loopSong = Screen.Level.CurrentRegion.Split(CChar(","))(0) & "_wild"
        If battleType = BattleIntroScreen.BattleType.PVP Then
            loopSong = "pvp"
        ElseIf battleType = BattleIntroScreen.BattleType.TRAINER Then
            fallbackLoopSong = Trainer.GetBattleMusicName()
            loopSong = Trainer.GetBattleMusicName()
        ElseIf battleType = BattleIntroScreen.BattleType.SAFARI Then
            fallbackLoopSong = "johto_wild"
            loopSong = Screen.Level.CurrentRegion.Split(CChar(","))(0) & "_wild"
        ElseIf battleType = BattleIntroScreen.BattleType.BUG_CATCHING Then
            fallbackLoopSong = "johto_wild"
            loopSong = Screen.Level.CurrentRegion.Split(CChar(","))(0) & "_wild"
        ElseIf battleType = BattleIntroScreen.BattleType.ROAMING Then
            If BattleSystem.BattleScreen.RoamingPokemonStorage.MusicLoop <> "" Then
                loopSong = BattleSystem.BattleScreen.RoamingPokemonStorage.MusicLoop
            End If
        ElseIf battleType = BattleIntroScreen.BattleType.WILD Then
            fallbackLoopSong = "johto_wild"
            loopSong = Screen.Level.CurrentRegion.Split(CChar(","))(0) & "_wild"
        Else
            Console.WriteLine("Unknown Battle Type: " & battleType)
        End If

        If ShouldPlayNightTheme(loopSong) Then
            loopSong = loopSong & "_night"
            fallbackLoopSong = "johto_wild_night"
        End If

        If MusicManager.SongExists(loopSong) = True Then
            Return loopSong
        End If
        Return fallbackLoopSong
    End Function

    Private Function SongOver() As Boolean
        Return startTime + duration < Date.Now
    End Function

    Private Function ShouldPlayNightTheme(dayThemeName As String) As Boolean
        Return World.IsNight() And MusicManager.SongExists(dayThemeName & "_night", False)
    End Function

    'Protected Overrides Sub Finalize()
    '    If blurTexture IsNot Nothing
    '        blurTexture.Dispose()
    '    End If
    'End Sub
End Class