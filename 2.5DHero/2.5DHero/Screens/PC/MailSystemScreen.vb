Public Class MailSystemScreen

    Inherits Screen

    Dim index As Integer = -1
    Dim scrollIndex As Integer = 0
    Dim selectIndex As Integer = 0

    Dim message As String = ""

    Public Sub New(ByVal currentScreen As Screen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.MailSystemScreen

        Me.MouseVisible = False
        Me.CanBePaused = True
        Me.CanMuteMusic = False
        Me.CanChat = False
    End Sub

    Public Overrides Sub Draw()
        Canvas.DrawRectangle(Core.windowSize, Color.White)

        Core.SpriteBatch.DrawString(FontManager.InGameFont, "Inbox", New Vector2(42, 28), Color.Black)
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
            Dim t As String = message.CropStringToWidth(FontManager.InGameFont, 800)

            Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.InGameFont.MeasureString(t).X / 2), 340), Color.White)
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
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Write new mail.", New Vector2(CInt(P.X) + 13, CInt(P.Y) + 14), Color.Black)
        Else
            Dim item As Item = item.GetItemByID(mail.MailID)

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

            Core.SpriteBatch.DrawString(FontManager.MiniFont, mail.MailHeader, New Vector2(CInt(P.X) + 52, CInt(P.Y) + 14), Color.Black)

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
            Dim item As Item = item.GetItemByID(mail.MailID)

            Core.SpriteBatch.Draw(item.Texture, New Rectangle(420, 84, 48, 48), Color.White)

            Dim c As Color = Color.Gray

            If EditMailIndex = 0 Then
                c = Color.Blue
            Else
                c = Color.Gray
            End If

            Core.SpriteBatch.DrawString(FontManager.InGameFont, "Header: " & mail.MailHeader, New Vector2(480, 92), c)

            Canvas.DrawRectangle(New Rectangle(420, 140, 660, 2), Color.DarkGray)

            Dim text As String = ("Text: (" & mail.MailText.Length & "/" & 200 & ")" & vbNewLine & vbNewLine & mail.MailText.Replace("<br>", vbNewLine)).CropStringToWidth(FontManager.MiniFont, 600)
            If EditMailIndex = 1 Then
                c = Color.Blue
                text &= "_"
            Else
                c = Color.Gray
            End If
            Core.SpriteBatch.DrawString(FontManager.MiniFont, text, New Vector2(430, 160), c)

            Dim yPlus As Integer = CInt(FontManager.MiniFont.MeasureString(text).Y)

            If EditMailIndex = 2 Then
                c = Color.Blue
            Else
                c = Color.Gray
            End If
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Signature: " & mail.MailSignature, New Vector2(430, yPlus + 200), c)

            Canvas.DrawRectangle(New Rectangle(420, yPlus + 240, 660, 2), Color.DarkGray)

            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Mail OT: " & mail.MailSender & " (" & mail.MailOriginalTrainerOT & ")", New Vector2(430, yPlus + 260), Color.Black)

            If EditMailIndex = 3 Then
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), ""), 1, New Rectangle(440, yPlus + 320, 160, 32))
            Else
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 1, New Rectangle(440, yPlus + 320, 160, 32))
            End If
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Attach", New Vector2(496, yPlus + 334), Color.Black)

            If EditMailIndex = 4 Then
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), ""), 1, New Rectangle(640, yPlus + 320, 160, 32))
            Else
                Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 1, New Rectangle(640, yPlus + 320, 160, 32))
            End If
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Cancel", New Vector2(696, yPlus + 334), Color.Black)
        Else
            Dim mail As Items.MailItem.MailData = Core.Player.Mails(index - 1)
            Dim item As Item = item.GetItemByID(mail.MailID)

            Core.SpriteBatch.Draw(item.Texture, New Rectangle(420, 84, 48, 48), Color.White)

            Core.SpriteBatch.DrawString(FontManager.InGameFont, mail.MailHeader, New Vector2(480, 92), Color.Black)

            Canvas.DrawRectangle(New Rectangle(420, 140, 660, 2), Color.DarkGray)

            Dim text As String = ("Text: " & vbNewLine & vbNewLine & mail.MailText).CropStringToWidth(FontManager.MiniFont, 600)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, text, New Vector2(430, 160), Color.Black)

            Dim yPlus As Integer = CInt(FontManager.MiniFont.MeasureString(text).Y)

            Core.SpriteBatch.DrawString(FontManager.MiniFont, mail.MailSignature, New Vector2(430, yPlus + 200), Color.Black)

            Canvas.DrawRectangle(New Rectangle(420, yPlus + 240, 660, 2), Color.DarkGray)

            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Mail OT: " & mail.MailSender & " (" & mail.MailOriginalTrainerOT & ")", New Vector2(430, yPlus + 260), Color.Black)

            If mail.MailAttachment > -1 Then
                Canvas.DrawRectangle(New Rectangle(420, yPlus + 300, 660, 2), Color.DarkGray)

                Dim t As TrophyInformation = GetTrophyInformation(mail.MailAttachment)
                Core.SpriteBatch.DrawString(FontManager.MiniFont, "Trophy:", New Vector2(430, yPlus + 320), Color.Black)
                Core.SpriteBatch.Draw(t.Texture, New Rectangle(430, yPlus + 340, 64, 64), Color.White)
                Core.SpriteBatch.DrawString(FontManager.MiniFont, (t.Name & vbNewLine & vbNewLine & t.Description).CropStringToWidth(FontManager.MiniFont, 500), New Vector2(510, yPlus + 340), Color.Black)
            End If
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

            If Controls.Accept(True, True, True) = True Then
                If Me.selectIndex = 0 Then
                    Core.SetScreen(New InventoryScreen(Me, {5}, 5, AddressOf Me.ChosenMail))
                Else
                    If Me.index = Me.selectIndex Then
                        Me.index = -1
                    Else
                        Me.index = Me.selectIndex

                        Dim m As Items.MailItem.MailData = Core.Player.Mails(Me.index - 1)
                        Core.Player.Mails(Me.index - 1) = New Items.MailItem.MailData With {.MailHeader = m.MailHeader, .MailID = m.MailID, .MailOriginalTrainerOT = m.MailOriginalTrainerOT, .MailAttachment = m.MailAttachment, .MailRead = True, .MailSender = m.MailSender, .MailSignature = m.MailSignature, .MailText = m.MailText}
                    End If
                End If
            End If

            If Controls.Dismiss(True, True, True) = True Then
                If Me.index <> -1 Then
                    Me.index = -1
                Else
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
            If Controls.Left(True, True, False, False, False) = True Or Controls.Right(True, True, False, False, False) = True Then
                pressedSystemKey = True
            End If

            EditMailIndex = EditMailIndex.Clamp(0, 4)

            If pressedSystemKey = False Then
                Select Case EditMailIndex
                    Case 0
                        KeyBindings.GetInput(TempNewMail.MailHeader, 25, True, True)
                        TempNewMail.MailHeader = TempNewMail.MailHeader.Replace("\,", ",").Replace(vbNewLine, "").Replace("|", "/")
                    Case 1
                        KeyBindings.GetInput(TempNewMail.MailText, 200, True, True)
                        TempNewMail.MailText = TempNewMail.MailText.Replace("\,", ",").Replace(vbNewLine, "<br>").Replace("|", "/")
                    Case 2
                        KeyBindings.GetInput(TempNewMail.MailSignature, 25, True, True)
                        TempNewMail.MailSignature = TempNewMail.MailSignature.Replace("\,", ",").Replace(vbNewLine, "").Replace("|", "/")
                End Select
            End If

            If Controls.Accept(True, True, True) = True Then
                Select Case EditMailIndex
                    Case 3
                        If TempNewMail.MailHeader = "" Or TempNewMail.MailText = "" Or TempNewMail.MailSignature = "" Then
                            message = "Please fill in the Header, the Message and the Signature."
                        Else
                            Core.SetScreen(New ChoosePokemonScreen(Me, Item.GetItemByID(TempNewMail.MailID), AddressOf Me.ChosenPokemon, "Give mail to:", True))
                        End If
                    Case 4
                        Me.index = -1
                End Select
            End If

            If Controls.Dismiss(True, False, True) = True Then
                Me.index = -1
            End If
        End If
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

    Private Sub ChosenPokemon(ByVal PokeIndex As Integer)
        If Not Core.Player.Pokemons(PokeIndex).Item Is Nothing Then
            Core.Player.Inventory.AddItem(Core.Player.Pokemons(PokeIndex).Item.ID, 1)
        End If

        Core.Player.Pokemons(PokeIndex).Item = Item.GetItemByID(TempNewMail.MailID)
        Core.Player.Pokemons(PokeIndex).Item.AdditionalData = Items.MailItem.GetStringFromMail(TempNewMail)

        Core.Player.Inventory.RemoveItem(TempNewMail.MailID, 1)
        Me.index = -1

        Me.message = "Attached the Mail to " & Core.Player.Pokemons(PokeIndex).GetDisplayName() & "."
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
                T.Description = "You are the winner of a competition that took place at the GTS. It might has been an important competition."
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