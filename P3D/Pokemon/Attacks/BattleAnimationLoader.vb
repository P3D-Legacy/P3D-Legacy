Namespace BattleSystem

    ''' <summary>
    ''' Provides an interface to load additional GameMode moves.
    ''' </summary>
    Public Class BattleAnimationLoader

        'The default relative path to load moves from (Content folder).
        Const PATH As String = "Data\BattleAnimations\"

        Public UserPokemonAnimation As List(Of QueryObject)
        Public OpponentPokemonAnimation As List(Of QueryObject)
        Public FailPokemonAnimation As List(Of QueryObject)

        Public TempAnimation As AnimationQueryObject

        Public BattleFlip As Boolean = False

        Public Sub New(ByVal BattleFlipped As Boolean)
            Me.BattleFlip = BattleFlip
        End Sub
        Public Function MoveAnimationExists(MoveID As Integer) As Boolean
            If MoveID <> -1 Then
                Dim ScriptPath As String = GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & "\" & PATH & MoveID.ToString
                If System.IO.File.Exists(ScriptPath & ".dat") = True Then
                    Return True
                End If
            End If
            Return False
        End Function
        ''' <summary>
        ''' Clears the old animations and loads the animations for the currently set move
        Public Sub LoadMoveAnimation(MoveID As Integer)
            If MoveID <> -1 Then
                Me.Clear()
                Dim ScriptPath As String = GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & "\" & PATH & MoveID.ToString
                If System.IO.File.Exists(ScriptPath & ".dat") = True Then
                    CType(CurrentScreen, BattleScreen).ActionScript.StartScript(ScriptPath, 0, False, False, "LoadMoveAnimation", False)
                End If
            End If

        End Sub

        Public Sub SetQuery(AnimationType As String)
            Select Case AnimationType.ToLower
                Case "own"
                    UserPokemonAnimation.Add(TempAnimation)
                Case "opp"
                    OpponentPokemonAnimation.Add(TempAnimation)
                Case "fail"
                    FailPokemonAnimation.Add(TempAnimation)
            End Select
        End Sub

        Public Sub Clear()
            TempAnimation = Nothing
            UserPokemonAnimation.Clear()
            OpponentPokemonAnimation.Clear()
            FailPokemonAnimation.Clear()
        End Sub
    End Class

End Namespace