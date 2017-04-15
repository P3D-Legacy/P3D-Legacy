Imports System.Runtime.Serialization

Namespace DataModel.Json.PlayerData

    ''' <summary>
    ''' The data model for the options save file.
    ''' </summary>
    <DataContract>
    Public Class OptionsModel

        Inherits JsonDataModel

        <DataMember>
        Public Music As Integer

        <DataMember>
        Public Sound As Integer

        <DataMember>
        Public Muted As Boolean

        <DataMember>
        Public RenderDistance As Integer

        <DataMember>
        Public ShowDebug As Integer

        <DataMember>
        Public ShowBoundingBoxes As Boolean

        <DataMember>
        Public ShowDebugConsole As Boolean

        <DataMember>
        Public ShowGUI As Boolean

        <DataMember>
        Public GraphicStyle As Integer

        <DataMember>
        Public LoadOffsetMaps As Integer

        <DataMember>
        Public Language As String

        <DataMember>
        Public ViewBobbing As Boolean

        <DataMember>
        Public GamePadEnabled As Boolean

        <DataMember>
        Public LightingEnabled As Boolean

        <DataMember>
        Public StartedOfflineGame As Boolean

        <DataMember>
        Public PreferMultiSampling As Boolean

        <DataMember>
        Public ContentPacks As String()

        <DataMember>
        Public WindowSize As WindowSizeModel

        <DataMember>
        Public ForceMusic As Boolean

        <DataMember>
        Public MaxOffsetLevel As Integer

        <DataContract>
        Public Class WindowSizeModel

            <DataMember(Order:=0)>
            Public Width As Integer

            <DataMember(Order:=1)>
            Public Height As Integer

        End Class

    End Class

End Namespace