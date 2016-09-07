Public Class ContentPackManager

    Private Shared TextureReplacements As New Dictionary(Of TextureSource, TextureSource)
    Private Shared FilesExist As New Dictionary(Of String, Boolean)
    Private Shared TextureResolutions As New Dictionary(Of String, Integer)

    Public Shared Sub Load(ByVal ContentPackFile As String)
        If System.IO.Directory.Exists(GameController.GamePath & "\ContentPacks") = True Then
            If System.IO.File.Exists(ContentPackFile) = True Then
                Dim Lines() As String = System.IO.File.ReadAllLines(ContentPackFile)
                For Each Line As String In Lines
                    Select Case Line.CountSplits("|")
                        Case 2 'ResolutionChange
                            Dim TextureName As String = Line.GetSplit(0, "|")
                            Dim Resolution As Integer = CInt(Line.GetSplit(1, "|"))

                            If TextureResolutions.ContainsKey(TextureName) = False Then
                                TextureResolutions.Add(TextureName, Resolution)
                            End If
                        Case 4 'TextureReplacement
                            Dim oldTextureName As String = Line.GetSplit(0, "|")
                            Dim newTextureName As String = Line.GetSplit(2, "|")
                            Dim oRS As String = Line.GetSplit(1, "|") 'oRS = oldRectangleSource
                            Dim nRS As String = Line.GetSplit(3, "|") 'nRS = newRectangleSource

                            Dim oldTextureSource As New TextureSource(oldTextureName, New Rectangle(CInt(oRS.GetSplit(0)), CInt(oRS.GetSplit(1)), CInt(oRS.GetSplit(2)), CInt(oRS.GetSplit(3))))
                            Dim newTextureSource As New TextureSource(newTextureName, New Rectangle(CInt(nRS.GetSplit(0)), CInt(nRS.GetSplit(1)), CInt(nRS.GetSplit(2)), CInt(nRS.GetSplit(3))))

                            If TextureReplacements.ContainsKey(oldTextureSource) = False Then
                                TextureReplacements.Add(oldTextureSource, newTextureSource)
                            End If
                    End Select
                Next
            End If
        End If
    End Sub

    Public Shared Function GetTextureReplacement(ByVal TexturePath As String, ByVal r As Rectangle) As TextureSource
        Dim TextureSource As New TextureSource(TexturePath, r)
        For i = 0 To TextureReplacements.Count - 1
            If TextureReplacements.Keys(i).IsEqual(TextureSource) = True Then
                Return TextureReplacements.Values(i)
            End If
        Next
        Return TextureSource
    End Function

    Public Shared Function GetTextureResolution(ByVal TextureName As String) As Integer
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
            Dim gameModePath As String = GameController.GamePath & "\" & gameMode.ContentPath
            For Each fileEnding As String In fileEndings.Split(CChar(","))
                If System.IO.File.Exists(gameModePath & file & fileEnding) = True Then
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
            Dim s As String = "1.00" & vbNewLine & "Pokémon3D" & vbNewLine & "[Add information here!]"
            System.IO.File.WriteAllText(GameController.GamePath & "\ContentPacks\" & ContentPackName & "\info.dat", s)
        End If
        Return System.IO.File.ReadAllLines(GameController.GamePath & "\ContentPacks\" & ContentPackName & "\info.dat")
    End Function

    Public Shared Sub Clear()
        TextureReplacements.Clear()
        TextureResolutions.Clear()
        FilesExist.Clear()
        MusicManager.ReloadMusic()
        SoundManager.ReloadSounds()
        ModelManager.Clear()
        TextureManager.TextureList.Clear()
        Water.ClearAnimationResources()
        Whirlpool.LoadedWaterTemp = False
        Waterfall.ClearAnimationResources()

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
        If Me.TexturePath = TextureSource.TexturePath And Me.TextureRectangle = TextureSource.TextureRectangle Then
            Return True
        End If

        Return False
    End Function

    Public Function IsEqual(ByVal TexturePath As String, ByVal TextureRectangle As Rectangle) As Boolean
        Return IsEqual(New TextureSource(TexturePath, TextureRectangle))
    End Function

End Class