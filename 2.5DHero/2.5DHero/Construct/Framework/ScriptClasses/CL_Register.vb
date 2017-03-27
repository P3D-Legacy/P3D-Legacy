Namespace Construct.Framework.Classes

    <ScriptClass("Register")>
    <ScriptDescription("A class to use the game's register system.")>
    Public Class CL_Register

        Inherits ScriptClass

#Region "Commands"

#Region "New"

        <ScriptCommand("New")>
        <ScriptDescription("Registers a new register.")>
        Private Function M_New(ByVal argument As String) As String
            'Formats for this argument:
            'No commas: empty value register
            'One comma: value register with initial value.

            Dim registerName As String = ""
            Dim registerValue As String = ""

            Dim data() As String = argument.Split(CChar(","))
            If data.Length = 3 Then
                registerName = data(0)
                registerValue = data(2)
            ElseIf data.Length = 2 Then
                registerName = data(0)
                registerValue = data(1)
            ElseIf data.Length = 1 Then
                registerName = data(0)
                registerValue = ""
            Else
                Logger.Debug("056", "Wrong format for a register argument.")
                Return Core.Null
            End If

            RegisterHandler.NewRegister(registerName, registerValue)

            Return Core.Null
        End Function

        <ScriptCommand("NewTime")>
        <ScriptDescription("Register a new register that runs out automatically after a specified amount of time.")>
        Private Function M_NewTime(ByVal argument As String) As String
            'Argument format:
            'registerName,timeDiff,timeFormat

            Dim data() As String = argument.Split(CChar(","))

            If data.Length = 3 Then
                Dim validFormats As String() = {"day", "days", "hour", "hours", "minute", "minutes", "second", "seconds", "year", "years", "week", "weeks", "month", "months"}

                Dim registerName As String = data(0)
                Dim timeValue As Integer = Int(data(1))
                Dim timeUnit As String = data(2)

                If validFormats.Contains(timeUnit.ToLower()) = True Then
                    If timeValue >= 0 Then
                        RegisterHandler.NewRegister(registerName, timeUnit, timeValue.ToString())
                    Else
                        Logger.Debug("057", "Wrong format for a time register argument.")
                    End If
                Else
                    Logger.Debug("058", "Wrong format for a time register argument.")
                End If
            Else
                Logger.Debug("059", "Wrong format for a time register argument.")
            End If

            Return Core.Null
        End Function

        'We don't want to break the probably most used command here, so we allow @register.register.
        'The thing is "@register.register" reads very badly, so we use @register.new instead.
        <ScriptCommand("Register")>
        <ScriptDescription("Registers a new register.")>
        Private Function M_Register(ByVal argument As String) As String
            Logger.Debug("060", "@register.register is deprecated. Use @register.new instead.")

            M_New(argument)
            Return Core.Null
        End Function

        '(See above, this exist for compatibility)
        <ScriptCommand("RegisterTime")>
        <ScriptDescription("Register a new register that runs out automatically after a specified amount of time.")>
        Private Function M_RegisterTime(ByVal argument As String) As String
            Logger.Debug("061", "@register.registertime is deprecated. Use @register.newtime instead.")

            M_NewTime(argument)
            Return Core.Null
        End Function

#End Region

#Region "Remove"

        <ScriptCommand("Remove")>
        <ScriptDescription("Removes a register.")>
        Private Function M_Remove(ByVal argument As String) As String
            RegisterHandler.RemoveRegister(argument)

            Return Core.Null
        End Function

        'Again, for compatibility reasons.
        <ScriptCommand("Unregister")>
        <ScriptDescription("Removes a register.")>
        Private Function M_Unregister(ByVal argument As String) As String
            Logger.Debug("062", "@register.unregister is deprecated. Use @register.remove instead.")
            M_Remove(argument)

            Return Core.Null
        End Function

#End Region

        <ScriptCommand("Change")>
        <ScriptDescription("Changes the content of a register.")>
        Private Function M_Change(ByVal argument As String) As String
            If argument.Contains(",") = True Then
                Dim registerName As String = argument.Remove(argument.IndexOf(","))
                Dim registerValue As String = argument.Remove(0, argument.IndexOf(",") + 1)

                RegisterHandler.SetRegisterValue(registerName, registerValue)
            Else
                Logger.Debug("063", "Wrong format for a register argument.")
            End If

            Return Core.Null
        End Function

#End Region

#Region "Constructs"

#Region "Exists"

        <ScriptConstruct("Exists")>
        <ScriptDescription("Returns if a register exists.")>
        Private Function F_Exists(ByVal argument As String) As String
            Return ToString(RegisterHandler.IsRegistered(argument))
        End Function

        'Again, for compatibility reasons.
        <ScriptConstruct("Registered")>
        <ScriptDescription("Returns if a register exists.")>
        Private Function F_Registered(ByVal argument As String) As String
            Logger.Debug("064", "<register.registered> is deprecated. Use <register.exists> instead.")
            Return F_Exists(argument)
        End Function

#End Region

        <ScriptConstruct("Value")>
        <ScriptDescription("Returns the value of a register.")>
        Private Function F_Value(ByVal argument As String) As String
            Return RegisterHandler.GetRegisterValue(argument)
        End Function

#End Region

    End Class

End Namespace