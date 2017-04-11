Public Class RotationTile

    Inherits Entity

    Public Enum RotationTypes
        StartSpin
        StopSpin
    End Enum

    Dim RotationType As RotationTypes
    Dim RotateTo As Integer = 0

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        Select Case Me.ActionValue
            Case 0
                Me.RotationType = RotationTypes.StartSpin
            Case 1
                Me.RotationType = RotationTypes.StopSpin
        End Select

        Me.RotateTo = CInt(Me.AdditionalValue)
        Me.NeedsUpdate = True
    End Sub

    Public Overrides Sub Update()
        If Me.RotationType = RotationTypes.StartSpin Then
            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                If Construct.Controller.GetInstance().IsReady = True Then
                    If Me.Position.X = Screen.Camera.Position.X And CInt(Me.Position.Y) = CInt(Screen.Camera.Position.Y) And Me.Position.Z = Screen.Camera.Position.Z Then
                        Dim steps As Integer = GetSteps()

                        Dim s As String = "@player.move(0)" & vbNewLine &
                       "@player.turnto(" & Me.RotateTo.ToString() & ")" & vbNewLine &
                       "@player.move(" & steps & ")"

                        Construct.Controller.GetInstance().RunFromString(s, {Construct.Controller.ScriptRunOptions.CheckDelay})
                    End If
                End If
            End If
        End If
    End Sub

    Private Function GetSteps() As Integer
        Dim steps As Integer = 0
        Dim direction As Vector2 = New Vector2(0)
        Select Case Me.RotateTo
            Case 0
                direction.Y = -1
            Case 1
                direction.X = -1
            Case 2
                direction.Y = 1
            Case 3
                direction.X = 1
        End Select

        Dim stepY As Integer = CInt(direction.Y)
        If stepY = 0 Then
            stepY = 1
        End If

        For x = 0 To direction.X * 100 Step direction.X
            For y = 0 To direction.Y * 100 Step stepY
                Dim p As Vector3 = New Vector3(x, 0, y) + Me.Position
                For Each e As Entity In Screen.Level.Entities
                    If e.Equals(Me) = False Then
                        If e.EntityID.ToLower() = "rotationtile" Then
                            If CInt(e.Position.X) = CInt(p.X) And CInt(e.Position.Y) = CInt(p.Y) And CInt(e.Position.Z) = CInt(p.Z) Then
                                GoTo theend
                            End If
                        End If
                    End If
                Next
                steps += 1
            Next
        Next

theend:

        Return steps
    End Function

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
    End Sub

    Public Overrides Function LetPlayerMove() As Boolean
        Return Me.RotationType = RotationTypes.StopSpin
    End Function

    Public Overrides Function WalkIntoFunction() As Boolean
        If Me.RotationType = RotationTypes.StartSpin Then
            CType(Screen.Camera, OverworldCamera).YawLocked = True
        End If
        Return False
    End Function

End Class