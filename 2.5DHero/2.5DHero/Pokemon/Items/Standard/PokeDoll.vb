Namespace Items.Standard

    Public Class PokeDoll

        Inherits Item

        Public Sub New()
            MyBase.New("Poké Doll", 1000, ItemTypes.BattleItems, 37, 1, 0, New Rectangle(312, 24, 24, 24), "A doll that attracts the attention of a Pokémon. It guarantees escape from any battle with wild Pokémon.")

            Me._canBeHold = True
            Me._canBeTraded = True
            Me._canBeUsed = False
            Me._canBeUsedInBattle = True

            Me._requiresPokemonSelectInBattle = False
        End Sub

        Public Overrides Function UseOnPokemon(PokeIndex As Integer) As Boolean
            Dim i As Integer = 0
            Dim s As Screen = Core.CurrentScreen
            While s.Identification <> Screen.Identifications.BattleScreen
                s = s.PreScreen
            End While

            Dim BattleScreen As BattleSystem.BattleScreen = CType(s, BattleSystem.BattleScreen)

            If BattleScreen.IsTrainerBattle = True Then
                Screen.TextBox.Show("Cannot run from a trainer battle!", {}, False, False)
                Return False
            Else
                Me.RemoveItem()
                BattleScreen.BattleQuery.Clear()
                BattleScreen.BattleQuery.Insert(0, New BattleSystem.ToggleMenuQueryObject(True))
                BattleScreen.BattleQuery.Add(BattleScreen.FocusOwnPlayer())
                BattleScreen.BattleQuery.Add(New BattleSystem.TextQueryObject(Core.Player.Name & " used a Pokédoll!"))
                BattleScreen.BattleQuery.Add(New BattleSystem.TextQueryObject("Got away safely!"))
                BattleScreen.BattleQuery.Add(New BattleSystem.EndBattleQueryObject(False))
                Return True
            End If
        End Function

    End Class

End Namespace