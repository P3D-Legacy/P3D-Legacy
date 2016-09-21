Imports Pokemon3D.Scripting.Adapters

Namespace Scripting.V3.Prototypes

    <ScriptPrototype(VariableName:="Trainer")>
    Friend NotInheritable Class TrainerWrapper

        <ScriptVariable(VariableName:="file")>
        Public file As String = ""

        Public Sub New() : End Sub

        Public Sub New(file As String)
            Me.file = file
        End Sub

        <ScriptFunction(ScriptFunctionType.Constructor, VariableName:="constructor")>
        Public Shared Function Constructor(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            If TypeContract.Ensure(parameters, {GetType(String)}) Then

                Dim trainerFile = CType(parameters(0), String)
                objLink.SetMember("file", trainerFile)

            End If

            Return NetUndefined.Instance

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="defeatMessage")>
        Public Shared Function GetDefeatMessage(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim wrapper = CType(This, TrainerWrapper)
            Dim trainer = New Trainer(wrapper.file)

            Return trainer.DefeatMessage

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="introMessage")>
        Public Shared Function GetIntroMessage(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim wrapper = CType(This, TrainerWrapper)
            Dim trainer = New Trainer(wrapper.file)

            Return trainer.IntroMessage

        End Function

        <ScriptFunction(ScriptFunctionType.Getter, VariableName:="outroMessage")>
        Public Shared Function GetOutroMessage(This As Object, objLink As ScriptObjectLink, parameters As Object()) As Object

            Dim wrapper = CType(This, TrainerWrapper)
            Dim trainer = New Trainer(wrapper.file)

            Return trainer.OutroMessage

        End Function

    End Class

End Namespace
