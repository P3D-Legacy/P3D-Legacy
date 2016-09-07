Public Class ChooseBox

    Public Delegate Sub DoAnswer(ByVal result As Integer)

    Public Options() As String
    Public index As Integer = 0

    Dim PositionY As Single = 0

    Public Showing As Boolean = False
    Public readyForResult As Boolean = False
    Public result As Integer = 0
    Public resultID As Integer = 0
    Public ActionScript As Boolean = False

    Public Shared CancelIndex As Integer = -1

    Public TextFont As FontContainer

    Public DoDelegate As Boolean = False

    Dim Subs As DoAnswer

    Public UpdateEntities() As Entity

    Public Sub Show(ByVal Options() As String, ByVal DoSubs As DoAnswer)
        Me.resultID = 0
        Me.Options = Options
        Me.index = 0
        Me.readyForResult = False
        Me.Showing = True
        Me.Subs = DoSubs
        Me.ActionScript = False
        Me.DoDelegate = True
        Me.TextFont = FontManager.GetFontContainer("textfont")

        SetupOptions()
    End Sub

    Public Sub Show(ByVal Options() As String, ByVal ID As Integer, ByVal ActionScript As Boolean)
        Me.resultID = ID
        Me.Options = Options
        Me.index = 0
        Me.readyForResult = False
        Me.Showing = True
        Me.ActionScript = ActionScript
        Me.DoDelegate = False
        Me.TextFont = FontManager.GetFontContainer("textfont")

        SetupOptions()
    End Sub

    Public Sub Show(ByVal Options() As String, ByVal ID As Integer, ByVal UpdateEntities() As Entity)
        Me.resultID = ID
        Me.Options = Options
        Me.index = 0
        Me.readyForResult = False
        Me.Showing = True
        Me.UpdateEntities = UpdateEntities
        Me.ActionScript = False
        Me.DoDelegate = False
        Me.TextFont = FontManager.GetFontContainer("textfont")

        SetupOptions()
    End Sub

    Private Sub SetupOptions()
        For i = 0 To Options.Count - 1
            Options(i) = Options(i).Replace("<playername>", Core.Player.Name)
        Next
    End Sub

    Public Function getResult(ByVal ID As Integer) As Integer
        If Me.readyForResult = True Then
            If Me.resultID = ID Then
                Return result
            Else
                Return -1
            End If
        Else
            Return -1
        End If
    End Function

    Public Sub Update()
        Update(True)
    End Sub

    Public Sub Update(ByVal RaiseClickEvent As Boolean)
        If Me.Showing = True Then
            If Controls.Down(True, True, True) Then
                Me.index += 1
            End If
            If Controls.Up(True, True, True) Then
                Me.index -= 1
            End If

            If Me.index < 0 Then
                Me.index = Me.Options.Count - 1
            End If
            If Me.index = Me.Options.Count Then
                Me.index = 0
            End If
            If RaiseClickEvent = True Then
                If Controls.Accept() = True Then
                    Me.PlayClickSound()
                    Me.result = index
                    Me.HandleResult()
                End If
                If Controls.Dismiss() = True And CancelIndex > -1 Then
                    Me.PlayClickSound()
                    Me.result = CancelIndex
                    Me.HandleResult()
                End If
            End If
        End If
    End Sub

    Private Sub PlayClickSound()
        If Screen.TextBox.Showing = False Then
            SoundManager.PlaySound("select")
        End If
    End Sub

    Private Sub HandleResult()
        ChooseBox.CancelIndex = -1
        Me.readyForResult = True
        Me.Showing = False
        If Me.DoDelegate = True Then
            Subs(result)
        Else
            If Core.CurrentScreen.Identification = Screen.Identifications.OverworldScreen Then
                If Me.ActionScript = True Then
                    Dim c As OverworldScreen = CType(Core.CurrentScreen, OverworldScreen)
                    c.ActionScript.Switch(Me.Options(result))
                Else
                    For Each Entity As Entity In UpdateEntities
                        Entity.ResultFunction(result)
                    Next
                End If
            End If
        End If
    End Sub

    Public Sub Draw(ByVal Position As Vector2)
        If Me.Showing = True Then
            With Core.SpriteBatch
                .Draw(TextureManager.GetTexture("GUI\Overworld\ChooseBox"), New Rectangle(CInt(Position.X), CInt(Position.Y), 288, 48), New Rectangle(0, 0, 96, 16), Color.White)
                For i = 0 To Options.Count - 2
                    .Draw(TextureManager.GetTexture("GUI\Overworld\ChooseBox"), New Rectangle(CInt(Position.X), CInt(Position.Y) + 48 + i * 48, 288, 48), New Rectangle(0, 16, 96, 16), Color.White)
                Next
                .Draw(TextureManager.GetTexture("GUI\Overworld\ChooseBox"), New Rectangle(CInt(Position.X), CInt(Position.Y) + 96 + (Options.Count - 2) * 48, 288, 48), New Rectangle(0, 32, 96, 16), Color.White)
                For i = 0 To Options.Count - 1
                    Dim m As Single = 1.0F
                    Select Case Me.TextFont.FontName.ToLower()
                        Case "textfont", "braille"
                            m = 2.0F
                    End Select

                    .DrawString(Me.TextFont.SpriteFont, Options(i).Replace("[POKE]", "Poké"), New Vector2(CInt(Position.X + 40), CInt(Position.Y) + 32 + i * 48), Color.Black, 0.0F, Vector2.Zero, m, SpriteEffects.None, 0.0F)
                Next
                .Draw(TextureManager.GetTexture("GUI\Overworld\ChooseBox"), New Rectangle(CInt(Position.X + 20), CInt(Position.Y) + 36 + index * 48, 10, 20), New Rectangle(96, 0, 3, 6), Color.White)
            End With
        End If
    End Sub

    Public Sub Draw()
        If Me.Showing = True Then
            Dim Position As Vector2 = New Vector2(CInt(Core.windowSize.Width / 2) - 48, Core.windowSize.Height - 160.0F - 96.0F - (Options.Count - 1) * 48)
            Me.Draw(Position)
        End If
    End Sub

End Class