Namespace Items.Standard

    <Item(104, "Pink Bow")>
    Public Class PinkBow

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "Powers up Fairy-type moves."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(144, 96, 24, 24)
        End Sub

    End Class

End Namespace
