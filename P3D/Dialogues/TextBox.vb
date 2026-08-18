Public Class TextBox

    Public Shared ReadOnly DefaultColor As Color = New Color(16, 24, 32)
    Public Shared ReadOnly PlayerColor As Color = New Color(0, 0, 180)

    Public Shared TextSpeed As Integer = 2

    Public Text As String
    Dim currentChar As Integer = 0
    Dim currentLine As Integer = 0
    Dim fullLines As Integer = 0
    Dim through As Boolean = False
    Dim ProcessChooseBoxAfterConfirm As Integer = 0 '' 0 = don't process, 1 = start process, 2 = check if it's gone after starting
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

    Public ResultFunction As ChooseBox.DoAnswer = Nothing

    Public Delegate Sub FollowUpDelegate()
    Public FollowUp As FollowUpDelegate = Nothing

    Public Sub Show(ByVal Text As String, ByVal ResultFunction As ChooseBox.DoAnswer, ByVal doReDelay As Boolean, ByVal CheckDelay As Boolean, ByVal TextColor As Color)
        If reDelay = 0.0F Or CheckDelay = False Then
            If Me.Showing = False Then
                If Core.GameOptions.BlindMode = True Then
                    PositionY = Core.windowSize.Height - CSng(160.0F * Math.Ceiling(Core.SpriteBatch.InterfaceScale))
                Else
                    PositionY = Core.windowSize.Height
                End If
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
                If Core.GameOptions.BlindMode = True Then
                    PositionY = Core.windowSize.Height - CSng(160.0F * Math.Ceiling(Core.SpriteBatch.InterfaceScale))
                Else
                    PositionY = Core.windowSize.Height
                End If
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
        Me.Text = Me.Text.Replace("<playername>", Core.Player.Name)
        Me.Text = Me.Text.Replace("<player.name>", Core.Player.Name)

        Me.Text = Me.Text.Replace("<rivalname>", Core.Player.RivalName)
        Me.Text = Me.Text.Replace("<rival.name>", Core.Player.RivalName)

        Me.Text = Me.Text.Replace("[POKE]", "Poké")
        Me.Text = Me.Text.Replace("[POKEMON]", "Pokémon")

        Dim ClockTime = New DateTime(My.Computer.Clock.LocalTime.Year, My.Computer.Clock.LocalTime.Month, My.Computer.Clock.LocalTime.Day, My.Computer.Clock.LocalTime.Hour, My.Computer.Clock.LocalTime.Minute, My.Computer.Clock.LocalTime.Second)
        Me.Text = Me.Text.Replace("<clocktime>", ClockTime.ToString("t", New System.Globalization.CultureInfo("en-US")))
        Me.Text = Me.Text.Replace("<daytime>", World.GetTime.ToString)
    End Sub

    Public Sub Update()
        If Showing = True Then
            ResetCursor()
            If PositionY <= Core.windowSize.Height - CSng(160.0F * Math.Ceiling(Core.SpriteBatch.InterfaceScale)) Then
                If through = False Then
                    If Text.Count > currentChar Then
                        If Delay <= 0.0F Then
                            If Core.GameOptions.BlindMode = True Then
                                TextBox.TextSpeed = 5
                            End If
                            If TextBox.TextSpeed = 5 Then
                                Dim line As String = Text.Remove(0, currentChar)
                                Dim line1 As String = ""
                                Dim line2 As String = ""

                                Dim specialSymbolIndex As Integer = -1
                                Dim softLineBreakIndex As Integer = -1
                                If line.StartsWith("~") OrElse line.StartsWith("*") OrElse line.StartsWith("%") = True Then
                                    line = line.Remove(0, 1)
                                End If

                                If line.Contains(CChar("~")) Then
                                    softLineBreakIndex = line.IndexOf(CChar("~"))
                                End If
                                If line.Contains(CChar("*")) Then
                                    specialSymbolIndex = line.IndexOf(CChar("*"))
                                End If
                                If line.Contains(CChar("%")) Then
                                    If specialSymbolIndex = -1 OrElse line.IndexOf(CChar("%")) < specialSymbolIndex Then
                                        specialSymbolIndex = line.IndexOf(CChar("%"))
                                    End If
                                End If


                                    If Core.GameOptions.BlindMode = True Then
                                    If softLineBreakIndex <> -1 Then
                                        If specialSymbolIndex <> -1 Then
                                            If softLineBreakIndex < specialSymbolIndex Then
                                                line2 = line.Remove(0, line.IndexOf("~") + 1)
                                                line1 = line.Remove(line.IndexOf("~"))
                                            End If
                                        Else
                                            line2 = line.Remove(0, line.IndexOf("~") + 1)
                                            line1 = line.Remove(line.IndexOf("~"))
                                        End If
                                    End If
                                Else
                                    If currentLine > 0 Then
                                        If softLineBreakIndex <> -1 Then
                                            If specialSymbolIndex <> -1 Then
                                                If softLineBreakIndex < specialSymbolIndex Then
                                                    line1 = showText(0)
                                                    line2 = line.Remove(line.IndexOf("~") + 1)
                                                    specialSymbolIndex = -1
                                                Else
                                                    line1 = showText(0)
                                                    line2 = line
                                                End If
                                            Else
                                                line1 = showText(0)
                                                line2 = line.Remove(line.IndexOf("~") + 1)
                                                line = line2
                                            End If
                                        Else
                                            line1 = showText(0)
                                            line2 = line
                                        End If
                                    Else
                                        If softLineBreakIndex <> -1 Then
                                            If specialSymbolIndex <> -1 Then
                                                If softLineBreakIndex < specialSymbolIndex Then
                                                    line2 = line.Remove(0, line.IndexOf("~") + 1)
                                                    line1 = line.Remove(line.IndexOf("~"))
                                                End If
                                            Else
                                                line2 = line.Remove(0, line.IndexOf("~") + 1)
                                                line1 = line.Remove(line.IndexOf("~"))
                                            End If
                                        End If
                                    End If
                                End If

                                If specialSymbolIndex <> -1 Then
                                    Select Case line(specialSymbolIndex)
                                        Case CChar("*")
                                            Dim FoundSoftLineBreak = False
                                            If line2 <> "" Then
                                                line2 = line2.Remove(line2.IndexOf(CChar("*")))

                                                If line2.Contains(CChar("~")) AndAlso line2.IndexOf(CChar("~")) < specialSymbolIndex Then
                                                    If softLineBreakIndex <> -1 Then
                                                        specialSymbolIndex = line.IndexOf(CChar("~"), softLineBreakIndex + 1)
                                                    Else
                                                        specialSymbolIndex += line2.IndexOf(CChar("~"))
                                                    End If
                                                    line2 = line2.Remove(line2.IndexOf(CChar("~")))
                                                    FoundSoftLineBreak = True
                                                End If
                                                line = line1 & " " & line2
                                                showText(0) = line1
                                                showText(1) = line2
                                            Else
                                                specialSymbolIndex = line.IndexOf(CChar("*"))
                                                line = line.Remove(line.IndexOf(CChar("*")))
                                                showText(0) = line
                                                showText(1) = ""
                                            End If
                                            If Core.GameOptions.BlindMode = True Then
                                                NVDA.Speak(line)
                                            End If

                                            currentChar += specialSymbolIndex + 1
                                            If FoundSoftLineBreak = False Then
                                                currentLine = 0
                                                clearNextLine = True
                                            Else
                                                currentLine += 1
                                            End If
                                            through = True
                                        Case CChar("%")
                                            Dim FoundSoftLineBreak = False
                                            If line2 <> "" Then
                                                line2 = line2.Remove(line2.IndexOf(CChar("%")))
                                                If line2.Contains(CChar("~")) AndAlso line2.IndexOf(CChar("~")) < specialSymbolIndex Then
                                                    If softLineBreakIndex <> -1 Then
                                                        specialSymbolIndex = line.IndexOf(CChar("~"), softLineBreakIndex + 1)
                                                    Else
                                                        specialSymbolIndex += line2.IndexOf(CChar("~"))
                                                    End If
                                                    line2 = line2.Remove(line2.IndexOf(CChar("~")))
                                                    FoundSoftLineBreak = True
                                                End If

                                                line = line1 & " " & line2
                                                showText(0) = line1
                                                showText(1) = line2
                                                If Core.GameOptions.BlindMode = True Then
                                                    NVDA.Speak(line)
                                                End If

                                                If FoundSoftLineBreak = False Then
                                                    currentChar += specialSymbolIndex
                                                    If Core.GameOptions.BlindMode = True Then
                                                        ProcessChooseBoxAfterConfirm = 1
                                                        through = True
                                                    Else
                                                        ProcessChooseBox()
                                                    End If
                                                Else
                                                    currentChar += specialSymbolIndex + 1
                                                    If Core.GameOptions.BlindMode = True Then
                                                        currentLine = 0
                                                        clearNextLine = True
                                                    Else
                                                        currentLine += 1
                                                    End If
                                                    through = True
                                                End If
                                            Else
                                                specialSymbolIndex = line.IndexOf(CChar("%"))
                                                line = line.Remove(line.IndexOf(CChar("%")))
                                                showText(0) = line
                                                showText(1) = ""

                                                If Core.GameOptions.BlindMode = True Then
                                                    NVDA.Speak(line)
                                                End If
                                                currentChar += specialSymbolIndex
                                                If Core.GameOptions.BlindMode = True Then
                                                    ProcessChooseBoxAfterConfirm = 1
                                                    through = True
                                                Else
                                                    ProcessChooseBox()
                                                End If
                                            End If
                                    End Select

                                Else
                                    Dim addChar As Integer = 0
                                    If line2 <> "" Then
                                        If line2.Contains(CChar("~")) Then
                                            addChar = 1
                                            line2 = line2.Remove(line2.IndexOf(CChar("~")))
                                        End If

                                        line = line1 & " " & line2
                                        showText(0) = line1
                                        showText(1) = line2
                                    Else
                                        showText(0) = line
                                        showText(1) = ""
                                    End If
                                    If Core.GameOptions.BlindMode = True Then
                                        NVDA.Speak(line)
                                    End If
                                    If addChar > 0 Then
                                        If Core.GameOptions.BlindMode = True Then
                                            currentChar += line.Length + addChar
                                            currentLine = 0
                                            clearNextLine = True
                                            through = True
                                        Else
                                            If currentLine = 0 Then
                                                currentChar += line.Length + addChar
                                            Else
                                                currentChar += line2.Length + addChar
                                            End If
                                            currentLine += 1
                                            through = True
                                        End If
                                    Else
                                        currentChar += line.Length + addChar
                                    End If
                                End If
                            Else
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
                            If Core.GameOptions.BlindMode = True Then
                                If ProcessChooseBoxAfterConfirm = 0 Then
                                    showText(0) = ""
                                    showText(1) = ""
                                    NVDA.CancelSpeech()
                                Else
                                    NVDA.CancelSpeech()
                                    ProcessChooseBox()
                                End If
                            Else
                                If clearNextLine = True Then
                                    showText(0) = ""
                                Else
                                    showText(0) = showText(1)
                                End If
                                showText(1) = ""
                            End If
                            through = False
                            clearNextLine = False
                        End If
                    Else
                        If ProcessChooseBoxAfterConfirm = 2 AndAlso Screen.ChooseBox.Showing = False Then
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
                            End If
                            ProcessChooseBoxAfterConfirm = 0
                        End If

                    End If
                End If
            Else
                If Core.GameOptions.BlindMode = False Then
                    Me.PositionY -= CSng(8.0F * Core.SpriteBatch.InterfaceScale)
                End If
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
            c.oldMousePos = New Vector2(CInt(windowSize.Width / 2), CInt(windowSize.Height / 2))
        End If
    End Sub

    Public Sub Draw()
        If Me.Showing = True Then
            With Core.SpriteBatch
                .Draw(TextureManager.GetTexture("GUI\Overworld\TextBox"), New Rectangle(CInt(Core.windowSize.Width / 2) - CInt(240 * Math.Ceiling(Core.SpriteBatch.InterfaceScale)), CInt(PositionY), CInt(480 * Math.Ceiling(Core.SpriteBatch.InterfaceScale)), CInt(144 * Math.Ceiling(Core.SpriteBatch.InterfaceScale))), New Rectangle(0, 0, 160, 48), Color.White)

                If Me.CanProceed = True And Me.through = True Then
                    .Draw(TextureManager.GetTexture("GUI\Overworld\TextBox"), New Rectangle(CInt(Core.windowSize.Width / 2) + CInt(240 * Math.Ceiling(Core.SpriteBatch.InterfaceScale)) - CInt(48 * Math.Ceiling(Core.SpriteBatch.InterfaceScale)), CInt(PositionY) + CInt(144 * Math.Ceiling(Core.SpriteBatch.InterfaceScale)) - CInt(24 * Math.Ceiling(Core.SpriteBatch.InterfaceScale)), CInt(24 * Math.Ceiling(Core.SpriteBatch.InterfaceScale)), CInt(24 * Math.Ceiling(Core.SpriteBatch.InterfaceScale))), New Rectangle(0, 48, 24, 24), Color.White)
                End If

                Dim m As Single = 1.0F
                Select Case Me.TextFont.FontName.ToLower()
                    Case "textfont", "braille"
                        m = 2.0F
                End Select

                m = CInt(m * Math.Ceiling(Core.SpriteBatch.InterfaceScale))
                .DrawString(Me.TextFont.SpriteFont, Me.showText(0), New Vector2(CInt(Core.windowSize.Width / 2) - CInt(210 * Math.Ceiling(Core.SpriteBatch.InterfaceScale)), CInt(PositionY) + CInt(40 * Math.Ceiling(Core.SpriteBatch.InterfaceScale))), Me.TextColor, 0.0F, Vector2.Zero, m, SpriteEffects.None, 0.0F)
                .DrawString(Me.TextFont.SpriteFont, Me.showText(1), New Vector2(CInt(Core.windowSize.Width / 2) - CInt(210 * Math.Ceiling(Core.SpriteBatch.InterfaceScale)), CInt(PositionY) + CInt(75 * Math.Ceiling(Core.SpriteBatch.InterfaceScale))), Me.TextColor, 0.0F, Vector2.Zero, m, SpriteEffects.None, 0.0F)

            End With
        End If
    End Sub

    Private Sub ProcessChooseBox()
        Dim SplitText As String = Text.Remove(0, currentChar + 1)
        SplitText = SplitText.Remove(SplitText.IndexOf("%"))
        through = True
        Dim Options() As String = SplitText.Split(CChar("|"))

        Dim AddOne As Integer = 0
        If TextSpeed = 5 Then
            AddOne = 1
        End If
        Text = Text.Remove(currentChar, SplitText.Length + 1 + AddOne)
        If Me.Entities Is Nothing And Not Me.ResultFunction Is Nothing OrElse Me.Entities.Count = 0 And Not Me.ResultFunction Is Nothing Then
            Screen.ChooseBox.Show(Options, Me.ResultFunction)
        Else
            Screen.ChooseBox.Show(Options, 0, Entities)
        End If
        Screen.ChooseBox.TextFont = Me.TextFont
        If ProcessChooseBoxAfterConfirm > 0 Then
            ProcessChooseBoxAfterConfirm = 2
        End If
    End Sub

    Private Function GetTextSpeed() As Single
        Select Case TextSpeed
            Case 1
                Return 0.3F
            Case 2
                Return 0.2F
            Case 3
                Return 0.1F
            Case 4
                Return 0.0F
            Case 5
                Return 0.0F
        End Select
        Return 0.2F
    End Function

End Class