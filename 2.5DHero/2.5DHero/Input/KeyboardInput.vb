''' <summary>
''' Handles text input from the keyboard.
''' </summary>
Public Class KeyboardInput

    ''' <summary>
    ''' Creates a new instance of the KeyboardInput class.
    ''' </summary>
    Public Sub New()
    End Sub

#Region "TextInput"

    'Input method needed for the game:
    'ChatScreen: Latin255
    'Name player: Text
    'Name Pokémon: Text
    'Filter GTS by Pokémon name: Letters
    'GameJolt login: GameJolt specific
    'Mails: Latin255

    ''' <summary>
    ''' Modifiers for input methods.
    ''' </summary>
    Public Enum InputModifier
        AllChars = 0

        ''' <summary>
        ''' 0-9
        ''' </summary>
        Numbers = 1
        ''' <summary>
        ''' a-z and A-Z
        ''' </summary>
        Letters = 2
        ''' <summary>
        ''' Numbers and Letters combined and Space.
        ''' </summary>
        Alphanumeric = 3
        ''' <summary>
        ''' All valid characters in the first 255 unicode spaces.
        ''' </summary>
        Latin255 = 4
        ''' <summary>
        ''' All characters that are allowed as GameJolt name and token.
        ''' </summary>
        GameJolt = 5
    End Enum

    Private _holdDelay As Single = 3.0F
    Private _holdKey As Keys = Keys.A

    Private Const STARTHOLDDELAY As Single = 3.0F

    ''' <summary>
    ''' Keys to ignore when adding text to a string.
    ''' </summary>
    Private ReadOnly _ignoreKeys As Keys() = {Keys.Enter, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Escape, Keys.LeftShift, Keys.RightShift, Keys.LeftAlt, Keys.RightAlt, Keys.LeftControl, Keys.RightControl, Keys.LeftWindows, Keys.RightWindows, Keys.Delete, Keys.Home, Keys.End}

    Public Function GetInput() As String
        Return GetInput("", InputModifier.AllChars, True, False)
    End Function

    Public Function GetInput(ByVal InputModifiers As InputModifier) As String
        Return GetInput("", InputModifiers, True, False)
    End Function

    Public Function GetInput(ByRef CurrentText As String, ByVal InputModifier As InputModifier) As String
        Return GetInput(CurrentText, InputModifier, True, True)
    End Function

    Public Function GetInput(ByRef CurrentText As String, ByVal InputModifiers As InputModifier, ByVal CanPaste As Boolean, ByVal CanDelete As Boolean) As String
        Return GetInput(CurrentText, CurrentText.Length, InputModifiers, True, CanDelete)
    End Function

    Public Function GetInput(ByRef CurrentText As String, ByRef CarretPosition As Integer, ByVal InputModifiers As InputModifier, ByVal CanPaste As Boolean, ByVal CanDelete As Boolean) As String
        Dim pressedKeys As Keys() = KeyBoardHandler.GetPressedKeys()

        If pressedKeys.Count > 0 Then
            For Each k As Keys In pressedKeys
                'Check for Ctrl+V
                If k = Keys.V And KeyBoardHandler.KeyPressed(Keys.V) = True And Controls.CtrlPressed() = True Then
                    If System.Windows.Forms.Clipboard.ContainsText() = True Then
                        Dim pasteText As String = System.Windows.Forms.Clipboard.GetText().Replace(vbNewLine, "")

                        Me.AppendString(CurrentText, pasteText, CarretPosition)
                    End If
                ElseIf k = Keys.Back Or k = Keys.Delete Then
                    If CurrentText.Length > 0 And CanDelete = True Then
                        Dim isBackSpace As Boolean = (k = Keys.Back)

                        'Remove text from the string:
                        If _holdDelay <= 0F And _holdKey = k Then
                            Me.RemoveString(CurrentText, CarretPosition, isBackSpace)
                        Else
                            If KeyBoardHandler.KeyPressed(k) = True Then
                                Me.RemoveString(CurrentText, CarretPosition, isBackSpace)

                                _holdKey = k
                                _holdDelay = STARTHOLDDELAY
                            End If
                        End If
                    End If
                Else
                    'Dont paste strings, try to append normal char input:
                    If _ignoreKeys.Contains(k) = False Then
                        Dim pressedChar As Char? = KeyCharConverter.GetCharFromKey(CType(k, Windows.Forms.Keys))

                        If pressedChar.HasValue = True Then
                            Dim charString = CStr(pressedChar)

                            If _holdDelay <= 0F And _holdKey = k Then
                                Me.AppendString(CurrentText, charString, CarretPosition)
                            Else
                                If KeyBoardHandler.KeyPressed(k) = True Then
                                    Me.AppendString(CurrentText, charString, CarretPosition)

                                    _holdKey = k
                                    _holdDelay = STARTHOLDDELAY
                                End If
                            End If
                        End If
                    End If
                End If
            Next

            'Update the key hold delay:
            If KeyBoardHandler.KeyUp(_holdKey) = True Then
                _holdDelay = STARTHOLDDELAY
            Else
                If _holdDelay > 0.0F Then
                    _holdDelay -= 0.1F
                    If _holdDelay <= 0.0F Then
                        _holdDelay = 0.0F
                    End If
                End If
            End If

            'Replace invalid characters:
            If InputModifiers <> InputModifier.AllChars Then
                Dim charRange As Char() = GetCharRange(InputModifiers)

                For i = 0 To CurrentText.Length - 1
                    If i <= CurrentText.Length - 1 Then
                        Dim c As Char = CurrentText(i)
                        If charRange.Contains(c) = False Then
                            CurrentText = CurrentText.Remove(i, 1)
                            i -= 1
                        End If
                    End If
                Next
            End If
        End If

        Return CurrentText
    End Function

    Private Sub RemoveString(ByRef Text As String, ByRef CarretPosition As Integer, ByVal isBackSpace As Boolean)
        If isBackSpace = True Then
            'Back key was pressed:
            If CarretPosition > 0 Then
                Text = Text.Remove(CarretPosition - 1, 1)
                CarretPosition -= 1
            End If
        Else
            'Delete key was pressed:
            If CarretPosition < Text.Length Then
                Text = Text.Remove(CarretPosition, 1)
                CarretPosition -= 1
            End If
        End If
    End Sub

    Private Sub AppendString(ByRef Text As String, ByVal AppendText As String, ByRef CarretPosition As Integer)
        Text = Text.Insert(CarretPosition, AppendText)
        CarretPosition += AppendText.Length
    End Sub

    Private Function GetCharRange(ByVal [mod] As InputModifier) As Char()
        Dim s As String = ""

        Select Case [mod]
            Case InputModifier.Latin255
                s = " !""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~¡¢£¤¥¦§¨©ª«¬®¯°±²³´µ¶·¸¹º»¼½¾¿ÀÁÂÃÄÅÆÇÈÉÊËÌÍÎÏÐÑÒÓÔÕÖ×ØÙÚÛÜÝÞßàáâãäåæçèéêëìíîïðñòóôõö÷øùúûüýþÿ"
            Case InputModifier.Letters
                s = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
            Case InputModifier.Numbers
                s = "0123456789"
            Case InputModifier.Alphanumeric
                s = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
            Case InputModifier.GameJolt
                s = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
        End Select

        Return s.ToCharArray()
    End Function

#End Region

#Region "Textbox"

    'input from this class
    'carret jumping with customizable characters
    'deleting chars
    'capture to type
    'blacklist/whitelist for characters
    'MaxCount + make it depend on width of textbox if needed.
    'Support Ctrl+C and Ctrl+X, Ctrl+V is supported by GetInput
    'Support Ctrl+A to select all.
    'Password property
    'Pos1+End buttons

    ''' <summary>
    ''' A textbox representation to handle and display keyboard input.
    ''' </summary>
    Public Class Textbox

        Private _text As String = String.Empty

        Dim _inputHandler As New KeyboardInput()

        'Visuals:
        Private _font As SpriteFont
        Private _fontColor As Color = Color.Black
        Private _backColor As Color = Color.White
        Private _borderColor As Color = Color.Black
        Private _width As Integer = 120
        Private _height As Integer = -1
        Private _borderWidth As Integer = 1
        Private _position As Drawing.Point = New Drawing.Point(100, 100)

        'Password:
        Private _isPassword As Boolean = False
        Private _passwordChar As Char = CChar("●")

        'Characters:
        Private _carretJumpmarks As Char() = " *²³+#°',.;-:><|!""§%&/()=?{[]}\@".ToCharArray()
        Private _inputType As InputModifier = InputModifier.Latin255

        Const TESTFORHEIGHTCHARS As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz;:_-.,*~+'#1234567890?ß\/!""§$%&/()}][{"

        'Properties:
        Private _canPaste As Boolean = True
        Private _canCopyAndCut As Boolean = True
        Private _maxLength As Integer = -1

        Private _carretPosition As Integer = 0
        Private _selectionStart As Integer = 0
        Private _selectionLength As Integer = 0

        Private _isFocused As Boolean = True

        Public Property Text() As String
            Get
                Return Me._text
            End Get
            Set(value As String)
                Me._text = value
            End Set
        End Property

        Public Property IsPassword() As Boolean
            Get
                Return Me._isPassword
            End Get
            Set(value As Boolean)
                Me._isPassword = value
            End Set
        End Property

        Public Property PasswordChar() As Char
            Get
                Return Me._passwordChar
            End Get
            Set(value As Char)
                Me._passwordChar = value
            End Set
        End Property

        Public Property MaxLength() As Integer
            Get
                Return Me._maxLength
            End Get
            Set(value As Integer)
                Me._maxLength = value
            End Set
        End Property

        Public Property CanPaste() As Boolean
            Get
                Return Me._canPaste
            End Get
            Set(value As Boolean)
                Me._canPaste = value
            End Set
        End Property

        Public Property CanCopyAndCut() As Boolean
            Get
                Return Me._canCopyAndCut
            End Get
            Set(value As Boolean)
                Me._canCopyAndCut = value
            End Set
        End Property

        Public Property Width() As Integer
            Get
                Return Me._width
            End Get
            Set(value As Integer)
                Me._width = value
            End Set
        End Property

        Public Property Height As Integer
            Get
                Return Me._height
            End Get
            Set(value As Integer)
                Me._height = value
            End Set
        End Property

        Public Property CarretPosition() As Integer
            Get
                Return Me._carretPosition
            End Get
            Set(value As Integer)
                Me._carretPosition = value.Clamp(0, Me._text.Length)
            End Set
        End Property

        Public Property SelectionStart() As Integer
            Get
                Return Me._selectionStart
            End Get
            Set(value As Integer)
                Me._selectionStart = value.Clamp(0, Me._text.Length)
            End Set
        End Property

        Public Property SelectionLength() As Integer
            Get
                Return Me._selectionLength
            End Get
            Set(value As Integer)
                Me._selectionLength = value.Clamp(Me._selectionStart, Me._text.Length)
            End Set
        End Property

        Public Property InputType() As InputModifier
            Get
                Return Me._inputType
            End Get
            Set(value As InputModifier)
                Me._inputType = value
            End Set
        End Property

        Public Sub New(ByVal Font As SpriteFont)
            Me._font = Font
        End Sub

