Imports System.Runtime.Serialization

Namespace DataModel.Json.Network

    <DataContract>
    Public Class PackageModel

        Inherits JsonDataModel

        <DataMember(Order:=0)>
        Public ProtocolVersion As String

        <DataMember(Order:=1)>
        Public PackageType As Integer

        <DataMember(Order:=2)>
        Public Origin As Integer

        <DataMember(Order:=3)>
        Public Data As List(Of String)

    End Class

End Namespace