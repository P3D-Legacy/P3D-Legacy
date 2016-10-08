''' <summary>
''' Represents a Pokémon.
''' </summary>
Public Class Pokemon

    ''' <summary>
    ''' Defines which Pokémon in the default GameMode are considered "legendary".
    ''' </summary>
    Public Shared ReadOnly Legendaries() As Integer = {144, 145, 146, 150, 151, 243, 244, 245, 249, 250, 251, 377, 378, 379, 380, 381, 382, 383, 384, 385, 386, 480, 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 638, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 716, 717, 718, 719, 720, 721}

#Region "Events"

    Public Event TexturesCleared(ByVal sender As Object, ByVal e As EventArgs)

#End Region

#Region "Enums"

    ''' <summary>
    ''' The different experience types a Pokémon can have.
    ''' </summary>
    Public Enum ExperienceTypes
        Fast
        MediumFast
        MediumSlow
        Slow
    End Enum

    ''' <summary>
    ''' EggGroups a Pokémon can have to define its breeding compatibility.
    ''' </summary>
    Public Enum EggGroups
        Monster
        Water1
        Water2
        Water3
        Bug
        Flying
        Field
        Fairy
        Grass
        Undiscovered
        HumanLike
        Mineral
        Amorphous
        Ditto
        Dragon
        GenderUnknown
        None
    End Enum

    ''' <summary>
    ''' Genders of a Pokémon.
    ''' </summary>
    Public Enum Genders
        Male
        Female
        Genderless
    End Enum

    ''' <summary>
    ''' The status problems a Pokémon can have.
    ''' </summary>
    Public Enum StatusProblems
        None
        Burn
        Freeze
        Paralyzed
        Poison
        BadPoison
        Sleep
        Fainted
    End Enum

    ''' <summary>
    ''' The volatile status a Pokémon can have.
    ''' </summary>
    Public Enum VolatileStatus
        Confusion
        Flinch
        Infatuation
        Trapped
    End Enum

    ''' <summary>
    ''' Different natures of a Pokémon.
    ''' </summary>
    Public Enum Natures
        Hardy
        Lonely
        Brave
        Adamant
        Naughty
        Bold
        Docile
        Relaxed
        Impish
        Lax
        Timid
        Hasty
        Serious
        Jolly
        Naive
        Modest
        Mild
        Quiet
        Bashful
        Rash
        Calm
        Gentle
        Sassy
        Careful
        Quirky
    End Enum

    ''' <summary>
    ''' Ways to change the Friendship value of a Pokémon.
    ''' </summary>
    Public Enum FriendShipCauses
        Walking
        LevelUp
        Fainting
        EnergyPowder
        HealPowder
        EnergyRoot
        RevivalHerb
        Trading
        Vitamin
        EVBerry
    End Enum

#End Region

#Region "Properties"

    ''' <summary>
    ''' Returns the name to reference to the animation/model of this Pokémon.
    ''' </summary>
    Public AbilityTag As New List(Of Integer)

    Public ReadOnly Property AnimationName() As String
        Get
            Return PokemonForms.GetAnimationName(Me)
        End Get
    End Property

    Public Property AdditionalData() As String
        Get
            Return Me._additionalData
        End Get
        Set(value As String)
            Me._additionalData = value

            Me.ClearTextures()
        End Set
    End Property

    Public Property Number() As Integer
        Get
            Return Me._number
        End Get
        Set(value As Integer)
            Me._number = value
        End Set
    End Property

    Public Property ExperienceType() As ExperienceTypes
        Get
            Return Me._experienceType
        End Get
        Set(value As ExperienceTypes)
            Me._experienceType = value
        End Set
    End Property

    Public Property BaseExperience() As Integer
        Get
            Return Me._baseExperience
        End Get
        Set(value As Integer)
            Me._baseExperience = value
        End Set
    End Property

    Private Property Name() As String
        Get
            Return Me._name
        End Get
        Set(value As String)
            Me._name = value
        End Set
    End Property

    Public Property CatchRate() As Integer
        Get
            Return Me._catchRate
        End Get
        Set(value As Integer)
            Me._catchRate = value
        End Set
    End Property

    Public Property BaseFriendship() As Integer
        Get
            Return Me._baseFriendship
        End Get
        Set(value As Integer)
            Me._baseFriendship = value
        End Set
    End Property

    Public Property BaseEggSteps() As Integer
        Get
            Return Me._baseEggSteps
        End Get
        Set(value As Integer)
            Me._baseEggSteps = value
        End Set
    End Property

    Public Property EggGroup1() As EggGroups
        Get
            Return Me._eggGroup1
        End Get
        Set(value As EggGroups)
            Me._eggGroup1 = value
        End Set
    End Property

    Public Property EggGroup2() As EggGroups
        Get
            Return Me._eggGroup2
        End Get
        Set(value As EggGroups)
            Me._eggGroup2 = value
        End Set
    End Property

    Public Property IsMale() As Decimal
        Get
            Return Me._isMale
        End Get
        Set(value As Decimal)
            Me._isMale = value
        End Set
    End Property

    Public Property IsGenderless() As Boolean
        Get
            Return Me._isGenderLess
        End Get
        Set(value As Boolean)
            Me._isGenderLess = value
        End Set
    End Property

    Public Property Devolution() As Integer
        Get
            Return Me._devolution
        End Get
        Set(value As Integer)
            Me._devolution = value
        End Set
    End Property

    Public Property CanLearnAllMachines() As Boolean
        Get
            Return Me._canLearnAllMachines
        End Get
        Set(value As Boolean)
            Me._canLearnAllMachines = value
        End Set
    End Property

    Public Property CanSwim() As Boolean
        Get
            Return Me._canSwim
        End Get
        Set(value As Boolean)
            Me._canSwim = value
        End Set
    End Property

    Public Property CanFly() As Boolean
        Get
            Return Me._canFly
        End Get
        Set(value As Boolean)
            Me._canFly = value
        End Set
    End Property

    Public Property EggPokemon() As Integer
        Get
            Return Me._eggPokemon
        End Get
        Set(value As Integer)
            Me._eggPokemon = value
        End Set
    End Property

    Public Property TradeValue() As Integer
        Get
            Return Me._tradeValue
        End Get
        Set(value As Integer)
            Me._tradeValue = value
        End Set
    End Property

    Public Property CanBreed() As Boolean
        Get
            Return Me._canBreed
        End Get
        Set(value As Boolean)
            Me._canBreed = value
        End Set
    End Property

    Public Property Experience() As Integer
        Get
            Return Me._experience
        End Get
        Set(value As Integer)
            Me._experience = value
        End Set
    End Property

    Public Property Gender() As Genders
        Get
            Return Me._gender
        End Get
        Set(value As Genders)
            Me._gender = value
        End Set
    End Property

    Public Property EggSteps() As Integer
        Get
            Return Me._eggSteps
        End Get
        Set(value As Integer)
            Me._eggSteps = value
        End Set
    End Property

    Public Property NickName() As String
        Get
            Return Me._nickName
        End Get
        Set(value As String)
            Me._nickName = value
        End Set
    End Property

    Public Property Level() As Integer
        Get
            Return Me._level
        End Get
        Set(value As Integer)
            Me._level = value
        End Set
    End Property

    Public Property OT() As String
        Get
            Return Me._OT
        End Get
        Set(value As String)
            Me._OT = value
        End Set
    End Property

    Public Property Status() As StatusProblems
        Get
            Return Me._status
        End Get
        Set(value As StatusProblems)
            Me._status = value
        End Set
    End Property

    Public Property Nature() As Natures
        Get
            Return Me._nature
        End Get
        Set(value As Natures)
            Me._nature = value
        End Set
    End Property

    Public Property CatchLocation() As String
        Get
            Return Me._catchLocation
        End Get
        Set(value As String)
            Me._catchLocation = value
        End Set
    End Property

    Public Property CatchTrainerName() As String
        Get
            Return Me._catchTrainerName
        End Get
        Set(value As String)
            Me._catchTrainerName = value
        End Set
    End Property

    Public Property CatchMethod() As String
        Get
            Return Me._catchMethod
        End Get
        Set(value As String)
            Me._catchMethod = value
        End Set
    End Property

    Public Property Friendship() As Integer
        Get
            Return Me._friendship
        End Get
        Set(value As Integer)
            Me._friendship = value
        End Set
    End Property

    Public Property IsShiny() As Boolean
        Get
            Return Me._isShiny
        End Get
        Set(value As Boolean)
            Me._isShiny = value
        End Set
    End Property

    Public Property IndividualValue() As String
        Get
            Return Me._individualValue
        End Get
        Set(value As String)
            Me._individualValue = value
        End Set
    End Property

#End Region

#Region "Definition"

#Region "Base Stats"

    Public BaseHP As Integer
    Public BaseAttack As Integer
    Public BaseDefense As Integer
    Public BaseSpAttack As Integer
    Public BaseSpDefense As Integer
    Public BaseSpeed As Integer

#End Region

#Region "GiveEVStats"

    Public Property GiveEVHP() As Integer
        Get
            Return Me._giveEVHP
        End Get
        Set(value As Integer)
            Me._giveEVHP = value

        End Set
    End Property

    Public Property GiveEVAttack() As Integer
        Get
            Return Me._giveEVAttack
        End Get
        Set(value As Integer)
            Me._giveEVAttack = value

        End Set
    End Property

    Public Property GiveEVDefense() As Integer
        Get
            Return Me._giveEVDefense
        End Get
        Set(value As Integer)
            Me._giveEVDefense = value

        End Set
    End Property

    Public Property GiveEVSpAttack() As Integer
        Get
            Return Me._giveEVSpAttack
        End Get
        Set(value As Integer)
            Me._giveEVSpAttack = value

        End Set
    End Property

    Public Property GiveEVSpDefense() As Integer
        Get
            Return Me._giveEVSpDefense
        End Get
        Set(value As Integer)
            Me._giveEVSpDefense = value

        End Set
    End Property

    Public Property GiveEVSpeed() As Integer
        Get
            Return Me._giveEVSpeed
        End Get
        Set(value As Integer)
            Me._giveEVSpeed = value

        End Set
    End Property

    Private _giveEVHP As Integer
    Private _giveEVAttack As Integer
    Private _giveEVDefense As Integer
    Private _giveEVSpAttack As Integer
    Private _giveEVSpDefense As Integer
    Private _giveEVSpeed As Integer

