Namespace BattleSystem

    ''' <summary>
    ''' Represents a Pokémon's move.
    ''' </summary>
    Public Class Attack

#Region "Fields"

        Public Const MOVE_COUNT As Integer = 560

        Public Enum Categories
            Physical
            Special
            Status
        End Enum

        Public Enum ContestCategories
            Tough
            Smart
            Beauty
            Cool
            Cute
        End Enum

        ''' <summary>
        ''' The target for an attack.
        ''' </summary>
        Public Enum Targets
            OneAdjacentTarget 'One adjacent target, excluding itself.
            OneAdjacentFoe 'One adjacent foe.
            OneAdjacentAlly 'One adjacent ally, excluding itself.

            OneTarget 'One target, excluding itself.
            OneFoe 'One Foe.
            OneAlly 'One ally, excluding itself.

            Self 'Only self

            AllAdjacentTargets 'All adjacent targets, exluding itself
            AllAdjacentFoes 'All adjacent foes
            AllAdjacentAllies 'All adjacent allies, excluding itself.

            AllTargets 'All Targets, excluding itself.
            AllFoes 'All Foes
            AllAllies 'All allies, excluding itself.

            All 'All Pokémon, including itself
        End Enum

        Public Enum AIField
            [Nothing]

            Damage

            Poison
            Burn
            Paralysis
            Sleep
            Freeze
            Confusion

            ConfuseOwn

            CanPoison
            CanBurn
            CanParalyse
            CanSleep
            CanFreeze
            CanConfuse

            RaiseAttack
            RaiseDefense
            RaiseSpAttack
            RaiseSpDefense
            RaiseSpeed
            RaiseAccuracy
            RaiseEvasion

            LowerAttack
            LowerDefense
            LowerSpAttack
            LowerSpDefense
            LowerSpeed
            LowerAccuracy
            LowerEvasion

            CanRaiseAttack
            CanRaiseDefense
            CanRaiseSpAttack
            CanRaiseSpDefense
            CanRaiseSpeed
            CanRaiseAccuracy
            CanRauseEvasion

            CanLowerAttack
            CanLowerDefense
            CanLowerSpAttack
            CanLowerSpDefense
            CanLowerSpeed
            CanLowerAccuracy
            CanLowerEvasion

            Flinch
            CanFlinch

            Infatuation

            Trap
            OHKO
            MultiTurn
            Recoil

            Healing
            CureStatus
            Support
            Recharge
            HighPriority
            Absorbing
            Selfdestruct
            ThrawOut
            CannotMiss
            RemoveReflectLightscreen
        End Enum

        '#Definitions
        Public Type As Element = New Element("Normal")

        Public Property ID() As Integer
            Get
                Return Me._ID
            End Get
            Set(value As Integer)
                Me._ID = value
            End Set
        End Property

        Public Property Power() As Integer
            Get
                Return Me._power
            End Get
            Set(value As Integer)
                Me._power = value
            End Set
        End Property

        Public Property Accuracy() As Integer
            Get
                Return Me._accuracy
            End Get
            Set(value As Integer)
                Me._accuracy = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(value As String)
                Me._name = value
            End Set
        End Property

        Private _ID As Integer = 1
        Public OriginalID As Integer = 1 'Original MoveID, remove when not needed anymore. This stores the original move ID when the move isn't programmed yet.
        Public IsDefaultMove As Boolean = False 'if Pound gets loaded instead of the correct move, this is true.

        Public GameModeFunction As String = "" 'A GameMode can specify a pre defined function for a move.
        Public IsGameModeMove As Boolean = False

        Private _power As Integer = 40
        Private _accuracy As Integer = 100
        Private _name As String = "Pound"

        Public OriginalPP As Integer = 35
        Public Category As Categories = Categories.Physical
        Public ContestCategory As ContestCategories = ContestCategories.Tough
        Public Description As String = "Pounds with forelegs or tail."
        Public CriticalChance As Integer = 1
        Public IsHMMove As Boolean = False
        Public Target As Targets = Targets.OneAdjacentTarget
        Public Priority As Integer = 0
        Public TimesToAttack As Integer = 1
        Public EffectChances As New List(Of Integer)
        '#End

        '#SpecialDefinitions
        Public MakesContact As Boolean = True
        Public ProtectAffected As Boolean = True
        Public MagicCoatAffected As Boolean = False
        Public SnatchAffected As Boolean = False
        Public MirrorMoveAffected As Boolean = True
        Public KingsrockAffected As Boolean = True
        Public CounterAffected As Boolean = True
        Public DisabledWhileGravity As Boolean = False
        Public UseEffectiveness As Boolean = True
        Public IsHealingMove As Boolean = False
        Public RemovesFrozen As Boolean = False
        Public IsRecoilMove As Boolean = False
        Public IsPunchingMove As Boolean = False
        Public ImmunityAffected As Boolean = True
        Public IsDamagingMove As Boolean = True
        Public IsProtectMove As Boolean = False
        Public IsSoundMove As Boolean = False
        Public HasSecondaryEffect As Boolean = False
        Public IsAffectedBySubstitute As Boolean = True
        Public IsOneHitKOMove As Boolean = False
        Public IsWonderGuardAffected As Boolean = True
        Public UseAccEvasion As Boolean = True
        Public CanHitInMidAir As Boolean = False
        Public CanHitUnderground As Boolean = False
        Public CanHitUnderwater As Boolean = False
        Public CanHitSleeping As Boolean = True
        Public CanGainSTAB As Boolean = True
        Public IsPowderMove As Boolean = False
        Public IsTrappingMove As Boolean = False
        Public IsPulseMove As Boolean = False
        Public IsBulletMove As Boolean = False
        Public IsJawMove As Boolean = False
        Public UseOppDefense As Boolean = True
        Public UseOppEvasion As Boolean = True

        Public FocusOppPokemon As Boolean = True
        '#End

        Public CurrentPP As Integer = 0
        Public MaxPP As Integer = 0
        Public Disabled As Integer = 0

        Public AIField1 As AIField = AIField.Damage
        Public AIField2 As AIField = AIField.Nothing
        Public AIField3 As AIField = AIField.Nothing

