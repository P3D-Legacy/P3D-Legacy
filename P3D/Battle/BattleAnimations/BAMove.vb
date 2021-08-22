Public Class BAMove

    Inherits BattleAnimation3D

    Public Destination As Vector3
    Public MoveSpeed As Single
    Public SpinX As Boolean = False
    Public SpinZ As Boolean = False

    Public InterpolationSpeed As Single
    Public SpinSpeedX As Single = 0.1F
    Public SpinSpeedZ As Single = 0.1F
	Public MovementCurve As Integer = 2

	Private EasedIn As Boolean = False
	Private EasedOut As Boolean = False
	Public Enum Curves As Integer
        EaseIn
        EaseOut
		EaseInAndOut
		NoEase
		Linear
	End Enum
    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal Destination As Vector3, ByVal Speed As Single, ByVal SpinX As Boolean, ByVal SpinZ As Boolean, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal SpinXSpeed As Single = 0.1F, Optional ByVal SpinZSpeed As Single = 0.1F, Optional MovementCurve As Integer = 2)
        MyBase.New(Position, Texture, Scale, startDelay, endDelay)

        Me.Position = Position
        Me.Destination = Destination
        Me.MovementCurve = CType(MovementCurve, Curves)

        Me.MoveSpeed = Speed
        Me.Scale = Scale
        Me.SpinSpeedX = SpinXSpeed
        Me.SpinSpeedZ = SpinZSpeed

        Me.SpinX = SpinX
        Me.SpinZ = SpinZ

        Select Case MovementCurve
            Case Curves.EaseIn
                InterpolationSpeed = 0.0F
            Case Curves.EaseOut
                InterpolationSpeed = MoveSpeed
            Case Curves.EaseInAndOut
				InterpolationSpeed = 0.0F
			Case Curves.NoEase
				InterpolationSpeed = MoveSpeed
		End Select

        Me.AnimationType = AnimationTypes.Move
    End Sub

    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal Destination As Vector3, ByVal Speed As Single, ByVal startDelay As Single, ByVal endDelay As Single)
		Me.New(Position, Texture, Scale, Destination, Speed, False, False, startDelay, endDelay, 0.1, 0.1, 2)
	End Sub

    Public Overrides Sub DoActionUpdate()
        Spin()
    End Sub

    Public Overrides Sub DoActionActive()
        Move()
    End Sub

    Private Sub Spin()
        If Me.SpinX = True Then
            Me.Rotation.X += SpinSpeedX
        End If
        If Me.SpinZ = True Then
            Me.Rotation.Z += SpinSpeedZ
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
			If Me.Position.X < Me.Destination.X Then
				Me.Position.X += Me.MoveSpeed

				If Me.Position.X >= Me.Destination.X Then
					Me.Position.X = Me.Destination.X
				End If
			ElseIf Me.Position.X > Me.Destination.X Then
				Me.Position.X -= Me.MoveSpeed

				If Me.Position.X <= Me.Destination.X Then
					Me.Position.X = Me.Destination.X
				End If
			End If
			If Me.Position.Y < Me.Destination.Y Then
				Me.Position.Y += Me.MoveSpeed

				If Me.Position.Y >= Me.Destination.Y Then
					Me.Position.Y = Me.Destination.Y
				End If
			ElseIf Me.Position.Y > Me.Destination.Y Then
				Me.Position.Y -= Me.MoveSpeed

				If Me.Position.Y <= Me.Destination.Y Then
					Me.Position.Y = Me.Destination.Y
				End If
			End If
			If Me.Position.Z < Me.Destination.Z Then
				Me.Position.Z += Me.MoveSpeed

				If Me.Position.Z >= Me.Destination.Z Then
					Me.Position.Z = Me.Destination.Z
				End If
			ElseIf Me.Position.Z > Me.Destination.Z Then
				Me.Position.Z -= Me.MoveSpeed

				If Me.Position.Z <= Me.Destination.Z Then
					Me.Position.Z = Me.Destination.Z
				End If
			End If
		Else
			If Me.Position.X < Me.Destination.X Then
				Me.Position.X = MathHelper.Lerp(Me.Position.X, Me.Destination.X, Me.InterpolationSpeed)
				If Me.Position.X > Me.Destination.X - 0.05 Then
					Me.Position.X = Me.Destination.X
				End If
			ElseIf Me.Position.X > Me.Destination.X Then
				Me.Position.X = MathHelper.Lerp(Me.Position.X, Me.Destination.X, Me.InterpolationSpeed)
				If Me.Position.X < Me.Destination.X + 0.05 Then
					Me.Position.X = Me.Destination.X
				End If
			End If
			If Me.Position.Y < Me.Destination.Y Then
				Me.Position.Y = MathHelper.Lerp(Me.Position.Y, Me.Destination.Y, Me.InterpolationSpeed)
				If Me.Position.Y > Me.Destination.Y - 0.05 Then
					Me.Position.Y = Me.Destination.Y
				End If
			ElseIf Me.Position.Y > Me.Destination.Y Then
				Me.Position.Y = MathHelper.Lerp(Me.Position.Y, Me.Destination.Y, Me.InterpolationSpeed)
				If Me.Position.Y < Me.Destination.Y + 0.05 Then
					Me.Position.Y = Me.Destination.Y
				End If
			End If
			If Me.Position.Z < Me.Destination.Z Then
				Me.Position.Z = MathHelper.Lerp(Me.Position.Z, Me.Destination.Z, Me.InterpolationSpeed)
				If Me.Position.Z > Me.Destination.Z - 0.05 Then
					Me.Position.Z = Me.Destination.Z
				End If
			ElseIf Me.Position.Z > Me.Destination.Z Then
				Me.Position.Z = MathHelper.Lerp(Me.Position.Z, Me.Destination.Z, Me.InterpolationSpeed)
				If Me.Position.Z < Me.Destination.Z + 0.05 Then
					Me.Position.Z = Me.Destination.Z
				End If
			End If
		End If

		If Me.Position = Destination Then
			Me.Ready = True
		End If

	End Sub

End Class