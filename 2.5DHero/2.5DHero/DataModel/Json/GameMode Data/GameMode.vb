Imports System.Runtime.Serialization

Namespace DataModel.Json.GameModeData

    ''' <summary>
    ''' The data model of a GameMode config file.
    ''' </summary>
    <DataContract>
    Public Class GameModeModel

        Inherits JsonDataModel

        <DataMember(Order:=0)>
        Public Name As String

        <DataMember(Order:=1)>
        Public Description As String

        <DataMember(Order:=2)>
        Public Version As String

        <DataMember(Order:=3)>
        Public Author As String

#Region "Paths"

        <DataMember(Order:=4)>
        Public MapPath As String

        <DataMember(Order:=5)>
        Public ScriptPath As String

        <DataMember(Order:=6)>
        Public PokeFilePath As String

        <DataMember(Order:=7)>
        Public ContentPath As String

        <DataMember(Order:=8)>
        Public DataPath As String

#End Region

        <DataMember(Order:=9)>
        Public StartConfiguration As GameModeStartConfigurationModel

        <DataMember(Order:=10)>
        Public Gamerules As List(Of GameRuleModel)

        <DataContract>
        Public Class GameRuleModel

            <DataMember>
            Public Name As String

            <DataMember>
            Public Value As String

        End Class

        <DataContract>
        Public Class GameModeStartConfigurationModel

            <DataMember>
            Public Map As String

            <DataMember>
            Public Script As String

        End Class

    End Class

End Namespace