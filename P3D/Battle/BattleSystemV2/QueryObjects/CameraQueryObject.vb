Namespace BattleSystem

    Public Class CameraQueryObject

        Inherits QueryObject

        Private _targetPosition As Vector3 = New Vector3(0)
        Private _startPosition As Vector3 = New Vector3(0)

        Private _startRotationSpeed As Single = 0.008F
        Private _targetRotationSpeed As Single = 0.008F

        Private _startSpeed As Single = 0.04F
        Private _targetSpeed As Single = 0.04F

        Private _startYaw As Single = 0.0F
        Private _targetYaw As Single = 0.0F

        Private _startPitch As Single = 0.0F
        Private _targetPitch As Single = 0.0F

        Private _applied As Boolean = False
        Private _ready As Boolean = False

        Public ApplyCurrentCamera As Boolean = False
        Public ReplacePVP As Boolean = False

        Public Property TargetPosition As Vector3
            Get
                Return Me._targetPosition
            End Get
            Set(value As Vector3)
                Me._targetPosition = value
            End Set
        End Property

        Public Property TargetRotationSpeed As Single
            Get
                Return Me._targetRotationSpeed
            End Get
            Set(value As Single)
                Me._targetRotationSpeed = value
            End Set
        End Property

        Public Property TargetYaw As Single
            Get
                Return Me._targetYaw
            End Get
            Set(value As Single)
                Me._targetYaw = value
            End Set
        End Property

        Public Property TargetPitch As Single
            Get
                Return Me._targetPitch
            End Get
            Set(value As Single)
                Me._targetPitch = value
            End Set
        End Property

        Public Property StartPosition As Vector3
            Get
                Return Me._startPosition
            End Get
            Set(value As Vector3)
                Me._startPosition = value
            End Set
        End Property

        Public Property StartRotationSpeed As Single
            Get
                Return Me._startRotationSpeed
            End Get
            Set(value As Single)
                Me._startRotationSpeed = value
            End Set
        End Property

        Public Property StartYaw As Single
            Get
                Return Me._startYaw
            End Get
            Set(value As Single)
                Me._startYaw = value
            End Set
        End Property

        Public Property StartPitch As Single
            Get
                Return Me._startPitch
            End Get
            Set(value As Single)
                Me._startPitch = value
            End Set
        End Property

        Public Sub New(ByVal TargetPosition As Vector3, ByVal StartPosition As Vector3, ByVal StartSpeed As Single, ByVal StartYaw As Single, ByVal StartPitch As Single)
            MyBase.New(QueryTypes.CameraMovement)

            Me._targetPosition = TargetPosition
            Me._startPosition = StartPosition

            Me._startSpeed = StartSpeed
            Me._targetSpeed = StartSpeed

            Me._startYaw = StartYaw
            Me._targetYaw = StartYaw

            Me._startPitch = StartPitch
            Me._targetPitch = StartPitch
        End Sub

        Public Sub New(ByVal TargetPosition As Vector3, ByVal StartPosition As Vector3, ByVal TargetSpeed As Single, ByVal StartSpeed As Single, ByVal TargetYaw As Single, ByVal StartYaw As Single, ByVal TargetPitch As Single, ByVal StartPitch As Single)
            MyBase.New(QueryTypes.CameraMovement)

            Me._targetPosition = TargetPosition
            Me._startPosition = StartPosition

            Me._startSpeed = StartSpeed
            Me._targetSpeed = TargetSpeed

            Me._startYaw = StartYaw
            Me._targetYaw = TargetYaw

            Me._startPitch = StartPitch
            Me._targetPitch = TargetPitch
        End Sub

        Public Sub New(ByVal TargetPosition As Vector3, ByVal StartPosition As Vector3, ByVal TargetSpeed As Single, ByVal StartSpeed As Single, ByVal TargetYaw As Single, ByVal StartYaw As Single, ByVal TargetPitch As Single, ByVal StartPitch As Single, ByVal TargetRotationSpeed As Single, ByVal StartRotationSpeed As Single)
            MyBase.New(QueryTypes.CameraMovement)

            Me._targetPosition = TargetPosition
            Me._startPosition = StartPosition

            Me._startSpeed = StartSpeed
            Me._targetSpeed = TargetSpeed

            Me._startYaw = StartYaw
            Me._targetYaw = TargetYaw

            Me._startPitch = StartPitch
            Me._targetPitch = TargetPitch

            Me._startRotationSpeed = StartRotationSpeed
            Me._targetRotationSpeed = TargetRotationSpeed
        End Sub

        Private Sub Apply(ByRef C As BattleCamera, ByVal BattleScreen As BattleScreen)
            C.Position = Me._startPosition
            C.TargetPosition = Me._targetPosition

            C.Speed = Me._startSpeed
            C.TargetSpeed = Me._targetSpeed

            C.Yaw = Me._startYaw
            C.TargetYaw = Me._targetYaw

            C.Pitch = Me._startPitch
            C.TargetPitch = Me._targetPitch

            C.RotationSpeed = Me._startRotationSpeed
            C.TargetRotationSpeed = Me._targetRotationSpeed
        End Sub

        Public Overrides Sub Update(ByVal BV2Screen As BattleScreen)
            If Me.ApplyCurrentCamera = True Then
                Me.ApplyCurrentCamera = False
                Me.StartPosition = Screen.Camera.Position
                Me.StartYaw = Screen.Camera.Yaw
                Me.StartPitch = Screen.Camera.Pitch
                Me.StartRotationSpeed = Screen.Camera.RotationSpeed
            End If

            If _applied = False Then
                Me._applied = True
                Apply(CType(Screen.Camera, BattleCamera), BV2Screen)
            End If

            If CType(Screen.Camera, BattleCamera).IsReady = True Then
                Me._ready = True
            End If
        End Sub

        Public Sub SetTargetToStart()
            Me._startPitch = Me._targetPitch
            Me._startPosition = Me._targetPosition
            Me._startRotationSpeed = Me._targetRotationSpeed
            Me._startSpeed = Me._targetSpeed
            Me._startYaw = Me._targetYaw
        End Sub

        Public Overrides ReadOnly Property IsReady() As Boolean
            Get
                Return Me._ready
            End Get
        End Property

        Public Overrides ReadOnly Property UpdateCamera() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Shared Shadows Function FromString(ByVal input As String) As QueryObject
            Dim d() As String = input.Split(CChar("|"))

            Dim c As New CameraQueryObject(New Vector3(CSng(STSE(d(0))), CSng(STSE(d(1))), CSng(STSE(d(2)))), New Vector3(CSng(STSE(d(3))), CSng(STSE(d(4))), CSng(STSE(d(5)))), CSng(STSE(d(6))), CSng(STSE(d(7))), CSng(STSE(d(8))), CSng(STSE(d(9))), CSng(STSE(d(10))), CSng(STSE(d(11))), CSng(STSE(d(12))), CSng(STSE(d(13))))
            c.ApplyCurrentCamera = CBool(14)
            c.PassThis = CBool(15)

            Return c
        End Function

        Public Overrides Function ToString() As String
            Dim s As String = SEST(_targetPosition.X.ToString()) & "|" & SEST(_targetPosition.Y.ToString()) & "|" & SEST(_targetPosition.Z.ToString()) & "|" &
                SEST(_startPosition.X.ToString()) & "|" & SEST(_startPosition.Y.ToString()) & "|" & SEST(_startPosition.Z.ToString()) & "|" &
                SEST(_targetSpeed.ToString()) & "|" &
                SEST(_startSpeed.ToString()) & "|" &
                SEST(_targetYaw.ToString()) & "|" &
                SEST(_startYaw.ToString()) & "|" &
                SEST(_targetPitch.ToString()) & "|" &
                SEST(_startPitch.ToString()) & "|" &
                SEST(_targetRotationSpeed.ToString()) & "|" &
                SEST(_startRotationSpeed.ToString()) & "|" &
                ApplyCurrentCamera.ToNumberString() & "|" &
                PassThis.ToNumberString()

            Return "{CAMERA|" & s & "}"
        End Function

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Private Shared Function STSE(ByVal s As String) As String
            Return s.Replace(".", GameController.DecSeparator)
        End Function

        Private Shared Function SEST(ByVal s As String) As String
            Return s.Replace(GameController.DecSeparator, ".")
        End Function

    End Class

End Namespace