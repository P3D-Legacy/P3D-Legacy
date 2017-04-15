Imports System.Runtime.Serialization

Namespace DataModel.Json.GameModeData

    ''' <summary>
    ''' The data model for a Pokémon interaction.
    ''' </summary>
    <DataContract>
    Public Class InteractionModel

        Inherits JsonDataModel

        <DataMember>
        Public MapFiles As String()

        <DataMember>
        Public PokemonIDs As Integer()

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

        <DataMember(Name:="Weathers")>
        Private WeathersStr As String()

        Public Property Weathers() As World.Weathers()
            Get
                Return ConvertStringArrayToEnumArray(Of World.Weathers)(WeathersStr)
            End Get
            Set(value As World.Weathers())
                WeathersStr = ConvertEnumArrayToStringArray(value)
            End Set
        End Property

        <DataMember(Name:="Seasons")>
        Private SeasonsStr As String()

        Public Property Seasons() As World.Seasons()
            Get
                Return ConvertStringArrayToEnumArray(Of World.Seasons)(SeasonsStr)
            End Get
            Set(value As World.Seasons())
                SeasonsStr = ConvertEnumArrayToStringArray(value)
            End Set
        End Property

        <DataMember(Name:="Types")>
        Private TypesStr As String()

        Public Property Types() As Element.Types()
            Get
                Return ConvertStringArrayToEnumArray(Of Element.Types)(TypesStr)
            End Get
            Set(value As Element.Types())
                TypesStr = ConvertEnumArrayToStringArray(value)
            End Set
        End Property

        <DataMember>
        Public Probability As Integer

        <DataMember>
        Public Emoji As String

        <DataMember>
        Public Message As String

    End Class

End Namespace