Namespace Resources.Blur

    Public Class BlurHandler

        Implements IDisposable

        Private Const BLUR_RADIUS As Integer = 7
        Private Const BLUR_AMOUNT As Single = 2.0F

        Private _blurCore As GaussianBlur
        Private _batch As SpriteBatch
        Private _rt1 As RenderTarget2D
        Private _rt2 As RenderTarget2D

        Private _isDisposed As Boolean = False

        Public Property IsDisposed As Boolean
            Get
                Return _isDisposed
            End Get
            Private Set(value As Boolean)
                _isDisposed = value
            End Set
        End Property

        Public Sub New(width As Integer, height As Integer)
            Me.New(Core.Content.Load(Of Effect)("Effects\GaussianBlur"), New SpriteBatch(Core.GraphicsDevice), width, height)
        End Sub

        Public Sub New(gaussianBlurEffect As Effect, batch As SpriteBatch, width As Integer, height As Integer)
            _batch = batch

            _blurCore = New GaussianBlur(gaussianBlurEffect)
            _blurCore.ComputeKernel(BLUR_RADIUS, BLUR_AMOUNT)

            Dim renderTargetWidth = CInt(width / 2)
            Dim renderTargetHeight = CInt(height / 2)

            _rt1 = New RenderTarget2D(Core.GraphicsDevice, renderTargetWidth, renderTargetHeight, False,
                                      Core.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None)
            _rt2 = New RenderTarget2D(Core.GraphicsDevice, renderTargetWidth, renderTargetHeight, False,
                                      Core.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None)

            _blurCore.ComputeOffsets(renderTargetWidth, renderTargetHeight)
        End Sub

        Public Function Perform(texture As Texture2D) As Texture2D
            Return _blurCore.PerformGaussianBlur(texture, _rt1, _rt2)
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

                    If _rt1 IsNot Nothing AndAlso Not _rt1.IsDisposed Then
                        _rt1.Dispose()
                    End If
                    If _rt2 IsNot Nothing AndAlso Not _rt2.IsDisposed Then
                        _rt2.Dispose()
                    End If
                    If _blurCore IsNot Nothing AndAlso Not _blurCore.IsDisposed Then
                        _blurCore.Dispose()
                    End If
                    If _batch IsNot Nothing AndAlso Not _batch.IsDisposed Then
                        _batch.Dispose()
                    End If

                End If

                _rt1 = Nothing
                _rt2 = Nothing
                _blurCore = Nothing
                _batch = Nothing

                IsDisposed = True

            End If
        End Sub

    End Class

End Namespace
