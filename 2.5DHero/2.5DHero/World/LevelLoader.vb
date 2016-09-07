Public Class LevelLoader

    Const MULTITHREAD As Boolean = False

    Public Shared LoadedOffsetMapOffsets As New List(Of Vector3)
    Public Shared LoadedOffsetMapNames As New List(Of String)

    Private Enum TagTypes
        Entity
        Floor
        EntityField
        Level
        LevelActions
        NPC
        Shader
        OffsetMap
        [Structure]
        Backdrop
        None
    End Enum

    Dim Offset As Vector3
    Dim loadOffsetMap As Boolean = True
    Dim offsetMapLevel As Integer = 0
    Dim MapOrigin As String = ""
    Dim sessionMapsLoaded As New List(Of String) 'Prevents infinite loops when loading more than one offset map level.

    'Store these so other classes can get them.
    Private Entities As New List(Of Entity)
    Private Floors As New List(Of Entity)

    'A counter across all LevelLoader instances to count how many instances across the program are active.
    Shared Busy As Integer = 0

    Public Shared ReadOnly Property IsBusy() As Boolean
        Get
            Return Busy > 0
        End Get
    End Property

#Region "File Loading"

    Dim TempParams As Object()

    ''' <summary>
    ''' Loads the level.
    ''' </summary>
    ''' <param name="Params">Params contruction: String LevelFile, bool IsOffsetMap, Vector3 Offset, int Offsetmaplevel, Str() InstanceLoadedOffsetMaps</param>
    Public Sub LoadLevel(ByVal Params As Object())
        Busy += 1
        TempParams = Params

        If MULTITHREAD = True Then
            Dim t As New Threading.Thread(AddressOf InternalLoad)
            t.IsBackground = True
            t.Start()
        Else
            InternalLoad()
        End If
    End Sub

    Private Sub InternalLoad()
        Dim levelPath As String = CStr(TempParams(0))
        Dim loadOffsetMap As Boolean = CBool(TempParams(1))
        Dim offset As Vector3 = CType(TempParams(2), Vector3)
        offsetMapLevel = CInt(TempParams(3))
        sessionMapsLoaded = CType(TempParams(4), List(Of String))

        Dim timer As Stopwatch = New Stopwatch
        timer.Start()

        Me.loadOffsetMap = loadOffsetMap
        MapOrigin = levelPath

        If loadOffsetMap = False Then
            Screen.Level.LevelFile = levelPath

            Core.Player.LastSavePlace = Screen.Level.LevelFile
            Core.Player.LastSavePlacePosition = Player.Temp.LastPosition.X & "," & Player.Temp.LastPosition.Y.ToString().Replace(GameController.DecSeparator, ".") & "," & Player.Temp.LastPosition.Z

            Screen.Level.Entities.Clear()
            Screen.Level.Floors.Clear()
            Screen.Level.Shaders.Clear()
            Screen.Level.BackdropRenderer.Clear()

            Screen.Level.OffsetmapFloors.Clear()
            Screen.Level.OffsetmapEntities.Clear()

            Screen.Level.WildPokemonFloor = False
            Screen.Level.WalkedSteps = 0

            LoadedOffsetMapNames.Clear()
            LoadedOffsetMapOffsets.Clear()
            Floor.ClearFloorTemp()

            Player.Temp.MapSteps = 0

            sessionMapsLoaded.Add(levelPath)
        End If

        levelPath = GameModeManager.GetMapPath(levelPath)
        Logger.Debug("Loading map: " & levelPath.Remove(0, GameController.GamePath.Length))
        Security.FileValidation.CheckFileValid(levelPath, False, "LevelLoader.vb")

        If IO.File.Exists(levelPath) = False Then
            Logger.Log(Logger.LogTypes.ErrorMessage, "LevelLoader.vb: Error accessing map file """ & levelPath & """. File not found.")
            Busy -= 1

            If CurrentScreen.Identification = Screen.Identifications.OverworldScreen And loadOffsetMap = False Then
                CType(CurrentScreen, OverworldScreen).Titles.Add(New OverworldScreen.Title("Couldn't find map file!", 20.0F, Color.White, 6.0F, Vector2.Zero, True))
            End If

            Exit Sub
        End If

        Dim Data As List(Of String) = IO.File.ReadAllLines(levelPath).ToList()
        Dim Tags As New Dictionary(Of String, Object)

        Me.Offset = offset

        For Each line As String In Data
            If line.Contains("{") = True Then
                line = line.Remove(0, line.IndexOf("{"))

                If line.StartsWith("{""Comment""{COM") = True Then
                    line = line.Remove(0, line.IndexOf("[") + 1)
                    line = line.Remove(line.IndexOf("]"))

                    Logger.Log(Logger.LogTypes.Debug, line)
                End If
            End If
        Next

        Dim countLines As Integer = 0

        For i = 0 To Integer.MaxValue
            If i > Data.Count - 1 Then
                Exit For
            End If

            Dim line As String = Data(i)
            Tags.Clear()
            If line.Contains("{") = True And line.Contains("}") = True Then
                Try
                    Dim TagType As TagTypes = TagTypes.None
                    line = line.Remove(0, line.IndexOf("{") + 2)

                    Select Case True
                        Case line.ToLower().StartsWith("structure""")
                            TagType = TagTypes.Structure
                    End Select

                    If TagType = TagTypes.Structure Then
                        line = line.Remove(0, line.IndexOf("[") + 1)
                        line = line.Remove(line.Length - 3, 3)

                        Tags = GetTags(line)

                        Dim newLines() As String = AddStructure(Tags)

                        Data.InsertRange(i + 1, newLines)
                    End If
                Catch ex As Exception
                    Logger.Log(Logger.LogTypes.Warning, "LevelLoader.vb: Failed to load map object! (Index: " & countLines & ") from mapfile: " & levelPath & "; Error message: " & ex.Message)
                End Try
            End If
        Next

        For Each line As String In Data
            Tags.Clear()
            Dim orgLine As String = line
            countLines += 1

            If line.Contains("{") = True And line.Contains("}") = True Then
                Try
                    Dim TagType As TagTypes = TagTypes.None
                    line = line.Remove(0, line.IndexOf("{") + 2)

                    Select Case True
                        Case line.ToLower().StartsWith("entity""")
                            TagType = TagTypes.Entity
                        Case line.ToLower().StartsWith("floor""")
                            TagType = TagTypes.Floor
                        Case line.ToLower().StartsWith("entityfield""")
                            TagType = TagTypes.EntityField
                        Case line.ToLower().StartsWith("level""")
                            TagType = TagTypes.Level
                        Case line.ToLower().StartsWith("actions""")
                            TagType = TagTypes.LevelActions
                        Case line.ToLower().StartsWith("npc""")
                            TagType = TagTypes.NPC
                        Case line.ToLower().StartsWith("shader""")
                            TagType = TagTypes.Shader
                        Case line.ToLower().StartsWith("offsetmap""")
                            TagType = TagTypes.OffsetMap
                        Case line.ToLower().StartsWith("backdrop""")
                            TagType = TagTypes.Backdrop
                    End Select

                    If TagType <> TagTypes.None Then
                        line = line.Remove(0, line.IndexOf("[") + 1)
                        line = line.Remove(line.Length - 3, 3)

                        Tags = GetTags(line)

                        Select Case TagType
                            Case TagTypes.EntityField
                                EntityField(Tags)
                            Case TagTypes.Entity
                                AddEntity(Tags, New Size(1, 1), 1, True, New Vector3(1, 1, 1))
                            Case TagTypes.Floor
                                AddFloor(Tags)
                            Case TagTypes.Level
                                If loadOffsetMap = False Then
                                    SetupLevel(Tags)
                                End If
                            Case TagTypes.LevelActions
                                If loadOffsetMap = False Then
                                    SetupActions(Tags)
                                End If
                            Case TagTypes.NPC
                                AddNPC(Tags)
                            Case TagTypes.Shader
                                If loadOffsetMap = False Then
                                    AddShader(Tags)
                                End If
                            Case TagTypes.OffsetMap
                                If loadOffsetMap = False Or offsetMapLevel <= Core.GameOptions.MaxOffsetLevel Then
                                    AddOffsetMap(Tags)
                                End If
                            Case TagTypes.Backdrop
                                If loadOffsetMap = False Then
                                    AddBackdrop(Tags)
                                End If
                        End Select
                    End If
                Catch ex As Exception
                    Logger.Log(Logger.LogTypes.Warning, "LevelLoader.vb: Failed to load map object! (Index: " & countLines & ") (Line: " & orgLine & ") from mapfile: " & levelPath & "; Error message: " & ex.Message)
                End Try
            End If
        Next

        If loadOffsetMap = False Then
            LoadBerries()
        End If

        For Each s As Shader In Screen.Level.Shaders
            If s.HasBeenApplied = False Then
                s.ApplyShader(Screen.Level.Entities.ToArray())
                s.ApplyShader(Screen.Level.Floors.ToArray())
            End If
        Next

        Logger.Debug("Map loading finished: " & levelPath.Remove(0, GameController.GamePath.Length))
        Logger.Debug("Loaded textures: " & TextureManager.TextureList.Count.ToString())
        timer.Stop()
        Logger.Debug("Map loading time: " & timer.ElapsedTicks & " Ticks; " & timer.ElapsedMilliseconds & " Milliseconds.")

        'Dim xmlLevelLoader As New XmlLevelLoader
        'xmlLevelLoader.Load(My.Computer.FileSystem.SpecialDirectories.Desktop & "\t.xml", _5DHero.XmlLevelLoader.LevelTypes.Default, Vector3.Zero)

        Busy -= 1

        If Busy = 0 Then
            Screen.Level.StartOffsetMapUpdate()
        End If
    End Sub

    Private Function GetTags(ByVal line As String) As Dictionary(Of String, Object)
        Dim Tags As New Dictionary(Of String, Object)

        Dim tagList = line.Split({"}{"}, StringSplitOptions.RemoveEmptyEntries)
        For i = 0 To tagList.Length - 1
            Dim currentTag As String = tagList(i)
            If currentTag.EndsWith("}}") = False Then
                currentTag &= "}"
            End If
            If currentTag.StartsWith("{") = False Then
                currentTag = "{" & currentTag
            End If
            ProcessTag(Tags, currentTag)
        Next

        Return Tags
    End Function

    Private Sub ProcessTag(ByRef Dictionary As Dictionary(Of String, Object), ByVal Tag As String)
        Dim TagName As String = ""
        Dim TagContent As String = ""

        Tag = Tag.Remove(0, 1)
        Tag = Tag.Remove(Tag.Length - 1, 1)

        TagName = Tag.Remove(Tag.IndexOf("{") - 1).Remove(0, 1)
        TagContent = Tag.Remove(0, Tag.IndexOf("{"))

        Dim ContentRows() As String = TagContent.Split(CChar("}"))
        For Each subTag As String In ContentRows
            If subTag.Length > 0 Then
                subTag = subTag.Remove(0, 1)

                Dim subTagType As String = subTag.Remove(subTag.IndexOf("["))
                Dim subTagValue As String = subTag.Remove(0, subTag.IndexOf("[") + 1)
                subTagValue = subTagValue.Remove(subTagValue.Length - 1, 1)

                Select Case subTagType.ToLower()
                    Case "int"
                        Dictionary.Add(TagName, CInt(subTagValue))
                    Case "str"
                        Dictionary.Add(TagName, CStr(subTagValue))
                    Case "sng"
                        subTagValue = subTagValue.Replace(".", GameController.DecSeparator)
                        Dictionary.Add(TagName, CSng(subTagValue))
                    Case "bool"
                        Dictionary.Add(TagName, CBool(subTagValue))
                    Case "intarr"
                        Dim values() As String = subTagValue.Split(CChar(","))
                        Dim arr As New List(Of Integer)
                        For Each value As String In values
                            arr.Add(CInt(value))
                        Next
                        Dictionary.Add(TagName, arr)
                    Case "rec"
                        Dim content() As String = subTagValue.Split(CChar(","))
                        Dictionary.Add(TagName, New Rectangle(CInt(content(0)), CInt(content(1)), CInt(content(2)), CInt(content(3))))
                    Case "recarr"
                        Dim values() As String = subTagValue.Split(CChar("]"))
                        Dim arr As New List(Of Rectangle)
                        For Each value As String In values
                            If value.Length > 0 Then
                                value = value.Remove(0, 1)

                                Dim content() As String = value.Split(CChar(","))
                                arr.Add(New Rectangle(CInt(content(0)), CInt(content(1)), CInt(content(2)), CInt(content(3))))
                            End If
                        Next
                        Dictionary.Add(TagName, arr)
                    Case "sngarr"
                        Dim values() As String = subTagValue.Split(CChar(","))
                        Dim arr As New List(Of Single)
                        For Each value As String In values
                            value = value.Replace(".", GameController.DecSeparator)
                            arr.Add(CSng(value))
                        Next
                        Dictionary.Add(TagName, arr)
                End Select
            End If
        Next
    End Sub

    Private Function GetTag(ByVal Tags As Dictionary(Of String, Object), ByVal TagName As String) As Object
        If Tags.ContainsKey(TagName) = True Then
            Return Tags(TagName)
        End If

        For i = 0 To Tags.Count - 1
            If Tags.Keys(i).ToLower() = TagName.ToLower() Then
                Return Tags.Values(i)
            End If
        Next

        Return Nothing
    End Function

    Private Function TagExists(ByVal Tags As Dictionary(Of String, Object), ByVal TagName As String) As Boolean
        If Tags.ContainsKey(TagName) = True Then
            Return True
        End If

        For i = 0 To Tags.Count - 1
            If Tags.Keys(i).ToLower() = TagName.ToLower() Then
                Return True
            End If
        Next

        Return False
    End Function

