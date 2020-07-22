Public Class TeachMovesScreen

    Inherits Screen

    Dim MovesList As New List(Of BattleSystem.Attack)
    Dim Pokemon As Pokemon

    Dim mainTexture As Texture2D
    Dim index As Integer = 0
    Dim scrollIndex As Integer = 0

    Public Shared LearnedMove As Boolean = False

    Public Sub New(ByVal currentScreen As Screen, ByVal PokemonIndex As Integer)
        Me.PreScreen = currentScreen
        Me.Pokemon = Core.Player.Pokemons(PokemonIndex)
        Me.Identification = Identifications.TeachMovesScreen

        For i = 0 To Pokemon.AttackLearns.Count - 1
            Dim tutorMove As BattleSystem.Attack = Pokemon.AttackLearns.Values(i)
            Dim learnLevel As Integer = Pokemon.AttackLearns.Keys(i)

            If learnLevel <= Pokemon.Level Then
                Dim canLearnMove As Boolean = True

                For Each learnedAttack As BattleSystem.Attack In Pokemon.Attacks
                    If learnedAttack.ID = tutorMove.ID Then
                        canLearnMove = False
                    End If
                Next

                For Each move As BattleSystem.Attack In MovesList
                    If move.ID = tutorMove.ID Then
                        canLearnMove = False
                    End If
                Next

                If canLearnMove = True Then
                    MovesList.Add(tutorMove)
                End If
            End If
        Next

        Me.MouseVisible = False
        Me.CanBePaused = True
        Me.CanMuteMusic = True

        LearnedMove = False

        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal PokemonIndex As Integer, ByVal MovesList() As BattleSystem.Attack)
        Me.PreScreen = currentScreen
        Me.Pokemon = Core.Player.Pokemons(PokemonIndex)
        Me.Identification = Identifications.TeachMovesScreen

        For Each a As BattleSystem.Attack In MovesList
            Dim canLearnMove As Boolean = False

            For Each tutorMove As BattleSystem.Attack In Pokemon.TutorAttacks
                If a.ID = tutorMove.ID Then
                    canLearnMove = True
                End If
            Next
            For i = 0 To Pokemon.AttackLearns.Count - 1
                Dim learnAttack As BattleSystem.Attack = Pokemon.AttackLearns.Values(i)
                If learnAttack.ID = a.ID Then
                    canLearnMove = True
                End If
            Next
            For Each eggMoveID As Integer In Pokemon.EggMoves
                If eggMoveID = a.ID Then
                    canLearnMove = True
                End If
            Next
            For Each TMMoveID As Integer In Pokemon.Machines
                If TMMoveID = a.ID Then
                    canLearnMove = True
                End If
            Next

            For Each learnedAttack As BattleSystem.Attack In Pokemon.Attacks
                If learnedAttack.ID = a.ID Then
                    canLearnMove = False
                End If
            Next

            For Each move As BattleSystem.Attack In Me.MovesList
                If move.ID = a.ID Then
                    canLearnMove = False
                End If
            Next

            If canLearnMove = True Then
                Me.MovesList.Add(a)
            End If
        Next

        Me.MouseVisible = False
        Me.CanBePaused = True
        Me.CanMuteMusic = True

        LearnedMove = False

        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()
        Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 0, 48, 48)), 2, New Rectangle(60, 100, 800, 480))

        Core.SpriteBatch.Draw(Pokemon.GetTexture(True), New Rectangle(80, 100, 128, 128), Color.White)

        Core.SpriteBatch.DrawString(FontManager.MiniFont, Pokemon.GetDisplayName() & Environment.NewLine & "Level: " & Pokemon.Level, New Vector2(80, 260), Color.Black)

        Core.SpriteBatch.DrawString(FontManager.MiniFont, "Pokémon's moves:", New Vector2(245, 140), Color.Black)
        For i = 0 To Pokemon.Attacks.Count - 1
            If i <= Pokemon.Attacks.Count - 1 Then
                DrawAttack(245, i, Pokemon.Attacks(i), False)
            End If
        Next

        If Me.MovesList.Count = 0 Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "The Pokémon cannot learn" & Environment.NewLine & "a new move here.", New Vector2(580, 140), Color.Black)
        Else
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Tutor moves (" & MovesList.Count & "):", New Vector2(580, 140), Color.Black)

            For i = scrollIndex To scrollIndex + 3
                If i <= MovesList.Count - 1 Then
                    DrawAttack(580, i, MovesList(i), True)
                End If
            Next
        End If

        Dim d As New Dictionary(Of Buttons, String)
        d.Add(Buttons.A, "Learn")
        d.Add(Buttons.B, "Close")

        DrawGamePadControls(d)
    End Sub

    Private Sub DrawAttack(ByVal x As Integer, ByVal i As Integer, ByVal A As BattleSystem.Attack, ByVal isLearnMove As Boolean)
        Dim y As Integer = i - scrollIndex
        If isLearnMove = False Then
            y = i
        End If

        Dim p As New Vector2(x, 160 + y * (64 + 32))

        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        If Me.index = i And isLearnMove = True Then
            CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
        End If

        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X - 18), CInt(p.Y), 256, 64))

        With Core.SpriteBatch
            .DrawString(FontManager.MiniFont, A.Name, New Vector2(p.X, CInt(p.Y + 26)), Color.Black)

            Dim c As Color = Color.Black
            Dim per As Integer = CInt((A.CurrentPP / A.MaxPP) * 100)

            If per <= 33 And per > 10 Then
                c = Color.Orange
            ElseIf per <= 10 Then
                c = Color.IndianRed
            End If

            .DrawString(FontManager.MiniFont, Localization.GetString("PP") & " " & A.CurrentPP & " / " & A.MaxPP, New Vector2(p.X + 130, CInt(p.Y + 58)), c)

            .Draw(TextureManager.GetTexture("GUI\Menus\Types", A.Type.GetElementImage(), ""), New Rectangle(CInt(p.X), CInt(p.Y + 54), 48, 16), Color.White)
        End With
    End Sub

    Public Overrides Sub Update()
        If Controls.Up(True, True, True, True, True) = True Then
            Me.index -= 1
        End If
        If Controls.Down(True, True, True, True, True) = True Then
            Me.index += 1
        End If

        index = index.Clamp(0, Me.MovesList.Count - 1)

        If index - scrollIndex > 3 Then
            scrollIndex += 1
        End If

        If index - scrollIndex < 0 Then
            scrollIndex -= 1
        End If

        If Controls.Accept(True, True, True) = True Then
            If Me.MovesList.Count = 0 Then
                Core.SetScreen(Me.PreScreen)
                SoundManager.PlaySound("select")
            Else
                LearnMove(MovesList(index))
                SoundManager.PlaySound("select")
            End If
        End If

        If Controls.Dismiss(True, True, True) = True Then
            Core.SetScreen(Me.PreScreen)
            SoundManager.PlaySound("select")
        End If
    End Sub

    Private Sub LearnMove(ByVal a As BattleSystem.Attack)
        If Pokemon.Attacks.Count < 4 Then
            LearnedMove = True
            Pokemon.Attacks.Add(BattleSystem.Attack.GetAttackByID(a.ID))
            TextBox.Show("... " & Pokemon.GetDisplayName() & " learned~" & a.Name & "!")
            SoundManager.PlaySound("success_small", False)
            Core.SetScreen(Me.PreScreen)
        Else
            Core.SetScreen(New LearnAttackScreen(Core.CurrentScreen.PreScreen, Me.Pokemon, BattleSystem.Attack.GetAttackByID(a.ID)))
        End If
    End Sub

End Class