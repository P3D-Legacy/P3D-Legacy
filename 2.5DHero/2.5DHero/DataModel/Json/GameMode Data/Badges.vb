Imports System.Runtime.Serialization

Namespace DataModel.Json.GameModeData

    ''' <summary>
    ''' The data model for a badge.
    ''' </summary>
    <DataContract>
    Public Class BadgeModel

        Inherits JsonDataModel

        <DataMember>
        Public Index As Integer

        <DataMember>
        Public Name As String

        <DataMember>
        Public Texture As TextureSourceModel

        <DataMember>
        Public Region As String

        'TODO: Add enum
        <DataMember>
        Public HM As String

    End Class

End Namespace