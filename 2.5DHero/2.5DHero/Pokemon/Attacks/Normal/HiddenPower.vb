Namespace BattleSystem.Moves.Normal

    Public Class HiddenPower

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 237
            Me.OriginalPP = 15
            Me.CurrentPP = 15
            Me.MaxPP = 15
            Me.Power = 70
            Me.Accuracy = 100
            Me.Category = Categories.Special
            Me.ContestCategory = ContestCategories.Smart
            Me.Name = "Hidden Power"
            Me.Description = "A unique attack that varies in type and intensity depending on the Pokémon using it."
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

            If p.Ability.Name.ToLower() = "normalize" Then
                Return New Element(Element.Types.Normal)
            Else
                Dim a As Integer = GetLastBit(p.IVHP)
                Dim b As Integer = GetLastBit(p.IVAttack)
                Dim c As Integer = GetLastBit(p.IVDefense)
                Dim d As Integer = GetLastBit(p.IVSpeed)
                Dim e As Integer = GetLastBit(p.IVSpAttack)
                Dim f As Integer = GetLastBit(p.IVSpDefense)

                Dim t As Integer = CInt(Math.Floor(((a + 2 * b + 4 * c + 8 * d + 16 * e + 32 * f) * 15) / 63))

                Select Case t
                    Case 0
                        Return New Element(Element.Types.Fighting)
                    Case 1
                        Return New Element(Element.Types.Flying)
                    Case 2
                        Return New Element(Element.Types.Poison)
                    Case 3
                        Return New Element(Element.Types.Ground)
                    Case 4
                        Return New Element(Element.Types.Rock)
                    Case 5
                        Return New Element(Element.Types.Bug)
                    Case 6
                        Return New Element(Element.Types.Ghost)
                    Case 7
                        Return New Element(Element.Types.Steel)
                    Case 8
                        Return New Element(Element.Types.Fire)
                    Case 9
                        Return New Element(Element.Types.Water)
                    Case 10
                        Return New Element(Element.Types.Grass)
                    Case 11
                        Return New Element(Element.Types.Electric)
                    Case 12
                        Return New Element(Element.Types.Psychic)
                    Case 13
                        Return New Element(Element.Types.Ice)
                    Case 14
                        Return New Element(Element.Types.Dragon)
                    Case 15
                        Return New Element(Element.Types.Dark)
                End Select
            End If

            Return Me.Type
        End Function

        Private Function GetLastBit(ByVal i As Integer) As Integer
            Return i Mod 2
        End Function

    End Class

End Namespace