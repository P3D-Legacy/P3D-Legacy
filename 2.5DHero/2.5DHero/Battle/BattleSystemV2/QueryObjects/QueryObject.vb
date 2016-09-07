Namespace BattleSystem

    Public MustInherit Class QueryObject

        Public Enum QueryTypes
            CameraMovement
            Textbox
            ToggleMenu
            ToggleEntity
            MathHP
            Delay
            EndBattle
            PlayMusic
            ChoosePokemon
            ScreenFade
            LearnMoves
            InflictStatus
            ChangeHP
            SwitchPokemon
            TriggerNewRound
            RoamingPokemonFled
            DisplayLevelUp
            PlaySound
            MoveAnimation
        End Enum

        Public QueryType As QueryTypes = QueryTypes.CameraMovement
        Public PassThis As Boolean = False

        Public Pokemon As Pokemon = Nothing
        Public RemoveFainted As Boolean = False

        Public Sub New(ByVal QueryType As QueryTypes)
            Me.QueryType = QueryType
        End Sub

        Public Overridable ReadOnly Property IsReady() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overridable Sub Update(ByVal BV2Screen As BattleScreen)
            'DO NOTHING
        End Sub

        Public Overridable Sub Draw(ByVal BV2Screen As BattleScreen)
            'DO NOTHING
        End Sub

        Public Overridable ReadOnly Property UpdateCamera() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overridable Function NeedForPVPData() As Boolean
            Return False
        End Function

        Public Shared Function FromString(ByVal input As String) As QueryObject
            If input.StartsWith("{") = True And input.EndsWith("}") = True Then
                input = input.Remove(input.Length - 1, 1).Remove(0, 1)

                Dim Type As String = input.Remove(input.IndexOf("|"))
                Dim Data As String = input.Remove(0, input.IndexOf("|") + 1)
                Select Case Type
                    Case "CAMERA"
                        Return CameraQueryObject.FromString(Data)
                    Case "DELAY"
                        Return DelayQueryObject.FromString(Data)
                    Case "ENDBATTLE"
                        Return EndBattleQueryObject.FromString(Data)
                    Case "MATHHP"
                        Return MathHPQueryObject.FromString(Data)
                    Case "MUSIC"
                        Return PlayMusicQueryObject.FromString(Data)
                    Case "SOUND"
                        Return PlaySoundQueryObject.FromString(Data)
                    Case "FADE"
                        Return ScreenFadeQueryObject.FromString(Data)
                    Case "TEXT"
                        Return TextQueryObject.FromString(Data)
                    Case "TOGGLEENTITY"
                        Return ToggleEntityQueryObject.FromString(Data)
                    Case "TOGGLEMENU"
                        Return ToggleMenuQueryObject.FromString(Data)
                    Case "TRIGGERNEWROUNDPVP"
                        Return TriggerNewRoundPVPQueryObject.FromString(Data)
                End Select
            End If
            Return Nothing
        End Function

        Public Overrides Function ToString() As String
            Return ""
        End Function

    End Class

End Namespace