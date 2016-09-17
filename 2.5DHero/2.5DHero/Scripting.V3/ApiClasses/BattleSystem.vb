Imports Pokemon3D.Scripting
Imports Pokemon3D.Scripting.Adapters
Imports Pokemon3D.Scripting.Types

Namespace Scripting.V3.ApiClasses

    <ApiClass("BattleSystem")>
    Friend NotInheritable Class BattleSystem

        Inherits ApiClass

        Public Shared Function getInstance(processor As ScriptProcessor, parameters As SObject()) As SObject

            Dim battle = New Prototypes.Battle()
            Return ScriptInAdapter.Translate(processor, battle)

        End Function

    End Class

End Namespace
