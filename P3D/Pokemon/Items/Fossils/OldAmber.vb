Namespace Items.Standard

    <Item(603, "Old Amber")>
    Public Class OldAmber

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "A piece of amber that still contains the genetic material of an ancient Pokémon. It's clear with a tawny, reddish tint."

        Public Sub New()
            _textureRectangle = New Rectangle(48, 0, 24, 24)
        End Sub

    End Class

End Namespace
