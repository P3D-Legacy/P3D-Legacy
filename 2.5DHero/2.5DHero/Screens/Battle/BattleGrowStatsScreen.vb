Public Class BattleGrowStatsScreen

    Inherits Screen

    Dim mainTexture As Texture2D

    Dim newMaxHP As Integer = 0
    Dim newAttack As Integer = 0
    Dim newDefense As Integer = 0
    Dim newSpDefense As Integer = 0
    Dim newSpAttack As Integer = 0
    Dim newSpeed As Integer = 0

    Dim Delay As Single = 0.0F

    Dim Pokemon As Pokemon = Nothing
    Dim OldStats() As Integer

    Public Sub New(ByVal currentScreen As Screen, ByVal p As Pokemon, ByVal OldStats() As Integer)
        Me.mainTexture = TextureManager.GetTexture("GUI\Menus\Menu")
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.BattleGrowStatsScreen

        newMaxHP = p.MaxHP - OldStats(0)
        newAttack = p.Attack - OldStats(1)
        newDefense = p.Defense - OldStats(2)
        newSpAttack = p.SpAttack - OldStats(3)
        newSpDefense = p.SpDefense - OldStats(4)
        newSpeed = p.Speed - OldStats(5)

        Me.Pokemon = p
        Me.OldStats = OldStats
    End Sub

    Public Overrides Sub Draw()
        PreScreen.Draw()

        Dim CanvasTexture As Texture2D = TextureManager.TextureRectangle(mainTexture, New Rectangle(0, 0, 48, 48))

        Dim p As New Vector2(Core.windowSize.Width - (544), 32)

        Canvas.DrawImageBorder(CanvasTexture, 2, New Rectangle(CInt(p.X), CInt(p.Y), 480, 352))

        Core.SpriteBatch.Draw(Pokemon.GetMenuTexture(), New Rectangle(CInt(p.X + 20), CInt(p.Y + 20), 64, 64), Color.White)
        Core.SpriteBatch.DrawString(FontManager.InGameFont, Pokemon.GetDisplayName(), New Vector2(p.X + 90, p.Y + 32), Color.Black)
        Core.SpriteBatch.DrawString(FontManager.MiniFont, " reached level " & Pokemon.Level & "!", New Vector2(p.X + 90 + FontManager.InGameFont.MeasureString(Pokemon.GetDisplayName()).X, p.Y + 41), Color.Black)

        If Delay >= 3.0F Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Max HP:      " & OldStats(0).ToString(), New Vector2(p.X + 32, p.Y + 84), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Attack:      " & OldStats(1).ToString(), New Vector2(p.X + 32, p.Y + 124), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Defense:     " & OldStats(2).ToString(), New Vector2(p.X + 32, p.Y + 164), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Sp Attack:   " & OldStats(3).ToString(), New Vector2(p.X + 32, p.Y + 204), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Sp Defense:  " & OldStats(4).ToString(), New Vector2(p.X + 32, p.Y + 244), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "Speed:       " & OldStats(5).ToString(), New Vector2(p.X + 32, p.Y + 284), Color.Black)
        End If
        If Delay >= 5.0F Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "+ " & newMaxHP, New Vector2(p.X + 200, p.Y + 84), Color.Black)
        End If
        If Delay >= 5.5F Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "+ " & newAttack, New Vector2(p.X + 200, p.Y + 124), Color.Black)
        End If
        If Delay >= 6.0F Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "+ " & newDefense, New Vector2(p.X + 200, p.Y + 164), Color.Black)
        End If
        If Delay >= 6.5F Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "+ " & newSpAttack, New Vector2(p.X + 200, p.Y + 204), Color.Black)
        End If
        If Delay >= 7.0F Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "+ " & newSpDefense, New Vector2(p.X + 200, p.Y + 244), Color.Black)
        End If
        If Delay >= 7.5F Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "+ " & newSpeed, New Vector2(p.X + 200, p.Y + 284), Color.Black)
        End If
        If Delay >= 9.0F Then
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "= " & Pokemon.MaxHP, New Vector2(p.X + 252, p.Y + 84), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "= " & Pokemon.Attack, New Vector2(p.X + 252, p.Y + 124), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "= " & Pokemon.Defense, New Vector2(p.X + 252, p.Y + 164), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "= " & Pokemon.SpAttack, New Vector2(p.X + 252, p.Y + 204), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "= " & Pokemon.SpDefense, New Vector2(p.X + 252, p.Y + 244), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.MiniFont, "= " & Pokemon.Speed, New Vector2(p.X + 252, p.Y + 284), Color.Black)
        End If
        If Delay >= 11.0F Then
            Dim newStat As Integer = 0
            newStat = newAttack + newDefense + newSpAttack + newMaxHP + newSpDefense + newSpeed

            Core.SpriteBatch.DrawString(FontManager.MiniFont, Pokemon.GetDisplayName() & " got a boost of " & newStat.ToString() & "!", New Vector2(p.X + 32, p.Y + 320), Color.DarkRed)
        End If
    End Sub

    Public Overrides Sub Update()
        Delay += 0.1F

        If Controls.Accept() = True Then
            If Delay >= 13.0F Then
                Core.SetScreen(Me.PreScreen)
            End If
        End If
    End Sub

End Class