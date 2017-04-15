Imports System.Runtime.Serialization

Namespace DataModel.Json.GameJolt

    <DataContract>
    Public Class UserDataResponseModel

        Inherits JsonDataModel

        <DataMember>
        Public response As UserDataCollectionModel

        <DataContract>
        Public Class UserDataCollectionModel

            <DataMember>
            Public success As String

            <DataMember>
            Public users As UserDataModel()

            <DataContract>
            Public Class UserDataModel

                <DataMember>
                Public message As String

                <DataMember>
                Public id As String

                <DataMember>
                Public type As String

                <DataMember>
                Public username As String

                <DataMember>
                Public avatar_url As String

                <DataMember>
                Public signed_up As String

                <DataMember>
                Public last_logged_in As String

                <DataMember>
                Public status As String

                <DataMember>
                Public developer_name As String

                <DataMember>
                Public developer_website As String

                <DataMember>
                Public developer_description As String

            End Class

        End Class

    End Class

End Namespace