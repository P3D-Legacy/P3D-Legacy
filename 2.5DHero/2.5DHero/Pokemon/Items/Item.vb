''' <summary>
''' An item the player stores in their inventory.
''' </summary>
Public Class Item

    ''Implement the interface to allow the copy of an item instance.
    Implements ICopyAble

    ''' <summary>
    ''' The type of item. This is also the bag they get sorted into.
    ''' </summary>
    Public Enum ItemTypes
        ''' <summary>
        ''' The default item category for misc. items.
        ''' </summary>
        Standard
        ''' <summary>
        ''' Medicine items that restore Pokémon.
        ''' </summary>
        Medicine
        ''' <summary>
        ''' Plants, like berries and apricorns.
        ''' </summary>
        Plants
        ''' <summary>
        ''' All Poké Balls.
        ''' </summary>
        Pokéballs
        ''' <summary>
        ''' TMs and HMs.
        ''' </summary>
        Machines
        ''' <summary>
        ''' Keyitems of the game.
        ''' </summary>
        KeyItems
        ''' <summary>
        ''' Mail items.
        ''' </summary>
        Mail
        ''' <summary>
        ''' Items to be used in battle.
        ''' </summary>
        BattleItems
    End Enum

#Region "Fields"

    Private _id As Integer = 0
    Protected _name As String = ""
    Protected _pluralName As String = ""
    Protected _description As String = ""

    Protected _pokeDollarPrice As Integer = 0
    Protected _battlePointsPrice As Integer = 1
    Protected _itemType As ItemTypes = ItemTypes.Standard
    Protected _catchMultiplier As Single = 1.0F
    Protected _maxStack As Integer = 999
    Protected _sortValue As Integer = 0

    Protected _texture As Texture2D
    Private _additionalData As String = ""

    Protected _canBeTraded As Boolean = True
    Protected _canBeHold As Boolean = True
    Protected _canBeUsed As Boolean = True
    Protected _canBeUsedInBattle As Boolean = True
    Protected _canBeTossed As Boolean = True

    Protected _isBall As Boolean = False
    Protected _isBerry As Boolean = False
    Protected _isHealingItem As Boolean = False
    Protected _isMail As Boolean = False
    Protected _isMegaStone As Boolean = False
    Protected _isPlate As Boolean = False

    Protected _flingDamage As Integer = 30
    Protected _requiresPokemonSelectInBattle As Boolean = True

#End Region

#Region "Properties"

    ''' <summary>
    ''' The singular item name.
    ''' </summary>
    Public ReadOnly Property Name() As String
        Get
            Return Me._name
        End Get
    End Property

    ''' <summary>
    ''' The ID of the item.
    ''' </summary>
    Public ReadOnly Property ID() As Integer
        Get
            Return Me._id
        End Get
    End Property

    ''' <summary>
    ''' The plural name of the item.
    ''' </summary>
    Public ReadOnly Property PluralName() As String
        Get
            Return Me._pluralName
        End Get
    End Property

    ''' <summary>
    ''' The price of this item if the player purchases it in exchange for PokéDollars. This halves when selling an item to the store.
    ''' </summary>
    Public ReadOnly Property PokéDollarPrice() As Integer
        Get
            Return Me._pokeDollarPrice
        End Get
    End Property

    ''' <summary>
    ''' The price of this item if the player purchases it exchange for BattlePoints.
    ''' </summary>
    Public ReadOnly Property BattlePointsPrice() As Integer
        Get
            Return Me._battlePointsPrice
        End Get
    End Property

    ''' <summary>
    ''' The type of this item. This also controls in which bag this item gets sorted.
    ''' </summary>
    Public ReadOnly Property ItemType() As ItemTypes
        Get
            Return Me._itemType
        End Get
    End Property

    ''' <summary>
    ''' The default catch multiplier if the item gets used as a Pokéball.
    ''' </summary>
    Public ReadOnly Property CatchMultiplier() As Single
        Get
            Return Me._catchMultiplier
        End Get
    End Property

    ''' <summary>
    ''' The maximum amount of this item type (per ID) that can be stored in the bag.
    ''' </summary>
    Public ReadOnly Property MaxStack() As Integer
        Get
            Return Me._maxStack
        End Get
    End Property

    ''' <summary>
    ''' A value that can be used to sort items in the bag after. Lower values make items appear closer to the top.
    ''' </summary>
    Public ReadOnly Property SortValue() As Integer
        Get
            Return Me._sortValue
        End Get
    End Property

    ''' <summary>
    ''' The texture of this item.
    ''' </summary>
    Public ReadOnly Property Texture() As Texture2D
        Get
            Return Me._texture
        End Get
    End Property

    ''' <summary>
    ''' The bag description of this item.
    ''' </summary>
    Public ReadOnly Property Description() As String
        Get
            Return Me._description
        End Get
    End Property

    ''' <summary>
    ''' The additional data that is stored with this item.
    ''' </summary>
    Public Property AdditionalData() As String
        Get
            Return Me._additionalData
        End Get
        Set(value As String)
            Me._additionalData = value
        End Set
    End Property

    ''' <summary>
    ''' The damage the Fling move does when this item is attached to a Pokémon.
    ''' </summary>
    Public ReadOnly Property FlingDamage() As Integer
        Get
            Return Me._flingDamage
        End Get
    End Property

    ''' <summary>
    ''' If this item can be traded in for money.
    ''' </summary>
    Public ReadOnly Property CanBeTraded() As Boolean
        Get
            Return Me._canBeTraded
        End Get
    End Property

    ''' <summary>
    ''' If this item can be given to a Pokémon.
    ''' </summary>
    Public ReadOnly Property CanBeHold() As Boolean
        Get
            Return Me._canBeHold
        End Get
    End Property

    ''' <summary>
    ''' If this item can be used from the bag.
    ''' </summary>
    Public ReadOnly Property CanBeUsed() As Boolean
        Get
            Return Me._canBeUsed
        End Get
    End Property

    ''' <summary>
    ''' If this item can be used in battle.
    ''' </summary>
    Public ReadOnly Property CanBeUsedInBattle() As Boolean
        Get
            Return Me._canBeUsedInBattle
        End Get
    End Property

    ''' <summary>
    ''' If this item can be tossed in the bag.
    ''' </summary>
    Public ReadOnly Property CanBeTossed() As Boolean
        Get
            Return Me._canBeTossed
        End Get
    End Property

    ''' <summary>
    ''' If this item requires the player to select a Pokémon to use the item on in battle.
    ''' </summary>
    Public ReadOnly Property BattleSelectPokemon() As Boolean
        Get
            Return Me._requiresPokemonSelectInBattle
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Pokéball item.
    ''' </summary>
    Public ReadOnly Property IsBall() As Boolean
        Get
            Return Me._isBall
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Berry item.
    ''' </summary>
    Public ReadOnly Property IsBerry() As Boolean
        Get
            Return Me._isBerry
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Healing item.
    ''' </summary>
    Public ReadOnly Property IsHealingItem() As Boolean
        Get
            Return Me._isHealingItem
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Mail item.
    ''' </summary>
    Public ReadOnly Property IsMail() As Boolean
        Get
            Return Me._isMail
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Mega Stone.
    ''' </summary>
    Public ReadOnly Property IsMegaStone() As Boolean
        Get
            Return Me._isMegaStone
        End Get
    End Property

    ''' <summary>
    ''' If this item is a Plate.
    ''' </summary>
    Public ReadOnly Property IsPlate() As Boolean
        Get
            Return Me._isPlate
        End Get
    End Property

    ''' <summary>
    ''' The color for player dialogues.
    ''' </summary>
    Public Shared ReadOnly Property PlayerDialogueColor() As Color
        Get
            Return New Color(0, 128, 227)
        End Get
    End Property

