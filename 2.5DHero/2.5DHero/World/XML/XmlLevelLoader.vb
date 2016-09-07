Imports System.Xml

Namespace XmlLevel

    Public Class XmlLevelLoader

        Public Enum NameSpaces
            None
            Settings
            Entities
            Structures
            Shaders
            Offsetmaps
        End Enum

        Public Enum LevelTypes
            [Default]
            Offset
            [Structure]
        End Enum

        Dim CurrentNamespace As NameSpaces = NameSpaces.None 'The current namespace the reader is in.
        Dim LevelOpen As Boolean = False 'Checks if the reader is inside the <level> tag.

        Dim Offset As Vector3 'The offset for offset and structure maps.
        Dim LevelType As LevelTypes = LevelTypes.Default 'The leveltype for the current instance of the XMLLevelLoader
        Dim MapOrigin As String = "" 'The map origin of the offset or structure map.

        Dim CurrentFieldSize As Vector3 = Vector3.One
        Dim CurrentFieldStep As Vector3 = Vector3.One

        Public Sub Load(ByVal path As String, ByVal levelType As LevelTypes, ByVal offset As Vector3)
            If IsValidXML(path) = False Then
                'Report invalid XML file
                Exit Sub
            End If

            Me.LevelType = levelType
            Me.Offset = offset
            Me.MapOrigin = path

            Dim reader As New XmlTextReader(path)

            Me.CleanLevel()

            Do While reader.Read()
                Select Case reader.NodeType
                    Case XmlNodeType.Element
                        If reader.Name.ToLower() = "level" Then
                            LevelOpen = True
                        Else
                            If LevelOpen = True Then
                                Select Case reader.Name.ToLower()
                                    Case "settings"
                                        Me.CurrentNamespace = NameSpaces.Settings
                                    Case "shaders"
                                        Me.CurrentNamespace = NameSpaces.Shaders
                                    Case "entities"
                                        Me.CurrentNamespace = NameSpaces.Entities
                                    Case "structures"
                                        Me.CurrentNamespace = NameSpaces.Structures
                                    Case "offsetmaps"
                                        Me.CurrentNamespace = NameSpaces.Offsetmaps

                                    Case "field"
                                        Me.ReadField(reader)

                                    Case "setting"
                                        If Me.CurrentNamespace = NameSpaces.Settings Then
                                            If Me.LevelType = LevelTypes.Default Then
                                                Me.ReadSetting(reader)
                                            End If
                                        Else
                                            Me.ReportInvalidNamespace(reader.Name)
                                        End If
                                    Case "entity"
                                        If Me.CurrentNamespace = NameSpaces.Entities Then
                                            Me.AddEntity(reader)
                                        Else
                                            Me.ReportInvalidNamespace(reader.Name)
                                        End If
                                    Case "structure"
                                        If Me.CurrentNamespace = NameSpaces.Structures Then
                                            If Me.LevelType <> LevelTypes.Structure Then
                                                Me.ReadStructure(reader)
                                            End If
                                        Else
                                            Me.ReportInvalidNamespace(reader.Name)
                                        End If
                                    Case "shader"
                                        If Me.CurrentNamespace = NameSpaces.Shaders Then
                                            'TODO: Read shader
                                        Else
                                            Me.ReportInvalidNamespace(reader.Name)
                                        End If
                                    Case "offset"
                                        If Me.CurrentNamespace = NameSpaces.Offsetmaps Then
                                            If Me.LevelType = LevelTypes.Default Then
                                                'TODO: Read offset map
                                            End If
                                        Else
                                            Me.ReportInvalidNamespace(reader.Name)
                                        End If
                                End Select
                            End If
                        End If
                    Case XmlNodeType.EndElement
                        Select Case reader.Name.ToLower()
                            Case "settings", "shaders", "entities", "offsetmaps", "structures"
                                CurrentNamespace = NameSpaces.None
                            Case "level"
                                LevelOpen = False
                            Case "field"
                                Me.CurrentFieldSize = Vector3.One
                                Me.CurrentFieldStep = Vector3.One
                        End Select
                End Select
            Loop
        End Sub

