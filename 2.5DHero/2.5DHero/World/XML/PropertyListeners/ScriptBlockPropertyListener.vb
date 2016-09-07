Namespace XmlLevel

    Public Class ScriptBlockPropertyListener

        Inherits XmlPropertyListener

        Dim TriggerID As Integer = 0
        Dim ScriptID As String = "0"
        Dim AcceptedRotations As New List(Of Integer)

        Dim ActivateScript As Boolean = False

        Public Sub New(ByVal XmlEntityReference As XmlEntity)
            MyBase.New(XmlEntityReference, "isscriptblock")

            Me.TriggerID = Me.XmlEntity.GetPropertyValue(Of Integer)("scripttrigger")
            Me.ScriptID = Me.XmlEntity.GetPropertyValue(Of String)("script")
            Me.AcceptedRotations = Me.XmlEntity.GetPropertyValue(Of List(Of Integer))("acceptedscriptrotations")

            Me.ImplementWalkInto = True

            Me.XmlEntity.EnableUpdate()
        End Sub

        Public Overrides Function WalkInto() As Boolean
            If Me.TriggerID = 0 Or Me.TriggerID = 4 Then
                Me.ActivateScript = True
                Screen.Level.WalkedSteps = 0
                Screen.Level.PokemonEncounterData.EncounteredPokemon = False
            End If

            Return False
        End Function

        Public Overrides Sub PlayerInteraction()
            If Me.TriggerID = 1 Then
                TriggerScript(False)
            End If
        End Sub

        Public Overrides Sub Update()
            If Me.ActivateScript = True And Screen.Camera.Position.X = XmlEntity.Position.X And Screen.Camera.Position.Z = XmlEntity.Position.Z Then
                Screen.Camera.StopMovement()
                ActivateScript = False
                TriggerScript(False)
            End If
        End Sub

        Public Sub TriggerScript(ByVal canAttach As Boolean)
            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                Dim oS As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)

                If oS.ActionScript.IsReady = True Or canAttach = True Then
                    Dim activate As Boolean = False
                    If Me.AcceptedRotations.Count > 0 Then
                        If Me.AcceptedRotations.Contains(Screen.Camera.GetPlayerFacingDirection()) Then 
                            activate = True
                        End If
                    Else
                        activate = True
                    End If

                    If activate = True Then
                        Dim activationID As Integer = 0
                        Select Case Me.TriggerID
                            Case 0, 1, 4
                                activationID = 0
                            Case 2
                                activationID = 1
                            Case 3
                                activationID = 2
                        End Select

                        oS.ActionScript.StartScript(Me.ScriptID, activationID)
                        ActionScript.TempSpin = True
                    End If
                End If
            End If
        End Sub

    End Class

End Namespace