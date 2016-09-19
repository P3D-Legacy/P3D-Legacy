''' <summary>
''' Defines and handles terrain definitions for a map.
''' </summary>
Public Class Terrain

#Region "Enums"

    ''' <summary>
    ''' A terrain type defined in the Terrain tag of a map.
    ''' </summary>
    Public Enum TerrainTypes
        Plain
        Sand
        Cave
        Rock
        TallGrass
        LongGrass
        PondWater
        SeaWater
        Underwater
        DisortionWorld
        Puddles
        Snow
        Magma
        PvPBattle
    End Enum

#End Region

#Region "Fields and Constants"

    Private _terrainType As TerrainTypes = TerrainTypes.Plain

#End Region

#Region "Properties"

    ''' <summary>
    ''' The terrain type of this Terrain instance.
    ''' </summary>
    Public Property TerrainType() As TerrainTypes
        Get
            Return Me._terrainType
        End Get
        Set(value As TerrainTypes)
            Me._terrainType = value
        End Set
    End Property

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Creates a new instance of the Terrain class and sets an initial TerrainType.
    ''' </summary>
    ''' <param name="InitialTerrainType">The TerrainType for this instance.</param>
    Public Sub New(ByVal InitialTerrainType As TerrainTypes)
        Me._terrainType = InitialTerrainType
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Converts a Terrain name to the correct Terrain class instance.
    ''' </summary>
    ''' <param name="input">The Terrain name.</param>
    Public Shared Function FromString(ByVal input As String) As Terrain.TerrainTypes
        Select Case input.ToLower()
            Case "plain"
                Return TerrainTypes.Plain
            Case "sand"
                Return TerrainTypes.Sand
            Case "cave"
                Return TerrainTypes.Cave
            Case "rock"
                Return TerrainTypes.Rock
            Case "tallgrass"
                Return TerrainTypes.TallGrass
            Case "longgrass"
                Return TerrainTypes.LongGrass
            Case "pondwater"
                Return TerrainTypes.PondWater
            Case "seawater"
                Return TerrainTypes.SeaWater
            Case "underwater"
                Return TerrainTypes.Underwater
            Case "disortionworld"
                Return TerrainTypes.DisortionWorld
            Case "puddles"
                Return TerrainTypes.Puddles
            Case "snow"
                Return TerrainTypes.Snow
            Case "magma"
                Return TerrainTypes.Magma
            Case "pvp"
                Return TerrainTypes.PvPBattle
        End Select

        'Default terrain:
        Logger.Log(Logger.LogTypes.Warning, "Terrain.vb: Invalid terrain name: """ & input & """. Returning ""Plains"".")
        Return TerrainTypes.Plain
    End Function

#End Region

    ''' <summary>
    ''' Test for TerrainType equality.
    ''' </summary>
    ''' <param name="value1">The first Terrain instance.</param>
    ''' <param name="value2">The second Terrain instance.</param>
    Public Shared Operator =(ByVal value1 As Terrain, ByVal value2 As Terrain) As Boolean
        Return value1.TerrainType = value2.TerrainType
    End Operator

    ''' <summary>
    ''' Test for TerrainType inequality.
    ''' </summary>
    ''' <param name="value1">The first Terrain instance.</param>
    ''' <param name="value2">The second Terrain instance.</param>
    Public Shared Operator <>(ByVal value1 As Terrain, ByVal value2 As Terrain) As Boolean
        Return Not value1 = value2
    End Operator

End Class
