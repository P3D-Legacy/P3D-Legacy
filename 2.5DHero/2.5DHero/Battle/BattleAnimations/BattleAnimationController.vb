Namespace BattleSystem

    Public Class BattleAnimationController

        Private _battleScreenInstance As BattleScreen

        'Background:
        Private _shaderColor As Color = Color.Black
        Private _applyShader As Boolean = False

        Public Sub New(ByVal BattleScreen As BattleScreen)
            Me._battleScreenInstance = BattleScreen
        End Sub

        ''' <summary>
        ''' Clears all effects from the screen.
        ''' </summary>
        Public Sub ClearEffects()
            Me.DeApplyShader()
        End Sub

        Public Sub Update()

        End Sub

        Public Sub Draw()

        End Sub

#Region "Shader"

        Public Sub ApplyShader(ByVal ShaderColor As Color)
            Me._shaderColor = ShaderColor
            Me._applyShader = True
        End Sub

        Public Sub DeApplyShader()
            Me._applyShader = False
        End Sub

#End Region

#Region "2DAnimations"



#End Region

    End Class

End Namespace