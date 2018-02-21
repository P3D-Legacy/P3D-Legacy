Public Class Minimap

    Public Squares As New List(Of MinimapSquare)
    Public DrawTextures As Boolean = True
    Public ObjectScale As Integer = 16

    Public Sub Initialize()
        Squares.Clear()

        Dim dontDraw() As String = {}

        Dim hasCeiling As Boolean = False
        Dim playerY As Single = Screen.Camera.Position.Y
        Dim playerR As New Rectangle(CInt(Screen.Camera.Position.X), CInt(Screen.Camera.Position.Z), 1, 1)
        Dim eList As New List(Of Entity)
        Dim fList As New List(Of Entity)
        Dim fullList As New List(Of Entity)
        For Each newE As Entity In Screen.Level.Entities
            eList.Add(newE)
            fullList.Add(newE)
        Next
        For Each newF As Entity In Screen.Level.Floors
            fList.Add(newF)
            fullList.Add(newF)
        Next
        For Each newE As Entity In Screen.Level.OffsetmapEntities
            eList.Add(newE)
            fullList.Add(newE)
        Next
        For Each newF As Entity In Screen.Level.OffsetmapFloors
            fList.Add(newF)
            fullList.Add(newF)
        Next

        For Each e As Entity In fullList
            If e.Position.Y > playerY Then
                Dim entityR As New Rectangle(CInt(e.Position.X), CInt(e.Position.Z), 1, 1)
                If entityR.Intersects(playerR) = True Then
                    hasCeiling = True
                    Exit For
                End If
            End If
        Next

        For Each e As Entity In fList
            If e.Visible = True Then
                If e.Position.Y >= Screen.Camera.Position.Y + 1.5F Then
                    If hasCeiling = True Then
                        Continue For
                    End If
                End If
                Me.Squares.Add(New MinimapSquare(New Rectangle(CInt(e.Position.X * OBJECTSCALE), CInt(e.Position.Z * OBJECTSCALE), CInt(e.Scale.X * OBJECTSCALE), CInt(e.Scale.Z * OBJECTSCALE)), Color.Black, e.Textures(0), DrawTextures))
            End If
        Next

        For Each e As Entity In eList
            If e.Position.Y >= Screen.Camera.Position.Y + 1.5F Then
                If hasCeiling = True Then
                    Continue For
                End If
            End If
            Select Case e.EntityID.ToLower
                Case "networkplayer"
                    Me.Squares.Add(New MinimapSquare(New Rectangle(CInt(e.Position.X * OBJECTSCALE), CInt(e.Position.Z * OBJECTSCALE), CInt(e.Scale.X * OBJECTSCALE), CInt(e.Scale.Z * OBJECTSCALE)), Color.Black, e.Textures(0), DrawTextures))
                Case "networkpokemon"
                    Dim t As Texture2D = CType(e, NetworkPokemon).Textures(0)
                    Me.Squares.Add(New MinimapSquare(New Rectangle(CInt(e.Position.X * OBJECTSCALE), CInt(e.Position.Z * OBJECTSCALE), CInt(e.Scale.X * OBJECTSCALE), CInt(e.Scale.Z * OBJECTSCALE)), Color.Black, t, DrawTextures))
                Case Else
                    If e.Visible = True Then
                        If dontDraw.Contains(e.EntityID) = False Then
                            Dim t As Texture2D = GetTextureFromEntity(e)

                            If Not t Is Nothing Then
                                Dim sO As Vector2 = GetScaleOffset(e.Scale)

                                Me.Squares.Add(New MinimapSquare(New Rectangle(CInt((e.Position.X + sO.X) * ObjectScale), CInt((e.Position.Z + sO.Y) * ObjectScale), CInt(e.Scale.X * ObjectScale), CInt(e.Scale.Z * ObjectScale)), Color.Black, t, DrawTextures))
                            End If
                        End If
                    End If
            End Select
        Next
    End Sub

    Private Shared Function GetScaleOffset(ByVal v As Vector3) As Vector2
        Dim scaleX As Single = (v.X - 1) / 2
        Dim scaleY As Single = (v.Z - 1) / 2

        Return New Vector2(scaleX * -1, scaleY * -1)
    End Function

    Private Shared Function GetTextureFromEntity(ByVal e As Entity) As Texture2D
        If e.Textures Is Nothing Then
            Return TextureManager.DefaultTexture
        End If
        If e.Textures.Count() > 0 And e.EntityID <> "" Then
            Dim t As Texture2D = e.Textures(0)

            Dim i As Integer = 0

            Select Case e.Model.ID
                Case 0
                    i = 0
                Case 1
                    i = Clamp(8, 0, e.TextureIndex.Count - 1)
                Case 2
                    i = Clamp(4, 0, e.TextureIndex.Count - 1)
                Case 3
                    i = 0
                Case 4
                    i = Clamp(2, 0, e.TextureIndex.Count - 1)
                Case 7
                    i = Clamp(4, 0, e.TextureIndex.Count - 1)
                Case 8
                    i = Clamp(4, 0, e.TextureIndex.Count - 1)
                Case 9
                    i = 0
                Case 10
                    i = 0
                Case 11
                    i = 0
                Case 12
                    i = Clamp(8, 0, e.TextureIndex.Count - 1)
                Case Else
                    i = 0
            End Select

            If e.TextureIndex(i) < 0 Then
                Return Nothing
            End If

            t = e.Textures(e.TextureIndex(i))

            Return t
        Else
            Return TextureManager.DefaultTexture
        End If
    End Function

    Dim delay As Single = 5.0F

    Public Sub Draw(ByVal drawOffset As Vector2)
        If delay > 0.0F Then
            delay -= 0.1F

            If delay <= 0.0F Then
                delay = 5.0F
                Initialize()
            End If
        End If

        Dim conRec As New Rectangle(CInt(Screen.Camera.Position.X * OBJECTSCALE - 160), CInt(Screen.Camera.Position.Z * OBJECTSCALE - 160), 336, 336)  '176, 176)
        Dim offset As New Vector2(Screen.Camera.Position.X * OBJECTSCALE - 128, Screen.Camera.Position.Z * OBJECTSCALE - 128)

        offset += drawOffset

        For Each sq As MinimapSquare In Squares
            If conRec.Intersects(sq.r) = True Then
                sq.Draw(Me.DrawTextures, offset)
            End If
        Next
    End Sub

