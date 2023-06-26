Public Class BAEntityMove

	Inherits BattleAnimation3D

	Public StartPosition As Vector3
	Public TargetEntity As Entity
	Public Destination As Vector3
	Public MoveDistance As New Vector3(0.0F)
	Public MoveSpeed As Single
	Public MoveYSpeed As Single
	Public InterpolationSpeed As Single
	Public InterpolationYSpeed As Single
	Public SpinX As Boolean = False
	Public SpinZ As Boolean = False
	Public SpinSpeedX As Single = 0.1F
	Public SpinSpeedZ As Single = 0.1F
	Public MovementCurve As Integer = 3
	Private EasedIn As Boolean = False
	Private EasedOut As Boolean = False
	Public RemoveEntityAfter As Boolean
	Dim ReadyAxis As Vector3 = New Vector3(0.0F)
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

		Me.StartPosition = TargetEntity.Position
		Me.MoveDistance.X = Math.Abs(Me.StartPosition.X - Me.Destination.X)

		If TargetEntity.Model IsNot Nothing Then
			Me.MoveDistance.Y = Math.Abs(Me.StartPosition.Y - Me.Destination.Y - 0.5F)
		Else
			Me.MoveDistance.Y = Math.Abs(Me.StartPosition.Y - Me.Destination.Y)
		End If
		Me.MoveDistance.Z = Math.Abs(Me.StartPosition.Z - Me.Destination.Z)

		Select Case MovementCurve
			Case Curves.EaseIn
				InterpolationSpeed = 0.0F
				InterpolationYSpeed = 0.0F
			Case Curves.EaseOut
				InterpolationSpeed = MoveSpeed
				InterpolationYSpeed = MoveYSpeed
			Case Curves.EaseInAndOut
				InterpolationSpeed = 0.0F
				InterpolationYSpeed = 0.0F
			Case Curves.Linear
				InterpolationSpeed = MoveSpeed
				InterpolationYSpeed = MoveYSpeed
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

		Dim DestinationOffset As Vector3 = New Vector3(0)
		If TargetEntity.Model IsNot Nothing Then
			DestinationOffset = New Vector3(0, -0.5, 0)
		End If

		Select Case MovementCurve
			Case Curves.EaseIn
				If EasedIn = False Then
					If InterpolationSpeed < MoveSpeed - 0.05F OrElse InterpolationYSpeed < MoveYSpeed - 0.05F Then
						If InterpolationSpeed < MoveSpeed - 0.05F Then
							InterpolationSpeed = MathHelper.Lerp(InterpolationSpeed, MoveSpeed, 0.9F)
						End If
						If InterpolationYSpeed < MoveYSpeed - 0.05F Then
							InterpolationYSpeed = MathHelper.Lerp(InterpolationYSpeed, MoveYSpeed, 0.9F)
						End If
					Else
						InterpolationSpeed = MoveSpeed
						InterpolationYSpeed = MoveYSpeed
						EasedIn = True
					End If
				End If
			Case Curves.EaseOut
				If EasedOut = False Then
					If InterpolationSpeed > 0.05F OrElse InterpolationYSpeed > 0.05F Then
						If InterpolationSpeed > 0.05F Then
							InterpolationSpeed = MathHelper.Lerp(InterpolationSpeed, 0.0F, 0.9F)
						End If
						If InterpolationYSpeed > 0.05F Then
							InterpolationYSpeed = MathHelper.Lerp(InterpolationYSpeed, 0.0F, 0.9F)
						End If
					Else
						InterpolationYSpeed = 0
						InterpolationSpeed = 0
						EasedOut = True
					End If
				End If
			Case Curves.EaseInAndOut
				If EasedIn = False Then
					If InterpolationSpeed < MoveSpeed - 0.05F OrElse InterpolationYSpeed < MoveYSpeed - 0.05F Then
						If InterpolationSpeed < MoveSpeed - 0.05F Then
							InterpolationSpeed = MathHelper.Lerp(InterpolationSpeed, MoveSpeed, 0.9F)
						End If
						If InterpolationYSpeed < MoveYSpeed - 0.05F Then
							InterpolationYSpeed = MathHelper.Lerp(InterpolationYSpeed, MoveYSpeed, 0.9F)
						End If
					Else
						InterpolationSpeed = MoveSpeed
						InterpolationYSpeed = MoveYSpeed
						EasedIn = True
					End If
				Else
					If EasedOut = False Then
						If InterpolationSpeed > 0.05F OrElse InterpolationYSpeed > 0.05F Then
							If InterpolationSpeed > 0.05F Then
								InterpolationSpeed = MathHelper.Lerp(InterpolationSpeed, 0.0F, 0.9F)
							End If
							If InterpolationYSpeed > 0.05F Then
								InterpolationYSpeed = MathHelper.Lerp(InterpolationYSpeed, 0.0F, 0.9F)
							End If
						Else
							InterpolationYSpeed = 0
							InterpolationSpeed = 0
							EasedOut = True
						End If
					End If
				End If
		End Select

		If MoveDistance.X > 0.05F Then
			If StartPosition.X < Me.Destination.X Then
				TargetEntity.Position.X += Me.InterpolationSpeed

				If TargetEntity.Position.X >= Me.Destination.X + 0.05 Then
					TargetEntity.Position.X = Me.Destination.X
				End If
			ElseIf StartPosition.X > Me.Destination.X Then
				TargetEntity.Position.X -= Me.InterpolationSpeed

				If TargetEntity.Position.X <= Me.Destination.X + 0.05 Then
					TargetEntity.Position.X = Me.Destination.X
				End If
			End If
			MoveDistance.X -= Me.InterpolationSpeed
		Else
			ReadyAxis.X = 1.0F
		End If

		If MoveDistance.Y > 0.05F Then
			If StartPosition.Y < Me.Destination.Y + DestinationOffset.Y Then
				TargetEntity.Position.Y += Me.MoveYSpeed

				If TargetEntity.Position.Y >= Me.Destination.Y + DestinationOffset.Y - 0.05 Then
					TargetEntity.Position.Y = Me.Destination.Y + DestinationOffset.Y
				End If
			ElseIf StartPosition.Y > Me.Destination.Y + DestinationOffset.Y Then
				TargetEntity.Position.Y -= Me.MoveYSpeed

				If TargetEntity.Position.Y <= Me.Destination.Y + DestinationOffset.Y + 0.05 Then
					TargetEntity.Position.Y = Me.Destination.Y + DestinationOffset.Y
				End If
			End If
			MoveDistance.Y -= Me.MoveYSpeed
		Else
			ReadyAxis.Y = 1.0F
		End If

		If MoveDistance.Z > 0.05F Then
			If StartPosition.Z < Me.Destination.Z Then
				TargetEntity.Position.Z += Me.InterpolationSpeed

				If TargetEntity.Position.Z >= Me.Destination.Z - 0.05 Then
					TargetEntity.Position.Z = Me.Destination.Z
				End If
			ElseIf StartPosition.Z > Me.Destination.Z Then
				TargetEntity.Position.Z -= Me.InterpolationSpeed

				If TargetEntity.Position.Z <= Me.Destination.Z + 0.05 Then
					TargetEntity.Position.Z = Me.Destination.Z
				End If
			End If
			MoveDistance.Z -= Me.MoveYSpeed
		Else
			ReadyAxis.Z = 1.0F
		End If

		If ReadyAxis.X = 1.0F AndAlso ReadyAxis.Y = 1.0F AndAlso ReadyAxis.Z = 1.0F Then
			Me.Ready = True
		End If
	End Sub
	Public Overrides Sub DoRemoveEntity()
		If Me.RemoveEntityAfter = True Then
			TargetEntity.CanBeRemoved = True
		End If
	End Sub

End Class