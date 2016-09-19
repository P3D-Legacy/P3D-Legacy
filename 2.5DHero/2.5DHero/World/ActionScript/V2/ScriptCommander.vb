Namespace ScriptVersion2

    Partial Class ScriptCommander

        Shared ScriptV2 As ScriptV2

        Shared Value As String = "" 'Stores a value that the ScriptCommander keeps across script calls and scripts.

        Public Shared Function Parse(ByVal input As String) As Object
            Return ScriptComparer.EvaluateConstruct(input)
        End Function

        ''' <summary>
        ''' If the script finished executing. If false, the script will get executed next frame.
        ''' </summary>
        Private Shared Property IsReady As Boolean
            Get
                Return ScriptV2.IsReady
            End Get
            Set(value As Boolean)
                ScriptV2.IsReady = value
            End Set
        End Property

        ''' <summary>
        ''' A value to indicate if the script has been started last frame. Not automatically set. Sometimes needed for when a script runs longer than one frame.
        ''' </summary>
        Private Shared Property Started As Boolean
            Get
                Return ScriptV2.started
            End Get
            Set(value As Boolean)
                ScriptV2.started = value
            End Set
        End Property

        ''' <summary>
        ''' If the ScriptController can execute the next script in the same frame once this finishes.
        ''' </summary>
        Private Shared Property CanContinue() As Boolean
            Get
                Return ScriptV2.CanContinue
            End Get
            Set(value As Boolean)
                ScriptV2.CanContinue = value
            End Set
        End Property

        ''' <summary>
        ''' Executes a command.
        ''' </summary>
        ''' <param name="ScriptV2">The primitive script (v2).</param>
        ''' <param name="inputString">The input command.</param>
        Public Shared Sub ExecuteCommand(ByRef ScriptV2 As ScriptV2, ByVal inputString As String)
            ScriptCommander.ScriptV2 = ScriptV2

            Dim classValue As String = inputString

            Dim mainClass As String = classValue
            Dim subClass As String = ""

            Dim bIndex As Integer = classValue.IndexOf("(")
            If classValue.Contains("(") = False Then
                bIndex = -1
            End If

            Dim pIndex As Integer = classValue.IndexOf(".")

            If classValue.Contains(".") = True And (pIndex < bIndex Or bIndex = -1) = True Then
                mainClass = classValue.Remove(classValue.IndexOf("."))
                subClass = classValue.Remove(0, classValue.IndexOf(".") + 1)
            Else
                If classValue.Contains("(") = True Then
                    mainClass = classValue.Remove(classValue.IndexOf("("))
                    subClass = classValue.Remove(0, classValue.IndexOf("(") + 1)
                End If
            End If

            Select Case mainClass.ToLower()
                Case "register"
                    DoRegister(subClass)
                Case "script"
                    DoScript(subClass)
                Case "screen"
                    DoScreen(subClass)
                Case "player"
                    If InsertSpin(inputString) = False Then
                        DoPlayer(subClass)
                    End If
                Case "music"
                    DoMusic(subClass)
                Case "sound"
                    DoSound(subClass)
                Case "entity"
                    If InsertSpin(inputString) = False Then
                        DoEntity(subClass)
                    End If
                Case "battle"
                    DoBattle(subClass)
                Case "pokemon"
                    DoPokemon(subClass)
                Case "overworldpokemon"
                    DoOverworldPokemon(subClass)
                Case "environment"
                    DoEnvironment(subClass)
                Case "text"
                    If InsertSpin(inputString) = False Then
                        DoText(subClass)
                    End If
                Case "options"
                    If InsertSpin(inputString) = False Then
                        DoOptions(subClass)
                    End If
                Case "level"
                    DoLevel(subClass)
                Case "camera"
                    If InsertSpin(inputString) = False Then
                        DoCamera(subClass)
                    End If
                Case "item"
                    DoItem(subClass)
                Case "storage"
                    DoStorage(subClass)
                Case "npc"
                    If InsertSpin(inputString) = False Then
                        DoNPC(subClass)
                    End If
                Case "chat"
                    DoChat(subClass)
                Case "daycare"
                    DoDayCare(subClass)
                Case "pokedex"
                    DoPokedex(subClass)
                Case "radio"
                    DoRadio(subClass)
                Case "help"
                    DoHelp(subClass)
                Case "title"
                    DoTitle(subClass)
                Case Else
                    Logger.Log(Logger.LogTypes.Message, "ScriptCommander.vb: This class (" & mainClass & ") doesn't exist.")
                    IsReady = True
            End Select
        End Sub 'crash handle

        ''' <summary>
        ''' Generates a script line that gets inserted infront of the current script to turn the player into the correct orientation.
        ''' </summary>
        Private Shared Function InsertSpin(ByVal inputString As String) As Boolean
            If ActionScript.TempSpin = True Then
                If ActionScript.TempInputDirection > -1 Then
                    If inputString.ToLower().StartsWith("player.turnto(") = False Then
                        If Screen.Camera.GetPlayerFacingDirection() <> ActionScript.TempInputDirection Then
                            If CType(Screen.Camera, OverworldCamera).ThirdPerson = False Then
                                CType(Core.CurrentScreen, OverworldScreen).ActionScript.Scripts.Insert(0, New Script("@player.turnto(" & ActionScript.TempInputDirection & ")", ActionScript.ScriptLevelIndex))
                                Return True
                            End If
                        End If
                    End If
                    ActionScript.TempInputDirection = -1
                    ActionScript.TempSpin = False
                End If
            End If
            Return False
        End Function

        ''' <summary>
        ''' Opens help content from the ScriptLibrary.
        ''' </summary>
        ''' <param name="subClass">The subClass used in @help().</param>
        Private Shared Sub DoHelp(ByVal subClass As String)
            If subClass.EndsWith(")") = True Then
                subClass = subClass.Remove(subClass.Length - 1, 1)
            End If
            Chat.AddLine(New Chat.ChatMessage("[HELP]", ScriptLibrary.GetHelpContent(subClass, 20), "0", Chat.ChatMessage.MessageTypes.CommandMessage))

            IsReady = True
        End Sub


        '//////////////////////////////////////////////////////////
        '//
        '// Shortens the ScriptConversion methods to shorter names.
        '//
        '//////////////////////////////////////////////////////////

        Private Shared Function int(ByVal expression As Object) As Integer
            Return ScriptConversion.ToInteger(expression)
        End Function

        Private Shared Function sng(ByVal expression As Object) As Single
            Return ScriptConversion.ToSingle(expression)
        End Function

        Private Shared Function dbl(ByVal expression As Object) As Double
            Return ScriptConversion.ToDouble(expression)
        End Function

    End Class

End Namespace
