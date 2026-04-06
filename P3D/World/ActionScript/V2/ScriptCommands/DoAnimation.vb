Imports P3D.BattleSystem

Namespace ScriptVersion2

    Partial Class ScriptCommander

        ' --------------------------------------------------------------------------------------------------------------------------
        ' Contains the @system commands.
        ' --------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoAnimation(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "start"
                    Dim UsePokemonOrigin As Boolean = CBool(argument.GetSplit(0, ","))
                    Dim BackgroundAnimation As Boolean = False
                    Dim OwnOrOpp As Boolean = False

                    Dim AnimationObject As AnimationQueryObject

                    If argument.Split(",").Count > 1 AndAlso argument.GetSplit(1, ",") <> "" Then
                        BackgroundAnimation = CBool(argument.GetSplit(2, ","))
                    End If

                    If argument.Split(",").Count > 2 AndAlso argument.GetSplit(2, ",") <> "" Then
                        OwnOrOpp = CBool(argument.GetSplit(1, ","))
                    End If
                    If UsePokemonOrigin = True Then
                        If OwnOrOpp = False Then
                            AnimationObject = New AnimationQueryObject(CType(CurrentScreen, BattleScreen).OwnPokemonNPC, BattleScreen.BattleAnimationLoaders(BattleScreen.BattleAnimationLoaderIndex).BattleFlip, BackgroundAnimation)
                        Else
                            AnimationObject = New AnimationQueryObject(CType(CurrentScreen, BattleScreen).OppPokemonNPC, BattleScreen.BattleAnimationLoaders(BattleScreen.BattleAnimationLoaderIndex).BattleFlip, BackgroundAnimation)
                        End If
                    Else
                        AnimationObject = New AnimationQueryObject(Nothing, BattleScreen.BattleAnimationLoaders(BattleScreen.BattleAnimationLoaderIndex).BattleFlip, BackgroundAnimation)
                    End If
                    BattleScreen.BattleAnimationLoaders(BattleSystem.BattleScreen.BattleAnimationLoaderIndex).TempAnimation = AnimationObject
                Case "end"
                    Dim AnimationType As String = argument '"own", "opp", or "fail"
                    BattleScreen.BattleAnimationLoaders(BattleSystem.BattleScreen.BattleAnimationLoaderIndex).SetQuery(AnimationType)

            End Select

            IsReady = True
        End Sub

    End Class

End Namespace