End Class

Public Class MinimapSquare

    Public r As Rectangle
    Public c As Color
    Public t As Texture2D

    Public Sub New(ByVal r As Rectangle, ByVal c As Color, ByVal t As Texture2D, ByVal drawTextures As Boolean)
        Me.r = r
        Me.t = t

        If drawTextures = False Then
            Dim colorR As Integer = 0
            Dim colorG As Integer = 0
            Dim colorB As Integer = 0

            Dim cs(t.Width * t.Height - 1) As Color
            t.GetData(cs)
            Dim pixelCount As Integer = t.Width * t.Height

            For i = 0 To cs.Length - 1
                colorR += cs(i).R
                colorG += cs(i).G
                colorB += cs(i).B
            Next

            Me.c = New Color(New Vector3(CInt((colorR / pixelCount) / 255), CInt((colorG / pixelCount) / 255), CInt((colorB / pixelCount) / 255)))
        Else
            Me.c = c
        End If

    End Sub

    Public Sub Draw(ByVal DrawTextures As Boolean, ByVal offset As Vector2)
        If DrawTextures = True Then
            Core.SpriteBatch.Draw(t, New Rectangle(CInt(r.X - offset.X), CInt(r.Y - offset.Y), r.Width, r.Height), Color.White)
        Else
            Canvas.DrawRectangle(New Rectangle(CInt(r.X - offset.X), CInt(r.Y - offset.Y), r.Width, r.Height), c)
        End If
    End Sub

End Class