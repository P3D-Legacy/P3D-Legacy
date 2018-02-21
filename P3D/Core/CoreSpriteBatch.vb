''' <summary>
''' A wrapper class for the default SpriteBatch class which extends certain functions of the original class to allow easy screen sizing.
''' </summary>
Public Class CoreSpriteBatch

    Inherits SpriteBatch

    ''' <summary>
    ''' Creates a new instance of the CoreSpriteBatch class.
    ''' </summary>
    ''' <param name="graphicsDevice">The GraphicsDevice where sprites will be drawn.</param>
    Public Sub New(ByVal graphicsDevice As GraphicsDevice)
        MyBase.New(graphicsDevice)

        Me.SetupCanvas()
    End Sub

#Region "StateManagement"

    Private _running As Boolean = False

    ''' <summary>
    ''' Begins the SpriteBatch.
    ''' </summary>
    Public Sub BeginBatch()
        Me.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise)
        Me._running = True
    End Sub

    ''' <summary>
    ''' Ends the SpriteBatch.
    ''' </summary>
    Public Sub EndBatch()
        Me.End()
        Me._running = False
    End Sub

    ''' <summary>
    ''' If the SpriteBatch is running.
    ''' </summary>
    Public ReadOnly Property Running() As Boolean
        Get
            Return Me._running
        End Get
    End Property

#End Region

