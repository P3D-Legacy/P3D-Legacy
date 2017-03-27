Namespace Construct.Framework.Classes

    <ScriptClass("Phone")>
    <ScriptDescription("A class to access the Pokégear's phone functions.")>
    Public Class CL_Phone

        Inherits ScriptClass

        <ScriptConstruct("CallFlag", RequiredContext:=ScriptContext.Overworld)>
        <ScriptDescription("Returns the current call flag of the pokegear.")>
        Private Function F_CallFlag(ByVal argument As String) As String
            Return GameJolt.PokegearScreen.Call_Flag
        End Function

    End Class

End Namespace