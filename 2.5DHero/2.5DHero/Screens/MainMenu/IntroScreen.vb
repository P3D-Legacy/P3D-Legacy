Public Class IntroScreen

    Inherits Screen

    Private Enum IntroStages As Integer
        RevealPokemon = 0
        P3DMoveIn = 1
    End Enum

    Dim _pokemonLogoTexture As Texture2D
    Dim _3DLogoTexture As Texture2D
    Dim _introStage As IntroStages = IntroStages.RevealPokemon

    Dim _pokemonRevealStage As Integer = 0
    Dim _3Dposition As Integer = -100
    Dim _pokemonLogoOffset As Integer = 0

    Public Sub New()
        Me.Identification = Identifications.IntroScreen
        Me.CanBePaused = False
        Me.CanMuteMusic = False
        Me.CanChat = False
        Me.CanTakeScreenshot = False
        Me.CanDrawDebug = False
        Me.MouseVisible = True
        Me.CanGoFullscreen = False

        Me._pokemonLogoTexture = Content.Load(Of Texture2D)("GUI\Logos\Pokemon_Small")
        Me._3DLogoTexture = Content.Load(Of Texture2D)("GUI\Logos\3D")

        Me._3Dposition = -(Me._3DLogoTexture.Height * 2)
        Me._pokemonLogoOffset = CInt(Core.windowSize.Height / 2 - Me._pokemonLogoTexture.Height)
    End Sub

    Public Overrides Sub Update()
        Select Case _introStage
            Case IntroStages.RevealPokemon
                Me.UpdateRevealPokemon()
            Case IntroStages.P3DMoveIn
                Me.Update3DMoveIn()
        End Select

        'In the end, do this:
        ' Core.SetScreen(New MainMenuScreen())
    End Sub

    Private Sub UpdateRevealPokemon()
        Dim textureWidth As Integer = Me._pokemonLogoTexture.Width
        If Me._pokemonRevealStage < textureWidth Then
            Me._pokemonRevealStage += 8
            If Me._pokemonRevealStage >= textureWidth Then
                Me._pokemonRevealStage = textureWidth
                Me._introStage = IntroStages.P3DMoveIn
            End If
        End If
    End Sub

    Private Sub Update3DMoveIn()
        Dim p3dLogoWay As Integer = CInt(Core.windowSize.Height / 2)

        If _3Dposition < p3dLogoWay Then
            _3Dposition += 6
        End If

        If _pokemonLogoOffset > CInt(Core.windowSize.Height / 2 - Me._pokemonLogoTexture.Height * 1.5) Then
            _pokemonLogoOffset -= 3
        End If
    End Sub

    Public Overrides Sub Draw()
        Core.SpriteBatch.DrawRectangle(Core.windowSize, Color.Black)

        Select Case _introStage
            Case IntroStages.RevealPokemon
                Me.DrawRevealPokemon()
            Case IntroStages.P3DMoveIn
                Me.Draw3DMoveIn()
        End Select
    End Sub

    Private Sub DrawRevealPokemon()
        Core.SpriteBatch.Draw(Me._pokemonLogoTexture, New Rectangle(CInt(Core.windowSize.Width / 2 - Me._pokemonLogoTexture.Width), CInt(Core.windowSize.Height / 2 - Me._pokemonLogoTexture.Height), Me._pokemonRevealStage * 2, Me._pokemonLogoTexture.Height * 2), New Rectangle(0, 0, Me._pokemonRevealStage, Me._pokemonLogoTexture.Height), Color.White)

        If Me._pokemonRevealStage < Me._pokemonLogoTexture.Width Then
            Canvas.DrawGradient(New Rectangle(CInt(Core.windowSize.Width / 2 - Me._pokemonLogoTexture.Width + Me._pokemonRevealStage * 2 - 60), CInt(Core.windowSize.Height / 2 - Me._pokemonLogoTexture.Height), 60, Me._pokemonLogoTexture.Height * 2), New Color(0, 0, 0, 0), Color.Black, True, -1)
        End If
    End Sub

    Private Sub Draw3DMoveIn()
        Core.SpriteBatch.Draw(Me._3DLogoTexture, New Rectangle(CInt(Core.windowSize.Width / 2 - Me._3DLogoTexture.Width), _3Dposition, Me._3DLogoTexture.Width * 2, Me._3DLogoTexture.Height * 2), Color.White)
        Core.SpriteBatch.Draw(Me._pokemonLogoTexture, New Rectangle(CInt(Core.windowSize.Width / 2 - Me._pokemonLogoTexture.Width), _pokemonLogoOffset, Me._pokemonLogoTexture.Width * 2, Me._pokemonLogoTexture.Height * 2), Color.White)
    End Sub

End Class