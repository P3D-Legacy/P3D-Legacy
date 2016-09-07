Namespace GameJolt

    Public Class GTSSetupScreen

        Inherits Screen

        Dim SetupList As New List(Of GTSDataItem)

        Dim loaded As Boolean = False
        Dim Selected As Integer = -1
        Dim ScrollIndex As Integer = 0
        Dim Emblem As Emblem = Nothing

        Public Sub New(ByVal currentScreen As Screen)
            Me.PreScreen = currentScreen
            Me.Identification = Identifications.GTSSetupScreen

            Me.CanBePaused = False
            Me.CanChat = False
            Me.CanDrawDebug = True
            Me.CanMuteMusic = True
            Me.CanTakeScreenshot = True
            Me.MouseVisible = True
        End Sub

        Dim TempPokemon As Pokemon = Nothing

        Private Sub DrawStringC(ByVal t As String, ByVal p As Vector2)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(p.X + 2, p.Y + 2), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, t, p, Color.White)
        End Sub

        Public Overrides Sub Draw()
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), Core.windowSize, New Rectangle(320, 176, 192, 160), Color.White)

            For Each F As Furr In GTSMainScreen.Furrs
                F.Draw()
            Next

            If Me.IsCurrentScreen() = True Then
                Core.SpriteBatch.DrawString(FontManager.InGameFont, "Setup", New Vector2(132, 100), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Core.windowSize.Width / 2 - 104), CInt(32), 208, 96), New Rectangle(304, 0, 208, 96), Color.White)
            End If

            If Working = False And Received = Lines Then
                If Me.IsCurrentScreen() = True Then
                    If SetupList.Count > 0 Then
                        For i = ScrollIndex To ScrollIndex + 5
                            If i < SetupList.Count Then
                                Dim Y As Integer = 132 + (i - ScrollIndex) * 64
                                Dim D As GTSDataItem = SetupList(i)
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
                    Else
                        DrawStringC("There are no Pokémon setup for trade.", New Vector2(132, 160))
                    End If

                    If CanSetup() = True Then
                        DrawButton(New Vector2(180, 200 + (SetupList.Count - ScrollIndex) * 64), "Setup new trade", 4)
                    End If

                    If Selected > -1 Then
                        Dim D As GTSDataItem = Me.SetupList(Selected)
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
                            Core.SpriteBatch.Draw(D.Pokemon.Item.Texture, New Rectangle(882, 314, 24, 24), Color.White)
                        End If

                        Core.SpriteBatch.DrawString(FontManager.MiniFont, "Level: " & D.Pokemon.Level & vbNewLine & vbNewLine &
                                                     "Gender: " & D.Pokemon.Gender.ToString() & vbNewLine & vbNewLine &
                                                     "OT: " & D.Pokemon.CatchTrainerName & "/" & D.Pokemon.OT & vbNewLine & vbNewLine &
                                                     "Item: " & ItemString & vbNewLine & vbNewLine &
                                                     "Message: " & vbNewLine & D.Message, New Vector2(524, 360), Color.Black)

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
                        If Core.Player.IsGamejoltSave = True Then
                            Core.SpriteBatch.DrawString(FontManager.MiniFont, "From:", New Vector2(516, 320), Color.Black)
                            Dim ownEmblem As Emblem = New Emblem(API.username, Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Points, Core.GameJoltSave.Gender, Core.GameJoltSave.Emblem)

                            Dim SpriteSize As New Size(CInt(ownEmblem.SpriteTexture.Width / 3), CInt(ownEmblem.SpriteTexture.Height / 4))
                            Core.SpriteBatch.Draw(ownEmblem.SpriteTexture, New Rectangle(564, 310, 32, 32), New Rectangle(0, SpriteSize.Height * 2, SpriteSize.Width, SpriteSize.Height), Color.White)
                            Core.SpriteBatch.DrawString(FontManager.MiniFont, ownEmblem.Username & " (" & ownEmblem.GameJoltID & ")", New Vector2(600, 320), Color.Black)
                        End If

                        'To:
                        Core.SpriteBatch.DrawString(FontManager.MiniFont, "To:", New Vector2(816, 320), Color.Black)
                        If SetupList(Selected).SecurityArea = GTSDataItem.SecurityCode.Private Then
                            If Not Emblem Is Nothing Then
                                If Emblem.DoneLoading = True Then
                                    Dim SpriteSize As New Size(CInt(Emblem.SpriteTexture.Width / 3), CInt(Emblem.SpriteTexture.Height / 4))
                                    Core.SpriteBatch.Draw(Emblem.SpriteTexture, New Rectangle(854, 310, 32, 32), New Rectangle(0, SpriteSize.Height * 2, SpriteSize.Width, SpriteSize.Height), Color.White)
                                    Core.SpriteBatch.DrawString(FontManager.MiniFont, Emblem.Username & " (" & Emblem.GameJoltID & ")", New Vector2(890, 320), Color.Black)
                                Else
                                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "Loading" & LoadingDots.Dots, New Vector2(864, 320), Color.Black)
                                End If
                            Else
                                Core.SpriteBatch.DrawString(FontManager.MiniFont, "Loading" & LoadingDots.Dots, New Vector2(864, 320), Color.Black)
                            End If
                        Else
                            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Global", New Vector2(864, 320), Color.Black)
                        End If

                        'Buttons:
                        DrawButton(New Vector2(600, 610), "Change", 3)
                        If Core.Player.Pokemons.Count < 6 Then
                            DrawButton(New Vector2(800, 610), "Withdraw", 3)
                        End If
                    End If

                    If Core.Player.Pokemons.Count > 6 Then
                        Canvas.DrawScrollBar(New Vector2(90, 96 + 54), Me.SetupList.Count, 6, ScrollIndex, New Size(6, 380), False, New Color(4, 84, 157), New Color(125, 214, 234))
                    End If
                End If
            Else
                DrawStringC("Loading" & LoadingDots.Dots, New Vector2(132, 160))
                DrawStringC("Please wait.", New Vector2(240, 160))
            End If

            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Version " & GTSMainScreen.GTSVersion, New Vector2(4, Core.windowSize.Height - 1 - FontManager.MiniFont.MeasureString("Version " & GTSMainScreen.GTSVersion).Y), Color.DarkGray)
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

        Private Function CanSetup() As Boolean
            If Me.SetupList.Count < 6 And Core.Player.CountFightablePokemon > 1 Then
                Return True
            End If
            Return False
        End Function

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

            SetupList = (From d As GTSDataItem In SetupList Order By CInt(d.TradeID) Ascending).ToList()

            If loaded = False Then
                loaded = True
                Selected = -1
                Me.SetupList.Clear()
                Working = True

                Dim APICall As New APICall(AddressOf GotKeys)
                APICall.GetKeys(False, "GTSTradeV" & GTSMainScreen.GTSVersion & "|Set|" & Core.GameJoltSave.GameJoltID & "|*|*|*|Pokemon 3D|*|*")
            Else
                If Working = False And Received = Lines Then
                    If Me.SetupList.Count > 0 Then
                        For i = 0 To 5
                            If i < Me.SetupList.Count Then
                                If New Rectangle(116, 148 + i * 64, 64, 64).Contains(MouseHandler.MousePosition) = True Then
                                    If Controls.Accept(True, True) = True Then
                                        If Selected = i + ScrollIndex Then
                                            Selected = -1
                                        Else
                                            Selected = i + ScrollIndex
                                            TempPokemon = Pokemon.GetPokemonByID(CInt(SetupList(Selected).RequestID))
                                            If SetupList(Selected).SecurityArea = GTSDataItem.SecurityCode.Private Then
                                                Emblem = New Emblem(SetupList(Selected).ToUserID, 0)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next

                        If SetupList.Count > 6 Then
                            If Controls.Up(True, True, True, True) = True Then
                                If Controls.ShiftDown() = True Then
                                    Me.ScrollIndex -= 5
                                Else
                                    Me.ScrollIndex -= 1
                                End If
                            End If
                            If Controls.Down(True, True, True, True) = True Then
                                If Controls.ShiftDown() = True Then
                                    Me.ScrollIndex += 5
                                Else
                                    Me.ScrollIndex += 1
                                End If
                            End If

                            Me.ScrollIndex = ScrollIndex.Clamp(0, Me.SetupList.Count - 6)
                        End If

                        If Selected > -1 Then
                            If New Rectangle(600, 610, 32 * 3 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                                If Controls.Accept(True, False) = True Then
                                    Core.SetScreen(New GTSEditTradeScreen(Me, SetupList(Selected)))
                                End If
                            End If
                            If New Rectangle(800, 610, 32 * 3 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                                If Controls.Accept(True, False) = True Then
                                    If Core.Player.Pokemons.Count < 6 Then
                                        Working = True
                                        FetchBack = Me.SetupList(Selected)
                                        Dim APICall As New APICall(AddressOf FetchTradeBack)
                                        APICall.RemoveKey(Me.SetupList(Selected).Key, False)
                                    End If
                                End If
                            End If
                        End If
                    End If

                    If CanSetup() = True Then
                        If New Rectangle(180, 200 + (SetupList.Count - ScrollIndex) * 64, 32 * 4 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                            If Controls.Accept(True, False) = True Then
                                Core.SetScreen(New GTSEditTradeScreen(Me, Nothing))
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
                End If
            End If
        End Sub

        Dim Lines As Integer = 0
        Dim Received As Integer = 0
        Dim Working As Boolean = True

        Dim BufferList As New Dictionary(Of String, String)

        Private Sub GotKeys(ByVal result As String)
            Dim l As List(Of API.JoltValue) = API.HandleData(result)

            Lines = l.Count - 1
            Received = 0
            Working = False

            If l(1).Value <> "" Then
                For Each Item As API.JoltValue In l
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
                Lines = 0
            End If
        End Sub

        Private Sub GotData(ByVal result As String)
            Dim l As List(Of API.JoltValue) = API.HandleData(result)

            Dim data As String = l(1).Value
            Dim D As New GTSDataItem(data)

            If BufferList.ContainsKey(D.Key) = False Then
                BufferList.Add(D.Key, data)
            End If

            Received += 1

            Me.SetupList.Add(D)
        End Sub

        Dim FetchBack As GTSDataItem = Nothing

        Private Sub FetchTradeBack(ByVal result As String)
            Core.Player.Pokemons.Add(FetchBack.Pokemon)
            Core.Player.SaveGame(False)
            loaded = False
        End Sub

        Class GTSEditTradeScreen

            Inherits Screen

            Dim D As GTSDataItem
            Dim NewTrade As Boolean = False
            Dim GTSSetupScreen As GTSSetupScreen

            Dim ToEmblem As Emblem = Nothing
            Dim TempPokemon As Pokemon = Nothing
            Dim PaidStars As Integer = 0

            Public Sub New(ByVal GTSSetupScreen As GTSSetupScreen, ByVal D As GTSDataItem)
                Me.GTSSetupScreen = GTSSetupScreen

                If Not D Is Nothing Then
                    Me.D = D
                    NewTrade = False
                    If D.RequestID <> "" Then
                        TempPokemon = Pokemon.GetPokemonByID(CInt(D.RequestID))

                        If D.SecurityArea = GTSDataItem.SecurityCode.Global Then
                            Dim v As Integer = TempPokemon.TradeValue - D.Pokemon.TradeValue

                            If v.ToString().EndsWith("5") = True Then
                                v += 5
                            End If

                            If v > 0 Then
                                PaidStars = v
                            End If
                        End If
                    End If
                Else
                    Me.D = New GTSDataItem(Core.GameJoltSave.GameJoltID, "", "", "9 and under", "", "", "", "Pokemon 3D", "", GTSDataItem.SecurityCode.Global, GTSDataItem.ActionSwitches.Set, "")
                    NewTrade = True
                End If

                Me.Identification = Identifications.GTSEditTradeScreen

                Me.CanBePaused = False
                Me.CanChat = False
                Me.CanDrawDebug = True
                Me.CanMuteMusic = True
                Me.CanTakeScreenshot = True
                Me.MouseVisible = True
            End Sub

            Public Overrides Sub Draw()
                Me.GTSSetupScreen.Draw()

                If Uploading = True Then
                    Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 200), 250, 400, 200), New Color(255, 255, 255, 150))

                    Core.SpriteBatch.DrawString(FontManager.MainFont, "Uploading" & LoadingDots.Dots, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Uploading").X / 2), 300), Color.Black)
                    If AssignedTradeID = True Then
                        Core.SpriteBatch.DrawString(FontManager.MainFont, "Transaction ID: " & NewTradeID, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Transaction ID: " & NewTradeID).X / 2), 370), Color.Black)
                    End If

                    Exit Sub
                End If

                Canvas.DrawRectangle(New Rectangle(50, 100, 320, 500), New Color(255, 255, 255, 150))
                Canvas.DrawRectangle(New Rectangle(400, 100, 750, 500), New Color(255, 255, 255, 150))

                If Not D.Pokemon Is Nothing Then
                    Core.SpriteBatch.Draw(D.Pokemon.GetTexture(True), New Rectangle(50, 120, 128, 128), Color.White)

                    If D.Pokemon.GetDisplayName() <> D.Pokemon.OriginalName Then
                        Core.SpriteBatch.DrawString(FontManager.MainFont, D.Pokemon.GetDisplayName(), New Vector2(180, 146), Color.Black)
                        Core.SpriteBatch.DrawString(FontManager.MainFont, "/" & D.Pokemon.OriginalName, New Vector2(190, 176), Color.Black)
                    Else
                        Core.SpriteBatch.DrawString(FontManager.MainFont, D.Pokemon.GetDisplayName(), New Vector2(180, 161), Color.Black)
                    End If

                    Dim ItemString As String = "None"
                    If Not D.Pokemon.Item Is Nothing Then
                        ItemString = "   " & D.Pokemon.Item.Name
                        Core.SpriteBatch.Draw(D.Pokemon.Item.Texture, New Rectangle(124, 475, 24, 24), Color.White)
                    End If

                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "Level: " & D.Pokemon.Level & vbNewLine & vbNewLine &
                                                 "Gender: " & D.Pokemon.Gender.ToString() & vbNewLine & vbNewLine &
                                                 "OT: " & D.Pokemon.CatchTrainerName & "/" & D.Pokemon.OT & vbNewLine & vbNewLine &
                                                 "Item: " & ItemString, New Vector2(74, 356), Color.Black)

                    'Stars:
                    GTSMainScreen.DrawStars(D.Pokemon.TradeValue, New Vector2(180, 222))
                End If

                DrawButton(New Vector2(100, 300), "Choose Pokémon", 5)

                If D.TradeID <> "" Then
                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "Transaction ID: " & D.TradeID, New Vector2(925, 102), Color.Black)
                End If

                Core.SpriteBatch.DrawString(FontManager.MainFont, "Request:", New Vector2(420, 140), Color.Black)

                If IsNumeric(D.RequestID) = True Then
                    DrawFilter(New Vector2(420, 200), 4, "Pokémon:", Pokemon.GetPokemonByID(CInt(D.RequestID)).GetDisplayName() & " (" & D.RequestID & ")")
                Else
                    DrawFilter(New Vector2(420, 200), 4, "Pokémon:", "")
                End If

                DrawFilter(New Vector2(420, 280), 4, "Level:", D.RequestLevel)
                DrawFilter(New Vector2(420, 360), 4, "Gender:", D.RequestGender)

                DrawFilter(New Vector2(600, 200), 4, "Area:", D.SecurityArea.ToString())

                If D.SecurityArea = GTSDataItem.SecurityCode.Private Then
                    Dim RecipientName As String = ""
                    If Not Me.ToEmblem Is Nothing Then
                        RecipientName = Me.ToEmblem.Username
                    End If

                    DrawFilter(New Vector2(600, 280), 4, "To (" & D.ToUserID & "):", RecipientName)
                End If

                If IsNumeric(D.RequestID) = True Then
                    If CInt(D.RequestID) > 0 Then
                        If TempPokemon Is Nothing OrElse TempPokemon.Number <> CInt(D.RequestID) Then
                            TempPokemon = Pokemon.GetPokemonByID(CInt(D.RequestID))
                        End If
                        Dim p As Pokemon = TempPokemon

                        Core.SpriteBatch.Draw(p.GetTexture(True), New Rectangle(800, 120, 128, 128), Color.White)
                        Core.SpriteBatch.DrawString(FontManager.MainFont, p.OriginalName, New Vector2(180 + 750, 176), Color.Black)

                        'Stars:
                        GTSMainScreen.DrawStars(p.TradeValue, New Vector2(930, 222))

                        'If Not D.Pokemon Is Nothing Then
                        '    If p.TradeValue > D.Pokemon.TradeValue And D.SecurityArea = GTSDataItem.SecurityCode.Global Then
                        '        Basic.SpriteBatch.DrawString(Basic.miniFont, "Needed stars:", New Vector2(780, 280), Color.Black)

                        '        Dim v As Integer = p.TradeValue - D.Pokemon.TradeValue

                        '        If v.ToString().EndsWith("5") = True Then
                        '            v += 5
                        '        End If

                        '        GTSMainScreen.DrawStars(v, New Vector2(780, 314))

                        '        If PaidStars > 0 Then
                        '            Basic.SpriteBatch.DrawString(Basic.miniFont, "Paid stars:", New Vector2(780, 380), Color.Black)

                        '            GTSMainScreen.DrawStars(PaidStars, New Vector2(780, 414))

                        '            Basic.SpriteBatch.DrawString(Basic.miniFont, "Additional stars:", New Vector2(970, 380), Color.Black)

                        '            If v - PaidStars > 0 Then
                        '                GTSMainScreen.DrawStars(v - PaidStars, New Vector2(970, 414))
                        '            End If
                        '        End If

                        '        Basic.SpriteBatch.DrawString(Basic.miniFont, "Your stars:", New Vector2(970, 280), Color.Black)

                        '        GTSMainScreen.DrawStars(Basic.Player.GTSStars * 10, New Vector2(970, 314))
                        '    End If
                        'End If
                    End If
                End If

                If TradeReady() = True Then
                    If NewTrade = True Then
                        If IsPresent() = True Then
                            If Not ToEmblem Is Nothing And Not D.Pokemon Is Nothing Then
                                Core.SpriteBatch.DrawString(FontManager.MiniFont, "You will send " & D.Pokemon.GetDisplayName() & " to " & vbNewLine & ToEmblem.Username & " and you cannot get it back!", New Vector2(480, 495), Color.Black)
                            End If
                            DrawButton(New Vector2(480, 540), "Send", 3)
                        Else
                            DrawButton(New Vector2(480, 540), "Setup", 3)
                        End If
                    Else
                        DrawButton(New Vector2(480, 540), "Change", 3)
                    End If
                Else
                    Dim message As String = "1 ERROR"
                    If Not D.Pokemon Is Nothing Then
                        If D.RequestID <> "" Then
                            If D.RequestLevel <> "" Then
                                If D.Pokemon.IsEgg() = False Then
                                    If D.Pokemon.HasHMMove() = False Then
                                        If D.SecurityArea = GTSDataItem.SecurityCode.Private And D.ToUserID <> "" Or D.SecurityArea = GTSDataItem.SecurityCode.Global Then
                                            Dim v As Integer = TempPokemon.TradeValue - D.Pokemon.TradeValue

                                            If v.ToString().EndsWith("5") = True Then
                                                v += 5
                                            End If

                                            If v - PaidStars > Core.Player.GTSStars * 10 Then
                                                message = "You don't have enough stars!"
                                            End If
                                        Else
                                            message = "Choose recipient!"
                                        End If
                                    Else
                                        message = "Choose a Pokémon without HM moves!"
                                    End If
                                Else
                                    message = "You can't trade an egg!"
                                End If
                            Else
                                message = "Choose request level cap!"
                            End If
                        Else
                            message = "Choose request Pokémon!"
                        End If
                    Else
                        message = "Choose own Pokémon!"
                    End If
                    Core.SpriteBatch.DrawString(FontManager.MiniFont, message, New Vector2(430, 545), Color.Black)
                End If

                DrawButton(New Vector2(700, 540), "Back", 3)
            End Sub

            Private Function IsPresent() As Boolean
                If D.SecurityArea = GTSDataItem.SecurityCode.Private And D.ToUserID <> "" And D.RequestID = "" Then
                    Return True
                End If
                Return False
            End Function

            Private Function TradeReady() As Boolean
                If Not D.Pokemon Is Nothing Then
                    If D.SecurityArea = GTSDataItem.SecurityCode.Private Then
                        If D.ToUserID <> "" Then
                            Return True
                        End If
                    Else
                        If D.RequestID <> "" And D.RequestLevel <> "" And D.Pokemon.IsEgg() = False And D.Pokemon.HasHMMove() = False Then
                            Return True

                            'Dim v As Integer = TempPokemon.TradeValue - D.Pokemon.TradeValue

                            'If v.ToString().EndsWith("5") = True Then
                            '    v += 5
                            'End If

                            'Dim HasPermission As Boolean = False
                            'For Each StaffMember As StaffProfile In StaffProfile.Staff
                            '    If StaffMember.GameJoltID = GamejoltSave.user_id Then
                            '        If StaffMember.StaffAreas.Contains(StaffProfile.StaffArea.GTSAdmin) = True Or StaffMember.StaffAreas.Contains(StaffProfile.StaffArea.GTSDaily) = True Then
                            '            HasPermission = True
                            '            Exit For
                            '        End If
                            '    End If
                            'Next

                            'If v - PaidStars <= Basic.Player.GTSStars * 10 Or D.SecurityArea = GTSDataItem.SecurityCode.Private Or HasPermission = True Then
                            '    Return True
                            'End If
                        End If
                    End If
                End If

                Return False
            End Function

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
                If touching = True And Me.IsCurrentScreen() = True Then
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

                If Uploading = False Then
                    If D.SecurityArea = GTSDataItem.SecurityCode.Private Then
                        If D.ToUserID <> "" Then
                            Dim makeNew As Boolean = False
                            If Me.ToEmblem Is Nothing Then
                                makeNew = True
                            Else
                                If CInt(Me.ToEmblem.GameJoltID) <> CInt(D.ToUserID) Then
                                    makeNew = True
                                End If
                            End If

                            If makeNew = True Then
                                ToEmblem = New Emblem(D.ToUserID, 0)
                            End If
                        End If
                    End If

                    If Controls.Accept(True, False) = True Then
                        If New Rectangle(420, 200, 5 * 32, 64).Contains(MouseHandler.MousePosition) = True Then
                            Core.SetScreen(New SelectPokemonScreen(Me, "Request"))
                        End If
                        If New Rectangle(420, 280, 5 * 32, 64).Contains(MouseHandler.MousePosition) = True Then
                            Core.SetScreen(New SelectLevelScreen(Me))
                        End If
                        If New Rectangle(420, 360, 5 * 32, 64).Contains(MouseHandler.MousePosition) = True Then
                            Core.SetScreen(New SelectGenderScreen(Me))
                        End If
                        If New Rectangle(600, 200, 5 * 32, 64).Contains(MouseHandler.MousePosition) = True Then
                            Core.SetScreen(New SelectAreaScreen(Me))
                        End If
                        If New Rectangle(600, 280, 5 * 32, 64).Contains(MouseHandler.MousePosition) = True Then
                            Core.SetScreen(New SelectFriendScreen(Me))
                        End If
                        If New Rectangle(100, 300, 32 * 5 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                            Core.SetScreen(New ChoosePokemonScreen(Me, Item.GetItemByID(5), AddressOf ChosenPokemon, "Choose Pokémon to trade", True, True, False))
                            CType(Core.CurrentScreen, ChoosePokemonScreen).CanChooseHMPokemon = False
                        End If
                        If New Rectangle(700, 540, 32 * 3 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                            Me.GTSSetupScreen.loaded = False
                            Core.SetScreen(Me.GTSSetupScreen)
                        End If
                        If TradeReady() = True AndAlso New Rectangle(480, 540, 32 * 3 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                            Uploading = True
                            AssignedTradeID = False

                            Dim APICall As New APICall(AddressOf GotTradeID)
                            APICall.GetStorageData("GTSTRADEID", False)
                        End If
                    End If

                    If Controls.Dismiss(True, True) = True Then
                        Me.GTSSetupScreen.loaded = False
                        Core.SetScreen(Me.GTSSetupScreen)
                    End If
                End If
            End Sub

            Private Sub ChosenPokemon(ByVal PokeIndex As Integer)
                If Me.PokeIndex = -1 And NewTrade = False Then
                    LoadedPokemon = D.Pokemon
                End If
                Me.PokeIndex = PokeIndex

                D.Pokemon = Core.Player.Pokemons(PokeIndex)
                D.PokemonData = D.Pokemon.GetSaveData()
            End Sub

            Dim NewTradeID As String = "1"
            Dim AssignedTradeID As Boolean = False
            Dim Uploading As Boolean = False
            Dim PokeIndex As Integer = -1
            Dim LoadedPokemon As Pokemon

            Private Sub GotTradeID(ByVal result As String)
                Dim l As List(Of API.JoltValue) = API.HandleData(result)

                If CBool(l(0).Value) = True Then
                    NewTradeID = CStr(CDbl(l(1).Value) + 1)
                Else
                    NewTradeID = "1"
                End If
                AssignedTradeID = True

                Logger.Debug("New trade set to: " & NewTradeID)

                If NewTrade = False Then
                    Dim APICall As New APICall(AddressOf KeyRemoved)
                    APICall.RemoveKey(D.Key, False)
                Else
                    KeyRemoved("success:""true""")
                End If
            End Sub

            Private Sub KeyRemoved(ByVal result As String)
                Dim l As List(Of API.JoltValue) = API.HandleData(result)

                If CBool(l(0).Value) = True Then
                    Dim APICall As New APICall()

                    If IsPresent() = True Then
                        Dim nD As New GTSDataItem(D.FromUserID, D.ToUserID, "", "", "", "", D.PokemonData, D.GameMode, "Sent as gift.", GTSDataItem.SecurityCode.Private, GTSDataItem.ActionSwitches.Got, NewTradeID)

                        APICall.SetStorageData(nD.Key, nD.Data, False)
                    Else
                        Dim nD As New GTSDataItem(D.FromUserID, D.ToUserID, D.RequestID, D.RequestLevel, D.RequestItemID, D.RequestGender, D.PokemonData, D.GameMode, D.Message, D.SecurityArea, D.ActionSwitch, NewTradeID)

                        APICall.SetStorageData(nD.Key, nD.Data, False)
                    End If

                    If PokeIndex > -1 And NewTrade = False Then
                        Core.Player.Pokemons.Add(LoadedPokemon)
                    End If

                    Dim nAPICall As New APICall(AddressOf UploadComplete)
                    nAPICall.SetStorageData("GTSTRADEID", NewTradeID, False)
                End If
            End Sub

            Private Sub UploadComplete(ByVal result As String)
                Uploading = False

                If PokeIndex > -1 Then
                    Core.Player.Pokemons.RemoveAt(PokeIndex)
                End If

                If Not TempPokemon Is Nothing Then
                    Dim v As Integer = TempPokemon.TradeValue - D.Pokemon.TradeValue

                    If v.ToString().EndsWith("5") = True Then
                        v += 5
                    End If

                    v -= PaidStars

                    If v > 0 Then
                        Core.Player.GTSStars -= CInt(v / 10)
                    End If
                End If

                Core.Player.SaveGame(False)

                Me.GTSSetupScreen.loaded = False
                Core.SetScreen(Me.GTSSetupScreen)
            End Sub

            Class SelectPokemonScreen

                Inherits Screen

                Dim GTSEditTradeScreen As GTSEditTradeScreen
                Dim Mode As String = "Request"
                Dim Page As Integer = 0
                Dim CurrentPokemon As New SortedDictionary(Of Integer, String)
                Dim SpriteList As New List(Of Texture2D)

                Shared TempOfferPage As Integer = 0
                Shared TempRequestPage As Integer = 0

                Public Sub New(ByVal GTSEditTradeScreen As GTSEditTradeScreen, ByVal Mode As String)
                    Me.GTSEditTradeScreen = GTSEditTradeScreen
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
                    Me.GTSEditTradeScreen.Draw()
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
                                        Me.GTSEditTradeScreen.D.RequestID = CurrentPokemon.Keys(i).ToString()
                                    ElseIf Mode = "Offer" Then
                                        Me.GTSEditTradeScreen.D.RequestID = CurrentPokemon.Keys(i).ToString()
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
                                Me.GTSEditTradeScreen.D.RequestID = ""
                            ElseIf Mode = "Offer" Then
                                Me.GTSEditTradeScreen.D.RequestID = ""
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
                    Core.SetScreen(Me.GTSEditTradeScreen)
                End Sub

            End Class

            Class SelectLevelScreen

                Inherits Screen

                Dim GTSEditTradeScreen As GTSEditTradeScreen

                Public Sub New(ByVal GTSEditTradeScreen As GTSEditTradeScreen)
                    Me.GTSEditTradeScreen = GTSEditTradeScreen
                    Me.Identification = Identifications.GTSSelectLevelScreen

                    Me.CanBePaused = False
                    Me.CanChat = False
                    Me.CanDrawDebug = True
                    Me.CanMuteMusic = True
                    Me.CanTakeScreenshot = True
                    Me.MouseVisible = True
                End Sub

                Public Overrides Sub Draw()
                    Me.GTSEditTradeScreen.Draw()
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
                                Me.GTSEditTradeScreen.D.RequestLevel = newSetting
                                Core.SetScreen(Me.GTSEditTradeScreen)
                            End If
                        Next

                        If New Rectangle(900, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If
                    End If

                    If Controls.Dismiss(True, True) = True Then
                        Core.SetScreen(Me.GTSEditTradeScreen)
                    End If
                End Sub

            End Class

            Class SelectGenderScreen

                Inherits Screen

                Dim GTSEditTradeScreen As GTSEditTradeScreen

                Public Sub New(ByVal GTSEditTradeScreen As GTSEditTradeScreen)
                    Me.GTSEditTradeScreen = GTSEditTradeScreen
                    Me.Identification = Identifications.GTSSelectGenderScreen

                    Me.CanBePaused = False
                    Me.CanChat = False
                    Me.CanDrawDebug = True
                    Me.CanMuteMusic = True
                    Me.CanTakeScreenshot = True
                    Me.MouseVisible = True
                End Sub

                Public Overrides Sub Draw()
                    Me.GTSEditTradeScreen.Draw()
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
                            Me.GTSEditTradeScreen.D.RequestGender = "Male"
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If

                        If New Rectangle(260, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            Me.GTSEditTradeScreen.D.RequestGender = "Female"
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If

                        If New Rectangle(420, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            Me.GTSEditTradeScreen.D.RequestGender = "Genderless"
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If

                        If New Rectangle(580, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            Me.GTSEditTradeScreen.D.RequestGender = ""
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If

                        If New Rectangle(900, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If
                    End If

                    If Controls.Dismiss(True, True) = True Then
                        Core.SetScreen(Me.GTSEditTradeScreen)
                    End If
                End Sub

            End Class

            Class SelectAreaScreen

                Inherits Screen

                Dim GTSEditTradeScreen As GTSEditTradeScreen

                Public Sub New(ByVal GTSEditTradeScreen As GTSEditTradeScreen)
                    Me.GTSEditTradeScreen = GTSEditTradeScreen
                    Me.Identification = Identifications.GTSSelectAreaScreen

                    Me.CanBePaused = False
                    Me.CanChat = False
                    Me.CanDrawDebug = True
                    Me.CanMuteMusic = True
                    Me.CanTakeScreenshot = True
                    Me.MouseVisible = True
                End Sub

                Public Overrides Sub Draw()
                    Me.GTSEditTradeScreen.Draw()
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
                            Me.GTSEditTradeScreen.D.SecurityArea = GTSDataItem.SecurityCode.Global
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If

                        If New Rectangle(260, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            Me.GTSEditTradeScreen.D.SecurityArea = GTSDataItem.SecurityCode.Private
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If

                        If New Rectangle(900, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If
                    End If

                    If Controls.Dismiss(True, True) = True Then
                        Core.SetScreen(Me.GTSEditTradeScreen)
                    End If
                End Sub

            End Class

            Class SelectFriendScreen

                Inherits Screen

                Dim GTSEditTradeScreen As GTSEditTradeScreen

                Shared Users As New Dictionary(Of Integer, String)

                Dim UserIDs As New Dictionary(Of Integer, Emblem)
                Dim Page As Integer = 0

                Public Shared Sub Clear()
                    Users.Clear()
                End Sub

                Public Sub New(ByVal GTSEditTradeScreen As GTSEditTradeScreen)
                    Me.GTSEditTradeScreen = GTSEditTradeScreen
                    Me.Identification = Identifications.GTSSelectUserScreen

                    Me.CanBePaused = False
                    Me.CanChat = False
                    Me.CanDrawDebug = True
                    Me.CanMuteMusic = True
                    Me.CanTakeScreenshot = True
                    Me.MouseVisible = True

                    If Core.GameJoltSave.Friends <> "" Then
                        For Each fr As String In Core.GameJoltSave.Friends.Split(CChar(","))
                            If Users.ContainsKey(CInt(fr)) = False Then
                                UserIDs.Add(CInt(fr), New Emblem(fr, 0))
                            End If
                        Next
                    End If
                End Sub

                Public Overrides Sub Draw()
                    Me.GTSEditTradeScreen.Draw()
                    Canvas.DrawRectangle(Core.windowSize, New Color(255, 255, 255, 150))

                    For i = 0 To 19
                        If i + Page * 20 < Users.Count Then
                            Dim x As Integer = i
                            Dim y As Integer = 0
                            While x > 4
                                x -= 5
                                y += 1
                            End While

                            Dim UserID As String = Users.Keys(i + Page * 20).ToString()
                            Dim UserName As String = Users.Values(i + Page * 20)

                            DrawButton(New Vector2(100 + x * 160, 200 + y * 100), 4, "User (" & UserID & ")", UserName)
                        End If
                    Next

                    DrawButton(New Vector2(900, 200), 4, "Navigation", "Last Page")
                    DrawButton(New Vector2(900, 300), 4, "Navigation", "Next Page")
                    DrawButton(New Vector2(900, 400), 4, "Friend", "No entry")
                    DrawButton(New Vector2(900, 500), 4, "Navigation", "Back")
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

                    For t = 0 To UserIDs.Count - 1
                        If Users.ContainsKey(UserIDs.Keys(t)) = False Then
                            If UserIDs.Values(t).DoneLoading = True Then
                                Users.Add(UserIDs.Keys(t), UserIDs.Values(t).Username)
                            End If
                        End If
                    Next

                    If Controls.Accept(True, False) = True Then
                        For i = 0 To 19
                            If i + Page * 20 < Users.Count Then
                                Dim x As Integer = i
                                Dim y As Integer = 0
                                While x > 4
                                    x -= 5
                                    y += 1
                                End While

                                If New Rectangle(100 + x * 160, 200 + y * 100, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                                    Me.GTSEditTradeScreen.D.ToUserID = Users.Keys(i + Page * 20).ToString()
                                    Core.SetScreen(Me.GTSEditTradeScreen)
                                End If
                            End If
                        Next
                        If New Rectangle(900, 200, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            If Page > 0 Then
                                Page -= 1
                            End If
                        End If
                        If New Rectangle(900, 300, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            If Users.Count - Page * 20 > 20 Then
                                Page += 1
                            End If
                        End If
                        If New Rectangle(900, 400, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            Me.GTSEditTradeScreen.D.ToUserID = ""
                            Me.GTSEditTradeScreen.ToEmblem = Nothing
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If
                        If New Rectangle(900, 500, 32 * 4, 64).Contains(MouseHandler.MousePosition) = True Then
                            Core.SetScreen(Me.GTSEditTradeScreen)
                        End If
                    End If

                    Users = (From entry In Users Order By entry.Value Ascending).ToDictionary(Function(pair) pair.Key, Function(pair) pair.Value)

                    If Controls.Dismiss(True, True) = True Then
                        Core.SetScreen(Me.GTSEditTradeScreen)
                    End If
                End Sub

            End Class

        End Class

    End Class

End Namespace