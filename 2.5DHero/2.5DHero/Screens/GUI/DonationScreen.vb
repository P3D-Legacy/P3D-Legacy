Public Class DonationScreen

    Inherits Screen

    Dim mainTexture As Texture2D
    Dim scrollTexture As Texture2D

    Dim DonatorList() As String = {"Steven Sinclair", "Diego López", "Corbin Lair", "Michael Langen", "Diego Lopez", "Sam Schultz", "Tom Bolen", "Lewis Thompson", "William Hafey", "Edward Akus", "Arno Wendorff", "Kim Nay", "Danie Daniels", "Joe Palacios", "Stuart Oxtoby", "Jack Mckenzie", "Michael Cutipa", "The Homies", "Alicia Barfoot", "Maintrain97", "Shinytish", "Michael Molina", "Edward Akus"}
    Dim OffsetY As Integer = 0
    Const ScrollSpeed As Single = 35.0F

    Public Sub New(ByVal currentScreen As Screen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.DonationScreen
        mainTexture = TextureManager.GetTexture("House", New Rectangle(83, 98, 10, 12))

        Me.scrollTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        Dim l As New List(Of String)
        Dim oldL As List(Of String) = DonatorList.ToList()

        While oldL.Count > 0
            Dim i As Integer = Core.Random.Next(0, oldL.Count)
            l.Add(oldL(i))
            oldL.RemoveAt(i)
        End While

        l.Add("Missingno.")

        DonatorList = l.ToArray()
    End Sub

    Public Overrides Sub Update()
        If Controls.Up(True, True, True, True) = True Then
            OffsetY -= 1
        End If
        If Controls.Down(True, True, True, True) = True Then
            OffsetY += 1
        End If
        OffsetY = CInt(MathHelper.Clamp(OffsetY, 0, DonatorList.Count - 13))

        If Controls.Dismiss() = True Then
            Core.SetScreen(Me.PreScreen)
        End If
    End Sub


    Public Overrides Sub Draw()
        Me.PreScreen.Draw()
        Core.SpriteBatch.Draw(mainTexture, New Rectangle(CInt(Core.windowSize.Width / 2) - 285, 0, 570, 680), Color.White)

        Dim t As String = ""
        For i = OffsetY To 12 + OffsetY
            If i <> OffsetY Then
                t &= vbNewLine & vbNewLine
            End If
            If DonatorList.Count - 1 >= i Then
                t &= DonatorList(i)
            End If
        Next

        If DonatorList.Count > 13 Then
            Canvas.DrawScrollBar(New Vector2(CInt(Core.windowSize.Width / 2) + 180, 100), DonatorList.Count, 13, OffsetY, New Size(4, 500), False, TextureManager.GetTexture(scrollTexture, New Rectangle(112, 12, 1, 1)), TextureManager.GetTexture(scrollTexture, New Rectangle(113, 12, 1, 1)))
        End If

        Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2) - 180, 100), Color.Black)
        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2) - 285, 0, 570, 57), New Color(56, 56, 56))
        Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("donation_screen_donators") & ": ", New Vector2(CInt(Core.windowSize.Width / 2) - FontManager.MainFont.MeasureString("Donators:").X / 2, 20), Color.White)
        Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.GetString("donation_screen_backadvice"), New Vector2(CInt(Core.windowSize.Width / 2) - FontManager.MainFont.MeasureString("Press E to close").X / 2, 640), Color.White)
    End Sub

End Class