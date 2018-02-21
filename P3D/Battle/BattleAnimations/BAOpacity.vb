Public Class BAOpacity

    Inherits BattleAnimation3D

    Public TransitionSpeed As Single = 0.01F
    Public FadeIn As Boolean = False
    Public EndState As Single = 0.0F

    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal TransitionSpeed As Single, ByVal FadeIn As Boolean, ByVal EndState As Single, ByVal startDelay As Single, ByVal endDelay As Single)
        MyBase.New(Position, Texture, Scale, startDelay, endDelay)

        Me.EndState = EndState
        Me.FadeIn = FadeIn
        Me.TransitionSpeed = TransitionSpeed

        Me.AnimationType = AnimationTypes.Transition
    End Sub

    Public Overrides Sub DoActionActive()
        If Me.FadeIn = True Then
            If Me.EndState > Me.Opacity Then
                Me.Opacity += Me.TransitionSpeed
                If Me.Opacity >= Me.EndState Then
                    Me.Opacity = Me.EndState
                End If
            End If
        Else
            If Me.EndState < Me.Opacity Then
                Me.Opacity -= Me.TransitionSpeed
                If Me.Opacity <= Me.EndState Then
                    Me.Opacity = Me.EndState
                End If
            End If
        End If

        If Me.Opacity = Me.EndState Then
            Me.Ready = True
        End If
    End Sub

End Class