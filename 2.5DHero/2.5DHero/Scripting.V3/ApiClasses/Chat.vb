Imports Pokemon3D.Scripting
Imports Pokemon3D.Scripting.Adapters
Imports Pokemon3D.Scripting.Types

Namespace Scripting.V3.ApiClasses

    <ApiClass("Chat")>
    Friend NotInheritable Class ChatWrapper

        Public Shared Function clear(processor As ScriptProcessor, parameters As SObject()) As SObject
            Chat.ClearChat()
            Return ScriptInAdapter.GetUndefined(processor)
        End Function

    End Class

End Namespace
