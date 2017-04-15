Imports System.Runtime.Serialization

Namespace DataModel.Json.GameModeData

    <DataContract>
    Public Class PokedexModel

        Inherits JsonDataModel

        <DataMember>
        Public Name As String

        <DataMember>
        Public UnlockRegister As String

        <DataMember>
        Public PokemonNumbers As RangeModel()

        <DataMember>
        Public IncludeExternalPokemon As Boolean

    End Class

End Namespace