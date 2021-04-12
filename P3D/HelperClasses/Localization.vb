Imports Newtonsoft.Json.Linq

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

        If System.IO.Directory.GetFiles(FullPath).Count > 0 Then
            If System.IO.File.Exists(LocaleFilePath) = False Then
                Logger.Debug("Localization.vb: Could not find file for: " & CurrentLanguage)
                CurrentLanguage = "en"
            End If

            If System.IO.File.Exists(LocaleFilePath) = True Then
                Logger.Debug("Localization.vb: Found file for suffix: " & CurrentLanguage)
                Dim TokensFile As JObject = JObject.Parse(System.IO.File.ReadAllText(DefaultLanguagePath))
                Dim SelectedLanguage As String = TokensFile.SelectToken("language_name").ToString
                Logger.Debug("Localization.vb: Loaded Language file and its name is: " & SelectedLanguage)
                For Each tokens In TokensFile.SelectToken("tokens").Values
                    If tokens.HasValues Then
                        For Each token In tokens
                            If token.HasValues Then
                                For Each subtoken In token
                                    AddToken(token.Path, token.First.ToString, SelectedLanguage, IsGameModeFile)
                                Next
                            End If
                            AddToken(token.Path, token.First.ToString, SelectedLanguage, IsGameModeFile)
                        Next
                    Else
                        AddToken(tokens.Path, tokens.First.ToString, SelectedLanguage, IsGameModeFile)
                    End If
                Next
            End If

            If CurrentLanguage IsNot "en" Then
                If System.IO.File.Exists(DefaultLanguagePath) Then
                    Dim FallbackTokensFile As JObject = JObject.Parse(System.IO.File.ReadAllText(DefaultLanguagePath))
                    Dim SelectedLanguage As String = FallbackTokensFile.SelectToken("language_name").ToString
                    Logger.Debug("Localization.vb: Loaded Fallback Language file and its name is: " & SelectedLanguage)
                    For Each tokens In FallbackTokensFile.SelectToken("tokens").Values
                        If tokens.HasValues Then
                            For Each token In tokens
                                If token.HasValues Then
                                    For Each subtoken In token
                                        AddToken(token.Path, token.First.ToString, SelectedLanguage, IsGameModeFile)
                                    Next
                                End If
                                AddToken(token.Path, token.First.ToString, SelectedLanguage, IsGameModeFile)
                            Next
                        Else
                            AddToken(tokens.Path, tokens.First.ToString, SelectedLanguage, IsGameModeFile)
                        End If
                    Next
                End If
            End If
        End If

        For Each lt In LocalizationTokens
            If lt.Key.StartsWith("press_start") Then
                Logger.Debug("Localization.vb: " & lt.Key.ToString)
            End If
        Next

    End Sub

    Public Shared Function GetString(ByVal s As String, Optional ByVal DefaultValue As String = "") As String
        Dim resultToken As Token = Nothing
        s = s.Replace(" ", "_").Replace("'", "").ToLower() ' Lets format the string before finding it
        If LocalizationTokens.ContainsKey(s) = True Then
            If LocalizationTokens.TryGetValue(s, resultToken) = False Then
                Return s
            Else
                Dim result As String = resultToken.TokenContent
                If Core.Player IsNot Nothing Then
                    result = result.Replace("<playername>", Core.Player.Name)
                    result = result.Replace("<rivalname>", Core.Player.RivalName)
                End If
                Return result
            End If
        Else
            If DefaultValue = "" Then
                Return s
            Else
                Return DefaultValue
            End If
        End If
    End Function

    Private Shared Function GetTokenName(ByVal s As String) As String
        s = s.Replace("tokens.", "").ToLower() ' Lets format the string before finding it
        Return s
    End Function

    Private Shared Function AddToken(ByVal key As String, ByVal value As String, ByVal lang As String, ByVal gmfile As Boolean) As String
        key = GetTokenName(key)
        If LocalizationTokens.ContainsKey(key) = False Then
            LocalizationTokens.Add(key, New Token(value, lang, gmfile))
        End If
    End Function

    Public Shared Function TokenExists(ByVal TokenName As String) As Boolean
        Return LocalizationTokens.ContainsKey(TokenName)
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
