Namespace Construct.Framework.Classes

    <ScriptClass("Chat")>
    <ScriptDescription("A class to control game chat operations.")>
    Public Class CL_Chat

        Inherits ScriptClass

        <ScriptCommand("Clear")>
        <ScriptDescription("Clears the chat output.")>
        Private Function M_Clear(ByVal argument As String) As String
            Chat.ClearChat()

            Return Core.Null
        End Function

    End Class

End Namespace