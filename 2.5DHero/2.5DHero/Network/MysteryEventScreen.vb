Public Class MysteryEventScreen

    Inherits Screen

    Public Shared ActivatedMysteryEvents As New List(Of MysteryEvent)

    Enum EventTypes
        EXPMultiplier
        MoneyMultiplier
        PointsMultiplier
        ShinyMultiplier
    End Enum

    Public Class MysteryEvent

        Public EventType As EventTypes
        Public Name As String
        Public Description As String
        Public Value As String
        Public ID As String

        Public Sub New(ByVal ID As String, ByVal EventTypeSTR As String, ByVal Name As String, ByVal Description As String, ByVal Value As String)
            Me.ID = ID

            Select Case EventTypeSTR.ToLower()
                Case "exp"
                    Me.EventType = EventTypes.EXPMultiplier
                Case "money"
                    Me.EventType = EventTypes.MoneyMultiplier
                Case "points"
                    Me.EventType = EventTypes.PointsMultiplier
                Case "shiny"
                    Me.EventType = EventTypes.ShinyMultiplier
            End Select

            Me.Name = Name
            Me.Description = Description
            Me.Value = Value
        End Sub

        Public Overrides Function ToString() As String
            Return "ID: " & ID & "|EVENT TYPE: " & Me.EventType.ToString() & "|NAME: " & Name & "|DESCRIPTION: " & Description & "|VALUE: " & Me.Value
        End Function

        Public Function IsEventActivated() As Boolean
            For Each cEvent As MysteryEvent In ActivatedMysteryEvents
                If cEvent.ID = Me.ID Then
                    Return True
                End If
            Next
            Return False
        End Function

    End Class

    Dim failedDownload As Boolean = False
    Dim finishedDownload As Boolean = False
    Dim EventData As New List(Of MysteryEvent)

    Dim cursor As Integer = 0

    Public Sub New(ByVal currentScreen As Screen)
        Me.PreScreen = currentScreen
        Me.Identification = Identifications.MysteryEventScreen
        Me.MouseVisible = True

        Dim t As New Threading.Thread(AddressOf DownloadInformation)
        t.Start()
    End Sub

    Private Sub DownloadInformation()
        'File Format: ID|EventType|Name|Description|Value

        Me.EventData.Clear()
        Me.failedDownload = False
        Me.finishedDownload = False

        Try
            Dim w As New System.Net.WebClient
            Dim data() As String = w.DownloadString("https://raw.githubusercontent.com/P3D-Legacy/P3D-Legacy-Data/master/Events.txt").SplitAtNewline()

            For Each line As String In data
                If line.Contains("|") = True Then
                    Dim eventData() As String = line.Split(CChar("|"))
                    If eventData.Count = 5 Then
                        Me.EventData.Add(New MysteryEvent(eventData(0), eventData(1), eventData(2), eventData(3), eventData(4)))
                    End If
                End If
            Next

            finishedDownload = True
            failedDownload = False
        Catch ex As Exception
            Logger.Log(Logger.LogTypes.ErrorMessage, "MysteryEventScreen.vb: Failed to download event data!")
            finishedDownload = True
            failedDownload = True
            ClearActivatedEvents()
        End Try
    End Sub

    Public Overrides Sub Draw()
        Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), Core.windowSize, New Rectangle(320, 176, 192, 160), Color.White)

        Dim header As String = "Mystery Event"
        Core.SpriteBatch.DrawString(FontManager.InGameFont, header, New Vector2(52, 52), Color.Black)
        Core.SpriteBatch.DrawString(FontManager.InGameFont, header, New Vector2(50, 50), Color.White)

        If Me.finishedDownload = False Then
            Dim t As String = "Downloading Mystery Event data. Please wait" & LoadingDots.Dots

            Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(52, 152), Color.Black)
            Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(50, 150), Color.White)
        Else
            If Me.failedDownload = True Then
                Dim t As String = "Failed to download Mystery Event data." & vbNewLine & "Please check your internet connection and try again."

                Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(52, 152), Color.Black)
                Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(50, 150), Color.White)
            Else
                If EventData.Count = 0 Then
                    Dim t As String = "There are no Mystery Events available" & vbNewLine & "at the moment. Please try again later."

                    Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(52, 152), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(50, 150), Color.White)
                Else
                    Dim t As String = "Please select the Mystery Events that" & vbNewLine & "you want to activate for your gameplay session:"

                    Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(52, 152), Color.Black)
                    Core.SpriteBatch.DrawString(FontManager.InGameFont, t, New Vector2(50, 150), Color.White)

                    Dim startY As Integer = 240
                    For i = 0 To Me.EventData.Count - 1
                        Dim cEvent As MysteryEvent = Me.EventData(i)

                        Dim textColor As Color = Color.White
                        Dim backColor As Color = Color.Transparent
                        Dim borderColor As Color = Color.White

                        If i = cursor Then
                            textColor = Color.Black
                            backColor = Color.White
                            borderColor = Color.Black
                        End If

                        Canvas.DrawRectangle(New Rectangle(50, startY + i * 100, 400, 60), backColor)
                        Canvas.DrawBorder(2, New Rectangle(50, startY + i * 100, 400, 60), borderColor)

                        Dim activated As String = "Activated"
                        If cEvent.IsEventActivated() = False Then
                            activated = "Not activated"
                            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(58, startY + i * 100 + 13, 32, 32), New Rectangle(368, 96, 16, 16), Color.White)
                        Else
                            Core.SpriteBatch.Draw(TextureManager.GetTexture("GUI\Menus\GTS"), New Rectangle(58, startY + i * 100 + 13, 32, 32), New Rectangle(384, 96, 16, 16), Color.White)
                        End If

                        Core.SpriteBatch.DrawString(FontManager.MiniFont, cEvent.EventType.ToString() & ": " & cEvent.Name & vbNewLine & activated, New Vector2(100, startY + i * 100 + 8), textColor)
                    Next

                    Canvas.DrawGradient(New Rectangle(500, startY, 400, 300), Color.White, Color.Gray, False, -1)

                    Dim sEvent As MysteryEvent = EventData(cursor)
                    Core.SpriteBatch.DrawString(FontManager.MiniFont, "Name: " & sEvent.Name & vbNewLine & vbNewLine &
                                                 "Type: " & sEvent.EventType.ToString() & vbNewLine & vbNewLine &
                                                 "Multiplicator: " & sEvent.Value & "x" & vbNewLine & vbNewLine &
                                                 "Description:" & vbNewLine &
                                                 sEvent.Description.CropStringToWidth(FontManager.MiniFont, 300), New Vector2(512, startY + 12), Color.Black)
                End If
            End If
        End If
    End Sub

    Public Overrides Sub Update()
        If Controls.Up(True, True, True, True, True, True) = True Then
            cursor -= 1
        End If
        If Controls.Down(True, True, True, True, True, True) = True Then
            cursor += 1
        End If

        cursor = cursor.Clamp(0, Me.EventData.Count - 1)

        If Controls.Accept(True, True, True) = True Then
            If EventData.Count > 0 Then
                Dim cEvent As MysteryEvent = EventData(cursor)
                If cEvent.IsEventActivated() = True Then
                    For i = 0 To ActivatedMysteryEvents.Count - 1
                        If cEvent.ID = ActivatedMysteryEvents(i).ID Then
                            ActivatedMysteryEvents.RemoveAt(i)
                            Exit For
                        End If
                    Next
                Else
                    ActivatedMysteryEvents.Add(cEvent)
                End If
            End If
        End If

        If Controls.Dismiss(True) = True Then
            Core.SetScreen(New TransitionScreen(Me, Me.PreScreen, Color.White, False))
        End If
    End Sub

    Public Overrides Sub ChangeTo()
        MusicManager.PlayMusic("gts", True)
    End Sub

    Public Shared Sub ClearActivatedEvents()
        ActivatedMysteryEvents.Clear()
    End Sub

End Class