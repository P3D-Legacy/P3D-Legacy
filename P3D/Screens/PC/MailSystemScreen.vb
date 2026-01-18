Public Class MailSystemScreen

    Inherits Screen

    Dim index As Integer = -1
    Dim scrollIndex As Integer = 0
    Dim selectIndex As Integer = 0

    Dim MenuEntries As New List(Of MenuEntry)
    Dim MenuVisible As Boolean = False
    Dim MenuCursor As Integer = 0
    Dim MenuHeader As String = ""
    Dim message As String = ""
    Dim UsedFromInventory As Boolean = False
    Dim TakenFromParty As Boolean = False
    Dim ReadyToExit As Boolean = False

    Public Sub New(ByVal currentScreen As Screen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.MailSystemScreen

        Me.MouseVisible = True
        Me.CanBePaused = True
        Me.CanMuteAudio = False
        Me.CanChat = False
    End Sub

    Public Sub New(ByVal currentScreen As Screen, InventoryMailItemID As String)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.MailSystemScreen

        Me.MouseVisible = True
        Me.CanBePaused = True
        Me.CanMuteAudio = False
        Me.CanChat = False
        If InventoryMailItemID <> "" Then
            Me.UsedFromInventory = True
            Me.index = 0
            ChosenMail(InventoryMailItemID)
        End If
    End Sub
    Public Sub New(ByVal currentScreen As Screen, PartyMailItem As Items.MailItem)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.MailSystemScreen

        Me.MouseVisible = True
        Me.CanBePaused = True
        Me.CanMuteAudio = False
        Me.CanChat = False
        If PartyMailItem IsNot Nothing Then
            Me.TakenFromParty = True
            Me.index = 1
            Me.TempNewMail = Items.MailItem.GetMailDataFromString(PartyMailItem.AdditionalData)
            Me.TempNewMail.MailRead = True
        End If
    End Sub

    Public Overrides Sub Draw()

        Dim backSize As New Size(windowSize.Width, windowSize.Height)
        Dim origSize As New Size(380, 210)
        Dim aspectRatio As Single = CSng(origSize.Width / origSize.Height)

        backSize.Width = CInt(windowSize.Width * aspectRatio)
        backSize.Height = CInt(backSize.Width / aspectRatio)

        If backSize.Width > backSize.Height Then
            backSize.Width = windowSize.Width
            backSize.Height = CInt(windowSize.Width / aspectRatio)
        Else
            backSize.Height = windowSize.Height
            backSize.Width = CInt(windowSize.Height / aspectRatio)
        End If
        If backSize.Height < windowSize.Height Then
            backSize.Height = windowSize.Height
            backSize.Width = CInt(windowSize.Height / origSize.Height * origSize.Width)
        End If

        Dim background As Texture2D = TextureManager.GetTexture("GUI\Menus\MailboxBackground")

        Dim xOffset As Integer = 0
        If windowSize.Width < backSize.Width Then
            Dim xAspectRatio As Single = CSng(origSize.Width / backSize.Width)
            xOffset = CInt(Math.Floor((backSize.Width - windowSize.Width) * xAspectRatio) / 2)
        End If

        Core.SpriteBatch.Draw(background, New Rectangle(0, 0, backSize.Width, backSize.Height), New Rectangle(xOffset, 0, origSize.Width, origSize.Height), Color.White)
        Canvas.DrawRectangle(New Rectangle(32, 16, 240, 48), New Color(255, 255, 255, 224))
        Canvas.DrawRectangle(New Rectangle(48, 64 - 2, 208, 2), Color.DarkGray)

        Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("mail_screen_title", "Mailbox"), New Vector2(56, 24), Color.Black)
        Canvas.DrawRectangle(New Rectangle(32, 64, 352, 624), New Color(255, 255, 255, 224))
        Canvas.DrawRectangle(New Rectangle(400, 64, 704, 624), New Color(255, 255, 255, 224))

        If UsedFromInventory = True Then
            DrawMail(Nothing, New Vector2(42, 78), 0)
        ElseIf TakenFromParty = True Then
            DrawMail(TempNewMail, New Vector2(42, 78), 1)
        Else
            For i = scrollIndex To scrollIndex + 8
                If i = 0 Then
                    DrawMail(Nothing, New Vector2(42, 78 + (i - scrollIndex) * 64), i)
                Else
                    If i <= Core.Player.Mails.Count Then
                        DrawMail(Core.Player.Mails(i - 1), New Vector2(42, 78 + (i - scrollIndex) * 64 + 2 * (i - scrollIndex)), i)
                    End If
                End If
            Next

        End If

        If UsedFromInventory = False AndAlso TakenFromParty = False Then
            Canvas.DrawScrollBar(New Vector2(368, 86), Core.Player.Mails.Count + 1, 9, scrollIndex, New Size(6, 560), False, Color.LightGray, Color.Black)
        End If

        If Me.index <> -1 Then
            DrawCurrentMail()
        End If

        If MenuVisible = True Then
            DrawMenuEntries()
        End If

        If message <> "" Then
            Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))
            Dim t As String = message.CropStringToWidth(FontManager.MainFont, 800)

            Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString(t).X / 2), 340), Color.White)
        End If

    End Sub

    Private Sub DrawMail(ByVal mail As Items.MailItem.MailData, ByVal P As Vector2, ByVal i As Integer)

        If i = 0 Then
            Dim x As Integer = 16
            Dim y As Integer = 16
            If i = Me.index Then
                x = 80
                y = 72
            Else
                If i = selectIndex Then
                    x = 48
                    y = 72
                End If
            End If

            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(x, y, 16, 16), ""), New Rectangle(CInt(P.X), CInt(CInt(P.Y)), 64, 64), Color.White)
            For i = 64 To 224 Step 64
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(x + 16, y, 16, 16), ""), New Rectangle(CInt(P.X + i), CInt(CInt(P.Y)), 64, 64), Color.White)
            Next
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(x, y, 16, 16), ""), New Rectangle(CInt(P.X + 256), CInt(CInt(P.Y)), 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)


            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("mail_screen_write_new_mail", "Write new mail."), New Vector2(CInt(P.X) + 13, CInt(P.Y) + 18), Color.Black)
        Else
            Dim item As Item = Item.GetItemByID(mail.MailID.ToString)

            Dim x As Integer = 16
            Dim y As Integer = 16
            If i = Me.index Then
                x = 80
                y = 72
            Else
                If i = selectIndex Then
                    x = 48
                    y = 72
                End If
            End If

            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(x, y, 16, 16), ""), New Rectangle(CInt(P.X), CInt(CInt(P.Y)), 64, 64), Color.White)
            For i = 64 To 224 Step 64
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(x + 16, y, 16, 16), ""), New Rectangle(CInt(P.X + i), CInt(CInt(P.Y)), 64, 64), Color.White)
            Next
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(x, y, 16, 16), ""), New Rectangle(CInt(P.X + 256), CInt(CInt(P.Y)), 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)

            Core.SpriteBatch.Draw(item.Texture, New Rectangle(CInt(P.X), CInt(P.Y) + 4, 48, 48), Color.White)

            Core.SpriteBatch.DrawString(FontManager.MainFont, mail.MailHeader, New Vector2(CInt(P.X) + 52, CInt(P.Y) + 18), Color.Black)

            If mail.MailAttachment > -1 Then
                Dim t As TrophyInformation = GetTrophyInformation(mail.MailAttachment)
                Core.SpriteBatch.Draw(t.Texture, New Rectangle(CInt(P.X) + 250, CInt(P.Y) + 8, 32, 32), Color.White)
            End If

            If mail.MailRead = False Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(CInt(P.X) + 272, CInt(P.Y) + 4, 32, 32), New Rectangle(320, 144, 32, 32), Color.White)
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

            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("mail_screen_mail_header", "Header:") & " " & mail.MailHeader, New Vector2(480, 92), c)

            Canvas.DrawRectangle(New Rectangle(420, 140, 660, 2), Color.DarkGray)

            Dim text As String = (Localization.GetString("mail_screen_mail_text", "Text:") & " (" & mail.MailText.Length & "/" & 200 & ")" & Environment.NewLine & Environment.NewLine & mail.MailText.Replace("<br>", Environment.NewLine)).CropStringToWidth(FontManager.MainFont, 600)
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
            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("mail_screen_mail_signature", "Signature:") & " " & mail.MailSignature, New Vector2(430, yPlus + 200), c)

            Canvas.DrawRectangle(New Rectangle(420, yPlus + 240, 660, 2), Color.DarkGray)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("mail_screen_mail_ot", "Mail OT:") & " " & mail.MailSender & " (" & mail.MailOriginalTrainerOT & ")", New Vector2(430, yPlus + 260), Color.Black)

            If EditMailIndex = 3 Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(80, 72, 16, 16), ""), New Rectangle(440, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(96, 72, 16, 16), ""), New Rectangle(440 + 64, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(80, 72, 16, 16), ""), New Rectangle(440 + 128, yPlus + 320, 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
            Else
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), ""), New Rectangle(440, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), ""), New Rectangle(440 + 64, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), ""), New Rectangle(440 + 128, yPlus + 320, 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("mail_screen_mail_button_attach", "Attach"), New Vector2(CInt(534 - FontManager.MainFont.MeasureString(Localization.GetString("mail_screen_mail_button_attach", "Attach")).X / 2), yPlus + CInt(348 - FontManager.MainFont.MeasureString(Localization.GetString("mail_screen_mail_button_attach", "Attach")).Y / 2)), Color.Black)

            If EditMailIndex = 4 Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(80, 72, 16, 16), ""), New Rectangle(640, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(96, 72, 16, 16), ""), New Rectangle(640 + 64, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(80, 72, 16, 16), ""), New Rectangle(640 + 128, yPlus + 320, 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
            Else
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), ""), New Rectangle(640, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), ""), New Rectangle(640 + 64, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), ""), New Rectangle(640 + 128, yPlus + 320, 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("global_cancel", "Cancel"), New Vector2(CInt(734 - FontManager.MainFont.MeasureString(Localization.GetString("global_cancel", "Cancel")).X / 2), yPlus + CInt(348 - FontManager.MainFont.MeasureString(Localization.GetString("global_cancel", "Cancel")).Y / 2)), Color.Black)
        Else
            Dim mail As Items.MailItem.MailData
            If TakenFromParty = True Then
                mail = TempNewMail
            Else
                mail = Core.Player.Mails(index - 1)
            End If
            Dim item As Item = Item.GetItemByID(mail.MailID.ToString)

            Core.SpriteBatch.Draw(item.Texture, New Rectangle(420, 84, 48, 48), Color.White)

            Core.SpriteBatch.DrawString(FontManager.MainFont, mail.MailHeader, New Vector2(480, 92), Color.Black)

            Canvas.DrawRectangle(New Rectangle(420, 140, 660, 2), Color.DarkGray)

            Dim text As String = mail.MailText.CropStringToWidth(FontManager.MainFont, 600)
            Core.SpriteBatch.DrawString(FontManager.MainFont, text, New Vector2(430, 160), Color.Black)

            Dim yPlus As Integer = CInt(FontManager.MainFont.MeasureString(text).Y)

            Core.SpriteBatch.DrawString(FontManager.MainFont, mail.MailSignature, New Vector2(430, yPlus + 200), Color.Black)

            Canvas.DrawRectangle(New Rectangle(420, yPlus + 240, 660, 2), Color.DarkGray)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("mail_screen_mail_ot", "Mail OT:") & " " & mail.MailSender & " (" & mail.MailOriginalTrainerOT & ")", New Vector2(430, yPlus + 260), Color.Black)

            If mail.MailAttachment > -1 Then
                Canvas.DrawRectangle(New Rectangle(420, yPlus + 300, 660, 2), Color.DarkGray)

                Dim t As TrophyInformation = GetTrophyInformation(mail.MailAttachment)
                Core.SpriteBatch.DrawString(FontManager.MainFont, "Trophy:", New Vector2(430, yPlus + 320), Color.Black)
                Core.SpriteBatch.Draw(t.Texture, New Rectangle(430, yPlus + 340, 64, 64), Color.White)
                Core.SpriteBatch.DrawString(FontManager.MainFont, (t.Name & Environment.NewLine & Environment.NewLine & t.Description).CropStringToWidth(FontManager.MainFont, 500), New Vector2(510, yPlus + 340), Color.Black)
            End If

            If EditMailIndex = 0 Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(80, 72, 16, 16), ""), New Rectangle(440, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(96, 72, 16, 16), ""), New Rectangle(440 + 64, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(80, 72, 16, 16), ""), New Rectangle(440 + 128, yPlus + 320, 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
            Else
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), ""), New Rectangle(440, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), ""), New Rectangle(440 + 64, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), ""), New Rectangle(440 + 128, yPlus + 320, 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
            End If
            Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("mail_screen_mail_button_attach", "Attach"), New Vector2(CInt(534 - FontManager.MainFont.MeasureString(Localization.GetString("mail_screen_mail_button_attach", "Attach")).X / 2), yPlus + CInt(348 - FontManager.MainFont.MeasureString(Localization.GetString("mail_screen_mail_button_attach", "Attach")).Y / 2)), Color.Black)

            If EditMailIndex = 1 Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(80, 72, 16, 16), ""), New Rectangle(640, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(96, 72, 16, 16), ""), New Rectangle(640 + 64, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(80, 72, 16, 16), ""), New Rectangle(640 + 128, yPlus + 320, 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
            Else
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), ""), New Rectangle(640, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), ""), New Rectangle(640 + 64, yPlus + 320, 64, 64), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), ""), New Rectangle(640 + 128, yPlus + 320, 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
            End If
            If TakenFromParty = True Then
                Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("mail_screen_mail_button_send_to_pc", "Send To PC"), New Vector2(CInt(734 - FontManager.MainFont.MeasureString(Localization.GetString("mail_screen_mail_button_send_to_pc", "Send To PC")).X / 2), yPlus + CInt(348 - FontManager.MainFont.MeasureString(Localization.GetString("mail_screen_mail_button_send_to_pc", "Send To PC")).Y / 2)), Color.Black)
            Else
                Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("global_delete", "Delete"), New Vector2(CInt(734 - FontManager.MainFont.MeasureString(Localization.GetString("global_delete", "Delete")).X / 2), yPlus + CInt(348 - FontManager.MainFont.MeasureString(Localization.GetString("global_delete", "Delete")).Y / 2)), Color.Black)
            End If

            If TakenFromParty = False Then
                If EditMailIndex = 2 Then
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(80, 72, 16, 16), ""), New Rectangle(840, yPlus + 320, 64, 64), Color.White)
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(96, 72, 16, 16), ""), New Rectangle(840 + 64, yPlus + 320, 64, 64), Color.White)
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(80, 72, 16, 16), ""), New Rectangle(840 + 128, yPlus + 320, 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
                Else
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), ""), New Rectangle(840, yPlus + 320, 64, 64), Color.White)
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), ""), New Rectangle(840 + 64, yPlus + 320, 64, 64), Color.White)
                    Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), ""), New Rectangle(840 + 128, yPlus + 320, 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)
                End If
                Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("global_cancel", "Cancel"), New Vector2(CInt(934 - FontManager.MainFont.MeasureString(Localization.GetString("global_cancel", "Cancel")).X / 2), yPlus + CInt(348 - FontManager.MainFont.MeasureString(Localization.GetString("global_cancel", "Cancel")).Y / 2)), Color.Black)
            End If
        End If
    End Sub

    Public Overrides Sub Update()
        If message <> "" Then
            If Controls.Accept(True, True, True) = True Or Controls.Dismiss(True, True, True) = True Then
                message = ""
                If ReadyToExit = True Then
                    Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.Black, False))
                End If
            End If

            Exit Sub
        Else
            If MenuVisible = True Then
                For i = 0 To Me.MenuEntries.Count - 1
                    If i <= Me.MenuEntries.Count - 1 Then
                        Dim m As MenuEntry = Me.MenuEntries(i)

                        m.Update(Me)
                    End If
                Next

                If Controls.Up(True, True) = True Then
                    Me.MenuCursor -= 1
                End If
                If Controls.Down(True, True) = True Then
                    Me.MenuCursor += 1
                End If

                Dim maxIndex As Integer = 0
                Dim minIndex As Integer = 100

                For Each e As MenuEntry In Me.MenuEntries
                    If e.Index < minIndex Then
                        minIndex = e.Index
                    End If
                    If e.Index > maxIndex Then
                        maxIndex = e.Index
                    End If
                Next

                If Me.MenuCursor > maxIndex Then
                    Me.MenuCursor = minIndex
                ElseIf Me.MenuCursor < minIndex Then
                    Me.MenuCursor = maxIndex
                End If

            Else

                If index <> 0 Then
                    If index > -1 Then

                        Dim pressedSystemKey As Boolean = False
                        If Controls.Down(True, True, True, False, True) = True Or KeyBoardHandler.KeyPressed(Keys.Tab) = True And Controls.ShiftDown() = False Then
                            EditMailIndex += 1
                        End If
                        If Controls.Up(True, True, True, False, True) = True Or KeyBoardHandler.KeyPressed(Keys.Tab) = True And Controls.ShiftDown() = True Then
                            EditMailIndex -= 1
                        End If
                        If Controls.Left(True, True, False, True, True) = True Then
                            EditMailIndex -= 1
                        End If
                        If Controls.Right(True, True, False, True, True) = True Then
                            EditMailIndex += 1
                        End If

                        If TakenFromParty = True Then
                            EditMailIndex = EditMailIndex.Clamp(0, 1)
                        Else
                            EditMailIndex = EditMailIndex.Clamp(0, 2)
                        End If


                        If Controls.Accept(False, True, True) = True Then

                            Select Case EditMailIndex
                                Case 0
                                    If TakenFromParty = False Then
                                        Me.TempNewMail = Core.Player.Mails(Me.index - 1)
                                    End If
                                    SoundManager.PlaySound("select")
                                    Dim selScreen = New PartyScreen(Me, Item.GetItemByID(TempNewMail.MailID.ToString), AddressOf Me.ChosenPokemon, Localization.GetString("mail_screen_give_mail_to", "Give mail to:"), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                                    AddHandler selScreen.SelectedObject, AddressOf ChosenPokemonHandler

                                    Core.SetScreen(selScreen)
                                Case 1
                                    If TakenFromParty = True Then
                                        Dim e1 As New MenuEntry(3, Localization.GetString("global_yes", "Yes"), False, AddressOf SendMailToPC)
                                        Dim e2 As New MenuEntry(4, Localization.GetString("global_no", "No"), True, Nothing)
                                        SetupMenu({e1, e2}, Localization.GetString("mail_screen_mail_send_to_pc_confirm", "Send this mail to PC?"))
                                    Else
                                        Dim e1 As New MenuEntry(3, Localization.GetString("global_yes", "Yes"), False, AddressOf DeleteMail)
                                        Dim e2 As New MenuEntry(4, Localization.GetString("global_no", "No"), True, Nothing)
                                        SetupMenu({e1, e2}, Localization.GetString("mail_screen_mail_delete_confirm", "Delete this mail?"))
                                    End If

                                Case 2
                                    If TakenFromParty = False Then
                                        Me.index = -1
                                        EditMailIndex = 0
                                    End If
                            End Select
                        End If

                        Dim text As String = ""
                        If index <> -1 Then
                            If TakenFromParty = False Then
                                text = Core.Player.Mails(index - 1).MailText.CropStringToWidth(FontManager.MainFont, 600)
                            Else
                                text = TempNewMail.MailText.CropStringToWidth(FontManager.MainFont, 600)
                            End If
                        End If
                        Dim yPlus As Integer = CInt(FontManager.MainFont.MeasureString(text).Y)

                        If Controls.Accept(True, False, False) = True Then
                            Dim MailIndex As Integer = -1
                            If TakenFromParty = False Then
                                For i = 0 To 8
                                    If i < Core.Player.Mails.Count + 1 Then
                                        If New Rectangle(46, 82 + 64 * i + 2 * (i - scrollIndex), 288, 64).Contains(MouseHandler.MousePosition) Then
                                            MailIndex = scrollIndex + i
                                            Exit For
                                        End If
                                    Else
                                        If New Rectangle(46, 82 + 64 * i + 2 * (i - scrollIndex), 288, 64).Contains(MouseHandler.MousePosition) Then
                                            EditMailIndex = 1
                                            Exit For
                                        End If
                                    End If
                                Next
                            End If

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

                                If New Rectangle(440, yPlus + 320, 192, 64).Contains(MouseHandler.MousePosition) Then
                                    EditMailIndex = 0
                                    If TakenFromParty = False Then
                                        Me.TempNewMail = Core.Player.Mails(Me.index - 1)
                                    End If
                                    SoundManager.PlaySound("select")
                                    Dim selScreen = New PartyScreen(Me, Item.GetItemByID(TempNewMail.MailID.ToString), AddressOf Me.ChosenPokemon, Localization.GetString("mail_screen_give_mail_to", "Give mail to:"), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                                    AddHandler selScreen.SelectedObject, AddressOf ChosenPokemonHandler

                                    Core.SetScreen(selScreen)
                                End If
                                If New Rectangle(640, yPlus + 320, 192, 64).Contains(MouseHandler.MousePosition) Then
                                    SoundManager.PlaySound("select")
                                    EditMailIndex = 1
                                    If TakenFromParty = True Then
                                        Dim e1 As New MenuEntry(3, Localization.GetString("global_yes", "Yes"), False, AddressOf SendMailToPC)
                                        Dim e2 As New MenuEntry(4, Localization.GetString("global_no", "No"), True, Nothing)
                                        SetupMenu({e1, e2}, Localization.GetString("mail_screen_mail_send_to_pc_confirm", "Send this mail to PC?"))
                                    Else
                                        Dim e1 As New MenuEntry(3, Localization.GetString("global_yes", "Yes"), False, AddressOf DeleteMail)
                                        Dim e2 As New MenuEntry(4, Localization.GetString("global_no", "No"), True, Nothing)
                                        SetupMenu({e1, e2}, Localization.GetString("mail_screen_mail_delete_confirm", "Delete this mail?"))
                                    End If
                                End If
                                If TakenFromParty = False Then
                                    If New Rectangle(840, yPlus + 320, 192, 64).Contains(MouseHandler.MousePosition) Then
                                        SoundManager.PlaySound("select")
                                        Me.index = -1
                                        EditMailIndex = 0
                                    End If
                                End If
                            End If
                        End If

                        If Controls.Dismiss(True, True, True) = True Then
                            If TakenFromParty = True Then
                                Dim e1 As New MenuEntry(3, Localization.GetString("global_yes", "Yes"), False, AddressOf SendMailToPC)
                                Dim e2 As New MenuEntry(4, Localization.GetString("global_no", "No"), True, Nothing)
                                SetupMenu({e1, e2}, Localization.GetString("mail_screen_mail_send_to_pc_confirm", "Send this mail to PC?"))
                            Else
                                SoundManager.PlaySound("select")
                                Me.index = -1
                            End If

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
                                    If New Rectangle(46, 82 + 64 * i + 2 * (i - scrollIndex), 288, 64).Contains(MouseHandler.MousePosition) Then
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
                            Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.Black, False))
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
                        If EditMailIndex = 4 Or EditMailIndex = 5 Then
                            EditMailIndex -= 1
                        End If
                    End If
                    If Controls.Right(True, True, False, False, False) = True Then
                        pressedSystemKey = True
                        If EditMailIndex = 3 Or EditMailIndex = 4 Then
                            EditMailIndex += 1
                        End If
                    End If
                    If Controls.Left(False, False, False, True, True) = True Then
                        If EditMailIndex = 4 Or EditMailIndex = 5 Then
                            EditMailIndex -= 1
                        End If
                    End If
                    If Controls.Right(False, False, False, True, True) = True Then
                        If EditMailIndex = 3 Or EditMailIndex = 4 Then
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
                                    message = Localization.GetString("mail_screen_mail_missing_contents", "Please fill in the Header, the Message and the Signature.")
                                Else
                                    Dim selScreen = New PartyScreen(Me, Item.GetItemByID(TempNewMail.MailID.ToString), AddressOf Me.ChosenPokemon, Localization.GetString("mail_screen_give_mail_to", "Give mail to:"), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                                    AddHandler selScreen.SelectedObject, AddressOf ChosenPokemonHandler

                                    Core.SetScreen(selScreen)
                                End If
                            Case 4
                                SoundManager.PlaySound("select")
                                Me.index = -1
                                EditMailIndex = 0
                        End Select
                    End If

                    Dim text As String = (Localization.GetString("mail_screen_mail_text", "Text:") & " (" & TempNewMail.MailText.Length & "/" & 200 & ")" & Environment.NewLine & Environment.NewLine & TempNewMail.MailText.Replace("<br>", Environment.NewLine)).CropStringToWidth(FontManager.MainFont, 600)
                    Dim yPlus As Integer = CInt(FontManager.MainFont.MeasureString(text).Y)

                    If Controls.Accept(True, False, False) = True Then
                        Dim MailIndex As Integer = -1
                        If UsedFromInventory = False Then
                            For i = 0 To 8
                                If i < Core.Player.Mails.Count + 1 Then
                                    If New Rectangle(46, 82 + 64 * i + 2 * (i - scrollIndex), 288, 64).Contains(MouseHandler.MousePosition) Then
                                        MailIndex = scrollIndex + i
                                        Exit For
                                    End If
                                Else
                                    If New Rectangle(46, 82 + 64 * i + 2 * (i - scrollIndex), 288, 64).Contains(MouseHandler.MousePosition) Then
                                        EditMailIndex = 4
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
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
                        If New Rectangle(440, yPlus + 320, 192, 64).Contains(MouseHandler.MousePosition) Then
                            EditMailIndex = 3
                        End If
                        If New Rectangle(640, yPlus + 320, 192, 64).Contains(MouseHandler.MousePosition) Then
                            EditMailIndex = 4
                        End If

                        Select Case EditMailIndex
                            Case 3
                                SoundManager.PlaySound("select")
                                If TempNewMail.MailHeader = "" Or TempNewMail.MailText = "" Or TempNewMail.MailSignature = "" Then
                                    message = Localization.GetString("mail_screen_mail_missing_contents", "Please fill in the Header, the Message and the Signature.")
                                Else
                                    Dim selScreen = New PartyScreen(Me, Item.GetItemByID(TempNewMail.MailID.ToString), AddressOf Me.ChosenPokemon, Localization.GetString("mail_screen_give_mail_to", "Give mail to:"), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
                                    AddHandler selScreen.SelectedObject, AddressOf ChosenPokemonHandler

                                    Core.SetScreen(selScreen)
                                End If
                            Case 4
                                SoundManager.PlaySound("select")
                                If UsedFromInventory = True Then
                                    Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.Black, False))
                                Else
                                    Me.index = -1
                                    EditMailIndex = 0
                                End If
                        End Select
                    End If

                    If Controls.Dismiss(True, False, True) = True Then
                        If UsedFromInventory = True Then
                            Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.Black, False))
                        Else
                            Me.index = -1
                            EditMailIndex = 0
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ChosenMailHandler(ByVal params As Object())
        ChosenMail(CStr(params(0)))
    End Sub

    Private Sub ChosenMail(ByVal ItemID As String)
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
        Dim text As String = Localization.GetString("mail_screen_mail_attached_mail", "Attached the Mail to [POKEMON].").Replace("[POKEMON]", Core.Player.Pokemons(PokeIndex).GetDisplayName())
        If Not Core.Player.Pokemons(PokeIndex).Item Is Nothing Then
            If Core.Player.Pokemons(PokeIndex).Item.IsGameModeItem Then
                Core.Player.Inventory.AddItem(Core.Player.Pokemons(PokeIndex).Item.gmID, 1)
            Else
                Core.Player.Inventory.AddItem(Core.Player.Pokemons(PokeIndex).Item.ID.ToString, 1)
            End If
            text = Localization.GetString("mail_screen_mail_taken_item_and_attached_mail", "Taken [ITEM] from [POKEMON], and attached the Mail to [POKEMON].").Replace("[POKEMON]", Core.Player.Pokemons(PokeIndex).GetDisplayName()).Replace("[ITEM]", Core.Player.Pokemons(PokeIndex).Item.OneLineName())
        End If

        Core.Player.Pokemons(PokeIndex).Item = Item.GetItemByID(TempNewMail.MailID.ToString)
        Core.Player.Pokemons(PokeIndex).Item.AdditionalData = Items.MailItem.GetStringFromMail(TempNewMail)

        If index = 0 Then
            Core.Player.Inventory.RemoveItem(TempNewMail.MailID.ToString, 1)
            Me.index = -1
        Else
            If TakenFromParty = False Then
                Core.Player.Mails.RemoveAt(Me.index - 1)
                selectIndex -= 1
                selectIndex = selectIndex.Clamp(0, Core.Player.Mails.Count)
                Me.index = -1
            Else
                ReadyToExit = True
            End If
        End If

        Dim s As Screen = Core.CurrentScreen
        While Not s.PreScreen Is Nothing And s.Identification <> Screen.Identifications.InventoryScreen
            s = s.PreScreen
        End While

        If s.Identification = Screen.Identifications.InventoryScreen Then
            CType(s, NewInventoryScreen).LoadItems()
        End If

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

