Namespace Items.Standard

    <Item(113, "Spell Tag")>
    Public Class SpellTag

        Inherits Item

        Public Overrides ReadOnly Property CanBeUsedInBattle As Boolean = False
        Public Overrides ReadOnly Property CanBeUsed As Boolean = False

        Public Sub New()
            _textureRectangle = New Rectangle(336, 96, 24, 24)
        End Sub

    End Class

End Namespace
