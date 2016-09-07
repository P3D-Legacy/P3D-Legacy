Public Class PokemonImageView

    Public Showing As Boolean = False
    Public Delay As Single = 0.0F

    Dim Texture As Texture2D
    Dim front As Boolean = True

    Public Sub Show(ByVal ID As Integer, ByVal shiny As Boolean, ByVal front As Boolean)
        Dim p As Pokemon = Pokemon.GetPokemonByID(ID)
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
            End If
        End If
    End Sub

    Public Sub Draw()
        If Me.Showing = True Then
            Dim p As Vector2 = Core.GetMiddlePosition(New Size(320, 320))

            Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 2, New Rectangle(CInt(p.X), CInt(p.Y), 320, 320))

            Core.SpriteBatch.Draw(Me.Texture, New Rectangle(CInt(p.X) + 36, CInt(p.Y) + 20, 280, 280), Color.White)

            If Me.Delay = 0.0F Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Overworld\TextBox"), New Rectangle(CInt(p.X) + 144 + 160, CInt(p.Y) + 144 + 160 + 32, 16, 16), New Rectangle(0, 48, 16, 16), Color.White)
            End If
        End If
    End Sub

End Class