#Region "Entity"

        Private Sub AddEntity(ByVal reader As XmlTextReader)
            Dim e As XmlEntity = ReadEntity(reader)
            MsgBox(e.ToString())
            Dim Entities As New List(Of XmlEntity)

            For x = 0 To CInt(Me.CurrentFieldSize.X) - 1 Step CInt(CurrentFieldStep.X)
                For z = 0 To CInt(Me.CurrentFieldSize.Z) - 1 Step CInt(CurrentFieldStep.Z)
                    For y = 0 To CInt(Me.CurrentFieldSize.Y) - 1 Step CInt(CurrentFieldStep.Y)
                        'Dim newEntity As XmlEntity = CType(e.Clone(), XmlEntity)
                        'newEntity.Position += New Vector3(x, y, z)
                        'Entities.Add(newEntity)
                    Next
                Next
            Next

            'TODO: add entities to level.
        End Sub

        Private Function ReadEntity(ByVal reader As XmlTextReader) As XmlEntity
            Dim attributes As Dictionary(Of String, String) = GetAttributes(reader)

            Dim e As New XmlEntity()

            For i = 0 To attributes.Count - 1
                Dim name As String = attributes.Keys(i)
                Dim value As String = attributes.Values(i)

                e.AddProperty(name, value)
            Next



            Return e
        End Function

#End Region

        Private Sub ReadStructure(ByVal reader As XmlTextReader)
            Dim attributes As Dictionary(Of String, String) = GetAttributes(reader)

            Dim Map As String = GetAttribute(Of String)("map", attributes, "")
            Dim Position As Vector3 = GetAttribute(Of Vector3)("position", attributes, Vector3.Zero)

            If Map <> "" Then
                Dim levelLoader As New XmlLevelLoader()
                levelLoader.Load(Map, LevelTypes.Structure, Position)
            End If
        End Sub

        Private Sub ReadOffsetMap(ByVal reader As XmlTextReader)
            Dim attributes As Dictionary(Of String, String) = GetAttributes(reader)

            Dim Map As String = GetAttribute(Of String)("map", attributes, "")
            Dim Position As Vector3 = GetAttribute(Of Vector3)("position", attributes, Vector3.Zero)

            If Map <> "" Then
                Dim levelLoader As New XmlLevelLoader()
                levelLoader.Load(Map, LevelTypes.Offset, Position)
            End If
        End Sub

        Private Sub ReadField(ByVal reader As XmlTextReader)
            Dim attributes As Dictionary(Of String, String) = GetAttributes(reader)

            Dim Size As Vector3 = GetAttribute(Of Vector3)("size", attributes, Vector3.One)
            Dim Steps As Vector3 = GetAttribute(Of Vector3)("step", attributes, Vector3.One)

            Me.CurrentFieldSize = Size
            Me.CurrentFieldStep = Steps
        End Sub

        Private Sub ReadSetting(ByVal reader As XmlTextReader)
            Dim attributes As Dictionary(Of String, String) = GetAttributes(reader)

            Dim name As String = ""
            Dim content As String = ""

            For i = 0 To attributes.Count - 1
                Dim attName As String = attributes.Keys(i)
                Dim attValue As String = attributes.Values(i).ToString()

                Select Case attName.ToLower()
                    Case "name"
                        name = attValue
                    Case "content"
                        content = attValue
                End Select
            Next

            If name <> "" And content <> "" Then
                Select Case name.ToLower()
                    Case "name"
                        Screen.Level.MapName = content
                    Case "music"
                        Screen.Level.MusicLoop = content
                    Case "canteleport"
                        Screen.Level.CanTeleport = GetBool(content)
                    Case "candig"
                        Screen.Level.CanDig = GetBool(content)
                    Case "canfly"
                        Screen.Level.CanFly = GetBool(content)
                    Case "environmenttype"
                        Screen.Level.EnvironmentType = GetInt(content)
                    Case "weather"
                        Screen.Level.WeatherType = GetInt(content)
                    Case "lightning"
                        Screen.Level.LightingType = GetInt(content)
                End Select
            End If
        End Sub

