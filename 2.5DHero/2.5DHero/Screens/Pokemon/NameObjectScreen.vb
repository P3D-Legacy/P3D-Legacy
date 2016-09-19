''' <summary>
''' A class to name Pokémon and other objects (namely the rival).
''' </summary>
''' <remarks>Inherits from screen.</remarks>
Public Class NameObjectScreen

    Inherits Screen

    Private _pokemon As Pokemon 'Temporarly stores the Pokémon to rename.
    Private _currentText As String = "" 'The current Text in the textbox.
    Private _mainTexture As Texture2D 'The temporary texture. Loads "GUI\Menus\Menu"

    Private _index As Integer = 0 'the button index (0 or 1)
    Private _askedRename As Boolean = False 'If the question to rename is answered or not.
    Private _renamePokemon As Boolean = False 'If a Pokémon is getting renamed.

    Private _canChooseNo As Boolean = True 'if the player can choose to not rename the object.
    Private _defaultName As String = "Name" 'The default name (that also gets mentioned in the question).
    Private _delay As Single = 0.0F 'The delay until the question can be answered.

    ''' <summary>
    ''' Handles the NameAccept event which fires if the object gets renamed.
    ''' </summary>
    ''' <param name="Name">The new name of the object</param>
    Public Delegate Sub DNameAcceptEventHandler(ByVal Name As String)

    Private _acceptName As DNameAcceptEventHandler

    ''' <summary>
    ''' Creates a new instance of the NameObjectScreen class and sets its mode to "Rename Pokémon".
    ''' </summary>
    ''' <param name="CurrentScreen">The currently active screen.</param>
    ''' <param name="Pokemon">The Pokémon reference to rename.</param>
    Public Sub New(ByVal CurrentScreen As Screen, ByVal Pokemon As Pokemon)
        'Set default values:
        Me.Identification = Identifications.NameObjectScreen
        Me.PreScreen = CurrentScreen
        Me.MouseVisible = True
        Me.CanChat = False
        Me.CanMuteMusic = False
        Me.CanBePaused = False

        Me._pokemon = Pokemon
        Me._defaultName = Pokemon.GetDisplayName()
        Me._renamePokemon = True

        'Load texture:
        Me._mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        'Show the Pokémon image:
        Screen.PokemonImageView.Show(Pokemon, True)
    End Sub

    Public Sub New(ByVal CurrentScreen As Screen, ByVal Texture As Texture2D, ByVal canExit As Boolean, ByVal canChoose As Boolean, ByVal NameString As String, ByVal DefaultName As String, ByVal AcceptName As DNameAcceptEventHandler)
        Me.Identification = Identifications.NameObjectScreen
        Me.PreScreen = CurrentScreen

        Me._mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        Me._acceptName = AcceptName
        Me._canChooseNo = canExit
        If canChoose = False Then
            Me._delay = 5.0F
        End If
        Me._askedRename = Not canChoose
        Me._defaultName = NameString
        Me._currentText = DefaultName

        Me._renamePokemon = False
        Me.MouseVisible = True
        Me.CanBePaused = True
        Me.CanChat = False
        Me.CanMuteMusic = False

        Screen.PokemonImageView.Show(Texture)
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))

        Dim CanvasTexture As Texture2D
        For i = 0 To 1
            Dim Text As String = "Rename"
            If _askedRename = True Then
                Text = "OK"
            End If

            If i = 1 Then
                Text = "Cancel"
            End If

            If i = _index Then
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                CanvasTexture = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            If i = 1 And _canChooseNo = True Or i = 0 Then
                Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 182 + i * 192 + 22, Core.windowSize.Height - 128, 128, 64))
                Core.SpriteBatch.DrawString(FontManager.InGameFont, Text, New Vector2(CInt(Core.windowSize.Width / 2) - 164 + i * 192 + 22, Core.windowSize.Height - 96), Color.Black)
            End If
        Next

        If _askedRename = False Then
            Dim genderString As String = ""
            Dim genderUnicode As Integer = 0
            If _renamePokemon = True Then
                If _pokemon.Gender = Pokemon.Genders.Male Then
                    genderString = "     "
                    genderUnicode = 156
                ElseIf _pokemon.Gender = Pokemon.Genders.Female Then
                    genderString = "     "
                    genderUnicode = 157
                End If
            End If

            Core.SpriteBatch.DrawString(FontManager.InGameFont, "Rename " & Me._defaultName & genderString & "?", New Vector2(CInt(Core.windowSize.Width / 2) - 182 + 25, 93), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "Rename " & Me._defaultName & genderString & "?", New Vector2(CInt(Core.windowSize.Width / 2) - 182 + 22, 90), Color.White)

            If genderUnicode <> 0 Then
                Core.SpriteBatch.DrawString(FontManager.TextFont, ChrW(genderUnicode), New Vector2(CInt(Core.windowSize.Width / 2) + FontManager.InGameFont.MeasureString("Rename " & Me._defaultName).X - 147, 93), Color.Black, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
                Core.SpriteBatch.DrawString(FontManager.TextFont, ChrW(genderUnicode), New Vector2(CInt(Core.windowSize.Width / 2) + FontManager.InGameFont.MeasureString("Rename " & Me._defaultName).X - 150, 90), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
            End If

            ChooseBox.Showing = False
        Else
            If _delay = 0.0F Then
                Dim genderString As String = ""
                Dim genderUnicode As Integer = 0
                If _renamePokemon = True Then
                    If _pokemon.Gender = Pokemon.Genders.Male Then
                        genderString = "     "
                        genderUnicode = 156
                    ElseIf _pokemon.Gender = Pokemon.Genders.Female Then
                        genderString = "     "
                        genderUnicode = 157
                    End If
                End If

                Core.SpriteBatch.DrawString(FontManager.InGameFont, "Enter name for " & Me._defaultName & genderString & ":", New Vector2(CInt(Core.windowSize.Width / 2) - 182 + 25, 93), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, "Enter name for " & Me._defaultName & genderString & ":", New Vector2(CInt(Core.windowSize.Width / 2) - 182 + 22, 90), Color.White)

                If genderUnicode <> 0 Then
                    Core.SpriteBatch.DrawString(FontManager.TextFont, ChrW(genderUnicode), New Vector2(CInt(Core.windowSize.Width / 2) + FontManager.InGameFont.MeasureString("Enter name for " & Me._defaultName).X - 150, 93), Color.Black, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
                    Core.SpriteBatch.DrawString(FontManager.TextFont, ChrW(genderUnicode), New Vector2(CInt(Core.windowSize.Width / 2) + FontManager.InGameFont.MeasureString("Enter name for " & Me._defaultName).X - 153, 90), Color.White, 0.0F, Vector2.Zero, 2.0F, SpriteEffects.None, 0.0F)
                End If

                Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2) - 89, 136, 208, 32), New Color(101, 142, 255))
                    DrawTextBox()
                End If
            End If

        PokemonImageView.Draw()
    End Sub

    Private Function TextboxPosition() As Vector2
        Return New Vector2(CInt(Core.windowSize.Width / 2) - 85, 140)
    End Function

    Private Sub DrawTextBox()
        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2) - 85, 140, 200, 24), Color.White)

        Dim t As String = Me._currentText
        If t.Length < 20 Then
            t &= "_"
        End If
        Core.SpriteBatch.DrawString(FontManager.MiniFont, t, TextboxPosition(), Color.Black)
    End Sub

    Public Overrides Sub Update()
        If CBool(GameModeManager.GetGameRuleValue("ForceRename", "0")) = True Then
            Me._askedRename = True
            Me._canChooseNo = False
        End If

        Dim mouseOver As Boolean = False
        For i = 0 To 1
            If New Rectangle(CInt(Core.windowSize.Width / 2) - 182 + i * 192, Core.windowSize.Height - 128, 128 + 32, 64 + 32).Contains(MouseHandler.MousePosition) = True Then
                _index = i
                mouseOver = True
            End If
        Next

        If Controls.Accept(True, False, True) = True Or KeyBoardHandler.KeyPressed(KeyBindings.EnterKey1) = True Then
            Select Case _index
                Case 0
                    ClickYes()
                Case 1
                    If _canChooseNo = True Then
                        ClickNo()
                    End If
            End Select
        End If

        If _askedRename = True Then
            If Controls.Right(True, True, True, False, True) = True Then
                _index = 1
            End If
            If Controls.Left(True, True, True, False, True) = True Then
                _index = 0
            End If

            If _delay > 0.0F Then
                _delay -= 0.1F

                If _delay <= 0.0F Then
                    _delay = 0.0F
                End If
            Else
                KeyBindings.GetNameInput(Me._currentText, 20)

                Me._currentText = Me.ReplaceInvalidChars(Me._currentText)

                If Controls.Dismiss(True, False, True) = True And _canChooseNo = True Then
                    ClickNo()
                End If
            End If
        Else
            If Controls.Right(True, True, True, True, True) = True Then
                _index = 1
            End If
            If Controls.Left(True, True, True, True, True) = True Then
                _index = 0
            End If
        End If
    End Sub

    ''' <summary>
    ''' This function replaces characters in the string with nothing if the characters aren't allowed in the name.
    ''' </summary>
    ''' <param name="text">The name string.</param>
    ''' <remarks>Only numbers and alphabetic characters are allowed (0-9, a-z, A-Z)</remarks>
    Private Function ReplaceInvalidChars(ByVal text As String) As String
        'Creating the char array.
        Dim chars() As Char = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()

        'Create a new string to store the "purified" text in. YOU SHALL NOT PASS, EXTENDED LATIN
        Dim newText As String = ""

        'Loop through all of the original text and only put in the allowed ones.
        For i = 0 To text.Length - 1
            If chars.Contains(text(i)) = True Then
                newText &= text(i).ToString()
            End If
        Next

        'Return the newly created string.
        Return newText
    End Function

    Private Sub ClickYes()
        If _askedRename = True Then
            If _currentText <> "" Then
                If _renamePokemon = True Then
                    If _pokemon.GetName() <> _currentText Then
                        _pokemon.NickName = _currentText
                    End If
                Else
                    Me._acceptName(_currentText)
                End If

                PokemonImageView.Showing = False
                Core.SetScreen(Me.PreScreen)
            End If
        Else
            Me._askedRename = True
            If ControllerHandler.IsConnected() = True Then
                Core.SetScreen(New InputScreen(Me, Me._defaultName, InputScreen.InputModes.Name, Me._defaultName, 14, New List(Of Texture2D), AddressOf Me.GetControllerInput))
            End If
        End If
    End Sub

    Private Sub ClickNo()
        If Me._askedRename = True Then
            Me._askedRename = False
        Else
            PokemonImageView.Showing = False
            Core.SetScreen(Me.PreScreen)
        End If
    End Sub

    Public Sub GetControllerInput(ByVal input As String)
        Me._currentText = input
    End Sub

End Class
