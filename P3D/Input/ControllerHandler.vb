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
            If OldState.IsButtonDown(Button) = False And NewState.IsButtonDown(Button) = True Then
                Return True
            Else
                Return False
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
            If NewState.IsButtonDown(Button) = True Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function IsConnected(Optional ByVal index As Integer = 0) As Boolean
        Return (GamePad.GetState(CType(index, PlayerIndex)).IsConnected = True And Core.GameOptions.GamePadEnabled = True)
    End Function

    Public Shared Function HasControlerInput(Optional ByVal index As Integer = 0) As Boolean
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