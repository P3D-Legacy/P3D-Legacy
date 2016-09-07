Public Class Ability

    Public Property Name() As String
        Get
            Return Me._name
        End Get
        Set(value As String)
            Me._name = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me._description
        End Get
        Set(value As String)
            Me._description = value
        End Set
    End Property

    Public Property ID() As Integer
        Get
            Return Me._ID
        End Get
        Set(value As Integer)
            Me._ID = value
        End Set
    End Property

    Private _name As String
    Private _description As String
    Private _ID As Integer

    Public Sub New(ByVal ID As Integer, ByVal Name As String, ByVal Description As String)
        Me.ID = ID
        Me.Name = Name
        Me.Description = Description
    End Sub

    Public Shared Function GetAbilityByID(ByVal ID As Integer) As Ability
        Select Case ID
            Case 1
                Return New Abilities.Stench
            Case 2
                Return New Abilities.Drizzle
            Case 3
                Return New Abilities.SpeedBoost
            Case 4
                Return New Abilities.BattleArmor
            Case 5
                Return New Abilities.Sturdy
            Case 6
                Return New Abilities.Damp
            Case 7
                Return New Abilities.Limber
            Case 8
                Return New Abilities.SandVeil
            Case 9
                Return New Abilities.StaticAbility
            Case 10
                Return New Abilities.VoltAbsorb
            Case 11
                Return New Abilities.WaterAbsorb
            Case 12
                Return New Abilities.Oblivious
            Case 13
                Return New Abilities.CloudNine
            Case 14
                Return New Abilities.Compoundeyes
            Case 15
                Return New Abilities.Insomnia
            Case 16
                Return New Abilities.ColorChange
            Case 17
                Return New Abilities.Immunity
            Case 18
                Return New Abilities.FlashFire
            Case 19
                Return New Abilities.ShieldDust
            Case 20
                Return New Abilities.OwnTempo
            Case 21
                Return New Abilities.SuctionCups
            Case 22
                Return New Abilities.Intimidate
            Case 23
                Return New Abilities.ShadowTag
            Case 24
                Return New Abilities.RoughSkin
            Case 25
                Return New Abilities.WonderGuard
            Case 26
                Return New Abilities.Levitate
            Case 27
                Return New Abilities.EffectSpore
            Case 28
                Return New Abilities.Synchronize
            Case 29
                Return New Abilities.ClearBody
            Case 30
                Return New Abilities.NaturalCure
            Case 31
                Return New Abilities.Lightningrod
            Case 32
                Return New Abilities.SereneGrace
            Case 33
                Return New Abilities.SwiftSwim
            Case 34
                Return New Abilities.Chlorophyll
            Case 35
                Return New Abilities.Illuminate
            Case 36
                Return New Abilities.Trace
            Case 37
                Return New Abilities.HugePower
            Case 38
                Return New Abilities.PoisonPoint
            Case 39
                Return New Abilities.InnerFocus
            Case 40
                Return New Abilities.MagmaArmor
            Case 41
                Return New Abilities.WaterVeil
            Case 42
                Return New Abilities.MagnetPull
            Case 43
                Return New Abilities.Soundproof
            Case 44
                Return New Abilities.RainDish
            Case 45
                Return New Abilities.SandStream
            Case 46
                Return New Abilities.Pressure
            Case 47
                Return New Abilities.ThickFat
            Case 48
                Return New Abilities.EarlyBird
            Case 49
                Return New Abilities.FlameBody
            Case 50
                Return New Abilities.RunAway
            Case 51
                Return New Abilities.KeenEye
            Case 52
                Return New Abilities.HyperCutter
            Case 53
                Return New Abilities.Pickup
            Case 54
                Return New Abilities.Truant
            Case 55
                Return New Abilities.Hustle
            Case 56
                Return New Abilities.CuteCharm
            Case 57
                Return New Abilities.Plus
            Case 58
                Return New Abilities.Minus
            Case 59
                Return New Abilities.Forecast
            Case 60
                Return New Abilities.StickyHold
            Case 61
                Return New Abilities.ShedSkin
            Case 62
                Return New Abilities.Guts
            Case 63
                Return New Abilities.MarvelScale
            Case 64
                Return New Abilities.LiquidOoze
            Case 65
                Return New Abilities.Overgrow
            Case 66
                Return New Abilities.Blaze
            Case 67
                Return New Abilities.Torrent
            Case 68
                Return New Abilities.Swarm
            Case 69
                Return New Abilities.RockHead
            Case 70
                Return New Abilities.Drought
            Case 71
                Return New Abilities.ArenaTrap
            Case 72
                Return New Abilities.VitalSpirit
            Case 73
                Return New Abilities.WhiteSmoke
            Case 74
                Return New Abilities.PurePower
            Case 75
                Return New Abilities.ShellArmor
            Case 76
                Return New Abilities.AirLock
            Case 77
                Return New Abilities.TangledFeet
            Case 78
                Return New Abilities.MotorDrive
            Case 79
                Return New Abilities.Rivalry
            Case 80
                Return New Abilities.Steadfast
            Case 81
                Return New Abilities.SnowCloak
            Case 82
                Return New Abilities.Gluttony
            Case 83
                Return New Abilities.AngerPoint
            Case 84
                Return New Abilities.Unburden
            Case 85
                Return New Abilities.Heatproof
            Case 86
                Return New Abilities.Simple
            Case 87
                Return New Abilities.DrySkin
            Case 88
                Return New Abilities.Download
            Case 89
                Return New Abilities.IronFist
            Case 90
                Return New Abilities.PoisonHeal
            Case 91
                Return New Abilities.Adaptability
            Case 92
                Return New Abilities.SkillLink
            Case 93
                Return New Abilities.Hydration
            Case 94
                Return New Abilities.SolarPower
            Case 95
                Return New Abilities.QuickFeet
            Case 96
                Return New Abilities.Normalize
            Case 97
                Return New Abilities.Sniper
            Case 98
                Return New Abilities.MagicGuard
            Case 99
                Return New Abilities.NoGuard
            Case 100
                Return New Abilities.Stall
            Case 101
                Return New Abilities.Technician
            Case 102
                Return New Abilities.LeafGuard
            Case 103
                Return New Abilities.Klutz
            Case 104
                Return New Abilities.MoldBreaker
            Case 105
                Return New Abilities.SuperLuck
            Case 106
                Return New Abilities.Aftermath
            Case 107
                Return New Abilities.Anticipation
            Case 108
                Return New Abilities.Forewarn
            Case 109
                Return New Abilities.Unaware
            Case 110
                Return New Abilities.TintedLens
            Case 111
                Return New Abilities.Filter
            Case 112
                Return New Abilities.SlowStart
            Case 113
                Return New Abilities.Scrappy
            Case 114
                Return New Abilities.StormDrain
            Case 115
                Return New Abilities.IceBody
            Case 116
                Return New Abilities.SolidRock
            Case 117
                Return New Abilities.SnowWarning
            Case 118
                Return New Abilities.HoneyGather
            Case 119
                Return New Abilities.Frisk
            Case 120
                Return New Abilities.Reckless
            Case 121
                Return New Abilities.Multitype
            Case 122
                Return New Abilities.FlowerGift
            Case 123
                Return New Abilities.BadDreams
            Case 124
                Return New Abilities.Pickpocket
            Case 125
                Return New Abilities.SheerForce
            Case 126
                Return New Abilities.Contrary
            Case 127
                Return New Abilities.Unnerve
            Case 128
                Return New Abilities.Defiant
            Case 129
                Return New Abilities.Defeatist
            Case 130
                Return New Abilities.CursedBody
            Case 131
                Return New Abilities.Healer
            Case 132
                Return New Abilities.FriendGuard
            Case 133
                Return New Abilities.WeakArmor
            Case 134
                Return New Abilities.HeavyMetal
            Case 135
                Return New Abilities.LightMetal
            Case 136
                Return New Abilities.Multiscale
            Case 137
                Return New Abilities.ToxicBoost
            Case 138
                Return New Abilities.FlareBoost
            Case 139
                Return New Abilities.Harvest
            Case 140
                Return New Abilities.Telepathy
            Case 141
                Return New Abilities.Moody
            Case 142
                Return New Abilities.Overcoat
            Case 143
                Return New Abilities.PoisonTouch
            Case 144
                Return New Abilities.Regenerator
            Case 145
                Return New Abilities.BigPecks
            Case 146
                Return New Abilities.SandRush
            Case 147
                Return New Abilities.WonderSkin
            Case 148
                Return New Abilities.Analytic
            Case 149
                Return New Abilities.Illusion
            Case 150
                Return New Abilities.Imposter
            Case 151
                Return New Abilities.Infiltrator
            Case 152
                Return New Abilities.Mummy
            Case 153
                Return New Abilities.Moxie
            Case 154
                Return New Abilities.Justified
            Case 155
                Return New Abilities.Rattled
            Case 156
                Return New Abilities.MagicBounce
            Case 157
                Return New Abilities.SapSipper
            Case 158
                Return New Abilities.Prankster
            Case 159
                Return New Abilities.SandForce
            Case 160
                Return New Abilities.IronBarbs
            Case 161
                Return New Abilities.ZenMode
            Case 162
                Return New Abilities.VictoryStar
            Case 163
                Return New Abilities.Turboblaze
            Case 164
                Return New Abilities.Teravolt
            Case 165
                Return New Abilities.AromaVeil
            Case 166
                Return New Abilities.FlowerVeil
            Case 167
                Return New Abilities.CheekPouch
            Case 168
                Return New Abilities.Protean
            Case 169
                Return New Abilities.FurCoat
            Case 170
                Return New Abilities.Magician
            Case 171
                Return New Abilities.Bulletproof
            Case 172
                Return New Abilities.Competitive
            Case 173
                Return New Abilities.StrongJaw
            Case 174
                Return New Abilities.Refrigerate
            Case 175
                Return New Abilities.SweetVeil
            Case 176
                Return New Abilities.StanceChange
            Case 177
                Return New Abilities.GaleWings
            Case 178
                Return New Abilities.MegaLauncher
            Case 179
                Return New Abilities.GrassPelt
            Case 180
                Return New Abilities.Symbiosis
            Case 181
                Return New Abilities.ToughClaws
            Case 182
                Return New Abilities.Pixilate
            Case 183
                Return New Abilities.Gooey
            Case 186
                Return New Abilities.DarkAura
            Case 187
                Return New Abilities.FairyAura
            Case 188
                Return New Abilities.AuraBreak
            Case Else
                Return New Abilities.Stench
        End Select
    End Function

    Public Overridable Sub EndBattle(ByVal parentPokemon As Pokemon)
    End Sub

    Public Overridable Sub SwitchOut(ByVal parentPokemon As Pokemon)
    End Sub

End Class