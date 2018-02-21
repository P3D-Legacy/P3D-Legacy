Namespace Items.Standard

    <Item(131, "Stardust")>
    Public Class Stardust

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "Lovely, red sand that flows between the fingers with a loose, silky feel. It can be sold at a high price to shops."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(240, 120, 24, 24)
        End Sub

    End Class

End Namespace
