Imports System.Text.RegularExpressions

Namespace Construct.Framework

    Public Class StorageHandler

#Region "Singleton Handler"

        Private Shared _singleton As StorageHandler = Nothing

        ''' <summary>
        ''' Returns the active instance of the ScriptController.
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetInstance() As StorageHandler
            If _singleton Is Nothing Then
                _singleton = New StorageHandler()
            End If
            Return _singleton
        End Function

        Private Sub New()
        End Sub

#End Region

        'The storage handler controls the values that can be accessed via the storage class.
        'The previous system divided the values into different types, then names.
        'This means that one can have two different values with the same name in two different types.
        'In this new version, we only store in the type String.
        'To work around the issue of having potential double identifiers, we add the type definition, if given, to the identifier.

        Const IDENTIFIER_REGEX As String = "^[a-z0-9A-Z_/\\\. \-]+$"

        'Stores the storage values:     Idenfitifer, Value
        Private _values As New Dictionary(Of String, String)

#Region "Set"

        ''' <summary>
        ''' Sets a storage value.
        ''' </summary>
        ''' <param name="identifier">The value identifier</param>
        ''' <param name="value">The value.</param>
        Public Sub SetValue(ByVal identifier As String, ByVal value As String)
            If IsValidName(identifier) = True Then
                InternalSetValue(identifier, value)
            Else
                Logger.Debug("014", "Invalid storage identifier: " & identifier)
            End If
        End Sub

        Private Sub InternalSetValue(ByVal identifier As String, ByVal value As String)
            If _values.Keys.Contains(identifier.ToLower()) = True Then
                _values(identifier.ToLower()) = value
            Else
                _values.Add(identifier.ToLower(), value)
            End If
        End Sub

#End Region

#Region "Get"

        ''' <summary>
        ''' Returns a storage value.
        ''' </summary>
        ''' <param name="dataType">The deprecated data type.</param>
        ''' <param name="identifier">The value identifier</param>
        ''' <returns></returns>
        Public Function GetValue(ByVal dataType As String, ByVal identifier As String) As String
            If IsValidName(identifier) = True And IsValidName(dataType) = True Then
                Return InternalGetValue(identifier)
            Else
                Logger.Debug("015", "Invalid storage identifier: " & identifier)
                Return Core.Null
            End If
        End Function

        ''' <summary>
        ''' Returns a storage value.
        ''' </summary>
        ''' <param name="identifier">The value identifier</param>
        ''' <returns></returns>
        Public Function GetValue(ByVal identifier As String) As String
            If IsValidName(identifier) = True Then
                Return InternalGetValue(identifier)
            Else
                Logger.Debug("016", "Invalid storage identifier: " & identifier)
                Return Core.Null
            End If
        End Function

        Private Function InternalGetValue(ByVal identifier As String) As String
            If _values.Keys.Contains(identifier.ToLower()) = True Then
                Return _values(identifier.ToLower())
            Else
                Logger.Debug("017", "Non existing storage identifier: " & identifier)
                Return Core.Null
            End If
        End Function

#End Region

        ''' <summary>
        ''' Clears all stored values.
        ''' </summary>
        Public Sub Clear()
            _values.Clear()
        End Sub

        ''' <summary>
        ''' Returns the amount of values in the storage.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Count() As Integer
            Get
                Return _values.Count
            End Get
        End Property

        ''' <summary>
        ''' Returns if a value for an identifier exists.
        ''' </summary>
        ''' <param name="identifier">The identifier to check.</param>
        ''' <returns></returns>
        Public Function Exists(ByVal identifier As String) As Boolean
            Return _values.Keys.Contains(identifier.ToLower())
        End Function

        ''' <summary>
        ''' Checks if a string is a valid identifier.
        ''' </summary>
        ''' <param name="registerName">The identifier to check.</param>
        ''' <returns></returns>
        Private Function IsValidName(ByVal registerName As String) As Boolean
            Return Regex.IsMatch(registerName, IDENTIFIER_REGEX, RegexOptions.Multiline)
        End Function

    End Class

End Namespace