Public Class LeadPokemonInfoScreen

    Inherits Screen

    Dim Pokemon As Pokemon

    Dim index As Integer = 0
    Dim mainTexture As Texture2D
    Public Delegate Sub DoStuff(ByVal PokeIndex As Integer)
    Dim ChoosePokemon As DoStuff
    Dim pokeIndex As Integer = 0

    Dim PokeSize As Integer = 128
    Dim ItemSize As Integer = 0

    Public Sub New(ByVal currentScreen As Screen, ByVal PokeIndex As Integer, ByVal ChoosePokemon As DoStuff)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.BattlePokemonScreen
        Me.ChoosePokemon = ChoosePokemon
        Me.pokeIndex = PokeIndex
        Me.Pokemon = Core.Player.Pokemons(PokeIndex)


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
            Me.index = 2
        ElseIf Me.index > 2 Then
            Me.index = 0
        End If

        If PokeSize < 128 Then
            PokeSize += 10
        End If
        If ItemSize > 0 Then
            ItemSize -= 10
        End If

        If Controls.Dismiss(True, True) = True Then
            Core.SetScreen(Me.PreScreen)
        End If

        If Controls.Accept(True, True) = True Then
            Select Case index
                Case 0
                    Core.SetScreen(Me.PreScreen)
                    Me.ChoosePokemon(Me.pokeIndex)
                Case 1
                    Core.SetScreen(New PokemonStatusScreen(Me, Me.pokeIndex, {}, Core.Player.Pokemons(Me.pokeIndex), True))
                Case 2
                    Core.SetScreen(Me.PreScreen)
            End Select
        End If
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        Canvas.DrawRectangle(New Rectangle(0, 0, Core.ScreenSize.Width, Core.ScreenSize.Height), New Color(0, 0, 0, 150))

        DrawPreview()
        DrawMenuPokemon()

    End Sub

    Private Sub DrawPreview() 'Draws the preview of the pokemon and it's holding item.
        Dim T As Texture2D = TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), "")
        Canvas.DrawImageBorder(T, 2, New Rectangle(CInt(Core.windowSize.Width / 2) - 70, 48, 96, 96))
        Core.SpriteBatch.Draw(Pokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2) - 70, 32, PokeSize, PokeSize), Color.White)
        If Not Pokemon.Item Is Nothing Then
            Core.SpriteBatch.Draw(Pokemon.Item.Texture, New Rectangle(CInt(Core.windowSize.Width / 2) - 70, 64, ItemSize, ItemSize), Color.White)
        End If
    End Sub

    Private Sub DrawMenuPokemon()
        Dim T As Texture2D
        For i = 0 To 2
            Dim Text As String = "Pick"
            Select Case i
                Case 1
                    Text = "Summary"
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

End Class