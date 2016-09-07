Public Class MapPreviewScreen

    Inherits Screen

    Public Shared MapViewMode As Boolean = False
    Public Shared MapViewModeMapPath As String = ""
    Public Shared MapViewModePosition As Vector3 = Nothing

    Dim ParticlesTexture As Texture2D

    Structure MapDisplay
        Public Position As Vector3
        Public Text As String
        Public Color As Color
    End Structure

    Dim TextDisplays As New List(Of MapDisplay)

    Public Sub New()
        Me.Identification = Identifications.MapPreviewScreen
        Me.CanChat = False
        Me.MouseVisible = False
        Me.CanBePaused = False
        Me.CanDrawDebug = True
        Me.CanGoFullscreen = True
        Me.CanMuteMusic = False
        Me.CanTakeScreenshot = True

        Effect = New BasicEffect(Core.GraphicsDevice)
        Effect.FogEnabled = True

        Camera = New MapPreviewCamera()
        SkyDome = New SkyDome()
        Level = New Level()
        Level.Load(MapViewModeMapPath)

        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)

        Me.ParticlesTexture = TextureManager.GetTexture("GUI\Overworld\Particles")
    End Sub

    Public Overrides Sub Update()
        Me.ControlMap()

        Lighting.UpdateLighting(Screen.Effect)
        Camera.Update()
        Me.PickWarp()
        Me.UpdateHiddenItems()
        Level.Update()
        SkyDome.Update()

        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
    End Sub

    Private Sub UpdateHiddenItems()
        For Each e As Entity In Screen.Level.Entities
            If e.EntityID.ToLower() = "itemobject" Then
                If e.Opacity <= 0.0F Then
                    Dim i As ItemObject = CType(e, ItemObject)

                    If i.IsHiddenItem() = True Then
                        i.Opacity = 1.0F
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub PickWarp()
        Me.TextDisplays.Clear()
        Dim Ray As Ray = Screen.Camera.Ray

        Dim entities As New List(Of Entity)
        entities.AddRange(Level.Entities.ToArray())

        If Core.GameOptions.LoadOffsetMaps > 0 Then
            entities.AddRange(Level.OffsetmapEntities.ToArray())
        End If

        For Each e As Entity In entities
            Select Case e.EntityID
                Case "WarpBlock"
                    If e.Shaders.Count > 0 Then
                        If e.Shaders.Last().X = 1.51337135F Then
                            e.Shaders.RemoveAt(e.Shaders.Count - 1)
                        End If
                    End If

                    Dim result As Single? = Ray.Intersects(e.ViewBox)
                    If result.HasValue = True Then
                        If result.Value < 12.0F And result.Value > 0.1F Then
                            e.Visible = True
                            e.Shaders.Add(New Vector3(1.51337135F))

                            Me.TextDisplays.Add(New MapDisplay() With {.Text = "To: " & e.AdditionalValue.GetSplit(0), .Position = e.Position, .Color = New Color(0, 232, 255, 200)})

                            If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                                CType(e, WarpBlock).Warp(True)
                            End If
                        End If
                    End If
                Case "NPC"
                    Dim result As Single? = Ray.Intersects(e.ViewBox)
                    If result.HasValue = True Then
                        If result.Value < 4.0F And result.Value > 0.1F Then
                            Dim t As String = ""
                            Select Case e.ActionValue
                                Case 0
                                    t = CType(e, NPC).Name & ": """ & GetDisplayText(e.AdditionalValue) & """"
                                Case 1
                                    t = CType(e, NPC).Name & ": Script start (" & e.AdditionalValue & ")"
                                Case 3
                                    t = CType(e, NPC).Name & ": Direct script input"
                                Case Else
                                    t = CType(e, NPC).Name & ": Script start (" & e.AdditionalValue & ")"
                            End Select

                            Me.TextDisplays.Add(New MapDisplay() With {.Text = t, .Position = e.Position, .Color = Color.LightCoral})
                        End If
                    End If
                Case "ScriptBlock"
                    Dim result As Single? = Ray.Intersects(e.ViewBox)
                    If result.HasValue = True Then
                        If result.Value < 4.0F And result.Value > 0.1F Then
                            Dim s As ScriptBlock = CType(e, ScriptBlock)
                            Dim t As String = ""
                            Select Case s.GetActivationID()
                                Case 0
                                    t = "ScriptBlock: Script start (" & s.ScriptID & ")"
                                Case 1
                                    t = "ScriptBlock: """ & GetDisplayText(s.ScriptID) & """"
                                Case 2
                                    t = "ScriptBlock: Direct script input"
                            End Select

                            If t <> "" Then
                                Me.TextDisplays.Add(New MapDisplay() With {.Text = t, .Position = e.Position, .Color = Color.LightGreen})
                            End If
                        End If
                    End If
                Case "SignBlock"
                    Dim result As Single? = Ray.Intersects(e.ViewBox)
                    If result.HasValue = True Then
                        If result.Value < 4.0F And result.Value > 0.1F Then
                            Dim t As String = ""

                            Select Case e.ActionValue
                                Case 0, 3
                                    t = "Sign: """ & GetDisplayText(e.AdditionalValue) & """"
                                Case 1
                                    t = "Sign: Script start (" & e.AdditionalValue & ")"
                                Case 2
                                    t = "Sign: Direct script input"
                            End Select

                            If t <> "" Then
                                Me.TextDisplays.Add(New MapDisplay() With {.Text = t, .Position = e.Position, .Color = Color.LightYellow})
                            End If
                        End If
                    End If
            End Select
        Next
    End Sub

    Private Function GetDisplayText(ByVal source As String) As String
        Dim text As String = source.Replace("~", " ").Replace("*", " ")

        If text.Length > 16 Then
            text = text.Remove(16)
            text &= "..."
        End If

        Return text
    End Function

    Private Sub ControlMap()
        If KeyBoardHandler.KeyPressed(KeyBindings.EscapeKey) = True Or ControllerHandler.ButtonPressed(Buttons.Start) = True Then
            Core.GameInstance.Exit()
        End If

        If Controls.Up(True, True, False, False, False, True) = True Then
            Core.GameOptions.RenderDistance += 1
        End If
        If Controls.Down(True, True, False, False, False, True) = True Then
            Core.GameOptions.RenderDistance -= 1
        End If
        Core.GameOptions.RenderDistance = Core.GameOptions.RenderDistance.Clamp(0, 5)

        If Controls.Left(False, True, False, False, False, True) = True Then
            Core.GameOptions.LoadOffsetMaps -= 1
        End If
        If Controls.Right(False, True, False, False, False, True) = True Then
            Core.GameOptions.LoadOffsetMaps += 1
        End If
        Core.GameOptions.LoadOffsetMaps = Core.GameOptions.LoadOffsetMaps.Clamp(0, 100)

        If KeyBoardHandler.KeyPressed(Keys.R) = True Or ControllerHandler.ButtonPressed(Buttons.Y) = True Then
            Core.OffsetMaps.Clear()
            Level.Load(Level.LevelFile)
        End If

        If KeyBoardHandler.KeyPressed(Keys.Q) = True Or ControllerHandler.ButtonPressed(Buttons.X) = True Then
            Camera.Position = MapViewModePosition
        End If
    End Sub

    Public Overrides Sub Draw()
        SkyDome.Draw(Camera.FOV)

        Level.Draw()

        World.DrawWeather(Screen.Level.World.CurrentMapWeather)

        If Core.GameOptions.ShowGUI = True Then
            Dim P As Vector2 = Core.GetMiddlePosition(New Size(16, 16))
            Core.SpriteBatch.Draw(ParticlesTexture, New Rectangle(CInt(P.X), CInt(P.Y), 16, 16), New Rectangle(0, 0, 9, 9), Color.White)

            Dim offsetString As String = "OFF"
            If Core.GameOptions.LoadOffsetMaps > 0 Then
                offsetString = "ON (QUALITY: " & (100 - Core.GameOptions.LoadOffsetMaps).ToString() & ")"
            End If

            Dim t As String = "MAP: " & Level.LevelFile & vbNewLine &
                "LEVEL: " & Level.MapName & vbNewLine &
                "RENDERDISTANCE: " & Core.GameOptions.RenderDistance.ToString() & vbNewLine &
                "OFFSETMAPS: " & offsetString

            Core.SpriteBatch.DrawString(FontManager.MiniFont, t, New Vector2(2, Core.windowSize.Height - FontManager.MiniFont.MeasureString(t).Y - 2), Color.White)

            Dim t2 As String = "WASD: Move around" & vbNewLine &
                "MOUSE SCROLL: Change camera speed" & vbNewLine &
                "LEFT MOUSE CLICK: Interact" & vbNewLine &
                "ARROW KEYS UP/DOWN: Change RenderDistance" & vbNewLine &
                "ARROW KEYS LEFT/RIGHT: Change Offset Map Quality" & vbNewLine &
                "R: Reload map" & vbNewLine &
                "Q: Replace player" & vbNewLine &
                "ESC: Close Map Preview"

            Core.SpriteBatch.DrawString(FontManager.MiniFont, t2, New Vector2(Core.windowSize.Width - FontManager.MiniFont.MeasureString(t2).X - 2, Core.windowSize.Height - FontManager.MiniFont.MeasureString(t2).Y - 2), Color.White)

            Dim t3 As String = "MAP PREVIEW MODE"
            Core.SpriteBatch.DrawString(FontManager.MiniFont, t3, New Vector2(Core.windowSize.Width - FontManager.MiniFont.MeasureString(t3).X - 2, 2), Color.White)

            Me.DrawMapDisplays()
        End If
    End Sub

    Private Sub DrawMapDisplays()
        For Each des As MapDisplay In Me.TextDisplays
            Dim p As Vector2 = des.Position.ProjectPoint(Screen.Camera.View, Screen.Camera.Projection)
            p.X -= FontManager.ChatFont.MeasureString(des.Text).X / 2.0F

            SpriteBatch.DrawString(FontManager.ChatFont, des.Text, New Vector2(p.X + 2, p.Y + 2), New Color(0, 0, 0, 128), 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
            SpriteBatch.DrawString(FontManager.ChatFont, des.Text, p, des.Color, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
        Next
    End Sub

    Public Shared Sub DetectMapPath(ByVal arg As String)
        Dim data() As String = arg.Split(CChar("|"))
        If data.Length = 1 Then
            MapViewMode = True
            MapViewModeMapPath = data(0)
            MapViewModePosition = Vector3.Zero
        ElseIf data.Length = 4 Then
            MapViewMode = True
            MapViewModeMapPath = data(0)
            MapViewModePosition = New Vector3(CSng(data(1).Replace(".", GameController.DecSeparator)),
                                              CSng(data(2).Replace(".", GameController.DecSeparator)),
                                              CSng(data(3).Replace(".", GameController.DecSeparator)))
        End If
    End Sub

End Class