#End Region

    Public Type1 As Element
    Public Type2 As Element
    Public StartItems As New Dictionary(Of Item, Integer)
    Public AttackLearns As New Dictionary(Of Integer, BattleSystem.Attack)
    Public EggMoves As New List(Of Integer)
    Public TutorAttacks As New List(Of BattleSystem.Attack)
    Public EvolutionConditions As New List(Of EvolutionCondition)
    Public NewAbilities As New List(Of Ability)
    Public HiddenAbility As Ability = Nothing
    Public Machines As New List(Of Integer)
    Public PokedexEntry As PokedexEntry
    Public Cry As SoundEffect
    Public WildItems As New Dictionary(Of Integer, Integer)

    Private _name As String
    Private _number As Integer
    Private _experienceType As ExperienceTypes
    Private _baseExperience As Integer
    Private _catchRate As Integer
    Private _baseFriendship As Integer
    Private _eggGroup1 As EggGroups
    Private _eggGroup2 As EggGroups
    Private _baseEggSteps As Integer
    Private _isMale As Decimal
    Private _isGenderLess As Boolean
    Private _devolution As Integer = 0
    Private _canLearnAllMachines As Boolean = False
    Private _canSwim As Boolean
    Private _canFly As Boolean
    Private _eggPokemon As Integer = 0
    Private _tradeValue As Integer = 10
    Private _canBreed As Boolean = True

#End Region

#Region "SavedStats"

#Region "Stats"

    Private _HP As Integer
    Private _maxHP As Integer
    Private _attack As Integer
    Private _defense As Integer
    Private _SpAttack As Integer
    Private _SpDefense As Integer
    Private _speed As Integer

    ''' <summary>
    ''' The HP of this Pokémon.
    ''' </summary>
    Public Property HP() As Integer
        Get
            Return Me._HP
        End Get
        Set(value As Integer)
            Me._HP = value

        End Set
    End Property

    ''' <summary>
    ''' The maximal HP of this Pokémon.
    ''' </summary>
    Public Property MaxHP() As Integer
        Get
            Return Me._maxHP
        End Get
        Set(value As Integer)
            Me._maxHP = value

        End Set
    End Property

    ''' <summary>
    ''' The Attack of this Pokémon.
    ''' </summary>
    Public Property Attack() As Integer
        Get
            Return Me._attack
        End Get
        Set(value As Integer)
            Me._attack = value

        End Set
    End Property

    ''' <summary>
    ''' The Defense of this Pokémon.
    ''' </summary>
    Public Property Defense() As Integer
        Get
            Return Me._defense
        End Get
        Set(value As Integer)
            Me._defense = value

        End Set
    End Property

    ''' <summary>
    ''' The Special Attack of this Pokémon.
    ''' </summary>
    Public Property SpAttack() As Integer
        Get
            Return Me._SpAttack
        End Get
        Set(value As Integer)
            Me._SpAttack = value

        End Set
    End Property

    ''' <summary>
    ''' The Special Defense of this Pokémon.
    ''' </summary>
    Public Property SpDefense() As Integer
        Get
            Return Me._SpDefense
        End Get
        Set(value As Integer)
            Me._SpDefense = value

        End Set
    End Property

    ''' <summary>
    ''' The Speed of this Pokémon.
    ''' </summary>
    Public Property Speed() As Integer
        Get
            Return Me._speed
        End Get
        Set(value As Integer)
            Me._speed = value

        End Set
    End Property

#End Region

#Region "EVStats"

    Private _EVHP As Integer
    Private _EVAttack As Integer
    Private _EVDefense As Integer
    Private _EVSpAttack As Integer
    Private _EVSpDefense As Integer
    Private _EVSpeed As Integer

    ''' <summary>
    ''' The HP EV this Pokémon got.
    ''' </summary>
    Public Property EVHP() As Integer
        Get
            Return Me._EVHP
        End Get
        Set(value As Integer)
            Me._EVHP = value

            Me.CalculateStats()
        End Set
    End Property

    ''' <summary>
    ''' The Attack EV this Pokémon got.
    ''' </summary>
    Public Property EVAttack() As Integer
        Get
            Return Me._EVAttack
        End Get
        Set(value As Integer)
            Me._EVAttack = value

            Me.CalculateStats()
        End Set
    End Property

    ''' <summary>
    ''' The Defense EV this Pokémon got.
    ''' </summary>
    Public Property EVDefense() As Integer
        Get
            Return Me._EVDefense
        End Get
        Set(value As Integer)
            Me._EVDefense = value

            Me.CalculateStats()
        End Set
    End Property

    ''' <summary>
    ''' The Special Attack EV this Pokémon got.
    ''' </summary>
    Public Property EVSpAttack() As Integer
        Get
            Return Me._EVSpAttack
        End Get
        Set(value As Integer)
            Me._EVSpAttack = value

            Me.CalculateStats()
        End Set
    End Property

    ''' <summary>
    ''' The Special Defense EV this Pokémon got.
    ''' </summary>
    Public Property EVSpDefense() As Integer
        Get
            Return Me._EVSpDefense
        End Get
        Set(value As Integer)
            Me._EVSpDefense = value

            Me.CalculateStats()
        End Set
    End Property

    ''' <summary>
    ''' The Speed EV this Pokémon got.
    ''' </summary>
    Public Property EVSpeed() As Integer
        Get
            Return Me._EVSpeed
        End Get
        Set(value As Integer)
            Me._EVSpeed = value

            Me.CalculateStats()
        End Set
    End Property

#End Region

#Region "IVStats"

    Private _IVHP As Integer
    Private _IVAttack As Integer
    Private _IVDefense As Integer
    Private _IVSpAttack As Integer
    Private _IVSpDefense As Integer
    Private _IVSpeed As Integer

    ''' <summary>
    ''' The HP IV this Pokémon got.
    ''' </summary>
    Public Property IVHP() As Integer
        Get
            Return Me._IVHP
        End Get
        Set(value As Integer)
            Me._IVHP = value

        End Set
    End Property

    ''' <summary>
    ''' The Attack IV this Pokémon got.
    ''' </summary>
    Public Property IVAttack() As Integer
        Get
            Return Me._IVAttack
        End Get
        Set(value As Integer)
            Me._IVAttack = value

        End Set
    End Property

    ''' <summary>
    ''' The Defense IV this Pokémon got.
    ''' </summary>
    Public Property IVDefense() As Integer
        Get
            Return Me._IVDefense
        End Get
        Set(value As Integer)
            Me._IVDefense = value

        End Set
    End Property

    ''' <summary>
    ''' The Special Attack IV this Pokémon got.
    ''' </summary>
    Public Property IVSpAttack() As Integer
        Get
            Return Me._IVSpAttack
        End Get
        Set(value As Integer)
            Me._IVSpAttack = value

        End Set
    End Property

    ''' <summary>
    ''' The Special Defense IV this Pokémon got.
    ''' </summary>
    Public Property IVSpDefense() As Integer
        Get
            Return Me._IVSpDefense
        End Get
        Set(value As Integer)
            Me._IVSpDefense = value

        End Set
    End Property

    ''' <summary>
    ''' The Speed IV this Pokémon got.
    ''' </summary>
    Public Property IVSpeed() As Integer
        Get
            Return Me._IVSpeed
        End Get
        Set(value As Integer)
            Me._IVSpeed = value

        End Set
    End Property

#End Region

    Private _item As Item
    Private _additionalData As String = ""

    Public Property Item() As Item
        Get
            Return Me._item
        End Get
        Set(value As Item)
            Me._item = value
            Me.ClearTextures()
        End Set
    End Property

    Public Attacks As New List(Of BattleSystem.Attack)
    Public Ability As Ability
    Public CatchBall As Item = Item.GetItemByID(5)

    Private _experience As Integer
    Private _gender As Genders
    Private _eggSteps As Integer
    Private _nickName As String
    Private _level As Integer
    Private _OT As String = "00000"
    Private _status As StatusProblems = StatusProblems.None
    Private _nature As Natures
    Private _catchLocation As String = "at unknown place"
    Private _catchTrainerName As String = "???"
    Private _catchMethod As String = "somehow obtained"
    Private _friendship As Integer
    Private _isShiny As Boolean
    Private _individualValue As String = ""

#End Region

#Region "Temp"

    Private _volatiles As New List(Of VolatileStatus)

    ''' <summary>
    ''' Returns if this Pokémon is affected by a Volatile Status effect.
    ''' </summary>
    ''' <param name="VolatileStatus">The Volatile Status effect to test for.</param>
    Public Function HasVolatileStatus(ByVal VolatileStatus As VolatileStatus) As Boolean
        Return Me._volatiles.Contains(VolatileStatus)
    End Function

    ''' <summary>
    ''' Affects this Pokémon with a Volatile Status.
    ''' </summary>
    ''' <param name="VolatileStatus">The Volatile Status to affect this Pokémon with.</param>
    Public Sub AddVolatileStatus(ByVal VolatileStatus As VolatileStatus)
        If Me._volatiles.Contains(VolatileStatus) = False Then
            Me._volatiles.Add(VolatileStatus)
        End If
    End Sub

    ''' <summary>
    ''' Removes a Volatile Status effect this Pokémon is affected by.
    ''' </summary>
    ''' <param name="VolatileStatus">The Volatile Status effect to remove.</param>
    Public Sub RemoveVolatileStatus(ByVal VolatileStatus As VolatileStatus)
        If Me._volatiles.Contains(VolatileStatus) = True Then
            Me._volatiles.Remove(VolatileStatus)
        End If
    End Sub

    ''' <summary>
    ''' Clears all Volatile Status effects affecting this Pokémon.
    ''' </summary>
    Public Sub ClearAllVolatiles()
        Me._volatiles.Clear()
    End Sub

    Public StatAttack As Integer = 0
    Public StatDefense As Integer = 0
    Public StatSpAttack As Integer = 0
    Public StatSpDefense As Integer = 0
    Public StatSpeed As Integer = 0

    Public Accuracy As Integer = 0
    Public Evasion As Integer = 0

    Public hasLeveledUp As Boolean = False

    Public SleepTurns As Integer = -1
    Public ConfusionTurns As Integer = -1

    Public LastHitByMove As BattleSystem.Attack
    Public LastDamageReceived As Integer = 0
    Public LastHitPhysical As Integer = -1

    ''' <summary>
    ''' Resets the temp storages of the Pokémon.
    ''' </summary>
    Public Sub ResetTemp()
        _volatiles.Clear()

        StatAttack = 0
        StatDefense = 0
        StatSpAttack = 0
        StatSpDefense = 0
        StatSpeed = 0

        Accuracy = 0
        Evasion = 0

        If _originalNumber > -1 Then
            Me.Number = _originalNumber
            _originalNumber = -1
        End If

        If Not _originalType1 Is Nothing Then
            Me.Type1 = _originalType1
            _originalType1 = Nothing
        End If

        If Not _originalType2 Is Nothing Then
            Me.Type2 = _originalType2
            _originalType2 = Nothing
        End If

        If _originalStats(0) > -1 Then
            Me.Attack = _originalStats(0)
            _originalStats(0) = -1
        End If
        If _originalStats(1) > -1 Then
            Me.Defense = _originalStats(1)
            _originalStats(1) = -1
        End If
        If _originalStats(2) > -1 Then
            Me.SpAttack = _originalStats(2)
            _originalStats(2) = -1
        End If
        If _originalStats(3) > -1 Then
            Me.SpDefense = _originalStats(3)
            _originalStats(3) = -1
        End If
        If _originalStats(4) > -1 Then
            Me.Speed = _originalStats(4)
            _originalStats(4) = -1
        End If

        If _originalShiny > -1 Then
            If _originalShiny = 0 Then
                Me.IsShiny = False
            ElseIf _originalShiny = 1 Then
                Me.IsShiny = True
            End If
            Me._originalShiny = -1
        End If

        If Not Me._originalMoves Is Nothing Then
            Me.Attacks.Clear()
            For Each a As BattleSystem.Attack In Me._originalMoves
                Me.Attacks.Add(a)
            Next
            Me._originalMoves = Nothing
        End If

        If Not Me._originalAbility Is Nothing Then
            Me.Ability = Me._originalAbility
            Me._originalAbility = Nothing
        End If

        If Not Me._originalItem Is Nothing Then
            Me.Item = net.Pokemon3D.Game.Item.GetItemByID(Me._originalItem.ID)
            Me.Item.AdditionalData = Me._originalItem.AdditionalData
            Me._originalItem = Nothing
        End If

        Me.IsTransformed = False

        Me.CalculateStats()
    End Sub

    'Just use these subs when doing/reverting mega evolutions.
    Public NormalAbility As Ability = New Abilities.Stench
    Public Sub LoadMegaAbility()
        NormalAbility = Ability
        Me.Ability = NewAbilities(0)
    End Sub
    Public Sub RestoreAbility()
        Me.Ability = NormalAbility
    End Sub

#End Region

#Region "OriginalStats"

    'Original Stats store the stats that the Pokémon has by default. When they get overwritten (for example by Dittos Transform move), the original values get stored in the "Original_X" value.
    'All these properties ensure that no part of the original Pokémon gets overwritten once the original value got set into place.

    ''' <summary>
    ''' The Pokémon's original primary type.
    ''' </summary>
    Public Property OriginalType1() As Element
        Get
            Return Me._originalType1
        End Get
        Set(value As Element)
            If Me._originalType1 Is Nothing Then
                Me._originalType1 = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original secondary type.
    ''' </summary>
    Public Property OriginalType2() As Element
        Get
            Return Me._originalType2
        End Get
        Set(value As Element)
            If Me._originalType2 Is Nothing Then
                Me._originalType2 = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original national Pokédex number.
    ''' </summary>
    Public Property OriginalNumber() As Integer
        Get
            Return Me._originalNumber
        End Get
        Set(value As Integer)
            If Me._originalNumber = -1 Then
                Me._originalNumber = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original shiny state.
    ''' </summary>
    Public Property OriginalShiny() As Integer
        Get
            Return Me._originalShiny
        End Get
        Set(value As Integer)
            If Me._originalShiny = -1 Then
                Me._originalShiny = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original stats.
    ''' </summary>
    Public Property OriginalStats() As Integer()
        Get
            Return Me._originalStats
        End Get
        Set(value As Integer())
            If Me._originalStats Is {-1, -1, -1, -1, -1} Then
                Me._originalStats = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original ability.
    ''' </summary>
    Public Property OriginalAbility() As Ability
        Get
            Return Me._originalAbility
        End Get
        Set(value As Ability)
            If Me._originalAbility Is Nothing Then
                Me._originalAbility = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original hold item.
    ''' </summary>
    Public Property OriginalItem() As Item
        Get
            Return Me._originalItem
        End Get
        Set(value As Item)
            If Me._originalItem Is Nothing Then
                Me._originalItem = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' The Pokémon's original moveset.
    ''' </summary>
    Public Property OriginalMoves() As List(Of BattleSystem.Attack)
        Get
            Return Me._originalMoves
        End Get
        Set(value As List(Of BattleSystem.Attack))
            If Me._originalMoves Is Nothing Then
                Me._originalMoves = value
            End If
        End Set
    End Property

    ''' <summary>
    ''' If this Pokémon has been using the Transform move (or any other move/ability that causes similar effects).
    ''' </summary>
    Public Property IsTransformed() As Boolean
        Get
            Return Me._isTransformed
        End Get
        Set(value As Boolean)
            Me._isTransformed = value
        End Set
    End Property

    Private _originalType1 As Element = Nothing
    Private _originalType2 As Element = Nothing
    Private _originalNumber As Integer = -1

    Private _originalStats() As Integer = {-1, -1, -1, -1, -1}
    Private _originalShiny As Integer = -1
    Private _originalMoves As List(Of BattleSystem.Attack) = Nothing
    Private _originalAbility As Ability = Nothing

    Private _originalItem As Item = Nothing

    Private _isTransformed As Boolean = False

