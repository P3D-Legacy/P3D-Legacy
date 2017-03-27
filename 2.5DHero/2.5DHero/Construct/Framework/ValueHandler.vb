Imports System.Text.RegularExpressions

Namespace Construct.Framework

    ''' <summary>
    ''' Handles script values that are used by the EasyValue feature.
    ''' </summary>
    Public Class ValueHandler

        'List of values: Name|Value
        Private _values As New Dictionary(Of String, ScriptValueHolder)

        ''' <summary>
        ''' Gets or sets the content of an EasyValue.
        ''' </summary>
        ''' <param name="identifier">The identifier of the EasyValue.</param>
        ''' <returns></returns>
        Public Property Value(ByVal identifier As String) As ScriptValueHolder
            Get
                Dim listName As String = identifier.ToLower()

                If _values.ContainsKey(listName) = True Then
                    Return _values(listName)
                Else
                    Return New ScriptValue(Core.Null)
                End If
            End Get
            Set(value As ScriptValueHolder)
                Dim listName As String = identifier.ToLower()

                If IsAllowedName(listName) = True Then
                    If _values.ContainsKey(listName) = False Then
                        _values.Add(listName, value)
                    Else
                        _values(listName) = value
                    End If
                Else
                    Logger.Debug("012", listName & " is not a valid identifier.")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Sets a parameter value starting with a $.
        ''' </summary>
        ''' <param name="index">The index of the parameter.</param>
        ''' <param name="value">The value of the parameter.</param>
        Public Sub SetParameterValue(ByVal index As Integer, ByVal value As String)
            _values.Add("$" & index.ToString(), New ScriptValue(value))
        End Sub

        ''' <summary>
        ''' Returns if a value with a specific identifier has been specified yet.
        ''' </summary>
        ''' <param name="identifier">The idenfitier to look for.</param>
        ''' <returns></returns>
        Public ReadOnly Property ValueExists(ByVal identifier As String) As Boolean
            Get
                Return _values.ContainsKey(identifier.ToLower())
            End Get
        End Property

        Private Function IsAllowedName(ByVal name As String) As Boolean
            If name.StartsWith("$") = True Then
                Return False
            End If
            Return {"and", "or", "to", "step", "null"}.Contains(name.ToLower()) = False
        End Function

    End Class

    ''' <summary>
    ''' The base class for value and array classes.
    ''' </summary>
    Public MustInherit Class ScriptValueHolder

        Public Enum Types
            Value
            Array
        End Enum

        Protected _rawValue As String = ""
        Protected _type As Types = Types.Value

        Protected Sub New(ByVal _type As Types)
            Me._type = _type
        End Sub

        ''' <summary>
        ''' The type of value holder.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ValueHolderType() As Types
            Get
                Return _type
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents a single string value.
    ''' </summary>
    Public Class ScriptValue

        Inherits ScriptValueHolder

        ''' <summary>
        ''' Creates a new instance of the script value.
        ''' </summary>
        ''' <param name="value">The raw value.</param>
        Public Sub New(ByVal value As String)
            MyBase.New(ScriptValueHolder.Types.Value)

            _rawValue = value
        End Sub

        ''' <summary>
        ''' Returns the value of this value holder.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return _rawValue
        End Function

    End Class

    ''' <summary>
    ''' Represents a string array.
    ''' </summary>
    Public Class ScriptArray

        Inherits ScriptValueHolder

        Private _internalArray As List(Of String)

        ''' <summary>
        ''' Creates a new instance of the script array.
        ''' </summary>
        ''' <param name="value">The raw value.</param>
        Public Sub New(ByVal value As String)
            MyBase.New(ScriptValueHolder.Types.Array)

            _rawValue = value

            createArrayFromRaw()
        End Sub

        Public Sub New(ByVal length As Integer)
            MyBase.New(ScriptValueHolder.Types.Array)

            _rawValue = ""

            'Create the empty array with the passed in length:
            _internalArray = New String(length - 1) {}.ToList()
        End Sub

        ''' <summary>
        ''' Get the array value.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Array() As String()
            Get
                Return _internalArray.ToArray()
            End Get
        End Property

        Public Sub AssignArrayItem(ByVal index As Integer, ByVal value As String)
            _internalArray(index) = value
        End Sub

        ''' <summary>
        ''' Returns the string representation of this array.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return Parser.ArrayToString(_internalArray.ToArray())
        End Function

        Private Sub createArrayFromRaw()
            Me._internalArray = Parser.ParseArray(_rawValue)
        End Sub

    End Class

End Namespace