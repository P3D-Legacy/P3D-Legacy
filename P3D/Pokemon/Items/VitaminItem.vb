Namespace Items

    ''' <summary>
    ''' Represents a Vitamin Item.
    ''' </summary>
    Public MustInherit Class VitaminItem

        Inherits Item

        Public Overrides ReadOnly Property ItemType As ItemTypes = ItemTypes.Medicine
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 9800
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False

        Public Overrides Sub Use()
            Dim selScreen = New PartyScreen(Core.CurrentScreen, Me, AddressOf Me.UseOnPokemon, Localization.GetString("global_use", "Use") & " " & Me.OneLineName(), True) With {.Mode = Screens.UI.ISelectionScreen.ScreenMode.Selection, .CanExit = True}
            AddHandler selScreen.SelectedObject, AddressOf UseItemhandler

            Core.SetScreen(selScreen)
        End Sub

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
