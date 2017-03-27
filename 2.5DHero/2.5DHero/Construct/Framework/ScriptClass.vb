Namespace Construct.Framework

    ''' <summary>
    ''' An enumeration of script subs that are targeted by the same class name.
    ''' </summary>
    Public MustInherit Class ScriptClass

        Inherits List(Of ScriptSub)

        Private _name As String = ""

        ''' <summary>
        ''' If this class requires the player to look into the correct direction.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NeedsCorrectPlayerOrientation() As Boolean = False

        ''' <summary>
        ''' The name of this script class.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Name() As String
            Get
                Return _name
            End Get
        End Property

        ''' <summary>
        ''' Returns a script sub by its name.
        ''' </summary>
        ''' <param name="subName">The name of the script sub.</param>
        ''' <param name="subType">The type of the script sub.</param>
        ''' <returns></returns>
        Public Function GetSubByName(ByVal subName As String, ByVal subType As ScriptSub.SubTypes) As ScriptSub
            For Each scriptSub In Me
                If scriptSub.Name.ToLower() = subName.ToLower() And scriptSub.SubType = subType Then
                    Return scriptSub
                End If
            Next

            Logger.Debug("019", "No sub with the name " & subName & " in class " & _name & " found.")
            Return Nothing
        End Function

        ''' <summary>
        ''' Creates a new instance of a script class.
        ''' </summary>
        Public Sub New()
            'Get the name of the class by reflecting the ScriptClassAttribute:
            For Each att In Me.GetType().GetCustomAttributes(False)
                If att.GetType() = GetType(ScriptClassAttribute) Then
                    Dim classAtt = CType(att, ScriptClassAttribute)
                    _name = classAtt.ClassName
                    Exit For
                End If
            Next

            Initialize()
        End Sub

        ''' <summary>
        ''' Creates a new instance of a script class.
        ''' </summary>
        ''' <param name="needsCorrectPlayerOrientation">If this class needs the correct player orientation.</param>
        Public Sub New(ByVal needsCorrectPlayerOrientation As Boolean)
            Me.New()
            _NeedsCorrectPlayerOrientation = needsCorrectPlayerOrientation
        End Sub

        Private Sub Initialize()
            'Search through the methods in this class and add those with the correct attributes:
            For Each methodInfo As Reflection.MethodInfo In Me.GetType().GetMethods(Reflection.BindingFlags.NonPublic Or
                                                                                    Reflection.BindingFlags.Instance)
                For Each att In methodInfo.GetCustomAttributes(False)
                    If att.GetType() = GetType(ScriptCommandAttribute) Or
                       att.GetType() = GetType(ScriptConstructAttribute) Then

                        Add(New ScriptSub(Me, methodInfo, CType(att, ScriptSubAttribute)))
                    End If
                Next
            Next
        End Sub

        ''' <summary>
        ''' Returns the active script line for convenience reasons into the current scope.
        ''' </summary>
        ''' <returns></returns>
        Protected ReadOnly Property ActiveLine() As ScriptLine
            Get
                Return Controller.GetInstance().ActiveScript.ActiveLine
            End Get
        End Property

        ''' <summary>
        ''' Converts an argument into an argument array. It detects if an array declaration is used -> {}.
        ''' </summary>
        ''' <returns></returns>
        Protected Function GetArgumentArray(ByVal arg As String) As String()
            If arg.StartsWith("{") = True And arg.EndsWith("}") And arg.Length > 2 Then
                'Try to parse as array:
                Return Parser.ParseArray(arg).ToArray()
            Else
                'Parse as normal argument list:
                Return arg.Split(CChar(","))
            End If
        End Function

        ''' <summary>
        ''' If an argument for a function can be converted to an array.
        ''' </summary>
        ''' <param name="arg">The argument.</param>
        ''' <returns></returns>
        Protected Function ArgumentIsArray(ByVal arg As String) As Boolean
            Return arg.StartsWith("{") = True And arg.EndsWith("}") And arg.Length > 2
        End Function

        Public Property WorkValues() As List(Of String)
            Get
                Return ActiveLine.workValues
            End Get
            Set(value As List(Of String))
                ActiveLine.workValues = value
            End Set
        End Property

        '///////////////////////////////
        '/////// Converts types: ///////
        '///////////////////////////////

        ''' <summary>
        ''' Converts the expression into an integer.
        ''' </summary>
        ''' <param name="expression">The expression to convert.</param>
        ''' <returns></returns>
        Protected Function Int(ByVal expression As String) As Integer
            Return Converter.ToInteger(expression)
        End Function

        ''' <summary>
        ''' Converts the expression into a single.
        ''' </summary>
        ''' <param name="expression">The expression to convert.</param>
        ''' <returns></returns>
        Protected Function Sng(ByVal expression As String) As Single
            Return Converter.ToSingle(expression)
        End Function

        ''' <summary>
        ''' Converts the expression into a double.
        ''' </summary>
        ''' <param name="expression">The expression to convert.</param>
        ''' <returns></returns>
        Protected Function Dbl(ByVal expression As String) As Double
            Return Converter.ToDouble(expression)
        End Function

        ''' <summary>
        ''' Converts the expression into a boolean.
        ''' </summary>
        ''' <param name="expression">The expression to convert.</param>
        ''' <returns></returns>
        Protected Function Bool(ByVal expression As String) As Boolean
            Return Converter.ToBoolean(expression)
        End Function

        ''' <summary>
        ''' Converts the boolean into a string.
        ''' </summary>
        ''' <param name="expression">The boolean.</param>
        ''' <returns></returns>
        Protected Shadows Function ToString(ByVal expression As Boolean) As String
            If expression = True Then
                Return "true"
            Else
                Return "false"
            End If
        End Function

        ''' <summary>
        ''' Converts the single into a string.
        ''' </summary>
        ''' <param name="expression">The single.</param>
        ''' <returns></returns>
        Protected Shadows Function ToString(ByVal expression As Single) As String
            Return expression.ToString().ReplaceDecSeparator()
        End Function

        ''' <summary>
        ''' Converts the double into a string.
        ''' </summary>
        ''' <param name="expression">The double.</param>
        ''' <returns></returns>
        Protected Shadows Function ToString(ByVal expression As Double) As String
            Return expression.ToString().ReplaceDecSeparator()
        End Function

        ''' <summary>
        ''' Converts the integer into a string.
        ''' </summary>
        ''' <param name="expression">The integer.</param>
        ''' <returns></returns>
        Protected Shadows Function ToString(ByVal expression As Integer) As String
            Return expression.ToString()
        End Function

    End Class

End Namespace