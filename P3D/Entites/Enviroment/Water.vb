Public Class Water

    Inherits Entity

    Public Shared WaterTexturesTemp As New Dictionary(Of String, Texture2D)
    Shared SetWaterTextures As Boolean = True
    Dim waterTextureName As String = ""

    Dim WaterAnimation As Animation
    Dim currentRectangle As New Rectangle(0, 0, 0, 0)

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        WaterAnimation = New Animation(TextureManager.GetTexture("Textures\Routes"), 1, 3, 16, 16, GameModeManager.ActiveGameMode.WaterSpeed, 15, 0)

        CreateWaterTextureTemp()
        ChangeTexture()
    End Sub

    Public Shared Sub ClearAnimationResources()
        WaterTexturesTemp.Clear()
    End Sub

    Public Shared Sub AddDefaultWaterAnimationResources()
        Water.WaterTexturesTemp.Add("_0", TextureManager.GetTexture("Routes", New Rectangle(0, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_1", TextureManager.GetTexture("Routes", New Rectangle(20, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_2", TextureManager.GetTexture("Routes", New Rectangle(40, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_3", TextureManager.GetTexture("Routes", New Rectangle(60, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_4", TextureManager.GetTexture("Routes", New Rectangle(80, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_5", TextureManager.GetTexture("Routes", New Rectangle(100, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_6", TextureManager.GetTexture("Routes", New Rectangle(120, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_7", TextureManager.GetTexture("Routes", New Rectangle(140, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_8", TextureManager.GetTexture("Routes", New Rectangle(160, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_9", TextureManager.GetTexture("Routes", New Rectangle(180, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_10", TextureManager.GetTexture("Routes", New Rectangle(200, 220, 20, 20)))
        Water.WaterTexturesTemp.Add("_11", TextureManager.GetTexture("Routes", New Rectangle(220, 220, 20, 20)))
    End Sub
    Private Sub CreateWaterTextureTemp()
        Dim textureData As List(Of String) = Me.AdditionalValue.Split(CChar(",")).ToList()
        If textureData.Count >= 5 Then
            Dim RotationOffsetVertical As Boolean = False '''False = Horizontal, True = Vertical
            If textureData.Count >= 6 Then
                RotationOffsetVertical = CBool(textureData(5))
            End If
            Dim r As New Rectangle(CInt(textureData(1)), CInt(textureData(2)), CInt(textureData(3)), CInt(textureData(4)))
            Dim texturePath As String = textureData(0)
            Me.waterTextureName = AdditionalValue
            If RotationOffsetVertical = True Then
                If Water.WaterTexturesTemp.ContainsKey(AdditionalValue & "_0") = False Then
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_0", TextureManager.GetTexture(texturePath, New Rectangle(r.X, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_1", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_2", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 2, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_3", TextureManager.GetTexture(texturePath, New Rectangle(r.X, r.Y + r.Height, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_4", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width, r.Y + r.Height, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_5", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 2, r.Y + r.Height, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_6", TextureManager.GetTexture(texturePath, New Rectangle(r.X, r.Y + r.Height * 2, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_7", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width, r.Y + r.Height * 2, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_8", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 2, r.Y + r.Height * 2, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_9", TextureManager.GetTexture(texturePath, New Rectangle(r.X, r.Y + r.Height * 3, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_10", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width, r.Y + r.Height * 3, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_11", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 2, r.Y + r.Height * 3, r.Width, r.Height)))
                End If
            Else
                If Water.WaterTexturesTemp.ContainsKey(AdditionalValue & "_0") = False Then
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_0", TextureManager.GetTexture(texturePath, New Rectangle(r.X, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_1", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_2", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 2, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_3", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 3, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_4", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 4, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_5", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 5, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_6", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 6, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_7", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 7, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_8", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 8, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_9", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 9, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_10", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 10, r.Y, r.Width, r.Height)))
                    Water.WaterTexturesTemp.Add(AdditionalValue & "_11", TextureManager.GetTexture(texturePath, New Rectangle(r.X + r.Width * 11, r.Y, r.Width, r.Height)))
                End If
            End If
        Else
            If Water.WaterTexturesTemp.ContainsKey("_0") = False Then
                Water.WaterTexturesTemp.Add("_0", TextureManager.GetTexture("Routes", New Rectangle(0, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_1", TextureManager.GetTexture("Routes", New Rectangle(20, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_2", TextureManager.GetTexture("Routes", New Rectangle(40, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_3", TextureManager.GetTexture("Routes", New Rectangle(60, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_4", TextureManager.GetTexture("Routes", New Rectangle(80, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_5", TextureManager.GetTexture("Routes", New Rectangle(100, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_6", TextureManager.GetTexture("Routes", New Rectangle(120, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_7", TextureManager.GetTexture("Routes", New Rectangle(140, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_8", TextureManager.GetTexture("Routes", New Rectangle(160, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_9", TextureManager.GetTexture("Routes", New Rectangle(180, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_10", TextureManager.GetTexture("Routes", New Rectangle(200, 220, 20, 20)))
                Water.WaterTexturesTemp.Add("_11", TextureManager.GetTexture("Routes", New Rectangle(220, 220, 20, 20)))
            End If
        End If
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
                Screen.Level.PokemonEncounter.TryEncounterWildPokemon(Me.Position, Spawner.EncounterMethods.Surfing, "")

                If CType(Screen.Camera, OverworldCamera)._debugWalk = False Then
                    Screen.Camera.Move(1)
                End If

            End If
        End If
    End Sub

    Private Sub Surf()
        If Screen.Camera.Turning = False Then
            If Screen.Level.Surfing = False AndAlso CType(CurrentScreen, OverworldScreen).ActionScript.IsReady = True Then
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
                            Dim message As String = Localization.GetString("fieldmove_surf_type0", "Do you want to Surf?") & "%" & Localization.GetString("global_yes", "Yes") & "|" & Localization.GetString("global_no", "No") & "%"
                            Dim waterType As String = ""
                            If Me.AdditionalValue.CountSeperators(",") >= 6 Then
                                waterType = Me.AdditionalValue.GetSplit(5)
                            Else
                                waterType = Me.AdditionalValue
                            End If
                            Select Case waterType.ToLower()
                                Case "0", ""
                                    message = Localization.GetString("fieldmove_surf_type0", "Do you want to Surf?") & "%" & Localization.GetString("global_yes", "Yes") & "|" & Localization.GetString("global_no", "No") & "%"
                                Case "1", "sea", "water"
                                    message = Localization.GetString("fieldmove_surf_type1", "The water looks still~and deep.~Do you want to Surf?") & "%" & Localization.GetString("global_yes", "Yes") & "|" & Localization.GetString("global_no", "No") & "%"
                                Case "2", "lake", "pond"
                                    message = Localization.GetString("fieldmove_surf_type2", "This lake is~calm and shallow~Do you want to Surf?") & "%" & Localization.GetString("global_yes", "Yes") & "|" & Localization.GetString("global_no", "No") & "%"
                            End Select

                            Screen.TextBox.Show(message, {Me}, True, True)
                            SoundManager.PlaySound("select")
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) As Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.2F
    End Function

    Public Overrides Sub UpdateEntity()
        If Core.GameOptions.GraphicStyle = 1 Then
            If Not WaterAnimation Is Nothing Then
                WaterAnimation.Update(0.005)
                If currentRectangle <> WaterAnimation.TextureRectangle Then
                    ChangeTexture()

                    currentRectangle = WaterAnimation.TextureRectangle
                End If
            End If
        End If
        MyBase.UpdateEntity()
    End Sub

    Private Sub ChangeTexture()
        If WaterTexturesTemp.Count = 0 OrElse Water.WaterTexturesTemp.ContainsKey("_0") = False OrElse Water.WaterTexturesTemp.ContainsKey(AdditionalValue & "_0") = False Then
            ClearAnimationResources()
            AddDefaultWaterAnimationResources()
            CreateWaterTextureTemp()
        End If

        Select Case Me.Rotation.Y
            Case 0, MathHelper.TwoPi
                Select Case WaterAnimation.CurrentColumn
                    Case 0
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_0")
                    Case 1
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_1")
                    Case 2
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_2")
                End Select
            Case MathHelper.Pi * 0.5F
                Select Case WaterAnimation.CurrentColumn
                    Case 0
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_3")
                    Case 1
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_4")
                    Case 2
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_5")
                End Select
            Case MathHelper.Pi
                Select Case WaterAnimation.CurrentColumn
                    Case 0
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_6")
                    Case 1
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_7")
                    Case 2
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_8")
                End Select
            Case MathHelper.Pi * 1.5
                Select Case WaterAnimation.CurrentColumn
                    Case 0
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_9")
                    Case 1
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_10")
                    Case 2
                        Me.Textures(0) = Water.WaterTexturesTemp(waterTextureName & "_11")
                End Select
        End Select
    End Sub

    Public Overrides Sub ResultFunction(ByVal Result As Integer)
        If Result = 0 Then
            Screen.TextBox.Show(Core.Player.Pokemons(Core.Player.SurfPokemon).GetDisplayName() & " " & Localization.GetString("fieldmove_surf_used", "used~Surf!"), {Me})
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

                SoundManager.PlayPokemonCry(pokemonNumber, PokemonForms.GetCrySuffix(Core.Player.Pokemons(Core.Player.SurfPokemon)))

                If Screen.Level.IsRadioOn = False OrElse GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) = False Then
                    MusicManager.Play("surf", True)
                End If
            End With
        End If
    End Sub

    Public Overrides Sub Render()
        Dim setRasterizerState As Boolean = Me.BaseModel.ID <> 0
        If Me.Model Is Nothing Then
            Me.Draw(Me.BaseModel, Textures, setRasterizerState)
        Else
            UpdateModel()
            Draw(Me.BaseModel, Me.Textures, True, Me.Model)
        End If
    End Sub

End Class