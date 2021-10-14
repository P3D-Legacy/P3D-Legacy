Namespace BattleSystem

	Public Class AnimationQueryObject
		Inherits QueryObject

		Public AnimationStarted As Boolean = False
		Public AnimationEnded As Boolean = False
		Public BattleFlipped As Boolean = Nothing
		Public AnimationSequence As List(Of BattleAnimation3D)
		Public SpawnedEntities As List(Of Entity)
		Public CurrentEntity As Entity
		Public CurrentModel As ModelEntity

		Public Overrides ReadOnly Property IsReady As Boolean
			Get
				Return AnimationEnded
			End Get
		End Property

		Public Sub New(ByVal entity As Entity, ByVal BattleFlipped As Boolean, Optional ByVal model As ModelEntity = Nothing)
			MyBase.New(QueryTypes.MoveAnimation)
			Me.AnimationSequence = New List(Of BattleAnimation3D)
			If BattleFlipped <> Nothing Then
				Me.BattleFlipped = BattleFlipped
			End If
			Me.CurrentEntity = entity
			Me.CurrentModel = model
			AnimationSequenceBegin()
		End Sub
		Public Overrides Sub Draw(ByVal BV2Screen As BattleScreen)
			Dim RenderObjects As New List(Of Entity)
			For Each a As BattleAnimation3D In Me.AnimationSequence
				RenderObjects.Add(a)
			Next
			If RenderObjects.Count > 0 Then
				RenderObjects = (From r In RenderObjects Order By r.CameraDistance Descending).ToList()
			End If

			For Each [Object] As Entity In RenderObjects
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
							If a.SpawnedEntity IsNot Nothing And a.Ready = True Then
								SpawnedEntities.Add(a.SpawnedEntity)
							End If
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
					Entity.UpdateEntity()
				Next
			End If
		End Sub

		Public Sub AnimationSequenceBegin()
			AnimationStarted = True
		End Sub

		Public Sub AnimationSequenceEnd()
			AnimationEnded = True
		End Sub

		Public Function SpawnEntity(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal Opacity As Single, Optional ByVal startDelay As Single = 0.0F, Optional ByVal endDelay As Single = 0.0F) As Entity
			If Not BattleFlipped = Nothing Then
				If BattleFlipped = True Then
					Position.X = CurrentEntity.Position.X - Position.X * 2.0F
					Position.Z = CurrentEntity.Position.Z - Position.Z * 2.0F
				Else
					Position.X = CurrentEntity.Position.X + Position.X * 2.0F
					Position.Z = CurrentEntity.Position.Z + Position.Z * 2.0F
				End If
			End If

			Dim SpawnedEntity As Entity = New Entity(Position.X, Position.Y, Position.Z, "BattleAnimation", {Texture}, {0, 0}, False, 0, Scale, BaseModel.BillModel, 0, "", New Vector3(1.0F))
			SpawnedEntity.Opacity = Opacity

			Dim SpawnDelayEntity As BattleAnimation3D = New BattleAnimation3D(New Vector3(0.0F), TextureManager.DefaultTexture, New Vector3(1.0F), startDelay, endDelay, SpawnedEntity)

			AnimationSequence.Add(SpawnDelayEntity)
			Return SpawnedEntity
		End Function
		Public Sub RemoveEntity(Entity As Entity)
			SpawnedEntities.Remove(Entity)
		End Sub
		Public Sub ChangeEntityTexture(ByVal Entity As Entity, RemoveEntityAfter As Boolean, ByVal Texture As Texture2D, ByVal startDelay As Single, ByVal endDelay As Single)
			Dim TextureChangeEntity As Entity

			If Entity Is Nothing Then
				TextureChangeEntity = CurrentEntity
			Else
				TextureChangeEntity = Entity
			End If

			Dim baEntityTextureChange As BAEntityTextureChange = New BAEntityTextureChange(TextureChangeEntity, Texture, startDelay, endDelay)
			AnimationSequence.Add(baEntityTextureChange)

			If RemoveEntityAfter = True Then
				If baEntityTextureChange.CanRemove = True Then
					RemoveEntity(Entity)
				End If
			End If
		End Sub

		Public Sub MoveEntity(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal DestinationX As Single, ByVal DestinationY As Single, ByVal DestinationZ As Single, ByVal Speed As Single, ByVal SpinX As Boolean, ByVal SpinZ As Boolean, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal SpinXSpeed As Single = 0.1F, Optional ByVal SpinZSpeed As Single = 0.1F, Optional MovementCurve As Integer = 3)
			Dim MoveEntity As Entity
			Dim Destination As Vector3

			If Not BattleFlipped = Nothing Then
				If BattleFlipped = True Then
					DestinationX -= DestinationX * 2.0F
					DestinationZ -= DestinationZ * 2.0F
				End If
				If Entity Is Nothing Then
					MoveEntity = CurrentEntity
				Else
					MoveEntity = Entity
				End If
				Destination = CurrentEntity.Position + New Vector3(DestinationX, DestinationY, DestinationZ)
			Else
				MoveEntity = Entity
				Destination = New Vector3(DestinationX, DestinationY, DestinationZ)
			End If

			Dim baEntityMove As BAEntityMove = New BAEntityMove(MoveEntity, Destination, Speed, SpinX, SpinZ, startDelay, endDelay, SpinXSpeed, SpinZSpeed, MovementCurve)
			AnimationSequence.Add(baEntityMove)

			If Me.CurrentModel IsNot Nothing Then
				Dim baModelMove As BAEntityMove = New BAEntityMove(CType(CurrentModel, Entity), Destination, Speed, SpinX, SpinZ, startDelay, endDelay, SpinXSpeed, SpinZSpeed, MovementCurve)
				AnimationSequence.Add(baModelMove)
			End If

			If RemoveEntityAfter = True Then
				If baEntityMove.CanRemove = True Then
					RemoveEntity(Entity)
				End If
			End If
		End Sub

		Public Sub FadeEntity(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal TransitionSpeed As Single, ByVal FadeIn As Boolean, ByVal EndState As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal startState As Single = -1.0F)
			Dim FadeEntity As Entity
			If Entity Is Nothing Then
				FadeEntity = CurrentEntity
			Else
				FadeEntity = Entity
			End If
			If startState = -1.0F Then startState = FadeEntity.Opacity
			Dim baEntityOpacity As BAEntityOpacity = New BAEntityOpacity(FadeEntity, TransitionSpeed, FadeIn, EndState, startDelay, endDelay, startState)
			AnimationSequence.Add(baEntityOpacity)

			If Me.CurrentModel IsNot Nothing Then
				Dim baModelOpacity As BAEntityOpacity = New BAEntityOpacity(CType(CurrentModel, Entity), TransitionSpeed, FadeIn, EndState, startDelay, endDelay, startState)
				AnimationSequence.Add(baModelOpacity)
			End If

			If RemoveEntityAfter = True Then
				If baEntityOpacity.CanRemove = True Then
					RemoveEntity(Entity)
				End If
			End If
		End Sub
		Public Sub RotateEntity(Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal RotationSpeedX As Single, ByVal RotationSpeedY As Single, ByVal RotationSpeedZ As Single, ByVal EndRotationX As Single, ByVal EndRotationY As Single, ByVal EndRotationZ As Single, ByVal startDelay As Single, ByVal endDelay As Single, ByVal DoXRotation As Boolean, ByVal DoYRotation As Boolean, ByVal DoZRotation As Boolean, ByVal DoReturn As Boolean)
			Dim RotateEntity As Entity
			If Entity Is Nothing Then
				RotateEntity = CurrentEntity
			Else
				RotateEntity = Entity
			End If

			Dim RotationSpeedVector As Vector3 = New Vector3(RotationSpeedX, RotationSpeedY, RotationSpeedZ)
			Dim EndRotation As Vector3 = New Vector3(EndRotationX, EndRotationY, EndRotationZ)
			Dim baEntityRotate As BAEntityRotate = New BAEntityRotate(RotateEntity, RotationSpeedVector, EndRotation, startDelay, endDelay, DoXRotation, DoYRotation, DoZRotation, DoReturn)
			AnimationSequence.Add(baEntityRotate)
			If RemoveEntityAfter = True Then
				If baEntityRotate.CanRemove = True Then
					RemoveEntity(Entity)
				End If
			End If
		End Sub
		Public Sub ScaleEntity(ByVal Entity As Entity, ByVal RemoveEntityAfter As Boolean, ByVal Grow As Boolean, ByVal EndSizeX As Single, ByVal EndSizeY As Single, ByVal EndSizeZ As Single, ByVal SizeSpeed As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal Anchors As String = "1")
			Dim ScaleEntity As Entity
			If Entity Is Nothing Then
				ScaleEntity = CurrentEntity
			Else
				ScaleEntity = Entity
			End If

			Dim Position As Vector3 = ScaleEntity.Position
			Dim Scale As Vector3 = ScaleEntity.Scale
			Dim EndSize As Vector3 = New Vector3(EndSizeX, EndSizeY, EndSizeZ)
			Dim baBillSize As BAEntityScale = New BAEntityScale(ScaleEntity, Scale, Grow, EndSize, SizeSpeed, startDelay, endDelay, Anchors)
			AnimationSequence.Add(baBillSize)
			If RemoveEntityAfter = True Then
				If baBillSize.CanRemove = True Then
					RemoveEntity(Entity)
				End If
			End If
		End Sub

		Public Sub AnimationSpawnFadingEntity(ByVal PositionX As Single, ByVal PositionY As Single, ByVal PositionZ As Single, ByVal Texture As String, ByVal ScaleX As Single, ByVal ScaleY As Single, ByVal ScaleZ As Single, ByVal TransitionSpeed As Single, ByVal FadeIn As Boolean, ByVal EndState As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal startState As Single = 1.0F)
			If CurrentEntity Is Nothing Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnMovingAnimation OUTSIDE OF ATTACK ANIMATION DELEGATE")
			ElseIf Not AnimationStarted Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnMovingAnimation BEFORE CALLING AnimationSequenceBegin")
			Else
				Dim stringArray = Texture.Split(","c)
				Dim texture2D As Texture2D = Nothing

				If stringArray.Length = 1 Then
					texture2D = TextureManager.GetTexture(Texture)
				ElseIf stringArray.Length = 5 Then
					Dim r As Rectangle = New Rectangle(CInt(stringArray(1)), CInt(stringArray(2)), CInt(stringArray(3)), CInt(stringArray(4)))
					texture2D = TextureManager.GetTexture(stringArray(0), r, "")
				End If

				If BattleFlipped Then
					PositionX -= PositionX * 2.0F
					PositionZ -= PositionZ * 2.0F
				End If

				Dim Position As Vector3 = New Vector3(CurrentEntity.Position.X + PositionX, CurrentEntity.Position.Y + PositionY, CurrentEntity.Position.Z + PositionZ)
				Dim Scale As Vector3 = New Vector3(ScaleX, ScaleY, ScaleZ)

				Dim baOpacity As BAOpacity = New BAOpacity(Position, texture2D, Scale, TransitionSpeed, FadeIn, EndState, startDelay, endDelay, startState)
				AnimationSequence.Add(baOpacity)
			End If
		End Sub

		Public Sub AnimationSpawnMovingEntity(ByVal PositionX As Single, ByVal PositionY As Single, ByVal PositionZ As Single, ByVal Texture As String, ByVal ScaleX As Single, ByVal ScaleY As Single, ByVal ScaleZ As Single, ByVal DestinationX As Single, ByVal DestinationY As Single, ByVal DestinationZ As Single, ByVal Speed As Single, ByVal SpinX As Boolean, ByVal SpinZ As Boolean, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal SpinXSpeed As Single = 0.1F, Optional ByVal SpinZSpeed As Single = 0.1F, Optional MovementCurve As Integer = 3)
			If CurrentEntity Is Nothing Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnMovingAnimation OUTSIDE OF ATTACK ANIMATION DELEGATE")
			ElseIf Not AnimationStarted Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnMovingAnimation BEFORE CALLING AnimationSequenceBegin")
			Else
				Dim stringArray = Texture.Split(","c)
				Dim texture2D As Texture2D = Nothing

				If stringArray.Length = 1 Then
					texture2D = TextureManager.GetTexture(Texture)
				ElseIf stringArray.Length = 5 Then
					Dim r As Rectangle = New Rectangle(CInt(stringArray(1)), CInt(stringArray(2)), CInt(stringArray(3)), CInt(stringArray(4)))
					texture2D = TextureManager.GetTexture(stringArray(0), r, "")
				End If

				If BattleFlipped Then
					PositionX -= PositionX * 2.0F
					PositionZ -= PositionZ * 2.0F
					DestinationX -= DestinationX * 2.0F
					DestinationZ -= DestinationZ * 2.0F
					SpinXSpeed -= SpinXSpeed * 2.0F
					SpinZSpeed -= SpinZSpeed * 2.0F
				End If

				Dim Position As Vector3 = New Vector3(CurrentEntity.Position.X + PositionX, CurrentEntity.Position.Y + PositionY, CurrentEntity.Position.Z + PositionZ)
				Dim Scale As Vector3 = New Vector3(ScaleX, ScaleY, ScaleZ)
				Dim Destination As Vector3 = New Vector3(CurrentEntity.Position.X + DestinationX, CurrentEntity.Position.Y + DestinationY, CurrentEntity.Position.Z + DestinationZ)

				Dim baMove As BAMove = New BAMove(Position, texture2D, Scale, Destination, Speed, SpinX, SpinZ, startDelay, endDelay, SpinXSpeed, SpinZSpeed, MovementCurve)
				AnimationSequence.Add(baMove)
			End If
		End Sub
		Public Sub PlaySound(ByVal sound As String, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal stopMusic As Boolean = False, Optional ByVal IsPokemon As Boolean = False)
			If CurrentEntity Is Nothing Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AnimationPlaySound OUTSIDE OF ATTACK ANIMATION DELEGATE")
			ElseIf Not AnimationStarted Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AnimationPlaySound BEFORE CALLING AnimationSequenceBegin")
			Else
				Dim baSound As BASound = New BASound(sound, startDelay, endDelay, stopMusic, IsPokemon)
				AnimationSequence.Add(baSound)
			End If
		End Sub

	End Class
End Namespace