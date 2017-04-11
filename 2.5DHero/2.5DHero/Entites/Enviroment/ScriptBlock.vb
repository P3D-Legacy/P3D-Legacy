Public Class ScriptBlock

    Inherits Entity

    Public Shared TriggeredScriptBlock As Boolean = False

    Dim TriggerID As Integer = 0
    Dim _scriptID As String = "0"
    Dim AcceptedRotations As New List(Of Integer)

    Dim ActivateScript As Boolean = False
    Dim clickedToActivate As Boolean = False

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        Me.TriggerID = Me.ActionValue
        If Me.AdditionalValue.Contains(",") Then
            Dim Data() As String = Me.AdditionalValue.Split(CChar(","))
            For i = 0 To Data.Count - 2
                AcceptedRotations.Add(CInt(Data(i)))
            Next
            Me._scriptID = Data(Data.Count - 1)
        Else
            Me._scriptID = Me.AdditionalValue
        End If

        Me.NeedsUpdate = True

        If Me.TriggerID = 0 Then
            Me.Visible = False
        End If
    End Sub

    Public Overrides Function WalkIntoFunction() As Boolean
        If Me.TriggerID = 0 Or Me.TriggerID = 4 Then
            ActivateScript = True
            TriggeredScriptBlock = True
            If Construct.Controller.GetInstance().CorrectPlayerOrientation = -1 Then
                Construct.Controller.GetInstance().CorrectPlayerOrientation = Screen.Camera.GetPlayerFacingDirection()
            End If

            If Screen.Camera.Name = "Overworld" Then
                If CType(Screen.Camera, OverworldCamera).FreeCameraMode = False Then
                    CType(Screen.Camera, OverworldCamera).YawLocked = True
                End If
            End If

            Screen.Level.WalkedSteps = 0
            Screen.Level.PokemonEncounterData.EncounteredPokemon = False
        End If

        Return False
    End Function

    Public Overrides Sub ClickFunction()
        If Me.TriggerID = 1 Then
            Construct.Controller.GetInstance().CorrectPlayerOrientation = -1
            Me.clickedToActivate = True
            TriggerScript(False)
        End If
    End Sub

    Public Overrides Sub Update()
        If Me.ActivateScript = True And Screen.Camera.Position.X = Me.Position.X And Screen.Camera.Position.Z = Me.Position.Z And CInt(Screen.Camera.Position.Y) = CInt(Me.Position.Y) Then
            Screen.Camera.StopMovement()
            ActivateScript = False
            TriggerScript(False)
        End If

        MyBase.Update()
    End Sub

    Public Sub TriggerScript(ByVal canAttach As Boolean)
        If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
            Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)

            With Construct.Controller.GetInstance()
                If .IsReady = True Or canAttach = True Then
                    If Me.CorrectRotation() = True Then
                        If Me.clickedToActivate = True Then
                            Me.clickedToActivate = False
                            SoundManager.PlaySound("select")
                        End If

                        .RunFromFile(Me._scriptID, {Construct.Controller.ScriptRunOptions.CheckDelay, Construct.Controller.ScriptRunOptions.OrientatePlayer})
                    End If
                End If
            End With
        End If
        TriggeredScriptBlock = False
    End Sub

    Public Function GetActivationID() As Integer
        Dim activationID As Integer = 0
        Select Case Me.TriggerID
            Case 0, 1, 4
                activationID = 0
            Case 2
                activationID = 1
            Case 3
                activationID = 2
        End Select
        Return activationID
    End Function

    Public Function CorrectRotation() As Boolean
        Dim activate As Boolean = False
        If AcceptedRotations.Count > 0 Then
            If AcceptedRotations.Contains(Screen.Camera.GetPlayerFacingDirection()) Then
                activate = True
            End If
        Else
            activate = True
        End If
        Return activate
    End Function

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, True)
    End Sub

    Public ReadOnly Property ScriptID() As String
        Get
            Return Me._scriptID
        End Get
    End Property

End Class