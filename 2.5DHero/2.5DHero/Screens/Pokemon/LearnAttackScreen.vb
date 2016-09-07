Public Class LearnAttackScreen

    Inherits Screen

    Dim Pokemon As Pokemon
    Dim newAttack As BattleSystem.Attack
    Dim mainTexture As Texture2D

    Dim chosen As Boolean = False
    Dim index As Integer = 0

    Dim AttackIndex As Integer = 0
    Dim AttackPos As Single = 320.0F

    Dim canForget As Boolean = True
    Dim MachineItemID As Integer = -1

    Dim currentCharIndex As Integer = 0

    Public Sub New(ByVal currentScreen As Screen, ByVal Pokemon As Pokemon, ByVal newAttack As BattleSystem.Attack)
        Me.New(currentScreen, Pokemon, newAttack, -1)
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal Pokemon As Pokemon, ByVal newAttack As BattleSystem.Attack, ByVal MachineItemID As Integer)
        Me.Identification = Identifications.LearnAttackScreen

        Me.PreScreen = currentScreen
        Me.Pokemon = Pokemon
        Me.newAttack = newAttack
        Me.MachineItemID = MachineItemID

        mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
    End Sub

    Public Overrides Sub Update()
        If Me.currentCharIndex < GetText().Length Then
            Me.currentCharIndex += 1
            Exit Sub
        End If

        TextBox.Update()
        If TextBox.Showing = False Then
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
                    canForget = Not Pokemon.Attacks(AttackIndex).IsHMMove
                Else
                    canForget = True
                End If

                If Controls.Accept() = True Then
                    chosen = True
                End If
            Else
                Core.GameInstance.IsMouseVisible = True

                If Controls.Right(True, True, False) = True And canForget = True Then
                    index = 1
                End If
                If Controls.Left(True, True, False) = True Then
                    index = 0
                End If

                For i = 0 To 1
                    If New Rectangle(CInt(Core.windowSize.Width / 2) - 182 + i * 192, 550, 128 + 32, 64 + 32).Contains(MouseHandler.MousePosition) = True Then
                        index = i

                        If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                            Select Case index
                                Case 0
                                    ClickNo()
                                Case 1
                                    ClickYes()
                            End Select
                        End If
                    End If
                Next

                If Controls.Accept(False, True) = True Then
                    Select Case index
                        Case 0
                            ClickNo()
                        Case 1
                            ClickYes()
                    End Select
                End If
            End If
        End If
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))
        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
        DrawText()

        If currentCharIndex < GetText().Length Then
            Exit Sub
        End If

        Dim p As Vector2 = New Vector2(40, 50)

        If Pokemon.Attacks.Count > 0 Then
            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X + 432 - 352 + AttackPos), CInt(p.Y + 18), 288, 384))

            Dim A As BattleSystem.Attack
            If AttackIndex = 4 Then
                A = newAttack
            Else
                A = Pokemon.Attacks(AttackIndex)
            End If

            With Core.SpriteBatch
                Dim fullText As String = A.Description
                Dim t As String = ""
                Dim i As Integer = 0
                Dim n As String = ""
                For i = 0 To fullText.Length - 1
                    Dim c As Char = CChar(fullText(i).ToString().Replace("’", "'"))

                    If c = CChar(" ") Then
                        If FontManager.MiniFont.MeasureString(n & c).X > 170 Then
                            t &= vbNewLine
                            n = ""
                        Else
                            t &= " "
                            n &= " "
                        End If
                    Else
                        t &= c
                        n &= c
                    End If
                Next

                Dim power As String = A.Power.ToString()
                If power = "0" Then
                    power = "-"
                End If

                Dim acc As String = A.Accuracy.ToString()
                If acc = "0" Then
                    acc = "-"
                End If

                .DrawString(FontManager.MiniFont, "Power: " & power & vbNewLine & "Accuracy: " & acc & vbNewLine & vbNewLine & t, New Vector2(CInt(p.X + 432 - 300 + AttackPos), p.Y + 38), Color.Black)
                .Draw(A.GetDamageCategoryImage(), New Rectangle(CInt(p.X + 432 - 150 + AttackPos), CInt(p.Y + 42), 56, 28), Color.White)
            End With

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X + 80), CInt(p.Y + 18), 320, 384))
            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X + 80), CInt(p.Y + 48 + 384), 320, 96))

            For i = 0 To Me.Pokemon.Attacks.Count - 1
                DrawAttack(i, Me.Pokemon.Attacks(i))
            Next
            DrawAttack(4, newAttack)
        End If

        If chosen = True Then
            Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(Core.windowSize.Width / 2 - 352), 172, 704, 96))

            Dim drawText As String = ""
            If AttackIndex = 4 Then
                drawText = "Don't learn """ & newAttack.Name & """?"
            Else
                drawText = "Forget """ & Pokemon.Attacks(AttackIndex).Name & """ to learn """ & newAttack.Name & """?"
                If canForget = False Then
                    drawText = "Cannot forget the move " & Pokemon.Attacks(AttackIndex).Name & " because" & vbNewLine & "it's an HM move."
                End If
            End If

            Core.SpriteBatch.DrawString(FontManager.InGameFont, drawText, New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(drawText).X / 2), 200), Color.Black)

            Dim endIndex As Integer = 1
            If canForget = False Then
                endIndex = 0
            End If

            For i = 0 To endIndex
                Dim Text As String = "Learn"
                If AttackIndex = 4 Then
                    Text = "OK"
                End If

                If i = 0 Then
                    Text = "Cancel"
                End If

                If i = index Then
                    CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
                Else
                    CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
                End If

                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 182 + i * 192 + 22, 550, 128, 64))
                Core.SpriteBatch.DrawString(FontManager.InGameFont, Text, New Vector2(CInt(Core.windowSize.Width / 2) - 164 + i * 192 + 22, 402 + 180), Color.Black)
            Next
        End If

        TextBox.Draw()
    End Sub

    Private Sub DrawText()
        If currentCharIndex < (Pokemon.GetDisplayName() & " ").Length Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, (Pokemon.GetDisplayName() & " ").Remove(currentCharIndex), New Vector2(120, 20), Color.White)
        Else
            Core.SpriteBatch.DrawString(FontManager.MiniFont, Pokemon.GetDisplayName() & " ", New Vector2(120, 20), Color.White)
        End If
        If currentCharIndex > (Pokemon.GetDisplayName() & " ").Length Then
            Core.SpriteBatch.Draw(Pokemon.GetMenuTexture(), New Rectangle(CInt(FontManager.MiniFont.MeasureString(Pokemon.GetDisplayName()).X + 120), 12, 32, 32), Color.White)
        End If
        If currentCharIndex > (Pokemon.GetDisplayName() & " ").Length + 1 Then
            If currentCharIndex < GetText().Length Then
                Core.SpriteBatch.DrawString(FontManager.MiniFont, ("wants to learn """ & newAttack.Name & """. But " & Pokemon.GetDisplayName() & " can only learn 4 attacks." & vbNewLine & "Do you want " & Pokemon.GetDisplayName() & " to forget an attack to learn """ & newAttack.Name & """?").Remove(currentCharIndex - (Pokemon.GetDisplayName() & " ").Length), New Vector2(FontManager.MiniFont.MeasureString(Pokemon.GetDisplayName()).X + 152, 20), Color.White)
            Else
                Core.SpriteBatch.DrawString(FontManager.MiniFont, "wants to learn """ & newAttack.Name & """. But " & Pokemon.GetDisplayName() & " can only learn 4 attacks." & vbNewLine & "Do you want " & Pokemon.GetDisplayName() & " to forget an attack to learn """ & newAttack.Name & """?", New Vector2(FontManager.MiniFont.MeasureString(Pokemon.GetDisplayName()).X + 152, 20), Color.White)
            End If
        End If
    End Sub

    Private Function GetText() As String
        Return Pokemon.GetDisplayName() & " wants to learn """ & newAttack.Name & """. But " & Pokemon.GetDisplayName() & " can only learn 4 attacks." & vbNewLine & "Do you want " & Pokemon.GetDisplayName() & " to forget an attack to learn """ & newAttack.Name & """?"
    End Function

    Private Sub DrawAttack(ByVal i As Integer, ByVal A As BattleSystem.Attack)
        Dim p As New Vector2(140, 80 + i * (64 + 32))

        If i = 4 Then
            p.Y += 32
        End If

        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
        If Me.AttackIndex = i Then
            CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(48, 0, 48, 48), "")
        End If

        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X) + 12, CInt(p.Y), 256, 64))

        With Core.SpriteBatch
            .DrawString(FontManager.MiniFont, A.Name, New Vector2(CInt(p.X) + 30, CInt(p.Y + 26)), Color.Black)

            Dim c As Color = Color.Black
            Dim per As Integer = CInt((A.CurrentPP / A.MaxPP) * 100)

            If per <= 33 And per > 10 Then
                c = Color.Orange
            ElseIf per <= 10 Then
                c = Color.IndianRed
            End If

            .DrawString(FontManager.MiniFont, "PP " & A.CurrentPP & " / " & A.MaxPP, New Vector2(CInt(p.X) + 160, CInt(p.Y + 58)), c)

            .Draw(TextureManager.GetTexture("GUI\Menus\Types", A.Type.GetElementImage(), ""), New Rectangle(CInt(p.X) + 30, CInt(p.Y + 54), 48, 16), Color.White)
        End With
    End Sub

    Private Sub ClickYes()
        If canForget = True Then
            Dim Text As String = Pokemon.GetDisplayName() & " didn't~learn " & newAttack.Name & "!"

            If AttackIndex <> 4 Then
                TeachMovesScreen.LearnedMove = True
                Text = "1...2...3...and*Poof!*" & Pokemon.GetDisplayName() & " forgot~" & Pokemon.Attacks(AttackIndex).Name & "! And..."
                Pokemon.Attacks.RemoveAt(AttackIndex)
                Pokemon.Attacks.Insert(AttackIndex, newAttack)

                If Me.MachineItemID > -1 Then
                    PlayerStatistics.Track("TMs/HMs used", 1)
                    If Core.Player.DifficultyMode > 0 Then
                        Dim TechMachine As Item = Item.GetItemByID(Me.MachineItemID)
                        If TechMachine.ItemType = Item.ItemTypes.Machines Then
                            If CType(TechMachine, Items.TechMachine).IsTM = True Then
                                Core.Player.Inventory.RemoveItem(Me.MachineItemID, 1)
                            End If
                        End If
                    End If
                End If
                PlayerStatistics.Track("Moves learned", 1)
                TextBox.FollowUp = AddressOf FollowUpText
            End If

            TextBox.Show(Text, {}, False, False)
            Core.GameInstance.IsMouseVisible = False
            Core.SetScreen(Me.PreScreen)
        End If
    End Sub

    Private Sub ClickNo()
        Me.chosen = False
        Core.GameInstance.IsMouseVisible = False
    End Sub

    Private Sub FollowUpText()
        TextBox.Show("... " & Pokemon.GetDisplayName() & " learned~" & newAttack.Name & "!")
        SoundManager.PlaySound("success_small", False)
    End Sub

End Class