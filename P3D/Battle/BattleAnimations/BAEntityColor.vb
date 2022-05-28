Public Class BAEntityColor

    Inherits BattleAnimation3D

    Public TargetEntity As Entity
    Public TransitionSpeed As Single = 0.01F
    Public FadeIn As Boolean = False
    Public ColorTo As Vector3 = New Vector3(1.0F, 1.0F, 1.0F)

    Public Sub New(ByVal Entity As Entity, ByVal TransitionSpeed As Single, ByVal startDelay As Single, ByVal endDelay As Single, ByVal ColorTo As Color, Optional ByVal ColorFrom As Color = Nothing)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)

        Me.TransitionSpeed = TransitionSpeed
        Me.TargetEntity = Entity

        If Not ColorFrom = Nothing Then
            TargetEntity.Color = ColorFrom.ToVector3
        End If
        Me.ColorTo = ColorTo.ToVector3

        Me.Visible = False

        Me.AnimationType = AnimationTypes.Transition
    End Sub

    Public Overrides Sub DoActionActive()

        If TargetEntity.Color.X > ColorTo.X Then
            TargetEntity.Color.X -= CByte(Me.TransitionSpeed)
            If TargetEntity.Color.X <= ColorTo.X Then
                TargetEntity.Color.X = ColorTo.X
            End If
        ElseIf TargetEntity.Color.X < ColorTo.X Then
            TargetEntity.Color.X += CByte(Me.TransitionSpeed)
            If TargetEntity.Color.X >= ColorTo.X Then
                TargetEntity.Color.X = ColorTo.X
            End If
        End If
        If TargetEntity.Color.Y > ColorTo.Y Then
            TargetEntity.Color.Y -= CByte(Me.TransitionSpeed)
            If TargetEntity.Color.Y <= ColorTo.Y Then
                TargetEntity.Color.Y = ColorTo.Y
            End If
        ElseIf TargetEntity.Color.Y < ColorTo.Y Then
            TargetEntity.Color.Y += CByte(Me.TransitionSpeed)
            If TargetEntity.Color.Y >= ColorTo.Y Then
                TargetEntity.Color.Y = ColorTo.Y
            End If
        End If
        If TargetEntity.Color.Z > ColorTo.Z Then
            TargetEntity.Color.Z -= CByte(Me.TransitionSpeed)
            If TargetEntity.Color.Z <= ColorTo.Z Then
                TargetEntity.Color.Z = ColorTo.Z
            End If
        ElseIf TargetEntity.Color.Z < ColorTo.Z Then
            TargetEntity.Color.Z += CByte(Me.TransitionSpeed)
            If TargetEntity.Color.Z >= ColorTo.Z Then
                TargetEntity.Color.Z = ColorTo.Z
            End If
        End If

        If TargetEntity.Color = ColorTo Then
            Me.Ready = True
        End If
    End Sub

End Class