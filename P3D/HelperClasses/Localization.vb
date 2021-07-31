Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

''' <summary>
''' TODO:
''' - Test and check if all screens has been passed thru this class
''' - Do a QA build for the QA team and check over everything before translation team gets the files
''' - Future todo: Pass dialog thru the localization class
''' </summary>

Public Class Localization
    Public Shared CurrentLanguage As String = "en"
    Public Shared LocalizationTokens As Dictionary(Of String, Token) = New Dictionary(Of String, Token)

    Public Shared Sub Load(ByVal CurrentLanguage As String)
        LocalizationTokens.Clear()

        Localization.CurrentLanguage = CurrentLanguage

        Logger.Debug("Loaded language [" & CurrentLanguage & "]")

        LoadTokenFile(GameMode.DefaultLocalizationsPath, False)

        If GameModeManager.GameModeCount > 0 Then
            Dim GameModeLocalizationPath As String = GameModeManager.ActiveGameMode.LocalizationsPath
            If GameModeLocalizationPath <> GameMode.DefaultLocalizationsPath Then
                LoadTokenFile(GameModeLocalizationPath, True)
            End If
        End If
    End Sub

    Public Shared Sub ReloadGameModeTokens()
        For i = 0 To LocalizationTokens.Count - 1
            If i <= LocalizationTokens.Count - 1 Then
                Dim Token As Token = LocalizationTokens.Values(i)

                If Token.IsGameModeToken = True Then
                    LocalizationTokens.Remove(LocalizationTokens.Keys(i))
                    i -= 1
                End If
            End If
        Next

        If GameModeManager.GameModeCount > 0 Then
            Dim GameModeLocalizationPath As String = GameModeManager.ActiveGameMode.LocalizationsPath
            If GameModeLocalizationPath <> GameMode.DefaultLocalizationsPath Then
                LoadTokenFile(GameModeLocalizationPath, True)
            End If
        End If

        Logger.Debug("---Reloaded GameMode Tokens---")
    End Sub

    Private Shared Sub LoadTokenFile(ByVal path As String, ByVal IsGameModeFile As Boolean)
        Dim FullPath As String = GameController.GamePath & path
        Dim LocaleFilePath As String = FullPath & CurrentLanguage & ".json"
        Dim DefaultLanguagePath As String = FullPath & "en.json"

        Logger.Debug("Localization.vb: LocaleFilePath: " & LocaleFilePath)

        If Directory.GetFiles(FullPath).Count > 0 Then
            If File.Exists(LocaleFilePath) = False Then
                Logger.Debug("Localization.vb: Could not find file for: " & CurrentLanguage)
                CurrentLanguage = "en"
            End If

            If File.Exists(LocaleFilePath) = True Then
                Logger.Debug("Localization.vb: Found file for suffix: " & CurrentLanguage)
                Dim TokensFile As JObject = JObject.Parse(File.ReadAllText(LocaleFilePath))
                Dim SelectedLanguage As String = TokensFile.SelectToken("language_name").ToString
                Logger.Debug("Localization.vb: Loaded Language file and its name is: " & SelectedLanguage)
                For Each tokens In TokensFile.SelectToken("p3d").Values
                    ParseTokens(tokens, SelectedLanguage, IsGameModeFile)
                Next
            End If

            If CurrentLanguage IsNot "en" Then
                If File.Exists(DefaultLanguagePath) Then
                    Dim FallbackTokensFile As JObject = JObject.Parse(File.ReadAllText(DefaultLanguagePath))
                    Dim SelectedLanguage As String = FallbackTokensFile.SelectToken("language_name").ToString
                    Logger.Debug("Localization.vb: Loaded Fallback Language file and its name is: " & SelectedLanguage)
                    For Each tokens In FallbackTokensFile.SelectToken("p3d").Values
                        ParseTokens(tokens, SelectedLanguage, IsGameModeFile)
                    Next
                End If
            End If
        End If
    End Sub
    Private Shared Sub ParseTokens(ByVal token As JToken, ByVal SelectedLanguage As String, ByVal IsGameModeFile As Boolean)
        If token.HasValues = True Then
            For Each child In token.Values
                ParseTokens(child, SelectedLanguage, IsGameModeFile)
            Next
        Else
            Dim key As String = token.Path.ToLower().Replace("p3d.", "")
            Dim value As String = token.Parent.First.ToString
            If LocalizationTokens.ContainsKey(key) = False Then
                LocalizationTokens.Add(key, New Token(value, SelectedLanguage, IsGameModeFile))
            End If
        End If
    End Sub
    Public Shared Function Translate(ByVal tokenInput As String, Optional ByVal type As Type = Nothing, Optional ByVal DefaultValue As String = "") As String
        Dim resultToken As Token = Nothing
        Dim CurrentScreen As String = GetCurrentScreen()
        tokenInput = tokenInput.ToLower().Replace(" ", "_").Replace("'", "").Replace("p3d.", "").Replace("._", "_") ' Lets format the string before finding it
        Dim prefix As String = ""
        If type Is Nothing Then
            prefix = CurrentScreen
        Else
            prefix = type.Name.ToString.ToLower
        End If
        Dim NewTokenName As String = prefix & "." & tokenInput
        If tokenInput.Contains(".") = False Then
            'Logger.Debug("Localization.vb: tokenInput.Contains: " & tokenInput)
            tokenInput = NewTokenName
        End If
        If LocalizationTokens.ContainsKey(tokenInput) = True Then
            If LocalizationTokens.TryGetValue(tokenInput, resultToken) = False Then
                Return tokenInput
            Else
                Dim result As String = resultToken.TokenContent
                If Core.Player IsNot Nothing Then
                    result = result.Replace("<playername>", Core.Player.Name)
                    result = result.Replace("<rivalname>", Core.Player.RivalName)
                End If
                Return result
            End If
        Else
            Dim FullPath As String = GameController.GamePath & GameMode.DefaultLocalizationsPath
            Dim LocaleFilePath As String = FullPath & "missing_tokens.json"
            Dim TokensFile As JObject = JObject.Parse(File.ReadAllText(LocaleFilePath))
            If TokensFile.ContainsKey(tokenInput) = False Then
                TokensFile.Add(tokenInput, tokenInput)
            End If
            File.WriteAllText(LocaleFilePath, JsonConvert.SerializeObject(TokensFile, Formatting.Indented))

            If DefaultValue = "" Then
                Return tokenInput
            Else
                Return DefaultValue
            End If
        End If
    End Function

    Public Shared Function TokenExists(ByVal TokenName As String) As Boolean
        Return LocalizationTokens.ContainsKey(TokenName)
    End Function

    Public Shared Function GetCurrentScreen() As String
        Dim CurrentScreen As String = Core.CurrentScreen.ToString.ToLower()
        If CurrentScreen.Contains("+") Then
            CurrentScreen = CurrentScreen.Split("+").Last.ToString
        End If
        Return CurrentScreen.Replace("p3d.", "")
    End Function

    Public Shared Function GetAvailableLanguagesList() As Dictionary(Of Integer, String)
        Dim FullPath As String = GameController.GamePath & GameMode.DefaultLocalizationsPath
        Dim AvailableLanguages As New Dictionary(Of Integer, String)
        Dim i As Integer = 0

        For Each TokenFile In IO.Directory.GetFiles(FullPath).Where(Function(f) Not IO.Path.GetFileName(f).Equals("missing_tokens.json"))
            Dim json As JObject = JObject.Parse(File.ReadAllText(TokenFile))
            Dim SelectedLanguage As String = json.SelectToken("language_name").ToString
            AvailableLanguages.Add(i, SelectedLanguage)
            i += 1
            'Logger.Debug("Localization.vb: GetAvailableLanguagesList: " & SelectedLanguage)
        Next

        Return AvailableLanguages
    End Function

    Public Shared Function GetLanguageName(ByVal lang As String) As String
        Dim FullPath As String = GameController.GamePath & GameMode.DefaultLocalizationsPath
        Dim LanguageNames As New Dictionary(Of String, String)

        For Each TokenFile In IO.Directory.GetFiles(FullPath).Where(Function(f) Not IO.Path.GetFileName(f).Equals("missing_tokens.json"))
            Dim iso = IO.Path.GetFileName(TokenFile).Replace(".json", "")
            Dim json As JObject = JObject.Parse(System.IO.File.ReadAllText(TokenFile))
            Dim name As String = json.SelectToken("language_name").ToString
            LanguageNames.Add(iso, name)
        Next

        'Logger.Debug("Localization.vb: GetLangugageName: " & lang & " / " & LanguageNames.Item(lang))
        Return LanguageNames.Item(lang)
    End Function

    Public Shared Function GetLanguageISO(ByVal lang As String) As String
        Dim FullPath As String = GameController.GamePath & GameMode.DefaultLocalizationsPath
        Dim LanguageISOs As New Dictionary(Of String, String)

        For Each TokenFile In IO.Directory.GetFiles(FullPath).Where(Function(f) Not IO.Path.GetFileName(f).Equals("missing_tokens.json"))
            Dim iso = IO.Path.GetFileName(TokenFile).Replace(".json", "")
            Dim json As JObject = JObject.Parse(System.IO.File.ReadAllText(TokenFile))
            Dim name As String = json.SelectToken("language_name").ToString
            LanguageISOs.Add(name, iso)
        Next

        'Logger.Debug("Localization.vb: GetLanguageISO: " & lang & " / " & LanguageISOs.Item(lang))
        Return LanguageISOs.Item(lang)
    End Function

End Class

Public Class Token

    Private _TokenContent As String = ""
    Private _TokenCurrentLanguage As String = "en"
    Private _IsGameModeToken As Boolean = False

    Public Sub New(ByVal TokenContent As String, ByVal TokenCurrentLanguage As String, ByVal IsGameModeToken As Boolean)
        Me._IsGameModeToken = IsGameModeToken
        Me._TokenContent = TokenContent
        Me._TokenCurrentLanguage = TokenCurrentLanguage
    End Sub

    Public Property TokenContent() As String
        Get
            Return Me._TokenContent
        End Get
        Set(value As String)
            Me._TokenContent = value
        End Set
    End Property

    Public Property TokenCurrentLanguage() As String
        Get
            Return Me._TokenCurrentLanguage
        End Get
        Set(value As String)
            Me._TokenCurrentLanguage = value
        End Set
    End Property

    Public Property IsGameModeToken() As Boolean
        Get
            Return Me._IsGameModeToken
        End Get
        Set(value As Boolean)
            Me._IsGameModeToken = value
        End Set
    End Property

End Class
