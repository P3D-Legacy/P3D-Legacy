Public Class MouseHandler

    Shared OldState As MouseState
    Shared NewState As MouseState

    Public Enum MouseButtons
        LeftButton
        MiddleButton
        RightButton
    End Enum

    Public Shared Property MouseState() As MouseState
        Get
            Return NewState
        End Get
        Set(value As MouseState)
            NewState = value
        End Set
    End Property

    Public Shared Sub Update()
        OldState = NewState
        NewState = Mouse.GetState()
    End Sub

    Public Shared Function ButtonPressed(ByVal Button As MouseButtons) As Boolean
        If WindowContainsMouse = True And Core.GameInstance.IsActive = True Then
            Select Case Button
                Case MouseButtons.LeftButton
                    If OldState.LeftButton = ButtonState.Released And NewState.LeftButton = ButtonState.Pressed Then
                        Return True
                    End If
                Case MouseButtons.MiddleButton
                    If OldState.MiddleButton = ButtonState.Released And NewState.MiddleButton = ButtonState.Pressed Then
                        Return True
                    End If
                Case MouseButtons.RightButton
                    If OldState.RightButton = ButtonState.Released And NewState.RightButton = ButtonState.Pressed Then
                        Return True
                    End If
            End Select
        End If
        Return False
    End Function

    Public Shared Function ButtonDown(ByVal Button As MouseButtons) As Boolean
        If WindowContainsMouse = True And Core.GameInstance.IsActive = True Then
            Select Case Button
                Case MouseButtons.LeftButton
                    Return (NewState.LeftButton = ButtonState.Pressed)
                Case MouseButtons.MiddleButton
                    Return (NewState.MiddleButton = ButtonState.Pressed)
                Case MouseButtons.RightButton
                    Return (NewState.RightButton = ButtonState.Pressed)
            End Select
        End If
        Return False
    End Function

    Public Shared Function ButtonUp(ByVal Button As MouseButtons) As Boolean
        Select Case Button
            Case MouseButtons.LeftButton
                Return (NewState.LeftButton = ButtonState.Released)
            Case MouseButtons.MiddleButton
                Return (NewState.MiddleButton = ButtonState.Released)
            Case MouseButtons.RightButton
                Return (NewState.RightButton = ButtonState.Released)
        End Select
        Return False
    End Function

    Public Shared Function IsInRectangle(ByVal rec As Rectangle) As Boolean
        Return rec.Contains(NewState.X, NewState.Y)
    End Function

    Public Shared Function GetScrollWheelChange() As Integer
        Return NewState.ScrollWheelValue - OldState.ScrollWheelValue
    End Function

    Public Shared ReadOnly Property HasMouseInput() As Boolean
        Get
            If ButtonDown(MouseButtons.LeftButton) = True OrElse ButtonDown(MouseButtons.RightButton) OrElse ButtonDown(MouseButtons.MiddleButton) OrElse
                GetScrollWheelChange() <> 0 Then
                If WindowContainsMouse = True And Core.GameInstance.IsActive = True Then
                    Return True
                End If
            End If
            Return False
        End Get
    End Property

    Public Shared ReadOnly Property WindowContainsMouse() As Boolean
        Get
            Return IsInRectangle(Core.GraphicsDevice.Viewport.Bounds)
        End Get
    End Property

    Public Shared ReadOnly Property MousePosition() As Point
        Get
            Return New Point(NewState.X, NewState.Y)
        End Get
    End Property

End Class