Namespace Items.Medicine

    Public Class SacredAsh

        Inherits Item

        Public Sub New()
            MyBase.New("Sacred Ash", 200, ItemTypes.Medicine, 156, 1, 1, New Rectangle(24, 144, 24, 24), "It revives all fainted Pokémon. In doing so, it also fully restores their HP.")

            Me._canBeUsed = True
            Me._canBeUsedInBattle = True
            Me._canBeTraded = True
            Me._canBeHold = True
        End Sub

        Public Overrides Sub Use()
            Dim hasFainted As Boolean = False

            For Each p As Pokemon In Core.Player.Pokemons
                If p.Status = Pokemon.StatusProblems.Fainted Then
                    hasFainted = True
                    Exit For
                End If
            Next

            If hasFainted = True Then
                For Each p As Pokemon In Core.Player.Pokemons
                    If p.Status = Pokemon.StatusProblems.Fainted Then
                        p.Status = Pokemon.StatusProblems.None
                        p.HP = p.MaxHP
                    End If
                Next

                SoundManager.PlaySound("single_heal", False)
                Screen.TextBox.Show("Your team has been~fully healed." & RemoveItem(), {})
                PlayerStatistics.Track("[17]Medicine Items used", 1)
            Else
                Screen.TextBox.Show("Cannot be used.", {})
            End If
        End Sub

    End Class

End Namespace