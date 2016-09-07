Public Class BerryPlant

    Inherits Entity

    Dim Phase As Integer = 0
    Dim Grow As Integer = 0
    Dim BerryIndex As Integer = 0
    Dim BerryGrowTime As Integer = 0
    Dim Berry As Items.Berry
    Dim Berries As Integer = 0
    Dim PlantDate As String = ""
    Dim FullGrown As Boolean = False

    Dim Watered As New List(Of Boolean)

    Dim LastUpdateDate As Date

    Public Overloads Sub Initialize(ByVal BerryIndex As Integer, ByVal BerriesYield As Integer, ByVal Watered As String, ByVal Time As String, ByVal FullGrown As Boolean)
        Me.Berry = CType(Item.GetItemByID(BerryIndex + 2000), Items.Berry)
        Me.Berries = BerriesYield
        Me.PlantDate = Time
        Me.BerryIndex = BerryIndex
        Me.FullGrown = FullGrown

        InitBerry(Time)

        Me.LastUpdateDate = Date.Now

        If Watered.CountSplits(",") <> 4 Then
            Me.Watered.AddRange({False, False, False, False})
        Else
            For Each b As String In Watered.Split(CChar(","))
                Me.Watered.Add(CBool(b))
            Next
        End If

        Me.NeedsUpdate = True
        Me.CreateWorldEveryFrame = True
    End Sub

    Private Sub InitBerry(ByVal oldDate As String)
        Dim Data() As String = oldDate.Split(CChar(","))
        Dim d As New Date(CInt(Data(0)), CInt(Data(1)), CInt(Data(2)), CInt(Data(3)), CInt(Data(4)), CInt(Data(5)))

        If FullGrown = True Then
            Me.Phase = 4

            NewTexture()
        Else
            With My.Computer.Clock.LocalTime
                Dim cD As New Date(.Year, .Month, .Day, .TimeOfDay.Hours, .TimeOfDay.Minutes, .TimeOfDay.Seconds)
                Dim diff As Integer = CInt(DateDiff(DateInterval.Second, d, cD))

                Grow += diff

                While Grow >= Berry.PhaseTime
                    Grow -= Berry.PhaseTime
                    Me.Phase += 1

                    If Me.Phase > 4 Then
                        Me.Phase = 0
                    End If
                End While

                NewTexture()
            End With
        End If
    End Sub

    Public Overrides Sub Update()
        If Me.LastUpdateDate.Year = 1 Then
            Me.LastUpdateDate = Date.Now
        End If

        Dim diff As Integer = CInt(DateDiff(DateInterval.Second, LastUpdateDate, Date.Now))
        If diff > 0 Then
            Me.Grow += diff

            If Me.Grow >= Me.Berry.PhaseTime Then
                While Grow >= Berry.PhaseTime
                    Grow -= Berry.PhaseTime
                    Me.Phase += 1

                    If Me.Phase > 4 Then
                        Me.Phase = 0
                    End If
                End While
                NewTexture()
            End If
        End If

        Me.LastUpdateDate = Date.Now
    End Sub

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
        End If

        MyBase.UpdateEntity()
    End Sub

    Private Sub NewTexture()
        Dim t As Texture2D
        If Me.Phase > 1 Then
            Dim x As Integer = Me.Berry.BerryIndex * 128 + ((Phase - 1) * 32)
            Dim y As Integer = 0
            While x > 512
                x -= 512
                y += 32
            End While
            Dim r As New Rectangle(x, y, 32, 32)
            t = net.Pokemon3D.Game.TextureManager.GetTexture("Textures\Berries", r, "")
        Else
            Dim r As Rectangle
            Select Case Me.Phase
                Case 0
                    r = New Rectangle(448, 480, 32, 32)
                Case 1
                    r = New Rectangle(480, 480, 32, 32)
            End Select
            t = net.Pokemon3D.Game.TextureManager.GetTexture("Items\ItemSheet", r, "")
        End If

        Me.Textures(0) = t
    End Sub

    Dim ResultIndex As Integer = 0

    Public Overrides Sub ClickFunction()
        Dim text As String = ""

        Dim hasBottle As Boolean = False
        If Core.Player.Inventory.GetItemAmount(175) > 0 Then
            hasBottle = True
        End If

        Select Case Me.Phase
            Case 0
                Me.ResultIndex = 1
                If hasBottle = True Then
                    text = "One " & Me.Berry.Name & " Berry was~planted here.*Do you want to~water it?%Yes|No%"
                Else
                    text = "One " & Me.Berry.Name & " Berry was~planted here."
                End If
            Case 1
                Me.ResultIndex = 1
                If hasBottle = True Then
                    text = Berry.Name & " has sprouted.*Do you want to~water it?%Yes|No%"
                Else
                    text = Berry.Name & " has sprouted."
                End If
            Case 2
                Me.ResultIndex = 1
                If hasBottle = True Then
                    text = "This " & Berry.Name & " plant is~growing taller.*Do you want to~water it?%Yes|No%"
                Else
                    text = "This " & Berry.Name & " plant is~growing taller."
                End If
            Case 3
                Me.ResultIndex = 1
                If hasBottle = True Then
                    text = "These " & Berry.Name & " flowers~are blooming.*Do you want to~water it?%Yes|No%"
                Else
                    text = "These " & Berry.Name & " flowers~are blooming."
                End If
            Case 4
                Me.ResultIndex = 0
                If Me.Berries = 1 Then
                    text = "There is a~" & Berry.Name & " Berry!*Do you want to pick~it?%Yes|No%"
                Else
                    text = "There are " & Berries & "~" & Berry.Name & " Berries!*Do you want to pick~them?%Yes|No%"
                End If
        End Select

        Screen.TextBox.Show(text, {Me})
        SoundManager.PlaySound("select")
    End Sub

    Public Overrides Sub ResultFunction(ByVal Result As Integer)
        If Result = 0 Then
            Select Case Me.ResultIndex
                Case 0
                    Core.Player.Inventory.AddItem(Me.Berry.ID, Me.Berries)
                    Dim Text As String = ""
                    If Me.Berries = 1 Then
                        Text = Core.Player.Name & " picked the~" & Berry.Name & " Berry.*" & Core.Player.Inventory.GetMessageReceive(Berry, Me.Berries)
                    Else
                        Text = Core.Player.Name & " picked the " & Berries & "~" & Berry.Name & " Berries.*" & Core.Player.Inventory.GetMessageReceive(Berry, Me.Berries)
                    End If

                    Core.Player.AddPoints(2, "Picked berries.")
                    PlayerStatistics.Track("[2006]Berries picked", Me.Berries)

                    SoundManager.PlaySound("item_found", True)
                    Screen.TextBox.TextColor = TextBox.PlayerColor
                    Screen.TextBox.Show(Text, {Me})
                    RemoveBerry()
                    Screen.Level.Entities.Remove(Me)
                Case 1
                    WaterBerry()
                    Dim Text As String = Core.Player.Name & " watered~the " & Berry.Name & "."
                    Screen.TextBox.Show(Text, {Me})
            End Select
        End If
    End Sub

    Public Overrides Sub Render()
        Me.Draw(Me.Model, Textures, False)
    End Sub

    Private Sub RemoveBerry()
        Dim Data() As String = Core.Player.BerryData.SplitAtNewline()
        Dim OutData As String = ""

        For Each Berry As String In Data
            If Berry <> "" Then
                If Berry.ToLower().StartsWith("{" & Screen.Level.LevelFile.ToLower() & "|" & (Me.Position.X & "," & Me.Position.Y & "," & Me.Position.Z).ToLower() & "|") = False Then
                    If OutData <> "" Then
                        OutData &= vbNewLine
                    End If
                    OutData &= Berry
                End If
            End If
        Next

        Core.Player.BerryData = OutData
    End Sub

    Public Shared Sub AddBerryPlant(ByVal LevelFile As String, ByVal Position As Vector3, ByVal BerryIndex As Integer)
        Dim cD As Date = Date.Now
        Dim DateData As String = cD.Year & "," & cD.Month & "," & cD.Day & "," & cD.TimeOfDay.Hours & "," & cD.TimeOfDay.Minutes & "," & cD.TimeOfDay.Seconds

        Dim Berry As Items.Berry = CType(Item.GetItemByID(BerryIndex + 2000), Items.Berry)

        Dim BerryAmount As Integer = GetBerryAmount(Berry, 0)

        Dim WateredData As String = "0,0,0,0"

        Dim FullGrownData As String = "0"

        Dim Data As String = "{" & LevelFile & "|" & Position.X & "," & Position.Y & "," & Position.Z & "|" & BerryIndex & "|" & BerryAmount & "|" & WateredData & "|" & DateData & "|" & FullGrownData & "}"

        Dim OldData As String = Core.Player.BerryData
        If OldData <> "" Then
            OldData &= vbNewLine
        End If
        OldData &= Data

        Core.Player.BerryData = OldData

        Dim newEnt As Entity = Entity.GetNewEntity("BerryPlant", Position, {Nothing}, {0, 0}, True, New Vector3(0), New Vector3(1), BaseModel.BillModel, 0, "", True, New Vector3(1.0F), -1, "", "", New Vector3(0))
        CType(newEnt, BerryPlant).Initialize(BerryIndex, 0, "", DateData, False)
        Screen.Level.Entities.Add(newEnt)

        Core.Player.Inventory.RemoveItem(BerryIndex + 2000, 1)
    End Sub

    Private Shared Function GetBerryAmount(ByVal Berry As Items.Berry, ByVal Watered As Integer) As Integer
        If Watered > 0 Then
            Dim a As Integer = Berry.maxBerries
            Dim b As Integer = Berry.minBerries
            Dim c As Integer = Core.Random.Next(b, a + 1)
            Dim d As Integer = Watered

            Dim amount As Integer = CInt((((a - b) * (d - 1) + c) / 4) + b)
            If amount < Berry.minBerries Then
                amount = Berry.minBerries
            End If

            Dim seasonGrow As Integer = 0
            Select Case net.Pokemon3D.Game.World.CurrentSeason
                Case net.Pokemon3D.Game.World.Seasons.Winter
                    seasonGrow = Berry.WinterGrow
                Case net.Pokemon3D.Game.World.Seasons.Spring
                    seasonGrow = Berry.SpringGrow
                Case net.Pokemon3D.Game.World.Seasons.Summer
                    seasonGrow = Berry.SummerGrow
                Case net.Pokemon3D.Game.World.Seasons.Fall
                    seasonGrow = Berry.FallGrow
            End Select

            Select Case seasonGrow
                Case 0
                    amount = Berry.minBerries
                Case 1
                    amount -= 1
                Case 2
                    'Do nothing here.
                Case 3
                    amount += 1
            End Select

            amount = amount.Clamp(Berry.minBerries, Berry.maxBerries)

            Return amount
        Else
            Return Berry.minBerries
        End If
    End Function

    Private Sub WaterBerry()
        Dim b As Boolean = Me.Watered(Me.Phase)
        If b = False Then
            Me.Watered(Me.Phase) = True

            RemoveBerry()

            Dim DateData As String = PlantDate

            Dim Berry As Items.Berry = CType(Item.GetItemByID(BerryIndex + 2000), Items.Berry)

            Dim WateredData As String = ""
            Dim wateredCount As Integer = 0
            For Each w As Boolean In Me.Watered
                If WateredData <> "" Then
                    WateredData &= ","
                End If
                If w = True Then
                    WateredData &= "1"
                    wateredCount += 1
                Else
                    WateredData &= "0"
                End If
            Next

            Dim BerryAmount As Integer = GetBerryAmount(Berry, wateredCount)

            Dim Data As String = "{" & Screen.Level.LevelFile & "|" & Me.Position.X & "," & Me.Position.Y & "," & Me.Position.Z & "|" & BerryIndex & "|" & BerryAmount & "|" & WateredData & "|" & DateData & "}"

            Dim OldData As String = Core.Player.BerryData
            If OldData <> "" Then
                OldData &= vbNewLine
            End If
            OldData &= Data

            Core.Player.BerryData = OldData
        End If
    End Sub

End Class