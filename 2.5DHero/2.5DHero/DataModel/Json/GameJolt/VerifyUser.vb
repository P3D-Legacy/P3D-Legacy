Imports System.Runtime.Serialization

Namespace DataModel.Json.GameJolt

    <DataContract>
    Public Class VerifyUserResponseModel

        Inherits JsonDataModel

        <DataMember>
        Public response As VerifyUserModel

        <DataContract>
        Public Class VerifyUserModel

            <DataMember>
            Public success As String

            <DataMember>
            Public message As String

        End Class

    End Class

End Namespace