#Region "DeleteMailMenu"
    Private Sub DeleteMail()
        Me.message = Localization.GetString("mail_screen_mail_deleted_from_mailbox", "The mail has been removed from the mailbox.")
        Core.Player.Mails.RemoveAt(Me.index - 1)
        Me.index = -1
        EditMailIndex = 0
        selectIndex -= 1
        selectIndex = selectIndex.Clamp(0, Core.Player.Mails.Count)
    End Sub
    Private Sub SendMailToPC()
        Me.message = Localization.GetString("mail_screen_mail_added_to_mailbox", "The Mail was taken to your inbox on your PC.")
        Core.Player.Mails.Add(TempNewMail)
        ReadyToExit = True
    End Sub

    Private Sub SetupMenu(ByVal entries() As MenuEntry, ByVal header As String)
        Me.MenuEntries.Clear()
        Me.MenuEntries.AddRange(entries)
        Me.MenuVisible = True
        Me.MenuCursor = MenuEntries(0).Index
        Me.MenuHeader = header
    End Sub

    Private Sub DrawMenuEntries()
        If Me.MenuHeader <> "" Then
            Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - 334, 100, 320, 64), New Color(0, 0, 0, 180))
            Core.SpriteBatch.DrawString(FontManager.MainFont, MenuHeader, New Vector2(Core.windowSize.Width - 174 - FontManager.MainFont.MeasureString(MenuHeader).X / 2, 120), Color.White)
        End If

        For Each e As MenuEntry In Me.MenuEntries
            e.Draw(Me.MenuCursor, TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), ""))
        Next
    End Sub

    Class MenuEntry

        Public Index As Integer = 0
        Public TAG As Object = Nothing

        Public Text As String = "Menu"
        Public IsBack As Boolean = False
        Public Delegate Sub ClickEvent(ByVal m As MenuEntry)
        Public ClickHandler As ClickEvent = Nothing

        Dim t1 As Texture2D
        Dim t2 As Texture2D

        Public Sub New(ByVal Index As Integer, ByVal text As String, ByVal isBack As Boolean, ByVal ClickHandler As ClickEvent)
            Me.New(Index, text, isBack, ClickHandler, Nothing)
        End Sub

        Public Sub New(ByVal Index As Integer, ByVal text As String, ByVal isBack As Boolean, ByVal ClickHandler As ClickEvent, ByVal TAG As Object)
            Me.Index = Index
            Me.TAG = TAG

            Me.Text = text
            Me.IsBack = isBack
            Me.ClickHandler = ClickHandler

            t1 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), "")
            t2 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), "")
        End Sub

        Public Sub Update(ByVal s As MailSystemScreen)
            If Controls.Accept(True, False, False) = True And s.MenuCursor = Me.Index And New Rectangle(Core.windowSize.Width - 270, 66 * Index, 256, 64).Contains(MouseHandler.MousePosition) = True Or Controls.Accept(False, True, True) = True And s.MenuCursor = Me.Index Or Controls.Dismiss(True, True, True) = True And Me.IsBack = True Then
                s.MenuVisible = False
                If Not ClickHandler Is Nothing Then
                    ClickHandler(Me)
                End If
            End If
            If New Rectangle(Core.windowSize.Width - 270, 66 * Index, 256, 64).Contains(MouseHandler.MousePosition) = True And Controls.Accept(True, False, False) = True Then
                s.MenuCursor = Me.Index
            End If
        End Sub

        Public Sub Draw(ByVal CursorIndex As Integer, ByVal CursorTexture As Texture2D)
            Dim startPos As New Vector2(Core.windowSize.Width - 270, 66 * Index)

            Core.SpriteBatch.Draw(t1, New Rectangle(CInt(startPos.X), CInt(startPos.Y), 64, 64), Color.White)
            Core.SpriteBatch.Draw(t2, New Rectangle(CInt(startPos.X + 64), CInt(startPos.Y), 64, 64), Color.White)
            Core.SpriteBatch.Draw(t2, New Rectangle(CInt(startPos.X + 128), CInt(startPos.Y), 64, 64), Color.White)
            Core.SpriteBatch.Draw(t1, New Rectangle(CInt(startPos.X + 192), CInt(startPos.Y), 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)

            Core.SpriteBatch.DrawString(FontManager.MainFont, Me.Text, New Vector2(startPos.X + 128 - (FontManager.MainFont.MeasureString(Me.Text).X * 1.4F) / 2, startPos.Y + 15), Color.Black, 0.0F, Vector2.Zero, 1.4F, SpriteEffects.None, 0.0F)

            If Me.Index = CursorIndex Then
                Dim cPosition As Vector2 = New Vector2(startPos.X + 128, startPos.Y - 40)
                Dim t As Texture2D = CursorTexture
                Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
            End If
        End Sub

    End Class
#End Region
End Class
