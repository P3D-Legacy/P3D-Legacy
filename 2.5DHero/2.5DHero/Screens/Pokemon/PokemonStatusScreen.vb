Public Class PokemonStatusScreen

    Inherits Screen
    Dim pageIndex As Integer = 0
    Dim PokeIndex As Integer = 0
    Dim BoxIndex As Integer = 0
    Dim BoxPokemon() As Pokemon
    Dim MainTexture As Texture2D
    Dim Pokemon As Pokemon
    Dim FrontView As Boolean = True
    Dim AttackIndex As Integer = 0
    Dim AttackToggle As Boolean = False
    Dim AttackPos As Single = 0
    Dim SwitchIndex As Integer = -1

    Dim viewParty As Boolean = True

    Dim EVColors As List(Of Color) = {New Color(0, 210, 0), New Color(253, 83, 0), New Color(0, 154, 226), New Color(253, 183, 97), New Color(100, 201, 226), New Color(178, 108, 204)}.ToList()

    Public Sub New(ByVal currentScreen As Screen, ByVal Index As Integer, ByVal BoxPokemon() As Pokemon, ByVal Pokemon As Pokemon, ByVal viewParty As Boolean)
        Me.Identification = Identifications.PokemonStatusScreen
        Me.PreScreen = currentScreen
        Me.PokeIndex = Index
        Me.Pokemon = Pokemon
        Me.BoxPokemon = BoxPokemon
        Me.BoxIndex = Index
        Me.viewParty = viewParty

        Me.MainTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        Me.pageIndex = Player.Temp.PokemonStatusPageIndex
    End Sub

    Public Overrides Sub Update()
        If AttackToggle = False Then
            If Me.AttackPos > 0.0F Then
                Me.AttackPos -= 15.0F
                If Me.AttackPos <= 0.0F Then
                    Me.AttackPos = 0.0F
                End If
            End If

            Dim dummyPokeIndex As Integer = PokeIndex

            If Controls.Down(True, False, False) Then
                Me.PokeIndex += 1
                FrontView = True
                AttackIndex = 0
            End If
            If Controls.Up(True, False, False) Then
                Me.PokeIndex -= 1
                FrontView = True
                AttackIndex = 0
            End If

            If Me.viewParty = True Then
                If Me.PokeIndex < 0 Then
                    Me.PokeIndex = 0
                ElseIf Me.PokeIndex > Core.Player.Pokemons.Count - 1 Then
                    Me.PokeIndex = Core.Player.Pokemons.Count - 1
                End If

                Me.Pokemon = Core.Player.Pokemons(PokeIndex)
            Else
                If Me.PokeIndex < 0 Then
                    Me.PokeIndex = 0
                ElseIf Me.PokeIndex > BoxPokemon.Count - 1 Then
                    Me.PokeIndex = BoxPokemon.Count - 1
                End If

                Me.Pokemon = Me.BoxPokemon(PokeIndex)
            End If

            If dummyPokeIndex <> Me.PokeIndex Then
                If Me.Pokemon.EggSteps = 0 Then
                    Me.Pokemon.PlayCry()
                End If
            End If
        Else
            If Me.AttackPos < 320.0F Then
                Me.AttackPos += 15.0F
                If Me.AttackPos >= 320.0F Then
                    Me.AttackPos = 320.0F
                End If
            End If

            If Controls.Down(True, False, True) Then
                Me.AttackIndex += 1
            End If
            If Controls.Up(True, False, True) Then
                Me.AttackIndex -= 1
            End If

            If AttackIndex < 0 Then
                AttackIndex = 0
            ElseIf AttackIndex > Pokemon.Attacks.Count - 1 Then
                AttackIndex = Pokemon.Attacks.Count - 1
            End If
        End If

        If SwitchIndex = -1 Then
            If Controls.Right(True, False, True) Then
                pageIndex += 1
                AttackToggle = False
                AttackIndex = 0
            End If
            If Controls.Left(True, False, True) Then
                pageIndex -= 1
                AttackToggle = False
                AttackIndex = 0
            End If
            If pageIndex < 0 Then
                pageIndex = 0
            ElseIf pageIndex > 2 Then
                pageIndex = 2
            End If
        End If

        Player.Temp.PokemonStatusPageIndex = Me.pageIndex
        Player.Temp.PokemonScreenIndex = Me.PokeIndex

        If pageIndex = 0 Then
            If Controls.Accept() Then
                FrontView = Not FrontView
            End If
        ElseIf pageIndex = 2 Then
            If Controls.Accept() And Me.Pokemon.EggSteps = 0 Then
                If AttackToggle = False Then
                    AttackToggle = True
                Else
                    If SwitchIndex = -1 Then
                        SwitchIndex = AttackIndex
                    Else
                        Dim A1 As BattleSystem.Attack = Me.Pokemon.Attacks(SwitchIndex)
                        Dim A2 As BattleSystem.Attack = Me.Pokemon.Attacks(AttackIndex)

                        Me.Pokemon.Attacks(AttackIndex) = A1
                        Me.Pokemon.Attacks(SwitchIndex) = A2

                        SwitchIndex = -1
                    End If
                End If
            End If
        End If

        If Controls.Dismiss() Then
            If AttackToggle = True Then
                If SwitchIndex <> -1 Then
                    SwitchIndex = -1
                Else
                    AttackToggle = False
                End If
            Else
                Core.SetScreen(Me.PreScreen)
            End If
        End If
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 2, New Rectangle(60, 100, 800, 480))
        DrawHeader()

        Dim TexturePositionPage As Vector2

        Select Case pageIndex
            Case 0
                If Me.Pokemon.EggSteps = 0 Then
                    DrawPage1()
                End If
                Core.SpriteBatch.DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_stats_page"), New Vector2(676, 138), Color.Black)
                TexturePositionPage = New Vector2(32, 96)
            Case 1
                DrawPage2()
                Core.SpriteBatch.DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_details_page"), New Vector2(676, 138), Color.Black)
                TexturePositionPage = New Vector2(32, 112)
            Case 2
                If Me.Pokemon.EggSteps = 0 Then
                    DrawPage3()
                End If
                Core.SpriteBatch.DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_moves_page"), New Vector2(676, 138), Color.Black)
                TexturePositionPage = New Vector2(80, 96)
        End Select

        Core.SpriteBatch.Draw(MainTexture, New Rectangle(574, 132, 96, 32), New Rectangle(CInt(TexturePositionPage.X), CInt(TexturePositionPage.Y), 48, 16), Color.White)

        If Me.AttackToggle = False Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_backadvice"), New Vector2(1200 - FontManager.MiniFont.MeasureString(Localization.GetString("poke_status_screen_backadvice")).X - 360, 580), Color.DarkGray)
        Else
            Core.SpriteBatch.DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_closeadvice"), New Vector2(1200 - FontManager.MiniFont.MeasureString(Localization.GetString("poke_status_screen_closeadvice")).X - 360, 580), Color.DarkGray)
        End If
    End Sub

    Private Sub DrawHeader()
        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        'EVColors:
        With Me.Pokemon
            Dim AllEVs As Integer = .EVHP + .EVAttack + .EVDefense + .EVSpAttack + .EVSpDefense + .EVSpeed

            If AllEVs > 0 Then
                Dim EVMax As Integer = 510
                If AllEVs > EVMax Then
                    EVMax = AllEVs
                End If

                Dim BlockWidth As Double = 490 / EVMax
                Dim CurrentWidth As Integer = 70
                Dim BlockY As Integer = 192

                Canvas.DrawRectangle(New Rectangle(CurrentWidth, BlockY, CInt(BlockWidth * .EVHP), 10), EVColors(0))
                CurrentWidth += CInt(BlockWidth * .EVHP)

                Canvas.DrawRectangle(New Rectangle(CurrentWidth, BlockY, CInt(BlockWidth * .EVAttack), 10), EVColors(1))
                CurrentWidth += CInt(BlockWidth * .EVAttack)

                Canvas.DrawRectangle(New Rectangle(CurrentWidth, BlockY, CInt(BlockWidth * .EVDefense), 10), EVColors(2))
                CurrentWidth += CInt(BlockWidth * .EVDefense)

                Canvas.DrawRectangle(New Rectangle(CurrentWidth, BlockY, CInt(BlockWidth * .EVSpAttack), 10), EVColors(3))
                CurrentWidth += CInt(BlockWidth * .EVSpAttack)

                Canvas.DrawRectangle(New Rectangle(CurrentWidth, BlockY, CInt(BlockWidth * .EVSpDefense), 10), EVColors(4))
                CurrentWidth += CInt(BlockWidth * .EVSpDefense)

                Canvas.DrawRectangle(New Rectangle(CurrentWidth, BlockY, CInt(BlockWidth * .EVSpeed), 10), EVColors(5))
                CurrentWidth += CInt(BlockWidth * .EVSpeed)
            End If
        End With

        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(60, 100, 480, 64))
        If Pokemon.GetDisplayName() = Pokemon.GetName() Or Pokemon.IsEgg() = True Then
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Pokemon.GetDisplayName(), New Vector2(158, 132), Color.Black)
        Else
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Pokemon.GetDisplayName(), New Vector2(158, 122), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, Pokemon.GetName(), New Vector2(164, 152), Color.Black)
        End If
        Core.SpriteBatch.Draw(Pokemon.GetMenuTexture(), New Rectangle(70, 110, 80, 80), BattleStats.GetStatColor(Pokemon.Status))
        If Not Pokemon.Item Is Nothing And Pokemon.EggSteps = 0 Then
            Core.SpriteBatch.Draw(Pokemon.Item.Texture, New Rectangle(118, 150, 28, 28), Color.White)
        End If

        'Portray
        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(60, 196, 128, 128))
        Core.SpriteBatch.Draw(Pokemon.GetTexture(FrontView), New Rectangle(74, 208, 124, 124), Color.White)
        Core.SpriteBatch.Draw(Pokemon.CatchBall.Texture, New Rectangle(74, 318, 24, 24), Color.White)
        If Me.Pokemon.IsShiny = True And Me.Pokemon.IsEgg() = False Then
            Core.SpriteBatch.Draw(MainTexture, New Rectangle(78, 218, 18, 18), New Rectangle(118, 4, 9, 9), Color.White)
        End If

        'Other:
        If Me.Pokemon.EggSteps = 0 Then
            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(60, 356, 128, 224))
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Types"), New Rectangle(76, 380, 48, 16), Me.Pokemon.Type1.GetElementImage(), Color.White)
            If Me.Pokemon.Type2.Type <> Element.Types.Blank Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\Types"), New Rectangle(124, 380, 48, 16), Me.Pokemon.Type2.GetElementImage(), Color.White)
            End If

            Dim r As New Rectangle(96, 0, 6, 10)
            If Me.Pokemon.Gender = net.Pokemon3D.Game.Pokemon.Genders.Female Then
                r = New Rectangle(102, 0, 6, 10)
            End If
            If Me.Pokemon.Gender <> net.Pokemon3D.Game.Pokemon.Genders.Genderless Then
                Core.SpriteBatch.Draw(MainTexture, New Rectangle(180, 376, 12, 20), r, Color.White)
            End If

            Core.SpriteBatch.DrawString(FontManager.MiniFont, Localization.GetString("Level") & ": " & Me.Pokemon.Level & vbNewLine & Localization.GetString("poke_status_screen_number") & Pokemon.Number & vbNewLine & vbNewLine & Localization.GetString("poke_status_screen_nature") & ":" & vbNewLine & Me.Pokemon.Nature.ToString(), New Vector2(76, 410), Color.Black)

            Dim StatusTexture As Texture2D = BattleStats.GetStatImage(Pokemon.Status)
            If Not StatusTexture Is Nothing Then
                Dim Y As Integer = 139
                If Pokemon.GetDisplayName() <> Pokemon.GetName() Then
                    Y = 127
                End If
                Canvas.DrawRectangle(New Rectangle(CInt(170 + FontManager.InGameFont.MeasureString(Pokemon.GetDisplayName()).X), Y, 61, 22), Color.Gray)
                Core.SpriteBatch.Draw(StatusTexture, New Rectangle(CInt(172 + FontManager.InGameFont.MeasureString(Pokemon.GetDisplayName()).X), Y + 2, 57, 18), Color.White)
            End If
        End If
    End Sub

    Private Sub DrawPage1()
        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        Dim p As Vector2 = New Vector2(140, 180)

        With Core.SpriteBatch
            'Stats:
            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(220, 196, 320, 256))
            Dim barX As Integer = CInt((Pokemon.HP / Pokemon.MaxHP) * 150)
            Dim barRectangle As Rectangle
            Dim barPercentage As Integer = CInt((Pokemon.HP / Pokemon.MaxHP) * 100).Clamp(0, 100)

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

            For x = barX To 149
                .Draw(MainTexture, New Rectangle(CInt(p.X + (x * 2) + 104), CInt(p.Y + 44), 4, 16), New Rectangle(114, 0, 1, 4), Color.White)
            Next
            .Draw(MainTexture, New Rectangle(CInt(p.X + 100), CInt(p.Y + 44), 4, 16), New Rectangle(112, 0, 1, 4), Color.White)
            .Draw(MainTexture, New Rectangle(CInt(p.X + 406), CInt(p.Y + 44), 4, 16), New Rectangle(112, 0, 1, 4), Color.White)

            Dim redText As String = vbNewLine & vbNewLine
            Dim blueText As String = vbNewLine & vbNewLine
            Dim blackText As String = Localization.GetString("HP") & vbNewLine & vbNewLine
            For i = 0 To 4
                Dim statText As String = ""
                Dim stat As String = ""
                Select Case i
                    Case 0
                        statText = Localization.GetString("Attack")
                        stat = "Attack"
                    Case 1
                        statText = Localization.GetString("Defense")
                        stat = "Defense"
                    Case 2
                        statText = Localization.GetString("Special_Attack")
                        stat = "SpAttack"
                    Case 3
                        statText = Localization.GetString("Special_Defense")
                        stat = "SpDefense"
                    Case 4
                        statText = Localization.GetString("Speed")
                        stat = "Speed"
                End Select

                Dim m As Single = Nature.GetMultiplier(Pokemon.Nature, stat)
                If m > 1.0F Then
                    redText &= statText & vbNewLine
                    blueText &= vbNewLine & vbNewLine
                    blackText &= vbNewLine & vbNewLine
                ElseIf m < 1.0F Then
                    redText &= vbNewLine & vbNewLine
                    blueText &= statText & vbNewLine
                    blackText &= vbNewLine & vbNewLine
                Else
                    redText &= vbNewLine & vbNewLine
                    blueText &= vbNewLine & vbNewLine
                    blackText &= statText & vbNewLine & vbNewLine
                End If
            Next

            .DrawString(FontManager.MiniFont, blackText, New Vector2(CInt(p.X + 100), CInt(p.Y + 68)), Color.Black)
            .DrawString(FontManager.MiniFont, redText, New Vector2(CInt(p.X + 100), CInt(p.Y + 68)), New Color(255, 0, 0, 200))
            .DrawString(FontManager.MiniFont, blueText, New Vector2(CInt(p.X + 100), CInt(p.Y + 68)), Color.Blue)
            .DrawString(FontManager.MiniFont, Pokemon.HP & " / " & Pokemon.MaxHP & vbNewLine & vbNewLine & Pokemon.Attack & vbNewLine & vbNewLine & Pokemon.Defense & vbNewLine & vbNewLine & Pokemon.SpAttack & vbNewLine & vbNewLine & Pokemon.SpDefense & vbNewLine & vbNewLine & Pokemon.Speed, New Vector2(CInt(p.X + 280), CInt(p.Y + 68)), Color.Black)

            'Experience:
            If Pokemon.Level < CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) Then
                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(220, 484, 320, 96))

                Dim NextLvExp As Integer = Pokemon.NeedExperience(Me.Pokemon.Level + 1) - Pokemon.NeedExperience(Me.Pokemon.Level)
                Dim currentExp As Integer = Me.Pokemon.Experience - Pokemon.NeedExperience(Me.Pokemon.Level)
                If Pokemon.Level = 1 Then
                    NextLvExp = Pokemon.NeedExperience(Me.Pokemon.Level + 1)
                    currentExp = Me.Pokemon.Experience
                End If

                Dim NeedExp As Integer = NextLvExp - currentExp

                If Pokemon.Level = CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")) Then
                    NextLvExp = 0
                Else
                    barPercentage = CInt((currentExp / NextLvExp) * 100)
                    barX = CInt((currentExp / NextLvExp) * 150).Clamp(0, 150)
                End If

                .DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_all_exp") & ": " & Pokemon.Experience & vbNewLine & Localization.GetString("poke_status_screen_nxt_lv") & ": " & NextLvExp - currentExp, New Vector2(240, 504), Color.Black)

                Dim i As Integer = 0
                For x = 0 To barX - 1
                    .Draw(MainTexture, New Rectangle((x * 2) + 240, 550, 4, 16), New Rectangle(118 + i, 0, 1, 4), Color.White)
                    i += 1
                    If i = 2 Then
                        i = 0
                    End If
                Next

                For x = barX To 149
                    .Draw(MainTexture, New Rectangle((x * 2) + 240, 550, 4, 16), New Rectangle(114, 0, 1, 4), Color.White)
                Next
                .Draw(MainTexture, New Rectangle(236, 550, 4, 16), New Rectangle(112, 0, 1, 4), Color.White)
                .Draw(MainTexture, New Rectangle(542, 550, 4, 16), New Rectangle(112, 0, 1, 4), Color.White)

                If barPercentage = 100 Then
                    barPercentage -= 1
                End If

                .DrawString(FontManager.MiniFont, barPercentage & " %", New Vector2(250, 575), Color.DarkBlue)
            End If
        End With

    End Sub

    Private Sub DrawPage2()
        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
        Dim p As Vector2 = New Vector2(140, 180)

        'Catchinfos
        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(220, 196, 320, 96))
        With Core.SpriteBatch
            .DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_OT") & ": " & Pokemon.OT & " /" & Pokemon.CatchTrainerName & vbNewLine & vbNewLine & Pokemon.CatchMethod & vbNewLine & Pokemon.CatchLocation, New Vector2(238, 214), Color.DarkBlue)
        End With

        'Item:
        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(220, 324, 320, 128))
        If Me.Pokemon.EggSteps = 0 Then
            If Not Pokemon.Item Is Nothing Then
                Core.SpriteBatch.Draw(Pokemon.Item.Texture, New Rectangle(232, 338, 24, 24), Color.White)
                Core.SpriteBatch.DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_Item") & ": " & Pokemon.Item.Name, New Vector2(262, 342), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.MiniFont, Pokemon.Item.Description.CropStringToWidth(FontManager.MiniFont, 300), New Vector2(234, 360), Color.Black)
            Else
                Core.SpriteBatch.DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_Item") & ": " & Localization.GetString("poke_status_screen_no_item"), New Vector2(262, 342), Color.Black)
            End If
        End If

        'Ability:
        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(220, 484, 320, 96))
        If Me.Pokemon.EggSteps = 0 Then
            With Core.SpriteBatch
                .DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_ability") & ": " & Me.Pokemon.Ability.Name & vbNewLine & vbNewLine & Me.Pokemon.Ability.Description.CropStringToWidth(FontManager.MiniFont, 300), New Vector2(234, 500), Color.Black)
            End With
        Else
            With Core.SpriteBatch
                Dim s As String = """The Egg Watch""" & vbNewLine
                Dim percent As Integer = CInt((Me.Pokemon.EggSteps / Me.Pokemon.BaseEggSteps) * 100)
                If percent <= 33 Then
                    s &= "It looks like this Egg will" & vbNewLine & "take a long time to hatch."
                ElseIf percent > 33 And percent <= 66 Then
                    s &= "It's getting warmer and moves" & vbNewLine & "a little. It will hatch soon."
                Else
                    s &= "There is strong movement" & vbNewLine & "noticeable. It will hatch soon!"
                End If

                .DrawString(FontManager.MiniFont, s, New Vector2(234, 500), Color.Black)
            End With
        End If
    End Sub

    Private Sub DrawPage3()
        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
        Dim p As Vector2 = New Vector2(140, 180)

        If Pokemon.Attacks.Count > 0 Then
            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(572 - 352 + AttackPos), 196, 288, 384))

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

                .DrawString(FontManager.MiniFont, Localization.GetString("poke_status_screen_power") & ": " & power & vbNewLine & Localization.GetString("poke_status_screen_accuracy") & ": " & acc & vbNewLine & vbNewLine & t, New Vector2(CInt(572 - 300 + AttackPos), 218), Color.Black)
                .Draw(A.GetDamageCategoryImage(), New Rectangle(CInt(572 - 150 + AttackPos), 222, 56, 28), Color.White)
            End With

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(220, 196, 320, 384))

            For i = 0 To Me.Pokemon.Attacks.Count - 1
                DrawAttack(i, Me.Pokemon.Attacks(i))
            Next
        End If
    End Sub

    Private Sub DrawAttack(ByVal i As Integer, ByVal A As BattleSystem.Attack)
        Dim p As New Vector2(240, 210 + i * (64 + 32))

        Dim CanvasTexture As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
        If Me.AttackToggle = True And Me.AttackIndex = i Then
            CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(48, 0, 48, 48), "")
        Else
            If SwitchIndex <> -1 And i = SwitchIndex Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            End If
        End If

        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(252, CInt(p.Y), 256, 64))

        With Core.SpriteBatch
            .DrawString(FontManager.MiniFont, A.Name, New Vector2(270, CInt(p.Y + 26)), Color.Black)

            Dim c As Color = Color.Black
            Dim per As Integer = CInt((A.CurrentPP / A.MaxPP) * 100)

            If per <= 33 And per > 10 Then
                c = Color.Orange
            ElseIf per <= 10 Then
                c = Color.IndianRed
            End If

            .DrawString(FontManager.MiniFont, Localization.GetString("PP") & " " & A.CurrentPP & " / " & A.MaxPP, New Vector2(400, CInt(p.Y + 58)), c)

            .Draw(TextureManager.GetTexture("GUI\Menus\Types", A.Type.GetElementImage(), ""), New Rectangle(270, CInt(p.Y + 54), 48, 16), Color.White)
        End With
    End Sub

    Public Overrides Sub ChangeTo()
        If Me.Pokemon.EggSteps = 0 Then
            Me.Pokemon.PlayCry()
        End If
    End Sub

End Class