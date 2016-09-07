Public Class BattlePokemonInfoScreen

    Inherits Screen

    Dim Pokemon As Pokemon

    Dim index As Integer = 0
    Dim mainTexture As Texture2D
    Public Delegate Sub DoStuff(ByVal PokeIndex As Integer)
    Dim ChoosePokemon As DoStuff
    Dim pokeIndex As Integer = 0
    Dim BattleScreen As BattleSystem.BattleScreen

    Dim IsItem As Boolean = False
    Dim PokeSize As Integer = 128
    Dim ItemSize As Integer = 0

    Public Sub New(ByVal currentScreen As Screen, ByVal PokeIndex As Integer, ByVal ChoosePokemon As DoStuff, ByVal BattleScreen As BattleSystem.BattleScreen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.BattlePokemonScreen
        Me.ChoosePokemon = ChoosePokemon
        Me.pokeIndex = PokeIndex
        Me.Pokemon = Core.Player.Pokemons(PokeIndex)
        Me.BattleScreen = BattleScreen

        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        Logger.Debug(currentScreen.Identification.ToString())
    End Sub

    Public Overrides Sub Update()
        If Controls.Up(True, True, True, True) = True Then
            Me.index -= 1
        End If
        If Controls.Down(True, True, True, True) = True Then
            Me.index += 1
        End If

        If Me.index < 0 Then
            Me.index = 3
        ElseIf Me.index > 3 Then
            Me.index = 0
        End If

        If IsItem = True Then
            If PokeSize > 0 Then
                PokeSize -= 10
            End If
            If ItemSize < 100 Then
                ItemSize += 10
            End If
        Else
            If PokeSize < 128 Then
                PokeSize += 10
            End If
            If ItemSize > 0 Then
                ItemSize -= 10
            End If
        End If

        If Controls.Dismiss(True, True) = True Then
            Core.SetScreen(Me.PreScreen)
        End If

        If Controls.Accept(True, True) = True Then
            If IsItem = True Then
                Select Case index
                    Case 0
                        Core.SetScreen(Me.PreScreen)
                        Dim i As Item = Core.Player.Pokemons(pokeIndex).Item

                        Core.Player.Inventory.AddItem(i.ID, 1)
                        Core.Player.Pokemons(pokeIndex).Item = Nothing

                        'Battle.StartAttackOpponent()
                    Case 1

                    Case 2
                        IsItem = False
                        Me.index = 2
                End Select
            Else
                Select Case index
                    Case 0
                        Core.SetScreen(Me.PreScreen)
                        Me.ChoosePokemon(Me.pokeIndex)
                    Case 1
                        Core.SetScreen(New PokemonStatusScreen(Me, Me.pokeIndex, {}, Core.Player.Pokemons(Me.pokeIndex), True))
                    Case 2
                        If Not Pokemon.Item Is Nothing Then
                            IsItem = True
                            Me.index = 0
                        End If
                    Case 3
                        Core.SetScreen(Me.PreScreen)
                End Select
            End If
        End If
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        Canvas.DrawRectangle(New Rectangle(0, 0, Core.ScreenSize.Width, Core.ScreenSize.Height), New Color(0, 0, 0, 150))

        DrawPreview()
        If IsItem = False Then
            DrawMenuPokemon()
        Else
            DrawMenuItem()
        End If
    End Sub

    Private Sub DrawPreview()
        Dim T As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
        Canvas.DrawImageBorder(T, 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 70, 48, 96, 96))
        Core.SpriteBatch.Draw(Pokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2) - 70, 32, PokeSize, PokeSize), Color.White)
        If Not Pokemon.Item Is Nothing Then
            Core.SpriteBatch.Draw(Pokemon.Item.Texture, New Rectangle(CInt(Core.windowSize.Width / 2) - 70, 64, ItemSize, ItemSize), Color.White)
        End If
    End Sub

    Private Sub DrawMenuItem()
        Dim T As Texture2D

        For i = 0 To 2
            Dim Text As String = "Give"
            Select Case i
                Case 1
                    Text = "Take"
                Case 2
                    Text = "Back"
            End Select

            If i = index Then
                T = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                T = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Canvas.DrawImageBorder(T, 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 180, 180 + 128 * i, 320, 64))
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Text, New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(Text).X / 2) - 7, 215 + 128 * i), Color.Black)
        Next
    End Sub

    Private Sub DrawMenuPokemon()
        Dim T As Texture2D

        For i = 0 To 3
            Dim Text As String = "Switch"
            Select Case i
                Case 1
                    Text = "Summary"
                Case 2
                    Text = "Item"
                Case 3
                    Text = "Back"
            End Select

            If i = index Then
                T = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 48, 48, 48), "")
            Else
                T = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
            End If

            Canvas.DrawImageBorder(T, 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 180, 180 + 128 * i, 320, 64))
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Text, New Vector2(CInt(Core.windowSize.Width / 2) - CInt(FontManager.InGameFont.MeasureString(Text).X / 2) - 7, 215 + 128 * i), Color.Black)
        Next
    End Sub

End Class