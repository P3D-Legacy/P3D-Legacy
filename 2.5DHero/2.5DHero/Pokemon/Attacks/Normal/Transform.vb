Namespace BattleSystem.Moves.Normal

    Public Class Transform

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 144
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Transform"
            Me.Description = "The user transforms into a copy of the target right down to having the same move set."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.OneAdjacentTarget
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = True
            Me.KingsrockAffected = False
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = False
            Me.ImmunityAffected = False
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = False
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            Me.FocusOppPokemon = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            'Changes: Type, stats (except HP), stat modifications, moveset, species, shiny, ability, additionalvalue
            'Set istransformed to true
            'fail if target is transformed
            'apply image to sprite after transform

            Dim p As Pokemon = BattleScreen.OwnPokemon
            Dim op As Pokemon = BattleScreen.OppPokemon

            If own = False Then
                p = BattleScreen.OppPokemon
                op = BattleScreen.OwnPokemon
            End If

            If op.IsTransformed = False Then
                'Save old stats:
                p.OriginalNumber = p.Number
                p.OriginalType1 = New Element(p.Type1.Type)
                p.OriginalType2 = New Element(p.Type2.Type)
                p.OriginalStats = {p.Attack, p.Defense, p.SpAttack, p.SpDefense, p.Speed}
                p.OriginalShiny = CInt(p.IsShiny.ToNumberString())
                p.OriginalMoves = New List(Of BattleSystem.Attack)
                p.OriginalMoves.AddRange(p.Attacks.ToArray())
                p.OriginalAbility = Ability.GetAbilityByID(p.Ability.ID)

                'Apply new stats:
                p.Number = op.Number

                p.Type1 = New Element(op.Type1.Type)
                p.Type2 = New Element(op.Type2.Type)

                p.Attack = op.Attack
                p.Defense = op.Defense
                p.SpAttack = op.SpAttack
                p.SpDefense = op.SpDefense
                p.Speed = op.Speed

                p.StatAttack = op.StatAttack
                p.StatDefense = op.StatDefense
                p.StatSpAttack = op.StatSpAttack
                p.StatSpDefense = op.StatSpDefense
                p.StatSpeed = op.StatSpeed

                p.IsShiny = op.IsShiny

                p.Attacks.Clear()
                p.Attacks.AddRange(op.Attacks.ToArray())

                p.Ability = Ability.GetAbilityByID(op.Ability.ID)

                p.IsTransformed = True

                'Apply new image to sprite:
                BattleScreen.BattleQuery.Add(New ToggleEntityQueryObject(own, ToggleEntityQueryObject.BattleEntities.OwnPokemon, PokemonForms.GetOverworldSpriteName(p), 0, 1, -1, -1))
                BattleScreen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " transformed into " & op.OriginalName & "!"))
            Else
                'Fails
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace