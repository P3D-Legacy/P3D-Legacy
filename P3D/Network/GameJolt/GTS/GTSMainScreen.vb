Namespace GameJolt

    Public Class GTSMainScreen

        Inherits Screen

        Public Shared GTSVersion As String = "1.X"
        Public Shared GTSPokemon As New List(Of Integer)
        Shared LoadedVersionData As Boolean = False
        Public Shared Furrs As New List(Of Furr)

        Dim State As Integer = 0
        Dim menuIndex As Integer = 0

        Public PokemonGTSCount As Integer = -1
        Public InboxPokemon As Integer = 0

        Public Sub New(ByVal currentScreen As Screen)
            Me.PreScreen = currentScreen
            Me.Identification = Identifications.GTSMainScreen

            Me.CanBePaused = False
            Me.CanChat = False
            Me.CanDrawDebug = True
            Me.CanMuteMusic = True
            Me.CanTakeScreenshot = True
            Me.MouseVisible = True

            Dim t As New Threading.Thread(AddressOf GetVersionData)
            t.IsBackground = True
            t.Start()
        End Sub

        Public Overrides Sub ChangeTo()
            PokemonGTSCount = -1

            MusicManager.PlayMusic("gts", True)

            If GTSVersion <> "1.X" Then
                GetGTSPokemonInfo()
            End If
        End Sub

        Private Sub GetGTSPokemonInfo()
            Dim APICall As New APICall(AddressOf GotCount)
            APICall.GetKeys(False, "GTSTradeV" & GTSMainScreen.GTSVersion & "|*|*|*|*|*|Pokemon 3D|*|*")

            Dim APICallInbox As New APICall(AddressOf GotInbox)
            APICallInbox.GetKeys(False, "GTSTradeV" & GTSMainScreen.GTSVersion & "|Got|*|" & Core.GameJoltSave.GameJoltID & "|*|*|Pokemon 3D|*|*")
        End Sub

        Private Sub GotInbox(ByVal result As String)
            Dim l As List(Of API.JoltValue) = API.HandleData(result)

            If l(1).Value <> "" Then
                Me.InboxPokemon = l.Count - 1
            Else
                Me.InboxPokemon = 0
            End If
        End Sub

        Private Sub GotCount(ByVal result As String)
            Dim l As List(Of API.JoltValue) = API.HandleData(result)

            If l(1).Value <> "" Then
                Me.PokemonGTSCount = l.Count - 1
            Else
                Me.PokemonGTSCount = 0
            End If
        End Sub

        Private Sub GetVersionData()
            Dim w As New System.Net.WebClient
            Dim data As String = w.DownloadString("https://raw.githubusercontent.com/P3D-Legacy/P3D-Legacy-Data/master/GTSVersion.dat")
            Dim lines() As String = data.SplitAtNewline()
            GTSVersion = lines(0)

            If GTSVersion <> "1.X" And PokemonGTSCount = -1 Then
                GetGTSPokemonInfo()
            End If

            Dim pokemonData() As String = lines(1).Split(CChar(","))
            For Each p As String In pokemonData
                If p.Contains("-") = True Then
                    Dim startPokemon As Integer = CInt(p.Substring(0, p.IndexOf("-")))
                    Dim endPokemon As Integer = CInt(p.Substring(p.IndexOf("-") + 1))

                    For i = startPokemon To endPokemon
                        If GTSPokemon.Contains(CInt(i)) = False Then
                            GTSPokemon.Add(CInt(i))
                        End If
                    Next
                Else
                    If GTSPokemon.Contains(CInt(p)) = False Then
                        GTSPokemon.Add(CInt(p))
                    End If
                End If
            Next

            LoadedVersionData = True
        End Sub

        Public Overrides Sub Draw()
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), Core.windowSize, New Rectangle(320, 176, 192, 160), Color.White)

            For Each F As Furr In Furrs
                F.Draw()
            Next

            Select Case State
                Case 0
                    DrawIntro()
                Case 1
                    DrawMain()
            End Select

            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Version " & GTSVersion, New Vector2(4, Core.windowSize.Height - 1 - FontManager.MiniFont.MeasureString("Version " & GTSVersion).Y), Color.DarkGray)

            If PokemonGTSCount > 0 Then
                Dim countString As String = "Pokémon in the GTS: " & PokemonGTSCount
                Dim sSize As Vector2 = FontManager.MiniFont.MeasureString(countString)
                Core.SpriteBatch.DrawString(FontManager.MiniFont, countString, New Vector2(Core.windowSize.Width - 8 - sSize.X, Core.windowSize.Height - 1 - sSize.Y), Color.DarkGray)
            End If
        End Sub

        Private Sub DrawIntro()
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Core.windowSize.Width / 2 - 152), CInt(GlobeY), 304, 304), New Rectangle(0, 32, 304, 304), Color.White)

            If GlobeY = 200.0F And LoadedVersionData = True Then
                Core.SpriteBatch.DrawString(FontManager.MainFont, "Click to enter!", New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Click to enter!").X / 2) + 2, 442), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MainFont, "Click to enter!", New Vector2(CInt(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Click to enter!").X / 2), 440), Color.White)

                Core.SpriteBatch.DrawString(FontManager.MiniFont, "The ""Pokémon GTS"" is not affiliated with Nintendo or GameFreak.", New Vector2(1, 1), Color.Gray)
            End If

            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Core.windowSize.Width / 2 - 104), CInt(-422 - GlobeY), 208, 96), New Rectangle(304, 0, 208, 96), Color.White)
        End Sub

        Private Sub DrawMain()
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Core.windowSize.Width / 2 - 104), CInt(32), 208, 96), New Rectangle(304, 0, 208, 96), Color.White)

            Dim CanvasTexture As Texture2D

            For i = 0 To 3
                If i = menuIndex Then
                    CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
                Else
                    CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
                End If

                Dim Text As String = ""
                Select Case i
                    Case 0
                        Text = "Inbox"
                    Case 1
                        Text = "Search"
                    Case 2
                        Text = "Setup"
                    Case 3
                        Text = "Exit"
                End Select

                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 180, 160 + i * 128, 320, 64))
                Core.SpriteBatch.DrawString(FontManager.InGameFont, Text, New Vector2(CInt(Core.windowSize.Width / 2) - (FontManager.InGameFont.MeasureString(Text).X / 2) - 10, 196 + i * 128), Color.Black)
            Next

            If Me.InboxPokemon > 0 Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Core.windowSize.Width / 2) + 144, 156, 32, 32), New Rectangle(320, 144, 32, 32), Color.White)
            End If
        End Sub

        Public Overrides Sub Update()
            For i = 0 To Furrs.Count - 1
                If i < Furrs.Count Then
                    Dim f As Furr = Furrs(i)
                    If f.IsOutOfBorder() = True Then
                        Furrs.Remove(f)
                        i -= 1
                    Else
                        f.Update()
                    End If
                End If
            Next

            If Core.Random.Next(0, 100) = 0 Then
                Furrs.Add(New Furr())
            End If

            Select Case Me.State
                Case 0
                    UpdateIntro()
                Case 1
                    UpdateMain()
            End Select

            If Controls.Dismiss(True, True) = True Then
                Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.White, False))
            End If
        End Sub

        Private GlobeY As Single = 200.0F
        Private GlobeSpeed As Single = 0.5F

        Private Sub UpdateIntro()
            If LoadedVersionData = True Then
                If GlobeY = 200.0F Then
                    If Controls.Accept(True, True) = True Then
                        GlobeY = 199.9F
                    End If
                Else
                    GlobeY -= GlobeSpeed
                    GlobeSpeed += 0.1F
                    If GlobeY <= -454.0F Then
                        GlobeY = -304.0F
                        State = 1
                    End If
                End If
            End If
        End Sub

        Private Sub UpdateMain()
            If Controls.Up(True, True) = True Then
                Me.menuIndex -= 1
            End If
            If Controls.Down(True, True) = True Then
                Me.menuIndex += 1
            End If

            For i = 0 To 3
                If New Rectangle(CInt(Core.windowSize.Width / 2) - 180, 160 + i * 128, 320 + 32, 64 + 32).Contains(MouseHandler.MousePosition) = True Then
                    Me.menuIndex = i

                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        Select Case Me.menuIndex
                            Case 0
                                InboxButton()
                            Case 1
                                SearchButton()
                            Case 2
                                SetupButton()
                            Case 3
                                ExitButton()
                        End Select
                    End If
                End If
            Next

            menuIndex = CInt(MathHelper.Clamp(menuIndex, 0, 3))

            If Controls.Accept(False, True) = True Then
                Select Case Me.menuIndex
                    Case 0
                        InboxButton()
                    Case 1
                        SearchButton()
                    Case 2
                        SetupButton()
                    Case 3
                        ExitButton()
                End Select
            End If
        End Sub

        Private Sub InboxButton()
            Core.SetScreen(New GTSInboxScreen(Me))
        End Sub

        Private Sub SearchButton()
            Core.SetScreen(New GTSSearchScreen(Me))
        End Sub

        Private Sub SetupButton()
            Core.SetScreen(New GTSSetupScreen(Me))
        End Sub

        Private Sub ExitButton()
            Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.White, False))
        End Sub

        Public Shared Sub DrawStars(ByVal Value As Integer, ByVal Position As Vector2)
            Dim stars As Integer = Value
            Dim miniStar As Boolean = False
            If stars >= 10 Then
                If CStr(stars).EndsWith("5") = True Then
                    stars -= 5
                    miniStar = True
                End If
                stars = CInt(stars / 10)
            Else
                stars = 0
                miniStar = True
            End If

            For i = 1 To stars
                Dim Y As Integer = CInt(Position.Y)
                Dim X As Integer = CInt(Position.X) + (i - 1) * 16
                Dim IND As Integer = i

                While IND > 5
                    Y += 18
                    X -= 5 * 16
                    IND -= 5
                End While

                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(X, Y, 16, 16), New Rectangle(384, 96, 16, 16), Color.White)
            Next
            If miniStar = True Then
                Dim Y As Integer = CInt(Position.Y)
                Dim X As Integer = CInt(Position.X) + stars * 16
                Dim IND As Integer = stars

                While IND > 4
                    Y += 18
                    X -= 5 * 16
                    IND -= 5
                End While

                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(X, Y, 16, 16), New Rectangle(368, 96, 16, 16), Color.White)
            End If
        End Sub

    End Class

    Public Class Furr

        Dim Size As Single = 1.0F
        Public Position As Vector2 = New Vector2(0)
        Dim Speed As Single = 1.0F

        Public Sub New()
            'Spawn

            Me.Size = Core.Random.Next(250, 1100) / 10.0F

            If Core.windowSize.Width > 0 Then
                If Core.Random.Next(0, 2) = 0 Then
                    Me.Speed = -(Core.Random.Next(2, 20) / 10.0F)
                    Me.Position = New Vector2(Core.windowSize.Width, Core.Random.Next(0, CInt(Core.windowSize.Height - Me.Size)))
                Else
                    Me.Speed = (Core.Random.Next(2, 20) / 10.0F)
                    Me.Position = New Vector2(-Me.Size, Core.Random.Next(0, CInt(Core.windowSize.Height - Me.Size)))
                End If
            End If
        End Sub

        Public Sub New(ByVal _size As Single, ByVal _position As Vector2, ByVal _speed As Single)
            Me.Size = _size
            Me.Position = _position
            Me.Speed = _speed
        End Sub

        Public Sub Update()
            Me.Position.X += Me.Speed
        End Sub

        Public Sub Draw()
            If Me.IsOutOfBorder() = False Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Position.X), CInt(Position.Y), CInt(Me.Size), CInt(Me.Size)), New Rectangle(320, 96, 48, 48), Color.White)
            End If
        End Sub

        Public Function IsOutOfBorder() As Boolean
            If Core.windowSize.Width = 0 Then
                Return True
            End If
            If Me.Speed < 0 Then
                If Me.Position.X < 0.0F - Me.Size Then
                    Return True
                End If
            Else
                If Me.Position.X > Core.windowSize.Width Then
                    Return True
                End If
            End If

            Return False
        End Function

    End Class

End Namespace