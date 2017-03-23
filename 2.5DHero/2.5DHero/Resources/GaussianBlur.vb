Imports Microsoft.Xna.Framework

Namespace Resources

    ''' <summary>
    ''' A class to apply the gaussian effect to a texture.
    ''' </summary>
    Class GaussianEffect

        Private Const BLUR_RADIUS As Integer = 7
        Private Const BLUR_AMOUNT As Single = 2.0F

        Private _spriteBatch As SpriteBatch
        Private _blurCore As GaussianBlur

        Private _rt1, _rt2 As RenderTarget2D
        Private _width, _height As Integer

        ''' <summary>
        ''' Creates a new instance of the effect handler class and sets its intended dimensions.
        ''' </summary>
        Public Sub New(ByVal width As Integer, ByVal height As Integer)
            _spriteBatch = New SpriteBatch(GraphicsDevice)

            _blurCore = New GaussianBlur(Core.GameInstance)
            _blurCore.ComputeKernel(BLUR_RADIUS, BLUR_AMOUNT)

            UpdateDimensions(width, height)
        End Sub

        Private Sub UpdateDimensions(ByVal width As Integer, ByVal height As Integer)
            _width = width
            _height = height

            Dim targetWidth As Integer = CInt(width / 2)
            Dim targetHeight As Integer = CInt(height / 2)

            _rt1 = New RenderTarget2D(GraphicsDevice,
                                      targetWidth, targetHeight)
            _rt2 = New RenderTarget2D(GraphicsDevice,
                                      targetWidth, targetHeight)

            _blurCore.ComputeOffsets(targetWidth, targetHeight)
        End Sub

        ''' <summary>
        ''' Performs the effect on a texture.
        ''' </summary>
        Public Function Perform(ByVal texture As Texture2D) As Texture2D
            If texture.Width <> _width Or texture.Height <> _height Then

                'fuk OpenTK
                ''UpdateDimensions(texture.Width, texture.Height)
            End If

            Return _blurCore.PerformGaussianBlur(texture, _rt1, _rt2, _spriteBatch)
        End Function

        Private Class GaussianBlur

            '-----------------------------------------------------------------------------
            ' Copyright (c) 2008-2011 dhpoware. All Rights Reserved.
            '
            ' Permission is hereby granted, free of charge, to any person obtaining a
            ' copy of this software and associated documentation files (the "Software"),
            ' to deal in the Software without restriction, including without limitation
            ' the rights to use, copy, modify, merge, publish, distribute, sublicense,
            ' and/or sell copies of the Software, and to permit persons to whom the
            ' Software is furnished to do so, subject to the following conditions:
            '
            ' The above copyright notice and this permission notice shall be included in
            ' all copies or substantial portions of the Software.
            '
            ' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
            ' OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
            ' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
            ' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
            ' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
            ' FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
            ' IN THE SOFTWARE.
            '-----------------------------------------------------------------------------
            ' Converted to VB.Net by nilllzz (C) 20XX scrollupabuildingandrevealmenu.
            '-----------------------------------------------------------------------------

            Private game As Microsoft.Xna.Framework.Game
            Private effect As Effect
            Private m_radius As Integer
            Private m_amount As Single
            Private m_sigma As Single
            Private m_kernel As Single()
            Private offsetsHoriz As Vector2()
            Private offsetsVert As Vector2()

            ''' <summary>
            ''' Returns the radius of the Gaussian blur filter kernel in pixels.
            ''' </summary>
            Public ReadOnly Property Radius() As Integer
                Get
                    Return m_radius
                End Get
            End Property

            ''' <summary>
            ''' Returns the blur amount. This value is used to calculate the
            ''' Gaussian blur filter kernel's sigma value. Good values for this
            ''' property are 2 and 3. 2 will give a more blurred result whilst 3
            ''' will give a less blurred result with sharper details.
            ''' </summary>
            Public ReadOnly Property Amount() As Single
                Get
                    Return m_amount
                End Get
            End Property

            ''' <summary>
            ''' Returns the Gaussian blur filter's standard deviation.
            ''' </summary>
            Public ReadOnly Property Sigma() As Single
                Get
                    Return m_sigma
                End Get
            End Property

            ''' <summary>
            ''' Returns the Gaussian blur filter kernel matrix. Note that the
            ''' kernel returned is for a 1D Gaussian blur filter kernel matrix
            ''' intended to be used in a two pass Gaussian blur operation.
            ''' </summary>
            Public ReadOnly Property Kernel() As Single()
                Get
                    Return m_kernel
                End Get
            End Property

            ''' <summary>
            ''' Returns the texture offsets used for the horizontal Gaussian blur
            ''' pass.
            ''' </summary>
            Public ReadOnly Property TextureOffsetsX() As Vector2()
                Get
                    Return offsetsHoriz
                End Get
            End Property

            ''' <summary>
            ''' Returns the texture offsets used for the vertical Gaussian blur
            ''' pass.
            ''' </summary>
            Public ReadOnly Property TextureOffsetsY() As Vector2()
                Get
                    Return offsetsVert
                End Get
            End Property

            ''' <summary>
            ''' Default constructor for the GaussianBlur class. This constructor
            ''' should be called if you don't want the GaussianBlur class to use
            ''' its GaussianBlur.fx effect file to perform the two pass Gaussian
            ''' blur operation.
            ''' </summary>
            Public Sub New()
            End Sub

            ''' <summary>
            ''' This overloaded constructor instructs the GaussianBlur class to
            ''' load and use its GaussianBlur.fx effect file that implements the
            ''' two pass Gaussian blur operation on the GPU. The effect file must
            ''' be already bound to the asset name: 'Effects\GaussianBlur' or
            ''' 'GaussianBlur'.
            ''' </summary>
            Public Sub New(game As Microsoft.Xna.Framework.Game)
                Me.game = game

                '''Requires file restructure
                'effect = Content.Load(Of Effect)("SharedResources\Effects\GaussianBlur")
                effect = Content.Load(Of Effect)("Effects\GaussianBlur")


                'Try
                '    effect = game.Content.Load(Of Effect)("SharedResources\Effects\GaussianBlur")
                'Catch generatedExceptionName As ContentLoadException
                '    effect = game.Content.Load(Of Effect)("GaussianBlur")
                'End Try
            End Sub

            ''' <summary>
            ''' Calculates the Gaussian blur filter kernel. This implementation is
            ''' ported from the original Java code appearing in chapter 16 of
            ''' "Filthy Rich Clients: Developing Animated and Graphical Effects for
            ''' Desktop Java".
            ''' </summary>
            ''' <param name="blurRadius">The blur radius in pixels.</param>
            ''' <param name="blurAmount">Used to calculate sigma.</param>
            Public Sub ComputeKernel(blurRadius As Integer, blurAmount As Single)
                m_radius = blurRadius
                m_amount = blurAmount

                m_kernel = Nothing
                m_kernel = New Single(m_radius * 2) {}
                m_sigma = m_radius / m_amount

                Dim twoSigmaSquare As Single = 2.0F * m_sigma * m_sigma
                Dim sigmaRoot As Single = CSng(Math.Sqrt(twoSigmaSquare * Math.PI))
                Dim total As Single = 0F
                Dim distance As Single = 0F
                Dim index As Integer = 0

                For i As Integer = -m_radius To m_radius
                    distance = i * i
                    index = i + m_radius
                    m_kernel(index) = CSng(Math.Exp(-distance / twoSigmaSquare)) / sigmaRoot
                    total += m_kernel(index)
                Next

                For i As Integer = 0 To m_kernel.Length - 1
                    m_kernel(i) /= total
                Next
            End Sub

            ''' <summary>
            ''' Calculates the texture coordinate offsets corresponding to the
            ''' calculated Gaussian blur filter kernel. Each of these offset values
            ''' are added to the current pixel's texture coordinates in order to
            ''' obtain the neighboring texture coordinates that are affected by the
            ''' Gaussian blur filter kernel. This implementation has been adapted
            ''' from chapter 17 of "Filthy Rich Clients: Developing Animated and
            ''' Graphical Effects for Desktop Java".
            ''' </summary>
            ''' <param name="textureWidth">The texture width in pixels.</param>
            ''' <param name="textureHeight">The texture height in pixels.</param>
            Public Sub ComputeOffsets(textureWidth As Single, textureHeight As Single)
                offsetsHoriz = Nothing
                offsetsHoriz = New Vector2(m_radius * 2) {}

                offsetsVert = Nothing
                offsetsVert = New Vector2(m_radius * 2) {}

                Dim index As Integer = 0
                Dim xOffset As Single = 1.0F / textureWidth
                Dim yOffset As Single = 1.0F / textureHeight

                For i As Integer = -m_radius To m_radius
                    index = i + m_radius
                    offsetsHoriz(index) = New Vector2(i * xOffset, 0F)
                    offsetsVert(index) = New Vector2(0F, i * yOffset)
                Next
            End Sub

            ''' <summary>
            ''' Performs the Gaussian blur operation on the source texture image.
            ''' The Gaussian blur is performed in two passes: a horizontal blur
            ''' pass followed by a vertical blur pass. The output from the first
            ''' pass is rendered to renderTarget1. The output from the second pass
            ''' is rendered to renderTarget2. The dimensions of the blurred texture
            ''' is therefore equal to the dimensions of renderTarget2.
            ''' </summary>
            ''' <param name="srcTexture">The source image to blur.</param>
            ''' <param name="renderTarget1">Stores the output from the horizontal blur pass.</param>
            ''' <param name="renderTarget2">Stores the output from the vertical blur pass.</param>
            ''' <param name="spriteBatch">Used to draw quads for the blur passes.</param>
            ''' <returns>The resulting Gaussian blurred image.</returns>
            Public Function PerformGaussianBlur(srcTexture As Texture2D, renderTarget1 As RenderTarget2D, renderTarget2 As RenderTarget2D, spriteBatch As SpriteBatch) As Texture2D
                If effect Is Nothing Then
                    Throw New InvalidOperationException("GaussianBlur.fx effect not loaded.")
                End If

                Dim outputTexture As Texture2D = Nothing
                Dim srcRect As New Rectangle(0, 0, srcTexture.Width, srcTexture.Height)
                Dim destRect1 As New Rectangle(0, 0, renderTarget1.Width, renderTarget1.Height)
                Dim destRect2 As New Rectangle(0, 0, renderTarget2.Width, renderTarget2.Height)

                ' Perform horizontal Gaussian blur.

                GraphicsDevice.SetRenderTarget(renderTarget1)

                effect.CurrentTechnique = effect.Techniques("GaussianBlur")
                effect.Parameters("weights").SetValue(m_kernel)
                effect.Parameters("colorMapTexture").SetValue(srcTexture)
                effect.Parameters("offsets").SetValue(offsetsHoriz)

                spriteBatch.Begin(0, BlendState.Opaque, Nothing, Nothing, Nothing, effect)
                spriteBatch.Draw(srcTexture, destRect1, Color.White)
                spriteBatch.[End]()

                ' Perform vertical Gaussian blur.

                GraphicsDevice.SetRenderTarget(renderTarget2)
                outputTexture = DirectCast(renderTarget1, Texture2D)

                effect.Parameters("colorMapTexture").SetValue(outputTexture)
                effect.Parameters("offsets").SetValue(offsetsVert)

                spriteBatch.Begin(0, BlendState.Opaque, Nothing, Nothing, Nothing, effect)
                spriteBatch.Draw(outputTexture, destRect2, Color.White)
                spriteBatch.[End]()

                ' Return the Gaussian blurred texture.

                GraphicsDevice.SetRenderTarget(Nothing)
                outputTexture = renderTarget2

                Return outputTexture
            End Function

        End Class

    End Class

End Namespace