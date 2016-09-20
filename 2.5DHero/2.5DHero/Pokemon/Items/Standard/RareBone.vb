Namespace Items.Standard

    <Item(109, "Rare Bone")>
    Public Class RareBone

        Inherits Item

        Public Overrides ReadOnly Property PokeDollarPrice As Integer = 30000
        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(456, 96, 24, 24)
        End Sub

    End Class

End Namespace
