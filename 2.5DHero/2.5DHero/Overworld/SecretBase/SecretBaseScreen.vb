Public Class SecretBaseScreen

    Inherits Screen

    Public SavedOverworld As OverworldStorage
    Public SecretBase As SecretBase

    Dim ParticlesTexture As Texture2D

    Public Sub New()
        Me.Identification = Identifications.SecretBaseScreen
        Me.CanBePaused = True

        Me.SavedOverworld = New OverworldStorage()
        Me.SavedOverworld.SetToCurrentEnvironment()

        Me.SecretBase = New SecretBase()

        Effect = New BasicEffect(Core.GraphicsDevice)

        Camera = New SecretBaseCamera()
        Level = New Level()
        Level.Load("|")

        Me.SecretBase.LoadSecretBaseFromStore(Screen.Level)
        MusicPlayer.GetInstance().Play(Level.MusicLoop, True)
    End Sub

    Public Overrides Sub Update()
        'Lighting.UpdateLighting(Screen.Effect)

        Camera.Update()
        Level.Update()
    End Sub

    Public Overrides Sub Draw()
        TextBox.Draw()
        If Me.IsCurrentScreen() = True Then
            ChooseBox.Draw()
        End If

        Level.Draw()
    End Sub

End Class