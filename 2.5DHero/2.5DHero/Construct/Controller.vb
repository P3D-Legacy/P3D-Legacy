Namespace Construct

    ''' <summary>
    ''' The contexts that the script controller can be assigned with.
    ''' </summary>
    Public Enum ScriptContext
        ''' <summary>
        ''' Describes all script contexts.
        ''' </summary>
        All = 0
        ''' <summary>
        ''' The Overworld during gameplay.
        ''' </summary>
        Overworld = 1
        ''' <summary>
        ''' The NewGame screen.
        ''' </summary>
        NewGame = 2
    End Enum

    ''' <summary>
    ''' Handles general script workflows.
    ''' </summary>
    Public Class Controller

#Region "Singleton Handler"

        Private Shared _singleton As Controller = Nothing

        ''' <summary>
        ''' Returns the active instance of the ScriptController.
        ''' </summary>
        Public Shared Function GetInstance() As Controller
            If _singleton Is Nothing Then
                _singleton = New Controller()
            End If
            Return _singleton
        End Function

#End Region

        Private _scripts As New List(Of Script)

        ''' <summary>
        ''' The current context of the script controller.
        ''' </summary>
        Public Property Context() As ScriptContext = ScriptContext.Overworld

        ''' <summary>
        ''' The currently active Value Handler.
        ''' </summary>
        Public ReadOnly Property ValueHandler() As Framework.ValueHandler
            Get
                If _tempAddScript IsNot Nothing Then
                    Return _tempAddScript.ValueHandler
                End If
                Return ActiveScript.ValueHandler
            End Get
        End Property

        ''' <summary>
        ''' The amount of scripts on the current stack.
        ''' </summary>
        Public ReadOnly Property ScriptCount() As Integer
            Get
                Return _scripts.Count
            End Get
        End Property

        ''' <summary>
        ''' If an insight script is running right now.
        ''' </summary>
        Public ReadOnly Property IsInsightScriptRunning() As Boolean

        Private _scriptStartDelay As Single = 0F

        ''' <summary>
        ''' If the player should get oriented into the correct direction if needed.
        ''' </summary>
        Public ReadOnly Property OrientatePlayer() As Boolean = False

        ''' <summary>
        ''' The orientation the player should face if the script does anything that needs the correct orientation.
        ''' </summary>
        Public Property CorrectPlayerOrientation() As Integer = -1

