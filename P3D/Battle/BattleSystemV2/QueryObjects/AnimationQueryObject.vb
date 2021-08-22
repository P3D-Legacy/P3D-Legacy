Namespace BattleSystem

	Public Class AnimationQueryObject
		Inherits QueryObject

		Public AnimationStarted As Boolean = False
		Public AnimationEnded As Boolean = False
		Public BAFlipped As Boolean
		Public AnimationSequence As List(Of BattleAnimation3D)
		Public CurrentEntity As Entity
		Public CurrentModel As ModelEntity

		Public Overrides ReadOnly Property IsReady As Boolean
			Get
				Return AnimationEnded
			End Get
		End Property

		Public Sub New(ByVal entity As NPC, ByVal BAFlipped As Boolean, Optional ByVal model As ModelEntity = Nothing)
			MyBase.New(QueryTypes.MoveAnimation)
			Me.AnimationSequence = New List(Of BattleAnimation3D)
			Me.BAFlipped = BAFlipped
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
			End If
		End Sub

		Public Sub AnimationSequenceBegin()
			If CurrentEntity Is Nothing Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AnimationSequenceBegin OUTSIDE OF ATTACK ANIMATION DELEGATE")
			ElseIf AnimationStarted Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AnimationSequenceBegin INSIDE ANIMATION SEQUENCE, DID YOU MEAN AnimationSequenceEnd?")
			Else
				AnimationStarted = True
			End If
		End Sub

		Public Sub AnimationSequenceEnd()
			If CurrentEntity Is Nothing Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AnimationSequenceEnd OUTSIDE OF ATTACK ANIMATION DELEGATE")
			ElseIf Not AnimationStarted Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AnimationSequenceEnd BEFORE CALLING AnimationSequenceBegin")
			Else
				AnimationEnded = True
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

				If BAFlipped Then
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

				If BAFlipped Then
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
		Public Sub AnimationMovePokemonEntity(ByVal DestinationX As Single, ByVal DestinationY As Single, ByVal DestinationZ As Single, ByVal Speed As Single, ByVal SpinX As Boolean, ByVal SpinZ As Boolean, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal SpinXSpeed As Single = 0.1F, Optional ByVal SpinZSpeed As Single = 0.1F, Optional MovementCurve As Integer = 3)
			If CurrentEntity Is Nothing Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnMovingAnimation OUTSIDE OF ATTACK ANIMATION DELEGATE")
			ElseIf Not AnimationStarted Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnMovingAnimation BEFORE CALLING AnimationSequenceBegin")
			Else
				If BAFlipped Then
					DestinationX -= DestinationX * 2.0F
					DestinationZ -= DestinationZ * 2.0F
				End If

				Dim Destination As Vector3 = New Vector3(CurrentEntity.Position.X + DestinationX, CurrentEntity.Position.Y + DestinationY, CurrentEntity.Position.Z + DestinationZ)

				Dim baBillMove As BABillMove = New BABillMove(CurrentEntity, Destination, Speed, SpinX, SpinZ, startDelay, endDelay, SpinXSpeed, SpinZSpeed, MovementCurve)
				AnimationSequence.Add(baBillMove)

				If Me.CurrentModel IsNot Nothing Then
					Dim baModelMove As BABillMove = New BABillMove(CType(CurrentModel, Entity), Destination, Speed, SpinX, SpinZ, startDelay, endDelay, SpinXSpeed, SpinZSpeed, MovementCurve)
					AnimationSequence.Add(baModelMove)
				End If
			End If
		End Sub
		Public Sub AnimationFadePokemonEntity(ByVal TransitionSpeed As Single, ByVal FadeIn As Boolean, ByVal EndState As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal startState As Single = -1.0F)
			If CurrentEntity Is Nothing Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnMovingAnimation OUTSIDE OF ATTACK ANIMATION DELEGATE")
			ElseIf Not AnimationStarted Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnMovingAnimation BEFORE CALLING AnimationSequenceBegin")
			Else
				If startState = -1.0F Then startState = CurrentEntity.Opacity
				Dim baBillOpacity As BABillOpacity = New BABillOpacity(CurrentEntity, TransitionSpeed, FadeIn, EndState, startDelay, endDelay, startState)
				AnimationSequence.Add(baBillOpacity)

				If Me.CurrentModel IsNot Nothing Then
					Dim baModelOpacity As BABillOpacity = New BABillOpacity(CType(CurrentModel, Entity), TransitionSpeed, FadeIn, EndState, startDelay, endDelay, startState)
					AnimationSequence.Add(baModelOpacity)
				End If
			End If
		End Sub
		Public Sub AnimationPlaySound(ByVal sound As String, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal stopMusic As Boolean = False, Optional ByVal IsPokemon As Boolean = False)
			If CurrentEntity Is Nothing Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AnimationPlaySound OUTSIDE OF ATTACK ANIMATION DELEGATE")
			ElseIf Not AnimationStarted Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AnimationPlaySound BEFORE CALLING AnimationSequenceBegin")
			Else
				Dim baSound As BASound = New BASound(sound, startDelay, endDelay, stopMusic, IsPokemon)
				AnimationSequence.Add(baSound)
			End If
		End Sub

		Public Sub AnimationSpawnScalingEntity(ByVal PositionX As Single, ByVal PositionY As Single, ByVal PositionZ As Single, ByVal Texture As String, ByVal ScaleX As Single, ByVal ScaleY As Single, ByVal ScaleZ As Single, ByVal Grow As Boolean, ByVal EndSizeX As Single, ByVal EndSizeY As Single, ByVal EndSizeZ As Single, ByVal SizeSpeed As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal Anchors As String = "1")
			If CurrentEntity Is Nothing Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnSizeAnimation OUTSIDE OF ATTACK ANIMATION DELEGATE")
			ElseIf Not AnimationStarted Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnSizeAnimation BEFORE CALLING AnimationSequenceBegin")
			Else
				Dim stringArray = Texture.Split(","c)
				Dim texture2D As Texture2D = Nothing

				If stringArray.Length = 1 Then
					texture2D = TextureManager.GetTexture(Texture)
				ElseIf stringArray.Length = 5 Then
					Dim r As Rectangle = New Rectangle(CInt(stringArray(1)), CInt(stringArray(2)), CInt(stringArray(3)), CInt(stringArray(4)))
					texture2D = TextureManager.GetTexture(stringArray(0), r, "")
				End If

				If BAFlipped Then
					PositionX -= PositionX * 2.0F
					PositionZ -= PositionZ * 2.0F
				End If
				Dim Position As Vector3 = New Vector3(CurrentEntity.Position.X + PositionX, CurrentEntity.Position.Y + PositionY, CurrentEntity.Position.Z + PositionZ)
				Dim Scale As Vector3 = New Vector3(ScaleX, ScaleY, ScaleZ)
				Dim EndSize As Vector3 = New Vector3(EndSizeX, EndSizeY, EndSizeZ)
				Dim baSize As BASize = New BASize(Position, texture2D, Scale, Grow, EndSize, SizeSpeed, startDelay, endDelay, Anchors)
				AnimationSequence.Add(baSize)
			End If
		End Sub
		Public Sub AnimationScalePokemonEntity(ByVal entity As Entity, ByVal PositionX As Single, ByVal PositionY As Single, ByVal PositionZ As Single, ByVal Texture As String, ByVal ScaleX As Single, ByVal ScaleY As Single, ByVal ScaleZ As Single, ByVal Grow As Boolean, ByVal EndSizeX As Single, ByVal EndSizeY As Single, ByVal EndSizeZ As Single, ByVal SizeSpeed As Single, ByVal startDelay As Single, ByVal endDelay As Single, Optional ByVal Anchors As String = "1")
			If CurrentEntity Is Nothing Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnSizeAnimation OUTSIDE OF ATTACK ANIMATION DELEGATE")
			ElseIf Not AnimationStarted Then
				Logger.Log(Logger.LogTypes.Warning, "ATTEMPT TO USE AttackSpawnSizeAnimation BEFORE CALLING AnimationSequenceBegin")
			Else
				Dim stringArray = Texture.Split(","c)


				If BAFlipped Then
					PositionX -= PositionX * 2.0F
					PositionZ -= PositionZ * 2.0F
				End If
				Dim Position As Vector3 = New Vector3(CurrentEntity.Position.X + PositionX, CurrentEntity.Position.Y + PositionY, CurrentEntity.Position.Z + PositionZ)
				Dim Scale As Vector3 = New Vector3(ScaleX, ScaleY, ScaleZ)
				Dim EndSize As Vector3 = New Vector3(EndSizeX, EndSizeY, EndSizeZ)
				Dim baBillSize As BABillSize = New BABillSize(entity, Scale, Grow, EndSize, SizeSpeed, startDelay, endDelay, Anchors)
				AnimationSequence.Add(baBillSize)
			End If
		End Sub
	End Class
End Namespace