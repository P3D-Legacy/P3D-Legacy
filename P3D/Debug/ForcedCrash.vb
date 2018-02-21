Public Class ForcedCrash

    Shared Delay As Single = 14.0F

    Public Shared Sub Update()
        If KeyBoardHandler.KeyDown(KeyBindings.DebugKey) = True And KeyBoardHandler.KeyDown(Keys.C) = True Then
            Debug.Print("CRASH IN: " & Delay.ToString())
            Delay -= 0.1F
            If Delay <= 0.0F Then
                Crash()
            End If
        Else
            Delay = 14.0F
        End If
    End Sub

    Private Shared Sub Crash()
        Dim canCrash As Boolean = True
        If Core.Player.loadedSave = True Then
            If Core.Player.IsGamejoltSave = True Or Core.Player.SandBoxMode = False Then
                canCrash = False
            End If
        End If
        If canCrash = True Then
            Dim ex As New Exception("Forced the game to crash.")
            Throw ex
        Else
            Delay = 14.0F
        End If
    End Sub

End Class