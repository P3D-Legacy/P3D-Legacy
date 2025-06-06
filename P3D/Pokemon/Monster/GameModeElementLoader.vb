﻿Namespace BattleSystem

    ''' <summary>
    ''' Provides an interface to load additional GameMode moves.
    ''' </summary>
    Public Class GameModeElementLoader

        'The default relative path to load moves from (Content folder).
        Const PATH As String = "Data\Types\"

        'List of loaded moves.
        Public Shared LoadedElements As New List(Of Element)

        ''' <summary>
        ''' Load the attack list for the loaded GameMode.
        ''' </summary>
        ''' <remarks>The game won't try to load the list if the default GameMode is selected.</remarks>
        Public Shared Sub Load()
            LoadedElements.Clear()

            If GameModeManager.ActiveGameMode.IsDefaultGamemode = False Then
                If System.IO.Directory.Exists(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & "\" & PATH) = True Then
                    For Each file As String In System.IO.Directory.GetFiles(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & PATH, "*.dat")
                        LoadElement(file)
                    Next
                End If
            End If
            If LoadedElements.Count > 0 Then
                For Each e As Element In LoadedElements
                    For Each id As Element In LoadedElements
                        If e.gmEffectivenessAttack.ContainsKey(id.Type) = False Then
                            e.gmEffectivenessAttack.Add(id.Type, 1.0F)
                        End If
                        If e.gmEffectivenessDefense.ContainsKey(id.Type) = False Then
                            e.gmEffectivenessDefense.Add(id.Type, 1.0F)
                        End If
                    Next
                Next

                Logger.Debug("Loaded " & LoadedElements.Count.ToString() & " GameMode type(s).")
            End If
        End Sub

        ''' <summary>
        ''' Loads a move from a file.
        ''' </summary>
        ''' <param name="file">The file to load the move from.</param>
        Private Shared Sub LoadElement(ByVal file As String)
            Dim element As New Element() 'Load a blank Element.
            element.IsGameModeElement = True

            Dim content() As String = System.IO.File.ReadAllLines(file)

            Dim key As String = ""
            Dim value As String = ""

            Dim setID As Boolean = False 'Controls if the move sets its ID.
            Dim nonCommentLines As Integer = 0

            Try
                'Go through lines of the file and set the properties depending on the content.
                'Lines starting with # are comments.
                For Each l As String In content
                    If l.Contains("|") = True And l.StartsWith("#") = False Then
                        nonCommentLines += 1
                        key = l.Remove(l.IndexOf("|"))
                        value = l.Remove(0, l.IndexOf("|") + 1)

                        Select Case key.ToLower()
                            Case "id"
                                element.Type = CInt(value)
                                setID = True
                            Case "name"
                                element.gmOriginalName = value
                            Case "typeimageoffset"
                                element.gmTypeRectangle = New Rectangle(CInt(value.GetSplit(0, ",")), CInt(value.GetSplit(1, ",")), 48, 16)
                            Case "itemtexturesource"
                                element.gmMachineTextureSource = value
                            Case "itemtextureoffset"
                                element.gmMachineTextureRectangle = New Rectangle(CInt(value.GetSplit(0, ",")), CInt(value.GetSplit(1, ",")), 24, 24)
                            Case "showindexfilters"
                                element.ShowInDexFilters = CBool(value)
                            Case "effectivenessattack"
                                Dim data() As String = value.Split(";")
                                For i = 0 To data.Count - 1
                                    Dim typeID As Integer = -1
                                    If StringHelper.IsNumeric(data(i).GetSplit(0, ",")) = True Then
                                        typeID = CInt(data(i).GetSplit(0, ","))
                                    Else
                                        Select Case data(i).GetSplit(0, ",").ToLower
                                            Case "normal"
                                                typeID = Element.Types.Normal
                                            Case "fighting"
                                                typeID = Element.Types.Fighting
                                            Case "flying"
                                                typeID = Element.Types.Flying
                                            Case "poison"
                                                typeID = Element.Types.Poison
                                            Case "ground"
                                                typeID = Element.Types.Ground
                                            Case "rock"
                                                typeID = Element.Types.Rock
                                            Case "bug"
                                                typeID = Element.Types.Bug
                                            Case "ghost"
                                                typeID = Element.Types.Ghost
                                            Case "steel"
                                                typeID = Element.Types.Steel
                                            Case "fire"
                                                typeID = Element.Types.Fire
                                            Case "water"
                                                typeID = Element.Types.Water
                                            Case "grass"
                                                typeID = Element.Types.Grass
                                            Case "electric"
                                                typeID = Element.Types.Electric
                                            Case "psychic"
                                                typeID = Element.Types.Psychic
                                            Case "ice"
                                                typeID = Element.Types.Ice
                                            Case "dragon"
                                                typeID = Element.Types.Dragon
                                            Case "dark"
                                                typeID = Element.Types.Dark
                                            Case "fairy"
                                                typeID = Element.Types.Fairy
                                            Case "shadow"
                                                typeID = Element.Types.Shadow
                                        End Select
                                    End If
                                    element.gmEffectivenessAttack.Add(typeID, CSng(data(i).GetSplit(1, ",").InsertDecSeparator))
                                Next
                            Case "effectivenessdefense"
                                Dim data() As String = value.Split(";")
                                For i = 0 To data.Count - 1
                                    Dim typeID As Integer = -1
                                    If StringHelper.IsNumeric(data(i).GetSplit(0, ",")) = True Then
                                        typeID = CInt(data(i).GetSplit(0, ","))
                                    Else
                                        Select Case data(i).GetSplit(0, ",").ToLower
                                            Case "normal"
                                                typeID = Element.Types.Normal
                                            Case "fighting"
                                                typeID = Element.Types.Fighting
                                            Case "flying"
                                                typeID = Element.Types.Flying
                                            Case "poison"
                                                typeID = Element.Types.Poison
                                            Case "ground"
                                                typeID = Element.Types.Ground
                                            Case "rock"
                                                typeID = Element.Types.Rock
                                            Case "bug"
                                                typeID = Element.Types.Bug
                                            Case "ghost"
                                                typeID = Element.Types.Ghost
                                            Case "steel"
                                                typeID = Element.Types.Steel
                                            Case "fire"
                                                typeID = Element.Types.Fire
                                            Case "water"
                                                typeID = Element.Types.Water
                                            Case "grass"
                                                typeID = Element.Types.Grass
                                            Case "electric"
                                                typeID = Element.Types.Electric
                                            Case "psychic"
                                                typeID = Element.Types.Psychic
                                            Case "ice"
                                                typeID = Element.Types.Ice
                                            Case "dragon"
                                                typeID = Element.Types.Dragon
                                            Case "dark"
                                                typeID = Element.Types.Dark
                                            Case "fairy"
                                                typeID = Element.Types.Fairy
                                            Case "shadow"
                                                typeID = Element.Types.Shadow
                                        End Select
                                    End If
                                    element.gmEffectivenessDefense.Add(typeID, CSng(data(i).GetSplit(1, ",").InsertDecSeparator))
                                Next
                        End Select
                    End If
                Next
            Catch ex As Exception
                'If an error occurs loading a move, log the error.
                Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeElementLoader.vb: Error loading GameMode element from file """ & file & """: " & ex.Message & "; Last Key/Value pair successfully loaded: " & key & "|" & value)
            End Try

            If nonCommentLines > 0 Then
                If setID = True Then
                    If element.Type >= 20 Then
                        For i = 0 To 18
                            If element.gmEffectivenessAttack.ContainsKey(i) = False Then
                                element.gmEffectivenessAttack.Add(i, 1.0F)
                            End If
                            If element.gmEffectivenessDefense.ContainsKey(i) = False Then
                                element.gmEffectivenessDefense.Add(i, 1.0F)
                            End If
                        Next
                        LoadedElements.Add(element) 'Add the element.
                    Else
                        Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeElementLoader.vb: User defined types are not allowed to have an ID of an already existing type or an ID below 20. The ID for the type loaded from """ & file & """ has the ID " & element.Type.ToString() & ", which is smaller than 20.")
                    End If
                Else
                    Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeElementLoader.vb: User defined types must set their ID through the ""ID"" property, however the type loaded from """ & file & """ has no ID set so it will be ignored.")
                End If
            Else
                Debug.Print("GameModeElementLoader.vb: The type loaded from """ & file & """ has no valid lines so it will be ignored.")
            End If
        End Sub

        ''' <summary>
        ''' Returns an element based on its ID.
        ''' </summary>
        ''' <param name="ID">The ID of the element.</param>
        ''' <returns>Returns an element or nothing.</returns>
        Public Shared Function GetElementByID(ByVal ID As Integer) As Element
            If ID <= 19 Then
                Return New Element(ID)
            Else
                For Each e As Element In LoadedElements
                    If e.Type = ID Then
                        Return e
                    End If
                Next
            End If
            Return Nothing
        End Function
        ''' <summary>
        ''' Returns an element based on its name.
        ''' </summary>
        ''' <param name="Name">The name of the element.</param>
        ''' <returns>Returns an element or nothing.</returns>
        Public Shared Function GetElementByName(ByVal Name As String) As Element
            Select Case Name.ToLower
                Case "normal"
                    Return New Element(Element.Types.Normal)
                Case "fighting"
                    Return New Element(Element.Types.Fighting)
                Case "flying"
                    Return New Element(Element.Types.Flying)
                Case "poison"
                    Return New Element(Element.Types.Poison)
                Case "ground"
                    Return New Element(Element.Types.Ground)
                Case "rock"
                    Return New Element(Element.Types.Rock)
                Case "bug"
                    Return New Element(Element.Types.Bug)
                Case "ghost"
                    Return New Element(Element.Types.Ghost)
                Case "steel"
                    Return New Element(Element.Types.Steel)
                Case "fire"
                    Return New Element(Element.Types.Fire)
                Case "water"
                    Return New Element(Element.Types.Water)
                Case "grass"
                    Return New Element(Element.Types.Grass)
                Case "electric"
                    Return New Element(Element.Types.Electric)
                Case "psychic"
                    Return New Element(Element.Types.Psychic)
                Case "ice"
                    Return New Element(Element.Types.Ice)
                Case "dragon"
                    Return New Element(Element.Types.Dragon)
                Case "dark"
                    Return New Element(Element.Types.Dark)
                Case "fairy"
                    Return New Element(Element.Types.Fairy)
                Case "shadow"
                    Return New Element(Element.Types.Shadow)
                Case "blank", ""
                    Return New Element(Element.Types.Blank)
                Case Else
                    For Each e As Element In LoadedElements
                        If e.gmOriginalName.ToLower = Name.ToLower Then
                            Return e
                        End If
                    Next
            End Select
            Return Nothing
        End Function

    End Class

End Namespace