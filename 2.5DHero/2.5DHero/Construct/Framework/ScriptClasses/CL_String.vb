Namespace Construct.Framework.Classes

    <ScriptClass("String")>
    <ScriptDescription("A class to manipulate strings.")>
    Public Class CL_String

        Inherits ScriptClass

        Shared ArgSeparator As String = ","

        ''' <summary>
        ''' Resets the string class settings.
        ''' </summary>
        Public Shared Sub Reset()
            ArgSeparator = ","
        End Sub

        <ScriptCommand("SetArgSeparator")>
        <ScriptDescription("Sets the argument separator.")>
        Private Function M_SetArgSeparator(ByVal argument As String) As String
            If argument.Length > 0 Then
                ArgSeparator = argument(0).ToString()
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Replace")>
        <ScriptDescription("Replaces a part of the string.")>
        Private Function F_Replace(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            Return args(0).Replace(args(1), args(2))
        End Function

        <ScriptConstruct("IndexOf")>
        <ScriptDescription("Returns the first index of a char in a string.")>
        Private Function F_IndexOf(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            Return ToString(args(0).IndexOf(args(1)))
        End Function

        <ScriptConstruct("LastIndexOf")>
        <ScriptDescription("Returns the last index of a char in a string.")>
        Private Function F_LastIndexOf(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            Return ToString(args(0).LastIndexOf(args(1)))
        End Function

        <ScriptConstruct("Remove")>
        <ScriptDescription("Removes part of a string.")>
        Private Function F_Remove(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            If args.Length = 2 Then
                Return args(0).Remove(Int(args(1)))
            ElseIf args.Length >= 3 Then
                Return args(0).Remove(Int(args(1)), Int(args(2)))
            End If

            Return Core.Null
        End Function

        <ScriptConstruct("Trim")>
        <ScriptDescription("Trims chars at the start/end of a string.")>
        Private Function F_Trim(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            If args.Length = 1 Then
                Return args(0).Trim()
            ElseIf args.Length >= 2 Then
                Dim trimChars = args(1).ToCharArray()
                Return args(0).Trim(trimChars)
            End If

            Return Core.Null
        End Function

        <ScriptConstruct("Split")>
        <ScriptDescription("Splits the string and returns an item by index.")>
        Private Function F_Split(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            Return args(0).Split(args(1))(Int(args(2)))
        End Function

        <ScriptConstruct("Char")>
        <ScriptDescription("Returns a char in the string sequence.")>
        Private Function F_Char(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            Return args(0)(Int(args(1))).ToString()
        End Function

        <ScriptConstruct("Length")>
        <ScriptDescription("Returns the length of a string.")>
        Private Function F_Length(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            Return ToString(args(0).Length)
        End Function

        <ScriptConstruct("ChrW")>
        <ScriptDescription("Converts an integer into the char representing the unicode value.")>
        Private Function F_ChrW(ByVal argument As String) As String
            Dim chars() As String = argument.Split(CChar(","))
            Dim output As String = ""
            For Each c As String In chars
                If IsNumeric(c) = True Then
                    output &= ChrW(CInt(c))
                End If
            Next
            Return output
        End Function

        <ScriptConstruct("First")>
        <ScriptDescription("Returns the first char of a string.")>
        Private Function F_First(ByVal argument As String) As String
            If argument.Length > 0 Then
                Return argument(0).ToString()
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Last")>
        <ScriptDescription("Returns the last char of a string.")>
        Private Function F_Last(ByVal argument As String) As String
            If argument.Length > 0 Then
                Return argument(argument.Length - 1).ToString()
            Else
                Return Core.Null
            End If
        End Function

        <ScriptConstruct("Reverse")>
        <ScriptDescription("Reverses the string.")>
        Private Function F_Reverse(ByVal argument As String) As String
            If argument.Length > 1 Then
                Return String.Join("", argument.Reverse().ToArray())
            Else
                Return argument
            End If
        End Function

        <ScriptConstruct("StartsWith")>
        <ScriptDescription("Returns if a string starts with a string.")>
        Private Function F_StartsWith(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            Return ToString(args(0).StartsWith(args(1)))
        End Function

        <ScriptConstruct("EndsWith")>
        <ScriptDescription("Returns if a string ends with a string.")>
        Private Function F_EndsWith(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            Return ToString(args(0).EndsWith(args(1)))
        End Function

        <ScriptConstruct("Contains")>
        <ScriptDescription("Returns if a string contains another string.")>
        Private Function F_Contains(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            Return ToString(args(0).Contains(args(1)))
        End Function

        <ScriptConstruct("Regex")>
        <ScriptDescription("Returns if a string matches a regex pattern.")>
        Private Function F_Regex(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            Return ToString(Text.RegularExpressions.Regex.IsMatch(args(0), args(1)))
        End Function

        <ScriptConstruct("ToUpper")>
        <ScriptDescription("Converts all chars in a string to their upper case parts.")>
        Private Function F_ToUpper(ByVal argument As String) As String
            Return argument.ToUpper()
        End Function

        <ScriptConstruct("ToLower")>
        <ScriptDescription("Converts all chars in a string to their lower case parts.")>
        Private Function F_ToLower(ByVal argument As String) As String
            Return argument.ToLower()
        End Function

        <ScriptConstruct("SubString")>
        <ScriptDescription("Returns a sub string of a string.")>
        Private Function F_SubString(ByVal argument As String) As String
            Dim args As String() = argument.Split(CChar(ArgSeparator))

            If args.Length = 2 Then
                Return args(0).Substring(Int(args(1)))
            ElseIf args.Length >= 3 Then
                Return args(0).Substring(Int(args(1)), Int(args(2)))
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Empty")>
        <ScriptDescription("Returns an empty string.")>
        Private Function F_Empty(ByVal argument As String) As String
            Return ""
        End Function

    End Class

End Namespace