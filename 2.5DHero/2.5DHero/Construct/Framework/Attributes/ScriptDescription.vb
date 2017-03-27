Namespace Construct.Framework

    ''' <summary>
    ''' The attribute to supply a description to a script object.
    ''' </summary>
    <AttributeUsage(AttributeTargets.Class Or AttributeTargets.Method, AllowMultiple:=False)>
    Public Class ScriptDescription

        Inherits Attribute

        Private _description As String

        ''' <summary>
        ''' Initializes a new instance of the ScriptDescriptionAttribute class.
        ''' </summary>
        ''' <param name="description">The description of the script object.</param>
        Public Sub New(ByVal description As String)
            _description = description
        End Sub

        Public Property Description() As String
            Get
                Return _description
            End Get
            Set(value As String)
                _description = value
            End Set
        End Property

    End Class

End Namespace