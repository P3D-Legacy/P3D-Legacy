Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <pokedex> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoPokedex(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "caught"
                    Return Pokedex.CountEntries(Core.Player.PokedexData, {2, 3}).ToString()
                Case "shiny"
                    Return Pokedex.CountEntries(Core.Player.PokedexData, {3}).ToString()
                Case "seen"
                    Return Pokedex.CountEntries(Core.Player.PokedexData, {1}).ToString()
                Case "dexcaught"
                    Dim dexIndex As Integer = int(argument)
                    Return Core.Player.Pokedexes(dexIndex).Obtained
                Case "dexseen"
                    Dim dexIndex As Integer = int(argument)
                    Return Core.Player.Pokedexes(dexIndex).Seen
                Case "getheight"

                Case "getweigth"

                Case "getentry"

                Case "getspecies"

                Case "getname"
                    Dim number As Integer = int(argument)
                    If Pokemon.PokemonDataExists(number)
                        return Pokemon.GetPokemonByID(number).GetName()
                    End If
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace