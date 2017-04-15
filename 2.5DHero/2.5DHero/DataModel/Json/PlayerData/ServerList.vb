Imports System.Runtime.Serialization

Namespace DataModel.Json.PlayerData

    ''' <summary>
    ''' The data model for the server list save file.
    ''' </summary>
    <DataContract>
    Public Class ServerListModel

        Inherits JsonDataModel

        <DataMember>
        Public Servers As ServerModel()

        ''' <summary>
        ''' The data model for a server.
        ''' </summary>
        <DataContract>
        Public Class ServerModel

            <DataMember(Order:=0)>
            Public ListName As String

            <DataMember(Order:=1)>
            Public IpAddress As String

            <DataMember(Order:=2)>
            Public Port As Integer

        End Class

    End Class

End Namespace