#Region "Draw"

    ''' <summary>
    ''' Adds a sprite to a batch of sprites for rendering using the specified texture, destination rectangle, and color. 
    ''' </summary>
    ''' <param name="Texture">A texture.</param>
    ''' <param name="DestinationRectangle">A rectangle that specifies (in screen coordinates) the destination for drawing the sprite.</param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    Public Sub DrawInterface(ByVal Texture As Texture2D, ByVal DestinationRectangle As Rectangle, ByVal Color As Color)
        DrawInterface(Texture, DestinationRectangle, Nothing, Color, 0.0F, Vector2.Zero, SpriteEffects.None, 0.0F, True)
    End Sub

    ''' <summary>
    ''' Adds a sprite to a batch of sprites for rendering using the specified texture, destination rectangle, source rectangle, and color. 
    ''' </summary>
    ''' <param name="Texture">A texture.</param>
    ''' <param name="DestinationRectangle">A rectangle that specifies (in screen coordinates) the destination for drawing the sprite.</param>
    ''' <param name="SourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    Public Sub DrawInterface(ByVal Texture As Texture2D, ByVal DestinationRectangle As Rectangle, ByVal SourceRectangle As Rectangle?, ByVal Color As Color)
        DrawInterface(Texture, DestinationRectangle, SourceRectangle, Color, 0.0F, Vector2.Zero, SpriteEffects.None, 0.0F, True)
    End Sub

    ''' <summary>
    ''' Adds a sprite to a batch of sprites for rendering using the specified texture, destination rectangle, source rectangle, color, rotation, origin, effects and layer. 
    ''' </summary>
    ''' <param name="Texture">A texture.</param>
    ''' <param name="DestinationRectangle">A rectangle that specifies (in screen coordinates) the destination for drawing the sprite.</param>
    ''' <param name="SourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
    ''' <param name="Origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="Effects">Effects to apply.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    Public Sub DrawInterface(ByVal Texture As Texture2D, ByVal DestinationRectangle As Rectangle, ByVal SourceRectangle As Rectangle?, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal Effects As SpriteEffects, ByVal LayerDepth As Single)
        DrawInterface(Texture, DestinationRectangle, SourceRectangle, Color, Rotation, Origin, Effects, LayerDepth, True)
    End Sub

    ''' <summary>
    ''' Adds a sprite to a batch of sprites for rendering using the specified texture, destination rectangle, source rectangle, color, rotation, origin, effects and layer. 
    ''' </summary>
    ''' <param name="Texture">A texture.</param>
    ''' <param name="DestinationRectangle">A rectangle that specifies (in screen coordinates) the destination for drawing the sprite.</param>
    ''' <param name="SourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
    ''' <param name="Origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="Effects">Effects to apply.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    ''' <param name="TransformPosition">If the position of the sprite should be transformed by screen sizing.</param>
    Public Sub DrawInterface(ByVal Texture As Texture2D, ByVal DestinationRectangle As Rectangle, ByVal SourceRectangle As Rectangle?, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal Effects As SpriteEffects, ByVal LayerDepth As Single, ByVal TransformPosition As Boolean)
        'Do the conversion of the rectangle here.

        Dim x As Double = InterfaceScale()
        If TransformPosition = True Then
            DestinationRectangle = New Rectangle(CInt(DestinationRectangle.X * x), CInt(DestinationRectangle.Y * x), CInt(DestinationRectangle.Width * x), CInt(DestinationRectangle.Height * x))
        Else
            DestinationRectangle = New Rectangle(CInt(DestinationRectangle.X * x), CInt(DestinationRectangle.Y * x), CInt(DestinationRectangle.Width), CInt(DestinationRectangle.Height))
        End If

        Draw(Texture, DestinationRectangle, SourceRectangle, Color, Rotation, Origin, Effects, LayerDepth)
    End Sub

    ''' <summary>
    ''' Adds a sprite to a batch of sprites for rendering using the specified texture, position and color. 
    ''' </summary>
    ''' <param name="Texture">A texture.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    Public Sub DrawInterface(ByVal Texture As Texture2D, ByVal Position As Vector2, ByVal Color As Color)
        DrawInterface(Texture, Position, Nothing, Color, 0.0F, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0F, True)
    End Sub

    ''' <summary>
    ''' Adds a sprite to a batch of sprites for rendering using the specified texture, position, source rectangle, and color. 
    ''' </summary>
    ''' <param name="Texture">A texture.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="SourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    Public Sub DrawInterface(ByVal Texture As Texture2D, ByVal Position As Vector2, ByVal SourceRectangle As Rectangle?, ByVal Color As Color)
        DrawInterface(Texture, Position, SourceRectangle, Color, 0.0F, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0F, True)
    End Sub

    ''' <summary>
    ''' Adds a sprite to a batch of sprites for rendering using the specified texture, position, source rectangle, color, rotation, origin, scale, effects, and layer. 
    ''' </summary>
    ''' <param name="Texture">A texture.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="SourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
    ''' <param name="Origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="Scale">Scale factor.</param>
    ''' <param name="Effects">Effects to apply.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    Public Sub DrawInterface(ByVal Texture As Texture2D, ByVal Position As Vector2, ByVal SourceRectangle As Rectangle?, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal Scale As Single, ByVal Effects As SpriteEffects, ByVal LayerDepth As Single)
        DrawInterface(Texture, Position, SourceRectangle, Color, Rotation, Origin, New Vector2(Scale), Effects, LayerDepth, True)
    End Sub

    ''' <summary>
    ''' Adds a sprite to a batch of sprites for rendering using the specified texture, position, source rectangle, color, rotation, origin, scale, effects, and layer. 
    ''' </summary>
    ''' <param name="Texture">A texture.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="SourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
    ''' <param name="Origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="Scale">Scale factor.</param>
    ''' <param name="Effects">Effects to apply.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    Public Sub DrawInterface(ByVal Texture As Texture2D, ByVal Position As Vector2, ByVal SourceRectangle As Rectangle?, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal Scale As Vector2, ByVal Effects As SpriteEffects, ByVal LayerDepth As Single)
        DrawInterface(Texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, Effects, LayerDepth, True)
    End Sub

    ''' <summary>
    ''' Adds a sprite to a batch of sprites for rendering using the specified texture, position, source rectangle, color, rotation, origin, scale, effects, and layer. 
    ''' </summary>
    ''' <param name="Texture">A texture.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="SourceRectangle">A rectangle that specifies (in texels) the source texels from a texture. Use null to draw the entire texture. </param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
    ''' <param name="Origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="Scale">Scale factor.</param>
    ''' <param name="Effects">Effects to apply.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    ''' <param name="TransformPosition">If the position of the sprite should be transformed by screen sizing.</param>
    Public Sub DrawInterface(ByVal Texture As Texture2D, ByVal Position As Vector2, ByVal SourceRectangle As Rectangle?, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal Scale As Vector2, ByVal Effects As SpriteEffects, ByVal LayerDepth As Single, ByVal TransformPosition As Boolean)
        'Do the conversion of the rectangle and scale here.

        Dim x As Double = InterfaceScale()
        If TransformPosition = True Then
            Position = New Vector2(CSng(Position.X * x), CSng(Position.Y * x))
        End If
        Scale = New Vector2(CSng(Scale.X * x), CSng(Scale.Y * x))

        Draw(Texture, Position, SourceRectangle, Color, Rotation, Origin, Scale, Effects, LayerDepth)
    End Sub

#End Region

#Region "DrawString"

    ''' <summary>
    ''' Adds a string to a batch of sprites for rendering using the specified font, text, position, and color. 
    ''' </summary>
    ''' <param name="Font">A font for diplaying text.</param>
    ''' <param name="Text">A text string.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    Public Sub DrawInterfaceString(ByVal Font As SpriteFont, ByVal Text As String, ByVal Position As Vector2, ByVal Color As Color)
        DrawInterfaceString(Font, New System.Text.StringBuilder(Text), Position, Color, 0.0F, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0F, True)
    End Sub

    ''' <summary>
    ''' Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer. 
    ''' </summary>
    ''' <param name="Font">A font for diplaying text.</param>
    ''' <param name="Text">A text string.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
    ''' <param name="Origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="Scale">Scale factor.</param>
    ''' <param name="Effects">Effects to apply.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    Public Sub DrawInterfaceString(ByVal Font As SpriteFont, ByVal Text As String, ByVal Position As Vector2, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal Scale As Single, ByVal Effects As SpriteEffects, ByVal LayerDepth As Single)
        DrawInterfaceString(Font, New System.Text.StringBuilder(Text), Position, Color, Rotation, Origin, New Vector2(Scale), Effects, LayerDepth, True)
    End Sub

    ''' <summary>
    ''' Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer. 
    ''' </summary>
    ''' <param name="Font">A font for diplaying text.</param>
    ''' <param name="Text">A text string.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
    ''' <param name="Origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="Scale">Scale factor.</param>
    ''' <param name="Effects">Effects to apply.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    Public Sub DrawInterfaceString(ByVal Font As SpriteFont, ByVal Text As String, ByVal Position As Vector2, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal Scale As Vector2, ByVal Effects As SpriteEffects, ByVal LayerDepth As Single)
        DrawInterfaceString(Font, New System.Text.StringBuilder(Text), Position, Color, Rotation, Origin, Scale, Effects, LayerDepth, True)
    End Sub

    ''' <summary>
    ''' Adds a string to a batch of sprites for rendering using the specified font, text, position, and color. 
    ''' </summary>
    ''' <param name="Font">A font for diplaying text.</param>
    ''' <param name="Text">A text string.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    Public Sub DrawInterfaceString(ByVal Font As SpriteFont, ByVal Text As System.Text.StringBuilder, ByVal Position As Vector2, ByVal Color As Color)
        DrawInterfaceString(Font, Text, Position, Color, 0.0F, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.0F, True)
    End Sub

    ''' <summary>
    ''' Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer. 
    ''' </summary>
    ''' <param name="Font">A font for diplaying text.</param>
    ''' <param name="Text">A text string.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
    ''' <param name="Origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="Scale">Scale factor.</param>
    ''' <param name="Effects">Effects to apply.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    Public Sub DrawInterfaceString(ByVal Font As SpriteFont, ByVal Text As System.Text.StringBuilder, ByVal Position As Vector2, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal Scale As Single, ByVal Effects As SpriteEffects, ByVal LayerDepth As Single)
        DrawInterfaceString(Font, Text, Position, Color, Rotation, Origin, New Vector2(Scale), Effects, LayerDepth, True)
    End Sub

    ''' <summary>
    ''' Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer. 
    ''' </summary>
    ''' <param name="Font">A font for diplaying text.</param>
    ''' <param name="Text">A text string.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
    ''' <param name="Origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="Scale">Scale factor.</param>
    ''' <param name="Effects">Effects to apply.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    Public Sub DrawInterfaceString(ByVal Font As SpriteFont, ByVal Text As System.Text.StringBuilder, ByVal Position As Vector2, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal Scale As Vector2, ByVal Effects As SpriteEffects, ByVal LayerDepth As Single)
        DrawInterfaceString(Font, Text, Position, Color, Rotation, Origin, Scale, Effects, LayerDepth, True)
    End Sub

    ''' <summary>
    ''' Adds a string to a batch of sprites for rendering using the specified font, text, position, color, rotation, origin, scale, effects and layer. 
    ''' </summary>
    ''' <param name="Font">A font for diplaying text.</param>
    ''' <param name="Text">A text string.</param>
    ''' <param name="Position">The location (in screen coordinates) to draw the sprite.</param>
    ''' <param name="Color">The color to tint a sprite. Use Color.White for full color with no tinting.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the sprite about its center.</param>
    ''' <param name="Origin">The sprite origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="Scale">Scale factor.</param>
    ''' <param name="Effects">Effects to apply.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    ''' <param name="TransformPosition">If the position of the text should be transformed by screen sizing.</param>
    Public Sub DrawInterfaceString(ByVal Font As SpriteFont, ByVal Text As System.Text.StringBuilder, ByVal Position As Vector2, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal Scale As Vector2, ByVal Effects As SpriteEffects, ByVal LayerDepth As Single, ByVal TransformPosition As Boolean)
        Dim x As Double = InterfaceScale()
        If TransformPosition = True Then
            Position = New Vector2(CSng(Position.X * x), CSng(Position.Y * x))
        End If
        Scale = New Vector2(CSng(Scale.X * x), CSng(Scale.Y * x))

        DrawString(Font, Text, Position, Color, Rotation, Origin, Scale, Effects, LayerDepth)
    End Sub

#End Region

    ''' <summary>
    ''' The scale of the interface depending on the window size.
    ''' </summary>
    Public ReadOnly Property InterfaceScale() As Double
        Get
            If Core.windowSize.Height < Core.CurrentScreen.GetScreenScaleMinimum().Height Or Core.windowSize.Width < Core.CurrentScreen.GetScreenScaleMinimum().Width Then
                Return 0.5D
            End If
            Return 1D
        End Get
    End Property

