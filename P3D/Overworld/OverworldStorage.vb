''' <summary>
''' Provides variables to store an overworld instance.
''' </summary>
Public Class OverworldStorage

    Public OverworldScreen As Screen
    Public Level As Level
    Public Camera As Camera
    Public Effect As BasicEffect
    Public SkyDome As SkyDome

    ''' <summary>
    ''' Uses the currently active Overworld screen and active level/camera/effect/skydome instances.
    ''' </summary>
    Public Sub SetToCurrentEnvironment()
        Dim s As Screen = Core.CurrentScreen
        While s.Identification <> Screen.Identifications.OverworldScreen And Not s.PreScreen Is Nothing
            s = s.PreScreen
        End While
        Me.OverworldScreen = s
        Me.Camera = Screen.Camera
        Me.Level = Screen.Level
        Me.Effect = Screen.Effect
        Me.SkyDome = Screen.SkyDome
    End Sub

End Class
