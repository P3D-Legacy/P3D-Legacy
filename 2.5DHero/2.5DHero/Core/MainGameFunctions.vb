Public Class MainGameFunctions

    Public Shared Sub FunctionKeys()
        If KeyBoardHandler.KeyPressed(KeyBindings.ScreenshotKey) = True And Core.CurrentScreen.CanTakeScreenshot = True Then
            CaptureScreen()
        End If
        If KeyBoardHandler.KeyPressed(KeyBindings.FullScreenKey) = True Then
            If Core.CurrentScreen.CanGoFullscreen = True Then
                ToggleFullScreen()
            End If
        End If
        If KeyBoardHandler.KeyPressed(KeyBindings.DebugKey) = True Then
            Core.GameOptions.ShowDebug += 1
            If Core.GameOptions.ShowDebug >= 2 Then
                Core.GameOptions.ShowDebug = 0
            End If
            Core.GameOptions.SaveOptions()
        End If
        If KeyBoardHandler.KeyPressed(KeyBindings.GUIControlKey) = True Then
            Core.GameOptions.ShowGUI = Not Core.GameOptions.ShowGUI
            Core.GameOptions.SaveOptions()
        End If
        If KeyBoardHandler.KeyPressed(KeyBindings.MuteMusicKey) = True And Core.CurrentScreen.CanMuteMusic = True Then
            MusicManager.Mute(Not MediaPlayer.IsMuted)
            SoundManager.Mute(MediaPlayer.IsMuted)
            Core.GameOptions.SaveOptions()
            Core.CurrentScreen.ToggledMute()
        End If
        If KeyBoardHandler.KeyPressed(KeyBindings.LightKey) = True Then
            Core.GameOptions.LightingEnabled = Not Core.GameOptions.LightingEnabled
        End If
        If KeyBoardHandler.KeyDown(KeyBindings.DebugKey) = True Then
            If KeyBoardHandler.KeyPressed(Keys.F) = True Then
                TextureManager.TextureList.Clear()
            End If
            If KeyBoardHandler.KeyPressed(Keys.S) = True Then
                Core.SetWindowSize(New Vector2(1200, 680))
            End If
        End If
        If ControllerHandler.ButtonPressed(Buttons.Back, True) = True Then
            Core.GameOptions.GamePadEnabled = Not Core.GameOptions.GamePadEnabled
            If Core.GameOptions.GamePadEnabled = True Then
                Core.GameMessage.ShowMessage("Enabled XBOX 360 GamePad support.", 12, FontManager.MainFont, Color.White)
            Else
                Core.GameMessage.ShowMessage("Disabled XBOX 360 GamePad support.", 12, FontManager.MainFont, Color.White)
            End If
            Core.GameOptions.SaveOptions()
        End If
        If KeyBoardHandler.KeyPressed(Keys.L) = True And KeyBoardHandler.KeyDown(KeyBindings.DebugKey) = True Then
            Logger.DisplayLog = Not Logger.DisplayLog
        End If
        If KeyBoardHandler.KeyPressed(Keys.B) = True And KeyBoardHandler.KeyDown(KeyBindings.DebugKey) = True Then
            Entity.drawViewBox = Not Entity.drawViewBox
        End If
    End Sub

    Private Shared Sub CaptureScreen()
        Try
            Core.GameMessage.HideMessage()

            Dim fileName As String = ""
            With My.Computer.Clock.LocalTime
                Dim month As String = .Month.ToString()
                If month.Length = 1 Then
                    month = "0" & month
                End If
                Dim day As String = .Day.ToString()
                If day.Length = 1 Then
                    day = "0" & day
                End If
                Dim hour As String = .Hour.ToString()
                If hour.Length = 1 Then
                    hour = "0" & hour
                End If
                Dim minute As String = .Minute.ToString()
                If minute.Length = 1 Then
                    minute = "0" & minute
                End If
                Dim second As String = .Second.ToString()
                If second.Length = 1 Then
                    second = "0" & second
                End If
                fileName = .Year & "-" & month & "-" & day & "_" & hour & "." & minute & "." & second & ".png"
            End With

            If System.IO.Directory.Exists(GameController.GamePath & "\screenshots\") = False Then
                System.IO.Directory.CreateDirectory(GameController.GamePath & "\screenshots\")
            End If

            If Core.GraphicsManager.IsFullScreen = False Then
                Dim b As New System.Drawing.Bitmap(Core.windowSize.Width, Core.windowSize.Height)
                Using g As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(b)
                    g.CopyFromScreen(Core.window.ClientBounds.X, Core.window.ClientBounds.Y, 0, 0, New System.Drawing.Size(b.Width, b.Height))
                End Using

                b.Save(GameController.GamePath & "\screenshots\" & fileName, System.Drawing.Imaging.ImageFormat.Png)
            Else
                Dim screenshot As New RenderTarget2D(Core.GraphicsDevice, Core.windowSize.Width, Core.windowSize.Height, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
                Core.GraphicsDevice.SetRenderTarget(screenshot)

                Core.Draw()

                Core.GraphicsDevice.SetRenderTarget(Nothing)

                Dim stream As System.IO.Stream = System.IO.File.OpenWrite(GameController.GamePath & "\screenshots\" & fileName)
                screenshot.SaveAsPng(stream, Core.windowSize.Width, Core.windowSize.Height)
                stream.Dispose()
            End If

            Core.GameMessage.SetupText(Localization.GetString("game_message_screenshot") & fileName, FontManager.MainFont, Color.White)
            Core.GameMessage.ShowMessage(12, Core.GraphicsDevice)
        Catch ex As Exception
            Logger.Log(Logger.LogTypes.ErrorMessage, "Basic.vb: " & Localization.GetString("game_message_screenshot_failed") & ". More information: " & ex.Message)
        End Try
    End Sub

    Private Shared Sub ToggleFullScreen()
        If Core.GraphicsManager.IsFullScreen = False Then
            Core.GraphicsManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width
            Core.GraphicsManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
            Core.windowSize = New Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)

            System.Windows.Forms.Application.VisualStyleState = Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled

            Core.GraphicsManager.ToggleFullScreen()

            Core.GameMessage.ShowMessage(Localization.GetString("game_message_fullscreen_on"), 12, FontManager.MainFont, Color.White)
        Else
            Core.GraphicsManager.PreferredBackBufferWidth = 1200
            Core.GraphicsManager.PreferredBackBufferHeight = 680
            Core.windowSize = New Rectangle(0, 0, 1200, 680)

            System.Windows.Forms.Application.VisualStyleState = Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled

            Core.GraphicsManager.ToggleFullScreen()

            Core.GameMessage.ShowMessage(Localization.GetString("game_message_fullscreen_off"), 12, FontManager.MainFont, Color.White)
        End If

        Core.GraphicsManager.ApplyChanges()
        NetworkPlayer.ScreenRegionChanged()
    End Sub

End Class