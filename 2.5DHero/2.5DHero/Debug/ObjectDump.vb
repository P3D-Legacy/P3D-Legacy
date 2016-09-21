''' <summary>
''' A class to access the dump of variables of an object.
''' </summary>
Public Class ObjectDump

    Private _dump As String = ""
    Private _nullReferenceError As Boolean = False

    Public ReadOnly Property Dump() As String
        Get
            Return _dump
        End Get
    End Property

    Public ReadOnly Property NullReferenceError() As Boolean
        Get
            Return _nullReferenceError
        End Get
    End Property

    ''' <summary>
    ''' Creates a new instance of the ObjectDump class and performs a dump on the passed object.
    ''' </summary>
    Public Sub New(ByVal obj As Object)
        CreateDump(obj)
    End Sub

    ''' <summary>
    ''' Creates a dump.
    ''' </summary>
    ''' <param name="obj">The object to create a dump from.</param>
    Private Sub CreateDump(ByVal obj As Object)
        If obj Is Nothing Then
            _nullReferenceError = True
            _dump = "Object reference not set to an instance of an object."
        Else
            Dim t As Type = obj.GetType()

            Dim fields As Reflection.FieldInfo() = t.GetFields(Reflection.BindingFlags.Public Or
                                                               Reflection.BindingFlags.NonPublic Or
                                                               Reflection.BindingFlags.Instance Or
                                                               Reflection.BindingFlags.Static)

            Dim dump As String = ""

            For Each field As Reflection.FieldInfo In fields
                If dump <> "" Then
                    dump &= vbNewLine
                End If

                Dim accessToken As String = ""
                Dim valueToken As String = "Nothing"
                Dim fieldToken As String = field.Name
                Dim typeToken As String = field.FieldType.Name

                If field.IsPublic = True Then
                    accessToken = "Public "
                End If
                If field.IsPrivate = True Then
                    accessToken = "Private "
                End If
                If field.IsFamily = True Then
                    accessToken = "Protected "
                End If
                If field.IsStatic = True Then
                    accessToken &= "Shared "
                End If

                Dim valueObj As Object = field.GetValue(obj)
                If valueObj IsNot Nothing Then
                    If typeToken.EndsWith("[]") = True Then
                        'Type is array, ToString wont return maintainable material, so we get its content:
                        valueToken = GetArrayDump(valueObj)
                    ElseIf typeToken = "List`1" Then
                        'Type is a list, ToString wont return maintainable material, so we get its content:
                        valueToken = GetListDump(valueObj)
                        typeToken = GetListTypeToken(valueObj)
                    Else
                        valueToken = valueObj.ToString()
                    End If
                End If

                dump &= "   " & accessToken & fieldToken & " As " & typeToken & " = " & valueToken
            Next

            _dump = dump
        End If
    End Sub

    Private Function GetArrayDump(ByVal valueObj As Object) As String
        Dim listDump As String = ""

        Dim valueArray As Array = CType(valueObj, Array)
        For i = 0 To valueArray.Length - 1
            If listDump <> "" Then
                listDump &= ", "
            End If
            If valueArray.GetValue(i) Is Nothing Then
                listDump &= "Nothing"
            Else
                listDump &= valueArray.GetValue(i).ToString()
            End If
        Next
        listDump = "{" & listDump & "}"
        Return listDump
    End Function

    Private Function GetListDump(ByVal valueObj As Object) As String
        'Grab the type of the list (List`1):
        Dim listType As Type = Type.GetType(valueObj.ToString())
        'Get the ToArray method of the list:
        Dim method As Reflection.MethodInfo = listType.GetMethod("ToArray", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)

        Dim listDump As String = ""

        'Create an array from the list by invoking its ToArray method. The array is not a generic type anymore, which means we can iterate through it:
        Dim valueArray As Array = CType(method.Invoke(valueObj, {}), Array)
        For i = 0 To valueArray.Length - 1
            If listDump <> "" Then
                listDump &= ", "
            End If
            If valueArray.GetValue(i) Is Nothing Then
                listDump &= "Nothing"
            Else
                listDump &= valueArray.GetValue(i).ToString()
            End If
        Next
        listDump = "{" & listDump & "}"
        Return listDump
    End Function

    Private Function GetListTypeToken(ByVal valueObj As Object) As String
        Dim listType As Type = Type.GetType(valueObj.ToString())
        Return "List<" & listType.GetGenericArguments()(0).Name & ">[]"
    End Function

End Class
