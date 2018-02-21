Public Class BASize

    Inherits BattleAnimation3D

    Public Enum Anchors
        Top
        Left
        Right
        Bottom
    End Enum

    Public Grow As Boolean = False
    Public EndSize As Vector3
    Public SizeSpeed As Single = 0.01F
    Public Anchor() As Anchors = {Anchors.Bottom}

    Public Change As New Vector3(1)

    Public Sub New(ByVal Position As Vector3, ByVal Texture As Texture2D, ByVal Scale As Vector3, ByVal Grow As Boolean, ByVal EndSize As Vector3, ByVal SizeSpeed As Single, ByVal startDelay As Single, ByVal endDelay As Single)
        MyBase.New(Position, Texture, Scale, startDelay, endDelay)

        Me.Grow = Grow
        Me.EndSize = EndSize
        Me.SizeSpeed = SizeSpeed

        Me.AnimationType = AnimationTypes.Size
    End Sub

    Public Overrides Sub DoActionActive()
        Dim saveScale As Vector3 = Me.Scale

        Dim changeX As Single = SizeSpeed * Change.X
        Dim changeY As Single = SizeSpeed * Change.Y
        Dim changeZ As Single = SizeSpeed * Change.Z

        If Grow = True Then
            If Me.Scale.X < Me.EndSize.X Then
                Me.Scale.X += changeX

                If Me.Scale.X >= Me.EndSize.X Then
                    Me.Scale.X = Me.EndSize.X
                End If
            End If
            If Me.Scale.Y < Me.EndSize.Y Then
                Me.Scale.Y += changeY

                If Me.Scale.Y >= Me.EndSize.Y Then
                    Me.Scale.Y = Me.EndSize.Y
                End If
            End If
            If Me.Scale.Z < Me.EndSize.Z Then
                Me.Scale.Z += changeZ

                If Me.Scale.Z >= Me.EndSize.Z Then
                    Me.Scale.Z = Me.EndSize.Z
                End If
            End If
        Else
            If Me.Scale.X > Me.EndSize.X Then
                Me.Scale.X -= changeX

                If Me.Scale.X <= Me.EndSize.X Then
                    Me.Scale.X = Me.EndSize.X
                End If
            End If
            If Me.Scale.Y > Me.EndSize.Y Then
                Me.Scale.Y -= changeY

                If Me.Scale.Y <= Me.EndSize.Y Then
                    Me.Scale.Y = Me.EndSize.Y
                End If
            End If
            If Me.Scale.Z > Me.EndSize.Z Then
                Me.Scale.Z -= changeZ

                If Me.Scale.Z <= Me.EndSize.Z Then
                    Me.Scale.Z = Me.EndSize.Z
                End If
            End If
        End If

        If Anchor.Contains(Anchors.Bottom) = True Then
            Dim diffY As Single = saveScale.Y - Me.Scale.Y
            Me.Position.Y -= diffY / 2
        End If
        If Anchor.Contains(Anchors.Top) = True Then
            Dim diffY As Single = saveScale.Y - Me.Scale.Y
            Me.Position.Y += diffY / 2
        End If
        If Anchor.Contains(Anchors.Left) = True Then
            Dim diffX As Single = saveScale.X - Me.Scale.X
            Me.Position.X -= diffX / 2
        End If
        If Anchor.Contains(Anchors.Right) = True Then
            Dim diffX As Single = saveScale.X - Me.Scale.X
            Me.Position.X += diffX / 2
        End If

        If Me.EndSize = Me.Scale Then
            Me.Ready = True
        End If
    End Sub

    Public Sub SetChange(ByVal changeX As Single, ByVal changeY As Single, ByVal changeZ As Single)
        Me.Change = New Vector3(changeX, changeY, changeZ)
    End Sub

End Class