#End Region

    Private Sub AddOffsetMap(ByVal Tags As Dictionary(Of String, Object))
        If Core.GameOptions.LoadOffsetMaps > 0 Then
            Dim OffsetList As List(Of Integer) = CType(GetTag(Tags, "Offset"), List(Of Integer))
            Dim MapOffset As Vector3 = New Vector3(OffsetList(0), 0, OffsetList(1))
            If OffsetList.Count >= 3 Then
                MapOffset = New Vector3(OffsetList(0), OffsetList(1), OffsetList(2))
            End If

            Dim MapName As String = CStr(GetTag(Tags, "Map"))

            If loadOffsetMap = True Then
                If sessionMapsLoaded.Contains(MapName) = True Then
                    Exit Sub
                End If
            End If
            sessionMapsLoaded.Add(MapName)

            LoadedOffsetMapNames.Add(MapName)
            LoadedOffsetMapOffsets.Add(MapOffset)

            Dim listName As String = Screen.Level.LevelFile & "|" & MapName & "|" & Screen.Level.World.CurrentMapWeather & "|" & World.GetCurrentRegionWeather() & "|" & World.GetTime() & "|" & World.CurrentSeason()
            If OffsetMaps.ContainsKey(listName) = False Then
                Dim mapList As New List(Of List(Of Entity))

                Dim params As New List(Of Object)
                params.AddRange({MapName, True, MapOffset + Offset, offsetMapLevel + 1, sessionMapsLoaded})

                Dim offsetEntityCount As Integer = Screen.Level.OffsetmapEntities.Count
                Dim offsetFloorCount As Integer = Screen.Level.OffsetmapFloors.Count

                Dim levelLoader As New LevelLoader()
                levelLoader.LoadLevel(params.ToArray())
                Dim entList As New List(Of Entity)
                Dim floorList As New List(Of Entity)

                For i = offsetEntityCount To Screen.Level.OffsetmapEntities.Count - 1
                    entList.Add(Screen.Level.OffsetmapEntities(i))
                Next
                For i = offsetFloorCount To Screen.Level.OffsetmapFloors.Count - 1
                    floorList.Add(Screen.Level.OffsetmapFloors(i))
                Next
                mapList.AddRange({entList, floorList})

                OffsetMaps.Add(listName, mapList)
            Else
                Logger.Debug("Loaded Offsetmap from store: " & MapName)

                For Each e As Entity In OffsetMaps(listName)(0)
                    If e.MapOrigin = MapName Then
                        e.IsOffsetMapContent = True
                        Screen.Level.OffsetmapEntities.Add(e)
                    End If
                Next
                For Each e As Entity In OffsetMaps(listName)(1)
                    If e.MapOrigin = MapName Then
                        e.IsOffsetMapContent = True
                        Screen.Level.OffsetmapFloors.Add(e)
                    End If
                Next
            End If
            Logger.Debug("Offset maps in store: " & OffsetMaps.Count)

            Screen.Level.OffsetmapEntities = (From e In Screen.Level.OffsetmapEntities Order By e.CameraDistance Descending).ToList()

            For Each Entity As Entity In Screen.Level.OffsetmapEntities
                Entity.UpdateEntity()
            Next
            For Each Floor As Entity In Screen.Level.OffsetmapFloors
                Floor.UpdateEntity()
            Next
        End If
    End Sub

