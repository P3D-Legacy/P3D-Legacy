Imports System.Runtime.Serialization

Namespace DataModel.Json.ContentPack

    <DataContract>
    Public Class ContentPackModel

        <DataMember>
        Public Name As String

        <DataMember>
        Public Version As String

        <DataMember>
        Public Author As String

        <DataMember>
        Public Description As String

    End Class

End Namespace