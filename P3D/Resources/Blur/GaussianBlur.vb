Imports Microsoft.VisualBasic
Namespace Resources.Blur

    Public Class GaussianBlur

        Implements IDisposable

        Private _effect As Effect
        Private _spriteBatch As SpriteBatch
        Private _radius As Integer
        Private _amount As Single
        Private _sigma As Single
        Private _kernel As Single()
        Private _offsetHoriz As Vector2()
        Private _offsetVert As Vector2()
        Private _isDisposed As Boolean = False

        Public Property IsDisposed As Boolean
            Get
                Return _isDisposed
            End Get
            Private Set(value As Boolean)
                _isDisposed = value
            End Set
        End Property

        Public Sub New(gaussianBlurEffect As Effect)
            _effect = gaussianBlurEffect
            _spriteBatch = New SpriteBatch(Core.GraphicsDevice)
        End Sub

        Friend Sub ComputeKernel(blurRadius As Integer, blurAmount As Single)
            _radius = blurRadius
            _amount = blurAmount

            _kernel = Nothing
            _kernel = New Single(_radius * 2) {}
            _sigma = _radius / _amount

            Dim twoSigmaSquare As Single = 2.0F * _sigma * _sigma
            Dim sigmaRoot As Single = CSng(Math.Sqrt(twoSigmaSquare * Math.PI))
            Dim total As Single = 0F
            Dim distance As Single = 0F
            Dim index As Integer = 0

            For i As Integer = -_radius To _radius
                distance = i * i
                index = i + _radius
                _kernel(index) = CSng(Math.Exp(-distance / twoSigmaSquare)) / sigmaRoot
                total += _kernel(index)
            Next

            For i As Integer = 0 To _kernel.Length - 1
                _kernel(i) /= total
            Next
        End Sub

        Friend Sub ComputeOffsets(textureWidth As Single, textureHeight As Single)
            _offsetHoriz = Nothing
            _offsetHoriz = New Vector2(_radius * 2) {}

            _offsetVert = Nothing
            _offsetVert = New Vector2(_radius * 2) {}

            Dim index As Integer = 0
            Dim xOffset As Single = 1.0F / textureWidth
            Dim yOffset As Single = 1.0F / textureHeight

            For i As Integer = -_radius To _radius
                index = i + _radius
                _offsetHoriz(index) = New Vector2(i * xOffset, 0F)
                _offsetVert(index) = New Vector2(0F, i * yOffset)
            Next
        End Sub

        Friend Function PerformGaussianBlur(srcTexture As Texture2D, renderTarget1 As RenderTarget2D, renderTarget2 As RenderTarget2D) As Texture2D

            If _effect Is Nothing Then
                Throw New InvalidOperationException("Blur effect not loaded")
            End If

            Dim outputTexture As Texture2D = Nothing
            Dim srcRect = New Rectangle(0, 0, srcTexture.Width, srcTexture.Height)
            Dim destRect1 = New Rectangle(0, 0, renderTarget1.Width, renderTarget1.Height)
            Dim destRect2 = New Rectangle(0, 0, renderTarget2.Width, renderTarget2.Height)

            ' perform horizontal blur
            GraphicsDevice.SetRenderTarget(renderTarget1)
            _effect.CurrentTechnique = _effect.Techniques("GaussianBlur")
            _effect.Parameters("weights").SetValue(_kernel)
            _effect.Parameters("colorMapTexture").SetValue(srcTexture)
            _effect.Parameters("offsets").SetValue(_offsetHoriz)

            _spriteBatch.Begin(effect:=_effect)
            _spriteBatch.Draw(srcTexture, destRect1, Color.White)
            _spriteBatch.End()

            ' perform vertical blur
            GraphicsDevice.SetRenderTarget(renderTarget2)
            outputTexture = CType(renderTarget1, Texture2D)

            _effect.Parameters("colorMapTexture").SetValue(outputTexture)
            _effect.Parameters("offsets").SetValue(_offsetVert)

            _spriteBatch.Begin(effect:=_effect)
            _spriteBatch.Draw(outputTexture, destRect2, Color.White)
            _spriteBatch.End()

            GraphicsDevice.SetRenderTarget(Nothing)
            Return renderTarget2

        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
        End Sub

        Protected Overrides Sub Finalize()
            Dispose(False)
        End Sub

        Private Sub Dispose(disposing As Boolean)
            If Not IsDisposed Then

                If disposing Then

                    If _spriteBatch IsNot Nothing AndAlso Not _spriteBatch.IsDisposed Then
                        _spriteBatch.Dispose()
                    End If

                End If

                _effect = Nothing
                _spriteBatch = Nothing

                IsDisposed = True

            End If
        End Sub

    End Class

End Namespace
