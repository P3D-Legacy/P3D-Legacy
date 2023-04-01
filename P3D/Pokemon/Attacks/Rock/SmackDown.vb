Namespace BattleSystem.Moves.Rock

    Public Class SmackDown

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Rock)
            Me.ID = 479
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 50
            Me.Accuracy = 100
            Me.Category = Categories.Physical
            Me.ContestCategory = ContestCategories.Tough
            Me.Name = Localization.GetString("move_name_" & Me.ID,"Smack Down")
            Me.Description = "The user throws a stone or similar projectile to attack an opponent. A flying Pokémon will fall to the ground when it's hit."
            Me.CriticalChance = 1
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = True
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = True
            Me.CounterAffected = True

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesOwnFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False

            Me.IsDamagingMove = True
            Me.IsProtectMove = False


            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            Me.CanHitInMidAir = True
            '#End
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim op As Pokemon = BattleScreen.OppPokemon
            If own = False Then
                op = BattleScreen.OwnPokemon
            End If

            If own = True Then
                If BattleScreen.FieldEffects.OppSmacked = 0 Then
                    BattleScreen.FieldEffects.OppSmacked = 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " fell straight down."))
                End If
            Else
                If BattleScreen.FieldEffects.OwnSmacked = 0 Then
                    BattleScreen.FieldEffects.OwnSmacked = 1
                    BattleScreen.BattleQuery.Add(New TextQueryObject(op.GetDisplayName() & " fell straight down."))
                End If
            End If
        End Sub

    End Class

End Namespace