Public Class BattleAnimation3D

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
    Public startDelay As Date
    Public endDelay As Date
    Public SpawnedEntity As Boolean = False
    Public Started As Boolean = False
    Private DelayDivide As Single = 6.0F
    Private StartDelayWhole As Single
    Private StartDelayFraction As Single
    Private EndDelayWhole As Single
    Private EndDelayFraction As Single

    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal startDelay As Single, ByVal endDelay As Single, Optional SpawnedEntity As Boolean = False)
        MyBase.New(Position.X, Position.Y, Position.Z, "BattleAnimation", {Texture}, {0, 0}, False, 0, Scale, BaseModel.BillModel, 0, "", New Vector3(1.0F))

        StartDelayWhole = CSng(Math.Truncate(CDbl(startDelay / DelayDivide)))
        StartDelayFraction = startDelay / DelayDivide - StartDelayWhole
        EndDelayWhole = CSng(Math.Truncate(CDbl(endDelay / DelayDivide)))
        EndDelayFraction = endDelay / DelayDivide - EndDelayWhole

        Me.SpawnedEntity = SpawnedEntity

        Me.CreateWorldEveryFrame = True
        Me.DropUpdateUnlessDrawn = False
    End Sub

    Public Overrides Sub Update()
        If Started = False Then
            Me.startDelay = Date.Now + New TimeSpan(0, 0, 0, CInt(StartDelayWhole), CInt(StartDelayFraction * 1000))
            Me.endDelay = Date.Now + New TimeSpan(0, 0, 0, CInt(EndDelayWhole), CInt(EndDelayFraction * 1000))
            Started = True
        End If
        If CanRemove = False Then
            If Ready = True Then
                If Date.Now >= endDelay Then
                    CanRemove = True
                    DoRemoveEntity()
                End If
            Else
                If Date.Now >= startDelay Then
                    If SpawnedEntity = True Then
                        Ready = True
                    Else
                        Me.Visible = True
                    End If
                    DoActionActive()
                End If
            End If
        End If
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
        If Date.Now >= startDelay Then
            If CanRemove = False Then
                If Me.Model Is Nothing Then
                    Draw(Me.BaseModel, Me.Textures, True)
                Else
                    UpdateModel()
                    Draw(Me.BaseModel, Me.Textures, True, Me.Model)
                End If
            End If
        End If
    End Sub

End Class