Public Class AddServerScreen

    Inherits Screen

    Private ServerNames As New List(Of String)

    Private IdentifyName As String = ""
    Private Address As String = ""

    Private Index As Integer = 0
    Private ButtonIndex As Integer = 0

    Private NewServer As Boolean = True
    Private EditServer As JoinServerScreen.Server = Nothing

    Public Sub New(ByVal currentScreen As Screen, ByVal servers As List(Of JoinServerScreen.Server), ByVal NewServer As Boolean, ByVal EditServer As JoinServerScreen.Server)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.AddServerScreen

        For Each s As JoinServerScreen.Server In servers
            Me.ServerNames.Add(s.IdentifierName.ToLower())
        Next

        Me.CanBePaused = True
        Me.CanChat = False
        Me.CanMuteMusic = False
        Me.MouseVisible = True

        Me.NewServer = NewServer
        Me.EditServer = EditServer

        If NewServer = False Then
            Me.Address = EditServer.GetAddressString()
            Me.IdentifyName = EditServer.IdentifierName
        End If
    End Sub

    Public Overrides Sub Draw()
        Dim Tx As Integer = CInt(World.CurrentSeason)
        Dim Ty As Integer = 0
        If Tx > 1 Then
            Tx -= 2
            Ty += 1
        End If

        Dim pattern As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(160 + Tx * 16, Ty * 16, 16, 16), "")
        For Dx = 0 To Core.windowSize.Width Step 128
            For Dy = 0 To Core.windowSize.Height Step 128
                Dim c As Color = Color.White

                Core.SpriteBatch.Draw(pattern, New Rectangle(Dx, Dy, 128, 128), c)
            Next
        Next

        Canvas.DrawRectangle(New Rectangle(0, 75, Core.windowSize.Width, 680 - 240), New Color(0, 0, 0, 128))

        Core.SpriteBatch.DrawString(FontManager.MainFont, "Add a server", New Vector2(CSng(Core.windowSize.Width / 2 - FontManager.MainFont.MeasureString("Add a server").X), 14), Color.White, 0.0F, New Vector2(0), 2.0F, SpriteEffects.None, 0.0F)

        Core.SpriteBatch.DrawString(FontManager.MainFont, "Name:", New Vector2(CSng(Core.windowSize.Width / 2 - 300), 140), Color.White)
        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 170, 600, 40), New Color(40, 40, 40, 255))

        If Index = 0 Then
            Canvas.DrawBorder(3, New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 170, 600, 40), New Color(220, 220, 220, 255))
        Else
            Canvas.DrawBorder(3, New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 170, 600, 40), New Color(100, 100, 100, 255))
        End If

        Dim t As String = Me.IdentifyName
        If t.Length < 30 And Index = 0 Then
            t &= "_"
            If Me.IdentifyName = "" And ControllerHandler.IsConnected() = True Then
                t = "Press Y to edit."
            End If
        End If
        Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - 294), 175), Color.White)

        Core.SpriteBatch.DrawString(FontManager.MainFont, "Address:", New Vector2(CSng(Core.windowSize.Width / 2 - 300), 270), Color.White)
        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 300, 600, 40), New Color(40, 40, 40, 255))

        If Index = 1 Then
            Canvas.DrawBorder(3, New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 300, 600, 40), New Color(220, 220, 220, 255))
        Else
            Canvas.DrawBorder(3, New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 300, 600, 40), New Color(100, 100, 100, 255))
        End If

        t = Me.Address
        If t.Length < 30 And Index = 1 Then
            t &= "_"
            If Me.Address = "" And ControllerHandler.IsConnected() = True Then
                t = "Press Y to edit."
            End If
        End If
        Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CSng(Core.windowSize.Width / 2 - 294), 305), Color.White)

        Dim CanvasTexture As Texture2D
        CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")

        For i = 0 To 1
            Dim Text As String = ""
            Select Case i
                Case 0
                    Text = "Done"
                Case 1
                    Text = "Back"
            End Select

            If i = Me.ButtonIndex Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 180 + i * 192, 544, 128, 64))
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Text, New Vector2(CInt(Core.windowSize.Width / 2) - 162 + i * 192, 574), Color.Black)
        Next

        If IsValid() <> "" Then
            Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 430, 600, 40), New Color(40, 40, 40, 255))
            Canvas.DrawBorder(3, New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 430, 600, 40), New Color(200, 200, 200, 255))
            Core.SpriteBatch.DrawString(FontManager.MainFont, IsValid(), New Vector2(CInt(Core.windowSize.Width / 2 - 294), 436), New Color(180, 0, 0, 255))
        End If

        Dim d As New Dictionary(Of Buttons, String)
        d.Add(Input.Buttons.A, "Accept")
        d.Add(Input.Buttons.B, "Back")
        d.Add(Input.Buttons.Y, "Edit")
        d.Add(Input.Buttons.X, "Clear")
        Me.DrawGamePadControls(d)
    End Sub

    Public Overrides Sub Update()
        If KeyBoardHandler.KeyPressed(Keys.Tab) = True Then
            If Controls.ShiftDown() = True Then
                Me.Index = 0
            Else
                Me.Index = 1
            End If
        End If
        If Controls.Up(True, True, False, False, True, True) = True Then
            Me.Index = 0
        End If
        If Controls.Down(True, True, False, False, True, True) = True Then
            Me.Index = 1
        End If
        If Controls.Left(True, True, True, False, True, True) = True Then
            Me.ButtonIndex = 0
        End If
        If Controls.Right(True, True, True, False, True, True) = True Then
            Me.ButtonIndex = 1
        End If

        If Controls.Accept(True, False, False) = True Then
            If New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 170, 600, 40).Contains(MouseHandler.MousePosition) Then
                Index = 0
            End If

            If New Rectangle(CInt(Core.windowSize.Width / 2 - 300), 300, 600, 40).Contains(MouseHandler.MousePosition) Then
                Index = 1
            End If
        End If

        If Core.GameInstance.IsMouseVisible = True Then
            For i = 0 To 1
                If New Rectangle(CInt(Core.windowSize.Width / 2) - 180 + i * 192, 544, 128 + 32, 64 + 32).Contains(MouseHandler.MousePosition) Then
                    If ButtonIndex = i Then
                        If Controls.Accept(True, False, False) = True Then
                            If i = 0 Then
                                ButtonDone()
                            Else
                                ButtonCancel()
                            End If
                        End If
                    Else
                        ButtonIndex = i
                    End If
                End If
            Next
        End If

        Select Case Index
            Case 0
                KeyBindings.GetInput(Me.IdentifyName, 30, True, True)
                If ControllerHandler.ButtonPressed(Buttons.Y) = True Then
                    Core.SetScreen(New InputScreen(Me, "Pokemon3D server", InputScreen.InputModes.Text, Me.IdentifyName, 30, New List(Of Texture2D), AddressOf Me.AcceptName))
                End If
                If ControllerHandler.ButtonPressed(Buttons.X) = True Then
                    Me.IdentifyName = ""
                End If
            Case 1
                KeyBindings.GetInput(Me.Address, 30, True, True)
                If ControllerHandler.ButtonPressed(Buttons.Y) = True Then
                    Core.SetScreen(New InputScreen(Me, "127.0.0.1", InputScreen.InputModes.Text, Me.Address, 30, New List(Of Texture2D), AddressOf Me.AcceptAddress))
                End If
                If ControllerHandler.ButtonPressed(Buttons.X) = True Then
                    Me.Address = ""
                End If
        End Select

        Select Case ButtonIndex
            Case 0
                If Controls.Accept(False, False, True) = True Or KeyBoardHandler.KeyPressed(Keys.Enter) = True Then
                    ButtonDone()
                End If
            Case 1
                If Controls.Accept(False, False, True) = True Or KeyBoardHandler.KeyPressed(Keys.Enter) = True Then
                    ButtonCancel()
                End If
        End Select

        If Controls.Dismiss(True, False, True) = True Then
            Core.SetScreen(Me.PreScreen)
        End If
    End Sub

    Private Sub AcceptName(ByVal input As String)
        Me.IdentifyName = input
    End Sub

    Private Sub AcceptAddress(ByVal input As String)
        Me.Address = input
    End Sub

    Private Function IsValid() As String
        If Me.IdentifyName = "" Then
            Return "The server name cannot be empty."
        End If
        If Me.Address = "" Then
            Return "The address cannot be empty."
        End If
        If Me.ServerNames.Contains(Me.IdentifyName.ToLower()) = True Then
            Return "This server name already exists on the list."
        End If

        Return ""
    End Function

    Private Sub ButtonDone()
        If IsValid() = "" Then
            Dim data As List(Of String) = System.IO.File.ReadAllLines(GameController.GamePath & "\Save\server_list.dat").ToList()
            data.Add(Me.IdentifyName & "," & Me.Address)
            System.IO.File.WriteAllLines(GameController.GamePath & "\Save\server_list.dat", data.ToArray())
            Core.SetScreen(Me.PreScreen)
        End If
    End Sub

    Private Sub ButtonCancel()
        If NewServer = False Then
            Dim data As List(Of String) = System.IO.File.ReadAllLines(GameController.GamePath & "\Save\server_list.dat").ToList()
            data.Add(EditServer.ToString())
            System.IO.File.WriteAllLines(GameController.GamePath & "\Save\server_list.dat", data.ToArray())
        End If

        Core.SetScreen(Me.PreScreen)
    End Sub

End Class