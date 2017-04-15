Imports System.Runtime.Serialization

Namespace DataModel.Json.GameModeData

    ''' <summary>
    ''' The data model for a region map.
    ''' </summary>
    <DataContract>
    Public Class RegionmapModel

        <DataMember>
        Public RegionName As String

        <DataMember>
        Public Places As MapPlaceModel()

    End Class

    ''' <summary>
    ''' The data model for a place on the region map.
    ''' </summary>
    <DataContract>
    Public Class MapPlaceModel

        <DataMember>
        Public PlaceType As String

        <DataMember>
        Public Name As String

        <DataMember>
        Public Mapfiles As String()

        <DataMember>
        Public Position As Vector2Model

        <DataMember>
        Public Direction As String

        <DataMember>
        Public RouteType As String

        <DataMember>
        Public Size As String

        <DataMember>
        Public FlyTarget As FlyTargetModel

        ''' <summary>
        ''' The data model for a fly target.
        ''' </summary>
        <DataContract>
        Public Class FlyTargetModel

            <DataMember>
            Public Mapfile As String

            <DataMember>
            Public Position As Vector3Model

        End Class

    End Class

End Namespace