#End Region

    Private Textures As New List(Of Texture2D)
    Private Scale As New Vector3(1)

    ''' <summary>
    ''' Empties the cached textures.
    ''' </summary>
    Private Sub ClearTextures()
        Textures.Clear()
        Textures.AddRange({Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing})
        RaiseEvent TexturesCleared(Me, New EventArgs())
    End Sub

#Region "Constructors and Data Handlers"

    ''' <summary>
    ''' Creates a new instance of the Pokémon class.
    ''' </summary>
    Private Sub New()
        MyBase.New()
        Me.ClearTextures()
    End Sub

    ''' <summary>
    ''' Returns a new Pokémon class instance.
    ''' </summary>
    ''' <param name="Number">The number of the Pokémon in the national Pokédex.</param>
    Public Shared Function GetPokemonByID(ByVal Number As Integer) As Pokemon
        Return GetPokemonByID(Number, "")
    End Function

    Public Shared Function GetPokemonByID(ByVal Number As Integer, ByVal AdditionalData As String) As Pokemon
        Dim p As New Pokemon()
        p.LoadDefinitions(Number, AdditionalData)
        Return p
    End Function

    ''' <summary>
    ''' Checks if a requested Pokémon data file exists.
    ''' </summary>
    Public Shared Function PokemonDataExists(ByVal Number As Integer) As Boolean
        Return System.IO.File.Exists(GameModeManager.GetPokemonDataFilePath(Number.ToString() & ".dat"))
    End Function

    ''' <summary>
    ''' Returns a new Pokémon class instance defined by data.
    ''' </summary>
    ''' <param name="InputData">The data that defines the Pokémon.</param>
    Public Shared Function GetPokemonByData(ByVal InputData As String) As Pokemon
        Dim Tags As New Dictionary(Of String, String)
        Dim Data() As String = InputData.Split(CChar("}"))
        For Each Tag As String In Data
            If Tag.Contains("{") = True And Tag.Contains("[") = True Then
                Dim TagName As String = Tag.Remove(0, 2)
                TagName = TagName.Remove(TagName.IndexOf(""""))

                Dim TagContent As String = Tag.Remove(0, Tag.IndexOf("[") + 1)
                TagContent = TagContent.Remove(TagContent.IndexOf("]"))

                If Tags.ContainsKey(TagName) = False Then
                    Tags.Add(TagName, TagContent)
                End If
            End If
        Next

        Dim NewAdditionalData As String = ""
        If Tags.ContainsKey("AdditionalData") = True Then
            NewAdditionalData = CStr(Tags("AdditionalData"))
        End If

        Dim PokemonID As Integer = 10
        If Tags.ContainsKey("Pokemon") = True Then
            PokemonID = CInt(Tags("Pokemon"))
        End If

        Dim p As Pokemon = GetPokemonByID(PokemonID, NewAdditionalData)
        p.LoadData(InputData)

        Return p
    End Function

    ''' <summary>
    ''' Loads definition data from the data files and empties the temp textures.
    ''' </summary>
    Public Sub ReloadDefinitions()
        Me.LoadDefinitions(Me.Number, Me.AdditionalData)
        Me.ClearTextures()
    End Sub

    ''' <summary>
    ''' Loads definition data from the data file.
    ''' </summary>
    ''' <param name="Number">The number of the Pokémon in the national Pokédex.</param>
    ''' <param name="AdditionalData">The additional data.</param>
    Public Sub LoadDefinitions(ByVal Number As Integer, ByVal AdditionalData As String)
        Dim path As String = PokemonForms.GetPokemonDataFile(Number, AdditionalData)
        Security.FileValidation.CheckFileValid(path, False, "Pokemon.vb")
        NewAbilities.Clear()

        Dim Data() As String = System.IO.File.ReadAllLines(path)

        For Each Line As String In Data
            Dim VarName As String = Line.GetSplit(0, "|")
            Dim Value As String = Line.GetSplit(1, "|")

            Select Case VarName.ToLower()
                Case "name"
                    Me.Name = Value
                Case "number"
                    Me.Number = CInt(Value)
                Case "baseexperience"
                    Me.BaseExperience = CInt(Value)
                Case "experiencetype"
                    Select Case CInt(Value)
                        Case 0
                            Me.ExperienceType = ExperienceTypes.Fast
                        Case 1
                            Me.ExperienceType = ExperienceTypes.MediumFast
                        Case 2
                            Me.ExperienceType = ExperienceTypes.MediumSlow
                        Case 3
                            Me.ExperienceType = ExperienceTypes.Slow
                    End Select
                Case "type1"
                    Me.Type1 = New Element(Value)
                Case "type2"
                    Me.Type2 = New Element(Value)
                Case "catchrate"
                    Me.CatchRate = CInt(Value)
                Case "basefriendship"
                    Me.BaseFriendship = CInt(Value)
                Case "egggroup1"
                    Me.EggGroup1 = ConvertIDToEggGroup(Value)
                Case "egggroup2"
                    Me.EggGroup2 = ConvertIDToEggGroup(Value)
                Case "baseeggsteps"
                    Me.BaseEggSteps = CInt(Value)
                Case "ismale"
                    If Value.Contains(".") Then
                        Value = Value.Replace(".", ",")
                    End If
                    Me.IsMale = CDec(Value.Replace(",", GameController.DecSeparator))
                Case "isgenderless"
                    Me.IsGenderless = CBool(Value)
                Case "devolution"
                    Me.Devolution = CInt(Value)
                Case "ability1", "ability"
                    If Value <> "Nothing" Then
                        Me.NewAbilities.Add(Ability.GetAbilityByID(CInt(Value)))
                    End If
                Case "ability2"
                    If Value <> "Nothing" Then
                        Me.NewAbilities.Add(Ability.GetAbilityByID(CInt(Value)))
                    End If
                Case "hiddenability"
                    If Value <> "Nothing" Then
                        Me.HiddenAbility = Ability.GetAbilityByID(CInt(Value))
                    End If
                Case "machines"
                    If Value <> "" Then
                        If Value.Contains(",") = True Then
                            Dim MachinesValue() As String = Value.Split(CChar(","))
                            For i = 0 To MachinesValue.Length - 1
                                If IsNumeric(MachinesValue(i)) = True Then
                                    Me.Machines.Add(CInt(MachinesValue(i)))
                                End If
                            Next
                        Else
                            If IsNumeric(Value) = True Then
                                If CInt(Value) = -1 Then
                                    Me.Machines.Clear()
                                    Me.CanLearnAllMachines = True
                                Else
                                    Me.Machines.Add(CInt(Value))
                                End If
                            End If
                        End If
                    End If
                Case "eggmoves"
                    If Value <> "" Then
                        If Value.Contains(",") = True Then
                            Dim EggMoveValues() As String = Value.Split(CChar(","))
                            For i = 0 To EggMoveValues.Length - 1
                                EggMoves.Add(CInt(EggMoveValues(i)))
                            Next
                        Else
                            EggMoves.Add(CInt(Value))
                        End If
                    End If
                Case "tutormoves"
                    If Value <> "" Then
                        If Value.Contains(",") = True Then
                            Dim TutorValue() As String = Value.Split(CChar(","))
                            For i = 0 To TutorValue.Length - 1
                                TutorAttacks.Add(BattleSystem.Attack.GetAttackByID(CInt(TutorValue(i))))
                            Next
                        Else
                            TutorAttacks.Add(BattleSystem.Attack.GetAttackByID(CInt(Value)))
                        End If
                    End If
                Case "eggpokemon"
                    Me.EggPokemon = CInt(Value)
                Case "canbreed"
                    Me.CanBreed = CBool(Value)
                Case "basehp"
                    Me.BaseHP = CInt(Value)
                Case "baseattack"
                    Me.BaseAttack = CInt(Value)
                Case "basedefense"
                    Me.BaseDefense = CInt(Value)
                Case "basespattack"
                    Me.BaseSpAttack = CInt(Value)
                Case "basespdefense"
                    Me.BaseSpDefense = CInt(Value)
                Case "basespeed"
                    Me.BaseSpeed = CInt(Value)
                Case "fphp"
                    Me.GiveEVHP = CInt(Value)
                Case "fpattack"
                    Me.GiveEVAttack = CInt(Value)
                Case "fpdefense"
                    Me.GiveEVDefense = CInt(Value)
                Case "fpspattack"
                    Me.GiveEVSpAttack = CInt(Value)
                Case "fpspdefense"
                    Me.GiveEVSpDefense = CInt(Value)
                Case "fpspeed"
                    Me.GiveEVSpeed = CInt(Value)
                Case "canswim"
                    Me.CanSwim = CBool(Value)
                Case "canfly"
                    Me.CanFly = CBool(Value)
                Case "pokedex"
                    Dim PokedexData() As String = Value.Split(CChar("\"))
                    Me.PokedexEntry = New PokedexEntry(PokedexData(0), PokedexData(1), CSng(PokedexData(2).Replace(".", GameController.DecSeparator)), CSng(PokedexData(3).Replace(".", GameController.DecSeparator)), New Color(CInt(PokedexData(4).GetSplit(0)), CInt(PokedexData(4).GetSplit(1)), CInt(PokedexData(4).GetSplit(2))))
                Case "scale"
                    If Value.Contains(".") Then
                        Value = Value.Replace(".", ",")
                    End If
                    Me.Scale = New Vector3(CSng(Value.Replace(",", GameController.DecSeparator)))
                Case "move"
                    Dim Level As Integer = CInt(Value.GetSplit(0))
                    Dim MoveID As Integer = CInt(Value.GetSplit(1))

                    If AttackLearns.ContainsKey(Level) = True Then
                        While AttackLearns.ContainsKey(Level)
                            Level += 1
                        End While
                    End If

                    Me.AttackLearns.Add(Level, BattleSystem.Attack.GetAttackByID(MoveID))
                Case "evolutioncondition"
                    'Evolution,Type,Argument,Trigger

                    Dim EvolutionData() As String = Value.Split(CChar(","))

                    Dim Evolution As Integer = CInt(EvolutionData(0))
                    Dim Type As String = EvolutionData(1)
                    Dim Argument As String = EvolutionData(2)
                    Dim Trigger As String = EvolutionData(3)

                    Dim EvolutionExists As Boolean = False
                    Dim e As EvolutionCondition = New EvolutionCondition

                    For Each oldE As EvolutionCondition In Me.EvolutionConditions
                        If Evolution = oldE.Evolution Then
                            e = oldE
                            EvolutionExists = True
                        End If
                    Next

                    e.SetEvolution(Evolution)
                    e.AddCondition(Type, Argument, Trigger)

                    If EvolutionExists = False Then
                        EvolutionConditions.Add(e)
                    End If
                Case "item"
                    Dim chance As Integer = CInt(Value.GetSplit(0))
                    Dim itemID As Integer = CInt(Value.GetSplit(1))

                    If Not WildItems.ContainsKey(chance) Then
                        WildItems.Add(chance, itemID)
                    End If
                Case "tradevalue"
                    Me.TradeValue = CInt(Value)
            End Select
        Next

        If Me.EggPokemon = 0 Then
            Me.EggPokemon = Me.Number
        End If

        Dim pAttacks As New SortedDictionary(Of Integer, BattleSystem.Attack)
        For i = 0 To AttackLearns.Count - 1
            pAttacks.Add(AttackLearns.Keys(i), AttackLearns.Values(i))
        Next
        AttackLearns.Clear()
        For i = 0 To pAttacks.Count - 1
            AttackLearns.Add(pAttacks.Keys(i), pAttacks.Values(i))
        Next

        If Me.Devolution = 0 Then
            If Me.EggPokemon > 0 And Me.EggPokemon <> Me.Number Then
                If Me.Number - Me.EggPokemon = 2 Then
                    Me.Devolution = Me.Number - 1
                ElseIf Me.Number - Me.EggPokemon = 1 Then
                    Me.Devolution = Me.EggPokemon
                End If
            End If
        End If

        If AdditionalData = "" Then
            Me.AdditionalData = PokemonForms.GetInitialAdditionalData(Me)
        End If
    End Sub

    ''' <summary>
    ''' Applies data to the Pokémon.
    ''' </summary>
    ''' <param name="InputData">The input data.</param>
    Public Sub LoadData(ByVal InputData As String)
        Dim loadedHP As Boolean = False

        Dim Tags As New Dictionary(Of String, String)
        Dim Data() As String = InputData.Split(CChar("}"))
        For Each Tag As String In Data
            If Tag.Contains("{") = True And Tag.Contains("[") = True Then
                Dim TagName As String = Tag.Remove(0, 2)
                TagName = TagName.Remove(TagName.IndexOf(""""))

                Dim TagContent As String = Tag.Remove(0, Tag.IndexOf("[") + 1)
                TagContent = TagContent.Remove(TagContent.IndexOf("]"))

                If Tags.ContainsKey(TagName) = False Then
                    Tags.Add(TagName, TagContent)
                End If
            End If
        Next

        For i = 0 To Tags.Count - 1
            Dim tagName As String = Tags.Keys(i)
            Dim tagValue As String = Tags.Values(i)

            Select Case tagName.ToLower()
                Case "experience"
                    Me.Experience = CInt(tagValue)
                Case "gender"
                    Select Case CInt(tagValue)
                        Case 0
                            Me.Gender = Genders.Male
                        Case 1
                            Me.Gender = Genders.Female
                        Case 2
                            Me.Gender = Genders.Genderless
                    End Select
                Case "eggsteps"
                    Me.EggSteps = CInt(tagValue)
                Case "item"
                    If IsNumeric(tagValue) = True Then
                        Me.Item = Item.GetItemByID(CInt(tagValue))
                    End If
                Case "itemdata"
                    If Not Me.Item Is Nothing Then
                        Me.Item.AdditionalData = tagValue
                    End If
                Case "nickname"
                    Me.NickName = tagValue
                Case "level"
                    Me.Level = CInt(tagValue).Clamp(1, CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")))
                Case "ot"
                    Me.OT = tagValue
                Case "ability"
                    Me.Ability = net.Pokemon3D.Game.Ability.GetAbilityByID(CInt(tagValue))
                    Me.NormalAbility = Ability
                Case "status"
                    Select Case tagValue
                        Case "BRN"
                            Me.Status = StatusProblems.Burn
                        Case "PSN"
                            Me.Status = StatusProblems.Poison
                        Case "PRZ"
                            Me.Status = StatusProblems.Paralyzed
                        Case "SLP"
                            Me.Status = StatusProblems.Sleep
                        Case "FNT"
                            Me.Status = StatusProblems.Fainted
                        Case "FRZ"
                            Me.Status = StatusProblems.Freeze
                        Case "BPSN"
                            Me.Status = StatusProblems.BadPoison
                        Case Else
                            Me.Status = StatusProblems.None
                    End Select
                Case "nature"
                    Me.Nature = ConvertIDToNature(CInt(tagValue))
                Case "catchlocation"
                    Me.CatchLocation = tagValue
                Case "catchtrainer"
                    Me.CatchTrainerName = tagValue
                Case "catchball"
                    Me.CatchBall = Item.GetItemByID(CInt(tagValue))
                Case "catchmethod"
                    Me.CatchMethod = tagValue
                Case "friendship"
                    Me.Friendship = CInt(tagValue)
                Case "isshiny"
                    Me.IsShiny = CBool(tagValue)
                Case "attack1", "attack2", "attack3", "attack4"
                    If Not net.Pokemon3D.Game.BattleSystem.Attack.ConvertStringToAttack(tagValue) Is Nothing Then
                        Attacks.Add(net.Pokemon3D.Game.BattleSystem.Attack.ConvertStringToAttack(tagValue))
                    End If
                Case "stats"
                    Dim Stats() As String = tagValue.Split(CChar(","))
                    HP = CInt(Stats(0)).Clamp(0, 999)
                    loadedHP = True
                Case "hp"
                    HP = CInt(tagValue)
                    loadedHP = True
                Case "fps", "evs"
                    Dim EVs() As String = tagValue.Split(CChar(","))
                    EVHP = CInt(EVs(0)).Clamp(0, 255)
                    EVAttack = CInt(EVs(1)).Clamp(0, 255)
                    EVDefense = CInt(EVs(2)).Clamp(0, 255)
                    EVSpAttack = CInt(EVs(3)).Clamp(0, 255)
                    EVSpDefense = CInt(EVs(4)).Clamp(0, 255)
                    EVSpeed = CInt(EVs(5)).Clamp(0, 255)
                Case "dvs", "ivs"
                    Dim IVs() As String = tagValue.Split(CChar(","))
                    IVHP = CInt(IVs(0))
                    IVAttack = CInt(IVs(1))
                    IVDefense = CInt(IVs(2))
                    IVSpAttack = CInt(IVs(3))
                    IVSpDefense = CInt(IVs(4))
                    IVSpeed = CInt(IVs(5))
                Case "additionaldata"
                    Me.AdditionalData = tagValue
                Case "idvalue"
                    Me.IndividualValue = tagValue
            End Select
        Next

        If Me.IndividualValue = "" Then
            Me.GenerateIndividualValue()
        End If

        CalculateStats()

        If loadedHP = False Then
            Me.HP = Me.MaxHP
        Else
            Me.HP = Me.HP.Clamp(0, Me.MaxHP)
        End If
    End Sub

    ''' <summary>
    ''' Returns the save data from the Pokémon.
    ''' </summary>
    Public Function GetSaveData() As String
        Dim SaveGender As Integer = 0
        If Me.Gender = Genders.Female Then
            SaveGender = 1
        End If
        If Me.IsGenderless = True Then
            SaveGender = 2
        End If

        Dim SaveStatus As String = ""
        Select Case Me.Status
            Case StatusProblems.Burn
                SaveStatus = "BRN"
            Case StatusProblems.Poison
                SaveStatus = "PSN"
            Case StatusProblems.Paralyzed
                SaveStatus = "PRZ"
            Case StatusProblems.Sleep
                SaveStatus = "SLP"
            Case StatusProblems.Fainted
                SaveStatus = "FNT"
            Case StatusProblems.Freeze
                SaveStatus = "FRZ"
            Case StatusProblems.BadPoison
                SaveStatus = "BPSN"
        End Select

        Dim A1 As String = ""
        If Attacks.Count > 0 Then
            If Not Attacks(0) Is Nothing Then
                A1 = Me.Attacks(0).ToString()
            End If
        End If

        Dim A2 As String = ""
        If Attacks.Count > 1 Then
            If Not Attacks(1) Is Nothing Then
                A2 = Me.Attacks(1).ToString()
            End If
        End If

        Dim A3 As String = ""
        If Attacks.Count > 2 Then
            If Not Attacks(2) Is Nothing Then
                A3 = Me.Attacks(2).ToString()
            End If
        End If

        Dim A4 As String = ""
        If Attacks.Count > 3 Then
            If Not Attacks(3) Is Nothing Then
                A4 = Me.Attacks(3).ToString()
            End If
        End If

        Dim SaveItemID As String = "0"
        If Not Me.Item Is Nothing Then
            SaveItemID = Me.Item.ID.ToString()
        End If

        Dim ItemData As String = ""
        If Not Me.Item Is Nothing Then
            ItemData = Me.Item.AdditionalData
        End If

        Dim EVSave As String = Me.EVHP & "," & Me.EVAttack & "," & Me.EVDefense & "," & Me.EVSpAttack & "," & Me.EVSpDefense & "," & Me.EVSpeed
        Dim IVSave As String = Me.IVHP & "," & Me.IVAttack & "," & Me.IVDefense & "," & Me.IVSpAttack & "," & Me.IVSpDefense & "," & Me.IVSpeed

        Dim shinyString As String = "0"
        If Me.IsShiny = True Then
            shinyString = "1"
        End If

        If Me.Ability Is Nothing Then
            Me.Ability = Me.NewAbilities(Core.Random.Next(0, Me.NewAbilities.Count))
        End If

        Dim Data As String = "{""Pokemon""[" & Me.Number & "]}" &
        "{""Experience""[" & Me.Experience & "]}" &
        "{""Gender""[" & SaveGender & "]}" &
        "{""EggSteps""[" & Me.EggSteps & "]}" &
        "{""Item""[" & SaveItemID & "]}" &
        "{""ItemData""[" & ItemData & "]}" &
        "{""NickName""[" & Me.NickName & "]}" &
        "{""Level""[" & Me.Level & "]}" &
        "{""OT""[" & Me.OT & "]}" &
        "{""Ability""[" & Me.Ability.ID & "]}" &
        "{""Status""[" & SaveStatus & "]}" &
        "{""Nature""[" & Me.Nature & "]}" &
        "{""CatchLocation""[" & Me.CatchLocation & "]}" &
        "{""CatchTrainer""[" & Me.CatchTrainerName & "]}" &
        "{""CatchBall""[" & Me.CatchBall.ID & "]}" &
        "{""CatchMethod""[" & Me.CatchMethod & "]}" &
        "{""Friendship""[" & Me.Friendship & "]}" &
        "{""isShiny""[" & shinyString & "]}" &
        "{""Attack1""[" & A1 & "]}" &
        "{""Attack2""[" & A2 & "]}" &
        "{""Attack3""[" & A3 & "]}" &
        "{""Attack4""[" & A4 & "]}" &
        "{""HP""[" & Me.HP & "]}" &
        "{""EVs""[" & EVSave & "]}" &
        "{""IVs""[" & IVSave & "]}" &
        "{""AdditionalData""[" & Me.AdditionalData & "]}" &
        "{""IDValue""[" & Me.IndividualValue & "]}"

        Return Data
    End Function

    ''' <summary>
    ''' Generates a Pokémon's initial values.
    ''' </summary>
    ''' <param name="newLevel">The level to set the Pokémon's level to.</param>
    ''' <param name="SetParameters">If the parameters like Nature and Ability should be set. Otherwise, it just loads the attacks and sets the level.</param>
    Public Sub Generate(ByVal newLevel As Integer, ByVal SetParameters As Boolean)
        Me.Level = 0

        If SetParameters = True Then
            Me.GenerateIndividualValue()
            Me.AdditionalData = PokemonForms.GetInitialAdditionalData(Me)

            Me.Nature = CType(Core.Random.Next(0, 25), Natures)

            'Synchronize ability:
            If Core.Player.Pokemons.Count > 0 Then
                If Core.Player.Pokemons(0).Ability.Name.ToLower() = "synchronize" Then
                    If Core.Random.Next(0, 100) < 50 Then
                        Me.Nature = Core.Player.Pokemons(0).Nature
                    End If
                End If
            End If

            If Screen.Level IsNot Nothing Then
                If Screen.Level.HiddenAbilityChance > Core.Random.Next(0, 100) And Me.HasHiddenAbility = True Then
                    Me.Ability = net.Pokemon3D.Game.Ability.GetAbilityByID(Me.HiddenAbility.ID)
                Else
                    Me.Ability = net.Pokemon3D.Game.Ability.GetAbilityByID(Me.NewAbilities(Core.Random.Next(0, Me.NewAbilities.Count)).ID)
                End If
            End If

            Dim shinyRate As Integer = 8192

            For Each mysteryEvent As MysteryEventScreen.MysteryEvent In MysteryEventScreen.ActivatedMysteryEvents
                If mysteryEvent.EventType = MysteryEventScreen.EventTypes.ShinyMultiplier Then
                    shinyRate = CInt(shinyRate / CSng(mysteryEvent.Value.Replace(".", GameController.DecSeparator)))
                End If
            Next

            'ShinyCharm
            If Core.Player.Inventory.GetItemAmount(242) > 0 Then
                shinyRate = CInt(shinyRate * 0.75F)
            End If

            If Core.Random.Next(0, shinyRate) = 0 And Pokemon.Legendaries.Contains(Me.Number) = False Then
                Me.IsShiny = True
            End If

            If Me.IsGenderless = True Then
                Me.Gender = Genders.Genderless
            Else
                'Determine if Pokemon is male or female depending on the rate defined in the data file:
                If Core.Random.Next(1, 101) > Me.IsMale Then
                    Me.Gender = Genders.Female
                Else
                    Me.Gender = Genders.Male
                End If

                'Cute Charm ability:
                If Core.Player.Pokemons.Count > 0 Then
                    If Core.Player.Pokemons(0).Gender <> Genders.Genderless And Core.Player.Pokemons(0).Ability.Name.ToLower() = "cute charm" Then
                        If Core.Random.Next(0, 100) < 67 Then
                            If Core.Player.Pokemons(0).Gender = Genders.Female Then
                                Me.Gender = Genders.Male
                            Else
                                Me.Gender = Genders.Female
                            End If
                        Else
                            Me.Gender = Core.Player.Pokemons(0).Gender
                        End If
                    End If
                End If
            End If

            If StartItems.Count > 0 Then
                Dim i As Integer = Core.Random.Next(0, StartItems.Count)
                If Core.Random.Next(0, StartItems.Values(i)) = 0 Then
                    Me.Item = StartItems.Keys(i)
                End If
            End If

            'Set the IV values of the Pokémon randomly, range 0-31.
            Me.IVHP = Core.Random.Next(0, 32)
            Me.IVAttack = Core.Random.Next(0, 32)
            Me.IVDefense = Core.Random.Next(0, 32)
            Me.IVSpAttack = Core.Random.Next(0, 32)
            Me.IVSpDefense = Core.Random.Next(0, 32)
            Me.IVSpeed = Core.Random.Next(0, 32)

            Me.Friendship = Me.BaseFriendship

            If WildItems.Count > 0 Then
                Dim has100 As Boolean = False
                Dim ChosenItemID As Integer = 0
                For i = 0 To WildItems.Count - 1
                    If WildItems.Keys(i) = 100 Then
                        has100 = True
                        ChosenItemID = WildItems.Values(i)
                        Exit For
                    End If
                Next
                If has100 = True Then
                    Me.Item = Item.GetItemByID(ChosenItemID)
                Else
                    Dim usedWildItems As Dictionary(Of Integer, Integer) = Me.WildItems

                    'Compound eyes ability:
                    If Core.Player.Pokemons.Count > 0 Then
                        If Core.Player.Pokemons(0).Ability.Name.ToLower() = "compound eyes" Then
                            usedWildItems = Abilities.Compoundeyes.ConvertItemChances(usedWildItems)
                        End If
                    End If

                    For i = 0 To usedWildItems.Count - 1
                        Dim v As Integer = Core.Random.Next(0, 100)
                        If v < usedWildItems.Keys(i) Then
                            ChosenItemID = usedWildItems.Values(i)
                            Exit For
                        End If
                    Next
                    Me.Item = Item.GetItemByID(ChosenItemID)
                End If
            End If
        End If

        'Level the Pokémon up and give the appropriate move set for its new level:
        While newLevel > Me.Level
            LevelUp(False)
            Me.Experience = NeedExperience(Me.Level)
        End While

        Dim canLearnMoves As New List(Of BattleSystem.Attack)
        For i = 0 To Me.AttackLearns.Count - 1
            If Me.AttackLearns.Keys(i) <= Me.Level Then

                Dim hasMove As Boolean = False
                For Each m As BattleSystem.Attack In Me.Attacks
                    If m.ID = Me.AttackLearns.Values(i).ID Then
                        hasMove = True
                        Exit For
                    End If
                Next
                If hasMove = False Then
                    For Each m As BattleSystem.Attack In canLearnMoves
                        If m.ID = Me.AttackLearns.Values(i).ID Then
                            hasMove = True
                            Exit For
                        End If
                    Next
                End If

                If hasMove = False Then
                    canLearnMoves.Add(Me.AttackLearns.Values(i))
                End If
            End If
        Next

        If canLearnMoves.Count > 0 Then
            Me.Attacks.Clear()

            Dim startIndex As Integer = canLearnMoves.Count - 4

            If startIndex < 0 Then
                startIndex = 0
            End If

            For t = startIndex To canLearnMoves.Count - 1
                Me.Attacks.Add(canLearnMoves(t))
            Next
        End If

        Me.HP = MaxHP
    End Sub

#End Region

#Region "Converters"

    ''' <summary>
    ''' Converts an EggGroup ID string to the EggGroup enum item.
    ''' </summary>
    ''' <param name="ID">The ID string.</param>
    Public Shared Function ConvertIDToEggGroup(ByVal ID As String) As EggGroups
        Select Case ID.ToLower()
            Case "monster"
                Return EggGroups.Monster
            Case "water1"
                Return EggGroups.Water1
            Case "water2"
                Return EggGroups.Water2
            Case "water3"
                Return EggGroups.Water3
            Case "bug"
                Return EggGroups.Bug
            Case "flying"
                Return EggGroups.Flying
            Case "field"
                Return EggGroups.Field
            Case "fairy"
                Return EggGroups.Fairy
            Case "grass"
                Return EggGroups.Grass
            Case "undiscovered"
                Return EggGroups.Undiscovered
            Case "humanlike"
                Return EggGroups.HumanLike
            Case "mineral"
                Return EggGroups.Mineral
            Case "amorphous"
                Return EggGroups.Amorphous
            Case "ditto"
                Return EggGroups.Ditto
            Case "dragon"
                Return EggGroups.Dragon
            Case "genderunknown"
                Return EggGroups.GenderUnknown
            Case "none", "", "0", "nothing"
                Return EggGroups.None
        End Select

        Return EggGroups.None
    End Function

    ''' <summary>
    ''' Converts a Nature ID to a Nature enum item.
    ''' </summary>
    ''' <param name="ID">The nature ID.</param>
    Public Shared Function ConvertIDToNature(ByVal ID As Integer) As Natures
        Select Case ID
            Case 0
                Return Natures.Hardy
            Case 1
                Return Natures.Lonely
            Case 2
                Return Natures.Brave
            Case 3
                Return Natures.Adamant
            Case 4
                Return Natures.Naughty
            Case 5
                Return Natures.Bold
            Case 6
                Return Natures.Docile
            Case 7
                Return Natures.Relaxed
            Case 8
                Return Natures.Impish
            Case 9
                Return Natures.Lax
            Case 10
                Return Natures.Timid
            Case 11
                Return Natures.Hasty
            Case 12
                Return Natures.Serious
            Case 13
                Return Natures.Jolly
            Case 14
                Return Natures.Naive
            Case 15
                Return Natures.Modest
            Case 16
                Return Natures.Mild
            Case 17
                Return Natures.Quiet
            Case 18
                Return Natures.Bashful
            Case 19
                Return Natures.Rash
            Case 20
                Return Natures.Calm
            Case 21
                Return Natures.Gentle
            Case 22
                Return Natures.Sassy
            Case 23
                Return Natures.Careful
            Case 24
                Return Natures.Quirky
            Case Else
                Return Natures.Hardy
        End Select
    End Function

#End Region

    ''' <summary>
    ''' Generates a new individual value for this Pokémon.
    ''' </summary>
    Private Sub GenerateIndividualValue()
        Dim chars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"

        Dim s As String = ""
        For x = 0 To 10
            s &= chars(Core.Random.Next(0, chars.Length)).ToString()
        Next

        Me.IndividualValue = s
    End Sub

    ''' <summary>
    ''' Returns the Display Name of this Pokémon.
    ''' </summary>
    ''' <remarks>Returns "Egg" when the Pokémon is in an egg. Returns the properly translated name if it exists. Returns the nickname if set.</remarks>
    Public Function GetDisplayName() As String
        If Me.EggSteps > 0 Then
            Return "Egg"
        Else
            If Me.NickName = "" Then
                If Localization.TokenExists("pokemon_name_" & Me.Name) = True Then
                    Return Localization.GetString("pokemon_name_" & Me.Name)
                Else
                    Return Me.Name
                End If
            Else
                Return Me.NickName
            End If
        End If
    End Function

    ''' <summary>
    ''' Returns the properly translated name of a Pokémon if defined in the language files.
    ''' </summary>
    Public Function GetName() As String
        If Localization.TokenExists("pokemon_name_" & Me.Name) = True Then
            Return Localization.GetString("pokemon_name_" & Me.Name)
        Else
            Return Me.Name
        End If
    End Function

    ''' <summary>
    ''' Returns the English name of the Pokémon.
    ''' </summary>
    Public Property OriginalName As String
        Get
            Return Me.Name
        End Get
        Set(value As String)
            Me.Name = value
        End Set
    End Property

