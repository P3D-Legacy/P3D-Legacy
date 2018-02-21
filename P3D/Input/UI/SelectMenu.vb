Namespace UI

    ''' <summary>
    ''' A menu that displays multiple selectable options to the player.
    ''' </summary>
    Public Class SelectMenu

        ''' <summary>
        ''' The event that gets fired upon selection.
        ''' </summary>
        ''' <param name="s">The menu that the item got selected on.</param>
        Public Delegate Sub ClickEvent(ByVal s As SelectMenu)

        Dim Items As New List(Of String)
        Dim Index As Integer = 0
        Dim ClickHandler As ClickEvent = Nothing
        Dim BackIndex As Integer = 0
        Public Visible As Boolean = True
        Public Scroll As Integer = 0

        Dim t1 As Texture2D
        Dim t2 As Texture2D

        ''' <summary>
        ''' Creates a new instance of a select menu.
        ''' </summary>
        ''' <param name="Items">The items in the menu.</param>
        ''' <param name="Index">The selected index.</param>
        ''' <param name="ClickHandle">The method to call when the player selects an item.</param>
        ''' <param name="BackIndex">The index of the item to select when back is pressed.</param>
        Public Sub New(ByVal Items As List(Of String), ByVal Index As Integer, ByVal ClickHandle As ClickEvent, ByVal BackIndex As Integer)
            Me.Items = Items
            Me.Index = Index
            Me.ClickHandler = ClickHandle
            Me.BackIndex = BackIndex
            If Me.BackIndex < 0 Then
                Me.BackIndex = Me.Items.Count + Me.BackIndex
            End If
            Me.Visible = True

            t1 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(16, 16, 16, 16), "")
            t2 = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(32, 16, 16, 16), "")

            SetCursorDest()
            cursorPos = cursorDest
        End Sub

        Public Sub Update()
            If Visible = True Then
                cursorPos.Y = MathHelper.Lerp(cursorDest.Y, cursorPos.Y, 0.6F)

                If Controls.Up(True, True, True, True, True, True) = True Then
                    Me.Index -= 1
                    If Me.Index < 0 Then
                        Me.Index = Me.Items.Count - 1
                    End If
                End If
                If Controls.Down(True, True, True, True, True, True) = True Then
                    Me.Index += 1
                    If Index > Items.Count - 1 Then
                        Index = 0
                    End If
                End If

                For i = Scroll To Me.Scroll + 8
                    If i <= Me.Items.Count - 1 Then
                        If Controls.Accept(True, False, False) = True And i = Me.Index And New Rectangle(Core.windowSize.Width - 270, 66 * ((i + 1) - Scroll), 256, 64).Contains(MouseHandler.MousePosition) = True Or
                            Controls.Accept(False, True, True) = True And i = Me.Index Or Controls.Dismiss(True, True, True) = True And Me.BackIndex = Me.Index Then

                            If Not ClickHandler Is Nothing Then
                                ClickHandler(Me)
                            End If
                            Me.Visible = False
                        End If
                        If Controls.Dismiss(True, True, True) = True Then
                            Me.Index = Me.BackIndex
                            If Not ClickHandler Is Nothing Then
                                ClickHandler(Me)
                            End If
                            Me.Visible = False
                        End If
                        If New Rectangle(Core.windowSize.Width - 270, 66 * ((i + 1) - Scroll), 256, 64).Contains(MouseHandler.MousePosition) = True And Controls.Accept(True, False, False) = True Then
                            Me.Index = i
                        End If
                    End If
                Next

                If Index - Scroll > 8 Then
                    Scroll = Index - 8
                End If
                If Index - Scroll < 0 Then
                    Scroll = Index
                End If
                SetCursorDest()
            End If
        End Sub

        Private cursorPos As Vector2
        Private cursorDest As Vector2

        Public Sub Draw()
            If Visible = True Then
                For i = Scroll To Me.Scroll + 8
                    If i <= Me.Items.Count - 1 Then
                        Dim Text As String = Items(i)

                        Dim startPos As New Vector2(Core.windowSize.Width - 270, 66 * ((i + 1) - Scroll))

                        Core.SpriteBatch.Draw(t1, New Rectangle(CInt(startPos.X), CInt(startPos.Y), 64, 64), Color.White)
                        Core.SpriteBatch.Draw(t2, New Rectangle(CInt(startPos.X + 64), CInt(startPos.Y), 64, 64), Color.White)
                        Core.SpriteBatch.Draw(t2, New Rectangle(CInt(startPos.X + 128), CInt(startPos.Y), 64, 64), Color.White)
                        Core.SpriteBatch.Draw(t1, New Rectangle(CInt(startPos.X + 192), CInt(startPos.Y), 64, 64), Nothing, Color.White, 0.0F, New Vector2(0), SpriteEffects.FlipHorizontally, 0.0F)

                        Core.SpriteBatch.DrawString(FontManager.MainFont, Text, New Vector2(startPos.X + 128 - (FontManager.MainFont.MeasureString(Text).X * 1.4F) / 2, startPos.Y + 15), Color.Black, 0.0F, Vector2.Zero, 1.4F, SpriteEffects.None, 0.0F)
                    End If
                Next
            End If

            Dim cPosition As Vector2 = New Vector2(cursorPos.X + 128, cursorPos.Y - 40)
            Dim t As Texture2D = TextureManager.GetTexture("GUI\Menus\General", New Rectangle(0, 0, 16, 16), "")
            Core.SpriteBatch.Draw(t, New Rectangle(CInt(cPosition.X), CInt(cPosition.Y), 64, 64), Color.White)
        End Sub

        Private Sub SetCursorDest()
            cursorDest = New Vector2(Core.windowSize.Width - 270, 66 * ((Index + 1) - Scroll))
        End Sub

        Public ReadOnly Property SelectedItem() As String
            Get
                Return Items(Me.Index)
            End Get
        End Property

    End Class

End Namespace