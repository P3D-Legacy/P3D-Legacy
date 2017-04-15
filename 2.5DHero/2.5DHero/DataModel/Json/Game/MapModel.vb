Imports System.Runtime.Serialization

Namespace DataModel.Json.Game

    <DataContract>
    Class MapModel

        Inherits JsonDataModel

        <DataMember(Order:=0)>
        Public Name As String

        <DataMember(Order:=1)>
        Public Region As String

        <DataMember(Order:=2)>
        Public Song As String

        <DataMember(Order:=3)>
        Public CanTeleport As Boolean

        <DataMember(Order:=4)>
        Public CanDig As Boolean

        <DataMember(Order:=5)>
        Public CanFly As Boolean

        <DataMember(Order:=6)>
        Public MapScriptBinding As String

        <DataMember(Order:=7)>
        Public EnvironmentType As Integer

        <DataMember(Order:=8)>
        Public WeatherType As Integer

        <DataMember(Order:=9)>
        Public LightType As Integer

        <DataMember(Order:=10)>
        Public RideType As Integer

        <DataMember(Order:=11)>
        Public IsDark As Boolean

        <DataMember(Name:="Terrain", Order:=12)>
        Private TerrainStr As String

        Public Property Terrain() As Terrain.TerrainTypes
            Get
                Return ConvertStringToEnum(Of Terrain.TerrainTypes)(TerrainStr)
            End Get
            Set(value As Terrain.TerrainTypes)
                TerrainStr = value.ToString()
            End Set
        End Property

        <DataMember(Order:=13)>
        Public IsSafariZone As Boolean

        <DataMember(Order:=14)>
        Public IsBugCatchingContest As Boolean

        <DataMember(Order:=15)>
        Public AllowedRadioChannels As Decimal()

        <DataMember(Order:=16)>
        Public BattleMapData As BattleMapDataModel

        <DataMember(Order:=17)>
        Public WildPokemonFloor As Boolean

        <DataMember(Order:=18)>
        Public ShowOverworldPokemon As Boolean

        <DataMember(Order:=19)>
        Public HiddenAbilityChance As Integer

        <DataMember(Order:=20)>
        Public Entities As EntityFieldModel()

        <DataMember(Order:=21)>
        Public NPCs As NPCModel()

        <DataMember(Order:=22)>
        Public OffsetMaps As OffsetMapModel()

        <DataMember(Order:=23)>
        Public Shaders As ShaderModel()

        <DataMember(Order:=24)>
        Public Backdrops As BackdropModel()

        <DataMember(Order:=25)>
        Public Structures As StructureModel()

        <DataContract>
        Class BattleMapDataModel

            <DataMember>
            Public BattleMapfile As String

            <DataMember>
            Public StartPosition As Vector3Model

        End Class

        <DataContract>
        Class EntityFieldModel

            <DataMember(Order:=0)>
            Public Placing As EntityFieldPositioningModel()

            <DataMember(Order:=1)>
            Public Entity As EntityModel

            Public Class EntityFieldPositioningModel

                <DataMember(Order:=0)>
                Public Position As Vector3Model

                <DataMember(Order:=1)>
                Public Size As Vector3Model

                <DataMember(Order:=2)>
                Public Fill As Boolean

                <DataMember(Order:=3)>
                Public Steps As Vector3Model

            End Class

        End Class

    End Class

    <DataContract>
    Class EntityModel

        Inherits JsonDataModel

        <DataMember(Order:=0)>
        Public Id As Integer

        <DataMember(Order:=1)>
        Public Rotation As Vector3Model

        <DataMember(Order:=2)>
        Public TakeFullRotation As Boolean

        <DataMember(Order:=3)>
        Public Scale As Vector3Model

        <DataMember(Order:=4)>
        Public RenderMode As EntityRenderModeModel

        <DataMember(Order:=5)>
        Public Collision As Boolean

        <DataMember(Order:=9)>
        Public IsFloor As Boolean

        <DataMember(Order:=10)>
        Public SeasonConfiguration As EntitySeasonConfigurationModel

        <DataMember(Order:=11)>
        Public Properties As PropertyDataModel()

        <DataContract>
        Public Class EntitySeasonConfigurationModel

            Inherits JsonDataModel

            <DataMember>
            Public TexturePath As String

            <DataMember(Name:="ActiveSeasons")>
            Private ActiveSeasonsStr As String()

            Public Property ActiveSeasons() As World.Seasons()
                Get
                    Return ConvertStringArrayToEnumArray(Of World.Seasons)(ActiveSeasonsStr)
                End Get
                Set(value As World.Seasons())
                    ActiveSeasonsStr = ConvertEnumArrayToStringArray(value)
                End Set
            End Property

        End Class

        <DataContract>
        Public Class PropertyDataModel

            <DataMember(Order:=0)>
            Public Name As String

            <DataMember(Order:=1)>
            Public Value As String

        End Class

    End Class

    <DataContract>
    Public Class NPCModel

        Inherits JsonDataModel

        <DataMember>
        Public Id As Integer

        <DataMember>
        Public Position As Vector3Model

        <DataMember>
        Public Skin As String

        <DataMember>
        Public Name As String

        <DataMember>
        Public Facing As Integer

        'TODO: Add enum
        <DataMember>
        Public Movement As String

        <DataMember>
        Public MoveRectangles As RectangleModel()

        <DataMember>
        Public SightDistance As Integer

        <DataMember>
        Public IsTrainer As Boolean

        <DataMember>
        Public ScriptBinding As String

    End Class

    <DataContract>
    Public Class OffsetMapModel

        <DataMember>
        Public Offset As Vector3Model

        <DataMember>
        Public MapFile As String

    End Class

    <DataContract>
    Class EntityRenderModeModel

        Inherits JsonDataModel

        <DataMember(Name:="RenderMethod", Order:=0)>
        Private RenderMethodStr As String

        Public Property RenderMethod() As GameModes.Maps.EntityRenderMode
            Get
                Return ConvertStringToEnum(Of GameModes.Maps.EntityRenderMode)(RenderMethodStr)
            End Get
            Set(value As GameModes.Maps.EntityRenderMode)
                RenderMethodStr = value.ToString()
            End Set
        End Property

