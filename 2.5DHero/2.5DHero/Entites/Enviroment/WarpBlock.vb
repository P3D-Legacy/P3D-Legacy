Public Class WarpBlock

    Inherits Entity

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
    End Sub

    Public Overrides Function WalkAgainstFunction() As Boolean
        Return Warp(False)
    End Function

    Public Function Warp(ByVal MapViewMode As Boolean) As Boolean
        If IsValidLink(Me.AdditionalValue) = True And ScriptBlock.TriggeredScriptBlock = False Then
            Dim destination As String = Me.AdditionalValue.GetSplit(0)

            Dim link As String = Me.AdditionalValue
            Dim c As Integer = 0
            For e = 0 To link.Length - 1
                If link(e) = CChar(",") Then
                    c += 1
                End If
            Next
            If c >= 5 Then
                Dim validRotations As New List(Of Integer)

                Dim rotationData() As String = link.GetSplit(5, ",").Split(CChar("|"))
                For Each Element As String In rotationData
                    validRotations.Add(CInt(Element))
                Next
                If validRotations.Contains(Screen.Camera.GetPlayerFacingDirection()) = False Then 
                    Return True
                End If
            End If

            If System.IO.File.Exists(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.MapPath & destination) = True Or System.IO.File.Exists(GameController.GamePath & "\maps\" & destination) = True Then
                If MapViewMode = False Then
                    Screen.Level.WarpData.WarpDestination = Me.AdditionalValue.GetSplit(0)
                    Screen.Level.WarpData.WarpPosition = New Vector3(CSng(Me.AdditionalValue.GetSplit(1)), CSng(Me.AdditionalValue.GetSplit(2).Replace(".", GameController.DecSeparator)), CSng(Me.AdditionalValue.GetSplit(3)))
                    Screen.Level.WarpData.WarpRotations = CInt(Me.AdditionalValue.GetSplit(4))
                    Screen.Level.WarpData.DoWarpInNextTick = True
                    Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw
                    Screen.Level.WarpData.IsWarpBlock = True
                    Logger.Debug("Lock Camera")
                    CType(Screen.Camera, OverworldCamera).YawLocked = True
                Else
                    Screen.Level = New Level()
                    Screen.Level.Load(Me.AdditionalValue.GetSplit(0))
                    Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

                    Screen.Camera.Position = New Vector3(CSng(Me.AdditionalValue.GetSplit(1)), CSng(Me.AdditionalValue.GetSplit(2).Replace(".", GameController.DecSeparator)), CSng(Me.AdditionalValue.GetSplit(3)))
                End If
            Else
                CallError("Map file """ & GameModeManager.ActiveGameMode.MapPath & destination & """ does not exist.")
            End If
        End If

        Return False
    End Function

    Public Shared Function IsValidLink(ByVal link As String) As Boolean
        If link <> "" Then
            If link.Contains(",") = True Then
                Dim c As Integer = 0
                For e = 0 To link.Length - 1
                    If link(e) = CChar(",") Then
                        c += 1
                    End If
                Next
                If c >= 4 Then
                    Dim destination As String = link.GetSplit(0)
                    If destination.EndsWith(".dat") = True Then
                        Dim x As String = link.GetSplit(1)
                        Dim y As String = link.GetSplit(2).Replace(".", GameController.DecSeparator)
                        Dim z As String = link.GetSplit(3)
                        Dim l As String = link.GetSplit(4)

                        If IsNumeric(x) = True And IsNumeric(y) = True And IsNumeric(z) = True And IsNumeric(l) = True Then
                            Return True
                        Else
                            CallError("Position values are not numeric.")
                            Return False
                        End If
                    Else
                        CallError("Destination file is not a valid map file.")
                        Return False
                    End If
                Else
                    CallError("Not enough or too much arguments to resolve the link.")
                    Return False
                End If
            Else
                CallError("Link does not contain seperators or has wrong seperators.")
                Return False
            End If
        Else
            CallError("Link is empty.")
            Return False
        End If
    End Function

    Private Shared Sub CallError(ByVal ex As String)
        Logger.Log(Logger.LogTypes.ErrorMessage, "WarpBlock.vb: Invalid warp! More information:" & ex)
    End Sub

End Class