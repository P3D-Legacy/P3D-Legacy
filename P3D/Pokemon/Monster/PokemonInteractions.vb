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
        If PickupItemID > "-1" Then
            If PickupIndividualValue = p.IndividualValue Then
                Return GenerateItemReaction(p, cPosition, facing)
            Else
                PickupItemID = "-1"
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

        Dim s As String = "version=2" & Environment.NewLine &
            "@pokemon.cry(" & PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True) & ")" & Environment.NewLine

        If CType(Screen.Camera, OverworldCamera).ThirdPerson = False Then
            If reaction.HasNotification = True Then
                s &= "@camera.activatethirdperson" & Environment.NewLine &
                     "@camera.setposition(" & newPosition.X & ",1," & newPosition.Y & ")" & Environment.NewLine

                s &= "@entity.showmessagebulb(" & CInt(reaction.GetNotification()).ToString() & "|" & cPosition.X + offset.X & "|" & cPosition.Y + 0.7F & "|" & cPosition.Z + offset.Y & ")" & Environment.NewLine

                s &= "@camera.deactivatethirdperson" & Environment.NewLine
            End If
            s &= "@text.show(" & reaction.GetMessage(p) & ")" & Environment.NewLine
        Else
            Dim preYaw As Single = Screen.Camera.Yaw
            If reaction.HasNotification = True Then
                s &= "@camera.setyaw(" & CType(Screen.Camera, OverworldCamera).GetAimYawFromDirection(Screen.Camera.GetPlayerFacingDirection()) & ")" & Environment.NewLine
                s &= "@camera.setposition(" & newPosition.X & ",1," & newPosition.Y & ")" & Environment.NewLine
                s &= "@entity.showmessagebulb(" & CInt(reaction.GetNotification()).ToString() & "|" & cPosition.X + offset.X & "|" & cPosition.Y + 0.7F & "|" & cPosition.Z + offset.Y & ")" & Environment.NewLine

                s &= "@camera.deactivatethirdperson" & Environment.NewLine
            End If
            s &= "@text.show(" & reaction.GetMessage(p) & ")" & Environment.NewLine
            s &= "@camera.activatethirdperson" & Environment.NewLine
            s &= "@camera.reset" & Environment.NewLine
            s &= "@camera.setyaw(" & preYaw & ")" & Environment.NewLine
        End If
        s &= ":end"

        Return s
    End Function

    Private Shared Function GenerateItemReaction(ByVal p As Pokemon, ByVal cPosition As Vector3, ByVal facing As Integer) As String
        Dim message As String = Localization.GetString("FollowerInteraction_HeldItem_Question", "It looks like your Pokémon~holds on to something.*Do you want to~take it?")

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

        Dim s As String = "version=2" & Environment.NewLine &
           "@pokemon.cry(" & PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData) & ")" & Environment.NewLine

        If CType(Screen.Camera, OverworldCamera).ThirdPerson = False Then
            s &= "@camera.activatethirdperson" & Environment.NewLine &
                 "@camera.setposition(" & newPosition.X & ",1," & newPosition.Y & ")" & Environment.NewLine

            s &= "@entity.showmessagebulb(" & CInt(MessageBulb.NotificationTypes.Question).ToString() & "|" & cPosition.X + offset.X & "|" & cPosition.Y + 0.7F & "|" & cPosition.Z + offset.Y & ")" & Environment.NewLine

            s &= "@camera.deactivatethirdperson" & Environment.NewLine
            s &= "@text.show(" & message & ")" & Environment.NewLine &
                "@options.show(<system.token(global_yes)>,<system.token(global_no)>)" & Environment.NewLine &
                ":when:<system.token(global_yes)>" & Environment.NewLine &
                "@text.show(" & Localization.GetString("FollowerInteraction_HeldItem_Answer_Yes", "Your Pokémon handed over~the <item>!").Replace("//ITEM//", item.OneLineName()) & ")" & Environment.NewLine &
                "@item.give(" & PickupItemID & ",1)" & Environment.NewLine &
                "@item.messagegive(" & PickupItemID & ",1)" & Environment.NewLine &
                ":when:<system.token(global_no)>" & Environment.NewLine &
                "@text.show(" & Localization.GetString("FollowerInteraction_HeldItem_Answer_No", "Your Pokémon kept~the item happily.") & ")" & Environment.NewLine &
                "@pokemon.addfriendship(0,10)" & Environment.NewLine &
                ":endwhen" & Environment.NewLine
        Else
            s &= "@camera.setposition(" & newPosition.X & ",1," & newPosition.Y & ")" & Environment.NewLine
            s &= "@entity.showmessagebulb(" & CInt(MessageBulb.NotificationTypes.Question).ToString() & "|" & cPosition.X + offset.X & "|" & cPosition.Y + 0.7F & "|" & cPosition.Z + offset.Y & ")" & Environment.NewLine

            s &= "@camera.deactivatethirdperson" & Environment.NewLine

            s &= "@text.show(" & message & ")" & Environment.NewLine &
                "@options.show(<system.token(global_yes)>,<system.token(global_no)>)" & Environment.NewLine &
                ":when:<system.token(global_yes)>" & Environment.NewLine &
                "@text.show(" & Localization.GetString("FollowerInteraction_HeldItem_Answer_Yes", "Your Pokémon handed over~the <item>!").Replace("//ITEM//", item.OneLineName()) & ")" & Environment.NewLine &
                "@item.give(" & PickupItemID & ",1)" & Environment.NewLine &
                "@item.messagegive(" & PickupItemID & ",1)" & Environment.NewLine &
                ":when:<system.token(global_no)>" & Environment.NewLine &
                "@text.show(" & Localization.GetString("FollowerInteraction_HeldItem_Answer_No", "Your Pokémon kept~the item happily.") & ")" & Environment.NewLine &
                "@pokemon.addfriendship(0,10)" & Environment.NewLine &
                ":endwhen" & Environment.NewLine
            s &= "@camera.activatethirdperson" & Environment.NewLine
        End If
        s &= ":end"

        PickupItemID = "-1"
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
                Return New ReactionContainer(Localization.GetString("FollowerInteraction_StatusEffect_Poison", "<name> is shivering~with the effects of being~poisoned."), MessageBulb.NotificationTypes.Poisoned)
            Case Pokemon.StatusProblems.Burn
                Return New ReactionContainer(Localization.GetString("FollowerInteraction_StatusEffect_Burn", "<name>'s burn~looks painful!"), MessageBulb.NotificationTypes.Poisoned)
            Case Pokemon.StatusProblems.Freeze
                Select Case Core.Random.Next(0, 2)
                    Case 0
                        Return New ReactionContainer(Localization.GetString("FollowerInteraction_StatusEffect_Freeze1", "<name> seems very cold!"), MessageBulb.NotificationTypes.Poisoned)
                    Case 1
                        Return New ReactionContainer(Localization.GetString("FollowerInteraction_StatusEffect_Freeze2", ".....Your Pokémon seems~a little cold."), MessageBulb.NotificationTypes.Poisoned)
                End Select
            Case Pokemon.StatusProblems.Paralyzed
                Return New ReactionContainer(Localization.GetString("FollowerInteraction_StatusEffect_Paralyzed", "<name> is trying~very hard to keep~up with you..."), MessageBulb.NotificationTypes.Poisoned)
            Case Pokemon.StatusProblems.Sleep
                Select Case Core.Random.Next(0, 3)
                    Case 0
                        Return New ReactionContainer(Localization.GetString("FollowerInteraction_StatusEffect_Sleep1", "<name> seems~a little tired."), MessageBulb.NotificationTypes.Poisoned)
                    Case 1
                        Return New ReactionContainer(Localization.GetString("FollowerInteraction_StatusEffect_Sleep2", "<name> is somehow~fighting off sleep..."), MessageBulb.NotificationTypes.Poisoned)
                    Case 2
                        Return New ReactionContainer(Localization.GetString("FollowerInteraction_StatusEffect_Sleep3", "<name> yawned~very loudly!"), MessageBulb.NotificationTypes.Poisoned)
                End Select
        End Select
        Return New ReactionContainer(Localization.GetString("FollowerInteraction_StatusEffect_Other", "<name> is trying~very hard to keep~up with you..."), MessageBulb.NotificationTypes.Poisoned)
    End Function

    Private Shared Function GetLowHPReaction(ByVal p As Pokemon) As ReactionContainer
        Select Case Core.Random.Next(0, 2)
            Case 0
                Return New ReactionContainer(Localization.GetString("FollowerInteraction_LowHP1", "<name> is going~to fall down!"), MessageBulb.NotificationTypes.Exclamation)
            Case 1
                Return New ReactionContainer(Localization.GetString("FollowerInteraction_LowHP2", "<name> seems to~be about to fall over!"), MessageBulb.NotificationTypes.Exclamation)
        End Select
        Return New ReactionContainer(Localization.GetString("FollowerInteraction_LowHP2", "<name> seems to~be about to fall over!"), MessageBulb.NotificationTypes.Exclamation)
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
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate1", "<name> is doing~it's best to keep up~with you."), MessageBulb.NotificationTypes.Unhappy)
                Case 1
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate2", "<name> is somehow~forcing itself to keep going."), MessageBulb.NotificationTypes.Unsure)
                Case 2
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate3", "<name> is staring~patiantly at nothing at all."), MessageBulb.NotificationTypes.Unsure)
                Case 3
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate4", "<name> is staring~intently into the distance."), MessageBulb.NotificationTypes.Waiting)
                Case 4
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate5", "<name> is dizzy..."), MessageBulb.NotificationTypes.Unhappy)
                Case 5
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate6", "<name> is stepping~on your feet!"), MessageBulb.NotificationTypes.Waiting)
                Case 6
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate7", "<name> seems~unhappy somehow..."), MessageBulb.NotificationTypes.Unhappy)
                Case 7
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate8", "<name> is making~an unhappy face."), MessageBulb.NotificationTypes.Unhappy)
                Case 8
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate9", "<name> seems~uneasy and is poking~<player.name>."), MessageBulb.NotificationTypes.Unsure)
                Case 9
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate10", "<name> is making~a face like its angry!"), MessageBulb.NotificationTypes.Angry)
                Case 10
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate11", "<name> seems to be~angry for some reason."), MessageBulb.NotificationTypes.Angry)
                Case 11
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate12", "Your Pokémon turned to face~the other way, showing a~defiant expression."), MessageBulb.NotificationTypes.Unsure)
                Case 12
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate13", "<name> is looking~down steadily..."), MessageBulb.NotificationTypes.Waiting)
                Case 13
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate14", "Your Pokémon is staring~intently at nothing..."), MessageBulb.NotificationTypes.Waiting)
                Case 14
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate15", "Your Pokémon turned to~face the other way,~showing a defiant expression."), MessageBulb.NotificationTypes.Unhappy)
                Case 15
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate16", "<name> seems~a bit nervous..."), MessageBulb.NotificationTypes.Unsure)
                Case 16
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate17", "Your Pokémon stumbled~and nearly fell!"), MessageBulb.NotificationTypes.Exclamation)
                Case 17
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Hate18", "<name> is having~a hard time keeping up."), MessageBulb.NotificationTypes.Unsure)
            End Select
        End While

        Return r
    End Function

    Private Shared Function GetNeutralReaction(ByVal p As Pokemon) As ReactionContainer
        Dim r As ReactionContainer = Nothing
        While r Is Nothing
            Select Case Core.Random.Next(0, 53)
                Case 0
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral1", "<name> is happy~but shy."), MessageBulb.NotificationTypes.Friendly)
                Case 1
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral2", "<name> puts in~extra effort."), MessageBulb.NotificationTypes.Friendly)
                Case 2
                    If IsOutside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral3", "<name> is smelling~the scents of the~surrounding air."), MessageBulb.NotificationTypes.Friendly)
                    End If
                Case 3
                    If IsOutside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral4", "Your Pokémon has caught~the scent of smoke."), MessageBulb.NotificationTypes.Friendly)
                    End If
                Case 4
                    If NPCAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral5", "<name> greeted everyone!"), MessageBulb.NotificationTypes.CatFace)
                    End If
                Case 5
                    If IsOutside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral6", "<name> is wandering~around and listening~to the different sounds."), MessageBulb.NotificationTypes.Note)
                    End If
                Case 6
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral7", "<name> looks very~interested!"), MessageBulb.NotificationTypes.Friendly)
                Case 7
                    If IsOutside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral8", "<name> is steadily~poking at the ground."), MessageBulb.NotificationTypes.Waiting)
                    End If
                Case 8
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral9", "Your Pokémon is looking~around restlessly."), MessageBulb.NotificationTypes.Note)
                Case 9
                    If IsOutside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral10", "<name> seems dazzled~after seeing the sky."), MessageBulb.NotificationTypes.Waiting)
                    End If
                Case 10
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral11", "<name> is gazing~around restlessly!"), MessageBulb.NotificationTypes.Waiting)
                Case 11
                    If TrainerAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral12", "<name> let out~a battle cry!"), MessageBulb.NotificationTypes.Shouting)
                    End If
                Case 12
                    If TrainerAround() = True And p.IsType(Element.Types.Fire) = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral13", "<name> is vigorously~breathing fire!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 13
                    If TrainerAround() = True Or NPCAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral14", "<name> is on~the lookout!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 14
                    If TrainerAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral15", "<name> roared!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 15
                    If TrainerAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral16", "<name> let out a roar!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 16
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral17", "<name> is surveying~the area..."), MessageBulb.NotificationTypes.Waiting)
                Case 17
                    If IsInside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral18", "<name> is sniffing~at the floor."), MessageBulb.NotificationTypes.Question)
                    End If
                Case 18
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral19", "<name> is peering~down."), MessageBulb.NotificationTypes.Question)
                Case 19
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral20", "<name> seems~to be wandering around."), MessageBulb.NotificationTypes.Note)
                Case 20
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral21", "<name> is looking~around absentmindedly."), MessageBulb.NotificationTypes.Waiting)
                Case 21
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral22", "<name> is relaxing~comfortably."), MessageBulb.NotificationTypes.Friendly)
                Case 22
                    If IsInside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral23", "<name> is sniffing~at the floor."), MessageBulb.NotificationTypes.Waiting)
                    End If
                Case 23
                    If IsOutside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral24", "<name> seems to~relax as it hears the~sound of rustling leaves..."), MessageBulb.NotificationTypes.Friendly)
                    End If
                Case 24
                    If IsOutside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral25", "<name> seems to~be listening to the~sound of rustling leaves..."), MessageBulb.NotificationTypes.Friendly)
                    End If
                Case 25
                    If WaterAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral26", "Your Pokémon is playing around~and splashing in the water!"), MessageBulb.NotificationTypes.Happy)
                    End If
                Case 26
                    If IsOutside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral27", "<name> is looking~up at the sky."), MessageBulb.NotificationTypes.Waiting)
                    End If
                Case 27
                    If IsOutside() = True And World.GetTime() = World.DayTimes.Night And World.GetCurrentRegionWeather() = World.Weathers.Clear Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral28", "Your Pokémon is happily~gazing at the beautiful,~starry sky!"), MessageBulb.NotificationTypes.Waiting)
                    End If
                Case 28
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral29", "<name> seems to be~enjoying this a little bit!"), MessageBulb.NotificationTypes.Note)
                Case 29
                    If IsInside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral30", "<name> is looking~up at the ceiling."), MessageBulb.NotificationTypes.Note)
                    End If
                Case 30
                    If IsOutside() = True And World.GetTime() = World.DayTimes.Night Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral31", "Your Pokémon is staring~spellbound at the night sky!"), MessageBulb.NotificationTypes.Friendly)
                    End If
                Case 31
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral32", "<name> is in~danger of falling over!"), MessageBulb.NotificationTypes.Exclamation)
                Case 32
                    If p.NickName <> "" Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral33", "<name> doesn't~seem to be used to its~own name yet."), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 33
                    If IsInside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral34", "<name> slipped~on the floor and seems~likely to fall!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 34
                    If TrainerAround() = True Or GrassAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral35", "<name> feels something~and is howling!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 35
                    If p.HP = p.MaxHP And p.Status = Pokemon.StatusProblems.None Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral36", "<name> seems~refreshed!"), MessageBulb.NotificationTypes.Friendly)
                    End If
                Case 36
                    If p.HP = p.MaxHP And p.Status = Pokemon.StatusProblems.None Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral37", "<name> feels~refreshed."), MessageBulb.NotificationTypes.Friendly)
                    End If
                Case 37
                    If ItemAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral38", "<name> seems to~have found something!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 38
                    If TrainerAround() = True Or GrassAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral39", "<name> suddenly~turned around and~started barking!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 39
                    If TrainerAround() = True Or GrassAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral40", "<name> suddenly~turned around!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 40
                    If IsOutside() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral41", "<name> looked up~at the sky and shouted loudly!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 41
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral42", "Your Pokémon was surprised~that you suddenly spoke to it!"), MessageBulb.NotificationTypes.Exclamation)
                Case 42
                    If Not p.Item Is Nothing Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral43", "<name> almost forgot~it was holding~that <item>!"), MessageBulb.NotificationTypes.Question)
                    End If
                Case 43
                    If IceAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral44", "Oh!~It's slipping and came~over here for support."), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 44
                    If IceAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral45", "Your Pokémon almost slipped~and fell over!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 45
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral46", "<name> sensed something~strange and was surprised!"), MessageBulb.NotificationTypes.Question)
                Case 46
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral47", "Your Pokémon is looking~around restlessly for~something."), MessageBulb.NotificationTypes.Question)
                Case 47
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral48", "Your Pokémon wasn't watching~where it was going and~ran into you!"), MessageBulb.NotificationTypes.Friendly)
                Case 48
                    If ItemAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral49", "Sniff, sniff!~Is there something nearby?"), MessageBulb.NotificationTypes.Question)
                    End If
                Case 49
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral50", "<name> is wandering~around and searching~for something."), MessageBulb.NotificationTypes.Question)
                Case 50
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral51", "<name> is sniffing~at <player.name>."), MessageBulb.NotificationTypes.Friendly)
                Case 51
                    If IsOutside() = True And World.GetCurrentRegionWeather() = World.Weathers.Rain And GrassAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral52", "<name> is taking~shelter in the grass from~the rain!"), MessageBulb.NotificationTypes.Waiting)
                    End If
                Case 52
                    If IsOutside() = True And World.GetCurrentRegionWeather() = World.Weathers.Rain And GrassAround() = True Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Neutral53", "<name> is splashing~around in the wet grass!"), MessageBulb.NotificationTypes.Note)
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
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like1", "Your Pokémon seems happy~about the great weather!"), MessageBulb.NotificationTypes.Happy)
                    End If
                Case 1
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like2", "<name> is coming along~happily."), MessageBulb.NotificationTypes.Friendly)
                Case 2
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like3", "<name> is composed!"), MessageBulb.NotificationTypes.Friendly)
                Case 3
                    If p.HP = p.MaxHP Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like4", "<name> is glowing~with health!"), MessageBulb.NotificationTypes.Note)
                    End If
                Case 4
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like5", "<name> looks~very happy!"), MessageBulb.NotificationTypes.Happy)
                Case 5
                    If p.HP = p.MaxHP Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like6", "<name> is full~of life!"), MessageBulb.NotificationTypes.Note)
                    End If
                Case 6
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like7", "<name> is very~eager!"), MessageBulb.NotificationTypes.Friendly)
                Case 7
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like8", "<name> gives you~a happy look and a smile!"), MessageBulb.NotificationTypes.Happy)
                Case 8
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like9", "<name> seems very~happy to see you!"), MessageBulb.NotificationTypes.Friendly)
                Case 9
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like10", "<name> faced this~way and grinned!"), MessageBulb.NotificationTypes.CatFace)
                Case 10
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like11", "<name> spun around~in a circle!"), MessageBulb.NotificationTypes.Note)
                Case 11
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like12", "<name> is looking~this way and smiling."), MessageBulb.NotificationTypes.Friendly)
                Case 12
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like13", "<name> is very~eager..."), MessageBulb.NotificationTypes.Waiting)
                Case 13
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like14", "<name> is focusing~its attention on you!"), MessageBulb.NotificationTypes.Exclamation)
                Case 14
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like15", "<name> focused~with a sharp gaze!"), MessageBulb.NotificationTypes.Exclamation)
                Case 15
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like16", "<name> is looking~at <player.name>'s footprints."), MessageBulb.NotificationTypes.Question)
                Case 16
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like17", "<name> is staring~straight into <player.name>'s~eyes."), MessageBulb.NotificationTypes.Friendly)
                Case 17
                    If p.BaseSpeed >= 100 Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like18", "<name> is showing off~its agility!"), MessageBulb.NotificationTypes.Note)
                    End If
                Case 18
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like19", "<name> is moving~around happily!"), MessageBulb.NotificationTypes.Note)
                Case 19
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like20", "<name> is steadily~keeping up with you!"), MessageBulb.NotificationTypes.Friendly)
                Case 20
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like21", "<name> seems to~want to play with~<player.name>!"), MessageBulb.NotificationTypes.Note)
                Case 21
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like22", "<name> is singing~and humming."), MessageBulb.NotificationTypes.Note)
                Case 22
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like23", "<name> is playfully~nibbling at the ground."), MessageBulb.NotificationTypes.Friendly)
                Case 23
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like24", "<name> is nipping~at your feet!"), MessageBulb.NotificationTypes.Note)
                Case 24
                    If p.BaseAttack >= 100 Or p.BaseSpAttack >= 100 Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like25", "<name> is working~hard to show off~its mighty power!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 25
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like26", "<name> is cheerful!"), MessageBulb.NotificationTypes.Note)
                Case 26
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like27", "<name> bumped~into <player.name>!"), MessageBulb.NotificationTypes.Exclamation)
                Case 27
                    If IsCave() = True And p.Level < 20 Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Like28", "<name> is scared~and snuggled up~to <player.name>!"), MessageBulb.NotificationTypes.Exclamation)
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
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal1", "<name> began poking~you in the stomach!"), MessageBulb.NotificationTypes.CatFace)
                Case 1
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal2", "<name> seems to be~feeling great about~walking with you!"), MessageBulb.NotificationTypes.Heart)
                Case 2
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal3", "<name> is still~feeling great!"), MessageBulb.NotificationTypes.Happy)
                Case 3
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal4", "<name> is poking~at your belly."), MessageBulb.NotificationTypes.Heart)
                Case 4
                    If p.Level > 30 Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal5", "<name> looks like~it wants to lead!"), MessageBulb.NotificationTypes.Note)
                    End If
                Case 5
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal6", "<name> seems to be~very happy!"), MessageBulb.NotificationTypes.Happy)
                Case 6
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal7", "<name> nodded slowly."), MessageBulb.NotificationTypes.Friendly)
                Case 7
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal8", "<name> gave you~a sunny look!"), MessageBulb.NotificationTypes.Happy)
                Case 8
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal9", "<name> is very~composed and sure of itself!"), MessageBulb.NotificationTypes.Waiting)
                Case 9
                    If p.BaseDefense >= 100 Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal10", "<name> is~standing guard!"), MessageBulb.NotificationTypes.Exclamation)
                    End If
                Case 10
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal11", "<name> danced a~wonderful dance!"), MessageBulb.NotificationTypes.Note)
                Case 11
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal12", "<name> is staring~steadfastly at~<player.name>'s face."), MessageBulb.NotificationTypes.Waiting)
                Case 12
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal13", "<name> is staring~intently at~<player.name>'s face."), MessageBulb.NotificationTypes.Waiting)
                Case 13
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal14", "<name> is concentrating."), MessageBulb.NotificationTypes.Unsure)
                Case 14
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal15", "<name> faced this~way and nodded."), MessageBulb.NotificationTypes.Friendly)
                Case 15
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal16", "<name> suddenly~started walking closer!"), MessageBulb.NotificationTypes.Heart)
                Case 16
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal17", "Woah!*<name> is suddenly~playful!"), MessageBulb.NotificationTypes.Heart)
                Case 17
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal18", "<name> blushes."), MessageBulb.NotificationTypes.Happy)
                Case 18
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal19", "Woah!*<name> suddenly started~dancing in happiness!"), MessageBulb.NotificationTypes.Note)
                Case 19
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal20", "<name> is happily~skipping about."), MessageBulb.NotificationTypes.Note)
                Case 20
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Loyal21", "Woah!*<name> suddenly~danced in happiness!"), MessageBulb.NotificationTypes.Note)
            End Select
        End While
        Return r
    End Function

    Private Shared Function GetLoveReaction(ByVal p As Pokemon) As ReactionContainer
        Dim r As ReactionContainer = Nothing
        While r Is Nothing
            Select Case Core.Random.Next(0, 13)
                Case 0
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love1", "<name> is jumping~for joy!"), MessageBulb.NotificationTypes.Heart)
                Case 1
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love2", "Your Pokémon stretched out~its body and is relaxing."), MessageBulb.NotificationTypes.Happy)
                Case 2
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love3", "<name> is happily~cuddling up to you!"), MessageBulb.NotificationTypes.Heart)
                Case 3
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love4", "<name> is so happy~that it can't stand still!"), MessageBulb.NotificationTypes.Heart)
                Case 4
                    If p.PokedexEntry.Height <= 1.6F Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love5", "<name> happily~cuddled up to you!"), MessageBulb.NotificationTypes.Heart)
                    End If
                Case 5
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love6", "<name>'s cheeks are~becoming rosy!"), MessageBulb.NotificationTypes.Heart)
                Case 6
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love7", "Woah!*<name> suddenly~hugged you!"), MessageBulb.NotificationTypes.Heart)
                Case 7
                    If p.PokedexEntry.Height <= 0.7F Then
                        r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love8", "<name> is rubbing~against your legs!"), MessageBulb.NotificationTypes.Heart)
                    End If
                Case 8
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love9", "Ah!~<name> cuddles you!"), MessageBulb.NotificationTypes.Heart)
                Case 9
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love10", "<name> is regarding~you with adoration!"), MessageBulb.NotificationTypes.Heart)
                Case 10
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love11", "<name> got~closer to <player.name>!"), MessageBulb.NotificationTypes.Heart)
                Case 11
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love12", "<name> is keeping~close to your feet."), MessageBulb.NotificationTypes.Heart)
                Case 12
                    r = New ReactionContainer(Localization.GetString("FollowerInteraction_Love13", "<name> is jumping~around in a carefree way!"), MessageBulb.NotificationTypes.Note)
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
        Public Notification As MessageBulb.NotificationTypes = MessageBulb.NotificationTypes.AFK
        Public EmojiString As String = ""
        Public HasNotification As Boolean = True
        Public MapFiles As New List(Of String)
        Public PokemonIDs As New List(Of String)
        Public ExcludeIDs As New List(Of String)
        Public Daytime As Integer = -1
        Public Weather As Integer = -1
        Public Season As Integer = -1
        Public Types As New List(Of Integer)
        Public Probability As Integer = 100

        Public Sub New(ByVal dataLine As String)
            dataLine = dataLine.Remove(dataLine.Length - 1, 1).Remove(0, 1)

            Dim dataParts() As String = dataLine.Split(CChar("|"))

            Me.MapFiles = dataParts(0).Split(CChar(",")).ToList()

            If dataParts(1) <> "-1" Then
                For Each pokePart As String In dataParts(1).Split(CChar(","))
                    Dim lReference As List(Of String) = PokemonIDs
                    If pokePart.StartsWith("!") = True Then
                        pokePart = pokePart.Remove(0, 1)
                        lReference = ExcludeIDs
                    End If
                    If lReference.Contains(pokePart) = False Then
                        lReference.Add(pokePart)
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
                    Me.Types.Add(BattleSystem.GameModeElementLoader.GetElementByName(typePart).Type)
                Next
            End If

            Me.Probability = CInt(dataParts(6))

            If dataParts(7) = "-1" Then
                Me.HasNotification = False
            Else
                Me.HasNotification = True
                Dim EmojiText As String = dataParts(7).Replace(">:(", "shouting").Replace("<3", "heart").Replace(":(", "unhappy").Replace(":)", "friendly").Replace(";)", "wink").Replace("/:(", "angry")
                EmojiString = EmojiText
                If Me.EmojiString.Contains("<") = False AndAlso Me.EmojiString.Contains(">") = False Then
                    Me.Notification = Me.ConvertEmoji(ScriptVersion2.ScriptCommander.Parse(EmojiText).ToString())
                End If
            End If

            Me.Message = dataParts(8)
        End Sub

        Public Sub New(ByVal Message As String, ByVal Notification As MessageBulb.NotificationTypes)
            Me.Message = Message
            Me.Notification = Notification
        End Sub

        Private Function ConvertEmoji(ByVal s As String) As MessageBulb.NotificationTypes
            Select Case s.ToLower()
                Case "..."
                    Return MessageBulb.NotificationTypes.Waiting
                Case "!"
                    Return MessageBulb.NotificationTypes.Exclamation
                Case ">:(", "shouting"
                    Return MessageBulb.NotificationTypes.Shouting
                Case "?"
                    Return MessageBulb.NotificationTypes.Question
                Case "note"
                    Return MessageBulb.NotificationTypes.Note
                Case "<3", "heart"
                    Return MessageBulb.NotificationTypes.Heart
                Case ":(", "unhappy"
                    Return MessageBulb.NotificationTypes.Unhappy
                Case "ball"
                    Return MessageBulb.NotificationTypes.Battle
                Case ":d"
                    Return MessageBulb.NotificationTypes.Happy
                Case ":)", "friendly"
                    Return MessageBulb.NotificationTypes.Friendly
                Case "bad"
                    Return MessageBulb.NotificationTypes.Poisoned
                Case ";)", "wink"
                    Return MessageBulb.NotificationTypes.Wink
                Case "afk"
                    Return MessageBulb.NotificationTypes.AFK
                Case "/:(", "angry"
                    Return MessageBulb.NotificationTypes.Angry
                Case ":3"
                    Return MessageBulb.NotificationTypes.CatFace
                Case ":/"
                    Return MessageBulb.NotificationTypes.Unsure
            End Select

            Return MessageBulb.NotificationTypes.Waiting
        End Function

        Public Function Match(ByVal p As Pokemon) As Boolean
            If MapFiles.Count > 0 Then
                If MapFiles.Contains(Screen.Level.LevelFile.ToLowerInvariant()) = False Then
                    Return False
                End If
            End If

            If PokemonIDs.Count > 0 Then
                Dim dexID As String = p.Number.ToString
                If p.AdditionalData <> "" Then
                    dexID = PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True)
                End If
                If PokemonIDs.Contains(dexID) = False Then
                    Return False
                End If
            End If

            If ExcludeIDs.Count > 0 Then
                Dim dexID As String = p.Number.ToString
                If p.AdditionalData <> "" Then
                    dexID = PokemonForms.GetPokemonDataFileName(p.Number, p.AdditionalData, True)
                End If
                If ExcludeIDs.Contains(dexID) = True Then
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
                For Each t As Integer In Me.Types
                    If p.IsType(t) = False Then
                        Return False
                    End If
                Next
            End If

            Return True
        End Function

        Public Function GetMessage(ByVal p As Pokemon) As String
            Return ScriptVersion2.ScriptCommander.Parse(Me.Message.Replace("<name>", p.GetDisplayName()).Replace("<item>", p.Item.OneLineName())).ToString.Replace("//POKEMONNAME//", p.GetDisplayName()).Replace("//ITEM//", p.Item.OneLineName())
        End Function

        Public Function GetNotification() As MessageBulb.NotificationTypes
            If EmojiString <> "" AndAlso EmojiString.Contains("<") = True AndAlso EmojiString.Contains(">") = True Then
                Return ConvertEmoji(ScriptVersion2.ScriptCommander.Parse(Me.EmojiString).ToString.Replace(">:(", "shouting").Replace("<3", "heart").Replace(":(", "unhappy").Replace(":)", "friendly").Replace(";)", "wink").Replace("/:(", "angry"))
            Else
                Return Me.Notification
            End If

        End Function
    End Class

    Shared PickupIndividualValue As String = "" 'This value holds the individual value of the Pokémon that picked up the item.
    Shared PickupItemID As String = "-1" 'This is the Item ID of the item that the Pokémon picked up. -1 means no item got picked up.

    Public Shared Sub CheckForRandomPickup()
        'Checks if the first Pokémon in the party is following the player:
        If Screen.Level.ShowOverworldPokemon = True And CBool(GameModeManager.GetGameRuleValue("ShowFollowPokemon", "1")) = True AndAlso CBool(GameModeManager.GetGameRuleValue("RandomFollowItemPickup", "1")) = True Then
            'Checks if the player has a Pokémon:
            If Core.Player.Pokemons.Count > 0 And Screen.Level.Surfing = False And Screen.Level.Riding = False And Screen.Level.ShowOverworldPokemon = True And Not Core.Player.GetWalkPokemon() Is Nothing Then
                If Core.Player.GetWalkPokemon().Status = Pokemon.StatusProblems.None Then
                    'If the player switched the Pokémon, reset the item ID.
                    If PickupIndividualValue <> Core.Player.GetWalkPokemon().IndividualValue Then
                        PickupItemID = "-1"
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
                            Logger.Debug("Pokémon picks up item (" & Item.GetItemByID(newItemID.ToString).Name & ")")
                            PickupItemID = newItemID.ToString
                            PickupIndividualValue = Core.Player.GetWalkPokemon().IndividualValue
                            SoundManager.PlaySound("pickup")
                        End If
                    End If
                End If
            Else
                'Reset the system if no Pokémon:
                PickupItemID = "-1"
                PickupIndividualValue = ""
            End If
        End If
    End Sub

#Region "Checks"

    Private Shared Function IsOutside() As Boolean
        If Screen.Level.IsOutside = True OrElse Screen.Level.CanFly = True And Screen.Level.CanDig = False And Screen.Level.CanTeleport = True Then
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
