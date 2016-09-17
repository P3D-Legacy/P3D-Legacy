#If WINDOWS Or XBOX Then

Module Program

    Private _gameCrashed As Boolean = False

    ''' <summary>
    ''' The main entry point for the application.
    ''' </summary>
    Sub Main(ByVal args As String())
        'Dim manager = Scripting.V3.ScriptManager.Instance
        'manager.StartScript("using battleSystem; var b = battleSystem.getInstance(); b.setCanRun(false); b.reset(); b.getCanRun();", Scripting.V3.ScriptInputType.Raw, Scripting.V3.ScriptStartFlag.None)

        Debug.Print(" ")
        Debug.Print("PROGRAM EXECUTION STARTED")
        Debug.Print("STACK TRACE ENTRY                   | MESSAGE")
        Debug.Print("------------------------------------|------------------------------------")

        CommandLineArgHandler.Initialize(args)

        Logger.Debug("---Start game---")

        Using Game As New GameController()
            If GameController.IS_DEBUG_ACTIVE = True And Debugger.IsAttached = True Then
                Game.Run()
            Else
                Try
                    Game.Run()
                Catch ex As Exception
                    _gameCrashed = True
                    Dim informationItem As New Logger.ErrorInformation(ex)
                    Logger.LogCrash(ex)
                    Logger.Log(Logger.LogTypes.ErrorMessage, "The game crashed with error ID: " & informationItem.ErrorIDString & " (" & ex.Message & ")")
                End Try
            End If
        End Using
    End Sub

    Public ReadOnly Property GameCrashed() As Boolean
        Get
            Return _gameCrashed
        End Get
    End Property

End Module

#End If
