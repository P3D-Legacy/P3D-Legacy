Public Class MailSystemScreen

    Inherits Screen

    Dim index As Integer = -1
    Dim scrollIndex As Integer = 0
    Dim selectIndex As Integer = 0

    Dim message As String = ""

    Public Sub New(ByVal currentScreen As Screen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.MailSystemScreen

        Me.MouseVisible = True
        Me.CanBePaused = True
        Me.CanMuteAudio = False
        Me.CanChat = False
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, Color.White)

        Core.SpriteBatch.DrawString(FontManager.MainFont, "Mailbox", New Vector2(42, 28), Color.Black)
        Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 2, New Rectangle(32, 64, 320, 576))

        Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 2, New Rectangle(400, 64, 672, 576))

        For i = scrollIndex To scrollIndex + 8
            If i = 0 Then
                DrawMail(Nothing, New Vector2(46, 82 + (i - scrollIndex) * 64), i)
            Else
                If i <= Core.Player.Mails.Count Then
                    DrawMail(Core.Player.Mails(i - 1), New Vector2(46, 82 + (i - scrollIndex) * 64), i)
                End If
            End If
        Next

        Canvas.DrawScrollBar(New Vector2(358, 86), Core.Player.Mails.Count + 1, 9, scrollIndex, New Size(6, 560), False, Color.LightGray, Color.Black)

        If Me.index <> -1 Then
            DrawCurrentMail()
        End If

        If message <> "" Then
            Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))
            Dim t As String = message.CropStringToWidth(FontManager.MainFont, 800)

            Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), 340), Color.White)
        End If
    End Sub

    Private Sub DrawMail(ByVal mail As Items.MailItem.MailData, ByVal P As Vector2, ByVal i As Integer)
        If i = 0 Then
            Dim x As Integer = 0
            Dim y As Integer = 0
            If i = Me.index Then
                y = 48
            Else
                If i = selectIndex Then
                    y = 0
                    x = 48
                End If
            End If

            Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(x, y, 48, 48), ""), 1, New Rectangle(CInt(P.X), CInt(P.Y), 288, 32))
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Write new mail.", New Vector2(CInt(P.X) + 13, CInt(P.Y) + 14), Color.Black)
        Else
            Dim item As Item = Item.GetItemByID(mail.MailID.ToString)

            Dim x As Integer = 0
            Dim y As Integer = 0
            If i = Me.index Then
                y = 48
            Else
                If i = selectIndex Then
                    y = 0
                    x = 48
                End If
            End If

            Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(x, y, 48, 48), ""), 1, New Rectangle(CInt(P.X), CInt(P.Y), 288, 32))
            Core.SpriteBatch.Draw(item.Texture, New Rectangle(CInt(P.X), CInt(P.Y), 48, 48), Color.White)

            Core.SpriteBatch.DrawString(FontManager.MainFont, mail.MailHeader, New Vector2(CInt(P.X) + 52, CInt(P.Y) + 14), Color.Black)

            If mail.MailAttachment > -1 Then
                Dim t As TrophyInformation = GetTrophyInformation(mail.MailAttachment)
                Core.SpriteBatch.Draw(t.Texture, New Rectangle(CInt(P.X) + 250, CInt(P.Y) + 8, 32, 32), Color.White)
            End If

            If mail.MailRead = False Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(P.X) + 272, CInt(P.Y), 32, 32), New Rectangle(320, 144, 32, 32), Color.White)
            End If
        End If
    End Sub

    Dim TempNewMail As New Items.MailItem.MailData
    Dim EditMailIndex As Integer = 0

    Private Sub DrawCurrentMail()
        If index = 0 Then
            Dim mail As Items.MailItem.MailData = TempNewMail
            Dim item As Item = Item.GetItemByID(mail.MailID.ToString)

            Core.SpriteBatch.Draw(item.Texture, New Rectangle(420, 84, 48, 48), Color.White)

            Dim c As Color = Color.Gray

            If EditMailIndex = 0 Then
                c = Color.Blue
            Else
                c = Color.Gray
            End If

            Core.SpriteBatch.DrawString(FontManager.MainFont, "Header: " & mail.MailHeader, New Vector2(480, 92), c)

            Canvas.DrawRectangle(New Rectangle(420, 140, 660, 2), Color.DarkGray)

            Dim text As String = ("Text: (" & mail.MailText.Length & "/" & 200 & ")" & Environment.NewLine & Environment.NewLine & mail.MailText.Replace("<br>", Environment.NewLine)).CropStringToWidth(FontManager.MainFont, 600)
            If EditMailIndex = 1 Then
                c = Color.Blue
                text &= "_"
            Else
                c = Color.Gray
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, text, New Vector2(430, 160), c)

            Dim yPlus As Integer = CInt(FontManager.MainFont.MeasureString(text).Y)

            If EditMailIndex = 2 Then
                c = Color.Blue
            Else
                c = Color.Gray
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Signature: " & mail.MailSignature, New Vector2(430, yPlus + 200), c)

            Canvas.DrawRectangle(New Rectangle(420, yPlus + 240, 660, 2), Color.DarkGray)

            Core.SpriteBatch.DrawString(FontManager.MainFont, "Mail OT: " & mail.MailSender & " (" & mail.MailOriginalTrainerOT & ")", New Vector2(430, yPlus + 260), Color.Black)

            If EditMailIndex = 3 Then
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), ""), 1, New Rectangle(440, yPlus + 320, 160, 32))
            Else
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 1, New Rectangle(440, yPlus + 320, 160, 32))
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Attach", New Vector2(CInt(524 - FontManager.MainFont.MeasureString("Attach").X / 2), yPlus + CInt(344 - FontManager.MainFont.MeasureString("Attach").Y / 2)), Color.Black)

            If EditMailIndex = 4 Then
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), ""), 1, New Rectangle(640, yPlus + 320, 160, 32))
            Else
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 1, New Rectangle(640, yPlus + 320, 160, 32))
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Cancel", New Vector2(CInt(724 - FontManager.MainFont.MeasureString("Cancel").X / 2), yPlus + CInt(344 - FontManager.MainFont.MeasureString("Cancel").Y / 2)), Color.Black)
        Else
            Dim mail As Items.MailItem.MailData = Core.Player.Mails(index - 1)
            Dim item As Item = Item.GetItemByID(mail.MailID.ToString)

            Core.SpriteBatch.Draw(item.Texture, New Rectangle(420, 84, 48, 48), Color.White)

            Core.SpriteBatch.DrawString(FontManager.MainFont, mail.MailHeader, New Vector2(480, 92), Color.Black)

            Canvas.DrawRectangle(New Rectangle(420, 140, 660, 2), Color.DarkGray)

            Dim text As String = mail.MailText.CropStringToWidth(FontManager.MainFont, 600)
            Core.SpriteBatch.DrawString(FontManager.MainFont, text, New Vector2(430, 160), Color.Black)

            Dim yPlus As Integer = CInt(FontManager.MainFont.MeasureString(text).Y)

            Core.SpriteBatch.DrawString(FontManager.MainFont, mail.MailSignature, New Vector2(430, yPlus + 200), Color.Black)

            Canvas.DrawRectangle(New Rectangle(420, yPlus + 240, 660, 2), Color.DarkGray)

            Core.SpriteBatch.DrawString(FontManager.MainFont, "Mail OT: " & mail.MailSender & " (" & mail.MailOriginalTrainerOT & ")", New Vector2(430, yPlus + 260), Color.Black)

            If mail.MailAttachment > -1 Then
                Canvas.DrawRectangle(New Rectangle(420, yPlus + 300, 660, 2), Color.DarkGray)

                Dim t As TrophyInformation = GetTrophyInformation(mail.MailAttachment)
                Core.SpriteBatch.DrawString(FontManager.MainFont, "Trophy:", New Vector2(430, yPlus + 320), Color.Black)
                Core.SpriteBatch.Draw(t.Texture, New Rectangle(430, yPlus + 340, 64, 64), Color.White)
                Core.SpriteBatch.DrawString(FontManager.MainFont, (t.Name & Environment.NewLine & Environment.NewLine & t.Description).CropStringToWidth(FontManager.MainFont, 500), New Vector2(510, yPlus + 340), Color.Black)
            End If

            If EditMailIndex = 0 Then
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), ""), 1, New Rectangle(440, yPlus + 320, 160, 32))
            Else
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 1, New Rectangle(440, yPlus + 320, 160, 32))
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Attach", New Vector2(CInt(524 - FontManager.MainFont.MeasureString("Attach").X / 2), yPlus + CInt(344 - FontManager.MainFont.MeasureString("Attach").Y / 2)), Color.Black)

            If EditMailIndex = 1 Then
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), ""), 1, New Rectangle(640, yPlus + 320, 160, 32))
            Else
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 1, New Rectangle(640, yPlus + 320, 160, 32))
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Cancel", New Vector2(CInt(724 - FontManager.MainFont.MeasureString("Cancel").X / 2), yPlus + CInt(344 - FontManager.MainFont.MeasureString("Cancel").Y / 2)), Color.Black)
        End If
    End Sub

    Public Overrides Sub Update()
        If message <> "" Then
            If Controls.Accept(True, True, True) = True Or Controls.Dismiss(True, True, True) = True Then
                message = ""
            End If

            Exit Sub
        End If

        If index <> 0 Then
            If index > -1 Then

                Dim pressedSystemKey As Boolean = False
                If Controls.Down(True, True, True, False, True) = True Or KeyBoardHandler.KeyPressed(Keys.Tab) = True And Controls.ShiftDown() = False Then
                    EditMailIndex += 1
                End If
                If Controls.Up(True, True, True, False, True) = True Or KeyBoardHandler.KeyPressed(Keys.Tab) = True And Controls.ShiftDown() = True Then
                    EditMailIndex -= 1
                End If
                If Controls.Left(True, True, False, False, False) = True Then
                    If EditMailIndex = 1 Then
                        EditMailIndex -= 1
                    End If
                End If
                If Controls.Right(True, True, False, False, False) = True Then
                    If EditMailIndex = 0 Then
                        EditMailIndex += 1
                    End If
                End If

                EditMailIndex = EditMailIndex.Clamp(0, 1)

                If Controls.Accept(False, True, True) = True Then

                    If EditMailIndex = 0 Then
                        Me.TempNewMail = Core.Player.Mails(Me.index - 1)
                        SoundManager.PlaySound("select")
                        Dim selScreen = New PartyScreen(Me, Item.GetItemByID(TempNewMail.MailID.ToString), AddressOf Me.ChosenPokemon, "Give mail to:", True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                        AddHandler selScreen.SelectedObject, AddressOf ChosenPokemonHandler

                        Core.SetScreen(selScreen)
                    Else
                        Me.index = -1
                        EditMailIndex = 0

                    End If
                End If

                Dim text As String = Core.Player.Mails(index - 1).MailText.CropStringToWidth(FontManager.MainFont, 600)
                Dim yPlus As Integer = CInt(FontManager.MainFont.MeasureString(text).Y)

                If Controls.Accept(True, False, False) = True Then
                    Dim MailIndex As Integer = -1
                    For i = 0 To 8
                        If i < Core.Player.Mails.Count + 1 Then
                            If New Rectangle(46, 82 + 64 * i, 288, 64).Contains(MouseHandler.MousePosition) Then
                                MailIndex = scrollIndex + i
                                Exit For
                            End If
                        Else
                            If New Rectangle(46, 82 + 64 * i, 288, 64).Contains(MouseHandler.MousePosition) Then
                                EditMailIndex = 1
                                Exit For
                            End If
                        End If
                    Next
                    If MailIndex <> -1 Then
                        selectIndex = MailIndex
                        If Me.selectIndex = 0 Then
                            Me.index = -1
                            SoundManager.PlaySound("select")
                            Dim selScreen As New NewInventoryScreen(Core.CurrentScreen, {5}, 5, Nothing)
                            selScreen.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection
                            selScreen.CanExit = True

                            AddHandler selScreen.SelectedObject, AddressOf ChosenMailHandler
                            Core.SetScreen(selScreen)
                        Else
                            SoundManager.PlaySound("select")
                            If Me.index = Me.selectIndex Then
                                Me.index = -1
                            Else
                                Me.index = Me.selectIndex

                                Dim m As Items.MailItem.MailData = Core.Player.Mails(Me.index - 1)
                                Core.Player.Mails(Me.index - 1) = New Items.MailItem.MailData With {.MailHeader = m.MailHeader, .MailID = m.MailID, .MailOriginalTrainerOT = m.MailOriginalTrainerOT, .MailAttachment = m.MailAttachment, .MailRead = True, .MailSender = m.MailSender, .MailSignature = m.MailSignature, .MailText = m.MailText}
                            End If
                        End If
                    Else

                        If New Rectangle(440, yPlus + 320, 160, 32).Contains(MouseHandler.MousePosition) Then
                            EditMailIndex = 0
                        End If
                        If New Rectangle(640, yPlus + 320, 160, 32).Contains(MouseHandler.MousePosition) Then
                            EditMailIndex = 1
                        End If

                        If EditMailIndex = 0 Then
                            Me.TempNewMail = Core.Player.Mails(Me.index - 1)
                            SoundManager.PlaySound("select")
                            Dim selScreen = New PartyScreen(Me, Item.GetItemByID(TempNewMail.MailID.ToString), AddressOf Me.ChosenPokemon, "Give mail to:", True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                            AddHandler selScreen.SelectedObject, AddressOf ChosenPokemonHandler

                            Core.SetScreen(selScreen)
                        Else
                            Me.index = -1
                            EditMailIndex = 0

                        End If
                    End If
                End If

                If Controls.Dismiss(True, True, True) = True Then
                    SoundManager.PlaySound("select")
                    Me.index = -1

                End If
            Else

                If Controls.Down(True, True, True, True, True) = True Then
                    Me.selectIndex += 1
                    If Me.selectIndex = Me.index And Me.selectIndex < Core.Player.Mails.Count Then
                        Me.selectIndex += 1
                    End If
                End If
                If Controls.Up(True, True, True, True, True) = True Then
                    Me.selectIndex -= 1
                    If Me.selectIndex = Me.index And Me.selectIndex > 0 Then
                        Me.selectIndex -= 1
                    End If
                End If

                selectIndex = selectIndex.Clamp(0, Core.Player.Mails.Count)

                While selectIndex - scrollIndex > 8
                    scrollIndex += 1
                End While

                While selectIndex - scrollIndex < 0
                    scrollIndex -= 1
                End While

                If Controls.Accept(False, True, True) = True Then
                    If Me.selectIndex = 0 Then
                        SoundManager.PlaySound("select")
                        Dim selScreen As New NewInventoryScreen(Core.CurrentScreen, {5}, 5, Nothing)
                        selScreen.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection
                        selScreen.CanExit = True

                        AddHandler selScreen.SelectedObject, AddressOf ChosenMailHandler
                        Core.SetScreen(selScreen)
                    Else
                        SoundManager.PlaySound("select")
                        If Me.index = Me.selectIndex Then
                            Me.index = -1
                        Else
                            Me.index = Me.selectIndex

                            Dim m As Items.MailItem.MailData = Core.Player.Mails(Me.index - 1)
                            Core.Player.Mails(Me.index - 1) = New Items.MailItem.MailData With {.MailHeader = m.MailHeader, .MailID = m.MailID, .MailOriginalTrainerOT = m.MailOriginalTrainerOT, .MailAttachment = m.MailAttachment, .MailRead = True, .MailSender = m.MailSender, .MailSignature = m.MailSignature, .MailText = m.MailText}
                        End If
                    End If
                End If
                If Controls.Accept(True, False, False) Then
                    Dim MailIndex As Integer = -1
                    For i = 0 To 8
                        If i < Core.Player.Mails.Count + 1 Then
                            If New Rectangle(46, 82 + 64 * i, 288, 64).Contains(MouseHandler.MousePosition) Then
                                MailIndex = scrollIndex + i
                                Exit For
                            End If
                        End If
                    Next
                    If MailIndex <> -1 Then
                        selectIndex = MailIndex
                        If Me.selectIndex = 0 Then
                            SoundManager.PlaySound("select")
                            Dim selScreen As New NewInventoryScreen(Core.CurrentScreen, {5}, 5, Nothing)
                            selScreen.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection
                            selScreen.CanExit = True

                            AddHandler selScreen.SelectedObject, AddressOf ChosenMailHandler
                            Core.SetScreen(selScreen)
                        Else
                            SoundManager.PlaySound("select")
                            If Me.index = Me.selectIndex Then
                                Me.index = -1
                            Else
                                Me.index = Me.selectIndex

                                Dim m As Items.MailItem.MailData = Core.Player.Mails(Me.index - 1)
                                Core.Player.Mails(Me.index - 1) = New Items.MailItem.MailData With {.MailHeader = m.MailHeader, .MailID = m.MailID, .MailOriginalTrainerOT = m.MailOriginalTrainerOT, .MailAttachment = m.MailAttachment, .MailRead = True, .MailSender = m.MailSender, .MailSignature = m.MailSignature, .MailText = m.MailText}
                            End If
                        End If
                    End If

                End If

                If Controls.Dismiss(True, True, True) = True Then
                    SoundManager.PlaySound("select")
                    Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.White, False))
                End If
            End If
        Else
            Dim pressedSystemKey As Boolean = False
            If Controls.Down(True, True, True, False, True) = True Or KeyBoardHandler.KeyPressed(Keys.Tab) = True And Controls.ShiftDown() = False Then
                EditMailIndex += 1
                pressedSystemKey = True
            End If
            If Controls.Up(True, True, True, False, True) = True Or KeyBoardHandler.KeyPressed(Keys.Tab) = True And Controls.ShiftDown() = True Then
                EditMailIndex -= 1
                pressedSystemKey = True
            End If
            If Controls.Left(True, True, False, False, False) = True Then
                pressedSystemKey = True
                If EditMailIndex = 4 Then
                    EditMailIndex -= 1
                End If
            End If
            If Controls.Right(True, True, False, False, False) = True Then
                pressedSystemKey = True
                If EditMailIndex = 3 Then
                    EditMailIndex += 1
                End If
            End If
            If Controls.Left(False, False, True, True, True) = True Then
                If EditMailIndex = 1 Then
                    EditMailIndex -= 1
                End If
            End If
            If Controls.Right(False, False, True, True, True) = True Then
                If EditMailIndex = 0 Then
                    EditMailIndex += 1
                End If
            End If

            EditMailIndex = EditMailIndex.Clamp(0, 4)

            If pressedSystemKey = False Then
                Select Case EditMailIndex
                    Case 0
                        KeyBindings.GetInput(TempNewMail.MailHeader, 25, True, True)
                        TempNewMail.MailHeader = TempNewMail.MailHeader.Replace("\,", ",").Replace(Environment.NewLine, "").Replace("|", "/")
                    Case 1
                        KeyBindings.GetInput(TempNewMail.MailText, 200, True, True)
                        TempNewMail.MailText = TempNewMail.MailText.Replace("\,", ",").Replace(Environment.NewLine, "<br>").Replace("|", "/")
                    Case 2
                        KeyBindings.GetInput(TempNewMail.MailSignature, 25, True, True)
                        TempNewMail.MailSignature = TempNewMail.MailSignature.Replace("\,", ",").Replace(Environment.NewLine, "").Replace("|", "/")
                End Select
            End If

            If Controls.Accept(False, True, True) = True Then
                Select Case EditMailIndex
                    Case 3
                        SoundManager.PlaySound("select")
                        If TempNewMail.MailHeader = "" Or TempNewMail.MailText = "" Or TempNewMail.MailSignature = "" Then
                            message = "Please fill in the Header, the Message and the Signature."
                        Else
                            Dim selScreen = New PartyScreen(Me, Item.GetItemByID(TempNewMail.MailID.ToString), AddressOf Me.ChosenPokemon, "Give mail to:", True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                            AddHandler selScreen.SelectedObject, AddressOf ChosenPokemonHandler

                            Core.SetScreen(selScreen)
                        End If
                    Case 4
                        SoundManager.PlaySound("select")
                        Me.index = -1
                        EditMailIndex = 0
                End Select
            End If

            Dim text As String = ("Text: (" & TempNewMail.MailText.Length & "/" & 200 & ")" & Environment.NewLine & Environment.NewLine & TempNewMail.MailText.Replace("<br>", Environment.NewLine)).CropStringToWidth(FontManager.MainFont, 600)
            Dim yPlus As Integer = CInt(FontManager.MainFont.MeasureString(text).Y)

            If Controls.Accept(True, False, False) = True Then
                Dim MailIndex As Integer = -1
                For i = 0 To 8
                    If i < Core.Player.Mails.Count + 1 Then
                        If New Rectangle(46, 82 + 64 * i, 288, 64).Contains(MouseHandler.MousePosition) Then
                            MailIndex = scrollIndex + i
                            Exit For
                        End If
                    Else
                        If New Rectangle(46, 82 + 64 * i, 288, 64).Contains(MouseHandler.MousePosition) Then
                            EditMailIndex = 4
                            Exit For
                        End If
                    End If
                Next
                If MailIndex <> -1 Then
                    selectIndex = MailIndex
                    SoundManager.PlaySound("select")
                    If Me.selectIndex = 0 Then
                        Me.index = -1
                    Else
                        Me.index = Me.selectIndex

                        Dim m As Items.MailItem.MailData = Core.Player.Mails(Me.index - 1)
                        Core.Player.Mails(Me.index - 1) = New Items.MailItem.MailData With {.MailHeader = m.MailHeader, .MailID = m.MailID, .MailOriginalTrainerOT = m.MailOriginalTrainerOT, .MailAttachment = m.MailAttachment, .MailRead = True, .MailSender = m.MailSender, .MailSignature = m.MailSignature, .MailText = m.MailText}
                    End If
                End If
                If New Rectangle(420, 92, 660, 40).Contains(MouseHandler.MousePosition) Then
                    EditMailIndex = 0
                End If
                If New Rectangle(420, 140, 660, yPlus + 20).Contains(MouseHandler.MousePosition) Then
                    EditMailIndex = 1
                End If
                If New Rectangle(420, yPlus + 200, 660, 40).Contains(MouseHandler.MousePosition) Then
                    EditMailIndex = 2
                End If
                If New Rectangle(440, yPlus + 320, 160, 32).Contains(MouseHandler.MousePosition) Then
                    EditMailIndex = 3
                End If
                If New Rectangle(640, yPlus + 320, 160, 32).Contains(MouseHandler.MousePosition) Then
                    EditMailIndex = 4
                End If

                Select Case EditMailIndex
                    Case 3
                        SoundManager.PlaySound("select")
                        If TempNewMail.MailHeader = "" Or TempNewMail.MailText = "" Or TempNewMail.MailSignature = "" Then
                            message = "Please fill in the Header, the Message and the Signature."
                        Else
                            Dim selScreen = New PartyScreen(Me, Item.GetItemByID(TempNewMail.MailID.ToString), AddressOf Me.ChosenPokemon, "Give mail to:", True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                            AddHandler selScreen.SelectedObject, AddressOf ChosenPokemonHandler

                            Core.SetScreen(selScreen)
                        End If
                    Case 4
                        SoundManager.PlaySound("select")
                        Me.index = -1
                        EditMailIndex = 0
                End Select
            End If

            If Controls.Dismiss(True, False, True) = True Then
                Me.index = -1
                SoundManager.PlaySound("select")
            End If
        End If
    End Sub

    Private Sub ChosenMailHandler(ByVal params As Object())
        ChosenMail(CInt(params(0)))
    End Sub

    Private Sub ChosenMail(ByVal ItemID As Integer)
        Me.index = 0
        Me.EditMailIndex = 0
        Me.TempNewMail = New Items.MailItem.MailData
        Me.TempNewMail.MailID = ItemID
        Me.TempNewMail.MailSender = Core.Player.Name
        Me.TempNewMail.MailOriginalTrainerOT = Core.Player.OT
        Me.TempNewMail.MailText = ""
        Me.TempNewMail.MailHeader = ""
        Me.TempNewMail.MailAttachment = -1
        Me.TempNewMail.MailRead = False
        Me.TempNewMail.MailSignature = ""
    End Sub

    Private Sub ChosenPokemonHandler(ByVal params As Object())
        ChosenPokemon(CInt(params(0)))
    End Sub

    Private Sub ChosenPokemon(ByVal PokeIndex As Integer)
        Dim text As String = "Attached the Mail to " & Core.Player.Pokemons(PokeIndex).GetDisplayName() & "."
        If Not Core.Player.Pokemons(PokeIndex).Item Is Nothing Then
            If Core.Player.Pokemons(PokeIndex).Item.IsGameModeItem Then
                Core.Player.Inventory.AddItem(Core.Player.Pokemons(PokeIndex).Item.gmID, 1)
            Else
                Core.Player.Inventory.AddItem(Core.Player.Pokemons(PokeIndex).Item.ID.ToString, 1)
            End If
            text = "Taken " & Core.Player.Pokemons(PokeIndex).Item.Name & " from " & Core.Player.Pokemons(PokeIndex).GetDisplayName() & ", and attached the Mail to " & Core.Player.Pokemons(PokeIndex).GetDisplayName() & "."
        End If

        Core.Player.Pokemons(PokeIndex).Item = Item.GetItemByID(TempNewMail.MailID.ToString)
        Core.Player.Pokemons(PokeIndex).Item.AdditionalData = Items.MailItem.GetStringFromMail(TempNewMail)

        If index = 0 Then
            Core.Player.Inventory.RemoveItem(TempNewMail.MailID.ToString, 1)
        Else
            Core.Player.Mails.RemoveAt(Me.index - 1)
            selectIndex -= 1
            selectIndex = selectIndex.Clamp(0, Core.Player.Mails.Count)
        End If
        Me.index = -1

        Me.message = text
    End Sub

#Region "Trophies"

    Public Structure TrophyInformation
        Public Name As String
        Public ID As Integer
        Public Texture As Texture2D
        Public Description As String
    End Structure

    Public Shared Function GetTrophyInformation(ByVal ID As Integer) As TrophyInformation
        Dim T As New TrophyInformation
        Dim TexturePosition As New Vector2(0)

        Select Case ID
            Case 0
                T.Name = "Won a GTS competition."
                T.Description = "You are the winner of a competition that took place at the GTS. It must have been an important competition."
                TexturePosition = New Vector2(0, 0)
            Case 1
                T.Name = "Won a GTS competition."
                T.Description = "You are the winner of a competition that took place at the GTS."
                TexturePosition = New Vector2(32, 0)
            Case 2
                T.Name = "Won a GTS competition."
                T.Description = "You are the winner of a competition that took place at the GTS."
                TexturePosition = New Vector2(64, 0)
            Case 3
                T.Name = "Kolben Support"
                T.Description = "This proves that the Kolben Support helped you with your game."
                TexturePosition = New Vector2(96, 0)
        End Select

        T.Texture = TextureManager.GetTexture("GUI\Trophies", New Rectangle(CInt(TexturePosition.X), CInt(TexturePosition.Y), 32, 32), "")
        T.ID = ID

        Return T
    End Function

#End Region

End Class