#Region "Experience, Level Up and Stats"

    ''' <summary>
    ''' Gives the Pokémon experience points and levels it up.
    ''' </summary>
    ''' <param name="Exp">The amount of EXP.</param>
    ''' <param name="LearnRandomAttack">If the Pokémon should learn an attack if it could learn one at level up.</param>
    Public Sub GetExperience(ByVal Exp As Integer, ByVal LearnRandomAttack As Boolean)
        Me.Experience += Exp
        While Me.Experience >= NeedExperience(Me.Level + 1)
            Me.LevelUp(LearnRandomAttack)
        End While
        Me.Level = Me.Level.Clamp(1, CInt(GameModeManager.GetGameRuleValue("MaxLevel", "100")))
    End Sub

    ''' <summary>
    ''' Rasies the Pokémon's level by one.
    ''' </summary>
    ''' <param name="LearnRandomAttack">If one attack of the Pokémon should be replaced by an attack potentially learned on the new level.</param>
    Public Sub LevelUp(ByVal LearnRandomAttack As Boolean)
        Me.Level += 1

        Dim currentMaxHP As Integer = Me.MaxHP

        Me.CalculateStats()

        'Heals the Pokémon by the HP difference.
        Dim HPDifference As Integer = Me.MaxHP - currentMaxHP
        If HPDifference > 0 Then
            Me.Heal(HPDifference)
        End If

        If LearnRandomAttack = True Then
            Me.LearnAttack(Me.Level)
        End If
    End Sub

    ''' <summary>
    ''' Recalculates all stats for this Pokémon using its current EVs, IVs and level.
    ''' </summary>
    ''' 
    Public Sub CalculateStatsBarSpeed()
        Me.MaxHP = CalcStatus(Me.Level, True, Me.BaseHP, Me.EVHP, Me.IVHP, "HP")
        Me.Attack = CalcStatus(Me.Level, False, Me.BaseAttack, Me.EVAttack, Me.IVAttack, "Attack")
        Me.Defense = CalcStatus(Me.Level, False, Me.BaseDefense, Me.EVDefense, Me.IVDefense, "Defense")
        Me.SpAttack = CalcStatus(Me.Level, False, Me.BaseSpAttack, Me.EVSpAttack, Me.IVSpAttack, "SpAttack")
        Me.SpDefense = CalcStatus(Me.Level, False, Me.BaseSpDefense, Me.EVSpDefense, Me.IVSpDefense, "SpDefense")
    End Sub

    Public Sub CalculateStats()
        CalculateStatsBarSpeed()
        Me.Speed = CalcStatus(Me.Level, False, Me.BaseSpeed, Me.EVSpeed, Me.IVSpeed, "Speed")
    End Sub

    ''' <summary>
    ''' Gets the value of a status.
    ''' </summary>
    ''' <param name="calcLevel">The level of the Pokémon.</param>
    ''' <param name="DoHP">If the requested stat is HP.</param>
    ''' <param name="baseStat">The base stat of the Pokémon.</param>
    ''' <param name="EVStat">The EV stat of the Pokémon.</param>
    ''' <param name="IVStat">The IV stat of the Pokémon.</param>
    ''' <param name="StatName">The name of the stat.</param>
    Private Function CalcStatus(ByVal calcLevel As Integer, ByVal DoHP As Boolean, ByVal baseStat As Integer, ByVal EVStat As Integer, ByVal IVStat As Integer, ByVal StatName As String) As Integer
        If DoHP = True Then
            If Me.Number = 292 Then
                Return 1
            Else
                Return CInt(Math.Floor((((IVStat + (2 * baseStat) + (EVStat / 4) + 100) * calcLevel) / 100) + 10))
            End If
        Else
            Return CInt(Math.Floor(((((IVStat + (2 * baseStat) + (EVStat / 4)) * calcLevel) / 100) + 5) * net.Pokemon3D.Game.Nature.GetMultiplier(Me.Nature, StatName)))
        End If
    End Function

    ''' <summary>
    ''' Replaces a random move of the Pokémon by one that it learns on a given level.
    ''' </summary>
    ''' <param name="learnLevel">The level the Pokémon learns the desired move on.</param>
    Public Sub LearnAttack(ByVal learnLevel As Integer)
        If AttackLearns.ContainsKey(learnLevel) = True Then
            Dim a As BattleSystem.Attack = AttackLearns(learnLevel)

            For Each la As BattleSystem.Attack In Attacks
                If la.ID = a.ID Then
                    Exit Sub 'Pokémon already knows that attack.
                End If
            Next

            Me.Attacks.Add(a)

            If Me.Attacks.Count = 5 Then
                Attacks.RemoveAt(Core.Random.Next(0, 5))
            End If
        End If
    End Sub

    ''' <summary>
    ''' Returns the EXP needed for the given level.
    ''' </summary>
    ''' <param name="EXPLevel">The level this function should return the exp amount for.</param>
    Public Function NeedExperience(ByVal EXPLevel As Integer) As Integer
        Dim n As Integer = EXPLevel
        Dim i As Integer = 0

        Select Case Me.ExperienceType
            Case ExperienceTypes.Fast
                i = CInt((4 * n * n * n) / 5)
            Case ExperienceTypes.MediumFast
                i = CInt(n * n * n)
            Case ExperienceTypes.MediumSlow
                i = CInt(((6 * n * n * n) / 5) - (15 * n * n) + (100 * n) - 140)
            Case ExperienceTypes.Slow
                i = CInt((5 * n * n * n) / 4)
            Case Else
                i = CInt(n * n * n)
        End Select

        If i < 0 Then
            i = 0
        End If

        Return i
    End Function

    ''' <summary>
    ''' Returns the cummulative PP count of all moves.
    ''' </summary>
    Public Function CountPP() As Integer
        Dim AllPP As Integer = 0
        For Each Attack As BattleSystem.Attack In Attacks
            AllPP += Attack.CurrentPP
        Next
        Return AllPP
    End Function

    ''' <summary>
    ''' Fully heals this Pokémon.
    ''' </summary>
    Public Sub FullRestore()
        Me.Status = StatusProblems.None
        Me.Heal(Me.MaxHP)
        Me._volatiles.Clear()
        If Me.Attacks.Count > 0 Then
            For d = 0 To Me.Attacks.Count - 1
                Me.Attacks(d).CurrentPP = Me.Attacks(d).MaxPP
            Next
        End If
    End Sub

    ''' <summary>
    ''' Heals the Pokémon.
    ''' </summary>
    ''' <param name="HealHP">The amount of HP to heal the Pokémon by.</param>
    Public Sub Heal(ByVal HealHP As Integer)
        Me.HP = (Me.HP + HealHP).Clamp(0, Me.MaxHP)
    End Sub

    ''' <summary>
    ''' Changes the Friendship value of this Pokémon.
    ''' </summary>
    ''' <param name="cause">The cause as to why the Friendship value should change.</param>
    Public Sub ChangeFriendShip(ByVal cause As FriendShipCauses)
        Dim add As Integer = 0

        Select Case cause
            Case FriendShipCauses.Walking
                add = 1
            Case FriendShipCauses.LevelUp
                If Me.Friendship <= 99 Then
                    add = 5
                ElseIf Me.Friendship > 99 And Me.Friendship <= 199 Then
                    add = 3
                Else
                    add = 2
                End If
            Case FriendShipCauses.Fainting
                Me.Friendship -= 1
            Case FriendShipCauses.EnergyPowder, FriendShipCauses.HealPowder
                If Me.Friendship <= 99 Then
                    add = -5
                ElseIf Me.Friendship > 99 And Me.Friendship <= 199 Then
                    add = -5
                Else
                    add = -10
                End If
            Case FriendShipCauses.EnergyRoot
                If Me.Friendship <= 99 Then
                    add = -10
                ElseIf Me.Friendship > 99 And Me.Friendship <= 199 Then
                    add = -10
                Else
                    add = -15
                End If
            Case FriendShipCauses.RevivalHerb
                If Me.Friendship <= 99 Then
                    add = -15
                ElseIf Me.Friendship > 99 And Me.Friendship <= 199 Then
                    add = -15
                Else
                    add = -20
                End If
            Case FriendShipCauses.Trading
                Me.Friendship = Me.BaseFriendship
            Case FriendShipCauses.Vitamin
                If Me.Friendship <= 99 Then
                    add = 5
                ElseIf Me.Friendship > 99 And Me.Friendship <= 199 Then
                    add = 3
                Else
                    add = 2
                End If
            Case FriendShipCauses.EVBerry
                If Me.Friendship <= 99 Then
                    add = 10
                ElseIf Me.Friendship > 99 And Me.Friendship <= 199 Then
                    add = 5
                Else
                    add = 2
                End If
        End Select

        If add > 0 Then
            If Me.CatchBall.ID = 174 Then
                add += 1
            End If
            If Not Me.Item Is Nothing Then
                If Me.Item.Name.ToLower() = "soothe bell" Then
                    add *= 2
                End If
            End If
        End If

        If add <> 0 Then
            Me.Friendship += add
        End If

        Me.Friendship = CInt(MathHelper.Clamp(Me.Friendship, 0, 255))
    End Sub

