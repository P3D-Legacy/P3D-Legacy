Imports System.Reflection
Imports System.Threading.Tasks
Imports net.Pokemon3D.Game.Scripting.V3.ApiClasses
Imports Pokemon3D.Scripting
Imports Pokemon3D.Scripting.Adapters
Imports Pokemon3D.Scripting.Types

Namespace Scripting.V3

    Friend Class ScriptManager

#Region "Singleton"

        Private Sub New()
            ' Set constructor to private to not allow instances.
        End Sub

        Private Shared _instance As ScriptManager

        Public Shared ReadOnly Property Instance As ScriptManager
            Get
                If _instance Is Nothing Then
                    _instance = New ScriptManager()
                End If

                Return _instance
            End Get
        End Property

#End Region

        Private _reDelay As Single = 0.0F
        Private _scriptName As String
        Private _activeProcessorCount As Integer = 0
        Private _unlockCamera As Boolean = False

        Private _prototypeBuffer As List(Of SObject)
        Private _apiClasses As Dictionary(Of String, MethodInfo())

        Public Property IsInSightScript As Boolean

        Public ReadOnly Property IsActive As Boolean
            Get
                Return _activeProcessorCount > 0
            End Get
        End Property

        Public Sub StartScript(input As String, inputType As ScriptInputType, flags As ScriptStartFlag)
            Dim checkDelay As Boolean = flags.HasFlag(ScriptStartFlag.CheckDelay)
            Dim resetInSight = flags.HasFlag(ScriptStartFlag.ResetInSight)

            If resetInSight Then
                IsInSightScript = False
            End If

            If Not checkDelay OrElse _reDelay = 0.0F Then
                Select Case inputType
                    Case ScriptInputType.File
                        StartScriptFromFile(input)
                    Case ScriptInputType.Text
                        StartScriptFromText(input)
                    Case ScriptInputType.Raw
                        StartScriptFromRaw(input)
                End Select
            End If

            _reDelay = 1.0F
        End Sub

        Private Sub StartScriptFromFile(input As String)
            Logger.Debug($"Start script (ID: {input})")
            _scriptName = $"Type: Script; Input: {input}"

            Dim path As String = GameModeManager.GetScriptPath($"{input}.dat")
            Security.FileValidation.CheckFileValid(path, False, "ActionScript.vb")

            If File.Exists(path) Then
                Dim source = File.ReadAllText(path)

                RunScript(source)
            Else
                Logger.Log(Logger.LogTypes.ErrorMessage, $"ActionScript.vb: The script file ""{path}"" doesn't exist!")
            End If
        End Sub

        Private Sub StartScriptFromText(input As String)
            Logger.Debug($"Start Script (Text: {input})")
            _scriptName = $"Type: Text; Input: {input}"

            Dim source As String = $"using text;text.show({input});"

            RunScript(source)
        End Sub

        Private Sub StartScriptFromRaw(input As String)
            Dim activator As String = Environment.StackTrace.Split(vbNewLine)(3)
            activator = activator.Remove(activator.IndexOf("("))

            Logger.Debug($"Start Script (DirectInput; {activator})")
            _scriptName = $"Type: Direct; Input: {input}"

            Dim source As String = input

            RunScript(source)
        End Sub

        Private Sub RunScript(source As String)
            _unlockCamera = True
            _activeProcessorCount += 1
            Task.Run(Sub()
                         Try
                             Dim processor = CreateProcessor()
                             Dim result = processor.Run(source)

                             If ScriptContextManipulator.ThrownRuntimeError(processor) Then
                                 Dim exObj = ScriptOutAdapter.Translate(result)

                                 Dim runtimeException = TryCast(exObj, ScriptRuntimeException)
                                 If runtimeException IsNot Nothing Then
                                     Throw runtimeException
                                 End If
                             End If

                         Catch ex As ScriptRuntimeException
                             Logger.Log(Logger.LogTypes.ErrorMessage, $"Script execution failed at runtime. {ex.Type} (L{ex.Line}: {ex.Message})")
                         End Try

                         _activeProcessorCount -= 1

                     End Sub)
        End Sub

        Private Function CreateProcessor() As ScriptProcessor
            If _prototypeBuffer Is Nothing Then
                InitializePrototypeBuffer()
            End If

            Dim processor = New ScriptProcessor(_prototypeBuffer)

            ScriptContextManipulator.SetCallbackExecuteMethod(processor, AddressOf ExecuteMethod)

            Return processor
        End Function

        Private Function ExecuteMethod(processor As ScriptProcessor, className As String, methodName As String, parameters As SObject()) As SObject
            If _apiClasses Is Nothing Then
                InitializeApiClasses()
            End If

            If _apiClasses.ContainsKey(className) Then
                Dim method = _apiClasses(className).FirstOrDefault(Function(m As MethodInfo)
                                                                       Return m.Name = methodName
                                                                   End Function)
                If method IsNot Nothing Then
                    Dim result = method.Invoke(Nothing, New Object() {processor, parameters})
                    Return TryCast(result, SObject)
                End If

            End If

            Return ScriptInAdapter.GetUndefined(processor)
        End Function

        Private Sub InitializePrototypeBuffer()
            _prototypeBuffer = New List(Of SObject)()
            Dim processor = New ScriptProcessor()

            For Each o As Type In GetType(ScriptManager).Assembly.GetTypes().Where(Function(t As Type)
                                                                                       Return t.GetCustomAttributes(GetType(ScriptPrototypeAttribute), True).Length > 0
                                                                                   End Function)
                _prototypeBuffer.Add(ScriptInAdapter.Translate(processor, o))
            Next
        End Sub

        Private Sub InitializeApiClasses()
            _apiClasses = New Dictionary(Of String, MethodInfo())()

            For Each o As Type In GetType(ScriptManager).Assembly.GetTypes().Where(Function(t As Type)
                                                                                       Return t.IsSubclassOf(GetType(ApiClass)) AndAlso
                                                                                              t.GetCustomAttributes(GetType(ApiClassAttribute), True).Length > 0
                                                                                   End Function)
                Dim attr = CType(o.GetCustomAttribute(GetType(ApiClassAttribute)), ApiClassAttribute)
                _apiClasses.Add(attr.ClassName, o.GetMethods(BindingFlags.Public Or BindingFlags.Static))
            Next
        End Sub

        Public Sub Update()
            If Not IsActive Then
                If _unlockCamera Then
                    _unlockCamera = False
                    Logger.Debug("Unlock Camera")
                    CType(Screen.Camera, OverworldCamera).YawLocked = False
                    CType(Screen.Camera, OverworldCamera).ResetCursor()
                End If

                If _reDelay > 0.0F Then
                    _reDelay -= 0.1F

                    If _reDelay <= 0.0F Then
                        _reDelay = 0.0F
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace
