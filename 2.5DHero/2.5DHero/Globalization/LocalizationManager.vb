Namespace Globalization

    ''' <summary>
    ''' A class to manage all localizations available for the game.
    ''' They are marked with the LCID of the language.
    ''' </summary>
    ''' <remarks>
    ''' More Information:
    ''' https://msdn.microsoft.com/en-us/goglobal/bb964664.aspx
    ''' </remarks>
    Public Class LocalizationManager

        ' Private Constructor to prevent instances.
        Private Sub New() : End Sub

        Private Const DATA_PATH As String = "Localization"
        Private Shared _localList As Dictionary(Of String, Localization)

        Private Shared Sub Initialize()
            _localList = New Dictionary(Of String, Localization)()

            Dim lookUpPath As String = IO.Path.Combine({GameCore.FileSystem.GamePath, GameCore.FileSystem.PATH_LOCALIZATION})

            'For Each file As String In IO.Directory.GetFiles(lookUpPath, "*.dat")
            '    Dim localization As New Globalization.Localization(file)
            '    _localList.Add(localization.LCID, localization)
            'Next
        End Sub

        ''' <summary>
        ''' Returns the current culture ISO code of the language used.
        ''' </summary>
        Private Shared Function GetLocaleId() As String
            Return System.Globalization.CultureInfo.CurrentCulture.LCID.ToString()
        End Function

        ''' <summary>
        ''' Returns the localized string or Nothing, if no language or no token in that language exists.
        ''' </summary>
        Public Shared Function GetLocalString(ByVal token As String) As String
            If _localList Is Nothing Then
                Initialize()
            End If

            Dim _lcid As String = GetLocaleId()

            If _localList.Keys.Contains(_lcid) Then
                Return _localList(_lcid).GetTranslation(token)
            Else
                Return Nothing
            End If
        End Function

    End Class

End Namespace