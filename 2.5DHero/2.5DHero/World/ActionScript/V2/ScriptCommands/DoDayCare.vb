Namespace ScriptVersion2

    Partial Class ScriptCommander

        '--------------------------------------------------------------------------------------------------------------------------
        'Contains the @daycare commands.
        '--------------------------------------------------------------------------------------------------------------------------

        Private Shared Sub DoDayCare(ByVal subClass As String)
            Dim command As String = ScriptComparer.GetSubClassArgumentPair(subClass).Command
            Dim argument As String = ScriptComparer.GetSubClassArgumentPair(subClass).Argument

            Select Case command.ToLower()
                Case "takeegg"
                    Dim newData As String = ""
                    Dim dayCareID As Integer = int(argument)

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|Egg|") = False Then
                            If newData <> "" Then
                                newData &= vbNewLine
                            End If
                            newData &= line
                        Else
                            Dim p As Pokemon = Daycare.ProduceEgg(dayCareID)
                            If Not p Is Nothing Then
                                Core.Player.Pokemons.Add(p)
                            End If
                        End If
                    Next

                    Core.Player.DaycareData = newData
                Case "takepokemon"
                    Dim newData As String = ""

                    Dim dayCareID As Integer = int(argument.GetSplit(0))
                    Dim PokemonIndex As Integer = int(argument.GetSplit(1))

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|" & PokemonIndex.ToString() & "|") = True Then
                            Dim data As String = line.Remove(0, line.IndexOf("{"))
                            Dim startStep As Integer = CInt(line.Split(CChar("|"))(2))
                            Dim p As Pokemon = Pokemon.GetPokemonByData(data)
                            p.GetExperience(Core.Player.DaycareSteps - startStep, True)
                            Core.Player.Pokemons.Add(p)
                        Else
                            If newData <> "" Then
                                newData &= vbNewLine
                            End If
                            newData &= line
                        End If
                    Next

                    Core.Player.DaycareData = newData
                Case "leavepokemon"
                    Dim dayCareID As Integer = int(argument.GetSplit(0))
                    Dim PokemonDaycareIndex As Integer = int(argument.GetSplit(1))
                    Dim PokemonIndex As Integer = int(argument.GetSplit(2))

                    If Core.Player.DaycareData <> "" Then
                        Core.Player.DaycareData &= vbNewLine
                    End If

                    Core.Player.DaycareData &= dayCareID.ToString() & "|" & PokemonDaycareIndex.ToString() & "|" & Core.Player.DaycareSteps & "|0|" & Core.Player.Pokemons(PokemonIndex).GetSaveData()

                    Core.Player.Pokemons.RemoveAt(PokemonIndex)
                Case "removeegg"
                    Dim newData As String = ""
                    Dim dayCareID As Integer = int(argument)

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(dayCareID.ToString() & "|Egg|") = False Then
                            If newData <> "" Then
                                newData &= vbNewLine
                            End If
                            newData &= line
                        End If
                    Next

                    Core.Player.DaycareData = newData
                Case "clean"
                    Dim daycareID As Integer = int(argument)
                    Dim newData As String = ""

                    Dim lines As New List(Of String)
                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(daycareID & "|") = True Then
                            lines.Add(line)
                        Else
                            If newData <> "" Then
                                newData &= vbNewLine
                            End If
                            newData &= line
                        End If
                    Next

                    For i = 0 To lines.Count - 1
                        Dim line As String = lines(i)
                        Dim data() As String = line.Split(CChar("|"))

                        If newData <> "" Then
                            newData &= vbNewLine
                        End If

                        If data(1) = "Egg" Then
                            newData &= daycareID.ToString() & "|Egg|" & data(2)
                        Else
                            newData &= daycareID.ToString() & "|" & i.ToString() & "|" & data(2) & "|" & data(3) & "|" & line.Remove(0, line.IndexOf("{"))
                        End If
                    Next

                    Core.Player.DaycareData = newData
                Case "cleardata"
                    Dim daycareID As Integer = int(argument)
                    Dim newData As String = ""

                    For Each line As String In Core.Player.DaycareData.SplitAtNewline()
                        If line.StartsWith(daycareID.ToString() & "|") = False Then
                            If newData <> "" Then
                                newData &= vbNewLine
                            End If
                            newData &= line
                        End If
                    Next

                    Core.Player.DaycareData = newData
                Case "call"
                    Daycare.TriggerCall(int(argument))
                Case Else
                    Logger.Log(Logger.LogTypes.Warning, "ScriptCommander.vb: (@daycare." & command & ") Command not found.")
            End Select

            IsReady = True
        End Sub 'crash handle

    End Class

End Namespace