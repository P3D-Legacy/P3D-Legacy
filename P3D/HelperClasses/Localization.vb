﻿Public Class Localization

    Public Shared LanguageSuffix As String = "en"
    Public Shared LocalizationTokens As Dictionary(Of String, Token) = New Dictionary(Of String, Token)

    Public Shared Sub Load(ByVal LanguageSuffix As String)
        LocalizationTokens.Clear()

        Localization.LanguageSuffix = LanguageSuffix

        Logger.Debug("Loaded language [" & LanguageSuffix & "]")

        LoadTokenFile(GameMode.DefaultLocalizationsPath, False)

        If GameModeManager.GameModeCount > 0 Then
            Dim GameModeLocalizationPath As String = GameModeManager.ActiveGameMode.LocalizationsPath
            If GameModeLocalizationPath <> GameMode.DefaultLocalizationsPath Then
                LoadTokenFile(GameModeLocalizationPath, True)
            End If
        End If
    End Sub

    Public Shared Sub ReloadGameModeTokens()
        LocalizationTokens.Clear()

        Localization.LanguageSuffix = LanguageSuffix

        LoadTokenFile(GameMode.DefaultLocalizationsPath, False)

        If GameModeManager.GameModeCount > 0 Then
            Dim GameModeLocalizationPath As String = GameModeManager.ActiveGameMode.LocalizationsPath
            If GameModeLocalizationPath <> GameMode.DefaultLocalizationsPath Then
                LoadTokenFile(GameModeLocalizationPath, True)
            End If
        End If

        Logger.Debug("---Reloaded GameMode Tokens---")
    End Sub

    Public Shared Sub LoadTokenFile(ByVal path As String, ByVal IsGameModeFile As Boolean)
        Dim fullpath As String = GameController.GamePath & path
        Dim tokenFullpath As String = fullpath & "Tokens_" & LanguageSuffix & ".dat"

        Logger.Debug("Token filepath: " & tokenFullpath)

        If System.IO.Directory.GetFiles(fullpath).Count > 0 Then
            If System.IO.File.Exists(tokenFullpath) = False Then
                Logger.Debug("Did NOT find token file for suffix: " & LanguageSuffix)
                LanguageSuffix = "en"
            End If

            If System.IO.File.Exists(tokenFullpath) = True Then
                Logger.Debug("Found token file for suffix: " & LanguageSuffix)
                Dim TokensFile() As String = System.IO.File.ReadAllLines(fullpath & "Tokens_" & LanguageSuffix & ".dat")
                Dim splitIdx As Integer = 0
                For Each TokenLine As String In TokensFile
                    If TokenLine.Contains(",") = True Then
                        splitIdx = TokenLine.IndexOf(",")

                        Dim TokenName As String = TokenLine.Substring(0, splitIdx)
                        Dim TokenContent As String = ""
                        If TokenLine.Length > TokenName.Length + 1 Then
                            TokenContent = TokenLine.Substring(splitIdx + 1, TokenLine.Length - splitIdx - 1)
                        End If

                        If LocalizationTokens.ContainsKey(TokenName) = False Then
                            LocalizationTokens.Add(TokenName, New Token(TokenContent, LanguageSuffix, IsGameModeFile))
                        Else
                            LocalizationTokens.Remove(TokenName)
                            LocalizationTokens.Add(TokenName, New Token(TokenContent, LanguageSuffix, IsGameModeFile))
                        End If
                    End If
                Next
            End If

            If Not LanguageSuffix = "en" Then
                If System.IO.File.Exists(fullpath & "Tokens_en.dat") Then
                    Dim FallbackTokensFile() As String = System.IO.File.ReadAllLines(fullpath & "Tokens_en.dat")
                    Dim splitIdx As Integer = 0
                    For Each TokenLine As String In FallbackTokensFile
                        If TokenLine.Contains(",") = True Then
                            splitIdx = TokenLine.IndexOf(",")

                            Dim TokenName As String = TokenLine.Substring(0, splitIdx)
                            Dim TokenContent As String = ""
                            If TokenLine.Length > TokenName.Length + 1 Then
                                TokenContent = TokenLine.Substring(splitIdx + 1, TokenLine.Length - splitIdx - 1)
                            End If

                            If LocalizationTokens.ContainsKey(TokenName) = False Then
                                LocalizationTokens.Add(TokenName, New Token(TokenContent, "en", IsGameModeFile))
                            Else
                                LocalizationTokens.Remove(TokenName)
                                LocalizationTokens.Add(TokenName, New Token(TokenContent, "en", IsGameModeFile))
                            End If
                        End If
                    Next
                End If
            End If
        End If
    End Sub

    Public Shared Function GetString(ByVal s As String, Optional ByVal DefaultValue As String = "") As String
        Dim resultToken As Token = Nothing
        If LocalizationTokens.ContainsKey(s) = True Then
            If LocalizationTokens.TryGetValue(s, resultToken) = False Then
                Return s
            Else
                Dim result As String = resultToken.TokenContent
                If Core.Player IsNot Nothing Then
                    result = result.Replace("<playername>", Core.Player.Name)
                    result = result.Replace("<rivalname>", Core.Player.RivalName)
                    If result.Contains("<") Then
                        result = CStr(ScriptVersion2.ScriptComparer.EvaluateConstruct(result))
                    End If
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

    Public Shared Function TokenExists(ByVal TokenName As String) As Boolean
        Return LocalizationTokens.ContainsKey(TokenName)
    End Function

End Class

Public Class Token

    Private _TokenContent As String = ""
    Private _TokenLanguageSuffix As String = "en"
    Private _IsGameModeToken As Boolean = False

    Public Sub New(ByVal TokenContent As String, ByVal TokenLanguageSuffix As String, ByVal IsGameModeToken As Boolean)
        Me._IsGameModeToken = IsGameModeToken
        Me._TokenContent = TokenContent
        Me._TokenLanguageSuffix = TokenLanguageSuffix
    End Sub

    Public Property TokenContent() As String
        Get
            Return Me._TokenContent
        End Get
        Set(value As String)
            Me._TokenContent = value
        End Set
    End Property

    Public Property TokenLanguageSuffix() As String
        Get
            Return Me._TokenLanguageSuffix
        End Get
        Set(value As String)
            Me._TokenLanguageSuffix = value
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