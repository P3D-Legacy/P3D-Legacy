Namespace BattleSystem.Moves.Normal

    Public Class Judgement

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 449
            Me.OriginalPP = 10
            Me.CurrentPP = 10
            Me.MaxPP = 10
            Me.Power = 100
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Judgement"
            Me.Description = "The user releases countless shots of light at the target. This move's type varies depending on the kind of Plate the user is holding."
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
            Me.CounterAffected = False

            Me.DisabledWhileGravity = False
            Me.UseEffectiveness = True
            Me.ImmunityAffected = True
            Me.HasSecondaryEffect = False
            Me.RemovesFrozen = False

            Me.IsHealingMove = False
            Me.IsRecoilMove = False
            Me.IsPunchingMove = False
            Me.IsDamagingMove = True
            Me.IsProtectMove = False
            Me.IsSoundMove = False

            Me.IsAffectedBySubstitute = True
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = True
            '#End
        End Sub

        Public Overrides Function GetAttackType(own As Boolean, BattleScreen As BattleScreen) As Element
            Dim p As Pokemon = BattleScreen.OwnPokemon
            If own = False Then
                p = BattleScreen.OppPokemon
            End If

            Dim itemID As Integer = 0
            If Not p.Item Is Nothing Then
                itemID = p.Item.ID
            Else
                Return New Element(Element.Types.Normal)
            End If

            Select Case p.Item.ID
                Case 267
                    Return New Element(Element.Types.Dragon)
                Case 268
                    Return New Element(Element.Types.Dark)
                Case 269
                    Return New Element(Element.Types.Ground)
                Case 270
                    Return New Element(Element.Types.Fighting)
                Case 271
                    Return New Element(Element.Types.Fire)
                Case 272
                    Return New Element(Element.Types.Ice)
                Case 273
                    Return New Element(Element.Types.Bug)
                Case 274
                    Return New Element(Element.Types.Steel)
                Case 275
                    Return New Element(Element.Types.Grass)
                Case 276
                    Return New Element(Element.Types.Psychic)
                Case 277
                    Return New Element(Element.Types.Fairy)
                Case 278
                    Return New Element(Element.Types.Flying)
                Case 279
                    Return New Element(Element.Types.Water)
                Case 280
                    Return New Element(Element.Types.Ghost)
                Case 281
                    Return New Element(Element.Types.Rock)
                Case 282
                    Return New Element(Element.Types.Poison)
                Case 283
                    Return New Element(Element.Types.Electric)
                Case Else
                    Return New Element(Element.Types.Normal)
            End Select

            Return Me.Type
        End Function

    End Class

End Namespace