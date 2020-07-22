Namespace Items.Standard

    <Item(578, "Choice Band")>
    Public Class ChoiceBand

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "Boosts Attack by 50%, but only allows the use of the first move selected."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 4000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(0, 288, 24, 24)
        End Sub

    End Class

End Namespace
