Public Class OverworldPokemon

    Inherits Entity

    Public PokemonID As Integer = 0
    Public WithEvents PokemonReference As Pokemon = Nothing

    Public Texture As Texture2D
    Dim lastRectangle As New Rectangle(0, 0, 0, 0)
    Public faceRotation As Integer = 0
    Public MoveSpeed As Single = 0.04F
    Public warped As Boolean = True

    Dim AnimationX As Integer = 1
    Dim AnimationDelayLenght As Single = 2.2F
    Dim AnimationDelay As Single = AnimationDelayLenght

    Public Sub New(ByVal X As Single, ByVal Y As Single, ByVal Z As Single)
        MyBase.New(X, Y, Z, "OverworldPokemon", {net.Pokemon3D.Game.TextureManager.DefaultTexture}, {0, 0}, False, 0, New Vector3(1.0F), BaseModel.BillModel, 0, "", New Vector3(1))

        Me.Respawn()
        If Core.Player.LastPokemonPosition = New Vector3(999, 999, 999) Then
            Me.Position = New Vector3(Screen.Camera.Position.X, Screen.Camera.Position.Y, Screen.Camera.Position.Z)
            Me.Visible = False
            Me.warped = False
        Else
            Me.Position = Core.Player.LastPokemonPosition
        End If

        Me.Position = New Vector3(CInt(Me.Position.X), Me.GetYPosition(), CInt(Me.Position.Z))
        Me.NeedsUpdate = True
        Me.CreateWorldEveryFrame = True

        Me.DropUpdateUnlessDrawn = False
    End Sub

    Private Sub ChangeTexture()
        If Me.Texture Is Nothing Then
            Me.Texture = PokemonReference.GetOverworldTexture()
        End If

        Dim r As New Rectangle(0, 0, 0, 0)
        Dim cameraRotation As Integer = Screen.Camera.GetFacingDirection()
        Dim spriteIndex As Integer = Me.faceRotation - cameraRotation

        spriteIndex = Me.faceRotation - cameraRotation
        If spriteIndex < 0 Then
            spriteIndex += 4
        End If

        Dim width As Integer = CInt(Me.Texture.Width / 3)

        Dim x As Integer = 0
        x = AnimationX * width

        Dim height As Integer = CInt(Me.Texture.Height / 4)

        Dim y As Integer = height * spriteIndex

        r = New Rectangle(x, y, width, height)

        If r <> lastRectangle Then
            lastRectangle = r

            Dim t As Texture2D = TextureManager.GetTexture(Me.Texture, r, 1)
            Textures(0) = t
        End If
    End Sub

    Public Overrides Sub Update()
        If Not Core.Player.GetWalkPokemon() Is Nothing Then
            Dim differentAdditionalData As Boolean = False
            Dim differentShinyState As Boolean = False
            If Not Me.PokemonReference Is Nothing Then
                differentAdditionalData = (Me.PokemonReference.AdditionalData <> Core.Player.GetWalkPokemon().AdditionalData)
                differentShinyState = (Me.PokemonReference.IsShiny <> Core.Player.GetWalkPokemon().IsShiny)
            End If

            If Me.PokemonID <> Core.Player.GetWalkPokemon().Number Or differentAdditionalData = True Or differentShinyState = True Then
                Me.Texture = Nothing
                Me.PokemonID = Core.Player.GetWalkPokemon().Number
                Me.PokemonReference = Core.Player.GetWalkPokemon()
            End If

            Me.ChangeTexture()

            Me.AnimationDelay -= 0.1F
            If AnimationDelay <= 0.0F Then
                AnimationDelay = AnimationDelayLenght
                AnimationX += 1
                If AnimationX > 2 Then
                    AnimationX = 1
                End If
            End If

            ChangePosition()
        End If
    End Sub

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
        End If
        Me.Scale = New Vector3(1.0F)
        Me.Position.Y = Me.GetYPosition()

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Render()
        If Me.IsVisible() = True Then
            Me.Draw(Me.Model, {Me.Textures(0)}, False)
        End If
    End Sub

    ''' <summary>
    ''' If the OverworldPokémon should be rendered.
    ''' </summary>
    Public Function IsVisible() As Boolean
        If CBool(GameModeManager.GetGameRuleValue("ShowFollowPokemon", "1")) = True Then
            If Screen.Level.ShowOverworldPokemon = True Then
                If IsCorrectScreen() = True Then
                    If Not Core.Player.GetWalkPokemon() Is Nothing Then
                        If Screen.Level.Surfing = False And Screen.Level.Riding = False Then
                            If Me.PokemonID > 0 Then
                                If Not Me.Textures Is Nothing Then
                                    Return True
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Public Sub ChangeRotation()
        Me.Position = New Vector3(CInt(Me.Position.X), CInt(Me.Position.Y) + 0.001F, CInt(Me.Position.Z))
        If Screen.Camera.Position.X = CInt(Me.Position.X) Or Screen.Camera.Position.Z = CInt(Me.Position.Z) Then
            If Me.Position.X < Screen.Camera.Position.X Then
                Me.faceRotation = 3
            ElseIf Me.Position.X > Screen.Camera.Position.X Then
                Me.faceRotation = 1
            End If
            If Me.Position.Z < Screen.Camera.Position.Z Then
                Me.faceRotation = 2
            ElseIf Me.Position.Z > Screen.Camera.Position.Z Then
                Me.faceRotation = 0
            End If
        End If
    End Sub

    Private Sub ChangePosition()
        If Screen.Camera.IsMoving() = True Then
            If CInt(Me.Position.X) <> CInt(Screen.Camera.Position.X) Or CInt(Me.Position.Z) <> CInt(Screen.Camera.Position.Z) Then
                Me.Position += GetMove()
                Me.AnimationDelayLenght = 1.1F
            End If
        Else
            Me.AnimationDelayLenght = 2.2F
        End If
    End Sub

    Private Function GetMove() As Vector3
        Dim moveVector As Vector3
        Select Case Me.faceRotation
            Case 0
                moveVector = New Vector3(0, 0, -1) * MoveSpeed
            Case 1
                moveVector = New Vector3(-1, 0, 0) * MoveSpeed
            Case 2
                moveVector = New Vector3(0, 0, 1) * MoveSpeed
            Case 3
                moveVector = New Vector3(1, 0, 0) * MoveSpeed
        End Select
        Return moveVector
    End Function

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
                While Not c.PreScreen Is Nothing
                    c = c.PreScreen
                End While
                If screens.Contains(c.Identification) = True Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Public Sub MakeVisible()
        If warped = True Then
            warped = False
        Else
            If Me.Visible = False Then
                Me.Visible = True
                Me.Respawn()
            End If
        End If
    End Sub

    Public Sub Respawn()
        Dim newPosition As New Vector3(0, -2, 0)
        If Screen.Camera.Name = "Overworld" Then
            newPosition = CType(Screen.Camera, OverworldCamera).LastStepPosition
        End If
        If newPosition <> New Vector3(0, -2, 0) Then
            Me.Position = newPosition
        Else
            Select Case Screen.Camera.GetPlayerFacingDirection()
                Case 0
                    Me.Position = New Vector3(Screen.Camera.Position.X, Me.GetYPosition(), Screen.Camera.Position.Z + 1)
                Case 1
                    Me.Position = New Vector3(Screen.Camera.Position.X + 1, Me.GetYPosition(), Screen.Camera.Position.Z)
                Case 2
                    Me.Position = New Vector3(Screen.Camera.Position.X, Me.GetYPosition(), Screen.Camera.Position.Z - 1)
                Case 3
                    Me.Position = New Vector3(Screen.Camera.Position.X - 1, Me.GetYPosition(), Screen.Camera.Position.Z)
            End Select
        End If

        ChangeRotation()
    End Sub

    Public Overrides Sub ClickFunction()
        If CBool(GameModeManager.GetGameRuleValue("ShowFollowPokemon", "1")) = True Then
            If Me.Visible = True And Not Core.Player.GetWalkPokemon() Is Nothing And Screen.Level.Surfing = False And Screen.Level.Riding = False And Screen.Level.ShowOverworldPokemon = True Then
                Dim p As Pokemon = Core.Player.GetWalkPokemon()
                Dim scriptString As String = PokemonInteractions.GetScriptString(p, Me.Position, Me.faceRotation)

                If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                    If CType(Core.CurrentScreen, OverworldScreen).ActionScript.IsReady = True Then
                        CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(scriptString, 2)
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub ApplyShaders()
        Me.Shaders.Clear()
        For Each Shader As Shader In Screen.Level.Shaders
            Shader.ApplyShader({Me})
        Next
    End Sub

    Private Sub PokemonReference_TexturesCleared(sender As Object, e As EventArgs) Handles PokemonReference.TexturesCleared
        Me.Texture = Nothing
        Me.ForceTextureChange()
    End Sub

    Private Function GetYPosition() As Single
        Return CInt(Screen.Camera.Position.Y)
    End Function

    Public Sub ForceTextureChange()
        Me.lastRectangle = New Rectangle(0, 0, 0, 0)
        Me.ChangeTexture()
    End Sub

End Class
