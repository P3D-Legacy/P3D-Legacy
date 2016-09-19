Public Class FontManager

    Private Shared FontList As New Dictionary(Of String, FontContainer)

    'we can maybe put language specific fonts in via localization system, we have a place to start at least

    'this sub looks for all fonts that should be in the system (base files, mode files, pack files) and shoves them into FontList
    Public Shared Sub LoadFonts()
        FontList.Clear()
        'because the pack manager will hoover up replacements from packs and mods already we just load every font name contained in our base files
        For Each s As String In System.IO.Directory.GetFiles(GameController.GamePath & "\Content\Fonts\BMP")
            If s.EndsWith(".xnb") = True Then
                Dim name As String = s.Substring(0, s.Length - 4)
                name = name.Substring(GameController.GamePath.Length + "\Content\Fonts\BMP\".Length)
                If FontList.ContainsKey(name.ToLower()) = False Then
                    Dim font As SpriteFont = ContentPackManager.GetContentManager("Fonts\BMP\" & name, ".xnb").Load(Of SpriteFont)("Fonts\BMP\" & name)
                    FontList.Add(name.ToLower(), New FontContainer(name, font))
                End If
            End If
        Next
        'then look for ADDITIONAL fonts in packs, the ones that exist will have the user's prefered copy already
        For Each c As String In Core.GameOptions.ContentPackNames
            If System.IO.Directory.Exists(GameController.GamePath & "\ContentPacks\" & c & "\Content\Fonts\BMP") = True Then
                For Each s As String In System.IO.Directory.GetFiles(GameController.GamePath & "\ContentPacks\" & c & "\Content\Fonts\BMP")
                    If s.EndsWith(".xnb") = True Then
                        Dim name As String = s.Substring(0, s.Length - 4)
                        name = name.Substring(GameController.GamePath.Length + "\Content\Fonts\BMP\".Length + "\ContentPacks\".Length + c.Length)
                        If FontList.ContainsKey(name.ToLower()) = False Then
                            Dim font As SpriteFont = ContentPackManager.GetContentManager("Fonts\BMP\" & name, ".xnb").Load(Of SpriteFont)("Fonts\BMP\" & name)
                            FontList.Add(name.ToLower(), New FontContainer(name, font))
                        End If
                    End If
                Next
            End If
        Next
        'if there's a game mode loaded, look in that too for additional fonts
        If Not GameModeManager.ActiveGameMode.Name = "Kolben" Then
            If Not GameModeManager.ActiveGameMode.ContentPath = "\Content\" Then
                If System.IO.Directory.Exists(GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "\Fonts\BMP") = True Then
                    For Each s As String In System.IO.Directory.GetFiles(GameController.GamePath & GameModeManager.ActiveGameMode.ContentPath & "\Fonts\BMP")
                        If s.EndsWith(".xnb") = True Then
                            Dim name As String = s.Substring(0, s.Length - 4)
                            name = name.Substring(GameController.GamePath.Length + "\Fonts\BMP\".Length + GameModeManager.ActiveGameMode.ContentPath.Length)
                            If FontList.ContainsKey(name.ToLower()) = False Then
                                Dim font As SpriteFont = ContentPackManager.GetContentManager("Fonts\BMP\" & name, ".xnb").Load(Of SpriteFont)("Fonts\BMP\" & name)
                                FontList.Add(name.ToLower(), New FontContainer(name, font))
                            End If
                        End If
                    Next
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Looks to see if a font is loaded. Code that uses this should generally check for a return of nothing, indicating the font does not exist.
    ''' </summary>
    ''' <param name="fontName">The name of the font.</param>
    Public Shared Function GetFont(ByVal fontName As String) As SpriteFont
        If FontList.ContainsKey(fontname.ToLower()) = True Then
            Return FontList(fontname.ToLower()).SpriteFont
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Looks to see if a FontContainer is loaded. Code that uses this should generally check for a return of nothing, indicating the font does not exist.
    ''' </summary>
    ''' <param name="fontName">The name of the font.</param>
    Public Shared Function GetFontContainer(ByVal fontName As String) As FontContainer
        If FontList.ContainsKey(fontName.ToLower()) = True Then
            Return FontList(fontName.ToLower())
        End If
        Return Nothing
    End Function

    Public Shared ReadOnly Property MainFont() As SpriteFont
        Get
            Return GetFont("mainfont")
        End Get
    End Property

    Public Shared ReadOnly Property TextFont() As SpriteFont
        Get
            Return GetFont("textfont")
        End Get
    End Property

    Public Shared ReadOnly Property InGameFont() As SpriteFont
        Get
            Return GetFont("ingame")
        End Get
    End Property

    Public Shared ReadOnly Property MiniFont() As SpriteFont
        Get
            Return GetFont("minifont")
        End Get
    End Property

    Public Shared ReadOnly Property ChatFont() As SpriteFont
        Get
            Return GetFont("chatfont")
        End Get
    End Property

    Public Shared ReadOnly Property UnownFont() As SpriteFont
        Get
            Return GetFont("unown")
        End Get
    End Property

    Public Shared ReadOnly Property BrailleFont() As SpriteFont
        Get
            Return GetFont("braille")
        End Get
    End Property

End Class
