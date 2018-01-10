Imports Kolben
Imports Kolben.Adapters
Imports Kolben.Types

Namespace Scripting.V3.ApiClasses

    <ApiClass("Chat")>
    Friend NotInheritable Class ChatWrapper

        Inherits ApiClass

        <ApiMethodSignature>
        Public Shared Function clear(processor As ScriptProcessor, parameters As SObject()) As SObject
            Chat.ClearChat()
            Return ScriptInAdapter.GetUndefined(processor)
        End Function

    End Class

End Namespace
