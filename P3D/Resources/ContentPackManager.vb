Public Class ContentPackManager

    Private Shared TextureReplacements As New Dictionary(Of TextureSource, TextureSource)
    Private Shared FilesExist As New Dictionary(Of String, Boolean)
    Private Shared TextureResolutions As New Dictionary(Of String, Single)
    Public Shared ScriptTextureReplacements As New Dictionary(Of TextureSource, TextureSource)

    Public Shared Sub Load(ByVal ContentPackFile As String, Optional IsScriptContent As Boolean = False)
        If System.IO.Directory.Exists(GameController.GamePath & "\ContentPacks") = True Then
            If System.IO.File.Exists(ContentPackFile) = True Then
                Dim Lines() As String = System.IO.File.ReadAllLines(ContentPackFile)
                For Each Line As String In Lines
                    Select Case Line.GetSplit(0, "|").ToLower
                        Case "waterspeed"
                            If IsScriptContent = False Then
                                GameModeManager.ForceWaterSpeed = CInt(Line.GetSplit(1, "|"))
                            End If
                        Case "pokemodelscale"
                            If IsScriptContent = False Then
                                GameModeManager.PokeModelScale = CSng(Line.GetSplit(1, "|").Replace(".", GameController.DecSeparator))
                            End If
                        Case "pokemodelrotation"
                            If IsScriptContent = False Then
                                GameModeManager.PokeModelRotation = New Vector3(CSng(Line.GetSplit(1, "|").GetSplit(0).Replace(".", GameController.DecSeparator)), 0, CSng(Line.GetSplit(1, "|").GetSplit(1).Replace(".", GameController.DecSeparator)))
                            End If
                        Case Else
                            Select Case Line.CountSplits("|")
                                Case 2 'ResolutionChange
                                    Dim TextureName As String = ScriptVersion2.ScriptCommander.Parse(Line.GetSplit(0, "|")).ToString
                                    Dim Resolution As Single = CSng(Line.GetSplit(1, "|").Replace(".", GameController.DecSeparator))

                                    If IsScriptContent = False Then
                                        If TextureResolutions.ContainsKey(TextureName) = False Then
                                            TextureResolutions.Add(TextureName, Resolution)
                                        End If
                                    End If

                                Case 4 'TextureReplacement
                                    Dim oldTextureName As String = ScriptVersion2.ScriptCommander.Parse(Line.GetSplit(0, "|")).ToString
                                    Dim newTextureName As String = ScriptVersion2.ScriptCommander.Parse(Line.GetSplit(2, "|")).ToString
                                    Dim oRS As String = Line.GetSplit(1, "|") 'oRS = oldRectangleSource
                                    Dim nRS As String = Line.GetSplit(3, "|") 'nRS = newRectangleSource

                                    Dim oldTextureSource As New TextureSource(oldTextureName, New Rectangle(CInt(oRS.GetSplit(0)), CInt(oRS.GetSplit(1)), CInt(oRS.GetSplit(2)), CInt(oRS.GetSplit(3))))
                                    Dim newTextureSource As New TextureSource(newTextureName, New Rectangle(CInt(nRS.GetSplit(0)), CInt(nRS.GetSplit(1)), CInt(nRS.GetSplit(2)), CInt(nRS.GetSplit(3))))

                                    If IsScriptContent = False Then
                                        If TextureReplacements.ContainsKey(oldTextureSource) = False Then
                                            TextureReplacements.Add(oldTextureSource, newTextureSource)
                                        End If
                                    Else
                                        If ScriptTextureReplacements.ContainsKey(oldTextureSource) = False Then
                                            ScriptTextureReplacements.Add(oldTextureSource, newTextureSource)
                                        End If
                                    End If
                            End Select
                    End Select
                Next
            End If
        End If
    End Sub

    Public Shared Function GetTextureReplacement(ByVal TexturePath As String, ByVal r As Rectangle) As TextureSource
        Dim TextureSource As New TextureSource(TexturePath.ToLower, r)
        If ScriptTextureReplacements.Count > 0 = True Then
            For i = 0 To ScriptTextureReplacements.Count - 1
                If ScriptTextureReplacements.Keys(i).IsEqual(TextureSource) = True Then
                    If TextureReplacements.Count > 0 Then
                        For j = 0 To TextureReplacements.Count - 1
                            If TextureReplacements.Keys(j).IsEqual(ScriptTextureReplacements.Values(i)) = True Then
                                Return TextureReplacements.Values(j)
                            End If
                        Next
                    End If
                    Return ScriptTextureReplacements.Values(i)
                End If
            Next
        End If
        If TextureReplacements.Count > 0 Then
            For i = 0 To TextureReplacements.Count - 1
                If TextureReplacements.Keys(i).IsEqual(TextureSource) = True Then
                    Return TextureReplacements.Values(i)
                End If
            Next
        End If
        Return TextureSource
    End Function

    Public Shared Function GetTextureResolution(ByVal TextureName As String) As Single
        For i = 0 To TextureResolutions.Count - 1
            If TextureResolutions.Keys(i).ToLower() = TextureName.ToLower() Then
                Return TextureResolutions.Values(i)
            End If
        Next

        Return 1
    End Function

    Public Shared Function GetContentManager(ByVal file As String, ByVal fileEndings As String) As ContentManager
        Dim contentPath As String = ""
        If Core.GameOptions.ContentPackNames.Count() > 0 Then
            For Each c As String In Core.GameOptions.ContentPackNames
                contentPath = "ContentPacks\" & c

                For Each fileEnding As String In fileEndings.Split(CChar(","))
                    If FilesExist.ContainsKey(GameController.GamePath & "\" & contentPath & "\" & file & fileEnding) = False Then
                        FilesExist.Add(GameController.GamePath & "\" & contentPath & "\" & file & fileEnding, System.IO.File.Exists(GameController.GamePath & "\" & contentPath & "\" & file & fileEnding))
                    End If

                    If FilesExist(GameController.GamePath & "\" & contentPath & "\" & file & fileEnding) = True Then
                        Return New ContentManager(Core.GameInstance.Services, contentPath)
                    End If
                Next
            Next
        End If

        Dim gameMode As GameMode = GameModeManager.ActiveGameMode
        If gameMode.ContentPath <> "\Content\" And gameMode.ContentPath <> "" Then
            Dim gameModePath As String = GameController.GamePath & gameMode.ContentPath
            For Each fileEnding As String In fileEndings.Split(CChar(","))
                If System.IO.File.Exists(gameModePath & file & fileEnding) = True Then
                    Dim RootDirectory As String = gameMode.ContentPath.Remove(0, 1)
                    If RootDirectory.EndsWith("\") = True Then
                        RootDirectory = RootDirectory.Remove(RootDirectory.Length - 1, 1)
                    End If
                    Return New ContentManager(Core.GameInstance.Services, gameMode.ContentPath.Remove(0, 1))
                End If
            Next
        End If

        Return New ContentManager(Core.GameInstance.Services, "Content")
    End Function

    Public Shared Sub CreateContentPackFolder()
        If System.IO.Directory.Exists(GameController.GamePath & "\ContentPacks") = False Then
            System.IO.Directory.CreateDirectory(GameController.GamePath & "\ContentPacks")
        End If
    End Sub

    Public Shared Function GetContentPackInfo(ByVal ContentPackName As String) As String()
        If System.IO.File.Exists(GameController.GamePath & "\ContentPacks\" & ContentPackName & "\info.dat") = False Then
            Dim s As String = "1.00" & Environment.NewLine & "Pokémon3D" & Environment.NewLine & "[Add information here!]"
            System.IO.File.WriteAllText(GameController.GamePath & "\ContentPacks\" & ContentPackName & "\info.dat", s)
        End If
        Return System.IO.File.ReadAllLines(GameController.GamePath & "\ContentPacks\" & ContentPackName & "\info.dat")
    End Function

    Public Shared Sub Clear()
        TextureReplacements.Clear()
        TextureResolutions.Clear()
        FilesExist.Clear()
        MusicManager.Clear()
        SoundManager.Clear()
        ModelManager.Clear()
        TextureManager.TextureList.Clear()
        TextureManager.TextureRectList.Clear()
        Water.ClearAnimationResources()
        Whirlpool.LoadedWaterTemp = False
        Waterfall.ClearAnimationResources()
        AnimatedBlock.ClearAnimationResources()

        Logger.Debug("---Cleared ContentPackManager---")
    End Sub

End Class

Public Class TextureSource

    Public TexturePath As String = ""
    Public TextureRectangle As Rectangle

    Public Sub New(ByVal TexturePath As String, ByVal TextureRectangle As Rectangle)
        Me.TexturePath = TexturePath
        Me.TextureRectangle = TextureRectangle
    End Sub

    Public Function GetString() As String
        Return TexturePath & "," & TextureRectangle.X & "," & TextureRectangle.Y & "," & TextureRectangle.Width & "," & TextureRectangle.Height
    End Function

    Public Function IsEqual(ByVal TextureSource As TextureSource) As Boolean
        If Me.TexturePath.ToLower() = TextureSource.TexturePath.ToLower() And Me.TextureRectangle = TextureSource.TextureRectangle Then
            Return True
        End If

        Return False
    End Function

    Public Function IsEqual(ByVal TexturePath As String, ByVal TextureRectangle As Rectangle) As Boolean
        Return IsEqual(New TextureSource(TexturePath, TextureRectangle))
    End Function

End Class