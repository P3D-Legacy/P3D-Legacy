Namespace Items.Standard

    <Item(580, "Choice Specs")>
    Public Class ChoiceSpecs

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "Boosts Special Attack by 50%, but only allows the use of the first move selected."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 4000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(48, 288, 24, 24)
        End Sub

    End Class

End Namespace
