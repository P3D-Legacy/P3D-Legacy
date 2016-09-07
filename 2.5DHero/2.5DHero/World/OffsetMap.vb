''' <summary>
''' Represents an Offset Map to be stored by the LevelLoader.
''' </summary>
Public Class OffsetMap

    ''' <summary>
    ''' The identifier of this offset map, which contains Weather, Time and Season.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Identifier() As String

    ''' <summary>
    ''' The name of the file relative to *\maps\ with extension.
    ''' Like Level.vb-LevelFile
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property MapName() As String

    ''' <summary>
    ''' If this map got loaded.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Loaded() As Boolean = False

    ''' <summary>
    ''' The list of entities.
    ''' Only filled if LoadMap was performed.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Entities() As List(Of Entity) = Nothing

    ''' <summary>
    ''' The list of floors.
    ''' Only filled if LoadMap was performed.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Floors() As List(Of Entity) = Nothing

    ''' <summary>
    ''' Creates a new instance of the OffsetMap class.
    ''' </summary>
    ''' <param name="MapName"></param>
    Public Sub New(ByVal MapName As String)
        _MapName = MapName

        'Set the identifier:
        '             Offset map       Map weather                                  Region weather                          Time                    Season
        _Identifier = _MapName & "|" & Screen.Level.World.CurrentMapWeather & "|" & World.GetCurrentRegionWeather() & "|" & World.GetTime() & "|" & World.CurrentSeason()
    End Sub

    ''' <summary>
    ''' Loads the offset map.
    ''' </summary>
    Public Sub LoadMap(ByVal Offset As Vector3)
        _Loaded = True

    End Sub

    Public Sub ApplyToLevel(ByVal Level As Level)

    End Sub

End Class