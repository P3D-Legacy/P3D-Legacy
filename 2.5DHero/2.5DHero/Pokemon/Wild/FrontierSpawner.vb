Public Class FrontierSpawner

    Public Shared Function GetPokemon(ByVal level As Integer, ByVal pokemon_class As Integer, ByVal IDPreset As List(Of Integer)) As Pokemon
        Dim validIDs As New List(Of Integer)

        Dim files() As String = System.IO.Directory.GetFiles(GameController.GamePath & "\Content\Pokemon\Data", "*.dat", IO.SearchOption.TopDirectoryOnly)
        For Each f As String In files
            If IsNumeric(System.IO.Path.GetFileNameWithoutExtension(f)) = True Then
                Dim newID As Integer = CInt(System.IO.Path.GetFileNameWithoutExtension(f))
                If IDPreset Is Nothing OrElse IDPreset.Contains(newID) = True Then
                    validIDs.Add(newID)
                End If
            End If
        Next

        Dim ID As Integer = validIDs(Core.Random.Next(0, validIDs.Count))

        Dim p As Pokemon = GetPredeterminedPokemon(ID, level, pokemon_class)

        If p Is Nothing Then
            p = Pokemon.GetPokemonByID(ID)
            p.Generate(level, True)
            p.FullRestore()
        End If

        Return p
    End Function

    Private Shared Function GetPredeterminedPokemon(ByVal ID As Integer, ByVal level As Integer, ByVal pokemon_class As Integer) As Pokemon
        Dim path As String = GameController.GamePath & "\Content\Pokemon\Data\frontier\" & pokemon_class.ToString() & ".dat"
        Security.FileValidation.CheckFileValid(path, False, "FrontierSpawner.vb")

        Dim data As List(Of String) = System.IO.File.ReadAllLines(path).ToList()

        For Each line As String In data
            Dim lData() As String = line.Split(CChar("|"))
            Dim InputIDs() As String = lData(0).Split(CChar(","))
            If InputIDs.Contains(ID.ToString()) = True Then
                Dim OutputID As Integer = CInt(lData(1))
                Dim Moveset As New List(Of Integer)
                For Each move As String In lData(2).Split(CChar(","))
                    If move <> "" And IsNumeric(move) = True Then
                        Moveset.Add(CInt(move))
                    End If
                Next
                Dim Stats() As String = lData(3).Split(CChar(","))
                Dim ItemID As String = lData(4)

                Dim p As Pokemon = Pokemon.GetPokemonByID(OutputID)
                p.Generate(level, True)
                p.Item = Nothing
                AddMoveset(p, Moveset.ToArray())
                SetStats(p, Stats(0), Stats(1), pokemon_class)

                If ItemID <> "" Then
                    p.Item = Item.GetItemByID(CInt(ItemID))
                End If
                If p.Item Is Nothing Then
                    Dim items() As Integer = {146, 2009, 119, 140, 73, 74}

                    p.Item = Item.GetItemByID(items(Core.Random.Next(0, items.Length)))
                End If

                p.FullRestore()
                Return p
            End If
        Next

        Return Nothing
    End Function

    Private Shared Sub GiveItem(ByRef p As Pokemon)
        If Not p Is Nothing Then
            If Not p.Item Is Nothing Then
                Dim items() As Integer = {146, 2009, 119, 140, 73, 74}

                p.Item = Item.GetItemByID(items(Core.Random.Next(0, items.Length)))
            End If
        End If
    End Sub

    Private Shared Sub SetStats(ByRef p As Pokemon, ByVal stat1 As String, ByVal stat2 As String, ByVal pokemon_class As Integer)
        Dim IVRange() As Integer = {0, 0}
        Dim standardEV As Integer = 10
        Dim maxEV As Integer = 255
        Dim maxIV As Integer = 31

        Select Case pokemon_class
            Case 0 'base
                IVRange = {0, 20}
                standardEV = 4
                maxEV = 150
                maxIV = 20
            Case 1 'normal
                IVRange = {5, 31}
                standardEV = 8
                maxEV = 200
                maxIV = 26
            Case 2 'master
                IVRange = {20, 31}
                standardEV = 10
                maxEV = 255
                maxIV = 31
        End Select

        p.IVHP = Core.Random.Next(IVRange(0), IVRange(1) + 1)
        p.IVAttack = Core.Random.Next(IVRange(0), IVRange(1) + 1)
        p.IVDefense = Core.Random.Next(IVRange(0), IVRange(1) + 1)
        p.IVSpAttack = Core.Random.Next(IVRange(0), IVRange(1) + 1)
        p.IVSpDefense = Core.Random.Next(IVRange(0), IVRange(1) + 1)
        p.IVSpeed = Core.Random.Next(IVRange(0), IVRange(1) + 1)

        p.EVHP = standardEV
        p.EVAttack = standardEV
        p.EVDefense = standardEV
        p.EVSpAttack = standardEV
        p.EVSpDefense = standardEV
        p.EVSpeed = standardEV

        Select Case stat1.ToLower()
            Case "hp"
                p.IVHP = maxIV
                p.EVHP = maxEV
            Case "atk", "attack"
                p.IVAttack = maxIV
                p.EVAttack = maxEV
            Case "def", "defense"
                p.IVDefense = maxIV
                p.EVDefense = maxEV
            Case "spatk", "spattack"
                p.IVSpAttack = maxIV
                p.EVSpAttack = maxEV
            Case "spdef", "spdefense"
                p.IVSpDefense = maxIV
                p.EVSpDefense = maxEV
            Case "speed"
                p.IVSpeed = maxIV
                p.EVSpeed = maxEV
        End Select
        Select Case stat2.ToLower()
            Case "hp"
                p.IVHP = maxIV
                p.EVHP = maxEV
            Case "atk", "attack"
                p.IVAttack = maxIV
                p.EVAttack = maxEV
            Case "def", "defense"
                p.IVDefense = maxIV
                p.EVDefense = maxEV
            Case "spatk", "spattack"
                p.IVSpAttack = maxIV
                p.EVSpAttack = maxEV
            Case "spdef", "spdefense"
                p.IVSpDefense = maxIV
                p.EVSpDefense = maxEV
            Case "speed"
                p.IVSpeed = maxIV
                p.EVSpeed = maxEV
        End Select

        If pokemon_class > 0 Then
            Select Case stat1.ToLower()
                Case "hp"
                    p.Nature = Pokemon.ConvertIDToNature(Core.Random.Next(0, 26))
                Case "atk"
                    If stat2.ToLower() <> "def" Then
                        p.Nature = Pokemon.Natures.Lonely
                    Else
                        p.Nature = Pokemon.Natures.Relaxed
                    End If
                Case "def"
                    If stat2.ToLower() <> "spatk" Then
                        p.Nature = Pokemon.Natures.Impish
                    Else
                        p.Nature = Pokemon.Natures.Lax
                    End If
                Case "spatk"
                    If stat2.ToLower() <> "spdef" Then
                        p.Nature = Pokemon.Natures.Rash
                    Else
                        p.Nature = Pokemon.Natures.Modest
                    End If
                Case "spdef"
                    If stat2.ToLower() <> "speed" Then
                        p.Nature = Pokemon.Natures.Sassy
                    Else
                        p.Nature = Pokemon.Natures.Gentle
                    End If
                Case "speed"
                    If stat2.ToLower() <> "atk" Then
                        p.Nature = Pokemon.Natures.Timid
                    Else
                        p.Nature = Pokemon.Natures.Jolly
                    End If
            End Select
        End If

        p.CalculateStats()
    End Sub

    Private Shared Sub AddMoveset(ByRef p As Pokemon, ByVal moveList() As Integer)
        p.Attacks.Clear()
        For Each moveID As Integer In moveList
            p.Attacks.Add(BattleSystem.Attack.GetAttackByID(moveID))
        Next
    End Sub

End Class