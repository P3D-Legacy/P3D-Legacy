Imports System.Runtime.Serialization

Namespace DataModel.Json.Game

    ''' <summary>
    ''' The data model for a trainer definition.
    ''' </summary>
    <DataContract>
    Public Class TrainerModel

        Inherits JsonDataModel

        <DataMember>
        Public Name As String

        <DataMember>
        Public [Class] As String

        <DataMember>
        Public Money As Integer

        <DataMember>
        Public Messages As TrainerMessagesModel

        <DataMember>
        Public Songs As TrainerMusicModel

        <DataMember>
        Public Skin As String

        <DataMember>
        Public Region As String

        <DataMember>
        Public Pokemon As TrainerPokemonModel()

        <DataMember>
        Public Items As Integer()

        <DataMember(Name:="Gender")>
        Private GenderStr As String

        Public Property Gender As Pokemon.Genders
            Get
                Return ConvertStringToEnum(Of Pokemon.Genders)(GenderStr)
            End Get
            Set(value As Pokemon.Genders)
                GenderStr = value.ToString()
            End Set
        End Property

        <DataMember>
        Public AI As Integer

        <DataMember>
        Public Intro As TrainerIntroSequenceModel

        ''' <summary>
        ''' The data model for a trainer VS intro.
        ''' </summary>
        <DataContract>
        Public Class TrainerIntroSequenceModel

            <DataMember>
            Public VSType As String

            <DataMember>
            Public BarType As String

        End Class

        ''' <summary>
        ''' The data model for a trainer's Pokémon data.
        ''' </summary>
        <DataContract>
        Public Class TrainerPokemonModel

            <DataMember>
            Public HasFullData As Boolean

            <DataMember>
            Public Level As Integer

            <DataMember>
            Public Number As Integer

            <DataMember>
            Public Data As PlayerData.PokemonDataModel

        End Class

        ''' <summary>
        ''' The data model for a trainer's messages.
        ''' </summary>
        <DataContract>
        Public Class TrainerMessagesModel

            <DataMember>
            Public Intro As String

            <DataMember>
            Public Outro As String

            <DataMember>
            Public Defeat As String

        End Class

        ''' <summary>
        ''' The data model for a trainer's music model
        ''' </summary>
        <DataContract>
        Public Class TrainerMusicModel

            <DataMember>
            Public Intro As String

            <DataMember>
            Public Defeat As String

            <DataMember>
            Public Battle As String

        End Class

    End Class

End Namespace