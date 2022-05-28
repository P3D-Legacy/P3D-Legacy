Namespace Screens.UI

    ''' <summary>
    ''' A class that supplies color information based on the currently loaded profile.
    ''' </summary>
    Public Class ColorProvider

        Public Shared ReadOnly Property IsGameJolt() As Boolean
            Get
                If Core.Player IsNot Nothing Then
                    Return Core.Player.IsGameJoltSave
                End If
                Return False
            End Get
        End Property

        Public Shared Function GetInterfaceColor(ByVal ColorType As Integer, ByVal isGameJolt As Boolean) As Color
            Dim y As Integer = 0
            If isGameJolt = True Then
                y = 1
            End If
            Dim Data(0) As Color
            Dim InterfaceColorTexture = TextureManager.GetTexture("GUI\Menus\InterfaceColors")
            InterfaceColorTexture.GetData(0, New Rectangle(ColorType, y, 1, 1), Data, 0, 1)
            Return Data(0)
        End Function

        Public Shared ReadOnly Property GradientColor() As Color
            Get
                Return GradientColor(IsGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property GradientColor(ByVal isGameJolt As Boolean) As Color
            Get
                Return GetInterfaceColor(0, isGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property GradientColor(ByVal isGameJolt As Boolean, ByVal alpha As Integer) As Color
            Get
                Dim _color As Color = GetInterfaceColor(0, isGameJolt)
                Return New Color(_color.R, _color.G, _color.B, alpha)
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
                Return GetInterfaceColor(1, isGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property MainColor(ByVal isGameJolt As Boolean, ByVal alpha As Integer) As Color
            Get
                Dim _color As Color = GetInterfaceColor(1, isGameJolt)
                Return New Color(_color.R, _color.G, _color.B, alpha)
            End Get
        End Property

        Private Shared ReadOnly _lightColor As Color = New Color(111, 249, 255)
        Private Shared ReadOnly _gameJolt_lightColor As Color = New Color(70, 70, 70)

        Public Shared ReadOnly Property LightColor() As Color
            Get
                Return LightColor(IsGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property LightColor(ByVal isGameJolt As Boolean) As Color
            Get
                Return GetInterfaceColor(2, isGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property LightColor(ByVal isGameJolt As Boolean, ByVal alpha As Integer) As Color
            Get
                Dim _color As Color = GetInterfaceColor(2, isGameJolt)
                Return New Color(_color.R, _color.G, _color.B, alpha)
            End Get
        End Property

        Private Shared ReadOnly _accentColor As Color = New Color(3, 155, 229)
        Private Shared ReadOnly _gameJolt_accentColor As Color = New Color(204, 255, 0)

        Public Shared ReadOnly Property AccentColor() As Color
            Get
                Return AccentColor(IsGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property AccentColor(ByVal isGameJolt As Boolean) As Color
            Get
                Return GetInterfaceColor(3, isGameJolt)
            End Get
        End Property

        Public Shared ReadOnly Property AccentColor(ByVal isGameJolt As Boolean, ByVal alpha As Integer) As Color
            Get
                Dim _color As Color = GetInterfaceColor(3, isGameJolt)
                Return New Color(_color.R, _color.G, _color.B, alpha)
            End Get
        End Property

    End Class

End Namespace