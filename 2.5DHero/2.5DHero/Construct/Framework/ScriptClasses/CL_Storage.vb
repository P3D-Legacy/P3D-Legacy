Namespace Construct.Framework.Classes

    <ScriptClass("Storage")>
    <ScriptDescription("A class to store and read values.")>
    Public Class CL_Storage

        Inherits ScriptClass

#Region "Commands"

        <ScriptCommand("Set")>
        <ScriptDescription("Sets the content of a storage value.")>
        Private Function M_Set(ByVal argument As String) As String
            Dim data As List(Of String) = argument.Split(CChar(",")).ToList()

            If {"str", "string", "int", "integer", "sng", "single", "bool", "boolean", "dbl", "double"}.Contains(data(0).ToLower()) = True Then
                data.RemoveAt(0)
            End If

            Dim value As String = data(1)
            If data.Count > 2 Then
                For i = 2 To data.Count - 1
                    value &= "," & data(i)
                Next
            End If

            StorageHandler.GetInstance().SetValue(data(0), value)

            Return Core.Null
        End Function

        <ScriptCommand("Clear")>
        <ScriptDescription("Clears all storage values.")>
        Private Function M_Clear(ByVal argument As String) As String
            StorageHandler.GetInstance().Clear()

            Return Core.Null
        End Function

        <ScriptCommand("Update")>
        <ScriptDescription("Updates the content of a storage value.")>
        Private Function M_Update(ByVal argument As String) As String
            Dim identifier As String = ""
            Dim operation As String = ""

            Dim data As List(Of String) = argument.Split(CChar(",")).ToList()

            'Data format: identifier,operation,value

            If {"str", "string", "int", "integer", "sng", "single", "bool", "boolean", "dbl", "double"}.Contains(data(0).ToLower()) = True Then
                data.RemoveAt(0)
            End If

            identifier = data(0)
            operation = data(1)
            Dim value As String = data(2)
            If data.Count > 3 Then
                For i = 3 To data.Count - 1
                    value &= "," & data(i)
                Next
            End If

            With StorageHandler.GetInstance()
                Dim currentValue As String = .GetValue(identifier)

                Select Case operation.ToLower()
                    Case "+", "plus", "add", "addition"
                        If Converter.IsNumeric(currentValue) = True And Converter.IsNumeric(value) = True Then
                            'Do numeric addition:
                            .SetValue(identifier, ToString(Converter.ToDouble(currentValue) + Converter.ToDouble(value)))
                        Else
                            'Concatenate strings:
                            .SetValue(identifier, currentValue & value)
                        End If
                    Case "-", "minus", "subtract", "subtraction"
                        If Converter.IsNumeric(currentValue) = True And Converter.IsNumeric(value) = True Then
                            .SetValue(identifier, ToString(Converter.ToDouble(currentValue) - Converter.ToDouble(value)))
                        Else
                            Logger.Debug("027", "Cannot subtract " & value & " from " & currentValue & ".")
                        End If
                    Case "*", "multiply", "multiplication"
                        If Converter.IsNumeric(currentValue) = True And Converter.IsNumeric(value) = True Then
                            .SetValue(identifier, ToString(Converter.ToDouble(currentValue) * Converter.ToDouble(value)))
                        Else
                            Logger.Debug("028", "Cannot multiply " & value & " with " & currentValue & ".")
                        End If
                    Case "/", ":", "divide", "division"
                        If Converter.IsNumeric(currentValue) = True And Converter.IsNumeric(value) = True Then
                            .SetValue(identifier, ToString(Converter.ToDouble(currentValue) / Converter.ToDouble(value)))
                        Else
                            Logger.Debug("029", "Cannot divide " & currentValue & " by " & value & ".")
                        End If
                    Case Else
                        Logger.Debug("030", operation & " is not a valid storage operation.")
                End Select
            End With

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

        <ScriptConstruct("Get")>
        <ScriptDescription("Returns the content of a storage value.")>
        Private Function F_Get(ByVal argument As String) As String
            Dim data As String() = argument.Split(CChar(","))

            With StorageHandler.GetInstance()
                If data.Length = 2 Then
                    Return .GetValue(data(0), data(1))
                ElseIf data.Length = 1 Then
                    Return .GetValue(data(0))
                End If
            End With

            Logger.Debug("031", "Invalid storage argument format.")
            Return Core.Null
        End Function

        <ScriptConstruct("Count")>
        <ScriptDescription("Returns the amount of values in the storage.")>
        Private Function F_Count(ByVal argument As String) As String
            Return ToString(StorageHandler.GetInstance().Count)
        End Function

        <ScriptConstruct("Exists")>
        <ScriptDescription("Returns if a value exists in the storage.")>
        Private Function F_Exists(ByVal argument As String) As String
            Dim data As String() = argument.Split(CChar(","))

            With StorageHandler.GetInstance()
                If data.Length = 2 Then
                    Return ToString( .Exists(data(0) & "|" & data(1)))
                ElseIf data.Length = 1 Then
                    Return ToString( .Exists(data(0)))
                End If
            End With

            Logger.Debug("032", "Invalid storage argument format.")
            Return Core.Null
        End Function

#End Region

    End Class

End Namespace