#Region "PrimitiveRenderMode"

        <DataMember(Order:=1)>
        Public Textures As TextureSourceModel()

        <DataMember(Order:=2)>
        Public TextureIndex As Integer()

        <DataMember(Order:=3)>
        Public PrimitiveModelId As Integer

        <DataMember(Order:=4)>
        Public RenderBackfaces As Boolean

#End Region

#Region "ModelRenderMode"

        <DataMember(Order:=5)>
        Public ModelPath As String

#End Region

        <DataMember(Order:=6)>
        Public Visible As Boolean

        <DataMember(Order:=7)>
        Public Opacity As Decimal

        <DataMember(Order:=8)>
        Public Shader As Vector3Model

        <DataMember(Order:=9)>
        Public ObstructCamera As Boolean

    End Class

    <DataContract>
    Public Class ShaderModel

        Inherits JsonDataModel

        <DataMember>
        Public Size As Vector3Model

        <DataMember>
        Public Shader As Vector3Model

        <DataMember>
        Public Position As Vector3Model

        <DataMember>
        Public StopOnContact As Boolean

        <DataMember(Name:="Daytimes")>
        Private DaytimesStr As String()

        Public Property Daytimes() As World.DayTime()
            Get
                Return ConvertStringArrayToEnumArray(Of World.DayTime)(DaytimesStr)
            End Get
            Set(value As World.DayTime())
                DaytimesStr = ConvertEnumArrayToStringArray(value)
            End Set
        End Property

    End Class

    <DataContract>
    Public Class BackdropModel

        <DataMember>
        Public Size As Vector2Model

        <DataMember>
        Public Position As Vector3Model

        <DataMember>
        Public Rotation As Vector3Model

        <DataMember>
        Public Type As String

        <DataMember>
        Public Texture As TextureSourceModel

        <DataMember>
        Public Trigger As String

    End Class

    <DataContract>
    Public Class StructureModel

        <DataMember>
        Public Offset As Vector3Model

        <DataMember>
        Public MapFile As String

        <DataMember>
        Public AddNPCs As Boolean

    End Class

End Namespace