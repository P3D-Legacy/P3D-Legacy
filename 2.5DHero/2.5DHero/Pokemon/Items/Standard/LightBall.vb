Namespace Items.Standard

    <Item(163, "Light Ball")>
    Public Class LightBall

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "An item to be held by Pikachu. It's a puzzling orb that boosts its Attack and Sp. Atk stats."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(168, 144, 24, 24)
        End Sub

    End Class

End Namespace
