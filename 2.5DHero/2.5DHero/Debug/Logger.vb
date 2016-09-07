Imports System.Management

Public Class Logger

    Public Enum LogTypes
        Message
        Debug
        ErrorMessage
        Warning
        Entry
    End Enum

    Private Const CRASHLOGSEPARATOR As String = "---------------------------------------------------------------------------------"

    Private Shared History As New List(Of String)
    Public Shared DisplayLog As Boolean = False

    Private Shared ErrorHeaders() As String = {"I AM ERROR!",
                                               "Minecraft crashed.",
                                               "Missingno.",
                                               "1 ERROR",
                                               "GET TO DA CHOPPA",
                                               "Fire attacks might be super effective...",
                                               "Does this help?",
                                               "Work! Pleeeeeeeease?",
                                               "WHAT IS THIS?",
                                               "I find your lack of [ERROR] disturbing.",
                                               "Blame Darkfire.",
                                               "RTFM",
                                               "FEZ II announced.",
                                               "At least it's not a Blue Screen.",
                                               "Kernel PANIC",
                                               "I'm sorry, Dave, I'm afraid I can't do that.",
                                               "Never gonna give you up ~",
                                               "Wouldn't have happend with Swift.",
                                               "Team Rocket blasting off again!",
                                               "Snorlax just sat on your computer!",
                                               "Wut?",
                                               "Mojang buys Microsoft! Get your new Mojang operating system now. With more blocks and scrolls.",
                                               "HλLF-LIFE 2 confirmed",
                                               "(╯°□°）╯︵ ┻━┻"}

    Const LOGVERSION As String = "2.4"

    Public Shared Sub Log(ByVal LogType As LogTypes, ByVal Message As String)
        Try
            Dim currentTime As String = GetLogTime(Date.Now)

            Dim LogString As String
            If LogType = LogTypes.Entry Then
                LogString = "]" & Message
            Else
                LogString = LogType.ToString() & " (" & currentTime & "): " & Message
            End If

            Debug("Logger: " & LogString)

            Dim Log As String = ""

            If IO.File.Exists(GameController.GamePath & "\log.dat") = True Then
                Log = IO.File.ReadAllText(GameController.GamePath & "\log.dat")
            End If

            If Log = "" Then
                Log = LogString
            Else
                Log &= vbNewLine & LogString
            End If

            IO.File.WriteAllText(GameController.GamePath & "\log.dat", Log)
        Catch ex As Exception : End Try
    End Sub

    Public Shared Function LogCrash(ByVal ex As Exception) As String
        Try
            Dim w32ErrorCode As Integer = -1

            Dim w32 = TryCast(ex, ComponentModel.Win32Exception)
            If w32 IsNot Nothing Then
                w32ErrorCode = w32.ErrorCode
            End If

            Dim logName As String = ""
            With My.Computer.Clock.LocalTime
                Dim month As String = .Month.ToString()
                If month.Length = 1 Then
                    month = "0" & month
                End If
                Dim day As String = .Day.ToString()
                If day.Length = 1 Then
                    day = "0" & day
                End If
                Dim hour As String = .Hour.ToString()
                If hour.Length = 1 Then
                    hour = "0" & hour
                End If
                Dim minute As String = .Minute.ToString()
                If minute.Length = 1 Then
                    minute = "0" & minute
                End If
                Dim second As String = .Second.ToString()
                If second.Length = 1 Then
                    second = "0" & second
                End If
                logName = .Year & "-" & month & "-" & day & "_" & hour & "." & minute & "." & second & "_crash.dat"
            End With

            Dim ContentPacks As String = "{}"
            If Not Core.GameOptions Is Nothing Then
                ContentPacks = Core.GameOptions.ContentPackNames.ArrayToString()
            End If

            Dim GameMode As String = "[No GameMode loaded]"
            If Not GameModeManager.ActiveGameMode Is Nothing Then
                GameMode = GameModeManager.ActiveGameMode.Name
            End If

            Dim OnlineInformation As String = "GameJolt Account: FALSE"
            If Not Core.Player Is Nothing Then
                OnlineInformation = "GameJolt Account: " & Core.Player.IsGameJoltSave.ToString().ToUpper()
                If Core.Player.IsGameJoltSave = True Then
                    OnlineInformation &= " (" & GameJoltSave.GameJoltID & ")"
                End If
            End If

            Dim ScriptInfo As String = "Actionscript: No script running"
            If Not CurrentScreen Is Nothing Then
                If CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                    If CType(CurrentScreen, OverworldScreen).ActionScript.IsReady = False Then
                        ScriptInfo = "Actionscript: " & ActionScript.CSL().ScriptName & "; Line: " & ActionScript.CSL().CurrentLine
                    End If
                End If
            End If

            Dim ServerInfo As String = "FALSE"
            If ConnectScreen.Connected = True Then
                ServerInfo = "TRUE (" & JoinServerScreen.SelectedServer.GetName() & "/" & JoinServerScreen.SelectedServer.GetAddressString() & ")"
            End If

            Dim GameEnvironment As String = "[No Game Environment loaded]"
            If Not CurrentScreen Is Nothing Then
                GameEnvironment = CurrentScreen.Identification.ToString()
            End If

            Dim IsSandboxMode As String = "False"
            If Not Core.Player Is Nothing Then
                IsSandboxMode = Core.Player.SandBoxMode.ToString()
            End If

            Dim gameInformation As String = GameController.GAMENAME & " " & GameController.GAMEDEVELOPMENTSTAGE & " version: " & GameController.GAMEVERSION & " (" & GameController.RELEASEVERSION & ")" & vbNewLine &
                "Content Packs: " & ContentPacks & vbNewLine &
                "Active GameMode: " & GameMode & vbNewLine &
                OnlineInformation & vbNewLine &
                "Playing on Servers: " & ServerInfo & vbNewLine &
                "Game Environment: " & GameEnvironment & vbNewLine &
                ScriptInfo & vbNewLine &
                "File Validation: " & Security.FileValidation.IsValid(True).ToString() & vbNewLine &
                "Sandboxmode: " & IsSandboxMode

            Dim ScreenState As String = "[Screen state object not available]"
            If Not CurrentScreen Is Nothing Then
                ScreenState = "Screen state for the current screen (" & CurrentScreen.Identification.ToString() & ")" & vbNewLine & vbNewLine &
                    CurrentScreen.GetScreenStatus()
            End If

            Dim architectureString As String = "32 Bit"
            If Environment.Is64BitOperatingSystem = True Then
                architectureString = "64 Bit"
            End If

            Dim specs As String = "Operating system: " & My.Computer.Info.OSFullName & " [" & My.Computer.Info.OSVersion & "]" & vbNewLine &
                "Core architecture: " & architectureString & vbNewLine &
                "System time: " & My.Computer.Clock.LocalTime.ToString() & vbNewLine &
                "System language: " & Globalization.CultureInfo.CurrentCulture.EnglishName & "(" & Globalization.CultureInfo.CurrentCulture.ThreeLetterWindowsLanguageName & ") / Loaded game language: " & Localization.LanguageSuffix & vbNewLine &
                "Decimal separator: " & GameController.DecSeparator & vbNewLine &
                "Available physical memory: " & Math.Round((My.Computer.Info.TotalPhysicalMemory / Math.Pow(1024, 3)), 2).ToString() & " Gigabyte" & vbNewLine &
                "Available logical processors: " & Environment.ProcessorCount.ToString()

            Dim innerException As String = "NOTHING"
            If Not ex.InnerException Is Nothing Then
                innerException = ex.InnerException.Message
            End If
            Dim message As String = "NOTHING"
            If Not ex.Message Is Nothing Then
                message = ex.Message
            End If
            Dim source As String = "NOTHING"
            If Not ex.Source Is Nothing Then
                source = ex.Source
            End If
            Dim StackTrace As String = "NOTHING"
            If Not ex.StackTrace Is Nothing Then
                StackTrace = ex.StackTrace
            End If

            Dim helpLink As String = "No helplink available."
            If Not ex.HelpLink Is Nothing Then
                helpLink = ex.HelpLink
            End If

            Dim BaseException As Exception = ex.GetBaseException()

            Dim data As String = "NOTHING"
            If Not ex.Data Is Nothing Then
                data = "Items: " & ex.Data.Count
                If ex.Data.Count > 0 Then
                    data = ""
                    For i = 0 To ex.Data.Count - 1
                        If data <> "" Then
                            data &= vbNewLine
                        End If
                        data &= "[" & ex.Data.Keys(i).ToString() & ": """ & ex.Data.Values(i).ToString() & """]"
                    Next
                End If
            End If

            Dim informationItem As New ErrorInformation(ex)

            Dim objDump As New ObjectDump(Core.CurrentScreen)
            Dim screenDump As String = objDump.Dump

            Dim content As String =
                            "Kolben Games Crash Log V " & LOGVERSION & vbNewLine &
                            GameController.GAMENAME & " has crashed!" & vbNewLine &
                            "// " & ErrorHeaders(Random.Next(0, ErrorHeaders.Length)) & vbNewLine & vbNewLine &
                            CRASHLOGSEPARATOR & vbNewLine & vbNewLine &
                            "Game information:" & vbNewLine & vbNewLine &
                            gameInformation & vbNewLine & vbNewLine &
                            CRASHLOGSEPARATOR & vbNewLine & vbNewLine &
                            ScreenState & vbNewLine & vbNewLine &
                            CRASHLOGSEPARATOR & vbNewLine & vbNewLine &
                            "System specifications:" & vbNewLine & vbNewLine &
                            specs & vbNewLine & vbNewLine &
                            CRASHLOGSEPARATOR & vbNewLine & vbNewLine &
                            ".Net installation information:" & vbNewLine & vbNewLine &
                            DotNetVersion.GetInstalled() & vbNewLine &
                            CRASHLOGSEPARATOR & vbNewLine & vbNewLine &
                            GetGraphicsCardInformation() & vbNewLine & vbNewLine &
                            CRASHLOGSEPARATOR & vbNewLine & vbNewLine &
                            "Error information:" & vbNewLine &
                           vbNewLine & "Message: " & message &
                           vbNewLine & "InnerException: " & innerException &
                           vbNewLine & "BaseException: " & BaseException.Message &
                           vbNewLine & "HelpLink: " & helpLink &
                           vbNewLine & "Data: " & data &
                           vbNewLine & "Source: " & source &
                           vbNewLine & "Win32 Errorcode: " & w32ErrorCode.ToString() & vbNewLine & vbNewLine &
                           CRASHLOGSEPARATOR & vbNewLine & vbNewLine &
                           informationItem.ToString() & vbNewLine & vbNewLine &
                           CRASHLOGSEPARATOR & vbNewLine & vbNewLine &
                           "CallStack: " & vbNewLine & vbNewLine &
                           StackTrace & vbNewLine & vbNewLine &
                           CRASHLOGSEPARATOR & vbNewLine & vbNewLine &
                           "Enviornment dump: " & vbNewLine & vbNewLine &
                           screenDump & vbNewLine & vbNewLine &
                           CRASHLOGSEPARATOR & vbNewLine & vbNewLine &
                           "You should report this error." & vbNewLine & vbNewLine & "Go to ""http://pokemon3d.net/forum/forums/6/create-thread"" to report this crash there."

            IO.File.WriteAllText(GameController.GamePath & "\" & logName, content)

            MsgBox(GameController.GAMENAME & " has crashed!" & vbNewLine & "---------------------------" & vbNewLine & vbNewLine & "Here is further information:" &
                           vbNewLine & "Message: " & ex.Message &
                           vbNewLine & vbNewLine & "You should report this error. When you do this, please attach the crash log to the report. You can find the file in your ""Pokemon"" folder." & vbNewLine & vbNewLine & "The name of the file is: """ & logName & """.", MsgBoxStyle.Critical, "Pokémon3D crashed!")

            Process.Start("explorer.exe", "/select,""" & GameController.GamePath & "\" & logName & """")

            'Returns the argument to start the launcher with:
            Return """CRASHLOG_" & GameController.GamePath & "\" & logName & """ " &
                """ERRORTYPE_" & informationItem.ErrorType & """ " &
                """ERRORID_" & informationItem.ErrorID & """ " &
                """GAMEVERSION_" & GameController.GAMEDEVELOPMENTSTAGE & " " & GameController.GAMEVERSION & """ " &
                """CODESOURCE_" & ex.Source & """ " &
                """TOPSTACK_" & ErrorInformation.GetStackItem(ex.StackTrace, 0) & """"
        Catch exs As Exception
            MsgBox(exs.Message & vbNewLine & exs.StackTrace)
        End Try

        Return ""
    End Function

    Shared longestStackEntryName As String = "GameModeManager.SetGameModePointer"

    Public Shared Sub Debug(ByVal message As String)
        Dim stackTraceEntry As String = Environment.StackTrace.SplitAtNewline()(3)

        While stackTraceEntry.StartsWith(" ") = True
            stackTraceEntry = stackTraceEntry.Remove(0, 1)
        End While
        stackTraceEntry = stackTraceEntry.Remove(0, stackTraceEntry.IndexOf(" ") + 1)
        stackTraceEntry = stackTraceEntry.Remove(stackTraceEntry.IndexOf("("))
        Dim pointString As String = stackTraceEntry.Remove(stackTraceEntry.LastIndexOf("."))
        stackTraceEntry = stackTraceEntry.Remove(0, pointString.LastIndexOf(".") + 1)

        Dim stackOutput As String = stackTraceEntry

        If stackOutput.Length > longestStackEntryName.Length Then
            longestStackEntryName = stackOutput
        Else
            While stackOutput.Length < longestStackEntryName.Length
                stackOutput &= " "
            End While
        End If

        Diagnostics.Debug.Print(stackOutput & vbTab & "| " & message)
        History.Add("(" & GetLogTime(Date.Now) & ") " & message)
    End Sub

    Public Shared Sub DrawLog()
        If DisplayLog = True And History.Count > 0 And Not FontManager.ChatFont Is Nothing Then
            Dim items As New List(Of String)
            Dim max As Integer = History.Count - 1

            Dim itemCount As Integer = 10
            If windowSize.Height > 680 Then
                itemCount += CInt(Math.Floor((windowSize.Height - 680) / 16))
            End If

            Dim min As Integer = max - itemCount
            If min < 0 Then
                min = 0
            End If

            Dim maxWidth As Integer = 0
            For i = min To max
                Dim s As Single = FontManager.ChatFont.MeasureString(History(i)).X * 0.51F
                If CInt(s) > maxWidth Then
                    maxWidth = CInt(s)
                End If
            Next

            Canvas.DrawRectangle(New Rectangle(0, 0, maxWidth + 10, (itemCount + 1) * 16 + 2), New Color(0, 0, 0, 150))

            Dim c As Integer = 0
            For i = min To max
                SpriteBatch.DrawString(FontManager.ChatFont, History(i), New Vector2(5, 2 + c * 16), Color.White, 0F, Vector2.Zero, 0.51F, SpriteEffects.None, 0F)
                c += 1
            Next
        End If
    End Sub

    Private Shared Function GetLogTime(ByVal d As Date) As String
        Dim hour As String = d.Hour.ToString()
        Dim minute As String = d.Minute.ToString()
        Dim second As String = d.Second.ToString()

        If hour.Length = 1 Then
            hour = "0" & hour
        End If
        If minute.Length = 1 Then
            minute = "0" & minute
        End If
        If second.Length = 1 Then
            second = "0" & second
        End If

        Return hour & ":" & minute & ":" & second
    End Function

    Private Shared Function GetGraphicsCardInformation() As String
        Dim CardName As String = ""
        Dim CardRAM As String = ""

        Dim WmiSelect As New ManagementObjectSearcher("root\CIMV2", "SELECT * FROM Win32_VideoController")

        For Each WmiResults As ManagementObject In WmiSelect.Get()
            Try
                If CardName <> "" Then
                    CardName &= "; "
                    CardRAM &= "; "
                End If

                CardName &= WmiResults.GetPropertyValue("Name").ToString()
                CardRAM &= WmiResults.GetPropertyValue("AdapterRAM").ToString()
            Catch ex As Exception

            End Try
        Next

        Return "Graphics Card information:" & vbNewLine & vbNewLine &
            "[CardName(s): """ & CardName & """]" & vbNewLine &
            "[CardRAM(s) : """ & CardRAM & """]"
    End Function

    Public Class ErrorInformation

        Public ErrorID As Integer = -1
        Public ErrorType As String = ""
        Public ErrorDescription As String = ""
        Public ErrorSolution As String = ""

        Public ErrorIDString As String = "-1"

        Public Sub New(ByVal EX As Exception)
            Dim stackTrace As String = EX.StackTrace

            If Not stackTrace Is Nothing Then
                Dim currentIndex As Integer = 0
                Dim callSub As String = ""

analyzeStack:
                callSub = GetStackItem(EX.StackTrace, currentIndex)

                Select Case callSub
                    'asset issues (000-099):
                    Case "Microsoft.Xna.Framework.Content.ContentManager.OpenStream"
                        ErrorID = 1
                        ErrorDescription = "The game was unable to load an asset (a Texture, a Sound or Music)."
                        ErrorSolution = "Make sure the file requested exists on your system."
                    Case "_2._5DHero.MusicManager.PlayMusic"
                        ErrorID = 2
                        ErrorDescription = "The game was unable to play a music file."
                        ErrorSolution = "Make sure the file requested exists on your system. This might be caused by an invalid file in a ContentPack."
                    Case "Microsoft.Xna.Framework.Graphics.Texture.GetAndValidateRect"
                        ErrorID = 3
                        ErrorDescription = "The game was unable to process a texture file."
                        ErrorSolution = "Code composed by Microsoft caused this issue. This might be caused by an invalid file in a ContentPack."
                    Case "Microsoft.Xna.Framework.Graphics.Texture2D.CopyData[T]"
                        ErrorID = 4
                        ErrorDescription = "The game was unable to process a texture file."
                        ErrorSolution = "Code composed by Microsoft caused this issue. This might be caused by an invalid file in a ContentPack. Try to update your Graphics Card drivers."
                    Case "Microsoft.Xna.Framework.Media.MediaQueue.Play"
                        ErrorID = 5
                        ErrorDescription = "The game was unable to load or play a music file."
                        ErrorSolution = "It is likely that the Windows Media Player is not installed on your computer or is wrongly configured. Please reinstall the Windows Media Player."

                        'GameJoltIssues (100-199)
                    Case "_2._5DHero.GameJolt.APICall.SetStorageData"
                        ErrorID = 100
                        ErrorDescription = "The was unable to connect to a GameJolt server because you tried to send a command without being logged in to GameJolt."
                        ErrorSolution = "This happend because you got logged out from GameJolt due to connection problems. Ensure that your connection to the internet is constant."

                        'scripts (200-299)
                    Case "_2._5DHero.ScriptCommander.DoNPC"
                        ErrorID = 200
                        ErrorDescription = "The game crashed trying to execute an NPC related command (starting with @npc.)"
                        ErrorSolution = "If this happend during your GameMode, inspect the file mentioned under ""Actionscript""."
                    Case "_2._5DHero.Trainer..ctor"
                        ErrorID = 201
                        ErrorDescription = "The game was unable to initialize a new instance of a trainer class."
                        ErrorSolution = "If this is caused by your GameMode, make sure the syntax in the trainer file is correct."
                    Case "_2._5DHero.ScriptComparer.GetArgumentValue"
                        ErrorID = 202
                        ErrorDescription = "The game crashed trying to process a script."
                        ErrorSolution = "If this is caused by your GameMode, make sure the syntax in the script or map file is correct."


                        'Crashes generated by game code (300-399)
                    Case "_2._5DHero.ForcedCrash.Crash"
                        ErrorID = 300
                        ErrorDescription = "The game crashed on purpose."
                        ErrorSolution = "Don't hold down F3 and C at the same time for a long time ;)"
                    Case "_2._5DHero.Security.ProcessValidation.ReportProcess"
                        ErrorID = 301
                        ErrorDescription = "A malicious process was detected. To ensure that you are not cheating or hacking, the game closed."
                        ErrorSolution = "Close all processes with the details given in the Data of the crashlog."
                    Case "_2._5DHero.Security.FileValidation.CheckFileValid"
                        ErrorID = 302
                        ErrorDescription = "The game detected edited or missing files."
                        ErrorSolution = "For online play, ensure that you are running the unmodded version of Pokémon3D. You can enable Content Packs."

                        'misc errors (900-999)
                    Case "Microsoft.Xna.Framework.Graphics.SpriteFont.GetIndexForCharacter"
                        ErrorID = 900
                        ErrorDescription = "The game was unable to display a certain character which is not in the standard latin alphabet."
                        ErrorSolution = "Make sure the GameMode you are playing doesn't use any invalid characters in its scripts and maps."
                    Case "_2._5DHero.Player.LoadPlayer"
                        ErrorID = 901
                        ErrorDescription = "The game failed to load a save state."
                        ErrorSolution = "There are multiple reasons for the game to fail at loading a save state. There could be a missing file in the player directory or corrupted files."
                    Case "Microsoft.Xna.Framework.BoundingFrustum.ComputeIntersectionLine"
                        ErrorID = 902
                        ErrorDescription = "The game failed to set up camera mechanics."
                        ErrorSolution = "This error is getting produced by an internal Microsoft class. Please redownload the game if this error keeps appearing."
                    Case "_2._5DHero.Pokemon.Wild"
                        ErrorID = 903
                        ErrorDescription = "The game crashed while attempting to generate a new Pokémon."
                        ErrorSolution = "This error could have multiple sources, so getting a solution here is difficult. If you made your own Pokémon data file for a GameMode, check it for invalid values."

                    Case "-1"
                        'No stack line found that applies to any error setting.
                        ErrorID = -1
                        ErrorDescription = "The error is undocumented in the error handling system."
                        ErrorSolution = "NaN"
                    Case Else
                        currentIndex += 1
                        GoTo analyzeStack
                End Select
            End If

            If ErrorID > -1 Then
                ErrorIDString = ErrorID.ToString()
                While ErrorIDString.Length < 3
                    ErrorIDString = "0" & ErrorIDString
                End While
            End If
        End Sub

        Public Overrides Function ToString() As String
            If ErrorID > -1 And ErrorID < 100 Then
                ErrorType = "Assets"
            ElseIf ErrorID > 99 And ErrorID < 200 Then
                ErrorType = "GameJolt"
            ElseIf ErrorID > 199 And ErrorID < 300 Then
                ErrorType = "Scripts"
            ElseIf ErrorID > 299 And ErrorID < 400 Then
                ErrorType = "Forced Crash"
            ElseIf ErrorID > 899 And ErrorID < 1000 Then
                ErrorType = "Misc."
            Else
                ErrorType = "NaN"
            End If

            Dim s As String = "Error solution:" & vbNewLine & "(The provided solution might not work for your problem)" & vbNewLine & vbNewLine &
                "Error ID: " & ErrorID & vbNewLine &
                "Error Type: " & ErrorType & vbNewLine &
                "Error Description: " & ErrorDescription & vbNewLine &
                "Error Solution: " & ErrorSolution
            Return s
        End Function

        Public Shared Function GetStackItem(ByVal stack As String, ByVal i As Integer) As String
            If i >= stack.SplitAtNewline().Count Then
                Return "-1"
            End If

            Dim line As String = stack.SplitAtNewline()(i)
            Dim callSub As String = line

            While callSub.StartsWith(" ") = True
                callSub = callSub.Remove(0, 1)
            End While
            callSub = callSub.Remove(0, callSub.IndexOf(" ") + 1)
            callSub = callSub.Remove(callSub.IndexOf("("))

            Return callSub
        End Function

    End Class

End Class