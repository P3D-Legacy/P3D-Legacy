Imports System.Runtime.Serialization

Namespace DataModel.Json.GameJolt

    ''' <summary>
    ''' An entry of the GameJolt ban list.
    ''' </summary>
    <DataContract>
    Public Class BanListEntryModel

        <DataMember(Order:=0)>
        Public GameJoltId As String 'For future compatibility, we use string instead of int.

        <DataMember(Order:=1)>
        Public BanReason As Integer

    End Class

    ''' <summary>
    ''' An entry of the GameJolt ban reason list.
    ''' </summary>
    <DataContract>
    Public Class BanReasonEntryModel

        <DataMember(Order:=0)>
        Public Id As Integer

        <DataMember(Order:=1)>
        Public Reason As String

    End Class

End Namespace