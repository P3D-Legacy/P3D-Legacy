Public Class PokemonScreenV2

    Inherits WindowScreen

    Dim texture As Texture2D
    Dim battleTexture As Texture2D

    Public Sub New(ByVal currentScreen As Screen, ByVal startSelectionindex As Integer)
        MyBase.New(currentScreen, Identifications.PokemonScreen, "Pokémon")

        Me.texture = TextureManager.GetTexture("GUI\Menus\General")
        Me.battleTexture = TextureManager.GetTexture("GUI\Battle\Interface")
    End Sub

    Public Overrides Sub Update()
        MyBase.Update()

        If Controls.Dismiss() = True Then
            Me.CloseScreen()
        End If
    End Sub

    Public Overrides Sub Draw()
        Me.PreScreen.Draw()

        MyBase.Draw()

        If FadedIn = True Then
            'Draw first Pokémon:
            DrawFirstPokemon()
        End If
    End Sub

    Private Sub DrawFirstPokemon()
        Dim poke As Pokemon = Core.Player.Pokemons(0)

        Dim topLeft As Vector2 = Me.GetPositionInWindowTopLeft(0, 0)

        'Shadow:
        Canvas.DrawRectangle(Me.OffsetRectangle(New Rectangle(topLeft.X.ToInteger() + 43, topLeft.Y.ToInteger() + 123, 80 * 3, 32 * 3)), New Color(0, 0, 0, 90))

        'Box:
        Core.SpriteBatch.Draw(Me.texture,
                              Me.OffsetRectangle(New Rectangle(topLeft.X.ToInteger() + 40, topLeft.Y.ToInteger() + 120, 80 * 3, 32 * 3)),
                              New Rectangle(48, 16, 80, 32),
                              Color.White)

        Dim pTexture As Texture2D = poke.GetMenuTexture()

        'Pokemon:
        Core.SpriteBatch.Draw(pTexture, Me.OffsetRectangle(New Rectangle(topLeft.X.ToInteger() + 40, topLeft.Y.ToInteger() + 120, pTexture.Width * 2, pTexture.Height * 2)), Color.White)
        'Name:
        Core.SpriteBatch.DrawString(FontManager.MiniFont, poke.GetDisplayName(), Me.OffsetVector(New Vector2(topLeft.X + 108, topLeft.Y + 134)), Color.White, 0.0F, Vector2.Zero, 1.2F, SpriteEffects.None, 0.0F)
        'Level:
        Core.SpriteBatch.DrawString(FontManager.MiniFont, "Lv. " & poke.Level.ToString(), Me.OffsetVector(New Vector2(topLeft.X + 44, topLeft.Y + 188)), Color.White)

        'Item:
        If poke.Item IsNot Nothing Then
            Core.SpriteBatch.Draw(poke.Item.Texture, Me.OffsetRectangle(New Rectangle(topLeft.X.ToInteger() + 80, topLeft.Y.ToInteger() + 160, 24, 24)), Color.White)
        End If

        '"HP":
        Core.SpriteBatch.Draw(Me.battleTexture,
                              Me.OffsetRectangle(New Rectangle(topLeft.X.ToInteger() + 108, topLeft.Y.ToInteger() + 170, 13 * 2, 6 * 2)),
                              New Rectangle(6, 37, 13, 6),
                              Color.White)
        'HP bar:
        Dim HPpercentage As Single = (100.0F / poke.MaxHP) * poke.HP
        Dim HPlength As Integer = CInt(Math.Ceiling(140 / 100 * HPpercentage.Clamp(1, 999)))

        If poke.HP = 0 Then
            HPlength = 0
        Else
            If HPlength <= 0 Then
                HPlength = 1
            End If
        End If
        If poke.HP = poke.MaxHP Then
            HPlength = 140
        Else
            If HPlength = 140 Then
                HPlength = 139
            End If
        End If

        Dim cX As Integer = 0
        If HPpercentage <= 50.0F And HPpercentage > 15.0F Then
            cX = 2
        ElseIf HPpercentage <= 15.0F Then
            cX = 4
        End If

        Dim pos As Vector2 = OffsetVector(New Vector2(topLeft.X + 136, topLeft.Y + 170))

        Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X), CInt(pos.Y), 140, 12), New Rectangle(19, 37, 70, 6), Color.White)

        If HPlength > 0 Then
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X), CInt(pos.Y), 2, 12), New Rectangle(cX, 37, 1, 6), Color.White)
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + 2, CInt(pos.Y), HPlength - 4, 12), New Rectangle(cX + 1, 37, 1, 6), Color.White)
            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Battle\Interface"), New Rectangle(CInt(pos.X) + HPlength - 2, CInt(pos.Y), 2, 12), New Rectangle(cX, 37, 1, 6), Color.White)
        End If

        'HP text:
        Core.SpriteBatch.DrawString(FontManager.MiniFont, poke.HP & " / " & poke.MaxHP, Me.OffsetVector(New Vector2(topLeft.X + 140, topLeft.Y + 188)), Color.White)

        'Status:
        Dim StatusTexture As Texture2D = BattleStats.GetStatImage(poke.Status)
        If Not StatusTexture Is Nothing Then
            Core.SpriteBatch.Draw(StatusTexture, Me.OffsetRectangle(New Rectangle(CInt(topLeft.X + 235), CInt(topLeft.Y + 192), 38, 12)), Color.White)
        End If
    End Sub

End Class