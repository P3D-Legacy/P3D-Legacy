Namespace BattleSystem

    ''' <summary>
    ''' Represents a Pokémon in battle.
    ''' </summary>
    Public Class PokemonProfile

        ''' <summary>
        ''' Creates a new instance of the Pokémon Profile.
        ''' </summary>
        ''' <param name="Target">The Position on the battle field.</param>
        ''' <param name="Pokemon">The reference to the Pokémon in the party.</param>
        Public Sub New(ByVal Target As PokemonTarget, ByVal Pokemon As Pokemon)
            Me._fieldPosition = Target
            Me._pokemon = Pokemon
        End Sub

        Private _pokemon As Pokemon = Nothing
        Private _fieldPosition As PokemonTarget = PokemonTarget.OwnCenter

        ''' <summary>
        ''' The Pokémon reference class instance that is associated with this profile.
        ''' </summary>
        Public ReadOnly Property Pokémon As Pokemon
            Get
                Return Me._pokemon
            End Get
        End Property

        ''' <summary>
        ''' The position of this Pokémon on the field.
        ''' </summary>
        Public ReadOnly Property FieldPosition As PokemonTarget
            Get
                Return Me._fieldPosition
            End Get
        End Property

        'Stuff that needs to be in FieldEffects cause its an effect for one side of the field or the entire field:
        'HealingWish: Heal next Pokémon switched in on the side.
        'Spikes, Toxic Spikes, Stealth Rocks, Mist, Guard Spec: Full side, not single Pokémon
        'Turn Count: Turn count for the entire battle, not just the Pokémon.
        'Reflect, Tailwind, HealBlock, Safeguard, and LightScreen: Affects whole team.
        'Wish: Need to store the target.
        'RemoteMoves (RemoteDamage,RemoteTurns,RemoteTarget,RemoteMoveID): per Target (Foresight)
        'TrickRoom, Gravity, MudSport, WaterSport, RoundCount, Weather, WeatherRounds, AmuletCoinUsed
        'PayDay: All uses add up.

