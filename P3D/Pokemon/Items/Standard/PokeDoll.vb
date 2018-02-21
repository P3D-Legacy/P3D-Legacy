Namespace Items.Standard

    <Item(37, "Poké Doll")>
    Public Class PokeDoll

        Inherits Item

        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.BattleItems
        Public Overrides ReadOnly Property Description As String = "A doll that attracts the attention of a Pokémon. It guarantees escape from any battle with wild Pokémon."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 1000
        Public Overrides ReadOnly Property BattleSelectPokemon As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(312, 24, 24, 24)
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
