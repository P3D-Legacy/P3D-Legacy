Public Class BAEntityOpacity

    Inherits BattleAnimation3D

    Public TargetEntity As Entity
    Public TransitionSpeed As Single = 0.01F
    Public FadeIn As Boolean = False
    Public EndState As Single = 0.0F
    Public RemoveEntityAfter As Boolean

    Public Sub New(ByVal entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal TransitionSpeed As Single, ByVal FadeIn As Boolean, ByVal EndState As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal StartState As Single = 1.0F)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
        Me.RemoveEntityAfter = RemoveEntityAfter
        Me.EndState = EndState
        Me.FadeIn = FadeIn
        Me.TransitionSpeed = TransitionSpeed
        Me.TargetEntity = entity

        Me.TargetEntity.NormalOpacity = StartState
        Me.Visible = False

        Me.AnimationType = AnimationTypes.Transition
    End Sub

    Public Overrides Sub DoActionActive()
        If Me.FadeIn = True Then
            If Me.EndState > TargetEntity.NormalOpacity Then
                TargetEntity.NormalOpacity += Me.TransitionSpeed
                If TargetEntity.NormalOpacity >= Me.EndState Then
                    TargetEntity.NormalOpacity = Me.EndState
                End If
            End If
        Else
            If Me.EndState < TargetEntity.NormalOpacity Then
                TargetEntity.NormalOpacity -= Me.TransitionSpeed
                If TargetEntity.NormalOpacity <= Me.EndState Then
                    TargetEntity.NormalOpacity = Me.EndState
                End If
            End If
        End If

        If TargetEntity.NormalOpacity = Me.EndState Then
            Me.Ready = True
        End If
    End Sub
    Public Overrides Sub DoRemoveEntity()
        If Me.RemoveEntityAfter = True Then
            TargetEntity.CanBeRemoved = True
        End If
    End Sub
End Class