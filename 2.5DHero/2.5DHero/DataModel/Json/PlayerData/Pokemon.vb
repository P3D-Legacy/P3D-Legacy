Imports System.Runtime.Serialization

Namespace DataModel.Json.PlayerData

    ''' <summary>
    ''' The data model of a Pokémon save.
    ''' </summary>
    <DataContract>
    Public Class PokemonDataModel

        Inherits JsonDataModel

        <DataMember>
        Public ID As Integer

        <DataMember>
        Public Experience As Integer

        <DataMember(Name:="Gender")>
        Private GenderStr As String

        Public Property Gender() As Pokemon.Genders
            Get
                Return ConvertStringToEnum(Of Pokemon.Genders)(GenderStr)
            End Get
            Set(value As Pokemon.Genders)
                GenderStr = value.ToString()
            End Set
        End Property

        <DataMember>
        Public EggSteps As Integer

        <DataMember>
        Public Item As HeldItemModel

        <DataMember>
        Public Nickname As String

        <DataMember>
        Public Level As Integer

        <DataMember>
        Public OT As String

        <DataMember>
        Public Ability As Integer

        <DataMember(Name:="Status")>
        Private StatusStr As String

        Public Property Status() As Pokemon.StatusProblems
            Get
                Return ConvertStringToEnum(Of Pokemon.StatusProblems)(StatusStr)
            End Get
            Set(value As Pokemon.StatusProblems)
                StatusStr = value.ToString()
            End Set
        End Property

        <DataMember(Name:="Nature")>
        Public NatureStr As String

        Public Property Nature() As Pokemon.Natures
            Get
                Return ConvertStringToEnum(Of Pokemon.Natures)(NatureStr)
            End Get
            Set(value As Pokemon.Natures)
                NatureStr = value.ToString()
            End Set
        End Property

        <DataMember>
        Public CatchInfo As PokemonCatchInformationModel

        <DataMember>
        Public Friendship As Integer

        <DataMember>
        Public IsShiny As Boolean

        <DataMember>
        Public Moves As MoveModel()

        <DataMember>
        Public HP As Integer

        <DataMember>
        Public EV As StatDefinitionModel

        <DataMember>
        Public IV As StatDefinitionModel

        <DataMember>
        Public Data As String

        <DataMember>
        Public IDValue As String

        ''' <summary>
        ''' The data model for a Pokémon's stat definitions.
        ''' </summary>
        <DataContract>
        Public Class StatDefinitionModel

            <DataMember(Order:=0)>
            Public HP As Integer

            <DataMember(Order:=1)>
            Public Atk As Integer

            <DataMember(Order:=2)>
            Public Def As Integer

            <DataMember(Order:=3)>
            Public SpAtk As Integer

            <DataMember(Order:=4)>
            Public SpDef As Integer

            <DataMember(Order:=5)>
            Public Speed As Integer

        End Class

        ''' <summary>
        ''' The data model for a move.
        ''' </summary>
        <DataContract>
        Public Class MoveModel

            <DataMember>
            Private ID As Integer

            <DataMember>
            Private CurrentPP As Integer

            <DataMember>
            Private MaxPP As Integer

        End Class

        ''' <summary>
        ''' The data model for Pokémon catch information.
        ''' </summary>
        <DataContract>
        Public Class PokemonCatchInformationModel

            <DataMember>
            Public Location As String

            <DataMember>
            Public Trainer As String

            <DataMember>
            Public BallID As Integer

            <DataMember>
            Public Method As String

        End Class

        ''' <summary>
        ''' The data model of a held item.
        ''' </summary>
        <DataContract>
        Public Class HeldItemModel

            <DataMember>
            Private ID As Integer

            <DataMember>
            Private Data As String

        End Class

    End Class

End Namespace

Namespace DataModel.Json.GameModeData

    ''' <summary>
    ''' The data model of a Pokémon definition.
    ''' </summary>
    <DataContract>
    Public Class PokemonDefinitionModel

        Inherits JsonDataModel

        <DataMember(Order:=0)>
        Public Name As String

        <DataMember(Order:=1)>
        Public Number As Integer

        <DataMember(Name:="ExperienceType", Order:=2)>
        Private ExperienceTypeStr As String

        Public Property ExperienceType() As Pokemon.ExperienceTypes
            Get
                Return ConvertStringToEnum(Of Pokemon.ExperienceTypes)(ExperienceTypeStr)
            End Get
            Set(value As Pokemon.ExperienceTypes)
                ExperienceTypeStr = value.ToString()
            End Set
        End Property

        <DataMember(Order:=3)>
        Public BaseExperience As Integer

        <DataMember(Name:="Type1", Order:=4)>
        Private Type1Str As String

        Public Property Type1() As Element
            Get
                Return New Element(Type1Str)
            End Get
            Set(value As Element)
                Type1Str = value.ToString()
            End Set
        End Property

        <DataMember(Name:="Type2", Order:=5)>
        Private Type2Str As String

        Public Property Type2() As Element
            Get
                Return New Element(Type2Str)
            End Get
            Set(value As Element)
                Type2Str = value.ToString()
            End Set
        End Property

        <DataMember(Order:=6)>
        Public CatchRate As Integer

        <DataMember(Order:=7)>
        Public BaseFriendship As Integer

        <DataMember(Name:="EggGroup1", Order:=8)>
        Private EggGroup1Str As String

        Public Property EggGroup1() As Pokemon.EggGroups
            Get
                Return ConvertStringToEnum(Of Pokemon.EggGroups)(EggGroup1Str)
            End Get
            Set(value As Pokemon.EggGroups)
                EggGroup1Str = value.ToString()
            End Set
        End Property

        <DataMember(Name:="EggGroup2", Order:=9)>
        Private EggGroup2Str As String

        Public Property EggGroup2() As Pokemon.EggGroups
            Get
                Return ConvertStringToEnum(Of Pokemon.EggGroups)(EggGroup2Str)
            End Get
            Set(value As Pokemon.EggGroups)
                EggGroup2Str = value.ToString()
            End Set
        End Property

        <DataMember(Order:=10)>
        Public BaseEggSteps As Integer

        <DataMember(Order:=11)>
        Public EggPokemon As Integer

        <DataMember(Order:=12)>
        Public Devolution As Integer

        <DataMember(Order:=13)>
        Public CanBreed As Boolean

        <DataMember(Order:=14)>
        Public IsGenderless As Boolean

        <DataMember(Order:=15)>
        Public IsMale As Decimal

        <DataMember(Order:=16)>
        Public Abilities As Integer()

        <DataMember(Name:="HiddenAbility", Order:=17)>
        Public HiddenAbilityID As Integer

        <DataMember(Order:=18)>
        Public EggMoves As Integer()

        <DataMember(Order:=19)>
        Public Machines As Integer()

        <DataMember(Order:=20)>
        Public TutorMoves As Integer()

        <DataMember(Order:=21)>
        Public BaseStats As PlayerData.PokemonDataModel.StatDefinitionModel

        <DataMember(Order:=22)>
        Public RewardEV As PlayerData.PokemonDataModel.StatDefinitionModel

        <DataMember(Order:=23)>
        Public CanFly As Boolean

        <DataMember(Order:=24)>
        Public CanSwim As Boolean

        <DataMember(Order:=25)>
        Public PokedexEntry As PokedexEntryModel

        <DataMember(Order:=26)>
        Public Scale As Vector3Model

        <DataMember(Order:=27)>
        Public TradeValue As Integer

        <DataMember(Order:=28)>
        Public Moves As LevelUpMoveModel()

        <DataMember(Order:=29)>
        Public EvolutionConditions As EvolutionConditionModel()

        <DataMember(Order:=30)>
        Public Items As WildItemModel()

        ''' <summary>
        ''' The data model of a Pokédex entry.
        ''' </summary>
        <DataContract>
        Public Class PokedexEntryModel

            <DataMember(Order:=0)>
            Public Text As String

            <DataMember(Order:=1)>
            Public Species As String

            <DataMember(Order:=2)>
            Public Height As Decimal

            <DataMember(Order:=3)>
            Public Weight As Decimal

            <DataMember(Order:=4)>
            Public Color As RGBColorModel

            Public Function GetPokedexEntry() As PokedexEntry
                Return New PokedexEntry(Text, Species, Weight, Height, Color.GetColor())
            End Function

        End Class

        ''' <summary>
        ''' The data model of a move a Pokémon learns on level up.
        ''' </summary>
        <DataContract>
        Public Class LevelUpMoveModel

            <DataMember(Order:=0)>
            Public Level As Integer

            <DataMember(Order:=1)>
            Public ID As Integer

        End Class

        ''' <summary>
        ''' The data model of an evolution condition.
        ''' </summary>
        <DataContract>
        Public Class EvolutionConditionModel

            <DataMember>
            Public Evolution As Integer

            'TODO: Add enum (Type + Trigger)
            <DataMember>
            Public ConditionType As String

            <DataMember>
            Public Trigger As String

            <DataMember>
            Public Condition As String

        End Class

        ''' <summary>
        ''' The data model of a held item on a wild Pokémon on encounter.
        ''' </summary>
        <DataContract>
        Public Class WildItemModel

            <DataMember(Order:=0)>
            Public Id As Integer

            <DataMember(Order:=1)>
            Public Chance As Integer

        End Class

    End Class

    ''' <summary>
    ''' The data model of a Battle Frontier Pokémon definition.
    ''' </summary>
    <DataContract>
    Public Class FrontierDataModel

        Inherits JsonDataModel

        <DataMember>
        Public Level As Integer

        <DataMember>
        Public Pokemon As FrontierPokemonDataModel()

        ''' <summary>
        ''' The data model of a Pokémon definition for a Battle Frontier Pokémon definition.
        ''' </summary>
        <DataContract>
        Public Class FrontierPokemonDataModel

            <DataMember>
            Public Numbers As Integer()

            <DataMember>
            Public ResultNumber As Integer

            <DataMember>
            Public MoveIds As Integer()

            <DataMember>
            Public Stat1 As String

            <DataMember>
            Public Stat2 As String

            <DataMember>
            Public HeldItemId As Integer

        End Class

    End Class

End Namespace