Namespace XmlLevel

    Public Class XmlProperty

        Shared InitializedDefaults As Boolean = False
        Shared DefaultProperties As New Dictionary(Of String, XmlProperty)

        Public Shared ReadOnly Property GetDefaultProperty(ByVal Name As String) As XmlProperty
            Get
                If InitializedDefaults = False Then
                    InitializeDefault()
                End If

                If DefaultProperties.ContainsKey(Name.ToLower()) = True Then
                    Return DefaultProperties(Name.ToLower())
                End If

                Return Nothing
            End Get
        End Property

        Private Shared Sub InitializeDefault()
            InitializedDefaults = True

            DefaultProperties.Clear()

            'Standard stuff:
            DefaultProperties.Add("position", New XmlProperty("position", "0,0,0", Types.Vector3))
            DefaultProperties.Add("scale", New XmlProperty("scale", "1,1,1", Types.Vector3))
            DefaultProperties.Add("seasontexture", New XmlProperty("seasontexture", "", Types.String))
            DefaultProperties.Add("texturepath", New XmlProperty("texturepath", "Village", Types.String))
            DefaultProperties.Add("textures", New XmlProperty("textures", "[0,0,16,16]", Types.RectangleList))
            DefaultProperties.Add("textureindex", New XmlProperty("textureindex", "0,0,0,0,0,0,0,0,0,0", Types.IntegerList))
            DefaultProperties.Add("collision", New XmlProperty("collision", "1", Types.Boolean))
            DefaultProperties.Add("basemodel", New XmlProperty("basemodel", "1", Types.Integer))
            DefaultProperties.Add("rotation", New XmlProperty("rotation", "0", Types.Integer))
            DefaultProperties.Add("xyzrotation", New XmlProperty("xyzrotation", "0,0,0", Types.Vector3))
            DefaultProperties.Add("model", New XmlProperty("model", "", Types.String))
            DefaultProperties.Add("rendertype", New XmlProperty("rendertype", "basemodel", Types.String))
            DefaultProperties.Add("visible", New XmlProperty("visible", "1", Types.Boolean))
            DefaultProperties.Add("id", New XmlProperty("id", "-1", Types.Integer))
            DefaultProperties.Add("culling", New XmlProperty("culling", "0,0,0", Types.Vector3))
            DefaultProperties.Add("seethrough", New XmlProperty("seethrough", "1", Types.Boolean))
            DefaultProperties.Add("shader", New XmlProperty("shader", "1,1,1", Types.Vector3))
            DefaultProperties.Add("opacity", New XmlProperty("opacity", "1", Types.Single))

            'StairBlock:
            DefaultProperties.Add("isstairs", New XmlProperty("isstairs", "0", Types.Boolean))

            'WallBill:
            DefaultProperties.Add("faceplayer", New XmlProperty("faceplayer", "0", Types.Boolean))

            'SignBlock:
            DefaultProperties.Add("issign", New XmlProperty("issign", "0", Types.Boolean))
            DefaultProperties.Add("signtext", New XmlProperty("signtext", "", Types.String))
            DefaultProperties.Add("signcontenttype", New XmlProperty("signcontenttype", "0", Types.Integer))

            'WarpBlock:
            DefaultProperties.Add("iswarp", New XmlProperty("iswarp", "0", Types.Boolean))
            DefaultProperties.Add("warpmap", New XmlProperty("warpmap", "", Types.String))
            DefaultProperties.Add("warpturns", New XmlProperty("warpturns", "0", Types.Integer))
            DefaultProperties.Add("warpposition", New XmlProperty("warpposition", "0,0,0", Types.Vector3))
            DefaultProperties.Add("validwarprotations", New XmlProperty("validwarprotations", "0,1,2,3", Types.IntegerList))

            'Floor:
            DefaultProperties.Add("isfloor", New XmlProperty("isfloor", "0", Types.Boolean))
            DefaultProperties.Add("hassnow", New XmlProperty("hassnow", "0", Types.Boolean))
            DefaultProperties.Add("hassand", New XmlProperty("hassand", "0", Types.Boolean))
            DefaultProperties.Add("isice", New XmlProperty("isice", "0", Types.Boolean))

            DefaultProperties.Add("isledge", New XmlProperty("isledge", "0", Types.Boolean))

            'CutTree:
            DefaultProperties.Add("iscuttree", New XmlProperty("iscuttree", "0", Types.Boolean))

            'Water:
            DefaultProperties.Add("iswater", New XmlProperty("iswater", "0", Types.Boolean))
            DefaultProperties.Add("animatewater", New XmlProperty("animatewater", "0", Types.Boolean))
            DefaultProperties.Add("wateranimationdata", New XmlProperty("wateranimationdata", "", Types.String))

            'Grass:
            DefaultProperties.Add("isgrass", New XmlProperty("isgrass", "0", Types.Boolean))
            DefaultProperties.Add("pokefile", New XmlProperty("pokefile", "", Types.String))

            'BerryPlant:
            DefaultProperties.Add("isberry", New XmlProperty("isberry", "0", Types.Boolean))
            DefaultProperties.Add("berryindex", New XmlProperty("berryindex", "0", Types.Integer))
            DefaultProperties.Add("berriesyield", New XmlProperty("berriesyield", "0", Types.Integer))
            DefaultProperties.Add("berrywatered", New XmlProperty("berrywatered", "0,0,0,0", Types.String))

            'LoamySoil:
            DefaultProperties.Add("isloamysoil", New XmlProperty("isloamysoil", "0", Types.Boolean))

            'ItemObject:
            DefaultProperties.Add("isloamysoil", New XmlProperty("isitem", "0", Types.Boolean))
            DefaultProperties.Add("isloamysoil", New XmlProperty("itemid", "0", Types.Integer))
            DefaultProperties.Add("isloamysoil", New XmlProperty("mapitemid", "0", Types.Integer))
            DefaultProperties.Add("isloamysoil", New XmlProperty("itemhidden", "0", Types.Boolean))

            'ScriptBlock:
            DefaultProperties.Add("isscripttrigger", New XmlProperty("isscripttrigger", "0", Types.Boolean))
            DefaultProperties.Add("scripttrigger", New XmlProperty("scripttrigger", "0", Types.Integer))
            DefaultProperties.Add("script", New XmlProperty("script", "", Types.String))
            DefaultProperties.Add("acceptedscriptrotations", New XmlProperty("acceptedscriptrotations", "0,1,2,3", Types.IntegerList))

            'TurningSign:
            DefaultProperties.Add("turnx", New XmlProperty("turnx", "0", Types.Boolean))
            DefaultProperties.Add("turny", New XmlProperty("turny", "0", Types.Boolean))
            DefaultProperties.Add("turnz", New XmlProperty("turnz", "0", Types.Boolean))
            DefaultProperties.Add("turnspeed", New XmlProperty("turnspeed", "0", Types.Single))
            DefaultProperties.Add("invertturn", New XmlProperty("invertturn", "0", Types.Boolean))

            'ApricornTree:
            DefaultProperties.Add("isapricorn", New XmlProperty("isapricorn", "0", Types.Boolean))
            DefaultProperties.Add("apricorncolor", New XmlProperty("apricorncolor", "white", Types.String))

            'HeadButtTree:
            DefaultProperties.Add("isheadbutttree", New XmlProperty("isheadbutttree", "0", Types.Boolean))

            'SmashRock:
            DefaultProperties.Add("canbesmashed", New XmlProperty("canbesmashed", "0", Types.Boolean))

            'StrengthMove:
            DefaultProperties.Add("canbemoved", New XmlProperty("canbemoved", "0", Types.Boolean))
            DefaultProperties.Add("isstrengthmove", New XmlProperty("isstrengthmove", "0", Types.Boolean))

            'Waterfall:
            DefaultProperties.Add("iswaterfall", New XmlProperty("iswaterfall", "0", Types.Boolean))
            DefaultProperties.Add("animatewaterfall", New XmlProperty("animatewaterfall", "0", Types.Boolean))
            DefaultProperties.Add("waterfallanimationdata", New XmlProperty("waterfallanimationdata", "", Types.String))

            'Whirlpool:
            DefaultProperties.Add("iswhirlpool", New XmlProperty("iswhirlpool", "0", Types.Boolean))

            'StrengthTriggers:
            DefaultProperties.Add("isstrengthtrigger", New XmlProperty("isstrengthtrigger", "0", Types.Boolean))
            DefaultProperties.Add("strengthtriggerdata", New XmlProperty("strengthtriggerdata", "0,0,", Types.String))

            'SpinTiles:
            DefaultProperties.Add("isspintile", New XmlProperty("isspintile", "0", Types.Boolean))
            DefaultProperties.Add("spinrotation", New XmlProperty("spinrotation", "0", Types.Integer))
            DefaultProperties.Add("spintype", New XmlProperty("spintype", "0", Types.Integer))

            'Dive:
            DefaultProperties.Add("isdivetile", New XmlProperty("isdivetile", "0", Types.Boolean))
            DefaultProperties.Add("diveup", New XmlProperty("diveup", "0", Types.Boolean))

            'NPC:
            DefaultProperties.Add("npcname", New XmlProperty("npcname", "", Types.String))
            DefaultProperties.Add("npcid", New XmlProperty("npcid", "-1", Types.Integer))
            DefaultProperties.Add("npctexture", New XmlProperty("npctexture", "0", Types.String))
            DefaultProperties.Add("istrainer", New XmlProperty("istrainer", "0", Types.Boolean))
            DefaultProperties.Add("npcsightdistance", New XmlProperty("npcsightdistance", "0", Types.Integer))
            DefaultProperties.Add("npcscript", New XmlProperty("npcscript", "", Types.String))
        End Sub

        Public Enum Types
            [String]
            [Integer]
            [Boolean]
            [Vector3]
            [Vector2]
            [Rectangle]
            RectangleList
            IntegerList
            [Single]
        End Enum

        Private _name As String = ""
        Private _value As String = ""
        Private _type As Types = Types.String

        Public Sub New(ByVal Name As String, ByVal Value As String, ByVal Type As Types)
            Me._name = Name
            Me._value = Value
            Me._type = Type
        End Sub

        Public Property Value() As String
            Get
                Return Me._value
            End Get
            Set(value As String)
                Me._value = value
            End Set
        End Property

        Public ReadOnly Property Name() As String
            Get
                Return Me._name
            End Get
        End Property

        Public ReadOnly Property Type() As Types
            Get
                Return Me._type
            End Get
        End Property

        Public Shared Function ConvertFromString(ByVal cProperty As XmlProperty) As Object
            If cProperty Is Nothing Then
                Return Nothing
            End If

            Dim v As String = cProperty.Value

            Select Case cProperty.Type
                Case Types.String
                    Return v
                Case Types.Integer
                    v = v.Replace(".", GameController.DecSeparator)

                    If IsNumeric(v) = True Then
                        Return CInt(v)
                    Else
                        If v.ToLower() = "true" Then
                            Return CInt(1)
                        End If
                        If v.ToLower() = "false" Then
                            Return CInt(0)
                        End If
                    End If

                    Return CInt(-1)
                Case Types.Boolean
                    Select Case v.ToLower()
                        Case "0"
                            Return False
                        Case "false"
                            Return False
                        Case "1"
                            Return True
                        Case "true"
                            Return True
                    End Select

                    Return False
                Case Types.Vector3
                    v = v.Replace(".", GameController.DecSeparator)

                    If IsNumeric(v.Replace(".", GameController.DecSeparator)) = True Then
                        Dim vN As Single = CSng(v.Replace(".", GameController.DecSeparator))
                        Return New Vector3(vN)
                    Else
                        Dim parts() As String = v.Split(CChar(","))
                        If parts.ToList().IsNumericList() = True Then
                            If parts.Length = 2 Then
                                Return New Vector3(CSng(parts(0).Replace(".", GameController.DecSeparator)), 0.0F, CSng(parts(1).Replace(".", GameController.DecSeparator)))
                            ElseIf parts.Length >= 3 Then
                                Return New Vector3(CSng(parts(0).Replace(".", GameController.DecSeparator)), CSng(parts(1).Replace(".", GameController.DecSeparator)), CSng(parts(2).Replace(".", GameController.DecSeparator)))
                            End If
                        End If
                    End If

                    Return Vector3.Zero
                Case Types.Vector2
                    v = v.Replace(".", GameController.DecSeparator)

                    If IsNumeric(v.Replace(".", GameController.DecSeparator)) = True Then
                        Dim vN As Single = CSng(v.Replace(".", GameController.DecSeparator))
                        Return New Vector2(vN)
                    Else
                        Dim parts() As String = v.Split(CChar(","))
                        If parts.ToList().IsNumericList() = True Then
                            If parts.Length >= 2 Then
                                Return New Vector2(CSng(parts(0).Replace(".", GameController.DecSeparator)), CSng(parts(1).Replace(".", GameController.DecSeparator)))
                            End If
                        End If
                    End If

                    Return Vector2.Zero
                Case Types.Rectangle
                    '[x,y,width,height]

                    If v.StartsWith("[") = True And v.EndsWith("]") = True Then
                        v = v.Remove(v.Length - 1, 1).Remove(0, 1)
                        Dim parts() As String = v.Split(CChar(","))
                        If parts.Length = 4 Then
                            If parts.ToList().IsNumericList() = True Then
                                Return New Rectangle(CInt(parts(0).Replace(".", GameController.DecSeparator)),
                                                     CInt(parts(1).Replace(".", GameController.DecSeparator)),
                                                     CInt(parts(2).Replace(".", GameController.DecSeparator)),
                                                     CInt(parts(3).Replace(".", GameController.DecSeparator)))
                            End If
                        End If
                    End If

                    v = v.Replace(".", GameController.DecSeparator)

                    Return New Rectangle(0, 0, 1, 1)
                Case Types.RectangleList
                    '[x,y,width,height][x,y,width,height]..
                    Dim l As New List(Of Rectangle)

                    v = v.Remove(v.Length - 1, 1).Remove(0, 1)
                    v = v.Replace("][", "|")

                    Dim rectangles() As String = v.Split(CChar("|"))

                    For Each r As String In rectangles
                        Dim parts() As String = r.Split(CChar(","))

                        l.Add(New Rectangle(CInt(parts(0).Replace(".", GameController.DecSeparator)),
                                                     CInt(parts(1).Replace(".", GameController.DecSeparator)),
                                                     CInt(parts(2).Replace(".", GameController.DecSeparator)),
                                                     CInt(parts(3).Replace(".", GameController.DecSeparator))))
                    Next

                    Return l
                Case Types.IntegerList
                    Dim l As New List(Of Integer)

                    Dim parts() As String = v.Split(CChar(","))
                    For Each i As String In parts
                        i = i.Replace(".", GameController.DecSeparator)
                        If IsNumeric(i) = True Then
                            l.Add(CInt(i))
                        End If
                    Next

                    Return l
                Case Types.Single
                    v = v.Replace(".", GameController.DecSeparator)
                    If IsNumeric(v) = True Then
                        Return CSng(v)
                    End If

                    Return CSng(-1)
            End Select

            Return Nothing
        End Function

        Public Shared Function ConvertToString(Of T)(ByVal Value As T) As String
            Select Case Value.GetType()
                Case GetType(String)
                    Return CType(CObj(Value), String)
                Case GetType(Integer)
                    Dim i As Integer = CType(CObj(Value), Integer)
                    Return i.ToString()
                Case GetType(Boolean)
                    Dim b As Boolean = CType(CObj(Value), Boolean)
                    If b = True Then
                        Return "1"
                    Else
                        Return "0"
                    End If
                Case GetType(Vector3)
                    Dim v As Vector3 = CType(CObj(Value), Vector3)

                    Return v.X.ToString() & "," & v.Y.ToString() & "," & v.Z.ToString()
                Case GetType(Vector2)
                    Dim v As Vector2 = CType(CObj(Value), Vector2)

                    Return v.X.ToString() & "," & v.Y.ToString()
                Case GetType(Rectangle)
                    Dim r As Rectangle = CType(CObj(Value), Rectangle)

                    Return "[" & r.X & "," & r.Y & "," & r.Width & "," & r.Height & "]"
                Case GetType(Single)
                    Dim s As Single = CType(CObj(Value), Single)

                    Return s.ToString()
                Case GetType(List(Of Rectangle))
                    Dim l As List(Of Rectangle) = CType(CObj(Value), List(Of Rectangle))

                    Dim s As String = ""
                    For Each r As Rectangle In l
                        s &= "[" & r.X & "," & r.Y & "," & r.Width & "," & r.Height & "]"
                    Next

                    Return s
                Case GetType(List(Of Integer))
                    Dim l As List(Of Integer) = CType(CObj(Value), List(Of Integer))

                    Dim s As String = ""
                    For Each i As Integer In l
                        If s <> "" Then
                            s &= ","
                        End If
                        s &= i.ToString()
                    Next

                    Return s
            End Select

            Return ""
        End Function

    End Class

End Namespace