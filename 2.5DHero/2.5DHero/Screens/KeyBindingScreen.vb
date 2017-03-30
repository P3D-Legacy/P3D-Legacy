''' <summary>
''' A GUI for key binding.
''' </summary>
Public Class KeyBindingScreen

    Inherits Screen

    Private _keys As New List(Of KeyBinding)

    Private _page As Integer = 1

    Public Sub New(ByVal currentScreen As Screen)
        Identification = Identifications.KeyBindingScreen

        PreScreen = currentScreen
        MouseVisible = True
        CanMuteMusic = False
        CanChat = False
        CanDrawDebug = False
        CanGoFullscreen = False
        CanTakeScreenshot = False
        CanBePaused = False

        SetupKeys()
    End Sub

    Private Sub SetupKeys()
        _keys.Clear()

        Select Case _page
            Case 1
                _keys.Add(New KeyBinding("ForwardMove", KeyToScreenPosition(3, 1), KeyBindings.ForwardMoveKey))
                _keys.Add(New KeyBinding("LeftMove", KeyToScreenPosition(3, 2), KeyBindings.LeftMoveKey))
                _keys.Add(New KeyBinding("BackwardMove", KeyToScreenPosition(3, 3), KeyBindings.BackwardMoveKey))
                _keys.Add(New KeyBinding("RightMove", KeyToScreenPosition(3, 4), KeyBindings.RightMoveKey))

                _keys.Add(New KeyBinding("Up", KeyToScreenPosition(9, 1), KeyBindings.UpKey))
                _keys.Add(New KeyBinding("Left", KeyToScreenPosition(9, 2), KeyBindings.LeftKey))
                _keys.Add(New KeyBinding("Down", KeyToScreenPosition(9, 3), KeyBindings.DownKey))
                _keys.Add(New KeyBinding("Right", KeyToScreenPosition(9, 4), KeyBindings.RightKey))
            Case 2
                _keys.Add(New KeyBinding("Inventory", KeyToScreenPosition(3, 1), KeyBindings.InventoryKey))
                _keys.Add(New KeyBinding("Chat", KeyToScreenPosition(3, 2), KeyBindings.ChatKey))
                _keys.Add(New KeyBinding("MuteMusic", KeyToScreenPosition(3, 3), KeyBindings.MuteMusicKey))
                _keys.Add(New KeyBinding("Special", KeyToScreenPosition(3, 4), KeyBindings.SpecialKey))
                _keys.Add(New KeyBinding("Sprinting", KeyToScreenPosition(3, 5), KeyBindings.SprintingKey))

                _keys.Add(New KeyBinding("Screenshot", KeyToScreenPosition(10, 1), KeyBindings.ScreenshotKey))
                _keys.Add(New KeyBinding("PerspectiveSwitch", KeyToScreenPosition(10, 2), KeyBindings.PerspectiveSwitchKey))
                _keys.Add(New KeyBinding("CameraLock", KeyToScreenPosition(10, 3), KeyBindings.CameraLockKey))
                _keys.Add(New KeyBinding("OnlineStatus", KeyToScreenPosition(10, 4), KeyBindings.OnlineStatusKey))
            Case 3
                _keys.Add(New KeyBinding("Enter1", KeyToScreenPosition(3, 1), KeyBindings.EnterKey1))
                _keys.Add(New KeyBinding("Enter2", KeyToScreenPosition(3, 2), KeyBindings.EnterKey2))
                _keys.Add(New KeyBinding("Back1", KeyToScreenPosition(3, 3), KeyBindings.BackKey1))
                _keys.Add(New KeyBinding("Back2", KeyToScreenPosition(3, 4), KeyBindings.BackKey2))
                _keys.Add(New KeyBinding("Escape", KeyToScreenPosition(3, 5), KeyBindings.EscapeKey))

                _keys.Add(New KeyBinding("GUIControl", KeyToScreenPosition(10, 1), KeyBindings.GUIControlKey))
                _keys.Add(New KeyBinding("DebugControl", KeyToScreenPosition(10, 2), KeyBindings.DebugKey))
                _keys.Add(New KeyBinding("Lighting", KeyToScreenPosition(10, 3), KeyBindings.LightKey))
                _keys.Add(New KeyBinding("Fullscreen", KeyToScreenPosition(10, 4), KeyBindings.FullScreenKey))
        End Select
    End Sub

    Private Function KeyToScreenPosition(ByVal x As Integer, ByVal y As Integer) As Vector2
        Return New Vector2(100 + x * 64, 100 + y * 64)
    End Function

    Private Function TextToScreenPosition(ByVal x As Integer, ByVal y As Integer) As Vector2
        Return New Vector2(100 + x * 64, 100 + y * 64 + 20)
    End Function

    Public Overrides Sub Draw()
        PreScreen.Draw()

        Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 210))

        SpriteBatch.DrawString(FontManager.MainFont, "Keyboard Rebinding", New Vector2(30, 48), Color.White)

        DrawGUIText()
        DrawNavigation()

        For Each k As KeyBinding In _keys
            k.Draw()
        Next
    End Sub

    Private Sub DrawGUIText()
        Select Case _page
            Case 1
                SpriteBatch.DrawString(FontManager.MainFont, "Movement & Primary navigation:", TextToScreenPosition(0, 0), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Forward/Up:", TextToScreenPosition(0, 1), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Left:", TextToScreenPosition(0, 2), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Backwards/Down:", TextToScreenPosition(0, 3), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Right:", TextToScreenPosition(0, 4), Color.White)

                SpriteBatch.DrawString(FontManager.MainFont, "Secondary navigation:", TextToScreenPosition(7, 0), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Up:", TextToScreenPosition(7, 1), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Left:", TextToScreenPosition(7, 2), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Down:", TextToScreenPosition(7, 3), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Right:", TextToScreenPosition(7, 4), Color.White)
            Case 2
                SpriteBatch.DrawString(FontManager.MainFont, "Game features:", TextToScreenPosition(0, 0), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Inventory:", TextToScreenPosition(0, 1), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Chat:", TextToScreenPosition(0, 2), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Mute music:", TextToScreenPosition(0, 3), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Special actions:", TextToScreenPosition(0, 4), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Sprinting:", TextToScreenPosition(0, 5), Color.White)

                SpriteBatch.DrawString(FontManager.ChatFont, "Screenshot:", TextToScreenPosition(7, 1), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Perspective:", TextToScreenPosition(7, 2), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Camera Mode:", TextToScreenPosition(7, 3), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Online status:", TextToScreenPosition(7, 4), Color.White)
            Case 3
                SpriteBatch.DrawString(FontManager.MainFont, "Basic navigation:", TextToScreenPosition(0, 0), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Enter Key (1):", TextToScreenPosition(0, 1), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Enter Key (2):", TextToScreenPosition(0, 2), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Back Key (1):", TextToScreenPosition(0, 3), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Back Key (2):", TextToScreenPosition(0, 4), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Game Menu:", TextToScreenPosition(0, 5), Color.White)

                SpriteBatch.DrawString(FontManager.MainFont, "Options:", TextToScreenPosition(7, 0), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "GUI display:", TextToScreenPosition(7, 1), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Debug display:", TextToScreenPosition(7, 2), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Lighting:", TextToScreenPosition(7, 3), Color.White)
                SpriteBatch.DrawString(FontManager.ChatFont, "Fullscreen:", TextToScreenPosition(7, 4), Color.White)
        End Select
    End Sub

    Private Sub DrawNavigation()
        DrawNavigationButton(New Vector2(300, 30), "<< Back")
        DrawNavigationButton(New Vector2(450, 30), "Next >>")
        DrawNavigationButton(New Vector2(Core.windowSize.Width - 150, 30), "Close")
        DrawNavigationButton(New Vector2(Core.windowSize.Width - 300, 30), "Reset")
    End Sub

    Private Sub DrawNavigationButton(ByVal position As Vector2, ByVal text As String)
        Dim widthUnits As Integer = 1
        Dim textSize As Vector2 = FontManager.MainFont.MeasureString(text)

        While widthUnits * 64 - 10 < textSize.X
            widthUnits += 1
        End While

        Canvas.DrawRectangle(New Rectangle(CInt(position.X), CInt(position.Y), widthUnits * 64, 64), New Color(150, 150, 150))
        If KeyBinding.GetRekt(position, text).Contains(MouseHandler.MousePosition) = True And IsCurrentScreen() Then
            Canvas.DrawRectangle(New Rectangle(CInt(position.X) + 3, CInt(position.Y) + 3, widthUnits * 64 - 6, 58), New Color(180, 180, 180))
        Else
            Canvas.DrawRectangle(New Rectangle(CInt(position.X) + 3, CInt(position.Y) + 3, widthUnits * 64 - 6, 58), New Color(210, 210, 210))
        End If

        SpriteBatch.DrawString(FontManager.MainFont, text, New Vector2(position.X + widthUnits * 32 - textSize.X / 2.0F, position.Y + 32 - textSize.Y / 2.0F), Color.Black)
    End Sub

    Public Overrides Sub Update()
        For Each k As KeyBinding In _keys
            k.Update()
        Next

        UpdateNavigation()

        If Controls.Dismiss(True, False, True) = True Then
            SetScreen(PreScreen)
        End If
    End Sub

    Private Sub UpdateNavigation()
        If Controls.Accept(True, False, False) = True Then
            Dim rBack As Rectangle = KeyBinding.GetRekt(New Vector2(300, 30), "<< Back")
            Dim rNext As Rectangle = KeyBinding.GetRekt(New Vector2(450, 30), "Next >>")
            Dim rReset As Rectangle = KeyBinding.GetRekt(New Vector2(Core.windowSize.Width - 300, 30), "Reset")
            Dim rClose As Rectangle = KeyBinding.GetRekt(New Vector2(Core.windowSize.Width - 150, 30), "Close")

            If rBack.Contains(MouseHandler.MousePosition) = True Then
                If _page > 1 Then
                    _page -= 1
                    SetupKeys()
                End If
            End If
            If rNext.Contains(MouseHandler.MousePosition) = True Then
                If _page < 3 Then
                    _page += 1
                    SetupKeys()
                End If
            End If
            If rReset.Contains(MouseHandler.MousePosition) = True Then
                KeyBindings.CreateDefaultData(True)
                SetupKeys()
            End If
            If rClose.Contains(MouseHandler.MousePosition) = True Then
                SetScreen(PreScreen)
            End If
        End If
    End Sub

    Private Class KeyBinding

        Private _position As Vector2
        Private _key As Keys
        Private _isSelected As Boolean = False
        Private _bindingName As String

        Public Sub New(ByVal bindingName As String, ByVal position As Vector2, ByVal currentKey As Keys)
            _position = position
            _key = currentKey
            _bindingName = bindingName
        End Sub

        Private Sub SetBinding(ByVal key As Keys)
            _key = key
            KeyBindings.SetKeyByName(_bindingName, _key)
        End Sub

        Public Sub Draw()
            Dim text As String = _key.ToString()

            If _isSelected = True Then
                text = ""
            End If

            Dim widthUnits As Integer = GetWidthUnits(text)
            Dim textSize As Vector2 = FontManager.MainFont.MeasureString(text)

            Canvas.DrawRectangle(New Rectangle(CInt(_position.X), CInt(_position.Y), widthUnits * 64, 64), New Color(150, 150, 150))

            If (GetRekt(_position, text).Contains(MouseHandler.MousePosition) = True Or _isSelected = True) And CurrentScreen.Identification = Screen.Identifications.KeyBindingScreen Then
                Canvas.DrawRectangle(New Rectangle(CInt(_position.X) + 3, CInt(_position.Y) + 3, widthUnits * 64 - 6, 58), New Color(180, 180, 180))
            Else
                Canvas.DrawRectangle(New Rectangle(CInt(_position.X) + 3, CInt(_position.Y) + 3, widthUnits * 64 - 6, 58), New Color(210, 210, 210))
            End If

            SpriteBatch.DrawString(FontManager.MainFont, text, New Vector2(_position.X + widthUnits * 32 - textSize.X / 2.0F, _position.Y + 32 - textSize.Y / 2.0F), Color.Black)
        End Sub

        Private Shared Function GetWidthUnits(ByVal text As String) As Integer
            Dim widthUnits As Integer = 1
            Dim textSize As Vector2 = FontManager.MainFont.MeasureString(text)

            While widthUnits * 64 - 10 < textSize.X
                widthUnits += 1
            End While

            Return widthUnits
        End Function

        Public Sub Update()
            If _isSelected = True Then
                Dim pressedKeys = KeyBoardHandler.GetPressedKeys

                If pressedKeys.Length > 0 Then
                    Dim i As Integer = 0
                    Dim pKey As Keys = Keys.None
                    While pKey = Keys.None And i < pressedKeys.Length
                        pKey = pressedKeys(i)
                        i += 1
                    End While
                    If pKey <> Keys.None Then
                        Me.SetBinding(pKey)
                        _isSelected = False
                    End If
                End If

                If Controls.Accept(True, False, False) = True Or Controls.Dismiss(True, False, False) = True Then
                    _isSelected = False
                End If
            Else
                If Controls.Accept(True, False, False) = True Then
                    If GetRekt(_position, _key.ToString()).Contains(MouseHandler.MousePosition) Then
                        _isSelected = True
                    End If
                End If
            End If
        End Sub

        Public Shared Function GetRekt(ByVal position As Vector2, ByVal text As String) As Rectangle
            Return New Rectangle(CInt(position.X), CInt(position.Y), GetWidthUnits(text) * 64, 64)
        End Function

    End Class

End Class