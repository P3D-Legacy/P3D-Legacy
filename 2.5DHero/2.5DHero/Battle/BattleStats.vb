Public Class BattleStats

    Public Shared Function GetStatImage(ByVal status As Pokemon.StatusProblems) As Texture2D
        Dim r As Rectangle = New Rectangle(0, 0, 0, 0)

        Select Case status
            Case Pokemon.StatusProblems.BadPoison, Pokemon.StatusProblems.Poison
                r = New Rectangle(96, 18, 19, 6)
            Case Pokemon.StatusProblems.Burn
                r = New Rectangle(96, 0, 19, 6)
            Case Pokemon.StatusProblems.Fainted
                r = New Rectangle(96, 30, 19, 6)
            Case Pokemon.StatusProblems.Freeze
                r = New Rectangle(96, 12, 19, 6)
            Case Pokemon.StatusProblems.Paralyzed
                r = New Rectangle(96, 6, 19, 6)
            Case Pokemon.StatusProblems.Sleep
                r = New Rectangle(96, 24, 19, 6)
            Case Pokemon.StatusProblems.None
                Return Nothing
        End Select

        Return TextureManager.GetTexture("GUI\Menus\Types", r, "")
    End Function

    Public Shared Function GetStatColor(ByVal Status As Pokemon.StatusProblems) As Color
        Select Case Status
            Case Pokemon.StatusProblems.BadPoison, Pokemon.StatusProblems.Poison
                Return New Color(214, 49, 222)
            Case Pokemon.StatusProblems.Burn
                Return New Color(231, 90, 74)
            Case Pokemon.StatusProblems.Paralyzed
                Return New Color(239, 173, 0)
            Case Pokemon.StatusProblems.Freeze
                Return New Color(33, 140, 247)
            Case Pokemon.StatusProblems.Sleep
                Return New Color(132, 132, 132)
            Case Pokemon.StatusProblems.Fainted
                Return New Color(181, 0, 0)
        End Select

        Return Color.White
    End Function

End Class