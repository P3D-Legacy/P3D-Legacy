Public Class DonationScreen

    Inherits Screen

    Dim mainTexture As Texture2D
    Dim scrollTexture As Texture2D

    Dim DonatorList() As String = {"Username99", "Merder222", "Felipe 2", "Kuro95", "WheresMyTea", "RandomBounty", "NumseFisK", "abcoanon", "SirMarty", "The_Merciless95", "adm0n", "Avaluque", "Duck Tard", "L3_Purr", "Derata", "TheFlipside", "Zippo", "Dirty Harry", "Chaos7777", "Sontee", "PsYcO363", "Sammyinside", "mickeystand1", "Tripsaur", "Fox405", "LoganKnez", "Jehowi", "Sedat", "Mischapus", "PeanutButter", "Nathan Wilson", "Fluffy", "Shou Liengod", "Gorogok", "Yoshina", "Hodsy Beats", "takenbycats", "sorixkhaos", "lordkango", "northway", "bloodeyezack", "gladdy16", "Paradetheday", "Gawerty", "Haydos709", "ShadyGame", "Mikolaj Nowicki", "Koolboyman", "TrainerStan", "carebear", "Bedders", "Matz", "ITAxDarko", "Rhyinn", "arthegon", "bmalfer", "Noah Cloud", "Matti", "Yrael", "Tornado9797", "Wilkojc", "Namu", "SACooper95", "nilllzz", "Nesasio", "beenlord", "Maria", "JohnnyRooks", "Calcifer", "Nyves", "Daniel Saavedra", "DannyM93", "The-amazing-blackstar", "DevoidLight", "OhSnapItsDavid", "Anvil555", "Clanor", "Liamash3", "Daysofthenew690", "Luan Nicholas", "Pushacher19", "Meowth", "DarknessYami", "Gameshark93", "Enethil", "Gnifle", "abovo", "p1neapple", "Destructosaur", "Darkfire", "Tim Dargan", "PrincessKooh", "Tyler Snyder", "hannes3120", "Raa", "Richard Tisher", "Brutalicious", "DarkLink", "Mpilemann", "PerrBearr", "robod", "Davey", "Colin_Mg", "Whitney", "mreh", "zXxLIPSxXz", "Xane", "LeeMan", "ekwilson79", "Darrin Danhauer", "AlessaGarnish", "Sola", "Luffy343", "Masasume", "Grabsak Turnkoff", "Sporkedmango", "Splint", "Mitchmack", "Pegasuraptor", "CrayonDoctor", "Olliewott", "Maizox", "Gamester565", "Michael", "Syrca", "PaperDanie2", "Gamerunner15", "Ashurnasirpal", "edward", "Gusty Glalie", "DracoHouston", "BakaOnibi", "Tj8805", "Lunick", "Karasu416", "Steven Sinclair", "Corbin Lair", "Michael Langen", "Diego López", "Sam Schultz", "Tom Bolen", "Lewis Thompson", "William Hafey", "Arno Wendorff", "Kim Nay", "Danie Daniels", "Joe Palacios", "Stuart Oxtoby", "Jack Mckenzie", "Michael Cutipa", "The Homies", "Alicia Barfoot", "Maintrain97", "Shinytish", "Michael Molina", "Edward Akus", "DanielRTRD"}
    Dim OffsetY As Integer = 0
    Const ScrollSpeed As Single = 35.0F

    Public Sub New(ByVal currentScreen As Screen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.DonationScreen
        mainTexture = TextureManager.GetTexture("House", New Rectangle(83, 98, 10, 12))

        Me.scrollTexture = TextureManager.GetTexture("GUI\Menus\Menu")

        ' Dim l As New List(Of String)
        Dim l As List(Of String) = DonatorList.ToList()

        ' Decided to remove the seemingly unnecessary randomization of the donor list. Re-implement if voted back in. - Tornado9797

        ' While oldL.Count > 0
        '   Dim i As Integer = Core.Random.Next(0, oldL.Count)
        '   l.Add(oldL(i))
        '   oldL.RemoveAt(i)
        ' End While

        l.Sort() ' Lets sort the list alphabetically
        l.Add("And many Anonymous donors!") ' Lets add this to the bottom of the list

        DonatorList = l.ToArray()
    End Sub

    Public Overrides Sub Update()
        If Controls.Up(True, True, True, True) = True Then
            OffsetY -= 1
        End If
        If Controls.Down(True, True, True, True) = True Then
            OffsetY += 1
        End If
        OffsetY = CInt(MathHelper.Clamp(OffsetY, 0, DonatorList.Count - 12))

        If Controls.Dismiss() = True Then
            Core.SetScreen(Me.PreScreen)
        End If
    End Sub


    Public Overrides Sub Draw()
        Me.PreScreen.Draw()
        Core.SpriteBatch.Draw(mainTexture, New Rectangle(CInt(Core.windowSize.Width / 2) - 285, 0, 570, 680), Color.White)

        Dim t As String = ""
        For i = OffsetY To 11 + OffsetY
            If i <> OffsetY Then
                t &= Environment.NewLine & Environment.NewLine
            End If
            If DonatorList.Count - 1 >= i Then
                t &= DonatorList(i)
            End If
        Next

        If DonatorList.Count > 12 Then
            Canvas.DrawScrollBar(New Vector2(CInt(Core.windowSize.Width / 2) + 180, 100), DonatorList.Count, 12, OffsetY, New Size(4, 500), False, TextureManager.GetTexture(scrollTexture, New Rectangle(112, 12, 1, 1)), TextureManager.GetTexture(scrollTexture, New Rectangle(113, 12, 1, 1)))
        End If

        Core.SpriteBatch.DrawString(FontManager.MainFont, t, New Vector2(CInt(Core.windowSize.Width / 2) - 180, 100), Color.Black)
        Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2) - 285, 0, 570, 57), New Color(56, 56, 56))
        Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.Translate("donation.donators") & ": ", New Vector2(CInt(Core.windowSize.Width / 2) - FontManager.MainFont.MeasureString(Localization.Translate("donation.donators") & ":").X / 2, 20), Color.White)
        Core.SpriteBatch.DrawString(FontManager.MainFont, Localization.Translate("donation.close"), New Vector2(CInt(Core.windowSize.Width / 2) - FontManager.MainFont.MeasureString(Localization.Translate("donation.close")).X / 2, 640), Color.White)
    End Sub

End Class