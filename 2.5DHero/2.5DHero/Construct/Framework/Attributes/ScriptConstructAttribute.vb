Namespace Construct.Framework

    ''' <summary>
    ''' The attribute to supply meta information for a script construct.
    ''' </summary>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False)>
    Public Class ScriptConstructAttribute

        Inherits ScriptSubAttribute

        ''' <summary>
        ''' Initializes a new instance of the ScriptConstructAttribute class.
        ''' </summary>
        ''' <param name="name">The name of the script construct.</param>
        Public Sub New(ByVal name As String)
            MyBase.New(name, ScriptSub.SubTypes.Construct)
        End Sub

    End Class

End Namespace