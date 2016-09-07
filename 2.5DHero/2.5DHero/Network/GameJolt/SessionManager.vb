Namespace GameJolt

    Public Class SessionManager

        Shared SessionStarted As Boolean = False
        Shared Status As String = "Idle"

        Shared LastPing As Date = Date.Now

        Public Shared Sub Update()
            If API.LoggedIn = True Then
                Select Case Status
                    Case "Idle"
                        If SessionStarted = False Then
                            Start()
                        Else
                            Ping()
                        End If
                    Case "WaitingForOpen"
                        Logger.Debug("Session started!")

                        Status = "Idle"
                        SessionStarted = True
                End Select
            Else
                Status = "Idle"
            End If
        End Sub

        Shared Sub Ping()
            Dim diff As Integer = CInt(DateDiff(DateInterval.Second, LastPing, Date.Now))

            If diff >= 30 Then
                Logger.Debug("Ping session...")

                LastPing = Date.Now

                If API.LoggedIn = True Then
                    Dim APICall As New APICall()
                    AddHandler APICall.CallFails, AddressOf Kick
                    APICall.PingSession()
                End If
            End If
        End Sub

        Shared Sub Start()
            If API.LoggedIn = True And Status <> "WaitingForCheck" Then
                Logger.Debug("Starting session...")

                Status = "WaitingForCheck"

                Dim APICall As New APICall(AddressOf CheckedSession)
                APICall.CheckSession()
            End If
        End Sub

        Shared Sub CheckedSession(ByVal result As String)
            Dim list As List(Of API.JoltValue) = API.HandleData(result)

            Dim APICall As New APICall()
            APICall.OpenSession()

            Status = "WaitingForOpen"

            LastPing = Date.Now

            If list(0).Value = "true" Then
                Logger.Log(Logger.LogTypes.Warning, "SessionManager.vb: Tried to log in with an already logged in account!")
            End If

            'If list(0).Value = "false" Or DGame.IS_DEBUG_ACTIVE = True Then
            '    Dim APICall As New APICall()
            '    APICall.OpenSession()

            '    Status = "WaitingForOpen"

            '    LastPing = Date.Now
            'Else
            '    API.LoggedIn = False
            '    SessionStarted = False
            '    Status = "Idle"
            '    Logger.Debug("Tried to log in with an already logged in account!")
            '    Basic.SetScreen(New ConnectScreen(ConnectScreen.Modes.Disconnect, "Kicked", "This account is already logged in!", True))
            'End If
        End Sub

        Public Shared Sub Close()
            If API.LoggedIn = True Then
                Logger.Debug("Closing session...")

                Dim APICall As New APICall()
                APICall.CloseSession()

                LastPing = Date.Now
                SessionStarted = False
            End If
        End Sub

        Private Shared Sub Kick()
            If API.LoggedIn = True Then
                API.LoggedIn = False
                SessionStarted = False
                Status = "Idle"
                If Core.Player.IsGamejoltSave = True Then
                    ConnectScreen.Setup(New ConnectScreen(ConnectScreen.Modes.Disconnect, "Disconnected", "The GameJolt server doesn't respond.", Core.CurrentScreen))
                End If
            End If
        End Sub

    End Class

End Namespace