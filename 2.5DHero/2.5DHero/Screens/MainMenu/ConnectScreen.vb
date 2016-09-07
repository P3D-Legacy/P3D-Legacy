Public Class ConnectScreen

    Inherits Screen

    Public Enum Modes
        Connect
        Disconnect
    End Enum

    Public MyMode As Modes = Modes.Connect

    Dim message As String = ""
    Dim header As String = ""
    Dim quitToMenu As Boolean = True

    Public Shared Connected As Boolean = False

    Public Sub New(ByVal MyMode As Modes, ByVal header As String, ByVal message As String, ByVal currentScreen As Screen)
        Me.PreScreen = Core.CurrentScreen
        Me.MyMode = MyMode
        Me.message = message
        Me.header = header
        Me.PreScreen = currentScreen
        Me.MouseVisible = True

        If Not currentScreen Is Nothing Then
            Dim s As Screen = Me.PreScreen
            While Not s.PreScreen Is Nothing And s.Identification <> Identifications.OverworldScreen
                s = s.PreScreen
            End While

            If s.Identification = Identifications.OverworldScreen Then
                quitToMenu = False
            Else
                If s.Identification = Identifications.BattleScreen Then
                    quitToMenu = False
                End If
            End If
        End If

        Me.Identification = Identifications.ConnectScreen
        Me.CanBePaused = False
        Me.CanChat = False
    End Sub

    Public Overrides Sub Draw()
        Dim Tx As Integer = CInt(World.CurrentSeason)
        Dim Ty As Integer = 0
        If Tx > 1 Then
            Tx -= 2
            Ty += 1
        End If

        Dim pattern As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(160 + Tx * 16, Ty * 16, 16, 16), "")
        For Dx = 0 To Core.windowSize.Width Step 128
            For Dy = 0 To Core.windowSize.Height Step 128
                Dim c As Color = Color.White
                If Dy = 128 Then
                    c = Color.Gray
                End If

                Core.SpriteBatch.Draw(pattern, New Rectangle(Dx, Dy, 128, 128), c)
            Next
        Next

        Dim t As String = Me.message.CropStringToWidth(FontManager.MainFont, 500)

        Core.SpriteBatch.DrawString(FontManager.MainFont, Me.header, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(Me.header).X), 168), Color.White, 0.0F, New Vector2(0), 2.0F, SpriteEffects.None, 0.0F)
        Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - (FontManager.MainFont.MeasureString(t).X * 1.4F) / 2), 320), Color.White, 0.0F, New Vector2(0), 1.4F, SpriteEffects.None, 0.0F)
    End Sub

    Public Overrides Sub Update()
        If Me.MyMode = Modes.Disconnect Then
            If Controls.Accept(True, True, True) = True Or Controls.Dismiss(True, True, True) = True Then
                If quitToMenu = True Then
                    Core.SetScreen(New MainMenuScreen())
                Else
                    Core.SetScreen(Me.PreScreen)
                End If
            End If
        Else
            If Core.ServersManager.PlayerManager.ReceivedIniData() = True Then
                Connected = True
                Core.SetScreen(New OverworldScreen())
            End If
            If Controls.Dismiss() = True Then
                Connected = False
                Core.ServersManager.ServerConnection.Disconnect()
                Core.SetScreen(New MainMenuScreen())
            End If
        End If
    End Sub

    Public Overrides Sub ChangeTo()
        If Me.MyMode = Modes.Connect Then
            Dim t As New Threading.Thread(AddressOf Core.ServersManager.Connect)
            t.IsBackground = True
            t.Start(JoinServerScreen.SelectedServer)
        End If
    End Sub

    Shared TempConnectScreen As ConnectScreen
    Shared NeedToSwitch As Boolean = False

    Public Shared Sub Setup(ByVal ConnectScreen As ConnectScreen)
        TempConnectScreen = ConnectScreen
        NeedToSwitch = True
    End Sub

    Public Shared Sub UpdateConnectSet()
        If NeedToSwitch = True Then
            NeedToSwitch = False
            Core.SetScreen(TempConnectScreen)
        End If
    End Sub

End Class