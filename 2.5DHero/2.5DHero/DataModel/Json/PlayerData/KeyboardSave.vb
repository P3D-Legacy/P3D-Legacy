Imports System.Runtime.Serialization

Namespace DataModel.Json.PlayerData

    ''' <summary>
    ''' The data model for the keyboard save file.
    ''' </summary>
    <DataContract>
    Public Class KeyboardSave

        Inherits JsonDataModel

        <DataMember>
        Public ForwardMove As String

        <DataMember>
        Public LeftMove As String

        <DataMember>
        Public BackwardMove As String

        <DataMember>
        Public RightMove As String

        <DataMember>
        Public Inventory As String

        <DataMember>
        Public Chat As String

        <DataMember>
        Public Special As String

        <DataMember>
        Public MuteMusic As String

        <DataMember>
        Public Up As String

        <DataMember>
        Public Down As String

        <DataMember>
        Public Left As String

        <DataMember>
        Public Right As String

        <DataMember>
        Public CameraLock As String

        <DataMember>
        Public GUIControl As String

        <DataMember>
        Public ScreenShot As String

        <DataMember>
        Public DebugControl As String

        <DataMember>
        Public Lighting As String

        <DataMember>
        Public PerspectiveSwitch As String

        <DataMember>
        Public FullScreen As String

        <DataMember>
        Public Enter1 As String

        <DataMember>
        Public Enter2 As String

        <DataMember>
        Public Back1 As String

        <DataMember>
        Public Back2 As String

        <DataMember>
        Public Escape As String

        <DataMember>
        Public OnlineStatus As String

        <DataMember>
        Public Sprinting As String

    End Class

End Namespace