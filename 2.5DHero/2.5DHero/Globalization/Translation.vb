Imports System.Reflection

Namespace Globalization

    ''' <summary>
    ''' The base class for translation objects for UI elements.
    ''' </summary>
    Public MustInherit Class Translation

        '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        '// Instructions how to use classes that inherit from this:                                                                                 //
        '// Their type name must start with "LOCAL_".                                                                                               //
        '// All constants delared private and with a name starting with "C_" are put in the dictionary.                                             //
        '// The "C_" and "LOCAL_" are not considered when looking up identifiers in external files.                                                 //
        '// External files use "TypeName:Constant" as lookup (again, without "LOCAL_" and "C_").                                                    //
        '// For the class LOCAL_InventoryScreen and its constant C_INFO_ITEM_OPTION_USE, the lookup is: InventoryScreen:INFO_ITEM_OPTION_USE.       //
        '/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ''' <summary>
        ''' A token to be inserted in a translated text.
        ''' </summary>
        Protected Structure TranslationToken
            Public Number As Integer
            Public Data As String
        End Structure

        Private _stringDic As Dictionary(Of String, String)
        Private _typeName As String = ""

        Public Sub New()
            _stringDic = New Dictionary(Of String, String)()

            ' Grab all constants in the class and add them to the dictionary.
            ' Those are the raw values to translate.
            Dim t As Type = Me.GetType()

            If Not t.Name.StartsWith("LOCAL_") Then
                Throw New ArgumentException("The type used as localization class has to start with ""LOCAL_"".")
            End If

            _typeName = t.Name.Remove(0, 6) 'Remove "LOCAL_".

            Dim constants = t.GetFields(BindingFlags.NonPublic Or BindingFlags.Static Or BindingFlags.FlattenHierarchy) _
                                 .Where(Function(fi) fi.IsLiteral AndAlso Not fi.IsInitOnly AndAlso fi.Name.StartsWith("C_")).ToArray()

            For Each cField In constants
                Dim cValue As String = CType(cField.GetRawConstantValue(), String)
                Dim cName As String = cField.Name.Remove(0, 2) 'Remove the C_ at the start of the constant name.

                AddTranslation(cName, cValue)
            Next
        End Sub

        ''' <summary>
        ''' Adds an entry to the dictionary with a default english string.
        ''' </summary>
        Private Sub AddTranslation(ByVal identifier As String, ByVal englishDefault As String)
            If englishDefault Is Nothing Then
                Throw New ArgumentException("The default value must not be null.", NameOf(englishDefault))
            End If

            _stringDic.Add(identifier, englishDefault)
        End Sub

        ''' <summary>
        ''' Returns the translation for a UI string.
        ''' </summary>
        ''' <param name="identifier">The UI string identifier set with <see cref="AddTranslation"/>.</param>
        ''' <param name="tokens">The tokens to be inserted into the string.</param>
        Protected Function GetTranslation(ByVal identifier As String, ByVal tokens As String()) As String
            Return GetTranslation(identifier, BuildTokens(tokens))
        End Function

        ''' <summary>
        ''' Returns the translation for a UI string.
        ''' </summary>
        ''' <param name="identifier">The UI string identifier set with <see cref="AddTranslation"/>.</param>
        Protected Function GetTranslation(ByVal identifier As String) As String
            ' Helper function for use without tokens.
            Return GetTranslation(identifier, New TranslationToken() {})
        End Function

        Private Function BuildTokens(ByVal inputVars As Object()) As TranslationToken()
            Dim tokens As New List(Of TranslationToken)

            For i = 0 To inputVars.Count - 1
                Dim var = inputVars(i)
                tokens.Add(New TranslationToken() With
                           {
                                .Data = var.ToString(),
                                .Number = i + 1
                           })
            Next

            Return tokens.ToArray()
        End Function

        ''' <summary>
        ''' Returns the translation for a UI string.
        ''' </summary>
        ''' <param name="identifier">The UI string identifier set with <see cref="AddTranslation"/>.</param>
        ''' <param name="tokens">The tokens inserted into the string.</param>
        Private Function GetTranslation(ByVal identifier As String, ByVal tokens As TranslationToken()) As String
            ' Translation tokens use .Net's String.Format format: {numeric}
            ' So the first token replaces "{0}".

            If identifier Is Nothing Then
                Throw New ArgumentException("The identifier must not be null.", NameOf(identifier))
            End If

            'Remove "C_" from the identifier:
            identifier = identifier.Remove(0, 2)

            If Not _stringDic.Keys.Contains(identifier) Then
                Throw New ArgumentException("The identifier is not present in the dictionary.", NameOf(identifier))
            End If

            ' The identifier for the files is: type name of the class (minus "LOCAL_") and the identifier.
            Dim translatedString = LocalizationManager.GetLocalString(_typeName & ":" & identifier)

            If translatedString Is Nothing Then
                ' Fallback string if no language translation is found.
                translatedString = _stringDic(identifier)
            End If

            ' Only attempt to replace tokens if possible:
            If tokens IsNot Nothing AndAlso tokens.Length > 0 Then
                tokens = tokens.OrderBy(Function(x)
                                            Return x.Number
                                        End Function).ToArray()

                Dim params As Object() = tokens.Select(Function(x)
                                                           Return x.Data
                                                       End Function).ToArray()

                translatedString = String.Format(translatedString, params)
            End If

            Return translatedString
        End Function

    End Class

End Namespace
