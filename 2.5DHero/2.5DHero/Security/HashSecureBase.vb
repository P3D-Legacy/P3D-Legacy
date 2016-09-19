Imports System.Reflection

Namespace Security

    ''' <summary>
    ''' Represents a base class that adds hash checked properties to other classes.
    ''' </summary>
    ''' <remarks>To use the features of this class, call UpdatePropertyHash after changing the value of a Property. This class does not work with properties that have parameters.</remarks>
    Public MustInherit Class HashSecureBase

        'List of Properties to keep track of.
        Private _properties As New Dictionary(Of String, Integer)

        ''' <summary>
        ''' Runs a test check on a secure property.
        ''' </summary>
        ''' <param name="valueName">The name of the value.</param>
        ''' <param name="currentValue">The value that SHOULD be stored in the property.</param>
        Protected Sub Assert(ByVal valueName As String, ByVal currentValue As Object)
            Assert(valueName, currentValue, currentValue)
        End Sub

        ''' <summary>
        ''' Runs a test check on a secure property.
        ''' </summary>
        ''' <param name="valueName">The name of the value.</param>
        ''' <param name="currentValue">The value that SHOULD be stored in the property.</param>
        ''' <param name="newValue">The value that will be stored in the value when the test is over.</param>
        Protected Sub Assert(ByVal valueName As String, ByVal currentValue As Object, ByVal newValue As Object)
            Dim hash As Integer = currentValue.GetHashCode()
            Dim listName As String = valueName.ToLower()

            If _properties.ContainsKey(listName) = True Then
                If _properties(listName) = hash Then
                    _properties(listName) = newValue.GetHashCode()
                Else
                    Throw New MismatchingHashValuesException(valueName)
                End If
            Else
                _properties.Add(listName, newValue.GetHashCode())
            End If
        End Sub

        ''' <summary>
        ''' The exception that gets thrown when the system detects mismatching hash values.
        ''' </summary>
        Private Class MismatchingHashValuesException

            Inherits Exception

            Public Sub New(ByVal PropertyName As String)
                MyBase.New("The internal Hash Validation System has detected external changes to secure properties.")

                Me.Data.Add("PropertyName", PropertyName)
            End Sub

        End Class

    End Class

End Namespace
