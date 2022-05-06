Namespace Items.Standard

    <Item(579, "Choice Scarf")>
    Public Class ChoiceScarf

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "Boosts Speed by 50%, but only allows the use of the first move selected."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 4000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False
        Public Overrides ReadOnly Property PluralName As String = "Choice Scarves"

        Public Sub New()
            _textureRectangle = New Rectangle(24, 288, 24, 24)
        End Sub

    End Class

End Namespace
