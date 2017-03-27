Namespace Construct.Framework.Classes

    <ScriptClass("Keyboard")>
    <ScriptDescription("A class to supply keyboard related operations.")>
    Public Class CL_Keyboard

        Inherits ScriptClass

#Region "Commands"

        <ScriptCommand("WaitForInput")>
        <ScriptDescription("Waits until a certain key on the Keyboard is pressed.")>
        Private Function M_WaitForInput(ByVal argument As String) As String
            Dim inputKey As Keys = Keys.None
            For Each k As Keys In [Enum].GetValues(GetType(Keys))
                If k.ToString().ToLower() = argument.ToLower() Then
                    inputKey = k
                    Exit For
                End If
            Next
            If KeyBoardHandler.GetPressedKeys().Contains(inputKey) Or inputKey = Keys.None Then
                ActiveLine.Preserve = False
            Else
                ActiveLine.Preserve = True
            End If
            ActiveLine.EndExecutionFrame = True

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

#End Region

    End Class

End Namespace