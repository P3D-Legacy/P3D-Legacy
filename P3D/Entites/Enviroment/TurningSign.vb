Public Class TurningSign

    Inherits Entity

    Dim TurningSpeed As Single = 1 / 100 * MathHelper.Pi

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        Dim randomValue As Single = CSng(MathHelper.TwoPi * Core.Random.NextDouble())
        Select Case Me.ActionValue
            Case 1
                Me.Rotation.X = randomValue
            Case 2
                Me.Rotation.Z = randomValue
            Case Else
                Me.Rotation.Y = randomValue
        End Select

        If Me.AdditionalValue <> "" Then
            If StringHelper.IsNumeric(Me.AdditionalValue) = True Then
                Me.TurningSpeed = CSng(CInt(Me.AdditionalValue) / 100 * MathHelper.Pi)
            End If
        End If
        Me.CreateWorldEveryFrame = True
    End Sub

    Public Overrides Sub UpdateEntity()
        If Me.AdditionalValue <> "" Then
            If Me.TurningSpeed <> CSng(CInt(Me.AdditionalValue) / 100 * MathHelper.Pi) Then
                If StringHelper.IsNumeric(Me.AdditionalValue) = True Then
                    Me.TurningSpeed = CSng(CInt(Me.AdditionalValue) / 100 * MathHelper.Pi)
                End If
            End If
        End If
        Select Case Me.ActionValue
            Case 1
                Me.Rotation.X += TurningSpeed
            Case 2
                Me.Rotation.Z += TurningSpeed
            Case Else
                Me.Rotation.Y += TurningSpeed
        End Select

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Render()
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, True)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

End Class