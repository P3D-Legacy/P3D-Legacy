Public Class Canvas

    Private Shared Canvas As Texture2D

    Public Shared Sub SetupCanvas()
        Dim tempData(0) As Color
        tempData(0) = Color.White

        Canvas = New Texture2D(Core.GraphicsDevice, 1, 1)
        Canvas.SetData(tempData)
    End Sub

    Public Shared Sub DrawRectangle(ByVal Rectangle As Rectangle, ByVal color As Color)
        Core.SpriteBatch.Draw(Canvas, Rectangle, color)
    End Sub

    Public Shared Sub DrawRectangle(ByVal spriteBatch As SpriteBatch, ByVal Rectangle As Rectangle, ByVal color As Color)
        spriteBatch.Draw(Canvas, Rectangle, color)
    End Sub

    Public Shared Sub DrawRectangle(ByVal Rectangle As Rectangle, ByVal color As Color, ByVal ScaleToScreen As Boolean)
        If ScaleToScreen = True Then
            Core.SpriteBatch.DrawInterface(Canvas, Rectangle, color)
        Else
            Core.SpriteBatch.Draw(Canvas, Rectangle, color)
        End If
    End Sub

#Region "Borders"

    Public Shared Sub DrawBorder(ByVal borderLength As Integer, ByVal Rectangle As Rectangle, ByVal color As Color)
        DrawBorder(borderLength, Rectangle, color, False)
    End Sub

    Public Shared Sub DrawBorder(ByVal borderLength As Integer, ByVal Rectangle As Rectangle, ByVal color As Color, ByVal ScaleToScreen As Boolean)
        DrawRectangle(New Rectangle(Rectangle.X + borderLength, Rectangle.Y, Rectangle.Width - borderLength, borderLength), color, ScaleToScreen)
        DrawRectangle(New Rectangle(Rectangle.X + Rectangle.Width - borderLength, Rectangle.Y + borderLength, borderLength, Rectangle.Height - borderLength), color, ScaleToScreen)
        DrawRectangle(New Rectangle(Rectangle.X, Rectangle.Y + Rectangle.Height - borderLength, Rectangle.Width - borderLength, borderLength), color, ScaleToScreen)
        DrawRectangle(New Rectangle(Rectangle.X, Rectangle.Y, borderLength, Rectangle.Height - borderLength), color, ScaleToScreen)
    End Sub

    Public Shared Sub DrawImageBorder(ByVal Texture As Texture2D, ByVal SizeMulitiplier As Integer, ByVal Rectangle As Rectangle, ByVal Color As Color, ByVal ScaleToScreen As Boolean)
        Dim borderSize As New Vector2(Rectangle.Width, Rectangle.Height)

        For x = 0 To borderSize.X Step CInt(Math.Floor(Texture.Width / 3)) * SizeMulitiplier
            For y = 0 To borderSize.Y Step CInt(Math.Floor(Texture.Height / 3)) * SizeMulitiplier

                Dim width As Integer = CInt(Math.Floor(Texture.Width / 3))
                Dim height As Integer = CInt(Math.Floor(Texture.Height / 3))

                Dim Tile As Rectangle = New Rectangle(width, height, width, height)
                If x = 0 And y = 0 Then
                    Tile = New Rectangle(0, 0, width, height)
                ElseIf x = borderSize.X And y = 0 Then
                    Tile = New Rectangle(width * 2, 0, width, height)
                ElseIf x = 0 And y = borderSize.Y Then
                    Tile = New Rectangle(0, height * 2, width, height)
                ElseIf x = borderSize.X And y = borderSize.Y Then
                    Tile = New Rectangle(width * 2, height * 2, width, height)
                ElseIf x = 0 Then
                    Tile = New Rectangle(0, height, width, height)
                ElseIf y = 0 Then
                    Tile = New Rectangle(width, 0, width, height)
                ElseIf x = borderSize.X Then
                    Tile = New Rectangle(width * 2, height, width, height)
                ElseIf y = borderSize.Y Then
                    Tile = New Rectangle(width, height * 2, width, height)
                End If

                If ScaleToScreen = True Then
                    Core.SpriteBatch.DrawInterface(Texture, New Rectangle(CInt(Math.Floor(x)) + Rectangle.X, CInt(Math.Floor(y)) + Rectangle.Y, SizeMulitiplier * width, SizeMulitiplier * height), Tile, Color.White)
                Else
                    Core.SpriteBatch.Draw(Texture, New Rectangle(CInt(Math.Floor(x)) + Rectangle.X, CInt(Math.Floor(y)) + Rectangle.Y, SizeMulitiplier * width, SizeMulitiplier * height), Tile, Color.White)
                End If
            Next
        Next
    End Sub

    Public Shared Sub DrawImageBorder(ByVal Texture As Texture2D, ByVal SizeMulitiplier As Integer, ByVal Rectangle As Rectangle)
        DrawImageBorder(Texture, SizeMulitiplier, Rectangle, Color.White, False)
    End Sub

    Public Shared Sub DrawImageBorder(ByVal Texture As Texture2D, ByVal SizeMulitiplier As Integer, ByVal Rectangle As Rectangle, ByVal ScaleToScreen As Boolean)
        DrawImageBorder(Texture, SizeMulitiplier, Rectangle, Color.White, ScaleToScreen)
    End Sub

