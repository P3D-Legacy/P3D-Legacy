''' <summary>
''' A class which can display an animation.
''' </summary>
Public Class Animation

    Inherits BasicObject

#Region "Enumerations"

    Public Enum PlayMode
        Playing
        Stopped
        Paused
    End Enum

#End Region

#Region "Fields"

    Private _TotalElapsed As Single

    Private _Rows As Integer
    Private _Columns As Integer
    Private _AnimationSpeed As Single

    Private _CurrentRow As Integer
    Private _CurrentColumn As Integer
    Private _StartRow As Integer
    Private _StartColumn As Integer

    Private _Running As PlayMode = PlayMode.Playing

#End Region

#Region "Propertys"

    Public Property totalElapsed As Single
        Get
            Return _TotalElapsed
        End Get
        Set(value As Single)
            _TotalElapsed = value
        End Set
    End Property

    Public Property Rows As Integer
        Get
            Return _Rows
        End Get
        Set(value As Integer)
            _Rows = value
        End Set
    End Property

    Public Property Columns As Integer
        Get
            Return _Columns
        End Get
        Set(value As Integer)
            _Columns = value
        End Set
    End Property

    Public Property AnimationSpeed As Single
        Get
            Return _AnimationSpeed
        End Get
        Set(value As Single)
            _AnimationSpeed = value
        End Set
    End Property

    Public Property CurrentRow As Integer
        Get
            Return _CurrentRow
        End Get
        Set(value As Integer)
            _CurrentRow = value
        End Set
    End Property

    Public Property CurrentColumn As Integer
        Get
            Return _CurrentColumn
        End Get
        Set(value As Integer)
            _CurrentColumn = value
        End Set
    End Property

    Public Property StartRow As Integer
        Get
            Return _StartRow
        End Get
        Set(value As Integer)
            _StartRow = value
        End Set
    End Property

    Public Property StartColumn As Integer
        Get
            Return _StartColumn
        End Get
        Set(value As Integer)
            _StartColumn = value
        End Set
    End Property

    Public ReadOnly Property TextureRectangle() As Rectangle
        Get
            Return New Rectangle(CurrentColumn * Width, CurrentRow * Height, Width, Height)
        End Get
    End Property

    Public ReadOnly Property Running As PlayMode
        Get
            Return _Running
        End Get
    End Property

#End Region

    Public Sub New(ByVal texture As Texture2D, ByVal rows As Integer, ByVal columns As Integer, ByVal width As Integer, ByVal height As Integer, ByVal animationSpeed As Integer, ByVal startRow As Integer, ByVal startColumn As Integer)

        MyBase.New(texture, width, height, New Vector2(0, 0))

        Me.Rows = rows
        Me.Columns = columns
        Me.AnimationSpeed = CSng(1 / animationSpeed)

        totalElapsed = 0
        CurrentRow = startRow
        CurrentColumn = startColumn
        Me.StartRow = startRow
        Me.StartColumn = startColumn
    End Sub

    Public Overloads Sub Update(ByVal elapsed As Single)
        If Running = PlayMode.Playing Then
            totalElapsed += elapsed
            If totalElapsed > AnimationSpeed Then
                totalElapsed -= AnimationSpeed

                CurrentColumn += 1
                If CurrentColumn >= Columns Then
                    CurrentRow += 1
                    CurrentColumn = StartColumn

                    If CurrentRow >= Rows Then
                        CurrentRow = StartRow
                    End If
                End If
            End If
        End If
    End Sub

#Region "Controls"

    ''' <summary>
    ''' Starts the animation.
    ''' </summary>
    Public Sub Start()
        _Running = PlayMode.Playing
    End Sub

    ''' <summary>
    ''' Stops the animation and returns to start.
    ''' </summary>
    Public Sub [Stop]()
        _Running = PlayMode.Stopped
        CurrentRow = StartRow
        CurrentColumn = StartColumn
    End Sub

    ''' <summary>
    ''' Returns to start and starts the animation afterwards.
    ''' </summary>
    Public Sub Restart()
        _Running = PlayMode.Playing
        CurrentRow = StartRow
        CurrentColumn = StartColumn
    End Sub

    ''' <summary>
    ''' Pauses the animation.
    ''' </summary>
    Public Sub Pause()
        _Running = PlayMode.Paused
    End Sub

#End Region

End Class