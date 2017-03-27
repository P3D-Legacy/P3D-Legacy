Namespace Construct.Framework

    ''' <summary>
    ''' The attribute to supply meta information for a script class.
    ''' </summary>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False)>
    Public Class ScriptClassAttribute

        Inherits Attribute

        Private _className As String

        ''' <summary>
        ''' Initializes a new instance of the ScriptClassAttribute class.
        ''' </summary>
        ''' <param name="className">The name of the script class.</param>
        Public Sub New(ByVal className As String)
            _className = className
        End Sub

        ''' <summary>
        ''' The name of the script class.
        ''' </summary>
        ''' <returns></returns>
        Public Property ClassName() As String
            Get
                Return _className
            End Get
            Set(value As String)
                _className = value
            End Set
        End Property

    End Class

End Namespace