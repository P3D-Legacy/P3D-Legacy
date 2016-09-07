Namespace GameJolt

    Public Class GTSSearchScreen

        Inherits Screen

        Dim LevelFilter As String = ""
        Dim RequestFilter As String = ""
        Dim OfferFilter As String = ""
        Dim GenderFilter As String = ""
        Dim AreaFilter As String = ""

        Dim Selected As Integer = -1
        Dim ScrollIndex As Integer = 0

        Dim SearchResults As New List(Of GTSDataItem)

        Public Sub New(ByVal currentScreen As Screen)
            Me.PreScreen = currentScreen
            Me.Identification = Identifications.GTSSearchScreen

            Me.CanBePaused = False
            Me.CanChat = False
            Me.CanDrawDebug = True
            Me.CanMuteMusic = True
            Me.CanTakeScreenshot = True
            Me.MouseVisible = True
        End Sub

        Public Overrides Sub ChangeTo()
            If Me.AreaFilter = "" Then
                Core.SetScreen(New SelectAreaScreen(Me))
            End If
        End Sub

        Public Overrides Sub Draw()
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), Core.windowSize, New Rectangle(320, 176, 192, 160), Color.White)

            For Each F As Furr In GTSMainScreen.Furrs
                F.Draw()
            Next

            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(32, CInt(32), 208, 96), New Rectangle(304, 0, 208, 96), Color.White)

            DrawFilters()
            DrawButton(New Vector2(1000, 64), "Search", 3)

            DrawMain()

            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Version " & GTSMainScreen.GTSVersion, New Vector2(4, Core.windowSize.Height - 1 - FontManager.MiniFont.MeasureString("Version " & GTSMainScreen.GTSVersion).Y), Color.DarkGray)
        End Sub

        Private Sub DrawFilters()
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Area: " & Me.AreaFilter, New Vector2(280, 4), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Offer:", New Vector2(280, 26), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Request:", New Vector2(792, 26), Color.White)

            DrawFilter(New Vector2(280, 48), 4, "Pokémon:", OfferFilter)
            DrawFilter(New Vector2(472, 48), 3, "Level:", LevelFilter)
            DrawFilter(New Vector2(632, 48), 3, "Gender:", GenderFilter)
            DrawFilter(New Vector2(792, 48), 4, "Pokémon:", RequestFilter)
        End Sub

        Private Sub DrawFilter(ByVal Position As Vector2, ByVal Size As Integer, ByVal Label As String, ByVal Text As String)
            Dim TexX As Integer = 368
            If New Rectangle(CInt(Position.X), CInt(Position.Y), (Size + 1) * 32, 64).Contains(MouseHandler.MousePosition) = True And Me.IsCurrentScreen() = True Then
                TexX = 400
            End If

            For i = 0 To Size - 1
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Position.X + i * 32), CInt(Position.Y), 32, 64), New Rectangle(TexX, 112, 16, 32), Color.White)
            Next
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Position.X + Size * 32), CInt(Position.Y), 32, 64), New Rectangle(TexX + 16, 112, 16, 32), Color.White)

            Core.SpriteBatch.DrawString(FontManager.MiniFont, Label, New Vector2(Position.X + 4, Position.Y + 4), New Color(100, 100, 100))
            Core.SpriteBatch.DrawString(FontManager.MiniFont, Text, New Vector2(Position.X + 4, Position.Y + 32), Color.Black)
        End Sub

        Private Sub DrawButton(ByVal Position As Vector2, ByVal Text As String, ByVal Size As Integer)
            Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\GTS")

            Dim touching As Boolean = New Rectangle(CInt(Position.X), CInt(Position.Y), 64 + Size * 32, 32).Contains(MouseHandler.MousePosition)

            Dim Y As Integer = 0
            If touching = True Then
                Y = 16
            End If

            Core.SpriteBatch.Draw(t, New Rectangle(CInt(Position.X), CInt(Position.Y), 32, 32), New Rectangle(0, Y, 16, 16), Color.White)

            For i = 1 To Size
                Core.SpriteBatch.Draw(t, New Rectangle(CInt(Position.X + i * 32), CInt(Position.Y), 32, 32), New Rectangle(16, Y, 16, 16), Color.White)
            Next

            Core.SpriteBatch.Draw(t, New Rectangle(CInt(Position.X + Size * 32 + 32), CInt(Position.Y), 32, 32), New Rectangle(32, Y, 16, 16), Color.White)

            Dim sizeX As Integer = Size * 32 + 64
            Dim TextSizeX As Integer = CInt(FontManager.MiniFont.MeasureString(Text).X)

            Core.SpriteBatch.DrawString(FontManager.MiniFont, Text, New Vector2(CSng(Position.X + sizeX / 2 - TextSizeX / 2 - 2), Position.Y + 4), Color.Black)
        End Sub

        Private Sub DrawStringC(ByVal t As String, ByVal p As Vector2)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(p.X + 2, p.Y + 2), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, t, p, Color.White)
        End Sub

        Dim TempPokemon As Pokemon = Nothing
        Dim Emblem As Emblem = Nothing

        Private Sub DrawMain()
            If ShowingResults = False Then
                If Searching = True Then
                    DrawStringC("Searching" & LoadingDots.Dots & " (" & LoadedResults & " / " & FoundResults & ")", New Vector2(100, 160))
                Else
                    DrawStringC("Enter a search pattern above and click ""Search"" to begin trading!", New Vector2(100, 160))
                End If
            Else
                If FoundResults = 0 Or SearchResults.Count = 0 Then
                    DrawStringC("No results found! Try to change the search pattern.", New Vector2(100, 160))
                Else
                    If SearchResults.Count > 0 Then
                        For i = ScrollIndex To ScrollIndex + 5
                            If i < SearchResults.Count Then
                                Dim Y As Integer = 132 + i * 64
                                Dim D As GTSDataItem = SearchResults(i)
                                Dim Touching As Boolean = New Rectangle(116, Y + 16, 64, 64).Contains(MouseHandler.MousePosition)
                                Dim C As Color = New Color(255, 255, 255, 150)
                                If i = Me.Selected Then
                                    C = New Color(0, 217, 237)
                                End If
                                If Touching = True Then
                                    C = Color.White
                                End If
                                If i = Me.Selected Then
                                    Canvas.DrawRectangle(New Rectangle(142, Y + 32, 358, 32), C)
                                Else
                                    Canvas.DrawRectangle(New Rectangle(142, Y + 32, 320, 32), C)
                                End If
                                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(100, Y, 96, 96), New Rectangle(320, 96, 48, 48), Color.White)
                                Core.SpriteBatch.Draw(D.Pokemon.GetMenuTexture(), New Rectangle(116, Y + 16, 64, 64), C)

                                Core.SpriteBatch.DrawString(FontManager.MiniFont, D.Pokemon.GetDisplayName(), New Vector2(198, Y + 37), Color.Black)
                                Core.SpriteBatch.DrawString(FontManager.MiniFont, "Lv. " & D.Pokemon.Level, New Vector2(360, Y + 37), Color.Black)
                            End If
                        Next
                    End If

                    If Selected > -1 Then
                        Dim D As GTSDataItem = Me.SearchResults(Selected)
                        Canvas.DrawRectangle(New Rectangle(500, 164, 600, 500), New Color(255, 255, 255, 150))

                        If D.TradeID <> "" Then
                            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Transaction ID: " & D.TradeID, New Vector2(510, 166), Color.Black)
                        End If

                        'Pokemon image/data:
                        Core.SpriteBatch.Draw(D.Pokemon.GetTexture(True), New Rectangle(500, 164, 128, 128), Color.White)

                        If D.Pokemon.GetDisplayName() <> D.Pokemon.OriginalName Then
                            Core.SpriteBatch.DrawString(FontManager.MainFont, D.Pokemon.GetDisplayName(), New Vector2(630, 190), Color.Black)
                            Core.SpriteBatch.DrawString(FontManager.MainFont, "/" & D.Pokemon.OriginalName, New Vector2(640, 220), Color.Black)
                        Else
                            Core.SpriteBatch.DrawString(FontManager.MainFont, D.Pokemon.GetDisplayName(), New Vector2(630, 205), Color.Black)
                        End If

                        Dim ItemString As String = "None"
                        If Not D.Pokemon.Item Is Nothing Then
                            ItemString = "   " & D.Pokemon.Item.Name
                            Core.SpriteBatch.Draw(D.Pokemon.Item.Texture, New Rectangle(575, 478, 24, 24), Color.White)
                        End If

                        Core.SpriteBatch.DrawString(FontManager.MiniFont, "Level: " & D.Pokemon.Level & vbNewLine & vbNewLine &
                                                     "Gender: " & D.Pokemon.Gender.ToString() & vbNewLine & vbNewLine &
                                                     "OT: " & D.Pokemon.CatchTrainerName & "/" & D.Pokemon.OT & vbNewLine & vbNewLine &
                                                     "Item: " & ItemString, New Vector2(524, 360), Color.Black)

                        'Stars:
                        GTSMainScreen.DrawStars(D.Pokemon.TradeValue, New Vector2(630, 256))

                        'RequestPokemon:
                        Dim p As Pokemon = TempPokemon

                        Core.SpriteBatch.Draw(p.GetTexture(True), New Rectangle(800, 164, 128, 128), Color.White)

                        If D.Pokemon.GetDisplayName() <> p.OriginalName Then
                            Core.SpriteBatch.DrawString(FontManager.MainFont, p.GetDisplayName(), New Vector2(930, 190), Color.Black)
                            Core.SpriteBatch.DrawString(FontManager.MainFont, "/" & p.OriginalName, New Vector2(940, 220), Color.Black)
                        Else
                            Core.SpriteBatch.DrawString(FontManager.MainFont, p.GetDisplayName(), New Vector2(930, 205), Color.Black)
                        End If

                        Core.SpriteBatch.DrawString(FontManager.MiniFont, "Request:" & vbNewLine & vbNewLine &
                                                     "Number: " & D.RequestID & vbNewLine & vbNewLine &
                                                     "Level: " & D.RequestLevel & vbNewLine & vbNewLine &
                                                     "Gender: " & D.RequestGender, New Vector2(824, 360), Color.Black)

                        'Stars:
                        GTSMainScreen.DrawStars(p.TradeValue, New Vector2(930, 256))

                        'From:
                        Core.SpriteBatch.DrawString(FontManager.MiniFont, "From:", New Vector2(516, 320), Color.Black)
                        If Not Emblem Is Nothing Then
                            If Emblem.DoneLoading = True Then
                                Dim SpriteSize As New Size(CInt(Emblem.SpriteTexture.Width / 3), CInt(Emblem.SpriteTexture.Height / 4))
                                Core.SpriteBatch.Draw(Emblem.SpriteTexture, New Rectangle(564, 310, 32, 32), New Rectangle(0, SpriteSize.Height * 2, SpriteSize.Width, SpriteSize.Height), Color.White)
                                Core.SpriteBatch.DrawString(FontManager.MiniFont, Emblem.Username & " (" & Emblem.GameJoltID & ")", New Vector2(600, 320), Color.Black)
                            Else
                                Core.SpriteBatch.DrawString(FontManager.MiniFont, "Loading" & LoadingDots.Dots, New Vector2(564, 320), Color.Black)
                            End If
                        Else
                            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Loading" & LoadingDots.Dots, New Vector2(564, 320), Color.Black)
                        End If

                        'To:
                        Core.SpriteBatch.DrawString(FontManager.MiniFont, "To:", New Vector2(816, 320), Color.Black)
                        If SearchResults(Selected).SecurityArea = GTSDataItem.SecurityCode.Private Then
                            Dim ownEmblem As Emblem = New Emblem(API.username, Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Points, Core.GameJoltSave.Gender, Core.GameJoltSave.Emblem)

                            Dim SpriteSize As New Size(CInt(ownEmblem.SpriteTexture.Width / 3), CInt(ownEmblem.SpriteTexture.Height / 4))
                            Core.SpriteBatch.Draw(ownEmblem.SpriteTexture, New Rectangle(864, 310, 32, 32), New Rectangle(0, SpriteSize.Height * 2, SpriteSize.Width, SpriteSize.Height), Color.White)
                            Core.SpriteBatch.DrawString(FontManager.MiniFont, ownEmblem.Username & " (" & ownEmblem.GameJoltID & ")", New Vector2(900, 320), Color.Black)
                        Else
                            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Global", New Vector2(864, 320), Color.Black)
                        End If

                        'Buttons:
                        DrawButton(New Vector2(600, 610), "Trade", 3)
                        If Me.AreaFilter = "Private" Then
                            'DrawButton(New Vector2(800, 610), "Refuse", 3)
                        End If
                    End If
                End If
            End If
        End Sub

        Public Overrides Sub Update()
            For i = 0 To GTSMainScreen.Furrs.Count - 1
                If i < GTSMainScreen.Furrs.Count Then
                    Dim f As Furr = GTSMainScreen.Furrs(i)
                    If f.IsOutOfBorder() = True Then
                        GTSMainScreen.Furrs.Remove(f)
                        i -= 1
                    Else
                        f.Update()
                    End If
                End If
            Next

            If Core.Random.Next(0, 100) = 0 Then
                GTSMainScreen.Furrs.Add(New Furr())
            End If

            If Searching = True And ShowingResults = False Then
                If LoadedResults = FoundResults Then
                    Searching = False
                    ShowingResults = True

                    Dim newL As New List(Of GTSDataItem)
                    newL.AddRange(SearchResults.ToArray())

                    SearchResults.Clear()

                    While newL.Count > 0
                        Dim i As Integer = Core.Random.Next(0, newL.Count)

                        SearchResults.Add(newL(i))
                        newL.RemoveAt(i)
                    End While
                End If
            End If

            If SearchResults.Count > 0 Then
                For i = 0 To 5
                    If i < Me.SearchResults.Count Then
                        If New Rectangle(116, 148 + i * 64, 64, 64).Contains(MouseHandler.MousePosition) = True Then
                            If Controls.Accept(True, True) = True Then
                                If Selected = i + ScrollIndex Then
                                    Selected = -1
                                Else
                                    Selected = i + ScrollIndex
                                    TempPokemon = Pokemon.GetPokemonByID(CInt(SearchResults(Selected).RequestID))
                                    Emblem = New Emblem(SearchResults(Selected).FromUserID, 0)
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            If Controls.Accept(True, False) = True Then
                If New Rectangle(280, 48, 5 * 32, 64).Contains(MouseHandler.MousePosition) = True Then
                    Core.SetScreen(New SelectPokemonScreen(Me, "Offer"))
                End If
                If New Rectangle(472, 48, 4 * 32, 64).Contains(MouseHandler.MousePosition) = True Then
                    Core.SetScreen(New SelectLevelScreen(Me))
                End If
                If New Rectangle(632, 48, 4 * 32, 64).Contains(MouseHandler.MousePosition) = True Then
                    Core.SetScreen(New SelectGenderScreen(Me))
                End If
                If New Rectangle(792, 48, 5 * 32, 64).Contains(MouseHandler.MousePosition) = True Then
                    Core.SetScreen(New SelectPokemonScreen(Me, "Request"))
                End If
                If New Rectangle(1000, 64, 32 * 3 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                    'If (OfferFilter <> "" And LevelFilter <> "") = True Or AreaFilter = "Private" Then
                    Dim APICall As New APICall(AddressOf GotKeys)

                    Dim RequestPattern As String = "*"
                    If Me.RequestFilter <> "" Then
                        RequestPattern = RequestFilter.Remove(RequestFilter.IndexOf(")"))
                        RequestPattern = RequestPattern.Remove(0, RequestPattern.IndexOf("(") + 1)
                    End If

                    Dim OfferPattern As String = "*"
                    If Me.OfferFilter <> "" Then
                        OfferPattern = OfferFilter.Remove(OfferFilter.IndexOf(")"))
                        OfferPattern = OfferPattern.Remove(0, OfferPattern.IndexOf("(") + 1)
                    End If

                    Dim AreaPattern As String = Me.AreaFilter

                    APICall.GetKeys(False, "GTSTradeV" & GTSMainScreen.GTSVersion & "|Set|*|*|" & OfferPattern & "|" & RequestPattern & "|Pokemon 3D|" & AreaPattern & "|*")

                    Searching = False
                    ShowingResults = False
                    FoundResults = 0
                    LoadedResults = 0
                    Selected = -1
                    ScrollIndex = 0
                    'End If
                End If
                If New Rectangle(600, 610, 32 * 3 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                    If Selected > -1 Then
                        Core.SetScreen(New GTSTradeScreen(Me, SearchResults(Selected)))
                    End If
                End If
            End If

            If Controls.Dismiss(True, True) = True Then
                If Selected > -1 Then
                    Selected = -1
                Else
                    Core.SetScreen(Me.PreScreen)
                End If
            End If
        End Sub

        Shared BufferList As New Dictionary(Of String, String)

        Dim LoadedResults As Integer = 0
        Dim FoundResults As Integer = 0

        Dim Searching As Boolean = False
        Dim ShowingResults As Boolean = False

        Private Sub GotKeys(ByVal result As String)
            Dim l As List(Of API.JoltValue) = API.HandleData(result)

            If l(1).Value <> "" Then
                If GenderFilter = "" And OfferFilter = "" And RequestFilter = "" And LevelFilter = "" Then
                    While l.Count > 7
                        l.RemoveAt(Core.Random.Next(1, l.Count))
                    End While
                End If

                Me.SearchResults.Clear()
                Searching = True
                FoundResults = l.Count - 1
                LoadedResults = 0

                For i = 0 To l.Count - 1
                    Dim Item As API.JoltValue = l(i)
                    If Item.Name.ToLower() = "key" Then
                        If BufferList.ContainsKey(Item.Value) = True Then
                            GotData("success:""true""" & vbNewLine & "data:""" & BufferList(Item.Value) & """")
                        Else
                            Dim APICall As New APICall(AddressOf GotData)
                            APICall.GetStorageData(Item.Value, False)
                        End If
                    End If
                Next
            Else
                Me.SearchResults.Clear()
                FoundResults = 0
                LoadedResults = 0
                Searching = False
                ShowingResults = True
            End If
        End Sub

        Private Sub GotData(ByVal result As String)
            LoadedResults += 1

            Dim l As List(Of API.JoltValue) = API.HandleData(result)

            Dim data As String = l(1).Value
            Dim D As New GTSDataItem(data)

            If BufferList.ContainsKey(D.Key) = False Then
                BufferList.Add(D.Key, data)
            End If

            Dim levelMax As Integer = 9
            Dim levelMin As Integer = 0

            If Me.LevelFilter = "" Then
                levelMin = 0
                levelMax = 100
            Else
                If Me.LevelFilter <> "9 and under" Then
                    levelMax = CInt(Me.LevelFilter.Remove(0, Me.LevelFilter.IndexOf(" - ") + 3))
                    levelMin = CInt(Me.LevelFilter.Remove(Me.LevelFilter.IndexOf(" ")))
                End If
            End If

            If (D.Pokemon.Level <= levelMax And D.Pokemon.Level >= levelMin) = True Or AreaFilter = "Private" And D.FromUserID <> Core.GameJoltSave.GameJoltID Then
                Dim hasGender As Boolean = False
                Select Case Me.GenderFilter
                    Case "Male"
                        If D.Pokemon.Gender = Pokemon.Genders.Male Then
                            hasGender = True
                        End If
                    Case "Female"
                        If D.Pokemon.Gender = Pokemon.Genders.Female Then
                            hasGender = True
                        End If
                    Case "Genderless"
                        If D.Pokemon.Gender = Pokemon.Genders.Genderless Then
                            hasGender = True
                        End If
                    Case ""
                        hasGender = True
                End Select

                If hasGender = True Then
                    If Me.AreaFilter = "Private" And D.ToUserID = Core.GameJoltSave.GameJoltID Or Me.AreaFilter = "Global" Then
                        Me.SearchResults.Add(D)
                    End If
                End If
            End If
        End Sub

        Class SelectLevelScreen

            Inherits Screen

            Dim GTSSearchScreen As GTSSearchScreen

            Public Sub New(ByVal GTSSearchScreen As GTSSearchScreen)
                Me.GTSSearchScreen = GTSSearchScreen
                Me.Identification = Identifications.GTSSelectLevelScreen

                Me.CanBePaused = False
                Me.CanChat = False
                Me.CanDrawDebug = True
                Me.CanMuteMusic = True
                Me.CanTakeScreenshot = True
                Me.MouseVisible = True
            End Sub

            Public Overrides Sub Draw()
                Me.GTSSearchScreen.Draw()
                Canvas.DrawRectangle(Core.windowSize, New Color(255, 255, 255, 150))

                DrawButton(New Vector2(100, 200), 4, "Level", "9 and under")
                DrawButton(New Vector2(260, 200), 4, "Level", "10 - 19")
                DrawButton(New Vector2(420, 200), 4, "Level", "20 - 29")
                DrawButton(New Vector2(580, 200), 4, "Level", "30 - 39")
                DrawButton(New Vector2(740, 200), 4, "Level", "40 - 49")
                DrawButton(New Vector2(100, 300), 4, "Level", "50 - 59")
                DrawButton(New Vector2(260, 300), 4, "Level", "60 - 69")
                DrawButton(New Vector2(420, 300), 4, "Level", "70 - 79")
                DrawButton(New Vector2(580, 300), 4, "Level", "80 - 89")
                DrawButton(New Vector2(740, 300), 4, "Level", "90 - 100")

                DrawButton(New Vector2(900, 200), 4, "Navigation", "Back")
            End Sub

            Private Sub DrawButton(ByVal Position As Vector2, ByVal Size As Integer, ByVal Label As String, ByVal Text As String)
                Dim TexX As Integer = 368
                If New Rectangle(CInt(Position.X), CInt(Position.Y), Size * 32, 64).Contains(MouseHandler.MousePosition) = True And Me.IsCurrentScreen() = True Then
                    TexX = 400
                End If

                For i = 0 To Size - 1
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Position.X + i * 32), CInt(Position.Y), 32, 64), New Rectangle(TexX, 112, 16, 32), Color.White)
                Next

                Core.SpriteBatch.DrawString(FontManager.MiniFont, Label, New Vector2(Position.X + 4, Position.Y + 4), New Color(100, 100, 100))
                Core.SpriteBatch.DrawString(FontManager.MiniFont, Text, New Vector2(Position.X + 4, Position.Y + 32), Color.Black)
            End Sub

            Public Overrides Sub Update()
                For i = 0 To GTSMainScreen.Furrs.Count - 1
                    If i < GTSMainScreen.Furrs.Count Then
                        Dim f As Furr = GTSMainScreen.Furrs(i)
                        If f.IsOutOfBorder() = True Then
                            GTSMainScreen.Furrs.Remove(f)
                            i -= 1
                        Else
                            f.Update()
                        End If
                    End If
                Next

                If Core.Random.Next(0, 100) = 0 Then
                    GTSMainScreen.Furrs.Add(New Furr())
                End If

                If Controls.Accept(True, False) = True Then
                    For i = 0 To 9
                        Dim x As Integer = i
                        Dim y As Integer = 0
                        If x > 4 Then
                            x -= 5
                            y += 1
                        End If
                        If New Rectangle(100 + x * 160, 200 + y * 100, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            Dim newSetting As String = "9 and under"
                            Select Case i
                                Case 0
                                    newSetting = "9 and under"
                                Case 1
                                    newSetting = "10 - 19"
                                Case 2
                                    newSetting = "20 - 29"
                                Case 3
                                    newSetting = "30 - 39"
                                Case 4
                                    newSetting = "40 - 49"
                                Case 5
                                    newSetting = "50 - 59"
                                Case 6
                                    newSetting = "60 - 69"
                                Case 7
                                    newSetting = "70 - 79"
                                Case 8
                                    newSetting = "80 - 89"
                                Case 9
                                    newSetting = "90 - 100"
                            End Select
                            Me.GTSSearchScreen.LevelFilter = newSetting
                            Core.SetScreen(Me.GTSSearchScreen)
                        End If
                    Next

                    If New Rectangle(900, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        Core.SetScreen(Me.GTSSearchScreen)
                    End If
                End If

                If Controls.Dismiss(True, True) = True Then
                    Core.SetScreen(Me.GTSSearchScreen)
                End If
            End Sub

        End Class

        Class SelectPokemonScreen

            Inherits Screen

            Dim GTSSearchScreen As GTSSearchScreen
            Dim Mode As String = "Request"
            Dim Page As Integer = 0
            Dim CurrentPokemon As New SortedDictionary(Of Integer, String)
            Dim SpriteList As New List(Of Texture2D)

            Shared TempOfferPage As Integer = 0
            Shared TempRequestPage As Integer = 0

            Public Sub New(ByVal GTSSearchScreen As GTSSearchScreen, ByVal Mode As String)
                Me.GTSSearchScreen = GTSSearchScreen
                Me.Identification = Identifications.GTSSelectPokemonScreen
                Me.Mode = Mode

                Me.CanBePaused = False
                Me.CanChat = False
                Me.CanDrawDebug = True
                Me.CanMuteMusic = True
                Me.CanTakeScreenshot = True
                Me.MouseVisible = True

                If Me.Mode = "Request" Then
                    Me.Page = TempRequestPage
                ElseIf Me.Mode = "Offer" Then
                    Me.Page = TempOfferPage
                End If

                GetPokemon()
            End Sub

            Private Sub GetPokemon()
                CurrentPokemon.Clear()
                SpriteList.Clear()

                Dim index As Integer = Page * 20
                Dim noMorePokemon As Boolean = False

                Dim fileList As New List(Of Integer)
                Dim d As List(Of String) = System.IO.Directory.GetFiles(GameController.GamePath & "\Content\Pokemon\Data\").ToList()
                For Each file As String In d
                    Dim fileName As String = System.IO.Path.GetFileNameWithoutExtension(file)
                    If IsNumeric(fileName) = True Then
                        If CInt(fileName) > 0 And CInt(fileName) <= Pokedex.POKEMONCOUNT Then
                            If GTSMainScreen.GTSPokemon.Contains(CInt(fileName)) = True Then
                                fileList.Add(CInt(fileName))
                            End If
                        End If
                    End If
                Next
                fileList.Sort()

                While CurrentPokemon.Count < 20 And noMorePokemon = False
                    If index <= fileList.Count - 1 Then
                        Dim fileName As Integer = fileList(index)
                        Dim p As Pokemon = Pokemon.GetPokemonByID(fileName)
                        CurrentPokemon.Add(p.Number, p.OriginalName)
                        SpriteList.Add(p.GetMenuTexture())
                        index += 1
                    Else
                        noMorePokemon = True
                    End If
                End While
            End Sub

            Public Overrides Sub Draw()
                Me.GTSSearchScreen.Draw()
                Canvas.DrawRectangle(Core.windowSize, New Color(255, 255, 255, 150))

                For i = 0 To 19
                    If i < CurrentPokemon.Count Then
                        Dim x As Integer = i
                        Dim y As Integer = 0
                        While x > 4
                            x -= 5
                            y += 1
                        End While

                        Dim Number As String = CurrentPokemon.Keys(i).ToString()
                        While Number.Length < 3
                            Number = "0" & Number
                        End While

                        DrawButton(New Vector2(100 + x * 160, 200 + y * 100), 4, "Pokémon " & Number, CurrentPokemon.Values(i), SpriteList(i))
                    End If
                Next

                DrawButton(New Vector2(900, 200), 4, "Navigation", "Last Page", Nothing)
                DrawButton(New Vector2(900, 300), 4, "Navigation", "Next Page", Nothing)
                DrawButton(New Vector2(900, 400), 4, "Pokémon", "No entry", Nothing)
                DrawButton(New Vector2(900, 500), 4, "Navigation", "Back", Nothing)
            End Sub

            Private Sub DrawButton(ByVal Position As Vector2, ByVal Size As Integer, ByVal Label As String, ByVal Text As String, ByVal Texture As Texture2D)
                Dim TexX As Integer = 368
                If New Rectangle(CInt(Position.X), CInt(Position.Y), Size * 32, 64).Contains(MouseHandler.MousePosition) = True And Me.IsCurrentScreen() = True Then
                    TexX = 400
                End If

                For i = 0 To Size - 1
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Position.X + i * 32), CInt(Position.Y), 32, 64), New Rectangle(TexX, 112, 16, 32), Color.White)
                Next

                If Not Texture Is Nothing Then
                    Dim s As New Size(Size * 32, 64)
                    Core.SpriteBatch.Draw(Texture, New Rectangle(CInt(s.Width / 2 - Texture.Width + Position.X), CInt(s.Height / 2 - Texture.Height + Position.Y), Texture.Width * 2, Texture.Height * 2), New Color(255, 255, 255, 100))
                End If

                Core.SpriteBatch.DrawString(FontManager.MiniFont, Label, New Vector2(Position.X + 4, Position.Y + 4), New Color(100, 100, 100))
                Core.SpriteBatch.DrawString(FontManager.MiniFont, Text, New Vector2(Position.X + 4, Position.Y + 32), Color.Black)
            End Sub

            Public Overrides Sub Update()
                For i = 0 To GTSMainScreen.Furrs.Count - 1
                    If i < GTSMainScreen.Furrs.Count Then
                        Dim f As Furr = GTSMainScreen.Furrs(i)
                        If f.IsOutOfBorder() = True Then
                            GTSMainScreen.Furrs.Remove(f)
                            i -= 1
                        Else
                            f.Update()
                        End If
                    End If
                Next

                If Core.Random.Next(0, 100) = 0 Then
                    GTSMainScreen.Furrs.Add(New Furr())
                End If

                If Controls.Accept(True, False) = True Then
                    For i = 0 To 19
                        If i < CurrentPokemon.Count Then
                            Dim x As Integer = i
                            Dim y As Integer = 0
                            While x > 4
                                x -= 5
                                y += 1
                            End While

                            If New Rectangle(100 + x * 160, 200 + y * 100, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                                Dim newSetting As String = CurrentPokemon.Values(i) & " (" & CurrentPokemon.Keys(i).ToString() & ")"

                                If Mode = "Request" Then
                                    Me.GTSSearchScreen.RequestFilter = newSetting
                                ElseIf Mode = "Offer" Then
                                    Me.GTSSearchScreen.OfferFilter = newSetting
                                End If

                                Close()
                            End If
                        End If
                    Next

                    If New Rectangle(900, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        If Me.Page > 0 Then
                            Me.Page -= 1
                            GetPokemon()
                            If Me.Mode = "Request" Then
                                TempRequestPage = Me.Page
                            ElseIf Me.Mode = "Offer" Then
                                TempOfferPage = Me.Page
                            End If
                        End If
                    End If
                    If New Rectangle(900, 300, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        If CurrentPokemon.Count = 20 Then
                            Me.Page += 1
                            GetPokemon()
                            If Me.Mode = "Request" Then
                                TempRequestPage = Me.Page
                            ElseIf Me.Mode = "Offer" Then
                                TempOfferPage = Me.Page
                            End If
                        End If
                    End If
                    If New Rectangle(900, 400, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        If Mode = "Request" Then
                            Me.GTSSearchScreen.RequestFilter = ""
                        ElseIf Mode = "Offer" Then
                            Me.GTSSearchScreen.OfferFilter = ""
                        End If

                        Close()
                    End If
                    If New Rectangle(900, 500, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        Close()
                    End If
                End If

                If Controls.Dismiss(True, True) = True Then
                    Close()
                End If
            End Sub

            Private Sub Close()
                If Me.Mode = "Request" Then
                    TempRequestPage = Me.Page
                ElseIf Me.Mode = "Offer" Then
                    TempOfferPage = Me.Page
                End If
                Core.SetScreen(Me.GTSSearchScreen)
            End Sub

        End Class

        Class SelectGenderScreen

            Inherits Screen

            Dim GTSSearchScreen As GTSSearchScreen

            Public Sub New(ByVal GTSSearchScreen As GTSSearchScreen)
                Me.GTSSearchScreen = GTSSearchScreen
                Me.Identification = Identifications.GTSSelectGenderScreen

                Me.CanBePaused = False
                Me.CanChat = False
                Me.CanDrawDebug = True
                Me.CanMuteMusic = True
                Me.CanTakeScreenshot = True
                Me.MouseVisible = True
            End Sub

            Public Overrides Sub Draw()
                Me.GTSSearchScreen.Draw()
                Canvas.DrawRectangle(Core.windowSize, New Color(255, 255, 255, 150))

                DrawButton(New Vector2(100, 200), 4, "Gender", "Male")
                DrawButton(New Vector2(260, 200), 4, "Gender", "Female")
                DrawButton(New Vector2(420, 200), 4, "Gender", "Genderless")
                DrawButton(New Vector2(580, 200), 4, "Gender", "No entry")

                DrawButton(New Vector2(900, 200), 4, "Navigation", "Back")
            End Sub

            Private Sub DrawButton(ByVal Position As Vector2, ByVal Size As Integer, ByVal Label As String, ByVal Text As String)
                Dim TexX As Integer = 368
                If New Rectangle(CInt(Position.X), CInt(Position.Y), Size * 32, 64).Contains(MouseHandler.MousePosition) = True And Me.IsCurrentScreen() = True Then
                    TexX = 400
                End If

                For i = 0 To Size - 1
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Position.X + i * 32), CInt(Position.Y), 32, 64), New Rectangle(TexX, 112, 16, 32), Color.White)
                Next

                Core.SpriteBatch.DrawString(FontManager.MiniFont, Label, New Vector2(Position.X + 4, Position.Y + 4), New Color(100, 100, 100))
                Core.SpriteBatch.DrawString(FontManager.MiniFont, Text, New Vector2(Position.X + 4, Position.Y + 32), Color.Black)
            End Sub

            Public Overrides Sub Update()
                For i = 0 To GTSMainScreen.Furrs.Count - 1
                    If i < GTSMainScreen.Furrs.Count Then
                        Dim f As Furr = GTSMainScreen.Furrs(i)
                        If f.IsOutOfBorder() = True Then
                            GTSMainScreen.Furrs.Remove(f)
                            i -= 1
                        Else
                            f.Update()
                        End If
                    End If
                Next

                If Core.Random.Next(0, 100) = 0 Then
                    GTSMainScreen.Furrs.Add(New Furr())
                End If

                If Controls.Accept(True, False) = True Then
                    If New Rectangle(100, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        Me.GTSSearchScreen.GenderFilter = "Male"
                        Core.SetScreen(Me.GTSSearchScreen)
                    End If

                    If New Rectangle(260, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        Me.GTSSearchScreen.GenderFilter = "Female"
                        Core.SetScreen(Me.GTSSearchScreen)
                    End If

                    If New Rectangle(420, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        Me.GTSSearchScreen.GenderFilter = "Genderless"
                        Core.SetScreen(Me.GTSSearchScreen)
                    End If

                    If New Rectangle(580, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        Me.GTSSearchScreen.GenderFilter = ""
                        Core.SetScreen(Me.GTSSearchScreen)
                    End If

                    If New Rectangle(900, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        Core.SetScreen(Me.GTSSearchScreen)
                    End If
                End If

                If Controls.Dismiss(True, True) = True Then
                    Core.SetScreen(Me.GTSSearchScreen)
                End If
            End Sub

        End Class

        Class SelectAreaScreen

            Inherits Screen

            Dim GTSSearchScreen As GTSSearchScreen

            Public Sub New(ByVal GTSSearchScreen As GTSSearchScreen)
                Me.GTSSearchScreen = GTSSearchScreen
                Me.Identification = Identifications.GTSSelectAreaScreen

                Me.CanBePaused = False
                Me.CanChat = False
                Me.CanDrawDebug = True
                Me.CanMuteMusic = True
                Me.CanTakeScreenshot = True
                Me.MouseVisible = True
            End Sub

            Public Overrides Sub Draw()
                Me.GTSSearchScreen.Draw()
                Canvas.DrawRectangle(Core.windowSize, New Color(255, 255, 255, 150))

                DrawButton(New Vector2(100, 200), 4, "Area", "Global")
                DrawButton(New Vector2(260, 200), 4, "Area", "Private")

                DrawButton(New Vector2(900, 200), 4, "Navigation", "Back")
            End Sub

            Private Sub DrawButton(ByVal Position As Vector2, ByVal Size As Integer, ByVal Label As String, ByVal Text As String)
                Dim TexX As Integer = 368
                If New Rectangle(CInt(Position.X), CInt(Position.Y), Size * 32, 64).Contains(MouseHandler.MousePosition) = True And Me.IsCurrentScreen() = True Then
                    TexX = 400
                End If

                For i = 0 To Size - 1
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Position.X + i * 32), CInt(Position.Y), 32, 64), New Rectangle(TexX, 112, 16, 32), Color.White)
                Next

                Core.SpriteBatch.DrawString(FontManager.MiniFont, Label, New Vector2(Position.X + 4, Position.Y + 4), New Color(100, 100, 100))
                Core.SpriteBatch.DrawString(FontManager.MiniFont, Text, New Vector2(Position.X + 4, Position.Y + 32), Color.Black)
            End Sub

            Public Overrides Sub Update()
                For i = 0 To GTSMainScreen.Furrs.Count - 1
                    If i < GTSMainScreen.Furrs.Count Then
                        Dim f As Furr = GTSMainScreen.Furrs(i)
                        If f.IsOutOfBorder() = True Then
                            GTSMainScreen.Furrs.Remove(f)
                            i -= 1
                        Else
                            f.Update()
                        End If
                    End If
                Next

                If Core.Random.Next(0, 100) = 0 Then
                    GTSMainScreen.Furrs.Add(New Furr())
                End If

                If Controls.Accept(True, False) = True Then
                    If New Rectangle(100, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        Me.GTSSearchScreen.AreaFilter = "Global"
                        Core.SetScreen(Me.GTSSearchScreen)
                    End If

                    If New Rectangle(260, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        Me.GTSSearchScreen.AreaFilter = "Private"
                        Core.SetScreen(Me.GTSSearchScreen)
                    End If

                    If New Rectangle(900, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                        Core.SetScreen(Me.GTSSearchScreen.PreScreen)
                    End If
                End If

                If Controls.Dismiss(True, True) = True Then
                    Core.SetScreen(Me.GTSSearchScreen.PreScreen)
                End If
            End Sub

        End Class

    End Class

End Namespace