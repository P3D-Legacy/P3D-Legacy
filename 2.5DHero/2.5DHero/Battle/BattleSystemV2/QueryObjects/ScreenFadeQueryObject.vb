Namespace BattleSystem

    Public Class ScreenFadeQueryObject

        Inherits QueryObject

        Public Enum FadeTypes
            Horizontal
            Vertical
            CloseRight
            CloseLeft
        End Enum

        Public _color As Color
        Public _fadeType As FadeTypes

        Dim ready As Boolean = False

        Dim current As Integer = 0
        Dim goal As Integer = 0
        Dim animationSpeed As Integer = 6
        Dim changedOverlay As Boolean = False
        Dim appear As Boolean = False

        Public Sub New(ByVal FadeType As FadeTypes, ByVal c As Color, ByVal appear As Boolean, ByVal speed As Integer)
            MyBase.New(QueryTypes.ScreenFade)

            Me._fadeType = FadeType
            Me._color = c
            Me.animationSpeed = speed
            Me.appear = appear

            Initialize()
        End Sub

        Private Sub Initialize()
            Select Case _fadeType
                Case FadeTypes.Horizontal
                    If appear = True Then
                        Me.goal = CInt(Core.windowSize.Width / 2)
                        Me.current = 0
                    Else
                        Me.goal = 0
                        Me.current = CInt(Core.windowSize.Width / 2)
                    End If
                Case FadeTypes.Vertical
                    If appear = True Then
                        Me.goal = CInt(Core.windowSize.Height / 2)
                        Me.current = 0
                    Else
                        Me.goal = 0
                        Me.current = CInt(Core.windowSize.Height / 2)
                    End If
                Case FadeTypes.CloseLeft
                    If appear = True Then
                        Me.goal = Core.windowSize.Width
                        Me.current = 0
                    Else
                        Me.goal = 0
                        Me.current = Core.windowSize.Width
                    End If
                Case FadeTypes.CloseRight
                    If appear = True Then
                        Me.goal = Core.windowSize.Width
                        Me.current = 0
                    Else
                        Me.goal = 0
                        Me.current = Core.windowSize.Width
                    End If
            End Select
        End Sub

        Public Overrides Sub Draw(BV2Screen As BattleScreen)
            If appear = False And BV2Screen.DrawColoredScreen = True Then
                ChangeOverlay(BV2Screen)
            End If

            Select Case Me._fadeType
                Case FadeTypes.Vertical
                    Canvas.DrawRectangle(New Rectangle(0, 0, Core.windowSize.Width, current), Me._color)
                    Canvas.DrawRectangle(New Rectangle(0, Core.windowSize.Height - current, Core.windowSize.Width, current), Me._color)
                Case FadeTypes.Horizontal
                    Canvas.DrawRectangle(New Rectangle(0, 0, current, Core.windowSize.Height), Me._color)
                    Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - current, 0, current, Core.windowSize.Height), Me._color)
                Case FadeTypes.CloseLeft
                    Canvas.DrawRectangle(New Rectangle(Core.windowSize.Width - Me.current, 0, Me.current, Core.windowSize.Height), Me._color)
                Case FadeTypes.CloseRight
                    Canvas.DrawRectangle(New Rectangle(0, 0, Me.current, Core.windowSize.Height), Me._color)
            End Select
        End Sub

        Private Sub ChangeOverlay(BV2Screen As BattleScreen)
            If Me.changedOverlay = False Then
                BV2Screen.DrawColoredScreen = Not BV2Screen.DrawColoredScreen
                Me.changedOverlay = True
            End If
        End Sub

        Public Overrides Sub Update(BV2Screen As BattleScreen)
            If Me.ready = False Then
                If Me.current < Me.goal Then
                    Me.current += Me.GetAnimationSpeed()
                    If Me.current >= Me.goal Then
                        Me.ready = True
                        If appear = True Then
                            ChangeOverlay(BV2Screen)
                        End If
                    End If
                Else
                    Me.current -= Me.GetAnimationSpeed()
                    If Me.current <= Me.goal Then
                        Me.ready = True
                        If appear = True Then
                            ChangeOverlay(BV2Screen)
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function GetAnimationSpeed() As Integer
            Dim multiplier As Single = 1.0F

            Select Case Me._fadeType
                Case FadeTypes.CloseLeft, FadeTypes.CloseRight, FadeTypes.Horizontal
                    multiplier = Core.windowSize.Width / 1200.0F
                Case FadeTypes.Vertical
                    multiplier = Core.windowSize.Height / 680.0F
            End Select

            Dim s As Integer = CInt(multiplier * Me.animationSpeed)
            If s < 1 Then
                s = 1
            End If

            Return s
        End Function

        Public Overrides ReadOnly Property IsReady As Boolean
            Get
                Return ready
            End Get
        End Property

        Public Overrides Function NeedForPVPData() As Boolean
            Return True
        End Function

        Public Shared Shadows Function FromString(ByVal input As String) As QueryObject
            Dim d() As String = input.Split(CChar("|"))
            Dim fadeType As FadeTypes = CType(CInt(d(0)), FadeTypes)

            Dim q As New ScreenFadeQueryObject(fadeType, New Color(CInt(d(1)), CInt(d(2)), CInt(d(3))), CBool(d(4)), CInt(d(5)))
            q.PassThis = CBool(d(6))

            Return q
        End Function

        Public Overrides Function ToString() As String
            Dim s As String = CInt(Me._fadeType) & "|" &
                Me._color.R & "|" & Me._color.G & "|" & Me._color.B & "|" &
                Me.appear.ToNumberString() & "|" &
                Me.animationSpeed.ToString() & "|" &
                Me.PassThis.ToNumberString()

            Return "{FADE|" & s & "}"
        End Function

    End Class

End Namespace