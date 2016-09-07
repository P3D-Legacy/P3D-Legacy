Public Class SplashScreen

    Inherits Screen

    Dim StartedLoad As Boolean = False
    Dim Texture() As Texture2D
    Dim Game As GameController
    Dim LoadThread As Threading.Thread

    Dim Delay As Single = 7.0F

    Public Sub New(ByVal GameReference As GameController)
        Me.Game = GameReference
        Me.CanBePaused = False
        Me.CanMuteMusic = False
        Me.CanChat = False
        Me.CanTakeScreenshot = False
        Me.CanDrawDebug = False
        Me.MouseVisible = True
        Me.CanGoFullscreen = False

        Me.Texture = {Nothing, Nothing}
        Me.Texture(0) = Core.Content.Load(Of Texture2D)("GUI\Logos\KolbenBrand")
        Me.Texture(1) = Core.Content.Load(Of Texture2D)("GUI\Logos\KolbenText")

        Me.Identification = Identifications.SplashScreen
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(164, 27, 27))

        Core.SpriteBatch.Draw(Me.Texture(0), New Rectangle(CInt(Core.windowSize.Width / 2) - CInt(Me.Texture(0).Width / 2), CInt(Core.windowSize.Height / 2) - CInt(Me.Texture(0).Height), Me.Texture(0).Width, Me.Texture(0).Height), Color.White)
        Core.SpriteBatch.Draw(Me.Texture(1), New Rectangle(CInt(Core.windowSize.Width / 2) - CInt(Me.Texture(1).Width / 2), CInt(Core.windowSize.Height / 2) - CInt(Me.Texture(1).Height / 2) + CInt(Me.Texture(0).Height / 2), Me.Texture(1).Width, Me.Texture(1).Height), Color.White)
    End Sub

    Public Overrides Sub Update()
        If StartedLoad = False Then
            LoadThread = New Threading.Thread(AddressOf LoadContent)
            LoadThread.Start()

            StartedLoad = True
        End If

        If LoadThread.IsAlive = False Then
            If Delay <= 0.0F Or GameController.IS_DEBUG_ACTIVE = True Then
                Core.GraphicsManager.ApplyChanges()

                Logger.Debug("---Loading content ready---")

                If MapPreviewScreen.MapViewMode = True Then
                    Core.SetScreen(New MapPreviewScreen())
                Else
                    Core.SetScreen(New MainMenuScreen())
                End If
                'Core.SetScreen(New TransitionScreen(Me, New IntroScreen(), Color.Black, False))
            End If
        End If

        Delay -= 0.1F
    End Sub

    Private Sub LoadContent()
        Logger.Debug("---Start loading content---")

        Core.LoadContent()
    End Sub

End Class