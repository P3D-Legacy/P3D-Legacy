Namespace Construct.Framework.Classes

    <ScriptClass("Entity")>
    <ScriptDescription("A class to work with entities in a level.")>
    Public Class CL_Entity

        Inherits ScriptClass

        Public Sub New()
            MyBase.New(True)
        End Sub

#Region "Utils"

        Private Function GetEntities(ByVal argument As String) As IEnumerable(Of Entity)
            Dim entID As Integer = Int(argument.Split(","c)(0))
            Return (From ent As Entity In Screen.Level.Entities Select ent Where ent.ID = entID)
        End Function

        Private Function GetFirstEntity(ByVal argument As String) As Entity
            Dim l = GetEntities(argument)
            If l.Count = 0 Then
                Return Nothing
            Else
                Return l(0)
            End If
        End Function

#End Region

#Region "Commands"

        <ScriptCommand("ShowMessageBulb")>
        <ScriptDescription("Displays a Message Bulb entity.")>
        Private Function M_ShowMessageBulb(ByVal argument As String) As String
            If ActiveLine.workValues.Count = 0 Then
                ActiveLine.Preserve = True

                ActiveLine.workValues.Add("started")
                Dim Data() As String = argument.Split(CChar("|"))

                Dim newData As New List(Of String)
                For Each d As String In Data
                    newData.Add(d)
                Next
                Data = newData.ToArray()

                Dim ID As Integer = Int(Data(0))
                Dim Position As New Vector3(Sng(Data(1).Replace(".", GameController.DecSeparator)), Sng(Data(2).Replace(".", GameController.DecSeparator)), Sng(Data(3).Replace(".", GameController.DecSeparator)))

                Dim noType As MessageBulb.NotifcationTypes = MessageBulb.NotifcationTypes.Waiting
                Select Case ID
                    Case 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15
                        noType = CType(ID, MessageBulb.NotifcationTypes)
                    Case Else
                        noType = MessageBulb.NotifcationTypes.Exclamation
                End Select

                Screen.Level.Entities.Add(New MessageBulb(Position, noType))
            End If

            Dim contains As Boolean = False
            Screen.Level.Entities = (From e In Screen.Level.Entities Order By e.CameraDistance Descending).ToList()
            For Each e As Entity In Screen.Level.Entities
                If e.EntityID = "MessageBulb" Then
                    e.Update()
                    contains = True
                End If
            Next
            If contains = False Then
                ActiveLine.Preserve = False
            Else
                For i = 0 To Screen.Level.Entities.Count - 1
                    If i <= Screen.Level.Entities.Count - 1 Then
                        If Screen.Level.Entities(i).CanBeRemoved = True Then
                            Screen.Level.Entities.RemoveAt(i)
                            i -= 1
                        End If
                    Else
                        Exit For
                    End If
                Next
            End If

            Return Core.Null
        End Function

        <ScriptCommand("Warp")>
        <ScriptDescription("Warps an entity to another location.")>
        Private Function M_Warp(ByVal argument As String) As String
            For Each ent As Entity In GetEntities(argument)
                Dim PositionList As List(Of String) = argument.Split(CChar(",")).ToList()
                Dim newPosition As Vector3 = New Vector3(Sng(PositionList(1).Replace("~", CStr(ent.Position.X))),
                                                         Sng(PositionList(2).Replace("~", CStr(ent.Position.Y))),
                                                         Sng(PositionList(3).Replace("~", CStr(ent.Position.Z))))

                ent.Position = newPosition
                ent.CreatedWorld = False
            Next

            Return Core.Null
        End Function

        <ScriptCommand("SetScale")>
        <ScriptDescription("Sets the scale of an entity.")>
        Private Function M_SetScale(ByVal argument As String) As String
            For Each ent As Entity In GetEntities(argument)
                Dim ScaleList As List(Of String) = argument.Split(CChar(",")).ToList()
                Dim newScale As Vector3 = New Vector3(Sng(ScaleList(1).Replace("~", CStr(ent.Position.X)).Replace(".", GameController.DecSeparator)),
                                                      Sng(ScaleList(2).Replace("~", CStr(ent.Position.Y)).Replace(".", GameController.DecSeparator)),
                                                      Sng(ScaleList(3).Replace("~", CStr(ent.Position.Z)).Replace(".", GameController.DecSeparator)))

                ent.Scale = newScale
                ent.CreatedWorld = False
            Next

            Return Core.Null
        End Function

        <ScriptCommand("Remove")>
        <ScriptDescription("Removes an entity from the map.")>
        Private Function M_Remove(ByVal argument As String) As String
            For Each ent As Entity In GetEntities(argument)
                ent.CanBeRemoved = True
            Next

            Return Core.Null
        End Function

        <ScriptCommand("SetID")>
        <ScriptDescription("Sets the ID of an entity.")>
        Private Function M_SetID(ByVal argument As String) As String
            For Each ent As Entity In GetEntities(argument)
                ent.ID = Int(argument.Split(","c)(1))
            Next

            Return Core.Null
        End Function

        <ScriptCommand("SetOpacity")>
        <ScriptDescription("Sets the opacity of an entity.")>
        Private Function M_SetOpacity(ByVal argument As String) As String
            For Each ent As Entity In GetEntities(argument)
                ent.NormalOpacity = CSng(Int(argument.Split(","c)(1)) / 100)
            Next

            Return Core.Null
        End Function

        <ScriptCommand("SetVisible")>
        <ScriptDescription("Sets the visibility of an entity.")>
        Private Function M_SetVisible(ByVal argument As String) As String
            For Each ent As Entity In GetEntities(argument)
                ent.Visible = Bool(argument.Split(","c)(1))
            Next

            Return Core.Null
        End Function

        <ScriptCommand("SetAdditionalValue")>
        <ScriptDescription("Sets the additional value of an entity.")>
        Private Function M_SetAdditionalValue(ByVal argument As String) As String
            For Each ent As Entity In GetEntities(argument)
                ent.AdditionalValue = argument.Split(","c)(1)
            Next

            Return Core.Null
        End Function

        <ScriptCommand("SetCollision")>
        <ScriptDescription("Sets the collision behaviour of an entity.")>
        Private Function M_SetCollision(ByVal argument As String) As String
            For Each ent As Entity In GetEntities(argument)
                ent.Collision = Bool(argument.GetSplit(1))
            Next

            Return Core.Null
        End Function

        <ScriptCommand("SetTexture")>
        <ScriptDescription("Sets texture data of an entity.")>
        Private Function M_SetTexture(ByVal argument As String) As String
            For Each ent As Entity In GetEntities(argument)
                'Data structure for the entire argument: entID,textureID,[texturename,x,y,width,height]
                Dim textureID As Integer = Int(argument.GetSplit(1))

                Dim textureData As String = argument.Remove(0, argument.IndexOf("[") + 1)
                textureData = textureData.Remove(textureData.Length - 1, 1)

                Dim textureInformation() As String = textureData.Split(CChar(","))

                ent.Textures(textureID) = TextureManager.GetTexture(textureInformation(0), New Rectangle(Int(textureInformation(1)), Int(textureInformation(2)), Int(textureInformation(3)), Int(textureInformation(4))))
            Next

            Return Core.Null
        End Function

        <ScriptCommand("AddToPosition")>
        <ScriptDescription("Adds a vector3 to an entitie's position.")>
        Private Function M_AddToPosition(ByVal argument As String) As String
            For Each ent As Entity In GetEntities(argument)
                Dim PositionList As List(Of String) = argument.Split(CChar(",")).ToList()
                Dim newPosition As Vector3 = New Vector3(Sng(PositionList(1).Replace("~", CStr(ent.Position.X)).Replace(".", GameController.DecSeparator)),
                                                         Sng(PositionList(2).Replace("~", CStr(ent.Position.Y)).Replace(".", GameController.DecSeparator)),
                                                         Sng(PositionList(3).Replace("~", CStr(ent.Position.Z)).Replace(".", GameController.DecSeparator)))

                ent.Position += newPosition
                ent.CreatedWorld = False
            Next

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("Visible")>
        <ScriptDescription("Returs the Visible attribute of an entity.")>
        Private Function F_Visible(ByVal argument As String) As String
            Dim ent = GetFirstEntity(argument)

            If ent IsNot Nothing Then
                Return ToString(ent.Visible)
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Opacity")>
        <ScriptDescription("Returns the Opacity attribute of an entity.")>
        Private Function F_Opacity(ByVal argument As String) As String
            Dim ent = GetFirstEntity(argument)

            If ent IsNot Nothing Then
                Return ToString(ent.Opacity * 100)
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Position")>
        <ScriptDescription("Returns the Position attribute of an entity.")>
        Private Function F_Position(ByVal argument As String) As String
            Dim ent = GetFirstEntity(argument)

            If ent IsNot Nothing Then
                Return ToString(ent.Position.X) & "," &
                    ToString(ent.Position.Y) & "," &
                    ToString(ent.Position.Z)
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("PositionX")>
        <ScriptDescription("Returns the X attribute of an entity.")>
        Private Function F_PositionX(ByVal argument As String) As String
            Dim ent = GetFirstEntity(argument)

            If ent IsNot Nothing Then
                Return ToString(ent.Position.X)
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("PositionY")>
        <ScriptDescription("Returns the Y attribute of an entity.")>
        Private Function F_PositionY(ByVal argument As String) As String
            Dim ent = GetFirstEntity(argument)

            If ent IsNot Nothing Then
                Return ToString(ent.Position.Y)
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("PositionZ")>
        <ScriptDescription("Returns the Z attribute of an entity.")>
        Private Function F_PositionZ(ByVal argument As String) As String
            Dim ent = GetFirstEntity(argument)

            If ent IsNot Nothing Then
                Return ToString(ent.Position.Z)
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Scale")>
        <ScriptDescription("Returns the scale attribute of an entity.")>
        Private Function F_Scale(ByVal argument As String) As String
            Dim ent = GetFirstEntity(argument)

            If ent IsNot Nothing Then
                Return ToString(ent.Scale.X) & "," &
                    ToString(ent.Scale.Y) & "," &
                    ToString(ent.Scale.Z)
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("AdditionalValue")>
        <ScriptDescription("Returns the Additional value of an entity.")>
        Private Function F_AdditionalValue(ByVal argument As String) As String
            Dim ent = GetFirstEntity(argument)

            If ent IsNot Nothing Then
                Return ent.AdditionalValue
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Collision")>
        <ScriptDescription("Returns the Collision value of an entity.")>
        Private Function F_Collision(ByVal argument As String) As String
            Dim ent = GetFirstEntity(argument)

            If ent IsNot Nothing Then
                Return ToString(ent.Collision)
            Else
                Return Core.Null
            End If
        End Function

#End Region

    End Class

End Namespace