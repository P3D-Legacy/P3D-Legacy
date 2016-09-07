Public Class Floor

    Inherits Entity

    Dim changedWeatherTexture As Boolean = False
    Public hasSnow As Boolean = True
    Public hasSand As Boolean = True
    Public IsIce As Boolean = False

    Private _changedToSnow As Boolean = False
    Private _changedToSand As Boolean = False

    Public Sub New()
    End Sub

    Public Sub New(ByVal X As Single, ByVal Y As Single, ByVal Z As Single, ByVal Textures() As Texture2D, ByVal TextureIndex() As Integer, ByVal Collision As Boolean, ByVal Rotation As Integer, ByVal Scale As Vector3, ByVal Model As BaseModel, ByVal ActionValue As Integer, ByVal AdditionalValue As String, ByVal Visible As Boolean, ByVal Shader As Vector3, ByVal hasSnow As Boolean, ByVal IsIce As Boolean, ByVal hasSand As Boolean)
        MyBase.New(X, Y, Z, "Floor", Textures, TextureIndex, Collision, Rotation, Scale, Model, ActionValue, AdditionalValue, Shader)

        Me.hasSnow = hasSnow
        Me.hasSand = hasSand
        Me.IsIce = IsIce
        Me.Visible = Visible
    End Sub

    Public Overloads Sub Initialize(ByVal hasSnow As Boolean, ByVal IsIce As Boolean, ByVal hasSand As Boolean)
        MyBase.Initialize()

        Me.hasSnow = hasSnow
        Me.hasSand = hasSand
        Me.IsIce = IsIce
    End Sub

    Public Sub SetRotation(ByVal Rotation As Integer)
        Select Case Rotation
            Case 0
                Me.Rotation.Y = 0
            Case 1
                Me.Rotation.Y = MathHelper.PiOver2
            Case 2
                Me.Rotation.Y = MathHelper.Pi
            Case 3
                Me.Rotation.Y = MathHelper.Pi * 1.5F
        End Select
        Me.CreatedWorld = False
    End Sub

    Public Overrides Sub Render()
        If changedWeatherTexture = False Then
            changedWeatherTexture = True
            If (Screen.Level.World.CurrentMapWeather = Game.World.Weathers.Snow Or Screen.Level.World.CurrentMapWeather = Game.World.Weathers.Blizzard) = True And Me.hasSnow = True Then
                ChangeSnow()
            End If
            If Screen.Level.World.CurrentMapWeather = Game.World.Weathers.Sandstorm And Me.hasSand = True Then
                ChangeSand()
            End If
        End If

        Me.Draw(Me.Model, Textures, False)
    End Sub

    Private Shared FloorDictionary As New Dictionary(Of String, Entity)

    Private Sub ChangeSnow()
        Me.Rotation = New Vector3(Me.Rotation.X, 0.0F, Me.Rotation.Z)
        If Core.CurrentScreen.Identification = Screen.Identifications.BattleScreen Then
            Me.Textures(0) = net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(208, 16, 16, 16))
            'Me.Position.Y += 0.05F
        Else
            Dim hasEntityOnAllSides As Boolean = True
            Dim ent(4) As Entity
            Dim sides() As Integer = {-1, -1, -1, -1}

            If Me.IsOffsetMapContent = True Then
                ent(0) = GetEntity(Screen.Level.OffsetmapFloors, New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z + 1))
                ent(1) = GetEntity(Screen.Level.OffsetmapFloors, New Vector3(Me.Position.X + 1, Me.Position.Y, Me.Position.Z))
                ent(2) = GetEntity(Screen.Level.OffsetmapFloors, New Vector3(Me.Position.X - 1, Me.Position.Y, Me.Position.Z))
                ent(3) = GetEntity(Screen.Level.OffsetmapFloors, New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z - 1))
            Else
                ent(0) = GetEntity(Screen.Level.Floors, New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z + 1))
                ent(1) = GetEntity(Screen.Level.Floors, New Vector3(Me.Position.X + 1, Me.Position.Y, Me.Position.Z))
                ent(2) = GetEntity(Screen.Level.Floors, New Vector3(Me.Position.X - 1, Me.Position.Y, Me.Position.Z))
                ent(3) = GetEntity(Screen.Level.Floors, New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z - 1))
            End If

            For i = 0 To 3
                If ent(i) Is Nothing Then
                    hasEntityOnAllSides = False
                    sides(i) = 0
                Else
                    If CType(ent(i), Floor).hasSnow = False Then
                        hasEntityOnAllSides = False
                        sides(i) = 0
                    End If
                End If
            Next

            If hasEntityOnAllSides = False Then
                Me.Textures = {net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(208, 16, 16, 2)), net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(208, 16, 16, 16))}
                Me.Model = BaseModel.BlockModel
                Me.TextureIndex = {sides(0), sides(0), sides(1), sides(1), sides(2), sides(2), sides(3), sides(3), 1, 1}
                Me.Scale = New Vector3(1, 0.1F, 1)
                Me.Position.Y -= 0.45F
            Else
                Me.Textures(0) = net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(208, 16, 16, 16))
                Me.Position.Y += 0.1F
            End If
        End If

        Me.Visible = True
        Me.CreatedWorld = False
        Me.UpdateEntity()

        If FloorDictionary.ContainsKey(Me.Position.ToString()) = False Then
            FloorDictionary.Add(Me.Position.ToString(), Me)
        End If

        Me._changedToSnow = True
    End Sub

    Private Sub ChangeSand()
        Me.Rotation = New Vector3(Me.Rotation.X, 0.0F, Me.Rotation.Z)
        If Core.CurrentScreen.Identification = Screen.Identifications.BattleScreen Then
            Me.Textures(0) = net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(240, 80, 16, 16))
            'Me.Position.Y += 0.05F
        Else
            Dim hasEntityOnAllSides As Boolean = True
            Dim ent(4) As Entity
            Dim sides() As Integer = {-1, -1, -1, -1}

            If Me.IsOffsetMapContent = True Then
                ent(0) = GetEntity(Screen.Level.OffsetmapFloors, New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z + 1))
                ent(1) = GetEntity(Screen.Level.OffsetmapFloors, New Vector3(Me.Position.X + 1, Me.Position.Y, Me.Position.Z))
                ent(2) = GetEntity(Screen.Level.OffsetmapFloors, New Vector3(Me.Position.X - 1, Me.Position.Y, Me.Position.Z))
                ent(3) = GetEntity(Screen.Level.OffsetmapFloors, New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z - 1))
            Else
                ent(0) = GetEntity(Screen.Level.Floors, New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z + 1))
                ent(1) = GetEntity(Screen.Level.Floors, New Vector3(Me.Position.X + 1, Me.Position.Y, Me.Position.Z))
                ent(2) = GetEntity(Screen.Level.Floors, New Vector3(Me.Position.X - 1, Me.Position.Y, Me.Position.Z))
                ent(3) = GetEntity(Screen.Level.Floors, New Vector3(Me.Position.X, Me.Position.Y, Me.Position.Z - 1))
            End If

            For i = 0 To 3
                If ent(i) Is Nothing Then
                    hasEntityOnAllSides = False
                    sides(i) = 0
                Else
                    If CType(ent(i), Floor).hasSnow = False Then
                        hasEntityOnAllSides = False
                        sides(i) = 0
                    End If
                End If
            Next

            If hasEntityOnAllSides = False Then
                Me.Textures = {net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(240, 80, 16, 2)), net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(240, 80, 16, 16))}
                Me.Model = BaseModel.BlockModel
                Me.TextureIndex = {sides(0), sides(0), sides(1), sides(1), sides(2), sides(2), sides(3), sides(3), 1, 1}
                Me.Scale = New Vector3(1, 0.1F, 1)
                Me.Position.Y -= 0.45F
            Else
                Me.Textures(0) = net.Pokemon3D.Game.TextureManager.GetTexture("Routes", New Rectangle(240, 80, 16, 16))
                Me.Position.Y += 0.1F
            End If
        End If

        Me.Visible = True
        Me.CreatedWorld = False
        Me.UpdateEntity()

        If FloorDictionary.ContainsKey(Me.Position.ToString()) = False Then
            FloorDictionary.Add(Me.Position.ToString(), Me)
        End If

        Me._changedToSand = True
    End Sub

    Public Function GetIceFloors() As Integer
        Dim Steps As Integer = 0

        Dim checkPosition As Vector3 = Screen.Camera.GetForwardMovedPosition()
        checkPosition.Y = checkPosition.Y.ToInteger()

        Dim foundSteps As Boolean = True
        While foundSteps = True
            Dim e As Entity = MyBase.GetEntity(Screen.Level.Floors, checkPosition, True, {GetType(Floor)})
            If Not e Is Nothing Then
                If e.EntityID = "Floor" Then
                    If CType(e, Floor).IsIce = True Then
                        If CType(Screen.Camera, OverworldCamera).CheckCollision(checkPosition) = False Then
                            Steps += 1
                            checkPosition.X += Screen.Camera.GetMoveDirection().X
                            checkPosition.Z += Screen.Camera.GetMoveDirection().Z

                            Screen.Level.OverworldPokemon.Visible = False
                            Screen.Level.OverworldPokemon.warped = True
                        Else
                            foundSteps = False
                        End If
                    Else
                        If CType(Screen.Camera, OverworldCamera).CheckCollision(checkPosition) = False Then
                            Steps += 1
                        End If
                        foundSteps = False
                    End If
                Else
                    foundSteps = False
                End If
            Else
                foundSteps = False
            End If
        End While

        Return Steps
    End Function

    Private Shadows Function GetEntity(ByVal List As List(Of Entity), ByVal Position As Vector3) As Entity
        Dim positionString As String = Position.ToString()
        If FloorDictionary.ContainsKey(positionString) = False Then
            FloorDictionary.Add(positionString, (From ent As Entity In List Select ent Where ent.Position = Position)(0))
        End If

        Return FloorDictionary(positionString)
    End Function

    ''' <summary>
    ''' Clears the list that stores the placements of floors.
    ''' </summary>
    Public Shared Sub ClearFloorTemp()
        FloorDictionary.Clear()
    End Sub

End Class