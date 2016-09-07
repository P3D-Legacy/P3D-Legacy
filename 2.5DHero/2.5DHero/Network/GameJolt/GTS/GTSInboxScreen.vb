Namespace GameJolt

    Public Class GTSInboxScreen

        Inherits Screen

        Dim InboxList As New List(Of GTSDataItem)

        Dim Loaded As Boolean = False
        Dim Selected As Integer = -1
        Dim ScrollIndex As Integer = 0
        Dim Emblem As Emblem = Nothing

        Dim ResultsFound As Integer = 0
        Dim ResultsLoaded As Integer = 0
        Dim Searching As Boolean = False
        Dim Loading As Boolean = False

        Public Sub New(ByVal currentScreen As Screen)
            Me.PreScreen = currentScreen
            Me.Identification = Identifications.GTSInboxScreen

            Me.CanBePaused = False
            Me.CanChat = False
            Me.CanDrawDebug = True
            Me.CanMuteMusic = True
            Me.CanTakeScreenshot = True
            Me.MouseVisible = True

            Me.Loading = True
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

            If Searching = True And Loading = False Then
                If ResultsFound = ResultsLoaded Then
                    Searching = False
                End If
            End If

            If Core.Random.Next(0, 100) = 0 Then
                GTSMainScreen.Furrs.Add(New Furr())
            End If

            InboxList = (From d As GTSDataItem In InboxList Order By CInt(d.TradeID) Ascending).ToList()

            If Loaded = False Then
                Dim APICall As New APICall(AddressOf GotKeys)
                APICall.GetKeys(False, "GTSTradeV" & GTSMainScreen.GTSVersion & "|Got|*|" & Core.GameJoltSave.GameJoltID & "|*|*|Pokemon 3D|*|*")
                Loaded = True
                Loading = True
            Else
                If Me.InboxList.Count > 0 Then
                    For i = 0 To 5
                        If i < Me.InboxList.Count Then
                            If New Rectangle(116, 148 + i * 64, 64, 64).Contains(MouseHandler.MousePosition) = True Then
                                If Controls.Accept(True, True) = True Then
                                    If Selected = i + ScrollIndex Then
                                        Selected = -1
                                    Else
                                        Selected = i + ScrollIndex
                                        Emblem = New Emblem(InboxList(Selected).FromUserID, 0)
                                    End If
                                End If
                            End If
                        End If
                    Next

                    If InboxList.Count > 6 Then
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

                        Me.ScrollIndex = ScrollIndex.Clamp(0, Me.InboxList.Count - 6)
                    End If

                    If Selected > -1 Then
                        If New Rectangle(600, 440, 32 * 3 + 64, 32).Contains(MouseHandler.MousePosition) = True Then
                            If Controls.Accept(True, False) = True Then
                                If Core.Player.Pokemons.Count < 6 Then
                                    WithdrawCurrent()
                                End If
                            End If
                        End If
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

        Private Sub WithdrawCurrent()
            Dim APICall As New APICall(AddressOf WithdrawCompleted)
            APICall.RemoveKey(Me.InboxList(Selected).Key, False)
        End Sub

        Private Sub WithdrawCompleted(ByVal result As String)
            Dim l As List(Of API.JoltValue) = API.HandleData(result)

            If CBool(l(0).Value) = True Then
                Core.Player.Pokemons.Add(Me.InboxList(Selected).Pokemon)

                Dim pokedexType As Integer = 2
                If InboxList(Selected).Pokemon.IsShiny = True Then
                    pokedexType = 3
                End If

                If InboxList(Selected).Pokemon.IsEgg() = False Then
                    Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, InboxList(Selected).Pokemon.Number, pokedexType)
                End If

                Core.Player.SaveGame(False)

                If Me.PreScreen.Identification = Identifications.GTSMainScreen Then
                    CType(Me.PreScreen, GTSMainScreen).PokemonGTSCount -= 1
                    CType(Me.PreScreen, GTSMainScreen).InboxPokemon -= 1
                End If

                Me.InboxList.RemoveAt(Selected)

                Selected = -1
            End If
        End Sub

        Private Sub GotKeys(ByVal result As String)
            Dim l As List(Of API.JoltValue) = API.HandleData(result)

            If l(1).Value <> "" Then
                ResultsLoaded = 0
                ResultsFound = l.Count - 1
                Searching = True
                Loading = False

                For Each Item As API.JoltValue In l
                    If Item.Name.ToLower() = "key" Then
                        Dim APICall As New APICall(AddressOf GotData)
                        APICall.GetStorageData(Item.Value, False)
                    End If
                Next
            Else
                Searching = False
                Loading = False
                ResultsLoaded = 0
                ResultsFound = 0
            End If
        End Sub

        Private Sub GotData(ByVal result As String)
            Dim l As List(Of API.JoltValue) = API.HandleData(result)

            Dim data As String = l(1).Value

            Me.InboxList.Add(New GTSDataItem(data))

            ResultsLoaded += 1
        End Sub

        Private Sub DrawStringC(ByVal t As String, ByVal p As Vector2)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(p.X + 2, p.Y + 2), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, t, p, Color.White)
        End Sub

        Public Overrides Sub Draw()
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), Core.windowSize, New Rectangle(320, 176, 192, 160), Color.White)

            For Each F As Furr In GTSMainScreen.Furrs
                F.Draw()
            Next

            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(Core.windowSize.Width / 2 - 104), CInt(32), 208, 96), New Rectangle(304, 0, 208, 96), Color.White)

            Core.SpriteBatch.DrawString(FontManager.InGameFont, "Inbox", New Vector2(132, 100), Color.White)

            If InboxList.Count > 0 Then
                For i = ScrollIndex To ScrollIndex + 5
                    If i < InboxList.Count Then
                        Dim Y As Integer = 132 + (i - ScrollIndex) * 64
                        Dim D As GTSDataItem = InboxList(i)
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
                If Searching = True Or Loading = True Then
                    DrawStringC("Please wait" & LoadingDots.Dots, New Vector2(132, 160))
                Else
                    DrawStringC("There are no Pokémon in your inbox.", New Vector2(132, 160))
                End If
            End If

            If Selected > -1 Then
                Dim D As GTSDataItem = Me.InboxList(Selected)
                Canvas.DrawRectangle(New Rectangle(500, 164, 600, 352), New Color(255, 255, 255, 150))

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
                                             "Message: " & vbNewLine & D.Message, New Vector2(830, 200), Color.Black)


                'Stars:
                GTSMainScreen.DrawStars(D.Pokemon.TradeValue, New Vector2(630, 256))

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
                If Core.Player.IsGamejoltSave = True Then
                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "To:", New Vector2(516, 360), Color.Black)
                    Dim ownEmblem As Emblem = New Emblem(API.username, Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Points, Core.GameJoltSave.Gender, Core.GameJoltSave.Emblem)

                    Dim SpriteSize As New Size(CInt(ownEmblem.SpriteTexture.Width / 3), CInt(ownEmblem.SpriteTexture.Height / 4))
                    Core.SpriteBatch.Draw(ownEmblem.SpriteTexture, New Rectangle(564, 350, 32, 32), New Rectangle(0, SpriteSize.Height * 2, SpriteSize.Width, SpriteSize.Height), Color.White)
                    Core.SpriteBatch.DrawString(FontManager.MiniFont, ownEmblem.Username & " (" & ownEmblem.GameJoltID & ")", New Vector2(600, 360), Color.Black)
                End If

                'Buttons:
                If Core.Player.Pokemons.Count < 6 Then
                    DrawButton(New Vector2(600, 440), "Withdraw", 3)
                End If
            End If

            If Me.InboxList.Count > 6 Then
                Canvas.DrawScrollBar(New Vector2(90, 96 + 54), Me.InboxList.Count, 6, ScrollIndex, New Size(6, 380), False, New Color(4, 84, 157), New Color(125, 214, 234))
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

    End Class

    Public Class GTSDataItem

        Public Enum SecurityCode
            [Global]
            [Private]
        End Enum

        Public Enum ActionSwitches
            [Got]
            [Set]
        End Enum

        Public Key As String
        Public Data As String

        Public FromUserID As String = ""
        Public ToUserID As String = ""
        Public RequestID As String = ""
        Public RequestLevel As String = ""
        Public RequestItemID As String = ""
        Public RequestGender As String = ""
        Public PokemonData As String = ""
        Public GameMode As String = ""
        Public Message As String = ""
        Public SecurityArea As SecurityCode = SecurityCode.Global
        Public ActionSwitch As ActionSwitches = ActionSwitches.Set
        Public TradeID As String = ""

        Public Pokemon As Pokemon = Nothing

        Public Sub New(ByVal FromUserID As String, ByVal ToUserID As String, ByVal RequestID As String, ByVal RequestLevel As String, ByVal RequestItemID As String, ByVal RequestGender As String, ByVal PokemonData As String, ByVal GameMode As String, ByVal Message As String, ByVal SecurityArea As SecurityCode, ByVal ActionSwitch As ActionSwitches, ByVal TradeID As String)
            Me.New(FromUserID & "|" & ToUserID & "|" & RequestID & "|" & RequestLevel & "|" & RequestItemID & "|" & RequestGender & "|" & PokemonData & "|" & GameMode & "|" & Message & "|" & SecurityArea.ToString() & "|" & ActionSwitch.ToString() & "|" & TradeID)
        End Sub

        Public Sub New(ByVal Data As String)
            Me.Data = Data
            Dim arr() As String = Data.Split(CChar("|"))

            FromUserID = arr(0)
            ToUserID = arr(1)
            RequestID = arr(2)
            RequestLevel = arr(3)
            RequestItemID = arr(4)
            RequestGender = arr(5)
            PokemonData = arr(6).Replace("\""", """")
            GameMode = arr(7).Replace("[ANDAND]", "&")
            Message = arr(8).Replace("\""", """").Replace("[ANDAND]", "&")

            Dim sec As String = arr(9)
            If sec.ToLower() = "global" Then
                SecurityArea = SecurityCode.Global
            ElseIf sec.ToLower() = "private" Then
                SecurityArea = SecurityCode.Private
            End If

            Dim ac As String = arr(10)
            If ac.ToLower() = "got" Then
                ActionSwitch = ActionSwitches.Got
            ElseIf ac = "set" Then
                ActionSwitch = ActionSwitches.Set
            End If

            If PokemonData <> "" Then
                PokemonData = PokemonData.Replace("[ANDAND]", "&")

                If PokemonData.StartsWith("{") = True And PokemonData.EndsWith("}") = True Then
                    Me.Pokemon = net.Pokemon3D.Game.Pokemon.GetPokemonByData(PokemonData)
                End If
            End If

            TradeID = arr(11)

            Dim PokemonNumber As Integer = 0
            If Not Me.Pokemon Is Nothing Then
                PokemonNumber = Me.Pokemon.Number
            End If

            Me.Key = "GTSTradeV" & GTSMainScreen.GTSVersion & "|" & arr(10) & "|" & arr(0) & "|" & arr(1) & "|" & PokemonNumber.ToString() & "|" & arr(2) & "|Pokemon 3D|" & arr(9) & "|" & TradeID
        End Sub

    End Class

End Namespace