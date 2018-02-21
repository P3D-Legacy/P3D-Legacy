Namespace BattleSystem.Moves.Normal

    Public Class PerishSong

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 195
            Me.OriginalPP = 5
            Me.CurrentPP = 5
            Me.MaxPP = 5
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Perish Song"
            Me.Description = "Any Pokémon that hears this song faints in three turns, unless it switches out of battle."
            Me.CriticalChance = 0
            Me.IsHMMove = False
            Me.Target = Targets.All
            Me.Priority = 0
            Me.TimesToAttack = 1
            '#End

            '#SpecialDefinitions
            Me.MakesContact = False
            Me.ProtectAffected = False
            Me.MagicCoatAffected = False
            Me.SnatchAffected = False
            Me.MirrorMoveAffected = False
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
            Me.IsSoundMove = True

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End

            Me.AIField1 = AIField.Support
            Me.AIField2 = AIField.Nothing
        End Sub

        Public Overrides Sub MoveHits(own As Boolean, BattleScreen As BattleScreen)
            Dim userPerishSongCount As Integer = 0
            If own = True Then
                userPerishSongCount = BattleScreen.FieldEffects.OwnPerishSongCount
            Else
                userPerishSongCount = BattleScreen.FieldEffects.OppPerishSongCount
            End If

            If userPerishSongCount = 0 Then
                BattleScreen.FieldEffects.OwnPerishSongCount = 4
                BattleScreen.FieldEffects.OppPerishSongCount = 4
                BattleScreen.BattleQuery.Add(New TextQueryObject("All Pokémon hearing the song will faint in three turns!"))
            Else
                BattleScreen.BattleQuery.Add(New TextQueryObject(Me.Name & " failed!"))
            End If
        End Sub

    End Class

End Namespace