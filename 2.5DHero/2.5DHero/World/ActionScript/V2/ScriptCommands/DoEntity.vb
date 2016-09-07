Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @entity commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoEntity(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "showmessagebulb"
                    If ScriptV2.started = False Then
                        ScriptV2.started = True
                        Dim Data() As String = argument.Split(CChar("|"))

                        Dim newData As New List(Of String)
                        For Each d As String In Data
                            newData.Add(d)
                        Next
                        Data = newData.ToArray()

                        Dim ID As Integer = int(Data(0))
                        Dim Position As New Vector3(sng(Data(1).Replace(".", GameController.DecSeparator)), sng(Data(2).Replace(".", GameController.DecSeparator)), sng(Data(3).Replace(".", GameController.DecSeparator)))

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
                        IsReady = True
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
                Case Else
                    Dim entID As Integer = int(argument.GetSplit(0))
                    Dim ents = (From ent As Entity In Screen.Level.Entities Select ent Where ent.ID = entID)

                    For Each ent As Entity In ents
                        Select Case command.ToLower()
                            Case "warp"
                                Dim PositionList As List(Of String) = argument.Split(CChar(",")).ToList()
                                Dim newPosition As Vector3 = New Vector3(sng(PositionList(1).Replace("~", CStr(ent.Position.X)).Replace(".", GameController.DecSeparator)),
                                                                         sng(PositionList(2).Replace("~", CStr(ent.Position.Y)).Replace(".", GameController.DecSeparator)),
                                                                         sng(PositionList(3).Replace("~", CStr(ent.Position.Z)).Replace(".", GameController.DecSeparator)))

                                ent.Position = newPosition
                                ent.CreatedWorld = False
                            Case "setscale"
                                Dim ScaleList As List(Of String) = argument.Split(CChar(",")).ToList()
                                Dim newScale As Vector3 = New Vector3(sng(ScaleList(1).Replace("~", CStr(ent.Position.X)).Replace(".", GameController.DecSeparator)),
                                                                      sng(ScaleList(2).Replace("~", CStr(ent.Position.Y)).Replace(".", GameController.DecSeparator)),
                                                                      sng(ScaleList(3).Replace("~", CStr(ent.Position.Z)).Replace(".", GameController.DecSeparator)))

                                ent.Scale = newScale
                                ent.CreatedWorld = False
                            Case "remove"
                                ent.CanBeRemoved = True
                            Case "setid"
                                ent.ID = int(argument.GetSplit(1))
                            Case "setopacity"
                                ent.NormalOpacity = sng(int(argument.GetSplit(1)) / 100)
                            Case "setvisible"
                                ent.Visible = CBool(argument.GetSplit(1))
                            Case "setadditionalvalue"
                                ent.AdditionalValue = argument.GetSplit(1)
                            Case "setcollision"
                                ent.Collision = CBool(argument.GetSplit(1))
                            Case "settexture"
                                'Data structure for the entire argument: entID,textureID,[texturename,x,y,width,height]
                                Dim textureID As Integer = int(argument.GetSplit(1))

                                Dim textureData As String = argument.Remove(0, argument.IndexOf("[") + 1)
                                textureData = textureData.Remove(textureData.Length - 1, 1)

                                Dim textureInformation() As String = textureData.Split(CChar(","))

                                ent.Textures(textureID) = TextureManager.GetTexture(textureInformation(0), New Rectangle(int(textureInformation(1)), int(textureInformation(2)), int(textureInformation(3)), int(textureInformation(4))))
                            Case "addtoposition"
                                Dim PositionList As List(Of String) = argument.Split(CChar(",")).ToList()
                                Dim newPosition As Vector3 = New Vector3(sng(PositionList(1).Replace("~", CStr(ent.Position.X)).Replace(".", GameController.DecSeparator)),
                                                                         sng(PositionList(2).Replace("~", CStr(ent.Position.Y)).Replace(".", GameController.DecSeparator)),
                                                                         sng(PositionList(3).Replace("~", CStr(ent.Position.Z)).Replace(".", GameController.DecSeparator)))

                                ent.Position += newPosition
                                ent.CreatedWorld = False

                                CanContinue = False
                        End Select
                    Next
                    IsReady = True
            End Select
        End Sub

    End Class

End Namespace