Namespace ScriptVersion2

    Partial Class ScriptComparer

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the <daycare> constructs.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Function DoDaycare(ByVal subClass As String) As Object
            Dim command As String = GetSubClassArgumentPair(subClass).Command
            Dim argument As String = GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "pokemonid"
                    Dim dayCareID As Integer = int(argument.GetSplit(0))
                    Dim PokemonIndex As Integer = int(argument.GetSplit(1))

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                            Dim data As String = line.Remove(0, line.IndexOf("{"))
                            Dim p As Pokemon = Pokemon.GetPokemonByData(data)
                            Return p.Number
                        End If
                    Next

                    Return 0
                Case "pokemonname"
                    Dim dayCareID As Integer = int(argument.GetSplit(0))
                    Dim PokemonIndex As Integer = int(argument.GetSplit(1))

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                            Dim data As String = line.Remove(0, line.IndexOf("{"))
                            Dim p As Pokemon = Pokemon.GetPokemonByData(data)
                            Return p.GetDisplayName()
                        End If
                    Next

                    Return "missingno"
                Case "shinyindicator"
                    Dim dayCareID As Integer = int(argument.GetSplit(0))
                    Dim PokemonIndex As Integer = int(argument.GetSplit(1))

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                            Dim data As String = line.Remove(0, line.IndexOf("{"))
                            Dim p As Pokemon = Pokemon.GetPokemonByData(data)

                            If p.IsShiny = True Then
                                Return "S"
                            Else
                                Return "N"
                            End If
                        End If
                    Next

                    Return "N"
                Case "pokemonsprite"
                    Dim dayCareID As Integer = int(argument.GetSplit(0))
                    Dim PokemonIndex As Integer = int(argument.GetSplit(1))

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                            Dim data As String = line.Remove(0, line.IndexOf("{"))
                            Dim p As Pokemon = Pokemon.GetPokemonByData(data)

                            Dim shiny As String = "N"
                            If p.IsShiny = True Then
                                shiny = "S"
                            End If

                            Return "[POKEMON|" & shiny & "]" & p.Number.ToString() & PokemonForms.GetOverworldAddition(p)
                        End If
                    Next

                    Return "[POKEMON|N]10"
                Case "countpokemon"
                    Dim count As Integer = 0
                    If Core.Player.DaycareData <> "" Then
                        Dim dayCareID As Integer = int(argument)
                        For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                            If line.StartsWith(dayCareID.ToString() & "|") = True Then
                                count += 1
                            End If
                        Next
                    End If
                    Return count
                Case "haspokemon"
                    Dim count As Integer = 0
                    If Core.Player.DaycareData <> "" Then
                        Dim dayCareID As Integer = int(argument)
                        For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                            If line.StartsWith(dayCareID.ToString() & "|") = True Then
                                count += 1
                            End If
                        Next
                    End If
                    If count = 0 Then
                        Return ReturnBoolean(False)
                    Else
                        Return ReturnBoolean(True)
                    End If
                Case "canswim"
                    Dim dayCareID As Integer = int(argument.GetSplit(0))
                    Dim PokemonIndex As Integer = int(argument.GetSplit(1))

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                            Dim data As String = line.Remove(0, line.IndexOf("{"))
                            Dim p As Pokemon = Pokemon.GetPokemonByData(data)

                            Return ReturnBoolean(p.CanSwim)
                        End If
                    Next

                    Return ReturnBoolean(False)
                Case "hasegg"
                    Dim dayCareID As Integer = int(argument)

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|Egg|") = True Then
                            Return ReturnBoolean(True)
                        End If
                    Next
                    Return ReturnBoolean(False)
                Case "grownlevels"
                    Dim dayCareID As Integer = int(argument.GetSplit(0))
                    Dim PokemonIndex As Integer = int(argument.GetSplit(1))

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                            Dim data As String = line.Remove(0, line.IndexOf("{"))
                            Dim startStep As Integer = CInt(line.Split(CChar("|"))(2))
                            Dim p As Pokemon = Pokemon.GetPokemonByData(data)
                            Dim startLevel As Integer = p.Level
                            p.GetExperience(Core.Player.DaycareSteps - startStep, True)

                            Return p.Level - startLevel
                        End If
                    Next
                Case "currentlevel"
                    Dim dayCareID As Integer = int(argument.GetSplit(0))
                    Dim PokemonIndex As Integer = int(argument.GetSplit(1))

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                            Dim data As String = line.Remove(0, line.IndexOf("{"))
                            Dim startStep As Integer = CInt(line.Split(CChar("|"))(2))
                            Dim p As Pokemon = Pokemon.GetPokemonByData(data)
                            p.GetExperience(Core.Player.DaycareSteps - startStep, True)

                            Return p.Level
                        End If
                    Next
                Case "canbreed"
                    Dim dayCareID As Integer = int(argument.GetSplit(0))

                    Return Daycare.CanBreed(dayCareID)
            End Select

            Return DEFAULTNULL
        End Function

    End Class

End Namespace