Namespace BattleSystem

    Public Class LearnMovesQueryObject

        Inherits QueryObject

        Dim p As Pokemon
        Dim a As BattleSystem.Attack

        Shared AddedAttacks As Integer = 0

        Dim hasAttack As Boolean = False

        Public Sub New(ByVal p As Pokemon, ByVal a As BattleSystem.Attack, ByVal BV2Screen As BattleScreen)
            MyBase.New(QueryTypes.LearnMoves)

            Me.p = p
            Me.a = a

            For Each Attack As BattleSystem.Attack In p.Attacks
                If Attack.ID = a.ID Then
                    Me.hasAttack = True
                    Exit For
                End If
            Next

            If p.Attacks.Count + AddedAttacks < 4 And hasAttack = False Then
                AddedAttacks += 1
                BV2Screen.BattleQuery.Add(New PlaySoundQueryObject("success_small", False))
                BV2Screen.BattleQuery.Add(New TextQueryObject(p.GetDisplayName() & " learned " & a.Name & "!"))
                PlayerStatistics.Track("Moves learned", 1)
            End If
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If hasAttack = False Then
                If p.Attacks.Count < 4 Then
                    p.Attacks.Add(a)
                Else
                    Core.SetScreen(New LearnAttackScreen(Core.CurrentScreen, p, a))
                End If
            End If
        End Sub

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return True
            End Get
        End Property

        Public Shared Sub ClearCache()
            AddedAttacks = 0
        End Sub

    End Class

End Namespace