#End Region

#Region "ScrollBars"

    Public Shared Sub DrawScrollBar(ByVal Position As Vector2, ByVal AllItems As Integer, ByVal SeeAbleItems As Integer, ByVal Selection As Integer, ByVal Size As Size, ByVal Horizontal As Boolean, ByVal Color1 As Color, ByVal Color2 As Color)
        DrawScrollBar(Position, AllItems, SeeAbleItems, Selection, Size, Horizontal, Color1, Color2, False)
    End Sub

    Public Shared Sub DrawScrollBar(ByVal Position As Vector2, ByVal AllItems As Integer, ByVal SeeAbleItems As Integer, ByVal Selection As Integer, ByVal Size As Size, ByVal Horizontal As Boolean, ByVal Color1 As Color, ByVal Color2 As Color, ByVal ScaleToScreen As Boolean)
        DrawRectangle(New Rectangle(CInt(Position.X), CInt(Position.Y), Size.Width, Size.Height), Color1, ScaleToScreen)
        Dim canSee As Integer = SeeAbleItems
        If Horizontal = False Then
            Dim sizeY As Integer = Size.Height
            Dim posY As Integer = 0
            If AllItems > SeeAbleItems Then
                sizeY = CInt((canSee / AllItems) * Size.Height)
                posY = CInt(Math.Abs(Selection) * Size.Height / AllItems)
            End If
            DrawRectangle(New Rectangle(CInt(Position.X), CInt(Position.Y) + posY, Size.Width, sizeY), Color2, ScaleToScreen)
        Else
            Dim sizeX As Integer = Size.Width
            Dim posX As Integer = 0
            If AllItems > SeeAbleItems Then
                sizeX = CInt((canSee / AllItems) * Size.Width)
                posX = CInt(Math.Abs(Selection) * Size.Width / AllItems)
            End If
            DrawRectangle(New Rectangle(CInt(Position.X) + posX, CInt(Position.Y), sizeX, Size.Height), Color2, ScaleToScreen)
        End If
    End Sub

    Public Shared Sub DrawScrollBar(ByVal Position As Vector2, ByVal AllItems As Integer, ByVal SeeAbleItems As Integer, ByVal Selection As Integer, ByVal Size As Size, ByVal Horizontal As Boolean, ByVal Texture1 As Texture2D, ByVal Texture2 As Texture2D)
        DrawScrollBar(Position, AllItems, SeeAbleItems, Selection, Size, Horizontal, Texture1, Texture2, False)
    End Sub

    Public Shared Sub DrawScrollBar(ByVal Position As Vector2, ByVal AllItems As Integer, ByVal SeeAbleItems As Integer, ByVal Selection As Integer, ByVal Size As Size, ByVal Horizontal As Boolean, ByVal Texture1 As Texture2D, ByVal Texture2 As Texture2D, ByVal ScaleToScreen As Boolean)
        If ScaleToScreen = True Then
            Core.SpriteBatch.DrawInterface(Texture1, New Rectangle(CInt(Position.X), CInt(Position.Y), Size.Width, Size.Height), Color.White)
        Else
            Core.SpriteBatch.Draw(Texture1, New Rectangle(CInt(Position.X), CInt(Position.Y), Size.Width, Size.Height), Color.White)
        End If
        Dim canSee As Integer = SeeAbleItems
        If Horizontal = False Then
            Dim sizeY As Integer = Size.Height
            Dim posY As Integer = 0
            If AllItems > SeeAbleItems Then
                sizeY = CInt((canSee / AllItems) * Size.Height)
                posY = CInt(Math.Abs(Selection) * Size.Height / AllItems)
            End If
            If ScaleToScreen = True Then
                Core.SpriteBatch.DrawInterface(Texture2, New Rectangle(CInt(Position.X), CInt(Position.Y) + posY, Size.Width, sizeY), Color.White)
            Else
                Core.SpriteBatch.Draw(Texture2, New Rectangle(CInt(Position.X), CInt(Position.Y) + posY, Size.Width, sizeY), Color.White)
            End If
        Else
            Dim sizeX As Integer = Size.Width
            Dim posX As Integer = 0
            If AllItems > SeeAbleItems Then
                sizeX = CInt((canSee / AllItems) * Size.Width)
                posX = CInt(Math.Abs(Selection) * Size.Width / AllItems)
            End If
            If ScaleToScreen = True Then
                Core.SpriteBatch.DrawInterface(Texture2, New Rectangle(CInt(Position.X) + posX, CInt(Position.Y), sizeX, Size.Height), Color.White)
            Else
                Core.SpriteBatch.Draw(Texture2, New Rectangle(CInt(Position.X) + posX, CInt(Position.Y), sizeX, Size.Height), Color.White)
            End If
        End If
    End Sub