#Region "ProfileElements"

        ''' <summary>
        ''' The amount of damage this Pokémon took in the last attack.
        ''' </summary>
        Public LastDamageTaken As Integer = 0
        ''' <summary>
        ''' The counter that indicates how many turns this Pokémon has been in battle.
        ''' </summary>
        Public TurnsInBattle As Integer = 0
        ''' <summary>
        ''' If this Pokémon directly damaged an opponent last turn.
        ''' </summary>
        Public DealtDamageThisTurn As Boolean = False
        ''' <summary>
        ''' List of used moves (IDs)
        ''' </summary>
        Public UsedMoves As New List(Of Integer)
        ''' <summary>
        ''' An item this Pokémon possibly lost in battle.
        ''' </summary>
        Public LostItem As Item = Nothing
        ''' <summary>
        ''' If this Pokémon Mega Evolved in this battle.
        ''' </summary>
        Public MegaEvolved As Boolean = False
        ''' <summary>
        ''' The last move this Pokémon used.
        ''' </summary>
        Public LastMove As Attack = Nothing
        ''' <summary>
        ''' The turn number this Pokémon last used a move.
        ''' </summary>
        Public LastTurnMoved As Integer = 0

        ''' <summary>
        ''' Trigger, if the Pokémon wants to switch out but another one used Pursuit on it.
        ''' </summary>
        Public Pursuit As Boolean = False

        ''' <summary>
        ''' The turns until the Pokémon wakes up.
        ''' </summary>
        Public SleepTurns As Integer = 0
        ''' <summary>
        ''' Round counter for toxic.
        ''' </summary>
        Public ToxicRound As Integer = 0
        ''' <summary>
        ''' Rounds until Yawn affects the Pokémon.
        ''' </summary>
        Public Yawn As Integer = 0
        ''' <summary>
        ''' Turns until confusion runs out.
        ''' </summary>
        Public ConfusionTurns As Integer = 0

        ''' <summary>
        ''' The Truant ability counter. When this is 1, the Pokémon won't move this round.
        ''' </summary>
        Public TruantRound As Integer = 0 '

        ''' <summary>
        ''' Turns True if Imprison was used on this Pokémon.
        ''' </summary>
        Public Imprisoned As Boolean = False
        ''' <summary>
        ''' Taunt move counter, if true, Pokémon cannot use status moves.
        ''' </summary>
        Public Taunted As Integer = 0
        ''' <summary>
        ''' If the Pokémon is affected by Embargo.
        ''' </summary>
        Public Embargo As Integer = 0
        ''' <summary>
        ''' Rounds until Encore wears off.
        ''' </summary>
        Public Encore As Integer = 0
        ''' <summary>
        ''' The move reference that the encored Pokémon has to use.
        ''' </summary>
        Public EncoreMove As Attack = Nothing
        ''' <summary>
        ''' Rounds until Torment wears off.
        ''' </summary>
        Public Torment As Integer = 0
        ''' <summary>
        ''' The move that the Pokémon is forced to use.
        ''' </summary>
        Public TormentMoveID As Integer = -1
        ''' <summary>
        ''' The move chosen for Choice Items.
        ''' </summary>
        Public ChoiceMove As Attack = Nothing
        ''' <summary>
        ''' Turns true when the Pokémon is affected by Gastro Acid.
        ''' </summary>
        Public GastroAcid As Integer = 0

        ''' <summary>
        ''' The counter for the rage move.
        ''' </summary>
        Public Rage As Integer = 0
        ''' <summary>
        ''' Checks if Defense Curl was used.
        ''' </summary>
        Public DefenseCurl As Integer = 0
        ''' <summary>
        ''' Charge move counter.
        ''' </summary>
        Public Charge As Integer = 0
        ''' <summary>
        ''' Indicates if minimize got used.
        ''' </summary>
        Public Minimize As Integer = 0
        ''' <summary>
        ''' Counter for Bide
        ''' </summary>
        Public Bide As Integer = 0
        ''' <summary>
        ''' Stores all the damage that the Pokémon will deal with Bide.
        ''' </summary>
        Public BideDamage As Integer = 0
        ''' <summary>
        ''' Doubles effect probability for four turns.
        ''' </summary>
        Public WaterPledge As Integer = 0

        ''' <summary>
        ''' The counter for the Uproar move.
        ''' </summary>
        Public Uproar As Integer = 0
        ''' <summary>
        ''' The Outrage move counter.
        ''' </summary>
        Public Outrage As Integer = 0
        ''' <summary>
        ''' The Thrash move counter.
        ''' </summary>
        Public Thrash As Integer = 0
        ''' <summary>
        ''' The Petal Dance move counter.
        ''' </summary>
        Public PetalDance As Integer = 0
        ''' <summary>
        ''' Rollout move counter
        ''' </summary>
        Public Rollout As Integer = 0
        ''' <summary>
        ''' Iceball move counter
        ''' </summary>
        Public IceBall As Integer = 0
        ''' <summary>
        ''' Recharge counter for moves like Hyperbeam.
        ''' </summary>
        Public Recharge As Integer = 0
        ''' <summary>
        ''' Razor Wind move counter.
        ''' </summary>
        Public RazorWind As Integer = 0
        ''' <summary>
        ''' Skull bash Move counter.
        ''' </summary>
        Public SkullBash As Integer = 0
        ''' <summary>
        ''' Sky Attack move counter.
        ''' </summary>
        Public SkyAttack As Integer = 0
        ''' <summary>
        ''' Solar beam move counter.
        ''' </summary>
        Public SolarBeam As Integer = 0
        ''' <summary>
        ''' Ice Burn move counter.
        ''' </summary>
        Public IceBurn As Integer = 0
        ''' <summary>
        ''' Freeze Shock move counter.
        ''' </summary>
        Public FreezeShock As Integer = 0
        ''' <summary>
        ''' Fly move counter.
        ''' </summary>
        Public Fly As Integer = 0
        ''' <summary>
        ''' Dig move counter.
        ''' </summary>
        Public Dig As Integer = 0
        ''' <summary>
        ''' Bounce move counter.
        ''' </summary>
        Public Bounce As Integer = 0
        ''' <summary>
        ''' Dive move counter.
        ''' </summary>
        Public Dive As Integer = 0
        ''' <summary>
        ''' Shadow Force move counter.
        ''' </summary>
        Public ShadowForce As Integer = 0
        ''' <summary>
        ''' Sky Drop move counter.
        ''' </summary>
        Public SkyDrop As Integer = 0

        ''' <summary>
        ''' Turns this Pokémon is trapped in Wrap.
        ''' </summary>
        Public Wrap As Integer = 0
        ''' <summary>
        ''' Turns this Pokémon is trapped in Whirlpool.
        ''' </summary>
        Public Whirlpool As Integer = 0
        ''' <summary>
        ''' Turns this Pokémon is trapped in Bind.
        ''' </summary>
        Public Bind As Integer = 0
        ''' <summary>
        ''' Turns this Pokémon is trapped in Clamp.
        ''' </summary>
        Public Clamp As Integer = 0
        ''' <summary>
        ''' Turns this Pokémon is trapped in FireSpin.
        ''' </summary>
        Public FireSpin As Integer = 0
        ''' <summary>
        ''' Turns this Pokémon is trapped in MagmaStorm.
        ''' </summary>
        Public MagmaStorm As Integer = 0
        ''' <summary>
        ''' Turns this Pokémon is trapped in SandTomb.
        ''' </summary>
        Public SandTomb As Integer = 0

        ''' <summary>
        ''' Focus Energy move counter.
        ''' </summary>
        Public FocusEnergy As Integer = 0
        ''' <summary>
        ''' Lucky Chant move counter.
        ''' </summary>
        Public LuckyChant As Integer = 0
        ''' <summary>
        ''' Counter for the Lock On and Mind Reader move.
        ''' </summary>
        Public LockedOn As Integer = 0
        ''' <summary>
        ''' Counter for the Fury Cutter move.
        ''' </summary>
        Public FuryCutter As Integer = 0
        ''' <summary>
        ''' Counter for the Stockpile move.
        ''' </summary>
        Public StockPile As Integer = 0
        ''' <summary>
        ''' Magic Coat move counter.
        ''' </summary>
        Public MagicCoat As Integer = 0
        ''' <summary>
        ''' Roost counter, set to flying type.
        ''' </summary>
        Public Roost As Integer = 0
        ''' <summary>
        ''' Destiny Bond move counter.
        ''' </summary>
        Public DestinyBond As Integer = 0

        ''' <summary>
        ''' Endure move counter.
        ''' </summary>
        Public Endure As Integer = 0
        ''' <summary>
        ''' Protect move counter
        ''' </summary>
        Public Protect As Integer = 0
        ''' <summary>
        ''' Detect move counter
        ''' </summary>
        Public Detect As Integer = 0
        ''' <summary>
        ''' Indicates how much HP the substitute has left.
        ''' </summary>
        Public Substitute As Integer = 0
        ''' <summary>
        ''' Counts the consecutive uses of Protect like moves.
        ''' </summary>
        Public ProtectMoveCounter As Integer = 0
        ''' <summary>
        ''' King's Shield move counter.
        ''' </summary>
        Public KingsShield As Integer = 0

        ''' <summary>
        ''' Ingrain move counter.
        ''' </summary>
        Public Ingrain As Integer = 0
        ''' <summary>
        ''' Counter for the Magnet Rise move.
        ''' </summary>
        Public MagnetRise As Integer = 0
        ''' <summary>
        ''' Counter for the Aqua Ring move.
        ''' </summary>
        Public AquaRing As Integer = 0
        ''' <summary>
        ''' If the Pokémon is affected by Nightmare.
        ''' </summary>
        Public Nightmare As Integer = 0
        ''' <summary>
        ''' If the Pokémon is affected by Curse.
        ''' </summary>
        Public Cursed As Integer = 0
        ''' <summary>
        ''' Turns until Perish Song faints Pokémon.
        ''' </summary>
        Public PerishSong As Integer = 0
        ''' <summary>
        ''' Counter for moves like Spider Web.
        ''' </summary>
        Public Trapped As Integer = 0
        ''' <summary>
        ''' Counter for the Foresight move.
        ''' </summary>
        Public Foresight As Integer = 0
        ''' <summary>
        ''' Counter for the Odor Sleught move.
        ''' </summary>
        Public OdorSleught As Integer = 0
        ''' <summary>
        ''' Counter for the Miracle Eye move.
        ''' </summary>
        Public MiracleEye As Integer = 0
        ''' <summary>
        ''' Halves this Pokémon's speed for four turns.
        ''' </summary>
        Public GrassPledge As Integer = 0
        ''' <summary>
        ''' Deals damage of 1/8 HP at the end of turn for four turns.
        ''' </summary>
        Public FirePledge As Integer = 0

        ''' <summary>
        ''' If Leech Seed got used on this Pokémon
        ''' </summary>
        Public LeechSeed As Integer = 0
        ''' <summary>
        ''' The target the leech seed HP gets sent to.
        ''' </summary>
        Public LeechSeedTarget As PokemonTarget = Nothing

        ''' <summary>
        ''' Counter for the Metronome item.
        ''' </summary>
        Public MetronomeItemCount As Integer = 0

        ''' <summary>
        ''' Raises critical Hit ratio, Lansat Berry trigger
        ''' </summary>
        Public LansatBerry As Integer = 0
        ''' <summary>
        ''' Raises attack speed when Custap berry got eaten.
        ''' </summary>
        Public CustapBerry As Integer = 0

#End Region

        ''' <summary>
        ''' Resets the fields to their default values when a new Pokémon gets switched in.
        ''' </summary>
        ''' <param name="BatonPassed">If the Pokémon got switched in using Baton Pass.</param>
        Public Sub ResetFields(ByVal BatonPassed As Boolean)
            Me.SleepTurns = 0
            Me.TruantRound = 0
            Me.Taunted = 0
            Me.Rage = 0
            Me.Uproar = 0
            Me.Endure = 0
            Me.Protect = 0
            Me.Detect = 0
            Me.KingsShield = 0
            Me.ProtectMoveCounter = 0
            Me.ToxicRound = 0
            Me.Nightmare = 0
            Me.Outrage = 0
            Me.Thrash = 0
            Me.PetalDance = 0
            Me.Encore = 0
            Me.EncoreMove = Nothing
            Me.Yawn = 0
            Me.ConfusionTurns = 0
            Me.Torment = 0
            Me.TormentMoveID = 0
            Me.ChoiceMove = Nothing
            Me.Recharge = 0
            Me.Rollout = 0
            Me.IceBall = 0
            Me.DefenseCurl = 0
            Me.Charge = 0
            Me.SolarBeam = 0
            Me.LansatBerry = 0
            Me.CustapBerry = 0
            Me.Trapped = 0
            Me.FuryCutter = 0
            Me.TurnsInBattle = 0
            Me.StockPile = 0
            Me.DestinyBond = 0
            Me.GastroAcid = 0
            Me.Foresight = 0
            Me.OdorSleught = 0
            Me.MiracleEye = 0
            Me.Fly = 0
            Me.Dig = 0
            Me.Bounce = 0
            Me.Dive = 0
            Me.ShadowForce = 0
            Me.SkyDrop = 0
            Me.SkyAttack = 0
            Me.RazorWind = 0
            Me.SkullBash = 0
            Me.Wrap = 0
            Me.Whirlpool = 0
            Me.Bind = 0
            Me.Clamp = 0
            Me.FireSpin = 0
            Me.MagmaStorm = 0
            Me.SandTomb = 0
            Me.Bide = 0
            Me.BideDamage = 0
            Me.Roost = 0

            'If Baton Pass is not used to switch, also reset these variables:
            If BatonPassed = False Then
                Me.FocusEnergy = 0
                Me.Ingrain = 0
                Me.Substitute = 0
                Me.MagnetRise = 0
                Me.AquaRing = 0
                Me.Cursed = 0
                Me.Embargo = 0
                Me.PerishSong = 0
                Me.LeechSeed = 0
                Me.LeechSeedTarget = Nothing
            End If
        End Sub

    End Class

    ''' <summary>
    ''' Represents a position on the battle field by targeting the Pokémon.
    ''' </summary>
    Public Class PokemonTarget

        ''' <summary>
        ''' The positions on the battle field a Pokémon can get targeted on.
        ''' </summary>
        Public Enum Targets
            OwnLeft
            OwnCenter
            OwnRight
            OppLeft
            OppCenter
            OppRight
        End Enum

        Private _target As Targets = Targets.OwnCenter

        ''' <summary>
        ''' Creates a new instance of a Pokémon Target.
        ''' </summary>
        ''' <param name="t">The Target type of this Pokémon Target.</param>
        Public Sub New(ByVal t As Targets)
            Me._target = t
        End Sub

        ''' <summary>
        ''' Creates a new instance of a Pokémon Target.
        ''' </summary>
        ''' <param name="s">The Target type of this Pokémon Target.</param>
        Public Sub New(ByVal s As String)
            Select Case s.ToLower()
                Case "ownleft"
                    Me._target = Targets.OwnLeft
                Case "ownright"
                    Me._target = Targets.OwnRight
                Case "owncenter"
                    Me._target = Targets.OwnCenter
                Case "oppleft"
                    Me._target = Targets.OppLeft
                Case "oppright"
                    Me._target = Targets.OppRight
                Case "oppcenter"
                    Me._target = Targets.OppCenter
            End Select
        End Sub

        ''' <summary>
        ''' The target of this PokémonTarget.
        ''' </summary>
        Public ReadOnly Property Target() As Targets
            Get
                Return Me._target
            End Get
        End Property

        Public Shared Operator =(ByVal FirstTarget As PokemonTarget, ByVal SecondTarget As PokemonTarget) As Boolean
            Return FirstTarget._target = SecondTarget._target
        End Operator

        Public Shared Operator <>(ByVal FirstTarget As PokemonTarget, ByVal SecondTarget As PokemonTarget) As Boolean
            Return FirstTarget._target <> SecondTarget._target
        End Operator

        ''' <summary>
        ''' Creates a target set to Own Left.
        ''' </summary>
        Public Shared ReadOnly Property OwnLeft() As PokemonTarget
            Get
                Return New PokemonTarget(Targets.OwnLeft)
            End Get
        End Property

        ''' <summary>
        ''' Creates a target set to Own Center.
        ''' </summary>
        Public Shared ReadOnly Property OwnCenter() As PokemonTarget
            Get
                Return New PokemonTarget(Targets.OwnCenter)
            End Get
        End Property

        ''' <summary>
        ''' Creates a target set to Own Right.
        ''' </summary>
        Public Shared ReadOnly Property OwnRight() As PokemonTarget
            Get
                Return New PokemonTarget(Targets.OwnRight)
            End Get
        End Property

        ''' <summary>
        ''' Creates a target set to Opp Left.
        ''' </summary>
        Public Shared ReadOnly Property OppLeft() As PokemonTarget
            Get
                Return New PokemonTarget(Targets.OppLeft)
            End Get
        End Property

        ''' <summary>
        ''' Creates a target set to Opp Center.
        ''' </summary>
        Public Shared ReadOnly Property OppCenter() As PokemonTarget
            Get
                Return New PokemonTarget(Targets.OppCenter)
            End Get
        End Property

        ''' <summary>
        ''' Creates a target set to Opp Right.
        ''' </summary>
        Public Shared ReadOnly Property OppRight() As PokemonTarget
            Get
                Return New PokemonTarget(Targets.OppRight)
            End Get
        End Property

        ''' <summary>
        ''' Reverses the sides of the target (Own => Opp; Opp => Own)
        ''' </summary>
        Public Sub Reverse()
            Select Case Me._target
                Case Targets.OwnLeft
                    Me._target = Targets.OppLeft
                Case Targets.OwnCenter
                    Me._target = Targets.OppCenter
                Case Targets.OwnRight
                    Me._target = Targets.OppRight
                Case Targets.OppLeft
                    Me._target = Targets.OwnLeft
                Case Targets.OppCenter
                    Me._target = Targets.OwnCenter
                Case Targets.OppRight
                    Me._target = Targets.OwnRight
            End Select
        End Sub

        ''' <summary>
        ''' Returns a string that represents the target.
        ''' </summary>
        Public Overrides Function ToString() As String
            Return Me._target.ToString()
        End Function

        ''' <summary>
        ''' This converts the Attack Target in an AttackV2 into a useable Pokemon Target on the field.
        ''' </summary>
        ''' <param name="AttackTarget">The Attack Target that should get converted.</param>
        ''' <returns>A list of PokemonTargets that can be accessed with the Attack Target.</returns>
        ''' <remarks>The list includes targets for which no Pokémon exist, so call the GetValidPokemonTargets function afterwards.</remarks>
        Public Function ConvertAttackTarget(ByVal AttackTarget As Attack.Targets) As List(Of PokemonTarget)
            If AttackTarget = Attack.Targets.All Then
                Return {OwnLeft, OwnCenter, OwnRight, OppLeft, OppCenter, OppRight}.ToList()
            End If
            If AttackTarget = Attack.Targets.Self Then
                Return {Me}.ToList()
            End If

            Select Case Me._target
                Case Targets.OwnLeft
                    Select Case AttackTarget
                        Case Attack.Targets.OneAdjacentTarget, Attack.Targets.AllAdjacentTargets
                            Return {OwnCenter, OppLeft, OppCenter}.ToList()
                        Case Attack.Targets.OneAdjacentFoe, Attack.Targets.AllAdjacentFoes
                            Return {OppLeft, OppCenter}.ToList()
                        Case Attack.Targets.OneAdjacentAlly, Attack.Targets.AllAdjacentAllies
                            Return {OwnCenter}.ToList()
                        Case Attack.Targets.OneTarget, Attack.Targets.AllTargets
                            Return {OwnCenter, OwnRight, OppLeft, OppCenter, OppRight}.ToList()
                        Case Attack.Targets.OneFoe, Attack.Targets.AllFoes
                            Return {OppLeft, OppCenter, OppRight}.ToList()
                        Case Attack.Targets.OneAlly, Attack.Targets.AllAllies
                            Return {OwnCenter, OwnRight}.ToList()
                    End Select
                Case Targets.OwnCenter
                    Select Case AttackTarget
                        Case Attack.Targets.OneAdjacentTarget, Attack.Targets.OneTarget, Attack.Targets.AllAdjacentTargets, Attack.Targets.AllTargets
                            Return {OwnLeft, OwnRight, OppLeft, OppCenter, OppRight}.ToList()
                        Case Attack.Targets.OneAdjacentFoe, Attack.Targets.OneFoe, Attack.Targets.AllAdjacentFoes, Attack.Targets.AllFoes
                            Return {OppLeft, OppCenter, OppRight}.ToList()
                        Case Attack.Targets.OneAdjacentAlly, Attack.Targets.OneAlly, Attack.Targets.AllAdjacentAllies, Attack.Targets.AllAllies
                            Return {OwnLeft, OwnRight}.ToList()
                    End Select
                Case Targets.OwnRight
                    Select Case AttackTarget
                        Case Attack.Targets.OneAdjacentTarget, Attack.Targets.AllAdjacentTargets
                            Return {OwnCenter, OppCenter, OppRight}.ToList()
                        Case Attack.Targets.OneAdjacentFoe, Attack.Targets.AllAdjacentFoes
                            Return {OppCenter, OppRight}.ToList()
                        Case Attack.Targets.OneAdjacentAlly, Attack.Targets.AllAdjacentAllies
                            Return {OwnCenter}.ToList()
                        Case Attack.Targets.OneTarget, Attack.Targets.AllTargets
                            Return {OwnLeft, OwnCenter, OppLeft, OppCenter, OppRight}.ToList()
                        Case Attack.Targets.OneFoe, Attack.Targets.AllFoes
                            Return {OppLeft, OppCenter, OppRight}.ToList()
                        Case Attack.Targets.OneAlly, Attack.Targets.AllAllies
                            Return {OwnLeft, OwnCenter}.ToList()
                    End Select
                Case Targets.OppLeft
                    Select Case AttackTarget
                        Case Attack.Targets.OneAdjacentTarget, Attack.Targets.AllAdjacentTargets
                            Return {OwnLeft, OwnCenter, OppCenter}.ToList()
                        Case Attack.Targets.OneAdjacentFoe, Attack.Targets.AllAdjacentFoes
                            Return {OwnLeft, OwnCenter}.ToList()
                        Case Attack.Targets.OneAdjacentAlly, Attack.Targets.AllAdjacentAllies
                            Return {OppCenter}.ToList()
                        Case Attack.Targets.OneTarget, Attack.Targets.AllTargets
                            Return {OwnLeft, OwnCenter, OwnRight, OppCenter, OppRight}.ToList()
                        Case Attack.Targets.OneFoe, Attack.Targets.AllFoes
                            Return {OwnLeft, OwnCenter, OwnRight}.ToList()
                        Case Attack.Targets.OneAlly, Attack.Targets.AllAllies
                            Return {OppCenter, OppRight}.ToList()
                    End Select
                Case Targets.OppCenter
                    Select Case AttackTarget
                        Case Attack.Targets.OneAdjacentTarget, Attack.Targets.OneTarget, Attack.Targets.AllAdjacentTargets, Attack.Targets.AllTargets
                            Return {OwnLeft, OwnCenter, OwnRight, OppLeft, OppRight}.ToList()
                        Case Attack.Targets.OneAdjacentFoe, Attack.Targets.OneFoe, Attack.Targets.AllAdjacentFoes, Attack.Targets.AllFoes
                            Return {OwnLeft, OwnCenter, OwnRight}.ToList()
                        Case Attack.Targets.OneAdjacentAlly, Attack.Targets.OneAlly, Attack.Targets.AllAdjacentAllies, Attack.Targets.AllAllies
                            Return {OppLeft, OppRight}.ToList()
                    End Select
                Case Targets.OppRight
                    Select Case AttackTarget
                        Case Attack.Targets.OneAdjacentTarget, Attack.Targets.AllAdjacentTargets
                            Return {OwnCenter, OwnRight, OppCenter}.ToList()
                        Case Attack.Targets.OneAdjacentFoe, Attack.Targets.AllAdjacentFoes
                            Return {OwnCenter, OwnRight}.ToList()
                        Case Attack.Targets.OneAdjacentAlly, Attack.Targets.AllAdjacentAllies
                            Return {OppCenter}.ToList()
                        Case Attack.Targets.OneTarget, Attack.Targets.AllTargets
                            Return {OwnLeft, OwnCenter, OwnRight, OppRight, OppCenter}.ToList()
                        Case Attack.Targets.OneFoe, Attack.Targets.AllFoes
                            Return {OwnLeft, OwnCenter, OwnRight}.ToList()
                        Case Attack.Targets.OneAlly, Attack.Targets.AllAllies
                            Return {OppLeft, OppCenter}.ToList()
                    End Select
            End Select

            Return New List(Of PokemonTarget)
        End Function

        ''' <summary>
        ''' Converts a list of Pokémon targets to a list of valid Pokémon targets that have a Pokémon profile assigned to them on the battle field.
        ''' </summary>
        ''' <param name="BattleScreen">The reference to the BattleScreen.</param>
        ''' <param name="l">The list of Pokémon targets.</param>
        Public Shared Function GetValidPokemonTargets(ByVal BattleScreen As BattleScreen, ByVal l As List(Of PokemonTarget)) As List(Of PokemonTarget)
            Dim returnList As New List(Of PokemonTarget)

            For Each Target As PokemonTarget In l
                If Not BattleScreen.GetProfile(Target) Is Nothing Then
                    returnList.Add(Target)
                End If
            Next

            Return returnList
        End Function

        ''' <summary>
        ''' If the Pokémon target is on the own side of the field.
        ''' </summary>
        Public ReadOnly Property IsOwn() As Boolean
            Get
                Select Case Me._target
                    Case Targets.OwnCenter, Targets.OwnLeft, Targets.OwnRight
                        Return True
                    Case Targets.OppCenter, Targets.OppLeft, Targets.OppRight
                        Return False
                End Select

                'Wont ever happen.
                Return True
            End Get
        End Property

    End Class

End Namespace