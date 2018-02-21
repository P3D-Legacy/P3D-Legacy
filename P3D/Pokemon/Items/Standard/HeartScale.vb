Namespace Items.Standard

    <Item(190, "Heart Scale")>
    Public Class HeartScale
        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A pretty, heart-shaped scale that is extremely rare. It glows faintly with all of the colors of the rainbow."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(264, 216, 24, 24)
        End Sub

    End Class

End Namespace
