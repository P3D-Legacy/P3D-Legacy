Public Class BAMove

    Inherits BattleAnimation3D

    Public Destination As Vector3
    Public MoveSpeed As Single
    Public SpinX As Boolean = False
    Public SpinZ As Boolean = False

    Public SpinSpeedX As Single = 0.1F
    Public SpinSpeedZ As Single = 0.1F

    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal Destination As Vector3, ByVal Speed As Single, ByVal SpinX As Boolean, ByVal SpinZ As Boolean, ByVal startDelay As Single, ByVal endDelay As Single)
        MyBase.New(Position, Texture, Scale, startDelay, endDelay)

        Me.Destination = Destination
        Me.MoveSpeed = Speed
        Me.Scale = Scale

        Me.SpinX = SpinX
        Me.SpinZ = SpinZ

        Me.AnimationType = AnimationTypes.Move
    End Sub

    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal Destination As Vector3, ByVal Speed As Single, ByVal startDelay As Single, ByVal endDelay As Single)
        Me.New(Position, Texture, Scale, Destination, Speed, False, False, startDelay, endDelay)
    End Sub

    Public Overrides Sub DoActionUpdate()
        Spin()
    End Sub

    Public Overrides Sub DoActionActive()
        Move()
    End Sub

    Private Sub Spin()
        If Me.SpinX = True Then
            Me.Rotation.X += SpinSpeedX
        End If
        If Me.SpinZ = True Then
            Me.Rotation.Z += SpinSpeedZ
        End If
    End Sub

    Private Sub Move()
        If Me.Position.X < Me.Destination.X Then
            Me.Position.X += Me.MoveSpeed

            If Me.Position.X >= Me.Destination.X Then
                Me.Position.X = Me.Destination.X
            End If
        ElseIf Me.Position.X > Me.Destination.X Then
            Me.Position.X -= Me.MoveSpeed

            If Me.Position.X <= Me.Destination.X Then
                Me.Position.X = Me.Destination.X
            End If
        End If
        If Me.Position.Y < Me.Destination.Y Then
            Me.Position.Y += Me.MoveSpeed

            If Me.Position.Y >= Me.Destination.Y Then
                Me.Position.Y = Me.Destination.Y
            End If
        ElseIf Me.Position.Y > Me.Destination.Y Then
            Me.Position.Y -= Me.MoveSpeed

            If Me.Position.Y <= Me.Destination.Y Then
                Me.Position.Y = Me.Destination.Y
            End If
        End If
        If Me.Position.Z < Me.Destination.Z Then
            Me.Position.Z += Me.MoveSpeed

            If Me.Position.Z >= Me.Destination.Z Then
                Me.Position.Z = Me.Destination.Z
            End If
        ElseIf Me.Position.Z > Me.Destination.Z Then
            Me.Position.Z -= Me.MoveSpeed

            If Me.Position.Z <= Me.Destination.Z Then
                Me.Position.Z = Me.Destination.Z
            End If
        End If

        'Dim x As Integer = 0
        'Dim y As Integer = 0
        'Dim z As Integer = 0

        'If Destination.X < Me.Position.X Then
        '    x = -1
        'ElseIf Destination.X > Me.Position.X Then
        '    x = 1
        'End If
        'If Destination.Y < Me.Position.Y Then
        '    y = -1
        'ElseIf Destination.X > Me.Position.Y Then
        '    y = 1
        'End If
        'If Destination.Z < Me.Position.Z Then
        '    z = -1
        'ElseIf Destination.Z > Me.Position.Z Then
        '    z = 1
        'End If

        'Dim v As Vector3 = New Vector3(x, y, z) * Speed
        'Position.X += v.X
        'Position.Z += v.Z
        'Position.Y += v.Y

        'If CInt(Destination.X) = CInt(Me.Position.X) Then
        '    If Functions.ToPositive(Me.Position.X - Destination.X) <= Me.Speed Then
        '        Me.Position.X = CInt(Me.Position.X)
        '    End If
        'End If
        'If CInt(Destination.Y) = CInt(Me.Position.Y) Then
        '    If Functions.ToPositive(Me.Position.Y - Destination.Y) <= Me.Speed + 0.01F Then
        '        Me.Position.Y = CInt(Me.Position.Y)
        '    End If
        'End If
        'If CInt(Destination.Z) = CInt(Me.Position.Z) Then
        '    If Functions.ToPositive(Me.Position.Z - Destination.Z) <= Me.Speed Then
        '        Me.Position.Z = CInt(Me.Position.Z)
        '    End If
        'End If

        If Me.Position = Destination Then
            Me.Ready = True
        End If
    End Sub

End Class