#Region "HelperFunctions"

        Private Sub CleanLevel()
            If Me.LevelType = LevelTypes.Default Then
                Screen.Level.LevelFile = Me.MapOrigin

                Core.Player.LastSavePlace = Screen.Level.LevelFile
                Core.Player.LastSavePlacePosition = Player.Temp.LastPosition.X & "," & Player.Temp.LastPosition.Y.ToString().Replace(GameController.DecSeparator, ".") & "," & Player.Temp.LastPosition.Z

                Screen.Level.Entities.Clear()
                Screen.Level.Floors.Clear()
                Screen.Level.Shaders.Clear()

                Screen.Level.OffsetmapFloors.Clear()
                Screen.Level.OffsetmapEntities.Clear()

                Screen.Level.WildPokemonFloor = False
                Screen.Level.WalkedSteps = 0

                LevelLoader.LoadedOffsetMapNames.Clear()
                LevelLoader.LoadedOffsetMapOffsets.Clear()

                Player.Temp.MapSteps = 0
            End If
        End Sub

        Private Function GetAttributes(ByVal reader As XmlTextReader) As Dictionary(Of String, String)
            Dim d As New Dictionary(Of String, String)
            If reader.HasAttributes = True Then
                While reader.MoveToNextAttribute()
                    d.Add(reader.Name, reader.Value)
                End While
            End If
            Return d
        End Function

        Private Function GetAttribute(Of T)(ByVal Name As String, ByVal Attributes As Dictionary(Of String, String), ByVal Separator As Char, ByVal DefaultValue As T) As T
            For i = 0 To Attributes.Count - 1
                If Attributes.Keys(i).ToLower() = Name.ToLower() Then
                    Dim s As String = Attributes.Values(i)

                    Select Case GetType(T)
                        Case GetType(String)
                            Return CType(CObj(s), T)
                        Case GetType(System.Collections.Generic.List(Of String))
                            Return CType(CObj(s.Split(Separator).ToList()), T)
                        Case GetType(Boolean)
                            Return CType(CObj(GetBool(s)), T)
                        Case GetType(System.Collections.Generic.List(Of Boolean))
                            Dim values() As String = s.Split(Separator)
                            Dim arr As New List(Of Boolean)
                            For Each v As String In values
                                arr.Add(GetBool(v))
                            Next
                            Return CType(CObj(arr), T)
                        Case GetType(Integer)
                            Return CType(CObj(GetInt(s)), T)
                        Case GetType(System.Collections.Generic.List(Of Integer))
                            Dim values() As String = s.Split(Separator)
                            Dim arr As New List(Of Integer)
                            For Each v As String In values
                                arr.Add(GetInt(v))
                            Next
                            Return CType(CObj(arr), T)
                        Case GetType(Single)
                            Return CType(CObj(GetSng(s)), T)
                        Case GetType(System.Collections.Generic.List(Of Single))
                            Dim values() As String = s.Split(Separator)
                            Dim arr As New List(Of Single)
                            For Each v As String In values
                                arr.Add(GetSng(v))
                            Next
                            Return CType(CObj(arr), T)
                        Case GetType(Rectangle)
                            Dim content() As String = s.Split(Separator)
                            If content.Length >= 4 Then
                                Dim rec As New Rectangle(CInt(content(0)), CInt(content(1)), CInt(content(2)), CInt(content(3)))
                            Else
                                Return DefaultValue
                            End If
                        Case GetType(System.Collections.Generic.List(Of Rectangle))
                            Dim values() As String = s.Split(CChar("]"))
                            Dim arr As New List(Of Rectangle)
                            For Each v As String In values
                                If v.Length > 0 Then
                                    v = v.Remove(0, 1)

                                    Dim content() As String = v.Split(Separator)
                                    If content.Length >= 4 Then
                                        arr.Add(New Rectangle(CInt(content(0)), CInt(content(1)), CInt(content(2)), CInt(content(3))))
                                    End If
                                End If
                            Next
                            Return CType(CObj(arr), T)
                        Case GetType(Vector2)
                            Dim values() As String = s.Split(Separator)
                            Dim arr As New List(Of Single)
                            For Each v As String In values
                                arr.Add(GetSng(v))
                            Next
                            If arr.Count >= 2 Then
                                Dim vector As New Vector2(arr(0), arr(1))
                                Return CType(CObj(vector), T)
                            ElseIf arr.Count = 1 Then
                                Dim vector As New Vector2(arr(0))
                                Return CType(CObj(vector), T)
                            Else
                                Return DefaultValue
                            End If
                        Case GetType(Vector3)
                            Dim values() As String = s.Split(Separator)
                            Dim arr As New List(Of Single)
                            For Each v As String In values
                                arr.Add(GetSng(v))
                            Next
                            If arr.Count >= 3 Then
                                Dim vector As New Vector3(arr(0), arr(1), arr(2))
                                Return CType(CObj(vector), T)
                            ElseIf arr.Count = 1 Then
                                Dim vector As New Vector3(arr(0))
                                Return CType(CObj(vector), T)
                            Else
                                Return DefaultValue
                            End If
                        Case GetType(Vector4)
                            Dim values() As String = s.Split(Separator)
                            Dim arr As New List(Of Single)
                            For Each v As String In values
                                arr.Add(GetSng(v))
                            Next
                            If arr.Count >= 4 Then
                                Dim vector As New Vector4(arr(0), arr(1), arr(2), arr(3))
                                Return CType(CObj(vector), T)
                            ElseIf arr.Count = 1 Then
                                Dim vector As New Vector4(arr(0))
                                Return CType(CObj(vector), T)
                            Else
                                Return DefaultValue
                            End If
                    End Select
                End If
            Next
            Return DefaultValue
        End Function

        Private Function GetAttribute(Of T)(ByVal Name As String, ByVal Attributes As Dictionary(Of String, String), ByVal DefaultValue As T) As T
            Return GetAttribute(Of T)(Name, Attributes, CChar(","), DefaultValue)
        End Function

        Private Function IsValidXML(ByVal xmlPath As String) As Boolean
            If System.IO.File.Exists(xmlPath) = True Then
                Dim xmlString As String = System.IO.File.ReadAllText(xmlPath)

                'Crash validation check:
                Try
                    Dim xmlDocument As XmlDocument = New XmlDocument()
                    xmlDocument.LoadXml(xmlString)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            Else
                Return False
            End If
        End Function

        Private Sub ReportInvalidNamespace(ByVal tag As String)
            Logger.Log(Logger.LogTypes.Warning, "The tag """ & tag & """ was used in the wrong namespace (" & CurrentNamespace.ToString() & ")")
        End Sub

#End Region

#Region "ConverterFunctions"

        Private Function GetBool(ByVal expression As Object) As Boolean
            If ScriptConversion.IsBoolean(expression) = True Then
                Return CBool(expression)
            Else
                Return False
            End If
        End Function

        Private Function GetInt(ByVal expression As Object) As Integer
            If ScriptConversion.IsArithmeticExpression(expression) = True Then
                Return ScriptConversion.ToInteger(expression)
            Else
                Return 0
            End If
        End Function

        Private Function GetSng(ByVal expression As Object) As Single
            If ScriptConversion.IsArithmeticExpression(expression) = True Then
                Return ScriptConversion.ToSingle(expression)
            Else
                Return 0.0F
            End If
        End Function

#End Region

    End Class

End Namespace