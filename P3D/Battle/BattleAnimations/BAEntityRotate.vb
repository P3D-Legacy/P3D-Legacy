Public Class BAEntityRotate

    Inherits BattleAnimation3D

    Dim TargetEntity As Entity
    Dim RotationSpeedVector As Vector3
    Dim EndRotation As Vector3
    Dim DoReturn As Boolean = False
    Dim ReturnVector As Vector3
    Dim hasReturned As Boolean = False
    Dim DoRotation As Vector3 = New Vector3(1.0F)
    Dim AmountRotated As Vector3 = New Vector3(0.0F)
    Dim ReadyAxis As Vector3 = New Vector3(0.0F)
    Dim RemoveEntityAfter As Boolean = False
    Const DegreesToRadiansCalc As Single = Math.PI / 180.0F

    Public Sub New(ByRef Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal RotationSpeedVector As Vector3, ByVal EndRotation As Vector3, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal DoReturn As Boolean = False, Optional ByVal ConvertDegreesToRadians As Boolean = False)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)

        Me.RemoveEntityAfter = RemoveEntityAfter
        If ConvertDegreesToRadians = False Then
            Me.RotationSpeedVector = RotationSpeedVector
            Me.EndRotation = EndRotation
        Else
            Me.RotationSpeedVector = New Vector3(RotationSpeedVector.X * DegreesToRadiansCalc, RotationSpeedVector.Y * DegreesToRadiansCalc, RotationSpeedVector.Z * DegreesToRadiansCalc)
            Me.EndRotation = New Vector3(EndRotation.X * DegreesToRadiansCalc, EndRotation.Y * DegreesToRadiansCalc, EndRotation.Z * DegreesToRadiansCalc)
        End If
        Me.TargetEntity = Entity
        Me.ReturnVector = TargetEntity.Rotation

        If RotationSpeedVector.X = 0.0F Then
            DoRotation.X = 0.0F
            ReadyAxis.X = 1.0F
        End If
        If RotationSpeedVector.Y = 0.0F Then
            DoRotation.Y = 0.0F
            ReadyAxis.Y = 1.0F
        End If
        If RotationSpeedVector.Z = 0.0F Then
            DoRotation.Z = 0.0F
            ReadyAxis.Z = 1.0F
        End If
        Me.DoReturn = DoReturn

        Me.AnimationType = AnimationTypes.Rotation
    End Sub

    Public Overrides Sub DoActionActive()
        If DoRotation.X = 1.0F Then
            If AmountRotated.X < Math.Abs(EndRotation.X) Then
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
                AmountRotated.X += Math.Abs(Me.RotationSpeedVector.X)
            Else
                ReadyAxis.X = 1.0F
            End If
        End If
        If DoRotation.Y = 1.0F Then
            If AmountRotated.Y < Math.Abs(EndRotation.Y) Then
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
                AmountRotated.Y += Math.Abs(Me.RotationSpeedVector.Y)
            Else
                ReadyAxis.y = 1.0F
            End If
        End If

        If DoRotation.Z = 1.0F Then
            If AmountRotated.Z < Math.Abs(EndRotation.Z) Then

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
                AmountRotated.Z += Math.Abs(Me.RotationSpeedVector.Z)
            Else
                ReadyAxis.Z = 1.0F
            End If
        End If

        If ReadyAxis.X = 1.0F AndAlso ReadyAxis.Y = 1.0F AndAlso ReadyAxis.Z = 1.0F Then
            RotationReady()
        End If
    End Sub

    Private Sub RotationReady()

        If Me.DoReturn = True And Me.hasReturned = False Then
            Me.hasReturned = True
            Me.EndRotation = Me.ReturnVector
            Me.RotationSpeedVector = New Vector3(Me.RotationSpeedVector.X * -1, Me.RotationSpeedVector.Y * -1, Me.RotationSpeedVector.Z * -1)
            If DoRotation.X = 1.0F Then
                ReadyAxis.X = 0.0F
                AmountRotated.X -= AmountRotated.X * 2
            End If
            If DoRotation.Y = 1.0F Then
                ReadyAxis.Y = 0.0F
                AmountRotated.Y -= AmountRotated.Y * 2
            End If
            If DoRotation.Z = 1.0F Then
                ReadyAxis.Z = 0.0F
                AmountRotated.Z -= AmountRotated.Z * 2
            End If
        Else
            Me.Ready = True
        End If
    End Sub

    Public Overrides Sub DoRemoveEntity()
        If Me.RemoveEntityAfter = True Then
            TargetEntity.CanBeRemoved = True
        End If
    End Sub
End Class