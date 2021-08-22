Public Class BABillOpacity

    Inherits BattleAnimation3D

    Public TargetEntity As Entity
    Public TransitionSpeed As Single = 0.01F
    Public FadeIn As Boolean = False
    Public EndState As Single = 0.0F

    Public Sub New(ByVal entity As Entity, ByVal TransitionSpeed As Single, ByVal FadeIn As Boolean, ByVal EndState As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal StartState As Single = 1.0F)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
        Me.EndState = EndState
        Me.FadeIn = FadeIn
        Me.TransitionSpeed = TransitionSpeed
        Me.TargetEntity = entity

        Me.TargetEntity.Opacity = StartState
        Me.Visible = False

        Me.AnimationType = AnimationTypes.Transition
    End Sub

    Public Overrides Sub DoActionActive()
        If Me.FadeIn = True Then
            If Me.EndState > TargetEntity.Opacity Then
                TargetEntity.Opacity += Me.TransitionSpeed
                If TargetEntity.Opacity >= Me.EndState Then
                    TargetEntity.Opacity = Me.EndState
                End If
            End If
        Else
            If Me.EndState < TargetEntity.Opacity Then
                TargetEntity.Opacity -= Me.TransitionSpeed
                If TargetEntity.Opacity <= Me.EndState Then
                    TargetEntity.Opacity = Me.EndState
                End If
            End If
        End If

        If TargetEntity.Opacity = Me.EndState Then
            Me.Ready = True
        End If
    End Sub

End Class