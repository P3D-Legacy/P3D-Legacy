Public Class ChoosePokemonScreen

    Inherits Screen

    Private PokemonList As New List(Of Pokemon)

    Public Shared Selected As Integer = -1
    Public Shared Exited As Boolean = False

    Public index As Integer = 0
    Dim MainTexture As Texture2D
    Dim Texture As Texture2D
    Dim yOffset As Single = 0

    Dim Item As Item
    Dim Title As String = ""

    Dim used As Boolean = False
    Dim canExit As Boolean = True

    Public CanChooseFainted As Boolean = True
    Public CanChooseEgg As Boolean = True
    Public CanChooseHMPokemon As Boolean = True

    Public Delegate Sub DoStuff(ByVal PokeIndex As Integer)
    Dim ChoosePokemon As DoStuff
    Public ExitedSub As DoStuff

    Public LearnAttack As BattleSystem.Attack
    Public LearnType As Integer = 0

    Public Sub New(ByVal currentScreen As Screen, ByVal Item As Item, ByVal ChoosePokemon As DoStuff, ByVal Title As String, ByVal canExit As Boolean, ByVal canChooseFainted As Boolean, ByVal canChooseEgg As Boolean)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.ChoosePokemonScreen

        Me.MouseVisible = False
        Me.CanChat = Me.PreScreen.CanChat
        Me.CanBePaused = Me.PreScreen.CanBePaused

        Me.Item = Item
        Me.Title = Title
        Me.canExit = canExit

        Me.CanChooseEgg = canChooseEgg
        Me.CanChooseFainted = canChooseFainted

        MainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
        Texture = TextureManager.GetTexture(MainTexture, New Rectangle(0, 0, 48, 48), ContentPackManager.GetTextureResolution("GUI\Menus\Menu"))


        Me.index = Player.Temp.PokemonScreenIndex
        Me.ChoosePokemon = ChoosePokemon

        GetPokemonList()
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal Item As Item, ByVal ChoosePokemon As DoStuff, ByVal Title As String, ByVal canExit As Boolean)
        Me.New(currentScreen, Item, ChoosePokemon, Title, canExit, True, True)
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal Item As Item, ByVal Title As String, ByVal canExit As Boolean)
        Me.New(currentScreen, Item, Nothing, Title, canExit, True, True)
    End Sub

    Private Sub GetPokemonList()
        Me.PokemonList.Clear()
        For Each p As Pokemon In Core.Player.Pokemons
            Me.PokemonList.Add(Pokemon.GetPokemonByData(p.GetSaveData()))
        Next
    End Sub

    Public Overrides Sub Update()
        TextBox.Update()
        yOffset += 0.45F

        If TextBox.Showing = False Then
            If used = True Then
                Core.SetScreen(Me.PreScreen)
            Else
                If ChooseBox.Showing = False Then
                    If Controls.Dismiss() = True And Me.canExit = True Then
                        Exited = True
                        Selected = -1
                        If Not ExitedSub Is Nothing Then
                            used = True
                            ExitedSub(index)
                        Else
                            Core.SetScreen(Me.PreScreen)
                        End If
                    End If
                    If Controls.Accept() = True Then
                        ShowMenu()
                    End If

                    If Controls.Right(True, False) Then
                        index += 1
                    End If
                    If Controls.Left(True, False) Then
                        index -= 1
                    End If
                    If Controls.Down(True, False, False) Then
                        index += 2
                    End If
                    If Controls.Up(True, False, False) Then
                        index -= 2
                    End If

                    index = CInt(MathHelper.Clamp(index, 0, Me.PokemonList.Count - 1))
                Else
                    ChooseBox.Update(False)
                    If Controls.Dismiss() = True Then
                        ChooseBox.Showing = False
                    End If
                    If Controls.Accept() = True Then
                        AcceptMenu()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub AcceptMenu()
        Select Case ChooseBox.index
            Case 0
                If CanChoosePokemon(Me.PokemonList(index)) = True Then
                    Player.Temp.PokemonScreenIndex = Me.index
                    ChooseBox.Showing = False

                    Selected = index

                    If Not ChoosePokemon Is Nothing Then
                        ChoosePokemon(index)
                        GetPokemonList()
                    End If

                    used = True
                    Exited = False
                Else
                    ChooseBox.Showing = False
                    TextBox.Show("Cannot choose this~Pokémon.")
                End If
            Case 1
                ChooseBox.Showing = False
                Core.SetScreen(New PokemonStatusScreen(Me, index, {}, Me.PokemonList(index), True))
            Case 2
                ChooseBox.Showing = False
        End Select
    End Sub

    Private Function CanChoosePokemon(ByVal p As Pokemon) As Boolean
        If Me.CanChooseFainted = False Then
            If p.HP <= 0 Or p.Status = Pokemon.StatusProblems.Fainted Then
                Return False
            End If
        End If
        If Me.CanChooseEgg = False Then
            If p.IsEgg() = True Then
                Return False
            End If
        End If
        If Me.CanChooseHMPokemon = False Then
            If p.HasHMMove() = True Then
                Return False
            End If
        End If
        Return True
    End Function

    Dim MenuID As Integer = 0
    Dim mPressed As Boolean = False
    Private Sub ShowMenu()
        Me.MenuID = 0
        ChooseBox.Show({"Select", Localization.GetString("pokemon_screen_summary"), Localization.GetString("pokemon_screen_back")}, 0, {})
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        Canvas.DrawImageBorder(Texture, 2, New Rectangle(60, 100, 800, 480))
        Canvas.DrawImageBorder(Texture, 2, New Rectangle(60, 100, 480, 64))
        Core.SpriteBatch.DrawString(FontManager.InGameFont, Me.Title, New Vector2(142, 132), Color.Black)
        Core.SpriteBatch.Draw(Item.Texture, New Rectangle(78, 124, 48, 48), Color.White)

        If Me.canExit = True Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "PRESS E TO GO BACK", New Vector2(710, 580), Color.DarkGray)
        End If

        For i = 0 To Me.PokemonList.Count - 1
            DrawPokemonTile(i, Me.PokemonList(i))
        Next
        If Me.PokemonList.Count < 6 Then
            For i = Me.PokemonList.Count To 5
                DrawEmptyTile(i)
            Next
        End If

        If ChooseBox.Showing = True Then
            Dim Position As New Vector2(0, 0)
            Select Case Me.index
                Case 0, 2, 4
                    Position = New Vector2(606, 566 - ChooseBox.Options.Count * 48)
                Case 1, 3, 5
                    Position = New Vector2(60, 566 - ChooseBox.Options.Count * 48)
            End Select
            ChooseBox.Draw(Position)
        End If

        TextBox.Draw()
    End Sub

    Private Sub DrawEmptyTile(ByVal i As Integer)
        Dim p As Vector2
        Select Case i
            Case 0, 2, 4
                p = New Vector2(32, 32 + (48 + 10) * i)
            Case Else
                p = New Vector2(416, 32 + (48 + 10) * (i - 1))
        End Select
        p.X += 80
        p.Y += 180

        With Core.SpriteBatch
            .Draw(Texture, New Rectangle(CInt(p.X), CInt(p.Y), 32, 96), New Rectangle(0, 0, 16, 48), Color.White)
            For x = p.X + 32 To p.X + 288 Step 32
                .Draw(Texture, New Rectangle(CInt(x), CInt(p.Y), 32, 96), New Rectangle(16, 0, 16, 48), Color.White)
            Next
            .Draw(Texture, New Rectangle(CInt(p.X) + 320, CInt(p.Y), 32, 96), New Rectangle(32, 0, 16, 48), Color.White)

            .DrawString(FontManager.MiniFont, "EMPTY", New Vector2(CInt(p.X + 72), CInt(p.Y + 18)), Color.Black)
        End With
    End Sub

    Private Sub DrawPokemonTile(ByVal i As Integer, ByVal Pokemon As Pokemon)
        Dim BorderTexture As Texture2D
        If i = index Then
            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                BorderTexture = TextureManager.GetTexture(MainTexture, New Rectangle(0, 128, 48, 48), ContentPackManager.GetTextureResolution("GUI\Menus\Menu"))
            Else
                BorderTexture = TextureManager.GetTexture(MainTexture, New Rectangle(48, 0, 48, 48), ContentPackManager.GetTextureResolution("GUI\Menus\Menu"))
            End If
        Else
            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                BorderTexture = TextureManager.GetTexture(MainTexture, New Rectangle(48, 48, 48, 48), ContentPackManager.GetTextureResolution("GUI\Menus\Menu"))
            Else
                BorderTexture =  TextureManager.GetTexture(MainTexture, New Rectangle(0, 0, 48, 48), ContentPackManager.GetTextureResolution("GUI\Menus\Menu"))
            End If
        End If

        Dim p As Vector2
        Select Case i
            Case 0, 2, 4
                p = New Vector2(32, 32 + (48 + 10) * i)
            Case Else
                p = New Vector2(416, 32 + (48 + 10) * (i - 1))
        End Select
        p.X += 80
        p.Y += 180

        With Core.SpriteBatch
            .Draw(BorderTexture, New Rectangle(CInt(p.X), CInt(p.Y), 32, 96), New Rectangle(0, 0, 16, 48), Color.White)
            For x = p.X + 32 To p.X + 288 Step 32
                .Draw(BorderTexture, New Rectangle(CInt(x), CInt(p.Y), 32, 96), New Rectangle(16, 0, 16, 48), Color.White)
            Next
            .Draw(BorderTexture, New Rectangle(CInt(p.X) + 320, CInt(p.Y), 32, 96), New Rectangle(32, 0, 16, 48), Color.White)

            If Pokemon.IsEgg() = False Then
                Dim barX As Integer = CInt((Pokemon.HP / Pokemon.MaxHP) * 50)
                Dim barRectangle As Rectangle
                Dim barPercentage As Integer = CInt((Pokemon.HP / Pokemon.MaxHP) * 100)

                If barPercentage >= 50 Then
                    barRectangle = New Rectangle(113, 0, 1, 4)
                ElseIf barPercentage < 50 And barPercentage > 10 Then
                    barRectangle = New Rectangle(116, 0, 1, 4)
                ElseIf barPercentage <= 10 Then
                    barRectangle = New Rectangle(115, 0, 1, 4)
                End If
                For x = 0 To barX - 1
                    .Draw(MainTexture, New Rectangle(CInt(p.X + (x * 2) + 104), CInt(p.Y + 44), 4, 16), barRectangle, Color.White)
                Next

                For x = barX To 49
                    .Draw(MainTexture, New Rectangle(CInt(p.X + (x * 2) + 104), CInt(p.Y + 44), 4, 16), New Rectangle(114, 0, 1, 4), Color.White)
                Next
                .Draw(MainTexture, New Rectangle(CInt(p.X + 100), CInt(p.Y + 44), 4, 16), New Rectangle(112, 0, 1, 4), Color.White)
                .Draw(MainTexture, New Rectangle(CInt(p.X + 206), CInt(p.Y + 44), 4, 16), New Rectangle(112, 0, 1, 4), Color.White)

                .DrawString(FontManager.MiniFont, Pokemon.HP & " / " & Pokemon.MaxHP, New Vector2(CInt(p.X + 120), CInt(p.Y + 64)), Color.Black)
            End If

            Dim offset As Single = CSng(Math.Sin(yOffset))
            If i = index Then
                offset *= 3
            End If
            If Pokemon.Status = net.Pokemon3D.Game.Pokemon.StatusProblems.Fainted Then
                offset = 0
            End If

            .Draw(Pokemon.GetMenuTexture(), New Rectangle(CInt(p.X + 5), CInt(p.Y + offset + 10), 64, 64), BattleStats.GetStatColor(Pokemon.Status))
            .DrawString(FontManager.MiniFont, Pokemon.GetDisplayName(), New Vector2(CInt(p.X + 72), CInt(p.Y + 18)), Color.Black)

            If Pokemon.IsEgg() = False Then
                .Draw(MainTexture, New Rectangle(CInt(p.X + 72), CInt(p.Y + 46), 26, 12), New Rectangle(96, 10, 13, 6), Color.White)

                If Pokemon.Gender = net.Pokemon3D.Game.Pokemon.Genders.Male Then
                    .Draw(MainTexture, New Rectangle(CInt(p.X + FontManager.MiniFont.MeasureString(Pokemon.GetDisplayName()).X + 80), CInt(p.Y + 18), 12, 20), New Rectangle(96, 0, 6, 10), Color.White)
                ElseIf Pokemon.Gender = net.Pokemon3D.Game.Pokemon.Genders.Female Then
                    .Draw(MainTexture, New Rectangle(CInt(p.X + FontManager.MiniFont.MeasureString(Pokemon.GetDisplayName()).X + 80), CInt(p.Y + 18), 12, 20), New Rectangle(102, 0, 6, 10), Color.White)
                End If
            End If

            If Not Pokemon.Item Is Nothing And Pokemon.IsEgg() = False Then
                .Draw(Pokemon.Item.Texture, New Rectangle(CInt(p.X + 40), CInt(p.Y + 42), 24, 24), Color.White)
            End If

            Dim space As String = ""
            For x = 1 To 3 - Pokemon.Level.ToString().Length
                space &= " "
            Next

            Dim AttackLable As String = ""
            If LearnType > 0 Then
                AttackLable = "Unable!"
                Select Case LearnType
                    Case 1 'TM/HM
                        If CType(moveLearnArg, Items.TechMachine).CanTeach(Pokemon) = "" Then
                            AttackLable = "Able!"
                        End If
                End Select
            End If

            If Pokemon.IsEgg() = False Then
                .DrawString(FontManager.MiniFont, "Lv." & space & Pokemon.Level, New Vector2(CInt(p.X + 14), CInt(p.Y + 64)), Color.Black)
                .DrawString(FontManager.MiniFont, AttackLable, New Vector2(CInt(p.X + 230), CInt(p.Y + 64)), Color.Black)
            End If

            Dim StatusTexture As Texture2D = BattleStats.GetStatImage(Pokemon.Status)
            If Not StatusTexture Is Nothing Then
                Canvas.DrawRectangle(New Rectangle(CInt(p.X + 216), CInt(p.Y + 44), 42, 16), Color.Gray)
                Core.SpriteBatch.Draw(StatusTexture, New Rectangle(CInt(p.X + 218), CInt(p.Y + 46), 38, 12), Color.White)
            End If
        End With
    End Sub

    Dim moveLearnArg As Object = Nothing

    Public Sub SetupLearnAttack(ByVal a As BattleSystem.Attack, ByVal learnType As Integer, ByVal arg As Object)
        Me.LearnAttack = a
        Me.LearnType = learnType
        Me.moveLearnArg = arg
    End Sub

End Class