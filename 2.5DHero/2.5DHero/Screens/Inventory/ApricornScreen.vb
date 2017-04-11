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

    Dim mainTexture As Texture2D
    Dim lastUpdateState As States = States.None

    Dim CursorIndex As Integer = 0

    Public Sub New(ByVal currentScreen As Screen, ByVal owner As String)
        Me.Identification = Identifications.ApricornScreen

        Me.PreScreen = currentScreen
        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
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

            Me.Labels.Add(New Label(Localization.GetString("apricorn_screen_apricorns"), New Vector2(80, 128), FontManager.MainFont))

            Select Case Me.State
                Case States.Wait
                    Me.Labels.Add(New Label(Localization.GetString("apricorn_screen_producing").Replace("~", vbNewLine), New Vector2(100, 200), FontManager.MainFont))
                    Me.Labels.Add(New Label(Localization.GetString("apricorn_screen_backadvice"), New Vector2(100, 260), Color.DarkGray, FontManager.MainFont))
                Case States.CanGive
                    Dim T As Texture2D = TextureManager.GetTexture("Items\ItemSheet")

                    Me.Labels.Add(New Label(Localization.GetString("apricorn_screen_choose_apricorns"), New Vector2(100, 200), FontManager.MainFont))
                    Dim RedApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(85), FontManager.MainFont, T, New Rectangle(240, 72, 24, 24), New Vector2(98, 240), New Size(48, 48), "85")
                    Dim BlueApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(89), FontManager.MainFont, T, New Rectangle(336, 72, 24, 24), New Vector2(98, 304), New Size(48, 48), "89")
                    Dim YellowApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(92), FontManager.MainFont, T, New Rectangle(384, 72, 24, 24), New Vector2(98, 368), New Size(48, 48), "92")
                    Dim GreenApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(93), FontManager.MainFont, T, New Rectangle(408, 72, 24, 24), New Vector2(98, 432), New Size(48, 48), "93")
                    Dim WhiteApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(97), FontManager.MainFont, T, New Rectangle(0, 96, 24, 24), New Vector2(98, 496), New Size(48, 48), "97")
                    Dim BlackApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(99), FontManager.MainFont, T, New Rectangle(48, 96, 24, 24), New Vector2(162, 240), New Size(48, 48), "99")
                    Dim PinkApricorn As ButtonIcon = New ButtonIcon(AddressOf Me.GiveApricorn, "0 / " & Core.Player.Inventory.GetItemAmount(101), FontManager.MainFont, T, New Rectangle(72, 96, 24, 24), New Vector2(162, 304), New Size(48, 48), "101")

                    Dim GiveButton As ButtonIcon = New ButtonIcon(AddressOf Me.Give, Localization.GetString("apricorn_screen_ok"), FontManager.MainFont, mainTexture, New Rectangle(48, 128, 16, 16), New Vector2(162, 496), New Size(48, 48), "OK")
                    GiveButton.Enabled = False
                    Dim ClearButton As ButtonIcon = New ButtonIcon(AddressOf Me.ClearApricorns, Localization.GetString("apricorn_screen_clear"), FontManager.MainFont, mainTexture, New Rectangle(64, 128, 16, 16), New Vector2(162, 432), New Size(48, 48), "Clear")

                    Buttons.AddRange({RedApricorn, BlueApricorn, YellowApricorn, GreenApricorn, WhiteApricorn, BlackApricorn, PinkApricorn, ClearButton, GiveButton})
                Case States.CanTake
                    Dim TakeButton As ButtonIcon = New ButtonIcon(AddressOf Me.Take, Localization.GetString("apricorn_screen_take"), FontManager.MainFont, mainTexture, New Rectangle(48, 128, 16, 16), New Vector2(98, 450), New Size(48, 48))
                    Buttons.AddRange({TakeButton})
                    Me.Labels.Add(New Label(Localization.GetString("apricorn_screen_ready"), New Vector2(100, 200), FontManager.MainFont))
            End Select
        End If
    End Sub

    Public Overrides Sub Update()
        TextBox.Update()
        InitializeScreen()

        If TextBox.Showing = False Then
            If Controls.Dismiss() = True Then
                Core.SetScreen(Me.PreScreen)
            End If

            If Controls.Up(True, True) = True Or Controls.Left(True, True) = True Then
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

            If Me.Buttons.Count > 0 Then
                If Controls.Accept(False, True, True) = True Then
                    Me.Buttons(Me.CursorIndex).Click()
                End If
            End If

            For Each B As ButtonIcon In Buttons
                B.Update()
            Next

            Select Case Me.State
                Case States.Wait
                    UpdateWait()
                Case States.CanGive
                    UpdateGive()
                Case States.CanTake
                    UpdateTake()
            End Select
        End If
    End Sub

    Private Sub UpdateWait()

    End Sub

    Private Sub UpdateGive()

    End Sub

    Private Sub UpdateTake()

    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 2, New Rectangle(60, 100, 800, 480))

        For i = 0 To Me.Buttons.Count - 1
            Dim b As ButtonIcon = Me.Buttons(i)

            If i = Me.CursorIndex Then
                Canvas.DrawRectangle(New Rectangle(CInt(b.Position.X) - 2, CInt(b.Position.Y) - 2, 52, 52), New Color(0, 125, 255))
                Canvas.DrawRectangle(New Rectangle(CInt(b.Position.X), CInt(b.Position.Y), 48, 48), New Color(102, 196, 255))
            End If

            b.Draw()
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

    Private Sub DrawWait()

    End Sub

    Private Sub DrawTake()
        For i = 0 To 6
            Dim x As Integer = 0
            Dim y As Integer = i
            While y > 1
                y -= 2
                x += 1
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

            Dim Item As Item = Item.GetItemByID(ItemID)
            Core.SpriteBatch.Draw(Item.Texture, New Rectangle(100 + x * 64, 260 + y * 96, 48, 48), Color.White)
            Core.SpriteBatch.DrawString(FontManager.MainFont, "x" & Apricorns(i), New Vector2(110 + x * 64, 300 + y * 96), Color.Black)
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

        If Core.Player.Inventory.GetItemAmount(CInt(b.AdditionalValue)) > CInt(Me.Apricorns(apricornID)) Then
            Me.Apricorns(apricornID) = CStr(CInt(Me.Apricorns(apricornID)) + 1)

            If HasApricorns() = True Then
                For Each Button As ButtonIcon In Me.Buttons
                    If Button.AdditionalValue = "OK" Then
                        Button.Enabled = True
                    End If
                Next
            End If
        End If

        AdjustButtonTexts()
    End Sub

    Private Sub AdjustButtonTexts()
        For Each b As ButtonIcon In Buttons
            If IsNumeric(b.AdditionalValue) = True Then
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

                b.Text = Me.Apricorns(apricornID) & " / " & Core.Player.Inventory.GetItemAmount(CInt(b.AdditionalValue))
            End If
        Next
    End Sub

    Private Sub ClearApricorns()
        Me.Apricorns = {"0", "0", "0", "0", "0", "0", "0"}.ToList()

        AdjustButtonTexts()
    End Sub

    Private Sub Give()
        Me.State = States.Wait

        Core.Player.Inventory.RemoveItem(85, CInt(Me.Apricorns(0)))
        Core.Player.Inventory.RemoveItem(89, CInt(Me.Apricorns(1)))
        Core.Player.Inventory.RemoveItem(92, CInt(Me.Apricorns(2)))
        Core.Player.Inventory.RemoveItem(93, CInt(Me.Apricorns(3)))
        Core.Player.Inventory.RemoveItem(97, CInt(Me.Apricorns(4)))
        Core.Player.Inventory.RemoveItem(99, CInt(Me.Apricorns(5)))
        Core.Player.Inventory.RemoveItem(101, CInt(Me.Apricorns(6)))

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
            Core.Player.ApricornData &= vbNewLine
        End If

        Core.Player.ApricornData &= s
    End Sub

    Private Sub Take()
        Me.State = States.CanGive

        Dim text As String = Core.Player.Name & Localization.GetString("apricorn_screen_obtain")

        If CInt(Apricorns(0)) > 0 Then
            Core.Player.Inventory.AddItem(159, CInt(Apricorns(0)))
            text &= "~" & Apricorns(0) & "  " & Item.GetItemByID(159).Name
        End If
        If CInt(Apricorns(1)) > 0 Then
            Core.Player.Inventory.AddItem(160, CInt(Apricorns(1)))
            text &= ",~" & Apricorns(1) & "  " & Item.GetItemByID(160).Name
        End If
        If CInt(Apricorns(2)) > 0 Then
            Core.Player.Inventory.AddItem(165, CInt(Apricorns(2)))
            text &= ",~" & Apricorns(2) & "  " & Item.GetItemByID(165).Name
        End If
        If CInt(Apricorns(3)) > 0 Then
            Core.Player.Inventory.AddItem(164, CInt(Apricorns(3)))
            text &= ",~" & Apricorns(3) & "  " & Item.GetItemByID(164).Name
        End If
        If CInt(Apricorns(4)) > 0 Then
            Core.Player.Inventory.AddItem(161, CInt(Apricorns(4)))
            text &= ",~" & Apricorns(4) & "  " & Item.GetItemByID(161).Name
        End If
        If CInt(Apricorns(5)) > 0 Then
            Core.Player.Inventory.AddItem(157, CInt(Apricorns(5)))
            text &= ",~" & Apricorns(5) & "  " & Item.GetItemByID(157).Name
        End If
        If CInt(Apricorns(6)) > 0 Then
            Core.Player.Inventory.AddItem(166, CInt(Apricorns(6)))
            text &= ",~" & Apricorns(6) & "  " & Item.GetItemByID(166).Name
        End If

        text &= "."
        ClearApricorns()

        Dim s As String = ""
        Dim Data() As String = Core.Player.ApricornData.SplitAtNewline()

        For i = 0 To Data.Count() - 1
            If Data(i).StartsWith("{" & Me.owner & "|") = False Then
                If s <> "" Then
                    s &= vbNewLine
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
                        Dim currentDate As Date = Date.Now

                        diff = CInt(DateDiff(DateInterval.Minute, gaveDate, currentDate))
                    End If
                End If
            Next
        End If

        Return diff
    End Function

    Private Class Label

        Public Position As Vector2
        Public Text As String
        Public ForeColor As Color = Color.Black
        Public Font As SpriteFont

        Public Sub New(ByVal Text As String, ByVal Position As Vector2, ByVal Font As SpriteFont)
            Me.New(Text, Position, Color.Black, Font)
        End Sub

        Public Sub New(ByVal Text As String, ByVal Position As Vector2, ByVal ForeColor As Color, ByVal Font As SpriteFont)
            Me.Position = Position
            Me.Text = Text
            Me.ForeColor = ForeColor
            Me.Font = Font
        End Sub

        Public Sub Draw()
            Core.SpriteBatch.DrawString(Me.Font, Me.Text, Me.Position, Me.ForeColor)
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

            Core.SpriteBatch.Draw(Me.Texture, New Rectangle(CInt(Me.Position.X), CInt(Me.Position.Y), Me.Size.Width, Me.Size.Height), TextureRectangle, bColor)

            Dim TextSize As Vector2 = Font.MeasureString(Me.Text)
            Dim TColor As Color = Color.Black
            Core.SpriteBatch.DrawString(Me.Font, Me.Text, New Vector2(CInt(Me.Size.Width / 2) - CInt(TextSize.X / 2) + Me.Position.X, Me.Size.Height + Me.Position.Y), TColor)
        End Sub

        Public Sub Click()
            Me.DoSub(Me)
        End Sub

    End Class

End Class