Public Class BARotation

    Inherits BattleAnimation3D

    Dim RotationVector As Vector3
    Dim EndRotation As Vector3
    Dim DoReturn As Boolean = False
    Dim ReturnVector As Vector3
    Dim hasReturned As Boolean = False
    Dim DoRotation As Vector3 = New Vector3(1.0F)

    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal RotationVector As Vector3, ByVal EndRotation As Vector3, ByVal startDelay As Single, ByVal endDelay As Single)
        MyBase.New(Position, Texture, Scale, startDelay, endDelay)

        Me.RotationVector = RotationVector
        Me.EndRotation = EndRotation
        Me.ReturnVector = Me.Rotation
    End Sub

    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal RotationVector As Vector3, ByVal EndRotation As Vector3, ByVal startDelay As Single, ByVal endDelay As Single, ByVal DoXRotation As Boolean, ByVal DoYRotation As Boolean, ByVal DoZRotation As Boolean)
        Me.New(Position, Texture, Scale, RotationVector, EndRotation, startDelay, endDelay)

        If DoXRotation = False Then
            DoRotation.X = 0.0F
        End If
        If DoYRotation = False Then
            DoRotation.Y = 0.0F
        End If
        If DoZRotation = False Then
            DoRotation.Z = 0.0F
        End If
    End Sub

    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal RotationVector As Vector3, ByVal EndRotation As Vector3, ByVal startDelay As Single, ByVal endDelay As Single, ByVal DoXRotation As Boolean, ByVal DoYRotation As Boolean, ByVal DoZRotation As Boolean, ByVal DoReturn As Boolean)
        Me.New(Position, Texture, Scale, RotationVector, EndRotation, startDelay, endDelay, DoXRotation, DoYRotation, DoZRotation)

        Me.DoReturn = DoReturn
    End Sub

    Public Overrides Sub DoActionActive()
        If VectorReached() = False Then

            If DoRotation.X = 1.0F Then
                If Me.Rotation.X > Me.EndRotation.X Then
                    Me.Rotation.X += Me.RotationVector.X

                    If Me.Rotation.X <= Me.EndRotation.X Then
                        Me.Rotation.X = Me.EndRotation.X
                    End If
                ElseIf Me.Rotation.X < Me.EndRotation.X Then
                    Me.Rotation.X += Me.RotationVector.X

                    If Me.Rotation.X >= Me.EndRotation.X Then
                        Me.Rotation.X = Me.EndRotation.X
                    End If
                End If
            End If

            If DoRotation.Y = 1.0F Then
                If Me.Rotation.Y > Me.EndRotation.Y Then
                    Me.Rotation.Y += Me.RotationVector.Y

                    If Me.Rotation.Y <= Me.EndRotation.Y Then
                        Me.Rotation.Y = Me.EndRotation.Y
                    End If
                ElseIf Me.Rotation.Y < Me.EndRotation.Y Then
                    Me.Rotation.Y += Me.RotationVector.Y

                    If Me.Rotation.Y >= Me.EndRotation.Y Then
                        Me.Rotation.Y = Me.EndRotation.Y
                    End If
                End If
            End If

            If DoRotation.Z = 1.0F Then
                If Me.Rotation.Z > Me.EndRotation.Z Then
                    Me.Rotation.Z += Me.RotationVector.Z

                    If Me.Rotation.Z <= Me.EndRotation.Z Then
                        Me.Rotation.Z = Me.EndRotation.Z
                    End If
                ElseIf Me.Rotation.Z < Me.EndRotation.Z Then
                    Me.Rotation.Z += Me.RotationVector.Z

                    If Me.Rotation.Z >= Me.EndRotation.Z Then
                        Me.Rotation.Z = Me.EndRotation.Z
                    End If
                End If
            End If

            If VectorReached() = True Then
                RotationReady()
            End If
        Else
            RotationReady()
        End If
    End Sub

    Private Sub RotationReady()
        If Me.DoReturn = True And Me.hasReturned = False Then
            Me.hasReturned = True
            Me.EndRotation = Me.ReturnVector
            Me.RotationVector = New Vector3(Me.RotationVector.X * -1, Me.RotationVector.Y * -1, Me.RotationVector.Z * -1)
        Else
            Me.Ready = True
        End If
    End Sub

    Private Function VectorReached() As Boolean
        If DoRotation.X = 1.0F Then
            If EndRotation.X <> Me.Rotation.X Then
                Return False
            End If
        End If
        If DoRotation.Y = 1.0F Then
            If EndRotation.Y <> Me.Rotation.Y Then
                Return False
            End If
        End If
        If DoRotation.Z = 1.0F Then
            If EndRotation.Z <> Me.Rotation.Z Then
                Return False
            End If
        End If

        Return True
    End Function

End Class