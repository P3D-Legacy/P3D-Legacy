Namespace Construct.Framework

    ''' <summary>
    ''' Represents a command or construct sub.
    ''' </summary>
    Public Class ScriptSub

        ''' <summary>
        ''' Represents a script call with return parameter.
        ''' </summary>
        ''' <param name="argument">The argument that gets passed to the script call.</param>
        ''' <returns></returns>
        Public Delegate Function DMethodReference(ByVal argument As String) As String

        Public Enum SubTypes
            Unspecified = 0
            Command = 1
            Construct = 2
        End Enum

        Private _parent As ScriptClass
        Private _method As DMethodReference = Nothing
        Private _subAttribute As ScriptSubAttribute = Nothing

        ''' <summary>
        ''' Creates a new instance of the ScriptSub class.
        ''' </summary>
        ''' <param name="parent">The parent script class.</param>
        ''' <param name="methodInfo">The method info.</param>
        Public Sub New(ByVal parent As ScriptClass, ByVal methodInfo As Reflection.MethodInfo, ByVal subAttribute As ScriptSubAttribute)
            _parent = parent
            _subAttribute = subAttribute

            _method = CType(System.Delegate.CreateDelegate(GetType(DMethodReference), parent, methodInfo.Name), DMethodReference)
        End Sub

        ''' <summary>
        ''' The sub type of this sub.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SubType() As SubTypes
            Get
                Return _subAttribute.SubType
            End Get
        End Property

        ''' <summary>
        ''' The name of this script sub.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Name() As String
            Get
                Return _subAttribute.Name
            End Get
        End Property

        ''' <summary>
        ''' Executes this sub with the given argument.
        ''' </summary>
        ''' <param name="argument">The argument.</param>
        Public Function Execute(ByVal argument As String) As String
            'Check if the set script context complies with the currently set script context:
            If _subAttribute.RequiredContext <> ScriptContext.All Then
                If _subAttribute.RequiredContext <> Controller.GetInstance().Context Then
                    Logger.Log("301", Logger.LogTypes.Warning, _parent.Name & "." & Name & " cannot be executed in the script context: " & Controller.GetInstance().Context.ToString())
                    Return Core.Null
                End If
            End If

            Return _method(argument)
        End Function

    End Class

End Namespace