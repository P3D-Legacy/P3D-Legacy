Public Class BAEntityFaceRotate

    Inherits BattleAnimation3D

    Dim TargetEntity As NPC
    Dim EndFaceRotation As Integer
    Dim TurnSteps As Integer = 0
    Dim TurnSpeed As Integer = 1
    Dim TurnTime As Date = Date.Now
    Dim DelayDivide As Single = 6.0F
    Dim TurnDelayWhole As Single = 0.0F
    Dim TurnDelayFraction As Single = 0.0F
    Dim InitialRotationSet As Boolean = False

    Public Sub New(ByRef TargetEntity As NPC, ByVal TurnSteps As Integer, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal EndFaceRotation As Integer = -1, Optional ByVal TurnDelay As Single = 0.25F, Optional ByVal TurnSpeed As Integer = 1)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)

        Me.TurnSteps = TurnSteps.ToPositive
        Me.TargetEntity = TargetEntity
        Me.TurnSpeed = TurnSpeed
        Me.EndFaceRotation = EndFaceRotation

        TurnDelayWhole = CSng(Math.Truncate(CDbl(TurnDelay / DelayDivide)))
        TurnDelayFraction = TurnDelay / DelayDivide - TurnDelayWhole

        Me.AnimationType = AnimationTypes.Rotation
    End Sub

    Public Overrides Sub DoActionActive()
        If InitialRotationSet = False Then
            If EndFaceRotation = -1 Then
                Me.EndFaceRotation = TargetEntity.faceRotation
            End If
            InitialRotationSet = True
        End If

        If Me.TurnSteps > 0 Then
            If Date.Now >= Me.TurnTime Then
                Me.TargetEntity.faceRotation += Me.TurnSpeed
                If Me.TargetEntity.faceRotation > 3 Then
                    Me.TargetEntity.faceRotation -= 4
                End If
                If Me.TargetEntity.faceRotation < 0 Then
                    Me.TargetEntity.faceRotation += 4
                End If
                Me.TurnSteps -= TurnSpeed.ToPositive()
                Me.TurnTime = Date.Now + New TimeSpan(0, 0, 0, CInt(TurnDelayWhole), CInt(TurnDelayFraction * 1000))
            End If
        Else
            If Me.TargetEntity.faceRotation <> Me.EndFaceRotation Then
                Me.TargetEntity.faceRotation = Me.EndFaceRotation
            End If
            Me.Ready = True
        End If
    End Sub

End Class