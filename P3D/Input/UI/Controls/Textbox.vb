Namespace UI.GameControls

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

        Inherits Control

        Dim _inputHandler As New KeyboardInput()

        'Password:
        Private _isPassword As Boolean = False
        Private _passwordChar As Char = CChar("●")

        'Characters:
        Private _carretJumpmarks As Char() = " *²³+#°',.;-:><|!""§%&/()=?{[]}\@".ToCharArray()
        Private _inputType As KeyboardInput.InputModifier = KeyboardInput.InputModifier.Latin255

        'Properties:
        Private _canPaste As Boolean = True
        Private _canCopyAndCut As Boolean = True
        Private _maxLength As Integer = -1

        Private _horizontalTextPadding As Integer = 1
        Private _verticalTextPadding As Integer = 1

        Private _carretPosition As Integer = 0
        Private _selectionStart As Integer = 0
        Private _selectionLength As Integer = 0

        Public Property IsPassword() As Boolean
            Get
                Return _isPassword
            End Get
            Set(value As Boolean)
                _isPassword = value
            End Set
        End Property

        Public Property PasswordChar() As Char
            Get
                Return _passwordChar
            End Get
            Set(value As Char)
                _passwordChar = value
            End Set
        End Property

        Public Property MaxLength() As Integer
            Get
                Return _maxLength
            End Get
            Set(value As Integer)
                _maxLength = value
            End Set
        End Property

        Public Property CanPaste() As Boolean
            Get
                Return _canPaste
            End Get
            Set(value As Boolean)
                _canPaste = value
            End Set
        End Property

        Public Property CanCopyAndCut() As Boolean
            Get
                Return _canCopyAndCut
            End Get
            Set(value As Boolean)
                _canCopyAndCut = value
            End Set
        End Property

        Public Property CarretPosition() As Integer
            Get
                Return _carretPosition
            End Get
            Set(value As Integer)
                _carretPosition = value.Clamp(0, Text.Length)
            End Set
        End Property

        Public Property SelectionStart() As Integer
            Get
                Return _selectionStart
            End Get
            Set(value As Integer)
                _selectionStart = value.Clamp(0, Text.Length)
            End Set
        End Property

        Public Property SelectionLength() As Integer
            Get
                Return _selectionLength
            End Get
            Set(value As Integer)
                _selectionLength = value.Clamp(_selectionStart, Text.Length)
            End Set
        End Property

        Public Property InputType() As KeyboardInput.InputModifier
            Get
                Return _inputType
            End Get
            Set(value As KeyboardInput.InputModifier)
                _inputType = value
            End Set
        End Property

        Public Property HorizonzalTextPadding() As Integer
            Get
                Return _horizontalTextPadding
            End Get
            Set(value As Integer)
                _horizontalTextPadding = value
            End Set
        End Property

        Public Property VerticalTextPadding() As Integer
            Get
                Return _verticalTextPadding
            End Get
            Set(value As Integer)
                _verticalTextPadding = value
            End Set
        End Property

        Public Sub New(ByVal screenInstance As Screen, ByVal font As SpriteFont)
            MyBase.New(screenInstance)

            Me.Font = font
        End Sub

