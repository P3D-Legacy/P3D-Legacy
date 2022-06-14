Public Class MessageBulb

    Inherits Entity

    Public Enum NotificationTypes
        Waiting = 0
        Exclamation = 1
        Shouting = 2
        Question = 3
        Note = 4
        Heart = 5
        Unhappy = 6
        Happy = 7
        Friendly = 8
        Poisoned = 9
        Battle = 10
        Wink = 11
        AFK = 12
        Angry = 13
        CatFace = 14
        Unsure = 15
    End Enum

    Public NotificationType As NotificationTypes = NotificationTypes.Exclamation
    Dim setTexture As Boolean = False
    Dim delay As Single = 0.0F

    Public Sub New(ByVal Position As Vector3, ByVal NotificationType As NotificationTypes)
        MyBase.New(Position.X, Position.Y, Position.Z, "MessageBulb", {}, {0, 0}, False, 0, New Vector3(0.8F), BaseModel.BillModel, 0, "", New Vector3(1.0F))

        Me.NotificationType = NotificationType
        LoadTexture()
        Me.NeedsUpdate = True
        Me.delay = 8.0F

        Me.DropUpdateUnlessDrawn = False
    End Sub

    Public Overrides Sub Update()
        If Me.delay > 0.0F Then
            Me.delay -= 0.1F
            If Me.delay <= 0.0F Then
                Me.delay = 0.0F
                Me.CanBeRemoved = True
            End If
        End If
    End Sub

    Private Sub LoadTexture()
        If Me.setTexture = False Then
            Me.setTexture = True

            Dim r As New Rectangle(0, 0, 16, 16)
            Select Case Me.NotificationType
                Case NotificationTypes.Waiting
                    r = New Rectangle(0, 0, 16, 16)
                Case NotificationTypes.Exclamation
                    r = New Rectangle(16, 0, 16, 16)
                Case NotificationTypes.Shouting
                    r = New Rectangle(32, 0, 16, 16)
                Case NotificationTypes.Question
                    r = New Rectangle(48, 0, 16, 16)
                Case NotificationTypes.Note
                    r = New Rectangle(0, 16, 16, 16)
                Case NotificationTypes.Heart
                    r = New Rectangle(16, 16, 16, 16)
                Case NotificationTypes.Unhappy
                    r = New Rectangle(32, 16, 16, 16)
                Case NotificationTypes.Happy
                    r = New Rectangle(0, 32, 16, 16)
                Case NotificationTypes.Friendly
                    r = New Rectangle(16, 32, 16, 16)
                Case NotificationTypes.Poisoned
                    r = New Rectangle(32, 32, 16, 16)
                Case NotificationTypes.Battle
                    r = New Rectangle(48, 16, 16, 16)
                Case NotificationTypes.Wink
                    r = New Rectangle(48, 32, 16, 16)
                Case NotificationTypes.AFK
                    r = New Rectangle(0, 48, 16, 16)
                Case NotificationTypes.Angry
                    r = New Rectangle(16, 48, 16, 16)
                Case NotificationTypes.CatFace
                    r = New Rectangle(32, 48, 16, 16)
                Case NotificationTypes.Unsure
                    r = New Rectangle(48, 48, 16, 16)
            End Select

            Me.Textures = {P3D.TextureManager.GetTexture("emoticons", r)}
        End If
    End Sub

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
            CreatedWorld = False
        End If

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Me.Textures, True)
    End Sub

End Class