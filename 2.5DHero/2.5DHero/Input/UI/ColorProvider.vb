Namespace Screens.UI

    ''' <summary>
    ''' A class that supplies color information based on the currently loaded profile.
    ''' </summary>
    Public Class ColorProvider

        Public Shared ReadOnly Property IsGameJolt() As Boolean
            Get
                If Not Core.Player Is Nothing Then
                    Return Core.Player.IsGameJoltSave
                End If
                Return False
            End Get
        End Property

        Private Shared ReadOnly _gradientColor As Color = New Color(42, 167, 198)
        Private Shared ReadOnly _gameJolt_gradientColor As Color = New Color(45, 45, 45)

        Public Shared ReadOnly Property GradientColor() As Color
            Get
                Return GradientColor(IsGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property GradientColor(ByVal isGameJolt As Boolean) As Color
            Get
                If isGameJolt Then
                    Return _gameJolt_gradientColor
                Else
                    Return _gradientColor
                End If
            End Get
        End Property

        Public Shared ReadOnly Property GradientColor(ByVal isGameJolt As Boolean, ByVal alpha As Integer) As Color
            Get
                If isGameJolt Then
                    Return New Color(_gameJolt_gradientColor.R, _gameJolt_gradientColor.G, _gameJolt_gradientColor.B, alpha)
                Else
                    Return New Color(_gradientColor.R, _gradientColor.G, _gradientColor.B, alpha)
                End If
            End Get
        End Property

        Private Shared ReadOnly _mainColor As Color = New Color(84, 198, 216)
        Private Shared ReadOnly _gameJolt_mainColor As Color = New Color(39, 39, 39)

        Public Shared ReadOnly Property MainColor() As Color
            Get
                Return MainColor(IsGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property MainColor(ByVal isGameJolt As Boolean) As Color
            Get
                If isGameJolt Then
                    Return _gameJolt_mainColor
                Else
                    Return _mainColor
                End If
            End Get
        End Property

        Public Shared ReadOnly Property MainColor(ByVal isGameJolt As Boolean, ByVal alpha As Integer) As Color
            Get
                If isGameJolt Then
                    Return New Color(_gameJolt_mainColor.R, _gameJolt_mainColor.G, _gameJolt_mainColor.B, alpha)
                Else
                    Return New Color(_mainColor.R, _mainColor.G, _mainColor.B, alpha)
                End If
            End Get
        End Property

        Private Shared ReadOnly _lightColor As Color = New Color(125, 204, 216)
        Private Shared ReadOnly _gameJolt_lightColor As Color = New Color(70, 70, 70)

        Public Shared ReadOnly Property LightColor() As Color
            Get
                Return LightColor(IsGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property LightColor(ByVal isGameJolt As Boolean) As Color
            Get
                If isGameJolt Then
                    Return _gameJolt_lightColor
                Else
                    Return _lightColor
                End If
            End Get
        End Property

        Public Shared ReadOnly Property LightColor(ByVal isGameJolt As Boolean, ByVal alpha As Integer) As Color
            Get
                If isGameJolt Then
                    Return New Color(_gameJolt_lightColor.R, _gameJolt_lightColor.G, _gameJolt_lightColor.B, alpha)
                Else
                    Return New Color(_lightColor.R, _lightColor.G, _lightColor.B, alpha)
                End If
            End Get
        End Property

        Private Shared ReadOnly _accentColor As Color = New Color(30, 139, 227)
        Private Shared ReadOnly _gameJolt_accentColor As Color = New Color(204, 255, 0)

        Public Shared ReadOnly Property AccentColor() As Color
            Get
                Return AccentColor(IsGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property AccentColor(ByVal isGameJolt As Boolean) As Color
            Get
                If isGameJolt Then
                    Return _gameJolt_accentColor
                Else
                    Return _accentColor
                End If
            End Get
        End Property

        Public Shared ReadOnly Property AccentColor(ByVal isGameJolt As Boolean, ByVal alpha As Integer) As Color
            Get
                If isGameJolt Then
                    Return New Color(_gameJolt_accentColor.R, _gameJolt_accentColor.G, _gameJolt_accentColor.B, alpha)
                Else
                    Return New Color(_accentColor.R, _accentColor.G, _accentColor.B, alpha)
                End If
            End Get
        End Property

    End Class

End Namespace