Public Class BAEntityScale

    Inherits BattleAnimation3D

    Public Grow As Boolean = False
    Public EndSize As Vector3
    Public SizeSpeed As Single = 0.01F
    Public TargetEntity As Entity
    Public Anchors As String

    Public Change As New Vector3(1)

    Public Sub New(ByVal Entity As Entity, ByVal Scale As Vector3, ByVal Grow As Boolean, ByVal EndSize As Vector3, ByVal SizeSpeed As Single, ByVal startDelay As Single, ByVal endDelay As Single, ByVal Anchors As String)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, Scale, startDelay, endDelay)

        Me.Anchors = Anchors
        Me.Grow = Grow
        Me.EndSize = EndSize
        Me.SizeSpeed = SizeSpeed
        Me.TargetEntity = Entity

        Me.AnimationType = AnimationTypes.Size
    End Sub

    Public Overrides Sub DoActionActive()
        Dim saveScale As Vector3 = TargetEntity.Scale

        Dim changeX As Single = SizeSpeed * Change.X
        Dim changeY As Single = SizeSpeed * Change.Y
        Dim changeZ As Single = SizeSpeed * Change.Z

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
            Me.Position.Y -= diffY / 2
        End If
        'Top
        If Anchors.Contains("2") = True Then
            Dim diffY As Single = saveScale.Y - TargetEntity.Scale.Y
            Me.Position.Y += diffY / 2
        End If
        'Left
        If Anchors.Contains("3") = True Then
            Dim diffX As Single = saveScale.X - TargetEntity.Scale.X
            Me.Position.X -= diffX / 2
        End If
        'Right
        If Anchors.Contains("4") = True Then
            Dim diffX As Single = saveScale.X - TargetEntity.Scale.X
            Me.Position.X += diffX / 2
        End If

        If Me.EndSize = TargetEntity.Scale Then
            Me.Ready = True
        End If
    End Sub

    Public Sub SetChange(ByVal changeX As Single, ByVal changeY As Single, ByVal changeZ As Single)
        Me.Change = New Vector3(changeX, changeY, changeZ)
    End Sub

End Class