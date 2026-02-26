Public Class BAEntityOpacity

    Inherits BattleAnimation3D

    Dim TargetEntity As Entity
    Dim TransitionSpeed As Single = 0.01F
    Dim EndState As Single = 0.0F
    Dim StartState As Single = 1.0F
    Dim RemoveEntityAfter As Boolean = False
    Dim InitialOpacitySet As Boolean = False

    Public Sub New(ByRef entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal TransitionSpeed As Single, ByVal EndState As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal StartState As Single = 1.0F)
        MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
        Me.RemoveEntityAfter = RemoveEntityAfter
        Me.EndState = EndState
        Me.TransitionSpeed = TransitionSpeed
        Me.TargetEntity = entity
        Me.StartState = StartState

        Me.Visible = False

        Me.AnimationType = AnimationTypes.Transition
    End Sub

    Public Overrides Sub DoActionActive()
        If InitialOpacitySet = False Then
            Me.TargetEntity.NormalOpacity = Me.StartState
            InitialOpacitySet = True
        End If

        If Me.EndState > Me.StartState Then
            If Me.EndState > TargetEntity.NormalOpacity Then
                TargetEntity.NormalOpacity += Me.TransitionSpeed
                If TargetEntity.NormalOpacity >= Me.EndState Then
                    TargetEntity.NormalOpacity = Me.EndState
                End If
            End If
        ElseIf Me.EndState < Me.StartState Then
            If Me.EndState < TargetEntity.NormalOpacity Then
                TargetEntity.NormalOpacity -= Me.TransitionSpeed
                If TargetEntity.NormalOpacity <= Me.EndState Then
                    TargetEntity.NormalOpacity = Me.EndState
                End If
            End If
        Else
            TargetEntity.NormalOpacity = Me.EndState
        End If

        If TargetEntity.NormalOpacity = Me.EndState Then
            Me.Ready = True
        End If
    End Sub
    Public Overrides Sub DoRemoveEntity()
        If Me.RemoveEntityAfter = True Then
            TargetEntity.CanBeRemoved = True
        End If
    End Sub
End Class