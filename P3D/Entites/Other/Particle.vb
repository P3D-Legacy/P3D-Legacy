Public Class Particle

    Inherits Entity

    Public Enum Behaviors
        Floating
        Falling
        Rising
        LeftToRight
        RightToLeft
    End Enum

    Public MoveSpeed As Single = 0.01F
    Public Delay As Single = 10.0F
    Public Behavior As Behaviors = Behaviors.Falling

    Public Destination As Single = 999.0F

    Public Shared ReadOnly UpdateOnIdentifications() as Screen.Identifications = {Screen.Identifications.OverworldScreen, Screen.Identifications.BattleScreen, Screen.Identifications.BattleCatchScreen}

    Public Sub New(ByVal Position As Vector3, ByVal Textures() As Texture2D, TextureIndex() As Integer, ByVal Rotation As Integer, ByVal Scale As Vector3, ByVal Model As BaseModel, ByVal Shader As Vector3)
        MyBase.New(Position.X, Position.Y, Position.Z, "Particle", Textures, TextureIndex, False, Rotation, Scale, Model, 0, "", Shader)

        Me.NeedsUpdate = True
        Me.CreateWorldEveryFrame = True

        If Destination = 999.0F Then
            Me.Destination = Me.Position.Y - 2.8F
        End If

        Me.DropUpdateUnlessDrawn = False
        Me.NormalOpacity = 0F
    End Sub

    Public Overrides Sub Update()
        If Array.Exists(UpdateOnIdentifications, Function(identifications) identifications = Core.CurrentScreen.Identification) Then
            Select Case Me.Behavior
                Case Behaviors.Falling
                    Me.Position.Y -= Me.MoveSpeed
                    If Me.Position.Y <= Me.Destination Then
                        Visible = False
                    End If
                Case Behaviors.Floating

                Case Behaviors.Rising
                    Me.Position.Y -= Me.MoveSpeed
                    If Me.Position.Y >= Me.Destination Then
                        Visible = False
                    End If
                Case Behaviors.LeftToRight
                    Me.Position.X += Me.MoveSpeed
                    Me.Position.Y -= Me.MoveSpeed / 4

                    If Me.Position.X >= Me.Destination Then
                        Visible = False
                    End If
                Case Behaviors.RightToLeft
                    Me.Position.X += Me.MoveSpeed
                    Me.Position.Y += Me.MoveSpeed / 4

                    If Me.Position.X >= Me.Destination Then
                        Visible = False
                    End If
            End Select

            If Me.NormalOpacity < 1.0F Then
                Me.NormalOpacity += 0.05F
                If Me.NormalOpacity >= 1 Then
                    Me.NormalOpacity = 1.0F
                End If
            End If
        End If
    End Sub

    Public Sub Reset(position As Vector3, rotation As Vector3, scale As Vector3, texture As Texture2D)
        Me.Position = position
        Me.Rotation = rotation
        Me.Scale = scale
        Textures(0) = texture
        Visible = True
    End Sub

    Public Sub MoveWithCamera(ByVal diff As Vector3)
        Me.Position -= diff
        Select Case Me.Behavior
            Case Behaviors.Falling, Behaviors.Floating 'y
                Me.Destination -= diff.Y
            Case Behaviors.Floating
                Me.Destination += diff.Y
            Case Else 'x
                Me.Destination -= diff.X
        End Select
    End Sub

    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) as Single
        Return MyBase.CalculateCameraDistance(CPosition) - 1000000f
    End Function

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
        End If

        Dim c_pitch As Single = Screen.Camera.Pitch
        Me.Rotation.X = c_pitch / 2.0F

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Render()
        MyBase.Draw(Me.BaseModel, Me.Textures, False)
    End Sub

End Class