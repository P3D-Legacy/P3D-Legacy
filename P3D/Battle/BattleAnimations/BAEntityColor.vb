Public Class BAEntityColor

    Inherits BattleAnimation3D

    Public TargetEntity As Entity
    Public TransitionSpeed As Single = 0.01F
    Public TransitionSpeedOut As Single = 0.01F
    Public FadeIn As Boolean = False
    Public ReturnToFromWhenDone As Boolean = False
    Public RemoveEntityAfter As Boolean = False
    Public InitialColorSet As Boolean = False
    Public IsReturning As Boolean = False
    Public ColorTo As Vector3 = New Vector3(1.0F, 1.0F, 1.0F)
    Public ColorFrom As Vector3 = New Vector3(1.0F, 1.0F, 1.0F)

    Public Sub New(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal TransitionSpeedIn As Single, ByVal ReturnToFromWhenDone As Boolean, ByVal startDelay As Single, ByVal endDelay As Single, ByVal ColorTo As Vector3, Optional ByVal ColorFrom As Vector3 = Nothing, Optional TransitionSpeedOut As Single = -1)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
        Me.RemoveEntityAfter = RemoveEntityAfter
        If TransitionSpeedOut = -1 Then
            Me.TransitionSpeedOut = TransitionSpeedIn
        Else
            Me.TransitionSpeedOut = TransitionSpeedOut
        End If
        Me.TransitionSpeed = TransitionSpeedIn
        Me.TargetEntity = Entity
        Me.ReturnToFromWhenDone = ReturnToFromWhenDone

        If Not ColorFrom = Nothing Then
            Me.ColorFrom = ColorFrom
        Else
            Me.ColorFrom = TargetEntity.Color
        End If
        Me.ColorTo = ColorTo

        Me.Visible = False

        Me.AnimationType = AnimationTypes.Transition
    End Sub

    Public Overrides Sub DoActionActive()
        If InitialColorSet = False Then
            TargetEntity.Color = ColorFrom
            InitialColorSet = True
        End If

        If TargetEntity.Color.X > ColorTo.X Then
            TargetEntity.Color.X -= Me.TransitionSpeed
            If TargetEntity.Color.X <= ColorTo.X Then
                TargetEntity.Color.X = ColorTo.X
            End If
        ElseIf TargetEntity.Color.X < ColorTo.X Then
            TargetEntity.Color.X += Me.TransitionSpeed
            If TargetEntity.Color.X >= ColorTo.X Then
                TargetEntity.Color.X = ColorTo.X
            End If
        End If
        If TargetEntity.Color.Y > ColorTo.Y Then
            TargetEntity.Color.Y -= Me.TransitionSpeed
            If TargetEntity.Color.Y <= ColorTo.Y Then
                TargetEntity.Color.Y = ColorTo.Y
            End If
        ElseIf TargetEntity.Color.Y < ColorTo.Y Then
            TargetEntity.Color.Y += Me.TransitionSpeed
            If TargetEntity.Color.Y >= ColorTo.Y Then
                TargetEntity.Color.Y = ColorTo.Y
            End If
        End If
        If TargetEntity.Color.Z > ColorTo.Z Then
            TargetEntity.Color.Z -= Me.TransitionSpeed
            If TargetEntity.Color.Z <= ColorTo.Z Then
                TargetEntity.Color.Z = ColorTo.Z
            End If
        ElseIf TargetEntity.Color.Z < ColorTo.Z Then
            TargetEntity.Color.Z += Me.TransitionSpeed
            If TargetEntity.Color.Z >= ColorTo.Z Then
                TargetEntity.Color.Z = ColorTo.Z
            End If
        End If

        If TargetEntity.Color = ColorTo Then
            If ReturnToFromWhenDone = False Then
                Me.Ready = True
            Else
                If IsReturning = False Then
                    ColorTo = ColorFrom
                    TransitionSpeed = TransitionSpeedOut
                    IsReturning = True
                Else
                    Me.Ready = True
                End If
            End If
        End If
    End Sub
    Public Overrides Sub DoRemoveEntity()
        If Me.RemoveEntityAfter = True Then
            TargetEntity.CanBeRemoved = True
        End If
    End Sub
End Class