#Region "AddElements"

    Shared tempStructureList As New Dictionary(Of String, List(Of String))

    Public Shared Sub ClearTempStructures()
        tempStructureList.Clear()
    End Sub

    Private Function AddStructure(ByVal Tags As Dictionary(Of String, Object)) As String()
        Dim OffsetList As List(Of Single) = CType(GetTag(Tags, "Offset"), List(Of Single))
        Dim MapOffset As Vector3 = New Vector3(OffsetList(0), 0, OffsetList(1))
        If OffsetList.Count >= 3 Then
            MapOffset = New Vector3(OffsetList(0), OffsetList(1), OffsetList(2))
        End If

        Dim MapRotation As Integer = -1
        If TagExists(Tags, "Rotation") = True Then
            MapRotation = CInt(GetTag(Tags, "Rotation"))
        End If

        Dim MapName As String = CStr(GetTag(Tags, "Map"))
        If MapName.EndsWith(".dat") = False Then
            MapName = MapName & ".dat"
        End If

        Dim addNPC As Boolean = False
        If TagExists(Tags, "AddNPC") = True Then
            addNPC = CBool(GetTag(Tags, "AddNPC"))
        End If

        Dim structureKey As String = MapOffset.X.ToString() & "|" & MapOffset.Y.ToString() & "|" & MapOffset.Z.ToString() & "|" & MapName

        If tempStructureList.ContainsKey(structureKey) = False Then
            Dim filepath As String = GameModeManager.GetMapPath(MapName)
            Security.FileValidation.CheckFileValid(filepath, False, "LevelLoader.vb/StructureSpawner")

            If IO.File.Exists(filepath) = False Then
                Logger.Log(Logger.LogTypes.ErrorMessage, "LevelLoader.vb: Error loading structure from """ & filepath & """. File not found.")

                Return {}
            End If

            Dim MapContent() As String = IO.File.ReadAllLines(filepath)
            Dim structureList As New List(Of String)

            For Each line As String In MapContent
                If line.EndsWith("}") = True Then
                    Dim addLine As Boolean = False
                    Select Case True
                        Case line.StartsWith("{""Entity""{ENT[")
                            addLine = True
                        Case line.StartsWith("{""Floor""{ENT[")
                            addLine = True
                        Case line.StartsWith("{""EntityField""{ENT[")
                            addLine = True
                        Case line.StartsWith("{""NPC""{NPC[")
                            If addNPC = True Then
                                addLine = True
                            End If
                        Case line.StartsWith("{""Shader""{SHA[")
                            addLine = True
                    End Select

                    If addLine = True Then
                        line = ReplaceStructurePosition(line, MapOffset)

                        If MapRotation > -1 Then
                            line = ReplaceStructureRotation(line, MapRotation)
                        End If

                        structureList.Add(line)
                    End If
                End If
            Next

            tempStructureList.Add(structureKey, structureList)
        End If

        Return tempStructureList(structureKey).ToArray()
    End Function

    Private Function ReplaceStructureRotation(ByVal line As String, ByVal MapRotation As Integer) As String
        Dim replaceString As String = ""

        If line.ToLower().Contains("{""rotation""{int[") = True Then
            replaceString = "{""rotation""{int["
        End If

        If replaceString <> "" Then
            Dim rotationString As String = line.Remove(0, line.ToLower().IndexOf(replaceString))
            rotationString = rotationString.Remove(rotationString.IndexOf("]}}") + 3)

            Dim rotationData As String = rotationString.Remove(0, rotationString.IndexOf("[") + 1)
            rotationData = rotationData.Remove(rotationData.IndexOf("]"))

            Dim newRotation As Integer = CInt(rotationData) + MapRotation
            While newRotation > 3
                newRotation -= 4
            End While

            line = line.Replace(rotationString, "{""rotation""{int[" & newRotation.ToString() & "]}}")
        End If

        Return line
    End Function

    Private Function ReplaceStructurePosition(ByVal line As String, ByVal MapOffset As Vector3) As String
        Dim replaceString As String = ""

        If line.ToLower().Contains("{""position""{sngarr[") = True Then
            replaceString = "{""position""{sngarr["
        ElseIf line.ToLower().Contains("{""position""{intarr[") = True Then
            replaceString = "{""position""{intarr["
        End If

        If replaceString <> "" Then
            Dim positionString As String = line.Remove(0, line.ToLower().IndexOf(replaceString))
            positionString = positionString.Remove(positionString.IndexOf("]}}") + 3)

            Dim positionData As String = positionString.Remove(0, positionString.IndexOf("[") + 1)
            positionData = positionData.Remove(positionData.IndexOf("]"))

            Dim posArr() As String = positionData.Split(CChar(","))
            Dim newPosition As New Vector3(ScriptConversion.ToSingle(posArr(0).Replace(".", GameController.DecSeparator)) + MapOffset.X, ScriptConversion.ToSingle(posArr(1).Replace(".", GameController.DecSeparator)) + MapOffset.Y, CSng(posArr(2).Replace(".", GameController.DecSeparator)) + MapOffset.Z)

            If line.ToLower().Contains("{""position""{sngarr[") = True Then
                line = line.Replace(positionString, "{""position""{sngarr[" & newPosition.X.ToString().Replace(GameController.DecSeparator, ".") & "," & newPosition.Y.ToString().Replace(GameController.DecSeparator, ".") & "," & newPosition.Z.ToString().Replace(GameController.DecSeparator, ".") & "]}}")
            Else
                line = line.Replace(positionString, "{""position""{intarr[" & CInt(newPosition.X).ToString().Replace(GameController.DecSeparator, ".") & "," & CInt(newPosition.Y).ToString().Replace(GameController.DecSeparator, ".") & "," & CInt(newPosition.Z).ToString().Replace(GameController.DecSeparator, ".") & "]}}")
            End If
        End If

        Return line
    End Function

    Private Sub EntityField(ByVal Tags As Dictionary(Of String, Object))
        Dim SizeList As List(Of Integer) = CType(GetTag(Tags, "Size"), List(Of Integer))
        Dim Fill As Boolean = True
        If TagExists(Tags, "Fill") = True Then
            Fill = CBool(GetTag(Tags, "Fill"))
        End If
        Dim Steps As New Vector3(1, 1, 1)
        If TagExists(Tags, "Steps") = True Then
            Dim StepList As List(Of Single) = CType(GetTag(Tags, "Steps"), List(Of Single))
            If StepList.Count = 3 Then
                Steps = New Vector3(StepList(0), StepList(1), StepList(2))
            Else
                Steps = New Vector3(StepList(0), 1, StepList(1))
            End If
        End If

        If SizeList.Count = 3 Then
            AddEntity(Tags, New Size(SizeList(0), SizeList(2)), SizeList(1), Fill, Steps)
        Else
            AddEntity(Tags, New Size(SizeList(0), SizeList(1)), 1, Fill, Steps)
        End If
    End Sub

    Private Sub AddNPC(ByVal Tags As Dictionary(Of String, Object))
        Dim PosList As List(Of Single) = CType(GetTag(Tags, "Position"), List(Of Single))
        Dim Position As Vector3 = New Vector3(PosList(0) + Offset.X, PosList(1) + Offset.Y, PosList(2) + Offset.Z)

        Dim ScaleList As List(Of Single)
        Dim Scale As New Vector3(1)
        If TagExists(Tags, "Scale") = True Then
            ScaleList = CType(GetTag(Tags, "Scale"), List(Of Single))
            Scale = New Vector3(ScaleList(0), ScaleList(1), ScaleList(2))
        End If

        Dim TextureID As String = CStr(GetTag(Tags, "TextureID"))
        Dim Rotation As Integer = CInt(GetTag(Tags, "Rotation"))
        Dim ActionValue As Integer = CInt(GetTag(Tags, "Action"))
        Dim AdditionalValue As String = CStr(GetTag(Tags, "AdditionalValue"))
        Dim Name As String = CStr(GetTag(Tags, "Name"))
        Dim ID As Integer = CInt(GetTag(Tags, "ID"))

        Dim Movement As String = CStr(GetTag(Tags, "Movement"))
        Dim MoveRectangles As List(Of Rectangle) = CType(GetTag(Tags, "MoveRectangles"), List(Of Rectangle))

        Dim Shader As New Vector3(1.0F)
        If TagExists(Tags, "Shader") = True Then
            Dim ShaderList As List(Of Single) = CType(GetTag(Tags, "Shader"), List(Of Single))
            Shader = New Vector3(ShaderList(0), ShaderList(1), ShaderList(2))
        End If

        Dim AnimateIdle As Boolean = False
        If TagExists(Tags, "AnimateIdle") = True Then
            AnimateIdle = CBool(GetTag(Tags, "AnimateIdle"))
        End If

        Dim NPC As NPC = CType(Entity.GetNewEntity("NPC", Position, {Nothing}, {0, 0}, True, New Vector3(0), Scale, BaseModel.BillModel, ActionValue, AdditionalValue, True, Shader, -1, MapOrigin, "", Offset, {TextureID, Rotation, Name, ID, AnimateIdle, Movement, MoveRectangles}), NPC)

        If loadOffsetMap = False Then
            Screen.Level.Entities.Add(NPC)
        Else
            Screen.Level.OffsetmapEntities.Add(NPC)
        End If
    End Sub

    Private Sub AddFloor(ByVal Tags As Dictionary(Of String, Object))
        Dim sizeList As List(Of Integer) = CType(GetTag(Tags, "Size"), List(Of Integer))
        Dim Size As Size = New Size(sizeList(0), sizeList(1))

        Dim PosList As List(Of Integer) = CType(GetTag(Tags, "Position"), List(Of Integer))
        Dim Position As Vector3 = New Vector3(PosList(0) + Offset.X, PosList(1) + Offset.Y, PosList(2) + Offset.Z)

        Dim TexturePath As String = CStr(GetTag(Tags, "TexturePath"))
        Dim TextureRectangle As Rectangle = CType(GetTag(Tags, "Texture"), Rectangle)
        Dim Texture As Texture2D = TextureManager.GetTexture(TexturePath, TextureRectangle)

        Dim Visible As Boolean = True
        If TagExists(Tags, "Visible") = True Then
            Visible = CBool(GetTag(Tags, "Visible"))
        End If

        Dim Shader As New Vector3(1.0F)
        If TagExists(Tags, "Shader") = True Then
            Dim ShaderList As List(Of Single) = CType(GetTag(Tags, "Shader"), List(Of Single))
            Shader = New Vector3(ShaderList(0), ShaderList(1), ShaderList(2))
        End If

        Dim RemoveFloor As Boolean = False
        If TagExists(Tags, "Remove") = True Then
            RemoveFloor = CBool(GetTag(Tags, "Remove"))
        End If

        Dim hasSnow As Boolean = True
        If TagExists(Tags, "hasSnow") = True Then
            hasSnow = CBool(GetTag(Tags, "hasSnow"))
        End If

        Dim hasSand As Boolean = True
        If TagExists(Tags, "hasSand") = True Then
            hasSand = CBool(GetTag(Tags, "hasSand"))
        End If

        Dim hasIce As Boolean = False
        If TagExists(Tags, "isIce") = True Then
            hasIce = CBool(GetTag(Tags, "isIce"))
        End If

        Dim rotation As Integer = 0
        If TagExists(Tags, "Rotation") = True Then
            rotation = CInt(GetTag(Tags, "Rotation"))
        End If

        Dim SeasonTexture As String = ""
        If TagExists(Tags, "SeasonTexture") = True Then
            SeasonTexture = CStr(GetTag(Tags, "SeasonTexture"))
        End If

        Dim floorList As List(Of Entity) = Screen.Level.Floors
        If loadOffsetMap = True Then
            floorList = Screen.Level.OffsetmapFloors
        End If

        If RemoveFloor = False Then
            For x = 0 To Size.Width - 1
                For z = 0 To Size.Height - 1
                    Dim exists As Boolean = False

                    Dim iZ As Integer = z
                    Dim iX As Integer = x

                    Dim Ent As Entity = Nothing

                    If loadOffsetMap = True Then
                        Ent = Screen.Level.OffsetmapFloors.Find(Function(e As Entity)
                                                                    Return e.Position = New Vector3(Position.X + iX, Position.Y, Position.Z + iZ)
                                                                End Function)
                    Else
                        Ent = Screen.Level.Floors.Find(Function(e As Entity)
                                                           Return e.Position = New Vector3(Position.X + iX, Position.Y, Position.Z + iZ)
                                                       End Function)
                    End If

                    If Not Ent Is Nothing Then
                        Ent.Textures = {Texture}
                        Ent.Visible = Visible
                        Ent.SeasonColorTexture = SeasonTexture
                        Ent.LoadSeasonTextures()
                        CType(Ent, Floor).SetRotation(rotation)
                        CType(Ent, Floor).hasSnow = hasSnow
                        CType(Ent, Floor).IsIce = hasIce
                        CType(Ent, Floor).hasSand = hasSand
                        exists = True
                    End If

                    If exists = False Then
                        Dim f As Floor = New Floor(Position.X + x, Position.Y, Position.Z + z, {TextureManager.GetTexture(TexturePath, TextureRectangle)}, {0, 0}, False, rotation, New Vector3(1.0F), BaseModel.FloorModel, 0, "", Visible, Shader, hasSnow, hasIce, hasSand)
                        f.MapOrigin = MapOrigin
                        f.SeasonColorTexture = SeasonTexture
                        f.LoadSeasonTextures()
                        f.IsOffsetMapContent = loadOffsetMap
                        floorList.Add(f)
                    End If
                Next
            Next
        Else
            For x = 0 To Size.Width - 1
                For z = 0 To Size.Height - 1
                    For i = 0 To floorList.Count
                        If i < floorList.Count Then
                            Dim floor As Entity = floorList(i)
                            If floor.Position.X = Position.X + x And floor.Position.Y = Position.Y And floor.Position.Z = Position.Z + z Then
                                floorList.RemoveAt(i)
                                i -= 1
                            End If
                        End If
                    Next
                Next
            Next
        End If
    End Sub

    Private Sub AddEntity(ByVal Tags As Dictionary(Of String, Object), ByVal Size As Size, ByVal SizeY As Integer, ByVal Fill As Boolean, ByVal Steps As Vector3)
        Dim EntityID As String = CStr(GetTag(Tags, "EntityID"))

        Dim ID As Integer = -1
        If TagExists(Tags, "ID") = True Then
            ID = CInt(GetTag(Tags, "ID"))
        End If

        Dim PosList As List(Of Single) = CType(GetTag(Tags, "Position"), List(Of Single))
        Dim Position As Vector3 = New Vector3(PosList(0) + Offset.X, PosList(1) + Offset.Y, PosList(2) + Offset.Z)

        Dim TexList As List(Of Rectangle) = CType(GetTag(Tags, "Textures"), List(Of Rectangle))
        Dim TextureList As New List(Of Texture2D)
        Dim TexturePath As String = CStr(GetTag(Tags, "TexturePath"))
        For Each TextureRectangle As Rectangle In TexList
            TextureList.Add(TextureManager.GetTexture(TexturePath, TextureRectangle))
        Next
        Dim TextureArray() As Texture2D = TextureList.ToArray()

        Dim TextureIndexList As List(Of Integer) = CType(GetTag(Tags, "TextureIndex"), List(Of Integer))
        Dim TextureIndex() As Integer = TextureIndexList.ToArray()

        Dim ScaleList As List(Of Single)
        Dim Scale As New Vector3(1)
        If TagExists(Tags, "Scale") = True Then
            ScaleList = CType(GetTag(Tags, "Scale"), List(Of Single))
            Scale = New Vector3(ScaleList(0), ScaleList(1), ScaleList(2))
        End If

        Dim Collision As Boolean = CBool(GetTag(Tags, "Collision"))

        Dim ModelID As Integer = CInt(GetTag(Tags, "ModelID"))

        Dim ActionValue As Integer = CInt(GetTag(Tags, "Action"))

        Dim AdditionalValue As String = ""
        If TagExists(Tags, "AdditionalValue") = True Then
            AdditionalValue = CStr(GetTag(Tags, "AdditionalValue"))
        End If

        Dim Rotation As Vector3 = Entity.GetRotationFromInteger(CInt(GetTag(Tags, "Rotation")))

        Dim Visible As Boolean = True
        If TagExists(Tags, "Visible") = True Then
            Visible = CBool(GetTag(Tags, "Visible"))
        End If

        Dim Shader As New Vector3(1.0F)
        If TagExists(Tags, "Shader") = True Then
            Dim ShaderList As List(Of Single) = CType(GetTag(Tags, "Shader"), List(Of Single))
            Shader = New Vector3(ShaderList(0), ShaderList(1), ShaderList(2))
        End If

        Dim RotationXYZ As Vector3 = Nothing
        If TagExists(Tags, "RotationXYZ") = True Then
            Dim rotationList As List(Of Single) = CType(GetTag(Tags, "RotationXYZ"), List(Of Single))
            Rotation = New Vector3(rotationList(0), rotationList(1), rotationList(2))
        End If

        Dim SeasonTexture As String = ""
        If TagExists(Tags, "SeasonTexture") = True Then
            SeasonTexture = CStr(GetTag(Tags, "SeasonTexture"))
        End If

        Dim SeasonToggle As String = ""
        If TagExists(Tags, "SeasonToggle") = True Then
            SeasonToggle = CStr(GetTag(Tags, "SeasonToggle"))
        End If

        Dim Opacity As Single = 1.0F
        If TagExists(Tags, "Opacity") = True Then
            Opacity = CSng(GetTag(Tags, "Opacity"))
        End If

        For X = 0 To Size.Width - 1 Step Steps.X
            For Z = 0 To Size.Height - 1 Step Steps.Z
                For Y = 0 To SizeY - 1 Step Steps.Y
                    Dim DoAdd As Boolean = False
                    If Fill = False Then
                        If X = 0 Or Z = 0 Or Z = Size.Height - 1 Or X = Size.Width - 1 Then
                            DoAdd = True
                        End If
                    Else
                        DoAdd = True
                    End If

                    If SeasonToggle <> "" Then
                        If SeasonToggle.Contains(",") = False Then
                            If SeasonToggle.ToLower() <> World.CurrentSeason.ToString().ToLower() Then
                                DoAdd = False
                            End If
                        Else
                            Dim seasons() As String = SeasonToggle.ToLower().Split(CChar(","))
                            If seasons.Contains(World.CurrentSeason.ToString().ToLower()) = False Then
                                DoAdd = False
                            End If
                        End If
                    End If

                    If DoAdd = True Then
                        Dim newEnt As Entity = Entity.GetNewEntity(EntityID,
                                                                   New Vector3(Position.X + X, Position.Y + Y, Position.Z + Z),
                                                                   TextureArray,
                                                                   TextureIndex,
                                                                   Collision,
                                                                   Rotation,
                                                                   Scale,
                                                                   BaseModel.getModelbyID(ModelID),
                                                                   ActionValue,
                                                                   AdditionalValue,
                                                                   Visible,
                                                                   Shader,
                                                                   ID,
                                                                   MapOrigin,
                                                                   SeasonTexture,
                                                                   Offset,
                                                                   {},
                                                                   Opacity)
                        newEnt.IsOffsetMapContent = loadOffsetMap

                        If Not newEnt Is Nothing Then
                            If loadOffsetMap = False Then
                                Screen.Level.Entities.Add(newEnt)
                            Else
                                Screen.Level.OffsetmapEntities.Add(newEnt)
                            End If
                        End If
                    End If
                Next
            Next
        Next
    End Sub

    Private Sub SetupLevel(ByVal Tags As Dictionary(Of String, Object))
        Dim Name As String = CStr(GetTag(Tags, "Name"))
        Dim MusicLoop As String = CStr(GetTag(Tags, "MusicLoop"))

        If TagExists(Tags, "WildPokemon") = True Then
            Screen.Level.WildPokemonFloor = CBool(GetTag(Tags, "WildPokemon"))
        Else
            Screen.Level.WildPokemonFloor = False
        End If

        If TagExists(Tags, "OverworldPokemon") = True Then
            Screen.Level.ShowOverworldPokemon = CBool(GetTag(Tags, "OverworldPokemon"))
        Else
            Screen.Level.ShowOverworldPokemon = True
        End If

        If TagExists(Tags, "CurrentRegion") = True Then
            Screen.Level.CurrentRegion = CStr(GetTag(Tags, "CurrentRegion"))
        Else
            Screen.Level.CurrentRegion = "Johto"
        End If

        If TagExists(Tags, "HiddenAbility") Then
            Screen.Level.HiddenAbilityChance = CInt(GetTag(Tags, "HiddenAbility"))
        Else
            Screen.Level.HiddenAbilityChance = 0
        End If
        Screen.Level.MapName = Name
        Screen.Level.MusicLoop = MusicLoop
    End Sub

    Public Shared MapScript As String = ""

    Private Sub SetupActions(ByVal Tags As Dictionary(Of String, Object))
        If TagExists(Tags, "CanTeleport") = True Then
            Screen.Level.CanTeleport = CBool(GetTag(Tags, "CanTeleport"))
        Else
            Screen.Level.CanTeleport = False
        End If

        If TagExists(Tags, "CanDig") = True Then
            Screen.Level.CanDig = CBool(GetTag(Tags, "CanDig"))
        Else
            Screen.Level.CanDig = False
        End If

        If TagExists(Tags, "CanFly") = True Then
            Screen.Level.CanFly = CBool(GetTag(Tags, "CanFly"))
        Else
            Screen.Level.CanFly = False
        End If

        If TagExists(Tags, "RideType") = True Then
            Screen.Level.RideType = CInt(GetTag(Tags, "RideType"))
        Else
            Screen.Level.RideType = 0
        End If

        If TagExists(Tags, "EnviromentType") = True Then
            Screen.Level.EnvironmentType = CInt(GetTag(Tags, "EnviromentType"))
        Else
            Screen.Level.EnvironmentType = 0
        End If

        If TagExists(Tags, "Weather") = True Then
            Screen.Level.WeatherType = CInt(GetTag(Tags, "Weather"))
        Else
            Screen.Level.WeatherType = 0
        End If

        'It's not my fault I swear. The keyboard was slippy, I was partly sick and there was fog on the road and I couldnt see.
        Dim lightningExists As Boolean = TagExists(Tags, "Lightning")
        Dim lightingExists As Boolean = TagExists(Tags, "Lighting")

        If lightningExists = True And lightingExists = True Then
            Screen.Level.LightingType = CInt(GetTag(Tags, "Lighting"))
        ElseIf lightingExists = True Then
            Screen.Level.LightingType = CInt(GetTag(Tags, "Lighting"))
        ElseIf lightningExists = True Then
            Screen.Level.LightingType = CInt(GetTag(Tags, "Lightning"))
        Else
            Screen.Level.LightingType = 1
        End If

        If TagExists(Tags, "IsDark") = True Then
            Screen.Level.IsDark = CBool(GetTag(Tags, "IsDark"))
        Else
            Screen.Level.IsDark = False
        End If

        If TagExists(Tags, "Terrain") = True Then
            Screen.Level.Terrain.TerrainType = Terrain.FromString(CStr(GetTag(Tags, "Terrain")))
        Else
            Screen.Level.Terrain.TerrainType = Terrain.TerrainTypes.Plain
        End If

        If TagExists(Tags, "IsSafariZone") = True Then
            Screen.Level.IsSafariZone = CBool(GetTag(Tags, "IsSafariZone"))
        Else
            Screen.Level.IsSafariZone = False
        End If

        If TagExists(Tags, "BugCatchingContest") = True Then
            Screen.Level.IsBugCatchingContest = True
            Screen.Level.BugCatchingContestData = CStr(GetTag(Tags, "BugCatchingContest"))
        Else
            Screen.Level.IsBugCatchingContest = False
            Screen.Level.BugCatchingContestData = ""
        End If

        If TagExists(Tags, "MapScript") = True Then
            Dim scriptName As String = CStr(GetTag(Tags, "MapScript"))
            If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                If CType(CurrentScreen, OverworldScreen).ActionScript.IsReady = True Then
                    CType(CurrentScreen, OverworldScreen).ActionScript.reDelay = 0.0F
                    CType(CurrentScreen, OverworldScreen).ActionScript.StartScript(scriptName, 0)
                Else 'A script intro is playing (fly)
                    MapScript = scriptName
                End If
            Else 'Must be a direct save load from the main menu.
                MapScript = scriptName
            End If
        Else
            MapScript = ""
        End If

        If TagExists(Tags, "RadioChannels") = True Then
            Dim channels() As String = CStr(GetTag(Tags, "RadioChannels")).Split(CChar(","))
            For Each c As String In channels
                Screen.Level.AllowedRadioChannels.Add(CDec(c.Replace(".", GameController.DecSeparator)))
            Next
        Else
            Screen.Level.AllowedRadioChannels.Clear()
        End If

        If TagExists(Tags, "BattleMap") = True Then
            Screen.Level.BattleMapData = CStr(GetTag(Tags, "BattleMap"))
        Else
            Screen.Level.BattleMapData = ""
        End If

        Screen.Level.World = New World(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
    End Sub

    Private Sub AddShader(ByVal Tags As Dictionary(Of String, Object))
        Dim SizeList As List(Of Integer) = CType(GetTag(Tags, "Size"), List(Of Integer))
        Dim Size As New Vector3(SizeList(0), 1, SizeList(1))
        If SizeList.Count = 3 Then
            Size = New Vector3(SizeList(0), SizeList(1), SizeList(2))
        End If

        Dim ShaderList As List(Of Single) = CType(GetTag(Tags, "Shader"), List(Of Single))
        Dim Shader As Vector3 = New Vector3(ShaderList(0), ShaderList(1), ShaderList(2))

        Dim StopOnContact As Boolean = CBool(GetTag(Tags, "StopOnContact"))

        Dim PosList As List(Of Integer) = CType(GetTag(Tags, "Position"), List(Of Integer))
        Dim Position As Vector3 = New Vector3(PosList(0) + Offset.X, PosList(1) + Offset.Y, PosList(2) + Offset.Z)

        Dim ObjectSizeList As List(Of Integer) = CType(GetTag(Tags, "Size"), List(Of Integer))
        Dim ObjectSize As New Size(ObjectSizeList(0), ObjectSizeList(1))

        Dim DayTime As New List(Of Integer)
        If TagExists(Tags, "DayTime") = True Then
            DayTime = CType(GetTag(Tags, "DayTime"), List(Of Integer))
        End If

        If DayTime.Contains(World.GetTime()) Or DayTime.Contains(-1) Or DayTime.Count = 0 Then
            Dim NewShader As New Shader(Position, Size, Shader, StopOnContact)
            Screen.Level.Shaders.Add(NewShader)
        End If
    End Sub

    Private Sub AddBackdrop(ByVal Tags As Dictionary(Of String, Object))
        Dim SizeList As List(Of Integer) = CType(GetTag(Tags, "Size"), List(Of Integer))
        Dim Width As Integer = SizeList(0)
        Dim Height As Integer = SizeList(1)

        Dim PosList As List(Of Single) = CType(GetTag(Tags, "Position"), List(Of Single))
        Dim Position As Vector3 = New Vector3(PosList(0) + Offset.X, PosList(1) + Offset.Y, PosList(2) + Offset.Z)

        Dim Rotation As Vector3 = Vector3.Zero
        If TagExists(Tags, "Rotation") = True Then
            Dim rotationList As List(Of Single) = CType(GetTag(Tags, "Rotation"), List(Of Single))
            Rotation = New Vector3(rotationList(0), rotationList(1), rotationList(2))
        End If

        Dim BackdropType As String = CStr(GetTag(Tags, "Type"))

        Dim TexturePath As String = CStr(GetTag(Tags, "TexturePath"))
        Dim TextureRectangle As Rectangle = CType(GetTag(Tags, "Texture"), Rectangle)
        Dim Texture As Texture2D = TextureManager.GetTexture(TexturePath, TextureRectangle)

        Dim trigger As String = ""
        Dim isTriggered As Boolean = True

        If TagExists(Tags, "Trigger") = True Then
            trigger = CStr(GetTag(Tags, "Trigger"))
        End If
        Select Case trigger.ToLower()
            Case "offset"
                If Core.GameOptions.LoadOffsetMaps = 0 Then
                    isTriggered = False
                End If
            Case "notoffset"
                If Core.GameOptions.LoadOffsetMaps > 0 Then
                    isTriggered = False
                End If
        End Select

        If isTriggered = True Then
            Screen.Level.BackdropRenderer.AddBackdrop(New BackdropRenderer.Backdrop(BackdropType, Position, Rotation, Width, Height, Texture))
        End If
    End Sub

#End Region

    Private Sub LoadBerries()
        Dim Data() As String = Core.Player.BerryData.Replace("}" & vbNewLine, "}").Split(CChar("}"))
        For Each Berry As String In Data
            If Berry.Contains("{") = True Then
                Berry = Berry.Remove(0, Berry.IndexOf("{"))
                Berry = Berry.Remove(0, 1)

                Dim BData As List(Of String) = Berry.Split(CChar("|")).ToList()
                Dim PData() As String = BData(1).Split(CChar(","))

                If BData.Count = 6 Then
                    BData.Add("0")
                End If

                If BData(0).ToLower() = Screen.Level.LevelFile.ToLower() Then
                    Dim newEnt As Entity = Entity.GetNewEntity("BerryPlant", New Vector3(CSng(PData(0)), CSng(PData(1)), CSng(PData(2))), {Nothing}, {0, 0}, True, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1.0F), -1, MapOrigin, "", Offset)
                    CType(newEnt, BerryPlant).Initialize(CInt(BData(2)), CInt(BData(3)), CStr(BData(4)), BData(5), CBool(BData(6)))

                    Screen.Level.Entities.Add(newEnt)
                End If
            End If
        Next
    End Sub

End Class