#Region "Update"

        Protected Overrides Sub UpdateClient()
            If IsFocused = True Then
                If Controls.CtrlPressed() = True Then
                    'Select All (Ctrl + A)
                    If KeyBoardHandler.KeyPressed(Keys.A) = True Then
                        SelectAll()
                    End If
                    'Copy (Ctrl + C)
                    If KeyBoardHandler.KeyPressed(Keys.C) = True Then
                        Copy()
                    End If
                    'Cut (Ctrl + X)
                    If KeyBoardHandler.KeyPressed(Keys.X) = True Then
                        Cut()
                    End If
                End If

                Dim beforeTextCount As Integer = Text.Length
                Dim beforeTextCarret As Integer = _carretPosition

                If _selectionLength > 0 Then
                    Dim testStr = _inputHandler.GetInput("w", 1, _inputType, True, True)
                    If testStr.Length <> 1 Then
                        Text = Text.Remove(SelectionStart, SelectionLength - SelectionStart)
                        _carretPosition = SelectionStart
                        SelectionLength = -1
                        _inputHandler.GetInput(Text, _carretPosition, _inputType, True, True)
                    End If
                Else
                    _inputHandler.GetInput(Text, _carretPosition, _inputType, True, True)
                End If

                If beforeTextCount = Text.Length Then
                    _carretPosition = beforeTextCarret
                End If

                If ControllerHandler.ButtonPressed(Buttons.A) Then
                    SetScreen(New InputScreen(CurrentScreen, "", InputScreen.InputModes.Name, Text, _maxLength, New List(Of Texture2D), AddressOf ControllerInputCallback))
                End If

                CropText()

                UpdateSelection()
            End If

            _carretPosition = _carretPosition.Clamp(0, Text.Length)
        End Sub

        Private Sub UpdateSelection()
            If Controls.Left(True, True, False, False, True, True) Then
                Dim carretJumpTo = _carretPosition - 1
                If KeyBoardHandler.KeyDown(Keys.LeftControl) Then
                    ' Find next jumpmark to the left:
                    Dim jumpTo As Integer = 0
                    For i = 0 To _carretPosition - 1
                        If _carretJumpmarks.Contains(Text(i)) Then
                            jumpTo = i
                        End If
                    Next
                    carretJumpTo = jumpTo
                End If

                If SelectionLength > 0 Then
                    Dim diff = carretJumpTo - _carretPosition

                Else
                    _carretPosition = carretJumpTo
                End If
            End If
            If Controls.Right(True, True, False, False, True, True) Then
                If KeyBoardHandler.KeyDown(Keys.LeftControl) Then
                    ' Find next jumpmark to the right:
                    Dim jumpTo As Integer = Text.Length
                    For i = _carretPosition To Text.Length - 1
                        If _carretJumpmarks.Contains(Text(i)) Then
                            jumpTo = i
                            Exit For
                        End If
                    Next
                    _carretPosition = jumpTo + 1
                Else
                    _carretPosition += 1
                End If
            End If
        End Sub

        Private Sub ControllerInputCallback(ByVal result As String)
            Text = result
            _selectionLength = 0
            _selectionStart = 0
            _carretPosition = Text.Length

            CropText()
            UpdateSelection()
        End Sub

        Protected Overrides Function GetClientRectangle() As Rectangle
            Dim contentHeight As Integer = Height
            If contentHeight < 0 Then
                contentHeight = CInt(Font.MeasureString(TESTFORHEIGHTCHARS).Y * FontSize) + 2
            End If

            Return New Rectangle(Position.X, 'X
                                            Position.Y, 'Y
                                            Width + (BorderWidth * 2), 'Width
                                            contentHeight + (BorderWidth * 2) 'Height
                                            )
        End Function

        ''' <summary>
        ''' Selects all text.
        ''' </summary>
        Public Sub SelectAll()
            _carretPosition = Text.Length
            _selectionStart = 0
            _selectionLength = Text.Length
        End Sub

        Public Sub Deselect()
            _selectionLength = 0
        End Sub

        ''' <summary>
        ''' Copies any selected text.
        ''' </summary>
        Public Sub Copy()
            If _selectionLength > 0 Then
                Try
                    Windows.Forms.Clipboard.SetText(SelectedText)
                Catch ex As Exception
                    Logger.Log(Logger.LogTypes.Message, "KeyboardInput.vb: An error occurred while copying text to the clipboard.")
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Cuts any selected text.
        ''' </summary>
        Public Sub Cut()
            If _selectionLength > 0 Then
                Try
                    Windows.Forms.Clipboard.SetText(SelectedText)
                    Text = Text.Remove(_selectionStart, _selectionLength)
                    Deselect()
                Catch ex As Exception
                    Logger.Log(Logger.LogTypes.Message, "KeyboardInput.vb: An error occurred while copying text to the clipboard.")
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Returns the selected text.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SelectedText() As String
            Get
                If _selectionLength <= 0 Then
                    Return ""
                End If
                Return Text.Substring(_selectionStart, _selectionLength)
            End Get
        End Property

        ''' <summary>
        ''' Crops the text based on the max amount of chars.
        ''' </summary>
        Private Sub CropText()
            If _maxLength = -1 Then
                'We need to crop the text based on the width of the textbox here.
                While CInt(Font.MeasureString(Text).X * FontSize) > Width - _horizontalTextPadding * 2
                    Text = Text.Remove(Text.Length - 1, 1)
                End While
            Else
                'Just crop away chars that are over the limit.
                If Text.Length > _maxLength Then
                    Text = Text.Remove(_maxLength)
                End If
            End If
        End Sub

