Namespace Scripting.V3.ApiClasses

    <AttributeUsage(AttributeTargets.Class)>
    Friend Class ApiClassAttribute

        Inherits Attribute

        Public Property ClassName As String

        Public Sub New(className As String)
            Me.ClassName = className
        End Sub

    End Class

End Namespace
