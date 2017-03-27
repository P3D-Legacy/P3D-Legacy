Namespace Construct.Framework

    ''' <summary>
    ''' The attribute to supply meta information for a script command.
    ''' </summary>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False)>
    Public Class ScriptCommandAttribute

        Inherits ScriptSubAttribute

        ''' <summary>
        ''' Initializes a new instance of the ScriptCommandAttribute class.
        ''' </summary>
        ''' <param name="name">The name of the script command.</param>
        Public Sub New(ByVal name As String)
            MyBase.New(name, ScriptSub.SubTypes.Command)
        End Sub

    End Class

End Namespace