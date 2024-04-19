Public Class ApricornScreen

    Inherits Screen

    Private Enum States
        CanGive
        CanTake
        Wait
        None
    End Enum

    Dim owner As String
    Dim State As States = States.Wait

    Dim Apricorns As New List(Of String)

    Dim Buttons As New List(Of ButtonIcon)
    Dim Labels As New List(Of Label)

    Dim lastUpdateState As States = States.None

    Dim buttonTexture As Texture2D
    Dim mainTexture As Texture2D
    Dim _closing As Boolean = False
    Dim _enrollY As Single = 0F
    Public Shared _interfaceFade As Single = 0F

    Dim CursorIndex As Integer = 0

    Public Sub New(ByVal currentScreen As Screen, ByVal owner As String)
        Me.Identification = Identifications.ApricornScreen

        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\General")

        Me.PreScreen = currentScreen
        Me.buttonTexture = TextureManager.GetTexture("GUI\Menus\Menu")
        Me.owner = owner
        Me.MouseVisible = True

        Me.GetSavedApricorns()

        If CheckApricornStatus() >= 1440 Then
            If Me.HasApricorns() = True Then
                Me.State = States.CanTake
            Else
                Me.State = States.CanGive
            End If
        Else
            Me.State = States.Wait
        End If

        InitializeScreen()
    End Sub

    Private Sub InitializeScreen()
        If Me.lastUpdateState <> Me.State Then
            Me.lastUpdateState = Me.State

            Me.Buttons.Clear()
            Me.Labels.Clear()

            Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
            Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

            Dim Delta_X As Integer = halfWidth - 400
            Dim Delta_Y As Integer = halfHeight - 200

            Select Case Me.State
                Case States.Wait
                    Me.Labels.Add(New Label(Localization.GetString("apricorn_screen_producing").Replace("~", Environment.NewLine), New Vector2(Delta_X + 56, Delta_Y + 48), FontManager.MainFont))
                    Me.Labels.Add(New Label(Localization.GetString("apricorn_screen_backadvice").Replace("~", Environment.NewLine), New Vector2(Delta_X + 56, Delta_Y + 16 + 96 * 2), FontManager.MainFont))
                Case States.CanGive
                    Dim T As Texture2D = TextureManager.GetTexture("Items\ItemSheet")

                    Me.Labels.Add(New Label(Localization.GetString("apricorn_screen_choose_apricorns").Replace("~", Environment.NewLine), New Vector2(Delta_X + 48, Delta_Y + 48), FontManager.MainFont))
                    Dim RedApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(85.ToString), FontManager.MainFont, T, New Rectangle(240, 72, 24, 24), New Vector2(Delta_X + 128, Delta_Y + 16 + 104), New Size(48, 48), "85")
                    Dim BlueApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(89.ToString), FontManager.MainFont, T, New Rectangle(336, 72, 24, 24), New Vector2(Delta_X + 128 * 2, Delta_Y + 16 + 104), New Size(48, 48), "89")
                    Dim YellowApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(92.ToString), FontManager.MainFont, T, New Rectangle(384, 72, 24, 24), New Vector2(Delta_X + 128 * 3, Delta_Y + 16 + 104), New Size(48, 48), "92")
                    Dim GreenApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(93.ToString), FontManager.MainFont, T, New Rectangle(408, 72, 24, 24), New Vector2(Delta_X + 128 * 4, Delta_Y + 16 + 104), New Size(48, 48), "93")
                    Dim WhiteApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(97.ToString), FontManager.MainFont, T, New Rectangle(0, 96, 24, 24), New Vector2(Delta_X + 128 * 5, Delta_Y + 16 + 104), New Size(48, 48), "97")
                    Dim BlackApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(99.ToString), FontManager.MainFont, T, New Rectangle(48, 96, 24, 24), New Vector2(Delta_X + 128, Delta_Y + 16 + 104 * 2), New Size(48, 48), "99")
                    Dim PinkApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(101.ToString), FontManager.MainFont, T, New Rectangle(72, 96, 24, 24), New Vector2(Delta_X + 128 * 2, Delta_Y + 16 + 104 * 2), New Size(48, 48), "101")

                    Dim AddAllButton As ButtonIcon = New ButtonIcon(AddressOf Me.AddAllApricorns, Localization.GetString("apricorn_screen_all"), FontManager.MainFont, buttonTexture, New Rectangle(64, 160, 16, 16), New Vector2(Delta_X + 128 * 3, Delta_Y + 16 + 104 * 2), New Size(48, 48), "Clear")

                    Dim ClearButton As ButtonIcon = New ButtonIcon(AddressOf Me.ClearApricorns, Localization.GetString("apricorn_screen_clear"), FontManager.MainFont, buttonTexture, New Rectangle(64, 128, 16, 16), New Vector2(Delta_X + 128 * 4, Delta_Y + 16 + 104 * 2), New Size(48, 48), "Clear")

                    Dim GiveButton As ButtonIcon = New ButtonIcon(AddressOf Me.Give, Localization.GetString("apricorn_screen_ok"), FontManager.MainFont, buttonTexture, New Rectangle(48, 128, 16, 16), New Vector2(Delta_X + 128 * 5, Delta_Y + 16 + 104 * 2), New Size(48, 48), "OK")
                    GiveButton.Enabled = False

                    Buttons.AddRange({RedApricorn, BlueApricorn, YellowApricorn, GreenApricorn, WhiteApricorn, BlackApricorn, PinkApricorn, AddAllButton, ClearButton, GiveButton})
                Case States.CanTake
                    Me.Labels.Add(New Label(Localization.GetString("apricorn_screen_ready"), New Vector2(Delta_X + 48, Delta_Y + 48), FontManager.MainFont))

                    Dim TakeButton As ButtonIcon = New ButtonIcon(AddressOf Me.Take, Localization.GetString("apricorn_screen_take"), FontManager.MainFont, buttonTexture, New Rectangle(48, 128, 16, 16), New Vector2(Delta_X + 128 * 5, Delta_Y + 16 + 104 * 2), New Size(48, 48))
                    Buttons.AddRange({TakeButton})

            End Select
        End If
    End Sub

    Public Overrides Sub Update()
        TextBox.Update()
        InitializeScreen()

        If _closing Then
            ' When the interface is closing, only update the closing animation
            ' Once the interface is completely closed, set to the previous screen.

            If _interfaceFade > 0F Then
                _interfaceFade = MathHelper.Lerp(0, _interfaceFade, 0.8F)
                If _interfaceFade < 0F Then
                    _interfaceFade = 0F
                End If
            End If
            If _enrollY > 0 Then
                _enrollY = MathHelper.Lerp(0, _enrollY, 0.8F)
                If _enrollY <= 0 Then
                    _enrollY = 0
                End If
            End If
            If _enrollY <= 2.0F Then
                SetScreen(Me.PreScreen)
            End If
        Else
            'Update intro animation:
            Dim maxWindowHeight As Integer = 400
            If _enrollY < maxWindowHeight Then
                _enrollY = MathHelper.Lerp(maxWindowHeight, _enrollY, 0.8F)
                If _enrollY >= maxWindowHeight Then
                    _enrollY = maxWindowHeight
                End If
            End If
            If _interfaceFade < 1.0F Then
                _interfaceFade = MathHelper.Lerp(1.0F, _interfaceFade, 0.95F)
                If _interfaceFade > 1.0F Then
                    _interfaceFade = 1.0F
                End If
            End If
            If TextBox.Showing = False Then
                Select Case Me.State
                    Case States.Wait
                        UpdateWait()
                    Case States.CanGive
                        UpdateGive()
                    Case States.CanTake
                        UpdateTake()
                End Select

                If Controls.Dismiss() = True Then
                    _closing = True
                End If

                If Me.State <> States.CanGive Then
                    If Controls.Left(True, True) = True Or Controls.Up(True, True) Then
                        Me.CursorIndex -= 1
                        If Me.CursorIndex < 0 Then
                            Me.CursorIndex = Me.Buttons.Count - 1
                        End If
                    End If

                    If Controls.Down(True, True) = True Or Controls.Right(True, True) = True Then
                        Me.CursorIndex += 1
                        If Me.CursorIndex > Me.Buttons.Count - 1 Then
                            Me.CursorIndex = 0
                        End If
                    End If
                End If

                If Me.Buttons.Count > 0 Then
                    If Controls.Accept(False, True, True) = True Then
                        SoundManager.PlaySound("select")
                        Me.Buttons(Me.CursorIndex).Click()
                    End If
                End If

                For Each B As ButtonIcon In Buttons
                    B.Update()
                Next

            End If
        End If
    End Sub

    Private Sub UpdateWait()

    End Sub

    Private Sub UpdateGive()
        If Controls.Up(True, True, False) = True Or Controls.Down(True, True, False) = True Then
            If Me.CursorIndex < 5 Then
                Me.CursorIndex += 5
            Else
                Me.CursorIndex -= 5
            End If
        End If

        If Controls.Left(True, True, False) = True Then
            If Me.CursorIndex <= 0 OrElse Me.CursorIndex = 5 Then
                Me.CursorIndex += 4
            Else
                Me.CursorIndex -= 1
            End If
        End If

        If Controls.Right(True, True, False) = True Then
            If Me.CursorIndex >= Me.Buttons.Count - 1 OrElse Me.CursorIndex = 4 Then
                Me.CursorIndex -= 4
            Else
                Me.CursorIndex += 1
            End If
        End If

        If Controls.Left(True, False, True, False, False, False) = True Then
            Me.CursorIndex -= 1
            If Me.CursorIndex < 0 Then
                Me.CursorIndex = Me.Buttons.Count - 1
            End If
        End If

        If Controls.Right(True, False, True, False, False, False) = True Then
            Me.CursorIndex += 1
            If Me.CursorIndex > Me.Buttons.Count - 1 Then
                Me.CursorIndex = 0
            End If
        End If
    End Sub

    Private Sub UpdateTake()

    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        DrawBackground()

        For i = 0 To Me.Buttons.Count - 1
            Dim b As ButtonIcon = Me.Buttons(i)

            If i = Me.CursorIndex Then
                Canvas.DrawRectangle(New Rectangle(CInt(b.Position.X - 4), CInt(b.Position.Y - 4), 56, 56), New Color(Color.White, CInt(255 * 0.5 * _interfaceFade)))
                Canvas.DrawBorder(2, New Rectangle(CInt(b.Position.X - 4), CInt(b.Position.Y - 4), 56, 56), New Color(Color.White, CInt(255 * _interfaceFade)))
            End If
        Next

        For Each Button As ButtonIcon In Buttons
            Button.Draw()
        Next

        For Each Label As Label In Labels
            Label.Draw()
        Next

        Select Case Me.State
            Case States.Wait
                DrawWait()
            Case States.CanGive
                DrawGive()
            Case States.CanTake
                DrawTake()
        End Select

        TextBox.Draw()
    End Sub
    Private Sub DrawBackground()
        Dim mainBackgroundColor As Color = Color.White
        If _closing Then
            mainBackgroundColor = New Color(255, 255, 255, CInt(255 * _interfaceFade))
        End If

        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        Canvas.DrawRectangle(New Rectangle(halfWidth - 400, halfHeight - 232, 260, 32), New Color(Screens.UI.ColorProvider.MainColor(False).R, Screens.UI.ColorProvider.MainColor(False).G, Screens.UI.ColorProvider.MainColor(False).B, mainBackgroundColor.A))
        Canvas.DrawRectangle(New Rectangle(halfWidth - 140, halfHeight - 216, 16, 16), New Color(Screens.UI.ColorProvider.MainColor(False).R, Screens.UI.ColorProvider.MainColor(False).G, Screens.UI.ColorProvider.MainColor(False).B, mainBackgroundColor.A))
        SpriteBatch.Draw(mainTexture, New Rectangle(halfWidth - 140, halfHeight - 232, 16, 16), New Rectangle(80, 0, 16, 16), mainBackgroundColor)
        SpriteBatch.Draw(mainTexture, New Rectangle(halfWidth - 124, halfHeight - 216, 16, 16), New Rectangle(80, 0, 16, 16), mainBackgroundColor)

        SpriteBatch.DrawString(FontManager.ChatFont, Localization.GetString("apricorn_screen_apricorns", "Apricorns"), New Vector2(halfWidth - 390, halfHeight - 228), mainBackgroundColor)

        For y = 0 To CInt(_enrollY) Step 16
            For x = 0 To 800 Step 16
                SpriteBatch.Draw(mainTexture, New Rectangle(halfWidth - 400 + x, halfHeight - 200 + y, 16, 16), New Rectangle(64, 0, 4, 4), mainBackgroundColor)
            Next
        Next

        Dim modRes As Integer = CInt(_enrollY) Mod 16
        If modRes > 0 Then
            For x = 0 To 800 Step 16
                SpriteBatch.Draw(mainTexture, New Rectangle(halfWidth - 400 + x, CInt(_enrollY + (halfHeight - 200)), 16, modRes), New Rectangle(64, 0, 4, 4), mainBackgroundColor)
            Next
        End If
    End Sub
    Private Sub DrawWait()

    End Sub

    Private Sub DrawTake()
        Dim halfWidth As Integer = CInt(Core.windowSize.Width / 2)
        Dim halfHeight As Integer = CInt(Core.windowSize.Height / 2)

        Dim Delta_X As Integer = halfWidth - 400
        Dim Delta_Y As Integer = halfHeight - 200

        For i = 0 To 6
            Dim x As Integer = i
            Dim y As Integer = 0
            While x > 4
                x -= 5
                y += 1
            End While

            Dim ItemID As Integer = 0
            Select Case i
                Case 0
                    ItemID = 159
                Case 1
                    ItemID = 160
                Case 2
                    ItemID = 165
                Case 3
                    ItemID = 164
                Case 4
                    ItemID = 161
                Case 5
                    ItemID = 157
                Case 6
                    ItemID = 166
            End Select

            Dim Item As Item = Item.GetItemByID(ItemID.ToString)
            Dim TextSize As Vector2 = FontManager.MainFont.MeasureString("x" & Apricorns(i))

            Core.SpriteBatch.Draw(Item.Texture, New Rectangle(Delta_X + 128 + x * 128, Delta_Y + 16 + 104 + y * 104, 48, 48), New Color(Color.White, _interfaceFade))
            Core.SpriteBatch.DrawRectangle(New Rectangle(CInt(Delta_X + 128 + x * 128 + 48 / 2 - TextSize.X / 2 - 8), CInt(Delta_Y + 16 + 104 + y * 104 + 48 + 8), CInt(TextSize.X + 16), CInt(TextSize.Y + 16)), New Color(Color.Black, CInt(255 * 0.4F * _interfaceFade)))
            Core.SpriteBatch.DrawString(FontManager.MainFont, "x" & Apricorns(i), New Vector2(CInt(48 / 2) - CInt(TextSize.X / 2) + Delta_X + 128 + x * 128, 48 + Delta_Y + 16 + 104 + y * 104 + CInt(TextSize.Y / 2)), New Color(Color.White, _interfaceFade))
        Next
    End Sub

    Private Sub DrawGive()

    End Sub

    Private Sub GiveApricorn(ByVal b As ButtonIcon)
        Dim apricornID As Integer = 0

        Select Case CInt(b.AdditionalValue)
            Case 85
                apricornID = 0
            Case 89
                apricornID = 1
            Case 92
                apricornID = 2
            Case 93
                apricornID = 3
            Case 97
                apricornID = 4
            Case 99
                apricornID = 5
            Case 101
                apricornID = 6
        End Select

        If Core.Player.Inventory.GetItemAmount(b.AdditionalValue) > CInt(Me.Apricorns(apricornID)) Then
            Me.Apricorns(apricornID) = CStr(CInt(Me.Apricorns(apricornID)) + 1)

            If HasApricorns() = True Then
                For Each Button As ButtonIcon In Me.Buttons
                    If Button.AdditionalValue = Localization.GetString("apricorn_screen_ok") Then
                        Button.Enabled = True
                    End If
                Next
            End If
        End If

        AdjustButtonTexts()
    End Sub

    Private Sub AdjustButtonTexts()
        For Each b As ButtonIcon In Buttons
            If StringHelper.IsNumeric(b.AdditionalValue) Then
                Dim apricornID As Integer = 0

                Select Case CInt(b.AdditionalValue)
                    Case 85
                        apricornID = 0
                    Case 89
                        apricornID = 1
                    Case 92
                        apricornID = 2
                    Case 93
                        apricornID = 3
                    Case 97
                        apricornID = 4
                    Case 99
                        apricornID = 5
                    Case 101
                        apricornID = 6
                End Select

                b.Text = Me.Apricorns(apricornID) & " / " & Core.Player.Inventory.GetItemAmount(b.AdditionalValue)
            End If
        Next
    End Sub

    Private Sub ClearApricorns()
        For Each Button As ButtonIcon In Me.Buttons
            If Button.AdditionalValue = Localization.GetString("apricorn_screen_ok") Then
                Button.Enabled = False
            End If
        Next

        Me.Apricorns = {"0", "0", "0", "0", "0", "0", "0"}.ToList()

        AdjustButtonTexts()
    End Sub

    Private Sub AddAllApricorns()
        Me.Apricorns = {Core.Player.Inventory.GetItemAmount(85.ToString).ToString, Core.Player.Inventory.GetItemAmount(89.ToString).ToString, Core.Player.Inventory.GetItemAmount(92.ToString).ToString, Core.Player.Inventory.GetItemAmount(93.ToString).ToString, Core.Player.Inventory.GetItemAmount(97.ToString).ToString, Core.Player.Inventory.GetItemAmount(99.ToString).ToString, Core.Player.Inventory.GetItemAmount(101.ToString).ToString}.ToList()

        If HasApricorns() = True Then
            For Each Button As ButtonIcon In Me.Buttons
                If Button.AdditionalValue = Localization.GetString("apricorn_screen_ok") Then
                    Button.Enabled = True
                End If
            Next
        End If

        AdjustButtonTexts()
    End Sub

    Private Sub Give()
        Me.State = States.Wait

        Core.Player.Inventory.RemoveItem(85.ToString, CInt(Me.Apricorns(0)))
        Core.Player.Inventory.RemoveItem(89.ToString, CInt(Me.Apricorns(1)))
        Core.Player.Inventory.RemoveItem(92.ToString, CInt(Me.Apricorns(2)))
        Core.Player.Inventory.RemoveItem(93.ToString, CInt(Me.Apricorns(3)))
        Core.Player.Inventory.RemoveItem(97.ToString, CInt(Me.Apricorns(4)))
        Core.Player.Inventory.RemoveItem(99.ToString, CInt(Me.Apricorns(5)))
        Core.Player.Inventory.RemoveItem(101.ToString, CInt(Me.Apricorns(6)))

        Dim d As Date = Date.Now

        Dim s As String = "{" & Me.owner & "|" &
            Me.Apricorns(0) & "," &
            Me.Apricorns(1) & "," &
            Me.Apricorns(2) & "," &
            Me.Apricorns(3) & "," &
            Me.Apricorns(4) & "," &
            Me.Apricorns(5) & "," &
            Me.Apricorns(6) & "|" &
            d.Year & "," & d.Month & "," & d.Day & "," & d.Hour & "," & d.Minute & "," & d.Second & "}"

        If Core.Player.ApricornData <> "" Then
            Core.Player.ApricornData &= Environment.NewLine
        End If

        Core.Player.ApricornData &= s
    End Sub

    Private Sub Take()
        Me.State = States.CanGive

        Dim text As String = Core.Player.Name & " " & Localization.GetString("apricorn_screen_obtain")

        If CInt(Apricorns(0)) > 0 Then
            Core.Player.Inventory.AddItem(159.ToString, CInt(Apricorns(0)))
            text &= "~" & Apricorns(0) & "  " & Item.GetItemByID(159.ToString).Name
        End If
        If CInt(Apricorns(1)) > 0 Then
            Core.Player.Inventory.AddItem(160.ToString, CInt(Apricorns(1)))
            text &= ",~" & Apricorns(1) & "  " & Item.GetItemByID(160.ToString).Name
        End If
        If CInt(Apricorns(2)) > 0 Then
            Core.Player.Inventory.AddItem(165.ToString, CInt(Apricorns(2)))
            text &= ",~" & Apricorns(2) & "  " & Item.GetItemByID(165.ToString).Name
        End If
        If CInt(Apricorns(3)) > 0 Then
            Core.Player.Inventory.AddItem(164.ToString, CInt(Apricorns(3)))
            text &= ",~" & Apricorns(3) & "  " & Item.GetItemByID(164.ToString).Name
        End If
        If CInt(Apricorns(4)) > 0 Then
            Core.Player.Inventory.AddItem(161.ToString, CInt(Apricorns(4)))
            text &= ",~" & Apricorns(4) & "  " & Item.GetItemByID(161.ToString).Name
        End If
        If CInt(Apricorns(5)) > 0 Then
            Core.Player.Inventory.AddItem(157.ToString, CInt(Apricorns(5)))
            text &= ",~" & Apricorns(5) & "  " & Item.GetItemByID(157.ToString).Name
        End If
        If CInt(Apricorns(6)) > 0 Then
            Core.Player.Inventory.AddItem(166.ToString, CInt(Apricorns(6)))
            text &= ",~" & Apricorns(6) & "  " & Item.GetItemByID(166.ToString).Name
        End If

        text &= "."
        ClearApricorns()

        Dim s As String = ""
        Dim Data() As String = Core.Player.ApricornData.SplitAtNewline()

        For i = 0 To Data.Count() - 1
            If Data(i).StartsWith("{" & Me.owner & "|") = False Then
                If s <> "" Then
                    s &= Environment.NewLine
                End If
                s &= Data(i)
            End If
        Next

        Core.Player.ApricornData = s

        TextBox.Show(text, {}, True, False)
    End Sub

    Private Function HasApricorns() As Boolean
        For i = 0 To 6
            If CInt(Apricorns(i)) > 0 Then
                Return True
            End If
        Next

        Return False
    End Function

    Private Sub GetSavedApricorns()
        ClearApricorns()

        If Core.Player.ApricornData <> "" Then
            Dim ApricornsData As List(Of String) = Core.Player.ApricornData.SplitAtNewline().ToList()

            For i = 0 To ApricornsData.Count - 1
                If i < ApricornsData.Count Then
                    Dim Apricorn As String = ApricornsData(i)

                    Apricorn = Apricorn.Remove(0, 1)
                    Apricorn = Apricorn.Remove(Apricorn.Length - 1, 1)

                    Dim ApricornData() As String = Apricorn.Split(CChar("|"))

                    If ApricornData(0) = Me.owner Then
                        Me.Apricorns = ApricornData(1).Split(CChar(",")).ToList()
                    End If
                End If
            Next
        End If
    End Sub

    Private Function CheckApricornStatus() As Integer
        Dim diff As Integer = 1440

        If Core.Player.ApricornData <> "" Then
            Dim ApricornsData As List(Of String) = Core.Player.ApricornData.SplitAtNewline().ToList()

            For i = 0 To ApricornsData.Count - 1
                If i < ApricornsData.Count Then
                    Dim Apricorn As String = ApricornsData(i)

                    Apricorn = Apricorn.Remove(0, 1)
                    Apricorn = Apricorn.Remove(Apricorn.Length - 1, 1)

                    Dim ApricornData() As String = Apricorn.Split(CChar("|"))

                    If ApricornData(0) = Me.owner Then
                        Dim d() As String = ApricornData(2).Split(CChar(","))
                        Dim gaveDate As Date = New Date(CInt(d(0)), CInt(d(1)), CInt(d(2)), CInt(d(3)), CInt(d(4)), CInt(d(5)))

                        diff = CInt((Date.Now - gaveDate).TotalMinutes)
                    End If
                End If
            Next
        End If

        Return diff
    End Function

    Private Class Label

        Public Position As Vector2
        Public Text As String
        Public ForeColor As Color = Color.White
        Public Font As SpriteFont

        Public Sub New(ByVal Text As String, ByVal Position As Vector2, ByVal Font As SpriteFont)
            Me.New(Text, Position, Color.White, Font)
        End Sub

        Public Sub New(ByVal Text As String, ByVal Position As Vector2, ByVal ForeColor As Color, ByVal Font As SpriteFont)
            Me.Position = Position
            Me.Text = Text
            Me.ForeColor = ForeColor
            Me.Font = Font
        End Sub

        Public Sub Draw()
            Core.SpriteBatch.DrawString(Me.Font, Me.Text, Me.Position, New Color(Me.ForeColor, _interfaceFade))
        End Sub

    End Class

    Public Class ButtonIcon

        Public Delegate Sub DoOnClick(ByVal e As ButtonIcon)

        Public Text As String
        Public Font As SpriteFont
        Public Position As Vector2
        Public Texture As Texture2D
        Public TextureRectangle As Rectangle
        Public Size As Size
        Public AdditionalValue As String = ""
        Public Enabled As Boolean = True

        Dim State As Integer = 0

        Dim DoSub As DoOnClick

        Public Sub New(ByVal DoOnClick As DoOnClick, ByVal Text As String, ByVal Font As SpriteFont, ByVal Texture As Texture2D, ByVal TextureRectangle As Rectangle, ByVal Position As Vector2, ByVal Size As Size, Optional ByVal AdditionalValue As String = "")
            Me.DoSub = DoOnClick

            Me.Text = Text
            Me.Font = Font
            Me.Position = Position
            Me.Size = Size

            Me.Texture = Texture
            Me.TextureRectangle = TextureRectangle
            Me.AdditionalValue = AdditionalValue
        End Sub

        Public Sub Update()
            If Enabled = True Then
                Dim MousePosition As Point = New Point(MouseHandler.MousePosition.X, MouseHandler.MousePosition.Y)
                Me.State = 0

                If New Rectangle(CInt(Me.Position.X), CInt(Me.Position.Y), Me.Size.Width, Me.Size.Height).Contains(MousePosition) = True Then
                    If MouseHandler.ButtonPressed(MouseHandler.MouseButtons.LeftButton) = True Then
                        Me.DoSub(Me)
                    Else
                        If MouseHandler.ButtonDown(MouseHandler.MouseButtons.LeftButton) = True Then
                            State = 2
                        Else
                            State = 1
                        End If
                    End If
                End If
            End If
        End Sub

        Public Sub Draw()
            Dim bColor As Color = New Color(Color.White.R - (State * 50), Color.White.G - (State * 50), Color.White.B - (State * 50))

            If Me.Enabled = False Then
                bColor = Color.DarkGray
            End If

            Core.SpriteBatch.Draw(Me.Texture, New Rectangle(CInt(Me.Position.X), CInt(Me.Position.Y), Me.Size.Width, Me.Size.Height), TextureRectangle, New Color(bColor, _interfaceFade))

            Dim TextSize As Vector2 = Font.MeasureString(Me.Text)
            Dim TColor As Color = New Color(Color.White, _interfaceFade)
            Core.SpriteBatch.DrawRectangle(New Rectangle(CInt(Me.Position.X + Me.Size.Width / 2 - TextSize.X / 2 - 8), CInt(Me.Position.Y + Me.Size.Height + 8), CInt(TextSize.X + 16), CInt(TextSize.Y + 16)), New Color(Color.Black, CInt(255 * 0.4F * _interfaceFade)))
            Core.SpriteBatch.DrawString(Me.Font, Me.Text, New Vector2(CInt(Me.Size.Width / 2) - CInt(TextSize.X / 2) + Me.Position.X, Me.Size.Height + Me.Position.Y + CInt(TextSize.Y / 2)), TColor)
        End Sub

        Public Sub Click()
            Me.DoSub(Me)
        End Sub

    End Class

End Class