#Region "Canvas"

    Private _canvasTexture As Texture2D

    ''' <summary>
    ''' Creates the canvas texture.
    ''' </summary>
    Private Sub SetupCanvas()
        _canvasTexture = New Texture2D(Me.GraphicsDevice, 1, 1)
        _canvasTexture.SetData(Of Color)({Color.White})
    End Sub

#Region "DrawRectangle"

    ''' <summary>
    ''' Adds a Rectangle sized sprite to a batch of sprites for rendering using the specified rectangle and color.
    ''' </summary>
    ''' <param name="DestinationRectangle">A rectangle.</param>
    ''' <param name="Color">The color of the rectangle.</param>
    Public Sub DrawRectangle(ByVal DestinationRectangle As Rectangle, ByVal Color As Color)
        Me.DrawRectangle(DestinationRectangle, Color, 0F, Vector2.Zero, 0F, False)
    End Sub

    ''' <summary>
    ''' Adds a Rectangle sized sprite to a batch of sprites for rendering using the specified rectangle, color, rotation, origin and layer.
    ''' </summary>
    ''' <param name="DestinationRectangle">A rectangle.</param>
    ''' <param name="Color">The color of the rectangle.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the rectangle about its center.</param>
    ''' <param name="Origin">The rectangle origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    Public Sub DrawRectangle(ByVal DestinationRectangle As Rectangle, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal LayerDepth As Single)
        Me.DrawRectangle(DestinationRectangle, Color, Rotation, Origin, LayerDepth, False)
    End Sub

    ''' <summary>
    ''' Adds a Rectangle sized sprite to a batch of sprites for rendering using the specified rectangle, color, rotation, origin and layer.
    ''' </summary>
    ''' <param name="DestinationRectangle">A rectangle.</param>
    ''' <param name="Color">The color of the rectangle.</param>
    ''' <param name="Rotation">Specifies the angle (in radians) to rotate the rectangle about its center.</param>
    ''' <param name="Origin">The rectangle origin; the default is (0,0) which represents the upper-left corner.</param>
    ''' <param name="LayerDepth">The depth of a layer. By default, 0 represents the front layer and 1 represents a back layer. Use SpriteSortMode if you want sprites to be sorted during drawing.</param>
    ''' <param name="ScaleToScreen">If the rectangle should get scaled by screen sizing.</param>
    Public Sub DrawRectangle(ByVal DestinationRectangle As Rectangle, ByVal Color As Color, ByVal Rotation As Single, ByVal Origin As Vector2, ByVal LayerDepth As Single, ByVal ScaleToScreen As Boolean)
        If ScaleToScreen = True Then
            Core.SpriteBatch.DrawInterface(Me._canvasTexture, DestinationRectangle, Nothing, Color, Rotation, Origin, SpriteEffects.None, LayerDepth, True)
        Else
            Core.SpriteBatch.Draw(Me._canvasTexture, DestinationRectangle, Nothing, Color, Rotation, Origin, SpriteEffects.None, LayerDepth)
        End If
    End Sub

#End Region

#Region "DrawLine"

    'TODO: FInish line drawing stuff

    '1: Color, Position, Width, Height
    '2: Color, StartingPosition, EndingPosition, Thickness

    Public Sub DrawLine(ByVal Position As Vector2, ByVal Width As Single, ByVal Height As Single, ByVal Color As Color)
        Dim thickness As Single = 0F
        Dim length As Single = 0F

        Dim startPos As New Vector2(Position.X, Position.Y)
        Dim endPos As New Vector2(Position.X + Width, Position.Y)

        If Width > Height Then
            If Width > 0 Then

            Else

            End If
        Else

        End If
    End Sub

    Public Sub DrawLine(ByVal StartingPosition As Vector2, ByVal EndingPosition As Vector2, ByVal Thickness As Single, ByVal Color As Color)
        Dim angle As Double = CDbl(Math.Atan2(EndingPosition.Y - StartingPosition.Y, EndingPosition.X - StartingPosition.X))
        Dim length As Double = Vector2.Distance(StartingPosition, EndingPosition)

        Core.SpriteBatch.Draw(_canvasTexture, StartingPosition, Nothing, Color, CSng(angle), Vector2.Zero, New Vector2(CSng(length), CSng(Thickness)), SpriteEffects.None, 0)
    End Sub

#End Region

#End Region

End Class
