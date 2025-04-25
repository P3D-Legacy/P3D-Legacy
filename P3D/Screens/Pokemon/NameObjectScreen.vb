''' <summary>
''' A class to name Pokémon and other objects (namely the rival).
''' </summary>
''' <remarks>Inherits from screen.</remarks>
Public Class NameObjectScreen

    Inherits Screen

    Private _pokemon As Pokemon ' Temporarly stores the Pokémon to rename.
    Private _currentText As String = "" ' The current Text in the textbox.
    Private _mainTexture As Texture2D ' The temporary texture. Loads "GUI\Menus\Menu".
    Private _maxLength As Integer = 20

    Private _index As Integer = 0 ' The button index (0 or 1).
    Private _askedRename As Boolean = False ' If the question to rename is answered or not.
    Private _renamePokemon As Boolean = False ' If a Pokémon is getting renamed.

    Private _canChooseNo As Boolean = True ' If the player can choose to not rename the object.
    Private _defaultName As String = "Name" ' The default name (that also gets mentioned in the question).
    Private _delay As Single = 0.0F ' The delay until the question can be answered.

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
        ' Set default values:
        Me.Identification = Identifications.NameObjectScreen
        Me.PreScreen = CurrentScreen
        Me.MouseVisible = True
        Me.CanChat = False
        Me.CanMuteAudio = False
        Me.CanBePaused = False
        Me._canChooseNo = True
        Me._pokemon = Pokemon
        Me._defaultName = Pokemon.GetDisplayName()
        Me._renamePokemon = True
        Me._maxLength = 12

        ' Load texture:
        Me._mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        ' Show the Pokémon image:
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
        Me.CanMuteAudio = False
        Me._maxLength = 20

        Screen.PokemonImageView.Show(Texture)
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()
        Canvas.DrawRectangle(Core.windowSize, New Color(0, 0, 0, 150))

        Dim CanvasTexture As Texture2D
        For i = 0 To 1
            Dim Text As String = Localization.GetString("rename_screen_button_Rename", "Rename")
            If _askedRename = True Then
                Text = Localization.GetString("global_ok", "OK")
            End If

            If i = 1 Then
                Text = Localization.GetString("global_cancel", "Cancel")
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
            If _renamePokemon = True Then
                If _pokemon.Gender = Pokemon.Genders.Male Then
                    genderString = " ♂"
                ElseIf _pokemon.Gender = Pokemon.Genders.Female Then
                    genderString = " ♀"
                End If
            End If

            Dim TitleText As String = Localization.GetString("rename_screen_title_Question", "Rename [NAME]?").Replace("[NAME]", Me._defaultName & genderString)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, TitleText, New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(TitleText).X / 2) + 16 + 2, 96 + 2), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, TitleText, New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(TitleText).X / 2) + 16, 96), Color.White)

            ChooseBox.Showing = False
        Else
            If _delay = 0.0F Then
                Dim genderString As String = ""
                Dim genderUnicode As Integer = 0
                If _renamePokemon = True Then
                    If _pokemon.Gender = Pokemon.Genders.Male Then
                        genderString = " ♂"
                    ElseIf _pokemon.Gender = Pokemon.Genders.Female Then
                        genderString = " ♀"
                    End If
                End If
                Dim TitleText As String = Localization.GetString("rename_screen_title_EnterName", "Enter name for [NAME]:").Replace("[NAME]", Me._defaultName & genderString)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, TitleText, New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(TitleText).X / 2) + 16 + 2, 96 + 2), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, TitleText, New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(TitleText).X / 2) + 16, 96), Color.White)

                Canvas.DrawRectangle(New Rectangle(CInt(TextboxPosition().X) - 4, CInt(TextboxPosition().Y) - 4, 320 + 8, 32), New Color(101, 142, 255))
                DrawTextBox()
            End If
        End If

        PokemonImageView.Draw()
        ImageView.Draw()
    End Sub

    Private Function TextboxPosition() As Vector2
        Return New Vector2(CInt(Core.windowSize.Width / 2) - 160 + 16, 140)
    End Function

    Private Sub DrawTextBox()
        Canvas.DrawRectangle(New Rectangle(CInt(TextboxPosition().X), CInt(TextboxPosition().Y), 320, 24), Color.White)

        Dim t As String = Me._currentText
        If t.Length < Me._maxLength Then
            t &= "_"
        End If
        Core.SpriteBatch.DrawString(FontManager.InGameFont, t, TextboxPosition(), Color.Black)
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
                    SoundManager.PlaySound("select")
                    ClickYes()
                Case 1
                    If _canChooseNo = True Then
                        SoundManager.PlaySound("select")
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
                If _pokemon IsNot Nothing Then
                    KeyBindings.GetNameInput(Me._currentText, 12)
                Else
                    KeyBindings.GetNameInput(Me._currentText, 20)
                End If

                Me._currentText = Me.ReplaceInvalidChars(Me._currentText)

                If Controls.Dismiss(True, False, True) = True And _canChooseNo = True Then
                    SoundManager.PlaySound("select")
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
        ' Creating the char array:
        Dim chars() As Char = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ ".ToCharArray()

        ' Create a new string to store the "purified" text in. YOU SHALL NOT PASS, EXTENDED LATIN.
        Dim newText As String = ""

        ' Loop through all of the original text and only put in the allowed ones.
        For i = 0 To text.Length - 1
            If chars.Contains(text(i)) = True Then
                newText &= text(i).ToString()
            End If
        Next

        ' Return the newly created string.
        Return newText
    End Function

    Private Sub ClickYes()
        If _askedRename = True Then
            ' Remove spaces at the start
            While _currentText.StartsWith(" ")
                _currentText = _currentText.Remove(0, 1)
            End While
            ' Remove spaces at the end
            While _currentText.EndsWith(" ")
                _currentText = _currentText.Remove(_currentText.Length - 1, 1)
            End While

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
                If _pokemon IsNot Nothing Then
                    Core.SetScreen(New InputScreen(Me, Me._defaultName, InputScreen.InputModes.Pokemon, Me._defaultName, 12, New List(Of Texture2D), AddressOf Me.GetControllerInput))
                Else
                    Core.SetScreen(New InputScreen(Me, Me._defaultName, InputScreen.InputModes.Name, Me._defaultName, 20, New List(Of Texture2D), AddressOf Me.GetControllerInput))
                End If
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
        Me._currentText = ReplaceInvalidChars(input)
    End Sub

End Class