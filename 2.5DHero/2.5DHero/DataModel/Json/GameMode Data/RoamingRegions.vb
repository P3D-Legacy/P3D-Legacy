Imports System.Runtime.Serialization

Namespace DataModel.Json.GameModeData

    <DataContract>
    Public Class RoamingRegionModel

        Inherits JsonDataModel

        <DataMember>
        Public Region As String

        <DataMember>
        Public MapFiles As String()

    End Class

End Namespace