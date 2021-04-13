Public Class MainGameFunctions

    Public Shared Sub FunctionKeys()
        If KeyBoardHandler.KeyPressed(KeyBindings.GUIControlKey) = True Then
            Core.GameOptions.ShowGUI = Not Core.GameOptions.ShowGUI
            Core.GameOptions.SaveOptions()
        ElseIf KeyBoardHandler.KeyPressed(KeyBindings.ScreenshotKey) AndAlso Core.CurrentScreen.CanTakeScreenshot Then
            CaptureScreen()
        ElseIf KeyBoardHandler.KeyPressed(KeyBindings.DebugKey) Then
            Core.GameOptions.ShowDebug += 1
            If Core.GameOptions.ShowDebug >= 2 Then
                Core.GameOptions.ShowDebug = 0
            End If
            Core.GameOptions.SaveOptions()
        ElseIf KeyBoardHandler.KeyPressed(KeyBindings.LightKey) Then
            Core.GameOptions.LightingEnabled = Not Core.GameOptions.LightingEnabled
            Core.GameOptions.SaveOptions()

            If Core.GameOptions.LightingEnabled Then
                Core.GameMessage.ShowMessage(Localization.Translate("game_message.lighting") & ": " & Localization.Translate("global.on"), 12, FontManager.MainFont, Color.White)
            Else
                Core.GameMessage.ShowMessage(Localization.Translate("game_message.lighting") & ": " & Localization.Translate("global.off"), 12, FontManager.MainFont, Color.White)
            End If
        ElseIf KeyBoardHandler.KeyPressed(KeyBindings.FullScreenKey) AndAlso Core.CurrentScreen.CanGoFullscreen Then
            ToggleFullScreen()
        ElseIf KeyBoardHandler.KeyPressed(KeyBindings.MuteAudioKey) AndAlso Core.CurrentScreen.CanMuteMusic Then
            If MusicManager.Muted Then
                MusicManager.Muted = False
                SoundManager.Muted = False
                Core.GameMessage.ShowMessage(Localization.Translate("game_message.audio") & ": " & Localization.Translate("global.on"), 12, FontManager.MainFont, Color.White)
            Else
                MusicManager.Muted = True
                SoundManager.Muted = True
                Core.GameMessage.ShowMessage(Localization.Translate("game_message.audio") & ": " & Localization.Translate("global.off"), 12, FontManager.MainFont, Color.White)
            End If

            Core.GameOptions.SaveOptions()
            Core.CurrentScreen.ToggledMute()
        End If

        If KeyBoardHandler.KeyDown(KeyBindings.DebugKey) = True Then
            If KeyBoardHandler.KeyPressed(Keys.F) Then
                TextureManager.TextureList.Clear()
                Core.GameMessage.ShowMessage("Texture list has been cleared", 12, FontManager.MainFont, Color.White)
            ElseIf KeyBoardHandler.KeyPressed(Keys.S) Then
                Core.SetWindowSize(New Vector2(1200, 680))
            ElseIf KeyBoardHandler.KeyPressed(Keys.L) Then
                Logger.DisplayLog = Not Logger.DisplayLog
            ElseIf KeyBoardHandler.KeyPressed(Keys.B) Then
                Entity.drawViewBox = Not Entity.drawViewBox
            End If
        End If

        If ControllerHandler.ButtonPressed(Buttons.Back, True) = True Then
            Core.GameOptions.GamePadEnabled = Not Core.GameOptions.GamePadEnabled
            If Core.GameOptions.GamePadEnabled Then
                Core.GameMessage.ShowMessage(Localization.Translate("game_message.xbox_controller") & ": " & Localization.Translate("global.enabled"), 12, FontManager.MainFont, Color.White)
            Else
                Core.GameMessage.ShowMessage(Localization.Translate("game_message.xbox_controller") & ": " & Localization.Translate("global.disabled"), 12, FontManager.MainFont, Color.White)
            End If
            Core.GameOptions.SaveOptions()
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

            If Directory.Exists(GameController.GamePath & "\screenshots\") = False Then
                Directory.CreateDirectory(GameController.GamePath & "\screenshots\")
            End If

            If Core.GraphicsManager.IsFullScreen = False Then
                Dim b As New Drawing.Bitmap(Core.windowSize.Width, Core.windowSize.Height)
                Using g As Drawing.Graphics = Drawing.Graphics.FromImage(b)
                    g.CopyFromScreen(Core.Window.ClientBounds.X, Core.Window.ClientBounds.Y, 0, 0, New Drawing.Size(b.Width, b.Height))
                End Using

                b.Save(GameController.GamePath & "\screenshots\" & fileName, Drawing.Imaging.ImageFormat.Png)
            Else
                Dim screenshot As New RenderTarget2D(Core.GraphicsDevice, Core.windowSize.Width, Core.windowSize.Height, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
                Core.GraphicsDevice.SetRenderTarget(screenshot)

                Core.Draw()

                Core.GraphicsDevice.SetRenderTarget(Nothing)

                Dim stream As Stream = File.OpenWrite(GameController.GamePath & "\screenshots\" & fileName)
                screenshot.SaveAsPng(stream, Core.windowSize.Width, Core.windowSize.Height)
                stream.Dispose()
            End If

            Core.GameMessage.SetupText(Localization.Translate("game_message.screenshot") & " " & fileName, FontManager.MainFont, Color.White)
            Core.GameMessage.ShowMessage(12, Core.GraphicsDevice)
        Catch ex As Exception
            Logger.Log(Logger.LogTypes.ErrorMessage, "Basic.vb: Could not save screenshot. More information: " & ex.Message)
        End Try
    End Sub

    Private Shared Sub ToggleFullScreen()
        If Core.GraphicsManager.IsFullScreen = False Then
            ' MonoGame Bug > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width != System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width
            ' MonoGame Bug > GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height != System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height
            ' Temp Fix just in case someone else face this as well.
            If GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width <> Windows.Forms.Screen.PrimaryScreen.Bounds.Width OrElse
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height <> Windows.Forms.Screen.PrimaryScreen.Bounds.Height Then
                Core.GraphicsManager.PreferredBackBufferWidth = Windows.Forms.Screen.PrimaryScreen.Bounds.Width
                Core.GraphicsManager.PreferredBackBufferHeight = Windows.Forms.Screen.PrimaryScreen.Bounds.Height
                Core.windowSize = New Rectangle(0, 0, Windows.Forms.Screen.PrimaryScreen.Bounds.Width, Windows.Forms.Screen.PrimaryScreen.Bounds.Height)
            Else
                Core.GraphicsManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width
                Core.GraphicsManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
                Core.windowSize = New Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            End If

            Windows.Forms.Application.VisualStyleState = Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled

            Core.GraphicsManager.ToggleFullScreen()

            Core.GameMessage.ShowMessage(Localization.Translate("game_message.fullscreen") & ": " & Localization.Translate("global.on"), 12, FontManager.MainFont, Color.White)
        Else
            Core.GraphicsManager.PreferredBackBufferWidth = 1200
            Core.GraphicsManager.PreferredBackBufferHeight = 680
            Core.windowSize = New Rectangle(0, 0, 1200, 680)

            Windows.Forms.Application.VisualStyleState = Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled

            Core.GraphicsManager.ToggleFullScreen()

            Core.GameMessage.ShowMessage(Localization.Translate("game_message.fullscreen") & ": " & Localization.Translate("global.off"), 12, FontManager.MainFont, Color.White)
        End If

        Core.GraphicsManager.ApplyChanges()
        NetworkPlayer.ScreenRegionChanged()
    End Sub

End Class