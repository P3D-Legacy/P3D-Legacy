Namespace Construct.Framework.Classes

    <ScriptClass("Script")>
    <ScriptDescription("Supplies script operations.")>
    Public Class CL_Script

        Inherits ScriptClass

        <ScriptCommand("Start")>
        <ScriptDescription("Starts a script from a file.")>
        Private Function M_Start(ByVal argument As String) As String
            'May take up to 9 additional arguments.
            Dim args As String() = argument.Split(","c)
            Dim fileName As String = args(0)

            Controller.GetInstance().RunFromFile(fileName)

            If args.Length > 1 Then
                For i = 1 To args.Length - 1
                    Controller.GetInstance().ValueHandler.SetParameterValue(i - 1, args(i))
                Next
            End If

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("Text")>
        <ScriptDescription("Displays a text as a script.")>
        Private Function M_Text(ByVal argument As String) As String
            Controller.GetInstance().RunFromText(argument)

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("Run")>
        <ScriptDescription("Runs direct input.")>
        Private Function M_Run(ByVal argument As String) As String
            Controller.GetInstance().RunFromString(argument)

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("SetContext")>
        <ScriptDescription("Sets the script controller's context.")>
        Private Function M_SetContext(ByVal argument As String) As String
            Select Case argument.ToLower()
                Case "overworld"
                    Controller.GetInstance().Context = ScriptContext.Overworld
                Case "newgame"
                    Controller.GetInstance().Context = ScriptContext.NewGame
            End Select

            Return Core.Null
        End Function

        <ScriptConstruct("GetContext")>
        <ScriptDescription("Returns the current script context.")>
        Private Function F_GetContext(ByVal argument As String) As String
            Return Controller.GetInstance().Context.ToString()
        End Function

    End Class

End Namespace