#Region "Update"

        Public Sub Update()
            If Me._isFocused = True Then
                If Controls.CtrlPressed() = True Then
                    'Select All (Ctrl + A)
                    If KeyBoardHandler.KeyPressed(Keys.A) = True Then
                        Me.SelectAll()
                    End If
                    'Copy (Ctrl + C)
                    If KeyBoardHandler.KeyPressed(Keys.C) = True Then
                        Me.Copy()
                    End If
                    'Cut (Ctrl + X)
                    If KeyBoardHandler.KeyPressed(Keys.X) = True Then
                        Me.Cut()
                    End If
                End If

                Dim beforeTextCount As Integer = Me._text.Length
                Me._inputHandler.GetInput(Me._text, InputModifier.AllChars, True, True)
                Me.CropText()
                If beforeTextCount <> Me._text.Length Then
                    Me._carretPosition += Me._text.Length - beforeTextCount
                End If

                Me.UpdateSelection()
            End If
        End Sub

        Private Sub UpdateSelection()

        End Sub

        ''' <summary>
        ''' Selects all text.
        ''' </summary>
        Public Sub SelectAll()
            Me._carretPosition = Me._text.Length
            Me._selectionStart = 0
            Me._selectionLength = Me._text.Length
        End Sub

        Public Sub Deselect()
            Me._selectionLength = 0
        End Sub

        ''' <summary>
        ''' Copies any selected text.
        ''' </summary>
        Public Sub Copy()
            If Me._selectionLength > 0 Then
                Try
                    System.Windows.Forms.Clipboard.SetText(Me.SelectedText)
                Catch ex As Exception
                    Logger.Log(Logger.LogTypes.Message, "KeyboardInput.vb: An error occurred while copying text to the clipboard.")
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Cuts any selected text.
        ''' </summary>
        Public Sub Cut()
            If Me._selectionLength > 0 Then
                Try
                    System.Windows.Forms.Clipboard.SetText(Me.SelectedText)
                    Me._text = Me._text.Remove(Me._selectionStart, Me._selectionLength)
                    Me.Deselect()
                Catch ex As Exception
                    Logger.Log(Logger.LogTypes.Message, "KeyboardInput.vb: An error occurred while copying text to the clipboard.")
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Returns the selected text.
        ''' </summary>
        Public ReadOnly Property SelectedText() As String
            Get
                If Me._selectionLength <= 0 Then
                    Return ""
                End If
                Return Me._text.Substring(Me._selectionStart, Me._selectionLength)
            End Get
        End Property

        ''' <summary>
        ''' Crops the text based on the max amount of chars.
        ''' </summary>
        Private Sub CropText()
            If Me._maxLength = -1 Then
                'We need to crop the text based on the width of the textbox here.
                While CInt(Me._font.MeasureString(Me._text).X) > Me._width
                    Me._text = Me._text.Remove(Me._text.Length - 1, 1)
                End While
            Else
                'Just crop away chars that are over the limit.
                If Me._text.Length > Me._maxLength Then
                    Me._text = Me._text.Remove(Me._maxLength)
                End If
            End If
        End Sub

#End Region

#Region "Rendering"

        Public Sub Draw()
            Dim contentHeight As Integer = Me._height
            If contentHeight < 0 Then
                contentHeight = CInt(Me._font.MeasureString(TESTFORHEIGHTCHARS).Y) + 2
            End If

            'Draw border:
            If _borderWidth > 0 Then
                Core.SpriteBatch.DrawRectangle(New Rectangle(Me._position.X, 'X
                                                             Me._position.Y, 'Y
                                                             Me._width + (Me._borderWidth * 2), 'Width
                                                             contentHeight + (Me._borderWidth * 2) 'Height
                                                             ), Me._borderColor)
            End If

            'Draw content:
            Core.SpriteBatch.DrawRectangle(New Rectangle(Me._position.X + Me._borderWidth, Me._position.X + Me._borderWidth, Me._width, contentHeight), Me._backColor)

            Core.SpriteBatch.DrawString(Me._font, Me._text, New Vector2(Me._position.X + Me._borderWidth, Me._position.Y + Me._borderWidth + 1), Me._fontColor)

            'Draw carret and selection:
            Dim carretDrawPosition As Integer = 0
            If Me._carretPosition = Me._text.Length Then
                carretDrawPosition = CInt(Me._font.MeasureString(Me._text).X)
            Else
                carretDrawPosition = CInt(Me._font.MeasureString(Me._text.Remove(Me._carretPosition)).X)
            End If
            Core.SpriteBatch.DrawLine(New Vector2(Me._position.X + carretDrawPosition + 1, Me._position.Y), New Vector2(Me._position.X + carretDrawPosition + 1, Me._position.Y + contentHeight), 1.0F, Color.Green)
        End Sub

#End Region

#Region "Focus"

        Public Sub Focus()
            Me._isFocused = True
        End Sub

        Public Sub DeFocus()
            Me._isFocused = False

            Me._selectionStart = 0
            Me._selectionLength = 0
        End Sub

        Public Function IsFocused() As Boolean
            Return Me._isFocused
        End Function

#End Region

    End Class

#End Region

End Class