#End Region

#Region "Rendering"

        Protected Overrides Sub DrawClient()
            Dim contentHeight As Integer = Height
            If contentHeight < 0 Then
                contentHeight = CInt(Font.MeasureString(TESTFORHEIGHTCHARS).Y * FontSize) + 2
            End If

            'Draw border:
            If BorderWidth > 0 Then
                SpriteBatch.DrawRectangle(New Rectangle(Position.X, 'X
                                                             Position.Y, 'Y
                                                             Width + (BorderWidth * 2), 'Width
                                                             contentHeight + (BorderWidth * 2) 'Height
                                                             ), BorderColor)
            End If

            'Draw content:
            SpriteBatch.DrawRectangle(New Rectangle(Position.X + BorderWidth, Position.Y + BorderWidth, Width, contentHeight), BackColor)

            Dim drawText As String = Text
            If _isPassword = True Then
                drawText = ""
                For i = 0 To Text.Length - 1
                    drawText &= _passwordChar.ToString()
                Next
            End If

            FontRenderer.DrawString(Font, drawText, New Vector2(Position.X + BorderWidth + _verticalTextPadding, Position.Y + BorderWidth + 1 + _horizontalTextPadding), FontColor, 0F, Vector2.Zero, FontSize, SpriteEffects.None, 0F)

            If IsFocused = True Then
                'Draw carret and selection:
                ' Carret
                Dim carretDrawPosition As Integer = 0
                Dim textSize = Font.MeasureString(drawText)

                If textSize.Y <= 0F Then
                    textSize.Y = Font.MeasureString(TESTFORHEIGHTCHARS).Y
                End If

                If _carretPosition = Text.Length Then
                    carretDrawPosition = CInt(textSize.X * FontSize)
                Else
                    carretDrawPosition = CInt(Font.MeasureString(drawText.Remove(_carretPosition)).X * FontSize)
                End If
                SpriteBatch.DrawLine(New Vector2(Position.X + carretDrawPosition + CInt(Math.Ceiling(FontSize)) + _verticalTextPadding + BorderWidth, Position.Y + BorderWidth + _horizontalTextPadding),
                                     New Vector2(Position.X + carretDrawPosition + CInt(Math.Ceiling(FontSize)) + _verticalTextPadding + BorderWidth, Position.Y + textSize.Y * FontSize + BorderWidth + _horizontalTextPadding), 2.0F, BackColor.Invert())

                ' Selection:

                Dim startPoint = SelectionStart
                Dim endPoint = SelectionStart + SelectionLength

                If SelectionLength < 0 Then
                    startPoint = SelectionStart + SelectionLength
                    endPoint = SelectionStart
                End If

                SpriteBatch.DrawRectangle(New Rectangle(Position.X + _verticalTextPadding + BorderWidth + startPoint * 10, Position.Y + BorderWidth + _horizontalTextPadding, (endPoint - startPoint) * 10, CInt(textSize.Y * FontSize)), New Color(0, 122, 230, 100))
            End If
        End Sub

#End Region

    End Class

End Namespace