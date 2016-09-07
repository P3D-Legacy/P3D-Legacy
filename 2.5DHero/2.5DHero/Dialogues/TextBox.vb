Public Class TextBox

    Public Shared ReadOnly DefaultColor As Color = New Color(16, 24, 32)
    Public Shared ReadOnly PlayerColor As Color = New Color(0, 0, 180)

    Public Shared TextSpeed As Integer = 0

    Public Text As String
    Dim currentChar As Integer = 0
    Dim currentLine As Integer = 0
    Dim fullLines As Integer = 0
    Dim through As Boolean = False
    Dim clearNextLine As Boolean = False

    Dim showText(2) As String

    Public Showing As Boolean = False
    Dim Delay As Single = 0.2F
    Dim doReDelay As Boolean = True
    Public reDelay As Single = 1.5F
    Public PositionY As Single = 0
    Public CanProceed As Boolean = True
    Public TextColor As Color = New Color(16, 24, 33)

    Public TextFont As FontContainer = FontManager.GetFontContainer("textfont")

    Dim Entities() As Entity

    Dim ResultFunction As ChooseBox.DoAnswer = Nothing

    Public Delegate Sub FollowUpDelegate()
    Public FollowUp As FollowUpDelegate = Nothing

    Public Sub Show(ByVal Text As String, ByVal ResultFunction As ChooseBox.DoAnswer, ByVal doReDelay As Boolean, ByVal CheckDelay As Boolean, ByVal TextColor As Color)
        If reDelay = 0.0F Or CheckDelay = False Then
            If Me.Showing = False Then
                PositionY = Core.windowSize.Height
                Showing = True
            End If
            Me.doReDelay = doReDelay
            Me.Text = Text
            Me.ResultFunction = ResultFunction
            Me.TextColor = TextColor
            showText(0) = ""
            showText(1) = ""
            through = False
            currentLine = 0
            currentChar = 0
            Delay = 0.2F
            clearNextLine = False

            FormatText()
        End If
    End Sub

    Public Sub Show(ByVal Text As String, ByVal Entities() As Entity, ByVal doReDelay As Boolean, ByVal CheckDelay As Boolean, ByVal TextColor As Color)
        If reDelay = 0.0F Or CheckDelay = False Then
            If Me.Showing = False Then
                PositionY = Core.windowSize.Height
                Showing = True
            End If
            Me.doReDelay = doReDelay
            Me.Text = Text
            Me.Entities = Entities
            Me.TextColor = TextColor
            showText(0) = ""
            showText(1) = ""
            through = False
            currentLine = 0
            currentChar = 0
            Delay = 0.2F
            clearNextLine = False

            FormatText()
        End If
    End Sub

    Public Sub Show(ByVal Text As String, ByVal Entities() As Entity, ByVal doReDelay As Boolean, ByVal CheckDelay As Boolean)
        Me.Show(Text, Entities, doReDelay, CheckDelay, Me.TextColor)
    End Sub

    Public Sub Show(ByVal Text As String, ByVal Entities() As Entity, ByVal doReDelay As Boolean)
        Me.Show(Text, Entities, doReDelay, True)
    End Sub

    Public Sub Show(ByVal Text As String)
        Me.Show(Text, {}, False, False)
    End Sub

    Public Sub Hide()
        Showing = False
        If Me.doReDelay = True Then
            Me.reDelay = 1.0F
        End If
    End Sub

    Public Sub Show(ByVal Text As String, ByVal Entities() As Entity)
        Me.Show(Text, Entities, True)
    End Sub

    Private Sub FormatText()
        Dim tokenSearchBuffer As String() = Me.Text.Split(CChar("<"))
        Dim tokenEndIdx As Integer = 0
        Dim validToken As String = ""
        Dim token As Token = Nothing
        For Each possibleToken As String In tokenSearchBuffer
            tokenEndIdx = possibleToken.IndexOf(">")
            If Not tokenEndIdx = -1 Then
                validToken = possibleToken.Substring(0, tokenEndIdx)
                If Localization.LocalizationTokens.ContainsKey(validToken) = True Then
                    If Localization.LocalizationTokens.TryGetValue(validToken, token) = True Then
                        Me.Text = Me.Text.Replace("<" & validToken & ">", token.TokenContent)
                    End If
                End If
            End If
        Next

        Me.Text = Me.Text.Replace("<playername>", Core.Player.Name)
        Me.Text = Me.Text.Replace("<rivalname>", Core.Player.RivalName)

        Me.Text = Me.Text.Replace("[POKE]", "Poké")
        Me.Text = Me.Text.Replace("[POKEMON]", "Pokémon")
    End Sub

    Public Sub Update()
        If Showing = True Then
            ResetCursor()
            If PositionY <= Core.windowSize.Height - 160.0F Then
                If through = False Then
                    If Text.Count > currentChar Then
                        If Delay <= 0.0F Then
                            If Text(currentChar).ToString() = "\" Then
                                If Text.Count > currentChar + 1 Then
                                    showText(currentLine) &= Text(currentChar + 1)

                                    currentChar += 2
                                Else
                                    currentChar += 1
                                End If
                            Else
                                Select Case Text(currentChar)
                                    Case CChar("~")
                                        If currentLine = 1 Then
                                            through = True
                                        Else
                                            currentLine += 1
                                        End If
                                    Case CChar("*")
                                        currentLine = 0
                                        clearNextLine = True
                                        through = True
                                    Case CChar("%")
                                        ProcessChooseBox()
                                    Case Else
                                        showText(currentLine) &= Text(currentChar)
                                End Select

                                currentChar += 1
                            End If

                            If KeyBoardHandler.KeyDown(KeyBindings.EnterKey1) Or KeyBoardHandler.KeyDown(KeyBindings.EnterKey2) Or MouseHandler.ButtonDown(MouseHandler.MouseButtons.LeftButton) = True Or ControllerHandler.ButtonDown(Buttons.A) = True Or ControllerHandler.ButtonDown(Buttons.B) = True Then
                                Delay = 0.0F
                            Else
                                Delay = GetTextSpeed()
                            End If
                        Else
                            Delay -= 0.1F
                        End If
                    Else
                        through = True
                    End If
                Else
                    If Controls.Accept() Or Controls.Dismiss() Then
                        SoundManager.PlaySound("select")
                        If Text.Count <= currentChar Then
                            If CanProceed = True Then
                                Showing = False
                                ResetCursor()

                                If Not Me.FollowUp Is Nothing Then
                                    Me.FollowUp()
                                    Me.FollowUp = Nothing
                                End If

                                Me.TextFont = FontManager.GetFontContainer("textfont")
                                Me.TextColor = TextBox.DefaultColor
                                If Me.doReDelay = True Then
                                    Me.reDelay = 1.0F
                                End If
                            End If
                        Else
                            If clearNextLine = True Then
                                showText(0) = ""
                            Else
                                showText(0) = showText(1)
                            End If
                            showText(1) = ""
                            through = False
                            clearNextLine = False
                        End If
                    End If
                End If
            Else
                Dim ySpeed As Single = 3.5F
                Select Case TextSpeed
                    Case 1
                        ySpeed = 3.5F
                    Case 2
                        ySpeed = 4.5F
                    Case 3
                        ySpeed = 6.5F
                End Select
                Me.PositionY -= ySpeed
            End If
        Else
            If reDelay > 0.0F Then
                reDelay -= 0.1F
                If reDelay <= 0.0F Then
                    reDelay = 0.0F
                End If
            End If
        End If
    End Sub

    Private Sub ResetCursor()
        If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)
            Mouse.SetPosition(CInt(Core.windowSize.Width / 2), CInt(Core.windowSize.Height / 2))
            c.oldX = CInt(Core.windowSize.Width / 2)
            c.oldY = CInt(Core.windowSize.Height / 2)
        End If
    End Sub

    Public Sub Draw()
        If Me.Showing = True Then
            With Core.SpriteBatch
                .Draw(TextureManager.GetTexture("GUI\Overworld\TextBox"), New Rectangle(CInt(Core.windowSize.Width / 2) - 240, CInt(PositionY), 480, 144), New Rectangle(0, 0, 160, 48), Color.White)

                Dim m As Single = 1.0F
                Select Case Me.TextFont.FontName.ToLower()
                    Case "textfont", "braille"
                        m = 2.0F
                End Select

                .DrawString(Me.TextFont.SpriteFont, Me.showText(0), New Vector2(CInt(Core.windowSize.Width / 2) - 210, CInt(PositionY) + 40), Me.TextColor, 0.0F, Vector2.Zero, m, SpriteEffects.None, 0.0F)
                .DrawString(Me.TextFont.SpriteFont, Me.showText(1), New Vector2(CInt(Core.windowSize.Width / 2) - 210, CInt(PositionY) + 75), Me.TextColor, 0.0F, Vector2.Zero, m, SpriteEffects.None, 0.0F)

                If Me.CanProceed = True And Me.through = True Then
                    .Draw(TextureManager.GetTexture("GUI\Overworld\TextBox"), New Rectangle(CInt(Core.windowSize.Width / 2) + 192, CInt(PositionY) + 128, 16, 16), New Rectangle(0, 48, 16, 16), Color.White)
                End If
            End With
        End If
    End Sub

    Private Sub ProcessChooseBox()
        Dim SplitText As String = Text.Remove(0, currentChar + 1)
        SplitText = SplitText.Remove(SplitText.IndexOf("%"))
        through = True
        Dim Options() As String = SplitText.Split(CChar("|"))
        Text = Text.Remove(currentChar, SplitText.Length + 1)
        If Me.Entities Is Nothing And Not Me.ResultFunction Is Nothing OrElse Me.Entities.Count = 0 And Not Me.ResultFunction Is Nothing Then
            Screen.ChooseBox.Show(Options, Me.ResultFunction)
        Else
            Screen.ChooseBox.Show(Options, 0, Entities)
        End If
        Screen.ChooseBox.TextFont = Me.TextFont
    End Sub

    Private Function GetTextSpeed() As Single
        Select Case TextSpeed
            Case 1
                Return 0.3F
            Case 2
                Return 0.2F
            Case 3
                Return 0.1F
        End Select
        Return 0.2F
    End Function

End Class