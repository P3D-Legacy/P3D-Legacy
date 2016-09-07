Public Class SignBlock

    Inherits Entity

    'Action value:  0=normal text in additional value
    '               1=script path in additional value
    '               2=direct script input in additional value
    '               3=normal text in additional value, block not resized

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        Me.Scale = New Vector3(0.7)

        If ActionValue < 3 Then
            Me.Position.Y -= 0.15F
        End If

        Me.CreatedWorld = False
    End Sub

    Public Overrides Sub ClickFunction()
        Dim canRead As Boolean = False

        Select Case Screen.Camera.GetPlayerFacingDirection() 
            Case 1, 3
                If Me.Rotation.Y = MathHelper.Pi * 1.5F Or Me.Rotation.Y = MathHelper.Pi * 0.5F Then
                    canRead = True
                End If
            Case 0, 2
                If Me.Rotation.Y = MathHelper.Pi Or Me.Rotation.Y = MathHelper.TwoPi Or Me.Rotation.Y = 0 Then
                    canRead = True
                End If
        End Select

        If canRead = True Then
            Dim oScreen As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
            If oScreen.ActionScript.IsReady = True Then
                SoundManager.PlaySound("select")
                Select Case Me.ActionValue
                    Case 0, 3
                        oScreen.ActionScript.StartScript(Me.AdditionalValue, 1)
                    Case 1
                        oScreen.ActionScript.StartScript(Me.AdditionalValue, 0)
                    Case 2
                        oScreen.ActionScript.StartScript(Me.AdditionalValue.Replace("<br>", vbNewLine), 2)
                    Case Else
                        oScreen.ActionScript.StartScript(Me.AdditionalValue, 1)
                End Select
            End If
        End If
    End Sub

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, True)
    End Sub

End Class