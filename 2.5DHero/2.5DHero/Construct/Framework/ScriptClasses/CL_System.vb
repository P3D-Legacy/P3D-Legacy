Namespace Construct.Framework.Classes

    <ScriptClass("System")>
    <ScriptDescription("A class to access system functions.")>
    Public Class CL_System

        Inherits ScriptClass

#Region "Commands"

        <ScriptCommand("Save", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Saves the game.")>
        Private Function M_Save(ByVal argument As String) As String
            Game.Core.Player.SaveGame()

            Return Core.Null
        End Function

        <ScriptCommand("EndNewGame", RequiredContext:=ScriptContext.NewGame)>
        <ScriptDescription("Ends the New Game sequence.")>
        Private Function M_EndNewGame(ByVal argument As String) As String
            Dim args As String() = argument.Split(","c)

            Screens.MainMenu.NewNewGameScreen.EndNewGame(args(0), Sng(args(1)), Sng(args(2)), Sng(args(3)), Int(args(4)))

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("Random")>
        <ScriptDescription("Returns a random number between min and max.")>
        Private Function F_Random(ByVal argument As String) As String
            Dim minRange As Integer = 1
            Dim maxRange As Integer = 2
            If argument <> "" Then
                If argument.Contains(",") = True Then
                    minRange = Int(argument.GetSplit(0))
                    maxRange = Int(argument.GetSplit(1))
                Else
                    If IsNumeric(argument) = True Then
                        maxRange = Int(argument)
                    End If
                End If
            End If
            Return ToString(Random.Next(minRange, maxRange + 1))
        End Function

        <ScriptConstruct("IsInsightScript")>
        <ScriptDescription("Returns if the script was started by a sighting of an NPC.")>
        Private Function F_IsInsightScript(ByVal argument As String) As String
            Return ToString(Controller.GetInstance().IsInsightScriptRunning)
        End Function

        <ScriptConstruct("LastInput")>
        <ScriptDescription("Returns the last input the user has taken on the input screen.")>
        Private Function F_LastInput(ByVal argument As String) As String
            Return InputScreen.LastInput
        End Function

        <ScriptConstruct("ScriptLevel")>
        <ScriptDescription("Returns the script level, starting with 0 for the first script.")>
        Private Function F_ScriptLevel(ByVal argument As String) As String
            Return ToString(Controller.GetInstance().ScriptCount - 1)
        End Function

        <ScriptConstruct("Compare")>
        <ScriptDescription("Executes the argument as comparison and returns the result.")>
        Private Function F_Compare(ByVal argument As String) As String
            Return ToString(Parser.ConditionalComparison(argument))
        End Function

        <ScriptConstruct("Null")>
        <ScriptDescription("Returns the default Null variable.")>
        Private Function F_Null(ByVal argument As String) As String
            Return Core.Null
        End Function

        <ScriptConstruct("Bool")>
        <ScriptDescription("Converts the argument to a booleanic value.")>
        Private Function F_Bool(ByVal argument As String) As String
            Return ToString(Bool(argument))
        End Function

#End Region

    End Class

End Namespace