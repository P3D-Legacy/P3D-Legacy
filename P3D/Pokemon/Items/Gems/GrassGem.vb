Namespace Items.Gems

    <Item(633, "Grass Gem")>
    Public Class GrassGem

        Inherits GemItem

        Public Overrides ReadOnly Property Description As String = "A gem with an essence of nature. When held, it strengthens the power of a Grass-type move one time."

        Public Sub New()
            MyBase.New(Element.Types.Grass)
        End Sub

    End Class

End Namespace
