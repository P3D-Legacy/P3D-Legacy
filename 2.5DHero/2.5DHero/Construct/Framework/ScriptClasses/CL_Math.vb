Namespace Construct.Framework.Classes

    <ScriptClass("Math")>
    <ScriptDescription("A class to handle math operations.")>
    Public Class CL_Math

        Inherits ScriptClass

#Region "Constructs"

        <ScriptConstruct("Int")>
        <ScriptDescription("Returns an integer from an expression.")>
        Private Function F_Int(ByVal argument As String) As String
            Return ToString(Int(argument))
        End Function

        <ScriptConstruct("Sng")>
        <ScriptDescription("Returns a single from an expression.")>
        Private Function F_Sng(ByVal argument As String) As String
            Return ToString(Sng(argument))
        End Function

        <ScriptConstruct("Dbl")>
        <ScriptDescription("Returns a double from an expression.")>
        Private Function F_Dbl(ByVal argument As String) As String
            Return ToString(Dbl(argument))
        End Function

        <ScriptConstruct("Abs")>
        <ScriptDescription("Return the absolute from an expression.")>
        Private Function F_Abs(ByVal argument As String) As String
            Return ToString(Math.Abs(Dbl(argument)))
        End Function

        <ScriptConstruct("Ceiling")>
        <ScriptDescription("Return the ceiling from an expression.")>
        Private Function F_Ceiling(ByVal argument As String) As String
            Return ToString(Math.Ceiling(Dbl(argument)))
        End Function

        <ScriptConstruct("Floor")>
        <ScriptDescription("Return the floor from an expression.")>
        Private Function F_Floor(ByVal argument As String) As String
            Return ToString(Math.Floor(Dbl(argument)))
        End Function

#Region "GROUP:IsNumeric"

        <ScriptConstruct("IsInt")>
        <ScriptDescription("Return if an expression is an integer.")>
        Private Function F_IsInt(ByVal argument As String) As String
            Return ToString(Converter.IsNumeric(argument))
        End Function

        <ScriptConstruct("IsSng")>
        <ScriptDescription("Return if an expression is a single.")>
        Private Function F_IsSng(ByVal argument As String) As String
            Return F_IsInt(argument)
        End Function

        <ScriptConstruct("IsDbl")>
        <ScriptDescription("Return if an expression is a double.")>
        Private Function F_IsDbl(ByVal argument As String) As String
            Return F_IsInt(argument)
        End Function

#End Region

        <ScriptConstruct("Clamp")>
        <ScriptDescription("Clamps a number.")>
        Private Function F_Clamp(ByVal argument As String) As String
            Dim args() As String = argument.Split(CChar(","))
            Dim n As Double = Dbl(args(0))
            Dim min As Double = Dbl(args(1))
            Dim max As Double = Dbl(args(2))

            Return ToString(n.Clamp(min, max))
        End Function

        <ScriptConstruct("Rollover")>
        <ScriptDescription("Rolls over a number.")>
        Private Function F_Rollover(ByVal argument As String) As String
            Dim args() As String = argument.Split(CChar(","))
            Dim n As Double = Dbl(args(0))
            Dim min As Double = Dbl(args(1))
            Dim max As Double = Dbl(args(2))

            Dim diff As Double = (max - min) + 1

            If n > max Then
                While n > max
                    n -= diff
                End While
            ElseIf n < min Then
                While n < min
                    n += diff
                End While
            End If

            Return ToString(n)
        End Function

#End Region

    End Class

End Namespace