#End Region

#Region "Textures/Models"

    ''' <summary>
    ''' Returns a texture for this Pokémon.
    ''' </summary>
    ''' <param name="index">0=normal,front
    ''' 1=normal,back
    ''' 2=shiny,front
    ''' 3=shiny,back
    ''' 4=menu sprite
    ''' 5=egg menu sprite
    ''' 6=Egg front sprite
    ''' 7=Egg back sprite
    ''' 8=normal overworld
    ''' 9=shiny overworld
    ''' 10=normal,front,animation</param>
    Private Function GetTexture(ByVal index As Integer) As Texture2D
        If Textures(index) Is Nothing Then
            Select Case index
                Case 0
                    Textures(index) = net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Sprites\" & Me.AnimationName, New Rectangle(0, 0, 128, 128), "")
                Case 1
                    Textures(index) = net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Sprites\" & Me.AnimationName, New Rectangle(128, 0, 128, 128), "")
                Case 2
                    Textures(index) = net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Sprites\" & Me.AnimationName, New Rectangle(0, 128, 128, 128), "")
                Case 3
                    Textures(index) = net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Sprites\" & Me.AnimationName, New Rectangle(128, 128, 128, 128), "")
                Case 4
                    Dim v As Vector2 = PokemonForms.GetMenuImagePosition(Me)
                    Dim s As Size = PokemonForms.GetMenuImageSize(Me)

                    Dim shiny As String = ""
                    If Me.IsShiny = True Then
                        shiny = "Shiny"
                    End If

                    Textures(index) = net.Pokemon3D.Game.TextureManager.GetTexture("GUI\PokemonMenu" & shiny, New Rectangle(CInt(v.X) * 32, CInt(v.Y) * 32, s.Width, s.Height), "")
                Case 5
                    Textures(index) = EggCreator.CreateEggSprite(Me, net.Pokemon3D.Game.TextureManager.GetTexture("GUI\PokemonMenu", New Rectangle(992, 992, 32, 32), ""), net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Egg\Templates\Menu"))
                Case 6
                    Textures(index) = EggCreator.CreateEggSprite(Me, net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Egg\Egg_front"), net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Egg\Templates\Front"))
                Case 7
                    Textures(index) = EggCreator.CreateEggSprite(Me, net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Egg\Egg_back"), net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Egg\Templates\Back"))
                Case 8
                    Dim addition As String = PokemonForms.GetOverworldAddition(Me)
                    Textures(index) = net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Overworld\Normal\" & Me.Number & addition)
                Case 9
                    Dim addition As String = PokemonForms.GetOverworldAddition(Me)
                    Textures(index) = net.Pokemon3D.Game.TextureManager.GetTexture("Pokemon\Overworld\Shiny\" & Me.Number & addition)
            End Select
        End If

        Return Textures(index)
    End Function

    ''' <summary>
    ''' Returns the Overworld texture of this Pokémon.
    ''' </summary>
    Public Function GetOverworldTexture() As Texture2D
        If Me.IsShiny = False Then
            Return Me.GetTexture(8)
        Else
            Return Me.GetTexture(9)
        End If
    End Function

    ''' <summary>
    ''' Returns the Menu Texture of this Pokémon.
    ''' </summary>
    ''' <param name="CanGetEgg">If the texture returned can represent the Pokémon in its egg.</param>
    Public Function GetMenuTexture(ByVal CanGetEgg As Boolean) As Texture2D
        If Me.EggSteps > 0 And CanGetEgg = True Then
            Return GetTexture(5)
        Else
            Return GetTexture(4)
        End If
    End Function

    ''' <summary>
    ''' Returns the Menu Texture of this Pokémon.
    ''' </summary>
    Public Function GetMenuTexture() As Texture2D
        Return GetMenuTexture(True)
    End Function

    ''' <summary>
    ''' Returns the display texture of this Pokémon.
    ''' </summary>
    ''' <param name="FrontView">If this Pokémon should be viewed from the front.</param>
    Public Function GetTexture(ByVal FrontView As Boolean) As Texture2D
        If FrontView = True Then
            If Me.IsEgg() = True Then
                Return GetTexture(6)
            Else
                If IsShiny = True Then
                    Return GetTexture(2)
                Else
                    Return GetTexture(0)
                End If
            End If
        Else
            If Me.IsEgg() = True Then
                Return GetTexture(7)
            Else
                If IsShiny = True Then
                    Return GetTexture(3)
                Else
                    Return GetTexture(1)
                End If
            End If
        End If
    End Function

    ''' <summary>
    ''' Returns properties to display models on a 2D GUI. Data structure: scale sng,posX sng,posY sng,posZ sng,roll sng
    ''' </summary>
    Public Function GetModelProperties() As Tuple(Of Single, Single, Single, Single, Single)
        Dim scale As Single = CSng(0.6 / Me.PokedexEntry.Height)
        Dim x As Single = 0.0F
        Dim y As Single = 0.0F
        Dim z As Single = 0.0F

        Dim roll As Single = 0.3F

        Select Case Me.Number
            Case 6
                scale = 0.55
            Case 9
                scale = 0.7F
            Case 15
                z = 4.0F
            Case 19
                scale = 1.1
            Case 20
                scale = 1.3
            Case 23
                scale = 1
            Case 24
                scale = 0.5
            Case 41
                z = 5.0F
            Case 55
                scale = 0.7
            Case 63
                z = -4.0F
            Case 74
                scale = 0.75
            Case 81
                z = 6.0F
            Case 82
                scale = 0.9
                z = 6.0F
            Case 95
                x = -6
                scale = 0.3
            Case 98
                scale = 1
            Case 102
                scale = 0.9
            Case 103
                scale = 0.45
            Case 115
                scale = 0.45
            Case 129
                z = -4.0F
            Case 130
                scale = 0.25
            Case 131
                z = -8
            Case 143
                scale = 0.5
                roll = 1.2F
            Case 144
                z = -9
                scale = 0.35
            Case 147
                scale = 0.7
            Case 148
                x = 5.0F
                scale = 0.4
            Case 149, 150
                scale = 0.42F
            Case 151
                z = 5
            Case 157
                scale = 0.6
            Case 160
                scale = 0.5
            Case 162
                scale = 0.8
            Case 164
                z = -3
            Case 168
                scale = 0.8
            Case 180
                scale = 0.5
            Case 181
                scale = 0.75
            Case 184, 185
                scale = 0.8
            Case 187
                scale = 0.65
            Case 196, 197
                scale = 0.900000036F
            Case 206
                scale = 0.9F
            Case 208
                scale = 0.4
            Case 211
                z = 5
            Case 212
                scale = 0.7
            Case 214
                scale = 1.2
            Case 217
                scale = 0.55
            Case 223
                z = -5
            Case 229
                scale = 0.8
            Case 230
                scale = 0.6
                z = 3
            Case 235
                scale = 0.7
            Case 241
                scale = 0.7
            Case 247
                scale = 0.7
            Case 248
                scale = 0.6
            Case 249
                scale = 0.3
            Case 250
                scale = 0.2
            Case 336
                scale = 0.8
        End Select

        Return New Tuple(Of Single, Single, Single, Single, Single)(scale, x, y, z, roll)
    End Function

#End Region

    ''' <summary>
    ''' Checks if this Pokémon can evolve by a given EvolutionTrigger and EvolutionArgument.
    ''' </summary>
    ''' <param name="trigger">The trigger of the evolution.</param>
    ''' <param name="argument">An argument that specifies the evolution.</param>
    Public Function CanEvolve(ByVal trigger As EvolutionCondition.EvolutionTrigger, ByVal argument As String) As Boolean
        Dim n As Integer = EvolutionCondition.EvolutionNumber(Me, trigger, argument)
        If n = 0 Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' Returns the evolution ID of this Pokémon.
    ''' </summary>
    ''' <param name="trigger">The trigger of the evolution.</param>
    ''' <param name="argument">An argument that specifies the evolution.</param>
    Public Function GetEvolutionID(ByVal trigger As EvolutionCondition.EvolutionTrigger, ByVal argument As String) As Integer
        Return EvolutionCondition.EvolutionNumber(Me, trigger, argument)
    End Function

    ''' <summary>
    ''' Sets the catch infos of the Pokémon. Uses the current map name and player name + OT.
    ''' </summary>
    ''' <param name="Ball">The Pokéball this Pokémon got captured in.</param>
    ''' <param name="Method">The capture method.</param>
    Public Sub SetCatchInfos(ByVal Ball As Item, ByVal Method As String)
        Me.CatchLocation = Screen.Level.MapName
        Me.CatchTrainerName = Core.Player.Name
        Me.OT = Core.Player.OT

        Me.CatchMethod = Method
        Me.CatchBall = Ball
    End Sub

    ''' <summary>
    ''' Checks if the Pokémon is of a certain type.
    ''' </summary>
    ''' <param name="CheckType">The type to check.</param>
    Public Function IsType(ByVal CheckType As Element.Types) As Boolean
        If Type1.Type = CheckType Or Type2.Type = CheckType Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Plays the cry of this Pokémon.
    ''' </summary>
    Public Sub PlayCry()
        Dim Pitch As Single = 0.0F
        Dim percent As Integer = 100
        If Me.HP > 0 And Me.MaxHP > 0 Then
            percent = CInt(Math.Ceiling(Me.HP / Me.MaxHP) * 100)
        End If

        If percent <= 50 Then
            Pitch = -0.4F
        End If
        If percent <= 15 Then
            Pitch = -0.8F
        End If
        If percent = 0 Then
            Pitch = -1.0F
        End If

        SoundManager.PlayPokemonCry(Me.Number, Pitch, 0F)
    End Sub

    ''' <summary>
    ''' Checks if this Pokémon knows a certain move.
    ''' </summary>
    ''' <param name="Move">The move to check for.</param>
    Public Function KnowsMove(ByVal Move As BattleSystem.Attack) As Boolean
        For Each a As BattleSystem.Attack In Me.Attacks
            If a.ID = Move.ID Then
                Return True
            End If
        Next

        Return False
    End Function

    ''' <summary>
    ''' Checks if this Pokémon is inside an Egg.
    ''' </summary>
    Public Function IsEgg() As Boolean
        If Me.EggSteps > 0 Then
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Adds Effort values (EV) to this Pokémon after defeated another Pokémon, if possible.
    ''' </summary>
    ''' <param name="DefeatedPokemon">The defeated Pokémon.</param>
    Public Sub GainEffort(ByVal DefeatedPokemon As Pokemon)
        Dim allEV As Integer = EVHP + EVAttack + EVDefense + EVSpeed + EVSpAttack + EVSpDefense
        If allEV < 510 Then
            Dim maxGainEV As Integer = 0
            If allEV < 510 Then
                maxGainEV = 510 - allEV
            End If
            If maxGainEV > 0 Then
                maxGainEV = CInt(MathHelper.Clamp(maxGainEV, 1, 3))

                If Me.EVHP < 255 And DefeatedPokemon.GiveEVHP > 0 Then
                    Dim gainHPEV As Integer = DefeatedPokemon.GiveEVHP
                    gainHPEV = CInt(MathHelper.Clamp(gainHPEV, 0, 255 - Me.EVHP))
                    Me.EVHP += gainHPEV
                End If

                If Me.EVAttack < 255 And DefeatedPokemon.GiveEVAttack > 0 Then
                    Dim gainAttackEV As Integer = DefeatedPokemon.GiveEVAttack
                    gainAttackEV = CInt(MathHelper.Clamp(gainAttackEV, 0, 255 - Me.EVAttack))
                    Me.EVAttack += gainAttackEV
                End If

                If Me.EVDefense < 255 And DefeatedPokemon.GiveEVDefense > 0 Then
                    Dim gainDefenseEV As Integer = DefeatedPokemon.GiveEVDefense
                    gainDefenseEV = CInt(MathHelper.Clamp(gainDefenseEV, 0, 255 - Me.EVDefense))
                    Me.EVDefense += gainDefenseEV
                End If

                If Me.EVSpeed < 255 And DefeatedPokemon.GiveEVSpeed > 0 Then
                    Dim gainSpeedEV As Integer = DefeatedPokemon.GiveEVSpeed
                    gainSpeedEV = CInt(MathHelper.Clamp(gainSpeedEV, 0, 255 - Me.EVSpeed))
                    Me.EVSpeed += gainSpeedEV
                End If

                If Me.EVSpAttack < 255 And DefeatedPokemon.GiveEVSpAttack > 0 Then
                    Dim gainSpAttackEV As Integer = DefeatedPokemon.GiveEVSpAttack
                    gainSpAttackEV = CInt(MathHelper.Clamp(gainSpAttackEV, 0, 255 - Me.EVSpAttack))
                    Me.EVSpAttack += gainSpAttackEV
                End If

                If Me.EVSpDefense < 255 And DefeatedPokemon.GiveEVSpDefense > 0 Then
                    Dim gainSpDefenseEV As Integer = DefeatedPokemon.GiveEVSpDefense
                    gainSpDefenseEV = CInt(MathHelper.Clamp(gainSpDefenseEV, 0, 255 - Me.EVSpDefense))
                    Me.EVSpDefense += gainSpDefenseEV
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Returns if this Pokémon knows an HM move.
    ''' </summary>
    Public Function HasHMMove() As Boolean
        For Each m As BattleSystem.Attack In Me.Attacks
            If m.IsHMMove = True Then
                Return True
            End If
        Next
        Return False
    End Function

    ''' <summary>
    ''' Returns if this Pokémon is fully evolved.
    ''' </summary>
    Public Function IsFullyEvolved() As Boolean
        Return Me.EvolutionConditions.Count = 0
    End Function

    ''' <summary>
    ''' Checks if this Pokémon has a Hidden Ability assigend to it.
    ''' </summary>
    Public ReadOnly Property HasHiddenAbility() As Boolean
        Get
            Return Not Me.HiddenAbility Is Nothing
        End Get
    End Property

    ''' <summary>
    ''' Checks if the Pokémon has its Hidden Ability set as its current ability.
    ''' </summary>
    Public ReadOnly Property IsUsingHiddenAbility() As Boolean
        Get
            If HasHiddenAbility() = True Then
                Return Me.HiddenAbility.ID = Me.Ability.ID
            End If
            Return False
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return GetSaveData()
    End Function
End Class
