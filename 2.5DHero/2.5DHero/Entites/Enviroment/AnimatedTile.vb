Public Class AnimatedTile

    Inherits Entity

    Shared TileTexturesTemp As New Dictionary(Of String, Texture2D)
    Dim TileTextureName As String = ""

    Dim tileAnimation As Animation
    Dim currentRectangle As New Rectangle(0, 0, 0, 0)

    Dim rows, columns, animationSpeed, startRow, startColumn As Integer


    Public Overloads Sub Initialize(ByVal AnimationData As List(Of Integer))
        MyBase.Initialize()
        rows = AnimationData(0)
        columns = AnimationData(1)
        animationSpeed = AnimationData(2)
        startRow = AnimationData(3)
        startColumn = AnimationData(4)

        tileAnimation = New Animation(TextureManager.GetTexture("Textures\Routes"), rows, columns, 16, 16, animationSpeed, startRow, startColumn)

        CreateTileTextureTemp()
    End Sub

    Public Shared Sub ClearAnimationResources()
        TileTexturesTemp.Clear()
    End Sub

    Private Sub CreateTileTextureTemp()
        'If Core.GameOptions.GraphicStyle = 1 Then
        Dim textureData As List(Of String) = Me.AdditionalValue.Split(CChar(",")).ToList()
        If textureData.Count >= 5 Then
            Dim r As New Rectangle(CInt(textureData(1)), CInt(textureData(2)), CInt(textureData(3)), CInt(textureData(4)))
            Dim texturePath As String = textureData(0)
            Me.TileTextureName = AdditionalValue
            If AnimatedTile.TileTexturesTemp.ContainsKey(AdditionalValue & "_0") = False Then
                For i = 0 To Me.rows - 1
                    For j = 0 To Me.columns - 1
                        AnimatedTile.TileTexturesTemp.Add(AdditionalValue & "_" & (j + columns * i).ToString, TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * j, r.Y + r.Height * i, r.Width, r.Height)))
                    Next
                Next
            End If
        Else
            Logger.Log(Logger.LogTypes.ErrorMessage, "AnimatedTile.vb: invalid AdditionalValue parameters")
        End If
        'End If
    End Sub

    Public Overrides Sub ClickFunction()
        Me.Surf()
    End Sub

    Public Overrides Function WalkAgainstFunction() As Boolean
        WalkOntoFunction()
        Return MyBase.WalkAgainstFunction()
    End Function

    Public Overrides Function WalkIntoFunction() As Boolean
        WalkOntoFunction()
        Return MyBase.WalkIntoFunction()
    End Function

    Public Overrides Sub WalkOntoFunction()
        If Screen.Level.Surfing = True Then
            Dim canSurf As Boolean = False

            For Each Entity As Entity In Screen.Level.Entities
                If Entity.boundingBox.Contains(Screen.Camera.GetForwardMovedPosition()) = ContainmentType.Contains Then
                    If Entity.EntityID = "Water" Then
                        canSurf = True
                    Else
                        If Entity.Collision = True Then
                            canSurf = False
                            Exit For
                        End If
                    End If
                End If
            Next

            If canSurf = True Then
                Screen.Camera.Move(1)

                Screen.Level.PokemonEncounter.TryEncounterWildPokemon(Me.Position, Spawner.EncounterMethods.Surfing, "")
            End If
        End If
    End Sub

    Private Sub Surf()
        If Screen.Camera.Turning = False Then
            If Screen.Level.Surfing = False Then
                If Badge.CanUseHMMove(Badge.HMMoves.Surf) = True Or GameController.IS_DEBUG_ACTIVE = True Or Core.Player.SandBoxMode = True Then
                    If Screen.ChooseBox.Showing = False Then
                        Dim canSurf As Boolean = False

                        If Me.ActionValue = 0 Then
                            For Each Entity As Entity In Screen.Level.Entities
                                If Entity.boundingBox.Contains(Screen.Camera.GetForwardMovedPosition()) = ContainmentType.Contains Then
                                    If Entity.EntityID = "Water" Then
                                        If Core.Player.SurfPokemon > -1 Then
                                            canSurf = True
                                        End If
                                    Else
                                        If Entity.Collision = True Then
                                            canSurf = False
                                            Exit For
                                        End If
                                    End If
                                End If
                            Next
                        End If

                        If Screen.Level.Riding = True Then
                            canSurf = False
                        End If

                        If canSurf = True Then
                            Dim message As String = "Do you want to Surf?%Yes|No%"
                            Screen.TextBox.Show(message, {Me}, True, True)
                            SoundManager.PlaySound("select")
                        End If
                    End If
                End If
            End If
        End If
    End Sub


    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) As Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.25F
    End Function

    Public Overrides Sub UpdateEntity()
        If Not tileAnimation Is Nothing Then
            tileAnimation.Update(0.01)
            If currentRectangle <> tileAnimation.TextureRectangle Then
                ChangeTexture()

                currentRectangle = tileAnimation.TextureRectangle
            End If
        End If

        MyBase.UpdateEntity()
    End Sub

    Private Sub ChangeTexture()
        'If Core.GameOptions.GraphicStyle = 1 Then
        If TileTexturesTemp.Count = 0 Then
            ClearAnimationResources()
            CreateTileTextureTemp()
        End If
        Dim i = tileAnimation.CurrentRow
            Dim j = tileAnimation.CurrentColumn
        Me.Textures(0) = AnimatedTile.TileTexturesTemp(TileTextureName & "_" & (j + columns * i))
        'End If
    End Sub

    Public Overrides Sub ResultFunction(ByVal Result As Integer)
        If Result = 0 Then
            Screen.TextBox.Show(Core.Player.Pokemons(Core.Player.SurfPokemon).GetDisplayName() & " used~Surf!", {Me})
            Screen.Level.Surfing = True
            Screen.Camera.Move(1)
            PlayerStatistics.Track("Surf used", 1)

            With Screen.Level.OwnPlayer
                Core.Player.TempSurfSkin = .SkinName

                Dim pokemonNumber As Integer = Core.Player.Pokemons(Core.Player.SurfPokemon).Number
                Dim SkinName As String = "[POKEMON|N]" & pokemonNumber & PokemonForms.GetOverworldAddition(Core.Player.Pokemons(Core.Player.SurfPokemon))
                If Core.Player.Pokemons(Core.Player.SurfPokemon).IsShiny = True Then
                    SkinName = "[POKEMON|S]" & pokemonNumber & PokemonForms.GetOverworldAddition(Core.Player.Pokemons(Core.Player.SurfPokemon))
                End If

                .SetTexture(SkinName, False)

                .UpdateEntity()

                SoundManager.PlayPokemonCry(pokemonNumber)

                If Screen.Level.IsRadioOn = False OrElse GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) = False Then
                    MusicManager.PlayMusic("surf", True)
                End If
            End With
        End If
    End Sub

    Public Overrides Sub Render()
        Dim setRasterizerState As Boolean = Me.Model.ID <> 0

        Me.Draw(Me.Model, Textures, setRasterizerState)
    End Sub

End Class