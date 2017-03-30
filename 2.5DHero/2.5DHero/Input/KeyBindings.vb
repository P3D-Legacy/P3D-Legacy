Public Class KeyBindings

    Private Shared _tempKeyMap As New Dictionary(Of String, Keys)
    Private Shared _dataModel As DataModel.Json.PlayerData.KeyboardSave
    Private Shared _initialized As Boolean = False

    Public Shared Property IsInitialized() As Boolean
        Get
            Return _initialized
        End Get
        Set(value As Boolean)
            _initialized = value
        End Set
    End Property

#Region "Keys"

    Public Shared Property ForwardMoveKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.W
            End If
            Return GetKey(_dataModel.ForwardMove)
        End Get
        Set(value As Keys)
            _dataModel.ForwardMove = value.ToString()
        End Set
    End Property

    Public Shared Property LeftMoveKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.A
            End If
            Return GetKey(_dataModel.LeftMove)
        End Get
        Set(value As Keys)
            _dataModel.LeftMove = value.ToString()
        End Set
    End Property

    Public Shared Property BackwardMoveKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.S
            End If
            Return GetKey(_dataModel.BackwardMove)
        End Get
        Set(value As Keys)
            _dataModel.BackwardMove = value.ToString()
        End Set
    End Property

    Public Shared Property RightMoveKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.D
            End If
            Return GetKey(_dataModel.RightMove)
        End Get
        Set(value As Keys)
            _dataModel.RightMove = value.ToString()
        End Set
    End Property

    Public Shared Property InventoryKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.E
            End If
            Return GetKey(_dataModel.Inventory)
        End Get
        Set(value As Keys)
            _dataModel.Inventory = value.ToString()
        End Set
    End Property

    Public Shared Property ChatKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.T
            End If
            Return GetKey(_dataModel.Chat)
        End Get
        Set(value As Keys)
            _dataModel.Chat = value.ToString()
        End Set
    End Property

    Public Shared Property SpecialKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.Q
            End If
            Return GetKey(_dataModel.Special)
        End Get
        Set(value As Keys)
            _dataModel.Special = value.ToString()
        End Set
    End Property

    Public Shared Property MuteMusicKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.M
            End If
            Return GetKey(_dataModel.MuteMusic)
        End Get
        Set(value As Keys)
            _dataModel.MuteMusic = value.ToString()
        End Set
    End Property

    Public Shared Property UpKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.Up
            End If
            Return GetKey(_dataModel.Up)
        End Get
        Set(value As Keys)
            _dataModel.Up = value.ToString()
        End Set
    End Property

    Public Shared Property DownKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.Down
            End If
            Return GetKey(_dataModel.Down)
        End Get
        Set(value As Keys)
            _dataModel.Down = value.ToString()
        End Set
    End Property

    Public Shared Property LeftKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.Left
            End If
            Return GetKey(_dataModel.Left)
        End Get
        Set(value As Keys)
            _dataModel.Left = value.ToString()
        End Set
    End Property

    Public Shared Property RightKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.Right
            End If
            Return GetKey(_dataModel.Right)
        End Get
        Set(value As Keys)
            _dataModel.Right = value.ToString()
        End Set
    End Property

    Public Shared Property CameraLockKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.C
            End If
            Return GetKey(_dataModel.CameraLock)
        End Get
        Set(value As Keys)
            _dataModel.CameraLock = value.ToString()
        End Set
    End Property

    Public Shared Property GUIControlKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.F1
            End If
            Return GetKey(_dataModel.GUIControl)
        End Get
        Set(value As Keys)
            _dataModel.GUIControl = value.ToString()
        End Set
    End Property

    Public Shared Property ScreenShotKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.F2
            End If
            Return GetKey(_dataModel.ScreenShot)
        End Get
        Set(value As Keys)
            _dataModel.ScreenShot = value.ToString()
        End Set
    End Property

    Public Shared Property DebugKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.F3
            End If
            Return GetKey(_dataModel.DebugControl)
        End Get
        Set(value As Keys)
            _dataModel.DebugControl = value.ToString()
        End Set
    End Property

    Public Shared Property LightKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.F4
            End If
            Return GetKey(_dataModel.Lighting)
        End Get
        Set(value As Keys)
            _dataModel.Lighting = value.ToString()
        End Set
    End Property

    Public Shared Property PerspectiveSwitchKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.F5
            End If
            Return GetKey(_dataModel.PerspectiveSwitch)
        End Get
        Set(value As Keys)
            _dataModel.PerspectiveSwitch = value.ToString()
        End Set
    End Property

    Public Shared Property FullScreenKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.F11
            End If
            Return GetKey(_dataModel.FullScreen)
        End Get
        Set(value As Keys)
            _dataModel.FullScreen = value.ToString()
        End Set
    End Property

    Public Shared Property EnterKey1() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.Enter
            End If
            Return GetKey(_dataModel.Enter1)
        End Get
        Set(value As Keys)
            _dataModel.Enter1 = value.ToString()
        End Set
    End Property

    Public Shared Property EnterKey2() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.Space
            End If
            Return GetKey(_dataModel.Enter2)
        End Get
        Set(value As Keys)
            _dataModel.Enter2 = value.ToString()
        End Set
    End Property

    Public Shared Property BackKey1() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.E
            End If
            Return GetKey(_dataModel.Back1)
        End Get
        Set(value As Keys)
            _dataModel.Back1 = value.ToString()
        End Set
    End Property

    Public Shared Property BackKey2() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.E
            End If
            Return GetKey(_dataModel.Back2)
        End Get
        Set(value As Keys)
            _dataModel.Back2 = value.ToString()
        End Set
    End Property

    Public Shared Property EscapeKey() As Keys
        Get
            If _dataModel Is Nothing Then
                Return Keys.Escape
            End If
            Return GetKey(_dataModel.Escape)
        End Get
        Set(value As Keys)
            _dataModel.Escape = value.ToString()
        End Set
    End Property

    Public Shared Property OnlineStatusKey() As Keys
        Get
            Return GetKey(_dataModel.OnlineStatus)
        End Get
        Set(value As Keys)
            _dataModel.OnlineStatus = value.ToString()
        End Set
    End Property

    Public Shared Property SprintingKey() As Keys
        Get
            Return GetKey(_dataModel.Sprinting)
        End Get
        Set(value As Keys)
            _dataModel.Sprinting = value.ToString()
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Converts the name of a key to the actual enum member.
    ''' </summary>
    ''' <param name="keyStr">The key name to convert.</param>
    ''' <remarks>The default is Keys.None.</remarks>
    Private Shared Function GetKey(ByVal keyStr As String) As Keys
        Dim keyCompare As String = keyStr.ToLower()

        If _tempKeyMap.ContainsKey(keyCompare) = False Then
            For Each k As Keys In [Enum].GetValues(GetType(Keys))
                If k.ToString().ToLower() = keyStr.ToLower() Then
                    _tempKeyMap.Add(keyCompare, k)
                End If
            Next

            If _tempKeyMap.ContainsKey(keyCompare) = False Then
                _tempKeyMap.Add(keyCompare, Keys.None)
            End If
        End If

        Return _tempKeyMap(keyCompare)
    End Function

    Public Shared Sub Load()
        If IO.File.Exists(GameController.GamePath & "\Save\Keyboard.dat") = True Then
            Try
                Dim jsonData As String = IO.File.ReadAllText(GameController.GamePath & "\Save\Keyboard.dat")
                _dataModel = DataModel.Json.JsonDataModel.FromString(Of DataModel.Json.PlayerData.KeyboardSave)(jsonData)
            Catch ex As Exception
                CreateDefaultData(True)
                Save()
            End Try
        Else
            CreateDefaultData(True)
            Save()
        End If
        _initialized = True
    End Sub

    Public Shared Sub Save()
        If _dataModel IsNot Nothing Then
            Dim jsonData As String = DataModel.Json.JsonFormatter.FormatMultiline(_dataModel.ToString(), "    ")

            IO.File.WriteAllText(GameController.GamePath & "\Save\Keyboard.dat", jsonData)

            Logger.Debug("151", "---Saved Keybindings---")
        End If
    End Sub

    ''' <summary>
    ''' Resets the data model to the default key bindings.
    ''' </summary>
    ''' <param name="force"></param>
    Public Shared Sub CreateDefaultData(ByVal force As Boolean)
        If IO.Directory.Exists(GameController.GamePath & "\Save") = True Then
            If IO.File.Exists(GameController.GamePath & "\Save\Keyboard.dat") = False Or force = True Then
                _dataModel = New DataModel.Json.PlayerData.KeyboardSave()

                _dataModel.ForwardMove = "W"
                _dataModel.LeftMove = "A"
                _dataModel.BackwardMove = "S"
                _dataModel.RightMove = "D"
                _dataModel.Inventory = "E"
                _dataModel.Chat = "T"
                _dataModel.Special = "Q"
                _dataModel.MuteMusic = "M"
                _dataModel.Up = "Up"
                _dataModel.Down = "Down"
                _dataModel.Left = "Left"
                _dataModel.Right = "Right"
                _dataModel.CameraLock = "C"
                _dataModel.GUIControl = "F1"
                _dataModel.ScreenShot = "F2"
                _dataModel.DebugControl = "F3"
                _dataModel.Lighting = "F4"
                _dataModel.PerspectiveSwitch = "F5"
                _dataModel.FullScreen = "F11"
                _dataModel.Enter1 = "Enter"
                _dataModel.Enter2 = "Space"
                _dataModel.Back1 = "E"
                _dataModel.Back2 = "E"
                _dataModel.Escape = "Escape"
                _dataModel.OnlineStatus = "Tab"
                _dataModel.Sprinting = "LeftShift"
            End If
        End If
    End Sub

    ''' <summary>
    ''' Sets a key binding by key name.
    ''' </summary>
    ''' <param name="keyName">The key name.</param>
    ''' <param name="keyBinding">The new key binding.</param>
    Public Shared Sub SetKeyByName(ByVal keyName As String, ByVal keyBinding As Keys)
        Select Case keyName
            Case "ForwardMove"
                ForwardMoveKey = keyBinding
            Case "LeftMove"
                LeftMoveKey = keyBinding
            Case "BackwardMove"
                BackwardMoveKey = keyBinding
            Case "RightMove"
                RightMoveKey = keyBinding
            Case "Inventory"
                InventoryKey = keyBinding
            Case "Chat"
                ChatKey = keyBinding
            Case "Special"
                SpecialKey = keyBinding
            Case "MuteMusic"
                MuteMusicKey = keyBinding
            Case "Up"
                UpKey = keyBinding
            Case "Down"
                DownKey = keyBinding
            Case "Left"
                LeftKey = keyBinding
            Case "Right"
                RightKey = keyBinding
            Case "CameraLock"
                CameraLockKey = keyBinding
            Case "GUIControl"
                GUIControlKey = keyBinding
            Case "ScreenShot"
                ScreenShotKey = keyBinding
            Case "DebugControl"
                DebugKey = keyBinding
            Case "Lighting"
                LightKey = keyBinding
            Case "PerspectiveSwitch"
                PerspectiveSwitchKey = keyBinding
            Case "FullScreen"
                FullScreenKey = keyBinding
            Case "Enter1"
                EnterKey1 = keyBinding
            Case "Enter2"
                EnterKey2 = keyBinding
            Case "Back1"
                BackKey1 = keyBinding
            Case "Back2"
                BackKey2 = keyBinding
            Case "Escape"
                EscapeKey = keyBinding
            Case "OnlineStatus"
                OnlineStatusKey = keyBinding
            Case "Sprinting"
                SprintingKey = keyBinding
        End Select
    End Sub

#Region "Legacy"

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

#End Region

End Class
