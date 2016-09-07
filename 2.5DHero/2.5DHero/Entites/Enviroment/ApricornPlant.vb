Public Class ApricornPlant

    Inherits Entity

    Public Enum ApricornColors
        White = 0
        Black = 1
        Yellow = 6
        Green = 5
        Red = 4
        Blue = 3
        Pink = 2
    End Enum

    Dim ApricornColor As ApricornColors = ApricornColors.White
    Dim hasApricorn As Boolean = True

    Public Overrides Sub Initialize()
        MyBase.Initialize()

        CreateWorldEveryFrame = True

        ApricornColor = GetApricornColor(CInt(AdditionalValue))
        CheckHasApricorn()
        ChangeTexture()
    End Sub

    Private Sub ChangeTexture()
        Dim r As New Rectangle(16, 32, 16, 16)

        If hasApricorn = True Then
            Dim x As Integer = GetColorCode(ApricornColor)
            Dim y As Integer = 0

            While x > 2
                x -= 3
                y += 1
            End While

            r = New Rectangle(x * 16, y * 16, 16, 16)
        End If

        Textures(0) = TextureManager.GetTexture("Apricorn", r)
    End Sub

    Private Sub CheckHasApricorn()
        If Core.Player.ApricornData = "" Then
            hasApricorn = True
        Else
            Dim ApricornsData As List(Of String) = Core.Player.ApricornData.SplitAtNewline().ToList()

            Dim hasRemoved As Boolean = False

            For i = 0 To ApricornsData.Count - 1
                If i < ApricornsData.Count Then
                    Dim Apricorn As String = ApricornsData(i)

                    Apricorn = Apricorn.Remove(0, 1)
                    Apricorn = Apricorn.Remove(Apricorn.Length - 1, 1)

                    Dim ApricornData() As String = Apricorn.Split(CChar("|"))

                    If ApricornData(0) = Screen.Level.LevelFile Then
                        Dim PositionData() As String = ApricornData(1).Split(CChar(","))
                        If Position.X = CInt(PositionData(0)) And Position.Y = CInt(PositionData(1)) And Position.Z = CInt(PositionData(2)) Then
                            Dim d() As String = ApricornData(2).Split(CChar(","))

                            Dim PickDate As Date = New Date(CInt(d(0)), CInt(d(1)), CInt(d(2)), CInt(d(3)), CInt(d(4)), CInt(d(5)))

                            Dim diff As Integer = CInt(DateDiff(DateInterval.Hour, PickDate, Date.Now))

                            Dim hasToDiff As Integer = 24
                            If Game.World.CurrentSeason = Game.World.Seasons.Winter Or Game.World.CurrentSeason = Game.World.Seasons.Fall Then
                                hasToDiff = 12
                            End If

                            If diff >= hasToDiff Then
                                ApricornsData.RemoveAt(i)
                                i -= 1
                                hasApricorn = True
                                hasRemoved = True
                            Else
                                hasApricorn = False
                            End If
                        End If
                    End If
                End If
            Next

            If hasRemoved = True Then
                Core.Player.ApricornData = ""
                For Each Apricorn As String In ApricornsData
                    If Core.Player.ApricornData <> "" Then
                        Core.Player.ApricornData &= vbNewLine
                    End If
                    Core.Player.ApricornData &= Apricorn
                Next
            End If
        End If
    End Sub

    Public Function GetApricornColor(ByVal ColorCode As Integer) As ApricornColors
        Return CType(ColorCode, ApricornColors)
    End Function

    Public Function GetColorCode(ByVal ApricornColor As ApricornColors) As Integer
        Return CType(ApricornColor, Integer)
    End Function

    Public Overrides Sub UpdateEntity()
        If Rotation.Y <> Screen.Camera.Yaw Then
            Rotation.Y = Screen.Camera.Yaw
        End If

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Render()
        Draw(Model, Textures, False)
    End Sub

    Public Overrides Sub ClickFunction()
        Dim text As String = "There are no apricorns~on this tree.*Better come back later..."

        If hasApricorn = True Then
            Dim Item As Item = GetItem()

            text = "There is a " & Item.Name & "~on this tree.*Do you want to pick it?%Yes|No%"
        End If

        Screen.TextBox.Show(text, {Me})
        SoundManager.PlaySound("select")
    End Sub

    Public Overrides Sub ResultFunction(Result As Integer)
        If Result = 0 Then
            Dim Item As Item = GetItem()

            Core.Player.Inventory.AddItem(Item.ID, 1)
            PlayerStatistics.Track("[85]Apricorns picked", 1)
            SoundManager.PlaySound("item_found", True)
            Screen.TextBox.TextColor = TextBox.PlayerColor
            Screen.TextBox.Show(Core.Player.Name & " picked the~" & Item.Name & ".*" & Core.Player.Inventory.GetMessageReceive(Item, 1), {Me})
            AddApriconSave()
            hasApricorn = False
            ChangeTexture()
        End If
    End Sub

    Private Sub AddApriconSave()
        Dim s As String = "{"

        Dim d As Date = Date.Now
        s &= Screen.Level.LevelFile & "|" &
            CInt(Position.X) & "," & CInt(Position.Y) & "," & CInt(Position.Z) & "|" &
            d.Year & "," & d.Month & "," & d.Day & "," & d.Hour & "," & d.Minute & "," & d.Second &
            "}"

        If Core.Player.ApricornData <> "" Then
            Core.Player.ApricornData &= vbNewLine
        End If

        Core.Player.ApricornData &= s
    End Sub

    Private Function GetItem() As Item
        Dim ItemID As Integer = 0

        Select Case ApricornColor
            Case ApricornColors.Red
                ItemID = 85
            Case ApricornColors.Blue
                ItemID = 89
            Case ApricornColors.Yellow
                ItemID = 92
            Case ApricornColors.Green
                ItemID = 93
            Case ApricornColors.White
                ItemID = 97
            Case ApricornColors.Black
                ItemID = 99
            Case ApricornColors.Pink
                ItemID = 101
        End Select

        Return Item.GetItemByID(ItemID)
    End Function

End Class