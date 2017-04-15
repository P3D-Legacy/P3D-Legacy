Imports System.Runtime.Serialization

Namespace DataModel.Json.GameModeData

    <DataContract>
    Public Class SmashRockDefinitionModel

        Inherits JsonDataModel

        <DataMember>
        Public MapFile As String

        <DataMember>
        Public Items As SmashRockItemModel()

        <DataContract>
        Public Class SmashRockItemModel

            <DataMember>
            Public Id As Integer

            <DataMember>
            Public Probability As Integer

        End Class

    End Class

End Namespace