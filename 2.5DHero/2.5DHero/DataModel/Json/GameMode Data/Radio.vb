Imports System.Runtime.Serialization

Namespace DataModel.Json.GameModeData

    ''' <summary>
    ''' The data model of a Pokégear channel.
    ''' </summary>
    <DataContract>
    Public Class ChannelModel

        Inherits JsonDataModel

        <DataMember>
        Public ListenRange As RangeModel

        <DataMember>
        Public OverwriteRange As RangeModel

        <DataMember>
        Public Name As String

        <DataMember>
        Public Region As String

        <DataMember(Name:="Daytimes")>
        Private DaytimesStr As String()

        Public Property Daytimes() As World.DayTime()
            Get
                Return ConvertStringArrayToEnumArray(Of World.DayTime)(DaytimesStr)
            End Get
            Set(value As World.DayTime())
                DaytimesStr = ConvertEnumArrayToStringArray(value)
            End Set
        End Property

        <DataMember>
        Public CardRequirements As String()

        <DataMember>
        Public Music As String

        <DataMember>
        Public Content As String

        'TODO: Add enum
        <DataMember>
        Public ContentType As Integer '0=Text/1=Script/2=Special

        <DataMember>
        Public CanBeOverwritten As Boolean

        'TODO: Add enum
        <DataMember>
        Public Activation As Integer '0=Alawys/1=LevelChannel

        <DataMember>
        Public ActivationRegister As String '0=no register

    End Class

End Namespace