Namespace Construct.Framework

    ''' <summary>
    ''' The attribute to supply meta information for a script sub.
    ''' </summary>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=False)>
    Public Class ScriptSubAttribute

        Inherits Attribute

        Private _name As String
        Private _requiredContext As ScriptContext = ScriptContext.All
        Private _subType As ScriptSub.SubTypes = ScriptSub.SubTypes.Unspecified

        ''' <summary>
        ''' Initializes a new instance of the ScriptSubAttribute class.
        ''' </summary>
        ''' <param name="name">The name of the script sub.</param>
        Public Sub New(ByVal name As String, ByVal subType As ScriptSub.SubTypes)
            _name = name
            _subType = subType
        End Sub

        ''' <summary>
        ''' The name of the script sub.
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
            End Set
        End Property

        ''' <summary>
        ''' The required context of this script sub to be executed.
        ''' </summary>
        ''' <returns></returns>
        Public Property RequiredContext As ScriptContext
            Get
                Return _requiredContext
            End Get
            Set(value As ScriptContext)
                _requiredContext = value
            End Set
        End Property

        ''' <summary>
        ''' The subtype of this script sub.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SubType As ScriptSub.SubTypes
            Get
                Return _subType
            End Get
        End Property

    End Class

End Namespace