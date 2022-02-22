Public Class BABackground

    Inherits BattleAnimation3D

    Public TransitionSpeed As Single = 0.01F
    Public FadeIn As Boolean = False
    Public FadeOut As Boolean = False
    Public BackgroundOpacity As Single = 1.0F
    Public EndState As Single = 0.0F
    Public Texture As Texture2D

    Public Sub New(ByVal Texture As Texture2D, ByVal TransitionSpeed As Single, ByVal FadeIn As Boolean, FadeOut As Boolean, ByVal EndState As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal StartState As Single = 0.0F)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
        Me.Texture = Texture
        Me.EndState = EndState
        Me.FadeIn = FadeIn
        Me.FadeOut = FadeOut
        Me.TransitionSpeed = TransitionSpeed

        Me.BackgroundOpacity = StartState
        Me.Visible = False

        Me.AnimationType = AnimationTypes.Background
    End Sub

    Public Overrides Sub Render()
        If startDelay = 0.0F AndAlso Me.BackgroundOpacity > 0.0F Then
            Core.SpriteBatch.Draw(Me.Texture, New Rectangle(0, 0, windowSize.Width, windowSize.Height), New Color(255, 255, 255, CInt(255 * Me.BackgroundOpacity)))
        End If
    End Sub

    Public Overrides Sub DoActionActive()
        If Me.FadeIn = True Then
            If Me.EndState > Me.BackgroundOpacity Then
                Me.BackgroundOpacity += Me.TransitionSpeed
                If Me.BackgroundOpacity >= Me.EndState Then
                    Me.BackgroundOpacity = Me.EndState
                    Me.FadeIn = False
                    Me.EndState = 0
                End If
            End If
        Else
            If Me.FadeOut = True Then
                If Me.EndState < Me.BackgroundOpacity Then
                    Me.BackgroundOpacity -= Me.TransitionSpeed
                    If Me.BackgroundOpacity <= Me.EndState Then
                        Me.BackgroundOpacity = Me.EndState
                    End If
                End If
                If Me.BackgroundOpacity = Me.EndState Then
                    Me.Ready = True
                End If
            Else
                Me.BackgroundOpacity = Me.EndState
                Me.Ready = True
            End If
        End If
    End Sub

End Class