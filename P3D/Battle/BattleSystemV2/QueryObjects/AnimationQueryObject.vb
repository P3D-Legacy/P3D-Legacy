Imports System.Xml

Namespace BattleSystem

	Public Class AnimationQueryObject
		Inherits QueryObject

		Public AnimationStarted As Boolean = False
		Public AnimationEnded As Boolean = False
		Public BattleFlipped As Boolean = Nothing
		Public AnimationSequence As List(Of BattleAnimation3D)
		Public SpawnedEntities As List(Of Entity)
		Public CurrentEntity As Entity
		Public StartPosition As New Vector3(0)
		Public DrawBeforeEntities As Boolean

		Public Overrides ReadOnly Property IsReady As Boolean
			Get
				Return AnimationEnded
			End Get
		End Property

		Public Sub New(ByVal entity As Entity, ByVal BattleFlipped As Boolean, Optional DrawBeforeEntities As Boolean = False)
			MyBase.New(QueryTypes.MoveAnimation)
			Me.AnimationSequence = New List(Of BattleAnimation3D)
			Me.SpawnedEntities = New List(Of Entity)
			Me.DrawBeforeEntities = DrawBeforeEntities
			Me.BattleFlipped = BattleFlipped
			If entity IsNot Nothing Then
				Me.CurrentEntity = entity
				Me.StartPosition = entity.Position
			End If
			AnimationSequenceBegin()
		End Sub
		Public Overrides Sub Draw(ByVal BV2Screen As BattleScreen)
			Dim Backgrounds As New List(Of Entity)

			Dim RenderObjects As New List(Of Entity)
			For Each a As BattleAnimation3D In Me.AnimationSequence
				If a.AnimationType = BattleAnimation3D.AnimationTypes.Background Then
					Backgrounds.Add(a)
				End If
			Next
			For Each entity As BattleAnimation3D In Me.SpawnedEntities
				RenderObjects.Add(entity)
			Next
			If RenderObjects.Count > 0 Then
				RenderObjects = (From r In RenderObjects Order By r.CameraDistance Descending).ToList()
			End If
			For Each [Object] As Entity In Backgrounds
				[Object].Render()
			Next
			For Each [Object] As Entity In RenderObjects
				[Object].UpdateModel()
				[Object].Render()
			Next
		End Sub

		Public Overrides Sub Update(BV2Screen As BattleScreen)
			If AnimationStarted = True Then
				For i = 0 To AnimationSequence.Count - 1
					If i <= AnimationSequence.Count - 1 Then
						Dim a As BattleAnimation3D = AnimationSequence(i)
						If a.CanRemove = True Then
							i -= 1
							AnimationSequence.Remove(a)
						Else
							a.Update()
						End If
					End If
				Next
				If AnimationSequence.Count <= 0 Then
					AnimationSequenceEnd()
				End If

				For Each Animation As BattleAnimation3D In AnimationSequence
					Animation.UpdateEntity()
				Next
				For Each Entity As Entity In SpawnedEntities
					Entity.Update()
					Entity.UpdateEntity()
				Next
				For i = 0 To Me.SpawnedEntities.Count - 1
					If i <= SpawnedEntities.Count - 1 Then
						Dim entity As Entity = SpawnedEntities(i)

						If entity.CanBeRemoved = True Then
							i -= 1
							RemoveEntity(entity)
						End If
					End If
				Next
			End If
		End Sub

		Public Sub AnimationSequenceBegin()
			AnimationStarted = True
		End Sub

		Public Sub AnimationSequenceEnd()
			AnimationEnded = True
		End Sub

		Public Function SpawnEntity(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal Opacity As Single, Optional ByVal startDelay As Single = 0.0F, Optional ByVal endDelay As Single = 0.0F, Optional ModelPath As String = "") As Entity
			Dim NewPosition As Vector3
			If Not Position = Nothing Then
				If BattleFlipped = True Then
					Position.X *= -1
				End If
				If CurrentEntity IsNot Nothing Then
					NewPosition = CurrentEntity.Position + Position
				Else
					NewPosition = Position
				End If
			Else
				If CurrentEntity IsNot Nothing Then
					NewPosition = CurrentEntity.Position
				Else
					NewPosition = New Vector3(0, 0, 0)
				End If
			End If
			Dim SpawnedEntity = New BattleAnimation3D(NewPosition, Texture, Scale, startDelay, endDelay, False)
			SpawnedEntity.Opacity = Opacity
			SpawnedEntity.Visible = False

			If ModelPath <> "" Then
				SpawnedEntity.ModelPath = ModelPath
				SpawnedEntity.Model = ModelManager.GetModel(SpawnedEntity.ModelPath)
				SpawnedEntity.Scale *= ModelManager.MODELSCALE
				If BattleFlipped = True Then
					Dim FlipRotation As Integer = Entity.GetRotationFromVector(SpawnedEntity.Rotation)
					FlipRotation += 2
					If FlipRotation > 3 Then
						FlipRotation -= 4
					End If
					SpawnedEntity.Rotation.Y = Entity.GetRotationFromInteger(FlipRotation).Y
				End If
			End If
			SpawnedEntities.Add(SpawnedEntity)

			Return SpawnedEntity
		End Function
		Public Sub RemoveEntity(Entity As Entity)
			SpawnedEntities.Remove(Entity)
		End Sub
		Public Sub AnimationChangeTexture(ByVal Entity As Entity, RemoveEntityAfter As Boolean, ByVal Texture As Texture2D, ByVal startDelay As Single, ByVal endDelay As Single)
			Dim TextureChangeEntity As Entity

			If Entity Is Nothing Then
				TextureChangeEntity = CurrentEntity
			Else
				TextureChangeEntity = Entity
			End If

			Dim baEntityTextureChange As BAEntityTextureChange = New BAEntityTextureChange(TextureChangeEntity, RemoveEntityAfter, Texture, startDelay, endDelay)
			AnimationSequence.Add(baEntityTextureChange)

		End Sub

		Public Sub AnimationMove(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal DestinationX As Single, ByVal DestinationY As Single, ByVal DestinationZ As Single, ByVal Speed As Single, ByVal SpinX As Boolean, ByVal SpinZ As Boolean, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal SpinXSpeed As Single = 0.1F, Optional ByVal SpinZSpeed As Single = 0.1F, Optional MoveYSpeed As Single = 0.0F, Optional MovementCurve As Integer = 3)
			Dim MoveEntity As Entity
			Dim Destination As Vector3

			If Entity Is Nothing Then
				MoveEntity = CurrentEntity
			Else
				MoveEntity = Entity
			End If

			If Not BattleFlipped = Nothing Then
				If BattleFlipped = True Then
					DestinationX *= -1.0F

					If SpinZ = True Then
						SpinXSpeed *= -1.0F
						SpinZSpeed *= -1.0F
					End If
				End If
			End If

			If CurrentEntity Is Nothing Then
				Destination = MoveEntity.Position + New Vector3(DestinationX, DestinationY, DestinationZ)
			Else
				Destination = CurrentEntity.Position + New Vector3(DestinationX, DestinationY, DestinationZ)
			End If


			Dim baEntityMove As BAEntityMove = New BAEntityMove(MoveEntity, RemoveEntityAfter, Destination, Speed, SpinX, SpinZ, startDelay, endDelay, SpinXSpeed, SpinZSpeed, MovementCurve, MoveYSpeed)
			AnimationSequence.Add(baEntityMove)

		End Sub
		Public Sub AnimationOscillateMove(ByRef Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal Distance As Vector3, ByVal Speed As Single, ByVal BothWays As Boolean, ByVal Duration As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional MovementCurve As Integer = 0, Optional ReturnToStart As Vector3 = Nothing)
			Dim MoveEntity As Entity
			Dim ReturnPosition As New Vector3(0)

			If Entity Is Nothing Then
				MoveEntity = CurrentEntity
			Else
				MoveEntity = Entity
			End If

			If Not BattleFlipped = Nothing Then
				If BattleFlipped = True Then
					Distance.Z *= -1.0F
				End If
			End If

			Dim DurationWhole = CSng(Math.Truncate(CDbl(Duration / 6.0F)))
			Dim DurationFraction = CSng((Duration / 6.0F - DurationWhole) * 1000)
			Dim DurationTime As TimeSpan = New TimeSpan(0, 0, 0, CInt(DurationWhole), CInt(DurationFraction))
			Dim baEntityOscillateMove As BAEntityOscillateMove = New BAEntityOscillateMove(MoveEntity, RemoveEntityAfter, Distance, Speed, BothWays, DurationTime, startDelay, endDelay, MovementCurve, ReturnToStart)
			AnimationSequence.Add(baEntityOscillateMove)

		End Sub

		Public Sub AnimationSetPosition(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal PositionX As Single, ByVal PositionY As Single, ByVal PositionZ As Single, ByVal startDelay As Single, ByVal endDelay As Single)
			Dim SetEntity As Entity
			Dim SetPosition As Vector3

			If Entity Is Nothing Then
				SetEntity = CurrentEntity
			Else
				SetEntity = Entity
			End If

			SetPosition = New Vector3(PositionX, PositionY, PositionZ) + BattleScreen.BattleMapOffset

			Dim baEntitySetPosition As BAEntitySetPosition = New BAEntitySetPosition(SetEntity, RemoveEntityAfter, SetPosition, startDelay, endDelay)
			AnimationSequence.Add(baEntitySetPosition)

		End Sub

		Public Sub AnimationColor(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal TransitionSpeedIn As Single, ByVal ReturnToFromWhenDone As Boolean, ByVal startDelay As Single, ByVal endDelay As Single, ByVal VectorColorTo As Vector3, Optional ByVal VectorColorFrom As Vector3 = Nothing, Optional TransitionSpeedOut As Single = -1)
			Dim ColorEntity As Entity
			If Entity Is Nothing Then
				ColorEntity = CurrentEntity
			Else
				ColorEntity = Entity
			End If
			Dim baEntityColor As BAEntityColor = New BAEntityColor(ColorEntity, RemoveEntityAfter, TransitionSpeedIn, ReturnToFromWhenDone, startDelay, endDelay, VectorColorTo, VectorColorFrom, TransitionSpeedOut)
			AnimationSequence.Add(baEntityColor)
		End Sub

		Public Sub AnimationColor(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal TransitionSpeedIn As Single, ByVal ReturnToFromWhenDone As Boolean, ByVal startDelay As Single, ByVal endDelay As Single, ByVal ColorTo As Color, Optional ByVal ColorFrom As Color = Nothing, Optional TransitionSpeedOut As Single = -1)
			Dim ColorEntity As Entity
			If Entity Is Nothing Then
				ColorEntity = CurrentEntity
			Else
				ColorEntity = Entity
			End If
			Dim VectorColorTo As Vector3 = ColorTo.ToVector3
			Dim VectorColorFrom As Vector3 = Nothing
			If ColorFrom <> Nothing Then
				VectorColorFrom = ColorFrom.ToVector3
			End If

			Dim baEntityColor As BAEntityColor = New BAEntityColor(ColorEntity, RemoveEntityAfter, TransitionSpeedIn, ReturnToFromWhenDone, startDelay, endDelay, VectorColorTo, VectorColorFrom, TransitionSpeedOut)
			AnimationSequence.Add(baEntityColor)
		End Sub

		Public Sub AnimationFade(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal TransitionSpeed As Single, ByVal FadeIn As Boolean, ByVal EndState As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal startState As Single = -1.0F)
			Dim FadeEntity As Entity
			If Entity Is Nothing Then
				FadeEntity = CurrentEntity
			Else
				FadeEntity = Entity
			End If
			If startState = -1 Then startState = FadeEntity.NormalOpacity
			Dim baEntityOpacity As BAEntityOpacity = New BAEntityOpacity(FadeEntity, RemoveEntityAfter, TransitionSpeed, FadeIn, EndState, startDelay, endDelay, startState)
			AnimationSequence.Add(baEntityOpacity)

		End Sub

		Public Sub AnimationRotate(Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal RotationSpeedX As Single, ByVal RotationSpeedY As Single, ByVal RotationSpeedZ As Single, ByVal EndRotationX As Single, ByVal EndRotationY As Single, ByVal EndRotationZ As Single, ByVal startDelay As Single, ByVal endDelay As Single, ByVal DoXRotation As Boolean, ByVal DoYRotation As Boolean, ByVal DoZRotation As Boolean, ByVal DoReturn As Boolean)
			Dim RotateEntity As Entity
			If Entity Is Nothing Then
				RotateEntity = CurrentEntity
			Else
				RotateEntity = Entity
			End If

			Dim RotationSpeedVector As Vector3 = New Vector3(RotationSpeedX, RotationSpeedY, RotationSpeedZ)
			Dim EndRotation As Vector3 = New Vector3(EndRotationX, EndRotationY, EndRotationZ)
			Dim baEntityRotate As BAEntityRotate = New BAEntityRotate(RotateEntity, RemoveEntityAfter, RotationSpeedVector, EndRotation, startDelay, endDelay, DoXRotation, DoYRotation, DoZRotation, DoReturn)
			AnimationSequence.Add(baEntityRotate)

		End Sub

		Public Sub AnimationTurnNPC(ByVal TurnSteps As Integer, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal EndFaceRotation As Integer = -1, Optional ByVal TurnSpeed As Integer = 1, Optional ByVal TurnDelay As Single = 0.25F)
			Dim TurnNPC As NPC = Nothing
			If CurrentEntity IsNot Nothing Then
				TurnNPC = CType(CurrentEntity, NPC)
			End If

			If Not BattleFlipped = Nothing AndAlso BattleFlipped = True Then
				EndFaceRotation += 2
				TurnSpeed *= -1
			End If

			Dim BAEntityFaceRotate As BAEntityFaceRotate = New BAEntityFaceRotate(TurnNPC, TurnSteps, startDelay, endDelay, EndFaceRotation, TurnSpeed, TurnDelay)
			AnimationSequence.Add(BAEntityFaceRotate)

		End Sub
		Public Sub AnimationScale(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal Grow As Boolean, ByVal EndSizeX As Single, ByVal EndSizeY As Single, ByVal EndSizeZ As Single, ByVal SizeSpeed As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal Anchors As String = "")
			Dim ScaleEntity As Entity
			If Entity Is Nothing Then
				ScaleEntity = CurrentEntity
				If ScaleEntity.Model IsNot Nothing Then
					EndSizeX *= ModelManager.MODELSCALE
					EndSizeY *= ModelManager.MODELSCALE
					EndSizeZ *= ModelManager.MODELSCALE
					SizeSpeed *= ModelManager.MODELSCALE
				End If
			Else
				ScaleEntity = Entity
			End If

			Dim Scale As Vector3 = ScaleEntity.Scale
			Dim EndSize As Vector3 = New Vector3(EndSizeX, EndSizeY, EndSizeZ)
			Dim baEntityScale As BAEntityScale = New BAEntityScale(ScaleEntity, RemoveEntityAfter, Scale, Grow, EndSize, SizeSpeed, startDelay, endDelay, Anchors)
			AnimationSequence.Add(baEntityScale)
		End Sub

		Public Sub AnimationPlaySound(ByVal sound As String, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal stopMusic As Boolean = False, Optional ByVal IsPokemon As Boolean = False)
			Dim baSound As BAPlaySound = New BAPlaySound(sound, startDelay, endDelay, stopMusic, IsPokemon)
			AnimationSequence.Add(baSound)
		End Sub

		Public Sub AnimationBackground(ByVal Texture As Texture2D, ByVal startDelay As Single, ByVal endDelay As Single, ByVal Duration As Single, Optional ByVal AfterFadeInOpacity As Single = 1.0F, Optional ByVal FadeInSpeed As Single = 0.125F, Optional ByVal FadeOutSpeed As Single = 0.125F, Optional ByVal DoTile As Boolean = False, Optional ByVal AnimationLength As Integer = 1, Optional ByVal AnimationSpeed As Integer = 4, Optional ByVal Scale As Integer = 4)
			Dim baBackground As BABackground = New BABackground(Texture, startDelay, endDelay, Duration, AfterFadeInOpacity, FadeInSpeed, FadeOutSpeed, DoTile, AnimationLength, AnimationSpeed, Scale)
			AnimationSequence.Add(baBackground)
		End Sub

		Public Sub AnimationCameraChangeAngle(ByRef Battlescreen As BattleScreen, ByVal CameraAngleID As Integer, ByVal startDelay As Single, ByVal endDelay As Single)
			Dim baCameraChangeAngle As BACameraChangeAngle = New BACameraChangeAngle(Battlescreen, CameraAngleID, startDelay, endDelay)
			AnimationSequence.Add(baCameraChangeAngle)
		End Sub

		Public Sub AnimationCameraOscillateMove(ByVal Distance As Vector3, ByVal Speed As Single, ByVal BothWays As Boolean, ByVal Duration As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional MovementCurve As Integer = 0, Optional ReturnToStart As Vector3 = Nothing)
			Dim ReturnPosition As New Vector3(0)

			If Not BattleFlipped = Nothing Then
				If BattleFlipped = True Then
					Distance.Z *= -1.0F
				End If
			End If

			Dim DurationWhole = CSng(Math.Truncate(CDbl(Duration / 6.0F)))
			Dim DurationFraction = CSng((Duration / 6.0F - DurationWhole) * 1000)
			Dim DurationTime As TimeSpan = New TimeSpan(0, 0, 0, CInt(DurationWhole), CInt(DurationFraction))
			Dim baCameraOscillateMove As BACameraOscillateMove = New BACameraOscillateMove(Distance, Speed, BothWays, DurationTime, startDelay, endDelay, MovementCurve, ReturnToStart)
			AnimationSequence.Add(baCameraOscillateMove)

		End Sub

	End Class
End Namespace