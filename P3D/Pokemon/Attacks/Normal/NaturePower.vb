Namespace BattleSystem.Moves.Normal

    Public Class NaturePower

        Inherits Attack

        Public Sub New()
            '#Definitions
            Me.Type = New Element(Element.Types.Normal)
            Me.ID = 267
            Me.OriginalPP = 20
            Me.CurrentPP = 20
            Me.MaxPP = 20
            Me.Power = 0
            Me.Accuracy = 0
            Me.Category = Categories.Status
            Me.ContestCategory = ContestCategories.Beauty
            Me.Name = "Nature Power"
            Me.Description = "An attack that makes use of nature’s power. Its effects vary depending on the user’s environment."
            Me.CriticalChance = 0
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

            Me.IsAffectedBySubstitute = False
            Me.IsOneHitKOMove = False
            Me.IsWonderGuardAffected = False
            '#End
        End Sub

        Public Shared Function GetMoveID(own As Boolean, Battlescreen As BattleScreen) As Integer
            If Battlescreen.FieldEffects.ElectricTerrain > 0 Then
                Return 85
            ElseIf Battlescreen.FieldEffects.GrassyTerrain > 0 Then
                Return 412
            ElseIf Battlescreen.FieldEffects.MistyTerrain > 0 Then
                Return 585
            ElseIf Battlescreen.FieldEffects.PsychicTerrain > 0 Then
                Return 94
            Else
                Select Case Screen.Level.Terrain.TerrainType
                    Case Terrain.TerrainTypes.Plain
                        Return 161
                    Case Terrain.TerrainTypes.Cave
                        Return 247
                    Case Terrain.TerrainTypes.DisortionWorld
                        Return 185
                    Case Terrain.TerrainTypes.LongGrass
                        Return 75
                    Case Terrain.TerrainTypes.Magma
                        Return 172
                    Case Terrain.TerrainTypes.PondWater
                        Return 61
                    Case Terrain.TerrainTypes.Puddles
                        Return 426
                    Case Terrain.TerrainTypes.Rock
                        Return 157
                    Case Terrain.TerrainTypes.Sand
                        Return 89
                    Case Terrain.TerrainTypes.SeaWater
                        Return 56
                    Case Terrain.TerrainTypes.Snow
                        Return 58
                    Case Terrain.TerrainTypes.TallGrass
                        Return 402
                    Case Terrain.TerrainTypes.Underwater
                        Return 291
                End Select
                Return 89
            End If
        End Function

    End Class

End Namespace