Public Class BAEntitySetPosition

	Inherits BattleAnimation3D

	Public TargetEntity As Entity
	Public SetPosition As Vector3
	Public RemoveEntityAfter As Boolean

	Public Sub New(ByRef Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal SetPosition As Vector3, ByVal startDelay As Single, ByVal endDelay As Single)
		MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)

		Me.RemoveEntityAfter = RemoveEntityAfter
		Me.SetPosition = SetPosition

		Me.Visible = False
		Me.TargetEntity = Entity

		Me.AnimationType = AnimationTypes.Move
	End Sub

	Public Overrides Sub DoActionActive()
		Dim SetPositionOffset As Vector3 = New Vector3(0)
		If TargetEntity.Model IsNot Nothing Then
			SetPositionOffset = New Vector3(0, -0.5, 0)
		End If
		TargetEntity.Position = Me.SetPosition + SetPositionOffset
		Me.Ready = True
	End Sub

	Public Overrides Sub DoRemoveEntity()
		If Me.RemoveEntityAfter = True Then
			TargetEntity.CanBeRemoved = True
		End If
	End Sub

End Class