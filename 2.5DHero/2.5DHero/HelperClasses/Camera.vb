Public Class Camera

    Public BoundingFrustum As BoundingFrustum
    Public View, Projection As Matrix
    Public Position As Vector3

    Public Yaw, Pitch As Single

    Protected _plannedMovement As New Vector3(0F)
    Protected _setPlannedMovement As Boolean = False

    Public Property PlannedMovement As Vector3
        Get
            Return Me._plannedMovement
        End Get
        Set(value As Vector3)
            Me._plannedMovement = value
            Me._setPlannedMovement = (value <> Vector3.Zero)
        End Set
    End Property

    Public Sub AddToPlannedMovement(ByVal v As Vector3)
        Me._plannedMovement += v
    End Sub

    Public Ray As Ray = New Ray()

    Public Turning As Boolean = False

    Public Speed As Single = 0.04F
    Public RotationSpeed As Single = 0.003

    Public FarPlane As Single = 30
    Public FOV As Single = 45.0

    Public Name As String = "INHERITS"

    Public Sub New(ByVal Name As String)
        Me.Name = Name
    End Sub

    Public Overridable Sub Update()
        Throw New NotImplementedException()
    End Sub

    Public Overridable Sub Turn(ByVal turns As Integer)
        Throw New NotImplementedException()
    End Sub

    Public Overridable Sub InstantTurn(ByVal turns As Integer)
        Throw New NotImplementedException()
    End Sub

    Public Function GetFacingDirection() As Integer
        If Yaw <= MathHelper.Pi * 0.25F Or Yaw > MathHelper.Pi * 1.75F Then
            Return 0
        End If
        If Yaw <= MathHelper.Pi * 0.75F And Yaw > MathHelper.Pi * 0.25F Then
            Return 1
        End If
        If Yaw <= MathHelper.Pi * 1.25F And Yaw > MathHelper.Pi * 0.75F Then
            Return 2
        End If
        If Yaw <= MathHelper.Pi * 1.75F And Yaw > MathHelper.Pi * 1.25F Then
            Return 3
        End If
        Return 0
    End Function

    Public Overridable Function GetPlayerFacingDirection() As Integer
        Return Me.GetFacingDirection()
    End Function

    Public Overridable Function GetForwardMovedPosition() As Vector3
        Throw New NotImplementedException()
    End Function

    Public Overridable Function GetMoveDirection() As Vector3
        Throw New NotImplementedException()
    End Function

    Public Overridable Sub Move(ByVal Steps As Single)
        Throw New NotImplementedException()
    End Sub

    Public Overridable Sub StopMovement()
        Throw New NotImplementedException()
    End Sub

    Public Overridable ReadOnly Property IsMoving() As Boolean
        Get
            Return False
        End Get
    End Property

    Public Sub CreateNewProjection(ByVal newFOV As Single)
        Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(newFOV), Core.GraphicsDevice.Viewport.AspectRatio, 0.01, Me.FarPlane)
        Me.FOV = newFOV
    End Sub

End Class