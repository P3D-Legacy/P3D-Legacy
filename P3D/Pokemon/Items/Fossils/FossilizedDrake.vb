Namespace Items.Standard

    <Item(614, "Fossilized Drake")>
    Public Class FossilizedDrake

        Inherits FossilItem

        Public Overrides ReadOnly Property Description As String = "The fossil of an ancient Pokémon that once roamed the land. What it looked like is a mystery."

        Public Sub New()
            _textureRectangle = New Rectangle(72, 72, 24, 24)
        End Sub

    End Class

End Namespace
