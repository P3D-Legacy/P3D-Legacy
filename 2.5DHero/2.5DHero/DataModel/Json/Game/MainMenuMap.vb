Imports System.Runtime.Serialization

Namespace DataModel.Json.Game

    <DataContract>
    Public Class MainMenuMapModel

        <DataMember>
        Public Mapfile As String

        <DataMember>
        Public Offset As Vector3Model

    End Class

End Namespace