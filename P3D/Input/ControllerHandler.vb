Public Class ControllerHandler

    Shared OldState As GamePadState
    Shared NewState As GamePadState

    Public Shared Property GamePadState As GamePadState
        Get
            Return NewState
        End Get
        Set(value As GamePadState)
            NewState = value
        End Set
    End Property

    Public Shared Sub Update()
        OldState = NewState
        NewState = GamePad.GetState(PlayerIndex.One)
    End Sub

    Public Shared Function ButtonPressed(ByVal Button As Microsoft.Xna.Framework.Input.Buttons) As Boolean
        Return ButtonPressed(Button, Core.GameOptions.GamePadEnabled)
    End Function

    Public Shared Function ButtonPressed(ByVal Button As Microsoft.Xna.Framework.Input.Buttons, ByVal GamePadEnabled As Boolean) As Boolean
        If GamePadEnabled = True Then
            Dim leftStickButtons As List(Of Microsoft.Xna.Framework.Input.Buttons) = {Buttons.LeftThumbstickUp, Buttons.LeftThumbstickLeft, Buttons.LeftThumbstickDown, Buttons.LeftThumbstickRight}.ToList

            If leftStickButtons.Contains(Button) = True Then
                Select Case Button
                    Case Buttons.LeftThumbstickUp
                        If LeftThumbstickDirection(OldState, GamePadEnabled) = -1 And LeftThumbstickDirection(NewState, GamePadEnabled) = 0 Then
                            Return True
                        End If
                    Case Buttons.LeftThumbstickLeft
                        If LeftThumbstickDirection(OldState, GamePadEnabled) = -1 And LeftThumbstickDirection(NewState, GamePadEnabled) = 1 Then
                            Return True
                        End If
                    Case Buttons.LeftThumbstickDown
                        If LeftThumbstickDirection(OldState, GamePadEnabled) = -1 And LeftThumbstickDirection(NewState, GamePadEnabled) = 2 Then
                            Return True
                        End If
                    Case Buttons.LeftThumbstickRight
                        If LeftThumbstickDirection(OldState, GamePadEnabled) = -1 And LeftThumbstickDirection(NewState, GamePadEnabled) = 3 Then
                            Return True
                        End If
                End Select
                Return False
            Else
                If OldState.IsButtonDown(Button) = False And NewState.IsButtonDown(Button) = True Then
                    Return True
                Else
                    Return False
                End If
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function ButtonDown(ByVal Button As Microsoft.Xna.Framework.Input.Buttons) As Boolean
        Return ButtonDown(Button, Core.GameOptions.GamePadEnabled)
    End Function

    Public Shared Function ButtonDown(ByVal Button As Microsoft.Xna.Framework.Input.Buttons, ByVal GamePadEnabled As Boolean) As Boolean
        If GamePadEnabled = True Then
            Dim leftStickButtons As List(Of Microsoft.Xna.Framework.Input.Buttons) = {Buttons.LeftThumbstickUp, Buttons.LeftThumbstickLeft, Buttons.LeftThumbstickDown, Buttons.LeftThumbstickRight}.ToList

            If leftStickButtons.Contains(Button) = True Then
                Select Case Button
                    Case Buttons.LeftThumbstickUp
                        If LeftThumbstickDirection(NewState, GamePadEnabled) = 0 Then
                            Return True
                        End If
                    Case Buttons.LeftThumbstickLeft
                        If LeftThumbstickDirection(NewState, GamePadEnabled) = 1 Then
                            Return True
                        End If
                    Case Buttons.LeftThumbstickDown
                        If LeftThumbstickDirection(NewState, GamePadEnabled) = 2 Then
                            Return True
                        End If
                    Case Buttons.LeftThumbstickRight
                        If LeftThumbstickDirection(NewState, GamePadEnabled) = 3 Then
                            Return True
                        End If
                End Select
                Return False
            Else
                If NewState.IsButtonDown(Button) = True Then
                    Return True
                Else
                    Return False
                End If
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function LeftThumbstickAngle(ByVal State As GamePadState, ByVal GamePadEnabled As Boolean) As Integer
        If GamePadEnabled = True Then
            If State.ThumbSticks.Left.X <> 0 Or State.ThumbSticks.Left.Y <> 0 Then
                Return CInt(Math.Atan2(State.ThumbSticks.Left.Y, State.ThumbSticks.Left.X) * 57)
            End If
        End If
        Return 999
    End Function
    Public Shared Function LeftThumbstickDirection(ByVal State As GamePadState, ByVal GamePadEnabled As Boolean) As Integer
        If GamePadEnabled = True AndAlso LeftThumbstickAngle(State, GamePadEnabled) <> 999 Then
            'Up
            If LeftThumbstickAngle(State, GamePadEnabled) > 45 AndAlso LeftThumbstickAngle(State, GamePadEnabled) <= 135 Then
                Return 0
            End If

            'Left
            If LeftThumbstickAngle(State, GamePadEnabled) <= -135 AndAlso LeftThumbstickAngle(State, GamePadEnabled) >= -180 OrElse
            LeftThumbstickAngle(State, GamePadEnabled) > 135 AndAlso LeftThumbstickAngle(State, GamePadEnabled) <= 180 Then
                Return 1
            End If

            'Down
            If LeftThumbstickAngle(State, GamePadEnabled) < -45 AndAlso LeftThumbstickAngle(State, GamePadEnabled) > -135 Then
                Return 2
            End If

            'Right
            If LeftThumbstickAngle(State, GamePadEnabled) >= -45 AndAlso LeftThumbstickAngle(State, GamePadEnabled) < 45 Then
                Return 3
            End If

        End If
        Return -1
    End Function
    Public Shared Function IsConnected(Optional ByVal index As Integer = 0) As Boolean
        Return (GamePad.GetState(CType(index, PlayerIndex)).IsConnected = True And Core.GameOptions.GamePadEnabled = True)
    End Function

    Public Shared Function HasControllerInput(Optional ByVal index As Integer = 0) As Boolean
        If IsConnected() = False Then
            Return False
        End If

        Dim gPadState As GamePadState = GamePad.GetState(CType(index, PlayerIndex))

        Dim bArr() As Buttons = {Buttons.A, Buttons.B, Buttons.Back, Buttons.BigButton, Buttons.DPadDown, Buttons.DPadLeft, Buttons.DPadRight, Buttons.DPadUp, Buttons.LeftShoulder, Buttons.LeftStick, Buttons.LeftThumbstickDown, Buttons.LeftThumbstickLeft, Buttons.LeftThumbstickRight, Buttons.LeftThumbstickUp, Buttons.LeftTrigger, Buttons.RightShoulder, Buttons.RightStick, Buttons.RightThumbstickDown, Buttons.RightThumbstickLeft, Buttons.RightThumbstickRight, Buttons.RightThumbstickUp, Buttons.RightTrigger, Buttons.Start, Buttons.X, Buttons.Y}

        For Each b As Buttons In bArr
            If gPadState.IsButtonDown(b) = True Then
                Return True
            End If
        Next

        Return False
    End Function

End Class