#End Region

    Private Structure GradientConfiguration

        Private Texture As Texture2D 'Stores the generated texture 

        Private Width As Integer
        Private Height As Integer
        Private fromColor As Color
        Private toColor As Color
        Private Horizontal As Boolean
        Private Steps As Integer

        Public Sub New(ByVal Width As Integer, ByVal Height As Integer, ByVal fromColor As Color, ByVal toColor As Color, ByVal Horizontal As Boolean, ByVal Steps As Integer)
            Me.Width = Width
            Me.Height = Height
            Me.fromColor = fromColor
            Me.toColor = toColor
            Me.Horizontal = Horizontal
            Me.Steps = Steps

            Me.GenerateTexture()
        End Sub

        Private Sub GenerateTexture()
            Dim uSize As Integer = Me.Height
            If Horizontal = False Then
                uSize = Me.Width
            End If

            Dim diffR As Integer = CInt(toColor.R) - CInt(fromColor.R)
            Dim diffG As Integer = CInt(toColor.G) - CInt(fromColor.G)
            Dim diffB As Integer = CInt(toColor.B) - CInt(fromColor.B)
            Dim diffA As Integer = CInt(toColor.A) - CInt(fromColor.A)

            Dim StepCount As Integer = Steps
            If StepCount < 0 Then
                StepCount = uSize
            End If

            Dim stepSize As Single = CSng(Math.Ceiling(CSng(uSize / StepCount)))

            Dim colorArray As Color() = New Color(Me.Width * Me.Height - 1) {}

            For cStep = 1 To StepCount
                Dim cR As Integer = CInt((diffR / StepCount) * cStep) + fromColor.R
                Dim cG As Integer = CInt((diffG / StepCount) * cStep) + fromColor.G
                Dim cB As Integer = CInt((diffB / StepCount) * cStep) + fromColor.B
                Dim cA As Integer = CInt((diffA / StepCount) * cStep) + fromColor.A

                If cR < 0 Then
                    cR = 255 + cR
                End If
                If cG < 0 Then
                    cG = 255 + cG
                End If
                If cB < 0 Then
                    cB = 255 + cB
                End If
                If cA < 0 Then
                    cA = 255 + cA
                End If

                If Horizontal = True Then 'left to right gradient
                    Dim c As Color = New Color(cR, cG, cB, cA)

                    Dim length As Integer = CInt(Math.Ceiling(stepSize))
                    Dim start As Integer = CInt((cStep - 1) * stepSize)

                    For y = start To start + length - 1
                        For x = 0 To Me.Width - 1
                            Dim i As Integer = x + y * Me.Width
                            colorArray(i) = c
                        Next
                    Next
                Else
                    Dim c As Color = New Color(cR, cG, cB, cA)

                    Dim length As Integer = CInt(Math.Ceiling(stepSize))
                    Dim start As Integer = CInt((cStep - 1) * stepSize)

                    For x = start To start + length - 1
                        For y = 0 To Me.Height - 1
                            Dim i As Integer = x + y * Me.Width
                            colorArray(i) = c
                        Next
                    Next
                End If
            Next

            Me.Texture = New Texture2D(Core.GraphicsDevice, Me.Width, Me.Height)
            Me.Texture.SetData(Of Color)(colorArray)
        End Sub

        Public Function IsConfig(ByVal Width As Integer, ByVal Height As Integer, ByVal fromColor As Color, ByVal toColor As Color, ByVal Horizontal As Boolean, ByVal Steps As Integer) As Boolean
            Return (Me.Width = Width And Me.Height = Height And Me.fromColor = fromColor And Me.toColor = toColor And Me.Horizontal = Horizontal And Me.Steps = Steps)
        End Function

        Public Sub Draw(ByVal spriteBatch As SpriteBatch, ByVal r As Rectangle)
            spriteBatch.Draw(Me.Texture, r, Color.White)
        End Sub

    End Structure

    Shared gradientConfigs As New List(Of GradientConfiguration)

    Public Shared Sub DrawGradient(ByVal Rectangle As Rectangle, ByVal fromColor As Color, ByVal toColor As Color, ByVal Horizontal As Boolean, ByVal Steps As Integer)
        DrawGradient(Core.SpriteBatch, Rectangle, fromColor, toColor, Horizontal, Steps)
    End Sub

    Public Shared Sub DrawGradient(ByVal spriteBatch As SpriteBatch, ByVal Rectangle As Rectangle, ByVal fromColor As Color, ByVal toColor As Color, ByVal Horizontal As Boolean, ByVal Steps As Integer)
        If Rectangle.Width > 0 And Rectangle.Height > 0 Then
            Horizontal = Not Horizontal 'because fuck you.

            Dim gConfig As GradientConfiguration = Nothing
            Dim foundConfig As Boolean = False
            For Each g As GradientConfiguration In gradientConfigs
                If g.IsConfig(Rectangle.Width, Rectangle.Height, fromColor, toColor, Horizontal, Steps) Then
                    gConfig = g
                    foundConfig = True
                    Exit For
                End If
            Next
            If foundConfig = False Then
                gConfig = New GradientConfiguration(Rectangle.Width, Rectangle.Height, fromColor, toColor, Horizontal, Steps)
                gradientConfigs.Add(gConfig)
            End If
            gConfig.Draw(spriteBatch, Rectangle)
        End If
    End Sub

    Public Shared Sub DrawLine(ByVal Color As Color, ByVal startPoint As Vector2, ByVal endPoint As Vector2, ByVal width As Double)
        Dim angle As Double = CDbl(Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X))
        Dim length As Double = Vector2.Distance(startPoint, endPoint)

        Core.SpriteBatch.Draw(Canvas, startPoint, Nothing, Color, CSng(angle), Vector2.Zero, New Vector2(CSng(length), CSng(width)), SpriteEffects.None, 0)
    End Sub

End Class