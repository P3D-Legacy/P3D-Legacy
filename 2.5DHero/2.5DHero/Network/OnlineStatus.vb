Public Class OnlineStatus

    Public Shared Sub Draw()
        If JoinServerScreen.Online = True And ConnectScreen.Connected = True Then
            If KeyBoardHandler.KeyDown(KeyBindings.OnlineStatusKey) = True Then
                Dim playerList = Core.ServersManager.PlayerCollection

                Dim width As Integer = 1
                Dim height As Integer = playerList.Count

                If height > 10 Then
                    width = CInt(Math.Ceiling(height / 10))
                End If
                height = 10

                Dim startX As Integer = CInt(Core.windowSize.Width / 2 - ((width * 256) / 2))
                Dim startY As Integer = 120

                For x = 1 To width
                    For y = 1 To height
                        Canvas.DrawRectangle(New Rectangle(startX + (x - 1) * 256, startY + (y - 1) * 40, 256, 40), New Color(0, 0, 0, 150))
                        If playerList.Count - 1 >= (x - 1) * 10 + (y - 1) Then
                            Dim name As String = playerList((x - 1) * 10 + (y - 1)).Name
                            Dim c As Color = Color.White
                            If playerList((x - 1) * 10 + (y - 1)).ServersID = Core.ServersManager.ID Then
                                name = Core.Player.Name
                                c = Chat.OwnColor
                            Else
                                If Core.Player.IsGamejoltSave = True Then
                                    Dim GJID As String = playerList((x - 1) * 10 + (y - 1)).GameJoltId
                                    If GJID <> "" AndAlso Core.GameJoltSave.Friends.Contains(GJID) = True Then
                                        c = Chat.FriendColor
                                    End If
                                End If
                            End If
                            Core.SpriteBatch.DrawString(FontManager.MainFont, name, New Vector2(startX + (x - 1) * 256 + 4, startY + (y - 1) * 40 + 6), c)

                            Select Case playerList((x - 1) * 10 + (y - 1)).BusyType
                                Case 1 'Battle
                                    Core.SpriteBatch.Draw(TextureManager.GetTexture("Textures\emoticons", New Rectangle(48, 16, 16, 16), ""), New Rectangle(startX + (x - 1) * 256 + 222, startY + (y - 1) * 40 + 6, 32, 32), Color.White)
                                Case 2 'Chat
                                    Core.SpriteBatch.Draw(TextureManager.GetTexture("Textures\emoticons", New Rectangle(0, 0, 16, 16), ""), New Rectangle(startX + (x - 1) * 256 + 222, startY + (y - 1) * 40 + 6, 32, 32), Color.White)
                                Case 3 'AFK
                                    Core.SpriteBatch.Draw(TextureManager.GetTexture("Textures\emoticons", New Rectangle(0, 48, 16, 16), ""), New Rectangle(startX + (x - 1) * 256 + 222, startY + (y - 1) * 40 + 6, 32, 32), Color.White)
                            End Select
                        End If
                        Canvas.DrawBorder(3, New Rectangle(startX + (x - 1) * 256, startY + (y - 1) * 40, 256, 40), New Color(220, 220, 220))
                    Next
                Next

                Dim serverName As String = JoinServerScreen.SelectedServer.GetName()
                Dim plateLength As Integer = 256
                If FontManager.MainFont.MeasureString(serverName).X > 230.0F Then
                    plateLength = 26 + CInt(FontManager.MainFont.MeasureString(serverName).X)
                End If

                Canvas.DrawRectangle(New Rectangle(CInt(Core.windowSize.Width / 2 - plateLength / 2), 80, plateLength, 40), New Color(0, 0, 0, 150))
                Core.SpriteBatch.DrawString(FontManager.MainFont, serverName, New Vector2(CInt(Core.windowSize.Width / 2 - plateLength / 2) + 4, 80 + 6), Chat.ServerColor)
                Canvas.DrawBorder(3, New Rectangle(CInt(Core.windowSize.Width / 2 - plateLength / 2), 80, plateLength, 40), New Color(220, 220, 220))
            End If
        End If
    End Sub

End Class