Imports P3D.Items

''' <summary>
''' Provides an interface to load additional GameMode items.
''' </summary>
Public Class GameModeItemLoader

    'The default relative path to load items from (Content folder).
    Const PATH As String = "Data\Items\"

    'List of loaded items.
    Shared LoadedItems As New List(Of GameModeItem)

    ''' <summary>
    ''' Load the attack list for the loaded GameMode.
    ''' </summary>
    ''' <remarks>The game won't try to load the list if the default GameMode is selected.</remarks>
    Public Shared Sub Load()
        LoadedItems.Clear()

        If GameModeManager.ActiveGameMode.IsDefaultGamemode = False Then
            If System.IO.Directory.Exists(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & "\" & PATH) = True Then
                For Each file As String In System.IO.Directory.GetFiles(GameController.GamePath & "\" & GameModeManager.ActiveGameMode.ContentPath & PATH, "*.dat")
                    LoadItem(file)
                Next
            End If
        End If

        If LoadedItems.Count > 0 Then
            Logger.Debug("Loaded " & LoadedItems.Count.ToString() & " GameMode item(s).")
        End If
    End Sub

    ''' <summary>
    ''' Loads a item from a file.
    ''' </summary>
    ''' <param name="file">The file to load the item from.</param>
    Private Shared Sub LoadItem(ByVal file As String)
        Dim item As New GameModeItem

        Dim content() As String = System.IO.File.ReadAllLines(file)

        Dim key As String = ""
        Dim value As String = ""

        Dim setID As Boolean = False 'Controls if the item sets its ID.
        Dim setName As Boolean = False 'Controls if the item sets its ID.

        Try
            'Go through lines of the file and set the properties depending on the content.
            'Lines starting with # are comments.
            For Each l As String In content
                If l.Contains("|") = True And l.StartsWith("#") = False Then
                    key = l.Remove(l.IndexOf("|"))
                    value = l.Remove(0, l.IndexOf("|") + 1)

                    Select Case key.ToLower()
                        Case "id"
                            item.gmID = "gm" & CInt(value).ToString
                            setID = True
                        Case "name"
                            item.gmName = value
                            setName = True
                        Case "pluralname"
                            item.gmPluralName = value
                        Case "description"
                            item.gmDescription = value
                        Case "type"
                            Select Case value.ToLower()
                                Case "standard", "0"
                                    item.gmItemType = ItemTypes.Standard
                                Case "medicine", "1"
                                    item.gmItemType = ItemTypes.Medicine
                                Case "plants", "2"
                                    item.gmItemType = ItemTypes.Plants
                                Case "balls", "pokeballs", "3"
                                    item.gmItemType = ItemTypes.Pokéballs
                                Case "machines", "4"
                                    item.gmItemType = ItemTypes.Machines
                                Case "keyitems", "5"
                                    item.gmItemType = ItemTypes.KeyItems
                                Case "mail", "6"
                                    item.gmItemType = ItemTypes.Mail
                                Case "battleitems", "7"
                                    item.gmItemType = ItemTypes.BattleItems
                            End Select
                        Case "textureindex"
                            Dim itemX As Integer = CInt(value)
                            Dim itemY As Integer = 0
                            Dim sheetWidth As Integer = CInt(TextureManager.GetTexture(item.gmTextureSource).Width / 24)

                            While itemX > sheetWidth - 1
                                itemX -= sheetWidth
                                itemY += 1
                            End While
                            item.gmTextureRectangle = New Rectangle(CInt(itemX * 24), CInt(itemY * 24), 24, 24)
                        Case "canbeused"
                            item.gmCanBeUsed = CBool(value)
                        Case "canbeusedinbattle"
                            item.gmCanBeUsedInBattle = CBool(value)
                        Case "useonpokemoninbattle"
                            item.gmBattleSelectPokemon = CBool(value)
                        Case "canbetossed"
                            item.gmCanBeTossed = CBool(value)
                        Case "canbeheld"
                            item.gmCanBeHeld = CBool(value)
                        Case "canbetraded"
                            item.gmCanBeTraded = CBool(value)
                        Case "price"
                            item.gmPrice = CInt(value)
                        Case "battlepointsprice"
                            item.gmBattlePointsPrice = CInt(value)
                        Case "catchmultiplier"
                            item.gmCatchMultiplier = CSng(value.ReplaceDecSeparator)
                        Case "maxstack"
                            item.gmMaxStack = CInt(value)
                        Case "flingdamage"
                            item.gmFlingDamage = CInt(value)
                        Case "ishealingitem"
                            item.gmIsHealingItem = CBool(value)
                        Case "healhpamount"
                            item.gmHealHPAmount = CInt(value)
                        Case "curestatuseffects"
                            Dim StatusEffectList As New List(Of String)
                            Dim valueSplit As String() = value.Split(",")
                            For i = 0 To valueSplit.Count - 1
                                Select Case valueSplit(i).ToLower
                                    Case "brn", "frz", "prz", "psn", "bpsn", "slp", "fnt", "confusion", "allwithoutfnt", "all"
                                        StatusEffectList.Add(valueSplit(i))
                                End Select
                            Next
                            If item.gmCureStatusEffects Is Nothing Then
                                item.gmCureStatusEffects = StatusEffectList
                            Else
                                item.gmCureStatusEffects.AddRange(StatusEffectList)
                            End If
                        Case "isevolutionitem"
                            item.gmIsEvolutionItem = CBool(value)
                        Case "evolutionpokemon"
                            Dim PokemonList As New List(Of Integer)
                            Dim valueSplit As String() = value.Split(CChar(","))
                            For i = 0 To valueSplit.Count - 1
                                If Pokemon.PokemonDataExists(CInt(valueSplit(i))) Then
                                    PokemonList.Add(CInt(valueSplit(i)))
                                End If
                            Next
                            If item.gmEvolutionPokemon Is Nothing Then
                                item.gmEvolutionPokemon = PokemonList
                            Else
                                item.gmEvolutionPokemon.AddRange(PokemonList)
                            End If
                        Case "script"
                            item.gmScriptPath = value
                        Case "istm"
                            item.gmIsTM = CBool(value)
                        Case "teachmove"
                            item.gmTeachMove = BattleSystem.Attack.GetAttackByID(CInt(value))
                    End Select
                End If
            Next
        Catch ex As Exception
            'If an error occurs loading a item, log the error.
            Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeItemLoader.vb: Error loading GameMode Item from file """ & file & """: " & ex.Message & "; Last Key/Value pair successfully loaded: " & key & "|" & value)
        End Try

        If setID = True AndAlso setName = True Then
            If item.gmIsMegaStone = True AndAlso item.gmMegaPokemonNumber <> Nothing AndAlso item.gmDescription = "" Then
                Dim MegaPokemonName As String = Pokemon.GetPokemonByID(item.gmMegaPokemonNumber, item.AdditionalData).GetName
                item.gmDescription = "One variety of the mysterious Mega Stones. Have " & MegaPokemonName & " hold it, and this stone will enable it to Mega Evolve during battle."
                item.gmCanBeTossed = False
                item.gmCanBeTraded = False
                item.gmCanBeUsed = False
                item.gmCanBeUsedInBattle = False
            End If
            If item.gmIsTM = True AndAlso item.gmTeachMove IsNot Nothing AndAlso item.gmDescription = "" Then
                Dim AttackName As String = item.gmTeachMove.Name
                item.gmDescription = "Teaches """ & AttackName & """ to a Pokémon."
                item.gmItemType = ItemTypes.Machines
                item.gmCanBeHeld = False
                item.gmCanBeTossed = True
                item.gmCanBeTraded = True
                item.gmCanBeUsed = True
                item.gmCanBeUsedInBattle = False
                If item.gmName.StartsWith("TM ") Then
                    item.gmSortValue = CInt(item.gmName.Remove(0, 3)) + 190
                ElseIf item.gmName.StartsWith("TM") Then
                    item.gmSortValue = CInt(item.gmName.Remove(0, 2)) + 190
                End If
                item.gmTextureSource = "Items\ItemSheet"
                item.SetTeachMoveTextureRectangle()

            End If
            If item.gmTextureRectangle = Nothing Then
                Dim itemX As Integer = CInt(item.gmID.Remove(0, 2))
                Dim itemY As Integer = 0
                Dim sheetWidth As Integer = CInt(TextureManager.GetTexture(item.gmTextureSource).Width / 24)

                While itemX > sheetWidth - 1
                    itemX -= sheetWidth
                    itemY += 1
                End While
                item.gmTextureRectangle = New Rectangle(CInt(itemX * 24), CInt(itemY * 24), 24, 24)
            End If
            LoadedItems.Add(item) 'Add the item.
        Else
            Logger.Log(Logger.LogTypes.ErrorMessage, "GameModeItemLoader.vb: User defined Items must set their ID through the ""ID"" property and their Name through the ""Name"" property, however the item loaded from """ & file & """ has no ID or Name set so it will be ignored.")
        End If
    End Sub

    ''' <summary>
    ''' Returns a custom item based on its ID.
    ''' </summary>
    ''' <param name="ID">The ID of the custom item.</param>
    ''' <returns>Returns a item or nothing.</returns>
    Public Shared Function GetItemByID(ByVal ID As String) As GameModeItem
        For Each i As GameModeItem In LoadedItems
            If i.gmID = ID Then
                Return i
            End If
        Next
        Return Nothing
    End Function
    Public Shared Function GetItemByName(ByVal Name As String) As GameModeItem
        For Each i As GameModeItem In LoadedItems
            If i.gmName.ToLowerInvariant() = Name.ToLowerInvariant() Then
                Return i
            End If
        Next
        Return Nothing
    End Function

End Class
