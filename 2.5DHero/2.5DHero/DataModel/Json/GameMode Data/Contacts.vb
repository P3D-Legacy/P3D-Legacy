Imports System.Runtime.Serialization

Namespace DataModel.Json.GameModeData

    <DataContract>
    Public Class ContactModel

        Inherits JsonDataModel

        <DataMember>
        Public Id As Integer

        <DataMember>
        Public Name As String

        <DataMember>
        Public Skin As String

        <DataMember>
        Public Location As String

        <DataMember>
        Public Removable As Boolean

    End Class

End Namespace