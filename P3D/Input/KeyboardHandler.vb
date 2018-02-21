Public Class KeyBoardHandler

    Shared OldState As KeyboardState
    Shared NewState As KeyboardState

    Public Shared Property KeyBoardState() As KeyboardState
        Get
            Return NewState
        End Get
        Set(value As KeyboardState)
            NewState = value
        End Set
    End Property

    Public Shared Sub Update()
        OldState = NewState
        NewState = Keyboard.GetState()
    End Sub

    Public Shared Function KeyPressed(ByVal Key As Microsoft.Xna.Framework.Input.Keys) As Boolean
        If OldState.IsKeyDown(Key) = False And NewState.IsKeyDown(Key) = True Then
            Return True
        End If
        Return False
    End Function

    Public Shared Function KeyDown(ByVal Key As Microsoft.Xna.Framework.Input.Keys) As Boolean
        Return NewState.IsKeyDown(Key)
    End Function

    Public Shared Function KeyUp(ByVal Key As Microsoft.Xna.Framework.Input.Keys) As Boolean
        Return NewState.IsKeyUp(Key)
    End Function

    Public Shared Function HasKeyboardInput() As Boolean
        Return (NewState.GetPressedKeys().Count > 0)
    End Function

    Public Shared ReadOnly Property GetPressedKeys() As Keys()
        Get
            Return NewState.GetPressedKeys()
        End Get
    End Property

End Class