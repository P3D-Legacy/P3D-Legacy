Public Class HallOfFameScreen

    Inherits Screen

    Public SavedOverworld As OverworldStorage
    Dim alpha As Integer = 255
    Dim alphaFade As Integer = -1
    Dim loadedLevel As Boolean = False

    Dim menuState As Integer = 0
    Dim texture As Texture2D

    Dim Entries As New List(Of HallOfFameEntry)

    Dim Scroll As Integer = 0
    Dim Cursor As Integer = 0
    Dim SelectedEntry As HallOfFameEntry = Nothing

    Dim Preselect As Integer = -1

    Public Class PokemonWrapper

        Private _pokemon As Pokemon = Nothing
        Private _pokemonData As String
        Private _loaded As Boolean = False

        Public Sub New(ByVal PokemonData As String)
            Me._pokemonData = PokemonData
        End Sub

        Public Sub New(ByVal p As Pokemon)
            Me._loaded = True
            Me._pokemon = p
        End Sub

        Public Function GetPokemon() As Pokemon
            If _loaded = False Then
                _loaded = True
                _pokemon = Pokemon.GetPokemonByData(Me._pokemonData)
            End If
            Return Me._pokemon
        End Function

    End Class

    Class HallOfFameEntry

        Public PokemonList As New List(Of PokemonWrapper)
        Public ID As Integer = 0
        Public Name As String = ""
        Public PlayTime As String = ""
        Public OT As String = ""
        Public Skin As String = ""
        Public Points As String = ""

        Public Sub New(ByVal ID As Integer)
            Me.ID = ID

            Dim data() As String = Core.Player.HallOfFameData.SplitAtNewline()
            For Each l As String In data
                If l.StartsWith(Me.ID.ToString() & ",{") = True Then
                    Dim pokeData As String = l.Remove(0, l.IndexOf("{"))
                    Me.PokemonList.Add(New PokemonWrapper(pokeData))
                ElseIf l.StartsWith(Me.ID.ToString() & ",(") = True Then
                    Dim playerData() As String = l.Remove(l.Length - 1, 1).Remove(0, l.IndexOf("(") + 1).Split(CChar("|"))

                    Select Case playerData.Length
                        Case 4
                            Me.Name = playerData(0)
                            Me.PlayTime = playerData(1)
                            Me.Points = playerData(2)
                            Me.Skin = playerData(3)
                            Me.OT = "00000"
                        Case 5
                            Me.Name = playerData(0)
                            Me.PlayTime = playerData(1)
                            Me.Points = playerData(2)
                            Me.OT = playerData(3)
                            Me.Skin = playerData(4)
                    End Select
                End If
            Next
        End Sub

    End Class

    Public Sub New(ByVal currentScreen As Screen, ByVal i As Integer)
        SetupScreen(currentScreen)
        Me.Preselect = i
        Me.Cursor = Me.Preselect
    End Sub

    Public Sub New(ByVal currentScreen As Screen)
        SetupScreen(currentScreen)
    End Sub

    Private Sub SetupScreen(ByVal currentScreen As Screen)
        Me.Identification = Identifications.HallofFameScreen
        Me.PreScreen = currentScreen

        Me.CanBePaused = True
        Me.MouseVisible = True

        SavedOverworld = New OverworldStorage()
        SavedOverworld.SetToCurrentEnvironment()

        Me.texture = TextureManager.GetTexture("GUI\Menus\General")

        LoadEntries()
    End Sub

    Private Sub LoadEntries()
        Dim IDs As New List(Of Integer)

        For Each line As String In Core.Player.HallOfFameData.SplitAtNewline()
            If line.Contains(",") = True Then
                Dim s As String = line.Remove(line.IndexOf(","))
                If IsNumeric(s) = True Then
                    If IDs.Contains(CInt(s)) = False Then
                        IDs.Add(CInt(s))
                    End If
                End If
            End If
        Next

        For Each ID As Integer In IDs
            Me.Entries.Add(New HallOfFameEntry(ID))
        Next
    End Sub

    Public Overrides Sub ChangeTo()
        If Me.loadedLevel = False Then
            Screen.TextBox.Showing = False
            Screen.PokemonImageView.Showing = False
            Screen.ChooseBox.Showing = False

            Effect = New BasicEffect(Core.GraphicsDevice)
            Effect.FogEnabled = True
            SkyDome = New SkyDome()
            Camera = New BattleSystem.BattleCamera()

            Level = New Level()
            Level.Load("indigo\halloffame_interface.dat")

            ResetCamera()
            Me.loadedLevel = True
        End If
    End Sub

    Private Sub ResetCamera()
        Dim bCamera As BattleSystem.BattleCamera = CType(Camera, BattleSystem.BattleCamera)
        bCamera.Position = New Vector3(10, 1, 14)
        bCamera.Yaw = 0.0F
        bCamera.Pitch = -0.04F

        bCamera.TargetPosition = New Vector3(10, 0.3F, 9)
        bCamera.TargetYaw = bCamera.Yaw
        bCamera.TargetPitch = bCamera.Pitch
    End Sub

    Public Overrides Sub Draw()
        Level.Draw()

        Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width - 128, Core.windowSize.Height), New Color(84, 198, 216, alpha))

        For y = -64 To Core.windowSize.Height Step 64
            Core.SpriteBatch.Draw(Me.texture, New Rectangle(Core.windowSize.Width - 128, y + TileOffset, 128, 64), New Rectangle(48, 0, 16, 16), New Color(255, 255, 255, alpha))
        Next

        Canvas.DrawGradient(New Rectangle(0, 0, CInt(Core.windowSize.Width), 200), New Color(42, 167, 198, alpha), New Color(42, 167, 198, 0), False, -1)
        Canvas.DrawGradient(New Rectangle(0, CInt(Core.windowSize.Height - 200), CInt(Core.windowSize.Width), 200), New Color(42, 167, 198, 0), New Color(42, 167, 198, alpha), False, -1)

        Core.SpriteBatch.DrawString(FontManager.MainFont, "Hall of Fame", New Vector2(100, 24), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)

        If Not Me.SelectedEntry Is Nothing Then
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Entry No. " & (Me.SelectedEntry.ID + 1).ToString(), New Vector2(-1100 + (255 - alpha) * 5.0F, 70), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
        End If

        If Me.Preselect = -1 Then
            For i = Scroll To Scroll + 5
                If i <= Me.Entries.Count - 1 Then
                    Dim p As Integer = i - Scroll

                    Core.SpriteBatch.Draw(Me.texture, New Rectangle(100, 100 + p * 96, 64, 64), New Rectangle(16, 16, 16, 16), New Color(255, 255, 255, alpha))
                    Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64, 100 + p * 96, 64 * 8, 64), New Rectangle(32, 16, 16, 16), New Color(255, 255, 255, alpha))
                    Core.SpriteBatch.Draw(Me.texture, New Rectangle(100 + 64 * 9, 100 + p * 96, 64, 64), New Rectangle(16, 16, 16, 16), New Color(255, 255, 255, alpha), 0.0F, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0F)

                    Core.SpriteBatch.DrawString(FontManager.MainFont, "Entry No. " & (Me.Entries(i).ID + 1).ToString(), New Vector2(120, 116 + p * 96), New Color(0, 0, 0, alpha), 0.0F, Vector2.Zero, 1.25F, SpriteEffects.None, 0.0F)

                    For d = 0 To Me.Entries(i).PokemonList.Count - 1
                        Core.SpriteBatch.Draw(Me.Entries(i).PokemonList(d).GetPokemon().GetMenuTexture(), New Rectangle(360 + d * 40, 116 + p * 96, 32, 32), New Color(255, 255, 255, alpha))
                    Next
                End If
            Next

            Core.SpriteBatch.DrawString(FontManager.MainFont, "Entries: ", New Vector2(90 + 64 * 11, 119), New Color(0, 0, 0, alpha))
            Core.SpriteBatch.DrawString(FontManager.MainFont, Me.Entries.Count.ToString(), New Vector2(190 + 64 * 11, 119), New Color(255, 255, 255, alpha))

            DrawCursor()
        End If

        If Me.menuState = 2 Then
            If Camera.Name = "BattleV2" Then
                If CType(Camera, BattleSystem.BattleCamera).IsReady = True Then
                    DrawInformation()
                End If
            End If
        End If
    End Sub

    Private Sub DrawCursor()
        Dim cPosition As Vector2 = New Vector2(520, 100 + Me.Cursor * 96 - 42)

        Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
        Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), New Color(255, 255, 255, alpha))
    End Sub

    Private Sub DrawInformation()
        Dim id As Integer = 0
        Select Case CInt(Screen.Camera.Position.X)
            Case 10
                id = -1
            Case 11
                id = 0
            Case 9
                id = 1
            Case 12
                id = 2
            Case 8
                id = 3
            Case 13
                id = 4
            Case 7
                id = 5
        End Select

        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width - 500), 50, 450, 200), New Color(0, 0, 0, 150))

        Dim pos As New Vector2(CInt(Core.windowSize.Width - 500), 50)

        If id = -1 Then
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Player " & SelectedEntry.Name, New Vector2(pos.X + 4, pos.Y + 4), Color.White, 0.0F, Vector2.Zero, 1.5F, SpriteEffects.None, 0.0F)
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Playtime" & vbNewLine & vbNewLine & "OT" & vbNewLine & vbNewLine & "Points", New Vector2(pos.X + 10, pos.Y + 54), Color.White, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
            Core.SpriteBatch.DrawString(FontManager.MainFont, SelectedEntry.PlayTime & vbNewLine & vbNewLine & SelectedEntry.OT & vbNewLine & vbNewLine & SelectedEntry.Points, New Vector2(pos.X + 116, pos.Y + 55), New Color(173, 216, 230), 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
        Else
            Dim p As Pokemon = SelectedEntry.PokemonList(id).GetPokemon()
            Core.SpriteBatch.Draw(p.GetMenuTexture(), New Rectangle(CInt(pos.X + 4), CInt(pos.Y + 6), 32, 32), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MainFont, p.GetDisplayName(), New Vector2(pos.X + 40, pos.Y + 4), Color.White, 0.0F, Vector2.Zero, 1.5F, SpriteEffects.None, 0.0F)
            Core.SpriteBatch.DrawString(FontManager.MainFont, "Level" & vbNewLine & vbNewLine & "OT" & vbNewLine & vbNewLine & "Type 1" & vbNewLine & vbNewLine & "Type 2", New Vector2(pos.X + 10, pos.Y + 43), Color.White, 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)

            Dim s As String = p.Level & vbNewLine & vbNewLine & p.OT & " / " & p.CatchTrainerName & vbNewLine & vbNewLine & p.Type1.Type.ToString()
            If p.Type2.Type <> Element.Types.Blank Then
                s &= vbNewLine & vbNewLine & p.Type2.Type.ToString()
            Else
                s &= vbNewLine & vbNewLine & "none"
            End If

            Core.SpriteBatch.DrawString(FontManager.MainFont, s, New Vector2(pos.X + 116, pos.Y + 44), New Color(173, 216, 230), 0.0F, Vector2.Zero, 1.0F, SpriteEffects.None, 0.0F)
        End If
    End Sub

    Public Shared TileOffset As Integer = 0

    Public Overrides Sub Update()
        Lighting.UpdateLighting(Screen.Effect)
        Camera.Update()
        Level.Update()

        Select Case menuState
            Case 0 'Menu updates
                Me.UpdateMenu()
            Case 1 'Menu fade/appear
                Me.alpha += (3 * Me.alphaFade)
                If Me.alpha >= 255 Then
                    Me.alpha = 255
                    Me.menuState = 0
                ElseIf Me.alpha <= 0 Then
                    Me.alpha = 0
                    Me.menuState = 2
                End If
            Case 2 'Level updates
                Me.UpdateCamera()
        End Select

        TileOffset += 1
        If TileOffset >= 64 Then
            TileOffset = 0
        End If
    End Sub

    Private Sub UpdateMenu()
        If KeyBoardHandler.KeyPressed(KeyBindings.SpecialKey) = True Or ControllerHandler.ButtonPressed(Buttons.Y) = True Then
            Me.Entries.Reverse()
        End If

        If Me.Preselect > -1 Then
            SelectEntry(Me.Preselect)
            Exit Sub
        ElseIf Me.Preselect = -2 Then
            Me.ChangeSavedScreen()
            Core.SetScreen(New TransitionScreen(Core.CurrentScreen, Me.PreScreen, Color.Black, False))
        End If

        If Controls.Down(True, True, True, True, True, True) = True Then
            Me.Cursor += 1
            If Controls.ShiftDown() = True Then
                Me.Cursor += 4
            End If
        End If
        If Controls.Up(True, True, True, True, True, True) = True Then
            Me.Cursor -= 1
            If Controls.ShiftDown() = True Then
                Me.Cursor -= 4
            End If
        End If

        While Me.Cursor > 5
            Me.Cursor -= 1
            Me.Scroll += 1
        End While
        While Me.Cursor < 0
            Me.Cursor += 1
            Me.Scroll -= 1
        End While

        If Me.Entries.Count < 7 Then
            Me.Scroll = 0
        Else
            Me.Scroll = Me.Scroll.Clamp(0, Me.Entries.Count - 6)
        End If

        If Me.Entries.Count < 6 Then
            Me.Cursor = Me.Cursor.Clamp(0, Me.Entries.Count - 1)
        Else
            Me.Cursor = Me.Cursor.Clamp(0, 5)
        End If

        If Me.Entries.Count > 0 Then
            If Controls.Accept(True, False, False) = True Then
                For i = Scroll To Scroll + 5
                    If i <= Me.Entries.Count - 1 Then
                        If New Rectangle(100, 100 + (i - Scroll) * 96, 640, 64).Contains(MouseHandler.MousePosition) = True Then
                            If i = Cursor + Scroll Then
                                SelectEntry(Me.Scroll + Me.Cursor)
                            Else
                                Cursor = i - Scroll
                            End If
                        End If
                    End If
                Next
            End If

            If Controls.Accept(False, True, True) = True Then
                SelectEntry(Me.Scroll + Me.Cursor)
            End If
        End If

        If Controls.Dismiss(True, True, True) = True Then
            Me.ChangeSavedScreen()
            Core.SetScreen(New TransitionScreen(Core.CurrentScreen, Me.PreScreen, Color.Black, False))
        End If
    End Sub

    Private Sub SelectEntry(ByVal index As Integer)
        For i = 0 To Screen.Level.Entities.Count - 1
            If i <= Screen.Level.Entities.Count - 1 Then
                Dim Entity As Entity = Screen.Level.Entities(i)
                If Entity.EntityID = "NPC" Then
                    If CType(Entity, NPC).NPCID = 9001 Then
                        Screen.Level.Entities.Remove(Entity)
                        i -= 1
                    End If
                End If
            End If
        Next

        Dim d As Integer = 1
        Dim e As Integer = 0
        For Each p As PokemonWrapper In Me.Entries(index).PokemonList
            Dim x As Integer = d
            If e = 1 Then
                e = 0
                x = -d
                d += 1
            Else
                e = 1
            End If

            Dim n As NPC = CType(Entity.GetNewEntity("NPC", New Vector3(10 + x, 0, 7), {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 1, "", "", New Vector3(0), {PokemonForms.GetOverworldSpriteName(p.GetPokemon()), 2, "", 9001, True, "Still", New List(Of Rectangle)}), NPC)
            Level.Entities.Add(n)
        Next

        Dim playerNPC As NPC = CType(Entity.GetNewEntity("NPC", New Vector3(10, 0, 7), {Nothing}, {0, 0}, False, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1), 1, "", "", New Vector3(0), {Me.Entries(index).Skin, 2, "", 9001, False, "Still", New List(Of Rectangle)}), NPC)
        Level.Entities.Add(playerNPC)
        Me.SelectedEntry = Me.Entries(index)

        Me.menuState = 1
        Me.alphaFade = -1
        Me.ResetCamera()
    End Sub

    Private Sub UpdateCamera()
        Dim bCamera As BattleSystem.BattleCamera = CType(Screen.Camera, BattleSystem.BattleCamera)
        If bCamera.IsReady = True Then
            Dim d As Integer = 1
            Dim e As Integer = 0

            Dim max As Integer = 0
            Dim min As Integer = 0

            For Each p As PokemonWrapper In Me.SelectedEntry.PokemonList
                Dim x As Integer = d
                If e = 1 Then
                    e = 0
                    x = -d
                    d += 1
                Else
                    e = 1
                End If

                If x < min Then
                    min = x
                End If
                If x > max Then
                    max = x
                End If
            Next

            min += 10
            max += 10

            If Controls.Left(True, True, False, True, True, True) = True And CInt(bCamera.Position.X) > min Then
                bCamera.TargetPosition.X = bCamera.Position.X - 1
            End If
            If Controls.Right(True, True, False, True, True, True) = True And CInt(bCamera.Position.X) < max Then
                bCamera.TargetPosition.X = bCamera.Position.X + 1
            End If
        End If
        If Controls.Dismiss() = True Then
            Me.menuState = 1
            Me.alphaFade = 1
            bCamera.TargetPosition = New Vector3(10, 1, 15)
            If Me.Preselect > -1 Then
                Me.Preselect = -2
            End If
        End If
    End Sub

    Public Sub ChangeSavedScreen()
        Screen.Level = SavedOverworld.Level
        Screen.Camera = SavedOverworld.Camera
        Screen.Effect = SavedOverworld.Effect
        Screen.SkyDome = SavedOverworld.SkyDome
        Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType)
    End Sub

    Public Shared Function GetHallOfFameCount() As Integer
        Dim count As Integer = -1

        If Core.Player.HallOfFameData <> "" Then
            Dim data() As String = Core.Player.HallOfFameData.SplitAtNewline()

            For Each l As String In data
                Dim id As Integer = CInt(l.Remove(l.IndexOf(",")))
                If id > count Then
                    count = id
                End If
            Next
        End If

        Return (count + 1)
    End Function

End Class