#Region "RunScripts"

        ''' <summary>
        ''' Types a script input can be represented in.
        ''' </summary>
        Public Enum ScriptInputTypes As Integer
            FromFile = 0
            TextDisplay = 1
            DirectScriptInput = 2
        End Enum

        ''' <summary>
        ''' Options that a script can be run with.
        ''' </summary>
        Public Enum ScriptRunOptions As Integer
            CheckDelay = 0
            InsightScript = 1
            OrientatePlayer = 2
        End Enum

        ''' <summary>
        ''' Runs a script from a generic input with specified input type.
        ''' </summary>
        ''' <param name="genericInput">The generic input string.</param>
        ''' <param name="inputType">The input type.</param>
        ''' <param name="options">Script options.</param>
        Public Sub RunFromInputType(ByVal genericInput As String, ByVal inputType As ScriptInputTypes, ByVal options As ScriptRunOptions())
            Select Case inputType
                Case ScriptInputTypes.FromFile
                    RunFromFile(genericInput, options)
                Case ScriptInputTypes.TextDisplay
                    RunFromText(genericInput, options)
                Case ScriptInputTypes.DirectScriptInput
                    RunFromString(genericInput, options)
                Case Else
                    Logger.Debug("000", String.Format("Invalid script input type {0}.", CInt(inputType).ToString()))
            End Select
        End Sub

        ''' <summary>
        ''' Runs a script from a file name.
        ''' </summary>
        ''' <param name="filePath">The script file path relative to the script path.</param>
        Public Sub RunFromFile(ByVal filePath As String)
            RunFromFile(filePath, New ScriptRunOptions() {})
        End Sub

        ''' <summary>
        ''' Runs a script from a file name.
        ''' </summary>
        ''' <param name="filePath">The script file path relative to the script path.</param>
        ''' <param name="options">Script options.</param>
        Public Sub RunFromFile(ByVal filePath As String, ByVal options As ScriptRunOptions())
            If filePath.EndsWith(".dat") = False Then 'Add .dat extension if not handed in.
                Logger.Debug("001", String.Format("A script call to a file ({0}) without extension has been issued. The "".dat"" has been added.", filePath))
                filePath = filePath & ".dat"
            End If

            Dim path As String = GameModeManager.GetScriptPath(filePath)
            Security.FileValidation.CheckFileValid(path, False, "ScriptController")

            If IO.File.Exists(path) = True Then
                Dim scriptContent As String = IO.File.ReadAllText(path)

                RunScript(scriptContent, Script.ScriptOriginTypes.File, filePath, options)
            End If
        End Sub

        ''' <summary>
        ''' Generates a script to display a text box.
        ''' </summary>
        ''' <param name="text">The text to display.</param>
        Public Sub RunFromText(ByVal text As String)
            RunFromText(text, New ScriptRunOptions() {})
        End Sub

        ''' <summary>
        ''' Generates a script to display a text box.
        ''' </summary>
        ''' <param name="text">The text to display.</param>
        ''' <param name="options">Script options.</param>
        Public Sub RunFromText(ByVal text As String, ByVal options As ScriptRunOptions())
            Dim scriptContent As String = "@text.show(" & text & ")"
            RunScript(scriptContent, Script.ScriptOriginTypes.Text, text, options)
        End Sub

        ''' <summary>
        ''' Runs a raw script directly.
        ''' </summary>
        ''' <param name="inputString">The script.</param>
        Public Sub RunFromString(ByVal inputString As String)
            RunFromString(inputString, New ScriptRunOptions() {})
        End Sub

        ''' <summary>
        ''' Runs a raw script directly.
        ''' </summary>
        ''' <param name="inputString">The script.</param>
        ''' <param name="options">Script options.</param>
        Public Sub RunFromString(ByVal inputString As String, ByVal options As ScriptRunOptions())
            RunScript(inputString, Script.ScriptOriginTypes.String, vbNewLine & inputString, options)
        End Sub

        'When there is already a script running a new script should be started, this is where the new script gets stored in temporarly.
        'We need to end to execution of the current frame with no changes to the script context, so we cannot add the new script right away.
        Private _tempAddScript As Script = Nothing

        Private Sub RunScript(ByVal scriptContent As String, ByVal scriptOriginType As Script.ScriptOriginTypes, ByVal scriptOriginIdentifier As String, ByVal options As ScriptRunOptions())
            'Only start the script if the re delay is 0 or we are told to not check the delay.
            If options.Contains(ScriptRunOptions.CheckDelay) = False Or _scriptStartDelay <= 0F Then
                _OrientatePlayer = options.Contains(ScriptRunOptions.OrientatePlayer) And _CorrectPlayerOrientation > -1
                _IsInsightScriptRunning = options.Contains(ScriptRunOptions.InsightScript)
                _scriptStartDelay = 0.5F

                'Add new script to the stack:
                Dim oScript As New Script(scriptContent, scriptOriginType, scriptOriginIdentifier)

                If _scripts.Count = 0 Then
                    _scripts.Add(oScript)
                Else
                    _tempAddScript = oScript
                End If
            End If
        End Sub

#End Region

        ''' <summary>
        ''' Returns the currently active script, which sits on the top of the stack.
        ''' </summary>
        Public ReadOnly Property ActiveScript() As Script
            Get
                If _scripts.Count > 0 Then
                    Return _scripts.Last()
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' If the script controller is done processing all scripts.
        ''' </summary>
        Public ReadOnly Property IsReady() As Boolean
            Get
                Return _scripts.Count = 0
            End Get
        End Property

        ''' <summary>
        ''' Updates the script controller.
        ''' </summary>
        Public Sub Update()
            If _tempAddScript IsNot Nothing Then
                _scripts.Add(_tempAddScript)
                _tempAddScript = Nothing
            End If

            If IsReady = False Then
                ActiveScript.Execute()

                If Not ActiveScript Is Nothing Then
                    If ActiveScript.IsReady = True And _scripts.Contains(ActiveScript) = True Then
                        _scripts.Remove(ActiveScript)
                    End If
                End If

                If IsReady = True Then
                    Reset()

                    If _context = ScriptContext.Overworld Then
                        'Unlock the camera after a script is finished running:
                        Logger.Debug("096", "Unlock Camera")
                        CType(Screen.Camera, OverworldCamera).YawLocked = False
                        CType(Screen.Camera, OverworldCamera).ResetCursor()
                    End If
                End If
            Else
                If _scriptStartDelay > 0F Then
                    _scriptStartDelay -= 0.1F
                    If _scriptStartDelay <= 0F Then
                        _scriptStartDelay = 0F
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Resets the ScriptController.
        ''' </summary>
        Public Sub Reset()
            _scripts.Clear()
            Framework.Classes.CL_String.Reset()
            ResetPlayerOrientation()
        End Sub

        ''' <summary>
        ''' Resets the player orientation settings.
        ''' </summary>
        Public Sub ResetPlayerOrientation()
            _OrientatePlayer = False
            _CorrectPlayerOrientation = -1
        End Sub

    End Class

End Namespace