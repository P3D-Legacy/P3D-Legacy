Public Class LoadingDots

    Private Shared PointsDelay As Single = 0.0F

    Public Shared Sub Update()
        PointsDelay += 0.1F
        If PointsDelay >= 4.0F Then
            PointsDelay = 0.0F
        End If
    End Sub

    Public Shared ReadOnly Property Dots As String
        Get
            Dim p As String = ""
            If PointsDelay >= 1.0F Then
                p &= "."
            End If
            If PointsDelay >= 2.0F Then
                p &= "."
            End If
            If PointsDelay >= 3.0F Then
                p &= "."
            End If
            Return p
        End Get
    End Property

End Class