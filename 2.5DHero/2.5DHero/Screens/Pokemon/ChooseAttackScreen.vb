Public Class ChooseAttackScreen

    Inherits Screen

    Dim Pokemon As Pokemon
    Dim mainTexture As Texture2D

    Dim index As Integer = 0

    Dim AttackIndex As Integer = 0
    Dim AttackPos As Single = 320.0F

    Dim canChooseHMMove As Boolean = True
    Dim canExit As Boolean = True

    Public Shared Selected As Integer = -1
    Public Shared Exited As Boolean = False
    Public Shared Chosen As Boolean = False

    Public Delegate Sub ChoseAttack(ByVal Pokemon As Pokemon, ByVal AttackIndex As Integer)

    Dim DoSub As ChoseAttack

    Public Sub New(ByVal currentScreen As Screen, ByVal Pokemon As Pokemon, ByVal canChooseHmMove As Boolean, ByVal canExit As Boolean, ByVal DoSub As ChoseAttack)
        Me.Identification = Identifications.ChooseAttackScreen

        Me.MouseVisible = False
        Me.CanBePaused = True
        Me.CanMuteMusic = True
        Me.CanChat = True
        Me.CanTakeScreenshot = True
        Me.CanDrawDebug = True

        Me.PreScreen = currentScreen
        Me.Pokemon = Pokemon

        mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        Me.canChooseHMMove = canChooseHmMove
        Me.canExit = canExit

        Me.DoSub = DoSub
    End Sub

    Public Overrides Sub Update()
        If TextBox.Showing = False Then
            If Controls.Up(True, True, True, True) = True Then
                Me.AttackIndex -= 1
            End If
            If Controls.Down(True, True, True, True) = True Then
                Me.AttackIndex += 1
            End If

            Me.AttackIndex = CInt(MathHelper.Clamp(Me.AttackIndex, 0, Pokemon.Attacks.Count - 1))

            If Controls.Accept() = True Then
                ClickYes()
            End If
            If Controls.Dismiss() = True Then
                If canExit = True Then
                    ClickNo()
                End If
            End If
        End If
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))
        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        Dim p As Vector2 = New Vector2(40, 50)

        If Pokemon.Attacks.Count > 0 Then
            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X + 432 - 352 + AttackPos), CInt(p.Y + 18), 288, 384))

            Dim A As BattleSystem.Attack = Pokemon.Attacks(AttackIndex)

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
        End If
    End Sub

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
        Dim valid As Boolean = True
        If canChooseHMMove = False Then
            Dim A As BattleSystem.Attack = Pokemon.Attacks(AttackIndex)
            If A.IsHMMove = True Then
                valid = False
                TextBox.Show("Cannot choose HM move.", {}, False, False)
            End If
        End If
        If valid = True Then
            Selected = AttackIndex
            Core.SetScreen(Me.PreScreen)
            Exited = True
            Chosen = True
            If Not DoSub Is Nothing Then
                DoSub(Pokemon, AttackIndex)
            End If
        End If
    End Sub

    Private Sub ClickNo()
        Selected = -1
        Core.SetScreen(Me.PreScreen)
        Exited = True
        Chosen = False
    End Sub

End Class