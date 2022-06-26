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

        Dim p As New Vector2(Core.windowSize.Width - (544), 32)

        Canvas.DrawImageBorder(TextureManager.GetTexture(mainTexture, New Rectangle(0, 0, 48, 48)), 2, New Rectangle(CInt(p.X), CInt(p.Y), 480, 352))
        Dim pokeTexture = Pokemon.GetMenuTexture()
        Core.SpriteBatch.Draw(pokeTexture, New Rectangle(CInt(p.X + 20), CInt(p.Y + 20), pokeTexture.Width * 2, pokeTexture.Height * 2), Color.White)
        Core.SpriteBatch.DrawString(FontManager.InGameFont, Pokemon.GetDisplayName(), New Vector2(p.X + 90, p.Y + 32), Color.Black)
        Core.SpriteBatch.DrawString(FontManager.InGameFont, " reached level " & Pokemon.Level & "!", New Vector2(p.X + 90 + FontManager.InGameFont.MeasureString(Pokemon.GetDisplayName()).X, p.Y + 41), Color.Black)

        Dim OldOffset As Integer = 160

        If Delay >= 3.0F Then
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("MaxHP") & ":", New Vector2(p.X + 32, p.Y + 84), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, OldStats(0).ToString(), New Vector2(p.X + 32 + OldOffset, p.Y + 84), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("Attack") & ":", New Vector2(p.X + 32, p.Y + 124), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, OldStats(1).ToString(), New Vector2(p.X + 32 + OldOffset, p.Y + 124), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("Defense") & ":", New Vector2(p.X + 32, p.Y + 164), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, OldStats(2).ToString(), New Vector2(p.X + 32 + OldOffset, p.Y + 164), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("Sp_Attack") & ":", New Vector2(p.X + 32, p.Y + 204), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, OldStats(3).ToString(), New Vector2(p.X + 32 + OldOffset, p.Y + 204), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("Sp_Defense") & ":", New Vector2(p.X + 32, p.Y + 244), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, OldStats(4).ToString(), New Vector2(p.X + 32 + OldOffset, p.Y + 244), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, Localization.GetString("Speed") & ":", New Vector2(p.X + 32, p.Y + 284), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, OldStats(5).ToString(), New Vector2(p.X + 32 + OldOffset, p.Y + 284), Color.Black)
        End If

        Dim NewOffset As Integer = 208
        If Delay >= 5.0F Then
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "+ " & newMaxHP, New Vector2(p.X + 32 + NewOffset, p.Y + 84), Color.Black)
        End If
        If Delay >= 5.5F Then
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "+ " & newAttack, New Vector2(p.X + 32 + NewOffset, p.Y + 124), Color.Black)
        End If
        If Delay >= 6.0F Then
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "+ " & newDefense, New Vector2(p.X + 32 + NewOffset, p.Y + 164), Color.Black)
        End If
        If Delay >= 6.5F Then
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "+ " & newSpAttack, New Vector2(p.X + 32 + NewOffset, p.Y + 204), Color.Black)
        End If
        If Delay >= 7.0F Then
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "+ " & newSpDefense, New Vector2(p.X + 32 + NewOffset, p.Y + 244), Color.Black)
        End If
        If Delay >= 7.5F Then
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "+ " & newSpeed, New Vector2(p.X + 32 + NewOffset, p.Y + 284), Color.Black)
        End If

        Dim ResultOffset As Integer = 272

        If Delay >= 9.0F Then
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "= " & Pokemon.MaxHP, New Vector2(p.X + 32 + ResultOffset, p.Y + 84), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "= " & Pokemon.Attack, New Vector2(p.X + 32 + ResultOffset, p.Y + 124), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "= " & Pokemon.Defense, New Vector2(p.X + 32 + ResultOffset, p.Y + 164), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "= " & Pokemon.SpAttack, New Vector2(p.X + 32 + ResultOffset, p.Y + 204), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "= " & Pokemon.SpDefense, New Vector2(p.X + 32 + ResultOffset, p.Y + 244), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, "= " & Pokemon.Speed, New Vector2(p.X + 32 + ResultOffset, p.Y + 284), Color.Black)
        End If
        If Delay >= 11.0F Then
            Dim newStat As Integer = 0
            newStat = newAttack + newDefense + newSpAttack + newMaxHP + newSpDefense + newSpeed

            Core.SpriteBatch.DrawString(FontManager.InGameFont, Pokemon.GetDisplayName() & " got a boost of " & newStat.ToString() & "!", New Vector2(p.X + 32, p.Y + 320), Color.DarkRed)
        End If
    End Sub

    Public Overrides Sub Update()
        Delay += 0.1F

        If Controls.Accept() = True Then
            If Delay >= 13.0F Then
                SoundManager.PlaySound("select")
                Core.SetScreen(Me.PreScreen)
            End If
        End If
    End Sub
End Class