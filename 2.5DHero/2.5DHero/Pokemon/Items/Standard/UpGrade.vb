Namespace Items.Standard

    <Item(172, "Up-Grade")>
    Public Class UpGrade

        Inherits Item

        Public Overrides ReadOnly Property Description As String = "A transparent device somehow filled with all sorts of data. It was produced by Silph Co."
        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 2100
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(336, 144, 24, 24)
        End Sub

    End Class

End Namespace
