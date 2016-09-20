Namespace Items

    ''' <summary>
    ''' Represents a Wing Item.
    ''' </summary>
    Public MustInherit Class WingItem

        Inherits Item

        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Medicine
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property FlingDamage As Integer = 20
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 3000

        Public Overrides Sub Use()
            Core.SetScreen(New ChoosePokemonScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, "Use " & Me.Name, True))
        End Sub

        ''' <summary>
        ''' Checks if a Wing can be used on a Pokémon.
        ''' </summary>
        ''' <param name="stat">An integer representing the stat that should be upped by the Wing.</param>
        ''' <param name="p">The Pokémon that the Wing should be used on.</param>
        Protected Shared Function CanUseWing(ByVal stat As Integer, ByVal p As Pokemon) As Boolean
            If stat < 255 Then
                Dim allStats As Integer = p.EVAttack + p.EVDefense + p.EVSpAttack + p.EVSpDefense + p.EVHP + p.EVSpeed
                If allStats < 510 Then
                    Return True
                End If
            End If

            Return False
        End Function

    End Class

End Namespace
