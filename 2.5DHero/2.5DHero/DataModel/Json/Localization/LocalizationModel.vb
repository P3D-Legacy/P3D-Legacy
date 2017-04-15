Imports System.Runtime.Serialization

Namespace DataModel.Json.Localization

    ''' <summary>
    ''' The data model for localization files containing <see cref="TokenModel"/>.
    ''' </summary>
    <DataContract>
    Public Class LocalizationModel

        Inherits JsonDataModel

        <DataMember>
        Public Property Tokens As List(Of TokenModel)

    End Class

    <DataContract>
    Public Class TokenModel

        Inherits JsonDataModel

        <DataMember>
        Public Property Id As String
        <DataMember>
        Public Property Val As String

    End Class

End Namespace
