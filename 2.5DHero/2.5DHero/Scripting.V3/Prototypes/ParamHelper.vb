Namespace Scripting.V3.Prototypes

    Friend NotInheritable Class ParamHelper

        Private _parameters As Object()
        Private _index As Integer

        Public Sub New(parameters As Object())
            _parameters = parameters
        End Sub

        ''' <summary>
        ''' Grabs the next item from the parameter stack and advances the stack.
        ''' </summary>
        Public Function Grab(Of T)(Optional defaultValue As T = Nothing) As T

            If HasEnded() Then Return defaultValue

            Dim result As T

            If _parameters(_index) IsNot Nothing Then
                result = CType(_parameters(_index), T)
            Else
                result = defaultValue
            End If

            _index += 1
            Return result

        End Function

        ''' <summary>
        ''' Skips a set amount of items on the stack.
        ''' </summary>
        Public Sub Skip(Optional steps As Integer = 1)
            _index += steps
        End Sub

        ''' <summary>
        ''' Checks if the helper has reached the end of the stack.
        ''' </summary>
        Public Function HasEnded() As Boolean
            Return _index = _parameters.Length
        End Function

    End Class

End Namespace
