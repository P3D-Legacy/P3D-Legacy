Public Class BAEntityFaceRotate

    Inherits BattleAnimation3D

    Dim TargetEntity As NPC
    Dim EndFaceRotation As Integer
    Dim TurnSteps As Integer = 0
    Dim TurnSpeed As Integer = 1
    Dim TurnTime As Single = 0.0F
    Dim TurnDelay As Single = 0.0F

    Public Sub New(ByRef TargetEntity As NPC, ByVal TurnSteps As Integer, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal EndFaceRotation As Integer = -1, Optional ByVal TurnSpeed As Integer = 1, Optional ByVal TurnDelay As Single = 0.25F)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
        If EndFaceRotation = -1 Then
            Me.EndFaceRotation = TargetEntity.faceRotation
        Else
            Me.EndFaceRotation = EndFaceRotation
        End If
        Me.TurnSteps = TurnSteps
        Me.TargetEntity = TargetEntity
        Me.TurnSpeed = TurnSpeed
        Me.TurnDelay = TurnDelay

        Me.AnimationType = AnimationTypes.Rotation
    End Sub

    Public Overrides Sub DoActionActive()
        If Me.TurnSteps > 0 Then
            If Me.TurnTime <= 0.0F Then
                Me.TargetEntity.faceRotation += Me.TurnSpeed
                If Me.TargetEntity.faceRotation > 3 Then
                    Me.TargetEntity.faceRotation -= 4
                End If
                If Me.TargetEntity.faceRotation < 0 Then
                    Me.TargetEntity.faceRotation += 4
                End If
                Me.TurnSteps -= TurnSpeed.ToPositive()
                Me.TurnTime = TurnDelay
            Else
                TurnTime -= 0.1F
            End If
        Else
            If Me.TargetEntity.faceRotation <> Me.EndFaceRotation Then
                Me.TargetEntity.faceRotation = Me.EndFaceRotation
            End If
            Me.Ready = True
        End If
    End Sub

End Class