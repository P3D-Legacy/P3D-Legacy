Imports System.Runtime.Serialization

Namespace DataModel.Json.GameJolt

    <DataContract>
    Public Class DataStorageResponseModel

        Inherits JsonDataModel

        <DataMember>
        Public response As DataStorageModel

        <DataContract>
        Public Class DataStorageModel

            <DataMember>
            Public success As String

            <DataMember>
            Public data As String

            <DataMember>
            Public message As String

        End Class

    End Class

End Namespace