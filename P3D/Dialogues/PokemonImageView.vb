Public Class PokemonImageView

    Public Showing As Boolean = False
    Public Delay As Single = 0.0F

    Dim Texture As Texture2D
    Dim front As Boolean = True

    Public Sub Show(ByVal ID As String, ByVal shiny As Boolean, ByVal front As Boolean)
        Dim PokemonID As String = ID.GetSplit(0)
        Dim PokemonAddition As String = "xXx"
        If PokemonID.Contains("_") Then
            PokemonAddition = PokemonForms.GetAdditionalValueFromDataFile(ID.GetSplit(0))
            PokemonID = ID.GetSplit(0).GetSplit(0, "_")
        End If
        If PokemonID.Contains(";") Then
            PokemonAddition = ID.GetSplit(0).GetSplit(1, ";")
            PokemonID = ID.GetSplit(0).GetSplit(0, ";")
        End If
        Dim p As Pokemon = Pokemon.GetPokemonByID(CInt(PokemonID), PokemonAddition, True)
        p.PlayCry()

        Me.Delay = 8.0F
        Me.Showing = True

        p.IsShiny = shiny

        Me.Texture = p.GetTexture(front)
    End Sub

    Public Sub Show(ByVal Pokemon As Pokemon, ByVal front As Boolean)
        Dim p As Pokemon = Pokemon
        p.PlayCry()

        Me.Delay = 8.0F
        Me.Showing = True

        Me.Texture = p.GetTexture(front)
    End Sub

    Public Sub Show(ByVal Texture As Texture2D)
        Me.Texture = Texture

        Me.Delay = 8.0F
        Me.Showing = True
    End Sub

    Public Sub Update()
        If Delay > 0.0F Then
            Delay -= 0.1F

            If Delay <= 0.0F Then
                Delay = 0.0F
            End If
        ElseIf Delay = 0.0F Then
            If Controls.Accept() = True Or Controls.Dismiss() = True Then
                Me.Showing = False
                SoundManager.PlaySound("select")
            End If
        End If
    End Sub

    Public Sub Draw()
        If Me.Showing = True Then
            Dim p As Vector2 = Core.GetMiddlePosition(New Size(320, 320))

            Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 2, New Rectangle(CInt(p.X), CInt(p.Y), 320, 320))

            If Me.Delay = 0.0F Then
                Core.SpriteBatch.DrawInterface(TextureManager.GetTexture("GUI\Overworld\ImageView"), New Rectangle(CInt(p.X) + 144 + 160 + 16, CInt(p.Y) + 144 + 160 + 32, 16, 16), New Rectangle(0, 0, 16, 16), Color.White)
            End If

            Core.SpriteBatch.Draw(Me.Texture, New Rectangle(CInt(p.X + 160 + 16 - MathHelper.Min(Me.Texture.Width * 3, 288) / 2), CInt(p.Y + 160 + 16 - MathHelper.Min(Me.Texture.Height * 3, 288) / 2), MathHelper.Min(Me.Texture.Width * 3, 288), MathHelper.Min(Me.Texture.Height * 3, 288)), Color.White)

        End If
    End Sub

End Class