Imports Pokemon3D.DataModel.Json
Imports Pokemon3D.DataModel.Json.Localization

Namespace Globalization

    ''' <summary>
    ''' The content of a localization file.
    ''' </summary>
    Public Class Localization

        Private _lcid As String

        ''' <summary>
        ''' The language specific LCID.
        ''' </summary>
        Public ReadOnly Property LCID As String
            Get
                Return _lcid
            End Get
        End Property

        Private _dictionary As New Dictionary(Of String, String)

        Private _filePath As String = ""
        Private _hasLoaded As Boolean = False

        Public Sub New(ByVal filePath As String)
            _filePath = filePath

            ' Take the lcid from the file name:
            _lcid = IO.Path.GetFileNameWithoutExtension(_filePath)
        End Sub

        ''' <summary>
        ''' Returns the translation of this localization for a token.
        ''' </summary>
        Public Function GetTranslation(ByVal token As String) As String
            If Not _hasLoaded Then 'Only load on demand.
                Load()
            End If

            If _dictionary.Keys.Contains(token) Then
                Return _dictionary(token)
            Else
                Return Nothing
            End If
        End Function

        Private Sub Load()

            ''DO NOTHING LOL

            '    _hasLoaded = True

            '    Dim fileContent As String = IO.File.ReadAllText(_filePath)
            '    Dim dataModel = JsonDataModel.FromString(Of LocalizationModel)(fileContent)

            '    For Each token In dataModel.Tokens
            '        _dictionary.Add(token.Id, token.Val)
            '    Next
        End Sub

    End Class

End Namespace