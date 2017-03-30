Namespace GameJolt

    Public Class API

        Public Const API_VERSION As String = "v1_1"

        Public Shared username As String = ""
        Public Shared token As String = ""
        Public Shared gameJoltId As String = ""

        Public Shared LoggedIn As Boolean = False

        Public Shared Exception As System.Exception = Nothing

        Public Shared APICallCount As Integer = 0

        Public Structure JoltValue
            Dim Name As String
            Dim Value As String
        End Structure

        ''' <summary>
        ''' Handles received data.
        ''' </summary>
        ''' <param name="data">The data to work with.</param>
        Public Shared Function HandleData(ByVal data As String) As List(Of JoltValue)
            'Old system:
            If data.Contains("data:""" & vbNewLine) = True Then
                data = data.Replace("data:""" & vbNewLine, "data:""")
            End If

            Dim arg() As String = {vbCrLf, vbLf}

            Dim list As List(Of String) = data.Split(arg, StringSplitOptions.None).ToList()
            Dim joltList As New List(Of JoltValue)

            For Each Item As String In list
                If Item.Contains(":") = True Then
                    Dim ValueName As String = Item.Remove(Item.IndexOf(":"))
                    Dim ValueContent As String = Item.Remove(0, Item.IndexOf(":") + 2)
                    ValueContent = ValueContent.Remove(ValueContent.Length - 1, 1)

                    Dim jValue As New JoltValue
                    jValue.Name = ValueName
                    jValue.Value = ValueContent

                    joltList.Add(jValue)
                End If
            Next

            Return joltList
        End Function

    End Class

End Namespace