Public Class PokemonInteractions

    'Procedure:
    '1. If has a status condition, return that
    '1.1 If a Pokémon has low HP, return reaction (40%)
    '2. If the Pokémon holds a pickup item, give that (if friendship is high enough, else say 50% that it doesn't want to give item, 50% proceed)
    '3. Return Special Location Text (found in Content\Data\interactions.dat) (60%)
    '4. Friendship based/random text

    'Friendship levels:
    '1: <= 50 (hate)
    '2: >50 <= 150 (neutral)
    '3: >150 <= 200 (like)
    '4: >200 <=250 (loyal)
    '5: >250 (love)

    Public Enum FriendshipLevels
        Hate
        Neutral
        Likes
        Loyal
        Love
    End Enum

    Public Shared Function GetScriptString(ByVal p As Pokemon, ByVal cPosition As Vector3, ByVal facing As Integer) As String
        If PickupItemID > -1 Then
            If PickupIndividualValue = p.IndividualValue Then
                Return GenerateItemReaction(p, cPosition, facing)
            Else
                PickupItemID = -1
                PickupIndividualValue = ""
            End If
        End If

        Dim reaction As ReactionContainer = GetReaction(p)

        Dim offset As New Vector2(0, 0)
        Select Case facing
            Case 0
                offset.Y = -0.02F
            Case 1
                offset.X = -0.02F
            Case 2
                offset.Y = 0.01F
            Case 3
                offset.X = 0.01F
        End Select

        Dim newPosition As New Vector2(0, 1)

        Dim s As String = "version=2" & vbNewLine &
            "@pokemon.cry(" & p.Number & ")" & vbNewLine

        If CType(Screen.Camera, OverworldCamera).ThirdPerson = False Then
            If reaction.HasNotification = True Then
                s &= "@camera.activatethirdperson" & vbNewLine &
                     "@camera.setposition(" & newPosition.X & ",1," & newPosition.Y & ")" & vbNewLine

                s &= "@entity.showmessagebulb(" & CInt(reaction.Notification).ToString() & "|" & cPosition.X + offset.X & "|" & cPosition.Y + 0.7F & "|" & cPosition.Z + offset.Y & ")" & vbNewLine

                s &= "@camera.deactivatethirdperson" & vbNewLine
            End If
            s &= "@text.show(" & reaction.GetMessage(p) & ")" & vbNewLine
        Else
            Dim preYaw As Single = Screen.Camera.Yaw
            If reaction.HasNotification = True Then
                s &= "@camera.setyaw(" & CType(Screen.Camera, OverworldCamera).GetAimYawFromDirection(Screen.Camera.GetPlayerFacingDirection()) & ")" & vbNewLine
                s &= "@camera.setposition(" & newPosition.X & ",1," & newPosition.Y & ")" & vbNewLine
                s &= "@entity.showmessagebulb(" & CInt(reaction.Notification).ToString() & "|" & cPosition.X + offset.X & "|" & cPosition.Y + 0.7F & "|" & cPosition.Z + offset.Y & ")" & vbNewLine

                s &= "@camera.deactivatethirdperson" & vbNewLine
            End If
            s &= "@text.show(" & reaction.GetMessage(p) & ")" & vbNewLine
            s &= "@camera.activatethirdperson" & vbNewLine
            s &= "@camera.setyaw(" & preYaw & ")" & vbNewLine
        End If
        s &= ":end"

        Return s
    End Function

    Private Shared Function GenerateItemReaction(ByVal p As Pokemon, ByVal cPosition As Vector3, ByVal facing As Integer) As String
        Dim message As String = "It looks like your Pokémon~holds on to something.*Do you want to~take it?"

        Dim offset As New Vector2(0, 0)
        Select Case facing
            Case 0
                offset.Y = -0.02F
            Case 1
                offset.X = -0.02F
            Case 2
                offset.Y = 0.01F
            Case 3
                offset.X = 0.01F
        End Select

        Dim newPosition As New Vector2(0, 1)

        Dim item As Item = Item.GetItemByID(PickupItemID)

        Dim s As String = "version=2" & vbNewLine &
           "@pokemon.cry(" & p.Number & ")" & vbNewLine

        If CType(Screen.Camera, OverworldCamera).ThirdPerson = False Then
            s &= "@camera.activatethirdperson" & vbNewLine &
                 "@camera.setposition(" & newPosition.X & ",1," & newPosition.Y & ")" & vbNewLine

            s &= "@entity.showmessagebulb(" & CInt(MessageBulb.NotifcationTypes.Question).ToString() & "|" & cPosition.X + offset.X & "|" & cPosition.Y + 0.7F & "|" & cPosition.Z + offset.Y & ")" & vbNewLine

            s &= "@camera.deactivatethirdperson" & vbNewLine
            s &= "@text.show(" & message & ")" & vbNewLine &
                "@options.show(Yes,No)" & vbNewLine &
                ":when:Yes" & vbNewLine &
                "@text.show(Your Pokémon handed over~the " & item.Name & "!)" & vbNewLine &
                "@item.give(" & PickupItemID & ",1)" & vbNewLine &
                "@item.messagegive(" & PickupItemID & ",1)" & vbNewLine &
                ":when:No" & vbNewLine &
                "@text.show(Your Pokémon kept~the item happily.)" & vbNewLine &
                "@pokemon.addfriendship(0,10)" & vbNewLine &
                ":endwhen" & vbNewLine
        Else
            s &= "@camera.setposition(" & newPosition.X & ",1," & newPosition.Y & ")" & vbNewLine
            s &= "@entity.showmessagebulb(" & CInt(MessageBulb.NotifcationTypes.Question).ToString() & "|" & cPosition.X + offset.X & "|" & cPosition.Y + 0.7F & "|" & cPosition.Z + offset.Y & ")" & vbNewLine

            s &= "@camera.deactivatethirdperson" & vbNewLine

            s &= "@text.show(" & message & ")" & vbNewLine &
                "@options.show(Yes,No)" & vbNewLine &
                ":when:Yes" & vbNewLine &
                "@text.show(Your Pokémon handed over~the " & item.Name & "!)" & vbNewLine &
                "@item.give(" & PickupItemID & ",1)" & vbNewLine &
                "@item.messagegive(" & PickupItemID & ",1)" & vbNewLine &
                ":when:No" & vbNewLine &
                "@text.show(Your Pokémon kept~the item happily.)" & vbNewLine &
                "@pokemon.addfriendship(0,10)" & vbNewLine &
                ":endwhen" & vbNewLine
            s &= "@camera.activatethirdperson" & vbNewLine
        End If
        s &= ":end"

        PickupItemID = -1
        PickupIndividualValue = ""

        Return s
    End Function

    Private Shared Function GetReaction(ByVal p As Pokemon) As ReactionContainer
        Dim FriendshipLevel As FriendshipLevels = GetFriendshipLevel(p)

        Dim reaction As ReactionContainer = Nothing

        If p.Status <> Pokemon.StatusProblems.None Then
            'Return status condition text
            reaction = GetStatusConditionReaction(p)
        End If

        If reaction Is Nothing And ((p.HP / p.MaxHP) * 100) <= 15 Then
            If Core.Random.Next(0, 100) < 40 Then
                reaction = GetLowHPReaction(p)
            End If
        End If

        'Get special place reaction  (60%)
        If reaction Is Nothing Then
            If Core.Random.Next(0, 100) < 60 Then
                reaction = GetSpecialReaction(p)
            End If
        End If

        'Friendship based:
        'If friendship level is hate, return hate.
        'If friendship level is above hate, never return hate.
        'If friendship level is neutral, only return neutral.
        'If friendship level is like, return 60% like, 40% neutral
        'If friendship level is loyal, return 60% loyal, 15% like, 25% neutral
        'If friendship level is love, return 55% love, 10% loyal, 10% like, 25% neutral
        If reaction Is Nothing Then
            Dim r As Integer = Core.Random.Next(0, 100)
            Select Case FriendshipLevel
                Case FriendshipLevels.Hate
                    reaction = GetHateReaction(p)
                Case FriendshipLevels.Neutral
                    reaction = GetNeutralReaction(p)
                Case FriendshipLevels.Likes
                    If r < 60 Then
                        reaction = GetLikeReaction(p)
                    Else
                        reaction = GetNeutralReaction(p)
                    End If
                Case FriendshipLevels.Loyal
                    If r < 60 Then
                        reaction = GetLoyalReaction(p)
                    ElseIf r >= 60 And r < 75 Then
                        reaction = GetLikeReaction(p)
                    Else
                        reaction = GetNeutralReaction(p)
                    End If
                Case FriendshipLevels.Love
                    If r < 55 Then
                        reaction = GetLoveReaction(p)
                    ElseIf r >= 55 And r < 65 Then
                        reaction = GetLoyalReaction(p)
                    ElseIf r >= 65 And r < 75 Then
                        reaction = GetLikeReaction(p)
                    Else
                        reaction = GetNeutralReaction(p)
                    End If
            End Select
        End If

        Return reaction
    End Function

    Private Shared Function GetFriendshipLevel(ByVal p As Pokemon) As FriendshipLevels
        Dim f As Integer = p.Friendship
        If f <= 50 Then
            Return FriendshipLevels.Hate
        ElseIf f > 50 And f <= 120 Then
            Return FriendshipLevels.Neutral
        ElseIf f > 120 And f <= 200 Then
            Return FriendshipLevels.Likes
        ElseIf f > 200 And f <= 245 Then
            Return FriendshipLevels.Loyal
        Else
            Return FriendshipLevels.Love
        End If
    End Function

    Private Shared Function GetStatusConditionReaction(ByVal p As Pokemon) As ReactionContainer
        Select Case p.Status
            Case Pokemon.StatusProblems.BadPoison, Pokemon.StatusProblems.Poison
                Return New ReactionContainer("<name> is shivering~with the effects of being~poisoned.", MessageBulb.NotifcationTypes.Poisoned)
            Case Pokemon.StatusProblems.Burn
                Return New ReactionContainer("<name>'s burn~looks painful!", MessageBulb.NotifcationTypes.Poisoned)
            Case Pokemon.StatusProblems.Freeze
                Select Case Core.Random.Next(0, 2)
                    Case 0
                        Return New ReactionContainer("<name> seems very cold!", MessageBulb.NotifcationTypes.Poisoned)
                    Case 1
                        Return New ReactionContainer(".....Your Pokémon seems~a little cold.", MessageBulb.NotifcationTypes.Poisoned)
                End Select
            Case Pokemon.StatusProblems.Paralyzed
                Return New ReactionContainer("<name> is trying~very hard to keep~up with you...", MessageBulb.NotifcationTypes.Poisoned)
            Case Pokemon.StatusProblems.Sleep
                Select Case Core.Random.Next(0, 3)
                    Case 0
                        Return New ReactionContainer("<name> seems~a little tiered.", MessageBulb.NotifcationTypes.Poisoned)
                    Case 1
                        Return New ReactionContainer("<name> is somehow~fighting off sleep...", MessageBulb.NotifcationTypes.Poisoned)
                    Case 2
                        Return New ReactionContainer("<name> yawned~very loudly!", MessageBulb.NotifcationTypes.Poisoned)
                End Select
        End Select
        Return New ReactionContainer("<name> is trying~very hard to keep~up with you...", MessageBulb.NotifcationTypes.Poisoned)
    End Function

    Private Shared Function GetLowHPReaction(ByVal p As Pokemon) As ReactionContainer
        Select Case Core.Random.Next(0, 2)
            Case 0
                Return New ReactionContainer("<name> is going~to fall down!", MessageBulb.NotifcationTypes.Exclamation)
            Case 1
                Return New ReactionContainer("<name> seems to~be about to fall over!", MessageBulb.NotifcationTypes.Exclamation)
        End Select
        Return New ReactionContainer("<name> seems to~be about to fall over!", MessageBulb.NotifcationTypes.Exclamation)
    End Function

    Private Shared Function GetSpecialReaction(ByVal p As Pokemon) As ReactionContainer
        Dim matching As New List(Of ReactionContainer)

        For Each spReaction As ReactionContainer In SpecialReactionList
            If spReaction.Match(p) = True Then
                matching.Add(spReaction)
            End If
        Next

        If matching.Count > 0 Then
            Dim chances As New List(Of Integer)
            For Each r In matching
                chances.Add(r.Probability)
            Next

            Dim index As Integer = GetRandomChance(chances)

            Return matching(index)
        End If

        Return Nothing
    End Function

    Private Shared Function GetHateReaction(ByVal p As Pokemon) As ReactionContainer
        Dim r As ReactionContainer = Nothing
        While r Is Nothing
            Select Case Core.Random.Next(0, 17)
                Case 0
                    r = New ReactionContainer("<name> is doing~it's best to keep up~with you.", MessageBulb.NotifcationTypes.Unhappy)
                Case 1
                    r = New ReactionContainer("<name> is somehow~forcing itself to keep going.", MessageBulb.NotifcationTypes.Unsure)
                Case 2
                    r = New ReactionContainer("<name> is staring~patiantly at nothing at all.", MessageBulb.NotifcationTypes.Unsure)
                Case 3
                    r = New ReactionContainer("<name> is staring~intently into the distance.", MessageBulb.NotifcationTypes.Waiting)
                Case 4
                    r = New ReactionContainer("<name> is dizzy...", MessageBulb.NotifcationTypes.Unhappy)
                Case 5
                    r = New ReactionContainer("<name> is stepping~on your feet!", MessageBulb.NotifcationTypes.Waiting)
                Case 6
                    r = New ReactionContainer("<name> seems~unhappy somehow...", MessageBulb.NotifcationTypes.Unhappy)
                Case 7
                    r = New ReactionContainer("<name> is making~an unhappy face.", MessageBulb.NotifcationTypes.Unhappy)
                Case 8
                    r = New ReactionContainer("<name> seems~uneasy and is poking~<player.name>.", MessageBulb.NotifcationTypes.Unsure)
                Case 9
                    r = New ReactionContainer("<name> is making~a face like its angry!", MessageBulb.NotifcationTypes.Angry)
                Case 10
                    r = New ReactionContainer("<name> seems to be~angry for some reason.", MessageBulb.NotifcationTypes.Angry)
                Case 11
                    r = New ReactionContainer("Your Pokémon turned to face~the other way, showing a~defiant expression. ", MessageBulb.NotifcationTypes.Unsure)
                Case 12
                    r = New ReactionContainer("<name> is looking~down steadily...", MessageBulb.NotifcationTypes.Waiting)
                Case 13
                    r = New ReactionContainer("Your Pokémon is staring~intently at nothing...", MessageBulb.NotifcationTypes.Waiting)
                Case 14
                    r = New ReactionContainer("Your Pokémon turned to~face the other way,~showing a defiant expression.", MessageBulb.NotifcationTypes.Unhappy)
                Case 15
                    r = New ReactionContainer("<name> seems~a bit nervous...", MessageBulb.NotifcationTypes.Unsure)
                Case 16
                    r = New ReactionContainer("Your Pokémon stumbled~and nearly fell!", MessageBulb.NotifcationTypes.Exclamation)
                Case 17
                    r = New ReactionContainer("<name> is having~a hard time keeping up.", MessageBulb.NotifcationTypes.Unsure)
            End Select
        End While

        Return r
    End Function

    Private Shared Function GetNeutralReaction(ByVal p As Pokemon) As ReactionContainer
        Dim r As ReactionContainer = Nothing
        While r Is Nothing
            Select Case Core.Random.Next(0, 53)
                Case 0
                    r = New ReactionContainer("<name> is happy~but shy.", MessageBulb.NotifcationTypes.Friendly)
                Case 1
                    r = New ReactionContainer("<name> puts in~extra effort.", MessageBulb.NotifcationTypes.Friendly)
                Case 2
                    If IsOutside() = True Then
                        r = New ReactionContainer("<name> is smelling~the scents of the~surrounding air", MessageBulb.NotifcationTypes.Friendly)
                    End If
                Case 3
                    If IsOutside() = True Then
                        r = New ReactionContainer("Your Pokémon has caught~the scent of smoke.", MessageBulb.NotifcationTypes.Friendly)
                    End If
                Case 4
                    If NPCAround() = True Then
                        r = New ReactionContainer("<name> greeted everyone!", MessageBulb.NotifcationTypes.CatFace)
                    End If
                Case 5
                    If IsOutside() = True Then
                        r = New ReactionContainer("<name> is wandering~around and listening~to the different sounds.", MessageBulb.NotifcationTypes.Note)
                    End If
                Case 6
                    r = New ReactionContainer("<name> looks very~interested!", MessageBulb.NotifcationTypes.Friendly)
                Case 7
                    If IsOutside() = True Then
                        r = New ReactionContainer("<name> is steadily~poking at the ground.", MessageBulb.NotifcationTypes.Waiting)
                    End If
                Case 8
                    r = New ReactionContainer("Your Pokémon is looking~around restlessly.", MessageBulb.NotifcationTypes.Note)
                Case 9
                    If IsOutside() = True Then
                        r = New ReactionContainer("<name> seems dazzled~after seeing the sky.", MessageBulb.NotifcationTypes.Waiting)
                    End If
                Case 10
                    r = New ReactionContainer("<name> is gazing~around restlessly!", MessageBulb.NotifcationTypes.Waiting)
                Case 11
                    If TrainerAround() = True Then
                        r = New ReactionContainer("<name> let out~a battle cry!", MessageBulb.NotifcationTypes.Shouting)
                    End If
                Case 12
                    If TrainerAround() = True And p.IsType(Element.Types.Fire) = True Then
                        r = New ReactionContainer("<name> is vigorously~breathing fire!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 13
                    If TrainerAround() = True Or NPCAround() = True Then
                        r = New ReactionContainer("<name> is on~the lookout!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 14
                    If TrainerAround() = True Then
                        r = New ReactionContainer("<name> roared!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 15
                    If TrainerAround() = True Then
                        r = New ReactionContainer("<name> let out a roar!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 16
                    r = New ReactionContainer("<name> is surveying~the area...", MessageBulb.NotifcationTypes.Waiting)
                Case 17
                    If IsInside() = True Then
                        r = New ReactionContainer("<name> is sniffing~at the floor.", MessageBulb.NotifcationTypes.Question)
                    End If
                Case 18
                    r = New ReactionContainer("<name> is peering~down.", MessageBulb.NotifcationTypes.Question)
                Case 19
                    r = New ReactionContainer("<name> seems~to be wandering around.", MessageBulb.NotifcationTypes.Note)
                Case 20
                    r = New ReactionContainer("<name> is looking~around absentmindedly.", MessageBulb.NotifcationTypes.Waiting)
                Case 21
                    r = New ReactionContainer("<name> is relaxing~comfortably.", MessageBulb.NotifcationTypes.Friendly)
                Case 22
                    If IsInside() = True Then
                        r = New ReactionContainer("<name> is sniffing~at the floor.", MessageBulb.NotifcationTypes.Waiting)
                    End If
                Case 23
                    If IsOutside() = True Then
                        r = New ReactionContainer("<name> seems to~relax as it hears the~sound of rustling leaves...", MessageBulb.NotifcationTypes.Friendly)
                    End If
                Case 24
                    If IsOutside() = True Then
                        r = New ReactionContainer("<name> seems to~be listening to the~sound of rustling leaves...", MessageBulb.NotifcationTypes.Friendly)
                    End If
                Case 25
                    If WaterAround() = True Then
                        r = New ReactionContainer("Your Pokémon is playing around~and splashing in the water!", MessageBulb.NotifcationTypes.Happy)
                    End If
                Case 26
                    If IsOutside() = True Then
                        r = New ReactionContainer("<name> is looking~up at the sky.", MessageBulb.NotifcationTypes.Waiting)
                    End If
                Case 27
                    If IsOutside() = True And World.GetTime() = World.DayTime.Night And World.GetCurrentRegionWeather() = World.Weathers.Clear Then
                        r = New ReactionContainer("Your Pokémon is happily~gazing at the beautiful,~starry sky!", MessageBulb.NotifcationTypes.Waiting)
                    End If
                Case 28
                    r = New ReactionContainer("<name> seems to be~enjoying this a little bit!", MessageBulb.NotifcationTypes.Note)
                Case 29
                    If IsInside() = True Then
                        r = New ReactionContainer("<name> is looking~up at the ceiling.", MessageBulb.NotifcationTypes.Note)
                    End If
                Case 30
                    If IsOutside() = True And World.GetTime() = World.DayTime.Night Then
                        r = New ReactionContainer("Your Pokémon is staring~spellbound at the night sky!", MessageBulb.NotifcationTypes.Friendly)
                    End If
                Case 31
                    r = New ReactionContainer("<name> is in~danger of falling over!", MessageBulb.NotifcationTypes.Exclamation)
                Case 32
                    If p.NickName <> "" Then
                        r = New ReactionContainer("<name> doesn't~seem to be used to its~own name yet.", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 33
                    If IsInside() = True Then
                        r = New ReactionContainer("<name> slipped~on the floor and seems~likely to fall!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 34
                    If TrainerAround() = True Or GrassAround() = True Then
                        r = New ReactionContainer("<name> feels something~and is howling!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 35
                    If p.HP = p.MaxHP And p.Status = Pokemon.StatusProblems.None Then
                        r = New ReactionContainer("<name> seems~refreshed!", MessageBulb.NotifcationTypes.Friendly)
                    End If
                Case 36
                    If p.HP = p.MaxHP And p.Status = Pokemon.StatusProblems.None Then
                        r = New ReactionContainer("<name> feels~refreshed.", MessageBulb.NotifcationTypes.Friendly)
                    End If
                Case 37
                    If ItemAround() = True Then
                        r = New ReactionContainer("<name> seems to~have found something!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 38
                    If TrainerAround() = True Or GrassAround() = True Then
                        r = New ReactionContainer("<name> suddenly~turned around and~started barking!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 39
                    If TrainerAround() = True Or GrassAround() = True Then
                        r = New ReactionContainer("<name> suddenly~turned around!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 40
                    If IsOutside() = True Then
                        r = New ReactionContainer("<name> looked up~at the sky and shouted loudly!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 41
                    r = New ReactionContainer("Your Pokémon was surprised~that you suddenly spoke to it!", MessageBulb.NotifcationTypes.Exclamation)
                Case 42
                    If Not p.Item Is Nothing Then
                        r = New ReactionContainer("<name> almost forgot~it was holding~that " & p.Item.Name & "!", MessageBulb.NotifcationTypes.Question)
                    End If
                Case 43
                    If IceAround() = True Then
                        r = New ReactionContainer("Oh!~its slipping and came~over here for support.", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 44
                    If IceAround() = True Then
                        r = New ReactionContainer("Your Pokémon almost slipped~and fell over!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 45
                    r = New ReactionContainer("<name> sensed something~strange and was surprised!", MessageBulb.NotifcationTypes.Question)
                Case 46
                    r = New ReactionContainer("Your Pokémon is looking~around restlessly for~something.", MessageBulb.NotifcationTypes.Question)
                Case 47
                    r = New ReactionContainer("Your Pokémon wasn't watching~where it was going and~ran into you!", MessageBulb.NotifcationTypes.Friendly)
                Case 48
                    If ItemAround() = True Then
                        r = New ReactionContainer("Sniff, sniff!~Is there something nearby?", MessageBulb.NotifcationTypes.Question)
                    End If
                Case 49
                    r = New ReactionContainer("<name> is wandering~around and searching~for something.", MessageBulb.NotifcationTypes.Question)
                Case 50
                    r = New ReactionContainer("<name> is sniffing~at <player.name>.", MessageBulb.NotifcationTypes.Friendly)
                Case 51
                    If IsOutside() = True And World.GetCurrentRegionWeather() = World.Weathers.Rain And GrassAround() = True Then
                        r = New ReactionContainer("<name> is taking shelter in the grass from the rain!", MessageBulb.NotifcationTypes.Waiting)
                    End If
                Case 52
                    If IsOutside() = True And World.GetCurrentRegionWeather() = World.Weathers.Rain And GrassAround() = True Then
                        r = New ReactionContainer("<name> is splashing~around in the wet grass", MessageBulb.NotifcationTypes.Note)
                    End If
            End Select
        End While
        Return r
    End Function

    Private Shared Function GetLikeReaction(ByVal p As Pokemon) As ReactionContainer
        Dim r As ReactionContainer = Nothing
        While r Is Nothing
            Select Case Core.Random.Next(0, 28)
                Case 0
                    If IsOutside() = True And World.GetCurrentRegionWeather() = World.Weathers.Clear Then
                        r = New ReactionContainer("Your Pokémon seems happy~about the great weather!", MessageBulb.NotifcationTypes.Happy)
                    End If
                Case 1
                    r = New ReactionContainer("<name> is coming along~happily.", MessageBulb.NotifcationTypes.Friendly)
                Case 2
                    r = New ReactionContainer("<name> is composed!", MessageBulb.NotifcationTypes.Friendly)
                Case 3
                    If p.HP = p.MaxHP Then
                        r = New ReactionContainer("<name> is glowing~with health!", MessageBulb.NotifcationTypes.Note)
                    End If
                Case 4
                    r = New ReactionContainer("<name> looks~very happy!", MessageBulb.NotifcationTypes.Happy)
                Case 5
                    If p.HP = p.MaxHP Then
                        r = New ReactionContainer("<name> is full~of life!", MessageBulb.NotifcationTypes.Note)
                    End If
                Case 6
                    r = New ReactionContainer("<name> is very~eager!", MessageBulb.NotifcationTypes.Friendly)
                Case 7
                    r = New ReactionContainer("<name> gives you~a happy look and a smile!", MessageBulb.NotifcationTypes.Happy)
                Case 8
                    r = New ReactionContainer("<name> seems very~happy to see you!", MessageBulb.NotifcationTypes.Friendly)
                Case 9
                    r = New ReactionContainer("<name> faced this~way and grinned!", MessageBulb.NotifcationTypes.CatFace)
                Case 10
                    r = New ReactionContainer("<name> spun around~in a circle!", MessageBulb.NotifcationTypes.Note)
                Case 11
                    r = New ReactionContainer("<name> is looking~this way and smiling.", MessageBulb.NotifcationTypes.Friendly)
                Case 12
                    r = New ReactionContainer("<name> is very~eager...", MessageBulb.NotifcationTypes.Waiting)
                Case 13
                    r = New ReactionContainer("<name> is focusing~its attention on you!", MessageBulb.NotifcationTypes.Exclamation)
                Case 14
                    r = New ReactionContainer("<name> focused~with a sharp gaze!", MessageBulb.NotifcationTypes.Exclamation)
                Case 15
                    r = New ReactionContainer("<name> is looking~at <player.name>'s footprints.", MessageBulb.NotifcationTypes.Question)
                Case 16
                    r = New ReactionContainer("<name> is staring~straight into <player.name>'s~eyes.", MessageBulb.NotifcationTypes.Friendly)
                Case 17
                    If p.BaseSpeed >= 100 Then
                        r = New ReactionContainer("<name> is showing off~its agility!", MessageBulb.NotifcationTypes.Note)
                    End If
                Case 18
                    r = New ReactionContainer("<name> is moving~around happily!", MessageBulb.NotifcationTypes.Note)
                Case 19
                    r = New ReactionContainer("<name> is steadily~keeping up with you!", MessageBulb.NotifcationTypes.Friendly)
                Case 20
                    r = New ReactionContainer("<name> seems to~want to play with~<player.name>!", MessageBulb.NotifcationTypes.Note)
                Case 21
                    r = New ReactionContainer("<name> is singing~and humming.", MessageBulb.NotifcationTypes.Note)
                Case 22
                    r = New ReactionContainer("<name> is playfully~nibbling at the ground.", MessageBulb.NotifcationTypes.Friendly)
                Case 23
                    r = New ReactionContainer("<name> is nipping~at your feet!", MessageBulb.NotifcationTypes.Note)
                Case 24
                    If p.BaseAttack >= 100 Or p.BaseSpAttack >= 100 Then
                        r = New ReactionContainer("<name> is working~hard to show off~its mighty power!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 25
                    r = New ReactionContainer("<name> is cheerful!", MessageBulb.NotifcationTypes.Note)
                Case 26
                    r = New ReactionContainer("<name> bumped~into <player.name>!", MessageBulb.NotifcationTypes.Exclamation)
                Case 27
                    If IsCave() = True And p.Level < 20 Then
                        r = New ReactionContainer("<name> is scared~and snuggled up~to <player.name>!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
            End Select
        End While
        Return r
    End Function

    Private Shared Function GetLoyalReaction(ByVal p As Pokemon) As ReactionContainer
        Dim r As ReactionContainer = Nothing
        While r Is Nothing
            Select Case Core.Random.Next(0, 21)
                Case 0
                    r = New ReactionContainer("<name> began poking~you in the stomach!", MessageBulb.NotifcationTypes.CatFace)
                Case 1
                    r = New ReactionContainer("<name> seems to be~feeling great about~walking with you!", MessageBulb.NotifcationTypes.Heart)
                Case 2
                    r = New ReactionContainer("<name> is still~feeling great!", MessageBulb.NotifcationTypes.Happy)
                Case 3
                    r = New ReactionContainer("<name> is poking~at your belly.", MessageBulb.NotifcationTypes.Heart)
                Case 4
                    If p.Level > 30 Then
                        r = New ReactionContainer("<name> looks like~it wants to lead!", MessageBulb.NotifcationTypes.Note)
                    End If
                Case 5
                    r = New ReactionContainer("<name> seems to be~very happy!", MessageBulb.NotifcationTypes.Happy)
                Case 6
                    r = New ReactionContainer("<name> nodded slowly.", MessageBulb.NotifcationTypes.Friendly)
                Case 7
                    r = New ReactionContainer("<name> gave you~a sunny look!", MessageBulb.NotifcationTypes.Happy)
                Case 8
                    r = New ReactionContainer("<name> is very~composed and sure of itself!", MessageBulb.NotifcationTypes.Waiting)
                Case 9
                    If p.BaseDefense >= 100 Then
                        r = New ReactionContainer("<name> is~standing guard!", MessageBulb.NotifcationTypes.Exclamation)
                    End If
                Case 10
                    r = New ReactionContainer("<name> danced a~wonderful dance!", MessageBulb.NotifcationTypes.Note)
                Case 11
                    r = New ReactionContainer("<name> is staring~steadfastly at~<player.name>'s face.", MessageBulb.NotifcationTypes.Waiting)
                Case 12
                    r = New ReactionContainer("<name> is staring~intently at~<player.name>'s face.", MessageBulb.NotifcationTypes.Waiting)
                Case 13
                    r = New ReactionContainer("<name> is concentrating.", MessageBulb.NotifcationTypes.Unsure)
                Case 14
                    r = New ReactionContainer("<name> faced this~way and nodded.", MessageBulb.NotifcationTypes.Friendly)
                Case 15
                    r = New ReactionContainer("<name> suddenly~started walking closer!", MessageBulb.NotifcationTypes.Heart)
                Case 16
                    r = New ReactionContainer("Woah!*<name> is suddenly~playful!", MessageBulb.NotifcationTypes.Heart)
                Case 17
                    r = New ReactionContainer("<name> blushes.", MessageBulb.NotifcationTypes.Happy)
                Case 18
                    r = New ReactionContainer("Woah!*<name> suddenly started~dancing in happiness!", MessageBulb.NotifcationTypes.Note)
                Case 19
                    r = New ReactionContainer("<name> is happily~skipping about.", MessageBulb.NotifcationTypes.Note)
                Case 20
                    r = New ReactionContainer("Woah!*<name> suddenly~danced in happiness!", MessageBulb.NotifcationTypes.Note)
            End Select
        End While
        Return r
    End Function

    Private Shared Function GetLoveReaction(ByVal p As Pokemon) As ReactionContainer
        Dim r As ReactionContainer = Nothing
        While r Is Nothing
            Select Case Core.Random.Next(0, 13)
                Case 0
                    r = New ReactionContainer("<name> is jumping~for joy!", MessageBulb.NotifcationTypes.Heart)
                Case 1
                    r = New ReactionContainer("Your Pokémon stretched out~its body and is relaxing.", MessageBulb.NotifcationTypes.Happy)
                Case 2
                    r = New ReactionContainer("<name> is happily~cuddling up to you!", MessageBulb.NotifcationTypes.Heart)
                Case 3
                    r = New ReactionContainer("<name> is so happy~that it can't stand still!", MessageBulb.NotifcationTypes.Heart)
                Case 4
                    If p.PokedexEntry.Height <= 1.6F Then
                        r = New ReactionContainer("<name> happily~cuddled up to you!", MessageBulb.NotifcationTypes.Heart)
                    End If
                Case 5
                    r = New ReactionContainer("<name>'s cheeks are~becoming rosy!", MessageBulb.NotifcationTypes.Heart)
                Case 6
                    r = New ReactionContainer("Woah!*<name> suddenly~hugged you!", MessageBulb.NotifcationTypes.Heart)
                Case 7
                    If p.PokedexEntry.Height <= 0.7F Then
                        r = New ReactionContainer("<name> is rubbing~against your legs!", MessageBulb.NotifcationTypes.Heart)
                    End If
                Case 8
                    r = New ReactionContainer("Ah!~<name> cuddles you!", MessageBulb.NotifcationTypes.Heart)
                Case 9
                    r = New ReactionContainer("<name> is regarding~you with adoration!", MessageBulb.NotifcationTypes.Heart)
                Case 10
                    r = New ReactionContainer("<name> got~closer to <player.name>!", MessageBulb.NotifcationTypes.Heart)
                Case 11
                    r = New ReactionContainer("<name> is keeping~close to your feet.", MessageBulb.NotifcationTypes.Heart)
                Case 12
                    r = New ReactionContainer("<name> is jumping~around in a carefree way!", MessageBulb.NotifcationTypes.Note)
            End Select
        End While
        Return r
    End Function

    Shared SpecialReactionList As New List(Of ReactionContainer)

    Public Shared Sub Load()
        SpecialReactionList.Clear()

        Dim path As String = GameModeManager.GetContentFilePath("Data\interactions.dat")
        Security.FileValidation.CheckFileValid(path, False, "PokemonInteractions.vb")

        Dim data() As String = System.IO.File.ReadAllLines(path)

        For Each line As String In data
            If line.StartsWith("{") = True And line.EndsWith("}") = True Then
                If line.CountSeperators("|") >= 8 Then
                    Dim r As New ReactionContainer(line)
                    SpecialReactionList.Add(r)
                End If
            End If
        Next
    End Sub

    Private Class ReactionContainer

        Public Message As String
        Public Notification As MessageBulb.NotifcationTypes = MessageBulb.NotifcationTypes.AFK
        Public HasNotification As Boolean = True
        Public MapFiles As New List(Of String)
        Public PokemonIDs As New List(Of Integer)
        Public ExcludeIDs As New List(Of Integer)
        Public Daytime As Integer = -1
        Public Weather As Integer = -1
        Public Season As Integer = -1
        Public Types As New List(Of Element.Types)
        Public Probability As Integer = 100

        Public Sub New(ByVal dataLine As String)
            dataLine = dataLine.Remove(dataLine.Length - 1, 1).Remove(0, 1)

            Dim dataParts() As String = dataLine.Split(CChar("|"))

            Me.MapFiles = dataParts(0).Split(CChar(",")).ToList()

            If dataParts(1) <> "-1" Then
                For Each pokePart As String In dataParts(1).Split(CChar(","))
                    Dim lReference As List(Of Integer) = PokemonIDs
                    If pokePart.StartsWith("!") = True Then
                        pokePart = pokePart.Remove(0, 1)
                        lReference = ExcludeIDs
                    End If
                    If IsNumeric(pokePart) = True Then
                        If lReference.Contains(CInt(pokePart)) = False Then
                            lReference.Add(CInt(pokePart))
                        End If
                    End If
                Next
            End If

            If dataParts(2) <> "-1" Then
                Daytime = CInt(dataParts(2)).Clamp(0, 3)
            End If
            If dataParts(3) <> "-1" Then
                Weather = CInt(dataParts(3)).Clamp(0, 9)
            End If
            If dataParts(4) <> "-1" Then
                Season = CInt(dataParts(4)).Clamp(0, 3)
            End If

            If dataParts(5) <> "-1" Then
                For Each typePart As String In dataParts(5).Split(CChar(","))
                    Me.Types.Add(New Element(typePart).Type)
                Next
            End If

            Me.Probability = CInt(dataParts(6))

            If dataParts(7) = "-1" Then
                Me.HasNotification = False
            Else
                Me.HasNotification = True
                Me.Notification = Me.ConvertEmoji(dataParts(7))
            End If

            Me.Message = dataParts(8)
        End Sub

        Public Sub New(ByVal Message As String, ByVal Notification As MessageBulb.NotifcationTypes)
            Me.Message = Message
            Me.Notification = Notification
        End Sub

        Private Function ConvertEmoji(ByVal s As String) As MessageBulb.NotifcationTypes
            Select Case s.ToLower()
                Case "..."
                    Return MessageBulb.NotifcationTypes.Waiting
                Case "!"
                    Return MessageBulb.NotifcationTypes.Exclamation
                Case ">:("
                    Return MessageBulb.NotifcationTypes.Shouting
                Case "?"
                    Return MessageBulb.NotifcationTypes.Question
                Case "note"
                    Return MessageBulb.NotifcationTypes.Note
                Case "<3"
                    Return MessageBulb.NotifcationTypes.Heart
                Case ":("
                    Return MessageBulb.NotifcationTypes.Unhappy
                Case "ball"
                    Return MessageBulb.NotifcationTypes.Battle
                Case ":D"
                    Return MessageBulb.NotifcationTypes.Happy
                Case ":)"
                    Return MessageBulb.NotifcationTypes.Friendly
                Case "bad"
                    Return MessageBulb.NotifcationTypes.Poisoned
                Case ";)"
                    Return MessageBulb.NotifcationTypes.Wink
                Case "afk"
                    Return MessageBulb.NotifcationTypes.AFK
                Case "/:("
                    Return MessageBulb.NotifcationTypes.Angry
                Case ":3"
                    Return MessageBulb.NotifcationTypes.CatFace
                Case ":/"
                    Return MessageBulb.NotifcationTypes.Unsure
            End Select

            Return MessageBulb.NotifcationTypes.Waiting
        End Function

        Public Function Match(ByVal p As Pokemon) As Boolean
            If MapFiles.Count > 0 Then
                If MapFiles.Any(Function(m As String)
                                    Return m.ToLowerInvariant() = Screen.Level.LevelFile.ToLowerInvariant()
                                End Function) Then
                    Return False

                End If
            End If

            If PokemonIDs.Count > 0 Then
                If PokemonIDs.Contains(p.Number) = False Then
                    Return False
                End If
            End If

            If ExcludeIDs.Count > 0 Then
                If ExcludeIDs.Contains(p.Number) = True Then
                    Return False
                End If
            End If

            If Daytime > -1 Then
                If Daytime <> CInt(World.GetTime()) Then
                    Return False
                End If
            End If

            If Weather > -1 Then
                If Weather <> CInt(World.GetCurrentRegionWeather) Then
                    Return False
                End If
            End If

            If Season > -1 Then
                If Season <> CInt(World.CurrentSeason) Then
                    Return False
                End If
            End If

            If Me.Types.Count > 0 Then
                For Each t As Element.Types In Me.Types
                    If p.IsType(t) = False Then
                        Return False
                    End If
                Next
            End If

            Return True
        End Function

        Public Function GetMessage(ByVal p As Pokemon) As String
            Return Me.Message.Replace("<name>", p.GetDisplayName())
        End Function

    End Class

    Shared PickupIndividualValue As String = "" 'This value holds the individual value of the Pokémon that picked up the item.
    Shared PickupItemID As Integer = -1 'This is the Item ID of the item that the Pokémon picked up. -1 means no item got picked up.

    Public Shared Sub CheckForRandomPickup()
        'Checks if the first Pokémon in the party is following the player:
        If Screen.Level.ShowOverworldPokemon = True And CBool(GameModeManager.GetGameRuleValue("ShowFollowPokemon", "1")) = True Then
            'Checks if the player has a Pokémon:
            If Core.Player.Pokemons.Count > 0 And Screen.Level.Surfing = False And Screen.Level.Riding = False And Screen.Level.ShowOverworldPokemon = True And Not Core.Player.GetWalkPokemon() Is Nothing Then
                If Core.Player.GetWalkPokemon().Status = Pokemon.StatusProblems.None Then
                    'If the player switched the Pokémon, reset the item ID.
                    If PickupIndividualValue <> Core.Player.GetWalkPokemon().IndividualValue Then
                        PickupItemID = -1
                    End If

                    'Check if an item should be generated:
                    If Core.Random.Next(0, 270) < Core.Player.GetWalkPokemon().Friendship Then
                        Dim newItemID As Integer = -1 'creates a temp value to hold the new Item ID.

                        'Checks if the player is outside:
                        If IsOutside() = True Then
                            'Checks if the leading Pokémon is holding a sticky feather, which ensures a 90% feather pickup outside:
                            If Not Core.Player.GetWalkPokemon().Item Is Nothing AndAlso Core.Player.GetWalkPokemon().Item.ID = 261 AndAlso Core.Random.Next(0, 100) < 90 Then
                                newItemID = Core.Random.Next(254, 261)
                            Else
                                'Checks if ice is around:
                                If IceAround() = True Then
                                    '20%: NeverMeltIce, 80% Aspear Berry
                                    If Core.Random.Next(0, 100) < 20 Then
                                        newItemID = 107
                                    Else
                                        newItemID = 2004
                                    End If
                                Else
                                    'Checks if loamy soil is around, if so, give a random berry (only 50% activation)
                                    If LoamySoilAround() = True And Core.Random.Next(0, 100) < 50 Then
                                        newItemID = Core.Random.Next(2000, 2064)
                                    Else
                                        'Checks if grass is around, if so, give a grass item (only 50% activation)
                                        If GrassAround() = True And Core.Random.Next(0, 100) < 50 Then
                                            'Leaf Stone (10%), energy root (50%), revival herb (15%), heal powder (25%):
                                            Dim r As Integer = Core.Random.Next(0, 100)
                                            If r < 10 Then
                                                newItemID = 34
                                            ElseIf r >= 10 And r < 60 Then
                                                newItemID = 122
                                            ElseIf r >= 60 And r < 75 Then
                                                newItemID = 124
                                            Else
                                                newItemID = 123
                                            End If
                                        Else
                                            'Checks if water is around, if so, give a water item (only 50% activation)
                                            If WaterAround() = True And Core.Random.Next(0, 100) < 50 Then
                                                'Water Stone (10%), pearl (50%), big pearl (10%), heart scale (40%):
                                                Dim r As Integer = Core.Random.Next(0, 100)
                                                If r < 10 Then
                                                    newItemID = 24
                                                ElseIf r >= 10 And r < 50 Then
                                                    newItemID = 110
                                                ElseIf r >= 50 And r < 60 Then
                                                    newItemID = 190
                                                Else
                                                    newItemID = 111
                                                End If
                                            Else
                                                'No special conditions apply:
                                                '(general): first 10 berries (45%), wings (45%), gold leaf (2%), silver leaf (8%):
                                                Dim r As Integer = Core.Random.Next(0, 100)
                                                If r < 45 Then
                                                    newItemID = Core.Random.Next(2000, 2011)
                                                ElseIf r >= 45 And r < 90 Then
                                                    newItemID = Core.Random.Next(254, 262)
                                                ElseIf r >= 90 And r < 92 Then
                                                    newItemID = 75
                                                Else
                                                    newItemID = 60
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        ElseIf IsInside() = True Then 'Player is inside:
                            'leftovers (5%), lavacookie (20%), super potion (10%), ether (20%), elixier (20%), potion (10%), quick claw (8%), gold leaf (2%), silver leaf (5%)
                            Dim r As Integer = Core.Random.Next(0, 100)
                            If r < 5 Then
                                newItemID = 146 'Leftovers
                            ElseIf r >= 5 And r < 25 Then
                                newItemID = 7 'Lavacookie
                            ElseIf r >= 25 And r < 45 Then
                                '0-1 badge: potion
                                '2-4 badges: super potion
                                '5-7 badges: hyper potion
                                '8 and above: max potion
                                Dim b As Integer = Core.Player.Badges.Count
                                If b <= 1 Then
                                    newItemID = 18
                                ElseIf b >= 2 And b <= 4 Then
                                    newItemID = 17
                                ElseIf b >= 5 And b <= 7 Then
                                    newItemID = 16
                                Else
                                    newItemID = 15
                                End If
                            ElseIf r >= 45 And r < 65 Then
                                newItemID = 63 'Ether
                            ElseIf r >= 65 And r < 85 Then
                                newItemID = 65 'Elixier
                            ElseIf r >= 85 And r < 93 Then
                                newItemID = 73 'Quick Claw
                            ElseIf r >= 93 And r < 95 Then
                                newItemID = 75 'Gold leaf
                            Else
                                newItemID = 60 'Silver leaf
                            End If
                            'Player is in cave:
                        ElseIf IsCave() = True Then
                            'Checks if the leading Pokémon is holding a sticky rock, which ensures a 90% feather pickup in a cave:
                            If Not Core.Player.GetWalkPokemon().Item Is Nothing AndAlso Core.Player.GetWalkPokemon().Item.ID = 262 AndAlso Core.Random.Next(0, 100) < 90 Then
                                'Thunderstone(20%),Firestone(20%),Waterstone(20%),Leafstone(20%),Moonstone(10%),Sunstone(10%)
                                Dim r1 As Integer = Core.Random.Next(0, 100)
                                If r1 < 20 Then
                                    newItemID = 22
                                ElseIf r1 >= 20 And r1 < 40 Then
                                    newItemID = 23
                                ElseIf r1 >= 40 And r1 < 60 Then
                                    newItemID = 24
                                ElseIf r1 >= 60 And r1 < 80 Then
                                    newItemID = 34
                                ElseIf r1 >= 80 And r1 < 90 Then
                                    newItemID = 8
                                Else
                                    newItemID = 169
                                End If
                            Else
                                'Checks if water is around, if so, give a water item (only 65% activation)
                                If WaterAround() = True And Core.Random.Next(0, 100) < 65 Then
                                    'Water Stone (30%), pearl (40%), big pearl (10%), heart scale (20%):
                                    Dim r As Integer = Core.Random.Next(0, 100)
                                    If r < 30 Then
                                        newItemID = 24
                                    ElseIf r >= 30 And r < 70 Then
                                        newItemID = 110
                                    ElseIf r >= 70 And r < 80 Then
                                        newItemID = 190
                                    Else
                                        newItemID = 111
                                    End If
                                Else
                                    'Fire Stone (10%), Thunder Stone (10%), pearl (30%), hard stone (10%), everstone (10%), first 10 berries (20%)
                                    Dim r As Integer = Core.Random.Next(0, 100)
                                    If r < 10 Then
                                        newItemID = 22
                                    ElseIf r >= 10 And r < 20 Then
                                        newItemID = 23
                                    ElseIf r >= 20 And r < 50 Then
                                        newItemID = 110
                                    ElseIf r >= 50 And r < 60 Then
                                        newItemID = 125
                                    ElseIf r >= 60 And r < 70 Then
                                        newItemID = 112
                                    ElseIf r >= 70 And r < 90 Then
                                        newItemID = Core.Random.Next(2000, 2011)
                                    Else
                                        newItemID = 262
                                    End If
                                End If
                            End If
                        End If

                        'If an item got generated, assign it to the global value to store it until the player interacts with the Pokémon. Also store the individual value.
                        If newItemID > -1 Then
                            Logger.Debug("Pokémon picks up item (" & Item.GetItemByID(newItemID).Name & ")")
                            PickupItemID = newItemID
                            PickupIndividualValue = Core.Player.GetWalkPokemon().IndividualValue
                            SoundManager.PlaySound("pickup")
                        End If
                    End If
                End If
            Else
                'Reset the system if no Pokémon:
                PickupItemID = -1
                PickupIndividualValue = ""
            End If
        End If
    End Sub

#Region "Checks"

    Private Shared Function IsOutside() As Boolean
        If Screen.Level.CanFly = True And Screen.Level.CanDig = False And Screen.Level.CanTeleport = True Then
            Return True
        End If
        Return False
    End Function

    Private Shared Function IsCave() As Boolean
        If Screen.Level.CanFly = False And Screen.Level.CanDig = True And Screen.Level.CanTeleport = False Then
            Return True
        End If
        Return False
    End Function

    Private Shared Function IsInside() As Boolean
        If Screen.Level.CanFly = False And Screen.Level.CanDig = False And Screen.Level.CanTeleport = False Then
            Return True
        End If
        Return False
    End Function

    Private Shared Function WildPokemon() As Boolean
        If Screen.Level.WildPokemonFloor = True Or Screen.Level.WildPokemonGrass = True Or Screen.Level.WildPokemonWater = True Then
            Return True
        End If
        Return False
    End Function

    Private Shared Function WaterAround() As Boolean
        For Each e As Entity In Screen.Level.Entities
            If e.EntityID.ToLower() = "water" Then
                If Vector3.Distance(e.Position, Screen.Camera.Position) <= 5.0F Then
                    Return True
                End If
            End If
        Next

        Return False
    End Function

    Private Shared Function NPCAround() As Boolean
        For Each e As Entity In Screen.Level.Entities
            If e.EntityID.ToLower() = "npc" Then
                If Vector3.Distance(e.Position, Screen.Camera.Position) <= 4.0F Then
                    Return True
                End If
            End If
        Next

        Return False
    End Function

    Private Shared Function TrainerAround() As Boolean
        For Each e As Entity In Screen.Level.Entities
            If e.EntityID.ToLower() = "npc" Then
                If CType(e, NPC).IsTrainer = True Then
                    If Vector3.Distance(e.Position, Screen.Camera.Position) <= 3.0F Then
                        Return True
                    End If
                End If
            End If
        Next

        Return False
    End Function

    Private Shared Function GrassAround() As Boolean
        For Each e As Entity In Screen.Level.Entities
            If e.EntityID.ToLower() = "grass" Then
                If Vector3.Distance(e.Position, Screen.Camera.Position) <= 5.0F Then
                    Return True
                End If
            End If
        Next

        Return False
    End Function

    Private Shared Function ItemAround() As Boolean
        For Each e As Entity In Screen.Level.Entities
            If e.EntityID.ToLower() = "itemobject" Then
                If Vector3.Distance(e.Position, Screen.Camera.Position) <= 5.0F Then
                    Return True
                End If
            End If
        Next

        Return False
    End Function

    Private Shared Function IceAround() As Boolean
        For Each e As Entity In Screen.Level.Entities
            If e.EntityID.ToLower() = "floor" Then
                If CType(e, Floor).IsIce = True Then
                    If Vector3.Distance(e.Position, Screen.Camera.Position) <= 2.0F Then
                        Return True
                    End If
                End If
            End If
        Next

        Return False
    End Function

    Private Shared Function LoamySoilAround() As Boolean
        For Each e As Entity In Screen.Level.Entities
            If e.EntityID.ToLower() = "loamysoil" Then
                If Vector3.Distance(e.Position, Screen.Camera.Position) <= 5.0F Then
                    Return True
                End If
            End If
        Next

        Return False
    End Function

#End Region

End Class