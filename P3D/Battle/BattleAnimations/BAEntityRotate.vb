Public Class BAEntityRotate

    Inherits BattleAnimation3D

    Dim TargetEntity As Entity
    Dim RotationSpeedVector As Vector3
    Dim EndRotation As Vector3
    Dim DoReturn As Boolean = False
    Dim ReturnVector As Vector3
    Dim hasReturned As Boolean = False
    Dim DoRotation As Vector3 = New Vector3(1.0F)
    Public RemoveEntityAfter As Boolean = False

    Public Sub New(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal RotationSpeedVector As Vector3, ByVal EndRotation As Vector3, ByVal startDelay As Single, ByVal endDelay As Single)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
        Me.RemoveEntityAfter = RemoveEntityAfter
        Me.RotationSpeedVector = RotationSpeedVector
        Me.EndRotation = EndRotation
        Me.TargetEntity = Entity
        Me.ReturnVector = TargetEntity.Rotation
    End Sub

    Public Sub New(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal RotationSpeedVector As Vector3, ByVal EndRotation As Vector3, ByVal startDelay As Single, ByVal endDelay As Single, ByVal DoXRotation As Boolean, ByVal DoYRotation As Boolean, ByVal DoZRotation As Boolean)
        Me.New(Entity, RemoveEntityAfter, RotationSpeedVector, EndRotation, startDelay, endDelay)

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

    Public Sub New(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal RotationSpeedVector As Vector3, ByVal EndRotation As Vector3, ByVal startDelay As Single, ByVal endDelay As Single, ByVal DoXRotation As Boolean, ByVal DoYRotation As Boolean, ByVal DoZRotation As Boolean, ByVal DoReturn As Boolean)
        Me.New(Entity, RemoveEntityAfter, RotationSpeedVector, EndRotation, startDelay, endDelay, DoXRotation, DoYRotation, DoZRotation)

        Me.DoReturn = DoReturn
    End Sub

    Public Overrides Sub DoActionActive()
        If VectorReached() = False Then

            If DoRotation.X = 1.0F Then
                If TargetEntity.Rotation.X > Me.EndRotation.X Then
                    TargetEntity.Rotation.X += Me.RotationSpeedVector.X

                    If TargetEntity.Rotation.X <= Me.EndRotation.X Then
                        TargetEntity.Rotation.X = Me.EndRotation.X
                    End If
                ElseIf TargetEntity.Rotation.X < Me.EndRotation.X Then
                    TargetEntity.Rotation.X += Me.RotationSpeedVector.X

                    If TargetEntity.Rotation.X >= Me.EndRotation.X Then
                        TargetEntity.Rotation.X = Me.EndRotation.X
                    End If
                End If
            End If

            If DoRotation.Y = 1.0F Then
                If TargetEntity.Rotation.Y > Me.EndRotation.Y Then
                    TargetEntity.Rotation.Y += Me.RotationSpeedVector.Y

                    If TargetEntity.Rotation.Y <= Me.EndRotation.Y Then
                        TargetEntity.Rotation.Y = Me.EndRotation.Y
                    End If
                ElseIf TargetEntity.Rotation.Y < Me.EndRotation.Y Then
                    TargetEntity.Rotation.Y += Me.RotationSpeedVector.Y

                    If TargetEntity.Rotation.Y >= Me.EndRotation.Y Then
                        TargetEntity.Rotation.Y = Me.EndRotation.Y
                    End If
                End If
            End If

            If DoRotation.Z = 1.0F Then
                If TargetEntity.Rotation.Z > Me.EndRotation.Z Then
                    TargetEntity.Rotation.Z += Me.RotationSpeedVector.Z

                    If TargetEntity.Rotation.Z <= Me.EndRotation.Z Then
                        TargetEntity.Rotation.Z = Me.EndRotation.Z
                    End If
                ElseIf TargetEntity.Rotation.Z < Me.EndRotation.Z Then
                    TargetEntity.Rotation.Z += Me.RotationSpeedVector.Z

                    If TargetEntity.Rotation.Z >= Me.EndRotation.Z Then
                        TargetEntity.Rotation.Z = Me.EndRotation.Z
                    End If
                End If
            End If
        Else
            RotationReady()
        End If
    End Sub

    Private Sub RotationReady()
        If Me.DoReturn = True And Me.hasReturned = False Then
            Me.hasReturned = True
            Me.EndRotation = Me.ReturnVector
            Me.RotationSpeedVector = New Vector3(Me.RotationSpeedVector.X * -1, Me.RotationSpeedVector.Y * -1, Me.RotationSpeedVector.Z * -1)
        Else
            Me.Ready = True
        End If
    End Sub

    Private Function VectorReached() As Boolean
        If DoRotation.X = 1.0F Then
            If EndRotation.X <> TargetEntity.Rotation.X Then
                Return False
            End If
        End If
        If DoRotation.Y = 1.0F Then
            If EndRotation.Y <> TargetEntity.Rotation.Y Then
                Return False
            End If
        End If
        If DoRotation.Z = 1.0F Then
            If EndRotation.Z <> TargetEntity.Rotation.Z Then
                Return False
            End If
        End If

        Return True
    End Function

    Public Overrides Sub DoRemoveEntity()
        If Me.RemoveEntityAfter = True Then
            TargetEntity.CanBeRemoved = True
        End If
    End Sub
End Class