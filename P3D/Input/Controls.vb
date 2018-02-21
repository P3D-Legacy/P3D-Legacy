Public Class Controls

    Private Enum PressDirections
        Up
        Left
        Down
        Right
        None
    End Enum

    Shared LastPressedDirection As PressDirections = PressDirections.None
    Shared PressedKeyDelay As Single = 0.0F

    Private Shared Sub ResetDirectionPressed(ByVal direction As PressDirections)
        If LastPressedDirection = direction Then
            PressedKeyDelay = 4.0F
            LastPressedDirection = PressDirections.None
        End If
    End Sub

    Private Shared Sub ChangeDirectionPressed(ByVal direction As PressDirections)
        If LastPressedDirection <> direction Then
            PressedKeyDelay = 4.0F
            LastPressedDirection = direction
        End If
    End Sub

    Private Shared Function HoldDownPress(ByVal direction As PressDirections) As Boolean
        If LastPressedDirection = direction Then
            PressedKeyDelay -= 0.1F
            If PressedKeyDelay <= 0.0F Then
                PressedKeyDelay = 0.3F
                Return True
            End If
        End If
        Return False
    End Function

    Private Shared Function CheckDirectionalPress(ByVal Direction As PressDirections, ByVal WASDKey As Keys, ByVal DirectionalKey As Keys, ByVal ThumbStickDirection As Buttons, ByVal DPadDirecion As Buttons,
                                                  ByVal ArrowKeys As Boolean, ByVal WASD As Boolean, ByVal ThumbStick As Boolean, ByVal DPad As Boolean) As Boolean
        Dim command As Boolean = False
        If WASD = True Then
            If KeyboardHandler.KeyDown(WASDKey) = True Then
                command = True
                If HoldDownPress(Direction) = True Then
                    Return True
                Else
                    If KeyboardHandler.KeyPressed(WASDKey) = True Then
                        ChangeDirectionPressed(Direction)
                        Return True
                    End If
                End If
            End If
        End If
        If ArrowKeys = True Then
            If KeyboardHandler.KeyDown(DirectionalKey) = True Then
                command = True
                If HoldDownPress(Direction) = True Then
                    Return True
                Else
                    If KeyboardHandler.KeyPressed(DirectionalKey) = True Then
                        ChangeDirectionPressed(Direction)
                        Return True
                    End If
                End If
            End If
        End If
        If ThumbStick = True Then
            If ControllerHandler.ButtonDown(ThumbStickDirection) = True Then
                command = True
                If HoldDownPress(Direction) = True Then
                    Return True
                Else
                    If ControllerHandler.ButtonPressed(ThumbStickDirection) = True Then
                        ChangeDirectionPressed(Direction)
                        Return True
                    End If
                End If
            End If
        End If
        If DPad = True Then
            If ControllerHandler.ButtonDown(DPadDirecion) = True Then
                command = True
                If HoldDownPress(Direction) = True Then
                    Return True
                Else
                    If ControllerHandler.ButtonPressed(DPadDirecion) = True Then
                        ChangeDirectionPressed(Direction)
                        Return True
                    End If
                End If
            End If
        End If

        If command = False Then
            ResetDirectionPressed(Direction)
        End If

        Return False
    End Function

    Public Shared Function Left(ByVal Pressed As Boolean, Optional ByVal ArrowKeys As Boolean = True, Optional ByVal Scroll As Boolean = True, Optional ByVal WASD As Boolean = True, Optional ByVal ThumbStick As Boolean = True, Optional ByVal DPad As Boolean = True) As Boolean
        If Core.GameInstance.IsActive = True Then
            If MouseHandler.WindowContainsMouse = True And Scroll = True Then
                If MouseHandler.GetScrollWheelChange() > 0 Then
                    Return True
                End If
            End If
            If Pressed = True Then
                Return CheckDirectionalPress(PressDirections.Left, KeyBindings.LeftMoveKey, KeyBindings.LeftKey, Buttons.LeftThumbstickLeft, Buttons.DPadLeft, ArrowKeys, WASD, ThumbStick, DPad)
            Else
                If WASD = True Then
                    If KeyBoardHandler.KeyDown(KeyBindings.LeftMoveKey) = True Then
                        Return True
                    End If
                End If
                If ArrowKeys = True Then
                    If KeyBoardHandler.KeyDown(KeyBindings.LeftKey) = True Then
                        Return True
                    End If
                End If
                If ThumbStick = True Then
                    If ControllerHandler.ButtonDown(Buttons.LeftThumbstickLeft) = True Then
                        Return True
                    End If
                End If
                If DPad = True Then
                    If ControllerHandler.ButtonDown(Buttons.DPadLeft) = True Then
                        Return True
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Public Shared Function Right(ByVal Pressed As Boolean, Optional ByVal ArrowKeys As Boolean = True, Optional ByVal Scroll As Boolean = True, Optional ByVal WASD As Boolean = True, Optional ByVal ThumbStick As Boolean = True, Optional ByVal DPad As Boolean = True) As Boolean
        If Core.GameInstance.IsActive = True Then
            If MouseHandler.WindowContainsMouse = True And Scroll = True Then
                If MouseHandler.GetScrollWheelChange() < 0 Then
                    Return True
                End If
            End If
            If Pressed = True Then
                Return CheckDirectionalPress(PressDirections.Right, KeyBindings.RightMoveKey, KeyBindings.RightKey, Buttons.LeftThumbstickRight, Buttons.DPadRight, ArrowKeys, WASD, ThumbStick, DPad)
            Else
                If WASD = True Then
                    If KeyBoardHandler.KeyDown(KeyBindings.RightMoveKey) = True Then
                        Return True
                    End If
                End If
                If ArrowKeys = True Then
                    If KeyBoardHandler.KeyDown(KeyBindings.RightKey) = True Then
                        Return True
                    End If
                End If
                If ThumbStick = True Then
                    If ControllerHandler.ButtonDown(Buttons.LeftThumbstickRight) = True Then
                        Return True
                    End If
                End If
                If DPad = True Then
                    If ControllerHandler.ButtonDown(Buttons.DPadRight) = True Then
                        Return True
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Public Shared Function Up(ByVal Pressed As Boolean, Optional ByVal ArrowKeys As Boolean = True, Optional ByVal Scroll As Boolean = True, Optional ByVal WASD As Boolean = True, Optional ByVal ThumbStick As Boolean = True, Optional ByVal DPad As Boolean = True) As Boolean
        If Core.GameInstance.IsActive = True Then
            If MouseHandler.WindowContainsMouse = True And Scroll = True Then
                If MouseHandler.GetScrollWheelChange() > 0 Then
                    Return True
                End If
            End If
            If Pressed = True Then
                Return CheckDirectionalPress(PressDirections.Up, KeyBindings.ForwardMoveKey, KeyBindings.UpKey, Buttons.LeftThumbstickUp, Buttons.DPadUp, ArrowKeys, WASD, ThumbStick, DPad)
            Else
                If WASD = True Then
                    If KeyBoardHandler.KeyDown(KeyBindings.ForwardMoveKey) = True Then
                        Return True
                    End If
                End If
                If ArrowKeys = True Then
                    If KeyBoardHandler.KeyDown(KeyBindings.UpKey) = True Then
                        Return True
                    End If
                End If
                If ThumbStick = True Then
                    If ControllerHandler.ButtonDown(Buttons.LeftThumbstickUp) = True Then
                        Return True
                    End If
                End If
                If DPad = True Then
                    If ControllerHandler.ButtonDown(Buttons.DPadUp) = True Then
                        Return True
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Public Shared Function Down(ByVal Pressed As Boolean, Optional ByVal ArrowKeys As Boolean = True, Optional ByVal Scroll As Boolean = True, Optional ByVal WASD As Boolean = True, Optional ByVal ThumbStick As Boolean = True, Optional ByVal DPad As Boolean = True) As Boolean
        If Core.GameInstance.IsActive = True Then
            If MouseHandler.WindowContainsMouse = True And Scroll = True Then
                If MouseHandler.GetScrollWheelChange() < 0 Then
                    Return True
                End If
            End If
            If Pressed = True Then
                Return CheckDirectionalPress(PressDirections.Down, KeyBindings.BackwardMoveKey, KeyBindings.DownKey, Buttons.LeftThumbstickDown, Buttons.DPadDown, ArrowKeys, WASD, ThumbStick, DPad)
            Else
                If WASD = True Then
                    If KeyBoardHandler.KeyDown(KeyBindings.BackwardMoveKey) = True Then
                        Return True
                    End If
                End If
                If ArrowKeys = True Then
                    If KeyBoardHandler.KeyDown(KeyBindings.DownKey) = True Then
                        Return True
                    End If
                End If
                If ThumbStick = True Then
                    If ControllerHandler.ButtonDown(Buttons.LeftThumbstickDown) = True Then
                        Return True
                    End If
                End If
                If DPad = True Then
                    If ControllerHandler.ButtonDown(Buttons.DPadDown) = True Then
                        Return True
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Public Shared Function ShiftDown(Optional ByVal PressedFlag As String = "LR", Optional TriggerButtons As Boolean = True) As Boolean
        If PressedFlag.Contains("L") And KeyBoardHandler.KeyDown(Keys.LeftShift) = True Then
            Return True
        End If
        If PressedFlag.Contains("L") And ControllerHandler.ButtonDown(Buttons.LeftTrigger) = True And TriggerButtons = True Then
            Return True
        End If
        If PressedFlag.Contains("R") And KeyBoardHandler.KeyDown(Keys.RightShift) = True Then
            Return True
        End If
        If PressedFlag.Contains("R") And ControllerHandler.ButtonDown(Buttons.RightTrigger) = True And TriggerButtons = True Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function ShiftPressed(Optional ByVal PressedFlag As String = "LR", Optional TriggerButtons As Boolean = True) As Boolean
        If PressedFlag.Contains("L") And KeyBoardHandler.KeyPressed(Keys.LeftShift) = True Then
            Return True
        End If
        If PressedFlag.Contains("L") And ControllerHandler.ButtonPressed(Buttons.LeftTrigger) = True And TriggerButtons = True Then
            Return True
        End If
        If PressedFlag.Contains("R") And KeyBoardHandler.KeyPressed(Keys.RightShift) = True Then
            Return True
        End If
        If PressedFlag.Contains("R") And ControllerHandler.ButtonPressed(Buttons.RightTrigger) = True And TriggerButtons = True Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function CtrlPressed(Optional ByVal PressedFlag As String = "LR", Optional TriggerButtons As Boolean = True) As Boolean
        If System.Windows.Forms.Control.ModifierKeys = (System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) Then
            Return False
        End If
        If PressedFlag.Contains("L") And My.Computer.Keyboard.CtrlKeyDown() = True Then
            Return True
        End If
        If PressedFlag.Contains("L") And ControllerHandler.ButtonDown(Buttons.LeftTrigger) = True And TriggerButtons = True Then
            Return True
        End If
        If PressedFlag.Contains("R") And My.Computer.Keyboard.CtrlKeyDown() = True Then
            Return True
        End If
        If PressedFlag.Contains("R") And ControllerHandler.ButtonDown(Buttons.RightTrigger) = True And TriggerButtons = True Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function HasKeyboardInput() As Boolean
        Dim keyArr() As Keys = {KeyBindings.ForwardMoveKey, KeyBindings.LeftMoveKey, KeyBindings.BackwardMoveKey, KeyBindings.RightMoveKey, KeyBindings.UpKey, KeyBindings.LeftKey, KeyBindings.DownKey, KeyBindings.RightKey}
        For Each key As Keys In keyArr
            If KeyBoardHandler.KeyPressed(key) = True Then
                Return True
            End If
        Next

        Return False
    End Function

    Public Shared Function Accept(Optional ByVal DoMouse As Boolean = True, Optional ByVal DoKeyBoard As Boolean = True, Optional ByVal DoGamePad As Boolean = True) As Boolean
        If Core.GameInstance.IsActive = True Then
            If DoKeyBoard = True Then
                If KeyBoardHandler.KeyPressed(KeyBindings.EnterKey1) = True Or KeyBoardHandler.KeyPressed(KeyBindings.EnterKey2) = True Then
                    Return True
                End If
            End If
            If DoMouse = True Then
                If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                    Return True
                End If
            End If
            If DoGamePad = True Then
                If ControllerHandler.ButtonPressed(Buttons.A) = True Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Public Shared Function Dismiss(Optional ByVal DoMouse As Boolean = True, Optional ByVal DoKeyBoard As Boolean = True, Optional ByVal DoGamePad As Boolean = True) As Boolean
        If Core.GameInstance.IsActive = True Then
            If DoKeyBoard = True Then
                If KeyBoardHandler.KeyPressed(KeyBindings.BackKey1) = True Or KeyBoardHandler.KeyPressed(KeyBindings.BackKey2) = True Then
                    Return True
                End If
            End If
            If DoMouse = True Then
                If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.RightButton) = True Then
                    Return True
                End If
            End If
            If DoGamePad = True Then
                If ControllerHandler.ButtonPressed(Buttons.B) = True Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Public Shared Sub MakeMouseVisible()
        If Core.GameInstance.IsMouseVisible = False And Core.CurrentScreen.MouseVisible = True Then
            Dim mState As MouseState = Mouse.GetState()
            If mState.X <> MouseHandler.MousePosition.X Or mState.Y <> MouseHandler.MousePosition.Y Then
                Core.GameInstance.IsMouseVisible = True
            End If
        ElseIf Core.GameInstance.IsMouseVisible = True Then
            If ControllerHandler.HasControlerInput() = True Or Controls.HasKeyboardInput() = True Then
                Core.GameInstance.IsMouseVisible = False
            End If
        End If
    End Sub

End Class