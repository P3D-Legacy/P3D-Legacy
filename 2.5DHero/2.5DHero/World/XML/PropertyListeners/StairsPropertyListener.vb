Namespace XmlLevel

    Public Class StairsPropertyListener

        Inherits XmlPropertyListener

        Public Sub New(ByVal XmlEntityReference As XmlEntity)
            MyBase.New(XmlEntityReference, "isstairs")

            Me.ImplementWalkAgainst = True
        End Sub

        'Public Overrides Function WalkAgainst() As Boolean
        '    Dim facing As Integer = CInt(Me.XmlEntity.Rotation.Y / MathHelper.PiOver2)
        '    facing -= 2
        '    If facing < 0 Then
        '        facing += 4
        '    End If
        '    Screen.Camera.moveDirectionY = 0.0F
        '    If Screen.Camera.GetPlayerFacingDirection() = facing And Screen.Camera.moved = 0.0F Then 
        '        Dim Steps As Integer = 0

        '        Dim checkPosition As New Vector3(Me.XmlEntity.Position.X + Screen.Camera.moveDirectionX, Me.XmlEntity.Position.Y + 1, Me.XmlEntity.Position.Z + Screen.Camera.moveDirectionZ)
        '        Dim foundSteps As Boolean = True
        '        While foundSteps = True
        '            Dim e As XmlEntity = Nothing 'TODO: Implement the correct entity list: GetEntity(Screen.Level.Entities, checkPosition)
        '            If Not e Is Nothing Then
        '                If e.GetPropertyValue(Of Boolean)("isstairs") = True Then
        '                    Steps += 1
        '                    checkPosition.X += Screen.Camera.moveDirectionX
        '                    checkPosition.Z += Screen.Camera.moveDirectionZ
        '                    checkPosition.Y += 1
        '                Else
        '                    If e.GetPropertyValue(Of Boolean)("isscripttrigger") = True Then
        '                        'TODO: Convert the temp entity to be a XmlEntity: Player.Temp.ScriptEntity = e
        '                    ElseIf e.GetPropertyValue(Of Boolean)("iswarp") = True Then
        '                        e.WalkAgainst()
        '                    End If
        '                    foundSteps = False
        '                End If
        '            Else
        '                foundSteps = False
        '            End If
        '        End While

        '        Screen.Level.OverworldPokemon.Visible = False
        '        Screen.Level.OverworldPokemon.warped = True

        '        Dim s As String = "@Player:SetMovement(" & Screen.Camera.moveDirectionX & ",1," & Screen.Camera.moveDirectionZ & ")" & vbNewLine &
        '            "@Player:Move(" & Steps & ")" & vbNewLine &
        '            "@Player:SetMovement(" & Screen.Camera.moveDirectionX & ",0," & Screen.Camera.moveDirectionZ & ")" & vbNewLine &
        '            "@Pokemon:Hide" & vbNewLine &
        '            "@Player:Move(1)" & vbNewLine &
        '            "@Pokemon:Hide" & vbNewLine &
        '            ":end"

        '        CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2, False)
        '        Return False
        '    End If

        '    facing = CInt(Me.XmlEntity.Rotation.Y / MathHelper.PiOver2)
        '    If facing < 0 Then
        '        facing += 4
        '    End If
        '    If Screen.Camera.GetPlayerFacingDirection() = facing Then 
        '        Return False
        '    End If

        '    Return True
        'End Function

        'Private Function GetEntity(ByVal List As List(Of XmlEntity), ByVal Position As Vector3) As XmlEntity
        '    For Each e As XmlEntity In List
        '        If e.Position.X = Position.X And e.Position.Y = Position.Y And e.Position.Z = Position.Z Then
        '            Return e
        '        End If
        '    Next

        '    Return Nothing
        'End Function

        'Public Overrides Sub WalkOnto()
        '    Dim facing As Integer = CInt(Me.XmlEntity.Rotation.Y / MathHelper.PiOver2)
        '    Screen.Camera.moveDirectionY = 0.0F

        '    If Screen.Camera.GetPlayerFacingDirection() = facing Then 
        '        Dim Steps As Integer = 1

        '        Dim checkPosition As New Vector3(Me.XmlEntity.Position.X + Screen.Camera.moveDirectionX, Me.XmlEntity.Position.Y - 1, Me.XmlEntity.Position.Z + Screen.Camera.moveDirectionZ)
        '        Dim foundSteps As Boolean = True
        '        While foundSteps = True
        '            Dim e As XmlEntity = Nothing 'TODO: Implement the correct entity list: GetEntity(Screen.Level.Entities, checkPosition)
        '            If Not e Is Nothing Then
        '                If e.GetPropertyValue(Of Boolean)("isstairs") = True Then
        '                    Steps += 1
        '                    checkPosition.X += Screen.Camera.moveDirectionX
        '                    checkPosition.Z += Screen.Camera.moveDirectionZ
        '                    checkPosition.Y -= 1
        '                Else
        '                    If e.GetPropertyValue(Of Boolean)("isscripttrigger") = True Then
        '                        'TODO: Convert the temp entity to be a XmlEntity: Player.Temp.ScriptEntity = e
        '                    ElseIf e.GetPropertyValue(Of Boolean)("iswarp") = True Then
        '                        e.WalkAgainst()
        '                    End If
        '                    foundSteps = False
        '                End If
        '            Else
        '                foundSteps = False
        '            End If
        '        End While

        '        Screen.Level.OverworldPokemon.Visible = False
        '        Screen.Level.OverworldPokemon.warped = True

        '        Dim s As String = "@Player:Move(1)" & vbNewLine &
        '        "@Player:SetMovement(" & Screen.Camera.moveDirectionX & ",-1," & Screen.Camera.moveDirectionZ & ")" & vbNewLine &
        '        "@Player:Move(" & Steps & ")" & vbNewLine &
        '        "@Pokemon:Hide" & vbNewLine &
        '        "@Player:SetMovement(" & Screen.Camera.moveDirectionX & ",0," & Screen.Camera.moveDirectionZ & ")" & vbNewLine &
        '        ":end"

        '        CType(Core.CurrentScreen, OverworldScreen).ActionScript.StartScript(s, 2, False)
        '    End If
        'End Sub

    End Class

End Namespace