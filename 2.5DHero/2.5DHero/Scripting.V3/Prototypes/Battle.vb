Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="battle")>
    Friend NotInheritable Class Battle

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="reset")>
        Public Shared Function Reset(This As Object, parameters As Object()) As Object

            BattleSystem.BattleScreen.ResetVars()
            Return Nothing

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="setCanRun")>
        Public Shared Function SetCanRun(This As Object, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(Boolean)}) Then

                BattleSystem.BattleScreen.CanRun = CType(parameters(0), Boolean)

            End If

            Return Nothing

        End Function

        <ScriptFunction(ScriptFunctionType.Standard, VariableName:="getCanRun")>
        Public Shared Function GetCanRun(This As Object, parameters As Object()) As Object

            Return BattleSystem.BattleScreen.CanRun

        End Function

    End Class

End Namespace
