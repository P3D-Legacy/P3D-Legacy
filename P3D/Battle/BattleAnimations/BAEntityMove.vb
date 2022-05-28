Public Class BAEntityMove

	Inherits BattleAnimation3D

	Public TargetEntity As Entity
	Public Destination As Vector3
	Public MoveSpeed As Single
	Public MoveYSpeed As Single
	Public InterpolationSpeed As Single
	Public SpinX As Boolean = False
	Public SpinZ As Boolean = False
	Public SpinSpeedX As Single = 0.1F
	Public SpinSpeedZ As Single = 0.1F
	Public MovementCurve As Integer = 3
	Private EasedIn As Boolean = False
	Private EasedOut As Boolean = False
	Public RemoveEntityAfter As Boolean
	Public Enum Curves As Integer
		EaseIn
		EaseOut
		EaseInAndOut
		Linear
	End Enum

	Public Sub New(ByRef Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal Destination As Vector3, ByVal Speed As Single, ByVal SpinX As Boolean, ByVal SpinZ As Boolean, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal SpinXSpeed As Single = 0.1F, Optional ByVal SpinZSpeed As Single = 0.1F, Optional MovementCurve As Integer = 3, Optional MoveYSpeed As Single = 0.0F)
		MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)

		Me.RemoveEntityAfter = RemoveEntityAfter
		Me.Destination = Destination
		Me.MoveSpeed = Speed
		If MoveYSpeed = 0F Then
			Me.MoveYSpeed = MoveSpeed
		Else
			Me.MoveYSpeed = MoveYSpeed
		End If
		Me.MovementCurve = CType(MovementCurve, Curves)

		Me.SpinX = SpinX
		Me.SpinZ = SpinZ
		Me.SpinSpeedX = SpinXSpeed
		Me.SpinSpeedZ = SpinZSpeed

		Me.Visible = False
		Me.TargetEntity = Entity

		Select Case MovementCurve
			Case Curves.EaseIn
				InterpolationSpeed = 0.0F
			Case Curves.EaseOut
				InterpolationSpeed = MoveSpeed
			Case Curves.EaseInAndOut
				InterpolationSpeed = 0.0F
			Case Curves.Linear
				InterpolationSpeed = MoveSpeed
		End Select

		Me.AnimationType = AnimationTypes.Move
	End Sub

	Public Overrides Sub DoActionUpdate()
		Spin()
	End Sub

	Public Overrides Sub DoActionActive()
		Move()
	End Sub

	Private Sub Spin()
		If Me.SpinX = True Then
			TargetEntity.Rotation.X += SpinSpeedX
		End If
		If Me.SpinZ = True Then
			TargetEntity.Rotation.Z += SpinSpeedZ
		End If
	End Sub

	Private Sub Move()
		Select Case MovementCurve
			Case Curves.EaseIn
				If EasedIn = False Then
					If InterpolationSpeed < MoveSpeed Then
						InterpolationSpeed += MoveSpeed / 10
					Else
						EasedIn = True
						InterpolationSpeed = MoveSpeed
					End If
				End If
			Case Curves.EaseOut
				If EasedOut = False Then
					If InterpolationSpeed > 0 Then
						InterpolationSpeed -= MoveSpeed / 10
					Else
						EasedOut = True
						InterpolationSpeed = 0
					End If
				End If
			Case Curves.EaseInAndOut
				If EasedIn = False Then
					If InterpolationSpeed < MoveSpeed Then
						InterpolationSpeed += MoveSpeed / 10
					Else
						EasedIn = True
						InterpolationSpeed = MoveSpeed
					End If
				Else
					If EasedOut = False Then
						If InterpolationSpeed > 0 Then
							InterpolationSpeed -= MoveSpeed / 10
						Else
							EasedOut = True
							InterpolationSpeed = 0
						End If
					End If
				End If
		End Select

		If MovementCurve = Curves.Linear Then
			If TargetEntity.Position.X < Me.Destination.X Then
				TargetEntity.Position.X += Me.MoveSpeed

				If TargetEntity.Position.X >= Me.Destination.X Then
					TargetEntity.Position.X = Me.Destination.X
				End If
			ElseIf TargetEntity.Position.X > Me.Destination.X Then
				TargetEntity.Position.X -= Me.MoveSpeed

				If TargetEntity.Position.X <= Me.Destination.X Then
					TargetEntity.Position.X = Me.Destination.X
				End If
			End If
			If TargetEntity.Position.Y < Me.Destination.Y Then
				TargetEntity.Position.Y += Me.MoveYSpeed

				If TargetEntity.Position.Y >= Me.Destination.Y Then
					TargetEntity.Position.Y = Me.Destination.Y
				End If
			ElseIf TargetEntity.Position.Y > Me.Destination.Y Then
				TargetEntity.Position.Y -= Me.MoveYSpeed

				If TargetEntity.Position.Y <= Me.Destination.Y Then
					TargetEntity.Position.Y = Me.Destination.Y
				End If
			End If
			If TargetEntity.Position.Z < Me.Destination.Z Then
				TargetEntity.Position.Z += Me.MoveSpeed

				If TargetEntity.Position.Z >= Me.Destination.Z Then
					TargetEntity.Position.Z = Me.Destination.Z
				End If
			ElseIf TargetEntity.Position.Z > Me.Destination.Z Then
				TargetEntity.Position.Z -= Me.MoveSpeed

				If TargetEntity.Position.Z <= Me.Destination.Z Then
					TargetEntity.Position.Z = Me.Destination.Z
				End If
			End If
		Else
			If TargetEntity.Position.X < Me.Destination.X Then
				TargetEntity.Position.X = MathHelper.Lerp(TargetEntity.Position.X, Me.Destination.X, Me.InterpolationSpeed)
				If TargetEntity.Position.X > Me.Destination.X - 0.05 Then
					TargetEntity.Position.X = Me.Destination.X
				End If
			ElseIf TargetEntity.Position.X > Me.Destination.X Then
				TargetEntity.Position.X = MathHelper.Lerp(TargetEntity.Position.X, Me.Destination.X, Me.InterpolationSpeed)
				If TargetEntity.Position.X < Me.Destination.X + 0.05 Then
					TargetEntity.Position.X = Me.Destination.X
				End If
			End If
			If TargetEntity.Position.Y < Me.Destination.Y Then
				TargetEntity.Position.Y = MathHelper.Lerp(TargetEntity.Position.Y, Me.Destination.Y, Me.InterpolationSpeed)
				If TargetEntity.Position.Y > Me.Destination.Y - 0.05 Then
					TargetEntity.Position.Y = Me.Destination.Y
				End If
			ElseIf TargetEntity.Position.Y > Me.Destination.Y Then
				TargetEntity.Position.Y = MathHelper.Lerp(TargetEntity.Position.Y, Me.Destination.Y, Me.InterpolationSpeed)
				If TargetEntity.Position.Y < Me.Destination.Y + 0.05 Then
					TargetEntity.Position.Y = Me.Destination.Y
				End If
			End If
			If TargetEntity.Position.Z < Me.Destination.Z Then
				TargetEntity.Position.Z = MathHelper.Lerp(TargetEntity.Position.Z, Me.Destination.Z, Me.InterpolationSpeed)
				If TargetEntity.Position.Z > Me.Destination.Z - 0.05 Then
					TargetEntity.Position.Z = Me.Destination.Z
				End If
			ElseIf TargetEntity.Position.Z > Me.Destination.Z Then
				TargetEntity.Position.Z = MathHelper.Lerp(TargetEntity.Position.Z, Me.Destination.Z, Me.InterpolationSpeed)
				If TargetEntity.Position.Z < Me.Destination.Z + 0.05 Then
					TargetEntity.Position.Z = Me.Destination.Z
				End If
			End If
		End If
		If TargetEntity.Position = Destination Then
			Me.Ready = True
		End If
	End Sub
	Public Overrides Sub DoRemoveEntity()
		If Me.RemoveEntityAfter = True Then
			TargetEntity.CanBeRemoved = True
		End If
	End Sub

End Class