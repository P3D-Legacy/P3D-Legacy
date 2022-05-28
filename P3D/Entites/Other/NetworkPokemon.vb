﻿Public Class NetworkPokemon

    Inherits Entity

    Public PlayerID As Integer = 0
    Public LevelFile As String = ""
    Public PokemonTexture As String = ""
    Public faceRotation As Integer = 0
    Dim Moving As Boolean = False

    Dim Texture As Texture2D
    Dim lastRectangle As New Rectangle(0, 0, 0, 0)
    Dim loadedTexture As String = ""

    Dim AnimationX As Integer = 1
    Dim AnimationDelayLength As Single = 1.1F
    Dim AnimationDelay As Single = AnimationDelayLength

    Public Sub New(ByVal pos As Vector3, ByVal PokemonTexture As String, ByVal visible As Boolean)
        MyBase.New(pos.X, pos.Y, pos.Z, "NetworkPokemon", {TextureManager.DefaultTexture}, {0, 0}, False, 0, New Vector3(0.9F), BaseModel.BillModel, 0, "", New Vector3(1))

        Me.Visible = visible

        Me.PokemonTexture = PokemonTexture

        Me.Position = New Vector3(CInt(Me.Position.X), CInt(Me.Position.Y) + 0.001F, CInt(Me.Position.Z))
        Me.NeedsUpdate = True
        Me.CreateWorldEveryFrame = True

        Me.NeedsUpdate = True

        Me.DropUpdateUnlessDrawn = False
    End Sub

    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) as Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.2f
    End Function

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
        End If

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Update()
        If Me.PokemonTexture <> Me.loadedTexture Then
            Me.Texture = Nothing
        End If
        Me.loadedTexture = Me.PokemonTexture

        If Me.PokemonTexture <> "" Then
            Me.ChangeTexture()
            If Moving = True Then
                Me.AnimationDelay -= 0.1F
                If AnimationDelay <= 0.0F Then
                    AnimationX += 1
                    AnimationDelay = AnimationDelayLength
                    If AnimationX > 4 Then
                        AnimationX = 1
                    End If
                End If
            Else
                If Me.Texture.Width = Me.Texture.Height Then
                    AnimationX = 1
                Else
                    Me.AnimationDelay -= 0.1F
                    If AnimationDelay <= 0.0F Then
                        AnimationX += 1
                        AnimationDelay = 2.2F
                        If AnimationX > 4 Then
                            AnimationX = 1
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Public Overrides Sub Render()
        If ConnectScreen.Connected = True Then
            If CBool(GameModeManager.GetGameRuleValue("ShowFollowPokemon", "1")) = True Then
                If Screen.Level.ShowOverworldPokemon = True Then
                    If IsCorrectScreen() = True Then
                        If Me.PokemonTexture <> "" Then
                            If Me.Textures IsNot Nothing Then
                                Dim state = GraphicsDevice.DepthStencilState
                                GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead
                                Draw(Me.Model, {Me.Textures(0)}, False)
                                GraphicsDevice.DepthStencilState = state
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function IsCorrectScreen() As Boolean
        Dim screens() As Screen.Identifications = {Screen.Identifications.BattleCatchScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleGrowStatsScreen, Screen.Identifications.BattleScreen, Screen.Identifications.CreditsScreen, Screen.Identifications.BattleAnimationScreen, Screen.Identifications.ViewModelScreen, Screen.Identifications.HallofFameScreen}
        If screens.Contains(Core.CurrentScreen.Identification) = True Then
            Return False
        Else
            If Core.CurrentScreen.Identification = Screen.Identifications.TransitionScreen Then
                If screens.Contains(CType(Core.CurrentScreen, TransitionScreen).OldScreen.Identification) = True Or screens.Contains(CType(Core.CurrentScreen, TransitionScreen).NewScreen.Identification) = True Then
                    Return False
                End If
            Else
                Dim c As Screen = Core.CurrentScreen
                While c.PreScreen IsNot Nothing
                    c = c.PreScreen
                End While
                If screens.Contains(c.Identification) = True Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Private Sub ChangeTexture()
        If Me.Texture Is Nothing Then
            Dim path As String = Me.PokemonTexture.Replace("[POKEMON|N]", "Pokemon\Overworld\Normal\").Replace("[POKEMON|S]", "Pokemon\Overworld\Shiny\")
            Me.Texture = TextureManager.GetTexture(path)
        End If

        Dim cameraRotation As Integer = Screen.Camera.GetFacingDirection()
        Dim spriteIndex As Integer = Me.faceRotation - cameraRotation

        If spriteIndex < 0 Then
            spriteIndex += 4
        End If

        Dim width As Integer

        If Me.Texture.Width = Me.Texture.Height / 2 Then
            width = CInt(Me.Texture.Width / 2)
        ElseIf Me.Texture.Width = Me.Texture.Height Then
            width = CInt(Me.Texture.Width / 4)
        Else
            width = CInt(Me.Texture.Width / 3)
        End If

        Dim x As Integer = GetAnimationX() * width

        Dim height As Integer = CInt(Me.Texture.Height / 4)

        Dim y As Integer = height * spriteIndex

        Dim r = New Rectangle(x, y, width, height)

        If r <> lastRectangle Then
            lastRectangle = r

            Dim t As Texture2D = TextureManager.GetTexture(Me.Texture, r, 1)
            Textures(0) = t
        End If
    End Sub

    Private Function GetAnimationX() As Integer
        If Me.Texture.Width = Me.Texture.Height / 2 Then
            Select Case AnimationX
                Case 1
                    Return 0
                Case 2
                    Return 1
                Case 3
                    Return 0
                Case 4
                    Return 1
            End Select
        ElseIf Me.Texture.Width = Me.Texture.Height Then
            Select Case AnimationX
                Case 1
                    Return 0
                Case 2
                    Return 1
                Case 3
                    Return 2
                Case 4
                    Return 3
            End Select
        Else
            Select Case AnimationX
                Case 1
                    Return 0
                Case 2
                    Return 1
                Case 3
                    Return 0
                Case 4
                    Return 2
            End Select
        End If
        Return 0
    End Function

    Public Sub ApplyShaders()
        Me.Shaders.Clear()
        For Each Shader As Shader In Screen.Level.Shaders
            Shader.ApplyShader({Me})
        Next
    End Sub

    Public Sub ApplyPlayerData(ByVal p As Servers.Player)
        Try
            Me.PlayerID = p.ServersID
            Me.Moving = p.Moving
            Me.PokemonTexture = p.PokemonSkin
            Me.Position = p.PokemonPosition
            Me.LevelFile = p.LevelFile
            Me.Visible = p.PokemonVisible
            Me.faceRotation = p.PokemonFacing
            Me.FaceDirection = p.PokemonFacing

            If Me.Visible = True Then
                Me.Visible = False
                If Screen.Level.LevelFile = p.LevelFile Then
                    Me.Visible = True
                Else
                    If LevelLoader.LoadedOffsetMapNames.Contains(p.LevelFile) = True Then
                        Offset = LevelLoader.LoadedOffsetMapOffsets(LevelLoader.LoadedOffsetMapNames.IndexOf(p.LevelFile))
                        Me.Position.X += Offset.X
                        Me.Position.Y += Offset.Y
                        Me.Position.Z += Offset.Z
                        Me.Visible = True
                    End If
                End If
            End If
        Catch : End Try
    End Sub

End Class