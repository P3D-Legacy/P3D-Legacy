Public Class OfflineGameWarningScreen

    Inherits Screen

    Public Sub New(ByVal currentScreen As Screen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.OfflineGameWarningScreen

        Me.CanBePaused = False
        Me.CanChat = False
        Me.CanDrawDebug = True
        Me.CanGoFullscreen = True
        Me.CanMuteAudio = True
        Me.CanTakeScreenshot = True
        Me.MouseVisible = True
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))
        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 310), 90, 620, 420), New Color(16, 16, 16))
        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 100, 600, 400), New Color(39, 39, 39))

        Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.Translate("offline_game.title"), New Vector2(CSng(Core.windowSize.Width / 2 - 280), 130), Color.White)

        Dim t As String = Localization.Translate("offline_game.description")
        t = t.CropStringToWidth(FontManager.MainFont, 450)

        Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - 310) + 50, 240), Color.White)

        Dim d As New Dictionary(Of Buttons, String)
        d.Add(Buttons.A, Localization.Translate("global.accept"))
        d.Add(Buttons.B, Localization.Translate("global.cancel"))
        DrawGamePadControls(d, New Vector2(CSng(Core.windowSize.Width / 2) - 140, 420))
    End Sub

    Public Overrides Sub Update()
        If Controls.Accept(True, True, True) = True Then
            Core.SetScreen(Me.PreScreen)
        End If
        If Controls.Dismiss(True, True, True) = True Then
            Core.SetScreen(Me.PreScreen)
            Core.GameOptions.StartedOfflineGame = False
            Core.GameOptions.SaveOptions()
        End If
    End Sub

End Class