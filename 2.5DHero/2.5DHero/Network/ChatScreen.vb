Public Class ChatScreen

    Inherits Screen

    Dim scrollIndex As Integer = 0
    Dim currentText As String = ""
    Dim selectIndex As Integer = -1
    Dim enterDelay As Single = 0.2F

    Dim Selection As Integer = 0 'Carret position
    Dim SelectionStart As Integer = 0
    Dim SelectionLength As Integer = 0

    Public Enum ChatStates
        [Global]
        PM
        Command
    End Enum

    Public Shared ChatState As ChatStates = ChatStates.Global
    Private Shared HasCommandChat As Boolean = False
    Private Shared PMChats As New Dictionary(Of String, Boolean)
    Private Shared HasNewGlobalMessages As Boolean = False
    Private Shared CurrentPMChat As String = ""

    Public Shared Sub ResetChatState()
        ChatState = ChatStates.Global
        PMChats.Clear()
        HasCommandChat = False
        CurrentPMChat = ""
        newMessages.Clear()
        newMessagesDelays.Clear()
        HasNewGlobalMessages = False
    End Sub

    Public Shared newMessages As New List(Of Chat.ChatMessage)
    Public Shared newMessagesDelays As New List(Of Single)

    Public Sub New(ByVal currentScreen As Screen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.ChatScreen
        Me.CanBePaused = True
        Me.CanMuteMusic = False
        Me.CanChat = False
        Me.MouseVisible = True
    End Sub

    Public Overrides Sub EscapePressed()
        If KeyBoardHandler.KeyPressed(KeyBindings.EscapeKey) = True Or ControllerHandler.ButtonDown(Buttons.Start) = True Then
            Core.SetScreen(Me.PreScreen)
        End If
    End Sub

    Shared RightButtonDownDelay As Single = 3.0F
    Shared LeftButtonDownDelay As Single = 3.0F

    Public Overrides Sub Update()
        'Updates the level screen so online player entities get drawn.
        If PreScreen.Identification = Identifications.OverworldScreen And JoinServerScreen.Online = True Then
            Screen.Level.Update()
        End If

        If Controls.Dismiss(True, False) = True Or (JoinServerScreen.Online = False And Core.Player.SandBoxMode = False And GameController.IS_DEBUG_ACTIVE = False) = True Then
            Core.SetScreen(Me.PreScreen)
        Else
            If KeyBoardHandler.KeyPressed(Input.Keys.Enter) = True Or ControllerHandler.ButtonPressed(Buttons.A) = True Then
                Me.UpdateSendMessage()
            End If
            If KeyBoardHandler.KeyPressed(Keys.Tab) = True Then
                Me.UpdateTabCompletion()
            End If

            If ControllerHandler.ButtonPressed(Buttons.X) = True Then
                Me.currentText = ""
            End If

            If ControllerHandler.ButtonPressed(Buttons.Y) = True Then
                Dim t As String = Me.currentText
                While t.Length > 38
                    t = t.Remove(t.Length - 1, 1)
                End While
                Core.SetScreen(New InputScreen(Me, t, InputScreen.InputModes.Text, t, 38, New List(Of Texture2D), AddressOf Me.GetControllerInput))
            End If

            Me.UpdateTextInput()
        End If

        'Crops too long input text:
        If FontManager.TextFont.MeasureString(Me.currentText).X * 2.0F > Core.windowSize.Width - 200 Then
            While FontManager.TextFont.MeasureString(Me.currentText).X * 2.0F > Core.windowSize.Width - 200
                Me.currentText = Me.currentText.Remove(Me.currentText.Length - 1, 1)
            End While
        End If

        Me.UpdateSelection()

        Me.UpdateChatScroll()

        Me.ClickOnTabs()

        UpdateNewMessages()
    End Sub

    Private Sub UpdateTextInput()
        Dim inputText As String = " "
        If Controls.CtrlPressed() = True Then
            If KeyBoardHandler.KeyPressed(Keys.A) = True Then
                Me.SelectAll()
            End If
            If KeyBoardHandler.KeyPressed(Keys.C) = True Then
                Me.Copy()
            End If
            If KeyBoardHandler.KeyPressed(Keys.V) = True Then
                If System.Windows.Forms.Clipboard.ContainsText() = True Then
                    Dim t As String = System.Windows.Forms.Clipboard.GetText().Replace(vbNewLine, " ")

                    inputText &= t
                End If
            End If
        Else
            KeyBindings.GetInput(inputText)
        End If
        If inputText = "" Then
            If Me.SelectionLength > 0 Then
                Me.currentText = Me.currentText.Remove(Me.SelectionStart, Me.SelectionLength)
                Me.Selection = Me.SelectionStart
                Me.SelectionLength = 0
                Me.SelectionStart = 0
            Else
                If Me.currentText.Length > 0 And Me.Selection > 0 Then
                    Me.currentText = Me.currentText.Remove(Me.Selection - 1, 1)
                    Me.Selection -= 1
                End If
            End If
        ElseIf inputText.Length > 1 Then
            If Me.SelectionLength > 0 Then
                Me.currentText = Me.currentText.Remove(Me.SelectionStart, Me.SelectionLength)
                Me.SelectionLength = 0
                Me.Selection = Me.SelectionStart
            End If
            inputText = inputText.Remove(0, 1)
            Me.currentText = Me.currentText.Substring(0, Me.Selection) & inputText & Me.currentText.Substring(Me.Selection)
            Me.Selection += inputText.Length

            For i = 0 To currentText.Length - 1
                If i <= Me.currentText.Length - 1 Then
                    If FontManager.TextFont.Characters.Contains(currentText(i)) = False Then
                        currentText = currentText.Remove(i, 1)
                        i -= 1
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub Copy()
        If Me.SelectionLength > 0 Then
            Try
                System.Windows.Forms.Clipboard.SetText(Me.currentText.Substring(Me.SelectionStart, Me.SelectionLength))
            Catch ex As Exception
                Logger.Log(Logger.LogTypes.Warning, "ChatScreen.vb: An error occurred while copying text to the clipboard (""" & Me.currentText.Substring(Me.SelectionStart, Me.SelectionLength) & """).")
            End Try
        End If
    End Sub

    Private Sub SelectAll()
        Me.Selection = Me.currentText.Length
        Me.SelectionStart = 0
        Me.SelectionLength = Me.currentText.Length
    End Sub

    Private Sub UpdateSendMessage()
        If Me.selectIndex > -1 Then
            If Me.currentText = "" Then
                If Controls.ShiftDown() = True Then
                    Me.currentText = Chat.TransferedLines(ChatState, CurrentPMChat)(Me.selectIndex).ToString()
                Else
                    Me.currentText = Chat.TransferedLines(ChatState, CurrentPMChat)(Me.selectIndex).Message
                End If
                Me.SelectionLength = 0
                Me.SelectionStart = 0
                Me.Selection = Me.currentText.Length
            End If

            Me.selectIndex = -1
        Else
            If Me.currentText <> "" Then
                While currentText.EndsWith(" ") = True
                    currentText = currentText.Remove(currentText.Length - 1, 1)
                End While
                If currentText.StartsWith("/pm ") = True Then
                    Dim playerName As String = currentText.Remove(0, 4)
                    While Core.ServersManager.PlayerCollection.HasPlayer(playerName) = False And playerName.Contains(" ") = True
                        playerName = playerName.Remove(playerName.LastIndexOf(" "))
                    End While
                    If playerName <> "" And (Core.ServersManager.PlayerCollection.HasPlayer(playerName) = True Or PMChats.ContainsKey(playerName) = True) And playerName.ToLower() <> Core.Player.Name.ToLower() Then
                        EnterPMChat(playerName)
                    Else
                        Core.GameMessage.ShowMessage("Player " & playerName & " not found!", 20, FontManager.MainFont, Color.White)
                    End If
                ElseIf currentText.StartsWith("/global") = True Then
                    CurrentPMChat = ""
                    ChangeChatState(ChatStates.Global)
                    newMessages.Clear()
                    newMessagesDelays.Clear()
                ElseIf currentText.StartsWith("/command") = True Then
                    CurrentPMChat = ""
                    HasCommandChat = True
                    ChangeChatState(ChatStates.Command)
                    newMessages.Clear()
                    newMessagesDelays.Clear()
                ElseIf currentText.StartsWith("/close") = True Then
                    Chat.ClearChatMessages(ChatState, CurrentPMChat)
                    Select Case ChatState
                        Case ChatStates.Command
                            HasCommandChat = False
                        Case ChatStates.PM
                            PMChats.Remove(CurrentPMChat)
                    End Select
                    ChangeChatState(ChatStates.Global)
                    CurrentPMChat = ""
                ElseIf currentText.StartsWith("/clear") = True Then
                    Chat.ClearChatMessages(ChatState, CurrentPMChat)
                Else
                    Dim GJID As String = ""
                    If Core.Player.IsGameJoltSave = True Then
                        GJID = Core.GameJoltSave.GameJoltID
                    End If

                    Dim chatMessage As New Chat.ChatMessage(Core.Player.Name, Me.currentText, GJID, Chat.ChatMessage.MessageTypes.GlobalMessage)

                    If Chat.IsCommandMessage(chatMessage.Message) = True Then
                        ChangeChatState(ChatStates.Command)
                        HasCommandChat = True

                        Chat.WriteLine(chatMessage)
                        Core.SetScreen(Me.PreScreen)
                    Else
                        If ChatState = ChatStates.Command Then
                            ChangeChatState(ChatStates.Global)
                        ElseIf ChatState = ChatStates.PM Then
                            chatMessage.MessageType = Chat.ChatMessage.MessageTypes.PMMessage
                            chatMessage.Message = "/pm " & CurrentPMChat & " " & chatMessage.Message
                        End If
                        Chat.WriteLine(chatMessage)
                    End If
                End If

                currentText = ""
                Selection = 0
                SelectionLength = 0
                SelectionStart = 0
                selectIndex = -1
                scrollIndex = 0
            Else
                Core.SetScreen(Me.PreScreen)
            End If
        End If
    End Sub

    Private Sub UpdateTabCompletion()
        Dim completionCommands() = {"pm", "kick",
            "list add whitelist", "list remove whitelist", "list toggle whitelist",
            "list add blacklist", "list remove blacklist", "list toggle blacklist",
            "list add operators", "list remove operators", "list toggle operators",
            "list add mutelist", "list remove mutelist", "list toggle mutelist"}

        For Each tabCommand As String In completionCommands
            Dim commandLength As Integer = tabCommand.Length + 2

            If Me.currentText.ToLower().StartsWith("/" & tabCommand & " ") = True And Me.currentText.Length > commandLength Then
                Dim playerName As String = currentText.Remove(0, commandLength)

                Dim currentTextLength As Integer = Me.currentText.Length
                Dim currentCursorPosition As Integer = Me.Selection

                Me.currentText = Me.currentText.Remove(commandLength) & Core.ServersManager.PlayerCollection.GetMatchingPlayerName(playerName)

                If Me.currentText.Length > currentTextLength Then
                    Me.Selection += Me.currentText.Length - currentTextLength
                    Me.SelectionStart = 0
                    Me.SelectionLength = 0
                End If
            End If
        Next
    End Sub

    Private Sub UpdateSelection()
        If Controls.Right(False, True, False, False, True, True) = True Or KeyBoardHandler.KeyDown(Keys.End) = True Then
            If RightButtonDownDelay <= 0.0F Or RightButtonDownDelay = 4.0F Then
                If Controls.ShiftDown() = True Then
                    If Me.SelectionLength = 0 Then
                        If Selection < Me.currentText.Length Then
                            Me.SelectionStart = Me.Selection
                        End If
                    End If
                    If Me.Selection >= Me.SelectionStart + Me.SelectionLength Then
                        If Selection < Me.currentText.Length Then
                            Dim selectionJump As Integer = Me.GetSelectionJump(False)

                            Me.SelectionLength += selectionJump
                            Me.Selection += selectionJump
                        End If
                    ElseIf Me.Selection = Me.SelectionStart Then
                        Dim selectionJump As Integer = Me.GetSelectionJump(False)

                        Me.Selection += selectionJump
                        Me.SelectionStart += selectionJump
                        Me.SelectionLength -= selectionJump
                    End If
                Else
                    If Me.SelectionLength > 0 Then
                        Me.Selection = Me.SelectionStart + Me.SelectionLength
                        Me.SelectionStart = 0
                        Me.SelectionLength = 0
                    Else
                        Dim selectionJump As Integer = Me.GetSelectionJump(False)

                        Me.Selection += selectionJump
                    End If
                End If
                If RightButtonDownDelay = 4.0F Then
                    RightButtonDownDelay -= 0.2F
                End If
                RightButtonDownDelay += 0.1F
            Else
                RightButtonDownDelay -= 0.2F
            End If
        Else
            RightButtonDownDelay = 4.0F
        End If
        If Controls.Left(False, True, False, False, True, True) = True Or KeyBoardHandler.KeyDown(Keys.Home) = True Then
            If LeftButtonDownDelay <= 0.0F Or LeftButtonDownDelay = 4.0F Then
                If Controls.ShiftDown() = True Then
                    If Me.SelectionLength = 0 Then
                        If Me.Selection > 0 Then
                            Me.SelectionStart = Me.Selection
                        End If
                    End If
                    If Me.Selection <= Me.SelectionStart Then
                        If Me.Selection > 0 Then
                            Dim selectionJump As Integer = Me.GetSelectionJump(True)

                            Me.SelectionLength += selectionJump
                            Me.SelectionStart -= selectionJump
                            Me.Selection -= selectionJump
                        End If
                    ElseIf Me.Selection = Me.SelectionStart + Me.SelectionLength Then
                        Dim selectionJump As Integer = Me.GetSelectionJump(True)

                        Me.Selection -= selectionJump
                        Me.SelectionLength -= selectionJump
                    End If
                Else
                    If Me.SelectionLength > 0 Then
                        Me.Selection = Me.SelectionStart
                        Me.SelectionStart = 0
                        Me.SelectionLength = 0
                    Else
                        Dim selectionJump As Integer = Me.GetSelectionJump(True)

                        Me.Selection -= selectionJump
                    End If
                End If
                If LeftButtonDownDelay = 4.0F Then
                    LeftButtonDownDelay -= 0.2F
                End If
                LeftButtonDownDelay += 0.1F
            Else
                LeftButtonDownDelay -= 0.2F
            End If
        Else
            LeftButtonDownDelay = 4.0F
        End If

        Me.Selection = Me.Selection.Clamp(0, Me.currentText.Length)
        Me.SelectionStart = Me.SelectionStart.Clamp(0, Me.currentText.Length - 1)
        Me.SelectionLength = Me.SelectionLength.Clamp(0, Me.currentText.Length - Me.SelectionStart)
    End Sub

    Private Sub UpdateChatScroll()
        Dim lineCount As Integer = Chat.TransferedLines(ChatState, CurrentPMChat).Count
        Dim drawCount As Integer = Me.GetDrawLinesCount() 'default=14

        If Controls.Up(True, False, True, False) = True Then
            If Controls.ShiftDown() = True Then
                Me.scrollIndex += 5
            Else
                Me.scrollIndex += 1
            End If
        End If
        If Controls.Down(True, False, True, False) = True Then
            If Controls.ShiftDown() = True Then
                Me.scrollIndex -= 5
            Else
                Me.scrollIndex -= 1
            End If
        End If

        If Controls.Up(True, True, False, False, True) = True Then
            If Me.selectIndex < 0 Then
                Me.selectIndex = lineCount - 1
            Else
                If Controls.ShiftDown() = True Then
                    Me.selectIndex -= 5
                Else
                    Me.selectIndex -= 1
                End If

                If selectIndex < 0 Then
                    selectIndex = 0
                End If

                If Me.selectIndex < lineCount - drawCount - scrollIndex Then
                    scrollIndex -= Me.selectIndex - (lineCount - drawCount - scrollIndex)
                End If
            End If
        End If
        If Controls.Down(True, True, False, False, True) = True Then
            If Controls.ShiftDown() = True Then
                Me.selectIndex += 5
            Else
                Me.selectIndex += 1
            End If

            If Me.selectIndex >= lineCount And scrollIndex = 0 Then
                Me.selectIndex = -1
            Else
                If selectIndex >= lineCount - scrollIndex Then
                    scrollIndex = lineCount - 1 - selectIndex
                End If
            End If
        End If

        Me.scrollIndex = CInt(MathHelper.Clamp(scrollIndex, 0, lineCount - drawCount))

        If Me.selectIndex > -1 Then
            Me.selectIndex = CInt(MathHelper.Clamp(MathHelper.Clamp(Me.selectIndex, lineCount - drawCount - scrollIndex, lineCount - 1 - scrollIndex), -1, lineCount - 1))
        End If
    End Sub

    Private Function GetSelectionJump(ByVal left As Boolean) As Integer
        If KeyBoardHandler.KeyDown(Keys.End) = True Then
            Return Me.currentText.Length - Selection
        End If
        If KeyBoardHandler.KeyDown(Keys.Home) = True Then
            Return Selection
        End If

        Dim jumpStops As String() = {" ", "*", "²", "³", "+", "#", "°", "'", ".", ";", "-", ":", ">", "<", "|", "!", """", "§", "%", "&", "/", "(", ")", "=", "?", "{", "[", "]", "}", "\", "@"}

        If Controls.CtrlPressed() = True Then
            Dim jumpIndex As Integer = 1
            If left = True Then
                While Me.Selection - jumpIndex >= 0 AndAlso jumpStops.Contains(Me.currentText(Me.Selection - jumpIndex).ToString()) = False Or jumpIndex = 1
                    jumpIndex += 1
                End While
                Return jumpIndex - 1
            Else
                While Me.Selection + jumpIndex <= Me.currentText.Length - 1 AndAlso jumpStops.Contains(Me.currentText(Me.Selection + jumpIndex).ToString()) = False Or jumpIndex = 1
                    jumpIndex += 1
                End While
                Return jumpIndex
            End If
        Else
            Return 1
        End If
    End Function

    Private Function IsUpper() As Boolean
        If KeyBoardHandler.KeyDown(Input.Keys.LeftShift) = True Or KeyBoardHandler.KeyDown(Input.Keys.RightShift) = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function GetDrawLinesCount() As Integer
        Dim items As Integer = 0
        Dim lines As Integer = 0

        Dim transferedLines = Chat.TransferedLines(ChatState, CurrentPMChat)
        If transferedLines.Count > 0 Then
            For i = 0 To transferedLines.Count - 1
                If i <= 13 And items <= 13 Then
                    Dim takeIndex As Integer = (transferedLines.Count - 1) - (i + scrollIndex)
                    If takeIndex >= 0 And takeIndex < transferedLines.Count Then
                        Dim line As String = transferedLines(takeIndex).ToString()

                        Dim dispLine As String = ""
                        Dim SplitIndicies As List(Of Integer) = GetSplitIndicies(line)
                        Dim preIndex As Integer = 0
                        For Each index As Integer In SplitIndicies
                            If dispLine <> "" Then
                                dispLine &= vbNewLine
                            End If
                            If index - preIndex > line.Length - preIndex Then
                                dispLine &= line.Substring(preIndex)
                            Else
                                dispLine &= line.Substring(preIndex, index - preIndex)
                            End If
                            preIndex = index
                        Next

                        Dim lineArr() As String = dispLine.SplitAtNewline()

                        items += lineArr.Count
                        lines += 1
                    End If
                End If
            Next
        End If

        Return lines
    End Function

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        Dim transferedLines = Chat.TransferedLines(ChatState, CurrentPMChat)

        Dim offset As Integer = 0
        Dim items As Integer = 0
        Dim lines As Integer = 0
        If transferedLines.Count > 0 Then
            For i = 0 To transferedLines.Count - 1
                If i <= 13 And items <= 13 Then
                    Dim takeIndex As Integer = (transferedLines.Count - 1) - (i + scrollIndex)
                    If takeIndex >= 0 And takeIndex < transferedLines.Count Then
                        Dim line As String = transferedLines((transferedLines.Count - 1) - (i + scrollIndex)).ToString()
                        Dim chatMessage As Chat.ChatMessage = transferedLines((transferedLines.Count - 1) - (i + scrollIndex))

                        Dim c As Color = chatMessage.MessageColor()

                        Dim backC = New Color(0, 0, 0, 150)
                        If selectIndex = transferedLines.Count - 1 - (i + scrollIndex) Then
                            backC = New Color(100, 100, 100, 150)
                        End If

                        Dim dispLine As String = ""
                        Dim SplitIndicies As List(Of Integer) = GetSplitIndicies(line)
                        Dim preIndex As Integer = 0
                        For Each index As Integer In SplitIndicies
                            If dispLine <> "" Then
                                dispLine &= vbNewLine
                            End If
                            If index - preIndex > line.Length - preIndex Then
                                dispLine &= line.Substring(preIndex)
                            Else
                                dispLine &= line.Substring(preIndex, index - preIndex)
                            End If
                            preIndex = index
                        Next

                        Dim lineArr() As String = dispLine.SplitAtNewline()
                        Array.Reverse(lineArr)

                        For Each l As String In lineArr
                            Canvas.DrawRectangle(New Rectangle(100, (Core.windowSize.Height - 82) - offset - 64, Core.windowSize.Width - 200, 32), backC)
                            Core.SpriteBatch.DrawString(FontManager.TextFont, l, New Vector2(100, (Core.windowSize.Height - 80) - offset - 64), c, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
                            offset += 32
                            items += 1
                        Next
                        lines += 1
                    End If
                End If
            Next

            Canvas.DrawScrollBar(New Vector2(Core.windowSize.Width - 100 - 6, (Core.windowSize.Height - 80) - offset - 34), transferedLines.Count - (lines - 1), 1, transferedLines.Count - lines - (scrollIndex), New Size(6, items * 32), False, New Color(0, 0, 0, 0), New Color(255, 255, 255, 200))
        End If

        Me.DrawChatTabs()

        Canvas.DrawRectangle(New Rectangle(100, Core.windowSize.Height - 82, Core.windowSize.Width - 200, 32), New Color(0, 0, 0, 150))

        If Me.SelectionLength > 0 Then
            Dim startX As Integer = CInt(FontManager.TextFont.MeasureString(Me.currentText.Substring(0, SelectionStart)).X * 2.0F)
            Dim length As Integer = CInt(FontManager.TextFont.MeasureString(Me.currentText.Substring(Me.SelectionStart, Me.SelectionLength)).X * 2.0F)

            Canvas.DrawRectangle(New Rectangle(100 + startX, Core.windowSize.Height - 82, length, 32), Color.White)
            Core.SpriteBatch.DrawString(FontManager.TextFont, Me.currentText.Substring(Me.SelectionStart, Me.SelectionLength), New Vector2(100 + startX, Core.windowSize.Height - 80), Color.Black, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
        End If

        Dim s As String = Me.currentText
        If Me.Selection <> -1 And Me.Selection <> currentText.Length Then
            s = Me.currentText.Substring(0, Me.Selection) & Me.currentText.Substring(Me.Selection)
        End If

        Core.SpriteBatch.DrawString(FontManager.TextFont, s, New Vector2(100, Core.windowSize.Height - 80), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
        Dim startXSelection As Integer = CInt(FontManager.TextFont.MeasureString(Me.currentText.Substring(0, Selection)).X * 2.0F)
        If Me.Selection = Me.SelectionStart And Me.SelectionLength > 0 Then
            startXSelection -= 2
        End If
        Core.SpriteBatch.DrawString(FontManager.TextFont, "|", New Vector2(98 + startXSelection, Core.windowSize.Height - 86), Color.White, 0.0F, Vector2.Zero, New Vector2(2.0F, 2.6F), SpriteEffects.None, 0.0F)

        If Me.SelectionLength > 0 Then
            Dim startX As Integer = CInt(FontManager.TextFont.MeasureString(Me.currentText.Substring(0, SelectionStart)).X * 2.0F)
            Core.SpriteBatch.DrawString(FontManager.TextFont, Me.currentText.Substring(Me.SelectionStart, Me.SelectionLength), New Vector2(100 + startX, Core.windowSize.Height - 80), Color.Black, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
        End If

        Dim d As New Dictionary(Of Buttons, String)
        d.Add(Input.Buttons.A, "Enter")
        d.Add(Input.Buttons.B, "Back")
        d.Add(Input.Buttons.Y, "Edit")
        d.Add(Input.Buttons.X, "Clear")
        Me.DrawGamePadControls(d)
    End Sub

#Region "ChatTabSystem"

    Private Sub ClickOnTabs()
        If canClickOnTab = True Then
            If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Or MouseHandler.ButtonPressed(MouseHandler.MouseButtons.MiddleButton) = True Then
                If canClickOnTabType = ChatStates.PM Then
                    EnterPMChat(canClickOnTabText)
                Else
                    CurrentPMChat = ""
                    ChangeChatState(canClickOnTabType)
                End If
            End If
            If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.MiddleButton) = True Then
                Select Case ChatState
                    Case ChatStates.Command
                        HasCommandChat = False
                        Chat.ClearChatMessages(ChatState, CurrentPMChat)
                    Case ChatStates.PM
                        PMChats.Remove(CurrentPMChat)
                        Chat.ClearChatMessages(ChatState, CurrentPMChat)
                End Select
                ChangeChatState(ChatStates.Global)
                CurrentPMChat = ""
            End If
        End If
    End Sub

    Private canClickOnTabType As ChatStates = ChatStates.Global
    Private canClickOnTabText As String = ""
    Private canClickOnTab As Boolean = False

    Private Sub DrawChatTabs()
        Dim p = MouseHandler.MousePosition

        'First, draw global:
        DrawChatTab(100, "global", HasNewGlobalMessages, "Global", ChatState = ChatStates.Global)
        If p.X >= 100 And p.X < 220 Then
            canClickOnTab = True
            canClickOnTabType = ChatStates.Global
            canClickOnTabText = "Global"
        End If

        'Then, if active, draw the Commands tab:
        Dim x As Integer = 120

        If HasCommandChat = True Then
            DrawChatTab(x + 100, "command", False, "Commands", ChatState = ChatStates.Command)
            x += 120
            If p.X >= 220 And p.X < 340 Then
                canClickOnTab = True
                canClickOnTabType = ChatStates.Command
                canClickOnTabText = "Commands"
            End If
        End If

        Dim tabWidth As Integer = 0
        'Then draw all PM tabs
        For i = 0 To PMChats.Count - 1
            If Core.ServersManager.PlayerCollection.HasPlayer(PMChats.Keys(i)) = True Then
                tabWidth = DrawChatTab(x + 100, "pm", PMChats.Values(i), PMChats.Keys(i), ChatState = ChatStates.PM And PMChats.Keys(i) = CurrentPMChat)

                If p.X >= 100 + x And p.X < 100 + x + tabWidth Then
                    canClickOnTab = True
                    canClickOnTabType = ChatStates.PM
                    canClickOnTabText = PMChats.Keys(i)
                End If

                x += tabWidth
            Else
                tabWidth = DrawChatTab(x + 100, "pmoff", PMChats.Values(i), PMChats.Keys(i), ChatState = ChatStates.PM And PMChats.Keys(i) = CurrentPMChat)

                If p.X >= 100 + x And p.X < 100 + x + tabWidth Then
                    canClickOnTab = True
                    canClickOnTabType = ChatStates.PM
                    canClickOnTabText = PMChats.Keys(i)
                End If

                x += tabWidth
            End If
        Next

        If p.Y < Core.windowSize.Height - 114 Or p.Y >= Core.windowSize.Height - 82 Then
            canClickOnTab = False
        End If
    End Sub

    Private Shared Sub DrawNewMessagesTab()
        Dim texType As String = "global"
        Dim text As String = "Global"

        Select Case ChatState
            Case ChatStates.Command
                text = "Commands"
                texType = "command"
            Case ChatStates.PM
                text = CurrentPMChat
                If Core.ServersManager.PlayerCollection.HasPlayer(CurrentPMChat) = True Then
                    texType = "pm"
                Else
                    texType = "pmoff"
                End If
        End Select

        DrawChatTab(100, texType, False, text, False)
    End Sub

    ''' <summary>
    ''' Draws a chat tab and returns its drawn width.
    ''' </summary>
    Private Shared Function DrawChatTab(ByVal xPosition As Integer, ByVal textureType As String, ByVal HasNewMessages As Boolean, ByVal Text As String, ByVal IsActive As Boolean) As Integer
        Dim drawHeight As Integer = 32
        Dim drawWidth As Integer = 120
        If IsActive = False Then
            drawHeight = 24
        End If

        If textureType = "pm" Or textureType = "pmoff" Then
            drawWidth = CInt(MathHelper.Clamp(40 + FontManager.MainFont.MeasureString(Text).X * 0.75F, 120, 200))
        End If

        Canvas.DrawRectangle(New Rectangle(xPosition, Core.windowSize.Height - 114, drawWidth, drawHeight), New Color(0, 0, 0, 150))

        Dim texture = TextureManager.GetTexture("GUI\Chat\Icons")
        Select Case textureType.ToLower()
            Case "global"
                Core.SpriteBatch.Draw(texture, New Rectangle(xPosition + 2, Core.windowSize.Height - 114 + 2, 20, 20), New Rectangle(0, 0, 10, 10), Color.White)
            Case "command"
                Core.SpriteBatch.Draw(texture, New Rectangle(xPosition + 5, Core.windowSize.Height - 114 + 3, 22, 18), New Rectangle(34, 0, 11, 9), Color.White)
            Case "pm"
                Core.SpriteBatch.Draw(texture, New Rectangle(xPosition + 7, Core.windowSize.Height - 114 + 8, 12, 12), New Rectangle(10, 0, 12, 12), Color.White)
            Case "pmoff"
                Core.SpriteBatch.Draw(texture, New Rectangle(xPosition + 7, Core.windowSize.Height - 114 + 8, 12, 12), New Rectangle(45, 0, 12, 12), Color.White)
        End Select

        If HasNewMessages = True Then
            Core.SpriteBatch.Draw(texture, New Rectangle(xPosition, Core.windowSize.Height - 114, 12, 12), New Rectangle(22, 0, 12, 12), Color.White)
        End If

        Core.SpriteBatch.DrawString(FontManager.MainFont, Text, New Vector2(xPosition + 30, Core.windowSize.Height - 114 + 4), Color.White, 0F, Vector2.Zero, 0.75F, SpriteEffects.None, 0F)

        Return drawWidth
    End Function

#End Region

    Private Shared Function GetSplitIndicies(ByVal line As String) As List(Of Integer)
        Dim lineLength As Integer = CInt(FontManager.TextFont.MeasureString(line).X * 2.0F)
        Dim BoxLength As Integer = CInt(Core.windowSize.Width - 220)
        Dim Parts As Double = lineLength / BoxLength
        Dim CharCount As Integer = line.Length
        Dim CharSplit As Integer = CInt(Math.Floor(CharCount / Parts))

        Dim iList As New List(Of Integer)
        For i = 1 To CInt(Math.Ceiling(Parts))
            iList.Add(CharSplit * i)
        Next
        Return iList
    End Function

    Public Shared Sub DrawNewMessages()
        If Core.GameOptions.ShowGUI = True Then
            Dim offset As Integer = 0
            For i = 0 To newMessages.Count - 1
                If newMessages.Count - 1 >= i And (newMessages.Count - 1) - i <= 10 Then
                    Dim delay As Single = newMessagesDelays(i)
                    Dim line As String = newMessages(i).ToString()
                    Dim chatMessage As Chat.ChatMessage = newMessages(i)
                    Dim opacity As Integer = 150

                    delay -= 0.1F

                    If delay < 1.5F And delay > 1.4F Then
                        opacity = 150
                    ElseIf delay <= 1.4F And delay > 1.3F Then
                        opacity = 140
                    ElseIf delay <= 1.3F And delay > 1.2F Then
                        opacity = 130
                    ElseIf delay <= 1.2F And delay > 1.1F Then
                        opacity = 120
                    ElseIf delay <= 1.1F And delay > 1.0F Then
                        opacity = 110
                    ElseIf delay <= 1.0F And delay > 0.9F Then
                        opacity = 100
                    ElseIf delay <= 0.9F And delay > 0.8F Then
                        opacity = 90
                    ElseIf delay <= 0.8F And delay > 0.7F Then
                        opacity = 80
                    ElseIf delay <= 0.7F And delay > 0.6F Then
                        opacity = 70
                    ElseIf delay <= 0.6F And delay > 0.5F Then
                        opacity = 60
                    ElseIf delay <= 0.5F And delay > 0.4F Then
                        opacity = 50
                    ElseIf delay <= 0.4F And delay > 0.3F Then
                        opacity = 40
                    ElseIf delay <= 0.3F And delay > 0.2F Then
                        opacity = 30
                    ElseIf delay <= 0.2F And delay > 0.1F Then
                        opacity = 20
                    ElseIf delay <= 0.1F And delay > 0.0F Then
                        opacity = 10
                    ElseIf delay <= 0.0F Then
                        opacity = 0
                    End If

                    If i = 0 Then
                        'draw tab:
                        DrawNewMessagesTab()
                    End If

                    If delay <= 0.0F Then
                        newMessages.RemoveAt(i)
                        newMessagesDelays.RemoveAt(i)
                    Else
                        newMessagesDelays(i) = delay
                    End If

                    Dim c As Color = chatMessage.MessageColor()

                    Dim dispLine As String = ""
                    Dim SplitIndicies As List(Of Integer) = GetSplitIndicies(line)
                    Dim preIndex As Integer = 0
                    For Each index As Integer In SplitIndicies
                        If dispLine <> "" Then
                            dispLine &= vbNewLine
                        End If
                        If index - preIndex > line.Length - preIndex Then
                            dispLine &= line.Substring(preIndex)
                        Else
                            dispLine &= line.Substring(preIndex, index - preIndex)
                        End If
                        preIndex = index
                    Next

                    Dim lineArr() As String = dispLine.SplitAtNewline()
                    Array.Reverse(lineArr)

                    For Each l As String In lineArr
                        Canvas.DrawRectangle(New Rectangle(100, (Core.windowSize.Height - 82) - offset - 64, Core.windowSize.Width - 200, 32), New Color(0, 0, 0, opacity))
                        Core.SpriteBatch.DrawString(FontManager.TextFont, l, New Vector2(100, (Core.windowSize.Height - 80) - offset - 64), New Color(c.R, c.G, c.B, CInt(opacity * 1.7)), 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
                        offset += 32
                    Next
                End If
            Next

            Dim hasNewMessages As Boolean = HasNewGlobalMessages
            For i = 0 To PMChats.Count - 1
                If PMChats.Values(i) = True Then
                    hasNewMessages = True
                    Exit For
                End If
            Next
            If hasNewMessages = True Then
                Canvas.DrawRectangle(New Rectangle(0, Core.windowSize.Height - 64, 64, 64), New Color(0, 0, 0, 150))

                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Chat\Icons"), New Rectangle(22, Core.windowSize.Height - 40, 24, 24), New Rectangle(10, 0, 12, 12), Color.White)
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Chat\Icons"), New Rectangle(8, Core.windowSize.Height - 52, 24, 24), New Rectangle(22, 0, 12, 12), Color.White)
            End If
        End If
    End Sub

    Public Shared Sub UpdateNewMessages()
        For i As Integer = 0 To newMessages.Count - 1
            If newMessages.Count - 1 >= i And (newMessages.Count - 1) - i <= 10 Then
                Dim delay As Single = newMessagesDelays(i)

                delay -= 0.1F

                If delay <= 0.0F Then
                    newMessages.RemoveAt(i)
                    newMessagesDelays.RemoveAt(i)
                Else
                    newMessagesDelays(i) = delay
                End If
            End If
        Next
    End Sub

    Dim ControllerShown As Boolean = False
    Public Overrides Sub ChangeTo()
        If ControllerHandler.IsConnected() = True And Core.GameOptions.GamePadEnabled = True And ControllerShown = False Then
            Core.SetScreen(New InputScreen(Me, "", InputScreen.InputModes.Text, "", 38, New List(Of Texture2D), AddressOf Me.GetControllerInput))
            ControllerShown = True
        End If
    End Sub

    ''' <summary>
    ''' Return sub for the Delegate controller input request screen.
    ''' </summary>
    Public Sub GetControllerInput(ByVal input As String)
        Me.currentText = input
    End Sub

    ''' <summary>
    ''' Inserts a new message to the NewMessages query.
    ''' </summary>
    Public Shared Sub InsertNewMessage(ByVal chatMessage As Chat.ChatMessage)
        Select Case chatMessage.MessageType
            Case Chat.ChatMessage.MessageTypes.CommandMessage
                If ChatState = ChatStates.Command Then
                    newMessages.Insert(0, chatMessage)
                    newMessagesDelays.Insert(0, Chat.NEWMESSAGEDELAY)
                End If
            Case Chat.ChatMessage.MessageTypes.GlobalMessage
                If ChatState = ChatStates.Global Then
                    newMessages.Insert(0, chatMessage)
                    newMessagesDelays.Insert(0, Chat.NEWMESSAGEDELAY)
                End If
            Case Chat.ChatMessage.MessageTypes.PMMessage
                If ChatState = ChatStates.PM Then
                    If CurrentPMChat = chatMessage.Sender Or CurrentPMChat = chatMessage.PMChatInclude Then
                        newMessages.Insert(0, chatMessage)
                        newMessagesDelays.Insert(0, Chat.NEWMESSAGEDELAY)
                    End If
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Enters a new PM chat.
    ''' </summary>
    Private Sub EnterPMChat(ByVal newPMChatName As String)
        If PMChats.Keys.Contains(newPMChatName) = False Then
            PMChats.Add(newPMChatName, False)
        End If
        CurrentPMChat = newPMChatName
        ChangeChatState(ChatStates.PM)
    End Sub

    ''' <summary>
    ''' Called when the client receives a PM message from the server.
    ''' </summary>
    Public Shared Sub ReceivedPMMessage(ByVal chatMessage As Chat.ChatMessage)
        If PMChats.Keys.Contains(chatMessage.Sender) = False Then
            PMChats.Add(chatMessage.Sender, True)
        Else
            If CurrentPMChat <> chatMessage.Sender Or ChatState <> ChatStates.PM Then
                PMChats(chatMessage.Sender) = True
            End If
        End If
    End Sub

    Public Shared Sub ReceivedGlobalMessage()
        If ChatState <> ChatStates.Global Then
            HasNewGlobalMessages = True
        End If
    End Sub

    Public Sub ChangeChatState(ByVal newChatState As ChatStates)
        If ChatState <> newChatState Then
            ChatState = newChatState
            newMessages.Clear()
            newMessagesDelays.Clear()
            Select Case ChatState
                Case ChatStates.Global
                    HasNewGlobalMessages = False
                Case ChatStates.PM
                    If PMChats.Keys.Contains(CurrentPMChat) = True Then
                        PMChats(CurrentPMChat) = False
                    End If
            End Select
        End If

        scrollIndex = 0
        selectIndex = -1
    End Sub

    Public Shared Sub ClosePMChat(ByVal PMChatName As String)
        If PMChats.ContainsKey(PMChatName) = True Then
            If CurrentPMChat <> PMChatName Or ChatState <> ChatStates.PM Then
                PMChats(PMChatName) = True
            End If
            Chat.AddLine(New Chat.ChatMessage("[SERVER]", PMChatName & " quit the game!", "", Chat.ChatMessage.MessageTypes.PMMessage) With {.PMChatInclude = PMChatName})
        End If
    End Sub

End Class
