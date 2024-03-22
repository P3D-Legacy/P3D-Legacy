Public Class HatchEggScreen

    Inherits Screen

    Dim Pokemons As New List(Of Pokemon)
    Dim Backgroud As Texture2D
    Dim Egg As Texture2D

    Dim Stage As Integer = 0
    Dim delay As Single = 4.0F
    Dim size As Single = 0.0F

    Dim CanRename As Boolean = True
    Dim Message As String = ""

    Dim cPokemon As Pokemon

    Public Sub New(ByVal currentScreen As Screen, ByVal Pokemon As List(Of Pokemon), Optional CanRename As Boolean = True, Optional Message As String = "")
        Me.PreScreen = currentScreen
        PlayerStatistics.Track("Eggs hatched", 1)

        Me.Identification = Identifications.HatchEggScreen
        Me.Pokemons = Pokemon

        Me.cPokemon = Me.Pokemons(0)
        cPokemon.EggSteps = 0
        Core.Player.Pokemons.Add(cPokemon)

        Me.CanRename = CanRename
        Me.Message = Message

        Dim dexID As String = PokemonForms.GetPokemonDataFileName(cPokemon.Number, cPokemon.AdditionalData)
        If dexID.Contains("_") = False Then
            If PokemonForms.GetAdditionalDataForms(cPokemon.Number) IsNot Nothing AndAlso PokemonForms.GetAdditionalDataForms(cPokemon.Number).Contains(cPokemon.AdditionalData) Then
                dexID = cPokemon.Number & ";" & cPokemon.AdditionalData
            Else
                dexID = cPokemon.Number.ToString
            End If
        End If

        If cPokemon.IsShiny = True Then
            Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, dexID, 3)
        Else
            Core.Player.PokedexData = Pokedex.ChangeEntry(Core.Player.PokedexData, dexID, 2)
        End If

        Me.Pokemons.Remove(cPokemon)

        Level.OverworldPokemon.Visible = False

        Me.Backgroud = TextureManager.GetTexture("GUI\EggBreak", New Rectangle(0, 0, 256, 192), "")
        Me.Egg = GetEggTexture()

        MusicManager.PlayNoMusic()
    End Sub

    Private Function GetEggTexture() As Texture2D
        Return TextureManager.GetTexture("GUI\EggBreak", New Rectangle(0 + 28 * Stage, 192, 28, 30), "")
    End Function

    Public Overrides Sub Draw()
        Core.SpriteBatch.Draw(Me.Backgroud, Core.windowSize, Color.White)

        If Stage < 6 Then
            Core.SpriteBatch.Draw(Egg, New Rectangle(CInt(Core.windowSize.Width / 2 - Egg.Width), CInt(Core.windowSize.Height / 2 - Egg.Height), Egg.Width * 2, Egg.Height * 2), Color.White)
        End If
        Core.SpriteBatch.Draw(cPokemon.GetTexture(True), New Rectangle(CInt(Core.windowSize.Width / 2 - (cPokemon.GetTexture(True).Width * size / 2)), CInt(Core.windowSize.Height / 2 - (cPokemon.GetTexture(True).Height * size / 1.5F)), CInt(cPokemon.GetTexture(True).Width * size), CInt(cPokemon.GetTexture(True).Height * size)), Color.White)

        TextBox.Draw()
        ChooseBox.Draw()
    End Sub

    Public Overrides Sub Update()
        ChooseBox.Update()
        If ChooseBox.Showing = False Then
            TextBox.Update()
        End If

        If TextBox.Showing = False And ChooseBox.Showing = False Then
            If Stage < 6 Then
                If delay > 0.0F Then
                    delay -= 0.1F
                End If
                If delay <= 0.0F Then
                    delay = 4.0F
                    Stage += 1

                    If Stage = 6 Then
                        SoundManager.PlaySound("egg_hatch")
                    Else
                        SoundManager.PlaySound("Battle\Attacks\Normal\Pound")
                    End If

                    Egg = GetEggTexture()
                End If
            ElseIf Stage = 6 Then
                If size < 4.0F Then
                    size += 0.08F
                Else
                    MusicManager.Play("wild_defeat")
                    cPokemon.PlayCry()
                    SoundManager.PlaySound("success", True)
                    Stage = 7
                    If Message = "" Then
                        TextBox.Show("Congratulations!~Your egg hatched into~" & cPokemon.GetName() & "!")
                    Else
                        TextBox.Show(Message)
                    End If
                    If CanRename = True Then
                        TextBox.Show("Do you want to give~a nickname to the freshly~hatched " & cPokemon.GetName() & "?%Yes|No%", AddressOf Me.ResultFunction, False, False, TextBox.DefaultColor)
                    End If
                End If
            ElseIf Stage = 7 Then
                If Me.IsCurrentScreen = True Then
                    EndScene()
                    Stage = 8
                End If
            End If
        End If
    End Sub

    Private Sub ResultFunction(ByVal result As Integer)
        If result = 0 Then
            Core.SetScreen(New NameObjectScreen(Core.CurrentScreen, Me.cPokemon))
        End If
    End Sub

    Private Sub EndScene()
        If Pokemons.Count = 0 Then
            Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.White, False))
        Else
            Core.SetScreen(New HatchEggScreen(Me.PreScreen, Pokemons))
        End If
    End Sub

End Class