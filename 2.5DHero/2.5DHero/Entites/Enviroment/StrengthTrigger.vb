Public Class StrengthTrigger

    Inherits Entity

    Dim RemoveRock As Boolean = False
    Dim RemoveForever As Boolean = False
    Dim ActivateScript As String = ""

    Dim Activated As Boolean = False
    Public Shared RemovedRegistered As Boolean = False

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        Dim v() As String = Me.AdditionalValue.Split(CChar(","))

        For i = 0 To v.Length
            Select Case i
                Case 0
                    RemoveRock = CBool(v(i))
                Case 1
                    RemoveForever = CBool(v(i))
                Case 2
                    ActivateScript = v(i)
            End Select
        Next

        Dim registers() As String = Core.Player.RegisterData.Split(CChar(","))
        For Each r As String In registers
            If r.StartsWith("ACTIVATOR_REMOVE_STRENGTH_ROCK_" & Screen.Level.LevelFile & "_") = True Then
                Dim RemoveID As String = r.Remove(0, ("ACTIVATOR_REMOVE_STRENGTHT_ROCK_" & Screen.Level.LevelFile & "_").Length - 1)
                For Each sRock As Entity In Screen.Level.Entities
                    If sRock.EntityID = "StrengthRock" And IsNumeric(sRock.ID) = True Then
                        If sRock.ID = CInt(RemoveID) Then
                            sRock.CanBeRemoved = True
                        End If
                    End If
                Next
            End If
        Next

        Me.NeedsUpdate = True
    End Sub

    Public Overrides Sub Update()
        If Activated = False Then
            For Each sRock As Entity In Screen.Level.Entities
                If sRock.EntityID = "StrengthRock" Then
                    If sRock.Position.X = Me.Position.X And sRock.Position.Z = Me.Position.Z Then
                        If RemoveRock = True Then
                            CType(sRock, StrengthRock).CanBeRemoved = True
                        End If
                        If RemoveForever = True Then
                            Construct.Framework.RegisterHandler.NewRegister("ACTIVATOR_REMOVE_STRENGTH_ROCK_" & Screen.Level.LevelFile & "_" & sRock.ID.ToString(), "")
                        End If
                        If ActivateScript <> "" Then
                            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                                Construct.Controller.GetInstance().RunFromString(Me.ActivateScript, {Construct.Controller.ScriptRunOptions.CheckDelay})
                            End If
                        End If

                        Activated = True
                    End If
                End If
            Next
        End If

        MyBase.Update()
    End Sub

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, True)
    End Sub

End Class