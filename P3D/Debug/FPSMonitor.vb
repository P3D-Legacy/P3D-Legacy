Public Class FPSMonitor

    Public Value As Double
    Public Sample As TimeSpan

    Private sw As Stopwatch
    Private Frames As Integer

    Public Sub New()
        Me.Sample = TimeSpan.FromMilliseconds(100)

        Value = 0
        Frames = 0
        sw = Stopwatch.StartNew()
    End Sub

    Public Sub Update(ByVal GameTime As GameTime)
        If sw.Elapsed > Sample Then
            Me.Value = Frames / sw.Elapsed.TotalSeconds

            Me.sw.Reset()
            Me.sw.Start()
            Me.Frames = 0
        End If
    End Sub

    Public Sub DrawnFrame()
        Me.Frames += 1
    End Sub

End Class