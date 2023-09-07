Imports P3D.Screen

Public Class BACameraChangeAngle

	Inherits BattleAnimation3D

	Public CameraAngleID As Integer
	Public BV2Screen As BattleSystem.BattleScreen

	Public Sub New(ByRef Battlescreen As BattleSystem.BattleScreen, ByVal CameraAngleID As Integer, ByVal startDelay As Single, ByVal endDelay As Single)
		MyBase.New(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay)
		Me.BV2Screen = Battlescreen
		Me.CameraAngleID = CameraAngleID

		Me.Visible = False

		Me.AnimationType = AnimationTypes.Camera
	End Sub

	Public Overrides Sub DoActionActive()
		Select Case CameraAngleID
			Case 0
				Me.BV2Screen.Battle.ChangeCameraAngle(0, True, Me.BV2Screen)
			Case 1
				Me.BV2Screen.Battle.ChangeCameraAngle(1, True, Me.BV2Screen)
			Case 2
				Me.BV2Screen.Battle.ChangeCameraAngle(2, True, Me.BV2Screen)
		End Select
		Me.Ready = True

	End Sub


End Class