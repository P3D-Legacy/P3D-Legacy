Public Class LearnAttackScreen

    Inherits Screen

    Dim Pokemon As Pokemon
    Dim newAttacks As List(Of BattleSystem.Attack)
    Dim mainTexture As Texture2D

    Dim chosen As Boolean = False
    Dim index As Integer = 0

    Dim AttackIndex As Integer = 0
    Dim AttackPos As Single = 320.0F

    Dim canForget As Boolean = True
    Dim MachineItemID As String = "-1"

    Dim currentCharIndex As Integer = 0

    Public Sub New(ByVal currentScreen As Screen, ByVal Pokemon As Pokemon, ByVal newAttacks As List(Of BattleSystem.Attack))
        Me.New(currentScreen, Pokemon, newAttacks, "-1")
    End Sub
    Public Sub New(ByVal currentScreen As Screen, ByVal Pokemon As Pokemon, ByVal newAttack As BattleSystem.Attack)
        Me.New(currentScreen, Pokemon, New List(Of BattleSystem.Attack) From {newAttack}, "-1")
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal Pokemon As Pokemon, ByVal newAttack As BattleSystem.Attack, ByVal MachineItemID As String)
        Me.New(currentScreen, Pokemon, New List(Of BattleSystem.Attack) From {newAttack}, MachineItemID)
    End Sub
    Public Sub New(ByVal currentScreen As Screen, ByVal Pokemon As Pokemon, ByVal newAttacks As List(Of BattleSystem.Attack), ByVal MachineItemID As String)
        Me.Identification = Identifications.LearnAttackScreen

        Me.PreScreen = currentScreen
        Me.Pokemon = Pokemon
        Me.newAttacks = newAttacks
        Me.MachineItemID = MachineItemID

        mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
    End Sub

    Public Overrides Sub Update()
        If TextBox.Showing = False Then
            If Me.currentCharIndex < GetText().Length Then
                Me.currentCharIndex += 1
                Exit Sub
            End If

            If chosen = False Then
                Core.GameInstance.IsMouseVisible = False
                If Controls.Up(True, True, True, True) = True Then
                    Me.AttackIndex -= 1
                End If
                If Controls.Down(True, True, True, True) = True Then
                    Me.AttackIndex += 1
                End If

                Me.AttackIndex = CInt(MathHelper.Clamp(Me.AttackIndex, 0, 4))

                If AttackIndex < 4 Then
                    If CBool(GameModeManager.GetGameRuleValue("CanForgetHM", "0")) = True Then
                        canForget = True
                    Else
                        canForget = Not Pokemon.Attacks(AttackIndex).IsHMMove
                    End If
                Else
                    canForget = True
                End If

                If Controls.Dismiss() = True Then
                    Me.AttackIndex = 4
                    chosen = True
                End If

                If Controls.Accept() = True Then
                    chosen = True
                End If
            Else
                Core.GameInstance.IsMouseVisible = True

                If Controls.Right(True, True, True) = True And canForget = True Then
                    index = 1
                End If
                If Controls.Left(True, True, True) = True Then
                    index = 0
                End If
                Dim accepted As Boolean = False
                For i = 0 To 1
                    If New Rectangle(CInt(Core.windowSize.Width / 2) - 182 + i * 192, 550, 128 + 32, 64 + 32).Contains(MouseHandler.MousePosition) = True Then
                        index = i

                        If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                            Select Case index
                                Case 0
                                    accepted = True
                                    ClickNo()
                                Case 1
                                    accepted = True
                                    ClickYes()
                            End Select
                        End If
                    End If
                Next

                If Controls.Accept(False, True) = True OrElse accepted = False AndAlso Controls.Accept(True, False, False) = True Then
                    Select Case index
                        Case 0
                            ClickNo()
                        Case 1
                            ClickYes()
                    End Select
                End If
                If Controls.Dismiss() = True Then
                    ClickNo()
                End If

            End If
        Else

            TextBox.Update()
        End If
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))
        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
        DrawText()

        If currentCharIndex < GetText().Length Then
            If TextBox.Showing = True Then
                TextBox.Draw()
            End If
        Else

            Dim p As Vector2 = New Vector2(96, 96)

            If Pokemon.Attacks.Count > 0 Then
                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X), CInt(p.Y), 672, 416))

                Dim A As BattleSystem.Attack
                If AttackIndex = 4 Then
                    A = newAttacks(0)
                Else
                    A = Pokemon.Attacks(AttackIndex)
                End If

                With Core.SpriteBatch
                    Dim Description As String = A.Description.Replace("’", "'").CropStringToWidth(FontManager.MainFont, 240)

                    Dim power As String = A.Power.ToString()
                    If power = "0" Then
                        power = "-"
                    End If

                    Dim acc As String = A.Accuracy.ToString()
                    If acc = "0" Then
                        acc = "-"
                    End If

                    .DrawString(FontManager.MainFont, Localization.GetString("property_Power", "Power") & ": " & power & Environment.NewLine & Localization.GetString("property_Accuracy", "Accuracy") & ": " & acc & Environment.NewLine & Environment.NewLine & Description, New Vector2(CInt(p.X + 352 + 48), p.Y + 48), Color.Black)
                    .Draw(A.GetDamageCategoryImage(), New Rectangle(CInt(p.X + 672 - 16 - 56), CInt(p.Y + 44), 56, 28), Color.White)
                End With

                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X), CInt(p.Y), 352, 416))
                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X), CInt(p.Y + 416 + 48), 352, 128))

                For i = 0 To Me.Pokemon.Attacks.Count - 1
                    DrawAttack(p, i, Me.Pokemon.Attacks(i))
                Next
                DrawAttack(p, 4, newAttacks(0))
            End If

            If chosen = True Then
                Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))

                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(Core.windowSize.Width / 2 - 352), 172, 704, 96))

                Dim drawText As String = ""
                If AttackIndex = 4 Then
                    drawText = Localization.GetString("learn_move_GiveUpOnLearning", "Give up on learning ""[MOVENAME]""").Replace("[MOVENAME]", newAttacks(0).Name)
                Else
                    drawText = Localization.GetString("learn_move_ForgetMoveToLearn", "Forget ""[OLDMOVENAME]"" to learn ""[NEWMOVENAME]""?").Replace("[OLDMOVENAME]", Pokemon.Attacks(AttackIndex).Name).Replace("[NEWMOVENAME]", newAttacks(0).Name)
                    If canForget = False Then
                        drawText = Localization.GetString("learn_move_CannotForgetMove", "Cannot forget the move ""[MOVENAME]"" because~it's a Hidden Machine move.").Replace("[MOVENAME]", Pokemon.Attacks(AttackIndex).Name).Replace("~", Environment.NewLine).Replace("*", Environment.NewLine)
                    End If
                End If

                Core.SpriteBatch.DrawString(FontManager.InGameFont, drawText, New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(drawText).X / 2), 200), Color.Black)

                Dim endIndex As Integer = 1
                If canForget = False Then
                    endIndex = 0
                End If

                For i = 0 To endIndex
                    Dim FontColor As Color = Color.Black
                    Dim Text As String = Localization.GetString("global_learn", "Learn")
                    If AttackIndex = 4 Then
                        Text = Localization.GetString("global_ok", "OK")
                    End If

                    If i = 0 Then
                        Text = Localization.GetString("global_cancel", "Cancel")
                    End If

                    If i = index Then
                        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
                        FontColor = Color.White
                    Else
                        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
                    End If

                    Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 182 + i * 192 + 22, 550, 128, 64))

                    If FontColor <> Color.Black Then
                        Core.SpriteBatch.DrawString(FontManager.InGameFont, Text, New Vector2(CInt(Core.windowSize.Width / 2) - 164 + i * 192 + 22 + 2, 404 + 180 + 2), Color.Black)
                    End If
                    Core.SpriteBatch.DrawString(FontManager.InGameFont, Text, New Vector2(CInt(Core.windowSize.Width / 2) - 164 + i * 192 + 22, 404 + 180), FontColor)

                Next
            End If

            TextBox.Draw()
        End If
    End Sub

    Private Sub DrawText()
        If currentCharIndex < (Pokemon.GetDisplayName() & " ").Length Then
            Core.SpriteBatch.DrawString(FontManager.MainFont, (Pokemon.GetDisplayName() & " ").Remove(currentCharIndex), New Vector2(120, 20), Color.White)
        Else
            Core.SpriteBatch.DrawString(FontManager.MainFont, Pokemon.GetDisplayName() & " ", New Vector2(120, 20), Color.White)
        End If
        If currentCharIndex > (Pokemon.GetDisplayName() & " ").Length Then
            Dim pokeTexture = Pokemon.GetMenuTexture()
            Dim pokeTextureScale As Vector2 = New Vector2(CSng(32 / pokeTexture.Width), CSng(32 / pokeTexture.Height))
            Core.SpriteBatch.Draw(pokeTexture, New Rectangle(CInt(FontManager.MainFont.MeasureString(Pokemon.GetDisplayName()).X + 120 + CInt(FontManager.MainFont.MeasureString(" ").X / 2)), 12, CInt(pokeTexture.Width * pokeTextureScale.X), CInt(pokeTexture.Height * pokeTextureScale.Y)), Color.White)
        End If
        If currentCharIndex > (Pokemon.GetDisplayName() & " ").Length + 1 Then
            Dim t As String = " " & (Localization.GetString("learn_move_AlreadyKnowsFourMoves1", "wants to learn ""[MOVENAME]"". But [POKEMONNAME] can only learn 4 moves.") & Environment.NewLine & " " & Localization.GetString("learn_move_AlreadyKnowsFourMoves2", "Do you want [POKEMONNAME] to forget a move to learn ""[MOVENAME]""?")).Replace("[MOVENAME]", newAttacks(0).Name).Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
            If currentCharIndex < GetText().Length Then
                Core.SpriteBatch.DrawString(FontManager.MainFont, t.Remove(currentCharIndex - (Pokemon.GetDisplayName() & " ").Length), New Vector2(FontManager.MainFont.MeasureString(Pokemon.GetDisplayName()).X + 152, 20), Color.White)
            Else
                Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(FontManager.MainFont.MeasureString(Pokemon.GetDisplayName()).X + 152, 20), Color.White)
            End If
        End If
    End Sub

    Private Function GetText() As String
        Return Pokemon.GetDisplayName() & " " & (Localization.GetString("learn_move_AlreadyKnowsFourMoves1", "wants to learn ""[MOVENAME]"". But [POKEMONNAME] can only learn 4 moves.") & Environment.NewLine & " " & Localization.GetString("learn_move_AlreadyKnowsFourMoves2", "Do you want [POKEMONNAME] to forget a move to learn ""[MOVENAME]""?")).Replace("[MOVENAME]", newAttacks(0).Name).Replace("[POKEMONNAME]", Pokemon.GetDisplayName())
    End Function

    Private Sub DrawAttack(ByVal StartPosition As Vector2, ByVal i As Integer, ByVal A As BattleSystem.Attack)
        Dim p As New Vector2(StartPosition.X + 16, StartPosition.Y + 32 + i * (64 + 32))

        If i = 4 Then
            p.Y += 80
        End If

        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
        Dim FontColor As Color = Color.Black
        If Me.AttackIndex = i Then
            CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            FontColor = Color.White
        End If

        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X) + 16, CInt(p.Y), 288, 64))

        With Core.SpriteBatch
            If FontColor <> Color.Black Then
                .DrawString(FontManager.MainFont, A.Name, New Vector2(CInt(p.X) + 34 + 2, CInt(p.Y + 26 + 2)), Color.Black)
            End If
            .DrawString(FontManager.MainFont, A.Name, New Vector2(CInt(p.X) + 34, CInt(p.Y + 26)), FontColor)

            Dim c As Color = FontColor
            Dim per As Integer = CInt((A.CurrentPP / A.MaxPP) * 100)

            If per <= 33 And per > 10 Then
                c = Color.Orange
            ElseIf per <= 10 Then
                c = Color.IndianRed
            End If

            If c <> Color.Black Then
                .DrawString(FontManager.MainFont, Localization.GetString("property_PP", "PP") & " " & A.CurrentPP & " / " & A.MaxPP, New Vector2(p.X + 96 + 2, CInt(p.Y + 56 + 2)), Color.Black)
            End If
            .DrawString(FontManager.MainFont, Localization.GetString("property_PP", "PP") & " " & A.CurrentPP & " / " & A.MaxPP, New Vector2(p.X + 96, CInt(p.Y + 56)), c)
            .Draw(TextureManager.GetTexture(Element.GetElementTexturePath(), A.Type.GetElementImage(), ""), New Rectangle(CInt(p.X + 34), CInt(p.Y + 56), 48, 16), Color.White)
        End With

    End Sub

    Private Sub ClickYes()
        If canForget = True Then
            Dim Text As String = Localization.GetString("learn_move_DidNotLearnMove", "[POKEMONNAME] didn't~learn [MOVENAME]!").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()).Replace("[MOVENAME]", newAttacks(0).Name)

            If AttackIndex <> 4 Then
                TeachMovesScreen.LearnedMove = True
                Text = Localization.GetString("learn_move_PokemonForgotMove", "1... 2... 3... and...*Ta-da!*[POKEMONNAME] forgot~[OLDMOVENAME] and...").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()).Replace("[OLDMOVENAME]", Pokemon.Attacks(AttackIndex).Name)
                Pokemon.Attacks.RemoveAt(AttackIndex)
                Pokemon.Attacks.Insert(AttackIndex, newAttacks(0))

                If Me.MachineItemID <> "-1" Then
                    PlayerStatistics.Track("TMs/HMs used", 1)
                    If CBool(GameModeManager.GetGameRuleValue("SingleUseTM", "0")) = True Then
                        Dim TechMachine As Item = Item.GetItemByID(Me.MachineItemID)
                        If TechMachine.ItemType = Items.ItemTypes.Machines Then
                            If TechMachine.IsGameModeItem = True Then
                                If CType(TechMachine, GameModeItem).gmIsHM = False Then
                                    TechMachine.RemoveItem()
                                End If
                            Else

                                If CType(TechMachine, Items.TechMachine).IsTM = True Then
                                    TechMachine.RemoveItem()
                                End If
                            End If
                        End If
                    End If
                End If
                PlayerStatistics.Track("Moves learned", 1)
                TextBox.FollowUp = AddressOf FollowUpText
            End If

            TextBox.Show(Text, {}, False, False)
            Core.GameInstance.IsMouseVisible = False
            If Me.newAttacks.Count > 1 Then
                Me.newAttacks.RemoveAt(0)
                Core.SetScreen(New LearnAttackScreen(Me.PreScreen, Me.Pokemon, Me.newAttacks))
            Else
                Core.SetScreen(Me.PreScreen)
            End If
        End If
    End Sub

    Private Sub ClickNo()
        Me.chosen = False
        Core.GameInstance.IsMouseVisible = False
    End Sub

    Private Sub FollowUpText()
        TextBox.Show(Localization.GetString("learn_move_PokemonLearnedMove_WithDots", "... [POKEMONNAME] learned~[MOVENAME]!").Replace("[POKEMONNAME]", Pokemon.GetDisplayName()).Replace("[MOVENAME]", newAttacks(0).Name))
        SoundManager.PlaySound("success_small", True)
    End Sub

End Class