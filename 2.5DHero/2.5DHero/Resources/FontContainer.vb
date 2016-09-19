''' <summary>
''' Class that contains a SpriteFont with its corresponding name.
''' </summary>
Public Class FontContainer

    Private _spriteFont As SpriteFont
    Private _fontName As String

    ''' <summary>
    ''' Creates a new instance of the FontContainer class.
    ''' </summary>
    ''' <param name="FontName">The name of the Font.</param>
    ''' <param name="Font">The SpriteFont.</param>
    Public Sub New(ByVal FontName As String, ByVal Font As SpriteFont)
        Me._fontName = FontName
        Me._spriteFont = Font

        Select Case FontName.ToLower()
            Case "braille"
                Me._spriteFont.DefaultCharacter = CChar(" ")
            Case Else
                Me._spriteFont.DefaultCharacter = CChar("?")
        End Select
    End Sub

    ''' <summary>
    ''' Returns the name of the Font.
    ''' </summary>
    Public ReadOnly Property FontName() As String
        Get
            Return Me._fontName
        End Get
    End Property

    ''' <summary>
    ''' The SpriteFont.
    ''' </summary>
    Public ReadOnly Property SpriteFont() As SpriteFont
        Get
            Return Me._spriteFont
        End Get
    End Property

End Class
