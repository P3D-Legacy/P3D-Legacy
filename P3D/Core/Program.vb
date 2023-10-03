Imports System.Runtime.InteropServices

#If WINDOWS Or XBOX Then

Module Program

    Private _gameCrashed As Boolean = False
    
    <DllImport("nvapi64.dll", EntryPoint := "fake")>
    Private Function LoadNvApi64() As Integer
    End Function
    
    <DllImport("nvapi.dll", EntryPoint := "fake")>
    Private Function LoadNvApi32() As Integer
    End Function
    
    <DllImport("vulkan-1.dll", EntryPoint := "fake")>
    Private Function LoadVulkan() As Integer
    End Function

    ''' <summary>
    ''' The main entry point for the application.
    ''' </summary>
    Sub Main(ByVal args As String())
        Debug.Print(" ")
        Debug.Print("PROGRAM EXECUTION STARTED")
        Debug.Print("STACK TRACE ENTRY                   | MESSAGE")
        Debug.Print("------------------------------------|------------------------------------")

        CommandLineArgHandler.Initialize(args)

        Logger.Debug("---Start game---")
        
        If CommandLineArgHandler.ForceGraphics
            Try
                If System.Environment.Is64BitProcess Then
                    LoadNvApi64()
                Else 
                    LoadNvApi32()
                End If
            Catch
            End Try
        
            Try
                LoadVulkan()
            Catch
            End Try
        End If
        
        Using Game As New GameController()
            If GameController.IS_DEBUG_ACTIVE And Debugger.IsAttached Then
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