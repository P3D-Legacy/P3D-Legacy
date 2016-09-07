Public Class Nature

    Private Enum StatNames
        Attack
        Defense
        SpAttack
        SpDefense
        Speed
    End Enum

    Public Shared Function GetMultiplier(ByVal Nature As Pokemon.Natures, ByVal StatName As String) As Single
        Dim stat As StatNames = StatNames.Attack

        Select Case StatName.ToLower()
            Case "attack", "atk"
                Stat = StatNames.Attack
            Case "defense", "def"
                Stat = StatNames.Defense
            Case "spattack", "spatk", "specialattack"
                Stat = StatNames.SpAttack
            Case "spdefense", "spdef", "specialdefense"
                Stat = StatNames.SpDefense
            Case "speed"
                Stat = StatNames.Speed
        End Select

        Select Case Nature
            Case Pokemon.Natures.Hardy
                Return 1
            Case Pokemon.Natures.Lonely
                Return CalcMulti(stat, StatNames.Attack, StatNames.Defense)
            Case Pokemon.Natures.Brave
                Return CalcMulti(stat, StatNames.Attack, StatNames.Speed)
            Case Pokemon.Natures.Adamant
                Return CalcMulti(stat, StatNames.Attack, StatNames.SpAttack)
            Case Pokemon.Natures.Naughty
                Return CalcMulti(stat, StatNames.Attack, StatNames.SpDefense)
            Case Pokemon.Natures.Bold
                Return CalcMulti(stat, StatNames.Defense, StatNames.Attack)
            Case Pokemon.Natures.Docile
                Return 1
            Case Pokemon.Natures.Relaxed
                Return CalcMulti(stat, StatNames.Defense, StatNames.Speed)
            Case Pokemon.Natures.Impish
                Return CalcMulti(stat, StatNames.Defense, StatNames.SpAttack)
            Case Pokemon.Natures.Lax
                Return CalcMulti(stat, StatNames.Defense, StatNames.SpDefense)
            Case Pokemon.Natures.Timid
                Return CalcMulti(stat, StatNames.Speed, StatNames.Attack)
            Case Pokemon.Natures.Hasty
                Return CalcMulti(stat, StatNames.Speed, StatNames.Defense)
            Case Pokemon.Natures.Jolly
                Return CalcMulti(stat, StatNames.Speed, StatNames.SpAttack)
            Case Pokemon.Natures.Naive
                Return CalcMulti(stat, StatNames.Speed, StatNames.SpDefense)
            Case Pokemon.Natures.Modest
                Return CalcMulti(stat, StatNames.SpAttack, StatNames.Attack)
            Case Pokemon.Natures.Mild
                Return CalcMulti(stat, StatNames.SpAttack, StatNames.Defense)
            Case Pokemon.Natures.Quiet
                Return CalcMulti(stat, StatNames.SpAttack, StatNames.Speed)
            Case Pokemon.Natures.Bashful
                Return 1
            Case Pokemon.Natures.Rash
                Return CalcMulti(stat, StatNames.SpAttack, StatNames.SpDefense)
            Case Pokemon.Natures.Calm
                Return CalcMulti(stat, StatNames.SpDefense, StatNames.Attack)
            Case Pokemon.Natures.Gentle
                Return CalcMulti(stat, StatNames.SpDefense, StatNames.Defense)
            Case Pokemon.Natures.Sassy
                Return CalcMulti(stat, StatNames.SpDefense, StatNames.Speed)
            Case Pokemon.Natures.Careful
                Return CalcMulti(stat, StatNames.SpDefense, StatNames.SpAttack)
            Case Pokemon.Natures.Quirky
                Return 1
            Case Pokemon.Natures.Serious
                Return 1
            Case Else
                Return 1
        End Select
    End Function

    Private Shared Function CalcMulti(ByVal calcStat As StatNames, ByVal PositiveStat As StatNames, ByVal NegativeStat As StatNames) As Single
        If calcStat = PositiveStat Then
            Return 1.1F
        ElseIf calcStat = NegativeStat Then
            Return 0.9F
        Else
            Return 1
        End If
    End Function

End Class