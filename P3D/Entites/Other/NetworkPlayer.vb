Public Class NetworkPlayer

    Inherits Entity

    Shared ReadOnly FallbackSkins() As String = {"0", "1", "2", "5", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "32", "49", "61", "63", "oldhatman", "PinkShirtGirl", "bugcatcher"}
    Shared FallBack As New Dictionary(Of Integer, String)

    Public Name As String = ""

    ''' <summary>
    ''' The Network ID of the player
    ''' </summary>
    Public NetworkID As Integer = 0

    Public faceRotation As Integer
    Public MapFile As String = ""

    Dim GameJoltID As String = ""
    Dim RotatedSprite As Boolean = False

    Public TextureID As String
    Public Texture As Texture2D

    Public moving As Boolean = False
    Dim lastRectangle As New Rectangle(0, 0, 0, 0)
    Dim AnimationX As Integer = 1
    Const AnimationDelayLenght As Single = 1.1F
    Dim AnimationDelay As Single = AnimationDelayLenght
    Public HasPokemonTexture As Boolean = False

    Dim NameTexture As Texture2D

    Dim LastName As String = ""

    Dim LevelFile As String = ""

    Public BusyType As String = "0"

    Private DownloadingSprite As Boolean = False
    Private CheckForOnlineSprite As Boolean = False

    Public Sub New(ByVal X As Single, ByVal Y As Single, ByVal Z As Single, ByVal Textures() As Texture2D, ByVal TextureID As String, ByVal Rotation As Integer, ByVal ActionValue As Integer, ByVal AdditionalValue As String, ByVal Scale As Vector3, ByVal Name As String, ByVal ID As Integer)
        MyBase.New(X, Y, Z, "NetworkPlayer", Textures, {0, 0}, True, Rotation, Scale, BaseModel.BillModel, 0, "", New Vector3(1.0F))

        Me.Name = Name
        Me.NetworkID = ID
        Me.faceRotation = Rotation
        Me.TextureID = TextureID
        Me.Collision = False
        Me.NeedsUpdate = True

        AssignFallback(ID)

        SetTexture(TextureID)
        ChangeTexture()
        Me.CreateWorldEveryFrame = True

        Me.DropUpdateUnlessDrawn = False
    End Sub

    Private Sub AssignFallback(ByVal ID As Integer)
        If FallBack.ContainsKey(ID) = False Then
            FallBack.Add(ID, FallbackSkins(Core.Random.Next(0, FallbackSkins.Length)))
        End If
    End Sub

    Public Sub SetTexture(ByVal TextureID As String)
        Me.TextureID = TextureID

        Dim texturePath As String = GetTexturePath(TextureID)

        Dim OnlineSprite As Texture2D = Nothing
        If Me.GameJoltID <> "" Then
            If GameJolt.Emblem.HasDownloadedSprite(Me.GameJoltID) = True Then
                OnlineSprite = GameJolt.Emblem.GetOnlineSprite(Me.GameJoltID)
            Else
                Dim t As New Threading.Thread(AddressOf DownloadOnlineSprite)
                t.IsBackground = True
                t.Start()
                DownloadingSprite = True
            End If
        End If

        If Not OnlineSprite Is Nothing Then
            Me.Texture = OnlineSprite
        Else
            If TextureManager.TextureExist(texturePath) = True Then
                Logger.Debug("Change network texture to [" & texturePath & "]")

                If texturePath.StartsWith("Pokemon\") = True Then
                    Me.HasPokemonTexture = True
                Else
                    Me.HasPokemonTexture = False
                End If

                Me.Texture = TextureManager.GetTexture(texturePath)
            Else
                Logger.Debug("Texture fallback!")
                Me.Texture = TextureManager.GetTexture("Textures\NPC\" & FallBack(Me.NetworkID))
            End If
        End If
    End Sub

    Private Sub DownloadOnlineSprite()
        Dim t As Texture2D = GameJolt.Emblem.GetOnlineSprite(Me.GameJoltID)

        If Not t Is Nothing Then
            Me.Texture = t
        End If
    End Sub

    Public Shared Function GetTexturePath(ByVal TextureID As String) As String
        Dim texturePath As String = "Textures\NPC\"
        Dim isPokemon As Boolean = False
        If TextureID.StartsWith("[POKEMON|N]") = True Or TextureID.StartsWith("[Pokémon|N]") = True Then
            TextureID = TextureID.Remove(0, 11)
            isPokemon = True
            texturePath = "Pokemon\Overworld\Normal\"
        ElseIf TextureID.StartsWith("[POKEMON|S]") = True Or TextureID.StartsWith("[Pokémon|S]") = True Then
            TextureID = TextureID.Remove(0, 11)
            isPokemon = True
            texturePath = "Pokemon\Overworld\Shiny\"
        End If
        Return texturePath & TextureID
    End Function

    Private Sub ChangeTexture()
        If Not Me.Texture Is Nothing Then
            Dim r As New Rectangle(0, 0, 0, 0)
            Dim cameraRotation As Integer = Screen.Camera.GetFacingDirection()
            Dim spriteIndex As Integer = Me.faceRotation - cameraRotation

            spriteIndex = Me.faceRotation - cameraRotation
            If spriteIndex < 0 Then
                spriteIndex += 4
            End If

            If RotatedSprite = True Then
                Select Case spriteIndex
                    Case 1
                        spriteIndex = 3
                    Case 3
                        spriteIndex = 1
                End Select
            End If

            Dim spriteSize As New Size(CInt(Me.Texture.Width / 3), CInt(Me.Texture.Height / 4))

            Dim x As Integer = 0
            If Me.moving = True Then
                x = GetAnimationX() * spriteSize.Width
            End If

            r = New Rectangle(x, spriteSize.Height * spriteIndex, spriteSize.Width, spriteSize.Height)

            If r <> lastRectangle Then
                lastRectangle = r

                Textures(0) = TextureManager.GetTexture(Me.Texture, r, 1)
            End If
        End If
    End Sub

    Private Function GetAnimationX() As Integer
        If Me.HasPokemonTexture = True Then
            Select Case AnimationX
                Case 1
                    Return 0
                Case 2
                    Return 1
                Case 3
                    Return 0
                Case 4
                    Return 1
            End Select
        End If
        Select Case AnimationX
            Case 1
                Return 0
            Case 2
                Return 1
            Case 3
                Return 0
            Case 4
                Return 2
        End Select
        Return 1
    End Function

    Private Sub Move()
        If Me.moving = True Then
            Me.AnimationDelay -= 0.1F
            If Me.AnimationDelay <= 0.0F Then
                Me.AnimationDelay = AnimationDelayLenght
                AnimationX += 1
                If AnimationX > 4 Then
                    AnimationX = 1
                End If
            End If
        End If
    End Sub

    Protected Overrides Function CalculateCameraDistance(CPosition As Vector3) as Single
        Return MyBase.CalculateCameraDistance(CPosition) - 0.2f
    End Function

    Public Overrides Sub UpdateEntity()
        If Me.Rotation.Y <> Screen.Camera.Yaw Then
            Me.Rotation.Y = Screen.Camera.Yaw
        End If
        If Not Me.TextureID Is Nothing AndAlso Me.TextureID.ToLower() = "nilllzz" And Me.GameJoltID = "17441" Then
            Me.Rotation.Z = MathHelper.Pi
            RotatedSprite = True
        Else
            RotatedSprite = False
            Me.Rotation.Z = 0
        End If

        ChangeTexture()

        MyBase.UpdateEntity()
    End Sub

    Public Overrides Sub Update()
        If Me.Name <> Me.LastName Then
            Me.LastName = Me.Name
            Me.NameTexture = SpriteFontTextToTexture(FontManager.InGameFont, Me.Name)
        End If

        Move()

        If DownloadingSprite AndAlso GameJolt.Emblem.HasDownloadedSprite(GameJoltID) Then
            SetTexture(TextureID)
            ChangeTexture()
            DownloadingSprite = False
        End If

        MyBase.Update()
    End Sub

    Public Overrides Sub Render()
        If ConnectScreen.Connected = True Then
            If IsCorrectScreen() = True Then
                Me.Draw(Me.Model, Textures, False)
                If Core.GameOptions.ShowGUI = True Then
                    If Me.NameTexture IsNot Nothing Then
                        Dim state = GraphicsDevice.DepthStencilState
                        GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead
                        Draw(BaseModel.BillModel, {Me.NameTexture}, False)
                        GraphicsDevice.DepthStencilState = state
                    End If
                    If Me.BusyType <> "0" Then
                        RenderBubble()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub RenderBubble()
        Dim b As MessageBulb = Nothing
        Select Case BusyType
            Case "1"
                b = New MessageBulb(New Vector3(Me.Position.X, Me.Position.Y + 1, Me.Position.Z), MessageBulb.NotifcationTypes.Battle)
            Case "2"
                b = New MessageBulb(New Vector3(Me.Position.X, Me.Position.Y + 1, Me.Position.Z), MessageBulb.NotifcationTypes.Waiting)
            Case "3"
                b = New MessageBulb(New Vector3(Me.Position.X, Me.Position.Y + 1, Me.Position.Z), MessageBulb.NotifcationTypes.AFK)
        End Select
        If Not b Is Nothing Then
            b.Visible = Me.Visible
            b.Render()
        End If
    End Sub

    Private Function IsCorrectScreen() As Boolean
        Dim screens() As Screen.Identifications = {Screen.Identifications.BattleCatchScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleGrowStatsScreen, Screen.Identifications.BattleScreen, Screen.Identifications.CreditsScreen, Screen.Identifications.BattleAnimationScreen, Screen.Identifications.ViewModelScreen, Screen.Identifications.HallofFameScreen}
        If screens.Contains(Core.CurrentScreen.Identification) = True Then
            Return False
        Else
            If Core.CurrentScreen.Identification = Screen.Identifications.TransitionScreen Then
                If screens.Contains(CType(Core.CurrentScreen, TransitionScreen).OldScreen.Identification) = True Or screens.Contains(CType(Core.CurrentScreen, TransitionScreen).NewScreen.Identification) = True Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Public Sub ApplyPlayerData(ByVal p As Servers.Player)
        Try
            Me.NetworkID = p.ServersID

            Me.Position = p.Position

            Me.Name = p.Name

            If Not p.Skin.StartsWith("[POKEMON|N]") AndAlso Not p.Skin.StartsWith("[Pokémon|N]") AndAlso Not p.Skin.StartsWith("[POKEMON|S]") AndAlso Not p.Skin.StartsWith("[Pokémon|S]") Then
                If Not String.IsNullOrWhiteSpace(GameJoltID) AndAlso CheckForOnlineSprite = False Then
                    CheckForOnlineSprite = True
                    Me.SetTexture(p.Skin)
                End If
            End If

            If Me.TextureID <> p.Skin Then
                Me.SetTexture(p.Skin)
            End If
            Me.ChangeTexture()

            Me.GameJoltID = p.GameJoltId
            Me.faceRotation = p.Facing
            Me.FaceDirection = p.Facing
            Me.moving = p.Moving
            Me.LevelFile = p.LevelFile
            Me.BusyType = p.BusyType.ToString()
            Me.Visible = False

            If Screen.Level.LevelFile.ToLower() = p.LevelFile.ToLower() Then
                Me.Visible = True
            Else
                If LevelLoader.LoadedOffsetMapNames.Contains(p.LevelFile) = True Then
                    Offset = LevelLoader.LoadedOffsetMapOffsets(LevelLoader.LoadedOffsetMapNames.IndexOf(p.LevelFile))
                    Me.Position.X += Offset.X
                    Me.Position.Y += Offset.Y
                    Me.Position.Z += Offset.Z
                    Me.Visible = True
                End If
            End If
        Catch ex As Exception
            Logger.Debug("NetworkPlayer.vb: Error while assigning player data over network: " & ex.Message)
        End Try
    End Sub

    Public Overrides Sub ClickFunction()
        Dim Data(4) As Object
        Data(0) = Me.NetworkID
        Data(1) = Me.GameJoltID
        Data(2) = Me.Name
        Data(3) = Me.Texture

        'Basic.SetScreen(New GameJolt.PokegearScreen(Basic.currentScreen, GameJolt.PokegearScreen.EntryModes.DisplayUser, Data))
    End Sub

    Public Shared Sub ScreenRegionChanged()
        If Not Core.CurrentScreen Is Nothing AndAlso Not Screen.Level Is Nothing Then
            For Each netPlayer As NetworkPlayer In Screen.Level.NetworkPlayers
                netPlayer.LastName = ""
            Next
        End If
    End Sub

    Shared SpriteTextStorage As New Dictionary(Of String, Texture2D)

    Private Shared Function SpriteFontTextToTexture(ByVal font As SpriteFont, ByVal text As String) As Texture2D
        If text.Length > 0 Then
            If SpriteTextStorage.ContainsKey(text) = True Then
                Return SpriteTextStorage(text)
            Else
                Dim size As Vector2 = font.MeasureString(text)
                Dim renderTarget As RenderTarget2D = New RenderTarget2D(Core.GraphicsDevice, CInt(size.X), CInt(size.Y * 3))
                Core.GraphicsDevice.SetRenderTarget(renderTarget)

                Core.GraphicsDevice.Clear(Color.Transparent)

                Core.SpriteBatch.Begin()
                Canvas.DrawRectangle(New Rectangle(0, 0, CInt(size.X), CInt(size.Y)), New Color(0, 0, 0, 150))
                Core.SpriteBatch.DrawString(font, text, Vector2.Zero, Color.White)
                Core.SpriteBatch.End()

                Core.GraphicsDevice.SetRenderTarget(Nothing)
                SpriteTextStorage.Add(text, renderTarget)

                Return renderTarget
            End If
        End If
        Return Nothing
    End Function

End Class