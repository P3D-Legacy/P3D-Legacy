Public Class FPSMonitor

    Private ReadOnly _stopwatch As Stopwatch
    Private ReadOnly _sample As TimeSpan

    Private _frames As Integer

    Public Property Value As Double

    Public Sub New()
        _stopwatch = Stopwatch.StartNew()
        _sample = TimeSpan.FromMilliseconds(100)
    End Sub

    Public Sub Update()
        If _stopwatch.Elapsed > _sample Then
            Value = _frames / _stopwatch.Elapsed.TotalSeconds

            _stopwatch.Restart()
            _frames = 0
        End If
    End Sub

    Public Sub DrawnFrame()
        _frames += 1
    End Sub

End Class