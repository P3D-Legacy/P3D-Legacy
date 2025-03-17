Public Class BAEntityScale

    Inherits BattleAnimation3D

    Dim Grow As Boolean = False
    Dim EndSize As Vector3
    Dim SizeSpeed As Single = 0.01F
    Dim TargetEntity As Entity
    Dim Anchors As String '1 = Bottom, 2 = Top, 3 = Left, 4 = Right. Combinations are possible.

    Dim SpeedMultiplier As Vector3
    Dim RemoveEntityAfter As Boolean

    Public Sub New(ByRef Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal Scale As Vector3, ByVal Grow As Boolean, ByVal EndSize As Vector3, ByVal SizeSpeed As Single, ByVal startDelay As Single, ByVal endDelay As Single, ByVal Anchors As String, Optional SpeedMultiplier As Vector3 = Nothing)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, Scale, startDelay, endDelay)
        Me.RemoveEntityAfter = RemoveEntityAfter
        Me.Anchors = Anchors
        Me.Grow = Grow
        Me.EndSize = EndSize
        Me.SizeSpeed = SizeSpeed
        Me.TargetEntity = Entity
        If SpeedMultiplier <> Nothing Then
            Me.SpeedMultiplier = SpeedMultiplier
        Else
            Me.SpeedMultiplier = New Vector3(1)
        End If

        Me.AnimationType = AnimationTypes.Size
    End Sub

    Public Overrides Sub DoActionActive()
        Dim saveScale As Vector3 = TargetEntity.Scale

        Dim changeX As Single = SizeSpeed * SpeedMultiplier.X
        Dim changeY As Single = SizeSpeed * SpeedMultiplier.Y
        Dim changeZ As Single = SizeSpeed * SpeedMultiplier.Z

        If Grow = True Then
            If TargetEntity.Scale.X < Me.EndSize.X Then
                TargetEntity.Scale.X += changeX

                If TargetEntity.Scale.X >= Me.EndSize.X Then
                    TargetEntity.Scale.X = Me.EndSize.X
                End If
            End If
            If TargetEntity.Scale.Y < Me.EndSize.Y Then
                TargetEntity.Scale.Y += changeY

                If TargetEntity.Scale.Y >= Me.EndSize.Y Then
                    TargetEntity.Scale.Y = Me.EndSize.Y
                End If
            End If
            If TargetEntity.Scale.Z < Me.EndSize.Z Then
                TargetEntity.Scale.Z += changeZ

                If TargetEntity.Scale.Z >= Me.EndSize.Z Then
                    TargetEntity.Scale.Z = Me.EndSize.Z
                End If
            End If
        Else
            If TargetEntity.Scale.X > Me.EndSize.X Then
                TargetEntity.Scale.X -= changeX

                If TargetEntity.Scale.X <= Me.EndSize.X Then
                    TargetEntity.Scale.X = Me.EndSize.X
                End If
            End If
            If TargetEntity.Scale.Y > Me.EndSize.Y Then
                TargetEntity.Scale.Y -= changeY

                If TargetEntity.Scale.Y <= Me.EndSize.Y Then
                    TargetEntity.Scale.Y = Me.EndSize.Y
                End If
            End If
            If TargetEntity.Scale.Z > Me.EndSize.Z Then
                TargetEntity.Scale.Z -= changeZ

                If TargetEntity.Scale.Z <= Me.EndSize.Z Then
                    TargetEntity.Scale.Z = Me.EndSize.Z
                End If
            End If
        End If

        'Bottom
        If Anchors.Contains("1") = True Then
            Dim diffY As Single = saveScale.Y - TargetEntity.Scale.Y
            TargetEntity.Position.Y -= diffY / 2
        End If
        'Top
        If Anchors.Contains("2") = True Then
            Dim diffY As Single = saveScale.Y - TargetEntity.Scale.Y
            TargetEntity.Position.Y += diffY / 2
        End If
        'Left
        If Anchors.Contains("3") = True Then
            Dim diffX As Single = saveScale.X - TargetEntity.Scale.X
            TargetEntity.Position.X -= diffX / 2
        End If
        'Right
        If Anchors.Contains("4") = True Then
            Dim diffX As Single = saveScale.X - TargetEntity.Scale.X
            TargetEntity.Position.X += diffX / 2
        End If

        If Me.EndSize = TargetEntity.Scale Then
            Me.Ready = True
        End If
    End Sub

    Public Overrides Sub DoRemoveEntity()
        If Me.RemoveEntityAfter = True Then
            TargetEntity.CanBeRemoved = True
        End If
    End Sub

End Class