Public Class BAEntityFaceRotate

    Inherits BattleAnimation3D

    Dim TargetEntity As NPC
    Dim TargetModel As Entity = Nothing
    Dim EndFaceRotation As Integer
    Dim TurnSteps As Integer = 0
    Dim TurnSpeed As Integer = 1
    Dim TurnTime As Single = 0.0F
    Dim TurnDelay As Single = 0.0F

    Public Sub New(ByVal TargetEntity As NPC, ByVal TurnSteps As Integer, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal EndFaceRotation As Integer = -1, Optional ByVal TurnSpeed As Integer = 1, Optional ByVal TurnDelay As Single = 0.25F, Optional TargetModel As Entity = Nothing)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
        If EndFaceRotation = -1 Then
            Me.EndFaceRotation = TargetEntity.faceRotation
        Else
            Me.EndFaceRotation = EndFaceRotation
        End If
        Me.TargetModel = TargetModel
        Me.TurnSteps = TurnSteps
        Me.TargetEntity = TargetEntity
        Me.TurnSpeed = TurnSpeed
        Me.TurnDelay = TurnDelay
    End Sub

    Public Overrides Sub DoActionActive()
        If Me.TurnSteps > 0 Then
            If Me.TurnTime <= 0.0F Then
                Me.TargetEntity.faceRotation += Me.TurnSpeed
                If Me.TargetEntity.faceRotation = 4 Then
                    Me.TargetEntity.faceRotation = 0
                End If
                If Me.TargetModel IsNot Nothing Then
                    Me.TargetModel.Rotation = Entity.GetRotationFromInteger(Me.TargetEntity.faceRotation)
                End If
                Me.TurnSteps -= TurnSpeed.ToPositive()
                Me.TurnTime = TurnDelay
            Else
                TurnDelay -= 0.1F
            End If
        Else
            If Me.TargetEntity.faceRotation <> Me.EndFaceRotation Then
                Me.TargetEntity.faceRotation = Me.EndFaceRotation
                If Me.TargetModel IsNot Nothing Then
                    Me.TargetModel.Rotation = Entity.GetRotationFromInteger(Me.EndFaceRotation)
                End If
            Else
                Me.Ready = True
            End If
        End If
    End Sub

End Class