Module Program
    ''' <summary>
    ''' The main entry point for the application.
    ''' </summary>
    Sub Main(args As String())
        Debug.Print(" ")
        Debug.Print("PROGRAM EXECUTION STARTED")
        Debug.Print("STACK TRACE ENTRY                   | MESSAGE")
        Debug.Print("------------------------------------|------------------------------------")

        CommandLineArgHandler.Initialize(args)

        Logger.Debug("---Start game---")

        Using game As New GameController()
            If GameController.IS_DEBUG_ACTIVE AndAlso Debugger.IsAttached Then
                game.Run()
            Else
                Try
                    game.Run()
                Catch ex As Exception
                    Dim informationItem As New Logger.ErrorInformation(ex)
                    Logger.LogCrash(ex)
                    Logger.Log(Logger.LogTypes.ErrorMessage, "The game crashed with error ID: " & informationItem.ErrorIDString & " (" & ex.Message & ")")
                End Try
            End If
        End Using
    End Sub
End Module