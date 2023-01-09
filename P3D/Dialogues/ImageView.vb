Public Class ImageView

    Public Showing As Boolean = False
    Public Delay As Single = 0.0F

    Dim Texture As Texture2D

    Public Sub Show(ByVal Texture As Texture2D, ByVal Sound As String)
        Me.Delay = 8.0F
        Me.Showing = True

        If Sound <> "" Then
            SoundManager.PlaySound(Sound)
        End If
        Me.Texture = Texture
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
            Dim FrameSize As Size = New Size(288, 288)
            Dim ImageSize As Size = New Size(288, 288)
            Dim DefaultImageSize As Size = New Size(288, 288)
            If Texture.Width > Texture.Height Then
                FrameSize.Width = CInt(Math.Ceiling(Texture.Width / Texture.Height) * 288)
                ImageSize.Width = CInt(Texture.Width / Texture.Height * 288)
                Dim ScaleInt = 9
                While ImageSize.Width > Core.windowSize.Width - 288
                    ImageSize = New Size(CInt(Texture.Width / Texture.Height * 32 * ScaleInt), CInt(DefaultImageSize.Height / 9 * ScaleInt))
                    FrameSize = New Size(CInt(Math.Ceiling(Texture.Width / Texture.Height) * 32 * ScaleInt), CInt(DefaultImageSize.Height / 9 * ScaleInt))
                    ScaleInt -= 1
                End While
                While ImageSize.Width > FrameSize.Width - 32
                    FrameSize.Width += 32
                End While
            End If
            If Texture.Height > Texture.Width Then
                FrameSize.Height = CInt(Math.Floor(Texture.Height / Texture.Width) * 288)
                ImageSize.Height = CInt(Texture.Height / Texture.Width * 288)
                Dim ScaleInt = 9
                While ImageSize.Height > Core.windowSize.Height - 288
                    ImageSize = New Size(CInt(DefaultImageSize.Height / 9 * ScaleInt), CInt(Texture.Height / Texture.Width * 32 * ScaleInt))
                    FrameSize = New Size(CInt(DefaultImageSize.Width / 9 * ScaleInt), CInt(Math.Floor(Texture.Height / Texture.Width) * 32 * ScaleInt))
                    ScaleInt -= 1
                End While
                While ImageSize.Height > FrameSize.Height - 32
                    FrameSize.Height += 32
                End While
            End If

            Dim p As Vector2 = Core.GetMiddlePosition(New Size(FrameSize.Width + 64, FrameSize.Height + 64))
            Dim pImage As Vector2 = Core.GetMiddlePosition(New Size(ImageSize.Width + 64, ImageSize.Height + 64))

            Canvas.DrawImageBorder(TextureManager.GetTexture("GUI\Menus\Menu", New Rectangle(0, 0, 48, 48), ""), 2, New Rectangle(CInt(p.X - 32), CInt(p.Y - 32), FrameSize.Width + 64, FrameSize.Height + 64))

            Core.SpriteBatch.Draw(Me.Texture, New Rectangle(CInt(pImage.X + 16), CInt(pImage.Y + 16), ImageSize.Width, ImageSize.Height), Color.White)

            If Me.Delay = 0.0F Then
                Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Overworld\TextBox"), New Rectangle(CInt(p.X - 32) + FrameSize.Width + 64 - 16, CInt(p.Y - 32) + FrameSize.Height + 64 + 16, 16, 16), New Rectangle(0, 48, 16, 16), Color.White)
            End If
        End If
    End Sub

End Class