#End Region

    ''' <summary>
    ''' Creates a new instance of the Item class.
    ''' </summary>
    ''' <param name="Name">The name of the Item.</param>
    ''' <param name="Price">The purchase price.</param>
    ''' <param name="ItemType">The type of Item.</param>
    ''' <param name="ID">The ID of this Item.</param>
    ''' <param name="CatchMultiplier">The CatchMultiplier of this Item.</param>
    ''' <param name="SortValue">The SortValue of this Item.</param>
    ''' <param name="TextureRectangle">The TextureRectangle from the "Items\ItemSheet" texture.</param>
    ''' <param name="Description">The description of this Item.</param>
    Public Sub New(ByVal Name As String, ByVal Price As Integer, ByVal ItemType As ItemTypes, ByVal ID As Integer, ByVal CatchMultiplier As Single, ByVal SortValue As Integer, ByVal TextureRectangle As Rectangle, ByVal Description As String)
        Me.Initialize(Name, Price, ItemType, ID, CatchMultiplier, SortValue, TextureRectangle, Description)
    End Sub

    ''' <summary>
    ''' Creates a new instance of the Item class without setting any properties.
    ''' </summary>
    Public Sub New()
        'Empty Constructor//
    End Sub

    ''' <summary>
    ''' Sets most properties of an Item class instance.
    ''' </summary>
    ''' <param name="Name">The name of the Item.</param>
    ''' <param name="Price">The purchase price.</param>
    ''' <param name="ItemType">The type of Item.</param>
    ''' <param name="ID">The ID of this Item.</param>
    ''' <param name="CatchMultiplier">The CatchMultiplier of this Item.</param>
    ''' <param name="SortValue">The SortValue of this Item.</param>
    ''' <param name="TextureRectangle">The TextureRectangle from the "Items\ItemSheet" texture.</param>
    ''' <param name="Description">The description of this Item.</param>
    Protected Sub Initialize(ByVal Name As String, ByVal Price As Integer, ByVal ItemType As ItemTypes, ByVal ID As Integer, ByVal CatchMultiplier As Single, ByVal SortValue As Integer, ByVal TextureRectangle As Rectangle, ByVal Description As String)
        Me._name = Name
        Me._pluralName = Name & "s" 'Default plural name with "s" at the end.
        Me._pokeDollarPrice = Price
        Me._itemType = ItemType
        Me._id = ID
        Me._catchMultiplier = CatchMultiplier
        Me._sortValue = SortValue
        Me._description = Description

        'If not registered, add the Item with name and ID to the temp list:
        If ItemNameID.ContainsKey(Me._name) = False Then
            ItemNameID.Add(Me._name, Me._id)
        End If

        Me._texture = TextureManager.GetTexture("Items\ItemSheet", TextureRectangle, "")
    End Sub

    ''' <summary>
    ''' The item gets used from the bag.
    ''' </summary>
    Public Overridable Sub Use()
        Logger.Debug("PLACEHOLDER FOR ITEM USE")
    End Sub

    ''' <summary>
    ''' A method that gets used when the item is applied to a Pokémon. Returns True if the action was successful.
    ''' </summary>
    ''' <param name="PokeIndex">The Index of the Pokémon in party.</param>
    Public Overridable Function UseOnPokemon(ByVal PokeIndex As Integer) As Boolean
        If PokeIndex < 0 Or PokeIndex > 5 Then
            Throw New ArgumentOutOfRangeException("PokeIndex", PokeIndex, "The index for a Pokémon in a player's party can only be between 0 and 5.")
        End If

        Logger.Debug("PLACEHOLDER FOR ITEM USE ON POKEMON")
        Return False
    End Function

    ''' <summary>
    ''' Tries to remove a single item of this item type from the player's bag and returns a message which changes depending on if the item that got removed was the last one of its kind.
    ''' </summary>
    Public Function RemoveItem() As String
        Core.Player.Inventory.RemoveItem(Me.ID, 1)
        If Core.Player.Inventory.GetItemAmount(Me.ID) = 0 Then
            Return "*There are no~" & Me.PluralName & " left." 'MASS EXTINCION
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Returns an item instance based on the passed in ID.
    ''' </summary>
    ''' <param name="ID">The desired item's ID.</param>
    Public Shared Function GetItemByID(ByVal ID As Integer) As Item
        'Check if the ID is available in the FileItem list.
        'Free Slots: 
        '243-246 (between TMs and HMs),
        '286-299 (Plates-Mail)
        '575-1999(MegaStones-Berries)
        For Each f As Items.FileItem In Core.Player.FileItems
            If f.Item.ID = ID Then
                Return f.Item
            End If
        Next

        Select Case ID
            Case 1
                Return New Items.Balls.Masterball()
            Case 2
                Return New Items.Balls.UltraBall()
            Case 3
                Return New Items.Balls.PremierBall()
            Case 4
                Return New Items.Balls.GreatBall()
            Case 5
                Return New Items.Balls.Pokeball()
            Case 6
                Return New Items.KeyItems.Bicycle()
            Case 7
                Return New Items.Medicine.LavaCookie()
            Case 8
                Return New Items.Stones.MoonStone()
            Case 9
                Return New Items.Medicine.Antidote()
            Case 10
                Return New Items.Medicine.BurnHeal()
            Case 11
                Return New Items.Medicine.IceHeal()
            Case 12
                Return New Items.Medicine.Awakening()
            Case 13
                Return New Items.Medicine.ParalyzeHeal()
            Case 14
                Return New Items.Medicine.FullRestore()
            Case 15
                Return New Items.Medicine.MaxPotion()
            Case 16
                Return New Items.Medicine.HyperPotion()
            Case 17
                Return New Items.Medicine.SuperPotion()
            Case 18
                Return New Items.Medicine.Potion()
            Case 19
                Return New Items.Standard.EscapeRope()
            Case 20
                Return New Items.Repels.Repel()
            Case 21
                Return New Items.Medicine.MaxElixir()
            Case 22
                Return New Items.Stones.FireStone()
            Case 23
                Return New Items.Stones.ThunderStone()
            Case 24
                Return New Items.Stones.WaterStone()
            Case 25
                Return New Items.Vitamins.Zinc()
            Case 26
                Return New Items.Vitamins.HPUp()
            Case 27
                Return New Items.Vitamins.Protein()
            Case 28
                Return New Items.Vitamins.Iron()
            Case 29
                Return New Items.Vitamins.Carbos()
            Case 30
                Return New Items.Standard.LuckyPunch()
            Case 31
                Return New Items.Vitamins.Calcium()
            Case 32, 501 'Remove shiny candy and return left over IDs here.
                Return New Items.Medicine.RareCandy()
            Case 33
                Return New Items.XItems.XAccuracy()
            Case 34
                Return New Items.Stones.LeafStone()
            Case 35
                Return New Items.Standard.MetalPowder()
            Case 36
                Return New Items.Standard.Nugget()
            Case 37
                Return New Items.Standard.PokeDoll()
            Case 38
                Return New Items.Medicine.FullHeal()
            Case 39
                Return New Items.Medicine.Revive()
            Case 40
                Return New Items.Medicine.MaxRevive()
            Case 41
                Return New Items.KeyItems.SSTicket()
            Case 42
                Return New Items.Repels.SuperRepel()
            Case 43
                Return New Items.Repels.MaxRepel()
            Case 44
                Return New Items.XItems.DireHit()
            Case 45
                Return New Items.Balls.CherishBall()
            Case 46
                Return New Items.Medicine.FreshWater()
            Case 47
                Return New Items.Medicine.SodaPop()
            Case 48
                Return New Items.Medicine.Lemonade()
            Case 49
                Return New Items.XItems.XAttack()
            Case 50
                Return New Items.XItems.XSpDef()
            Case 51
                Return New Items.XItems.XDefend()
            Case 52
                Return New Items.XItems.XSpeed()
            Case 53
                Return New Items.XItems.XSpecial()
            Case 54
                Return New Items.KeyItems.CoinCase()
            Case 55
                Return New Items.KeyItems.ItemFinder()
            Case 56
                Return New Items.KeyItems.CrystalWing()
            Case 57
                Return New Items.Standard.ExpShare()
            Case 58
                Return New Items.KeyItems.OldRod()
            Case 59
                Return New Items.KeyItems.GoodRod()
            Case 60
                Return New Items.Standard.SilverLeaf()
            Case 61
                Return New Items.KeyItems.SuperRod()
            Case 62
                Return New Items.Medicine.PPUp()
            Case 63
                Return New Items.Medicine.Ether()
            Case 64
                Return New Items.Medicine.MaxEther()
            Case 65
                Return New Items.Medicine.Elixir()
            Case 66
                Return New Items.KeyItems.RedScale()
            Case 67
                Return New Items.KeyItems.SecretPotion()
            Case 68
                Return New Items.XItems.GuardSpec()
            Case 69
                Return New Items.KeyItems.MysteryEgg()
            Case 70
                Return New Items.Standard.StickyBarb()
            Case 71
                Return New Items.KeyItems.SilverWing()
            Case 72
                Return New Items.Medicine.MooMooMilk()
            Case 73
                Return New Items.Standard.QuickClaw()
            Case 74
                Return New Items.Standard.ZoomLens()
            Case 75
                Return New Items.Standard.GoldLeaf()
            Case 76
                Return New Items.Standard.SoftSand()
            Case 77
                Return New Items.Standard.SharpBeak()
            Case 78
                Return New Items.KeyItems.RunningShoes()
            Case 79
                Return New Items.Balls.DiveBall()
            Case 80
                Return New Items.Balls.NetBall()
            Case 81
                Return New Items.Standard.PoisonBarb()
            Case 82
                Return New Items.Standard.KingsRock()
            Case 83
                Return New Items.Standard.PrismScale()
            Case 84
                Return New Items.Standard.ReaperCloth()
            Case 85
                Return New Items.Apricorns.RedApricorn()
            Case 86
                Return New Items.Plants.TinyMushroom()
            Case 87
                Return New Items.Plants.BigMushroom()
            Case 88
                Return New Items.Standard.SilverPowder()
            Case 89
                Return New Items.Apricorns.BluApricorn()
            Case 90
                Return New Items.Standard.SilkScarf()
            Case 91
                Return New Items.Standard.AmuletCoin()
            Case 92
                Return New Items.Apricorns.YlwApricorn()
            Case 93
                Return New Items.Apricorns.GrnApricorn()
            Case 94
                Return New Items.Standard.CleanseTag()
            Case 95
                Return New Items.Standard.MysticWater()
            Case 96
                Return New Items.Standard.TwistedSpoon()
            Case 97
                Return New Items.Apricorns.WhtApricorn()
            Case 98
                Return New Items.Standard.BlackBelt()
            Case 99
                Return New Items.Apricorns.BlkApricorn()
            Case 100
                Return New Items.Standard.Magmarizer()
            Case 101
                Return New Items.Apricorns.PnkApricorn()
            Case 102
                Return New Items.Standard.BlackGlasses()
            Case 103
                Return New Items.Standard.SlowPokeTail()
            Case 104
                Return New Items.Standard.PinkBow()
            Case 105
                Return New Items.Standard.Stick()
            Case 106
                Return New Items.Standard.SmokeBall()
            Case 107
                Return New Items.Standard.NeverMeltIce()
            Case 108
                Return New Items.Standard.Magnet()
            Case 109
                Return New Items.Standard.RareBone()
            Case 110
                Return New Items.Standard.Pearl()
            Case 111
                Return New Items.Standard.BigPearl()
            Case 112
                Return New Items.Stones.Everstone()
            Case 113
                Return New Items.Standard.SpellTag()
            Case 114
                Return New Items.Medicine.RageCandyBar()
            Case 115
                Return New Items.KeyItems.GSBall()
            Case 116
                Return New Items.KeyItems.BlueCard()
            Case 117
                Return New Items.Standard.MiracleSeed()
            Case 118
                Return New Items.Standard.ThickClub()
            Case 119
                Return New Items.Standard.FocusBand()
            Case 120
                Return New Items.Standard.Electirizer()
            Case 121
                Return New Items.Medicine.EnergyPowder()
            Case 122
                Return New Items.Medicine.EnergyRoot()
            Case 123
                Return New Items.Medicine.HealPowder()
            Case 124
                Return New Items.Medicine.RevivalHerb()
            Case 125
                Return New Items.Standard.HardStone()
            Case 126
                Return New Items.Standard.LuckyEgg()
            Case 127
                Return New Items.KeyItems.CardKey()
            Case 128
                Return New Items.KeyItems.MachinePart()
            Case 129
                Return New Items.Balls.QuickBall()
            Case 130
                Return New Items.KeyItems.LostItem()
            Case 131
                Return New Items.Standard.Stardust()
            Case 132
                Return New Items.Standard.StarPiece()
            Case 133
                Return New Items.KeyItems.BasementKey()
            Case 134
                Return New Items.KeyItems.Pass()
            Case 135
                Return New Items.Standard.ShinyStone()
            Case 136
                Return New Items.Standard.DuskStone()
            Case 137
                Return New Items.Standard.DawnStone()
            Case 138
                Return New Items.Standard.Charcoal()
            Case 139
                Return New Items.Medicine.BerryJuice()
            Case 140
                Return New Items.Standard.ScopeLens()
            Case 141
                Return New Items.Standard.Protector()
            Case 142
                Return New Items.Standard.LaggingTail()
            Case 143
                Return New Items.Standard.MetalCoat()
            Case 144
                Return New Items.Standard.DragonFang()
            Case 145
                Return New Items.Standard.WaveIncense()
            Case 146
                Return New Items.Standard.Leftovers()
            Case 147
                Return New Items.Standard.WhiteFlute()
            Case 148
                Return New Items.Standard.SootheBell()
            Case 149
                Return New Items.Standard.CometShard()
            Case 150
                Return New Items.Balls.TimerBall()
            Case 151
                Return New Items.Standard.DragonScale()
                'Case 152
                'Berserk Gene - needs image 
                'Should we actually make this?
            Case 153
                Return New Items.Standard.BalmMushroom()
            Case 154
                Return New Items.Standard.ShedShell()
            Case 155
                Return New Items.Standard.QuickPowder()
            Case 156
                Return New Items.Medicine.SacredAsh()
            Case 157
                Return New Items.Balls.HeavyBall()
            Case 158
                Return New Items.Balls.DuskBall()
            Case 159
                Return New Items.Balls.LevelBall()
            Case 160
                Return New Items.Balls.LureBall()
            Case 161
                Return New Items.Balls.FastBall()
            Case 162
                Return New Items.Standard.DeepSeaScale()
            Case 163
                Return New Items.Standard.LightBall()
            Case 164
                Return New Items.Balls.FriendBall()
            Case 165
                Return New Items.Balls.MoonBall()
            Case 166
                Return New Items.Balls.LoveBall()
            Case 167
                Return New Items.Standard.DeepSeaTooth()
            Case 168
                Return New Items.Balls.RepeatBall()
            Case 169
                Return New Items.Stones.SunStone()
            Case 170
                Return New Items.Standard.PolkadotBow()
            Case 171
                Return New Items.Standard.WideLens()
            Case 172
                Return New Items.Standard.UpGrade()
            Case 173
                Return New Items.Standard.PearlString()
            Case 174
                Return New Items.Balls.LuxuryBall()
            Case 175
                Return New Items.KeyItems.Squirtbottle()
            Case 176
                Return New Items.Standard.GripClaw()
            Case 177
                Return New Items.Balls.SportBall()
            Case 178
                Return New Items.KeyItems.RainbowWing()
            Case 179
                Return New Items.Standard.OvalStone()
            Case 180
                Return New Items.Standard.BrickPiece()
            Case 181
                Return New Items.Balls.SafariBall()
            Case 182
                Return New Items.Standard.BlackSludge()
            Case 183
                Return New Items.Standard.RazorFang()
            Case 184
                Return New Items.Standard.RazorClaw()
            Case 185
                Return New Items.Standard.DubiousDisc()
            Case 186
                Return New Items.Balls.HealBall()
            Case 187
                Return New Items.Standard.AbilityCapsule()
            Case 188
                Return New Items.Balls.NestBall()
            Case 189
                Return New Items.Standard.BigNugget()
            Case 190
                Return New Items.Standard.HeartScale()
            Case 191
                Return New Items.Machines.TM01()
            Case 192
                Return New Items.Machines.TM02()
            Case 193
                Return New Items.Machines.TM03()
            Case 194
                Return New Items.Machines.TM04()
            Case 195
                Return New Items.Machines.TM05()
            Case 196
                Return New Items.Machines.TM06()
            Case 197
                Return New Items.Machines.TM07()
            Case 198
                Return New Items.Machines.TM08()
            Case 199
                Return New Items.Machines.TM09()
            Case 200
                Return New Items.Machines.TM10()
            Case 201
                Return New Items.Machines.TM11()
            Case 202
                Return New Items.Machines.TM12()
            Case 203
                Return New Items.Machines.TM13()
            Case 204
                Return New Items.Machines.TM14()
            Case 205
                Return New Items.Machines.TM15()
            Case 206
                Return New Items.Machines.TM16()
            Case 207
                Return New Items.Machines.TM17()
            Case 208
                Return New Items.Machines.TM18()
            Case 209
                Return New Items.Machines.TM19()
            Case 210
                Return New Items.Machines.TM20()
            Case 211
                Return New Items.Machines.TM21()
            Case 212
                Return New Items.Machines.TM22()
            Case 213
                Return New Items.Machines.TM23()
            Case 214
                Return New Items.Machines.TM24()
            Case 215
                Return New Items.Machines.TM25()
            Case 216
                Return New Items.Machines.TM26()
            Case 217
                Return New Items.Machines.TM27()
            Case 218
                Return New Items.Machines.TM28()
            Case 219
                Return New Items.Machines.TM29()
            Case 220
                Return New Items.Machines.TM30()
            Case 221
                Return New Items.Machines.TM31()
            Case 222
                Return New Items.Machines.TM32()
            Case 223
                Return New Items.Machines.TM33()
            Case 224
                Return New Items.Machines.TM34()
            Case 225
                Return New Items.Machines.TM35()
            Case 226
                Return New Items.Machines.TM36()
            Case 227
                Return New Items.Machines.TM37()
            Case 228
                Return New Items.Machines.TM38()
            Case 229
                Return New Items.Machines.TM39()
            Case 230
                Return New Items.Machines.TM40()
            Case 231
                Return New Items.Machines.TM41()
            Case 232
                Return New Items.Machines.TM42()
            Case 233
                Return New Items.Machines.TM43()
            Case 234
                Return New Items.Machines.TM44()
            Case 235
                Return New Items.Machines.TM45()
            Case 236
                Return New Items.Machines.TM46()
            Case 237
                Return New Items.Machines.TM47()
            Case 238
                Return New Items.Machines.TM48()
            Case 239
                Return New Items.Machines.TM49()
            Case 240
                Return New Items.Machines.TM50()
            Case 241
                Return New Items.KeyItems.OvalCharm()
            Case 242
                Return New Items.KeyItems.ShinyCharm()
                'Case 243-246: Open
            Case 247
                Return New Items.Machines.HM01()
            Case 244
                Return New Items.Machines.HM02()
            Case 245
                Return New Items.Machines.HM03()
            Case 246
                Return New Items.Machines.HM04()
            Case 243
                Return New Items.Machines.HM05()
            Case 248
                Return New Items.Machines.HM06()
            Case 249
                Return New Items.Machines.HM07()
            Case 250
                Return New Items.Machines.HM08()
            Case 251
                Return New Items.Machines.HM09()
            Case 252
                Return New Items.Machines.HM10()
            Case 253
                Return New Items.Plants.Honey()
            Case 254
                Return New Items.Wings.HealthWing()
            Case 255
                Return New Items.Wings.MuscleWing()
            Case 256
                Return New Items.Wings.ResistWing()
            Case 257
                Return New Items.Wings.GeniusWing()
            Case 258
                Return New Items.Wings.CleverWing()
            Case 259
                Return New Items.Wings.SwiftWing()
            Case 260
                Return New Items.Wings.PrettyWing()
            Case 261
                Return New Items.Wings.StickyWing()
            Case 262
                Return New Items.Stones.StickyRock()
            Case 263
                Return New Items.Standard.OddIncense()
            Case 264
                Return New Items.Standard.SeaIncense()
            Case 265
                Return New Items.KeyItems.TriPass()
            Case 266
                Return New Items.Medicine.Fanta()
            Case 267
                Return New Items.Plates.DracoPlate()
            Case 268
                Return New Items.Plates.DreadPlate()
            Case 269
                Return New Items.Plates.EarthPlate()
            Case 270
                Return New Items.Plates.FistPlate()
            Case 271
                Return New Items.Plates.FlamePlate()
            Case 272
                Return New Items.Plates.IciclePlate()
            Case 273
                Return New Items.Plates.InsectPlate()
            Case 274
                Return New Items.Plates.IronPlate()
            Case 275
                Return New Items.Plates.MeadowPlate()
            Case 276
                Return New Items.Plates.MindPlate()
            Case 277
                Return New Items.Plates.PixiePlate()
            Case 278
                Return New Items.Plates.SkyPlate()
            Case 279
                Return New Items.Plates.SplashPlate()
            Case 280
                Return New Items.Plates.SpookyPlate()
            Case 281
                Return New Items.Plates.StonePlate()
            Case 282
                Return New Items.Plates.ToxicPlate()
            Case 283
                Return New Items.Plates.ZapPlate()
            Case 284
                Return New Items.KeyItems.RainbowPass()
            Case 285
                Return New Items.KeyItems.OldSeaMap()
                'Case 286 - 299: Reserved for upcoming items.

            Case 300
                Return New Items.Mail.GrassMail()
            Case 301
                Return New Items.Mail.BeadMail()
            Case 302
                Return New Items.Mail.DreamMail()
            Case 303
                Return New Items.Mail.FabMail()
            Case 304
                Return New Items.Mail.GlitterMail()
            Case 305
                Return New Items.Mail.HarborMail()
            Case 306
                Return New Items.Mail.MechMail()
            Case 307
                Return New Items.Mail.OrangeMail()
            Case 308
                Return New Items.Mail.RetroMail()
            Case 309
                Return New Items.Mail.ShadowMail()
            Case 310
                Return New Items.Mail.TropicMail()
            Case 311
                Return New Items.Mail.WaveMail()
            Case 312
                Return New Items.Mail.WoodMail()
            Case 313
                Return New Items.Mail.AirMail()
            Case 314
                Return New Items.Mail.BloomMail()
            Case 315
                Return New Items.Mail.BrickMail()
            Case 316
                Return New Items.Mail.BubbleMail()
            Case 317
                Return New Items.Mail.FlameMail()
            Case 318
                Return New Items.Mail.HeartMail()
            Case 319
                Return New Items.Mail.MosaicMail()
            Case 320
                Return New Items.Mail.SnowMail()
            Case 321
                Return New Items.Mail.SpaceMail()
            Case 322
                Return New Items.Mail.SteelMail()
            Case 323
                Return New Items.Mail.TunnelMail()
            Case 324
                Return New Items.Mail.BridgeMailT()
            Case 325
                Return New Items.Mail.BridgeMailD()
            Case 326
                Return New Items.Mail.BridgeMailS()
            Case 327
                Return New Items.Mail.BridgeMailV()
            Case 328
                Return New Items.Mail.BridgeMailM()
            Case 329
                Return New Items.Mail.FavoredMail()
            Case 330
                Return New Items.Mail.ThanksMail()
            Case 331
                Return New Items.Mail.InquiryMail()
            Case 332
                Return New Items.Mail.GreetMail()
            Case 333
                Return New Items.Mail.RSVPMail()
            Case 334
                Return New Items.Mail.LikeMail()
            Case 335
                Return New Items.Mail.ReplyMail()
            Case 336
                Return New Items.Mail.KolbenMail()
                'Case 337 - 350: Reserved for upcoming mail.
                'Case 351 - 500: Reserved for TMs 51 - 200.
            Case 351
                Return New Items.Machines.TM51()
            Case 352
                Return New Items.Machines.TM52()
            Case 353
                Return New Items.Machines.TM53()
            Case 354
                Return New Items.Machines.TM54()
            Case 355
                Return New Items.Machines.TM55()
            Case 356
                Return New Items.Machines.TM56()
            Case 357
                Return New Items.Machines.TM57()
            Case 358
                Return New Items.Machines.TM58()
            Case 359
                Return New Items.Machines.TM59()
            Case 360
                Return New Items.Machines.TM60()
            Case 361
                Return New Items.Machines.TM61()
            Case 362
                Return New Items.Machines.TM62()
            Case 363
                Return New Items.Machines.TM63()
            Case 364
                Return New Items.Machines.TM64()
            Case 365
                Return New Items.Machines.TM65()
            Case 366
                Return New Items.Machines.TM66()
            Case 367
                Return New Items.Machines.TM67()
            Case 368
                Return New Items.Machines.TM68()
            Case 369
                Return New Items.Machines.TM69()
            Case 370
                Return New Items.Machines.TM70()
            Case 371
                Return New Items.Machines.TM71()
            Case 372
                Return New Items.Machines.TM72()
            Case 373
                Return New Items.Machines.TM73()
            Case 374
                Return New Items.Machines.TM74()
            Case 375
                Return New Items.Machines.TM75()
            Case 376
                Return New Items.Machines.TM76()
            Case 377
                Return New Items.Machines.TM77()
            Case 378
                Return New Items.Machines.TM78()
            Case 379
                Return New Items.Machines.TM79()
            Case 380
                Return New Items.Machines.TM80()
            Case 381
                Return New Items.Machines.TM81()
            Case 382
                Return New Items.Machines.TM82()
            Case 383
                Return New Items.Machines.TM83()
            Case 384
                Return New Items.Machines.TM84()
            Case 385
                Return New Items.Machines.TM85()
            Case 386
                Return New Items.Machines.TM86()
            Case 387
                Return New Items.Machines.TM87()
            Case 388
                Return New Items.Machines.TM88()
            Case 389
                Return New Items.Machines.TM89()
            Case 390
                Return New Items.Machines.TM90()
            Case 391
                Return New Items.Machines.TM91()
            Case 392
                Return New Items.Machines.TM92()
            Case 393
                Return New Items.Machines.TM93()
            Case 394
                Return New Items.Machines.TM94()
            Case 395
                Return New Items.Machines.TM95()
            Case 396
                Return New Items.Machines.TM96()
            Case 397
                Return New Items.Machines.TM97()
            Case 398
                Return New Items.Machines.TM98()
            Case 399
                Return New Items.Machines.TM99()
            Case 400
                Return New Items.Machines.TM100()
            Case 401
                Return New Items.Machines.TM101()
            Case 402
                Return New Items.Machines.TM102()
            Case 403
                Return New Items.Machines.TM103()
            Case 404
                Return New Items.Machines.TM104()
            Case 405
                Return New Items.Machines.TM105()
            Case 406
                Return New Items.Machines.TM106()
            Case 407
                Return New Items.Machines.TM107()
            Case 408
                Return New Items.Machines.TM108()
            Case 409
                Return New Items.Machines.TM109()
            Case 410
                Return New Items.Machines.TM110()
            Case 411
                Return New Items.Machines.TM111()
            Case 412
                Return New Items.Machines.TM112()
            Case 413
                Return New Items.Machines.TM113()
            Case 414
                Return New Items.Machines.TM114()
            Case 415
                Return New Items.Machines.TM115()
            Case 416
                Return New Items.Machines.TM116()
            Case 417
                Return New Items.Machines.TM117()
            Case 418
                Return New Items.Machines.TM118()
            Case 419
                Return New Items.Machines.TM119()
            Case 420
                Return New Items.Machines.TM120()
            Case 421
                Return New Items.Machines.TM121()
            Case 422
                Return New Items.Machines.TM122()
            Case 423
                Return New Items.Machines.TM123()
            Case 424
                Return New Items.Machines.TM124()
            Case 425
                Return New Items.Machines.TM125()
            Case 426
                Return New Items.Machines.TM126()
            Case 427
                Return New Items.Machines.TM127()
            Case 428
                Return New Items.Machines.TM128()
            Case 429
                Return New Items.Machines.TM129()
            Case 430
                Return New Items.Machines.TM130()
            Case 431
                Return New Items.Machines.TM131()
            Case 432
                Return New Items.Machines.TM132()
            Case 433
                Return New Items.Machines.TM133()
            Case 434
                Return New Items.Machines.TM134()
            Case 435
                Return New Items.Machines.TM135()
            Case 436
                Return New Items.Machines.TM136()
            Case 437
                Return New Items.Machines.TM137()
            Case 438
                Return New Items.Machines.TM138()
            Case 439
                Return New Items.Machines.TM139()
            Case 440
                Return New Items.Machines.TM140()
            Case 441
                Return New Items.Machines.TM141()
            Case 442
                Return New Items.Machines.TM142()
            Case 443
                Return New Items.Machines.TM143()
            Case 444
                Return New Items.Machines.TM144()
            Case 445
                Return New Items.Machines.TM145()
            Case 446
                Return New Items.Machines.TM146()
            Case 447
                Return New Items.Machines.TM147()
            Case 448
                Return New Items.Machines.TM148()
            Case 449
                Return New Items.Machines.TM149()
            Case 450
                Return New Items.Machines.TM150()
            Case 451
                Return New Items.Machines.TM151()
            Case 452
                Return New Items.Machines.TM152()
            Case 453
                Return New Items.Machines.TM153()
            Case 454
                Return New Items.Machines.TM154()
            Case 455
                Return New Items.Machines.TM155()
            Case 456
                Return New Items.Machines.TM156()
            Case 457
                Return New Items.Machines.TM157()
            Case 458
                Return New Items.Machines.TM158()
            Case 459
                Return New Items.Machines.TM159()
            Case 460
                Return New Items.Machines.TM160()
            Case 461
                Return New Items.Machines.TM161()
            Case 462
                Return New Items.Machines.TM162()
            Case 463
                Return New Items.Machines.TM163()
            Case 464
                Return New Items.Machines.TM164()
            Case 465
                Return New Items.Machines.TM165()
            Case 466
                Return New Items.Machines.TM166()
            Case 467
                Return New Items.Machines.TM167()
            Case 468
                Return New Items.Machines.TM168()
            Case 469
                Return New Items.Machines.TM169()
            Case 470
                Return New Items.Machines.TM170()

                'Case 501
                '    Return New Items.Medicine.ShinyCandy() 'Don't return a shiny candy here, it's returning a rare candy additionally to its original ID (32).
            Case 502
                Return New Items.Medicine.PPMax()
            Case 503
                Return New Items.Standard.Sachet()
            Case 504
                Return New Items.Standard.WhippedDream()
            Case 505
                Return New Items.Standard.ToxicOrb()
            Case 506
                Return New Items.Standard.LifeOrb()
            Case 507
                Return New Items.MegaStones.Abomasite()
            Case 508
                Return New Items.MegaStones.Absolite()
            Case 509
                Return New Items.MegaStones.Aerodactylite()
            Case 510
                Return New Items.MegaStones.Aggronite()
            Case 511
                Return New Items.MegaStones.Alakazite()
            Case 512
                Return New Items.MegaStones.Ampharosite()
            Case 513
                Return New Items.MegaStones.Banettite()
            Case 514
                Return New Items.MegaStones.Blastoisinite()
            Case 515
                Return New Items.MegaStones.Blazikenite()
            Case 516
                Return New Items.MegaStones.CharizarditeX()
            Case 517
                Return New Items.MegaStones.CharizarditeY()
            Case 518
                Return New Items.MegaStones.Garchompite()
            Case 519
                Return New Items.MegaStones.Gardevoirite()
            Case 520
                Return New Items.MegaStones.Gengarite()
            Case 521
                Return New Items.MegaStones.Gyaradosite()
            Case 522
                Return New Items.MegaStones.Heracronite()
            Case 523
                Return New Items.MegaStones.Houndoominite()
            Case 524
                Return New Items.MegaStones.Kangaskhanite()
            Case 525
                Return New Items.MegaStones.Lucarionite()
            Case 526
                Return New Items.MegaStones.Manectite()
            Case 527
                Return New Items.MegaStones.Mawilite()
            Case 528
                Return New Items.MegaStones.Medichamite()
            Case 529
                Return New Items.MegaStones.MewtwoniteX()
            Case 530
                Return New Items.MegaStones.MewtwoniteY()
            Case 531
                Return New Items.MegaStones.Pinsirite()
            Case 532
                Return New Items.MegaStones.Scizorite()
            Case 533
                Return New Items.MegaStones.Tyranitarite()
            Case 534
                Return New Items.MegaStones.Venusaurite()
            Case 535
                Return New Items.MegaStones.Altarianite()
            Case 536
                Return New Items.MegaStones.Audinite()
            Case 537
                Return New Items.MegaStones.Beedrillite()
            Case 538
                Return New Items.MegaStones.Cameruptite()
            Case 539
                Return New Items.MegaStones.Diancite()
            Case 540
                Return New Items.MegaStones.Galladite()
            Case 541
                Return New Items.MegaStones.Glalitite()
            Case 542
                Return New Items.MegaStones.Latiasite()
            Case 543
                Return New Items.MegaStones.Latiosite()
            Case 544
                Return New Items.MegaStones.Lopunnite()
            Case 545
                Return New Items.MegaStones.Metagrossite()
            Case 546
                Return New Items.MegaStones.Pidgeotite()
            Case 547
                Return New Items.MegaStones.Sablenite()
            Case 548
                Return New Items.MegaStones.Salamencite()
            Case 549
                Return New Items.MegaStones.Sceptilite()
            Case 550
                Return New Items.MegaStones.Sharpedonite()
            Case 551
                Return New Items.MegaStones.Slowbronite()
            Case 552
                Return New Items.MegaStones.Steelixite()
            Case 553
                Return New Items.MegaStones.Swampertite()
                'Case 554 - 575 Future Mega Stones

                'Case 576 - 1999: Reserved for upcoming items.

            Case 2000
                Return New Items.Berries.CheriBerry()
            Case 2001
                Return New Items.Berries.ChestoBerry()
            Case 2002
                Return New Items.Berries.PechaBerry()
            Case 2003
                Return New Items.Berries.RawstBerry()
            Case 2004
                Return New Items.Berries.AspearBerry()
            Case 2005
                Return New Items.Berries.LeppaBerry()
            Case 2006
                Return New Items.Berries.OranBerry()
            Case 2007
                Return New Items.Berries.PersimBerry()
            Case 2008
                Return New Items.Berries.LumBerry()
            Case 2009
                Return New Items.Berries.SitrusBerry()
            Case 2010
                Return New Items.Berries.FigyBerry()
            Case 2011
                Return New Items.Berries.WikiBerry()
            Case 2012
                Return New Items.Berries.MagoBerry()
            Case 2013
                Return New Items.Berries.AguavBerry()
            Case 2014
                Return New Items.Berries.IapapaBerry()
            Case 2015
                Return New Items.Berries.RazzBerry()
            Case 2016
                Return New Items.Berries.BlukBerry()
            Case 2017
                Return New Items.Berries.NanabBerry()
            Case 2018
                Return New Items.Berries.WepearBerry()
            Case 2019
                Return New Items.Berries.PinapBerry()
            Case 2020
                Return New Items.Berries.PomegBerry()
            Case 2021
                Return New Items.Berries.KelpsyBerry()
            Case 2022
                Return New Items.Berries.QualotBerry()
            Case 2023
                Return New Items.Berries.HondewBerry()
            Case 2024
                Return New Items.Berries.GrepaBerry()
            Case 2025
                Return New Items.Berries.TamatoBerry()
            Case 2026
                Return New Items.Berries.CornnBerry()
            Case 2027
                Return New Items.Berries.MagostBerry()
            Case 2028
                Return New Items.Berries.RabutaBerry()
            Case 2029
                Return New Items.Berries.NomelBerry()
            Case 2030
                Return New Items.Berries.SpelonBerry()
            Case 2031
                Return New Items.Berries.PamtreBerry()
            Case 2032
                Return New Items.Berries.WatmelBerry()
            Case 2033
                Return New Items.Berries.DurinBerry()
            Case 2034
                Return New Items.Berries.BelueBerry()
            Case 2035
                Return New Items.Berries.OccaBerry()
            Case 2036
                Return New Items.Berries.PasshoBerry()
            Case 2037
                Return New Items.Berries.WacanBerry()
            Case 2038
                Return New Items.Berries.RindoBerry()
            Case 2039
                Return New Items.Berries.YacheBerry()
            Case 2040
                Return New Items.Berries.ChopleBerry()
            Case 2041
                Return New Items.Berries.KebiaBerry()
            Case 2042
                Return New Items.Berries.ShucaBerry()
            Case 2043
                Return New Items.Berries.CobaBerry()
            Case 2044
                Return New Items.Berries.PayapaBerry()
            Case 2045
                Return New Items.Berries.TangaBerry()
            Case 2046
                Return New Items.Berries.ChartiBerry()
            Case 2047
                Return New Items.Berries.KasibBerry()
            Case 2048
                Return New Items.Berries.HabanBerry()
            Case 2049
                Return New Items.Berries.ColburBerry()
            Case 2050
                Return New Items.Berries.BabiriBerry()
            Case 2051
                Return New Items.Berries.ChilanBerry()
            Case 2052
                Return New Items.Berries.LiechiBerry()
            Case 2053
                Return New Items.Berries.GanlonBerry()
            Case 2054
                Return New Items.Berries.SalacBerry()
            Case 2055
                Return New Items.Berries.PetayaBerry()
            Case 2056
                Return New Items.Berries.ApicotBerry()
            Case 2057
                Return New Items.Berries.LansatBerry()
            Case 2058
                Return New Items.Berries.StarfBerry()
            Case 2059
                Return New Items.Berries.EnigmaBerry()
            Case 2060
                Return New Items.Berries.MicleBerry()
            Case 2061
                Return New Items.Berries.CustapBerry()
            Case 2062
                Return New Items.Berries.JabocaBerry()
            Case 2063
                Return New Items.Berries.RowapBerry()
            Case Else
                'If no ID matches, return Nothing.
                Return Nothing
        End Select
    End Function

    'A cache that stores Names and IDs of items.
    Shared ItemNameID As New Dictionary(Of String, Integer)

    ''' <summary>
    ''' Returns an item based on its name.
    ''' </summary>
    ''' <param name="name">The name of the item.</param>
    ''' <remarks>This method is not as performant on initial use as the GetItemByID method.</remarks>
    Public Shared Function GetItemByName(ByVal name As String) As Item
        'Check if the name is available in the FileItem list.
        For Each FileItem As Items.FileItem In Core.Player.FileItems
            If FileItem.Item.Name.ToLower() = name.ToLower() Then
                Return FileItem.Item
            End If
        Next

        Dim maxID As Integer = 2063 'Set to max ID of all items.
        If ItemNameID.Keys.Contains(name.ToLower()) = True Then
            Return Item.GetItemByID(ItemNameID(name.ToLower()))
        Else
            For i = 0 To maxID
                If ItemNameID.Values.Contains(i) = False Then
                    Dim tItem As Item = GetItemByID(i)
                    If tItem IsNot Nothing Then
                        If ItemNameID.Keys.Contains(tItem.Name.ToLower()) = False Then
                            ItemNameID.Add(tItem.Name.ToLower(), i)
                        End If
                        If tItem.Name.ToLower() = name.ToLower() Then
                            Return tItem
                        End If
                    End If
                End If
            Next
        End If
        Logger.Log(Logger.LogTypes.Warning, "Item.vb: Cannot find item with the name """ & name & """.")
        Return Nothing
    End Function

    ''' <summary>
    ''' Creates a new instance of the Item instance based on its ID.
    ''' </summary>
    Public Function Copy() As Object Implements ICopyAble.Copy
        Return Item.GetItemByID(Me.ID)
    End Function

End Class
