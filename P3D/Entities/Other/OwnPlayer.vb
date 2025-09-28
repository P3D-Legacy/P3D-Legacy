﻿Public Class OwnPlayer

    Inherits Entity

    Public Texture As Texture2D
    Public SkinName As String = "Hilbert"

    Dim lastRectangle As New Rectangle(0, 0, 0, 0)
    Dim lastTexture As String = ""

    Dim AnimationX As Integer = 1
    Const AnimationDelayLength As Single = 1.1F
    Dim AnimationDelay As Single = AnimationDelayLength
    Public DoAnimation As Boolean = True

    Public UsingGameJoltTexture As Boolean = False

    Public Sub New(ByVal X As Single, ByVal Y As Single, ByVal Z As Single, ByVal Textures() As Texture2D, ByVal TextureID As String, ByVal Rotation As Integer, ByVal ActionValue As Integer, ByVal AdditionalValue As String, ByVal Name As String, ByVal ID As Integer)
        MyBase.New(X, Y, Z, "OwnPlayer", Textures, {0, 0}, False, 0, New Vector3(1.0F), BaseModel.BillModel, 0, "", New Vector3(1.0F))

        SetTexture(TextureID, True)

        Me.NeedsUpdate = True
        Me.CreateWorldEveryFrame = True

        Me.DropUpdateUnlessDrawn = False
    End Sub

    Public Sub SetTexture(ByVal TextureID As String, ByVal UseGameJoltID As Boolean)
        Me.SkinName = TextureID
        Dim texturePath As String = "Textures\NPC\"
        If TextureID.StartsWith("[POKEMON|N]") Or TextureID.StartsWith("[Pokémon|N]") Then
            TextureID = TextureID.Remove(0, 11)
            texturePath = "Pokemon\Overworld\Normal\"
        ElseIf TextureID.StartsWith("[POKEMON|S]") Or TextureID.StartsWith("[Pokémon|S]") Then
            TextureID = TextureID.Remove(0, 11)
            texturePath = "Pokemon\Overworld\Shiny\"
        End If

        Dim PokemonAddition As String = ""
        If StringHelper.IsNumeric(TextureID) And texturePath.StartsWith("Pokemon\Overworld\") Then
            PokemonAddition = PokemonForms.GetDefaultOverworldSpriteAddition(CInt(TextureID))
        End If

        If Core.Player.IsGameJoltSave Then
            If texturePath & TextureID & PokemonAddition = "Textures\NPC\" & GameJolt.Emblem.GetPlayerSpriteFile(GameJolt.Emblem.GetPlayerLevel(Core.GameJoltSave.Points), Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Gender) Then
                UseGameJoltID = True
            End If
        End If

        If UseGameJoltID And Core.Player.IsGameJoltSave And GameJolt.API.LoggedIn AndAlso Not GameJolt.Emblem.GetOnlineSprite(Core.GameJoltSave.GameJoltID) Is Nothing Then
            Logger.Debug("Change player texture to the online sprite.")
            Me.Texture = GameJolt.Emblem.GetOnlineSprite(Core.GameJoltSave.GameJoltID)
            UsingGameJoltTexture = True
        Else
            Logger.Debug("Change player texture to [" & texturePath & TextureID & PokemonAddition & "]")

            Me.Texture = P3D.TextureManager.GetTexture(texturePath & TextureID & PokemonAddition)
            UsingGameJoltTexture = False
        End If
    End Sub

    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) As Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.2F
    End Function

    Public Overrides Sub UpdateEntity()
        If Not Core.CurrentScreen Is Nothing Then
            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                If Screen.Camera.Name = "Overworld" Then
                    Dim c As OverworldCamera = CType(Screen.Camera, OverworldCamera)
                    Me.Position = New Vector3(c.Position.X, c.Position.Y - 0.1F, c.Position.Z)
                End If
            End If
            If Me.Rotation.Y <> Screen.Camera.Yaw Then
                Me.Rotation.Y = Screen.Camera.Yaw
            End If
        End If

        Move()
        ChangeTexture()

        MyBase.UpdateEntity()
    End Sub

    Private Sub Move()
        If Core.Player.DoWalkAnimation = True Then
            If (Screen.Camera.IsMoving() = True And Me.DoAnimation = True) OrElse (Screen.Level.OwnPlayer IsNot Nothing AndAlso Screen.Level.OwnPlayer.isDancing) Then
                If CurrentScreen.Identification <> Screen.Identifications.PauseScreen Then
                    Me.AnimationDelay -= 0.13F
                    If AnimationDelay <= 0.0F Then
                        AnimationDelay = GetAnimationDelay()
                        AnimationX += 1
                        If AnimationX > 4 Then
                            AnimationX = 1
                        End If
                    End If
                Else
                    AnimationX = 1
                    ChangeTexture()
                End If
            Else
                AnimationX = 1
                AnimationDelay = GetAnimationDelay()
                ChangeTexture()
            End If
        Else
            AnimationX = 1
            AnimationDelay = GetAnimationDelay()
            ChangeTexture()
        End If
    End Sub

    Public Sub ChangeTexture()
        If Me.Texture IsNot Nothing Then
            Dim r As New Rectangle(0, 0, 0, 0)
            Dim cameraRotation As Integer = 0
            Dim spriteIndex As Integer = 0

            spriteIndex = 0

            If Screen.Camera.Name = "Overworld" Then
                spriteIndex = Screen.Camera.GetPlayerFacingDirection() - Screen.Camera.GetFacingDirection()
                While spriteIndex > 3
                    spriteIndex -= 4
                End While
                While spriteIndex < 0
                    spriteIndex += 4
                End While
            End If

            Dim frameSize As New Size(CInt(Me.Texture.Width / 3), CInt(Me.Texture.Height / 4))

            If Me.Texture.Width = Me.Texture.Height / 2 Then
                frameSize.Width = CInt(Me.Texture.Width / 2)
            ElseIf Me.Texture.Width = Me.Texture.Height Then
                frameSize.Width = CInt(Me.Texture.Width / 4)
            Else
                frameSize.Width = CInt(Me.Texture.Width / 3)
            End If

            Dim x As Integer = 0
            If Screen.Camera.IsMoving() = True Then
                x = GetAnimationX() * frameSize.Width
            End If

            r = New Rectangle(x, frameSize.Width * spriteIndex, frameSize.Width, frameSize.Height)

            If r <> lastRectangle Or lastTexture <> SkinName Then
                lastRectangle = r
                lastTexture = SkinName
                Core.Player.Skin = SkinName

                Try
                    Dim t As Texture2D = TextureManager.GetTexture(Me.Texture, r, 1)
                    Textures(0) = t
                Catch
                    Logger.Log(Logger.LogTypes.Warning, "OwnPlayer.vb: Error assigning a new texture to the player.")
                End Try
            End If
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

    Public Overrides Sub Render()
        If InCameraFocus() = True Then
            Dim state = GraphicsDevice.DepthStencilState
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead
            Draw(Me.BaseModel, Me.Textures, True)
            GraphicsDevice.DepthStencilState = state
        End If
    End Sub

    Friend Function InCameraFocus() As Boolean
        If Screen.Camera.Name = "Overworld" Then
            Dim c = CType(Screen.Camera, OverworldCamera)

            If c.CameraFocusType = OverworldCamera.CameraFocusTypes.Player And c.ThirdPerson = True Or c.CameraFocusType <> OverworldCamera.CameraFocusTypes.Player Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Sub ApplyShaders()
        Me.Shaders.Clear()
        For Each Shader As Shader In Screen.Level.Shaders
            Shader.ApplyShader({Me})
        Next
    End Sub

    Private Function GetAnimationDelay() As Single
        If Core.Player.IsRunning() = True Then
            Return OwnPlayer.AnimationDelayLength / 1.4F
        End If
        Return OwnPlayer.AnimationDelayLength
    End Function

End Class