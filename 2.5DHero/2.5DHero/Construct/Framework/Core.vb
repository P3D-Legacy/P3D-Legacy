Namespace Construct.Framework

    ''' <summary>
    ''' A class that bundles all script classes and is the main entry point for commands.
    ''' </summary>
    Public Class Core

#Region "Singleton Handler"

        Private Shared _singleton As Core = Nothing

        ''' <summary>
        ''' Returns the active instance of the ScriptFramework.
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function GetInstance() As Core
            If _singleton Is Nothing Then
                _singleton = New Core()
            End If
            Return _singleton
        End Function

#End Region

        ''' <summary>
        ''' The name of the language.
        ''' </summary>
        Public Const LANGUAGE_NAME As String = "Construct"

        Private Const CLASSES_NAMESPACE As String = "Construct.Framework.Classes"

        ''' <summary>
        ''' The default return value: "Null".
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property Null() As String
            Get
                Return "Null"
            End Get
        End Property

        Private _classes As New List(Of ScriptClass)

        Private Sub New()
            'When the singleton instance gets created, initialize the Classes enumeration:
            Initialize()
        End Sub

        ''' <summary>
        ''' Executes a script command.
        ''' </summary>
        ''' <param name="className">The name of the class.</param>
        ''' <param name="subName">The sub of the class.</param>
        ''' <param name="argument">Arguments for the script call.</param>
        Public Sub ExecuteCommand(ByVal className As String, ByVal subName As String, ByVal argument As String)
            Dim scriptClass As ScriptClass = GetClassByName(className)

            If scriptClass IsNot Nothing Then
                Dim scriptSub As ScriptSub = scriptClass.GetSubByName(subName, ScriptSub.SubTypes.Command)
                If scriptSub IsNot Nothing Then
                    'if the player's orientation needs to be corrected, this is done here.
                    'We insert a turnto command into the script with this method and call it with the next iteration.
                    If InsertPlayerOrientationCorrection(scriptClass, scriptSub) = False Then
                        scriptSub.Execute(Parser.EvaluateScriptExpression(argument))
                    End If
                End If
            End If
        End Sub

        Private Function InsertPlayerOrientationCorrection(ByVal calledScriptClass As ScriptClass, ByVal calledScriptSub As ScriptSub) As Boolean
            Dim calledLine As String = calledScriptClass.Name.ToLower() & "." & calledScriptSub.Name.ToLower()

            With Controller.GetInstance()
                If calledScriptClass.NeedsCorrectPlayerOrientation = True Then
                    If .OrientatePlayer = True Then
                        If .CorrectPlayerOrientation > -1 Then
                            If calledLine <> "player.turnto" Then
                                If Screen.Camera.GetPlayerFacingDirection() <> .CorrectPlayerOrientation Then
                                    If CType(Screen.Camera, OverworldCamera).ThirdPerson = False Then
                                        .ActiveScript.InsertNewLine(String.Format("@player.turnto({0})", .CorrectPlayerOrientation.ToString()))
                                        .ActiveScript.LinePointer -= 1
                                        .ResetPlayerOrientation()
                                        Return True
                                    End If
                                End If
                            End If

                            .ResetPlayerOrientation()
                        End If
                    End If
                End If
            End With

            Return False
        End Function

        ''' <summary>
        ''' Asssigns a new value to an EasyValue.
        ''' </summary>
        ''' <param name="valueName">The name of the value.</param>
        ''' <param name="valueContent">The new content of the value.</param>
        ''' <param name="valueOperator">Empty, or +-*/ operations to perform on the value.</param>
        Public Sub AssignValue(ByVal valueName As String, ByVal valueContent As String, ByVal valueOperator As String)
            Select Case valueOperator
                Case "" 'No value operator.
                    Dim parsedValue As String = Parser.EvaluateScriptExpression(valueContent)

                    Controller.GetInstance().ValueHandler.Value(valueName) = New ScriptValue(parsedValue)
                Case "+", "-", "*", "/"
                    'Value operators can only be used when a value with the name has been assigned prior:
                    If Controller.GetInstance().ValueHandler.ValueExists(valueName) = True Then
                        'Built a new assignment with this pattern:
                        '   <var1>+=10
                        'transforms to:
                        '   <var1>=<var1>+10
                        Dim newValueContent As String = "<" & valueName & ">" & valueOperator & valueContent
                        Dim parsedValue = Parser.EvaluateScriptExpression(newValueContent)

                        If Converter.IsNumeric(parsedValue) = True Then
                            'If it's an arithmetic expression, apply it:
                            Controller.GetInstance().ValueHandler.Value(valueName) = New ScriptValue(Converter.ToDouble(parsedValue).ToString())
                        ElseIf valueOperator = "+" Then
                            'If the operator is a +, then concatenate the strings:
                            Controller.GetInstance().ValueHandler.Value(valueName) = New ScriptValue(Controller.GetInstance().ValueHandler.Value(valueName).ToString() & Parser.EvaluateScriptExpression(valueContent))
                        Else
                            Logger.Debug("045", "The operation (" & valueOperator & ") cannot be performed on the assignment.")
                        End If
                    Else
                        Logger.Debug("046", "A value operator cannot be used on an unassigned EasyValue!")
                    End If
                Case "&"
                    If Controller.GetInstance().ValueHandler.ValueExists(valueName) = True Then
                        'Concatenates the new value with the old one:

                        Controller.GetInstance().ValueHandler.Value(valueName) = New ScriptValue(Controller.GetInstance().ValueHandler.Value(valueName).ToString() & Parser.EvaluateScriptExpression(valueContent))
                    Else
                        Logger.Debug("295", "A value operator cannot be used on an unassigned EasyValue!")
                    End If
                Case Else
                    Logger.Debug("047", "Wrong value operator used:  " & valueOperator)
            End Select
        End Sub

        ''' <summary>
        ''' Creates a new empty array with a given length.
        ''' </summary>
        ''' <param name="valueName">The value to store the empty array in.</param>
        ''' <param name="length">The length of the array.</param>
        Public Sub CreateNewEmptyArray(ByVal valueName As String, ByVal length As String)
            Dim lengthInt As Integer = Converter.ToInteger(Parser.EvaluateScriptExpression(length))
            Controller.GetInstance().ValueHandler.Value(valueName) = New ScriptArray(lengthInt)
        End Sub

        ''' <summary>
        ''' Assigns a new array to an EasyValue.
        ''' </summary>
        ''' <param name="valueName">The name of the value.</param>
        ''' <param name="assignment">The assignment.</param>
        Public Sub AssignArray(ByVal valueName As String, ByVal assignment As String)
            Controller.GetInstance().ValueHandler.Value(valueName) = New ScriptArray(Parser.EvaluateScriptExpression(assignment))
        End Sub

        ''' <summary>
        ''' Assigns a new value to an item in an array.
        ''' </summary>
        ''' <param name="valueName">The name of the value holding the array.</param>
        ''' <param name="assignment">The assignment.</param>
        ''' <param name="itemIndex">The index of the item to change.</param>
        Public Sub AssignArrayItem(ByVal valueName As String, ByVal assignment As String, ByVal itemIndex As String)
            Dim indexInt As Integer = Converter.ToInteger(Parser.EvaluateScriptExpression(itemIndex))
            Dim arrayValue = Controller.GetInstance().ValueHandler.Value(valueName)
            If arrayValue.ValueHolderType = ScriptValueHolder.Types.Array Then
                CType(arrayValue, ScriptArray).AssignArrayItem(indexInt, Parser.EvaluateScriptExpression(assignment))
            End If
        End Sub

        ''' <summary>
        ''' Returns a script class by its name.
        ''' </summary>
        ''' <param name="className">The class name.</param>
        ''' <returns></returns>
        Public Function GetClassByName(ByVal className As String) As ScriptClass
            For Each c As ScriptClass In _classes
                If c.Name.ToLower() = className.ToLower() Then
                    Return c
                End If
            Next

            Logger.Debug("048", "No class with the name " & className & " found.")
            Return Nothing
        End Function

        Private Sub Initialize()
            Dim sw As New Stopwatch()
            sw.Start()

            Dim types As Type() = Reflection.Assembly.GetAssembly(Me.GetType()).GetTypes()
            For Each t As Type In types
                Dim attributes = t.GetCustomAttributes(False)
                If attributes.Length > 0 Then
                    For Each att In attributes
                        If att.GetType() = GetType(ScriptClassAttribute) Then
                            _classes.Add(CType(Activator.CreateInstance(t), ScriptClass))
                            Exit For
                        End If
                    Next
                End If

                'If Not t.Namespace Is Nothing Then
                '    If t.Namespace.ToLower().EndsWith("." & CLASSES_NAMESPACE.ToLower()) Then
                '        If t.Name.ToLower().StartsWith("cl_") = True Then
                '            _classes.Add(CType(Activator.CreateInstance(t), ScriptClass))
                '        End If
                '    End If
                'End If
            Next

            sw.Stop()
            Logger.Debug("049", "Initialized " & LANGUAGE_NAME & " Framework Core in " & sw.ElapsedMilliseconds.ToString() & " milliseconds.")
        End Sub

    End Class

End Namespace