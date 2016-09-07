Namespace BattleSystem

    Public Class BattleAnimationScreenV2

        Inherits Screen

        Public Sub New(ByVal currentScreen As Screen)
            Me.PreScreen = currentScreen

            Me.Identification = Identifications.BattleAnimationScreen
        End Sub

        Public Overrides Sub Draw()
            DrawBackgroundLayer()
            DrawBackLayer()
            DrawMainLayer()
            DrawFrontLayer1()
            DrawFrontLayer2()
        End Sub

        Public Overrides Sub Update()

        End Sub

#Region "Draws"

        Private Sub DrawBackgroundLayer()

        End Sub

        Private Sub DrawBackLayer()

        End Sub

        Private Sub DrawMainLayer()

        End Sub

        Private Sub DrawFrontLayer1()

        End Sub

        Private Sub DrawFrontLayer2()

        End Sub

#End Region

#Region "ScreenFunction"

        Public Sub SetBackground()

        End Sub

#End Region

    End Class

End Namespace