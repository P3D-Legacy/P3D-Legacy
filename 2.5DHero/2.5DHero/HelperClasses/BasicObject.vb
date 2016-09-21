''' <summary>
''' This is the BasicObject class for each Graphics-Component.
''' </summary>
Public MustInherit Class BasicObject

#Region "Fields"

    Private _Texture As Texture2D
    Private _Width As Integer
    Private _Height As Integer
    Private _Position As Vector2
    Private _Visible As Boolean = True
    Private _DisposeReady As Boolean = False

#End Region

#Region "Properties"

    ''' <summary>
    ''' The visual texture of the object.
    ''' </summary>
    Public Property Texture As Texture2D
        Get
            Return _Texture
        End Get
        Set(value As Texture2D)
            _Texture = value
        End Set
    End Property

    ''' <summary>
    ''' Height of the object.
    ''' </summary>
    Public Property Height As Integer
        Get
            Return _Height
        End Get
        Set(value As Integer)
            _Height = value
        End Set
    End Property

    ''' <summary>
    ''' Width of the object.
    ''' </summary>
    Public Property Width As Integer
        Get
            Return _Width
        End Get
        Set(value As Integer)
            _Width = value
        End Set
    End Property

    ''' <summary>
    ''' The position if the object in the window in pixels
    ''' </summary>
    Public Property Position As Vector2
        Get
            Return _Position
        End Get
        Set(value As Vector2)
            _Position = value
        End Set
    End Property

    ''' <summary>
    ''' A visible parameter which can be used to toggle object's visibility.
    ''' </summary>
    Public Property Visible As Boolean
        Get
            Return _Visible
        End Get
        Set(value As Boolean)
            _Visible = value
        End Set
    End Property

    ''' <summary>
    ''' Shows that you can dispose this object in the next Unload-Method.
    ''' </summary>
    Public Property DisposeReady As Boolean
        Get
            Return _DisposeReady
        End Get
        Set(value As Boolean)
            _DisposeReady = value
        End Set
    End Property

    ''' <summary>
    ''' The size of this object (width and height)
    ''' </summary>
    Public Property Size As Size
        Get
            Return New Size(Me.Width, Me.Height)
        End Get
        Set(value As Size)
            Me.Width = value.Width
            Me.Height = value.Height
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Creates a new instance of this basic calss.
    ''' </summary>
    ''' <param name="Texture">The texture for this object.</param>
    ''' <param name="Width">The width  of this object (x-axis)</param>
    ''' <param name="Height">The height of this object (y-axis)</param>
    ''' <param name="Position">The position of this object.</param>
    Public Sub New(ByVal Texture As Texture2D, ByVal Width As Integer, ByVal Height As Integer, ByVal Position As Vector2)
        Me._Texture = Texture
        Me._Width = Width
        Me._Height = Height
        Me._Position = Position
    End Sub

End Class