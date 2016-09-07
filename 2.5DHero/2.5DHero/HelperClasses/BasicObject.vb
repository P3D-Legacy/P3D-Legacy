''' <summary>
''' This is the BasicObject class for each Graphics-Component.
''' Version: 1.0.0.2 (17.07.2012)
''' </summary>
''' <remarks></remarks>
Public MustInherit Class BasicObject

#Region "Fields"

    Private _Texture As Texture2D
    Private _Width As Integer
    Private _Height As Integer
    Private _Position As Vector2
    Private _Visible As Boolean = True
    Private _DisposeReady As Boolean = False

    ''' <summary>
    ''' Use the random-function to create randomized values.
    ''' </summary>
    ''' <remarks></remarks>
    Public Random As New System.Random()

    ''' <summary>
    ''' You can store a value in the Tag.
    ''' </summary>
    ''' <remarks></remarks>
    Public Tag As New Object

#End Region

#Region "Properties"

    ''' <summary>
    ''' The visual texture of the object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DisposeReady As Boolean
        Get
            Return _DisposeReady
        End Get
        Set(value As Boolean)
            _DisposeReady = value
        End Set
    End Property

    ''' <summary>
    ''' Returns a rectangle created by position, width and height of this object.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Rectangle As Rectangle
        Get
            Return New Rectangle(CInt(Position.X), CInt(Position.Y), Width, Height)
        End Get
        Set(value As Rectangle)
            Me.Position = New Vector2(value.X, value.Y)
            Me.Width = value.Width
            Me.Height = value.Height
        End Set
    End Property

    ''' <summary>
    ''' The size of this object (width and height)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
    ''' <remarks></remarks>
    Public Sub New(ByVal Texture As Texture2D, ByVal Width As Integer, ByVal Height As Integer, ByVal Position As Vector2)
        Me._Texture = Texture
        Me._Width = Width
        Me._Height = Height
        Me._Position = Position
    End Sub

    ''' <summary>
    ''' Makes the object invisible and ready for disposing.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Remove()
        DisposeReady = True
        Visible = False
    End Sub

End Class