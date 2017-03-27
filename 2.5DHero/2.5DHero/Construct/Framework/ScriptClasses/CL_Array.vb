Namespace Construct.Framework.Classes

    <ScriptClass("Array")>
    <ScriptDescription("A class to work with arrays.")>
    Public Class CL_Array

        Inherits ScriptClass

        <ScriptConstruct("Reverse")>
        <ScriptDescription("Reverses the order of the items in the array.")>
        Private Function F_Reverse(ByVal argument As String) As String
            Dim value As String = argument.Remove(0, argument.LastIndexOf(",") + 1)
            Dim arrayString As String = argument.Remove(argument.LastIndexOf(","))

            Dim arr = Parser.ParseArray(arrayString)
            arr.Reverse()

            Return Parser.ArrayToString(arr.ToArray())
        End Function

        <ScriptConstruct("IndexOf")>
        <ScriptDescription("Returns the first index of an item in the array.")>
        Private Function F_IndexOf(ByVal argument As String) As String
            Dim value As String = argument.Remove(0, argument.LastIndexOf(",") + 1)
            Dim arrayString As String = argument.Remove(argument.LastIndexOf(","))

            Dim arr = Parser.ParseArray(arrayString)

            Return ToString(arr.IndexOf(value))
        End Function

        <ScriptConstruct("LastIndexOf")>
        <ScriptDescription("Returns the last index of an item in the array.")>
        Private Function F_LastIndexOf(ByVal argument As String) As String
            Dim value As String = argument.Remove(0, argument.LastIndexOf(",") + 1)
            Dim arrayString As String = argument.Remove(argument.LastIndexOf(","))

            Dim arr = Parser.ParseArray(arrayString)

            Return ToString(arr.LastIndexOf(value))
        End Function

        <ScriptConstruct("Sort")>
        <ScriptDescription("Sorts an array and returns the result.")>
        Private Function F_Sort(ByVal argument As String) As String
            Dim sortMode As String = argument.Remove(0, argument.LastIndexOf(",") + 1)
            Dim arrayString As String = argument.Remove(argument.LastIndexOf(","))

            Dim arr = Parser.ParseArray(arrayString)

            If sortMode.ToLower() = "ascending" Then
                Return Parser.ArrayToString((From s As String In arr Order By s Ascending).ToArray())
            ElseIf sortMode.ToLower() = "descending" Then
                Return Parser.ArrayToString((From s As String In arr Order By s Descending).ToArray())
            End If

            Return Parser.ArrayToString(arr.ToArray())
        End Function

        <ScriptConstruct("Contains")>
        <ScriptDescription("Returns if an array contains a specific string.")>
        Private Function F_Contains(ByVal argument As String) As String
            Dim value As String = argument.Remove(0, argument.LastIndexOf(",") + 1)
            Dim arrayString As String = argument.Remove(argument.LastIndexOf(","))

            Dim arr = Parser.ParseArray(arrayString)

            Return ToString(arr.Contains(value))
        End Function

        <ScriptConstruct("GetValue")>
        <ScriptDescription("Returns the value of the array at a specific position.")>
        Private Function F_GetValue(ByVal argument As String) As String
            Dim index As String = argument.Remove(0, argument.LastIndexOf(",") + 1)
            Dim arrayString As String = argument.Remove(argument.LastIndexOf(","))

            Dim arr = Parser.ParseArray(arrayString)

            Return arr(Int(index))
        End Function

    End Class

End Namespace