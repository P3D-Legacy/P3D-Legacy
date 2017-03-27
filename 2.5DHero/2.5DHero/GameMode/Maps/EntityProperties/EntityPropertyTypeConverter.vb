Imports System.Text.RegularExpressions

Namespace GameModes.Maps.EntityProperties

    ''' <summary>
    ''' Contains methods to convert the string data of entity properties to data types.
    ''' </summary>
    Class EntityPropertyTypeConverter

        'All methods in here are marked as Object return type,
        'because the CType conversion in the GetData method in
        'EntityProperty.vb needs to have a proper Object.

        ''' <summary>
        ''' Converts string data into a <see cref="Vector3"/>.
        ''' </summary>
        Public Shared Function ToVector3(ByVal data As String) As Object
            Dim dataParts As String() = data.Split(",")
            If dataParts.Length = 3 Then

                Dim xResult, yResult, zResult As Single
                If Single.TryParse(dataParts(0), xResult) Then
                    If Single.TryParse(dataParts(1), yResult) Then
                        If Single.TryParse(dataParts(2), zResult) Then
                            Return New Vector3(xResult, yResult, zResult)
                        End If
                    End If
                End If

            End If

            Return Vector3.Zero
        End Function

        ''' <summary>
        ''' Converts string data into a <see cref="Vector2"/>.
        ''' </summary>
        Public Shared Function ToVector2(ByVal data As String) As Object
            Dim dataParts As String() = data.Split(",")
            If dataParts.Length = 2 Then

                Dim xResult, yResult As Single
                If Single.TryParse(dataParts(0), xResult) Then
                    If Single.TryParse(dataParts(1), yResult) Then
                        Return New Vector2(xResult, yResult)
                    End If
                End If

            End If

            Return Vector2.Zero
        End Function

        ''' <summary>
        ''' Converts string data into a <see cref="Single"/>.
        ''' </summary>
        Public Shared Function ToSingle(ByVal data As String) As Object
            Dim result As Single = 0F
            Single.TryParse(data, result)
            Return result
        End Function

        ''' <summary>
        ''' Converts string data into a <see cref="Integer"/>.
        ''' </summary>
        Public Shared Function ToInteger(ByVal data As String) As Object
            Dim result As Integer = 0
            Integer.TryParse(data, result)
            Return result
        End Function

    End Class

End Namespace
