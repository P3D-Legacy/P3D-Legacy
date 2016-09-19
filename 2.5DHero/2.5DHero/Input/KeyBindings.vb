Public Class KeyBindings

    Public Shared ForwardMoveKey As Keys = Keys.W
    Public Shared LeftMoveKey As Keys = Keys.A
    Public Shared BackwardMoveKey As Keys = Keys.S
    Public Shared RightMoveKey As Keys = Keys.D

    Public Shared OpenInventoryKey As Keys = Keys.E
    Public Shared ChatKey As Keys = Keys.T
    Public Shared SpecialKey As Keys = Keys.Q

    Public Shared UpKey As Keys = Keys.Up
    Public Shared DownKey As Keys = Keys.Down
    Public Shared RightKey As Keys = Keys.Right
    Public Shared LeftKey As Keys = Keys.Left

    Public Shared CameraLockKey As Keys = Keys.C
    Public Shared MuteMusicKey As Keys = Keys.M
    Public Shared OnlineStatusKey As Keys = Keys.Tab

    Public Shared GUIControlKey As Keys = Keys.F1
    Public Shared ScreenshotKey As Keys = Keys.F2
    Public Shared DebugKey As Keys = Keys.F3
    Public Shared LightKey As Keys = Keys.F4
    Public Shared PerspectiveSwitchKey As Keys = Keys.F5
    Public Shared FullScreenKey As Keys = Keys.F11

    Public Shared EnterKey1 As Keys = Keys.Enter
    Public Shared EnterKey2 As Keys = Keys.Space
    Public Shared BackKey1 As Keys = Keys.E
    Public Shared BackKey2 As Keys = Keys.E
    Public Shared EscapeKey As Keys = Keys.Escape

    Public Shared Sub LoadKeys()
        'Check if the Keyboard.dat file exists in the save folder:
        If IO.File.Exists(GameController.GamePath & "\Save\Keyboard.dat") = True Then
            'Read lines from the file and try to assign the key to the correct Key field.
            Dim Lines() As String = IO.File.ReadAllLines(GameController.GamePath & "\Save\Keyboard.dat")

            For Each line As String In Lines
                If line.StartsWith("[") = True Then
                    Dim key As String = line.GetSplit(0, "=")
                    Dim binding As Keys = GetKey(line.GetSplit(1, "="))

                    key = key.Remove(0, 1)
                    key = key.Remove(key.Length - 1, 1)

                    Select Case key.ToLower()
                        Case "forwardmove"
                            ForwardMoveKey = binding
                        Case "leftmove"
                            LeftMoveKey = binding
                        Case "backwardmove"
                            BackwardMoveKey = binding
                        Case "rightmove"
                            RightMoveKey = binding
                        Case "inventory"
                            OpenInventoryKey = binding
                        Case "chat"
                            ChatKey = binding
                        Case "special", "pokegear"
                            SpecialKey = binding
                        Case "mutemusic"
                            MuteMusicKey = binding
                        Case "cameraleft"
                            LeftKey = binding
                        Case "cameraright"
                            RightKey = binding
                        Case "cameraup"
                            UpKey = binding
                        Case "cameradown"
                            DownKey = binding
                        Case "cameralock"
                            CameraLockKey = binding
                        Case "guicontrol"
                            GUIControlKey = binding
                        Case "screenshot"
                            ScreenshotKey = binding
                        Case "debugcontrol"
                            DebugKey = binding
                        Case "perspectiveswitch"
                            PerspectiveSwitchKey = binding
                        Case "fullscreen"
                            FullScreenKey = binding
                        Case "enter1"
                            EnterKey1 = binding
                        Case "enter2"
                            EnterKey2 = binding
                        Case "back1"
                            BackKey1 = binding
                        Case "back2"
                            BackKey2 = binding
                        Case "escape", "esc"
                            EscapeKey = binding
                        Case "onlinestatus"
                            OnlineStatusKey = binding
                        Case "lightning"
                            LightKey = binding
                    End Select
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Converts the name of a key to the actual Key class.
    ''' </summary>
    ''' <param name="keyStr">The key name to convert.</param>
    ''' <remarks>The default is Keys.None.</remarks>
    Public Shared Function GetKey(ByVal keyStr As String) As Keys
        For Each k As Keys In [Enum].GetValues(GetType(Keys))
            If k.ToString().ToLower() = keyStr.ToLower() Then
                Return k
            End If
        Next

        Return Keys.None
    End Function

    ''' <summary>
    ''' Returns the name of a key.
    ''' </summary>
    ''' <param name="key">The key to get the name for.</param>
    ''' <remarks>Returns String.Empty by default.</remarks>
    Public Shared Function GetKeyName(ByVal key As Keys) As String
        Return key.ToString()

        Select Case key
            Case Keys.A
                Return "A"
            Case Keys.B
                Return "B"
            Case Keys.C
                Return "C"
            Case Keys.D
                Return "D"
            Case Keys.E
                Return "E"
            Case Keys.F
                Return "F"
            Case Keys.G
                Return "G"
            Case Keys.H
                Return "H"
            Case Keys.I
                Return "I"
            Case Keys.J
                Return "J"
            Case Keys.K
                Return "K"
            Case Keys.L
                Return "L"
            Case Keys.M
                Return "M"
            Case Keys.N
                Return "N"
            Case Keys.O
                Return "O"
            Case Keys.P
                Return "P"
            Case Keys.Q
                Return "Q"
            Case Keys.R
                Return "R"
            Case Keys.S
                Return "S"
            Case Keys.T
                Return "T"
            Case Keys.U
                Return "U"
            Case Keys.V
                Return "V"
            Case Keys.W
                Return "W"
            Case Keys.X
                Return "X"
            Case Keys.Y
                Return "Y"
            Case Keys.Z
                Return "Z"
            Case Keys.F1
                Return "F1"
            Case Keys.F2
                Return "F2"
            Case Keys.F3
                Return "F3"
            Case Keys.F4
                Return "F4"
            Case Keys.F5
                Return "F5"
            Case Keys.F6
                Return "F6"
            Case Keys.F7
                Return "F7"
            Case Keys.F8
                Return "F8"
            Case Keys.F9
                Return "F9"
            Case Keys.F10
                Return "F10"
            Case Keys.F11
                Return "F11"
            Case Keys.F12
                Return "F12"
            Case Keys.Enter
                Return "Enter"
            Case Keys.Space
                Return "Space"
            Case Keys.Escape
                Return "Escape"
            Case Keys.Back
                Return "Back"
            Case Keys.Tab
                Return "Tab"
            Case Keys.Up
                Return "Up"
            Case Keys.Down
                Return "Down"
            Case Keys.Left
                Return "Left"
            Case Keys.Right
                Return "Right"
        End Select

        Return ""
    End Function

    ''' <summary>
    ''' Creates the default keyboard.dat file.
    ''' </summary>
    Public Shared Sub CreateKeySave(ByVal force As Boolean)
        If IO.Directory.Exists(GameController.GamePath & "\Save") = True Then
            If IO.File.Exists(GameController.GamePath & "\Save\Keyboard.dat") = False Or force = True Then
                Dim s As String = "[ForwardMove]=W" & vbNewLine &
                "[LeftMove]=" & GetKeyName(Keys.A) & vbNewLine &
                "[BackwardMove]=" & GetKeyName(Keys.S) & vbNewLine &
                "[RightMove]=" & GetKeyName(Keys.D) & vbNewLine &
                "[Inventory]=" & GetKeyName(Keys.E) & vbNewLine &
                "[Chat]=" & GetKeyName(Keys.T) & vbNewLine &
                "[Special]=" & GetKeyName(Keys.Q) & vbNewLine &
                "[MuteMusic]=" & GetKeyName(Keys.M) & vbNewLine &
                "[Up]=" & GetKeyName(Keys.Up) & vbNewLine &
                "[Down]=" & GetKeyName(Keys.Down) & vbNewLine &
                "[Left]=" & GetKeyName(Keys.Left) & vbNewLine &
                "[Right]=" & GetKeyName(Keys.Right) & vbNewLine &
                "[CameraLock]=" & GetKeyName(Keys.C) & vbNewLine &
                "[GUIControl]=" & GetKeyName(Keys.F1) & vbNewLine &
                "[ScreenShot]=" & GetKeyName(Keys.F2) & vbNewLine &
                "[DebugControl]=" & GetKeyName(Keys.F3) & vbNewLine &
                "[LightKey]=" & GetKeyName(Keys.F4) & vbNewLine &
                "[PerspectiveSwitch]=" & GetKeyName(Keys.F5) & vbNewLine &
                "[FullScreen]=" & GetKeyName(Keys.F11) & vbNewLine &
                "[Enter1]=" & GetKeyName(Keys.Enter) & vbNewLine &
                "[Enter2]=" & GetKeyName(Keys.Space) & vbNewLine &
                "[Back1]=" & GetKeyName(Keys.E) & vbNewLine &
                "[Back2]=" & GetKeyName(Keys.E) & vbNewLine &
                "[Escape]=" & GetKeyName(Keys.Escape) & vbNewLine &
                "[OnlineStatus]=" & GetKeyName(Keys.Tab)
                IO.File.WriteAllText(GameController.GamePath & "\Save\Keyboard.dat", s)
            End If
        End If
    End Sub

    ''' <summary>
    ''' Saves the current keyboard configuration to the keyboard.dat file.
    ''' </summary>
    Public Shared Sub SaveKeys()
        If IO.Directory.Exists(GameController.GamePath & "\Save") = True Then
            Dim s As String = "[ForwardMove]=" & GetKeyName(ForwardMoveKey) & vbNewLine &
                "[LeftMove]=" & GetKeyName(LeftMoveKey) & vbNewLine &
                "[BackwardMove]=" & GetKeyName(BackwardMoveKey) & vbNewLine &
                "[RightMove]=" & GetKeyName(RightMoveKey) & vbNewLine &
                "[Inventory]=" & GetKeyName(OpenInventoryKey) & vbNewLine &
                "[Chat]=" & GetKeyName(ChatKey) & vbNewLine &
                "[Special]=" & GetKeyName(SpecialKey) & vbNewLine &
                "[MuteMusic]=" & GetKeyName(MuteMusicKey) & vbNewLine &
                "[Up]=" & GetKeyName(UpKey) & vbNewLine &
                "[Down]=" & GetKeyName(DownKey) & vbNewLine &
                "[Left]=" & GetKeyName(LeftKey) & vbNewLine &
                "[Right]=" & GetKeyName(RightKey) & vbNewLine &
                "[CameraLock]=" & GetKeyName(CameraLockKey) & vbNewLine &
                "[GUIControl]=" & GetKeyName(GUIControlKey) & vbNewLine &
                "[ScreenShot]=" & GetKeyName(ScreenshotKey) & vbNewLine &
                "[DebugControl]=" & GetKeyName(DebugKey) & vbNewLine &
                "[LightKey]=" & GetKeyName(LightKey) & vbNewLine &
                "[PerspectiveSwitch]=" & GetKeyName(PerspectiveSwitchKey) & vbNewLine &
                "[FullScreen]=" & GetKeyName(FullScreenKey) & vbNewLine &
                "[Enter1]=" & GetKeyName(EnterKey1) & vbNewLine &
                "[Enter2]=" & GetKeyName(EnterKey2) & vbNewLine &
                "[Back1]=" & GetKeyName(BackKey1) & vbNewLine &
                "[Back2]=" & GetKeyName(BackKey2) & vbNewLine &
                "[Escape]=" & GetKeyName(EscapeKey) & vbNewLine &
                "[OnlineStatus]=" & GetKeyName(OnlineStatusKey)
            IO.File.WriteAllText(GameController.GamePath & "\Save\Keyboard.dat", s)

            Logger.Debug("---Saved Keybindings---")
        End If
    End Sub

    Shared holdDelay As Single = 3.0F
    Shared holdKey As Keys = Keys.A

    ''' <summary>
    ''' Gets keyboard inputs.
    ''' </summary>
    ''' <param name="WhiteKeys">All keys that allow a valid return. If the list is empty, all keys are allowed by default.</param>
    ''' <param name="BlackKeys">All keys that are not allowed for a valid return. If this list is empty, all keys are allowed by default.</param>
    ''' <param name="Text">The current text that this function adds text to.</param>
    ''' <param name="MaxLength">The maximum length of the text. -1 means infinite length.</param>
    ''' <param name="TriggerShift">Checks if the Shift variant of a key gets considered.</param>
    ''' <param name="TriggerAlt">Checks if the Alt variant of a key gets considered.</param>
    Public Shared Function GetInput(ByVal WhiteKeys() As Keys, ByVal BlackKeys() As Keys, Optional ByRef Text As String = "", Optional ByVal MaxLength As Integer = -1, Optional ByVal TriggerShift As Boolean = True, Optional ByVal TriggerAlt As Boolean = True) As String
        Dim Keys() As Keys = KeyBoardHandler.GetPressedKeys()
        For Each Key As Keys In Keys
            If Key = Input.Keys.V And KeyBoardHandler.KeyPressed(Input.Keys.V) = True And (KeyBoardHandler.KeyDown(Input.Keys.LeftControl) = True Or KeyBoardHandler.KeyDown(Input.Keys.RightControl) = True) = True Then
                If Windows.Forms.Clipboard.ContainsText() = True Then
                    Dim t As String = Windows.Forms.Clipboard.GetText().Replace(vbNewLine, " ")

                    Text &= Windows.Forms.Clipboard.GetText()
                End If
            Else
                If Key <> Input.Keys.Back Then
                    If KeyBlocked(WhiteKeys, BlackKeys, Key) = False Then
                        Dim cc As Char? = KeyCharConverter.GetCharFromKey(Key)
                        If cc.HasValue = True Then
                            If holdDelay <= 0.0F And holdKey = Key Then
                                Text &= cc.ToString()
                            Else
                                If KeyBoardHandler.KeyPressed(Key) = True Then
                                    Text &= cc.ToString()

                                    holdKey = Key
                                    holdDelay = 3.0F
                                End If
                            End If
                        End If
                    End If
                End If
                If Key = Input.Keys.Back And KeyBlocked(WhiteKeys, BlackKeys, Input.Keys.Back) = False Then
                    If holdDelay <= 0.0F And holdKey = Key Then
                        If Text.Length > 0 Then Text = Text.Remove(Text.Length - 1, 1)
                    Else
                        If KeyBoardHandler.KeyPressed(Key) = True Then
                            If Text.Length > 0 Then Text = Text.Remove(Text.Length - 1, 1)

                            holdKey = Key
                            holdDelay = 3.0F
                        End If
                    End If
                End If
            End If
        Next

        If KeyBoardHandler.KeyUp(holdKey) = True Then
            holdDelay = 3.0F
        Else
            holdDelay -= 0.1F
            If holdDelay <= 0.0F Then
                holdDelay = 0.0F
            End If
        End If

        If MaxLength > -1 Then
            While Text.Length > MaxLength
                Text = Text.Remove(Text.Length - 1, 1)
            End While
        End If

        Return Text
    End Function

    ''' <summary>
    ''' Checks if the Key used is blocked by either the key whitelist or key blacklist.
    ''' </summary>
    ''' <param name="WhiteKeys">The key whitelist.</param>
    ''' <param name="BlackKeys">The key blacklist.</param>
    ''' <param name="Key">The key to be checked.</param>
    Private Shared Function KeyBlocked(ByVal WhiteKeys() As Keys, ByVal BlackKeys() As Keys, ByVal Key As Keys) As Boolean
        Dim MapWhite As Boolean = WhiteKeys.Length > 0
        Dim MapBlack As Boolean = BlackKeys.Length > 0

        'Check if key is whitelisted:
        If MapWhite = False Or (MapWhite = True And WhiteKeys.Contains(Key) = True) = True Then
            'Check if key is blacklisted:
            If MapBlack = False Or (MapBlack = True And BlackKeys.Contains(Key) = False) = True Then
                Return False
            End If
        End If
        Return True
    End Function

    ''' <summary>
    ''' Gets default text input.
    ''' </summary>
    ''' <param name="Text">The text to modify.</param>
    ''' <param name="MaxLength">The maximum length of the text. -1 means infinite length.</param>
    ''' <param name="TriggerShift">Checks if the Shift variant of a key gets considered.</param>
    ''' <param name="TriggerAlt">Checks if the Alt variant of a key gets considered.</param>
    Public Shared Function GetInput(Optional ByRef Text As String = "", Optional ByVal MaxLength As Integer = -1, Optional ByVal TriggerShift As Boolean = True, Optional ByVal TriggerAlt As Boolean = True) As String
        Return GetInput({},
                        {Keys.Enter, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Tab, Keys.Delete, Keys.Home, Keys.End, Keys.Escape},
                        Text,
                        MaxLength,
                        TriggerShift,
                        TriggerAlt)
    End Function

    ''' <summary>
    ''' Gets input for names.
    ''' </summary>
    ''' <param name="Text">The text to modify.</param>
    ''' <param name="MaxLength">The maximum length of the text. -1 means infinite length.</param>
    ''' <param name="TriggerShift">Checks if the Shift variant of a key gets considered.</param>
    ''' <param name="TriggerAlt">Checks if the Alt variant of a key gets considered.</param>
    Public Shared Function GetNameInput(Optional ByRef Text As String = "", Optional ByVal MaxLength As Integer = -1, Optional ByVal TriggerShift As Boolean = True, Optional ByVal TriggerAlt As Boolean = True) As String
        Return GetInput({Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.Space, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, Keys.Back, Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L, Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y, Keys.Z},
                        {},
                        Text,
                        MaxLength,
                        TriggerShift,
                        TriggerAlt)
    End Function

End Class
