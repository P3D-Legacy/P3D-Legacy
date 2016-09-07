Public Class Shader

    Public Position As Vector3
    Public Size As Vector3
    Public Shader As Vector3
    Public StopOnContact As Boolean
    Public HasBeenApplied As Boolean = False

    Public Sub New(ByVal Position As Vector3, ByVal Size As Vector3, ByVal Shader As Vector3, ByVal StopOnContact As Boolean)
        Me.Position = Position
        Me.Size = Size
        Me.Shader = Shader
        Me.StopOnContact = StopOnContact

        If Screen.Level.World.EnvironmentType = World.EnvironmentTypes.Outside And World.GetTime() = 0 Then
            Me.Shader.X += 0.2F
            Me.Shader.Y += 0.2F
            Me.Shader.Z += 0.5F
        End If
    End Sub

    Public Sub ApplyShader(ByVal Entities() As Entity)
        For x = 0 To Size.X - 1
            For z = 0 To Size.Z - 1
                For Each e As Entity In Entities
                    If StopOnContact = True Then
                        If CInt(e.Position.X) = x + Position.X And CInt(e.Position.Z) = z + Position.Z And e.Position.Y <= Position.Y Then
                            e.Shaders.Add(Shader)
                        End If
                    Else
                        If CInt(e.Position.X) = x + Position.X And CInt(e.Position.Z) = z + Position.Z And e.Position.Y <= Position.Y + Size.Y And e.Position.Y >= Position.Y Then
                            e.Shaders.Add(Shader)
                        End If
                    End If
                Next
            Next
        Next

        Me.HasBeenApplied = True
    End Sub

End Class