#End Region

        ''' <summary>
        ''' Returns a new instance of AttackV2 based on the input ID.
        ''' </summary>
        ''' <param name="ID">The ID of the Move to return.</param>
        Public Shared Function GetAttackByID(ByVal ID As Integer) As Attack
            Dim returnMove As Attack

            Select Case ID
                Case 1
                    returnMove = New Moves.Normal.Pound()
                Case 2
                    returnMove = New Moves.Fighting.KarateChop()
                Case 3
                    returnMove = New Moves.Normal.DoubleSlap()
                Case 4
                    returnMove = New Moves.Normal.CometPunch()
                Case 5
                    returnMove = New Moves.Normal.MegaPunch()
                Case 6
                    returnMove = New Moves.Normal.PayDay()
                Case 7
                    returnMove = New Moves.Fire.FirePunch()
                Case 8
                    returnMove = New Moves.Ice.IcePunch()
                Case 9
                    returnMove = New Moves.Electric.ThunderPunch()
                Case 10
                    returnMove = New Moves.Normal.Scratch()
                Case 11
                    returnMove = New Moves.Normal.ViceGrip()
                Case 12
                    returnMove = New Moves.Normal.Guillotine()
                Case 13
                    returnMove = New Moves.Normal.RazorWind()
                Case 14
                    returnMove = New Moves.Normal.SwordsDance()
                Case 15
                    returnMove = New Moves.Normal.Cut()
                Case 16
                    returnMove = New Moves.Flying.Gust()
                Case 17
                    returnMove = New Moves.Flying.WingAttack()
                Case 18
                    returnMove = New Moves.Normal.Whirlwind()
                Case 19
                    returnMove = New Moves.Flying.Fly()
                Case 20
                    returnMove = New Moves.Normal.Bind()
                Case 21
                    returnMove = New Moves.Normal.Slam()
                Case 22
                    returnMove = New Moves.Grass.VineWhip()
                Case 23
                    returnMove = New Moves.Normal.Stomp()
                Case 24
                    returnMove = New Moves.Fighting.DoubleKick()
                Case 25
                    returnMove = New Moves.Normal.MegaKick()
                Case 26
                    returnMove = New Moves.Fighting.JumpKick()
                Case 27
                    returnMove = New Moves.Fighting.RollingKick()
                Case 28
                    returnMove = New Moves.Ground.SandAttack()
                Case 29
                    returnMove = New Moves.Normal.Headbutt()
                Case 30
                    returnMove = New Moves.Normal.HornAttack()
                Case 31
                    returnMove = New Moves.Normal.FuryAttack()
                Case 32
                    returnMove = New Moves.Normal.HornDrill()
                Case 33
                    returnMove = New Moves.Normal.Tackle()
                Case 34
                    returnMove = New Moves.Normal.BodySlam()
                Case 35
                    returnMove = New Moves.Normal.Wrap()
                Case 36
                    returnMove = New Moves.Normal.TakeDown()
                Case 37
                    returnMove = New Moves.Normal.Thrash()
                Case 38
                    returnMove = New Moves.Normal.DoubleEdge()
                Case 39
                    returnMove = New Moves.Normal.TailWhip()
                Case 40
                    returnMove = New Moves.Poison.PoisonSting()
                Case 41
                    returnMove = New Moves.Bug.Twineedle()
                Case 42
                    returnMove = New Moves.Bug.PinMissle()
                Case 43
                    returnMove = New Moves.Normal.Leer()
                Case 44
                    returnMove = New Moves.Dark.Bite()
                Case 45
                    returnMove = New Moves.Normal.Growl()
                Case 46
                    returnMove = New Moves.Normal.Roar()
                Case 47
                    returnMove = New Moves.Normal.Sing()
                Case 48
                    returnMove = New Moves.Normal.Supersonic()
                Case 49
                    returnMove = New Moves.Normal.SonicBoom()
                    'Case 50
                    'Disable
                Case 51
                    returnMove = New Moves.Poison.Acid()
                Case 52
                    returnMove = New Moves.Fire.Ember()
                Case 53
                    returnMove = New Moves.Fire.Flamethrower()
                Case 54
                    returnMove = New Moves.Ice.Mist()
                Case 55
                    returnMove = New Moves.Water.WaterGun()
                Case 56
                    returnMove = New Moves.Water.HydroPump()
                Case 57
                    returnMove = New Moves.Water.Surf()
                Case 58
                    returnMove = New Moves.Ice.IceBeam()
                Case 59
                    returnMove = New Moves.Ice.Blizzard()
                Case 60
                    returnMove = New Moves.Psychic.Psybeam()
                Case 61
                    returnMove = New Moves.Water.BubbleBeam()
                Case 62
                    returnMove = New Moves.Ice.AuroraBeam()
                Case 63
                    returnMove = New Moves.Normal.HyperBeam()
                Case 64
                    returnMove = New Moves.Flying.Peck()
                Case 65
                    returnMove = New Moves.Flying.DrillPeck()
                Case 66
                    returnMove = New Moves.Fighting.Submission()
                Case 67
                    returnMove = New Moves.Fighting.LowKick()
                Case 68
                    returnMove = New Moves.Fighting.Counter()
                Case 69
                    returnMove = New Moves.Fighting.SeismicToss()
                Case 70
                    returnMove = New Moves.Normal.Strength()
                Case 71
                    returnMove = New Moves.Grass.Absorb()
                Case 72
                    returnMove = New Moves.Grass.MegaDrain()
                Case 73
                    returnMove = New Moves.Grass.LeechSeed()
                Case 74
                    returnMove = New Moves.Normal.Growth()
                Case 75
                    returnMove = New Moves.Grass.RazorLeaf()
                Case 76
                    returnMove = New Moves.Grass.SolarBeam()
                Case 77
                    returnMove = New Moves.Poison.PoisonPowder()
                Case 78
                    returnMove = New Moves.Grass.StunSpore()
                Case 79
                    returnMove = New Moves.Grass.SleepPowder()
                Case 80
                    returnMove = New Moves.Grass.PetalDance()
                Case 81
                    returnMove = New Moves.Bug.StringShot()
                Case 82
                    returnMove = New Moves.Dragon.DragonRage()
                Case 83
                    returnMove = New Moves.Fire.FireSpin()
                Case 84
                    returnMove = New Moves.Electric.ThunderShock()
                Case 85
                    returnMove = New Moves.Electric.Thunderbolt()
                Case 86
                    returnMove = New Moves.Electric.ThunderWave()
                Case 87
                    returnMove = New Moves.Electric.Thunder()
                Case 88
                    returnMove = New Moves.Rock.RockThrow()
                Case 89
                    returnMove = New Moves.Ground.Earthquake()
                Case 90
                    returnMove = New Moves.Ground.Fissure()
                Case 91
                    returnMove = New Moves.Ground.Dig()
                Case 92
                    returnMove = New Moves.Poison.Toxic()
                Case 93
                    returnMove = New Moves.Psychic.Confusion()
                Case 94
                    returnMove = New Moves.Psychic.Psychic()
                Case 95
                    returnMove = New Moves.Psychic.Hypnosis()
                Case 96
                    returnMove = New Moves.Psychic.Meditate()
                Case 97
                    returnMove = New Moves.Psychic.Agility()
                Case 98
                    returnMove = New Moves.Normal.QuickAttack()
                Case 99
                    returnMove = New Moves.Normal.Rage()
                Case 100
                    returnMove = New Moves.Psychic.Teleport()
                Case 101
                    returnMove = New Moves.Ghost.NightShade()
                Case 102
                    returnMove = New Moves.Normal.Mimic()
                Case 103
                    returnMove = New Moves.Normal.Screech()
                Case 104
                    returnMove = New Moves.Normal.DoubleTeam()
                Case 105
                    returnMove = New Moves.Normal.Recover()
                Case 106
                    returnMove = New Moves.Normal.Harden()
                Case 107
                    returnMove = New Moves.Normal.Minimize()
                Case 108
                    returnMove = New Moves.Normal.SmokeScreen()
                Case 109
                    returnMove = New Moves.Ghost.ConfuseRay()
                Case 110
                    returnMove = New Moves.Water.Withdraw()
                Case 111
                    returnMove = New Moves.Normal.DefenseCurl()
                Case 112
                    returnMove = New Moves.Psychic.Barrier()
                Case 113
                    returnMove = New Moves.Psychic.LightScreen()
                Case 114
                    returnMove = New Moves.Ice.Haze()
                Case 115
                    returnMove = New Moves.Psychic.Reflect()
                Case 116
                    returnMove = New Moves.Normal.FocusEnergy()
                Case 117
                    returnMove = New Moves.Normal.Bide()
                Case 118
                    returnMove = New Moves.Normal.Metronome()
                Case 119
                    returnMove = New Moves.Flying.MirrorMove()
                Case 120
                    returnMove = New Moves.Normal.Selfdestruct()
                Case 121
                    returnMove = New Moves.Normal.EggBomb()
                Case 122
                    returnMove = New Moves.Ghost.Lick()
                Case 123
                    returnMove = New Moves.Poison.Smog()
                Case 124
                    returnMove = New Moves.Poison.Sludge()
                Case 125
                    returnMove = New Moves.Ground.BoneClub()
                Case 126
                    returnMove = New Moves.Fire.FireBlast()
                Case 127
                    returnMove = New Moves.Water.Waterfall()
                Case 128
                    returnMove = New Moves.Water.Clamp()
                Case 129
                    returnMove = New Moves.Normal.Swift()
                Case 130
                    returnMove = New Moves.Normal.SkullBash()
                Case 131
                    returnMove = New Moves.Normal.SpikeCannon()
                Case 132
                    returnMove = New Moves.Normal.Constrict()
                Case 133
                    returnMove = New Moves.Psychic.Amnesia()
                Case 134
                    returnMove = New Moves.Psychic.Kinesis()
                Case 135
                    returnMove = New Moves.Normal.Softboiled()
                Case 136
                    returnMove = New Moves.Fighting.HiJumpKick()
                Case 137
                    returnMove = New Moves.Normal.Glare()
                Case 138
                    returnMove = New Moves.Psychic.DreamEater()
                Case 139
                    returnMove = New Moves.Poison.PoisonGas()
                Case 140
                    returnMove = New Moves.Normal.Barrage()
                Case 141
                    returnMove = New Moves.Bug.LeechLife()
                Case 142
                    returnMove = New Moves.Normal.LovelyKiss()
                Case 143
                    returnMove = New Moves.Flying.SkyAttack()
                Case 144
                    returnMove = New Moves.Normal.Transform()
                Case 145
                    returnMove = New Moves.Water.Bubble()
                Case 146
                    returnMove = New Moves.Normal.DizzyPunch()
                Case 147
                    returnMove = New Moves.Grass.Spore()
                Case 148
                    returnMove = New Moves.Electric.Flash()
                Case 149
                    returnMove = New Moves.Psychic.Psywave()
                Case 150
                    returnMove = New Moves.Normal.Splash()
                Case 151
                    returnMove = New Moves.Poison.AcidArmor()
                Case 152
                    returnMove = New Moves.Water.Crabhammer()
                Case 153
                    returnMove = New Moves.Normal.Explosion()
                Case 154
                    returnMove = New Moves.Normal.FurySwipes()
                Case 155
                    returnMove = New Moves.Ground.Bonemerang()
                Case 156
                    returnMove = New Moves.Psychic.Rest()
                Case 157
                    returnMove = New Moves.Rock.RockSlide()
                Case 158
                    returnMove = New Moves.Normal.HyperFang()
                Case 159
                    returnMove = New Moves.Normal.Sharpen()
                Case 160
                    returnMove = New Moves.Normal.Conversion()
                Case 161
                    returnMove = New Moves.Normal.TriAttack()
                Case 162
                    returnMove = New Moves.Normal.SuperFang()
                Case 163
                    returnMove = New Moves.Normal.Slash()
                Case 164
                    returnMove = New Moves.Normal.Substitute()
                Case 165
                    returnMove = New Moves.Normal.Struggle()
                Case 166
                    returnMove = New Moves.Normal.Sketch()
                Case 167
                    returnMove = New Moves.Fighting.TripleKick()
                Case 168
                    returnMove = New Moves.Dark.Thief()
                Case 169
                    returnMove = New Moves.Bug.SpiderWeb()
                Case 170
                    returnMove = New Moves.Normal.MindReader()
                Case 171
                    returnMove = New Moves.Ghost.Nightmare()
                Case 172
                    returnMove = New Moves.Fire.FlameWheel()
                Case 173
                    returnMove = New Moves.Normal.Snore()
                Case 174
                    returnMove = New Moves.Ghost.Curse()
                Case 175
                    returnMove = New Moves.Normal.Flail()
                Case 176
                    returnMove = New Moves.Normal.Conversion2()
                Case 177
                    returnMove = New Moves.Flying.Aeroblast()
                Case 178
                    returnMove = New Moves.Grass.CottonSpore()
                Case 179
                    returnMove = New Moves.Fighting.Reversal()
                Case 180
                    returnMove = New Moves.Ghost.Spite()
                Case 181
                    returnMove = New Moves.Ice.PowderSnow()
                Case 182
                    returnMove = New Moves.Normal.Protect()
                Case 183
                    returnMove = New Moves.Fighting.MachPunch()
                Case 184
                    returnMove = New Moves.Normal.ScaryFace()
                Case 185
                    returnMove = New Moves.Dark.FaintAttack()
                Case 186
                    returnMove = New Moves.Normal.SweetKiss()
                Case 187
                    returnMove = New Moves.Normal.BellyDrum()
                Case 188
                    returnMove = New Moves.Poison.SludgeBomb()
                Case 189
                    returnMove = New Moves.Ground.MudSlap()
                Case 190
                    returnMove = New Moves.Water.Octazooka()
                Case 191
                    returnMove = New Moves.Ground.Spikes()
                Case 192
                    returnMove = New Moves.Electric.ZapCannon()
                Case 193
                    returnMove = New Moves.Normal.Foresight()
                Case 194
                    returnMove = New Moves.Ghost.DestinyBond()
                Case 195
                    returnMove = New Moves.Normal.PerishSong()
                Case 196
                    returnMove = New Moves.Ice.IcyWind()
                Case 197
                    returnMove = New Moves.Fighting.Detect()
                Case 198
                    returnMove = New Moves.Ground.BoneRush()
                Case 199
                    returnMove = New Moves.Normal.LockOn()
                Case 200
                    returnMove = New Moves.Dragon.Outrage()
                Case 201
                    returnMove = New Moves.Rock.Sandstorm()
                Case 202
                    returnMove = New Moves.Grass.GigaDrain()
                Case 203
                    returnMove = New Moves.Normal.Endure()
                Case 204
                    returnMove = New Moves.Normal.Charm()
                Case 205
                    returnMove = New Moves.Rock.Rollout()
                Case 206
                    returnMove = New Moves.Normal.FalseSwipe()
                Case 207
                    returnMove = New Moves.Normal.Swagger()
                Case 208
                    returnMove = New Moves.Normal.MilkDrink()
                Case 209
                    returnMove = New Moves.Electric.Spark()
                Case 210
                    returnMove = New Moves.Bug.FuryCutter()
                Case 211
                    returnMove = New Moves.Steel.SteelWing()
                Case 212
                    returnMove = New Moves.Normal.MeanLook()
                Case 213
                    returnMove = New Moves.Normal.Attract()
                Case 214
                    returnMove = New Moves.Normal.SleepTalk()
                Case 215
                    returnMove = New Moves.Normal.HealBell()
                Case 216
                    returnMove = New Moves.Normal.Return()
                Case 217
                    returnMove = New Moves.Normal.Present()
                Case 218
                    returnMove = New Moves.Normal.Frustration()
                Case 219
                    returnMove = New Moves.Normal.Safeguard()
                Case 220
                    returnMove = New Moves.Normal.PainSplit()
                Case 221
                    returnMove = New Moves.Fire.SacredFire()
                Case 222
                    returnMove = New Moves.Ground.Magnitude()
                Case 223
                    returnMove = New Moves.Fighting.DynamicPunch()
                Case 224
                    returnMove = New Moves.Bug.Megahorn()
                Case 225
                    returnMove = New Moves.Dragon.DragonBreath()
                Case 226
                    returnMove = New Moves.Normal.BatonPass()
                Case 227
                    returnMove = New Moves.Normal.Encore()
                Case 228
                    returnMove = New Moves.Dark.Pursuit()
                Case 229
                    returnMove = New Moves.Normal.RapidSpin()
                Case 230
                    returnMove = New Moves.Normal.SweetScent()
                Case 231
                    returnMove = New Moves.Steel.IronTail()
                Case 232
                    returnMove = New Moves.Steel.MetalClaw()
                Case 233
                    returnMove = New Moves.Fighting.VitalThrow()
                Case 234
                    returnMove = New Moves.Normal.MorningSun()
                Case 235
                    returnMove = New Moves.Grass.Synthesis()
                Case 236
                    returnMove = New Moves.Normal.Moonlight()
                Case 237
                    returnMove = New Moves.Normal.HiddenPower()
                Case 238
                    returnMove = New Moves.Fighting.CrossChop()
                Case 239
                    returnMove = New Moves.Dragon.Twister()
                Case 240
                    returnMove = New Moves.Water.RainDance()
                Case 241
                    returnMove = New Moves.Fire.SunnyDay()
                Case 242
                    returnMove = New Moves.Dark.Crunch()
                Case 243
                    returnMove = New Moves.Psychic.MirrorCoat()
                Case 244
                    returnMove = New Moves.Normal.PsychUp()
                Case 245
                    returnMove = New Moves.Normal.ExtremeSpeed()
                Case 246
                    returnMove = New Moves.Rock.AncientPower()
                Case 247
                    returnMove = New Moves.Ghost.ShadowBall()
                Case 248
                    returnMove = New Moves.Psychic.FutureSight()
                Case 249
                    returnMove = New Moves.Fighting.RockSmash()
                Case 250
                    returnMove = New Moves.Water.Whirlpool()
                Case 251
                    returnMove = New Moves.Dark.BeatUp()
                Case 252
                    returnMove = New Moves.Normal.FakeOut()
                Case 253
                    returnMove = New Moves.Normal.Uproar()
                Case 254
                    returnMove = New Moves.Normal.Stockpile()
                Case 255
                    returnMove = New Moves.Normal.SpitUp()
                Case 256
                    returnMove = New Moves.Normal.Swallow()
                Case 257
                    returnMove = New Moves.Fire.HeatWave()
                Case 258
                    returnMove = New Moves.Ice.Hail()
                Case 259
                    returnMove = New Moves.Dark.Torment()
                Case 260
                    returnMove = New Moves.Dark.Flatter()
                Case 261
                    returnMove = New Moves.Fire.WillOWisp()
                Case 262
                    returnMove = New Moves.Dark.Memento()
                Case 263
                    returnMove = New Moves.Normal.Facade()
                Case 264
                    returnMove = New Moves.Fighting.FocusPunch()
                Case 265
                    returnMove = New Moves.Normal.SmellingSalt()
                    'Case 266
                    'Follow me - Double Battles
                Case 267
                    returnMove = New Moves.Normal.NaturePower()
                Case 268
                    returnMove = New Moves.Electric.Charge()
                Case 269
                    returnMove = New Moves.Dark.Taunt()
                    'Case 270
                    'Helping hand - Double Battles
                Case 271
                    returnMove = New Moves.Psychic.Trick()
                Case 272
                    returnMove = New Moves.Psychic.RolePlay()
                Case 273
                    returnMove = New Moves.Normal.Wish()
                Case 274
                    returnMove = New Moves.Normal.Assist()
                Case 275
                    returnMove = New Moves.Grass.Ingrain()
                Case 276
                    returnMove = New Moves.Fighting.SuperPower()
                Case 277
                    returnMove = New Moves.Psychic.MagicCoat()
                Case 278
                    returnMove = New Moves.Normal.Recycle()
                Case 279
                    returnMove = New Moves.Fighting.Revenge()
                Case 280
                    returnMove = New Moves.Fighting.BrickBreak()
                Case 281
                    returnMove = New Moves.Normal.Yawn()
                Case 282
                    returnMove = New Moves.Dark.KnockOff()
                Case 283
                    returnMove = New Moves.Normal.Endeavor()
                Case 284
                    returnMove = New Moves.Fire.Eruption()
                Case 285
                    returnMove = New Moves.Psychic.SkillSwap()
                    'Case 286
                    'Imprison
                Case 287
                    returnMove = New Moves.Normal.Refresh()
                    'Case 288
                    'Grudge
                Case 289
                    returnMove = New Moves.Normal.Snatch()
                    'Case 290
                    'Secret Power
                Case 291
                    returnMove = New Moves.Water.Dive()
                Case 292
                    returnMove = New Moves.Fighting.ArmThrust()
                    'Case 293
                    'Camouflage
                Case 294
                    returnMove = New Moves.Bug.Tailglow()
                Case 295
                    returnMove = New Moves.Psychic.LusterPurge()
                Case 296
                    returnMove = New Moves.Psychic.MistBall()
                Case 297
                    returnMove = New Moves.Flying.FeatherDance()
                Case 298
                    returnMove = New Moves.Normal.TeeterDance()
                Case 299
                    returnMove = New Moves.Fire.BlazeKick()
                Case 300
                    returnMove = New Moves.Ground.MudSport()
                Case 301
                    returnMove = New Moves.Ice.IceBall()
                Case 302
                    returnMove = New Moves.Grass.NeedleArm()
                Case 303
                    returnMove = New Moves.Normal.SlackOff()
                Case 304
                    returnMove = New Moves.Normal.HyperVoice()
                Case 305
                    returnMove = New Moves.Poison.PoisonFang()
                Case 306
                    returnMove = New Moves.Normal.CrushClaw()
                Case 307
                    returnMove = New Moves.Fire.BlastBurn()
                Case 308
                    returnMove = New Moves.Water.Hydrocannon()
                Case 309
                    returnMove = New Moves.Steel.MeteorMash()
                Case 310
                    returnMove = New Moves.Ghost.Astonish()
                Case 311
                    returnMove = New Moves.Normal.WeatherBall()
                Case 312
                    returnMove = New Moves.Grass.Aromatherapy()
                Case 313
                    returnMove = New Moves.Dark.FakeTears()
                Case 314
                    returnMove = New Moves.Flying.AirCutter()
                Case 315
                    returnMove = New Moves.Fire.Overheat()
                Case 316
                    returnMove = New Moves.Normal.OdorSleuth()
                Case 317
                    returnMove = New Moves.Rock.RockTomb()
                Case 318
                    returnMove = New Moves.Bug.Silverwind()
                Case 319
                    returnMove = New Moves.Steel.MetalSound()
                Case 320
                    returnMove = New Moves.Grass.GrassWhistle()
                Case 321
                    returnMove = New Moves.Normal.Tickle()
                Case 322
                    returnMove = New Moves.Psychic.CosmicPower()
                Case 323
                    returnMove = New Moves.Water.WaterSpout()
                Case 324
                    returnMove = New Moves.Bug.SignalBeam()
                Case 325
                    returnMove = New Moves.Ghost.ShadowPunch()
                Case 326
                    returnMove = New Moves.Psychic.Extrasensory()
                Case 327
                    returnMove = New Moves.Fighting.SkyUppercut()
                Case 328
                    returnMove = New Moves.Ground.SandTomb()
                Case 329
                    returnMove = New Moves.Ice.SheerCold()
                Case 330
                    returnMove = New Moves.Water.Muddywater()
                Case 331
                    returnMove = New Moves.Grass.BulletSeed()
                Case 332
                    returnMove = New Moves.Flying.AerialAce()
                Case 333
                    returnMove = New Moves.Ice.IcicleSpear()
                Case 334
                    returnMove = New Moves.Steel.IronDefense()
                Case 335
                    returnMove = New Moves.Normal.Block()
                Case 336
                    returnMove = New Moves.Normal.Howl()
                Case 337
                    returnMove = New Moves.Dragon.DragonClaw()
                Case 338
                    returnMove = New Moves.Grass.FrenzyPlant()
                Case 339
                    returnMove = New Moves.Fighting.BulkUp()
                Case 340
                    returnMove = New Moves.Flying.Bounce()
                Case 341
                    returnMove = New Moves.Ground.MudShot()
                Case 342
                    returnMove = New Moves.Poison.PoisonTail()
                Case 343
                    returnMove = New Moves.Normal.Covet()
                Case 344
                    returnMove = New Moves.Electric.VoltTackle()
                Case 345
                    returnMove = New Moves.Grass.MagicalLeaf()
                Case 346
                    returnMove = New Moves.Water.WaterSport()
                Case 347
                    returnMove = New Moves.Psychic.CalmMind()
                Case 348
                    returnMove = New Moves.Grass.LeafBlade()
                Case 349
                    returnMove = New Moves.Dragon.DragonDance()
                Case 350
                    returnMove = New Moves.Rock.RockBlast()
                Case 351
                    returnMove = New Moves.Electric.ShockWave()
                Case 352
                    returnMove = New Moves.Water.WaterPulse()
                Case 353
                    returnMove = New Moves.Steel.DoomDesire()
                Case 354
                    returnMove = New Moves.Psychic.PsychoBoost()
                Case 355
                    returnMove = New Moves.Flying.Roost()
                Case 356
                    returnMove = New Moves.Psychic.Gravity()
                Case 357
                    returnMove = New Moves.Psychic.MiracleEye()
                Case 358
                    returnMove = New Moves.Fighting.WakeUpSlap()
                Case 359
                    returnMove = New Moves.Fighting.HammerArm()
                Case 360
                    returnMove = New Moves.Steel.GyroBall()
                Case 361
                    returnMove = New Moves.Psychic.HealingWish()
                Case 362
                    returnMove = New Moves.Water.Brine()
                Case 363
                    returnMove = New Moves.Normal.NaturalGift()
                Case 364
                    returnMove = New Moves.Normal.Feint()
                Case 365
                    returnMove = New Moves.Flying.Pluck()
                Case 366
                    returnMove = New Moves.Flying.Tailwind()
                Case 367
                    returnMove = New Moves.Normal.Acupressure()
                Case 368
                    returnMove = New Moves.Steel.MetalBurst()
                Case 369
                    returnMove = New Moves.Bug.UTurn()
                Case 370
                    returnMove = New Moves.Fighting.CloseCombat()
                Case 371
                    returnMove = New Moves.Dark.Payback()
                Case 372
                    returnMove = New Moves.Dark.Assurance()
                Case 373
                    returnMove = New Moves.Dark.Embargo()
                Case 374
                    returnMove = New Moves.Dark.Fling()
                Case 375
                    returnMove = New Moves.Psychic.PsychoShift()
                Case 376
                    returnMove = New Moves.Normal.TrumpCard()
                Case 377
                    returnMove = New Moves.Psychic.HealBlock()
                Case 378
                    returnMove = New Moves.Normal.WringOut()
                Case 379
                    returnMove = New Moves.Psychic.PowerTrick()
                    'Case 380
                    'Gastro Acid
                    'Case 381
                    'Lucky Chant
                    'Case 382
                    'Me First
                    'Case 383
                    'Copycat
                Case 384
                    returnMove = New Moves.Psychic.PowerSwap()
                Case 385
                    returnMove = New Moves.Psychic.GuardSwap()
                Case 386
                    returnMove = New Moves.Dark.Punishment()
                Case 387
                    returnMove = New Moves.Normal.LastResort()
                    'Case 388
                    'Worry Seed
                Case 389
                    returnMove = New Moves.Dark.SuckerPunch()
                Case 390
                    returnMove = New Moves.Poison.ToxicSpikes()
                Case 391
                    returnMove = New Moves.Psychic.HeartSwap()
                Case 392
                    returnMove = New Moves.Water.AquaRing()
                Case 393
                    returnMove = New Moves.Electric.MagnetRise()
                Case 394
                    returnMove = New Moves.Fire.FlareBlitz()
                Case 395
                    returnMove = New Moves.Fighting.ForcePalm()
                Case 396
                    returnMove = New Moves.Fighting.AuraSphere()
                Case 397
                    returnMove = New Moves.Rock.RockPolish()
                Case 398
                    returnMove = New Moves.Poison.PoisonJab()
                Case 399
                    returnMove = New Moves.Dark.DarkPulse()
                Case 400
                    returnMove = New Moves.Dark.Nightslash()
                Case 401
                    returnMove = New Moves.Water.Aquatail()
                Case 402
                    returnMove = New Moves.Grass.SeedBomb()
                Case 403
                    returnMove = New Moves.Flying.AirSlash()
                Case 404
                    returnMove = New Moves.Bug.Xscissor()
                Case 405
                    returnMove = New Moves.Bug.BugBuzz()
                Case 406
                    returnMove = New Moves.Dragon.DragonPulse()
                Case 407
                    returnMove = New Moves.Dragon.DragonRush()
                Case 408
                    returnMove = New Moves.Rock.PowerGem()
                Case 409
                    returnMove = New Moves.Fighting.DrainPunch()
                Case 410
                    returnMove = New Moves.Fighting.VacuumWave()
                Case 411
                    returnMove = New Moves.Fighting.FocusBlast()
                Case 412
                    returnMove = New Moves.Grass.EnergyBall()
                Case 413
                    returnMove = New Moves.Flying.BraveBird()
                Case 414
                    returnMove = New Moves.Ground.EarthPower()
                Case 415
                    returnMove = New Moves.Dark.Switcheroo()
                Case 416
                    returnMove = New Moves.Normal.GigaImpact()
                Case 417
                    returnMove = New Moves.Dark.NastyPlot()
                Case 418
                    returnMove = New Moves.Steel.BulletPunch()
                    'Case 419
                    'Avalanche
                Case 420
                    returnMove = New Moves.Ice.IceShard()
                Case 421
                    returnMove = New Moves.Ghost.ShadowClaw()
                Case 422
                    returnMove = New Moves.Electric.ThunderFang()
                Case 423
                    returnMove = New Moves.Ice.IceFang()
                Case 424
                    returnMove = New Moves.Fire.FireFang()
                Case 425
                    returnMove = New Moves.Ghost.ShadowSneak()
                Case 426
                    returnMove = New Moves.Ground.MudBomb()
                Case 427
                    returnMove = New Moves.Psychic.PsychoCut()
                Case 428
                    returnMove = New Moves.Psychic.ZenHeadbutt()
                Case 429
                    returnMove = New Moves.Steel.MirrorShot()
                Case 430
                    returnMove = New Moves.Steel.FlashCannon()
                Case 431
                    returnMove = New Moves.Normal.RockClimb()
                Case 432
                    returnMove = New Moves.Flying.Defog()
                Case 433
                    returnMove = New Moves.Psychic.TrickRoom()
                Case 434
                    returnMove = New Moves.Dragon.DracoMeteor()
                Case 435
                    returnMove = New Moves.Electric.Discharge()
                Case 436
                    returnMove = New Moves.Fire.LavaPlume()
                Case 437
                    returnMove = New Moves.Grass.LeafStorm()
                Case 438
                    returnMove = New Moves.Grass.PowerWhip()
                Case 439
                    returnMove = New Moves.Rock.RockWrecker()
                Case 440
                    returnMove = New Moves.Poison.Crosspoison()
                Case 441
                    returnMove = New Moves.Poison.GunkShot()
                Case 442
                    returnMove = New Moves.Steel.Ironhead()
                Case 443
                    returnMove = New Moves.Steel.MagnetBomb()
                Case 444
                    returnMove = New Moves.Rock.StoneEdge()
                Case 445
                    returnMove = New Moves.Normal.Captivate()
                Case 446
                    returnMove = New Moves.Rock.StealthRock()
                Case 447
                    returnMove = New Moves.Grass.GrassKnot()
                Case 448
                    returnMove = New Moves.Flying.Chatter()
                    'Case 449
                    'Judgment
                Case 450
                    returnMove = New Moves.Bug.BugBite()
                Case 451
                    returnMove = New Moves.Electric.Chargebeam()
                Case 452
                    returnMove = New Moves.Grass.WoodHammer()
                Case 453
                    returnMove = New Moves.Water.Aquajet()
                Case 454
                    returnMove = New Moves.Bug.AttackOrder()
                Case 455
                    returnMove = New Moves.Bug.DefendOrder()
                Case 456
                    returnMove = New Moves.Bug.HealOrder()
                Case 457
                    returnMove = New Moves.Rock.HeadSmash()
                Case 458
                    returnMove = New Moves.Normal.DoubleHit()
                Case 459
                    returnMove = New Moves.Dragon.RoarOfTime()
                    'Case 460
                    'Spacial Rend
                    'Case 461
                    'Lunar Dance
                Case 462
                    returnMove = New Moves.Normal.CrushGrip()
                Case 463
                    returnMove = New Moves.Fire.MagmaStorm()
                    'Case 464
                    'Dark Void
                Case 465
                    returnMove = New Moves.Grass.Seedflare()
                Case 466
                    returnMove = New Moves.Ghost.OminousWind()
                    'Case 467
                    'Shadow Force
                Case 468
                    returnMove = New Moves.Dark.HoneClaws()
                    'Case 469
                    'Wide Guard
                    'Case 470
                    'Guard Split
                    'Case 471
                    'Power Split
                    'Case 472
                    'Wonder Room
                Case 473
                    returnMove = New Moves.Psychic.Psyshock()
                Case 474
                    returnMove = New Moves.Poison.Venoshock()
                Case 475
                    returnMove = New Moves.Steel.Autotomize()
                    'Case 476
                    'Rage Powder
                    'Case 477
                    'Telekinesis
                    'Case 478
                    'Magic Room
                Case 479
                    returnMove = New Moves.Rock.SmackDown()
                    'Case 480
                    'Storm Throw
                Case 481
                    returnMove = New Moves.Fire.FlameBurst()
                Case 482
                    returnMove = New Moves.Poison.SludgeWave()
                Case 483
                    returnMove = New Moves.Bug.QuiverDance()
                Case 484
                    returnMove = New Moves.Steel.HeavySlam()
                    'Case 485
                    'Synchronoise
                Case 486
                    returnMove = New Moves.Electric.ElectroBall()
                    'Case 487
                    'Soak
                Case 488
                    returnMove = New Moves.Fire.FlameCharge()
                Case 489
                    returnMove = New Moves.Poison.Coil()
                Case 490
                    returnMove = New Moves.Fighting.LowSweep()
                Case 491
                    returnMove = New Moves.Poison.AcidSpray()
                Case 492
                    returnMove = New Moves.Dark.FoulPlay()
                    'Case 493
                    'Simple Beam
                    'Case 494
                    'Entertainment
                    'Case 495
                    'After You
                    'Case 496
                    'Round
                    'Case 497
                    'Echoed Voice
                Case 498
                    returnMove = New Moves.Normal.ChipAway()
                Case 499
                    returnMove = New Moves.Poison.ClearSmog()
                Case 500
                    returnMove = New Moves.Psychic.StoredPower()
                    'Case 501
                    'Quick Guard
                    'Case 502
                    'Ally Switch
                Case 503
                    returnMove = New Moves.Water.Scald()
                Case 504
                    returnMove = New Moves.Normal.ShellSmash()
                Case 505
                    returnMove = New Moves.Psychic.HealPulse()
                Case 506
                    returnMove = New Moves.Ghost.Hex()
                    'Case 507
                    'Sky Drop
                Case 508
                    returnMove = New Moves.Steel.ShiftGear()
                Case 509
                    returnMove = New Moves.Fighting.CircleThrow()
                    'Case 510
                    'Incinerate
                    'Case 511
                    'Quash
                Case 512
                    returnMove = New Moves.Flying.Acrobatics()
                    'Case 513
                    'Reflect Type
                    'Case 514
                    'Retaliate
                    'Case 515
                    'Final Gambit
                    'Case 516
                    'Bestow
                Case 517
                    returnMove = New Moves.Fire.Inferno()
                Case 518
                    returnMove = New Moves.Water.WaterPledge()
                Case 519
                    returnMove = New Moves.Fire.FirePledge()
                Case 520
                    returnMove = New Moves.Grass.GrassPledge()
                Case 521
                    returnMove = New Moves.Electric.VoltSwitch()
                Case 522
                    returnMove = New Moves.Bug.StruggleBug()
                Case 523
                    returnMove = New Moves.Ground.Bulldoze()
                Case 524
                    returnMove = New Moves.Ice.FrostBreath
                Case 525
                    returnMove = New Moves.Dragon.DragonTail()
                Case 526
                    returnMove = New Moves.Normal.WorkUp()
                Case 527
                    returnMove = New Moves.Electric.Electroweb()
                Case 528
                    returnMove = New Moves.Electric.WildCharge()
                Case 529
                    returnMove = New Moves.Ground.DrillRun()
                Case 530
                    returnMove = New Moves.Dragon.DualChop()
                Case 531
                    returnMove = New Moves.Psychic.HeartStamp()
                Case 532
                    returnMove = New Moves.Grass.HornLeech()
                Case 533
                    returnMove = New Moves.Fighting.SacredSword()
                Case 534
                    returnMove = New Moves.Water.RazorShell()
                Case 535
                    returnMove = New Moves.Fire.HeatCrash()
                Case 536
                    returnMove = New Moves.Grass.LeafTornado()
                Case 537
                    returnMove = New Moves.Bug.Steamroller()
                Case 538
                    returnMove = New Moves.Grass.CottonGuard()
                Case 539
                    returnMove = New Moves.Dark.NightDaze()
                    'Case 540
                    'Psystrike
                Case 541
                    returnMove = New Moves.Normal.TailSlap()
                Case 542
                    returnMove = New Moves.Flying.Hurricane()
                Case 543
                    returnMove = New Moves.Normal.HeadCharge()
                Case 544
                    returnMove = New Moves.Steel.GearGrind()
                Case 545
                    returnMove = New Moves.Fire.SearingShot()
                    'Case 546
                    'Techno Blast
                    'Case 547
                    'Relic Song
                Case 548
                    returnMove = New Moves.Fighting.SecretSword()
                    'Case 549
                    'Glaciate
                Case 550
                    returnMove = New Moves.Electric.BoltStrike()
                Case 551
                    returnMove = New Moves.Fire.BlueFlare()
                Case 552
                    returnMove = New Moves.Fire.FieryDance()
                Case 553
                    returnMove = New Moves.Ice.FreezeShock()
                Case 554
                    returnMove = New Moves.Ice.IceBurn()
                Case 555
                    returnMove = New Moves.Dark.Snarl()
                Case 556
                    returnMove = New Moves.Ice.IcicleCrash()
                Case 557
                    returnMove = New Moves.Fire.VCreate()
                Case 558
                    returnMove = New Moves.Fire.FusionFlare()
                Case 559
                    returnMove = New Moves.Electric.FusionBolt()
                Case 560
                    returnMove = New Moves.Fighting.Ride()
                    'Case 561
                    'Mat Block
                    'Case 562
                    'Belch
                    'Case 563
                    'Rototiller
                Case 564
                    returnMove = New Moves.Bug.StickyWeb()
                    'Case 565
                    'Fell Stinger
                    'Case 566
                    'Phantom Force
                    'Case 567
                    'Trick-or-Treat
                Case 568
                    returnMove = New Moves.Normal.NobleRoar()
                    'Case 569
                    'Ion Deluge
                Case 570
                    returnMove = New Moves.Electric.ParabolicCharge()
                    'Case 571
                    'Forest's Curse
                Case 572
                    returnMove = New Moves.Grass.PetalBlizzard()
                Case 573
                    returnMove = New Moves.Ice.FreezeDry()
                Case 574
                    returnMove = New Moves.Fairy.DisarmingVoice()
                    'Case 575
                    'Parting Shot
                    'Case 576
                    'Topsy-Turvy
                Case 577
                    returnMove = New Moves.Fairy.DrainingKiss()
                    'Case 578
                    'Crafty Shield
                    'Case 579
                    'Flower Shield
                    'Case 580
                    'Grassy Terrain
                    'Case 581
                    'Misty Terrain
                    'Case 582
                    'Electrify
                Case 583
                    returnMove = New Moves.Fairy.PlayRough()
                Case 584
                    returnMove = New Moves.Fairy.FairyWind()
                Case 585
                    returnMove = New Moves.Fairy.Moonblast()
                Case 586
                    returnMove = New Moves.Normal.Boomburst()
                    'Case 587
                    'Fairy Lock
                Case 588
                    returnMove = New Moves.Steel.KingsShield()
                Case 589
                    returnMove = New Moves.Normal.PlayNice()
                Case 590
                    returnMove = New Moves.Normal.Confide()
                    'Case 591
                    'Diamond Storm
                Case 592
                    returnMove = New Moves.Water.SteamEruption()
                    'Case 593
                    'Hyperspace Hole
                Case 594
                    returnMove = New Moves.Water.WaterShuriken()
                Case 595
                    returnMove = New Moves.Fire.MysticalFire()
                    'Case 596
                    'Spiky Shield
                    'Case 597
                    'Aromatic Mist
                Case 598
                    returnMove = New Moves.Electric.EerieImpulse()
                    'Case 599
                    'Vemon Drench
                    'Case 600
                    'Powder
                    'Case 601
                    'Geomancy
                    'Case 602
                    'Macnetic Flux
                    'Case 603
                    'Happy Hour
                    'Case 604
                    'Electric Terrain
                Case 605
                    returnMove = New Moves.Fairy.DazzlingGleam()
                    'Case 606
                    'Celebrate
                    'Case 607
                    'Hold Hands
                Case 608
                    returnMove = New Moves.Fairy.BabyDollEyes()
                Case 609
                    returnMove = New Moves.Electric.Nuzzle()
                    'Case 610
                    'Hold Back
                Case 611
                    returnMove = New Moves.Bug.Infestation()
                Case 612
                    returnMove = New Moves.Fighting.PowerUpPunch()
                Case 613
                    returnMove = New Moves.Flying.OblivionWing()
                    'Case 614
                    'Tousand Arrows
                    'Case 615
                    'Thousand Waves
                Case 616
                    returnMove = New Moves.Ground.LandsWrath()
                    'Case 617
                    'Light of Ruin
                Case 999
                    If GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                        returnMove = New Moves.Special.TheDerpMove()
                    Else
                        returnMove = New Moves.Normal.Pound()
                    End If
                Case Else
                    'Try to load a GameMode move.
                    Dim gameModeMove As Attack = GameModeAttackLoader.GetAttackByID(ID)
                    If Not gameModeMove Is Nothing And GameModeManager.ActiveGameMode.IsDefaultGamemode = False Then
                        returnMove = gameModeMove.Copy()
                    Else
                        returnMove = New Moves.Normal.Pound()
                        returnMove.IsDefaultMove = True
                    End If
            End Select

            returnMove.OriginalID = ID
            Return returnMove
        End Function

        Public Function GetEffectChance(ByVal i As Integer, ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Dim chance As Integer = Me.EffectChances(i)

            If Me.HasSecondaryEffect = True Then
                Dim p As Pokemon = BattleScreen.OwnPokemon
                If own = False Then
                    p = BattleScreen.OppPokemon
                End If

                If p.Ability.Name.ToLower() = "serene grace" Then
                    chance *= 2
                End If

                Dim waterPledge As Integer = BattleScreen.FieldEffects.OwnWaterPledge
                If own = False Then
                    waterPledge = BattleScreen.FieldEffects.OppWaterPledge
                End If
                If waterPledge > 0 Then
                    chance *= 2
                End If
            End If

            chance = chance.Clamp(0, 100)

            Return chance
        End Function

        ''' <summary>
        ''' Gets called prior to using the attack.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub PreAttack(ByVal Own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING HERE
        End Sub

        ''' <summary>
        ''' If the move fails prior to using it. Return True for failing.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function MoveFailBeforeAttack(ByVal Own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            'DO NOTHING HERE
            Return False
        End Function

        ''' <summary>
        ''' Returns the BasePower of this move. Defaults to Power.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetBasePower(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Return Me.Power
        End Function

        ''' <summary>
        ''' Returns the calculated damage of this move.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetDamage(ByVal Critical As Boolean, ByVal Own As Boolean, ByVal targetPokemon As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Return BattleCalculation.CalculateDamage(Me, Critical, Own, targetPokemon, BattleScreen)
        End Function

        ''' <summary>
        ''' Returns how many times this move is getting used in a row.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetTimesToAttack(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Return Me.TimesToAttack
        End Function

        ''' <summary>
        ''' Event that occurs when the move connects.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveHits(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            If Me.IsGameModeMove = True Then
                AttackSpecialFunctions.ExecuteAttackFunction(Me, own, BattleScreen)
            Else
                'DO NOTHING HERE (will do secondary effect if moves overrides it)
            End If
        End Sub

        Public Overridable Sub MoveRecoil(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING HERE (will do recoil if moves overrides it)
        End Sub

        ''' <summary>
        ''' Event that occurs when the move misses its target.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveMisses(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING HERE
        End Sub

        ''' <summary>
        ''' If the move gets blocked by a protection move.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveProtectedDetected(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING HERE
        End Sub

        ''' <summary>
        ''' Event that occurs when the move has no effect on the target.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveHasNoEffect(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING HERE
        End Sub

        ''' <summary>
        ''' Returns the type of the move. Defaults to the Type field.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetAttackType(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Element
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            If p.Ability.Name.ToLower() = "normalize" Then
                Return New Element(Element.Types.Normal)
            End If

            If Me.Type.Type = Element.Types.Normal Then
                If p.Ability.Name.ToLower() = "pixilate" Then
                    Return New Element(Element.Types.Fairy)
                End If
                If p.Ability.Name.ToLower() = "refrigerate" Then
                    Return New Element(Element.Types.Ice)
                End If
                If p.Ability.Name.ToLower() = "aerilate" Then
                    Return New Element(Element.Types.Flying)
                End If
            End If

            Return Me.Type
        End Function

        ''' <summary>
        ''' Returns the accuracy of this move.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetAccuracy(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Integer
            Return Me.Accuracy
        End Function

        ''' <summary>
        ''' If the PP of this move should get deducted when using it. Default is True.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function DeductPP(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            Return True
        End Function

        ''' <summary>
        ''' If the Accuracy and Evasion parameters of Pokémon and moves should get used for this attack.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Function GetUseAccEvasion(ByVal own As Boolean, ByVal BattleScreen As BattleScreen) As Boolean
            Return Me.UseAccEvasion
        End Function

        ''' <summary>
        ''' Event that occurs when the move gets selected in the menu.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveSelected(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING
        End Sub

        ''' <summary>
        ''' Event that occurs before this move deals damage.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub BeforeDealingDamage(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING
        End Sub

        ''' <summary>
        ''' Event that occurs when this move's damage gets absorbed by a substitute.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub AbsorbedBySubstitute(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING
        End Sub

        ''' <summary>
        ''' Event that occurs when the Soundproof ability blocks this move.
        ''' </summary>
        ''' <param name="Own">If the own Pokémon used the move.</param>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Overridable Sub MoveFailsSoundproof(ByVal own As Boolean, ByVal BattleScreen As BattleScreen)
            'DO NOTHING
        End Sub

        ''' <summary>
        ''' Returns the attack stat of a Pokémon (Physical or Special).
        ''' </summary>
        ''' <param name="p">The Pokémon that used the move.</param>
        Public Overridable Function GetUseAttackStat(ByVal p As Pokemon) As Integer
            If Me.Category = Categories.Physical Then
                Return p.Attack
            Else
                Return p.SpAttack
            End If
        End Function

        ''' <summary>
        ''' Returns the defense stat of a Pokémon (Physical or Special).
        ''' </summary>
        ''' <param name="p">The Pokémon that used the move.</param>
        Public Overridable Function GetUseDefenseStat(ByVal p As Pokemon) As Integer
            If Me.Category = Categories.Physical Then
                Return p.Defense
            Else
                Return p.SpDefense
            End If
        End Function

        ''' <summary>
        ''' If the AI is allowed to use this move.
        ''' </summary>
        ''' <param name="BattleScreen">Reference to the BattleScreen.</param>
        Public Function AIUseMove(ByVal BattleScreen As BattleScreen) As Boolean
            Return True
        End Function

#Region "Animation"

        Public Sub UserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen)
            If Core.Player.ShowBattleAnimations = 1 Then
                Me.InternalUserPokemonMoveAnimation(BattleScreen)
            End If
        End Sub

        Public Overridable Sub InternalUserPokemonMoveAnimation(ByVal BattleScreen As BattleScreen)
            'Override this method in the attack class to insert the move animation query objects into the queue.
        End Sub

        Public Sub OpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen)
            If Core.Player.ShowBattleAnimations = 1 Then
                Me.InternalOpponentPokemonMoveAnimation(BattleScreen)
            End If
        End Sub

        Public Overridable Sub InternalOpponentPokemonMoveAnimation(ByVal BattleScreen As BattleScreen)
            'Override this method in the attack class to insert the move animation query objects into the queue.
        End Sub

#End Region

        ''' <summary>
        ''' Returns a copy of this move.
        ''' </summary>
        Public Function Copy() As Attack
            Dim m As Attack

            If Me.IsGameModeMove = True Then
                m = GameModeAttackLoader.GetAttackByID(Me.ID)
            Else
                m = GetAttackByID(Me.ID)
            End If

            'Set definition properties:
            m.OriginalPP = Me.OriginalPP
            m.CurrentPP = Me.CurrentPP
            m.MaxPP = Me.MaxPP
            m.OriginalID = Me.OriginalID

            Return m
        End Function

        ''' <summary>
        ''' Builds an instance of AttackV2 with PP and MaxPP set.
        ''' </summary>
        ''' <param name="InputData">Data in the format "ID,MaxPP,CurrentPP"</param>
        Public Shared Function ConvertStringToAttack(ByVal InputData As String) As Attack
            If InputData <> "" Then
                Dim Data() As String = InputData.Split(CChar(","))
                Dim a As Attack = GetAttackByID(CInt(Data(0)))

                If Not a Is Nothing Then
                    a.MaxPP = CInt(Data(1))
                    a.CurrentPP = CInt(Data(2))
                End If

                Return a
            Else
                Return Nothing
            End If
        End Function

        ''' <summary>
        ''' Raises the PP of the move by one stage.
        ''' </summary>
        Public Function RaisePP() As Boolean
            Select Case Me.OriginalPP
                Case 5
                    Select Case Me.MaxPP
                        Case 5, 6, 7
                            Me.CurrentPP += 1
                            Me.MaxPP += 1
                            Return True
                    End Select
                Case 10
                    Select Case Me.MaxPP
                        Case 10, 12, 14
                            Me.CurrentPP += 2
                            Me.MaxPP += 2
                            Return True
                    End Select
                Case 15
                    Select Case Me.MaxPP
                        Case 15, 18, 21
                            Me.CurrentPP += 3
                            Me.MaxPP += 3
                            Return True
                    End Select
                Case 20
                    Select Case Me.MaxPP
                        Case 20, 24, 28
                            Me.CurrentPP += 4
                            Me.MaxPP += 4
                            Return True
                    End Select
                Case 25
                    Select Case Me.MaxPP
                        Case 25, 30, 35
                            Me.CurrentPP += 5
                            Me.MaxPP += 5
                            Return True
                    End Select
                Case 30
                    Select Case Me.MaxPP
                        Case 30, 36, 42
                            Me.CurrentPP += 6
                            Me.MaxPP += 6
                            Return True
                    End Select
                Case 35
                    Select Case Me.MaxPP
                        Case 35, 42, 49
                            Me.CurrentPP += 7
                            Me.MaxPP += 7
                            Return True
                    End Select
                Case 40
                    Select Case Me.MaxPP
                        Case 40, 48, 56
                            Me.CurrentPP += 8
                            Me.MaxPP += 8
                            Return True
                    End Select
            End Select

            Me.CurrentPP = CInt(MathHelper.Clamp(Me.CurrentPP, 0, Me.MaxPP))

            Return False
        End Function

        ''' <summary>
        ''' Returns the texture representing the category of this move.
        ''' </summary>
        Public Function GetDamageCategoryImage() As Texture2D
            Dim r As New Rectangle(0, 0, 0, 0)

            Select Case Me.Category
                Case Categories.Physical
                    r = New Rectangle(115, 0, 28, 14)
                Case Categories.Special
                    r = New Rectangle(115, 14, 28, 14)
                Case Categories.Status
                    r = New Rectangle(115, 28, 28, 14)
            End Select

            Return TextureManager.GetTexture("GUI\Menus\Types", r, "")
        End Function

        ''' <summary>
        ''' Returns a saveable string.
        ''' </summary>
        Public Overrides Function ToString() As String
            Return Me.OriginalID.ToString() & "," & Me.MaxPP.ToString() & "," & Me.CurrentPP.ToString()
        End Function

    End Class

End Namespace
