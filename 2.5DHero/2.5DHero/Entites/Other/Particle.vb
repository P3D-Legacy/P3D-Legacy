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

    Dim Realtive As New Vector3(0)
    Dim LastPosition As Vector3
    Dim time As Single = 0

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
        Dim identifications() As Screen.Identifications = {Screen.Identifications.OverworldScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleScreen, Screen.Identifications.BattleCatchScreen}
        If identifications.Contains(Core.CurrentScreen.Identification) = True Then
            Select Case Me.Behavior
                Case Behaviors.Falling
                    Me.Position.Y -= Me.MoveSpeed
                    If Me.Position.Y <= Me.Destination Then
                        Me.CanBeRemoved = True
                    End If
                Case Behaviors.Floating

                Case Behaviors.Rising
                    Me.Position.Y -= Me.MoveSpeed
                    If Me.Position.Y >= Me.Destination Then
                        Me.CanBeRemoved = True
                    End If
                Case Behaviors.LeftToRight
                    Me.Position.X += Me.MoveSpeed
                    Me.Position.Y -= Me.MoveSpeed / 4

                    If Me.Position.X >= Me.Destination Then
                        Me.CanBeRemoved = True
                    End If
                Case Behaviors.RightToLeft
                    Me.Position.X += Me.MoveSpeed
                    Me.Position.Y += Me.MoveSpeed / 4

                    If Me.Position.X >= Me.Destination Then
                        Me.CanBeRemoved = True
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

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
        End If

        Dim c_pitch As Single = Screen.Camera.Pitch
        Me.Rotation.X = c_pitch / 2.0F

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Render()
        MyBase.Draw(Me.Model, Me.Textures, False)
    End Sub

End Class