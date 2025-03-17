Public Class BABackground

    Inherits BattleAnimation3D

    Dim FadeInSpeed As Single = 0.01F
    Dim FadeOutSpeed As Single = 0.01F
    Dim BackgroundOpacity As Single = 0.0F
    Dim Texture As Texture2D
    Dim DoTile As Boolean = False
    Dim AnimationWidth As Integer = -1
    Dim AfterFadeInOpacity As Single = 1.0F
    Dim FadeProgress As FadeSteps = FadeSteps.FadeIn
    Dim DurationDate As Date
    Dim DurationWhole As Single
    Dim DurationFraction As Single
    Dim BackgroundAnimation As Animation
    Dim CurrentRectangle As New Rectangle(0, 0, 0, 0)
    Dim TextureScale As Integer = 4

    Public Enum FadeSteps As Integer
        FadeIn
        Duration
        FadeOut
    End Enum

    Public Sub New(ByVal Texture As Texture2D, ByVal startDelay As Single, ByVal endDelay As Single, ByVal Duration As Single, Optional ByVal AfterFadeInOpacity As Single = 1.0F, Optional ByVal FadeInSpeed As Single = 0.125F, Optional ByVal FadeOutSpeed As Single = 0.125F, Optional ByVal DoTile As Boolean = False, Optional ByVal AnimationLength As Integer = 1, Optional ByVal AnimationSpeed As Integer = 2, Optional TextureScale As Integer = 4)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
        Me.Texture = Texture
        Me.AfterFadeInOpacity = AfterFadeInOpacity
        Me.FadeInSpeed = FadeInSpeed
        Me.FadeOutSpeed = FadeOutSpeed
        Me.DoTile = DoTile
        Me.AnimationWidth = CInt(Texture.Width / AnimationLength)
        DurationWhole = CSng(Math.Truncate(CDbl(Duration / 6.0F)))
        DurationFraction = CSng((Duration / 6.0F - DurationWhole) * 1000)
        Me.TextureScale = TextureScale

        If Me.AnimationWidth <> -1 OrElse Me.AnimationWidth <> Nothing Then
            BackgroundAnimation = New Animation(Me.Texture, 1, AnimationLength, Me.AnimationWidth, Me.Texture.Height, AnimationSpeed * 24, 0, 0)
            CurrentRectangle = BackgroundAnimation.TextureRectangle
        Else
            Me.AnimationWidth = Texture.Width
        End If
        Me.Visible = False

        Me.AnimationType = AnimationTypes.Background
    End Sub

    Public Overrides Sub Render()
        Dim BackgroundTarget As New RenderTarget2D(Core.GraphicsDevice, Core.windowSize.Width, Core.windowSize.Height, False, SurfaceFormat.Color, DepthFormat.Depth24Stencil8)
        Core.GraphicsDevice.SetRenderTarget(BackgroundTarget)
        GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Transparent)

        If Date.Now >= startDelay AndAlso Me.BackgroundOpacity > 0.0F Then
            If DoTile = False Then
                Core.SpriteBatch.Draw(Texture, New Rectangle(0, 0, windowSize.Width, windowSize.Height), CurrentRectangle, New Color(255, 255, 255, CInt(255 * Me.BackgroundOpacity)))
            Else
                For Dx = 0 To Core.windowSize.Width Step AnimationWidth
                    For Dy = 0 To Core.windowSize.Height Step Texture.Height
                        Core.SpriteBatch.Draw(Texture, New Rectangle(Dx * TextureScale, Dy * TextureScale, AnimationWidth * TextureScale, Texture.Height * TextureScale), CurrentRectangle, New Color(255, 255, 255, CInt(255 * Me.BackgroundOpacity)))
                    Next
                Next
            End If
        End If
        Core.GraphicsDevice.SetRenderTarget(Nothing)
        Core.SpriteBatch.Draw(BackgroundTarget, windowSize, New Color(255, 255, 255, CInt(255 * Me.BackgroundOpacity)))
    End Sub

    Public Overrides Sub DoActionActive()
        If BackgroundAnimation IsNot Nothing Then
            BackgroundAnimation.Update(0.005)
            If CurrentRectangle <> BackgroundAnimation.TextureRectangle Then
                CurrentRectangle = BackgroundAnimation.TextureRectangle
            End If
        End If
        Select Case Me.FadeProgress
            Case FadeSteps.FadeIn
                If Me.AfterFadeInOpacity > Me.BackgroundOpacity Then
                    Me.BackgroundOpacity += Me.FadeInSpeed
                    If Me.BackgroundOpacity >= Me.AfterFadeInOpacity Then
                        DurationDate = Date.Now + New TimeSpan(0, 0, 0, CInt(DurationWhole), CInt(DurationFraction))
                        FadeProgress = FadeSteps.Duration
                        Me.BackgroundOpacity = Me.AfterFadeInOpacity
                    End If
                Else
                    FadeProgress = FadeSteps.Duration
                    Me.BackgroundOpacity = Me.AfterFadeInOpacity
                End If
            Case FadeSteps.Duration
                If Date.Now >= DurationDate Then
                    FadeProgress = FadeSteps.FadeOut
                End If
            Case FadeSteps.FadeOut
                If Me.BackgroundOpacity > 0.0F Then
                    Me.BackgroundOpacity -= Me.FadeOutSpeed
                    If Me.BackgroundOpacity <= 0.0F Then
                        Me.BackgroundOpacity = 0.0F
                        Me.Ready = True
                    End If
                Else
                    Me.BackgroundOpacity = 0.0F
                    Me.Ready = True
                End If

        End Select

    End Sub

End Class