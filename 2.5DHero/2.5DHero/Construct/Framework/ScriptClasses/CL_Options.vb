Namespace Construct.Framework.Classes

    <ScriptClass("Options")>
    <ScriptDescription("A class to use the Choose Box dialogue.")>
    Public Class CL_Options

        Inherits ScriptClass

        Public Sub New()
            MyBase.New(needsCorrectPlayerOrientation:=True)
        End Sub

        <ScriptCommand("Show")>
        <ScriptDescription("Shows the choose box with an array of selections.")>
        Private Function M_Show(ByVal argument As String) As String
            'If needed, display textbox here:
            If Screen.ChooseBox.ShowTextBox = True Then
                If Not Screen.TextBox Is Nothing And Not Screen.TextBox.Text Is Nothing Then
                    If Screen.TextBox.Text.Length > 0 Then
                        Screen.TextBox.Showing = True
                    End If
                End If
            End If

            Dim Options() As String = argument.Split(CChar(","))
            Screen.ChooseBox.Show(Options, 0, True)

            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

        <ScriptCommand("SetCancelIndex")>
        <ScriptDescription("Sets the cancel index of the choose box (closes the choose box with that index when pressing E).")>
        Private Function M_SetCancelIndex(ByVal argument As String) As String
            Screen.ChooseBox.CancelIndex = Int(argument)
            Return Core.Null
        End Function

        <ScriptCommand("HideTextbox")>
        <ScriptDescription("Sets if the textbox should be visible when showing the next choose box.")>
        Private Function M_HideTextbox(ByVal argument As String) As String
            'With empty argument, hide the textbox as the command describes:
            If argument = "" Then
                argument = "true"
            End If

            If Converter.IsBoolean(argument) = True Then
                Screen.ChooseBox.ShowTextBox = Not Bool(argument)
            Else
                Screen.ChooseBox.ShowTextBox = False
            End If
            Return Core.Null
        End Function

        <ScriptConstruct("Index")>
        <ScriptDescription("Returns the last chosen choose box index.")>
        Private Function F_Index(ByVal argument As String) As String
            Return ToString(Screen.ChooseBox.result)
        End Function

        <ScriptConstruct("Result")>
        <ScriptDescription("Returns the last chosen choose box result.")>
        Private Function F_Result(ByVal argument As String) As String
            Return Screen.ChooseBox.resultString
        End Function

    End Class

End Namespace