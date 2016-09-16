Imports Pokemon3D.Scripting
Imports Pokemon3D.Scripting.Adapters
Imports Pokemon3D.Scripting.Types

Namespace Scripting.V3.ApiClasses

    <ApiClass("battle")>
    Friend NotInheritable Class Battle

        Inherits ApiClass

        Private Shared Function resetVars(processor As ScriptProcessor, parameters As SObject()) As SObject

            BattleSystem.BattleScreen.ResetVars()

            Return ScriptInAdapter.GetUndefined(processor)

        End Function

        Private Shared Function setVar(processor As ScriptProcessor, parameters As SObject()) As SObject

            Dim netObjects As Object() = Nothing

            If EnsureTypeContract(parameters, {GetType(String), GetType(String)}, netObjects) Then

                Dim varName = CType(netObjects(0), String)
                Dim varValue = CType(netObjects(1), String)

                Select Case varName.ToLower()
                    Case "canrun"
                        BattleSystem.BattleScreen.CanRun = CBool(varValue)
                    Case "cancatch"
                        BattleSystem.BattleScreen.CanCatch = CBool(varValue)
                    Case "canblackout"
                        BattleSystem.BattleScreen.CanBlackout = CBool(varValue)
                    Case "canreceiveexp"
                        BattleSystem.BattleScreen.CanReceiveEXP = CBool(varValue)
                    Case "canuseitems"
                        BattleSystem.BattleScreen.CanUseItems = CBool(varValue)
                    Case "frontiertrainer"
                        Trainer.FrontierTrainer = CInt(varValue)
                    Case "divebattle"
                        BattleSystem.BattleScreen.DiveBattle = CBool(varValue)
                    Case "inversebattle"
                        BattleSystem.BattleScreen.IsInverseBattle = CBool(varValue)
                    Case "custombattlemusic"
                        BattleSystem.BattleScreen.CustomBattleMusic = varValue
                    Case "hiddenabilitychance"
                        Screen.Level.HiddenAbilityChance = CInt(varValue)
                End Select

            End If

            Return ScriptInAdapter.GetUndefined(processor)

        End Function

    End Class

End Namespace
