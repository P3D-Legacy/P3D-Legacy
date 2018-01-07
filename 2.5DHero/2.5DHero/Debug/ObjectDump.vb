Imports System.Collections
Imports System.Reflection

''' <summary>
''' A class to access the dump of variables of an object.
''' </summary>
Public Class ObjectDump

    Public ReadOnly Property Dump As String = ""

    Public Sub New(ByVal sender As Object)
        If sender Is Nothing Then
            Dump = "Object reference not set to an instance of an object."
        Else
            Dim fields() As FieldInfo = sender.GetType().GetFields(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Static)
            Dim properties() As PropertyInfo = sender.GetType().GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Static)

            Dump =
                "--------------------------------------------------" & Environment.NewLine &
                "Generated Fields:" & Environment.NewLine &
                "--------------------------------------------------" & Environment.NewLine

            For Each field As FieldInfo In fields
                If Dump <> "" Then
                    Dump &= Environment.NewLine
                End If

                Dim fieldAccessToken As String = ""
                Dim fieldNameToken As String = ""
                Dim fieldTypeToken As String = ""
                Dim fieldValueToken As String = ""

                If field.IsPublic Then
                    fieldAccessToken = "Public "
                ElseIf field.IsPrivate Then
                    fieldAccessToken = "Private "
                ElseIf field.IsFamily Then
                    fieldAccessToken = "Protected "
                End If

                If field.IsStatic Then
                    fieldAccessToken &= "Shared "
                End If

                fieldNameToken = field.Name
                fieldTypeToken = field.FieldType.Name

                If field.FieldType.IsArray Then
                    fieldValueToken = DumpArray(field.GetValue(sender))
                ElseIf field.FieldType.IsGenericType Then
                    If field.FieldType.Name = "List`1" Then
                        fieldTypeToken = $"List(Of {field.FieldType.GetGenericArguments()(0).Name})"
                        fieldValueToken = DumpGenericArray(field.GetValue(sender), "List`1")
                    ElseIf field.FieldType.Name = "Dictionary`2" Then
                        fieldTypeToken = $"Dictionary(Of {field.FieldType.GetGenericArguments()(0).Name}, {field.FieldType.GetGenericArguments()(1).Name})"
                        fieldValueToken = DumpGenericArray(field.GetValue(sender), "Dictionary`2")
                    End If
                ElseIf field.FieldType.Name = "Texture2D" Then
                    fieldValueToken = DumpTexture2D(field.GetValue(sender))
                Else
                    fieldValueToken = DumpObject(field.GetValue(sender))
                End If

                Dump &= fieldAccessToken & fieldNameToken & " As " & fieldTypeToken & " = " & fieldValueToken
            Next

            Dump &= Environment.NewLine & Environment.NewLine &
                "--------------------------------------------------" & Environment.NewLine &
                "Generated Property:" & Environment.NewLine &
                "--------------------------------------------------" & Environment.NewLine

            For Each [property] As PropertyInfo In properties
                If [property].CanRead Then
                    If Dump <> "" Then
                        Dump &= Environment.NewLine
                    End If

                    Dim propertyNameToken As String = ""
                    Dim propertyTypeToken As String = ""
                    Dim propertyValueToken As String = ""

                    propertyNameToken = [property].Name
                    propertyTypeToken = [property].PropertyType.Name

                    If [property].PropertyType.IsArray Then
                        propertyValueToken = DumpArray([property].GetValue(sender))
                    ElseIf [property].PropertyType.IsGenericType Then
                        If [property].PropertyType.Name = "List`1" Then
                            propertyTypeToken = $"List(Of {[property].PropertyType.GetGenericArguments()(0).Name})"
                            propertyValueToken = DumpGenericArray([property].GetValue(sender), "List`1")
                        ElseIf [property].PropertyType.Name = "Dictionary`2" Then
                            propertyTypeToken = $"Dictionary(Of {[property].PropertyType.GetGenericArguments()(0).Name}, {[property].PropertyType.GetGenericArguments()(1).Name})"
                            propertyValueToken = DumpGenericArray([property].GetValue(sender), "Dictionary`2")
                        End If
                    ElseIf [property].PropertyType.Name = "Texture2D" Then
                        propertyValueToken = DumpTexture2D([property].GetValue(sender))
                    Else
                        propertyValueToken = DumpObject([property].GetValue(sender))
                    End If

                    Dump &= "Property " & propertyNameToken & " As " & propertyTypeToken & " = " & propertyValueToken
                End If
            Next
        End If
    End Sub

    Private Function DumpArray(ByVal obj As Object) As String
        Try
            If obj IsNot Nothing Then
                Dim listValue As Array = CType(obj, Array)
                If listValue.Length = 0 Then
                    Return "{}"
                Else
                    Return "{" & String.Join(", ", listValue.Cast(Of Object).Select(Function(a)
                                                                                        Return a.ToString()
                                                                                    End Function).ToArray()) & "}"
                End If
            Else
                Return "Nothing"
            End If
        Catch ex As Exception
            Return "Array too complex to dump."
        End Try
    End Function

    Private Function DumpGenericArray(ByVal obj As Object, ByVal genericType As String) As String
        Try
            If obj IsNot Nothing Then
                If genericType = "List`1" Then
                    Dim listValue As Array = CType(obj.GetType().GetMethod("ToArray").Invoke(obj, Nothing), Array)
                    If listValue.Length = 0 Then
                        Return "{}"
                    Else
                        Return "{" & String.Join(", ", listValue.Cast(Of Object).Select(Function(a)
                                                                                            Return a.ToString()
                                                                                        End Function).ToArray()) & "}"
                    End If
                ElseIf genericType = "Dictionary`2" Then
                    Dim dictionaryKeys As Array = CType(obj.GetType().GetProperty("Keys").GetValue(obj), IEnumerable).Cast(Of Object).ToArray()
                    Dim dictonaryValues As Array = CType(obj.GetType().GetProperty("Values").GetValue(obj), IEnumerable).Cast(Of Object).ToArray()

                    If dictionaryKeys.Length = 0 OrElse dictonaryValues.Length = 0 Then
                        Return "{}"
                    Else
                        Dim result As String = ""
                        For i As Integer = 0 To dictionaryKeys.Length - 1
                            If i > 0 Then
                                result &= ", "
                            End If
                            result &= "{" & dictionaryKeys.Cast(Of Object)()(i).ToString() & ", " & dictonaryValues.Cast(Of Object)()(i).ToString() & "}"
                        Next
                        Return "{" & result & "}"
                    End If
                Else
                    Return "Generic Type too complex to dump."
                End If
            Else
                Return "Nothing"
            End If
        Catch ex As Exception
            Return "Generic Type too complex to dump."
        End Try
    End Function

    Private Function DumpTexture2D(ByVal obj As Object) As String
        Try
            If obj IsNot Nothing Then
                Dim textureName As String = ""
                Dim width As Integer = Convert.ToInt32(obj.GetType().GetProperty("Width").GetValue(obj))
                Dim height As Integer = Convert.ToInt32(obj.GetType().GetProperty("Height").GetValue(obj))

                If String.IsNullOrEmpty((obj.GetType().GetProperty("Name").GetValue(obj)?.ToString())) Then
                    textureName = """"""
                Else
                    textureName = obj.GetType().GetProperty("Name").GetValue(obj)?.ToString()
                End If

                Return $"{{Name = {textureName}, Width = {width}, Height = {height}}}"
            Else
                Return "Nothing"
            End If
        Catch ex As Exception
            Return "Texture2D too complex to dump."
        End Try
    End Function

    Private Function DumpObject(ByVal obj As Object) As String
        Try
            If obj IsNot Nothing Then
                If String.IsNullOrEmpty(obj.ToString()) Then
                    Return """"""
                Else
                    Return obj.ToString()
                End If
            Else
                Return "Nothing"
            End If
        Catch ex As Exception
            Return "Object too complex to dump."
        End Try
    End Function
End Class