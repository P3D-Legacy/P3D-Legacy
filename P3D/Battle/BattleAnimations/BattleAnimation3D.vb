﻿Public Class BattleAnimation3D

    Inherits Entity
    Public Enum AnchorTypes
        Top
        Left
        Right
        Bottom
    End Enum
    Public Enum AnimationTypes
        [Nothing]
        Move
        Transition
        Size
        Rotation
        Texture
        Wait
        ViewPokeBill
        Sound
        Background
    End Enum

    Public AnimationType As AnimationTypes = AnimationTypes.Nothing
    Public CanRemove As Boolean = False
    Public Ready As Boolean = False
    Public startDelay As Single
    Public endDelay As Single
    Public SpawnedEntity As Boolean = False

    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal startDelay As Single, ByVal endDelay As Single, Optional SpawnedEntity As Boolean = False)
        MyBase.New(Position.X, Position.Y, Position.Z, "BattleAnimation", {Texture}, {0, 0}, False, 0, Scale, BaseModel.BillModel, 0, "", New Vector3(1.0F))

        Me.startDelay = startDelay
        Me.endDelay = endDelay

        Me.SpawnedEntity = SpawnedEntity

        Me.CreateWorldEveryFrame = True
        Me.DropUpdateUnlessDrawn = False
    End Sub

    Public Overrides Sub Update()
        If CanRemove = False Then
            If Ready = True Then
                If endDelay > 0.0F Then
                    endDelay -= 0.1F

                    If endDelay <= 0.0F Then
                        endDelay = 0.0F
                    End If
                Else
                    CanRemove = True
                    DoRemoveEntity()
                End If
            Else
                If startDelay > 0.0F Then
                    startDelay -= 0.1F

                    If startDelay <= 0.0F Then
                        startDelay = 0.0F
                    End If
                Else
                    If SpawnedEntity = True Then
                        Ready = True
                    Else
                        Me.Visible = True
                    End If
                    DoActionActive()
                End If
            End If
        End If

        MyBase.Update()
    End Sub

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
        End If

        DoActionUpdate()

        MyBase.UpdateEntity()
    End Sub

    Public Overridable Sub DoActionUpdate()
        'Insert code in Inherits class for every update here.
    End Sub

    Public Overridable Sub DoActionActive()
        'Insert code in Inherits class here.
    End Sub
    Public Overridable Sub DoRemoveEntity()
        'Insert code in Inherits class here.
    End Sub

    Public Overrides Sub Render()
        If Me.startDelay <= 0.0F Then
            If CanRemove = False Then
                Draw(Me.Model, Me.Textures, True)
            End If
        End If
    End Sub

End Class