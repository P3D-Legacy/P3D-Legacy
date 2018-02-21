Public Class Shedinja

    ''' <summary>
    ''' Generates a new Shedinja based on the Nincada that evolved into Ninjask.
    ''' </summary>
    Public Shared Function GenerateNew(ByVal Ninjask As Pokemon) As Pokemon
        Dim p As Pokemon = Pokemon.GetPokemonByID(292)
        p.Generate(20, True)

        'Set attacks from Nincada
        p.Attacks.Clear()
        For Each a As BattleSystem.Attack In Ninjask.Attacks
            p.Attacks.Add(BattleSystem.Attack.GetAttackByID(a.ID))
        Next

        'Set IVs and EVs:
        p.EVHP = Ninjask.EVHP
        p.EVAttack = Ninjask.EVAttack
        p.EVDefense = Ninjask.EVDefense
        p.EVSpAttack = Ninjask.EVSpAttack
        p.EVSpDefense = Ninjask.EVSpDefense
        p.EVSpeed = Ninjask.EVSpeed

        p.IVHP = Ninjask.IVHP
        p.IVAttack = Ninjask.IVAttack
        p.IVDefense = Ninjask.IVDefense
        p.IVSpAttack = Ninjask.IVSpAttack
        p.IVSpDefense = Ninjask.IVSpDefense
        p.IVSpeed = Ninjask.IVSpeed

        'Set base infos:
        p.OT = Ninjask.OT
        p.CatchTrainerName = Ninjask.CatchTrainerName
        p.CatchMethod = "appeared at"
        p.CatchLocation = Screen.Level.MapName
        p.IsShiny = Ninjask.IsShiny
        p.Nature = Ninjask.Nature

        Return p
    End Function

    ''' <summary>
    ''' Checks if the Pokémon that just evolved can additionally spawn a Shedinja.
    ''' </summary>
    Public Shared Function CanEvolveInto(ByVal EvolvedPokemon As Pokemon, ByVal Trigger As EvolutionCondition.EvolutionTrigger) As Boolean
        If EvolvedPokemon.Number = 291 And Trigger = EvolutionCondition.EvolutionTrigger.LevelUp Then
            If Core.Player.Pokemons.Count < 6 Then
                If Core.Player.Inventory.GetItemAmount(5) > 0 Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

End Class
