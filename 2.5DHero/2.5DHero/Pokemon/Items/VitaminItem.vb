Namespace Items

    ''' <summary>
    ''' Represents a Vitamin Item.
    ''' </summary>
    Public MustInherit Class VitaminItem

        Inherits Item

        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Medicine

        ''' <summary>
        ''' Checks if a Vitamin can be used on a Pokémon.
        ''' </summary>
        ''' <param name="stat">An integer representing the stat that should be upped by the Vitamin.</param>
        ''' <param name="p">The Pokémon that the Vitamin should be used on.</param>
        Protected Shared Function CanUseVitamin(ByVal stat As Integer, ByVal p As Pokemon) As Boolean
            If stat < 100 Then
                Dim allStats As Integer = p.EVAttack + p.EVDefense + p.EVSpAttack + p.EVSpDefense + p.EVHP + p.EVSpeed
                If allStats < 510 Then
                    Return True
                End If
            End If

            Return False
        End Function

    End Class

End Namespace
