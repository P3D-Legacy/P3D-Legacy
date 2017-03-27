Namespace Construct.Framework.Classes

    <ScriptClass("FileSystem")>
    <ScriptDescription("A class to work with the computer's file system.")>
    Public Class CL_Filesystem

        Inherits ScriptClass

        <ScriptConstruct("PathSplit")>
        <ScriptDescription("Returns a split of a path.")>
        Private Function F_PathSplit(ByVal argument As String) As String
            Dim index As Integer = Int(argument.Remove(argument.IndexOf(",")))
            Dim folderpath As String = argument.Remove(0, argument.IndexOf(",") + 1)

            Dim folderSplits() As String = folderpath.Split(CChar("\"))

            Return folderSplits(index)
        End Function

        <ScriptConstruct("PathSplitCount")>
        <ScriptDescription("Returns the amount of parts of a path.")>
        Private Function F_PathSplitCount(ByVal argument As String) As String
            Return ToString(argument.Split(CChar("\")).Length)
        End Function

        <ScriptConstruct("PathUp")>
        <ScriptDescription("Returns the parent folder of a folder/file.")>
        Private Function F_PathUp(ByVal argument As String) As String
            Dim folderPath As String = argument
            If folderPath.Contains("\") = True Then
                Return folderPath.Remove(folderPath.LastIndexOf("\"))
            End If
            Return folderPath
        End Function

    End Class

End Namespace