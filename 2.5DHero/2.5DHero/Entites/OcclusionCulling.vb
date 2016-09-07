Public Class OcclusionCulling

    Public Shared Sub Draw()
        If Screen.Level.Entities.Count > 0 Then
            Dim r As Rectangle = GetTransformed(Screen.Level.OwnPlayer)
            Debug.Print(r.ToString())
            Canvas.DrawRectangle(r, Color.Red)
        End If
    End Sub

    Public Shared Sub DoCulling(ByVal list As List(Of Entity))
        Dim pixels(Core.windowSize.Width * Core.windowSize.Height - 1) As Color


    End Sub

    Private Shared Function GetTransformed(ByVal Entity As Entity) As Rectangle
        Dim min As Vector2 = ProjectPoint(Entity.ViewBox.Min)
        Dim max As Vector2 = ProjectPoint(Entity.ViewBox.Max)

        Debug.Print(min.ToString() & max.ToString())

        Return New Rectangle(CInt(min.X), CInt(min.Y), CInt(max.X - min.X), CInt(max.Y - min.Y))
    End Function

    Private Shared Function ProjectPoint(ByVal Position As Vector3) As Vector2
        Dim mat As Matrix = Matrix.Identity * Screen.Camera.View * Screen.Camera.Projection

        Dim v4 As Vector4 = Vector4.Transform(Position, mat)

        Return New Vector2(CSng(((v4.X / v4.W + 1) * (Core.windowSize.Width / 2))), CSng(((1 - v4.Y / v4.W) * (Core.windowSize.Height / 2))))
    End Function

End Class