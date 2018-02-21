Public Class InputScreen

    Inherits Screen

    Public Shared LastInput As String = ""

    Public Enum InputModes As Integer
        Text = 0
        Numbers = 1
        Name = 2
    End Enum

    Dim DefaultName As String = ""
    Dim CurrentText As String = ""
    Dim Sprites As New List(Of Texture2D)
    Dim Buttons As New List(Of InputButton)
    Dim InputMode As InputModes = InputModes.Text
    Dim MaxChars As Integer = 100
    Dim ButtonSelector As Vector2 = New Vector2(0, 0)

    Public PasswordMode As Boolean = False

    Public Delegate Sub ConfirmInput(ByVal input As String)

    Dim ConfirmSub As ConfirmInput = Nothing

    Public Sub New(ByVal currentScreen As Screen, ByVal DefaultName As String, ByVal InputMode As InputModes, ByVal CurrentText As String, ByVal MaxChars As Integer, ByVal Sprites As List(Of Texture2D), ByVal ConfirmSub As ConfirmInput)
        Me.PreScreen = currentScreen
        Me.DefaultName = DefaultName
        Me.CurrentText = CurrentText
        Me.Sprites = Sprites
        Me.InputMode = InputMode
        Me.MaxChars = MaxChars
        Me.ConfirmSub = ConfirmSub

        Initialize()
    End Sub

    Public Sub New(ByVal currentScreen As Screen, ByVal DefaultName As String, ByVal InputMode As InputModes, ByVal CurrentText As String, ByVal MaxChars As Integer, ByVal Sprites As List(Of Texture2D))
        Me.PreScreen = currentScreen
        Me.DefaultName = DefaultName
        Me.CurrentText = CurrentText
        Me.Sprites = Sprites
        Me.InputMode = InputMode
        Me.MaxChars = MaxChars

        Initialize()
    End Sub

    Private Sub Initialize()
        Me.Identification = Identifications.InputScreen
        Me.CanBePaused = True
        Me.CanChat = False
        Me.CanMuteMusic = False
        Me.MouseVisible = True

        If Me.MaxChars < 0 Then
            Me.MaxChars = 100
        End If

        InputButton.CapsLock = True

        Select Case Me.InputMode
            Case InputModes.Text
                Me.InitializeText()
            Case InputModes.Name
                Me.InitializeName()
            Case InputModes.Numbers
                Me.InitializeNumbers()
        End Select

        Buttons.Add(New InputButton("Shift", New Vector2(0, 4), InputButton.ButtonModes.CapsLock, 2))
        Buttons.Add(New InputButton("Delete", New Vector2(2, 4), InputButton.ButtonModes.Delete, 2))
        Buttons.Add(New InputButton("Default", New Vector2(4, 4), InputButton.ButtonModes.Default, 2))
        Buttons.Add(New InputButton("Confirm", New Vector2(6, 4), InputButton.ButtonModes.Enter, 2))
    End Sub

    Private Sub InitializeText()
        Dim chars() As String = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", " ", ".", ",", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", " ", "'", "-", "u", "v", "w", "x", "y", "z", " ", " ", " ", " ", "!", "?", "_"}
        Dim x As Integer = 0
        Dim y As Integer = 0

        For Each c As String In chars
            Me.Buttons.Add(New InputButton(c, New Vector2(x, y), InputButton.ButtonModes.Key, 1))

            x += 1
            If x > 12 Then
                x = 0
                y += 1
            End If
        Next

        Dim numbers() As String = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0", " ", " ", " "}
        x = 0
        y = 3
        For Each c As String In numbers
            Me.Buttons.Add(New InputButton(c, New Vector2(x, y), InputButton.ButtonModes.Key, 1))

            x += 1
        Next
    End Sub

    Private Sub InitializeName()
        Dim chars() As String = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", " ", " ", " ", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", " ", "'", "-", "u", "v", "w", "x", "y", "z", " ", " ", " ", " ", "!", "?", "_"}
        Dim x As Integer = 0
        Dim y As Integer = 0

        For Each c As String In chars
            Me.Buttons.Add(New InputButton(c, New Vector2(x, y), InputButton.ButtonModes.Key, 1))

            x += 1
            If x > 12 Then
                x = 0
                y += 1
            End If
        Next

        Dim numbers() As String = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0", " ", " ", " "}
        x = 0
        y = 3
        For Each c As String In numbers
            Me.Buttons.Add(New InputButton(c, New Vector2(x, y), InputButton.ButtonModes.Key, 1))

            x += 1
        Next
    End Sub

    Private Sub InitializeNumbers()
        Dim x As Integer = 0
        Dim y As Integer = 0
        Dim numbers() As String = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0", " ", " ", " "}

        For Each c As String In numbers
            Me.Buttons.Add(New InputButton(c, New Vector2(x, y), InputButton.ButtonModes.Key, 1))

            x += 1
        Next
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))

        If Sprites.Count > 0 Then
            Canvas.DrawRectangle(New Rectangle(CInt((Core.windowSize.Width / 2) - 384), 100, 704, 64), Color.White)
            Canvas.DrawBorder(1, New Rectangle(CInt((Core.windowSize.Width / 2) - 384), 100, 704, 64), Color.Gray)
            Core.SpriteBatch.Draw(Sprites(0), New Rectangle(CInt((Core.windowSize.Width / 2) - 384), 96, 64, 64), Color.White)
        Else
            Canvas.DrawRectangle(New Rectangle(CInt((Core.windowSize.Width / 2) - 320), 100, 640, 64), Color.White)
            Canvas.DrawBorder(1, New Rectangle(CInt((Core.windowSize.Width / 2) - 320), 100, 640, 64), Color.Gray)
        End If
        Canvas.DrawRectangle(New Rectangle(CInt((Core.windowSize.Width / 2) - 316), 104, 632, 56), Color.LightGray)

        Dim t As String = CurrentText
        If PasswordMode = True Then
            t = ""
            For cc = 0 To CurrentText.Length - 1
                t &= "*"
            Next
        End If

        If CurrentText.Length < Me.MaxChars Then
            t &= "_"
        End If
        Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(CInt((Core.windowSize.Width / 2) - 306), 112), Color.Black)

        Canvas.DrawRectangle(New Rectangle(CInt((Core.windowSize.Width / 2) - ((13 * 64) / 2)) - 4, 196, (13 * 64) + 8, 5 * 64 + 8), Color.White)
        Canvas.DrawBorder(1, New Rectangle(CInt((Core.windowSize.Width / 2) - ((13 * 64) / 2)) - 4, 196, (13 * 64) + 8, 5 * 64 + 8), Color.Gray)
        For Each b As InputButton In Me.Buttons
            b.Draw(New Vector2(CSng((Core.windowSize.Width / 2) - ((13 * 64) / 2)), 200), Me.ButtonSelector)
        Next

        Core.SpriteBatch.DrawString(FontManager.MainFont, "Chars left: " & (MaxChars - Me.CurrentText.Length).ToString(), New Vector2(CInt((Core.windowSize.Width / 2) + 180), 477), Color.Gray)

        Dim d As New Dictionary(Of Buttons, String)
        d.Add(Input.Buttons.A, "Enter")
        d.Add(Input.Buttons.B, "Delete")
        d.Add(Input.Buttons.Y, "Confirm")
        d.Add(Input.Buttons.X, "Clear")
        Me.DrawGamePadControls(d)
    End Sub

    Public Overrides Sub Update()
        If Controls.Right(True, True, False, False, True) = True Then
            Dim currentX As Integer = CInt(Me.ButtonSelector.X)
            Dim newX As Integer = 1000
            For Each b As InputButton In Me.Buttons
                If CInt(b.RelPosition.X) < newX And CInt(b.RelPosition.X) > currentX And CInt(b.RelPosition.Y) = CInt(Me.ButtonSelector.Y) Then
                    newX = CInt(b.RelPosition.X)
                End If
            Next
            If newX <> 1000 Then
                Me.ButtonSelector.X = newX
            End If
        End If
        If Controls.Left(True, True, False, False, True) = True Then
            Dim currentX As Integer = CInt(Me.ButtonSelector.X)
            Dim newX As Integer = -1
            For Each b As InputButton In Me.Buttons
                If CInt(b.RelPosition.X) > newX And CInt(b.RelPosition.X) < currentX And CInt(b.RelPosition.Y) = CInt(Me.ButtonSelector.Y) Then
                    newX = CInt(b.RelPosition.X)
                End If
            Next
            If newX <> -1 Then
                Me.ButtonSelector.X = newX
            End If
        End If
        If Controls.Down(True, True, False, False, True) = True Then
            Dim currentY As Integer = CInt(Me.ButtonSelector.Y)
            Dim newY As Integer = 1000
            For Each b As InputButton In Me.Buttons
                If CInt(b.RelPosition.Y) < newY And CInt(b.RelPosition.Y) > currentY And CInt(b.RelPosition.X) = CInt(Me.ButtonSelector.X) Then
                    newY = CInt(b.RelPosition.Y)
                End If
            Next
            If newY <> 1000 Then
                Me.ButtonSelector.Y = newY
            End If
        End If
        If Controls.Up(True, True, False, False, True) = True Then
            Dim currentY As Integer = CInt(Me.ButtonSelector.Y)
            Dim newY As Integer = -1
            For Each b As InputButton In Me.Buttons
                If CInt(b.RelPosition.Y) > newY And CInt(b.RelPosition.Y) < currentY And CInt(b.RelPosition.X) = CInt(Me.ButtonSelector.X) Then
                    newY = CInt(b.RelPosition.Y)
                End If
            Next
            If newY <> -1 Then
                Me.ButtonSelector.Y = newY
            End If
        End If

        For Each b As InputButton In Me.Buttons
            b.Update(New Vector2(CSng((Core.windowSize.Width / 2) - ((13 * 64) / 2)), 200), Me.ButtonSelector, Me)
        Next

        If Controls.Dismiss(False, False, True) = True Then
            If CurrentText.Length > 0 Then
                CurrentText = CurrentText.Remove(CurrentText.Length - 1, 1)
            End If
        End If

        If ControllerHandler.ButtonPressed(Input.Buttons.X) = True Then
            Me.CurrentText = ""
        End If

        If ControllerHandler.ButtonPressed(Input.Buttons.Y) = True Then
            If Me.ButtonSelector = New Vector2(6, 4) Then
                Me.Confirm()
            Else
                Me.ButtonSelector = New Vector2(6, 4)
            End If
        End If

        If ControllerHandler.ButtonPressed(Input.Buttons.LeftStick) = True Or ControllerHandler.ButtonPressed(Input.Buttons.RightStick) = True Then
            InputButton.CapsLock = Not InputButton.CapsLock
        End If

        Dim bText As String = Me.CurrentText
        KeyBindings.GetInput(Me.CurrentText, Me.MaxChars, True, True)

        If bText <> Me.CurrentText Then
            Dim chars() As String = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", " ", ".", ",", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "'", "-", "u", "v", "w", "x", "y", "z", "!", "?", "_", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"}
            Dim newText As String = ""

            For Each c As Char In Me.CurrentText
                If chars.Contains(c.ToString().ToLower()) Then
                    newText &= c.ToString()
                End If
            Next
            Me.CurrentText = newText
        End If
    End Sub

    Public Sub Confirm()
        Dim t As String = Me.CurrentText
        If t = "" Then
            t = Me.DefaultName
        End If
        LastInput = t

        If Not Me.ConfirmSub Is Nothing Then
            Me.ConfirmSub(LastInput)
        End If

        Core.SetScreen(Me.PreScreen)
    End Sub

    Class InputButton

        Public Shared CapsLock As Boolean = True
        Shared T As Texture2D
        Shared loadedTexture As Boolean = False

        Const RasterSize As Integer = 64

        Public Enum ButtonModes
            Enter
            Delete
            [Default]
            Key
            CapsLock
        End Enum

        Public DisplayText As String = "A"
        Public ReturnText As String = "A"
        Public Size As Integer = 64
        Public RelPosition As New Vector2(0, 0)
        Public ButtonMode As ButtonModes = ButtonModes.Key

        Public Sub New(ByVal DisplayText As String, ByVal RelPosition As Vector2, ByVal ButtonMode As ButtonModes, ByVal Size As Integer)
            If loadedTexture = False Then
                loadedTexture = True
                T = TextureManager.GetTexture("GUI\Menus\GTS", New Rectangle(368, 112, 1, 32), "")
            End If

            Me.DisplayText = DisplayText
            Me.ReturnText = DisplayText
            Me.RelPosition = RelPosition
            Me.ButtonMode = ButtonMode
            Me.Size = Size * RasterSize
        End Sub

        Public Sub Draw(ByVal StartPosition As Vector2, ByVal Selector As Vector2)
            Dim p As New Vector2(RelPosition.X * RasterSize + StartPosition.X, RelPosition.Y * RasterSize + StartPosition.Y)
            For i = 0 To Me.Size - 1
                Dim buttonColor As Color = Color.White
                Select Case Me.ButtonMode
                    Case ButtonModes.Key
                        buttonColor = Color.White
                    Case ButtonModes.Enter
                        buttonColor = Color.CadetBlue
                    Case ButtonModes.Delete
                        buttonColor = Color.Tomato
                    Case Else
                        buttonColor = New Color(100, 100, 100)
                End Select

                If Selector = Me.RelPosition Then
                    Core.SpriteBatch.Draw(T, New Rectangle(CInt(p.X) + i, CInt(p.Y), 1, RasterSize), Nothing, buttonColor, 0.0F, Vector2.Zero, SpriteEffects.FlipVertically, 0.0F)
                Else
                    Core.SpriteBatch.Draw(T, New Rectangle(CInt(p.X) + i, CInt(p.Y), 1, RasterSize), buttonColor)
                End If
            Next

            If Selector = Me.RelPosition Then
                Canvas.DrawBorder(2, New Rectangle(CInt(p.X), CInt(p.Y), Size, RasterSize), New Color(255, 0, 0, 100))
            Else
                Canvas.DrawBorder(1, New Rectangle(CInt(p.X), CInt(p.Y), Size, RasterSize), Color.Gray)
            End If

            Dim text As String = Me.DisplayText
            If Me.ButtonMode = ButtonModes.Key Then
                If CapsLock = True Then
                    text = text.ToUpper()
                Else
                    text = text.ToLower()
                End If
            End If

            Dim fontColor As Color = Color.Black
            If Me.ButtonMode <> ButtonModes.Key Then
                fontColor = Color.White
            End If

            Dim middelPoint As New Vector2(CSng(Me.Size / 2) + p.X, CSng(RasterSize / 2) + p.Y)
            Dim f As SpriteFont = FontManager.MainFont
            Core.SpriteBatch.DrawString(f, text, New Vector2(middelPoint.X - CSng(f.MeasureString(text).X / 2), middelPoint.Y - CSng(f.MeasureString(text).Y / 2)), fontColor)
        End Sub

        Public Sub Update(ByVal StartPosition As Vector2, ByRef Selector As Vector2, ByVal s As InputScreen)
            Dim p As New Vector2(RelPosition.X * RasterSize + StartPosition.X, RelPosition.Y * RasterSize + StartPosition.Y)

            Dim enterKey As Boolean = False
            If Controls.Accept(True, False, False) = True And New Rectangle(CInt(p.X), CInt(p.Y), Size, RasterSize).Contains(MouseHandler.MousePosition) = True Then
                If Selector = Me.RelPosition Then
                    enterKey = True
                Else
                    Selector = Me.RelPosition
                End If
            End If
            If Controls.Accept(False, False, True) = True Then
                If Selector = Me.RelPosition Then
                    enterKey = True
                End If
            End If
            If KeyBoardHandler.KeyPressed(Keys.Enter) = True Then
                If Selector = Me.RelPosition Then
                    enterKey = True
                End If
            End If

            If enterKey = True Then
                Select Case Me.ButtonMode
                    Case ButtonModes.Key
                        If s.CurrentText.Length < s.MaxChars Then
                            Dim st As String = Me.DisplayText
                            If Me.ButtonMode = ButtonModes.Key Then
                                If CapsLock = True Then
                                    st = ReturnText.ToUpper()
                                Else
                                    st = ReturnText.ToLower()
                                End If
                            End If
                            s.CurrentText &= st

                            If CapsLock = True Then
                                CapsLock = False
                            End If
                        End If
                    Case ButtonModes.Delete
                        If s.CurrentText <> "" Then
                            s.CurrentText = s.CurrentText.Remove(s.CurrentText.Length - 1, 1)
                        End If
                    Case ButtonModes.Default
                        s.CurrentText = s.DefaultName
                    Case ButtonModes.Enter
                        s.Confirm()
                    Case ButtonModes.CapsLock
                        CapsLock = Not CapsLock
                End Select
            End If
        End Sub

    End Class



End Class