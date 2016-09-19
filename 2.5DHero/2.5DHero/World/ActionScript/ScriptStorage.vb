''' <summary>
''' Storage space for scripts.
''' Used with @storage and the storage construct.
''' </summary>
Public Class ScriptStorage

    Private Shared Pokemons As New Dictionary(Of String, Pokemon)
    Private Shared Strings As New Dictionary(Of String, String)
    Private Shared Integers As New Dictionary(Of String, Integer)
    Private Shared Booleans As New Dictionary(Of String, Boolean)
    Private Shared Items As New Dictionary(Of String, Item)
    Private Shared Singles As New Dictionary(Of String, Single)
    Private Shared Doubles As New Dictionary(Of String, Double)

    ''' <summary>
    ''' Returns storage content.
    ''' </summary>
    ''' <param name="type">The type of the storage content.</param>
    ''' <param name="name">The name of the storage content.</param>
    Public Shared Function GetObject(ByVal type As String, ByVal name As String) As Object
        Select Case type.ToLower()
            Case "pokemon"
                If Pokemons.ContainsKey(name) = True Then
                    Return Pokemons(name)
                End If
            Case "string", "str"
                If Strings.ContainsKey(name) = True Then
                    Return Strings(name)
                End If
            Case "integer", "int"
                If Integers.ContainsKey(name) = True Then
                    Return Integers(name)
                End If
            Case "boolean", "bool"
                If Booleans.ContainsKey(name) = True Then
                    Return Booleans(name)
                End If
            Case "item"
                If Items.ContainsKey(name) = True Then
                    Return Items(name)
                End If
            Case "single", "sng"
                If Singles.ContainsKey(name) = True Then
                    Return Singles(name)
                End If
            Case "double", "dbl"
                If Doubles.ContainsKey(name) = True Then
                    Return Doubles(name)
                End If
        End Select

        Return ScriptVersion2.ScriptComparer.DefaultNull
    End Function

    ''' <summary>
    ''' Adds or updates storage content.
    ''' </summary>
    ''' <param name="type">The type of the storage content.</param>
    ''' <param name="name">The name of the storage content.</param>
    ''' <param name="newContent">The new storage content.</param>
    Public Shared Sub SetObject(ByVal type As String, ByVal name As String, ByVal newContent As Object)
        Select Case type.ToLower()
            Case "pokemon"
                If Pokemons.ContainsKey(name) = True Then
                    Pokemons(name) = CType(newContent, Pokemon)
                Else
                    Pokemons.Add(name, CType(newContent, Pokemon))
                End If
            Case "string", "str"
                If Strings.ContainsKey(name) = True Then
                    Strings(name) = CStr(newContent)
                Else
                    Strings.Add(name, CStr(newContent))
                End If
            Case "integer", "int"
                If Integers.ContainsKey(name) = True Then
                    Integers(name) = int(newContent)
                Else
                    Integers.Add(name, int(newContent))
                End If
            Case "boolean", "bool"
                If Booleans.ContainsKey(name) = True Then
                    Booleans(name) = CBool(newContent)
                Else
                    Booleans.Add(name, CBool(newContent))
                End If
            Case "item"
                If Items.ContainsKey(name) = True Then
                    Items(name) = CType(newContent, Item)
                Else
                    Items.Add(name, CType(newContent, Item))
                End If
            Case "single", "sng"
                If Singles.ContainsKey(name) = True Then
                    Singles(name) = sng(newContent)
                Else
                    Singles.Add(name, sng(newContent))
                End If
            Case "double", "dbl"
                If Doubles.ContainsKey(name) = True Then
                    Doubles(name) = dbl(newContent)
                Else
                    Doubles.Add(name, dbl(newContent))
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Clears all script storage.
    ''' </summary>
    Public Shared Sub Clear()
        Pokemons.Clear()
        Strings.Clear()
        Integers.Clear()
        Booleans.Clear()
        Items.Clear()
        Singles.Clear()
        Doubles.Clear()
    End Sub

    ''' <summary>
    ''' Counts the content entries.
    ''' </summary>
    ''' <param name="type">The type of the content entires to count or empty for all entires.</param>
    Public Shared Function Count(ByVal type As String) As Integer
        If type = "" Then
            Return Pokemons.Count + Strings.Count + Integers.Count + Booleans.Count + Items.Count
        Else
            Select Case type.ToLower()
                Case "pokemon"
                    Return Pokemons.Count
                Case "string", "str"
                    Return Strings.Count
                Case "integer", "int"
                    Return Integers.Count
                Case "boolean", "bool"
                    Return Booleans.Count
                Case "item"
                    Return Items.Count
                Case "single", "sng"
                    Return Singles.Count
                Case "double", "dbl"
                    Return Doubles.Count
            End Select
        End If

        Return 0
    End Function

    Private Shared Function int(ByVal expression As Object) As Integer
        Return ScriptConversion.ToInteger(expression)
    End Function

    Private Shared Function sng(ByVal expression As Object) As Single
        Return ScriptConversion.ToSingle(expression)
    End Function

    Private Shared Function dbl(ByVal expression As Object) As Double
        Return ScriptConversion.ToDouble(expression)
    End Function

End Class
