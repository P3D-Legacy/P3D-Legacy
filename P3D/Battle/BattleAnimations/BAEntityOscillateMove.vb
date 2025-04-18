﻿Public Class BAEntityOscillateMove

	Inherits BattleAnimation3D

	Dim StartPosition As Vector3
	Dim ReturnToStart As Vector3
	Dim TargetEntity As Entity
	Dim HalfDistance As New Vector3(0.0F)
	Dim DestinationDistance As New Vector3(0.0F)
	Dim CurrentDistance As New Vector3(0.0F)
	Dim MoveSpeed As Single
	Dim MoveBothWays As Boolean = True
	Dim MovementCurve As Integer = 0
	Dim RemoveEntityAfter As Boolean
	Dim Duration As TimeSpan
	Dim ReadyTime As Date
	Dim ReadyAxis As New Vector3(0)
	Dim InterpolationSpeed As Vector3
	Dim InterpolationDirection As Boolean = True
	Public Enum Curves As Integer
		Linear
		Smooth
	End Enum

	Public Sub New(ByRef Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal Distance As Vector3, ByVal Speed As Single, ByVal BothWays As Boolean, ByVal Duration As TimeSpan, ByVal startDelay As Single, ByVal endDelay As Single, Optional MovementCurve As Integer = 0, Optional ReturnToStart As Vector3 = Nothing)
		MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
		Me.RemoveEntityAfter = RemoveEntityAfter
		Me.HalfDistance = Distance
		Me.DestinationDistance = Me.HalfDistance
		Me.MoveSpeed = Speed
		Me.MoveBothWays = BothWays
		Me.Duration = Duration
		Me.MovementCurve = CType(MovementCurve, Curves)

		Me.Visible = False
		Me.TargetEntity = Entity
		Select Case MovementCurve
			Case Curves.Linear
				InterpolationSpeed = New Vector3(MoveSpeed)
			Case Curves.Smooth
				InterpolationSpeed = New Vector3(0.0F)
		End Select
		If ReturnToStart <> Nothing Then
			Me.ReturnToStart = ReturnToStart
		End If
		Me.AnimationType = AnimationTypes.Move
	End Sub

	Public Overrides Sub DoActionActive()
		Move()
	End Sub

	Private Sub Move()
		If StartPosition = Nothing Then
			StartPosition = TargetEntity.Position
		End If
		If ReadyTime = Nothing Then
			ReadyTime = Date.Now + Duration
		End If
		If MovementCurve = Curves.Smooth Then
			If InterpolationDirection = True Then
				If InterpolationSpeed.X < MoveSpeed Then
					InterpolationSpeed.X += MoveSpeed / 10
				Else
					InterpolationSpeed.X = MoveSpeed
				End If
				If InterpolationSpeed.Y < MoveSpeed Then
					InterpolationSpeed.Y += MoveSpeed / 10
				Else
					InterpolationSpeed.Y = MoveSpeed
				End If
				If InterpolationSpeed.Z < MoveSpeed Then
					InterpolationSpeed.Z += MoveSpeed / 10
				Else
					InterpolationSpeed.Z = MoveSpeed
				End If
			Else
				If Date.Now < ReadyTime Then
					If InterpolationSpeed.X > 0 Then
						InterpolationSpeed.X -= MoveSpeed / 10
					Else
						InterpolationSpeed.X = 0
					End If
					If InterpolationSpeed.Y > 0 Then
						InterpolationSpeed.Y -= MoveSpeed / 10
					Else
						InterpolationSpeed.Y = 0
					End If
					If InterpolationSpeed.Z > 0 Then
						InterpolationSpeed.Z -= MoveSpeed / 10
					Else
						InterpolationSpeed.Z = 0
					End If
				Else
					If InterpolationSpeed.X > MoveSpeed / 10 * 3 Then
						InterpolationSpeed.X -= MoveSpeed / 10
					Else
						InterpolationSpeed.X = MoveSpeed / 10 * 3
					End If
					If InterpolationSpeed.Y > MoveSpeed / 10 * 3 Then
						InterpolationSpeed.Y -= MoveSpeed / 10
					Else
						InterpolationSpeed.Y = MoveSpeed / 10 * 3
					End If
					If InterpolationSpeed.Z > MoveSpeed / 10 * 3 Then
						InterpolationSpeed.Z -= MoveSpeed / 10
					Else
						InterpolationSpeed.Z = MoveSpeed / 10 * 3
					End If
				End If
			End If
		End If

		If CurrentDistance.X <> DestinationDistance.X Then
			If CurrentDistance.X < DestinationDistance.X Then
				CurrentDistance.X += InterpolationSpeed.X
			Else
				CurrentDistance.X -= InterpolationSpeed.X
			End If

			If Math.Abs(CurrentDistance.X) / Math.Abs(HalfDistance.X) > 0.75F Then
				InterpolationDirection = False
			End If

			If DestinationDistance.X > 0.0F Then
				If CurrentDistance.X >= DestinationDistance.X Then
					CurrentDistance.X = DestinationDistance.X
				End If
			ElseIf DestinationDistance.x < 0.0F Then
				If CurrentDistance.X <= DestinationDistance.X Then
					CurrentDistance.X = DestinationDistance.X
				End If
			Else
				If CurrentDistance.X > DestinationDistance.X Then
					If CurrentDistance.X - InterpolationSpeed.X <= DestinationDistance.X Then
						CurrentDistance.X = DestinationDistance.X
					End If
				Else
					If CurrentDistance.X + InterpolationSpeed.X >= DestinationDistance.X Then
						CurrentDistance.X = DestinationDistance.X
					End If
				End If
			End If
		Else
			If Date.Now < ReadyTime Then
				If MoveBothWays = True Then
					If DestinationDistance.X > 0.0F Then
						DestinationDistance.X = 0.0F - Math.Abs(HalfDistance.X) * 2
					Else
						DestinationDistance.X = 0.0F + Math.Abs(HalfDistance.X) * 2
					End If
				Else
					If DestinationDistance.X > 0.0F Then
						DestinationDistance.X = 0.0F
					Else
						DestinationDistance.X = HalfDistance.X
					End If
				End If
				InterpolationDirection = True
			Else
				If ReturnToStart.X = 0.0F Then
					ReadyAxis.X = 1.0F
				Else
					If DestinationDistance.X <> 0.0F Then
						DestinationDistance.X = 0.0F
						InterpolationDirection = True
					End If
					If CurrentDistance.X = 0.0F Then
						ReadyAxis.X = 1.0F
					End If
				End If
			End If
		End If

		If CurrentDistance.Y <> DestinationDistance.Y Then
			If CurrentDistance.Y < DestinationDistance.Y Then
				CurrentDistance.Y += InterpolationSpeed.Y
			Else
				CurrentDistance.Y -= InterpolationSpeed.Y
			End If

			If Math.Abs(CurrentDistance.Y) / Math.Abs(HalfDistance.Y) > 0.75F Then
				InterpolationDirection = False
			End If

			If DestinationDistance.Y > 0.0F Then
				If CurrentDistance.Y >= DestinationDistance.Y Then
					CurrentDistance.Y = DestinationDistance.Y
				End If
			ElseIf DestinationDistance.Y < 0.0F Then
				If CurrentDistance.Y <= DestinationDistance.Y Then
					CurrentDistance.Y = DestinationDistance.Y
				End If
			Else
				If CurrentDistance.Y > DestinationDistance.Y Then
					If CurrentDistance.Y - InterpolationSpeed.Y <= DestinationDistance.Y Then
						CurrentDistance.Y = DestinationDistance.Y
					End If
				Else
					If CurrentDistance.Y + InterpolationSpeed.Y >= DestinationDistance.Y Then
						CurrentDistance.Y = DestinationDistance.Y
					End If
				End If
			End If
		Else
			If Date.Now < ReadyTime Then
				If MoveBothWays = True Then
					If DestinationDistance.Y > 0.0F Then
						DestinationDistance.Y = 0.0F - Math.Abs(HalfDistance.Y) * 2
					Else
						DestinationDistance.Y = 0.0F + Math.Abs(HalfDistance.Y) * 2
					End If
				Else
					If DestinationDistance.Y > 0.0F Then
						DestinationDistance.Y = 0.0F
					Else
						DestinationDistance.Y = HalfDistance.Y
					End If
				End If
				InterpolationDirection = True
			Else
				If ReturnToStart.Y = 0.0F Then
					ReadyAxis.Y = 1.0F
				Else
					If DestinationDistance.Y <> 0.0F Then
						DestinationDistance.Y = 0.0F
						InterpolationDirection = True
					End If
					If CurrentDistance.Y = 0.0F Then
						ReadyAxis.Y = 1.0F
					End If
				End If
			End If
		End If

		If CurrentDistance.Z <> DestinationDistance.Z Then
			If CurrentDistance.Z < DestinationDistance.Z Then
				CurrentDistance.Z += InterpolationSpeed.Z
			Else
				CurrentDistance.Z -= InterpolationSpeed.Z
			End If

			If Math.Abs(CurrentDistance.Y) / Math.Abs(HalfDistance.Y) > 0.75F Then
				InterpolationDirection = False
			End If

			If DestinationDistance.Z > 0.0F Then
				If CurrentDistance.Z >= DestinationDistance.Z Then
					CurrentDistance.Z = DestinationDistance.Z
				End If
			ElseIf DestinationDistance.z < 0.0F Then
				If CurrentDistance.Z <= DestinationDistance.Z Then
					CurrentDistance.Z = DestinationDistance.Z
				End If
			Else
				If CurrentDistance.Z > DestinationDistance.Z Then
					If CurrentDistance.Z - InterpolationSpeed.Z <= DestinationDistance.Z Then
						CurrentDistance.Z = DestinationDistance.Z
					End If
				Else
					If CurrentDistance.Z + InterpolationSpeed.Z >= DestinationDistance.Z Then
						CurrentDistance.Z = DestinationDistance.Z
					End If
				End If
			End If
		Else
			If Date.Now < ReadyTime Then
				If MoveBothWays = True Then
					If DestinationDistance.Z > 0.0F Then
						DestinationDistance.Z = 0.0F - Math.Abs(HalfDistance.Z) * 2
					Else
						DestinationDistance.Z = 0.0F + Math.Abs(HalfDistance.Z) * 2
					End If
				Else
					If DestinationDistance.Z > 0.0F Then
						DestinationDistance.Z = 0.0F
					Else
						DestinationDistance.Z = HalfDistance.Z
					End If
				End If
				InterpolationDirection = True
			Else
				If ReturnToStart.Z = 0.0F Then
					ReadyAxis.Z = 1.0F
				Else
					If DestinationDistance.Z <> 0.0F Then
						DestinationDistance.Z = 0.0F
						InterpolationDirection = True
					End If
					If CurrentDistance.Z = 0.0F Then
						ReadyAxis.Z = 1.0F
					End If
				End If
			End If
		End If

		If HalfDistance.X <> 0.0F Then
			TargetEntity.Position.X = StartPosition.X + Me.CurrentDistance.X
		Else
			ReadyAxis.X = 1.0F
		End If
		If HalfDistance.Y <> 0.0F Then
			TargetEntity.Position.Y = StartPosition.Y + Me.CurrentDistance.Y
		Else
			ReadyAxis.Y = 1.0F
		End If
		If HalfDistance.Z <> 0.0F Then
			TargetEntity.Position.Z = StartPosition.Z + Me.CurrentDistance.Z
		Else
			ReadyAxis.Z = 1.0F
		End If

		If Date.Now > ReadyTime AndAlso ReadyAxis.X = 1.0F AndAlso ReadyAxis.Y = 1.0F AndAlso ReadyAxis.Z = 1.0F Then
			Me.Ready = True
		End If
	End Sub
	Public Overrides Sub DoRemoveEntity()
		If Me.RemoveEntityAfter = True Then
			TargetEntity.CanBeRemoved = True
		End If
	End Sub

End Class